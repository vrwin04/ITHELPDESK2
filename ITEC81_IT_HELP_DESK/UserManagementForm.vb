Imports System.Data.OleDb

Public Class UserManagementForm

    Private Sub UserManagementForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadUsers()
    End Sub

    Private Sub LoadUsers()
        Using conn As New OleDbConnection(Session.connString)
            Try
                conn.Open()
                Dim da As New OleDbDataAdapter("SELECT UserID, Username, Role FROM tblUsers", conn)
                Dim dt As New DataTable()
                da.Fill(dt)
                dgvUsers.DataSource = dt
            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If dgvUsers.CurrentRow Is Nothing Then Return
        Dim userID As Integer = Convert.ToInt32(dgvUsers.CurrentRow.Cells("UserID").Value)

        If userID = Session.CurrentUserID Then
            MessageBox.Show("Cannot delete yourself.")
            Return
        End If

        If MessageBox.Show("Delete user?", "Confirm", MessageBoxButtons.YesNo) = DialogResult.Yes Then
            Using conn As New OleDbConnection(Session.connString)
                Dim cmd As New OleDbCommand("DELETE FROM tblUsers WHERE UserID=?", conn)
                cmd.Parameters.AddWithValue("?", userID)
                conn.Open()
                cmd.ExecuteNonQuery()
            End Using
            LoadUsers()
        End If
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        If dgvUsers.CurrentRow Is Nothing Then Return
        Dim userID As Integer = Convert.ToInt32(dgvUsers.CurrentRow.Cells("UserID").Value)
        Dim currentRole As String = dgvUsers.CurrentRow.Cells("Role").Value.ToString()
        Dim newRole As String = If(currentRole = "Student", "Admin", "Student")

        Using conn As New OleDbConnection(Session.connString)
            Dim cmd As New OleDbCommand("UPDATE tblUsers SET Role=? WHERE UserID=?", conn)
            cmd.Parameters.AddWithValue("?", newRole)
            cmd.Parameters.AddWithValue("?", userID)
            conn.Open()
            cmd.ExecuteNonQuery()
        End Using
        LoadUsers()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
End Class