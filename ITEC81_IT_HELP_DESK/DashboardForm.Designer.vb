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
        Me.pnlHeader = New System.Windows.Forms.Panel()
        Me.lblUser = New System.Windows.Forms.Label()
        Me.lblLogo = New System.Windows.Forms.Label()
        Me.pnlSidebar = New System.Windows.Forms.Panel()
        Me.btnNavLogout = New System.Windows.Forms.Button()
        Me.btnNavUsers = New System.Windows.Forms.Button()
        Me.btnNavArchive = New System.Windows.Forms.Button()
        Me.btnNavTickets = New System.Windows.Forms.Button()
        Me.btnNavDashboard = New System.Windows.Forms.Button()
        Me.pnlContent = New System.Windows.Forms.Panel()
        Me.pnlViewDashboard = New System.Windows.Forms.Panel()
        Me.picGraph = New System.Windows.Forms.PictureBox()
        Me.pnlTileResolved = New System.Windows.Forms.Panel()
        Me.lblTileResolvedVal = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Panel6 = New System.Windows.Forms.Panel()
        Me.pnlTilePending = New System.Windows.Forms.Panel()
        Me.lblTilePendingVal = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.pnlTileTotal = New System.Windows.Forms.Panel()
        Me.lblTileTotalVal = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.pnlViewTickets = New System.Windows.Forms.Panel()
        Me.lblStats = New System.Windows.Forms.Label()
        Me.flpHistory = New System.Windows.Forms.FlowLayoutPanel()
        Me.dgvTickets = New System.Windows.Forms.DataGridView()
        Me.btnArchive = New System.Windows.Forms.Button()
        Me.btnAction = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.btnRefresh = New System.Windows.Forms.Button()
        Me.cmbStatusFilter = New System.Windows.Forms.ComboBox()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.pnlHeader.SuspendLayout()
        Me.pnlSidebar.SuspendLayout()
        Me.pnlContent.SuspendLayout()
        Me.pnlViewDashboard.SuspendLayout()
        CType(Me.picGraph, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlTileResolved.SuspendLayout()
        Me.pnlTilePending.SuspendLayout()
        Me.pnlTileTotal.SuspendLayout()
        Me.pnlViewTickets.SuspendLayout()
        CType(Me.dgvTickets, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlHeader
        '
        Me.pnlHeader.BackColor = System.Drawing.Color.White
        Me.pnlHeader.Controls.Add(Me.lblUser)
        Me.pnlHeader.Controls.Add(Me.lblLogo)
        Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeader.Location = New System.Drawing.Point(240, 0)
        Me.pnlHeader.Name = "pnlHeader"
        Me.pnlHeader.Size = New System.Drawing.Size(1040, 60)
        Me.pnlHeader.TabIndex = 1
        '
        'lblUser
        '
        Me.lblUser.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblUser.AutoSize = True
        Me.lblUser.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.lblUser.ForeColor = System.Drawing.Color.DimGray
        Me.lblUser.Location = New System.Drawing.Point(850, 20)
        Me.lblUser.Name = "lblUser"
        Me.lblUser.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblUser.Size = New System.Drawing.Size(126, 19)
        Me.lblUser.TabIndex = 1
        Me.lblUser.Text = "Welcome, User (ID)"
        Me.lblUser.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblLogo
        '
        Me.lblLogo.AutoSize = True
        Me.lblLogo.Font = New System.Drawing.Font("Segoe UI", 16.0!, System.Drawing.FontStyle.Bold)
        Me.lblLogo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.lblLogo.Location = New System.Drawing.Point(20, 12)
        Me.lblLogo.Name = "lblLogo"
        Me.lblLogo.Size = New System.Drawing.Size(170, 30)
        Me.lblLogo.TabIndex = 0
        Me.lblLogo.Text = "HELPDESK PRO"
        '
        'pnlSidebar
        '
        Me.pnlSidebar.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(25, Byte), Integer), CType(CType(45, Byte), Integer))
        Me.pnlSidebar.Controls.Add(Me.btnNavLogout)
        Me.pnlSidebar.Controls.Add(Me.btnNavUsers)
        Me.pnlSidebar.Controls.Add(Me.btnNavArchive)
        Me.pnlSidebar.Controls.Add(Me.btnNavTickets)
        Me.pnlSidebar.Controls.Add(Me.btnNavDashboard)
        Me.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left
        Me.pnlSidebar.Location = New System.Drawing.Point(0, 0)
        Me.pnlSidebar.Name = "pnlSidebar"
        Me.pnlSidebar.Size = New System.Drawing.Size(240, 720)
        Me.pnlSidebar.TabIndex = 0
        '
        'btnNavLogout
        '
        Me.btnNavLogout.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnNavLogout.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.btnNavLogout.FlatAppearance.BorderSize = 0
        Me.btnNavLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNavLogout.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.btnNavLogout.ForeColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.btnNavLogout.Location = New System.Drawing.Point(0, 670)
        Me.btnNavLogout.Name = "btnNavLogout"
        Me.btnNavLogout.Padding = New System.Windows.Forms.Padding(20, 0, 0, 0)
        Me.btnNavLogout.Size = New System.Drawing.Size(240, 50)
        Me.btnNavLogout.TabIndex = 4
        Me.btnNavLogout.Text = "  🚪  Log Out"
        Me.btnNavLogout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnNavLogout.UseVisualStyleBackColor = True
        '
        'btnNavUsers
        '
        Me.btnNavUsers.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnNavUsers.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnNavUsers.FlatAppearance.BorderSize = 0
        Me.btnNavUsers.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNavUsers.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.btnNavUsers.ForeColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.btnNavUsers.Location = New System.Drawing.Point(0, 150)
        Me.btnNavUsers.Name = "btnNavUsers"
        Me.btnNavUsers.Padding = New System.Windows.Forms.Padding(20, 0, 0, 0)
        Me.btnNavUsers.Size = New System.Drawing.Size(240, 50)
        Me.btnNavUsers.TabIndex = 3
        Me.btnNavUsers.Text = "  👥  Manage Users"
        Me.btnNavUsers.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnNavUsers.UseVisualStyleBackColor = True
        '
        'btnNavArchive
        '
        Me.btnNavArchive.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnNavArchive.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnNavArchive.FlatAppearance.BorderSize = 0
        Me.btnNavArchive.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNavArchive.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.btnNavArchive.ForeColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.btnNavArchive.Location = New System.Drawing.Point(0, 100)
        Me.btnNavArchive.Name = "btnNavArchive"
        Me.btnNavArchive.Padding = New System.Windows.Forms.Padding(20, 0, 0, 0)
        Me.btnNavArchive.Size = New System.Drawing.Size(240, 50)
        Me.btnNavArchive.TabIndex = 2
        Me.btnNavArchive.Text = "  📂  Archives"
        Me.btnNavArchive.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnNavArchive.UseVisualStyleBackColor = True
        '
        'btnNavTickets
        '
        Me.btnNavTickets.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnNavTickets.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnNavTickets.FlatAppearance.BorderSize = 0
        Me.btnNavTickets.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNavTickets.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.btnNavTickets.ForeColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.btnNavTickets.Location = New System.Drawing.Point(0, 50)
        Me.btnNavTickets.Name = "btnNavTickets"
        Me.btnNavTickets.Padding = New System.Windows.Forms.Padding(20, 0, 0, 0)
        Me.btnNavTickets.Size = New System.Drawing.Size(240, 50)
        Me.btnNavTickets.TabIndex = 1
        Me.btnNavTickets.Text = "  🎫  Ticket List"
        Me.btnNavTickets.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnNavTickets.UseVisualStyleBackColor = True
        '
        'btnNavDashboard
        '
        Me.btnNavDashboard.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnNavDashboard.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnNavDashboard.FlatAppearance.BorderSize = 0
        Me.btnNavDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNavDashboard.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.btnNavDashboard.ForeColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.btnNavDashboard.Location = New System.Drawing.Point(0, 0)
        Me.btnNavDashboard.Name = "btnNavDashboard"
        Me.btnNavDashboard.Padding = New System.Windows.Forms.Padding(20, 0, 0, 0)
        Me.btnNavDashboard.Size = New System.Drawing.Size(240, 50)
        Me.btnNavDashboard.TabIndex = 0
        Me.btnNavDashboard.Text = "  📊  Dashboard"
        Me.btnNavDashboard.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnNavDashboard.UseVisualStyleBackColor = True
        '
        'pnlContent
        '
        Me.pnlContent.Controls.Add(Me.pnlViewTickets)
        Me.pnlContent.Controls.Add(Me.pnlViewDashboard)
        Me.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlContent.Location = New System.Drawing.Point(240, 60)
        Me.pnlContent.Name = "pnlContent"
        Me.pnlContent.Padding = New System.Windows.Forms.Padding(20)
        Me.pnlContent.Size = New System.Drawing.Size(1040, 660)
        Me.pnlContent.TabIndex = 2
        '
        'pnlViewDashboard
        '
        Me.pnlViewDashboard.Controls.Add(Me.picGraph)
        Me.pnlViewDashboard.Controls.Add(Me.pnlTileResolved)
        Me.pnlViewDashboard.Controls.Add(Me.pnlTilePending)
        Me.pnlViewDashboard.Controls.Add(Me.pnlTileTotal)
        Me.pnlViewDashboard.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlViewDashboard.Location = New System.Drawing.Point(20, 20)
        Me.pnlViewDashboard.Name = "pnlViewDashboard"
        Me.pnlViewDashboard.Size = New System.Drawing.Size(1000, 620)
        Me.pnlViewDashboard.TabIndex = 0
        '
        'picGraph
        '
        Me.picGraph.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.picGraph.BackColor = System.Drawing.Color.White
        Me.picGraph.Location = New System.Drawing.Point(0, 160)
        Me.picGraph.Name = "picGraph"
        Me.picGraph.Size = New System.Drawing.Size(1000, 450)
        Me.picGraph.TabIndex = 3
        Me.picGraph.TabStop = False
        '
        'pnlTileResolved
        '
        Me.pnlTileResolved.BackColor = System.Drawing.Color.White
        Me.pnlTileResolved.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlTileResolved.Controls.Add(Me.lblTileResolvedVal)
        Me.pnlTileResolved.Controls.Add(Me.Label6)
        Me.pnlTileResolved.Controls.Add(Me.Panel6)
        Me.pnlTileResolved.Location = New System.Drawing.Point(520, 10)
        Me.pnlTileResolved.Name = "pnlTileResolved"
        Me.pnlTileResolved.Size = New System.Drawing.Size(240, 120)
        Me.pnlTileResolved.TabIndex = 2
        '
        'lblTileResolvedVal
        '
        Me.lblTileResolvedVal.AutoSize = True
        Me.lblTileResolvedVal.Font = New System.Drawing.Font("Segoe UI", 32.0!, System.Drawing.FontStyle.Bold)
        Me.lblTileResolvedVal.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblTileResolvedVal.Location = New System.Drawing.Point(15, 45)
        Me.lblTileResolvedVal.Name = "lblTileResolvedVal"
        Me.lblTileResolvedVal.Size = New System.Drawing.Size(50, 59)
        Me.lblTileResolvedVal.TabIndex = 2
        Me.lblTileResolvedVal.Text = "0"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label6.ForeColor = System.Drawing.Color.Gray
        Me.Label6.Location = New System.Drawing.Point(20, 20)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(65, 15)
        Me.Label6.TabIndex = 1
        Me.Label6.Text = "RESOLVED"
        '
        'Panel6
        '
        Me.Panel6.BackColor = System.Drawing.Color.SeaGreen
        Me.Panel6.Dock = System.Windows.Forms.DockStyle.Left
        Me.Panel6.Location = New System.Drawing.Point(0, 0)
        Me.Panel6.Name = "Panel6"
        Me.Panel6.Size = New System.Drawing.Size(6, 118)
        Me.Panel6.TabIndex = 0
        '
        'pnlTilePending
        '
        Me.pnlTilePending.BackColor = System.Drawing.Color.White
        Me.pnlTilePending.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlTilePending.Controls.Add(Me.lblTilePendingVal)
        Me.pnlTilePending.Controls.Add(Me.Label4)
        Me.pnlTilePending.Controls.Add(Me.Panel4)
        Me.pnlTilePending.Location = New System.Drawing.Point(260, 10)
        Me.pnlTilePending.Name = "pnlTilePending"
        Me.pnlTilePending.Size = New System.Drawing.Size(240, 120)
        Me.pnlTilePending.TabIndex = 1
        '
        'lblTilePendingVal
        '
        Me.lblTilePendingVal.AutoSize = True
        Me.lblTilePendingVal.Font = New System.Drawing.Font("Segoe UI", 32.0!, System.Drawing.FontStyle.Bold)
        Me.lblTilePendingVal.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblTilePendingVal.Location = New System.Drawing.Point(15, 45)
        Me.lblTilePendingVal.Name = "lblTilePendingVal"
        Me.lblTilePendingVal.Size = New System.Drawing.Size(50, 59)
        Me.lblTilePendingVal.TabIndex = 2
        Me.lblTilePendingVal.Text = "0"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label4.ForeColor = System.Drawing.Color.Gray
        Me.Label4.Location = New System.Drawing.Point(20, 20)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(60, 15)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "PENDING"
        '
        'Panel4
        '
        Me.Panel4.BackColor = System.Drawing.Color.Orange
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Left
        Me.Panel4.Location = New System.Drawing.Point(0, 0)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(6, 118)
        Me.Panel4.TabIndex = 0
        '
        'pnlTileTotal
        '
        Me.pnlTileTotal.BackColor = System.Drawing.Color.White
        Me.pnlTileTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlTileTotal.Controls.Add(Me.lblTileTotalVal)
        Me.pnlTileTotal.Controls.Add(Me.Label2)
        Me.pnlTileTotal.Controls.Add(Me.Panel2)
        Me.pnlTileTotal.Location = New System.Drawing.Point(0, 10)
        Me.pnlTileTotal.Name = "pnlTileTotal"
        Me.pnlTileTotal.Size = New System.Drawing.Size(240, 120)
        Me.pnlTileTotal.TabIndex = 0
        '
        'lblTileTotalVal
        '
        Me.lblTileTotalVal.AutoSize = True
        Me.lblTileTotalVal.Font = New System.Drawing.Font("Segoe UI", 32.0!, System.Drawing.FontStyle.Bold)
        Me.lblTileTotalVal.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblTileTotalVal.Location = New System.Drawing.Point(15, 45)
        Me.lblTileTotalVal.Name = "lblTileTotalVal"
        Me.lblTileTotalVal.Size = New System.Drawing.Size(50, 59)
        Me.lblTileTotalVal.TabIndex = 2
        Me.lblTileTotalVal.Text = "0"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label2.ForeColor = System.Drawing.Color.Gray
        Me.Label2.Location = New System.Drawing.Point(20, 20)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(92, 15)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "TOTAL TICKETS"
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.SlateGray
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Left
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(6, 118)
        Me.Panel2.TabIndex = 0
        '
        'pnlViewTickets
        '
        Me.pnlViewTickets.Controls.Add(Me.lblStats)
        Me.pnlViewTickets.Controls.Add(Me.flpHistory)
        Me.pnlViewTickets.Controls.Add(Me.dgvTickets)
        Me.pnlViewTickets.Controls.Add(Me.btnArchive)
        Me.pnlViewTickets.Controls.Add(Me.btnAction)
        Me.pnlViewTickets.Controls.Add(Me.btnExport)
        Me.pnlViewTickets.Controls.Add(Me.btnRefresh)
        Me.pnlViewTickets.Controls.Add(Me.cmbStatusFilter)
        Me.pnlViewTickets.Controls.Add(Me.txtSearch)
        Me.pnlViewTickets.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlViewTickets.Location = New System.Drawing.Point(20, 20)
        Me.pnlViewTickets.Name = "pnlViewTickets"
        Me.pnlViewTickets.Size = New System.Drawing.Size(1000, 620)
        Me.pnlViewTickets.TabIndex = 1
        Me.pnlViewTickets.Visible = False
        '
        'lblStats
        '
        Me.lblStats.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblStats.Location = New System.Drawing.Point(0, 590)
        Me.lblStats.Name = "lblStats"
        Me.lblStats.Size = New System.Drawing.Size(1000, 30)
        Me.lblStats.TabIndex = 8
        Me.lblStats.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'flpHistory
        '
        Me.flpHistory.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.flpHistory.AutoScroll = True
        Me.flpHistory.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.flpHistory.Location = New System.Drawing.Point(0, 50)
        Me.flpHistory.Name = "flpHistory"
        Me.flpHistory.Size = New System.Drawing.Size(1000, 540)
        Me.flpHistory.TabIndex = 7
        Me.flpHistory.Visible = False
        Me.flpHistory.WrapContents = False
        '
        'dgvTickets
        '
        Me.dgvTickets.AllowUserToAddRows = False
        Me.dgvTickets.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvTickets.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvTickets.BackgroundColor = System.Drawing.Color.White
        Me.dgvTickets.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvTickets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvTickets.Location = New System.Drawing.Point(0, 50)
        Me.dgvTickets.Name = "dgvTickets"
        Me.dgvTickets.ReadOnly = True
        Me.dgvTickets.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvTickets.Size = New System.Drawing.Size(1000, 540)
        Me.dgvTickets.TabIndex = 6
        '
        'btnArchive
        '
        Me.btnArchive.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnArchive.BackColor = System.Drawing.Color.IndianRed
        Me.btnArchive.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnArchive.ForeColor = System.Drawing.Color.White
        Me.btnArchive.Location = New System.Drawing.Point(720, 9)
        Me.btnArchive.Name = "btnArchive"
        Me.btnArchive.Size = New System.Drawing.Size(100, 40)
        Me.btnArchive.TabIndex = 5
        Me.btnArchive.Text = "📂 Archive"
        Me.btnArchive.UseVisualStyleBackColor = False
        '
        'btnAction
        '
        Me.btnAction.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAction.BackColor = System.Drawing.Color.SeaGreen
        Me.btnAction.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAction.ForeColor = System.Drawing.Color.White
        Me.btnAction.Location = New System.Drawing.Point(830, 9)
        Me.btnAction.Name = "btnAction"
        Me.btnAction.Size = New System.Drawing.Size(160, 40)
        Me.btnAction.TabIndex = 4
        Me.btnAction.Text = "Action"
        Me.btnAction.UseVisualStyleBackColor = False
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(510, 9)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(120, 40)
        Me.btnExport.TabIndex = 3
        Me.btnExport.Text = "📤 Export CSV"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnRefresh
        '
        Me.btnRefresh.Location = New System.Drawing.Point(400, 9)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(100, 40)
        Me.btnRefresh.TabIndex = 2
        Me.btnRefresh.Text = "🔄 Refresh"
        Me.btnRefresh.UseVisualStyleBackColor = True
        '
        'cmbStatusFilter
        '
        Me.cmbStatusFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbStatusFilter.FormattingEnabled = True
        Me.cmbStatusFilter.Items.AddRange(New Object() {"All", "Pending", "In Progress", "Resolved"})
        Me.cmbStatusFilter.Location = New System.Drawing.Point(260, 15)
        Me.cmbStatusFilter.Name = "cmbStatusFilter"
        Me.cmbStatusFilter.Size = New System.Drawing.Size(130, 23)
        Me.cmbStatusFilter.TabIndex = 1
        '
        'txtSearch
        '
        Me.txtSearch.Location = New System.Drawing.Point(0, 15)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.PlaceholderText = "Search tickets..."
        Me.txtSearch.Size = New System.Drawing.Size(250, 23)
        Me.txtSearch.TabIndex = 0
        '
        'DashboardForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(252, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1280, 720)
        Me.Controls.Add(Me.pnlContent)
        Me.Controls.Add(Me.pnlHeader)
        Me.Controls.Add(Me.pnlSidebar)
        Me.Name = "DashboardForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "IT Help Desk Professional"
        Me.pnlHeader.ResumeLayout(False)
        Me.pnlHeader.PerformLayout()
        Me.pnlSidebar.ResumeLayout(False)
        Me.pnlContent.ResumeLayout(False)
        Me.pnlViewDashboard.ResumeLayout(False)
        CType(Me.picGraph, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlTileResolved.ResumeLayout(False)
        Me.pnlTileResolved.PerformLayout()
        Me.pnlTilePending.ResumeLayout(False)
        Me.pnlTilePending.PerformLayout()
        Me.pnlTileTotal.ResumeLayout(False)
        Me.pnlTileTotal.PerformLayout()
        Me.pnlViewTickets.ResumeLayout(False)
        Me.pnlViewTickets.PerformLayout()
        CType(Me.dgvTickets, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pnlHeader As System.Windows.Forms.Panel
    Friend WithEvents pnlSidebar As System.Windows.Forms.Panel
    Friend WithEvents pnlContent As System.Windows.Forms.Panel
    Friend WithEvents btnNavLogout As System.Windows.Forms.Button
    Friend WithEvents btnNavUsers As System.Windows.Forms.Button
    Friend WithEvents btnNavArchive As System.Windows.Forms.Button
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
    Friend WithEvents btnArchive As System.Windows.Forms.Button
    Friend WithEvents dgvTickets As System.Windows.Forms.DataGridView
    Friend WithEvents flpHistory As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents lblStats As System.Windows.Forms.Label
End Class