Imports MARREG.App_Code.BusinessLogicLayer
Imports System.Net.NetworkInformation
Imports System.Data
Imports MARREG.App_Code.DataAccessLayer
Imports Microsoft.Reporting.WebForms
Imports InfoSoftGlobal
Imports System.Net.Mail
Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.html.simpleparser
Imports iTextSharp.text.pdf
Imports Accord.Vision.Detection
Imports Accord.Vision.Detection.Cascades
Imports System.Diagnostics
Imports System.Web.UI.DataVisualization.Charting

Partial Class MARREG_Portal
    Inherits System.Web.UI.MasterPage

#Region "Declaration"
    Private portal_bl As New MARREG_Portal_Page_BL()
    Dim a As New Marreg_Class
    'Shared user_info As New Marreg_Loginfo
    Dim color As String() = New String(18) {}
    Dim GraphWidth As String = "450"
    Dim GraphHeight As String = "420"
    Dim dt As DataTable
    Dim dt1 As DataTable
    Dim dt_dist_grid As DataTable = New DataTable()
    Private logindb As New MARREG_Logib_DB()
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                
                Dim chartname As String = "MARRIAGES REGISTERED in West Bengal"
                ConfigureColors()
                'dt_dist_grid.Columns.Add(New DataColumn("dist_color", GetType(String)))
                Dim column As DataColumn

                column = New DataColumn()
                column.DataType = System.Type.GetType("System.String")
                column.ColumnName = "dist_color"
                dt_dist_grid.Columns.Add(column)

                column = New DataColumn()
                column.DataType = System.Type.GetType("System.String")
                column.ColumnName = "dist_nm"
                dt_dist_grid.Columns.Add(column)
            End If
			GetDistrictChartData()
			GetChartData()
        Catch ex As Exception

        End Try
    End Sub

#Region "Fill Data Table"
    Private Sub fillDataTable()
        '25.01.16
        'Dim p = "select dist.EDistrict_Name,count(subdiv.Sub_Division_Cd)SubDivision from MARREG_District_Mt dist,MARREG_SubDivision_Mt subdiv " & _
        '        "where dist.District_Cd=subdiv.District_Cd group by dist.EDistrict_Name"
        'dt = a.selectdata(p)

        'dt = portal_bl.Fill_DataTable(" ", " ", "19", " ", " ", " ", " ", " ")
        dt = portal_bl.Fill_DataTable("19")

        Dim p = "select upper(EDistrict_Name) EDistrict_Name from MARREG_District_Mt where State_Cd='19' order by EDistrict_Name asc"
        dt1 = a.selectdata(p)
        'end 25.01.16
    End Sub
#End Region

#Region "Chart Colors"
    Private Sub ConfigureColors()
        color(0) = "AFD8F8"
        color(1) = "F6BD0F"
        color(2) = "8BBA00"
        color(3) = "FF8E46"
        color(4) = "008E8E"
        color(5) = "D64646"
        color(6) = "8E468E"
        color(7) = "588526"
        color(8) = "B3AA00"
        color(9) = "008ED6"
        color(10) = "9D080D"
        color(11) = "A186BE"
        color(12) = "447AA3"
        color(13) = "7DAB8B"
        color(14) = "C99D87"
        color(15) = "994A4A"
        color(16) = "9CB154"
        color(17) = "369C81"
        color(18) = "BF956D"
    End Sub
#End Region

