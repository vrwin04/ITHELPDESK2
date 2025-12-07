Imports System.Data.OleDb

Public Class Form1

    ' --- APP STARTUP CHECK ---
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' This checks for the file AND the drivers immediately
        If Not Session.VerifyDatabase() Then
            Application.Exit()
        End If
    End Sub

    ' --- LOGIN LOGIC ---
    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Using conn As New OleDbConnection(Session.ConnectionString)
            Dim cmd As New OleDbCommand("SELECT UserID, Role FROM tblUsers WHERE [Username]=? AND [Password]=?", conn)

            ' Parameterized query
            cmd.Parameters.AddWithValue("?", txtUsername.Text)
            cmd.Parameters.AddWithValue("?", Session.HashPassword(txtPassword.Text))

            Try
                conn.Open()
                Dim reader As OleDbDataReader = cmd.ExecuteReader()

                If reader.Read() Then
                    ' Login Success
                    Session.CurrentUserID = Convert.ToInt32(reader("UserID"))
                    Session.CurrentUserRole = reader("Role").ToString()
                    Session.CurrentUserName = txtUsername.Text

                    MessageBox.Show("Login Successful!", "Welcome", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    Dim dash As New DashboardForm()
                    dash.Show()
                    Me.Hide()
                Else
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

    Private Sub Form1_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        Application.Exit()
    End Sub
End Class