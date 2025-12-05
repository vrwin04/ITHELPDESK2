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

    ' --- UI CONTROLS ---
    Private WithEvents ui_pnlHeader As New Panel
    Private WithEvents ui_pnlSidebar As New Panel
    Private WithEvents ui_pnlContent As New Panel

    ' Navigation Buttons
    Private WithEvents ui_btnNavDashboard As New Button
    Private WithEvents ui_btnNavTickets As New Button
    Private WithEvents ui_btnNavUsers As New Button
    Private WithEvents ui_btnNavLogout As New Button

    ' Dashboard View (Admin Only)
    Private ui_pnlViewDashboard As New Panel
    Private WithEvents ui_picGraph As New PictureBox
    Private ui_lblTileTotal As New Label
    Private ui_lblTilePending As New Label
    Private ui_lblTileResolved As New Label

    ' Tickets View (Container)
    Private ui_pnlViewTickets As New Panel

    ' Admin Grid View
    Private WithEvents ui_dgvTickets As New DataGridView

    ' Student Text View (NEW)
    Private WithEvents ui_flpHistory As New FlowLayoutPanel

    ' Controls
    Private WithEvents ui_txtSearch As New TextBox
    Private WithEvents ui_cmbStatusFilter As New ComboBox
    Private WithEvents ui_btnRefresh As New Button
    Private WithEvents ui_btnExport As New Button
    Private WithEvents ui_btnAction As New Button
    Private WithEvents ui_btnDelete As New Button
    Private WithEvents ui_lblStats As New Label

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

        ' --- ROLE BASED STARTUP ---
        If Session.CurrentUserRole = "Student" Then
            ApplyStudentLayout()
            LoadTickets()
            ShowView("Tickets") ' Student starts at History
        Else
            ApplyAdminLayout()
            LoadTickets()
            ShowView("Dashboard") ' Admin starts at Dashboard
        End If
    End Sub

    ' --- 1. LAYOUT SETUP ---
    Private Sub SetupLayout()
        Me.Size = New Size(1200, 750)
        Me.Text = "IT Help Desk"
        Me.BackColor = Color.WhiteSmoke
        Me.StartPosition = FormStartPosition.CenterScreen

        ' -- SIDEBAR --
        ui_pnlSidebar.Parent = Me
        ui_pnlSidebar.Dock = DockStyle.Left
        ui_pnlSidebar.Width = 220
        ui_pnlSidebar.BackColor = Color.FromArgb(30, 30, 40)

        Dim lblLogo As New Label
        lblLogo.Text = "IT HELP DESK"
        lblLogo.ForeColor = Color.White
        lblLogo.Font = New Font("Segoe UI", 16, FontStyle.Bold)
        lblLogo.Dock = DockStyle.Top
        lblLogo.Height = 80
        lblLogo.TextAlign = ContentAlignment.MiddleCenter
        lblLogo.Parent = ui_pnlSidebar

        ' Nav Buttons
        CreateNavBtn(ui_btnNavLogout, "Log Out", DockStyle.Bottom)
        If Session.CurrentUserRole = "Manager" Then CreateNavBtn(ui_btnNavUsers, "Manage Users", DockStyle.Top)

        Dim tListTitle As String = If(Session.CurrentUserRole = "Student", "My Concerns", "Ticket Management")
        CreateNavBtn(ui_btnNavTickets, tListTitle, DockStyle.Top)
        CreateNavBtn(ui_btnNavDashboard, "Overview", DockStyle.Top)

        ' -- HEADER --
        ui_pnlHeader.Parent = Me
        ui_pnlHeader.Dock = DockStyle.Top
        ui_pnlHeader.Height = 60
        ui_pnlHeader.BackColor = Color.White
        ui_pnlHeader.BringToFront()

        Dim lblUser As New Label
        lblUser.Text = "User: " & Session.CurrentUserName & " (" & Session.CurrentUserRole & ")"
        lblUser.Font = New Font("Segoe UI", 11, FontStyle.Italic)
        lblUser.AutoSize = True
        lblUser.Location = New Point(20, 20)
        lblUser.Parent = ui_pnlHeader

        ' -- CONTENT AREA --
        ui_pnlContent.Parent = Me
        ui_pnlContent.Dock = DockStyle.Fill
        ui_pnlContent.Padding = New Padding(20)
        ui_pnlContent.BringToFront()

        ' -- VIEW: DASHBOARD --
        ui_pnlViewDashboard.Parent = ui_pnlContent
        ui_pnlViewDashboard.Dock = DockStyle.Fill

        CreateTile(ui_pnlViewDashboard, ui_lblTileTotal, "Total Tickets", Color.SlateGray, 0)
        CreateTile(ui_pnlViewDashboard, ui_lblTilePending, "Pending", Color.Orange, 220)
        CreateTile(ui_pnlViewDashboard, ui_lblTileResolved, "Resolved", Color.SeaGreen, 440)

        ui_picGraph.Parent = ui_pnlViewDashboard
        ui_picGraph.Location = New Point(0, 150)
        ui_picGraph.Size = New Size(900, 400)
        ui_picGraph.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Bottom
        ui_picGraph.BackColor = Color.White
        ui_picGraph.BorderStyle = BorderStyle.FixedSingle

        ' -- VIEW: TICKETS --
        ui_pnlViewTickets.Parent = ui_pnlContent
        ui_pnlViewTickets.Dock = DockStyle.Fill
        ui_pnlViewTickets.Visible = False

        ' Action Bar
        ui_txtSearch.Parent = ui_pnlViewTickets
        ui_txtSearch.Location = New Point(0, 10)
        ui_txtSearch.Size = New Size(250, 30)
        ui_txtSearch.PlaceholderText = "Search..."

        ui_cmbStatusFilter.Parent = ui_pnlViewTickets
        ui_cmbStatusFilter.Location = New Point(260, 10)
        ui_cmbStatusFilter.DropDownStyle = ComboBoxStyle.DropDownList
        ui_cmbStatusFilter.Items.AddRange(New String() {"All", "Pending", "In Progress", "Resolved"})
        ui_cmbStatusFilter.SelectedIndex = 0

        ui_btnRefresh.Parent = ui_pnlViewTickets
        ui_btnRefresh.Text = "Refresh"
        ui_btnRefresh.Location = New Point(400, 9)
        ui_btnRefresh.Size = New Size(80, 25)

        ui_btnExport.Parent = ui_pnlViewTickets
        ui_btnExport.Text = "Export"
        ui_btnExport.Location = New Point(490, 9)
        ui_btnExport.Size = New Size(80, 25)

        ui_btnAction.Parent = ui_pnlViewTickets
        ui_btnAction.Size = New Size(150, 30)
        ui_btnAction.FlatStyle = FlatStyle.Flat
        ui_btnAction.Location = New Point(ui_pnlViewTickets.Width - 160, 9)
        ui_btnAction.Anchor = AnchorStyles.Top Or AnchorStyles.Right

        ui_btnDelete.Parent = ui_pnlViewTickets
        ui_btnDelete.Text = "Delete"
        ui_btnDelete.BackColor = Color.IndianRed
        ui_btnDelete.ForeColor = Color.White
        ui_btnDelete.FlatStyle = FlatStyle.Flat
        ui_btnDelete.Size = New Size(100, 30)
        ui_btnDelete.Location = New Point(ui_pnlViewTickets.Width - 270, 9)
        ui_btnDelete.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        ui_btnDelete.Visible = False

        ' -- ADMIN GRID --
        ui_dgvTickets.Parent = ui_pnlViewTickets
        ui_dgvTickets.Location = New Point(0, 50)
        ui_dgvTickets.Size = New Size(ui_pnlViewTickets.Width, ui_pnlViewTickets.Height - 50)
        ui_dgvTickets.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        ui_dgvTickets.BackgroundColor = Color.White
        ui_dgvTickets.BorderStyle = BorderStyle.None
        ui_dgvTickets.AllowUserToAddRows = False
        ui_dgvTickets.ReadOnly = True
        ui_dgvTickets.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        ui_dgvTickets.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        ' -- STUDENT HISTORY LIST (NEW) --
        ui_flpHistory.Parent = ui_pnlViewTickets
        ui_flpHistory.Location = New Point(0, 50)
        ui_flpHistory.Size = New Size(ui_pnlViewTickets.Width, ui_pnlViewTickets.Height - 50)
        ui_flpHistory.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        ui_flpHistory.AutoScroll = True
        ui_flpHistory.FlowDirection = FlowDirection.TopDown
        ui_flpHistory.WrapContents = False
        ui_flpHistory.Visible = False
    End Sub

    Private Sub CreateNavBtn(btn As Button, text As String, dock As DockStyle)
        btn.Parent = ui_pnlSidebar
        btn.Text = "  " & text
        btn.Dock = dock
        btn.Height = 55
        btn.FlatStyle = FlatStyle.Flat
        btn.FlatAppearance.BorderSize = 0
        btn.ForeColor = Color.LightGray
        btn.Font = New Font("Segoe UI", 11)
        btn.TextAlign = ContentAlignment.MiddleLeft
        btn.Cursor = Cursors.Hand
    End Sub

    Private Sub CreateTile(parentPnl As Panel, lbl As Label, title As String, color As Color, x As Integer)
        Dim pnl As New Panel
        pnl.Parent = parentPnl
        pnl.Location = New Point(x, 10)
        pnl.Size = New Size(200, 100)
        pnl.BackColor = color

        Dim lTitle As New Label
        lTitle.Text = title
        lTitle.ForeColor = Color.White
        lTitle.Location = New Point(10, 10)
        lTitle.AutoSize = True
        lTitle.Parent = pnl

        lbl.Text = "0"
        lbl.ForeColor = Color.White
        lbl.Font = New Font("Segoe UI", 24, FontStyle.Bold)
        lbl.Location = New Point(10, 35)
        lbl.AutoSize = True
        lbl.Parent = pnl
    End Sub

    ' --- 2. LAYOUT LOGIC ---
    Private Sub ApplyStudentLayout()
        ' Hide Admin stuff
        ui_btnNavDashboard.Visible = False ' Students don't need stats overview
        ui_btnNavUsers.Visible = False
        ui_btnExport.Visible = False
        ui_btnDelete.Visible = False
        ui_dgvTickets.Visible = False ' HIDE GRID

        ' Show Student stuff
        ui_flpHistory.Visible = True ' SHOW LIST

        ' Config Action Button
        ui_btnAction.Text = "+ New Concern"
        ui_btnAction.BackColor = Color.DodgerBlue
        ui_btnAction.ForeColor = Color.White
    End Sub

    Private Sub ApplyAdminLayout()
        ' Show Admin stuff
        ui_btnNavDashboard.Visible = True
        ui_btnExport.Visible = True
        ui_dgvTickets.Visible = True ' SHOW GRID
        ui_btnDelete.Visible = True

        ' Hide Student stuff
        ui_flpHistory.Visible = False

        ' Config Action Button
        ui_btnAction.Text = "Mark Resolved"
        ui_btnAction.BackColor = Color.SeaGreen
        ui_btnAction.ForeColor = Color.White
    End Sub

    Private Sub ShowView(view As String)
        ui_pnlViewDashboard.Visible = (view = "Dashboard")
        ui_pnlViewTickets.Visible = (view = "Tickets")
    End Sub

    ' --- 3. DATA LOADING & RENDERING ---
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
                dtTickets = New DataTable()
                da.Fill(dtTickets)

                ' Render based on role
                If Session.CurrentUserRole = "Student" Then
                    RenderStudentCards(dtTickets)
                Else
                    ui_dgvTickets.DataSource = dtTickets
                    UpdateStats()
                End If

            Catch ex As Exception
                ' Error handling
            End Try
        End Using
    End Sub

    ' --- NEW: TEXT/CARD RENDERER FOR STUDENTS ---
    Private Sub RenderStudentCards(dt As DataTable)
        ui_flpHistory.Controls.Clear()
        ui_flpHistory.SuspendLayout()

        If dt.Rows.Count = 0 Then
            Dim lbl As New Label With {.Text = "No concerns reported yet.", .AutoSize = True, .Font = New Font("Segoe UI", 12), .ForeColor = Color.Gray, .Margin = New Padding(20)}
            ui_flpHistory.Controls.Add(lbl)
        Else
            For Each row As DataRow In dt.Rows
                Dim pnl As New Panel
                pnl.Width = ui_flpHistory.Width - 40
                pnl.Height = 140
                pnl.BackColor = Color.White
                pnl.Margin = New Padding(0, 0, 0, 15)
                pnl.Padding = New Padding(15)

                ' Color Strip based on Status
                Dim pnlStatus As New Panel
                pnlStatus.Width = 5
                pnlStatus.Dock = DockStyle.Left
                Dim status As String = row("Status").ToString()
                If status = "Resolved" Then pnlStatus.BackColor = Color.SeaGreen Else pnlStatus.BackColor = Color.Orange
                pnl.Controls.Add(pnlStatus)

                ' Header (Date & Category)
                Dim lblHead As New Label
                lblHead.Text = Convert.ToDateTime(row("DateSubmitted")).ToString("MMM dd, yyyy") & "  •  " & row("Category").ToString() & "  •  " & row("Priority").ToString() & " Priority"
                lblHead.Font = New Font("Segoe UI", 9, FontStyle.Bold)
                lblHead.ForeColor = Color.Gray
                lblHead.Location = New Point(20, 15)
                lblHead.AutoSize = True
                pnl.Controls.Add(lblHead)

                ' Issue Body (Text)
                Dim lblBody As New Label
                lblBody.Text = row("IssueSubject").ToString()
                lblBody.Font = New Font("Segoe UI", 11, FontStyle.Regular)
                lblBody.Location = New Point(20, 40)
                lblBody.Size = New Size(pnl.Width - 40, 50)
                lblBody.AutoEllipsis = True
                pnl.Controls.Add(lblBody)

                ' Footer (Status text + Remarks)
                Dim lblFoot As New Label
                lblFoot.Location = New Point(20, 100)
                lblFoot.AutoSize = True
                lblFoot.Font = New Font("Segoe UI", 9, FontStyle.Regular)

                If status = "Resolved" Then
                    lblFoot.Text = "Status: Resolved  |  Admin Remarks: " & row("AdminRemarks").ToString()
                    lblFoot.ForeColor = Color.SeaGreen
                Else
                    lblFoot.Text = "Status: Pending (Waiting for support)"
                    lblFoot.ForeColor = Color.OrangeRed
                End If
                pnl.Controls.Add(lblFoot)

                ui_flpHistory.Controls.Add(pnl)
            Next
        End If

        ui_flpHistory.ResumeLayout()
    End Sub

    ' --- BUTTON ACTIONS ---
    Private Sub ui_btnAction_Click(sender As Object, e As EventArgs) Handles ui_btnAction.Click
        If Session.CurrentUserRole = "Student" Then
            ' STUDENT: NEW TICKET
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
            ' ADMIN: RESOLVE TICKET
            If ui_dgvTickets.SelectedRows.Count = 0 Then Return
            Dim id = Convert.ToInt32(ui_dgvTickets.SelectedRows(0).Cells("TicketID").Value)
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

    ' --- NAV EVENTS ---
    Private Sub ui_btnNavDashboard_Click(sender As Object, e As EventArgs) Handles ui_btnNavDashboard.Click
        ShowView("Dashboard")
        LoadTickets()
    End Sub

    Private Sub ui_btnNavTickets_Click(sender As Object, e As EventArgs) Handles ui_btnNavTickets.Click
        ShowView("Tickets")
        LoadTickets()
    End Sub

    Private Sub ui_btnNavUsers_Click(sender As Object, e As EventArgs) Handles ui_btnNavUsers.Click
        Dim frm As New UserManagementForm
        frm.ShowDialog()
    End Sub

    Private Sub ui_btnNavLogout_Click(sender As Object, e As EventArgs) Handles ui_btnNavLogout.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    ' --- HELPERS ---
    Private Sub UpdateStats()
        If dtTickets Is Nothing Then Return
        Dim total = dtTickets.Rows.Count
        Dim pending = 0
        Dim resolved = 0
        CategoryCounts.Clear()

        For Each row As DataRow In dtTickets.Rows
            Dim st = row("Status").ToString()
            If st = "Pending" Then pending += 1
            If st = "Resolved" Then resolved += 1

            Dim cat = row("Category").ToString()
            If CategoryCounts.ContainsKey(cat) Then CategoryCounts(cat) += 1 Else CategoryCounts.Add(cat, 1)
        Next

        ui_lblTileTotal.Text = total.ToString()
        ui_lblTilePending.Text = pending.ToString()
        ui_lblTileResolved.Text = resolved.ToString()
        ui_picGraph.Invalidate()
    End Sub

    Private Sub ui_picGraph_Paint(sender As Object, e As PaintEventArgs) Handles ui_picGraph.Paint
        If CategoryCounts.Count = 0 Then Return
        Dim g = e.Graphics
        g.SmoothingMode = SmoothingMode.AntiAlias
        Dim w = ui_picGraph.Width
        Dim h = ui_picGraph.Height
        Dim colW = w / CategoryCounts.Count
        Dim maxVal = 1
        For Each v In CategoryCounts.Values
            If v > maxVal Then maxVal = v
        Next

        Dim i = 0
        For Each kvp In CategoryCounts
            Dim barH = (kvp.Value / maxVal) * (h - 50)
            Dim rect As New Rectangle(i * colW + 20, h - barH - 30, colW - 40, barH)
            g.FillRectangle(Brushes.SteelBlue, rect)
            g.DrawString(kvp.Value.ToString(), Me.Font, Brushes.Black, rect.X + 10, rect.Y - 20)
            g.DrawString(kvp.Key, Me.Font, Brushes.Black, rect.X, h - 20)
            i += 1
        Next
    End Sub

    Private Sub ui_btnDelete_Click(sender As Object, e As EventArgs) Handles ui_btnDelete.Click
        If ui_dgvTickets.SelectedRows.Count = 0 Then Return
        If MessageBox.Show("Delete this ticket?", "Confirm", MessageBoxButtons.YesNo) = DialogResult.Yes Then
            Dim id = Convert.ToInt32(ui_dgvTickets.SelectedRows(0).Cells("TicketID").Value)
            Using conn As New OleDbConnection(ConnString)
                conn.Open()
                Dim cmd As New OleDbCommand("DELETE FROM tblTickets WHERE TicketID=?", conn)
                cmd.Parameters.AddWithValue("?", id)
                cmd.ExecuteNonQuery()
            End Using
            LoadTickets()
        End If
    End Sub

    Private Sub ui_btnRefresh_Click(sender As Object, e As EventArgs) Handles ui_btnRefresh.Click
        LoadTickets()
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