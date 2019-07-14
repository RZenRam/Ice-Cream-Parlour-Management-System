Public Class NewItem
    Dim c As New Module1
    Dim rs As ADODB.Recordset
    Dim allowedchar As String = "0123456789." & vbBack
    Private Sub NewItem_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        c.Connect()
        rs = New ADODB.Recordset
        rs.Open("SELECT * FROM Stock", c.cn)
        rs.MoveFirst()
        While rs.EOF <> True
            ComboBox1.Items.Add(rs.Fields("Item_Category").Value)
            rs.MoveNext()
        End While
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If ComboBox1.Text = "" Then
            MsgBox("Select Item Category")
        ElseIf TextBox1.Text = "" Or TextBox2.Text = "" Or txtqty.Text = "" Then
            MsgBox("Please Fill All the Fields")
        Else
            rs = New ADODB.Recordset
            rs.Open("SELECT * FROM " & ComboBox1.Text & " ", c.cn, 3, 3)
            rs.AddNew()
            rs.Fields("Item_Name").Value = TextBox1.Text
            rs.Fields("qty").Value = txtqty.Text
            rs.Fields("Price").Value = TextBox2.Text
            rs.Fields("dt").Value = DateTime.Now.Date
            rs.Update()
            MsgBox("Item Added Successfully")
            Call clear()
        End If
    End Sub
    Private Sub clear()
        TextBox1.Clear()
        txtqty.Clear()
        TextBox2.Clear()
        TextBox1.Focus()
    End Sub

    Private Sub TextBox1_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox1.KeyPress
        If allowedchar.IndexOf(e.KeyChar) <> -1 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtqty_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtqty.KeyPress
        If allowedchar.IndexOf(e.KeyChar) = -1 Then
            e.Handled = True
        End If
    End Sub

    Private Sub TextBox2_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox2.KeyPress
        If allowedchar.IndexOf(e.KeyChar) = -1 Then
            e.Handled = True
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub
End Class