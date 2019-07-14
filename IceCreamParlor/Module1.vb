Imports ADODB
Imports System.Data.OleDb
Public Class Module1
    Public cn As ADODB.Connection
    Public Sub Connect()
        On Error Resume Next
        cn = New ADODB.Connection
        cn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Application.StartupPath & "\IceCream.accdb"
        cn.Open()
    End Sub
End Class
