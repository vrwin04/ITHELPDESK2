<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class DashboardForm
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        pnlHeader = New Panel()
        lblUser = New Label()
        lblLogo = New Label()
        pnlSidebar = New Panel()
        btnNavLogout = New Button()
        btnNavUsers = New Button()
        btnNavTickets = New Button()
        btnNavDashboard = New Button()
        pnlContent = New Panel()
        pnlViewTickets = New Panel()
        lblStats = New Label()
        flpHistory = New FlowLayoutPanel()
        dgvTickets = New DataGridView()
        btnDelete = New Button()
        btnAction = New Button()
        btnExport = New Button()
        btnRefresh = New Button()
        cmbStatusFilter = New ComboBox()
        txtSearch = New TextBox()
        pnlViewDashboard = New Panel()
        picGraph = New PictureBox()
        pnlTileResolved = New Panel()
        lblTileResolvedVal = New Label()
        Label6 = New Label()
        Panel6 = New Panel()
        pnlTilePending = New Panel()
        lblTilePendingVal = New Label()
        Label4 = New Label()
        Panel4 = New Panel()
        pnlTileTotal = New Panel()
        lblTileTotalVal = New Label()
        Label2 = New Label()
        Panel2 = New Panel()
        pnlHeader.SuspendLayout()
        pnlSidebar.SuspendLayout()
        pnlContent.SuspendLayout()
        pnlViewTickets.SuspendLayout()
        CType(dgvTickets, ComponentModel.ISupportInitialize).BeginInit()
        pnlViewDashboard.SuspendLayout()
        CType(picGraph, ComponentModel.ISupportInitialize).BeginInit()
        pnlTileResolved.SuspendLayout()
        pnlTilePending.SuspendLayout()
        pnlTileTotal.SuspendLayout()
        SuspendLayout()
        ' 
        ' pnlHeader
        ' 
        pnlHeader.BackColor = Color.White
        pnlHeader.Controls.Add(lblUser)
        pnlHeader.Controls.Add(lblLogo)
        pnlHeader.Dock = DockStyle.Top
        pnlHeader.Location = New Point(274, 0)
        pnlHeader.Margin = New Padding(3, 4, 3, 4)
        pnlHeader.Name = "pnlHeader"
        pnlHeader.Size = New Size(1189, 80)
        pnlHeader.TabIndex = 1
        ' 
        ' lblUser
        ' 
        lblUser.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        lblUser.AutoSize = True
        lblUser.Font = New Font("Segoe UI", 10.0F)
        lblUser.ForeColor = Color.DimGray
        lblUser.Location = New Point(971, 27)
        lblUser.Name = "lblUser"
        lblUser.RightToLeft = RightToLeft.No
        lblUser.Size = New Size(155, 23)
        lblUser.TabIndex = 1
        lblUser.Text = "Welcome, User (ID)"
        lblUser.TextAlign = ContentAlignment.TopRight
        ' 
        ' lblLogo
        ' 
        lblLogo.AutoSize = True
        lblLogo.Font = New Font("Segoe UI", 16.0F, FontStyle.Bold)
        lblLogo.ForeColor = Color.FromArgb(CByte(60), CByte(100), CByte(200))
        lblLogo.Location = New Point(23, 16)
        lblLogo.Name = "lblLogo"
        lblLogo.Size = New Size(212, 37)
        lblLogo.TabIndex = 0
        lblLogo.Text = "HELPDESK PRO"
        ' 
        ' pnlSidebar
        ' 
        pnlSidebar.BackColor = Color.FromArgb(CByte(20), CByte(25), CByte(45))
        pnlSidebar.Controls.Add(btnNavLogout)
        pnlSidebar.Controls.Add(btnNavUsers)
        pnlSidebar.Controls.Add(btnNavTickets)
        pnlSidebar.Controls.Add(btnNavDashboard)
        pnlSidebar.Dock = DockStyle.Left
        pnlSidebar.Location = New Point(0, 0)
        pnlSidebar.Margin = New Padding(3, 4, 3, 4)
        pnlSidebar.Name = "pnlSidebar"
        pnlSidebar.Size = New Size(274, 960)
        pnlSidebar.TabIndex = 0
        ' 
        ' btnNavLogout
        ' 
        btnNavLogout.Cursor = Cursors.Hand
        btnNavLogout.Dock = DockStyle.Bottom
        btnNavLogout.FlatAppearance.BorderSize = 0
        btnNavLogout.FlatStyle = FlatStyle.Flat
        btnNavLogout.Font = New Font("Segoe UI", 10.0F)
        btnNavLogout.ForeColor = Color.FromArgb(CByte(200), CByte(200), CByte(200))
        btnNavLogout.Location = New Point(0, 893)
        btnNavLogout.Margin = New Padding(3, 4, 3, 4)
        btnNavLogout.Name = "btnNavLogout"
        btnNavLogout.Padding = New Padding(23, 0, 0, 0)
        btnNavLogout.Size = New Size(274, 67)
        btnNavLogout.TabIndex = 3
        btnNavLogout.Text = "  🚪  Log Out"
        btnNavLogout.TextAlign = ContentAlignment.MiddleLeft
        btnNavLogout.UseVisualStyleBackColor = True
        ' 
        ' btnNavUsers
        ' 
        btnNavUsers.Cursor = Cursors.Hand
        btnNavUsers.Dock = DockStyle.Top
        btnNavUsers.FlatAppearance.BorderSize = 0
        btnNavUsers.FlatStyle = FlatStyle.Flat
        btnNavUsers.Font = New Font("Segoe UI", 10.0F)
        btnNavUsers.ForeColor = Color.FromArgb(CByte(200), CByte(200), CByte(200))
        btnNavUsers.Location = New Point(0, 134)
        btnNavUsers.Margin = New Padding(3, 4, 3, 4)
        btnNavUsers.Name = "btnNavUsers"
        btnNavUsers.Padding = New Padding(23, 0, 0, 0)
        btnNavUsers.Size = New Size(274, 67)
        btnNavUsers.TabIndex = 2
        btnNavUsers.Text = "  👥  Manage Users"
        btnNavUsers.TextAlign = ContentAlignment.MiddleLeft
        btnNavUsers.UseVisualStyleBackColor = True
        ' 
        ' btnNavTickets
        ' 
        btnNavTickets.Cursor = Cursors.Hand
        btnNavTickets.Dock = DockStyle.Top
        btnNavTickets.FlatAppearance.BorderSize = 0
        btnNavTickets.FlatStyle = FlatStyle.Flat
        btnNavTickets.Font = New Font("Segoe UI", 10.0F)
        btnNavTickets.ForeColor = Color.FromArgb(CByte(200), CByte(200), CByte(200))
        btnNavTickets.Location = New Point(0, 67)
        btnNavTickets.Margin = New Padding(3, 4, 3, 4)
        btnNavTickets.Name = "btnNavTickets"
        btnNavTickets.Padding = New Padding(23, 0, 0, 0)
        btnNavTickets.Size = New Size(274, 67)
        btnNavTickets.TabIndex = 1
        btnNavTickets.Text = "  🎫  Ticket List"
        btnNavTickets.TextAlign = ContentAlignment.MiddleLeft
        btnNavTickets.UseVisualStyleBackColor = True
        ' 
        ' btnNavDashboard
        ' 
        btnNavDashboard.Cursor = Cursors.Hand
        btnNavDashboard.Dock = DockStyle.Top
        btnNavDashboard.FlatAppearance.BorderSize = 0
        btnNavDashboard.FlatStyle = FlatStyle.Flat
        btnNavDashboard.Font = New Font("Segoe UI", 10.0F)
        btnNavDashboard.ForeColor = Color.FromArgb(CByte(200), CByte(200), CByte(200))
        btnNavDashboard.Location = New Point(0, 0)
        btnNavDashboard.Margin = New Padding(3, 4, 3, 4)
        btnNavDashboard.Name = "btnNavDashboard"
        btnNavDashboard.Padding = New Padding(23, 0, 0, 0)
        btnNavDashboard.Size = New Size(274, 67)
        btnNavDashboard.TabIndex = 0
        btnNavDashboard.Text = "  📊  Dashboard"
        btnNavDashboard.TextAlign = ContentAlignment.MiddleLeft
        btnNavDashboard.UseVisualStyleBackColor = True
        ' 
        ' pnlContent
        ' 
        pnlContent.Controls.Add(pnlViewTickets)
        pnlContent.Controls.Add(pnlViewDashboard)
        pnlContent.Dock = DockStyle.Fill
        pnlContent.Location = New Point(274, 80)
        pnlContent.Margin = New Padding(3, 4, 3, 4)
        pnlContent.Name = "pnlContent"
        pnlContent.Padding = New Padding(23, 27, 23, 27)
        pnlContent.Size = New Size(1189, 880)
        pnlContent.TabIndex = 2
        ' 
        ' pnlViewTickets
        ' 
        pnlViewTickets.Controls.Add(lblStats)
        pnlViewTickets.Controls.Add(flpHistory)
        pnlViewTickets.Controls.Add(dgvTickets)
        pnlViewTickets.Controls.Add(btnDelete)
        pnlViewTickets.Controls.Add(btnAction)
        pnlViewTickets.Controls.Add(btnExport)
        pnlViewTickets.Controls.Add(btnRefresh)
        pnlViewTickets.Controls.Add(cmbStatusFilter)
        pnlViewTickets.Controls.Add(txtSearch)
        pnlViewTickets.Dock = DockStyle.Fill
        pnlViewTickets.Location = New Point(23, 27)
        pnlViewTickets.Margin = New Padding(3, 4, 3, 4)
        pnlViewTickets.Name = "pnlViewTickets"
        pnlViewTickets.Size = New Size(1143, 826)
        pnlViewTickets.TabIndex = 1
        pnlViewTickets.Visible = False
        ' 
        ' lblStats
        ' 
        lblStats.Dock = DockStyle.Bottom
        lblStats.Location = New Point(0, 786)
        lblStats.Name = "lblStats"
        lblStats.Size = New Size(1143, 40)
        lblStats.TabIndex = 8
        lblStats.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' flpHistory
        ' 
        flpHistory.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        flpHistory.AutoScroll = True
        flpHistory.FlowDirection = FlowDirection.TopDown
        flpHistory.Location = New Point(0, 67)
        flpHistory.Margin = New Padding(3, 4, 3, 4)
        flpHistory.Name = "flpHistory"
        flpHistory.Size = New Size(1143, 719)
        flpHistory.TabIndex = 7
        flpHistory.Visible = False
        flpHistory.WrapContents = False
        ' 
        ' dgvTickets
        ' 
        dgvTickets.AllowUserToAddRows = False
        dgvTickets.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        dgvTickets.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvTickets.BackgroundColor = Color.White
        dgvTickets.BorderStyle = BorderStyle.None
        dgvTickets.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvTickets.Location = New Point(0, 67)
        dgvTickets.Margin = New Padding(3, 4, 3, 4)
        dgvTickets.Name = "dgvTickets"
        dgvTickets.ReadOnly = True
        dgvTickets.RowHeadersWidth = 51
        dgvTickets.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvTickets.Size = New Size(1143, 719)
        dgvTickets.TabIndex = 6
        ' 
        ' btnDelete
        ' 
        btnDelete.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnDelete.BackColor = Color.IndianRed
        btnDelete.FlatStyle = FlatStyle.Flat
        btnDelete.ForeColor = Color.White
        btnDelete.Location = New Point(994, 8)
        btnDelete.Margin = New Padding(3, 4, 3, 4)
        btnDelete.Name = "btnDelete"
        btnDelete.Size = New Size(117, 52)
        btnDelete.TabIndex = 5
        btnDelete.Text = "🗑️ Delete"
        btnDelete.UseVisualStyleBackColor = False
        ' 
        ' btnAction
        ' 
        btnAction.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnAction.BackColor = Color.SeaGreen
        btnAction.FlatStyle = FlatStyle.Flat
        btnAction.ForeColor = Color.White
        btnAction.Location = New Point(760, 9)
        btnAction.Margin = New Padding(3, 4, 3, 4)
        btnAction.Name = "btnAction"
        btnAction.Size = New Size(228, 52)
        btnAction.TabIndex = 4
        btnAction.Text = "Action"
        btnAction.UseVisualStyleBackColor = False
        ' 
        ' btnExport
        ' 
        btnExport.Location = New Point(577, 9)
        btnExport.Margin = New Padding(3, 4, 3, 4)
        btnExport.Name = "btnExport"
        btnExport.Size = New Size(137, 53)
        btnExport.TabIndex = 3
        btnExport.Text = "📤 Export CSV"
        btnExport.UseVisualStyleBackColor = True
        ' 
        ' btnRefresh
        ' 
        btnRefresh.Location = New Point(457, 7)
        btnRefresh.Margin = New Padding(3, 4, 3, 4)
        btnRefresh.Name = "btnRefresh"
        btnRefresh.Size = New Size(114, 53)
        btnRefresh.TabIndex = 2
        btnRefresh.Text = "🔄 Refresh"
        btnRefresh.UseVisualStyleBackColor = True
        ' 
        ' cmbStatusFilter
        ' 
        cmbStatusFilter.DropDownStyle = ComboBoxStyle.DropDownList
        cmbStatusFilter.FormattingEnabled = True
        cmbStatusFilter.Items.AddRange(New Object() {"All", "Pending", "In Progress", "Resolved"})
        cmbStatusFilter.Location = New Point(297, 20)
        cmbStatusFilter.Margin = New Padding(3, 4, 3, 4)
        cmbStatusFilter.Name = "cmbStatusFilter"
        cmbStatusFilter.Size = New Size(148, 28)
        cmbStatusFilter.TabIndex = 1
        ' 
        ' txtSearch
        ' 
        txtSearch.Location = New Point(7, 20)
        txtSearch.Margin = New Padding(3, 4, 3, 4)
        txtSearch.Name = "txtSearch"
        txtSearch.PlaceholderText = "Search tickets..."
        txtSearch.Size = New Size(285, 27)
        txtSearch.TabIndex = 0
        ' 
        ' pnlViewDashboard
        ' 
        pnlViewDashboard.Controls.Add(picGraph)
        pnlViewDashboard.Controls.Add(pnlTileResolved)
        pnlViewDashboard.Controls.Add(pnlTilePending)
        pnlViewDashboard.Controls.Add(pnlTileTotal)
        pnlViewDashboard.Dock = DockStyle.Fill
        pnlViewDashboard.Location = New Point(23, 27)
        pnlViewDashboard.Margin = New Padding(3, 4, 3, 4)
        pnlViewDashboard.Name = "pnlViewDashboard"
        pnlViewDashboard.Size = New Size(1143, 826)
        pnlViewDashboard.TabIndex = 0
        ' 
        ' picGraph
        ' 
        picGraph.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        picGraph.BackColor = Color.White
        picGraph.Location = New Point(0, 213)
        picGraph.Margin = New Padding(3, 4, 3, 4)
        picGraph.Name = "picGraph"
        picGraph.Size = New Size(1143, 599)
        picGraph.TabIndex = 3
        picGraph.TabStop = False
        ' 
        ' pnlTileResolved
        ' 
        pnlTileResolved.BackColor = Color.White
        pnlTileResolved.BorderStyle = BorderStyle.FixedSingle
        pnlTileResolved.Controls.Add(lblTileResolvedVal)
        pnlTileResolved.Controls.Add(Label6)
        pnlTileResolved.Controls.Add(Panel6)
        pnlTileResolved.Location = New Point(594, 13)
        pnlTileResolved.Margin = New Padding(3, 4, 3, 4)
        pnlTileResolved.Name = "pnlTileResolved"
        pnlTileResolved.Size = New Size(274, 159)
        pnlTileResolved.TabIndex = 2
        ' 
        ' lblTileResolvedVal
        ' 
        lblTileResolvedVal.AutoSize = True
        lblTileResolvedVal.Font = New Font("Segoe UI", 32.0F, FontStyle.Bold)
        lblTileResolvedVal.ForeColor = Color.FromArgb(CByte(64), CByte(64), CByte(64))
        lblTileResolvedVal.Location = New Point(17, 60)
        lblTileResolvedVal.Name = "lblTileResolvedVal"
        lblTileResolvedVal.Size = New Size(61, 72)
        lblTileResolvedVal.TabIndex = 2
        lblTileResolvedVal.Text = "0"
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
        Label6.ForeColor = Color.Gray
        Label6.Location = New Point(23, 27)
        Label6.Name = "Label6"
        Label6.Size = New Size(82, 20)
        Label6.TabIndex = 1
        Label6.Text = "RESOLVED"
        ' 
        ' Panel6
        ' 
        Panel6.BackColor = Color.SeaGreen
        Panel6.Dock = DockStyle.Left
        Panel6.Location = New Point(0, 0)
        Panel6.Margin = New Padding(3, 4, 3, 4)
        Panel6.Name = "Panel6"
        Panel6.Size = New Size(7, 157)
        Panel6.TabIndex = 0
        ' 
        ' pnlTilePending
        ' 
        pnlTilePending.BackColor = Color.White
        pnlTilePending.BorderStyle = BorderStyle.FixedSingle
        pnlTilePending.Controls.Add(lblTilePendingVal)
        pnlTilePending.Controls.Add(Label4)
        pnlTilePending.Controls.Add(Panel4)
        pnlTilePending.Location = New Point(297, 13)
        pnlTilePending.Margin = New Padding(3, 4, 3, 4)
        pnlTilePending.Name = "pnlTilePending"
        pnlTilePending.Size = New Size(274, 159)
        pnlTilePending.TabIndex = 1
        ' 
        ' lblTilePendingVal
        ' 
        lblTilePendingVal.AutoSize = True
        lblTilePendingVal.Font = New Font("Segoe UI", 32.0F, FontStyle.Bold)
        lblTilePendingVal.ForeColor = Color.FromArgb(CByte(64), CByte(64), CByte(64))
        lblTilePendingVal.Location = New Point(17, 60)
        lblTilePendingVal.Name = "lblTilePendingVal"
        lblTilePendingVal.Size = New Size(61, 72)
        lblTilePendingVal.TabIndex = 2
        lblTilePendingVal.Text = "0"
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
        Label4.ForeColor = Color.Gray
        Label4.Location = New Point(23, 27)
        Label4.Name = "Label4"
        Label4.Size = New Size(77, 20)
        Label4.TabIndex = 1
        Label4.Text = "PENDING"
        ' 
        ' Panel4
        ' 
        Panel4.BackColor = Color.Orange
        Panel4.Dock = DockStyle.Left
        Panel4.Location = New Point(0, 0)
        Panel4.Margin = New Padding(3, 4, 3, 4)
        Panel4.Name = "Panel4"
        Panel4.Size = New Size(7, 157)
        Panel4.TabIndex = 0
        ' 
        ' pnlTileTotal
        ' 
        pnlTileTotal.BackColor = Color.White
        pnlTileTotal.BorderStyle = BorderStyle.FixedSingle
        pnlTileTotal.Controls.Add(lblTileTotalVal)
        pnlTileTotal.Controls.Add(Label2)
        pnlTileTotal.Controls.Add(Panel2)
        pnlTileTotal.Location = New Point(0, 13)
        pnlTileTotal.Margin = New Padding(3, 4, 3, 4)
        pnlTileTotal.Name = "pnlTileTotal"
        pnlTileTotal.Size = New Size(274, 159)
        pnlTileTotal.TabIndex = 0
        ' 
        ' lblTileTotalVal
        ' 
        lblTileTotalVal.AutoSize = True
        lblTileTotalVal.Font = New Font("Segoe UI", 32.0F, FontStyle.Bold)
        lblTileTotalVal.ForeColor = Color.FromArgb(CByte(64), CByte(64), CByte(64))
        lblTileTotalVal.Location = New Point(17, 60)
        lblTileTotalVal.Name = "lblTileTotalVal"
        lblTileTotalVal.Size = New Size(61, 72)
        lblTileTotalVal.TabIndex = 2
        lblTileTotalVal.Text = "0"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
        Label2.ForeColor = Color.Gray
        Label2.Location = New Point(23, 27)
        Label2.Name = "Label2"
        Label2.Size = New Size(116, 20)
        Label2.TabIndex = 1
        Label2.Text = "TOTAL TICKETS"
        ' 
        ' Panel2
        ' 
        Panel2.BackColor = Color.SlateGray
        Panel2.Dock = DockStyle.Left
        Panel2.Location = New Point(0, 0)
        Panel2.Margin = New Padding(3, 4, 3, 4)
        Panel2.Name = "Panel2"
        Panel2.Size = New Size(7, 157)
        Panel2.TabIndex = 0
        ' 
        ' DashboardForm
        ' 
        AutoScaleDimensions = New SizeF(8.0F, 20.0F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.FromArgb(CByte(248), CByte(249), CByte(252))
        ClientSize = New Size(1463, 960)
        Controls.Add(pnlContent)
        Controls.Add(pnlHeader)
        Controls.Add(pnlSidebar)
        Margin = New Padding(3, 4, 3, 4)
        Name = "DashboardForm"
        StartPosition = FormStartPosition.CenterScreen
        Text = "IT Help Desk Professional"
        pnlHeader.ResumeLayout(False)
        pnlHeader.PerformLayout()
        pnlSidebar.ResumeLayout(False)
        pnlContent.ResumeLayout(False)
        pnlViewTickets.ResumeLayout(False)
        pnlViewTickets.PerformLayout()
        CType(dgvTickets, ComponentModel.ISupportInitialize).EndInit()
        pnlViewDashboard.ResumeLayout(False)
        CType(picGraph, ComponentModel.ISupportInitialize).EndInit()
        pnlTileResolved.ResumeLayout(False)
        pnlTileResolved.PerformLayout()
        pnlTilePending.ResumeLayout(False)
        pnlTilePending.PerformLayout()
        pnlTileTotal.ResumeLayout(False)
        pnlTileTotal.PerformLayout()
        ResumeLayout(False)

    End Sub

    Friend WithEvents pnlHeader As System.Windows.Forms.Panel
    Friend WithEvents pnlSidebar As System.Windows.Forms.Panel
    Friend WithEvents pnlContent As System.Windows.Forms.Panel
    Friend WithEvents btnNavLogout As System.Windows.Forms.Button
    Friend WithEvents btnNavUsers As System.Windows.Forms.Button
    Friend WithEvents btnNavTickets As System.Windows.Forms.Button
    Friend WithEvents btnNavDashboard As System.Windows.Forms.Button
    Friend WithEvents lblLogo As System.Windows.Forms.Label
    Friend WithEvents lblUser As System.Windows.Forms.Label
    Friend WithEvents pnlViewDashboard As System.Windows.Forms.Panel
    Friend WithEvents pnlTileTotal As System.Windows.Forms.Panel
    Friend WithEvents pnlTileResolved As System.Windows.Forms.Panel
    Friend WithEvents pnlTilePending As System.Windows.Forms.Panel
    Friend WithEvents picGraph As System.Windows.Forms.PictureBox
    Friend WithEvents lblTileTotalVal As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents lblTileResolvedVal As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Panel6 As System.Windows.Forms.Panel
    Friend WithEvents lblTilePendingVal As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents pnlViewTickets As System.Windows.Forms.Panel
    Friend WithEvents txtSearch As System.Windows.Forms.TextBox
    Friend WithEvents cmbStatusFilter As System.Windows.Forms.ComboBox
    Friend WithEvents btnRefresh As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnAction As System.Windows.Forms.Button
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents dgvTickets As System.Windows.Forms.DataGridView
    Friend WithEvents flpHistory As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents lblStats As System.Windows.Forms.Label
End Class