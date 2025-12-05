<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
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
        Label1 = New Label()
        username_tb = New TextBox()
        password_tb = New TextBox()
        Label2 = New Label()
        btn_login = New Button()
        btn_signin = New Button()
        SuspendLayout()
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(239, 149)
        Label1.Name = "Label1"
        Label1.Size = New Size(75, 20)
        Label1.TabIndex = 0
        Label1.Text = "Username"
        ' 
        ' username_tb
        ' 
        username_tb.Location = New Point(320, 146)
        username_tb.Name = "username_tb"
        username_tb.Size = New Size(125, 27)
        username_tb.TabIndex = 1
        ' 
        ' password_tb
        ' 
        password_tb.Location = New Point(320, 179)
        password_tb.Name = "password_tb"
        password_tb.PasswordChar = "*"c
        password_tb.Size = New Size(125, 27)
        password_tb.TabIndex = 3
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(239, 182)
        Label2.Name = "Label2"
        Label2.Size = New Size(70, 20)
        Label2.TabIndex = 2
        Label2.Text = "Password"
        ' 
        ' btn_login
        ' 
        btn_login.Location = New Point(333, 212)
        btn_login.Name = "btn_login"
        btn_login.Size = New Size(94, 29)
        btn_login.TabIndex = 4
        btn_login.Text = "Log In"
        btn_login.UseVisualStyleBackColor = True
        ' 
        ' btn_signin
        ' 
        btn_signin.Location = New Point(333, 247)
        btn_signin.Name = "btn_signin"
        btn_signin.Size = New Size(94, 29)
        btn_signin.TabIndex = 5
        btn_signin.Text = "Sign In"
        btn_signin.UseVisualStyleBackColor = True
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(800, 450)
        Controls.Add(btn_signin)
        Controls.Add(btn_login)
        Controls.Add(password_tb)
        Controls.Add(Label2)
        Controls.Add(username_tb)
        Controls.Add(Label1)
        Name = "Form1"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Form1"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents username_tb As TextBox
    Friend WithEvents password_tb As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents btn_login As Button
    Friend WithEvents btn_signin As Button

End Class
