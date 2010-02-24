Imports System.IO
Imports System.Threading
Imports System.Text
Imports System.Runtime.Serialization
Imports System.Xml
Imports System.Net.NetworkInformation
Imports System.Collections.Specialized
Imports System.Runtime.InteropServices
Imports System.Net
Imports System.Net.Sockets

Public Class iViewer
    Public Port As Integer
    Public Password As String = "na"
    Public Const Delimeter As String = Chr(&H3)
    Public Event OnDigital(ByVal Join As Integer, ByVal State As Boolean, ByVal ClientID As Long)
    Public Event OnAnalog(ByVal Join As Integer, ByVal Value As Integer, ByVal ClientID As Long)
    Public Event OnSerial(ByVal Join As Integer, ByVal Value As String, ByVal ClientID As Long)
    Public Event OnListDigital(ByVal Join As Integer, ByVal State As Boolean, ByVal ListJoin As Integer, ByVal ListItem As Integer, ByVal ClientID As Long)
    Public Event OnListAnalog(ByVal Join As Integer, ByVal Value As Integer, ByVal ListJoin As Integer, ByVal ListItem As Integer, ByVal ClientID As Long)
    Public Event OnListSerial(ByVal Join As Integer, ByVal Value As String, ByVal ListJoin As Integer, ByVal ListItem As Integer, ByVal ClientID As Long)
    Public Event ServerStarted()
    Public Event ServerStopped()
    Public Event OnRotatePortrait(ByVal ClientID As Long)
    Public Event OnRotateLandscape(ByVal ClientID As Long)
    Public Event OnConnect(ByVal ClientID As Long)
    Public Event OnInit(ByVal ClientID As Long)
    Public Event OnDisconnect(ByVal ClientID As Long)
    Public Event ServerStartError(ByVal Message As String)
    Public Event ExceptionRaised(ByVal Exception As System.Exception)
    Private Shared ContinueProcessing As Boolean = True

