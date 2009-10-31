Public Class frmMain

    ' The iViewerControl
    Private WithEvents iViewer As New iViewerControl.iViewer

    Public Function BooleanToInt(ByVal val As Boolean) As Integer
        If val Then
            Return 1
        Else
            Return 0
        End If
    End Function

    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Load settings from last use
        txtPort.Text = My.Settings.TCPServerPort.ToString

        ' Loop through available ethernet connections on your PC
        For Each anIP As String In iViewer.ListIPAddresses
            cboIPAddress.Items.Add(anIP)
        Next
        cboIPAddress.SelectedIndex = 0

        ' Start iViewer Server
        Me.btnRestart.PerformClick()
    End Sub

    Private Sub frmMain_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        ' IMPORTANT - CLOSE ALL CONNECTIONS IN IVIEWER BEFORE EXITING YOUR APPLICATION BY CALLING DISPOSE!
        iViewer.StopServer()
    End Sub

    Private Sub btnRestart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRestart.Click
        ' Stop server if its already running
        iViewer.StopServer()
        ' Set iViewer port to listen on
        My.Settings.TCPServerPort = txtPort.Text
        iViewer.Port = My.Settings.TCPServerPort
        ' Restart the iViewerControl server
        iViewer.StartServer()
    End Sub

    Private Sub OnServerStart() Handles iViewer.ServerStarted
        ' iViewerControl server started
        lblTCPStatus.Text = "TCP Server running on: " & iViewer.ListIPAddresses(0) & ":" & My.Settings.TCPServerPort
        Me.lblTCPStatus.ForeColor = Color.Green
    End Sub

    Private Sub OnServerStopped() Handles iViewer.ServerStopped
        ' iViewerControl server stopped for some reason
        lblTCPStatus.Text = "TCP Server not running."
        Me.lblTCPStatus.ForeColor = Color.Red
    End Sub

    ' An iViewer client connected
    Private Sub OnConnect(ByVal client As Long) Handles iViewer.OnConnect
        ' Do something if required
        Log("Client Connected: " & iViewer.GetClient(client).structIP)
    End Sub

    ' An iViewer client disconnected
    Private Sub OnDisconnect(ByVal client As Long) Handles iViewer.OnDisconnect
        ' Do something if required
        Log("Client Disconnected.")
    End Sub

    Private Sub OnInit(ByVal client As Long) Handles iViewer.OnInit

        ' iViewer connected and received initialization request
        ' Send any initial states of buttons, etc, here.
        'iViewer.SendMsg("d1=1", client)
        'iViewer.SendMsg("a1=1000", client)
        'iViewer.SendMsg("s1=Example Text", client)

        Log("Initialize")
    End Sub

    Private Sub OnDigital(ByVal join As Integer, ByVal state As Boolean, ByVal client As Long) Handles iViewer.OnDigital
        ' Digital Join (button, subpage, slider press, etc)

        Log("Digital Join: " & join & " = " & BooleanToInt(state))

        ' Send reply to iViewer to complete join feedback - only necessary if you need feedback on the iPhone
        ' Messages are automatically appended with the \x03 hex byte before being sent
        iViewer.SendMsg("d" & join & "=" & BooleanToInt(state), client)

    End Sub

    Private Sub OnAnalog(ByVal join As Integer, ByVal value As Integer, ByVal client As Long) Handles iViewer.OnAnalog
        ' Analog Join (gauge, slider, etc)

        Log("Analog Join: " & join & " = " & value)

        ' Send reply to iViewer to complete join feedback - only necessary if you need feedback on the iPhone
        iViewer.SendMsg("a" & join & "=" & value, client)
    End Sub

    Private Sub OnSerial(ByVal join As Integer, ByVal value As String, ByVal client As Long) Handles iViewer.OnSerial
        ' Serial Join (text data, input fields, etc)

        Log("Serial Join: " & join & " = " & value)

        ' Send reply to iViewer to complete join feedback - only necessary if you need feedback on the iPhone
        iViewer.SendMsg("s" & join & "=" & value, client)
    End Sub

    Private Sub OnListDigital(ByVal join As Integer, ByVal state As Boolean, ByVal listJoin As Integer, ByVal itemNum As Integer, ByVal client As Long) Handles iViewer.OnListDigital
        ' Digital List Join
    End Sub

    Private Sub OnListAnalog(ByVal join As Integer, ByVal value As Integer, ByVal listJoin As Integer, ByVal itemNum As Integer, ByVal client As Long) Handles iViewer.OnListAnalog
        ' Analog List Join
    End Sub

    Private Sub OnListSerial(ByVal join As Integer, ByVal value As String, ByVal listJoin As Integer, ByVal itemNum As Integer, ByVal client As Long) Handles iViewer.OnListSerial
        ' Serial List Join
    End Sub

    Private Sub Log(ByVal msg As String)
        ' Append to log textbox
        Me.txtLog.Text &= msg & Environment.NewLine

        ' Scroll new log data into view
        Me.txtLog.SelectionStart = Me.txtLog.TextLength
        Me.txtLog.ScrollToCaret()
    End Sub
End Class
