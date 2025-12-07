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
        ' NOTE: We no longer call SetupLayout() or Me.Controls.Clear() 
        ' because the Designer handles the UI now.

        Me.DoubleBuffered = True

        ' Load initial data
        LoadTickets()

        ' Apply role-specific visibility
        If Session.CurrentUserRole = "Student" Then
            ApplyStudentLayout()
            ' Default view for students
            ShowView("Tickets")
        Else
            ApplyAdminLayout()
            ' Default view for admins
            ShowView("Dashboard")
        End If
    End Sub

    ' --- NAVIGATION LOGIC ---
    Private Sub ShowView(view As String)
        ' Toggles visibility based on the Designer panel names
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
        ' Return to Login screen
        Dim login As New Form1()
        login.Show()
        Me.Close()
    End Sub

    ' --- DATA LOADING LOGIC ---
    Private Sub LoadTickets()
        Dim sql As String
        Dim params As New List(Of Object)

        ' Different queries based on Role
        If Session.CurrentUserRole = "Student" Then
            sql = "SELECT [TicketID], [Category], [Priority], [IssueSubject], [Status], [AdminRemarks], [DateSubmitted] FROM tblTickets WHERE [SubmittedBy] = ? ORDER BY [DateSubmitted] DESC"
            params.Add(Session.CurrentUserID)
        Else
            sql = "SELECT t.TicketID, u.Username AS [ReportedBy], t.Category, t.Priority, t.IssueSubject, t.Status, t.AdminRemarks, t.DateSubmitted FROM tblTickets t INNER JOIN tblUsers u ON t.SubmittedBy = u.UserID ORDER BY t.DateSubmitted DESC"
        End If

        ' Fetch data using Session helper
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

    ' --- STUDENT VIEW: CARD RENDERING ---
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
            pnl.Height = 110
            pnl.BackColor = Color.White
            pnl.Margin = New Padding(0, 0, 0, 10)
            pnl.Padding = New Padding(15)
            pnl.BorderStyle = BorderStyle.FixedSingle

            Dim status As String = row("Status").ToString()
            Dim statusColor As Color = If(status = "Resolved", Color.SeaGreen, If(status = "In Progress", Color.DodgerBlue, Color.Orange))

            ' Status Color Bar
            Dim pnlStatus As New Panel
            pnlStatus.Width = 5
            pnlStatus.Dock = DockStyle.Left
            pnlStatus.BackColor = statusColor
            pnl.Controls.Add(pnlStatus)

            ' Header Info
            Dim lblHead As New Label
            lblHead.Text = Convert.ToDateTime(row("DateSubmitted")).ToString("MMM dd, yyyy") & "  •  " & row("Category").ToString() & "  •  " & row("Priority").ToString()
            lblHead.Font = New Font("Segoe UI", 9, FontStyle.Bold)
            lblHead.ForeColor = Color.Gray
            lblHead.Location = New Point(20, 10)
            lblHead.AutoSize = True
            pnl.Controls.Add(lblHead)

            ' Subject
            Dim lblBody As New Label
            lblBody.Text = row("IssueSubject").ToString()
            lblBody.Font = New Font("Segoe UI", 12, FontStyle.Bold)
            lblBody.Location = New Point(20, 35)
            lblBody.Size = New Size(pnl.Width - 40, 25)
            lblBody.AutoEllipsis = True
            pnl.Controls.Add(lblBody)

            ' Footer Status
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

    ' --- ADMIN VIEW: FILTERING & STATS ---
    Private Sub FilterData()
        If dtTickets Is Nothing Then Return
        Dim view As DataView = dtTickets.DefaultView
        Dim filter As String = ""

        ' Search logic
        If Not String.IsNullOrEmpty(txtSearch.Text) AndAlso dtTickets.Columns.Contains("IssueSubject") Then
            filter = String.Format("(IssueSubject LIKE '%{0}%' OR Category LIKE '%{0}%')", txtSearch.Text.Replace("'", "''"))
        End If

        ' Status Dropdown logic
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

        ' Update Designer Labels
        lblTileTotalVal.Text = total.ToString()
        lblTilePendingVal.Text = pending.ToString()
        lblTileResolvedVal.Text = resolved.ToString()

        ' Reset and start Graph animation
        AnimationProgress = 0
        AnimationTimer.Start()
    End Sub

    ' --- CHARTING LOGIC ---
    Private Sub LoadAnalyticsData()
        CategoryCounts.Clear()
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

    Private Sub picGraph_Paint(sender As Object, e As PaintEventArgs) Handles picGraph.Paint
        Dim g = e.Graphics
        g.SmoothingMode = SmoothingMode.AntiAlias
        g.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit

        g.Clear(Color.White)

        If CategoryCounts.Count = 0 Then
            Dim msg = "No Ticket Data Available for Charting"
            Dim sf As New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
            g.DrawString(msg, New Font("Segoe UI", 12), Brushes.Gray, picGraph.ClientRectangle, sf)
            Return
        End If

        ' Chart Drawing Logic
        Dim w As Integer = picGraph.Width
        Dim h As Integer = picGraph.Height
        Dim paddingBottom As Integer = 60
        Dim paddingTop As Integer = 40
        Dim graphHeight As Integer = h - paddingBottom - paddingTop

        Dim maxVal As Integer = 0
        For Each v In CategoryCounts.Values
            If v > maxVal Then maxVal = v
        Next
        Dim topScale As Integer = If(maxVal = 0, 5, maxVal)

        ' Draw Grid
        Dim penGrid As New Pen(Color.FromArgb(240, 240, 240), 1)
        For i As Integer = 0 To 4
            Dim y As Integer = paddingTop + CInt((graphHeight / 4) * i)
            g.DrawLine(penGrid, 20, y, w - 20, y)
        Next

        ' Draw Bars
        Dim count As Integer = CategoryCounts.Count
        Dim slotWidth As Integer = CInt((w - 40) / count)
        Dim barWidth As Integer = Math.Min(80, CInt(slotWidth * 0.6))
        Dim startX As Integer = 20
        Dim idx As Integer = 0

        Dim colors() As Color = {Color.FromArgb(52, 152, 219), Color.FromArgb(46, 204, 113), Color.FromArgb(243, 156, 18), Color.FromArgb(231, 76, 60)}
        Dim lightColors() As Color = {Color.FromArgb(100, 180, 255), Color.FromArgb(100, 255, 170), Color.FromArgb(255, 200, 100), Color.FromArgb(255, 120, 100)}

        For Each kvp In CategoryCounts
            Dim val As Integer = kvp.Value
            Dim barH As Integer = CInt((val / topScale) * graphHeight * AnimationProgress)
            Dim x As Integer = startX + (idx * slotWidth) + (slotWidth - barWidth) \ 2
            Dim y As Integer = (h - paddingBottom) - barH
            Dim rect As New Rectangle(x, y, barWidth, barH)

            If barH > 0 Then
                Using br As New LinearGradientBrush(rect, lightColors(idx Mod colors.Length), colors(idx Mod colors.Length), LinearGradientMode.Vertical)
                    g.FillRectangle(br, rect)
                End Using
            End If

            ' Value Label
            Dim valStr = val.ToString()
            Dim fontVal As New Font("Segoe UI", 10, FontStyle.Bold)
            Dim szVal = g.MeasureString(valStr, fontVal)
            g.DrawString(valStr, fontVal, Brushes.Black, CSng(x + (barWidth - szVal.Width) / 2), CSng(y - 20))

            ' Category Label
            Dim catStr = kvp.Key
            If catStr.Length > 10 Then catStr = catStr.Substring(0, 8) & ".."
            Dim fontCat As New Font("Segoe UI", 9, FontStyle.Regular)
            Dim szCat = g.MeasureString(catStr, fontCat)
            g.DrawString(catStr, fontCat, Brushes.DimGray, CSng(x + (barWidth - szCat.Width) / 2), CSng(h - paddingBottom + 5))

            idx += 1
        Next
    End Sub

    ' --- LAYOUT HELPERS ---
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

    ' --- BUTTON ACTIONS ---
    Private Sub btnAction_Click(sender As Object, e As EventArgs) Handles btnAction.Click
        If Session.CurrentUserRole = "Student" Then
            ' Student: Create New Ticket
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
            ' Admin: Mark Resolved
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
        Dim sfd As New SaveFileDialog With {.Filter = "CSV|*.csv", .FileName = "Report_" & DateTime.Now.ToString("yyyyMMdd") & ".csv"}
        If sfd.ShowDialog() = DialogResult.OK Then
            Dim sb As New StringBuilder
            For Each col As DataColumn In dtTickets.Columns
                sb.Append("""" & col.ColumnName.Replace("""", """""") & """,")
            Next
            sb.Remove(sb.Length - 1, 1)
            sb.AppendLine()

            For Each row As DataRow In dtTickets.Rows
                For Each item In row.ItemArray
                    sb.Append("""" & item.ToString().Replace("""", """""") & """,")
                Next
                sb.Remove(sb.Length - 1, 1)
                sb.AppendLine()
            Next
            File.WriteAllText(sfd.FileName, sb.ToString())
            MessageBox.Show("Export Complete: " & sfd.FileName)
        End If
    End Sub

    ' --- DIALOG HELPERS ---
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

        If f.ShowDialog() = DialogResult.OK Then
            category = c1.SelectedItem.ToString()
            priority = c2.SelectedItem.ToString()
            issue = t1.Text
            Return True
        End If
        Return False
    End Function

End Class