Imports System.Windows.Forms

Public Class frmIRLearn

    Public LearntCommand As String
    Public LearntFrequency As String
    Friend GCDevice As GC100

    Public Event AddCommand()

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If Me.txtCommand.Text = Nothing Or Me.txtName.Text = Nothing Or Me.cboSystem.SelectedItem Is Nothing Then
            MsgBox("You must select a system and give the command a name and value before assigning it to your project.")
            If Me.txtCommand.Text = Nothing Then
                Me.txtCommand.Focus()
            ElseIf Me.txtName.Text = Nothing Then
                Me.txtName.Focus()
            Else
                Me.cboSystem.Focus()
            End If
            Exit Sub
        End If

        RaiseEvent AddCommand()

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Hide()

    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Hide()
    End Sub

    Private Sub cboIRPort_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboIRPort.SelectedIndexChanged
        UpdateCommand()
    End Sub

    Private Sub numRepeat_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles numRepeat.ValueChanged
        UpdateCommand()
    End Sub

    Public Sub UpdateCommand()
        Dim IRPort As String = "3:1"
        Dim offset As String = "0"
        Select Case Me.cboIRPort.SelectedItem
            Case "1", "2", "3"
                IRPort = "4:" & cboIRPort.SelectedItem
            Case Else
                IRPort = "5:" & -3 + cboIRPort.SelectedItem
        End Select
        If numRepeat.Value > 1 Then
            offset = "1"
        End If
        txtCommand.Text = "sendir," & IRPort & ",0," & LearntFrequency & "," & numRepeat.Value & "," & offset & LearntCommand & "\x0D"
    End Sub

    Private Sub btnTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTest.Click
        If GCDevice IsNot Nothing Then
            GCDevice.TCPSend(txtCommand.Text)
        End If
    End Sub

    Private Sub btnCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopy.Click
        My.Computer.Clipboard.SetText(Me.txtCommand.Text)
    End Sub
End Class
