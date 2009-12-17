<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class GCHelper
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
        Me.components = New System.ComponentModel.Container
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.gridUnits = New System.Windows.Forms.DataGridView
        Me.btnRefresh = New System.Windows.Forms.Button
        Me.cmnuUnit = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.BlinkLEDToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuBlinkStart = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuBlinkStop = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuInitIR = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAddSystems = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuConfigureDevice = New System.Windows.Forms.ToolStripMenuItem
        Me.lblTitle = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.colModel = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.colIPAddress = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.colMac = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.colIRLearner = New System.Windows.Forms.DataGridViewTextBoxColumn
        CType(Me.gridUnits, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmnuUnit.SuspendLayout()
        Me.SuspendLayout()
        '
        'gridUnits
        '
        Me.gridUnits.AllowUserToAddRows = False
        Me.gridUnits.AllowUserToDeleteRows = False
        Me.gridUnits.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gridUnits.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.gridUnits.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridUnits.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colModel, Me.colIPAddress, Me.colMac, Me.colIRLearner})
        Me.gridUnits.Location = New System.Drawing.Point(12, 25)
        Me.gridUnits.MultiSelect = False
        Me.gridUnits.Name = "gridUnits"
        Me.gridUnits.ReadOnly = True
        Me.gridUnits.RowHeadersVisible = False
        Me.gridUnits.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridUnits.Size = New System.Drawing.Size(404, 101)
        Me.gridUnits.TabIndex = 1
        '
        'btnRefresh
        '
        Me.btnRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRefresh.Location = New System.Drawing.Point(341, 132)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(75, 23)
        Me.btnRefresh.TabIndex = 3
        Me.btnRefresh.Text = "Refresh List"
        Me.btnRefresh.UseVisualStyleBackColor = True
        '
        'cmnuUnit
        '
        Me.cmnuUnit.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BlinkLEDToolStripMenuItem, Me.mnuInitIR, Me.mnuAddSystems, Me.ToolStripSeparator1, Me.mnuConfigureDevice})
        Me.cmnuUnit.Name = "cmnuUnit"
        Me.cmnuUnit.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.cmnuUnit.Size = New System.Drawing.Size(198, 98)
        '
        'BlinkLEDToolStripMenuItem
        '
        Me.BlinkLEDToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuBlinkStart, Me.mnuBlinkStop})
        Me.BlinkLEDToolStripMenuItem.Name = "BlinkLEDToolStripMenuItem"
        Me.BlinkLEDToolStripMenuItem.Size = New System.Drawing.Size(197, 22)
        Me.BlinkLEDToolStripMenuItem.Text = "Blink LED"
        '
        'mnuBlinkStart
        '
        Me.mnuBlinkStart.Name = "mnuBlinkStart"
        Me.mnuBlinkStart.Size = New System.Drawing.Size(109, 22)
        Me.mnuBlinkStart.Text = "Start"
        '
        'mnuBlinkStop
        '
        Me.mnuBlinkStop.Name = "mnuBlinkStop"
        Me.mnuBlinkStop.Size = New System.Drawing.Size(109, 22)
        Me.mnuBlinkStop.Text = "Stop"
        '
        'mnuInitIR
        '
        Me.mnuInitIR.Name = "mnuInitIR"
        Me.mnuInitIR.Size = New System.Drawing.Size(197, 22)
        Me.mnuInitIR.Text = "Initialize IR Learner"
        '
        'mnuAddSystems
        '
        Me.mnuAddSystems.Name = "mnuAddSystems"
        Me.mnuAddSystems.Size = New System.Drawing.Size(197, 22)
        Me.mnuAddSystems.Text = "Add Systems to Project"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(194, 6)
        '
        'mnuConfigureDevice
        '
        Me.mnuConfigureDevice.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.mnuConfigureDevice.Name = "mnuConfigureDevice"
        Me.mnuConfigureDevice.Size = New System.Drawing.Size(197, 22)
        Me.mnuConfigureDevice.Text = "Configure Device..."
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Location = New System.Drawing.Point(12, 9)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(146, 13)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "GlobalCache Devices Found:"
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 137)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(309, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "GlobalCache units may take up to 60 seconds to be discovered."
        '
        'colModel
        '
        Me.colModel.FillWeight = 99.49239!
        Me.colModel.HeaderText = "Model"
        Me.colModel.Name = "colModel"
        Me.colModel.ReadOnly = True
        '
        'colIPAddress
        '
        Me.colIPAddress.FillWeight = 99.49239!
        Me.colIPAddress.HeaderText = "IP Address"
        Me.colIPAddress.Name = "colIPAddress"
        Me.colIPAddress.ReadOnly = True
        '
        'colMac
        '
        Me.colMac.FillWeight = 99.49239!
        Me.colMac.HeaderText = "MAC Address"
        Me.colMac.Name = "colMac"
        Me.colMac.ReadOnly = True
        '
        'colIRLearner
        '
        Me.colIRLearner.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.colIRLearner.DefaultCellStyle = DataGridViewCellStyle2
        Me.colIRLearner.FillWeight = 101.5228!
        Me.colIRLearner.HeaderText = "IR Learner"
        Me.colIRLearner.Name = "colIRLearner"
        Me.colIRLearner.ReadOnly = True
        Me.colIRLearner.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.colIRLearner.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.colIRLearner.Width = 80
        '
        'GCHelper
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(428, 164)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.gridUnits)
        Me.Controls.Add(Me.btnRefresh)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MinimumSize = New System.Drawing.Size(428, 164)
        Me.Name = "GCHelper"
        Me.Text = "GlobalCache Helper"
        CType(Me.gridUnits, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmnuUnit.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents gridUnits As System.Windows.Forms.DataGridView
    Friend WithEvents btnRefresh As System.Windows.Forms.Button
    Friend WithEvents cmnuUnit As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents BlinkLEDToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuInitIR As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAddSystems As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuBlinkStart As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuBlinkStop As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuConfigureDevice As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents colModel As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colIPAddress As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colMac As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colIRLearner As System.Windows.Forms.DataGridViewTextBoxColumn

End Class
