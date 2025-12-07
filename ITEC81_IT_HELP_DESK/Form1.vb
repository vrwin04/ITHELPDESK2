Imports System.Data.OleDb

Public Class Form1

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        ' Use the connection string from your Session module
        Using conn As New OleDbConnection(Session.connString)
            ' Query using plain text password (since HashPassword function is gone)
            Dim query As String = "SELECT UserID, Role FROM tblUsers WHERE [Username]=? AND [Password]=?"
            Dim cmd As New OleDbCommand(query, conn)

            ' Add parameters
            cmd.Parameters.AddWithValue("?", txtUsername.Text)
            cmd.Parameters.AddWithValue("?", txtPassword.Text)

            Try
                conn.Open()
                Dim reader As OleDbDataReader = cmd.ExecuteReader()

                If reader.Read() Then
                    ' --- LOGIN SUCCESS ---
                    ' Save user info to Session variables
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
                    MessageBox.Show("Invalid Username or Password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If
            Catch ex As Exception
                MessageBox.Show("Database Error: " & ex.Message & vbCrLf & "Check if file path in Session.vb is correct.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Application.Exit()
    End Sub

    Private Sub Form1_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        Application.Exit()
    End Sub

    Private Sub close_click(sender As Object, e As EventArgs) Handles closeBtn.Click
        Me.Close()
    End Sub
End Class