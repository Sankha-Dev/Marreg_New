
Partial Class clock
    Inherits System.Web.UI.Page
    Private Sub Clock_Load(sender As Object, e As EventArgs) Handles Me.Load
        lbl_clock.Text = System.DateTime.Now.ToString
    End Sub
End Class
