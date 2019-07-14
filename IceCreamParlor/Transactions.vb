Imports System.Data.OleDb
Public Class Transactions
    Dim a As New Module1
    Dim rs As ADODB.Recordset
    Dim da As OleDbDataAdapter
    Dim ds As DataSet
    Private Sub Transactions_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        a.Connect()
    End Sub

    Private Sub radiodaily_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radiodaily.CheckedChanged
        If radiodaily.Checked = True Then
            Label2.Visible = False
            ComboBox1.Visible = False
        Else
            Label2.Visible = True
            ComboBox1.Visible = True
        End If
    End Sub

    Private Sub DateTimePicker1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateTimePicker1.ValueChanged
        ComboBox1.Items.Clear()
        If radiosingle.Checked = True Then
            rs = New ADODB.Recordset
            rs.Open("SELECT Bill_No FROM Billing WHERE CDate(Date)='" & DateTimePicker1.Value.Date & "' GROUP BY Bill_No", a.cn)
            If rs.EOF = True Then
                MsgBox("Sorry No Any Record Found")
            Else
                rs.MoveFirst()
                While rs.EOF <> True
                    ComboBox1.Items.Add(rs.Fields("Bill_No").Value)
                    rs.MoveNext()
                End While
            End If
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try
            Dim rpt As New BillPrint
            If radiodaily.Checked = True Then
                rs = New ADODB.Recordset
                rs.Open("SELECT Bill_No,Item_No,Item_Category,Item_Name,qty,Price,Total_Amt FROM Billing WHERE CDate(Date)='" & DateTimePicker1.Value.Date & "'", a.cn)
                da = New OleDbDataAdapter
                ds = New DataSet
                da.Fill(ds, rs, "Billing")
            Else
                rs = New ADODB.Recordset
                rs.Open("SELECT * FROM Billing WHERE CDate(Date)='" & DateTimePicker1.Value.Date & "' AND CInt(Bill_No)='" & ComboBox1.Text & "'", a.cn)
                da = New OleDbDataAdapter
                ds = New DataSet
                da.Fill(ds, rs, "Billing")
            End If
            Dim T As CrystalDecisions.CrystalReports.Engine.TextObject
            T = rpt.ReportDefinition.Sections(0).ReportObjects("txtBill")
            T.Text = "Date : " & DateTimePicker1.Value.Date
            rpt.SetDataSource(ds)
            BillReport.CrystalReportViewer1.ReportSource = rpt
            BillReport.Show()
            BillReport.CrystalReportViewer1.RefreshReport()
        Catch ex As Exception
            MsgBox("Sorry No Any Record Found")
        End Try
    End Sub

    Private Sub radiosingle_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radiosingle.CheckedChanged

    End Sub
End Class