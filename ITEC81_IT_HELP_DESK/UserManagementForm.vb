Imports System.Collections.Specialized.BitVector32
Imports System.Data.OleDb
Imports System.Drawing
Imports System.Windows.Forms

Public Class UserManagementForm
    Inherits Form

    ' --- UI CONTROLS with EVENTS ---
    Private WithEvents dgvUsers As New DataGridView
    Private WithEvents btnDelete As New Button
    Private WithEvents btnClose As New Button
    Private WithEvents btnEdit As New Button

    Private ReadOnly Property ConnString As String
        Get
            Dim dbPath As String = IO.Path.Combine(Application.StartupPath, "ITHelpDeskDB.accdb")
            Return "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & dbPath & ";"
        End Get
    End Property

    Private Sub UserManagementForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Manage Users"
        Me.Size = New Size(600, 450)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.BackColor = Color.WhiteSmoke

        ' Grid
        dgvUsers.Parent = Me
        dgvUsers.Location = New Point(12, 12)
        dgvUsers.Size = New Size(560, 330)
        dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvUsers.MultiSelect = False
        dgvUsers.AllowUserToAddRows = False
        dgvUsers.ReadOnly = True
        dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvUsers.BackgroundColor = Color.White

        ' Delete Button
        btnDelete.Parent = Me
        btnDelete.Text = "Delete User"
        btnDelete.Location = New Point(12, 360)
        btnDelete.Size = New Size(150, 40)
        btnDelete.BackColor = Color.IndianRed
        btnDelete.ForeColor = Color.White
        btnDelete.FlatStyle = FlatStyle.Flat

        btnEdit.Parent = Me
        btnEdit.Text = "Edit Role"
        btnEdit.Location = New Point(170, 360) ' Placed next to Delete button
        btnEdit.Size = New Size(150, 40)
        btnEdit.BackColor = Color.DodgerBlue
        btnEdit.ForeColor = Color.White
        btnEdit.FlatStyle = FlatStyle.Flat

        ' Close Button
        btnClose.Parent = Me
        btnClose.Text = "Close"
        btnClose.Location = New Point(420, 360)
        btnClose.Size = New Size(150, 40)
        btnClose.FlatStyle = FlatStyle.Flat

        LoadUsers()
    End Sub

    Private Sub LoadUsers()
        Using conn As New OleDbConnection(ConnString)
            Try
                conn.Open()
                Dim da As New OleDbDataAdapter("SELECT UserID, Username, Role FROM tblUsers", conn)
                Dim dt As New DataTable()
                da.Fill(dt)
                dgvUsers.DataSource = dt
            Catch ex As Exception
                MessageBox.Show("Error loading users: " & ex.Message)
            End Try
        End Using
    End Sub

    ' --- DELETE LOGIC (FIXED) ---
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        ' 1. Robust Selection Check
        If dgvUsers.CurrentRow Is Nothing OrElse dgvUsers.SelectedRows.Count = 0 Then
            MessageBox.Show("Please click on a user row to select it.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' 2. Get Data
        Dim userID As Integer = Convert.ToInt32(dgvUsers.CurrentRow.Cells("UserID").Value)
        Dim userName As String = dgvUsers.CurrentRow.Cells("Username").Value.ToString()

        ' 3. Confirmation
        Dim result As DialogResult = MessageBox.Show("Are you sure you want to delete the user '" & userName & "'?" & vbCrLf &
                                                     "This action cannot be undone.",
                                                     "Confirm Delete",
                                                     MessageBoxButtons.YesNo,
                                                     MessageBoxIcon.Warning)

        If result = DialogResult.Yes Then
            Using conn As New OleDbConnection(ConnString)
                Try
                    conn.Open()
                    Dim cmd As New OleDbCommand("DELETE FROM tblUsers WHERE UserID = ?", conn)
                    cmd.Parameters.AddWithValue("?", userID)
                    cmd.ExecuteNonQuery()

                    MessageBox.Show("User '" & userName & "' deleted successfully.")
                    LoadUsers() ' Refresh grid
                Catch ex As Exception
                    MessageBox.Show("Database Error: " & ex.Message)
                End Try
            End Using
        End If
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs)
        If dgvUsers.SelectedRows.Count = 0 Then Return

        Dim userId As Integer = Convert.ToInt32(dgvUsers.CurrentRow.Cells("UserID").Value)
        Dim currentRole As String = dgvUsers.CurrentRow.Cells("Role").Value.ToString()

        ' Simple toggle for now, or create a small dialog to select role
        Dim newRole As String = If(currentRole = "Student", "Admin", "Student")

        Using conn As New OleDbConnection(Session.ConnectionString)
            conn.Open()
            Dim cmd As New OleDbCommand("UPDATE tblUsers SET Role = ? WHERE UserID = ?", conn)
            cmd.Parameters.AddWithValue("?", newRole)
            cmd.Parameters.AddWithValue("?", userId)
            cmd.ExecuteNonQuery()
        End Using

        MessageBox.Show($"User role updated to {newRole}")
        LoadUsers()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

End Class