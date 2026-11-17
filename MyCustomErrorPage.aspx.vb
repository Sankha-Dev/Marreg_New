
Partial Class MyCustomErrorPage
    Inherits System.Web.UI.Page

    Private Sub form1_Load(sender As Object, e As EventArgs) Handles form1.Load
        Dim ex As Exception = Context.Items("current_exception")
        If ex IsNot Nothing Then
            Response.Write("<br/>-----------------------------")
            Response.Write("<br/>Source: " & ex.Source)
            Response.Write("<br/>-----------------------------")
            Response.Write("<br/>StackTrace: " & ex.StackTrace)
            Response.Write("<br/>-----------------------------")
        End If
    End Sub
End Class
