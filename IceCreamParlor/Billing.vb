Imports System.Data.OleDb
Public Class Billing
    Dim A As New Module1
    Dim rs As ADODB.Recordset
    Dim da As OleDbDataAdapter
    Dim ds As DataSet
    Dim d As Integer
    Dim allowednum As String = "0123456789" & vbBack
    Private Sub Billing_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        A.Connect()
        rs = New ADODB.Recordset
        rs.Open("SELECT * FROM Stock", A.cn)
        rs.MoveFirst()
        While rs.EOF <> True
            ComboBox1.Items.Add(rs.Fields("Item_Category").Value)
            rs.MoveNext()
        End While
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        ComboBox2.Items.Clear()
        rs = New ADODB.Recordset
        Dim str As String = ComboBox1.Text
        rs.Open("SELECT * FROM " & str & "", A.cn)
        If rs.EOF = True Then
            Exit Sub
        Else
            rs.MoveFirst()
            While rs.EOF <> True
                Dim p As Integer = 0
                For i = 0 To DataGridView1.RowCount - 1
                    If ComboBox1.Text = DataGridView1.Rows(i).Cells("Item_Category").Value And rs.Fields("Item_Name").Value = DataGridView1.Rows(i).Cells("Item_Name").Value Then
                        p = 1
                    End If
                Next
                If p = 1 Then
                    rs.MoveNext()
                    Continue While
                Else
                    ComboBox2.Items.Add(rs.Fields("Item_Name").Value)
                End If
                rs.MoveNext()
            End While
        End If
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox2.SelectedIndexChanged
        rs = New ADODB.Recordset
        rs.Open("SELECT * FROM " & ComboBox1.Text & " WHERE Item_Name='" & ComboBox2.Text & "'", A.cn)
        txtprice.Text = rs.Fields("price").Value
    End Sub

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtqty.TextChanged
        If txtprice.Text = "" Then
            MsgBox("Please Select Item")
        Else
            Label7.Text = Val(txtprice.Text) * Val(txtqty.Text)
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If txtprice.Text = "" Then
            MsgBox("Please Select Item")
        ElseIf txtqty.Text = "" Then
            MsgBox("Please Enter Quantity")
        Else
            If Val(txtqty.Text) > rs.Fields("qty").Value Then
                MsgBox("Sorry There is Only " & rs.Fields("qty").Value & " " & ComboBox2.Text & " Available")
                Call ComboBox2_SelectedIndexChanged(Me, e)
            Else
                Dim d As Integer = DataGridView1.RowCount + 1
                DataGridView1.Rows.Add()
                DataGridView1.Rows(d - 1).Cells("Item_No").Value = d
                DataGridView1.Rows(d - 1).Cells("Item_Category").Value = ComboBox1.Text
                DataGridView1.Rows(d - 1).Cells("Item_Name").Value = ComboBox2.Text
                DataGridView1.Rows(d - 1).Cells("Quantity").Value = txtqty.Text
                DataGridView1.Rows(d - 1).Cells("Price").Value = txtprice.Text
                DataGridView1.Rows(d - 1).Cells("total").Value = Label7.Text
                Call ComboBox1_SelectedIndexChanged(Me, e)
            End If
            txtqty.Clear()
        End If
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Dim d As Integer = DataGridView1.SelectedRows(0).Cells("Item_No").Value
        DataGridView1.Rows.RemoveAt(d - 1)
        For i = 0 To DataGridView1.RowCount - 1
            DataGridView1.Rows(i).Cells("Item_No").Value = i + 1
        Next
    End Sub

    Private Sub DataGridView1_RowEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.RowEnter
        Button4.Enabled = True
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If DataGridView1.RowCount = 0 Then
            MsgBox("Please Enter Items")
        Else
            rs = New ADODB.Recordset
            rs.Open("SELECT MAX(Bill_No) FROM Billing WHERE CDate(Date)='" & DateTimePicker1.Value.Date & "'", A.cn)
            If IsDBNull(rs.Fields(0).Value) Then
                d = 1
            Else
                d = rs.Fields(0).Value + 1
            End If
            rs.Close()
            Call updatestock()
            Dim t As Double = 0
            For i = 0 To DataGridView1.RowCount - 1
                rs = New ADODB.Recordset
                rs.Open("SELECT * FROM Billing", A.cn, 3, 3)
                rs.AddNew()
                rs.Fields("Date").Value = DateTimePicker1.Value.Date
                rs.Fields("Bill_No").Value = d
                rs.Fields("Item_No").Value = DataGridView1.Rows(i).Cells("Item_No").Value
                rs.Fields("Item_Category").Value = DataGridView1.Rows(i).Cells("Item_Category").Value
                rs.Fields("Item_Name").Value = DataGridView1.Rows(i).Cells("Item_Name").Value
                rs.Fields("qty").Value = DataGridView1.Rows(i).Cells("Quantity").Value
                rs.Fields("Price").Value = DataGridView1.Rows(i).Cells("Price").Value
                rs.Fields("Total_Amt").Value = DataGridView1.Rows(i).Cells("Total").Value
                rs.Update()
                t = t + DataGridView1.Rows(i).Cells("Total").Value
            Next
            rs.AddNew()
            rs.Fields("Bill_No").Value = d
            rs.Fields("Date").Value = DateTimePicker1.Value.Date
            rs.Fields("Item_Name").Value = "Total"
            rs.Fields("Total_Amt").Value = t
            rs.Update()
            MsgBox("Record Added Successfully, Your Bill No is " & d & "")
            Button1.Enabled = False
            Button2.Enabled = False
            btnPrint.Enabled = True
            DataGridView1.Rows.Clear()
        End If
    End Sub
    Private Sub updatestock()
        For i = 0 To DataGridView1.RowCount - 1
            rs = New ADODB.Recordset
            rs.Open("SELECT * FROM " & DataGridView1.Rows(i).Cells("Item_Category").Value & " WHERE Item_Name='" & DataGridView1.Rows(i).Cells("Item_Name").Value & "'", A.cn, 3, 3)
            rs.Fields("qty").Value = rs.Fields("qty").Value - DataGridView1.Rows(i).Cells("Quantity").Value
            rs.Update()
        Next
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        rs = New ADODB.Recordset
        rs.Open("SELECT * FROM Billing WHERE CDate(Date)='" & DateTimePicker1.Value.Date & "' AND CInt(Bill_No)='" & d & "'", A.cn)
        da = New OleDbDataAdapter
        ds = New DataSet
        da.Fill(ds, rs, "Billing")
        Dim rpt As New BillPrint
        Dim T As CrystalDecisions.CrystalReports.Engine.TextObject
        T = rpt.ReportDefinition.Sections(0).ReportObjects("txtBill")
        rs.Open()
        T.Text = "Date : " & rs.Fields("Date").Value
        rpt.SetDataSource(ds)
        BillReport.CrystalReportViewer1.ReportSource = rpt
        BillReport.Show()
        BillReport.CrystalReportViewer1.RefreshReport()
        Button1.Enabled = True
        btnPrint.Enabled = False
        Button2.Enabled = True
        Call ComboBox1_SelectedIndexChanged(Me, e)
    End Sub

    Private Sub txtqty_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtqty.KeyPress
        If allowednum.IndexOf(e.KeyChar) = -1 Then
            e.Handled = True
        End If
    End Sub
End Class