<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        lblTitle = New Label()
        lblUser = New Label()
        txtUsername = New TextBox()
        lblPass = New Label()
        txtPassword = New TextBox()
        btnLogin = New Button()
        btnExit = New Button()
        SuspendLayout()
        ' 
        ' lblTitle
        ' 
        lblTitle.AutoSize = True
        lblTitle.Font = New Font("Segoe UI", 14.0F, FontStyle.Bold)
        lblTitle.Location = New Point(149, 30)
        lblTitle.Margin = New Padding(4, 0, 4, 0)
        lblTitle.Name = "lblTitle"
        lblTitle.Size = New Size(78, 32)
        lblTitle.TabIndex = 0
        lblTitle.Text = "Login"
        ' 
        ' lblUser
        ' 
        lblUser.AutoSize = True
        lblUser.Location = New Point(53, 108)
        lblUser.Margin = New Padding(4, 0, 4, 0)
        lblUser.Name = "lblUser"
        lblUser.Size = New Size(78, 20)
        lblUser.TabIndex = 1
        lblUser.Text = "Username:"
        ' 
        ' txtUsername
        ' 
        txtUsername.Location = New Point(57, 132)
        txtUsername.Margin = New Padding(4, 5, 4, 5)
        txtUsername.Name = "txtUsername"
        txtUsername.Size = New Size(265, 27)
        txtUsername.TabIndex = 2
        ' 
        ' lblPass
        ' 
        lblPass.AutoSize = True
        lblPass.Location = New Point(53, 185)
        lblPass.Margin = New Padding(4, 0, 4, 0)
        lblPass.Name = "lblPass"
        lblPass.Size = New Size(73, 20)
        lblPass.TabIndex = 3
        lblPass.Text = "Password:"
        ' 
        ' txtPassword
        ' 
        txtPassword.Location = New Point(57, 209)
        txtPassword.Margin = New Padding(4, 5, 4, 5)
        txtPassword.Name = "txtPassword"
        txtPassword.PasswordChar = "*"c
        txtPassword.Size = New Size(265, 27)
        txtPassword.TabIndex = 4
        ' 
        ' btnLogin
        ' 
        btnLogin.Location = New Point(57, 277)
        btnLogin.Margin = New Padding(4, 5, 4, 5)
        btnLogin.Name = "btnLogin"
        btnLogin.Size = New Size(120, 46)
        btnLogin.TabIndex = 5
        btnLogin.Text = "Login"
        btnLogin.UseVisualStyleBackColor = True
        ' 
        ' btnExit
        ' 
        btnExit.Location = New Point(204, 277)
        btnExit.Margin = New Padding(4, 5, 4, 5)
        btnExit.Name = "btnExit"
        btnExit.Size = New Size(120, 46)
        btnExit.TabIndex = 6
        btnExit.Text = "Exit"
        btnExit.UseVisualStyleBackColor = True
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(8.0F, 20.0F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(379, 402)
        Controls.Add(btnExit)
        Controls.Add(btnLogin)
        Controls.Add(txtPassword)
        Controls.Add(lblPass)
        Controls.Add(txtUsername)
        Controls.Add(lblUser)
        Controls.Add(lblTitle)
        Margin = New Padding(4, 5, 4, 5)
        Name = "Form1"
        StartPosition = FormStartPosition.CenterScreen
        Text = "IT Help Desk - Login"
        ResumeLayout(False)
        PerformLayout()

    End Sub

    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents lblUser As System.Windows.Forms.Label
    Friend WithEvents txtUsername As System.Windows.Forms.TextBox
    Friend WithEvents lblPass As System.Windows.Forms.Label
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents btnLogin As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
End Class