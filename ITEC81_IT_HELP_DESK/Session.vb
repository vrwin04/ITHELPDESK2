Imports System.Security.Cryptography
Imports System.Text
Imports System.Data.OleDb
Imports System.Windows.Forms
Imports System.IO

Public Module Session

    ' --- GLOBAL USER SESSION ---
    Public CurrentUserID As Integer = 0
    Public CurrentUserRole As String = ""
    Public CurrentUserName As String = ""

    ' --- SMART DATABASE CONFIGURATION ---
    Public ReadOnly Property ConnectionString As String
        Get
            Dim dbName As String = "ITHelpDeskDB.accdb"

            ' 1. Check in the folder where the App is running (bin\Debug\net8.0-windows)
            Dim path1 As String = Path.Combine(Application.StartupPath, dbName)
            If File.Exists(path1) Then
                Return "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & path1 & ";"
            End If

            ' 2. Check in the Project Folder (3 levels up) - Good for development
            Dim path2 As String = Path.GetFullPath(Path.Combine(Application.StartupPath, "..\..\..\" & dbName))
            If File.Exists(path2) Then
                Return "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & path2 & ";"
            End If

            ' 3. Fallback (Returns default path so the error message shows where it looked)
            Return "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & path1 & ";"
        End Get
    End Property

    ' --- HELPER: CHECK IF DATABASE EXISTS ---
    Public Function VerifyDatabase() As Boolean
        Dim connStr As String = ConnectionString
        ' Extract just the path from the connection string for checking
        Dim pathStart As Integer = connStr.IndexOf("Data Source=") + 12
        Dim pathEnd As Integer = connStr.IndexOf(";", pathStart)
        Dim dbPath As String = connStr.Substring(pathStart, pathEnd - pathStart)

        If Not File.Exists(dbPath) Then
            MessageBox.Show("DATABASE ERROR!" & vbCrLf & vbCrLf &
                            "Could not find 'ITHelpDeskDB.accdb'." & vbCrLf &
                            "Please ensure the file is in your Project Folder.",
                            "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End If
        Return True
    End Function

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

    ' --- DATABASE HELPER ENGINE ---
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

        Private Shared Sub AddParams(cmd As OleDbCommand, params As List(Of Object))
            If params IsNot Nothing Then
                For Each p In params
                    cmd.Parameters.AddWithValue("?", p)
                Next
            End If
        End Sub
    End Class

End Module