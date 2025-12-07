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
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.lblUser = New System.Windows.Forms.Label()
        Me.txtUsername = New System.Windows.Forms.TextBox()
        Me.lblPass = New System.Windows.Forms.Label()
        Me.txtPassword = New System.Windows.Forms.TextBox()
        Me.btnLogin = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.pnlLoginBox = New System.Windows.Forms.Panel()
        Me.lblBrand = New System.Windows.Forms.Label()
        Me.pnlLoginBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlLoginBox
        '
        Me.pnlLoginBox.BackColor = System.Drawing.Color.White
        Me.pnlLoginBox.Controls.Add(Me.lblTitle)
        Me.pnlLoginBox.Controls.Add(Me.btnExit)
        Me.pnlLoginBox.Controls.Add(Me.lblUser)
        Me.pnlLoginBox.Controls.Add(Me.btnLogin)
        Me.pnlLoginBox.Controls.Add(Me.txtUsername)
        Me.pnlLoginBox.Controls.Add(Me.txtPassword)
        Me.pnlLoginBox.Controls.Add(Me.lblPass)
        Me.pnlLoginBox.Location = New System.Drawing.Point(40, 60)
        Me.pnlLoginBox.Name = "pnlLoginBox"
        Me.pnlLoginBox.Size = New System.Drawing.Size(300, 320)
        Me.pnlLoginBox.TabIndex = 7
        '
        'lblBrand
        '
        Me.lblBrand.AutoSize = True
        Me.lblBrand.Font = New System.Drawing.Font("Segoe UI", 16.0!, System.Drawing.FontStyle.Bold)
        Me.lblBrand.ForeColor = System.Drawing.Color.White
        Me.lblBrand.Location = New System.Drawing.Point(100, 20)
        Me.lblBrand.Name = "lblBrand"
        Me.lblBrand.Size = New System.Drawing.Size(180, 30)
        Me.lblBrand.TabIndex = 8
        Me.lblBrand.Text = "HELPDESK FOR STUDENTS"
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Bold)
        Me.lblTitle.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64)
        Me.lblTitle.Location = New System.Drawing.Point(115, 20)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(71, 25)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "LOGIN"
        '
        'lblUser
        '
        Me.lblUser.AutoSize = True
        Me.lblUser.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblUser.ForeColor = System.Drawing.Color.DimGray
        Me.lblUser.Location = New System.Drawing.Point(30, 70)
        Me.lblUser.Name = "lblUser"
        Me.lblUser.Size = New System.Drawing.Size(63, 15)
        Me.lblUser.TabIndex = 1
        Me.lblUser.Text = "Username:"
        '
        'txtUsername
        '
        Me.txtUsername.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.txtUsername.Location = New System.Drawing.Point(33, 90)
        Me.txtUsername.Name = "txtUsername"
        Me.txtUsername.Size = New System.Drawing.Size(235, 25)
        Me.txtUsername.TabIndex = 2
        '
        'lblPass
        '
        Me.lblPass.AutoSize = True
        Me.lblPass.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblPass.ForeColor = System.Drawing.Color.DimGray
        Me.lblPass.Location = New System.Drawing.Point(30, 130)
        Me.lblPass.Name = "lblPass"
        Me.lblPass.Size = New System.Drawing.Size(60, 15)
        Me.lblPass.TabIndex = 3
        Me.lblPass.Text = "Password:"
        '
        'txtPassword
        '
        Me.txtPassword.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.txtPassword.Location = New System.Drawing.Point(33, 150)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = "*"c
        Me.txtPassword.Size = New System.Drawing.Size(235, 25)
        Me.txtPassword.TabIndex = 4
        '
        'btnLogin
        '
        Me.btnLogin.BackColor = System.Drawing.Color.DodgerBlue
        Me.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLogin.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.btnLogin.ForeColor = System.Drawing.Color.White
        Me.btnLogin.Location = New System.Drawing.Point(33, 210)
        Me.btnLogin.Name = "btnLogin"
        Me.btnLogin.Size = New System.Drawing.Size(235, 40)
        Me.btnLogin.TabIndex = 5
        Me.btnLogin.Text = "Sign In"
        Me.btnLogin.UseVisualStyleBackColor = False
        '
        'btnExit
        '
        Me.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExit.ForeColor = System.Drawing.Color.IndianRed
        Me.btnExit.Location = New System.Drawing.Point(33, 260)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(235, 30)
        Me.btnExit.TabIndex = 6
        Me.btnExit.Text = "Exit System"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(20, 25, 45)
        Me.ClientSize = New System.Drawing.Size(380, 420)
        Me.Controls.Add(Me.lblBrand)
        Me.Controls.Add(Me.pnlLoginBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Login"
        Me.pnlLoginBox.ResumeLayout(False)
        Me.pnlLoginBox.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

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
End Class