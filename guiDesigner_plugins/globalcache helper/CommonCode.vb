Imports System.Net.Sockets
Imports System.Threading
Imports System.IO
Imports System.Text
Imports CommandFusion


Module CommonCode

    Public WithEvents FormIRLearn As New frmIRLearn
    Public bProjectSelected As Boolean = False

#Region "GC-100 Class"

    Public Class GC100

        Public IPAddress, MACAddress, Model As String

        Private client As New TcpClient
        Private clientStream As NetworkStream
        Private receiveThread As Thread

        Private clientLearner As New TcpClient
        Private clientStreamLearner As NetworkStream
        Private receiveThreadLearner As Thread

        Public irLearnerFound As Boolean = False
        Private receivingIRCode As Boolean = False
        Private IRQueue As String = ""

        Public Event UpdateLog(ByVal msg As String)
        Public Event RequestSystemList()
        Public Event ShowIRLearner()
        Public Event IRLearnerDetected(ByVal unit As GC100, ByVal state As Integer)

        Public Modules As New Dictionary(Of Integer, String)

        Public Sub GetModules()
            Try

                receiveThread = New Thread(AddressOf Receive)
                receiveThread.Start()

            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
        End Sub

        Private Sub Receive()

            Try

                ' First setup the connection
                client = New TcpClient
                client.Connect(IPAddress, 4998)
                clientStream = client.GetStream

                ' Ask for ID of IR Learner device
                TCPSend("getdevices\x0D")

            Catch ex As Exception
                MsgBox("Connection to GlobalCache device could not be established:" & Environment.NewLine & _
                "IP Address: " & Me.IPAddress & Environment.NewLine & _
                "Port: 4998")

                Exit Sub
            End Try


            While True
                Try

                    If clientStream.CanRead Then
                        Dim bytes(client.Available - 1) As Byte
                        Dim ret = clientStream.Read(bytes, 0, client.Available)
                        If ret > 0 Then
                            Dim msg As String = Encoding.ASCII.GetString(bytes)
                            RaiseEvent UpdateLog("RECV: " & msg)
                            ' Check data
                            If msg.StartsWith("device,") Then
                                Dim modulesData As String() = msg.Split(Chr(&HD))
                                For Each aModule As String In modulesData
                                    If aModule.StartsWith("device,") Then
                                        Dim ModuleNum As Integer = aModule.Substring(7, 1)
                                        Dim ModuleType As String = aModule.Substring(9, aModule.Length - 9)
                                        Modules.Add(ModuleNum, ModuleType)
                                    End If
                                Next
                            End If
                        End If
                    End If

                    While clientStream.CanRead = False
                        Thread.Sleep(100)
                    End While
                Catch ex As Exception
                    'MsgBox(ex.ToString)
                End Try
            End While
        End Sub

        Public Sub Dispose()
            Try
                'clientStream.Close()
                client.Close()
            Catch ex As Exception
            End Try

            Try
                receiveThread.Abort()
            Catch ex As Exception
            End Try

            Try
                'clientStreamLearner.Close()
                clientLearner.Close()
            Catch ex As Exception
            End Try

            Try
                receiveThreadLearner.Abort()
            Catch ex As Exception
            End Try

        End Sub

        Private Delegate Sub dlgUpdateUI(ByVal msg As String)

        Public Sub TCPSend(ByVal msg As String)
            Try
                If clientStream.CanWrite And clientStream.CanRead Then
                    Try
                        Dim newBytes As New List(Of Byte)

                        For i As Integer = 0 To msg.Length - 1
                            If msg(i) = "\" And msg(i + 1) = "x" Then
                                Dim hexChars As String = msg(i + 2) & msg(i + 3)
                                newBytes.Add(Byte.Parse(hexChars, System.Globalization.NumberStyles.HexNumber))
                                i += 3
                            Else
                                newBytes.Add(Convert.ToByte(msg(i)))
                            End If
                        Next

                        Dim sendBytes(newBytes.Count - 1) As [Byte]
                        newBytes.CopyTo(sendBytes)
                        Try
                            clientStream.Write(sendBytes, 0, sendBytes.Length)
                            RaiseEvent UpdateLog("SENT: " & msg)
                        Catch ex As Exception
                            MsgBox(ex.ToString)
                        End Try

                    Catch ex As Exception
                        If Not TypeOf ex Is ThreadAbortException Then
                            'MsgBox(ex.ToString)
                        End If
                    End Try
                Else
                    'MsgBox("Unable to send string to 'TCP/IP Send' Hostname/Port.")
                End If
            Catch ex As Exception
                'MsgBox(ex.ToString)
            End Try
        End Sub

