Imports System.Data.OleDb
Imports System.IO

Public Class Form1

    ' --- VARIABLES ---
    Private IsRegisterMode As Boolean = False
    Private WithEvents lblSwitchMode As New LinkLabel

    ' --- SMART ASSET PATH FINDER ---
    Private ReadOnly Property AssetPath As String
        Get
            ' Start where the app is running (e.g., bin/Debug/net8.0-windows)
            Dim currentPath As String = Application.StartupPath

            ' Look for the "Assets" folder by going up the folder tree (up to 5 levels)
            For i As Integer = 0 To 5
                Dim checkPath As String = Path.Combine(currentPath, "Assets")
                If Directory.Exists(checkPath) Then Return checkPath

                ' Move up one level
                Dim parent As DirectoryInfo = Directory.GetParent(currentPath)
                If parent Is Nothing Then Exit For
                currentPath = parent.FullName
            Next

            ' If not found, default to a folder named Assets next to the exe
            Return Path.Combine(Application.StartupPath, "Assets")
        End Get
    End Property

    ' Database Connection
    Private ReadOnly Property ConnString As String
        Get
            Dim dbPath As String = Path.Combine(Application.StartupPath, "ITHelpDeskDB.accdb")
            Return "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & dbPath & ";"
        End Get
    End Property

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ApplyDesign()
        CreateAdminIfMissing()
    End Sub

    ' --- MODERN DESIGN LAYOUT ---
    Private Sub ApplyDesign()
        ' 1. Form Settings
        Me.Text = "IT Help Desk Portal"
        Me.BackColor = Color.White
        Me.FormBorderStyle = FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Size = New Size(400, 550)
        Me.StartPosition = FormStartPosition.CenterScreen

        ' 2. Main Header Title
        Dim lblTitle As New Label
        lblTitle.Text = "IT Help Desk"
        lblTitle.Font = New Font("Segoe UI", 24, FontStyle.Bold)
        lblTitle.ForeColor = Color.DodgerBlue
        lblTitle.AutoSize = True
        Me.Controls.Add(lblTitle)

        ' Center Title
        lblTitle.Location = New Point((Me.ClientSize.Width - lblTitle.Width) / 2, 60)

        ' 3. Calculate Center
        Dim centerX As Integer = (Me.ClientSize.Width - 250) / 2
        Dim startY As Integer = 150

        ' 4. Username Section with Icon
        AddIcon("user-24.png", centerX - 30, startY + 3)

        Label1.Text = "Username"
        Label1.Font = New Font("Segoe UI", 9, FontStyle.Bold)
        Label1.ForeColor = Color.DarkGray
        Label1.Location = New Point(centerX, startY)

        username_tb.Size = New Size(250, 30)
        username_tb.Font = New Font("Segoe UI", 11)
        username_tb.Location = New Point(centerX, startY + 25)

        ' 5. Password Section with Icon
        AddIcon("key-4-24.png", centerX - 30, startY + 80 + 3)

        Label2.Text = "Password"
        Label2.Font = New Font("Segoe UI", 9, FontStyle.Bold)
        Label2.ForeColor = Color.DarkGray
        Label2.Location = New Point(centerX, startY + 80)

        password_tb.Size = New Size(250, 30)
        password_tb.Font = New Font("Segoe UI", 11)
        password_tb.UseSystemPasswordChar = True
        password_tb.Location = New Point(centerX, startY + 105)

        ' 6. Action Button
        btn_login.Text = "LOG IN"
        btn_login.Size = New Size(250, 50)
        btn_login.FlatStyle = FlatStyle.Flat
        btn_login.BackColor = Color.DodgerBlue
        btn_login.ForeColor = Color.White
        btn_login.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        btn_login.FlatAppearance.BorderSize = 0
        btn_login.Cursor = Cursors.Hand
        btn_login.Location = New Point(centerX, startY + 170)

        ' 7. Switch Mode Link
        lblSwitchMode.Text = "Don't have an account? Create one."
        lblSwitchMode.Font = New Font("Segoe UI", 9, FontStyle.Regular)
        lblSwitchMode.LinkColor = Color.DimGray
        lblSwitchMode.ActiveLinkColor = Color.DodgerBlue
        lblSwitchMode.LinkBehavior = LinkBehavior.HoverUnderline
        lblSwitchMode.AutoSize = True
        lblSwitchMode.Cursor = Cursors.Hand
        Me.Controls.Add(lblSwitchMode)

        ' Hide old Sign In button
        btn_signin.Visible = False

        CenterLink()
    End Sub

    Private Sub AddIcon(fileName As String, x As Integer, y As Integer)
        Dim pb As New PictureBox
        pb.Size = New Size(24, 24)
        pb.SizeMode = PictureBoxSizeMode.StretchImage
        pb.Location = New Point(x, y)

        Dim fullPath As String = Path.Combine(AssetPath, fileName)

        If File.Exists(fullPath) Then
            pb.Image = Image.FromFile(fullPath)
        Else
            ' Visual fallback if image is still missing
            pb.BackColor = Color.Silver
        End If

        Me.Controls.Add(pb)
    End Sub

    Private Sub CenterLink()
        lblSwitchMode.Location = New Point((Me.ClientSize.Width - lblSwitchMode.Width) / 2, btn_login.Bottom + 25)
    End Sub

    ' --- LOGIC: SWITCH MODES ---
    Private Sub lblSwitchMode_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lblSwitchMode.LinkClicked
        IsRegisterMode = Not IsRegisterMode

        If IsRegisterMode Then
            btn_login.Text = "CREATE ACCOUNT"
            btn_login.BackColor = Color.SeaGreen
            lblSwitchMode.Text = "Already have an account? Log In."
        Else
            btn_login.Text = "LOG IN"
            btn_login.BackColor = Color.DodgerBlue
            lblSwitchMode.Text = "Don't have an account? Create one."
        End If
        CenterLink()
    End Sub

    ' --- LOGIC: BUTTON CLICK ---
    Private Sub btn_login_Click(sender As Object, e As EventArgs) Handles btn_login.Click
        If IsRegisterMode Then
            PerformRegister()
        Else
            PerformLogin()
        End If
    End Sub

    Private Sub PerformLogin()
        Dim user As String = username_tb.Text
        Dim pass As String = password_tb.Text

        If String.IsNullOrWhiteSpace(user) Or String.IsNullOrWhiteSpace(pass) Then
            MessageBox.Show("Please enter your Username and Password.")
            Return
        End If

        Using conn As New OleDbConnection(ConnString)
            Try
                conn.Open()
                Dim sql As String = "SELECT UserID, Role FROM tblUsers WHERE Username = ? AND Password = ?"
                Using cmd As New OleDbCommand(sql, conn)
                    cmd.Parameters.AddWithValue("?", user)
                    cmd.Parameters.AddWithValue("?", pass)

                    Dim reader As OleDbDataReader = cmd.ExecuteReader()

                    If reader.Read() Then
                        Session.CurrentUserID = reader("UserID")
                        Session.CurrentUserRole = reader("Role").ToString()
                        Session.CurrentUserName = user

                        Me.Hide()
                        Dim dash As New DashboardForm()
                        If dash.ShowDialog() = DialogResult.OK Then
                            Me.Show()
                            password_tb.Clear()
                            username_tb.Focus()
                        Else
                            Me.Close()
                        End If
                    Else
                        MessageBox.Show("Invalid Username or Password.")
                    End If
                End Using
            Catch ex As Exception
                MessageBox.Show("Database Error: " & ex.Message)
            End Try
        End Using
    End Sub

    Private Sub PerformRegister()
        Dim user As String = username_tb.Text
        Dim pass As String = password_tb.Text

        If String.IsNullOrWhiteSpace(user) Or String.IsNullOrWhiteSpace(pass) Then
            MessageBox.Show("Please enter a desired Username and Password.")
            Return
        End If

        Using conn As New OleDbConnection(ConnString)
            Try
                conn.Open()
                Dim checkSql As String = "SELECT COUNT(*) FROM tblUsers WHERE Username = ?"
                Using checkCmd As New OleDbCommand(checkSql, conn)
                    checkCmd.Parameters.AddWithValue("?", user)
                    If Convert.ToInt32(checkCmd.ExecuteScalar()) > 0 Then
                        MessageBox.Show("Username is already taken.")
                        Return
                    End If
                End Using

                Dim insertSql As String = "INSERT INTO tblUsers ([Username], [Password], [Role]) VALUES (?, ?, ?)"
                Using insertCmd As New OleDbCommand(insertSql, conn)
                    insertCmd.Parameters.AddWithValue("?", user)
                    insertCmd.Parameters.AddWithValue("?", pass)
                    insertCmd.Parameters.AddWithValue("?", "Student")
                    insertCmd.ExecuteNonQuery()
                    MessageBox.Show("Account created! Please Log In.")
                    lblSwitchMode_LinkClicked(Nothing, Nothing)
                End Using
            Catch ex As Exception
                MessageBox.Show("Error creating account: " & ex.Message)
            End Try
        End Using
    End Sub

    Private Sub CreateAdminIfMissing()
        Using conn As New OleDbConnection(ConnString)
            Try
                conn.Open()
                Dim checkSql As String = "SELECT COUNT(*) FROM tblUsers WHERE Username = 'admin'"
                Using checkCmd As New OleDbCommand(checkSql, conn)
                    If Convert.ToInt32(checkCmd.ExecuteScalar()) = 0 Then
                        Dim sql As String = "INSERT INTO tblUsers ([Username], [Password], [Role]) VALUES ('admin', 'admin123', 'Admin')"
                        Using cmd As New OleDbCommand(sql, conn)
                            cmd.ExecuteNonQuery()
                        End Using
                    End If
                End Using
            Catch ex As Exception
            End Try
        End Using
    End Sub
End Class