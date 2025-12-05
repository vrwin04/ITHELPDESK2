Imports System.Security.Cryptography
Imports System.Text

' MISSING MODULE WRAPPER ADDED HERE
Public Module Session

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

    Public ReadOnly Property ConnectionString As String
        Get
            Dim dbPath As String = IO.Path.Combine(Application.StartupPath, "ITHelpDeskDB.accdb")
            Return "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & dbPath & ";"
        End Get
    End Property

    Public Enum UserRole
        Student
        Admin
        Manager
    End Enum

End Module