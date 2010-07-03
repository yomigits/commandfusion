<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CFPluginDemo
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
        Me.btnAbout = New System.Windows.Forms.Button
        Me.lblTitle = New System.Windows.Forms.Label
        Me.lblStatus = New System.Windows.Forms.Label
        Me.btnAddSystem = New System.Windows.Forms.Button
        Me.cboSystems = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnRequest = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'btnAbout
        '
        Me.btnAbout.Location = New System.Drawing.Point(12, 137)
        Me.btnAbout.Name = "btnAbout"
        Me.btnAbout.Size = New System.Drawing.Size(114, 23)
        Me.btnAbout.TabIndex = 0
        Me.btnAbout.Text = "About This Plugin"
        Me.btnAbout.UseVisualStyleBackColor = True
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Location = New System.Drawing.Point(13, 13)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(72, 13)
        Me.lblTitle.TabIndex = 1
        Me.lblTitle.Text = "Project Open:"
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.ForeColor = System.Drawing.Color.Red
        Me.lblStatus.Location = New System.Drawing.Point(92, 13)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(25, 13)
        Me.lblStatus.TabIndex = 2
        Me.lblStatus.Text = "NO"
        '
        'btnAddSystem
        '
        Me.btnAddSystem.Location = New System.Drawing.Point(12, 108)
        Me.btnAddSystem.Name = "btnAddSystem"
        Me.btnAddSystem.Size = New System.Drawing.Size(114, 23)
        Me.btnAddSystem.TabIndex = 3
        Me.btnAddSystem.Text = "Add Demo System"
        Me.btnAddSystem.UseVisualStyleBackColor = True
        '
        'cboSystems
        '
        Me.cboSystems.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSystems.FormattingEnabled = True
        Me.cboSystems.Location = New System.Drawing.Point(12, 52)
        Me.cboSystems.Name = "cboSystems"
        Me.cboSystems.Size = New System.Drawing.Size(160, 21)
        Me.cboSystems.TabIndex = 4
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 36)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(85, 13)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Project Systems:"
        '
        'btnRequest
        '
        Me.btnRequest.Location = New System.Drawing.Point(12, 79)
        Me.btnRequest.Name = "btnRequest"
        Me.btnRequest.Size = New System.Drawing.Size(114, 23)
        Me.btnRequest.TabIndex = 6
        Me.btnRequest.Text = "Request Systems"
        Me.btnRequest.UseVisualStyleBackColor = True
        '
        'CFPluginDemo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(184, 178)
        Me.Controls.Add(Me.btnRequest)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cboSystems)
        Me.Controls.Add(Me.btnAddSystem)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.btnAbout)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MinimumSize = New System.Drawing.Size(184, 145)
        Me.Name = "CFPluginDemo"
        Me.Text = "CF Plugin Demo"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnAbout As System.Windows.Forms.Button
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents btnAddSystem As System.Windows.Forms.Button
    Friend WithEvents cboSystems As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnRequest As System.Windows.Forms.Button
End Class