#Region "Doughnut Chart"
    'Protected Sub create_DoughnutChart()
    '    'Dim strCaption As String = "Marriages Registered in West Bengal"
    '    'Dim strSubCaption As String = "For the Current Year"
    '    Dim xAxis As String = "District"
    '    Dim yAxis As String = "Registration"
    '    'ConfigureColors()
    '    'strXML will be used to store the entire XML document generated
    '    Dim strXML As String = Nothing

    '    'Generate the graph element
    '    strXML = "<graph decimalPrecision='0' " & vbCr & vbLf & "pieSliceDepth='30' formatNumberScale='0'" & vbCr & vbLf & "xAxisName='" & xAxis & "' yAxisName='" & yAxis & "' rotateNames='1'" & vbCr & vbLf & ">"

    '    '25.01.16
    '    'Dim i As Integer = 0
    '    'For Each dr2 As DataRow In dt.Rows
    '    '    strXML += ("<set name='" & dr2(0).ToString() & "' value='" & dr2(1).ToString() & "' color='") + color(i) & "' link=&quot;JavaScript:myJS('" & dr2("EDistrict_Name").ToString() & ", " & dr2("SubDivision").ToString() & "'); &quot;/>"
    '    '    i += 1
    '    '    If i > 18 Then
    '    '        i = 0
    '    '    End If
    '    'Next

    '    Dim dt_rw_cnt = dt.Rows.Count()
    '    Dim dst1, dst2 As String


    '    Dim i As Integer = 0
    '    For Each dr2 As DataRow In dt1.Rows
    '        Dim dr As DataRow = dt_dist_grid.NewRow()
    '        dr("dist_color") = "#" + color(i)
    '        dr("dist_nm") = dr2("EDistrict_Name").ToString()
    '        dt_dist_grid.Rows.Add(dr)
    '        grid_dist.DataSource = dt_dist_grid
    '        grid_dist.DataBind()

    '        dst1 = dr2("EDistrict_Name").ToString()

    '        If dt_rw_cnt > 0 Then
    '            For Each dr3 As DataRow In dt.Rows
    '                dst2 = dr3("name").ToString()
    '                If dst1.Equals(dst2) Then
    '                    strXML += ("<set name='" & dr3(0).ToString() & "' value='" & dr3(1).ToString() & "' color='") + color(i) & "' link=&quot;JavaScript:myJS('" & dr3("name").ToString() & ", " & dr3("cnt").ToString() & "'); &quot;/>"
    '                    dt_rw_cnt = dt_rw_cnt - 1
    '                End If
    '            Next
    '        End If

    '        i += 1
    '        If i > 18 Then
    '            i = 0
    '        End If
    '    Next

    '    'end 25.01.16

    '    'Finally, close <graph> element
    '    strXML += "</graph>"

    '    ' Path to chart's SWF
    '    ' Leave blank when using Data String method
    '    ' xmlStr contains the chart data
    '    ' Unique chart ID
    '    ' Width & Height of chart
    '    doughnut_Chart.Text = FusionCharts.RenderChartHTML("../MARREG_Charts/FCF_Doughnut2D.swf", "", strXML, "mygraph1", GraphWidth, GraphHeight, _
    '        False)

    'End Sub
#End Region


#Region "Count Online User"
    Private Sub CountOnlieUser()
        Try
            Dim dt As DataTable = logindb.countonlineuser("onlineuser")
            lblonlineuser.Text = dt.Rows(0)("cnt").ToString()
            'griddistrict.DataSource = ds.Tables(0)
            'griddistrict.DataBind()
        Catch ex As Exception
            ' Errormsgbox(ex.Message())
        End Try
    End Sub
#End Region


#Region "Show online user in gridview"
    Private Sub Show_OnlineUsers()
        Try
            Dim dt As DataTable = logindb.countonlineuser("user")
            'lblonlineuser.Text = dt.Rows(0)("cnt").ToString()
            gridonlineuser.DataSource = dt
            gridonlineuser.DataBind()
        Catch ex As Exception
            ' Errormsgbox(ex.Message())
        End Try
    End Sub
    Protected Sub gridonlineuser_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gridonlineuser.PageIndexChanging
        gridonlineuser.PageIndex = e.NewPageIndex
        Show_OnlineUsers()
        modalpopup2.show()
    End Sub
