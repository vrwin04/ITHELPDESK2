Imports System.Data.OleDb

Public Class frmRegister

    ' --- EVENTS ---
    Private Sub btnRegister_Click(sender As Object, e As EventArgs) Handles btnRegister.Click
        ' 1. Validation
        If txtUsername.Text = "" Or txtPassword.Text = "" Or txtConfirmPass.Text = "" Then
            MessageBox.Show("Please fill in all fields.", "Missing Info", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If txtPassword.Text <> txtConfirmPass.Text Then
            MessageBox.Show("Passwords do not match!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' 2. Database Insertion
        Using conn As New OleDbConnection(Session.connString)
            ' Check if user already exists
            Dim checkSql As String = "SELECT COUNT(*) FROM tblUsers WHERE [Username] = ?"
            Dim insertSql As String = "INSERT INTO tblUsers ([Username], [Password], [Role]) VALUES (?, ?, ?)"

            Try
                conn.Open()

                ' Check Duplicate
                Using checkCmd As New OleDbCommand(checkSql, conn)
                    checkCmd.Parameters.AddWithValue("?", txtUsername.Text)
                    Dim count As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())
                    If count > 0 Then
                        MessageBox.Show("Username already taken. Please choose another.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return
                    End If
                End Using

                ' Insert New User
                Using cmd As New OleDbCommand(insertSql, conn)
                    cmd.Parameters.AddWithValue("?", txtUsername.Text)
                    cmd.Parameters.AddWithValue("?", txtPassword.Text) ' Ideally, use Session.HashPassword here if enabled
                    cmd.Parameters.AddWithValue("?", "Student") ' Default role

                    cmd.ExecuteNonQuery()
                    MessageBox.Show("Registration Successful! You can now login.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Me.Close()
                End Using

            Catch ex As Exception
                MessageBox.Show("Database Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
End Class