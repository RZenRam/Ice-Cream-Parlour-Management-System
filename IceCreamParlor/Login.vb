Public Class Login

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If TextBox1.Text = "" Then
            MsgBox("Please Enter UserName !")
            TextBox1.Focus()
        ElseIf TextBox2.Text = "" Then
            MsgBox("Please Enter Password !")
            TextBox2.Focus()
        ElseIf TextBox1.Text = "admin" And TextBox2.Text = "admin" Then
            Me.Close()
            Form1.Hide()
            Main.Show()
        Else
            MsgBox("Check UserName or Password")
            TextBox1.Focus()
        End If
    End Sub

    Private Sub Login_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class