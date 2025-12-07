Imports System.Data.OleDb ' Required for MS Access connection

Public Class Form1
    ' Connection String (Update path if necessary)
    Dim connString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\verwi\source\repos\ITHELPDESK2\ITEC81_IT_HELP_DESK\bin\Debug\net8.0-windows\ITHelpDeskDB.accdb;"

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Dim conn As New OleDbConnection(connString)
        Dim cmd As New OleDbCommand("SELECT COUNT(*) FROM tblUsers WHERE Username=? AND Password=?", conn)

        ' Parameterized query to prevent SQL Injection
        cmd.Parameters.AddWithValue("?", txtUsername.Text)
        cmd.Parameters.AddWithValue("?", txtPassword.Text)

        Try
            conn.Open()
            Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())

            If count > 0 Then
                MessageBox.Show("Login Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Dim dash As New DashboardForm()
                dash.Show()
                Me.Hide()
            Else
                MessageBox.Show("Invalid Username or Password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            conn.Close()
        End Try
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Application.Exit()
    End Sub
End Class