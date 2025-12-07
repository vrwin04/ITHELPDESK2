Imports System.Data.OleDb
Imports System.Security.Cryptography
Imports System.Text
Imports System.IO

Module Session

    ' --- GLOBAL VARIABLES (To track the logged-in user) ---
    Public CurrentUserID As Integer
    Public CurrentUserRole As String
    Public CurrentUserName As String

    ' --- DATABASE CONFIGURATION ---
    ' Make sure this matches the filename of the Access database you uploaded
    Public DbName As String = "ITHelpDesk.accdb"

    ' Connection String using |DataDirectory| which points to the bin\Debug folder
    Public ReadOnly Property ConnectionString As String
        Get
            Return "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\" & DbName
        End Get
    End Property

    ' --- VERIFY DATABASE EXISTS ---
    ' Called by Form1_Load to ensure the app doesn't crash if the file is missing
    Public Function VerifyDatabase() As Boolean
        Dim dbPath As String = Path.Combine(Application.StartupPath, DbName)
        If Not File.Exists(dbPath) Then
            MessageBox.Show("Database not found!" & vbCrLf & vbCrLf &
                            "Expected at: " & dbPath & vbCrLf & vbCrLf &
                            "Please ensure 'ITHelpDesk.accdb' is in the 'bin\Debug\net8.0-windows' folder.",
                            "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End If
        Return True
    End Function

    ' --- SECURITY ---
    ' Hashes password so we don't store plain text in the database
    Public Function HashPassword(password As String) As String
        Using sha As SHA256 = SHA256.Create()
            Dim bytes As Byte() = Encoding.UTF8.GetBytes(password)
            Dim hash As Byte() = sha.ComputeHash(bytes)
            Dim sb As New StringBuilder()
            For Each b As Byte In hash
                sb.Append(b.ToString("x2"))
            Next
            Return sb.ToString()
        End Using
    End Function

    ' --- DATABASE HELPER CLASS ---
    ' This handles all the SQL commands for the Dashboard
    Public Class Db

        ' Fetch data (SELECT)
        Public Shared Function GetTable(sql As String, Optional params As List(Of Object) = Nothing) As DataTable
            Dim dt As New DataTable()
            Using conn As New OleDbConnection(Session.ConnectionString)
                Using cmd As New OleDbCommand(sql, conn)
                    AddParams(cmd, params)
                    Try
                        conn.Open()
                        dt.Load(cmd.ExecuteReader())
                    Catch ex As Exception
                        MessageBox.Show("Data Error: " & ex.Message)
                    End Try
                End Using
            End Using
            Return dt
        End Function

        ' Execute Action (INSERT, UPDATE, DELETE)
        Public Shared Function RunSql(sql As String, Optional params As List(Of Object) = Nothing) As Boolean
            Using conn As New OleDbConnection(Session.ConnectionString)
                Using cmd As New OleDbCommand(sql, conn)
                    AddParams(cmd, params)
                    Try
                        conn.Open()
                        cmd.ExecuteNonQuery()
                        Return True
                    Catch ex As Exception
                        MessageBox.Show("Operation Error: " & ex.Message)
                        Return False
                    End Try
                End Using
            End Using
        End Function

        ' Helper to add parameters to the query safely
        Private Shared Sub AddParams(cmd As OleDbCommand, params As List(Of Object))
            If params IsNot Nothing Then
                For Each p In params
                    ' Access uses ? for parameters. Order is important!
                    If p Is Nothing Then
                        cmd.Parameters.AddWithValue("?", DBNull.Value)
                    Else
                        cmd.Parameters.AddWithValue("?", p)
                    End If
                Next
            End If
        End Sub
    End Class

End Module