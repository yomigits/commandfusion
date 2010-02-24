<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.cboIPAddress = New System.Windows.Forms.ComboBox
        Me.lblIPAddress = New System.Windows.Forms.Label
        Me.lblServerPort = New System.Windows.Forms.Label
        Me.txtPort = New System.Windows.Forms.TextBox
        Me.btnRestart = New System.Windows.Forms.Button
        Me.lblTCPStatus = New System.Windows.Forms.Label
        Me.lblStatustitle = New System.Windows.Forms.Label
        Me.txtLog = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'cboIPAddress
        '
        Me.cboIPAddress.FormattingEnabled = True
        Me.cboIPAddress.Location = New System.Drawing.Point(107, 12)
        Me.cboIPAddress.Name = "cboIPAddress"
        Me.cboIPAddress.Size = New System.Drawing.Size(150, 21)
        Me.cboIPAddress.TabIndex = 6
        '
        'lblIPAddress
        '
        Me.lblIPAddress.AutoSize = True
        Me.lblIPAddress.Location = New System.Drawing.Point(6, 15)
        Me.lblIPAddress.Name = "lblIPAddress"
        Me.lblIPAddress.Size = New System.Drawing.Size(95, 13)
        Me.lblIPAddress.TabIndex = 5
        Me.lblIPAddress.Text = "Server IP Address:"
        '
        'lblServerPort
        '
        Me.lblServerPort.AutoSize = True
        Me.lblServerPort.Location = New System.Drawing.Point(38, 44)
        Me.lblServerPort.Name = "lblServerPort"
        Me.lblServerPort.Size = New System.Drawing.Size(63, 13)
        Me.lblServerPort.TabIndex = 7
        Me.lblServerPort.Text = "Server Port:"
        '
        'txtPort
        '
        Me.txtPort.Location = New System.Drawing.Point(107, 39)
        Me.txtPort.Name = "txtPort"
        Me.txtPort.Size = New System.Drawing.Size(85, 20)
        Me.txtPort.TabIndex = 8
        '
        'btnRestart
        '
        Me.btnRestart.Location = New System.Drawing.Point(107, 65)
        Me.btnRestart.Name = "btnRestart"
        Me.btnRestart.Size = New System.Drawing.Size(150, 23)
        Me.btnRestart.TabIndex = 9
        Me.btnRestart.Text = "Restart Server"
        Me.btnRestart.UseVisualStyleBackColor = True
        '
        'lblTCPStatus
        '
        Me.lblTCPStatus.ForeColor = System.Drawing.Color.Red
        Me.lblTCPStatus.Location = New System.Drawing.Point(6, 104)
        Me.lblTCPStatus.Name = "lblTCPStatus"
        Me.lblTCPStatus.Size = New System.Drawing.Size(251, 43)
        Me.lblTCPStatus.TabIndex = 14
        Me.lblTCPStatus.Text = "TCP server is not running."
        '
        'lblStatustitle
        '
        Me.lblStatustitle.AutoSize = True
        Me.lblStatustitle.Location = New System.Drawing.Point(6, 88)
        Me.lblStatustitle.Name = "lblStatustitle"
        Me.lblStatustitle.Size = New System.Drawing.Size(74, 13)
        Me.lblStatustitle.TabIndex = 13
        Me.lblStatustitle.Text = "Server Status:"
        '
        'txtLog
        '
        Me.txtLog.Location = New System.Drawing.Point(9, 150)
        Me.txtLog.Multiline = True
        Me.txtLog.Name = "txtLog"
        Me.txtLog.Size = New System.Drawing.Size(251, 104)
        Me.txtLog.TabIndex = 15
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(272, 266)
        Me.Controls.Add(Me.txtLog)
        Me.Controls.Add(Me.lblTCPStatus)
        Me.Controls.Add(Me.lblStatustitle)
        Me.Controls.Add(Me.cboIPAddress)
        Me.Controls.Add(Me.lblIPAddress)
        Me.Controls.Add(Me.lblServerPort)
        Me.Controls.Add(Me.txtPort)
        Me.Controls.Add(Me.btnRestart)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmMain"
        Me.Text = "iViewerControl Sample"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cboIPAddress As System.Windows.Forms.ComboBox
    Friend WithEvents lblIPAddress As System.Windows.Forms.Label
    Friend WithEvents lblServerPort As System.Windows.Forms.Label
    Friend WithEvents txtPort As System.Windows.Forms.TextBox
    Friend WithEvents btnRestart As System.Windows.Forms.Button
    Friend WithEvents lblTCPStatus As System.Windows.Forms.Label
    Friend WithEvents lblStatustitle As System.Windows.Forms.Label
    Friend WithEvents txtLog As System.Windows.Forms.TextBox

End Class
