Imports System.Data.OleDb

Public Class Form1

    ' --- EVENTS ---
    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        ' Use the global connection string from Session.vb
        Using conn As New OleDbConnection(Session.ConnectionString)
            ' Query now checks for the hashed password
            Dim cmd As New OleDbCommand("SELECT UserID, Role FROM tblUsers WHERE Username=? AND [Password]=?", conn)

            cmd.Parameters.AddWithValue("?", txtUsername.Text)
            ' Hash the input password to match the database
            cmd.Parameters.AddWithValue("?", Session.HashPassword(txtPassword.Text))

            Try
                conn.Open()
                Dim reader As OleDbDataReader = cmd.ExecuteReader()

                If reader.Read() Then
                    ' --- LOGIN SUCCESS ---
                    ' Store user details in the global Session
                    Session.CurrentUserID = Convert.ToInt32(reader("UserID"))
                    Session.CurrentUserRole = reader("Role").ToString()
                    Session.CurrentUserName = txtUsername.Text

                    MessageBox.Show("Login Successful!", "Welcome", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    ' Open Dashboard
                    Dim dash As New DashboardForm()
                    dash.Show()
                    Me.Hide()
                Else
                    ' --- LOGIN FAILED ---
                    MessageBox.Show("Invalid Username or Password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            Catch ex As Exception
                MessageBox.Show("Database Error: " & ex.Message)
            End Try
        End Using
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Application.Exit()
    End Sub

    ' Close the entire app if the user closes the login form
    Private Sub Form1_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        Application.Exit()
    End Sub
End Class