#Region " TCP SERVER "
    ' TCP/IP Communication
    Private tcpLsn As TcpListener
    Private tcpThd, msgThd As Thread
    Private IPAddress As String
    Private dataHolder As New Hashtable
    Private Shared connectId As Long = 0
    Private msgList As New Queue
    Private localIPAddress As String

    ' This stores data about each client 
    Public Structure ClientData
        Public structSocket As Socket
        Public structThread As Thread
        Public structIP As String
        Public structIPLocal As String
    End Structure 'ClientData

    Public Function GetIPAddress() As IPAddress
        Dim sHostName As String = Dns.GetHostName()
        Dim ipE As IPHostEntry = Dns.GetHostEntry(sHostName)
        Dim ipA() As IPAddress = ipE.AddressList
        GetIPAddress = ipA(ipA.Length - 1)
    End Function

    Public Function ListIPAddresses() As ArrayList
        Dim ipAddresses As New ArrayList
        Dim sHostName As String = System.Net.Dns.GetHostName()
        Dim ipE As System.Net.IPHostEntry = System.Net.Dns.GetHostEntry(sHostName)
        Dim ipA() As System.Net.IPAddress = ipE.AddressList
        For Each anIP As System.Net.IPAddress In ipA
            ipAddresses.Add(anIP.ToString)
        Next
        Return ipAddresses
    End Function

    Private Sub ReadSocket()
        ' realId will be not changed for each thread, but connectId is changed. It can't be used to delete object from Hashtable
        Dim realId As Long = connectId
        Dim receive() As [Byte]
        Dim cd As ClientData = CType(dataHolder(realId), ClientData)
        Dim s As Socket = cd.structSocket
        Dim ret As Integer = 0
        Dim clientIP As String

        clientIP = cd.structIP

        While True
            Try
                If s.Connected Then
                    receive = New [Byte](100) {}

                    ' Receive will block until returns 0 or Exception
                    ret = s.Receive(receive, receive.Length, 0)
                    If ret > 0 Then
                        msgList.Enqueue(New Message(System.Text.Encoding.ASCII.GetString(receive), realId, clientIP, Now))
                    Else
                        Exit While
                    End If
                Else
                    Exit While
                End If
            Catch e As Exception
                Exit While
            End Try
        End While

        RaiseEvent OnDisconnect(connectId)

        ' Remove the client data from the data holder
        SyncLock Me
            dataHolder.Remove(connectId)
        End SyncLock

        ' Client Disconnected

    End Sub 'ReadSocket

    Private Sub WaitingForClient()
        Dim CData As ClientData

        While True
            Try

                ' AcceptSocket will block until someone connects 
                CData.structSocket = tcpLsn.AcceptSocket()

                Interlocked.Increment(connectId)
                CData.structIP = CType(CData.structSocket.RemoteEndPoint(), System.Net.IPEndPoint).Address.ToString()
                CData.structIPLocal = CType(CData.structSocket.LocalEndPoint(), System.Net.IPEndPoint).Address.ToString()
                CData.structThread = New Thread(AddressOf ReadSocket)

                SyncLock Me
                    ' it is used to keep connected Sockets and active thread
                    dataHolder.Add(connectId, CData)
                End SyncLock

                CData.structThread.Start()

                RaiseEvent OnConnect(connectId)
            Catch ex As Exception
                If ex.GetType IsNot GetType(Threading.ThreadAbortException) Then
                    RaiseEvent ExceptionRaised(ex)
                End If
            End Try
        End While
    End Sub 'WaitingForClient

    Public Sub StartServer()
        Try
            StopServer()
        Catch ex As Exception
        End Try

        Try
            
            'Start the communication server
            tcpLsn = New TcpListener(Net.IPAddress.Any, Me.Port)
            tcpLsn.Start()

            tcpThd = New Thread(AddressOf WaitingForClient)
            tcpThd.Start()

            msgThd = New Thread(AddressOf ProcessMessages)
            msgThd.Start()

            RaiseEvent ServerStarted()

        Catch ex As Exception
            RaiseEvent ServerStartError(ex.Message)
            'WriteToLog(ex.ToString)
        End Try
    End Sub

    Public Sub StopServer()
        Try
            'Abort the "Waiting for clients" thread
            If Not tcpThd Is Nothing Then
                tcpThd.Abort()
                tcpLsn.Stop()
            End If
        Catch ex As Exception
            'RaiseEvent ExceptionRaised(ex)
        End Try

        Try
            'Abort the Message Processing thread
            ContinueProcessing = False
        Catch ex As Exception
            'RaiseEvent ExceptionRaised(ex)
        End Try

        Try
            SyncLock Me
                For Each clntData As ClientData In dataHolder.Values
                    clntData.structSocket.Close()
                Next clntData
            End SyncLock
        Catch ex As Exception
            'RaiseEvent ExceptionRaised(ex)
        End Try

        RaiseEvent ServerStopped()
    End Sub

    Public Sub SendToAll(ByVal msg As String)
        Dim clntData As ClientData
        Dim bytesSend() As [Byte] = System.Text.ASCIIEncoding.Default.GetBytes(msg & Delimeter)

        Try
            SyncLock Me
                For Each clntData In dataHolder.Values
                    If clntData.structSocket.Connected Then
                        clntData.structSocket.Send(bytesSend, bytesSend.Length, SocketFlags.None)
                    End If
                Next clntData
            End SyncLock
        Catch ex As Exception
            RaiseEvent ExceptionRaised(ex)
        End Try
    End Sub

    Public Sub SendMsg(ByVal msg As String, ByVal clientID As Long)
        Dim bytesSend() As [Byte] = System.Text.ASCIIEncoding.Default.GetBytes(msg & Delimeter)

        Try
            SyncLock Me
                If dataHolder(clientID).structSocket.Connected Then
                    ' Send the message
                    dataHolder(clientID).structSocket.Send(bytesSend, bytesSend.Length, SocketFlags.None)
                End If
            End SyncLock
        Catch ex As Exception
            RaiseEvent ExceptionRaised(ex)
        End Try
    End Sub

