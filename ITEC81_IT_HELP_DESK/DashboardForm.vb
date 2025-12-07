Imports System.Data.OleDb
Imports System.Drawing.Drawing2D

Public Class DashboardForm

    ' --- DATA VARIABLES ---
    Private dtTickets As New DataTable
    Private CategoryCounts As New Dictionary(Of String, Integer)

    ' --- ANIMATION VARIABLES ---
    Private WithEvents AnimationTimer As New Timer With {.Interval = 15}
    Private AnimationProgress As Single = 0.0F

    ' --- FORM LOAD ---
    Private Sub DashboardForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.DoubleBuffered = True

        LoadTickets()

        ' Setup View based on Role
        If Session.CurrentUserRole = "Student" Then
            ApplyStudentLayout()
            ShowView("Tickets")
        Else
            ApplyAdminLayout()
            ShowView("Dashboard")
        End If
    End Sub

    ' --- NAVIGATION ---
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
        Dim login As New Form1()
        login.Show()
        Me.Close()
    End Sub

    ' --- DATA LOADING (Manual Code) ---
    Private Sub LoadTickets()
        Dim query As String

        If Session.CurrentUserRole = "Student" Then
            query = "SELECT [TicketID], [Category], [Priority], [IssueSubject], [Status], [AdminRemarks], [DateSubmitted] FROM tblTickets WHERE [SubmittedBy] = ? ORDER BY [DateSubmitted] DESC"
        Else
            query = "SELECT t.TicketID, u.Username AS [ReportedBy], t.Category, t.Priority, t.IssueSubject, t.Status, t.AdminRemarks, t.DateSubmitted FROM tblTickets t INNER JOIN tblUsers u ON t.SubmittedBy = u.UserID ORDER BY t.DateSubmitted DESC"
        End If

        dtTickets = New DataTable()

        Using conn As New OleDbConnection(Session.connString)
            Using cmd As New OleDbCommand(query, conn)
                ' Only add parameter if Student
                If Session.CurrentUserRole = "Student" Then
                    cmd.Parameters.AddWithValue("?", Session.CurrentUserID)
                End If

                Try
                    conn.Open()
                    Dim da As New OleDbDataAdapter(cmd)
                    da.Fill(dtTickets)
                Catch ex As Exception
                    MessageBox.Show("Error loading tickets: " & ex.Message)
                End Try
            End Using
        End Using

        If Session.CurrentUserRole = "Student" Then
            RenderStudentCards()
        Else
            FilterData()
            LoadAnalyticsData()
        End If
    End Sub

    ' --- ACTIONS (Submit/Resolve) ---
    Private Sub btnAction_Click(sender As Object, e As EventArgs) Handles btnAction.Click
        If Session.CurrentUserRole = "Student" Then
            ' --- STUDENT: CREATE TICKET ---
            Dim category As String = ""
            Dim priority As String = ""
            Dim issue As String = ""
            If ShowReportDialog(category, priority, issue) Then
                Using conn As New OleDbConnection(Session.connString)
                    Dim sql As String = "INSERT INTO tblTickets (SubmittedBy, Category, Priority, IssueSubject, Status, DateSubmitted) VALUES (?, ?, ?, ?, ?, ?)"
                    Using cmd As New OleDbCommand(sql, conn)
                        cmd.Parameters.AddWithValue("?", Session.CurrentUserID)
                        cmd.Parameters.AddWithValue("?", category)
                        cmd.Parameters.AddWithValue("?", priority)
                        cmd.Parameters.AddWithValue("?", issue)
                        cmd.Parameters.AddWithValue("?", "Pending")
                        cmd.Parameters.AddWithValue("?", DateTime.Now.ToString())
                        conn.Open()
                        cmd.ExecuteNonQuery()
                        MessageBox.Show("Concern submitted!")
                        LoadTickets()
                    End Using
                End Using
            End If
        Else
            ' --- ADMIN: RESOLVE TICKET ---
            If dgvTickets.SelectedRows.Count = 0 Then Return
            Dim id = Convert.ToInt32(dgvTickets.SelectedRows(0).Cells("TicketID").Value)
            Dim remk = CustomInputBox("Resolution Note:", "Resolve Ticket")

            Using conn As New OleDbConnection(Session.connString)
                Dim sql As String = "UPDATE tblTickets SET Status='Resolved', AdminRemarks=? WHERE TicketID=?"
                Using cmd As New OleDbCommand(sql, conn)
                    cmd.Parameters.AddWithValue("?", remk)
                    cmd.Parameters.AddWithValue("?", id)
                    conn.Open()
                    cmd.ExecuteNonQuery()
                    MessageBox.Show("Ticket Resolved!")
                    LoadTickets()
                End Using
            End Using
        End If
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If dgvTickets.SelectedRows.Count = 0 Then Return
        If MessageBox.Show("Delete this ticket?", "Confirm", MessageBoxButtons.YesNo) = DialogResult.Yes Then
            Dim id = Convert.ToInt32(dgvTickets.SelectedRows(0).Cells("TicketID").Value)
            Using conn As New OleDbConnection(Session.connString)
                Dim cmd As New OleDbCommand("DELETE FROM tblTickets WHERE TicketID=?", conn)
                cmd.Parameters.AddWithValue("?", id)
                conn.Open()
                cmd.ExecuteNonQuery()
            End Using
            LoadTickets()
        End If
    End Sub

    ' --- STUDENT UI RENDERER ---
    Private Sub RenderStudentCards()
        flpHistory.Controls.Clear()
        flpHistory.SuspendLayout()

        If dtTickets.Rows.Count = 0 Then
            Dim lbl As New Label With {.Text = "No concerns reported yet.", .AutoSize = True, .Font = New Font("Segoe UI", 12), .ForeColor = Color.Gray, .Margin = New Padding(20)}
            flpHistory.Controls.Add(lbl)
        End If

        For Each row As DataRow In dtTickets.Rows
            Dim pnl As New Panel With {.Width = flpHistory.Width - 40, .Height = 110, .BackColor = color.White, .Margin = New Padding(0, 0, 0, 10), .Padding = New Padding(15), .BorderStyle = BorderStyle.FixedSingle}
            Dim status As String = row("Status").ToString()
            Dim color As Color = If(status = "Resolved", Color.SeaGreen, If(status = "In Progress", Color.DodgerBlue, Color.Orange))

            pnl.Controls.Add(New Panel With {.Width = 5, .Dock = DockStyle.Left, .BackColor = color})
            pnl.Controls.Add(New Label With {.Text = Convert.ToDateTime(row("DateSubmitted")).ToString("MMM dd") & " • " & row("Category").ToString(), .Font = New Font("Segoe UI", 9, FontStyle.Bold), .ForeColor = Color.Gray, .Location = New Point(20, 10), .AutoSize = True})
            pnl.Controls.Add(New Label With {.Text = row("IssueSubject").ToString(), .Font = New Font("Segoe UI", 12, FontStyle.Bold), .Location = New Point(20, 35), .Size = New Size(pnl.Width - 40, 25), .AutoEllipsis = True})
            pnl.Controls.Add(New Label With {.Text = "Status: " & status.ToUpper(), .Location = New Point(20, 70), .AutoSize = True, .Font = New Font("Segoe UI", 9, FontStyle.Bold), .ForeColor = color})
            flpHistory.Controls.Add(pnl)
        Next
        flpHistory.ResumeLayout()
    End Sub

    ' --- ADMIN UI HELPERS ---
    Private Sub FilterData()
        Dim view As DataView = dtTickets.DefaultView
        Dim filter As String = ""
        If Not String.IsNullOrEmpty(txtSearch.Text) Then filter = $"(IssueSubject LIKE '%{txtSearch.Text}%')"
        If cmbStatusFilter.SelectedIndex > 0 Then
            If filter.Length > 0 Then filter &= " AND "
            filter &= $"Status = '{cmbStatusFilter.SelectedItem}'"
        End If
        Try : view.RowFilter = filter : Catch : End Try
        dgvTickets.DataSource = view
        UpdateStats(view)
    End Sub

    Private Sub UpdateStats(view As DataView)
        Dim total = view.Count
        Dim pending = 0, resolved = 0
        For Each row As DataRowView In view
            If row("Status").ToString() = "Pending" Then pending += 1
            If row("Status").ToString() = "Resolved" Then resolved += 1
        Next
        lblTileTotalVal.Text = total.ToString()
        lblTilePendingVal.Text = pending.ToString()
        lblTileResolvedVal.Text = resolved.ToString()
        AnimationProgress = 0
        AnimationTimer.Start()
    End Sub

    ' --- CHARTING ---
    Private Sub LoadAnalyticsData()
        CategoryCounts.Clear()
        For Each row As DataRow In dtTickets.Rows
            Dim cat = row("Category").ToString()
            If CategoryCounts.ContainsKey(cat) Then CategoryCounts(cat) += 1 Else CategoryCounts.Add(cat, 1)
        Next
    End Sub

    Private Sub AnimationTimer_Tick(sender As Object, e As EventArgs) Handles AnimationTimer.Tick
        AnimationProgress += 0.05F
        If AnimationProgress >= 1.0F Then AnimationProgress = 1.0F : AnimationTimer.Stop()
        picGraph.Invalidate()
    End Sub

    Private Sub picGraph_Paint(sender As Object, e As PaintEventArgs) Handles picGraph.Paint
        Dim g = e.Graphics
        g.SmoothingMode = SmoothingMode.AntiAlias
        g.Clear(Color.White)
        If CategoryCounts.Count = 0 Then Return

        Dim w = picGraph.Width, h = picGraph.Height
        Dim maxVal = 0
        For Each v In CategoryCounts.Values : If v > maxVal Then maxVal = v : Next
            If maxVal = 0 Then maxVal = 5

            Dim count = CategoryCounts.Count
            Dim barWidth = 60
            Dim spacing = (w - (count * barWidth)) / (count + 1)
            Dim idx = 0

            For Each kvp In CategoryCounts
                Dim barH = CInt((kvp.Value / maxVal) * (h - 100) * AnimationProgress)
                Dim x = spacing + idx * (barWidth + spacing)
                Dim y = h - 50 - barH
                g.FillRectangle(Brushes.DodgerBlue, x, y, barWidth, barH)
                g.DrawString(kvp.Value.ToString(), Me.Font, Brushes.Black, x + 20, y - 20)
                g.DrawString(kvp.Key, Me.Font, Brushes.DimGray, x, h - 40)
                idx += 1
            Next
    End Sub

    ' --- LAYOUT & EVENTS ---
    Private Sub ApplyStudentLayout()
        btnNavDashboard.Visible = False : btnNavUsers.Visible = False : btnExport.Visible = False : btnDelete.Visible = False
        dgvTickets.Visible = False : flpHistory.Visible = True : btnAction.Text = "➕ New Concern" : btnAction.BackColor = Color.DodgerBlue
    End Sub
    Private Sub ApplyAdminLayout()
        btnNavDashboard.Visible = True : btnExport.Visible = True : btnDelete.Visible = True
        dgvTickets.Visible = True : flpHistory.Visible = False : btnAction.Text = "✅ Mark Resolved" : btnAction.BackColor = Color.SeaGreen
    End Sub
    Private Sub btnRefresh_Click(s As Object, e As EventArgs) Handles btnRefresh.Click : LoadTickets() : End Sub
    Private Sub txtSearch_TextChanged(s As Object, e As EventArgs) Handles txtSearch.TextChanged : FilterData() : End Sub
    Private Sub cmbStatusFilter_SelectedIndexChanged(s As Object, e As EventArgs) Handles cmbStatusFilter.SelectedIndexChanged : FilterData() : End Sub
    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click : MessageBox.Show("Export feature placeholder") : End Sub

    ' --- DIALOGS ---
    Private Function CustomInputBox(prompt As String, title As String) As String
        Dim f As New Form With {.Width = 400, .Height = 150, .Text = title, .StartPosition = FormStartPosition.CenterParent}
        Dim t As New TextBox With {.Left = 20, .Top = 50, .Width = 340, .Parent = f}
        Dim b As New Button With {.Text = "OK", .Left = 260, .Top = 80, .DialogResult = DialogResult.OK, .Parent = f}
        f.Controls.Add(New Label With {.Text = prompt, .Left = 20, .Top = 20, .AutoSize = True})
        f.AcceptButton = b
        Return If(f.ShowDialog() = DialogResult.OK, t.Text, "")
    End Function

    Private Function ShowReportDialog(ByRef category As String, ByRef priority As String, ByRef issue As String) As Boolean
        Dim f As New Form With {.Width = 400, .Height = 400, .Text = "New Concern", .StartPosition = FormStartPosition.CenterParent}
        Dim c1 As New ComboBox With {.Left = 20, .Top = 40, .Width = 340, .DropDownStyle = ComboBoxStyle.DropDownList, .Parent = f}
        c1.Items.AddRange({"Hardware", "Software", "Network", "Other"}) : c1.SelectedIndex = 0
        Dim c2 As New ComboBox With {.Left = 20, .Top = 100, .Width = 340, .DropDownStyle = ComboBoxStyle.DropDownList, .Parent = f}
        c2.Items.AddRange({"Low", "Medium", "High"}) : c2.SelectedIndex = 1
        Dim t1 As New TextBox With {.Left = 20, .Top = 160, .Width = 340, .Height = 100, .Multiline = True, .Parent = f}
        Dim btn As New Button With {.Text = "Submit", .Left = 260, .Top = 280, .DialogResult = DialogResult.OK, .Parent = f}
        f.Controls.AddRange({New Label With {.Text = "Category", .Top = 20, .Left = 20}, New Label With {.Text = "Priority", .Top = 80, .Left = 20}, New Label With {.Text = "Description", .Top = 140, .Left = 20}})
        If f.ShowDialog() = DialogResult.OK Then
            category = c1.SelectedItem.ToString() : priority = c2.SelectedItem.ToString() : issue = t1.Text : Return True
        End If
        Return False
    End Function
End Class