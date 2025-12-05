Imports System.Security.Cryptography
Imports System.Text
Imports System.Windows.Forms

Public Module Session

    ' --- 1. RESTORED VARIABLES (Required for Login/Dashboard) ---
    Public CurrentUserID As Integer = 0
    Public CurrentUserRole As String = ""
    Public CurrentUserName As String = ""

    ' --- 2. SECURITY HELPER ---
    Public Function HashPassword(password As String) As String
        Using sha256 As SHA256 = SHA256.Create()
            Dim bytes As Byte() = sha256.ComputeHash(Encoding.UTF8.GetBytes(password))
            Dim builder As New StringBuilder()
            For Each b As Byte In bytes
                builder.Append(b.ToString("x2"))
            Next
            Return builder.ToString()
        End Using
    End Function

    ' --- 3. DATABASE CONNECTION ---
    Public ReadOnly Property ConnectionString As String
        Get
            Dim dbPath As String = IO.Path.Combine(Application.StartupPath, "ITHelpDeskDB.accdb")
            Return "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & dbPath & ";"
        End Get
    End Property

    ' --- 4. ROLES ENUM ---
    Public Enum UserRole
        Student
        Admin
        Manager
    End Enum

End Module