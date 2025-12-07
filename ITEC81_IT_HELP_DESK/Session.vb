Module Session
    ' KEEP YOUR CONNECTION STRING EXACTLY AS IS:
    Public Const connString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\verwi\source\repos\ITHELPDESK2\ITEC81_IT_HELP_DESK\bin\Debug\net8.0-windows\ITHelpDesk.accdb;"

    ' ADD THESE VARIABLES SO THE FORMS CAN TALK TO EACH OTHER:
    Public CurrentUserID As Integer = 0
    Public CurrentUserRole As String = ""
    Public CurrentUserName As String = ""
End Module