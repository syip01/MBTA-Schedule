<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
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
        Me.components = New System.ComponentModel.Container()
        Me.tmrUpdate = New System.Windows.Forms.Timer(Me.components)
        Me.lstSched = New System.Windows.Forms.ListBox()
        Me.lblArrivals = New System.Windows.Forms.Label()
        Me.radDirBoth = New System.Windows.Forms.RadioButton()
        Me.radDirOut = New System.Windows.Forms.RadioButton()
        Me.radDirIn = New System.Windows.Forms.RadioButton()
        Me.grpInputDir = New System.Windows.Forms.GroupBox()
        Me.grpListSort = New System.Windows.Forms.GroupBox()
        Me.radSortDescend = New System.Windows.Forms.RadioButton()
        Me.radSortAscend = New System.Windows.Forms.RadioButton()
        Me.cboDest = New System.Windows.Forms.ComboBox()
        Me.lblDest = New System.Windows.Forms.Label()
        Me.grpSortType = New System.Windows.Forms.GroupBox()
        Me.radDepart = New System.Windows.Forms.RadioButton()
        Me.radArrive = New System.Windows.Forms.RadioButton()
        Me.grpInputDir.SuspendLayout()
        Me.grpListSort.SuspendLayout()
        Me.grpSortType.SuspendLayout()
        Me.SuspendLayout()
        '
        'tmrUpdate
        '
        Me.tmrUpdate.Enabled = True
        Me.tmrUpdate.Interval = 5000
        '
        'lstSched
        '
        Me.lstSched.FormattingEnabled = True
        Me.lstSched.Location = New System.Drawing.Point(12, 38)
        Me.lstSched.Name = "lstSched"
        Me.lstSched.Size = New System.Drawing.Size(458, 69)
        Me.lstSched.TabIndex = 0
        '
        'lblArrivals
        '
        Me.lblArrivals.AutoSize = True
        Me.lblArrivals.Location = New System.Drawing.Point(9, 19)
        Me.lblArrivals.Name = "lblArrivals"
        Me.lblArrivals.Size = New System.Drawing.Size(149, 13)
        Me.lblArrivals.TabIndex = 0
        Me.lblArrivals.Text = "Expected Arrivals/Departures:"
        '
        'radDirBoth
        '
        Me.radDirBoth.AutoSize = True
        Me.radDirBoth.Location = New System.Drawing.Point(6, 19)
        Me.radDirBoth.Name = "radDirBoth"
        Me.radDirBoth.Size = New System.Drawing.Size(47, 17)
        Me.radDirBoth.TabIndex = 0
        Me.radDirBoth.Text = "Both"
        Me.radDirBoth.UseVisualStyleBackColor = True
        '
        'radDirOut
        '
        Me.radDirOut.AutoSize = True
        Me.radDirOut.Checked = True
        Me.radDirOut.Location = New System.Drawing.Point(6, 42)
        Me.radDirOut.Name = "radDirOut"
        Me.radDirOut.Size = New System.Drawing.Size(72, 17)
        Me.radDirOut.TabIndex = 1
        Me.radDirOut.TabStop = True
        Me.radDirOut.Text = "Outbound"
        Me.radDirOut.UseVisualStyleBackColor = True
        '
        'radDirIn
        '
        Me.radDirIn.AutoSize = True
        Me.radDirIn.Location = New System.Drawing.Point(6, 65)
        Me.radDirIn.Name = "radDirIn"
        Me.radDirIn.Size = New System.Drawing.Size(64, 17)
        Me.radDirIn.TabIndex = 2
        Me.radDirIn.Text = "Inbound"
        Me.radDirIn.UseVisualStyleBackColor = True
        '
        'grpInputDir
        '
        Me.grpInputDir.Controls.Add(Me.radDirBoth)
        Me.grpInputDir.Controls.Add(Me.radDirIn)
        Me.grpInputDir.Controls.Add(Me.radDirOut)
        Me.grpInputDir.Location = New System.Drawing.Point(267, 113)
        Me.grpInputDir.Name = "grpInputDir"
        Me.grpInputDir.Size = New System.Drawing.Size(92, 87)
        Me.grpInputDir.TabIndex = 2
        Me.grpInputDir.TabStop = False
        Me.grpInputDir.Text = "Direction:"
        '
        'grpListSort
        '
        Me.grpListSort.Controls.Add(Me.radSortDescend)
        Me.grpListSort.Controls.Add(Me.radSortAscend)
        Me.grpListSort.Location = New System.Drawing.Point(365, 113)
        Me.grpListSort.Name = "grpListSort"
        Me.grpListSort.Size = New System.Drawing.Size(105, 69)
        Me.grpListSort.TabIndex = 3
        Me.grpListSort.TabStop = False
        Me.grpListSort.Text = "List Order:"
        '
        'radSortDescend
        '
        Me.radSortDescend.AutoSize = True
        Me.radSortDescend.Location = New System.Drawing.Point(6, 42)
        Me.radSortDescend.Name = "radSortDescend"
        Me.radSortDescend.Size = New System.Drawing.Size(82, 17)
        Me.radSortDescend.TabIndex = 1
        Me.radSortDescend.Text = "Descending"
        Me.radSortDescend.UseVisualStyleBackColor = True
        '
        'radSortAscend
        '
        Me.radSortAscend.AutoSize = True
        Me.radSortAscend.Checked = True
        Me.radSortAscend.Location = New System.Drawing.Point(6, 19)
        Me.radSortAscend.Name = "radSortAscend"
        Me.radSortAscend.Size = New System.Drawing.Size(75, 17)
        Me.radSortAscend.TabIndex = 0
        Me.radSortAscend.TabStop = True
        Me.radSortAscend.Text = "Ascending"
        Me.radSortAscend.UseVisualStyleBackColor = True
        '
        'cboDest
        '
        Me.cboDest.FormattingEnabled = True
        Me.cboDest.Items.AddRange(New Object() {"Quincy Center", "Braintree", "South Station", "North Station", "Back Bay"})
        Me.cboDest.Location = New System.Drawing.Point(12, 129)
        Me.cboDest.Name = "cboDest"
        Me.cboDest.Size = New System.Drawing.Size(234, 21)
        Me.cboDest.TabIndex = 1
        '
        'lblDest
        '
        Me.lblDest.AutoSize = True
        Me.lblDest.Location = New System.Drawing.Point(12, 113)
        Me.lblDest.Name = "lblDest"
        Me.lblDest.Size = New System.Drawing.Size(63, 13)
        Me.lblDest.TabIndex = 1
        Me.lblDest.Text = "Destination:"
        '
        'grpSortType
        '
        Me.grpSortType.Controls.Add(Me.radArrive)
        Me.grpSortType.Controls.Add(Me.radDepart)
        Me.grpSortType.Location = New System.Drawing.Point(365, 188)
        Me.grpSortType.Name = "grpSortType"
        Me.grpSortType.Size = New System.Drawing.Size(105, 66)
        Me.grpSortType.TabIndex = 4
        Me.grpSortType.TabStop = False
        Me.grpSortType.Text = "Sort By:"
        '
        'radDepart
        '
        Me.radDepart.AutoSize = True
        Me.radDepart.Checked = True
        Me.radDepart.Location = New System.Drawing.Point(6, 19)
        Me.radDepart.Name = "radDepart"
        Me.radDepart.Size = New System.Drawing.Size(77, 17)
        Me.radDepart.TabIndex = 0
        Me.radDepart.TabStop = True
        Me.radDepart.Text = "Departures"
        Me.radDepart.UseVisualStyleBackColor = True
        '
        'radArrive
        '
        Me.radArrive.AutoSize = True
        Me.radArrive.Location = New System.Drawing.Point(6, 42)
        Me.radArrive.Name = "radArrive"
        Me.radArrive.Size = New System.Drawing.Size(59, 17)
        Me.radArrive.TabIndex = 1
        Me.radArrive.Text = "Arrivals"
        Me.radArrive.UseVisualStyleBackColor = True
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(482, 266)
        Me.Controls.Add(Me.grpSortType)
        Me.Controls.Add(Me.lblDest)
        Me.Controls.Add(Me.cboDest)
        Me.Controls.Add(Me.grpListSort)
        Me.Controls.Add(Me.grpInputDir)
        Me.Controls.Add(Me.lblArrivals)
        Me.Controls.Add(Me.lstSched)
        Me.Name = "frmMain"
        Me.Text = "MBTA Schedule"
        Me.grpInputDir.ResumeLayout(False)
        Me.grpInputDir.PerformLayout()
        Me.grpListSort.ResumeLayout(False)
        Me.grpListSort.PerformLayout()
        Me.grpSortType.ResumeLayout(False)
        Me.grpSortType.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents tmrUpdate As Timer
    Friend WithEvents lstSched As ListBox
    Friend WithEvents lblArrivals As Label
    Friend WithEvents radDirBoth As RadioButton
    Friend WithEvents radDirOut As RadioButton
    Friend WithEvents radDirIn As RadioButton
    Friend WithEvents grpInputDir As GroupBox
    Friend WithEvents grpListSort As GroupBox
    Friend WithEvents radSortDescend As RadioButton
    Friend WithEvents radSortAscend As RadioButton
    Friend WithEvents cboDest As ComboBox
    Friend WithEvents lblDest As Label
    Friend WithEvents grpSortType As GroupBox
    Friend WithEvents radArrive As RadioButton
    Friend WithEvents radDepart As RadioButton
End Class
