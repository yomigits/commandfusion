Imports System.Net.Sockets
Imports System.Threading
Imports System.IO
Imports System.Text
Imports CommandFusion

Public Class GCHelper
    Implements CFPlugin


#Region "PLUGIN IMPLEMENTATION"

    Private m_Name, m_Author As String

    Public Event ToggleWindow(ByVal sender As CFPlugin) Implements CommandFusion.CFPlugin.ToggleWindow
    Public Event WriteToLog(ByVal sender As CommandFusion.CFPlugin, ByVal msg As String) Implements CommandFusion.CFPlugin.WriteToLog
    Public Event AddSystem(ByVal sender As CommandFusion.CFPlugin, ByVal newSystem As SystemClass) Implements CommandFusion.CFPlugin.AddSystem
    Public Event AddCommand(ByVal sender As CommandFusion.CFPlugin, ByVal newCommand As SystemCommand) Implements CommandFusion.CFPlugin.AddCommand
    Public Event AddMacro(ByVal sender As CommandFusion.CFPlugin, ByVal ExistingSystem As SystemClass, ByVal newMacro As SystemMacro) Implements CommandFusion.CFPlugin.AddMacro
    Public Event RequestSystemListEvent(ByVal sender As CommandFusion.CFPlugin) Implements CommandFusion.CFPlugin.RequestSystemList
    Public Event AddFeedback(ByVal sender As CommandFusion.CFPlugin, ByVal newFB As CommandFusion.SystemFeedback) Implements CommandFusion.CFPlugin.AddFeedback

    Public Property Author() As String Implements CommandFusion.CFPlugin.Author
        Get
            Return "CommandFusion"
        End Get
        Set(ByVal value As String)
        End Set
    End Property

    Public Sub DisposePlugin() Implements CommandFusion.CFPlugin.Dispose
        Try
            client.Close()
        Catch ex As Exception
        End Try

        Try
            receiveThread.Abort()
        Catch ex As Exception
        End Try

        For Each aUnit As GC100 In units
            aUnit.Dispose()
        Next

        FormIRLearn.Close()

        Me.Form.Close()
    End Sub

    Public Property Form() As System.Windows.Forms.Form Implements CommandFusion.CFPlugin.Form
        Get
            Return Me
        End Get
        Set(ByVal value As System.Windows.Forms.Form)

        End Set
    End Property

    Public Sub Init(ByVal menu As System.Windows.Forms.MainMenu) Implements CommandFusion.CFPlugin.Init

        Dim pluginMenu As New MenuItem("GC Helper")
        Dim showHideMenu As New MenuItem("Toggle Window")
        Dim RefreshListMenu As New MenuItem("Refresh Device List")
        AddHandler showHideMenu.Click, AddressOf DoToggleWindow
        AddHandler RefreshListMenu.Click, AddressOf btnRefresh_Click
        AddHandler FormIRLearn.AddCommand, AddressOf DoAddCommand
        pluginMenu.MenuItems.Add(showHideMenu)
        pluginMenu.MenuItems.Add(RefreshListMenu)
        menu.MenuItems.Add(pluginMenu)

        Try
            client.Close()
        Catch ex As Exception
        End Try

        Try

            client = New UdpClient
            client.EnableBroadcast = True

            client.Client.Bind(New Net.IPEndPoint(Net.IPAddress.Any, 9131))

            Dim sHostName As String = System.Net.Dns.GetHostName()
            Dim ipE As System.Net.IPHostEntry = System.Net.Dns.GetHostEntry(sHostName)
            Dim ipA() As System.Net.IPAddress = ipE.AddressList
            For Each ipAddy As System.Net.IPAddress In ipA
                'MsgBox(ipAddy.ToString)
                Dim mcastOption As MulticastOption = New MulticastOption(Net.IPAddress.Parse("239.255.250.250"), ipAddy)
                client.Client.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, mcastOption)
            Next

        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try

        receiveThread.Start()

    End Sub

    Private Sub DoToggleWindow(ByVal sender As Object, ByVal e As System.EventArgs)
        RaiseEvent ToggleWindow(Me)
    End Sub

    Public Property NamePlugin() As String Implements CommandFusion.CFPlugin.Name
        Get
            Return "GC Helper"
        End Get
        Set(ByVal value As String)

        End Set
    End Property

    Public Sub ProjectSelected(ByVal selected As Boolean) Implements CommandFusion.CFPlugin.ProjectSelected
        If selected Then
            Me.mnuAddSystems.Text = "Add Systems to Project"
            Me.mnuInitIR.Text = "Initialize IR Learner"
            RaiseEvent RequestSystemListEvent(Me)
        Else
            Me.mnuAddSystems.Text = "Add Systems to Project (Open Project First!)"
            Me.mnuInitIR.Text = "Initialize IR Learner (Open Project First!)"
        End If
        Me.mnuAddSystems.Enabled = selected
        Me.mnuInitIR.Enabled = selected
        bProjectSelected = selected
    End Sub

