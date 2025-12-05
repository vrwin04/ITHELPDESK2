Imports System.Data.OleDb
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms
Imports System.Text
Imports System

Public Class DashboardForm

    ' --- DATA ---
    Private dtTickets As New DataTable
    Private CategoryCounts As New Dictionary(Of String, Integer)

    ' --- ANIMATION ---
    Private WithEvents AnimationTimer As New Timer With {.Interval = 15}
    Private AnimationProgress As Single = 0.0F

    ' --- UI CONTROLS ---
    Private WithEvents pnlHeader As New Panel
    Private WithEvents pnlSidebar As New Panel
    Private WithEvents pnlContent As New Panel

    ' Navigation Buttons
    Private WithEvents btnNavDashboard As New Button
    Private WithEvents btnNavTickets As New Button
    Private WithEvents btnNavUsers As New Button
    Private WithEvents btnNavLogout As New Button

    ' Dashboard View
    Private pnlViewDashboard As New Panel
    Private WithEvents picGraph As New PictureBox
    ' Tile Panels
    Private pnlTileTotal As New Panel
    Private pnlTilePending As New Panel
    Private pnlTileResolved As New Panel
    ' Tile Labels
    Private lblTileTotalVal As New Label
    Private lblTilePendingVal As New Label
    Private lblTileResolvedVal As New Label

    ' Tickets View
    Private pnlViewTickets As New Panel
    Private WithEvents dgvTickets As New DataGridView
    Private WithEvents flpHistory As New FlowLayoutPanel
    Private WithEvents txtSearch As New TextBox
    Private WithEvents cmbStatusFilter As New ComboBox
    Private WithEvents btnRefresh As New Button
    Private WithEvents btnExport As New Button
    Private WithEvents btnAction As New Button
    Private WithEvents btnDelete As New Button
    Private WithEvents lblStats As New Label

    ' REMOVED: ConnString property is obsolete, now using Session.Db

    Private Sub DashboardForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Controls.Clear()
        Me.DoubleBuffered = True
        SetupLayout()

        ' Set up DataGridView style immediately for better look
        ApplyDataGridStyle()

        LoadTickets()

        If Session.CurrentUserRole = "Student" Then
            ApplyStudentLayout()
            ShowView("Tickets")
        Else
            ApplyAdminLayout()
            ShowView("Dashboard")
        End If
    End Sub

    ' --------------------------------------------------------------------------
    ' --- 1. LAYOUT SETUP (MAJOR AESTHETIC OVERHAUL) ---
    ' --------------------------------------------------------------------------
    Private Sub SetupLayout()
        Me.Size = New Size(1280, 720)
        Me.Text = "IT Help Desk Professional" ' Updated title
        Me.BackColor = Color.FromArgb(248, 249, 252) ' Lighter, softer background
        Me.StartPosition = FormStartPosition.CenterScreen

        ' -- SIDEBAR (Darker, more contrast) --
        pnlSidebar.Parent = Me
        pnlSidebar.Dock = DockStyle.Left
        pnlSidebar.Width = 240
        pnlSidebar.BackColor = Color.FromArgb(20, 25, 45) ' Dark Navy/Charcoal

        ' Nav Buttons (Using emojis/unicode for simple icons)
        CreateNavBtn(btnNavLogout, "Log Out", "🚪", DockStyle.Bottom)
        If Session.CurrentUserRole = "Manager" Then CreateNavBtn(btnNavUsers, "Manage Users", "👥", DockStyle.Top)
        Dim tListTitle As String = If(Session.CurrentUserRole = "Student", "My Concerns", "Ticket List")
        CreateNavBtn(btnNavTickets, tListTitle, "🎫", DockStyle.Top)
        CreateNavBtn(btnNavDashboard, "Dashboard", "📊", DockStyle.Top)

        ' -- HEADER (Clean White) --
        pnlHeader.Parent = Me
        pnlHeader.Dock = DockStyle.Top
        pnlHeader.Height = 60
        pnlHeader.BackColor = Color.White
        pnlHeader.BringToFront()

        ' --- BRANDING ---
        Dim lblLogo As New Label
        lblLogo.Text = "HELPDESK PRO"
        lblLogo.ForeColor = Color.FromArgb(60, 100, 200) ' Professional Blue
        lblLogo.Font = New Font("Segoe UI", 16, FontStyle.Bold)
        lblLogo.AutoSize = True
        lblLogo.Location = New Point(20, 12)
        lblLogo.Parent = pnlHeader
        lblLogo.BringToFront()

        ' --- WELCOME MESSAGE ---
        Dim lblUser As New Label
        lblUser.Text = "Welcome, " & Session.CurrentUserName & " (" & Session.CurrentUserRole & ")"
        lblUser.Font = New Font("Segoe UI", 10, FontStyle.Regular)
        lblUser.ForeColor = Color.DimGray
        lblUser.AutoSize = True
        lblUser.Location = New Point(pnlHeader.Width - 200, 20) ' Placeholder position
        lblUser.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        lblUser.Parent = pnlHeader

        ' Adjust welcome message position based on form size
        AddHandler Me.SizeChanged, Sub(s, ev)
                                       lblUser.Location = New Point(pnlHeader.Width - lblUser.Width - 20, 20)
                                   End Sub

        ' -- CONTENT AREA --
        pnlContent.Parent = Me
        pnlContent.Dock = DockStyle.Fill
        pnlContent.Padding = New Padding(20) ' Reduced padding for more space
        pnlContent.BringToFront()

        ' ==========================
        '      DASHBOARD VIEW
        ' ==========================
        pnlViewDashboard.Parent = pnlContent
        pnlViewDashboard.Dock = DockStyle.Fill

        ' 1. Summary Tiles 
        CreateModernTile(pnlTileTotal, lblTileTotalVal, "Total Tickets", Color.SlateGray, 0)
        CreateModernTile(pnlTilePending, lblTilePendingVal, "Pending", Color.Orange, 260)
        CreateModernTile(pnlTileResolved, lblTileResolvedVal, "Resolved", Color.SeaGreen, 520)

        ' 2. Graph 
        picGraph.Parent = pnlViewDashboard
        picGraph.Location = New Point(0, 160)
        picGraph.Size = New Size(pnlContent.Width - 40, 450)
        picGraph.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Bottom
        picGraph.BackColor = Color.White
        picGraph.BorderStyle = BorderStyle.None

        ' ==========================
        '      TICKETS VIEW
        ' ==========================
        pnlViewTickets.Parent = pnlContent
        pnlViewTickets.Dock = DockStyle.Fill
        pnlViewTickets.Visible = False

        ' Search Bar
        txtSearch.Parent = pnlViewTickets
        txtSearch.Location = New Point(0, 10)
        txtSearch.Size = New Size(250, 30)
        txtSearch.PlaceholderText = "Search tickets by subject or category..."

        ' Filter Dropdown
        cmbStatusFilter.Parent = pnlViewTickets
        cmbStatusFilter.Location = New Point(260, 10)
        cmbStatusFilter.DropDownStyle = ComboBoxStyle.DropDownList
        cmbStatusFilter.Items.AddRange(New String() {"All", "Pending", "In Progress", "Resolved"})
        cmbStatusFilter.SelectedIndex = 0

        ' Refresh Button
        btnRefresh.Parent = pnlViewTickets
        btnRefresh.Text = "🔄 Refresh"
        btnRefresh.Location = New Point(400, 9)
        btnRefresh.Size = New Size(100, 28)

        ' Export Button
        btnExport.Parent = pnlViewTickets
        btnExport.Text = "📤 Export CSV"
        btnExport.Location = New Point(510, 9)
        btnExport.Size = New Size(120, 28)

        ' Action Buttons (Right Aligned)
        btnAction.Parent = pnlViewTickets
        btnAction.Size = New Size(160, 30)
        btnAction.FlatStyle = FlatStyle.Flat
        btnAction.Location = New Point(pnlViewTickets.Width - 170, 9)
        btnAction.Anchor = AnchorStyles.Top Or AnchorStyles.Right

        btnDelete.Parent = pnlViewTickets
        btnDelete.Text = "🗑️ Delete"
        btnDelete.BackColor = Color.IndianRed
        btnDelete.ForeColor = Color.White
        btnDelete.FlatStyle = FlatStyle.Flat
        btnDelete.Size = New Size(100, 30)
        btnDelete.Location = New Point(pnlViewTickets.Width - 280, 9)
        btnDelete.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnDelete.Visible = False

        ' Admin Grid 
        dgvTickets.Parent = pnlViewTickets
        dgvTickets.Location = New Point(0, 50)
        dgvTickets.Size = New Size(pnlViewTickets.Width, pnlViewTickets.Height - 80)
        dgvTickets.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        dgvTickets.BackgroundColor = Color.White
        dgvTickets.BorderStyle = BorderStyle.None
        dgvTickets.AllowUserToAddRows = False
        dgvTickets.ReadOnly = True
        dgvTickets.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvTickets.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        ' Student List
        flpHistory.Parent = pnlViewTickets
        flpHistory.Location = New Point(0, 50)
        flpHistory.Size = New Size(pnlViewTickets.Width, pnlViewTickets.Height - 80)
        flpHistory.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        flpHistory.AutoScroll = True
        flpHistory.FlowDirection = FlowDirection.TopDown
        flpHistory.WrapContents = False
        flpHistory.Visible = False

        lblStats.Parent = pnlViewTickets
        lblStats.Dock = DockStyle.Bottom
        lblStats.Height = 30
        lblStats.TextAlign = ContentAlignment.MiddleLeft
    End Sub

    ' --- HELPER: NEW STYLED NAVIGATION BUTTON ---
    Private Sub CreateNavBtn(btn As Button, text As String, icon As String, dock As DockStyle)
        btn.Parent = pnlSidebar
        btn.Text = "  " & icon & "  " & text ' Include the icon
        btn.Dock = dock
        btn.Height = 50
        btn.FlatStyle = FlatStyle.Flat
        btn.FlatAppearance.BorderSize = 0
        btn.ForeColor = Color.FromArgb(200, 200, 200)
        btn.Font = New Font("Segoe UI", 10, FontStyle.Regular)
        btn.TextAlign = ContentAlignment.MiddleLeft
        btn.Padding = New Padding(20, 0, 0, 0)
        btn.Cursor = Cursors.Hand
        AddHandler btn.MouseEnter, Sub() btn.BackColor = Color.FromArgb(50, 55, 75) ' Subtle hover
        AddHandler btn.MouseLeave, Sub() btn.BackColor = Color.Transparent
    End Sub

    ' --- HELPER: PROFESSIONAL DATA GRID STYLE ---
    Private Sub ApplyDataGridStyle()
        With dgvTickets
            .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
            .EnableHeadersVisualStyles = False
            .ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(235, 235, 235) ' Light Gray Header
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(64, 64, 64)
            .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9, FontStyle.Bold)
            .RowHeadersVisible = False ' Hide ugly default row headers
            .BorderStyle = BorderStyle.FixedSingle ' Add subtle border
            .GridColor = Color.FromArgb(240, 240, 240) ' Soft grid lines
            .DefaultCellStyle.Padding = New Padding(5)
        End With
    End Sub

    ' --- NEW TILE DESIGN (Added Box Shadow effect) ---
    Private Sub CreateModernTile(pnl As Panel, lblVal As Label, title As String, accentColor As Color, x As Integer)
        pnl.Parent = pnlViewDashboard
        pnl.Location = New Point(x, 10)
        pnl.Size = New Size(240, 120)
        pnl.BackColor = Color.White
        ' NEW: Add a subtle border/shadow effect
        pnl.BorderStyle = BorderStyle.FixedSingle
        pnl.Tag = "Tile" ' Tag for easy identification

        ' Color Strip on Left
        Dim strip As New Panel
        strip.Width = 6
        strip.Dock = DockStyle.Left
        strip.BackColor = accentColor
        strip.Parent = pnl

        ' Title Label
        Dim lblTitle As New Label
        lblTitle.Text = title.ToUpper()
        lblTitle.ForeColor = Color.Gray
        lblTitle.Font = New Font("Segoe UI", 9, FontStyle.Bold)
        lblTitle.Location = New Point(20, 20)
        lblTitle.AutoSize = True
        lblTitle.Parent = pnl

        ' Value Label
        lblVal.Text = "0"
        lblVal.ForeColor = Color.FromArgb(64, 64, 64)
        lblVal.Font = New Font("Segoe UI", 32, FontStyle.Bold)
        lblVal.Location = New Point(15, 45)
        lblVal.AutoSize = True
        lblVal.Parent = pnl
    End Sub

    ' --- VIEW LOGIC ---
    Private Sub ShowView(view As String)
        pnlViewDashboard.Visible = (view = "Dashboard")
        pnlViewTickets.Visible = (view = "Tickets")
    End Sub

    Private Sub btnNavDashboard_Click(sender As Object, e As EventArgs) Handles btnNavDashboard.Click
        ShowView("Dashboard")
        LoadTickets()
    End Sub

    Private Sub btnNavTickets_Click(sender As Object, e As EventArgs) Handles btnNavTickets.Click
        ShowView("Tickets")
        LoadTickets()
    End Sub

    Private Sub btnNavUsers_Click(sender As Object, e As EventArgs) Handles btnNavUsers.Click
        Dim frm As New UserManagementForm
        frm.ShowDialog()
    End Sub

    Private Sub btnNavLogout_Click(sender As Object, e As EventArgs) Handles btnNavLogout.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    ' --------------------------------------------------------------------------
    ' --- DATA LOGIC (REFACTORED TO USE Session.Db) ---
    ' --------------------------------------------------------------------------
    Private Sub LoadTickets()
        Dim sql As String
        Dim params As New List(Of Object)

        If Session.CurrentUserRole = "Student" Then
            sql = "SELECT [TicketID], [Category], [Priority], [IssueSubject], [Status], [AdminRemarks], [DateSubmitted] FROM tblTickets WHERE [SubmittedBy] = ? ORDER BY [DateSubmitted] DESC"
            params.Add(Session.CurrentUserID)
        Else
            sql = "SELECT t.TicketID, u.Username AS [ReportedBy], t.Category, t.Priority, t.IssueSubject, t.Status, t.AdminRemarks, t.DateSubmitted FROM tblTickets t INNER JOIN tblUsers u ON t.SubmittedBy = u.UserID ORDER BY t.DateSubmitted DESC"
        End If

        dtTickets = Session.Db.GetTable(sql, params)

        If dtTickets IsNot Nothing Then
            If Session.CurrentUserRole = "Student" Then
                RenderStudentCards()
            Else
                FilterData()
                LoadAnalyticsData()
            End If
        End If
    End Sub

    ' --- IMPROVED STUDENT CARD RENDERING (CLEANER DESIGN) ---
    Private Sub RenderStudentCards()
        flpHistory.Controls.Clear()
        flpHistory.SuspendLayout()

        If dtTickets Is Nothing OrElse dtTickets.Rows.Count = 0 Then
            Dim lbl As New Label With {.Text = "No concerns reported yet.", .AutoSize = True, .Font = New Font("Segoe UI", 12), .ForeColor = Color.Gray, .Margin = New Padding(20)}
            flpHistory.Controls.Add(lbl)
            flpHistory.ResumeLayout()
            Return
        End If

        For Each row As DataRow In dtTickets.Rows
            Dim pnl As New Panel
            pnl.Width = flpHistory.Width - 40
            pnl.Height = 110 ' Reduced height for tighter look
            pnl.BackColor = Color.White
            pnl.Margin = New Padding(0, 0, 0, 10) ' Reduced margin
            pnl.Padding = New Padding(15)
            pnl.BorderStyle = BorderStyle.FixedSingle ' Subtle border

            Dim status As String = row("Status").ToString()
            Dim statusColor As Color = If(status = "Resolved", Color.SeaGreen, If(status = "In Progress", Color.DodgerBlue, Color.Orange))

            ' Status Bar (Left)
            Dim pnlStatus As New Panel
            pnlStatus.Width = 5
            pnlStatus.Dock = DockStyle.Left
            pnlStatus.BackColor = statusColor
            pnl.Controls.Add(pnlStatus)

            ' Header (Date | Category | Priority)
            Dim lblHead As New Label
            lblHead.Text = Convert.ToDateTime(row("DateSubmitted")).ToString("MMM dd, yyyy") & "  •  " & row("Category").ToString() & "  •  " & row("Priority").ToString()
            lblHead.Font = New Font("Segoe UI", 9, FontStyle.Bold)
            lblHead.ForeColor = Color.Gray
            lblHead.Location = New Point(20, 10)
            lblHead.AutoSize = True
            pnl.Controls.Add(lblHead)

            ' Body (Subject)
            Dim lblBody As New Label
            lblBody.Text = row("IssueSubject").ToString()
            lblBody.Font = New Font("Segoe UI", 12, FontStyle.Bold) ' Bolder subject
            lblBody.Location = New Point(20, 35)
            lblBody.Size = New Size(pnl.Width - 40, 25)
            lblBody.AutoEllipsis = True
            pnl.Controls.Add(lblBody)

            ' Footer (Status)
            Dim lblFoot As New Label
            lblFoot.Location = New Point(20, 70)
            lblFoot.AutoSize = True
            lblFoot.Font = New Font("Segoe UI", 9, FontStyle.Bold)
            lblFoot.ForeColor = statusColor

            If status = "Resolved" Then
                lblFoot.Text = "Status: RESOLVED. (Admin Comments Available)"
            Else
                lblFoot.Text = "Status: " & status.ToUpper() & " (Awaiting action)"
            End If
            pnl.Controls.Add(lblFoot)

            flpHistory.Controls.Add(pnl)
        Next
        flpHistory.ResumeLayout()
    End Sub

    Private Sub FilterData()
        If dtTickets Is Nothing Then Return
        Dim view As DataView = dtTickets.DefaultView
        Dim filter As String = ""

        If Not String.IsNullOrEmpty(txtSearch.Text) AndAlso dtTickets.Columns.Contains("IssueSubject") Then
            filter = String.Format("(IssueSubject LIKE '%{0}%' OR Category LIKE '%{0}%')", txtSearch.Text.Replace("'", "''"))
        End If

        If cmbStatusFilter.SelectedIndex > 0 AndAlso dtTickets.Columns.Contains("Status") Then
            If filter.Length > 0 Then filter &= " AND "
            filter &= String.Format("Status = '{0}'", cmbStatusFilter.SelectedItem.ToString())
        End If

        Try
            view.RowFilter = filter
            dgvTickets.DataSource = view
            UpdateStats(view)
        Catch
            view.RowFilter = ""
        End Try
    End Sub

    Private Sub UpdateStats(view As DataView)
        Dim total As Integer = view.Count
        Dim pending As Integer = 0
        Dim resolved As Integer = 0

        For Each rowView As DataRowView In view
            Dim st = rowView("Status").ToString()
            If st = "Pending" Then pending += 1
            If st = "Resolved" Then resolved += 1
        Next

        lblTileTotalVal.Text = total.ToString()
        lblTilePendingVal.Text = pending.ToString()
        lblTileResolvedVal.Text = resolved.ToString()

        ' Trigger Animation
        AnimationProgress = 0
        AnimationTimer.Start()
    End Sub

    Private Sub LoadAnalyticsData()
        CategoryCounts.Clear()
        ' Only add categories that actually exist in the rows (Count > 0)
        For Each row As DataRow In dtTickets.Rows
            Dim cat = row("Category").ToString()
            If CategoryCounts.ContainsKey(cat) Then CategoryCounts(cat) += 1 Else CategoryCounts.Add(cat, 1)
        Next
    End Sub

    Private Sub AnimationTimer_Tick(sender As Object, e As EventArgs) Handles AnimationTimer.Tick
        AnimationProgress += 0.05F
        If AnimationProgress >= 1.0F Then
            AnimationProgress = 1.0F
            AnimationTimer.Stop()
        End If
        picGraph.Invalidate()
    End Sub

    ' --- REDESIGNED BAR CHART (IMPROVED AESTHETICS AND COLOR PALETTE) ---
    Private Sub picGraph_Paint(sender As Object, e As PaintEventArgs) Handles picGraph.Paint
        Dim g = e.Graphics
        g.SmoothingMode = SmoothingMode.AntiAlias
        g.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit

        ' 1. Clear Background (Ensure White)
        g.Clear(Color.White)

        If CategoryCounts.Count = 0 Then
            Dim msg = "No Ticket Data Available for Charting"
            Dim sf As New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
            g.DrawString(msg, New Font("Segoe UI", 12), Brushes.Gray, picGraph.ClientRectangle, sf)
            Return
        End If

        ' 2. Calculate Metrics
        Dim w As Integer = picGraph.Width
        Dim h As Integer = picGraph.Height

        ' INCREASED PADDING TO SHOW LABELS
        Dim paddingBottom As Integer = 60
        Dim paddingTop As Integer = 40
        Dim graphHeight As Integer = h - paddingBottom - paddingTop

        Dim maxVal As Integer = 0
        For Each v In CategoryCounts.Values
            If v > maxVal Then maxVal = v
        Next
        Dim topScale As Integer = If(maxVal = 0, 5, maxVal)

        ' 3. Draw Soft Grid Lines (Horizontal Only)
        Dim penGrid As New Pen(Color.FromArgb(240, 240, 240), 1)
        For i As Integer = 0 To 4
            Dim y As Integer = paddingTop + CInt((graphHeight / 4) * i)
            g.DrawLine(penGrid, 20, y, w - 20, y)
        Next

        ' 4. Draw Bars
        Dim count As Integer = CategoryCounts.Count
        Dim slotWidth As Integer = CInt((w - 40) / count)
        Dim barWidth As Integer = Math.Min(80, CInt(slotWidth * 0.6))
        Dim startX As Integer = 20
        Dim idx As Integer = 0

        ' NEW: Professional Color Palette
        Dim colors() As Color = {Color.FromArgb(52, 152, 219), Color.FromArgb(46, 204, 113), Color.FromArgb(243, 156, 18), Color.FromArgb(231, 76, 60)}
        Dim lightColors() As Color = {Color.FromArgb(100, 180, 255), Color.FromArgb(100, 255, 170), Color.FromArgb(255, 200, 100), Color.FromArgb(255, 120, 100)}


        For Each kvp In CategoryCounts
            Dim val As Integer = kvp.Value
            ' Animate Height (using AnimationProgress)
            Dim barH As Integer = CInt((val / topScale) * graphHeight * AnimationProgress)

            Dim x As Integer = startX + (idx * slotWidth) + (slotWidth - barWidth) \ 2
            Dim y As Integer = (h - paddingBottom) - barH

            Dim rect As New Rectangle(x, y, barWidth, barH)

            ' Gradient Fill
            If barH > 0 Then
                Using br As New LinearGradientBrush(rect, lightColors(idx Mod colors.Length), colors(idx Mod colors.Length), LinearGradientMode.Vertical)
                    g.FillRectangle(br, rect)
                End Using
            End If

            ' Value Label (Floating Top)
            Dim valStr = val.ToString()
            Dim fontVal As New Font("Segoe UI", 10, FontStyle.Bold)
            Dim szVal = g.MeasureString(valStr, fontVal)
            g.DrawString(valStr, fontVal, Brushes.Black, CSng(x + (barWidth - szVal.Width) / 2), CSng(y - 20))

            ' Category Label (Bottom)
            Dim catStr = kvp.Key
            If catStr.Length > 10 Then catStr = catStr.Substring(0, 8) & ".."
            Dim fontCat As New Font("Segoe UI", 9, FontStyle.Regular)
            Dim szCat = g.MeasureString(catStr, fontCat)

            ' Draw Label Darker and Higher
            g.DrawString(catStr, fontCat, Brushes.DimGray, CSng(x + (barWidth - szCat.Width) / 2), CSng(h - paddingBottom + 5))

            idx += 1
        Next
    End Sub

    ' --- ROLES ---
    Private Sub ApplyStudentLayout()
        btnNavDashboard.Visible = False
        btnNavUsers.Visible = False
        btnExport.Visible = False
        btnDelete.Visible = False
        dgvTickets.Visible = False
        flpHistory.Visible = True
        btnAction.Text = "➕ New Concern" ' New icon/text
        btnAction.BackColor = Color.DodgerBlue
        btnAction.ForeColor = Color.White
    End Sub

    Private Sub ApplyAdminLayout()
        btnNavDashboard.Visible = True
        btnExport.Visible = True
        btnDelete.Visible = True
        dgvTickets.Visible = True
        flpHistory.Visible = False
        btnAction.Text = "✅ Mark Resolved" ' New icon/text
        btnAction.BackColor = Color.SeaGreen
        btnAction.ForeColor = Color.White
    End Sub

    ' --------------------------------------------------------------------------
    ' --- ACTIONS (REFACTORED TO USE Session.Db) ---
    ' --------------------------------------------------------------------------
    Private Sub btnAction_Click(sender As Object, e As EventArgs) Handles btnAction.Click
        If Session.CurrentUserRole = "Student" Then
            Dim category As String = ""
            Dim priority As String = ""
            Dim issue As String = ""
            If ShowReportDialog(category, priority, issue) Then

                Dim sql As String = "INSERT INTO tblTickets (SubmittedBy, Category, Priority, IssueSubject, Status, DateSubmitted) VALUES (?, ?, ?, ?, ?, ?)"
                Dim params As New List(Of Object) From {Session.CurrentUserID, category, priority, issue, "Pending", DateTime.Now.ToString()}

                If Session.Db.RunSql(sql, params) Then
                    MessageBox.Show("Concern submitted successfully!")
                    LoadTickets()
                End If
            End If
        Else
            If dgvTickets.SelectedRows.Count = 0 Then Return
            Dim id = Convert.ToInt32(dgvTickets.SelectedRows(0).Cells("TicketID").Value)
            Dim remk = CustomInputBox("Enter resolution summary:", "Resolve Ticket")

            Dim sql As String = "UPDATE tblTickets SET Status='Resolved', AdminRemarks=? WHERE TicketID=?"
            Dim params As New List(Of Object) From {remk, id}

            If Session.Db.RunSql(sql, params) Then
                MessageBox.Show("Ticket marked as Resolved!")
                LoadTickets()
            End If
        End If
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If dgvTickets.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a ticket to delete.")
            Return
        End If

        If MessageBox.Show("Are you sure you want to permanently delete this ticket?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
            Dim id = Convert.ToInt32(dgvTickets.SelectedRows(0).Cells("TicketID").Value)

            Dim sql As String = "DELETE FROM tblTickets WHERE TicketID=?"
            If Session.Db.RunSql(sql, New List(Of Object) From {id}) Then
                LoadTickets()
                MessageBox.Show("Ticket deleted.")
            End If
        End If
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        LoadTickets()
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        FilterData()
    End Sub

    Private Sub cmbStatusFilter_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbStatusFilter.SelectedIndexChanged
        FilterData()
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If dtTickets Is Nothing Then Return
        Dim sfd As New SaveFileDialog With {.Filter = "CSV|*.csv", .FileName = "Report_" & DateTime.Now.ToString("yyyyMMdd") & ".csv"} ' Better filename
        If sfd.ShowDialog() = DialogResult.OK Then
            Dim sb As New StringBuilder
            For Each col As DataColumn In dtTickets.Columns
                sb.Append("""" & col.ColumnName.Replace("""", """""") & """,") ' CSV quoting headers
            Next
            sb.Remove(sb.Length - 1, 1) ' Remove trailing comma
            sb.AppendLine()

            For Each row As DataRow In dtTickets.Rows
                For Each item In row.ItemArray
                    sb.Append("""" & item.ToString().Replace("""", """""") & """,") ' CSV quoting values
                Next
                sb.Remove(sb.Length - 1, 1) ' Remove trailing comma
                sb.AppendLine()
            Next
            File.WriteAllText(sfd.FileName, sb.ToString())
            MessageBox.Show("Data successfully exported to: " & sfd.FileName, "Export Complete") ' Better message
        End If
    End Sub

    Private Function CustomInputBox(prompt As String, title As String) As String
        Dim f As New Form With {.Width = 400, .Height = 200, .Text = title, .StartPosition = FormStartPosition.CenterParent, .FormBorderStyle = FormBorderStyle.FixedDialog, .MaximizeBox = False}
        Dim lbl As New Label With {.Text = prompt, .Left = 20, .Top = 20, .Width = 340, .Parent = f, .Font = New Font("Segoe UI", 10)}
        Dim txt As New TextBox With {.Left = 20, .Top = 60, .Width = 340, .Parent = f, .Font = New Font("Segoe UI", 10)}
        Dim btn As New Button With {.Text = "OK", .Left = 260, .Top = 100, .DialogResult = DialogResult.OK, .Parent = f}
        f.AcceptButton = btn
        Return If(f.ShowDialog() = DialogResult.OK, txt.Text, "")
    End Function

    Private Function ShowReportDialog(ByRef category As String, ByRef priority As String, ByRef issue As String) As Boolean
        Dim f As New Form With {.Width = 450, .Height = 450, .Text = "Report a Concern", .StartPosition = FormStartPosition.CenterParent, .FormBorderStyle = FormBorderStyle.FixedDialog, .MaximizeBox = False}

        Dim l1 As New Label With {.Text = "Category", .Left = 20, .Top = 20, .Parent = f, .Font = New Font("Segoe UI", 9, FontStyle.Bold)}
        Dim c1 As New ComboBox With {.Left = 20, .Top = 45, .Width = 390, .Parent = f, .DropDownStyle = ComboBoxStyle.DropDownList, .Font = New Font("Segoe UI", 10)}
        c1.Items.AddRange({"Hardware", "Software", "Network", "Account", "Other"})
        c1.SelectedIndex = 0

        Dim l2 As New Label With {.Text = "Priority", .Left = 20, .Top = 85, .Parent = f, .Font = New Font("Segoe UI", 9, FontStyle.Bold)}
        Dim c2 As New ComboBox With {.Left = 20, .Top = 110, .Width = 390, .Parent = f, .DropDownStyle = ComboBoxStyle.DropDownList, .Font = New Font("Segoe UI", 10)}
        c2.Items.AddRange({"Low", "Medium", "High", "Urgent"})
        c2.SelectedIndex = 1

        Dim l3 As New Label With {.Text = "Detailed Description of Issue", .Left = 20, .Top = 150, .Parent = f, .Font = New Font("Segoe UI", 9, FontStyle.Bold)}
        Dim t1 As New TextBox With {.Left = 20, .Top = 175, .Width = 390, .Height = 100, .Multiline = True, .Parent = f, .Font = New Font("Segoe UI", 10)}

        Dim btn As New Button With {.Text = "Submit Concern", .Left = 260, .Top = 300, .DialogResult = DialogResult.OK, .Parent = f, .BackColor = Color.DodgerBlue, .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat}
        btn.FlatAppearance.BorderSize = 0

        If f.ShowDialog() = DialogResult.OK Then
            category = c1.SelectedItem.ToString()
            priority = c2.SelectedItem.ToString()
            issue = t1.Text
            Return True
        End If
        Return False
    End Function

End Class