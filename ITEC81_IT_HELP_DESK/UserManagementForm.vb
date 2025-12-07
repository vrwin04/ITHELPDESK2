Imports System.Data.OleDb

Public Class UserManagementForm

    ' --- LOAD ---
    Private Sub UserManagementForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' UI setup is now in Designer.vb
        LoadUsers()
    End Sub

    Private Sub LoadUsers()
        Using conn As New OleDbConnection(Session.ConnectionString)
            Try
                conn.Open()
                ' Fetching basic user data
                Dim da As New OleDbDataAdapter("SELECT UserID, Username, Role FROM tblUsers", conn)
                Dim dt As New DataTable()
                da.Fill(dt)
                dgvUsers.DataSource = dt
            Catch ex As Exception
                MessageBox.Show("Error loading users: " & ex.Message)
            End Try
        End Using
    End Sub

    ' --- DELETE LOGIC ---
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If dgvUsers.CurrentRow Is Nothing OrElse dgvUsers.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a user to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim userID As Integer = Convert.ToInt32(dgvUsers.CurrentRow.Cells("UserID").Value)
        Dim userName As String = dgvUsers.CurrentRow.Cells("Username").Value.ToString()

        ' Prevent deleting yourself
        If userID = Session.CurrentUserID Then
            MessageBox.Show("You cannot delete your own account while logged in.", "Action Denied", MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Return
        End If

        If MessageBox.Show("Are you sure you want to delete '" & userName & "'?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
            Using conn As New OleDbConnection(Session.ConnectionString)
                Try
                    conn.Open()
                    Dim cmd As New OleDbCommand("DELETE FROM tblUsers WHERE UserID = ?", conn)
                    cmd.Parameters.AddWithValue("?", userID)
                    cmd.ExecuteNonQuery()

                    MessageBox.Show("User deleted successfully.")
                    LoadUsers()
                Catch ex As Exception
                    MessageBox.Show("Database Error: " & ex.Message)
                End Try
            End Using
        End If
    End Sub

    ' --- EDIT ROLE LOGIC ---
    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        If dgvUsers.SelectedRows.Count = 0 Then Return

        Dim userId As Integer = Convert.ToInt32(dgvUsers.CurrentRow.Cells("UserID").Value)
        Dim currentRole As String = dgvUsers.CurrentRow.Cells("Role").Value.ToString()

        ' Simple Toggle: Student <-> Admin
        Dim newRole As String = If(currentRole = "Student", "Admin", "Student")

        Using conn As New OleDbConnection(Session.ConnectionString)
            Try
                conn.Open()
                Dim cmd As New OleDbCommand("UPDATE tblUsers SET Role = ? WHERE UserID = ?", conn)
                cmd.Parameters.AddWithValue("?", newRole)
                cmd.Parameters.AddWithValue("?", userId)
                cmd.ExecuteNonQuery()

                MessageBox.Show($"User role updated to {newRole}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadUsers()
            Catch ex As Exception
                MessageBox.Show("Update Error: " & ex.Message)
            End Try
        End Using
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

End Class