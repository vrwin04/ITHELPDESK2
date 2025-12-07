Imports System.Data.OleDb
Imports System.IO
Imports System.Drawing.Drawing2D
Imports System.Text

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

        ' Load initial data
        LoadTickets()

        ' Apply role-specific visibility
        If Session.CurrentUserRole = "Student" Then
            ApplyStudentLayout()
            ShowView("Tickets")
        Else
            ApplyAdminLayout()
            ShowView("Dashboard")
        End If
    End Sub

    ' --- NAVIGATION LOGIC ---
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

    ' --- DATA LOADING ---
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

    ' --- ACTIONS ---
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
                        MessageBox.Show("Concern submitted successfully!")
                        LoadTickets()
                    End Using
                End Using
            End If
        Else
            ' --- ADMIN: RESOLVE TICKET ---
            If dgvTickets.SelectedRows.Count = 0 Then Return

            Dim id As Integer = Convert.ToInt32(dgvTickets.SelectedRows(0).Cells("TicketID").Value)
            Dim remk As String = CustomInputBox("Resolution Note:", "Resolve Ticket")

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
            Dim id As Integer = Convert.ToInt32(dgvTickets.SelectedRows(0).Cells("TicketID").Value)

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
            ' Define color FIRST before creating the panel
            Dim status As String = row("Status").ToString()
            Dim stColor As Color ' Renamed variable to avoid conflict with Color class

            If status = "Resolved" Then
                stColor = Color.SeaGreen
            ElseIf status = "In Progress" Then
                stColor = Color.DodgerBlue
            Else
                stColor = Color.Orange
            End If

            ' Now create the panel using the variable
            Dim pnl As New Panel With {
                .Width = flpHistory.Width - 40,
                .Height = 110,
                .BackColor = Color.White,
                .Margin = New Padding(0, 0, 0, 10),
                .Padding = New Padding(15),
                .BorderStyle = BorderStyle.FixedSingle
            }

            ' Add controls to panel
            Dim pnlBar As New Panel With {.Width = 5, .Dock = DockStyle.Left, .BackColor = stColor}
            pnl.Controls.Add(pnlBar)

            Dim dateStr As String = Convert.ToDateTime(row("DateSubmitted")).ToString("MMM dd")
            Dim catStr As String = row("Category").ToString()
            Dim lblHead As New Label With {
                .Text = dateStr & " • " & catStr,
                .Font = New Font("Segoe UI", 9, FontStyle.Bold),
                .ForeColor = Color.Gray,
                .Location = New Point(20, 10),
                .AutoSize = True
            }
            pnl.Controls.Add(lblHead)

            Dim lblBody As New Label With {
                .Text = row("IssueSubject").ToString(),
                .Font = New Font("Segoe UI", 12, FontStyle.Bold),
                .Location = New Point(20, 35),
                .Size = New Size(pnl.Width - 40, 25),
                .AutoEllipsis = True
            }
            pnl.Controls.Add(lblBody)

            Dim lblFoot As New Label With {
                .Text = "Status: " & status.ToUpper(),
                .Location = New Point(20, 70),
                .AutoSize = True,
                .Font = New Font("Segoe UI", 9, FontStyle.Bold),
                .ForeColor = stColor
            }
            pnl.Controls.Add(lblFoot)

            flpHistory.Controls.Add(pnl)
        Next

        flpHistory.ResumeLayout()
    End Sub

    ' --- ADMIN UI HELPERS ---
    Private Sub FilterData()
        Dim view As DataView = dtTickets.DefaultView
        Dim filter As String = ""

        If Not String.IsNullOrEmpty(txtSearch.Text) Then
            filter = String.Format("(IssueSubject LIKE '%{0}%')", txtSearch.Text)
        End If

        If cmbStatusFilter.SelectedIndex > 0 Then
            If filter.Length > 0 Then filter &= " AND "
            filter &= String.Format("Status = '{0}'", cmbStatusFilter.SelectedItem.ToString())
        End If

        Try
            view.RowFilter = filter
        Catch
            view.RowFilter = ""
        End Try

        dgvTickets.DataSource = view
        UpdateStats(view)
    End Sub

    Private Sub UpdateStats(view As DataView)
        Dim total As Integer = view.Count
        Dim pending As Integer = 0
        Dim resolved As Integer = 0

        For Each row As DataRowView In view
            If row("Status").ToString() = "Pending" Then pending += 1
            If row("Status").ToString() = "Resolved" Then resolved += 1
        Next

        lblTileTotalVal.Text = total.ToString()
        lblTilePendingVal.Text = pending.ToString()
        lblTileResolvedVal.Text = resolved.ToString()

        AnimationProgress = 0.0F
        AnimationTimer.Start()
    End Sub

    ' --- CHARTING ---
    Private Sub LoadAnalyticsData()
        CategoryCounts.Clear()
        For Each row As DataRow In dtTickets.Rows
            Dim cat As String = row("Category").ToString()
            If CategoryCounts.ContainsKey(cat) Then
                CategoryCounts(cat) += 1
            Else
                CategoryCounts.Add(cat, 1)
            End If
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

    Private Sub picGraph_Paint(sender As Object, e As PaintEventArgs) Handles picGraph.Paint
        Dim g = e.Graphics
        g.SmoothingMode = SmoothingMode.AntiAlias
        g.Clear(Color.White)

        If CategoryCounts.Count = 0 Then Return

        Dim w As Integer = picGraph.Width
        Dim h As Integer = picGraph.Height
        Dim maxVal As Integer = 0

        For Each v In CategoryCounts.Values
            If v > maxVal Then maxVal = v
        Next

        If maxVal = 0 Then maxVal = 5

        Dim count As Integer = CategoryCounts.Count
        Dim barWidth As Integer = 60
        Dim spacing As Integer = CInt((w - (count * barWidth)) / (count + 1))
        Dim idx As Integer = 0

        For Each kvp In CategoryCounts
            ' Calculate dimensions using Singles then convert to Integer for drawing
            Dim rawHeight As Double = (kvp.Value / maxVal) * (h - 100) * AnimationProgress
            Dim barH As Integer = CInt(rawHeight)

            Dim x As Integer = spacing + idx * (barWidth + spacing)
            Dim y As Integer = h - 50 - barH

            ' Fix: Convert numbers to Integer/Single for FillRectangle
            g.FillRectangle(Brushes.DodgerBlue, x, y, barWidth, barH)

            ' Fix: Convert numbers to Single for DrawString
            g.DrawString(kvp.Value.ToString(), Me.Font, Brushes.Black, CSng(x + 20), CSng(y - 20))
            g.DrawString(kvp.Key, Me.Font, Brushes.DimGray, CSng(x), CSng(h - 40))

            idx += 1
        Next
    End Sub

    ' --- LAYOUT & EVENTS ---
    Private Sub ApplyStudentLayout()
        btnNavDashboard.Visible = False
        btnNavUsers.Visible = False
        btnExport.Visible = False
        btnDelete.Visible = False
        dgvTickets.Visible = False
        flpHistory.Visible = True
        btnAction.Text = "➕ New Concern"
        btnAction.BackColor = Color.DodgerBlue
    End Sub

    Private Sub ApplyAdminLayout()
        btnNavDashboard.Visible = True
        btnExport.Visible = True
        btnDelete.Visible = True
        dgvTickets.Visible = True
        flpHistory.Visible = False
        btnAction.Text = "✅ Mark Resolved"
        btnAction.BackColor = Color.SeaGreen
    End Sub

    Private Sub btnRefresh_Click(s As Object, e As EventArgs) Handles btnRefresh.Click
        LoadTickets()
    End Sub

    Private Sub txtSearch_TextChanged(s As Object, e As EventArgs) Handles txtSearch.TextChanged
        FilterData()
    End Sub

    Private Sub cmbStatusFilter_SelectedIndexChanged(s As Object, e As EventArgs) Handles cmbStatusFilter.SelectedIndexChanged
        FilterData()
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        MessageBox.Show("Export feature placeholder")
    End Sub

    ' --- DIALOG HELPERS ---
    Private Function CustomInputBox(prompt As String, title As String) As String
        Dim f As New Form With {.Width = 400, .Height = 150, .Text = title, .StartPosition = FormStartPosition.CenterParent}
        Dim t As New TextBox With {.Left = 20, .Top = 50, .Width = 340, .Parent = f}
        Dim b As New Button With {.Text = "OK", .Left = 260, .Top = 80, .DialogResult = DialogResult.OK, .Parent = f}

        Dim lbl As New Label With {.Text = prompt, .Left = 20, .Top = 20, .AutoSize = True}
        f.Controls.Add(lbl)

        f.AcceptButton = b

        If f.ShowDialog() = DialogResult.OK Then
            Return t.Text
        Else
            Return ""
        End If
    End Function

    Private Function ShowReportDialog(ByRef category As String, ByRef priority As String, ByRef issue As String) As Boolean
        Dim f As New Form With {.Width = 400, .Height = 400, .Text = "New Concern", .StartPosition = FormStartPosition.CenterParent}

        Dim c1 As New ComboBox With {.Left = 20, .Top = 40, .Width = 340, .DropDownStyle = ComboBoxStyle.DropDownList, .Parent = f}
        c1.Items.AddRange({"Hardware", "Software", "Network", "Other"})
        c1.SelectedIndex = 0

        Dim c2 As New ComboBox With {.Left = 20, .Top = 100, .Width = 340, .DropDownStyle = ComboBoxStyle.DropDownList, .Parent = f}
        c2.Items.AddRange({"Low", "Medium", "High"})
        c2.SelectedIndex = 1

        Dim t1 As New TextBox With {.Left = 20, .Top = 160, .Width = 340, .Height = 100, .Multiline = True, .Parent = f}
        Dim btn As New Button With {.Text = "Submit", .Left = 260, .Top = 280, .DialogResult = DialogResult.OK, .Parent = f}

        ' Create labels separately and add them
        Dim l1 As New Label With {.Text = "Category", .Top = 20, .Left = 20}
        Dim l2 As New Label With {.Text = "Priority", .Top = 80, .Left = 20}
        Dim l3 As New Label With {.Text = "Description", .Top = 140, .Left = 20}

        f.Controls.Add(l1)
        f.Controls.Add(l2)
        f.Controls.Add(l3)

        If f.ShowDialog() = DialogResult.OK Then
            category = c1.SelectedItem.ToString()
            priority = c2.SelectedItem.ToString()
            issue = t1.Text
            Return True
        End If
        Return False
    End Function

End Class