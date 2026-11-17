Imports MARREG.App_Code.DataAccessLayer
Imports System.Data

Partial Class MARREG_Portal_MARREG_Employee_Directory
    Inherits System.Web.UI.Page

    Dim c As New Marreg_Class
#Region "Page Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            page_validation()
            Dim meta As New HtmlMeta()
            meta.HttpEquiv = "Refresh"
            meta.Content = "1200;url=" + Common.baseUrl() + "MARREG_Portal/MARREG_Home.aspx"
            Me.Page.Header.Controls.Add(meta)
            LoadEmployeeCards()
        End If
    End Sub
#End Region
#Region "Page Validation"
    Private Sub page_validation()
        Try
            Dim mode As String = If(Not (Request("mode") Is Nothing Or Request("mode") = ""), DirectCast(Request("mode"), String).Trim(), "")
            If Request.UrlReferrer Is Nothing And mode.ToLower <> "verify" Then
                Response.Redirect(Common.baseUrl() & "MARREG_Portal/MARREG_Home.aspx")
            End If
        Catch ex As Exception
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "alert", "<script>bootbox.alert('" + ex.Message() + "');</script>")
        End Try
    End Sub
#End Region
    Public Function SentenceCase(value As Object) As String
        If value Is Nothing Then Return ""
        Dim s As String = value.ToString().Trim()
        If s = "" Then Return ""
        Return Char.ToUpper(s(0)) & s.Substring(1).ToLower()
    End Function

    Private Sub LoadEmployeeCards()
        Try
            'Dim sql1 As String = "SELECT ROW_NUMBER() OVER (ORDER BY M.emp_cd) AS slno, P.emp_post, M.emp_name, dbo.sqlInj_out(M.emp_pwrandfunc) AS emp_pwrandfunc FROM MARREG_Employee_MT M INNER JOIN MARREG_Employee_Post P ON M.empPost_cd=P.emppost_cd WHERE M.empType_cd = 1;"
            'Dim sql2 As String = "SELECT ROW_NUMBER() OVER (ORDER BY M.emp_cd) AS slno, P.emp_post, M.emp_name, dbo.sqlInj_out(M.emp_pwrandfunc) AS emp_pwrandfunc FROM MARREG_Employee_MT M INNER JOIN MARREG_Employee_Post P ON M.empPost_cd=P.emppost_cd WHERE M.empType_cd = 2;"
            'Dim sql3 As String = "SELECT ROW_NUMBER() OVER (ORDER BY M.emp_cd) AS slno, P.emp_post, M.emp_name, dbo.sqlInj_out(M.emp_pwrandfunc) AS emp_pwrandfunc FROM MARREG_Employee_MT M INNER JOIN MARREG_Employee_Post P ON M.empPost_cd=P.emppost_cd WHERE M.empType_cd = 3;"

            Dim sql1 As String = "EXEC MARREG_GetEmployees 1"
            Dim sql2 As String = "EXEC MARREG_GetEmployees 2"
            Dim sql3 As String = "EXEC MARREG_GetEmployees 3"

            rptOfficials.DataSource = c.selectdata(sql1)
            rptOfficials.DataBind()

            rptSftrPrsnl.DataSource = c.selectdata(sql2)
            rptSftrPrsnl.DataBind()

            rptContract.DataSource = c.selectdata(sql3)
            rptContract.DataBind()
        Catch ex As Exception
            ' Handle exception
        End Try
    End Sub

#Region "show_grid"
    'Private Sub showGrid()
    '    Try
    '        Dim sql As String
    '        sql = "SELECT ROW_NUMBER() OVER (ORDER BY M.emp_cd) AS slno,P.emp_post, M.emp_name,dbo.sqlInj_out(M.emp_pwrandfunc) AS emp_pwrandfunc,dbo.sqlInj_out(M.emp_paylvl) AS emp_paylvl FROM MARREG_Employee_MT M INNER JOIN MARREG_Employee_Post P  ON  M.empPost_cd=P.emppost_cd WHERE M.empType_cd = 1;"
    '        c.GridViewLoad(sql, gridOfficials)
    '        'sql = "select ROW_NUMBER() over(order by wh_Id) slno,wh_Id,wh_name,wh_degnation,wh_addr,wh_contct,wh_extn,wh_FAX,(replace(replace(dbo.sqlInj_out([wh_E-mail]) ,'.','[dot]'),'@','[at]'))[wh_E-mail] from MARREG_WHO_IS_WHO_Mt WHERE wh_status='01'"
    '        'c.GridViewLoad(sql, gridWho)
    '        Dim sql1 As String
    '        sql1 = "SELECT ROW_NUMBER() OVER (ORDER BY M.emp_cd) AS slno,P.emp_post, M.emp_name,dbo.sqlInj_out(M.emp_paylvl) AS emp_paylvl FROM MARREG_Employee_MT M INNER JOIN MARREG_Employee_Post P  ON  M.empPost_cd=P.emppost_cd WHERE M.empType_cd = 2;"
    '        c.GridViewLoad(sql1, GridSftrPrsnl)
    '        Dim sql2 As String
    '        sql2 = "SELECT ROW_NUMBER() OVER (ORDER BY M.emp_cd) AS slno,P.emp_post, M.emp_name,dbo.sqlInj_out(M.emp_pwrandfunc) AS emp_pwrandfunc,dbo.sqlInj_out(M.emp_paylvl) AS emp_paylvl FROM MARREG_Employee_MT M INNER JOIN MARREG_Employee_Post P  ON  M.empPost_cd=P.emppost_cd WHERE M.empType_cd = 3;"
    '        c.GridViewLoad(sql2, gridContract)
    '    Catch ex As Exception
    '        'Errormsgbox(ex.Message())
    '    End Try
    'End Sub
#End Region
End Class