#End Region

    Private units As New List(Of GC100)
    Private client As New UdpClient
    Private receiveThread As New Thread(AddressOf Receive)

    Public Function GenerateReadable(ByVal bytes As Byte()) As String
        Dim tmpMsg As String = System.Text.Encoding.ASCII.GetString(bytes)
        Dim i As Integer = 0
        Dim readable As String = ""
        For Each aByte As Byte In bytes
            If Int32.Parse(aByte) >= 20 And Int32.Parse(aByte) < 127 Then
                readable &= tmpMsg(i)
            Else
                readable &= "\x" & Conversion.Hex(aByte).PadLeft(2, "0")
            End If
            i += 1
        Next
        Return readable
    End Function

    Private Sub Receive()
        Dim RemoteIpEndPoint As New Net.IPEndPoint(Net.IPAddress.Any, 9131)
        While True
            If client Is Nothing OrElse client.Client Is Nothing Then
                Thread.Sleep(10)
            Else
                Try
                    Dim receiveBytes As [Byte]() = client.Receive(RemoteIpEndPoint)
                    Dim readable As String = GenerateReadable(receiveBytes)
                    If readable.StartsWith("AMXB<-UUID=GC100") Then
                        ' Found a GC-100 unit, what model?
                        Dim model, ipAddy, macAddy As String
                        ipAddy = ""
                        Dim newUnit As New GC100
                        Dim attributes As String() = readable.Split("<")
                        For Each attr As String In attributes
                            If attr.Contains("Model") Then
                                model = attr.Substring(attr.IndexOf("=") + 1)
                                model = model.Remove(model.Length - 1, 1)
                                newUnit.Model = model
                            ElseIf attr.Contains("UUID") Then
                                macAddy = attr.Substring(attr.IndexOf("=") + 7)
                                macAddy = macAddy.Remove(macAddy.Length - 13, 13)
                                newUnit.MACAddress = New System.Text.RegularExpressions.Regex("(.{2})").Replace(macAddy, "$1:").Substring(0, 14)
                            ElseIf attr.Contains("URL") Then
                                ipAddy = attr.Substring(attr.IndexOf("=") + 8)
                                ipAddy = ipAddy.Remove(ipAddy.Length - 5, 5)
                                newUnit.IPAddress = ipAddy
                            End If
                        Next
                        If FoundNewUnit(newUnit.IPAddress) Then
                            units.Add(newUnit)
                            ' Update the list of units
                            UpdateUnitsGrid(newUnit)
                            AddHandler newUnit.UpdateLog, AddressOf UpdateLog
                            AddHandler newUnit.RequestSystemList, AddressOf RequestSystemList
                            AddHandler newUnit.ShowIRLearner, AddressOf ShowIRLearner
                            AddHandler newUnit.IRLearnerDetected, AddressOf IRLearnerDetected
                            newUnit.GetModules()
                        ElseIf ipAddy <> Nothing Then
                            ' Unit already found, do nothing
                        End If

                    End If
                Catch ex As Exception
                    If Not TypeOf ex Is ThreadAbortException Then
                        'MsgBox(ex.ToString)
                        'RaiseEvent WriteToLog(Me, ex.ToString)
                    End If
                    Thread.Sleep(10)
                End Try
            End If
        End While
    End Sub

    ' Make sure unit hasn't previously been found and added to the units list
    Private Function FoundNewUnit(ByVal ipAddy As String) As Boolean
        For Each aUnit As GC100 In units
            If aUnit.IPAddress = ipAddy Then
                Return False
            End If
        Next
        Return True
    End Function

    Private Function GetUnitByIP(ByVal ipAddy As String) As GC100
        For Each aUnit As GC100 In units
            If aUnit.IPAddress = ipAddy Then
                Return aUnit
            End If
        Next
        Return Nothing
    End Function

    Public Sub UDPSend(ByVal msg As String)
        Try
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
                    client.Send(sendBytes, sendBytes.Length, New Net.IPEndPoint(Net.IPAddress.Parse("239.255.250.250"), 9131))
                Catch ex As Exception
                    MsgBox(ex.ToString)
                End Try

            Catch ex As Exception
                If Not TypeOf ex Is ThreadAbortException Then
                    'MsgBox(ex.ToString)
                End If
            End Try
        Catch ex As Exception
            'MsgBox(ex.ToString)

        End Try
    End Sub

    Private Sub btnRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
        units.Clear()
        gridUnits.Rows.Clear()
    End Sub

    Private Delegate Sub dlgUpdateUnitsGrid(ByVal newUnit As GC100)
    Private Sub UpdateUnitsGrid(ByVal newUnit As GC100)
        If Me.InvokeRequired Then
            Dim d As New dlgUpdateUnitsGrid(AddressOf UpdateUnitsGrid)
            Me.Invoke(d, newUnit)
        Else
            If newUnit IsNot Nothing Then
                gridUnits.Rows.Add()
                Dim newRow As DataGridViewRow = gridUnits.Rows(gridUnits.Rows.Count - 1)
                newRow.Cells("colModel").Value = newUnit.Model
                newRow.Cells("colIPAddress").Value = newUnit.IPAddress
                newRow.Cells("colMac").Value = newUnit.MACAddress
                newRow.Cells("colIRLearner").Value = "not detected"
                newRow.Cells("colIRLearner").Style.ForeColor = Color.Red
            Else
                gridUnits.Rows.Clear()
                For Each aUnit As GC100 In units
                    gridUnits.Rows.Add()
                    Dim newRow As DataGridViewRow = gridUnits.Rows(gridUnits.Rows.Count - 1)
                    newRow.Cells("colModel").Value = aUnit.Model
                    newRow.Cells("colIPAddress").Value = aUnit.IPAddress
                    newRow.Cells("colMac").Value = aUnit.MACAddress
                    If aUnit.irLearnerFound Then
                        newRow.Cells("colIRLearner").Value = "ready"
                        newRow.Cells("colIRLearner").Style.ForeColor = Color.Green
                    Else
                        newRow.Cells("colIRLearner").Value = "not detected"
                        newRow.Cells("colIRLearner").Style.ForeColor = Color.Red
                    End If
                Next
            End If

        End If
    End Sub

    Private Function GetDeviceRowIndex(ByVal ipAddy As String) As Integer
        For Each aRow As DataGridViewRow In gridUnits.Rows
            If aRow.Cells("colIPAddress").Value = ipAddy Then
                Return aRow.Index
            End If
        Next
    End Function

    Private Sub mnuInitIR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuInitIR.Click
        ' Open TCP connection to selected Device to see if learner is attached
        Dim theUnit As GC100 = GetUnitByIP(gridUnits.SelectedRows(0).Cells("colIPAddress").Value)
        If theUnit IsNot Nothing Then
            theUnit.ConnectLearner()
        End If
    End Sub

    Private Delegate Sub dlgUpdateUI(ByVal msg As String)
    Private Sub UpdateLog(ByVal msg As String)
        'If Me.InvokeRequired Then
        '    Dim d As New dlgUpdateUI(AddressOf UpdateLog)
        '    Me.Invoke(d, msg)
        'Else
        '    txtLog.AppendText(msg & Environment.NewLine)
        'End If
    End Sub

    Private Sub RequestSystemList()
        RaiseEvent RequestSystemListEvent(Me)
    End Sub

    Private Sub gridUnits_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles gridUnits.MouseUp
        Dim hit As DataGridView.HitTestInfo = gridUnits.HitTest(e.Location.X, e.Location.Y)
        If hit.Type = DataGridViewHitTestType.None Then
            gridUnits.ClearSelection()
            Exit Sub
        End If
        If e.Button = Windows.Forms.MouseButtons.Right Then
            If hit.Type = DataGridViewHitTestType.Cell Then
                gridUnits.Rows(hit.RowIndex).Cells(hit.ColumnIndex).Selected = True

                gridUnits.ContextMenuStrip = cmnuUnit
                gridUnits.ContextMenuStrip.Show(gridUnits, e.Location)
                gridUnits.ContextMenuStrip = Nothing
            End If
        End If
    End Sub

    Private Sub mnuAddSystems_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAddSystems.Click
        Dim theUnit As GC100 = GetUnitByIP(gridUnits.SelectedRows(0).Cells("colIPAddress").Value)
        If theUnit IsNot Nothing Then
            If theUnit.Model = "GC-100-12" Then
                Dim systemStandard As New SystemClass
                systemStandard.AlwaysOn = True
                systemStandard.EOM = "\x0D"
                systemStandard.IPAddress = theUnit.IPAddress
                systemStandard.Name = "GC-100_" & theUnit.IPAddress
                systemStandard.PortDestination = 4998
                systemStandard.PortOrigin = 4998
                systemStandard.ProtocolUsed = SystemClass.Protocol.TCP

                Dim aCmd As New CommandFusion.SystemCommand
                Dim aFB As CommandFusion.SystemFeedback
                Dim aGroup As CommandFusion.SystemCommandElement
                For i As Integer = 1 To 3
                    ' Relay Commands
                    aCmd = New CommandFusion.SystemCommand
                    aCmd.Name = "Relay Close " & i
                    aCmd.Value = "setstate,3:" & i & ",1\x0D"
                    aCmd.System = systemStandard
                    systemStandard.Commands.Add(aCmd)

                    aCmd = New CommandFusion.SystemCommand
                    aCmd.Name = "Relay Open " & i
                    aCmd.Value = "setstate,3:" & i & ",0\x0D"
                    aCmd.System = systemStandard
                    systemStandard.Commands.Add(aCmd)

                    ' Digital Input Commands
                    aCmd = New CommandFusion.SystemCommand
                    aCmd.Name = "Get Input " & i
                    aCmd.Value = "getstate,4:" & i & "\x0D"
                    aCmd.System = systemStandard
                    systemStandard.Commands.Add(aCmd)

                    aCmd = New CommandFusion.SystemCommand
                    aCmd.Name = "Get Input " & (3 + i)
                    aCmd.Value = "getstate,5:" & i & "\x0D"
                    aCmd.System = systemStandard
                    systemStandard.Commands.Add(aCmd)

                    ' Digital Input Feedbacks
                    aFB = New CommandFusion.SystemFeedback
                    aFB.Name = "Input State " & i
                    aFB.Value = "state(?:change)?,4:" & i & ",(\d)\x0D"
                    aGroup = New CommandFusion.SystemCommandElement
                    aGroup.DataType = "d"
                    aGroup.Join = i
                    aGroup.Name = "Input State"
                    aFB.DataElements.Add(aGroup)
                    aFB.System = systemStandard
                    systemStandard.Feedback.Add(aFB)

                    aFB = New CommandFusion.SystemFeedback
                    aFB.Name = "Input State " & (3 + i)
                    aFB.Value = "state(?:change)?,5:" & i & ",(\d)\x0D"
                    aGroup = New CommandFusion.SystemCommandElement
                    aGroup.DataType = "d"
                    aGroup.Join = (3 + i)
                    aGroup.Name = "Input State"
                    aFB.DataElements.Add(aGroup)
                    aFB.System = systemStandard
                    systemStandard.Feedback.Add(aFB)
                Next

                RaiseEvent AddSystem(Me, systemStandard)

                Dim systemSerial1 As New SystemClass
                systemSerial1.AlwaysOn = True
                systemSerial1.IPAddress = theUnit.IPAddress
                systemSerial1.Name = "GC-100_Serial1_" & theUnit.IPAddress
                systemSerial1.PortDestination = 4999
                systemSerial1.PortOrigin = 4999
                systemSerial1.ProtocolUsed = SystemClass.Protocol.TCP

                RaiseEvent AddSystem(Me, systemSerial1)

                Dim systemSerial2 As New SystemClass
                systemSerial2.AlwaysOn = True
                systemSerial2.IPAddress = theUnit.IPAddress
                systemSerial2.Name = "GC-100_Serial2_" & theUnit.IPAddress
                systemSerial2.PortDestination = 5000
                systemSerial2.PortOrigin = 5000
                systemSerial2.ProtocolUsed = SystemClass.Protocol.TCP

                RaiseEvent AddSystem(Me, systemSerial2)

            End If
        End If
    End Sub

    Private Sub mnuBlinkStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuBlinkStart.Click
        Dim theUnit As GC100 = GetUnitByIP(gridUnits.SelectedRows(0).Cells("colIPAddress").Value)
        If theUnit IsNot Nothing Then
            theUnit.TCPSend("blink,1\x0D")
        End If
    End Sub

    Private Sub mnuBlinkStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuBlinkStop.Click
        Dim theUnit As GC100 = GetUnitByIP(gridUnits.SelectedRows(0).Cells("colIPAddress").Value)
        If theUnit IsNot Nothing Then
            theUnit.TCPSend("blink,0\x0D")
        End If
    End Sub

    Private Sub mnuConfigureDevice_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuConfigureDevice.Click
        Dim theUnit As GC100 = GetUnitByIP(gridUnits.SelectedRows(0).Cells("colIPAddress").Value)
        If theUnit IsNot Nothing Then
            Process.Start("http://" & theUnit.IPAddress)
        End If
    End Sub

    Private Sub gridUnits_CellContentDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridUnits.CellDoubleClick
        Me.mnuConfigureDevice.PerformClick()
    End Sub

    Private Delegate Sub dlgUpdateSystemList(ByVal systemList As System.Collections.Generic.List(Of CommandFusion.SystemClass))
    Public Sub UpdateSystemList(ByVal systemList As System.Collections.Generic.List(Of CommandFusion.SystemClass)) Implements CommandFusion.CFPlugin.UpdateSystemList
        If Me.InvokeRequired Then
            Dim d As New dlgUpdateSystemList(AddressOf UpdateSystemList)
            Me.Invoke(d, systemList)
        Else

            Try
                ' Save selection before clearning and updating list
                Dim selectedSystem As SystemClass = FormIRLearn.cboSystem.SelectedItem()
                FormIRLearn.cboSystem.Items.Clear()
                ' Update the list of systems
                For Each aSystem As SystemClass In systemList
                    FormIRLearn.cboSystem.Items.Add(aSystem)
                Next
                ' Revert selection before list update
                If selectedSystem IsNot Nothing AndAlso FormIRLearn.cboSystem.Items.Contains(selectedSystem) Then
                    FormIRLearn.cboSystem.SelectedItem = selectedSystem
                End If
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
        End If
    End Sub

    Private Delegate Sub dlgShowIRLearner()
    Public Sub ShowIRLearner()
        If Me.InvokeRequired Then
            Dim d As New dlgShowIRLearner(AddressOf ShowIRLearner)
            Me.Invoke(d)
        Else
            FormIRLearn.Show(Me)
        End If
    End Sub

    Private Sub DoAddCommand()
        Dim newCmd As New SystemCommand
        newCmd.Name = FormIRLearn.txtName.Text
        newCmd.Value = FormIRLearn.txtCommand.Text
        newCmd.System = FormIRLearn.cboSystem.SelectedItem

        RaiseEvent AddCommand(Me, newCmd)
    End Sub

    Private Delegate Sub dlgIRLearnerDetected(ByVal unit As GC100, ByVal state As Integer)
    Private Sub IRLearnerDetected(ByVal unit As GC100, ByVal state As Integer)
        If Me.InvokeRequired Then
            Dim d As New dlgIRLearnerDetected(AddressOf IRLearnerDetected)
            Me.Invoke(d, unit, state)
        Else
            Dim theRow As DataGridViewRow = gridUnits.Rows(GetDeviceRowIndex(unit.IPAddress))
            If state = 0 Then
                theRow.Cells("colIRLearner").Value = "not found"
                theRow.Cells("colIRLearner").Style.ForeColor = Color.Red
                theRow.Cells("colIRLearner").Style.SelectionForeColor = Color.White
            ElseIf state = 1 Then
                theRow.Cells("colIRLearner").Value = "ready"
                theRow.Cells("colIRLearner").Style.ForeColor = Color.Green
                theRow.Cells("colIRLearner").Style.SelectionForeColor = Color.White
            ElseIf state = 2 Then
                theRow.Cells("colIRLearner").Value = "looking..."
                theRow.Cells("colIRLearner").Style.ForeColor = Color.Black
                theRow.Cells("colIRLearner").Style.SelectionForeColor = Color.White
            End If
        End If
    End Sub

End Class