<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmIRLearn
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.OK_Button = New System.Windows.Forms.Button
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.lblSystem = New System.Windows.Forms.Label
        Me.cboSystem = New System.Windows.Forms.ComboBox
        Me.lblInfo = New System.Windows.Forms.Label
        Me.cboIRPort = New System.Windows.Forms.ComboBox
        Me.lblIRPort = New System.Windows.Forms.Label
        Me.lblName = New System.Windows.Forms.Label
        Me.txtName = New System.Windows.Forms.TextBox
        Me.txtCommand = New System.Windows.Forms.TextBox
        Me.lblCommand = New System.Windows.Forms.Label
        Me.lblRepeat = New System.Windows.Forms.Label
        Me.numRepeat = New System.Windows.Forms.NumericUpDown
        Me.btnTest = New System.Windows.Forms.Button
        Me.btnCopy = New System.Windows.Forms.Button
        CType(Me.numRepeat, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OK_Button.Location = New System.Drawing.Point(232, 279)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 13
        Me.OK_Button.Text = "Assign"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(305, 279)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
        Me.Cancel_Button.TabIndex = 14
        Me.Cancel_Button.Text = "Cancel"
        '
        'lblSystem
        '
        Me.lblSystem.AutoSize = True
        Me.lblSystem.Location = New System.Drawing.Point(22, 43)
        Me.lblSystem.Name = "lblSystem"
        Me.lblSystem.Size = New System.Drawing.Size(44, 13)
        Me.lblSystem.TabIndex = 1
        Me.lblSystem.Text = "System:"
        '
        'cboSystem
        '
        Me.cboSystem.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSystem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSystem.FormattingEnabled = True
        Me.cboSystem.Location = New System.Drawing.Point(72, 40)
        Me.cboSystem.Name = "cboSystem"
        Me.cboSystem.Size = New System.Drawing.Size(300, 21)
        Me.cboSystem.TabIndex = 2
        '
        'lblInfo
        '
        Me.lblInfo.AutoSize = True
        Me.lblInfo.Location = New System.Drawing.Point(22, 13)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(187, 13)
        Me.lblInfo.TabIndex = 0
        Me.lblInfo.Text = "Assign IR command to current project."
        '
        'cboIRPort
        '
        Me.cboIRPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboIRPort.FormattingEnabled = True
        Me.cboIRPort.Location = New System.Drawing.Point(72, 67)
        Me.cboIRPort.Name = "cboIRPort"
        Me.cboIRPort.Size = New System.Drawing.Size(42, 21)
        Me.cboIRPort.TabIndex = 4
        '
        'lblIRPort
        '
        Me.lblIRPort.AutoSize = True
        Me.lblIRPort.Location = New System.Drawing.Point(23, 70)
        Me.lblIRPort.Name = "lblIRPort"
        Me.lblIRPort.Size = New System.Drawing.Size(43, 13)
        Me.lblIRPort.TabIndex = 3
        Me.lblIRPort.Text = "IR Port:"
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.Location = New System.Drawing.Point(28, 97)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(38, 13)
        Me.lblName.TabIndex = 7
        Me.lblName.Text = "Name:"
        '
        'txtName
        '
        Me.txtName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtName.Location = New System.Drawing.Point(72, 94)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(300, 20)
        Me.txtName.TabIndex = 8
        '
        'txtCommand
        '
        Me.txtCommand.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCommand.Location = New System.Drawing.Point(72, 120)
        Me.txtCommand.Multiline = True
        Me.txtCommand.Name = "txtCommand"
        Me.txtCommand.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtCommand.Size = New System.Drawing.Size(300, 153)
        Me.txtCommand.TabIndex = 10
        '
        'lblCommand
        '
        Me.lblCommand.AutoSize = True
        Me.lblCommand.Location = New System.Drawing.Point(9, 123)
        Me.lblCommand.Name = "lblCommand"
        Me.lblCommand.Size = New System.Drawing.Size(57, 13)
        Me.lblCommand.TabIndex = 9
        Me.lblCommand.Text = "Command:"
        '
        'lblRepeat
        '
        Me.lblRepeat.AutoSize = True
        Me.lblRepeat.Location = New System.Drawing.Point(120, 70)
        Me.lblRepeat.Name = "lblRepeat"
        Me.lblRepeat.Size = New System.Drawing.Size(76, 13)
        Me.lblRepeat.TabIndex = 5
        Me.lblRepeat.Text = "Repeat Count:"
        '
        'numRepeat
        '
        Me.numRepeat.Location = New System.Drawing.Point(202, 68)
        Me.numRepeat.Maximum = New Decimal(New Integer() {99, 0, 0, 0})
        Me.numRepeat.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.numRepeat.Name = "numRepeat"
        Me.numRepeat.Size = New System.Drawing.Size(40, 20)
        Me.numRepeat.TabIndex = 6
        Me.numRepeat.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'btnTest
        '
        Me.btnTest.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnTest.Location = New System.Drawing.Point(72, 279)
        Me.btnTest.Name = "btnTest"
        Me.btnTest.Size = New System.Drawing.Size(67, 23)
        Me.btnTest.TabIndex = 11
        Me.btnTest.Text = "Test"
        Me.btnTest.UseVisualStyleBackColor = True
        '
        'btnCopy
        '
        Me.btnCopy.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnCopy.AutoSize = True
        Me.btnCopy.Location = New System.Drawing.Point(145, 279)
        Me.btnCopy.Name = "btnCopy"
        Me.btnCopy.Size = New System.Drawing.Size(67, 23)
        Me.btnCopy.TabIndex = 12
        Me.btnCopy.Text = "Copy"
        Me.btnCopy.UseVisualStyleBackColor = True
        '
        'frmIRLearn
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(384, 316)
        Me.Controls.Add(Me.btnCopy)
        Me.Controls.Add(Me.Cancel_Button)
        Me.Controls.Add(Me.OK_Button)
        Me.Controls.Add(Me.btnTest)
        Me.Controls.Add(Me.numRepeat)
        Me.Controls.Add(Me.lblRepeat)
        Me.Controls.Add(Me.txtCommand)
        Me.Controls.Add(Me.lblCommand)
        Me.Controls.Add(Me.txtName)
        Me.Controls.Add(Me.lblName)
        Me.Controls.Add(Me.cboIRPort)
        Me.Controls.Add(Me.lblIRPort)
        Me.Controls.Add(Me.lblInfo)
        Me.Controls.Add(Me.cboSystem)
        Me.Controls.Add(Me.lblSystem)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(389, 341)
        Me.Name = "frmIRLearn"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "IR Command Detected"
        Me.TopMost = True
        CType(Me.numRepeat, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents lblSystem As System.Windows.Forms.Label
    Friend WithEvents cboSystem As System.Windows.Forms.ComboBox
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents cboIRPort As System.Windows.Forms.ComboBox
    Friend WithEvents lblIRPort As System.Windows.Forms.Label
    Friend WithEvents lblName As System.Windows.Forms.Label
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents txtCommand As System.Windows.Forms.TextBox
    Friend WithEvents lblCommand As System.Windows.Forms.Label
    Friend WithEvents lblRepeat As System.Windows.Forms.Label
    Friend WithEvents numRepeat As System.Windows.Forms.NumericUpDown
    Friend WithEvents btnTest As System.Windows.Forms.Button
    Friend WithEvents btnCopy As System.Windows.Forms.Button

End Class
