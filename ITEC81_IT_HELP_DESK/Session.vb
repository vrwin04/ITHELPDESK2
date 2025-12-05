Imports System.Security.Cryptography
Imports System.Text
Imports System.Data.OleDb
Imports System.Windows.Forms

Public Module Session

    ' --- GLOBAL USER SESSION ---
    Public CurrentUserID As Integer = 0
    Public CurrentUserRole As String = ""
    Public CurrentUserName As String = ""

    ' --- DATABASE CONFIGURATION ---
    Public ReadOnly Property ConnectionString As String
        Get
            ' Automatically finds the database in the startup folder
            Dim dbPath As String = IO.Path.Combine(Application.StartupPath, "ITHelpDeskDB.accdb")
            Return "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & dbPath & ";"
        End Get
    End Property

    ' --- SECURITY: PASSWORD HASHING ---
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

    ' --- DATABASE HELPER ENGINE (New Major Feature) ---
    ' This handles all database connections centrally.
    Public Class Db
        Public Shared Function GetTable(sql As String, Optional params As List(Of Object) = Nothing) As DataTable
            Dim dt As New DataTable
            Using conn As New OleDbConnection(ConnectionString)
                Using cmd As New OleDbCommand(sql, conn)
                    AddParams(cmd, params)
                    Try
                        conn.Open()
                        dt.Load(cmd.ExecuteReader())
                    Catch ex As Exception
                        MessageBox.Show("Database Read Error: " & ex.Message)
                    End Try
                End Using
            End Using
            Return dt
        End Function

        Public Shared Function RunSql(sql As String, Optional params As List(Of Object) = Nothing) As Boolean
            Using conn As New OleDbConnection(ConnectionString)
                Using cmd As New OleDbCommand(sql, conn)
                    AddParams(cmd, params)
                    Try
                        conn.Open()
                        cmd.ExecuteNonQuery()
                        Return True
                    Catch ex As Exception
                        MessageBox.Show("Database Write Error: " & ex.Message)
                        Return False
                    End Try
                End Using
            End Using
        End Function

        Public Shared Function GetScalar(sql As String, Optional params As List(Of Object) = Nothing) As Object
            Using conn As New OleDbConnection(ConnectionString)
                Using cmd As New OleDbCommand(sql, conn)
                    AddParams(cmd, params)
                    Try
                        conn.Open()
                        Return cmd.ExecuteScalar()
                    Catch ex As Exception
                        Return Nothing
                    End Try
                End Using
            End Using
        End Function

        Private Shared Sub AddParams(cmd As OleDbCommand, params As List(Of Object))
            If params IsNot Nothing Then
                For Each p In params
                    cmd.Parameters.AddWithValue("?", p)
                Next
            End If
        End Sub
    End Class

End Module