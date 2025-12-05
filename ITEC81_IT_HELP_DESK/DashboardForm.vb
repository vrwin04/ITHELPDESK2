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

    ' Database Connection
    Private ReadOnly Property ConnString As String
        Get
            Dim dbPath As String = Path.Combine(Application.StartupPath, "ITHelpDeskDB.accdb")
            Return "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & dbPath & ";"
        End Get
    End Property

    Private Sub DashboardForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Controls.Clear()
        Me.DoubleBuffered = True
        SetupLayout()
        LoadTickets()

        If Session.CurrentUserRole = "Student" Then
            ApplyStudentLayout()
            ShowView("Tickets")
        Else
            ApplyAdminLayout()
            ShowView("Dashboard")
        End If
    End Sub

    ' --- 1. LAYOUT SETUP ---
    Private Sub SetupLayout()
        Me.Size = New Size(1280, 720) ' Fixed size 1280x720
        Me.Text = "IT Help Desk System"
        Me.BackColor = Color.FromArgb(240, 242, 245) ' Soft gray background
        Me.StartPosition = FormStartPosition.CenterScreen

        ' -- SIDEBAR --
        pnlSidebar.Parent = Me
        pnlSidebar.Dock = DockStyle.Left
        pnlSidebar.Width = 240
        pnlSidebar.BackColor = Color.FromArgb(30, 35, 50)

        ' Nav Buttons (Added FIRST so they are pushed down by the logo later)
        CreateNavBtn(btnNavLogout, "Log Out", DockStyle.Bottom)

        If Session.CurrentUserRole = "Manager" Then CreateNavBtn(btnNavUsers, "Manage Users", DockStyle.Top)

        Dim tListTitle As String = If(Session.CurrentUserRole = "Student", "My Concerns", "Ticket List")
        CreateNavBtn(btnNavTickets, tListTitle, DockStyle.Top)
        CreateNavBtn(btnNavDashboard, "Dashboard", DockStyle.Top)

        ' Redesigned Logo Label (Added LAST to ensure it stays at the TOP LEFT)
        Dim lblLogo As New Label
        lblLogo.Text = "IT HELP DESK"
        lblLogo.ForeColor = Color.White
        lblLogo.Font = New Font("Segoe UI", 14, FontStyle.Bold)
        lblLogo.Dock = DockStyle.Top
        lblLogo.Height = 80
        lblLogo.TextAlign = ContentAlignment.MiddleLeft
        lblLogo.Padding = New Padding(20, 0, 0, 0) ' Push text to the right
        lblLogo.Parent = pnlSidebar
        lblLogo.BringToFront() ' Force to top

        ' -- HEADER --
        pnlHeader.Parent = Me
        pnlHeader.Dock = DockStyle.Top
        pnlHeader.Height = 60
        pnlHeader.BackColor = Color.White
        pnlHeader.BringToFront()

        Dim lblUser As New Label With {.Text = "Welcome, " & Session.CurrentUserName, .Font = New Font("Segoe UI", 11, FontStyle.Bold), .ForeColor = Color.DimGray, .AutoSize = True, .Location = New Point(25, 20), .Parent = pnlHeader}

        ' -- CONTENT AREA --
        pnlContent.Parent = Me
        pnlContent.Dock = DockStyle.Fill
        pnlContent.Padding = New Padding(30)
        pnlContent.BringToFront()

        ' ==========================
        '      DASHBOARD VIEW
        ' ==========================
        pnlViewDashboard.Parent = pnlContent
        pnlViewDashboard.Dock = DockStyle.Fill

        ' 1. Summary Tiles (Redesigned as Cards)
        CreateModernTile(pnlTileTotal, lblTileTotalVal, "Total Tickets", Color.SlateGray, 0)
        CreateModernTile(pnlTilePending, lblTilePendingVal, "Pending", Color.Orange, 260)
        CreateModernTile(pnlTileResolved, lblTileResolvedVal, "Resolved", Color.SeaGreen, 520)

        ' 2. Graph (Redesigned - No Border)
        picGraph.Parent = pnlViewDashboard
        picGraph.Location = New Point(0, 160)
        picGraph.Size = New Size(pnlContent.Width - 60, 450)
        picGraph.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Bottom
        picGraph.BackColor = Color.White ' Clean white background
        picGraph.BorderStyle = BorderStyle.None ' REMOVED BORDER

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
        txtSearch.PlaceholderText = "Search tickets..."

        ' Filter Dropdown
        cmbStatusFilter.Parent = pnlViewTickets
        cmbStatusFilter.Location = New Point(260, 10)
        cmbStatusFilter.DropDownStyle = ComboBoxStyle.DropDownList
        cmbStatusFilter.Items.AddRange(New String() {"All", "Pending", "In Progress", "Resolved"})
        cmbStatusFilter.SelectedIndex = 0

        ' Refresh Button
        btnRefresh.Parent = pnlViewTickets
        btnRefresh.Text = "Refresh"
        btnRefresh.Location = New Point(400, 9)
        btnRefresh.Size = New Size(80, 25)

        ' Export Button
        btnExport.Parent = pnlViewTickets
        btnExport.Text = "Export"
        btnExport.Location = New Point(490, 9)
        btnExport.Size = New Size(80, 25)

        ' Action Buttons
        btnAction.Parent = pnlViewTickets
        btnAction.Size = New Size(140, 30)
        btnAction.FlatStyle = FlatStyle.Flat
        btnAction.Location = New Point(pnlViewTickets.Width - 150, 9)
        btnAction.Anchor = AnchorStyles.Top Or AnchorStyles.Right

        btnDelete.Parent = pnlViewTickets
        btnDelete.Text = "Delete"
        btnDelete.BackColor = Color.IndianRed
        btnDelete.ForeColor = Color.White
        btnDelete.FlatStyle = FlatStyle.Flat
        btnDelete.Size = New Size(100, 30)
        btnDelete.Location = New Point(pnlViewTickets.Width - 260, 9)
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

    Private Sub CreateNavBtn(btn As Button, text As String, dock As DockStyle)
        btn.Parent = pnlSidebar
        btn.Text = "  " & text
        btn.Dock = dock
        btn.Height = 55
        btn.FlatStyle = FlatStyle.Flat
        btn.FlatAppearance.BorderSize = 0
        btn.ForeColor = Color.FromArgb(200, 200, 200)
        btn.Font = New Font("Segoe UI", 10, FontStyle.Regular)
        btn.TextAlign = ContentAlignment.MiddleLeft
        btn.Padding = New Padding(20, 0, 0, 0)
        btn.Cursor = Cursors.Hand
        AddHandler btn.MouseEnter, Sub() btn.BackColor = Color.FromArgb(50, 55, 70)
        AddHandler btn.MouseLeave, Sub() btn.BackColor = Color.Transparent
    End Sub

    ' --- NEW TILE DESIGN ---
    Private Sub CreateModernTile(pnl As Panel, lblVal As Label, title As String, accentColor As Color, x As Integer)
        pnl.Parent = pnlViewDashboard
        pnl.Location = New Point(x, 10)
        pnl.Size = New Size(240, 120)
        pnl.BackColor = Color.White
        ' No border for cleaner look

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

    ' --- DATA LOGIC ---
    Private Sub LoadTickets()
        Using conn As New OleDbConnection(ConnString)
            Try
                conn.Open()
                Dim sql As String
                Dim cmd As OleDbCommand

                If Session.CurrentUserRole = "Student" Then
                    sql = "SELECT [TicketID], [Category], [Priority], [IssueSubject], [Status], [AdminRemarks], [DateSubmitted] FROM tblTickets WHERE [SubmittedBy] = ? ORDER BY [DateSubmitted] DESC"
                    cmd = New OleDbCommand(sql, conn)
                    cmd.Parameters.AddWithValue("?", Session.CurrentUserID)
                Else
                    sql = "SELECT t.TicketID, u.Username AS [ReportedBy], t.Category, t.Priority, t.IssueSubject, t.Status, t.AdminRemarks, t.DateSubmitted FROM tblTickets t INNER JOIN tblUsers u ON t.SubmittedBy = u.UserID ORDER BY t.DateSubmitted DESC"
                    cmd = New OleDbCommand(sql, conn)
                End If

                Dim da As New OleDbDataAdapter(cmd)
                dtTickets.Clear()
                da.Fill(dtTickets)

                If Session.CurrentUserRole = "Student" Then
                    RenderStudentCards()
                Else
                    FilterData()
                    LoadAnalyticsData()
                End If
            Catch ex As Exception
            End Try
        End Using
    End Sub

    Private Sub RenderStudentCards()
        flpHistory.Controls.Clear()
        flpHistory.SuspendLayout()

        If dtTickets.Rows.Count = 0 Then
            Dim lbl As New Label With {.Text = "No concerns reported yet.", .AutoSize = True, .Font = New Font("Segoe UI", 12), .ForeColor = Color.Gray, .Margin = New Padding(20)}
            flpHistory.Controls.Add(lbl)
        Else
            For Each row As DataRow In dtTickets.Rows
                Dim pnl As New Panel
                pnl.Width = flpHistory.Width - 40
                pnl.Height = 140
                pnl.BackColor = Color.White
                pnl.Margin = New Padding(0, 0, 0, 15)
                pnl.Padding = New Padding(15)

                Dim pnlStatus As New Panel
                pnlStatus.Width = 5
                pnlStatus.Dock = DockStyle.Left
                Dim status As String = row("Status").ToString()
                If status = "Resolved" Then pnlStatus.BackColor = Color.SeaGreen Else pnlStatus.BackColor = Color.Orange
                pnl.Controls.Add(pnlStatus)

                Dim lblHead As New Label
                lblHead.Text = Convert.ToDateTime(row("DateSubmitted")).ToString("MMM dd, yyyy") & "  •  " & row("Category").ToString() & "  •  " & row("Priority").ToString() & " Priority"
                lblHead.Font = New Font("Segoe UI", 9, FontStyle.Bold)
                lblHead.ForeColor = Color.Gray
                lblHead.Location = New Point(20, 15)
                lblHead.AutoSize = True
                pnl.Controls.Add(lblHead)

                Dim lblBody As New Label
                lblBody.Text = row("IssueSubject").ToString()
                lblBody.Font = New Font("Segoe UI", 11, FontStyle.Regular)
                lblBody.Location = New Point(20, 40)
                lblBody.Size = New Size(pnl.Width - 40, 50)
                lblBody.AutoEllipsis = True
                pnl.Controls.Add(lblBody)

                Dim lblFoot As New Label
                lblFoot.Location = New Point(20, 100)
                lblFoot.AutoSize = True
                lblFoot.Font = New Font("Segoe UI", 9, FontStyle.Regular)

                If status = "Resolved" Then
                    lblFoot.Text = "Status: Resolved  | Admin Remarks: " & row("AdminRemarks").ToString()
                    lblFoot.ForeColor = Color.SeaGreen
                Else
                    lblFoot.Text = "Status: Pending (Waiting for support)"
                    lblFoot.ForeColor = Color.OrangeRed
                End If
                pnl.Controls.Add(lblFoot)

                flpHistory.Controls.Add(pnl)
            Next
        End If
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

    ' --- REDESIGNED BAR CHART (No Border, Floating Style, FIXED TYPES) ---
    Private Sub picGraph_Paint(sender As Object, e As PaintEventArgs) Handles picGraph.Paint
        Dim g = e.Graphics
        g.SmoothingMode = SmoothingMode.AntiAlias
        g.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit

        ' 1. Clear Background (Ensure White)
        g.Clear(Color.White)

        If CategoryCounts.Count = 0 Then
            Dim msg = "No Data Available"
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

        Dim colors() As Color = {Color.FromArgb(100, 149, 237), Color.FromArgb(60, 179, 113), Color.FromArgb(255, 165, 0), Color.FromArgb(255, 99, 71)}

        For Each kvp In CategoryCounts
            Dim val As Integer = kvp.Value
            ' Animate Height
            Dim barH As Integer = CInt((val / topScale) * graphHeight * AnimationProgress)

            Dim x As Integer = startX + (idx * slotWidth) + (slotWidth - barWidth) \ 2
            Dim y As Integer = (h - paddingBottom) - barH

            Dim rect As New Rectangle(x, y, barWidth, barH)

            ' Gradient Fill
            If barH > 0 Then
                Using br As New LinearGradientBrush(rect, colors(idx Mod colors.Length), ControlPaint.Light(colors(idx Mod colors.Length)), LinearGradientMode.Vertical)
                    g.FillRectangle(br, rect)
                End Using
            End If

            ' Value Label (Floating Top) - VISIBLE ALWAYS
            Dim valStr = val.ToString()
            Dim fontVal As New Font("Segoe UI", 10, FontStyle.Bold)
            Dim szVal = g.MeasureString(valStr, fontVal)
            g.DrawString(valStr, fontVal, Brushes.Black, CSng(x + (barWidth - szVal.Width) / 2), CSng(y - 20))

            ' Category Label (Bottom) - WITH CSNG FIX
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
        btnAction.Text = "+ New Ticket"
        btnAction.BackColor = Color.DodgerBlue
        btnAction.ForeColor = Color.White
    End Sub

    Private Sub ApplyAdminLayout()
        btnNavDashboard.Visible = True
        btnExport.Visible = True
        btnDelete.Visible = True
        dgvTickets.Visible = True
        flpHistory.Visible = False
        btnAction.Text = "Mark Resolved"
        btnAction.BackColor = Color.SeaGreen
        btnAction.ForeColor = Color.White
    End Sub

    ' --- ACTIONS ---
    Private Sub btnAction_Click(sender As Object, e As EventArgs) Handles btnAction.Click
        If Session.CurrentUserRole = "Student" Then
            Dim category As String = ""
            Dim priority As String = ""
            Dim issue As String = ""
            If ShowReportDialog(category, priority, issue) Then
                Using conn As New OleDbConnection(ConnString)
                    conn.Open()
                    Dim cmd As New OleDbCommand("INSERT INTO tblTickets (SubmittedBy, Category, Priority, IssueSubject, Status, DateSubmitted) VALUES (?, ?, ?, ?, ?, ?)", conn)
                    cmd.Parameters.AddWithValue("?", Session.CurrentUserID)
                    cmd.Parameters.AddWithValue("?", category)
                    cmd.Parameters.AddWithValue("?", priority)
                    cmd.Parameters.AddWithValue("?", issue)
                    cmd.Parameters.AddWithValue("?", "Pending")
                    cmd.Parameters.AddWithValue("?", DateTime.Now.ToString())
                    cmd.ExecuteNonQuery()
                End Using
                MessageBox.Show("Concern submitted!")
                LoadTickets()
            End If
        Else
            If dgvTickets.SelectedRows.Count = 0 Then Return
            Dim id = Convert.ToInt32(dgvTickets.SelectedRows(0).Cells("TicketID").Value)
            Dim remk = CustomInputBox("Resolution Remarks:", "Resolve")
            Using conn As New OleDbConnection(ConnString)
                conn.Open()
                Dim cmd As New OleDbCommand("UPDATE tblTickets SET Status='Resolved', AdminRemarks=? WHERE TicketID=?", conn)
                cmd.Parameters.AddWithValue("?", remk)
                cmd.Parameters.AddWithValue("?", id)
                cmd.ExecuteNonQuery()
            End Using
            MessageBox.Show("Resolved!")
            LoadTickets()
        End If
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If dgvTickets.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a ticket to delete.")
            Return
        End If

        If MessageBox.Show("Delete this ticket?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
            Dim id = Convert.ToInt32(dgvTickets.SelectedRows(0).Cells("TicketID").Value)
            Using conn As New OleDbConnection(ConnString)
                conn.Open()
                Dim cmd As New OleDbCommand("DELETE FROM tblTickets WHERE TicketID=?", conn)
                cmd.Parameters.AddWithValue("?", id)
                cmd.ExecuteNonQuery()
            End Using
            LoadTickets()
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
        Dim sfd As New SaveFileDialog With {.Filter = "CSV|*.csv", .FileName = "Report.csv"}
        If sfd.ShowDialog() = DialogResult.OK Then
            Dim sb As New StringBuilder
            For Each col As DataColumn In dtTickets.Columns
                sb.Append(col.ColumnName & ",")
            Next
            sb.AppendLine()
            For Each row As DataRow In dtTickets.Rows
                For Each item In row.ItemArray
                    sb.Append(item.ToString().Replace(",", " ") & ",")
                Next
                sb.AppendLine()
            Next
            File.WriteAllText(sfd.FileName, sb.ToString())
            MessageBox.Show("Exported!")
        End If
    End Sub

    Private Function CustomInputBox(prompt As String, title As String) As String
        Dim f As New Form With {.Width = 400, .Height = 200, .Text = title, .StartPosition = FormStartPosition.CenterParent}
        Dim lbl As New Label With {.Text = prompt, .Left = 20, .Top = 20, .Width = 340, .Parent = f}
        Dim txt As New TextBox With {.Left = 20, .Top = 60, .Width = 340, .Parent = f}
        Dim btn As New Button With {.Text = "OK", .Left = 260, .Top = 100, .DialogResult = DialogResult.OK, .Parent = f}
        f.AcceptButton = btn
        Return If(f.ShowDialog() = DialogResult.OK, txt.Text, "")
    End Function

    Private Function ShowReportDialog(ByRef category As String, ByRef priority As String, ByRef issue As String) As Boolean
        Dim f As New Form With {.Width = 450, .Height = 450, .Text = "Report a Concern", .StartPosition = FormStartPosition.CenterParent}
        Dim l1 As New Label With {.Text = "Category", .Left = 20, .Top = 20, .Parent = f}
        Dim c1 As New ComboBox With {.Left = 20, .Top = 45, .Width = 390, .Parent = f, .DropDownStyle = ComboBoxStyle.DropDownList}
        c1.Items.AddRange({"Hardware", "Software", "Network", "Account"})
        c1.SelectedIndex = 0
        Dim l2 As New Label With {.Text = "Priority", .Left = 20, .Top = 85, .Parent = f}
        Dim c2 As New ComboBox With {.Left = 20, .Top = 110, .Width = 390, .Parent = f, .DropDownStyle = ComboBoxStyle.DropDownList}
        c2.Items.AddRange({"Low", "Medium", "High"})
        c2.SelectedIndex = 1
        Dim l3 As New Label With {.Text = "Description", .Left = 20, .Top = 150, .Parent = f}
        Dim t1 As New TextBox With {.Left = 20, .Top = 175, .Width = 390, .Height = 100, .Multiline = True, .Parent = f}
        Dim btn As New Button With {.Text = "Submit", .Left = 260, .Top = 300, .DialogResult = DialogResult.OK, .Parent = f}
        If f.ShowDialog() = DialogResult.OK Then
            category = c1.SelectedItem.ToString()
            priority = c2.SelectedItem.ToString()
            issue = t1.Text
            Return True
        End If
        Return False
    End Function

End Class