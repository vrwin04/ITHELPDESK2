<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

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

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        lblTitle = New Label()
        lblUser = New Label()
        txtUsername = New TextBox()
        lblPass = New Label()
        txtPassword = New TextBox()
        btnLogin = New Button()
        pnlLoginBox = New Panel()
        registerBtn = New Button()
        lblBrand = New Label()
        closeBtn = New PictureBox()
        pnlLoginBox.SuspendLayout()
        CType(closeBtn, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' lblTitle
        ' 
        lblTitle.AutoSize = True
        lblTitle.Font = New Font("Segoe UI", 14.0F, FontStyle.Bold)
        lblTitle.ForeColor = Color.FromArgb(CByte(64), CByte(64), CByte(64))
        lblTitle.Location = New Point(131, 27)
        lblTitle.Name = "lblTitle"
        lblTitle.Size = New Size(87, 32)
        lblTitle.TabIndex = 0
        lblTitle.Text = "LOGIN"
        ' 
        ' lblUser
        ' 
        lblUser.AutoSize = True
        lblUser.Font = New Font("Segoe UI", 9.0F)
        lblUser.ForeColor = Color.DimGray
        lblUser.Location = New Point(34, 93)
        lblUser.Name = "lblUser"
        lblUser.Size = New Size(78, 20)
        lblUser.TabIndex = 1
        lblUser.Text = "Username:"
        ' 
        ' txtUsername
        ' 
        txtUsername.Font = New Font("Segoe UI", 10.0F)
        txtUsername.Location = New Point(38, 120)
        txtUsername.Margin = New Padding(3, 4, 3, 4)
        txtUsername.Name = "txtUsername"
        txtUsername.Size = New Size(268, 30)
        txtUsername.TabIndex = 2
        ' 
        ' lblPass
        ' 
        lblPass.AutoSize = True
        lblPass.Font = New Font("Segoe UI", 9.0F)
        lblPass.ForeColor = Color.DimGray
        lblPass.Location = New Point(34, 173)
        lblPass.Name = "lblPass"
        lblPass.Size = New Size(73, 20)
        lblPass.TabIndex = 3
        lblPass.Text = "Password:"
        ' 
        ' txtPassword
        ' 
        txtPassword.Font = New Font("Segoe UI", 10.0F)
        txtPassword.Location = New Point(38, 200)
        txtPassword.Margin = New Padding(3, 4, 3, 4)
        txtPassword.Name = "txtPassword"
        txtPassword.PasswordChar = "*"c
        txtPassword.Size = New Size(268, 30)
        txtPassword.TabIndex = 4
        ' 
        ' btnLogin
        ' 
        btnLogin.BackColor = Color.DodgerBlue
        btnLogin.FlatStyle = FlatStyle.Flat
        btnLogin.Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
        btnLogin.ForeColor = Color.White
        btnLogin.Location = New Point(38, 280)
        btnLogin.Margin = New Padding(3, 4, 3, 4)
        btnLogin.Name = "btnLogin"
        btnLogin.Size = New Size(269, 53)
        btnLogin.TabIndex = 5
        btnLogin.Text = "Sign In"
        btnLogin.UseVisualStyleBackColor = False
        ' 
        ' pnlLoginBox
        ' 
        pnlLoginBox.BackColor = Color.White
        pnlLoginBox.Controls.Add(registerBtn)
        pnlLoginBox.Controls.Add(lblTitle)
        pnlLoginBox.Controls.Add(lblUser)
        pnlLoginBox.Controls.Add(btnLogin)
        pnlLoginBox.Controls.Add(txtUsername)
        pnlLoginBox.Controls.Add(txtPassword)
        pnlLoginBox.Controls.Add(lblPass)
        pnlLoginBox.Location = New Point(46, 80)
        pnlLoginBox.Margin = New Padding(3, 4, 3, 4)
        pnlLoginBox.Name = "pnlLoginBox"
        pnlLoginBox.Size = New Size(343, 427)
        pnlLoginBox.TabIndex = 7
        ' 
        ' registerBtn
        ' 
        registerBtn.BackColor = Color.DodgerBlue
        registerBtn.FlatStyle = FlatStyle.Flat
        registerBtn.Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
        registerBtn.ForeColor = Color.White
        registerBtn.Location = New Point(38, 341)
        registerBtn.Margin = New Padding(3, 4, 3, 4)
        registerBtn.Name = "registerBtn"
        registerBtn.Size = New Size(269, 53)
        registerBtn.TabIndex = 6
        registerBtn.Text = "Register"
        registerBtn.UseVisualStyleBackColor = False
        ' 
        ' lblBrand
        ' 
        lblBrand.AutoSize = True
        lblBrand.Font = New Font("Segoe UI", 16.0F, FontStyle.Bold)
        lblBrand.ForeColor = Color.White
        lblBrand.Location = New Point(42, 32)
        lblBrand.Name = "lblBrand"
        lblBrand.Size = New Size(353, 37)
        lblBrand.TabIndex = 8
        lblBrand.Text = "HELPDESK FOR STUDENTS"
        ' 
        ' closeBtn
        ' 
        closeBtn.Image = CType(resources.GetObject("closeBtn.Image"), Image)
        closeBtn.Location = New Point(398, 3)
        closeBtn.Name = "closeBtn"
        closeBtn.Size = New Size(32, 32)
        closeBtn.SizeMode = PictureBoxSizeMode.AutoSize
        closeBtn.TabIndex = 9
        closeBtn.TabStop = False
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(8.0F, 20.0F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.FromArgb(CByte(20), CByte(25), CByte(45))
        ClientSize = New Size(434, 560)
        Controls.Add(closeBtn)
        Controls.Add(lblBrand)
        Controls.Add(pnlLoginBox)
        FormBorderStyle = FormBorderStyle.None
        Margin = New Padding(3, 4, 3, 4)
        Name = "Form1"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Login"
        pnlLoginBox.ResumeLayout(False)
        pnlLoginBox.PerformLayout()
        CType(closeBtn, ComponentModel.ISupportInitialize).EndInit()
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
    Friend WithEvents pnlLoginBox As System.Windows.Forms.Panel
    Friend WithEvents lblBrand As System.Windows.Forms.Label
    Friend WithEvents closeBtn As PictureBox
    Friend WithEvents Button1 As Button
    Friend WithEvents registerBtn As Button
End Class