#End Region

    Public Function GetClient(ByVal clientID As Long) As ClientData
        Try
            If dataHolder(clientID) IsNot Nothing Then
                Return dataHolder(clientID)
            End If
        Catch ex As Exception
            RaiseEvent ExceptionRaised(ex)
        End Try
        Return Nothing
    End Function

    Public Function FirstClient() As ClientData
        For Each aClient As ClientData In dataHolder.Values
            If aClient.structSocket.Connected Then
                Return aClient
            End If
        Next
        Return Nothing
    End Function

    Private Sub ProcessMessages()
        ContinueProcessing = True
        While ContinueProcessing
            If msgList.Count Then
                Dim msgItem As Message = CType(msgList.Dequeue, Message)
                Try
                    Dim strArray() As String

                    strArray = msgItem.MsgString.Trim.Split(Delimeter)
                    Dim CData As ClientData = dataHolder(msgItem.clientID)
                    For Each msg As String In strArray
                        If msg.StartsWith("d") Then
                            ' Digital Join
                            Dim state As Boolean = msg.Substring(msg.IndexOf("=") + 1, 1)
                            Dim join As Integer = msg.Substring(1, msg.IndexOf("=") - 1)
                            RaiseEvent OnDigital(join, state, msgItem.clientID)
                        ElseIf msg.StartsWith("a") Then
                            ' Analog Join
                            Dim value As Integer = msg.Substring(msg.IndexOf("=") + 1, msg.Length - 1 - msg.IndexOf("="))
                            Dim join As Integer = msg.Substring(1, msg.IndexOf("=") - 1)
                            RaiseEvent OnAnalog(join, value, msgItem.clientID)
                        ElseIf msg.StartsWith("s") Then
                            ' Serial Join
                            Dim value As String = msg.Substring(msg.IndexOf("=") + 1, msg.Length - 1 - msg.IndexOf("="))
                            Dim join As Integer = msg.Substring(1, msg.IndexOf("=") - 1)
                            RaiseEvent OnSerial(join, value, msgItem.clientID)
                        ElseIf msg.StartsWith("p") Then
                            Dim passVal As String = msg.Substring(msg.IndexOf("=") + 1, msg.Length - 1 - msg.IndexOf("="))
                            If passVal = Me.Password Or Me.Password = "na" Then
                                SendMsg("p=ok", msgItem.clientID)
                            Else
                                SendMsg("p=bad", msgItem.clientID)
                            End If
                        ElseIf msg.StartsWith("h") Then
                            SendMsg("h=1", msgItem.clientID)
                        ElseIf msg.StartsWith("i") Then
                            ' Initialize Connection
                            RaiseEvent OnInit(msgItem.clientID)
                        ElseIf msg.StartsWith("m") Then
                            ' Orientation changed
                            Dim state As String = msg.Substring(msg.LastIndexOf("=") + 1, 1)
                            If state = "p" Then
                                RaiseEvent OnRotatePortrait(msgItem.clientID)
                            Else
                                RaiseEvent OnRotateLandscape(msgItem.clientID)
                            End If
                        ElseIf msg.StartsWith("l") Then
                            Dim listJoin As Integer = msg.Substring(1, msg.IndexOf(":") - 1)
                            ' Get the item number first
                            Dim itemNum As Integer = msg.Substring(msg.IndexOf(":") + 1, msg.IndexOf(":", msg.IndexOf(":") + 1) - (msg.IndexOf(":") + 1))
                            ' Get join type
                            Dim joinType As String = msg.Substring(msg.LastIndexOf(":") + 1, 1)
                            Select Case joinType
                                Case "d"
                                    ' Digital join
                                    Dim join As Integer = msg.Substring(msg.LastIndexOf(":") + 2, msg.IndexOf("=") - msg.LastIndexOf(":") - 2)
                                    Dim state As Boolean = msg.Substring(msg.LastIndexOf("=") + 1, 1)
                                    RaiseEvent OnListDigital(join, state, listJoin, itemNum, msgItem.clientID)
                                Case "a"
                                    ' Analog Join
                                    Dim join As Integer = msg.Substring(msg.LastIndexOf(":") + 2, msg.IndexOf("=") - msg.LastIndexOf(":") - 2)
                                    Dim value As Integer = msg.Substring(msg.LastIndexOf("=") + 1, msg.Length - msg.LastIndexOf("=") - 1)
                                    RaiseEvent OnListAnalog(join, value, listJoin, itemNum, msgItem.clientID)
                                Case "s"
                                    ' Serial Join
                                    Dim join As Integer = msg.Substring(msg.LastIndexOf(":") + 2, msg.IndexOf("=") - msg.LastIndexOf(":") - 2)
                                    Dim value As String = msg.Substring(msg.LastIndexOf("=") + 1, msg.Length - msg.LastIndexOf("=") - 1)
                                    RaiseEvent OnListSerial(join, value, listJoin, itemNum, msgItem.clientID)
                            End Select
                        End If
                    Next
                Catch ex As Exception
                    RaiseEvent ExceptionRaised(ex)
                End Try
            Else
                Thread.Sleep(10)
            End If
        End While
    End Sub
End Class