#Region "IR LEARNER"

        Public Sub ConnectLearner()
            RaiseEvent IRLearnerDetected(Me, 2)

            Try
                clientLearner.Close()
                clientStreamLearner.Close()
            Catch ex As Exception
            End Try

            Try
                receiveThreadLearner.Abort()
            Catch ex As Exception
            End Try

            Try

                receiveThreadLearner = New Thread(AddressOf ReceiveLearner)
                receiveThreadLearner.Start()

            Catch ex As Exception
                'MsgBox(ex.ToString)
            End Try

        End Sub

        Private Sub WaitForReply()
            Dim endTime As DateTime = Now.AddSeconds(3)
            While Now < endTime
                If Me.irLearnerFound Then
                    Exit While
                End If
                Thread.Sleep(10)
            End While

            If Me.irLearnerFound Then
                ' Do nothing
            Else
                RaiseEvent IRLearnerDetected(Me, 0)
            End If
        End Sub

        Private Sub ReceiveLearner()

            Try

                Me.irLearnerFound = False

                clientLearner = New TcpClient
                clientLearner.Connect(IPAddress, 4999)
                clientStreamLearner = clientLearner.GetStream

                ' Ask for ID of IR Learner device
                TCPSendLearner("id\x0D")

                ' Start timer waiting for device reply
                Dim timerThread As New Thread(AddressOf WaitForReply)
                timerThread.Start()
            Catch ex As Exception
                MsgBox("Connection to IR Learner on Serial Port 1 of GlobalCache device could not be established:" & Environment.NewLine & _
                                "IP Address: " & Me.IPAddress & Environment.NewLine & _
                                "Port: 4999")
                Exit Sub
            End Try

            While True
                Try

                    If clientStreamLearner.CanRead Then
                        Dim bytes(clientLearner.Available - 1) As Byte
                        Dim ret = clientStreamLearner.Read(bytes, 0, clientLearner.Available)
                        If ret > 0 Then
                            Dim msg As String = Encoding.ASCII.GetString(bytes)
                            RaiseEvent UpdateLog("RECV: " & msg)
                            ' Check data
                            If msg.StartsWith("device,GC-IRL") Then
                                Me.irLearnerFound = True
                                RaiseEvent IRLearnerDetected(Me, 1)
                            ElseIf msg.StartsWith("GC-IRL") Then
                                ' Add message to IR parsing queue, and all other messages until carriage return
                                IRQueue = msg
                                If msg.EndsWith(Chr(&HD)) Then
                                    ParseIRCode()
                                Else
                                    receivingIRCode = True
                                End If
                            ElseIf receivingIRCode Then
                                IRQueue &= msg
                                If msg.EndsWith(Chr(&HD)) Then
                                    ParseIRCode()
                                Else
                                    receivingIRCode = True
                                End If
                            End If
                        End If
                    End If

                    While clientStreamLearner.CanRead = False
                        Thread.Sleep(100)
                    End While
                Catch ex As Exception
                    'MsgBox(ex.ToString)
                End Try
            End While
        End Sub

        Public Sub TCPSendLearner(ByVal msg As String)
            Try
                If clientStreamLearner.CanWrite And clientStreamLearner.CanRead Then
                    Try
                        Dim newBytes As New List(Of Byte)
                        For i As Integer = 0 To msg.Length - 1
                            If msg(i) = "\" And msg(i + 1) = "x" Then
                                Dim hexChars As String = msg(i + 2) & msg(i + 3)
                                newBytes.Add(Byte.Parse(hexChars, System.Globalization.NumberStyles.HexNumber))
                                i += 3
                            Else
                                newBytes.Add(Convert.ToByte(msg(i)))
                            End If
                        Next

                        Dim sendBytes(newBytes.Count - 1) As [Byte]
                        newBytes.CopyTo(sendBytes)
                        Try
                            clientStreamLearner.Write(sendBytes, 0, sendBytes.Length)
                            RaiseEvent UpdateLog("SENT: " & msg)
                        Catch ex As Exception
                            MsgBox(ex.ToString)
                        End Try

                    Catch ex As Exception
                        If Not TypeOf ex Is ThreadAbortException Then
                            'MsgBox(ex.ToString)
                        End If
                    End Try
                Else
                    'MsgBox("Unable to send string to 'TCP/IP Send' Hostname/Port.")
                End If
            Catch ex As Exception
                'MsgBox(ex.ToString)

            End Try
        End Sub

        Public Sub ParseIRCode()
            If Not bProjectSelected Then
                MsgBox("An IR command was detected, but no project is open. Please open a project before learning IR commands.")
                Exit Sub
            End If

            receivingIRCode = False
            Dim data As String() = RegularExpressions.Regex.Replace(IRQueue, "[^\d,]+", "").Split(",")
            Dim Frequency As String = data(1)
            Dim A As String = "," & data(2) & "," & data(3)
            Dim B As String = "," & data(4) & "," & data(5)
            Dim C As String = "," & data(6) & "," & data(7)
            Dim D As String = "," & data(8) & "," & data(9)
            IRQueue = IRQueue.Substring(IRQueue.IndexOf(Frequency) + Frequency.Length)
            IRQueue = IRQueue.Replace("A", A)
            IRQueue = IRQueue.Replace("B", B)
            IRQueue = IRQueue.Replace("C", C)
            IRQueue = IRQueue.Replace("D", D)
            FormIRLearn.GCDevice = Me
            FormIRLearn.LearntFrequency = Frequency
            FormIRLearn.LearntCommand = IRQueue.Substring(0, IRQueue.Length - 1)
            FormIRLearn.UpdateCommand()
            ' Update ports correctly
            If Me.Model = "GC-100-12" And FormIRLearn.cboIRPort.Items.Count <> 6 Then
                FormIRLearn.cboIRPort.Items.Clear()
                FormIRLearn.cboIRPort.Items.AddRange(New String() {1, 2, 3, 4, 5, 6})
                FormIRLearn.cboIRPort.SelectedIndex = 0
            ElseIf Me.Model = "GC-100-6" And FormIRLearn.cboIRPort.Items.Count <> 3 Then
                FormIRLearn.cboIRPort.Items.Clear()
                FormIRLearn.cboIRPort.Items.AddRange(New String() {1, 2, 3})
                FormIRLearn.cboIRPort.SelectedIndex = 0
            End If
            ' Show learner form
            If Not FormIRLearn.Visible Then
                RaiseEvent ShowIRLearner()
            End If

            ' Select the command name ready to enter quickly
            FormIRLearn.txtName.Focus()
            FormIRLearn.txtName.SelectionStart = 0
            FormIRLearn.txtName.SelectionLength = FormIRLearn.txtName.TextLength

            RaiseEvent RequestSystemList()
        End Sub
#End Region
    End Class
#End Region

End Module
