Public Class Stock
    Dim a As New Module1
    Dim rs As ADODB.Recordset
    Dim category, item As String
    Private Sub Stock_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        a.Connect()
        rs = New ADODB.Recordset
        rs.Open("SELECT * FROM Stock", a.cn)
        rs.MoveFirst()
        While rs.EOF <> True
            ComboBox1.Items.Add(rs.Fields("Item_Category").Value)
            rs.MoveNext()
        End While
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        If btnSave.Enabled = True Then
            ComboBox1.Text = category
        Else
            ComboBox2.Items.Clear()
            Call clear()
            rs = New ADODB.Recordset
            Dim str As String = ComboBox1.Text
            rs.Open("SELECT * FROM " & str & "", a.cn)
            If rs.EOF = True Then
                Exit Sub
            Else
                rs.MoveFirst()
                While rs.EOF <> True
                    ComboBox2.Items.Add(rs.Fields("Item_Name").Value)
                    rs.MoveNext()
                End While
            End If
        End If
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox2.SelectedIndexChanged
        If btnSave.Enabled = True Then
            ComboBox2.Text = item
        Else
            rs = New ADODB.Recordset
            Dim str As String = ComboBox1.Text
            rs.Open("SELECT * FROM " & str & " WHERE Item_Name='" & ComboBox2.Text & "'", a.cn)
            txtqty.Text = rs.Fields("qty").Value
            txtprice.Text = rs.Fields("price").Value
            txtdt.Text = rs.Fields("dt").Value
        End If
    End Sub
    Private Sub clear()
        txtdt.Clear()
        txtprice.Clear()
        txtqty.Clear()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        If txtqty.Text = "" Or txtprice.Text = "" Or txtdt.Text = "" Then
            MsgBox("Please Select Item")
        Else
            txtqty.ReadOnly = False
            txtprice.ReadOnly = False
            btnUpdate.Enabled = False
            btnSave.Enabled = True
            category = ComboBox1.Text
            item = ComboBox2.Text
        End If
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        rs = New ADODB.Recordset
        Dim str As String = ComboBox1.Text
        rs.Open("SELECT * FROM " & str & " WHERE Item_Name='" & ComboBox2.Text & "'", a.cn, 3, 3)
        rs.Fields("qty").Value = txtqty.Text
        rs.Fields("price").Value = txtprice.Text
        rs.Fields("dt").Value = DateTime.Now.Date
        rs.Update()
        MsgBox("Stock Updated Successfully")
        Call clear()
        txtprice.ReadOnly = True
        txtqty.ReadOnly = True
        btnSave.Enabled = False
        btnUpdate.Enabled = True
        Call ComboBox1_SelectedIndexChanged(Me, e)
    End Sub
End Class