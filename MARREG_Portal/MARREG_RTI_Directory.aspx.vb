Imports MARREG.App_Code.DataAccessLayer
Partial Class MARREG_Portal_MARREG_RTI_Directory
    Inherits System.Web.UI.Page

#Region "Declaration"
    Dim c As New Marreg_Class
    'Dim newBox As New MessageBox()
#End Region

#Region "Page Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                'page_validation()
                LoadRTIAuthCards()
            End If
        Catch ex As Exception
            'Errormsgbox(ex.Message())
        End Try
    End Sub
#End Region

#Region "Forced Browsing"
    Private Sub page_validation()
        Try
            If Request.UrlReferrer Is Nothing Then
                Response.Redirect("~/MARREG_Portal/MARREG_Home.aspx")
            End If
        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region "show_grid"
    Private Sub LoadRTIAuthCards()
        Try
            Dim sql = "SELECT ROW_NUMBER() OVER (ORDER BY A.rtiA_post) AS slno, P.rtiPost_name AS emp_post, A.rtiA_name AS emp_name, E.emp_post AS emp_desig FROM MARREG_RTI_Auth_MT A INNER JOIN MARREG_RTI_Post P ON A.rtiA_post = P.rtiPost_cd INNER JOIN MARREG_Employee_Post E  ON A.rtiA_desig = E.emppost_cd WHERE A.active = 'Y';"

            rptRtiPrsnl.DataSource = c.selectdata(sql)
            rptRtiPrsnl.DataBind()
        Catch ex As Exception

        End Try
    End Sub
    'Private Sub showGrid()
    '    Try
    '        Dim sql As String
    '        sql = "SELECT ROW_NUMBER() OVER (ORDER BY A.rtiA_cd) AS slno, P.rtiPost_name AS emp_post, A.rtiA_name AS emp_name, E.emp_post AS emp_desig FROM MARREG_RTI_Auth_MT A INNER JOIN MARREG_RTI_Post P ON A.rtiA_post = P.rtiPost_cd INNER JOIN MARREG_Employee_Post E  ON A.rtiA_desig = E.emppost_cd;"
    '        c.GridViewLoad(sql, gridOfficials)

    '    Catch ex As Exception
    '        Errormsgbox(ex.Message())
    '    End Try
    'End Sub
#End Region
End Class
