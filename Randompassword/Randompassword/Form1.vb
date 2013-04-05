Public Class Form1

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim s As String = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ!#%&?"
        Dim rnd As New Random
        Dim o As String = String.Empty
        For n As Integer = 0 To 14
            Dim r As Integer = rnd.Next(0, s.Length - 1)
            o += s.Substring(r, 1)
        Next
        txtPwd.Text = o
    End Sub
End Class