#End Region

    '#Region "Report Generate"
    '    Public Class ReportViewerCredentials
    '        Implements IReportServerCredentials
    '        Private _userName As String
    '        Private _password As String
    '        Private _domain As String

    '        Public Sub New(ByVal userName As String, ByVal password As String, ByVal domain As String)
    '            _userName = userName
    '            _password = password
    '            _domain = domain
    '        End Sub

    '        Public ReadOnly Property ImpersonationUser() As System.Security.Principal.WindowsIdentity Implements Microsoft.Reporting.WebForms.IReportServerCredentials.ImpersonationUser
    '            Get
    '                Return Nothing
    '            End Get
    '        End Property

    '        Public ReadOnly Property NetworkCredentials() As System.Net.ICredentials Implements Microsoft.Reporting.WebForms.IReportServerCredentials.NetworkCredentials
    '            Get
    '                Return New Net.NetworkCredential(_userName, _password, _domain)
    '            End Get
    '        End Property ''

    '        Public Function GetFormsCredentials(ByRef authCookie As System.Net.Cookie, ByRef userName As String, ByRef password As String, ByRef authority As String) As Boolean Implements Microsoft.Reporting.WebForms.IReportServerCredentials.GetFormsCredentials
    '            userName = _userName
    '            password = _password
    '            authority = _domain
    '            Return Nothing
    '        End Function
    '    End Class

    '    Private Sub SetReportParameters(State_cd As String, from_dt As Date, to_date As String, work_area_cd As String,
    '                                    Chart_name As String, ByVal District_cd As String,
    '                                    sub_division_cd As String, Block_cd As String,
    '                                     Muni_GP_cd As String)

    '        Try

    '            ReportViewer1.Visible = True
    '            'ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote

    '            ReportViewer1.ServerReport.ReportServerUrl = New Uri("http://10.25.116.19/reportserver")
    '            Dim strUserName As String = ConfigurationManager.AppSettings("uid").ToString()
    '            Dim strPassword As String = ConfigurationManager.AppSettings("pwd").ToString()
    '            Dim strDomain As String = ConfigurationManager.AppSettings("domain").ToString()

    '            ReportViewer1.ServerReport.ReportServerCredentials = New ReportViewerCredentials(strUserName, strPassword, "")
    '            ReportViewer1.ServerReport.ReportPath = "/rgmwb/" + "MARREG_Certifct_Count"

    '            Dim paramList As New Generic.List(Of ReportParameter)
    '            paramList.Add(New ReportParameter("From_dt", from_dt))
    '            paramList.Add(New ReportParameter("To_dt", to_date))
    '            paramList.Add(New ReportParameter("State_cd", State_cd))
    '            paramList.Add(New ReportParameter("District_cd", District_cd))
    '            paramList.Add(New ReportParameter("sub_division_cd", sub_division_cd))
    '            paramList.Add(New ReportParameter("Block_cd", Block_cd))
    '            paramList.Add(New ReportParameter("work_area_cd", work_area_cd))
    '            paramList.Add(New ReportParameter("Muni_GP_cd", Muni_GP_cd))
    '            paramList.Add(New ReportParameter("Chart_Name", Chart_name))
    '            ReportViewer1.ServerReport.SetParameters(paramList)
    '            'ReportViewer1.ServerReport.DisplayName = ddl_application_no.SelectedItem.ToString()

    '            'Process and render the report
    '            ReportViewer1.ServerReport.Refresh()
    '            ReportViewer1.AsyncRendering = False
    '            ReportViewer1.ShowPrintButton = True

    '            ReportViewer1.DocumentMapCollapsed = False

    '        Catch ex As Exception
    '            Dim p = ex.Message
    '        End Try
    '    End Sub
    '#End Region
    Protected Sub GetDistrictChartData()
        Try

            Dim series1 As Series = district_wise_certificate.Series("DISTRICTS")

            dt = portal_bl.Fill_DataTable("19")

            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    series1.Points.AddXY(dt.Rows(i)(0).ToString(), dt.Rows(i)(1).ToString())

                Next
            End If
            'district_wise_certificate.Titles.Add("MARRIAGES REGISTERED IN WEST BENGAL                (From 01-01-" + DateTime.Now.ToString("yyyy") + " To " + DateTime.Now.ToString("dd-MM-yyyy") + ")")
            'start shaibal das 30.04.2019 add
            district_wise_certificate.Titles.Add("                                            " + ViewState("Total_certificate_of_this_year") + " MARRIAGES REGISTERED IN WEST BENGAL  (From 01-01-" + DateTime.Now.ToString("yyyy") + " To " + DateTime.Now.ToString("dd-MM-yyyy") + ")")
            district_wise_certificate.Titles(0).Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0F, Drawing.FontStyle.Bold)
            district_wise_certificate.ChartAreas("ChartArea1").AxisY.TitleFont = New System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Bold)
            district_wise_certificate.ChartAreas("ChartArea1").AxisX.TitleFont = New System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Bold)
            'end shaibal das 30.04.2019
            district_wise_certificate.ChartAreas("ChartArea1").AxisX.Interval = 1
            district_wise_certificate.ChartAreas("ChartArea1").AxisX.MajorGrid.Enabled = False
            

        Catch ex As Exception
            'Errormsgbox(ex.Message())
        End Try
    End Sub
    Private Sub DistWiseChartBtnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles DistWiseChartBtnSave.Click
        Dim Doc As Document = New Document(PageSize.A4, 10.0!, 10.0!, 10.0!, 0.0!)
        'Doc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate())  'for landscape
        PdfWriter.GetInstance(Doc, Response.OutputStream)
        Doc.Open()
        Dim memoryStream As MemoryStream = New MemoryStream
        'GetDistrictChartData()
        district_wise_certificate.SaveImage(memoryStream, ChartImageFormat.Png)
        Dim img As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(memoryStream.GetBuffer)
        Dim img1 As Image = Image.GetInstance(memoryStream.GetBuffer)
        img1.ScalePercent(78.0!)
        img1.RotationDegrees = 90.0F
        img1.SetAbsolutePosition(130.0F, 100.0F)
        Doc.Add(img1)
        Doc.Close()
        Response.ContentType = "application/pdf"
        Response.AddHeader("content-disposition", "attachment;filename=marriages_registered_in_wb.pdf")
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.Write(Doc)
        Response.End()
    End Sub
    Protected Sub GetChartData()
        Try
            Dim dataset_result As DataSet = portal_bl.MoTrainsaction()
            Dim dt As DataTable = dataset_result.Tables(0)
            Dim series1 As Series = mo_transaction_chart.Series("NOMO/MOHMR FUNCTIONING")
            Dim series2 As Series = mo_transaction_chart.Series("NOMO/MOHMR USING PORTAL")
            Dim series3 As Series = mo_transaction_chart.Series("SOLEMNISED/REGISTERED")
            Dim series4 As Series = mo_transaction_chart.Series("CUMULATIVE SOLEMNISED/REGISTERED")

            If dataset_result.Tables(0).Rows.Count > 0 Then
                If dt.Rows.Count > 0 Then
                    For i As Integer = 0 To dt.Rows.Count - 1
                        series1.Points.AddXY(dt.Rows(i)(0).ToString(), dt.Rows(i)(1).ToString())
                        series2.Points.AddXY(dt.Rows(i)(0).ToString(), dt.Rows(i)(2).ToString())
                        series3.Points.AddXY(dt.Rows(i)(0).ToString(), dt.Rows(i)(3).ToString())
                        series4.Points.AddXY(dt.Rows(i)(0).ToString(), dt.Rows(i)(4).ToString())
                    Next
                End If
            Else
            End If
        Catch ex As Exception
            'Errormsgbox(ex.Message())
        End Try
    End Sub
    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
        Dim Doc As Document = New Document(PageSize.A4, 10.0!, 10.0!, 10.0!, 0.0!)
        PdfWriter.GetInstance(Doc, Response.OutputStream)
        Doc.Open()
        Dim memoryStream As MemoryStream = New MemoryStream
        'GetChartData()
        mo_transaction_chart.SaveImage(memoryStream, ChartImageFormat.Png)
        Dim img As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(memoryStream.GetBuffer)
         Dim img1 As Image = Image.GetInstance(memoryStream.GetBuffer)
        img1.ScalePercent(78.0!)
        img1.RotationDegrees = 90.0F
        img1.SetAbsolutePosition(140.0F, 110.0F)
        Doc.Add(img1)
        Doc.Close()
        Response.ContentType = "application/pdf"
        Response.AddHeader("content-disposition", "attachment;filename=overview_of_transaction.pdf")
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.Write(Doc)
        Response.End()
    End Sub
	Private Sub LoadVisitorCount()
        Dim ipadd As String = Request.ServerVariables("REMOTE_ADDR").ToString()
        Dim nics As NetworkInterface() = NetworkInterface.GetAllNetworkInterfaces()
        Dim macaddress As String = nics(0).GetPhysicalAddress().ToString()
        Dim ms As String = portal_bl.PortalLoginSave("0", Session.SessionID, ipadd, macaddress, "Login", 1)
        'If ms = "Login" Then
        Dim dt As DataTable = portal_bl.Portal_Visitors_count()
        vis_cnt.Text = dt.Rows(0)("cnt").ToString()
    End Sub
    Protected Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        LoadVisitorCount()
        Timer1.Enabled = False
        lblVloading.Visible = False
    End Sub

    Protected Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        CountOnlieUser()
        Show_OnlineUsers()
        Timer2.Enabled = False
        lblMOloading.Visible = False
    End Sub
    Private Sub LoadCertificateCount()
        Dim qu = "select mnth.month_cont, yr.year_cont from (select count(Entry_dt) month_cont " & _
                        "from [MARREG_Certificate_New] where OTP is not null and month(Entry_dt)=month(GETDATE()) and year(Entry_dt)=year(GETDATE()))mnth," & _
                        "(select count(Entry_dt) year_cont from [MARREG_Certificate_New] where OTP is not null and year(Entry_dt)=year(GETDATE()))yr"

        Dim dt_certfct As DataTable = a.selectdata(qu)
        lbl_cert_jun.Text = dt_certfct.Rows(0)("year_cont").ToString()
        ViewState("Total_certificate_of_this_year") = dt_certfct.Rows(0)("year_cont").ToString() 'add shaibaldas 30.04.2019
        ViewState("Total_certificate_of_this_month") = dt_certfct.Rows(0)("month_cont").ToString()
    End Sub
    Protected Sub Timer3_Tick(sender As Object, e As EventArgs) Handles Timer3.Tick
        lbl_cert_curnt.Text = ViewState("Total_certificate_of_this_month")
        Timer3.Enabled = False
        lblCertCountLoading.Visible = False
    End Sub

    Protected Sub Timer4_Tick(sender As Object, e As EventArgs) Handles Timer4.Tick
        LoadCertificateCount()
        Timer4.Enabled = False
        lbl_CertCountYearLoading.Visible = False
    End Sub

    Protected Sub Timer5_Tick(sender As Object, e As EventArgs) Handles Timer5.Tick
        fillDataTable()
		link_mo_transaction.Visible=True
        Timer5.Enabled = False
        lbl_OverviewOfTransactions.Visible = False
    End Sub
End Class

