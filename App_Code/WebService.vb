Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Web.Script.Services
Imports System.Collections.Generic
Imports System.Web.Script.Serialization
Imports System.Data.SqlClient
Imports System.Data
Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates
Imports MARREG.App_Code
Imports EncryptionDecryption
Imports MARREG.App_Code.DataAccessLayer
Imports System.IO
Imports System.Security.Cryptography

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class WebService
    Inherits System.Web.Services.WebService
    Dim a As New Marreg_Class
    Dim eml As New BusinessLogicLayer.Email_Sms_BL()
    Dim enc As New EncryptionDecryption.EncryptionDecryption()
    <WebMethod()> _
    Public Function HelloWorld() As String
        Try
            Dim javaScriptSerializer1 As New JavaScriptSerializer()
            Dim l1 As New Dictionary(Of String, Object)
            l1("name") = "BIKASH CHATTERJEE"
            l1("address") = "188/1 BLOCK J NEW ALIPORE"
            l1("email") = "BKTECHNOLOGY6@GMAIL.COM".ToLower()
            l1("req_dt") = "22/05/2024"
            l1("req_id") = "O/2024/00004068"
            l1("serve_dt") = Convert.ToDateTime("22/05/2024").AddDays(90).ToString("dd/MM/yyyy") 'DateTime.Today.AddDays(90).ToString("dd/MM/yyyy")
            l1("old") = "Y"
            Dim data = javaScriptSerializer1.Serialize(l1)
            Return data
            'Dim encData = enc.encrypt(data)
            'Return encData
        Catch ex As Exception
        Finally

        End Try
        'Return "Hello World"
    End Function
#Region "GRN Checking"
    Public Shared Function ValidateRemoteCertificate(ByVal sender As Object, ByVal certificate As X509Certificate, ByVal chain As X509Chain, ByVal sslPolicyErrors As SslPolicyErrors) As Boolean
        Return True
    End Function
    Public Function check_Grn(ByVal nodeListChallan As System.Xml.XmlNodeList) As String
        Dim HEAD_OF_ACCOUNT As String = "0070-60-108-001-12"
        Dim hoa1 As String = ""
        For Each node As System.Xml.XmlNode In nodeListChallan
            Try
                Dim AMOUNT1 As String = node.SelectSingleNode("AMOUNT1").InnerText     'ViewState("HEAD_OF_ACCOUNT1")
                hoa1 = node.SelectSingleNode("HEAD_OF_ACCOUNT1").InnerText
            Catch ex As Exception

            End Try
        Next
        If HEAD_OF_ACCOUNT = hoa1 Then
            Return "MARREG HEAD"
        Else
            Return "NOT MARREG HEAD"
        End If
    End Function

    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub CheckGRN()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
        'Dim encdec = New EncryptionDecryption.EncryptionDecryption
        Dim amount As String = String.Empty
        Try
            Dim ajax, token As String
            Dim method As String
            ajax = HttpContext.Current.Request.Headers("X-Requested-With")
            token = HttpContext.Current.Request.Headers("X-CSRF-TOKEN")
            method = HttpContext.Current.Request.HttpMethod
            If Not (method = "POST" And ajax = "XMLHttpRequest") Then
                response("error") = "Method not allowed"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.MethodNotAllowed, Integer)
                Exit Sub
            End If
            If token = "" Then
                If Session.Item("Marreg_user_info") Is Nothing Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            Else
                If Not token = Session("CSRF") Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            End If

            Dim cat As String = If(Not Request("cat") Is Nothing, DirectCast(Request("cat"), String).Trim(), "")
            Dim period As String = If(Not Request("period") Is Nothing, DirectCast(Request("period"), String).Trim(), "")
            Dim GRN_NO As String = If(Not Request("GRN_NO") Is Nothing, DirectCast(Request("GRN_NO"), String).Trim(), "")
            Dim No_Copy As Integer = If(Not Request("No_Copy") Is Nothing, Convert.ToInt32(Request("No_Copy")), 0)
            Dim reason As String = If(Not Request("reason") Is Nothing, DirectCast(Request("reason"), String).Trim(), "")

            Net.ServicePointManager.SecurityProtocol = Net.SecurityProtocolType.Tls12
            System.Net.ServicePointManager.ServerCertificateValidationCallback = AddressOf ValidateRemoteCertificate
            Dim status, status_desc, total_amount As String
            'ConfigurationManager.AppSettings("gi_grncheck_url")
            'Dim grn = ("https://wbifms.gov.in/GRIPS/ChallanDetails/query.do?GRN_NO=" + GRN_NO + "")
            Dim grn = String.Format(ConfigurationManager.AppSettings("gi_grncheck_url").ToString(), GRN_NO)
            Dim xmlDoc As System.Xml.XmlDocument = New System.Xml.XmlDocument
            xmlDoc.Load(grn)
            Dim nodeList As System.Xml.XmlNodeList = xmlDoc.DocumentElement.SelectNodes("/GRIPS_EPAYMENT")
            Dim nodeListChallan As System.Xml.XmlNodeList = xmlDoc.DocumentElement.SelectNodes("/GRIPS_EPAYMENT/CHALLAN_DETAILS")
            Dim grn_stat = check_Grn(nodeListChallan)
            'grn_stat = "MARREG HEAD"

            If grn_stat = "MARREG HEAD" Then
                For Each node As System.Xml.XmlNode In nodeList
                    status = node.SelectSingleNode("STATUS").InnerText
                    'status = "S"
                    If status <> "S" Then    'status = "I" Or status = "F" Or status = "P"
                        status_desc = node.SelectSingleNode("STATUS_DESC").InnerText
                        response("error") = status_desc
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Else
                        total_amount = node.SelectSingleNode("TOTAL_AMOUNT").InnerText
                        'total_amount = "250"
                        Select Case cat
                            Case "c"        ' For Certified Copy
                                amount = ConfigurationManager.AppSettings("amount_for_certified_copy")
                                If (No_Copy < Convert.ToInt32(ConfigurationManager.AppSettings("minimum_copy"))) Then
                                    json("error") = "Minimum " + ConfigurationManager.AppSettings("minimum_copy") + " copy needed to be entered."
                                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                                    Exit Sub
                                End If
                                If (total_amount <> Convert.ToInt32(amount) * No_Copy) Then
                                    json("error") = "Your GRN Amount should be <i class='fa fa-rupee-sign' aria-hidden='true'></i> " + (Convert.ToInt32(amount) * No_Copy).ToString() + "/-. Please pay this amount in GRIPS and submit correct GRN."
                                    json("xml_data") = xmlDoc.InnerXml
                                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
                                Else
                                    'Save_GRN_Details_Ag_Search() start
                                    Dim obj As New WebService_BL
                                    Dim Payment_Reason = "Certified copy of Notice/Application"
                                    Session("Payment_Reason") = Payment_Reason
                                    Session("gid") = Nothing
                                    Session("no_copy") = Nothing
                                    Dim rets = obj.Save_GRN_Details_Ag_Search(GRN_NO, Convert.ToInt64(total_amount), Payment_Reason, reason)
                                    If rets = "-1" Then     'Duplicate value
                                        json("error") = "GRN is already used"
                                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                                    ElseIf rets = "-2" Then   'Transaction rollback
                                        json("error") = "Internal error occured. Please try again later."
                                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                                    Else
                                        If Convert.ToInt64(rets) >= 1 Then
                                            json("gid") = Convert.ToInt64(rets)
                                            json("no_copy") = No_Copy
                                            Session("gid") = Convert.ToInt64(rets)
                                            Session("no_copy") = No_Copy
                                        End If
                                        json("success") = "GRN is Valid"
                                        json("xml_data") = xmlDoc.InnerXml
                                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
                                    End If
                                    'Save_GRN_Details_Ag_Search() end
                                End If
                            Case "s"        ' For Searching
                                Select Case period
                                    Case "1"
                                        amount = ConfigurationManager.AppSettings("search_period_1")
                                    Case "2"
                                        amount = ConfigurationManager.AppSettings("search_period_2")
                                    Case "3"
                                        amount = ConfigurationManager.AppSettings("search_period_3")
                                    Case Else
                                        amount = "0"
                                End Select
                                If (total_amount <> amount) Then
                                    json("error") = "Your GRN Amount should be <i class='fa fa-rupee-sign' aria-hidden='true'></i> " + amount + "/-. Please pay this amount in GRIPS and submit correct GRN."
                                    json("xml_data") = xmlDoc.InnerXml
                                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
                                Else
                                    'Save_GRN_Details_Ag_Search() start
                                    Dim obj As New WebService_BL
                                    Dim Payment_Reason = "Searching of Marriage Records"
                                    Session("Payment_Reason") = Payment_Reason
                                    Session("gid") = Nothing
                                    Dim rets = obj.Save_GRN_Details_Ag_Search(GRN_NO, Convert.ToInt64(total_amount), Payment_Reason, "")
                                    If rets = "-1" Then     'Duplicate value
                                        json("error") = "GRN is already used"
                                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                                    ElseIf rets = "-2" Then   'Transaction rollback
                                        json("error") = "Internal error occured. Please try again later."
                                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                                    Else
                                        If Convert.ToInt64(rets) >= 1 Then
                                            json("gid") = Convert.ToInt64(rets)
                                            Session("gid") = Convert.ToInt64(rets)
                                        End If
                                        json("success") = "GRN is Valid"
                                        json("xml_data") = xmlDoc.InnerXml
                                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
                                    End If
                                    'Save_GRN_Details_Ag_Search() end
                                End If
                            Case "p"
                                If status = "S" Then
                                    If total_amount = Session("paymt_amt") Then
                                        json("success") = "GRN IS VALID. PLEASE PROCEED"
                                    Else
                                        json("error") = "AMOUNT DOSEN'T MATCH. PLEASE ENTER CORRECT GRN"
                                    End If
                                Else
                                    json("error") = "GRN NOT VALID.ENTER CORRECT GRN"
                                End If
                                json("xml_data") = xmlDoc.InnerXml
                            Case "a"
                                If status = "S" Then
                                    If total_amount = Session("pay_amnt") Then
                                        json("success") = "GRN IS VALID. PLEASE PROCEED"
                                    Else
                                        json("error") = "AMOUNT DOSEN'T MATCH. PLEASE ENTER CORRECT GRN"
                                    End If
                                Else
                                    json("error") = "GRN NOT VALID.ENTER CORRECT GRN"
                                End If
                                json("xml_data") = xmlDoc.InnerXml
                            Case Else
                                response("error") = "Neither Certified Copy nor Search Record option have been selected"
                                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        End Select
                    End If
                    'status_desc = node.SelectSingleNode("STATUS_DESC").InnerText
                Next
            Else
                response("error") = "Incorrect GRN"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
            End If
        Catch ex As Exception
            response("exception") = "true"
            response("exception_message") = ex.Message
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
        Finally
            response = If(response.Count < 1, json, response)
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "application/json"
            HttpContext.Current.Response.Write(javaScriptSerializer.Serialize(response))
        End Try
    End Sub

    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)> _
    Public Sub UpdateGRNAgPrint()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
        Try
            Dim ajax, token As String
            Dim method As String
            ajax = HttpContext.Current.Request.Headers("X-Requested-With")
            token = HttpContext.Current.Request.Headers("X-CSRF-TOKEN")
            method = HttpContext.Current.Request.HttpMethod
            If Not (method = "POST" And ajax = "XMLHttpRequest") Then
                response("error") = "Method not allowed"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.MethodNotAllowed, Integer)
                Exit Sub
            End If
            If token = "" Then
                If Session.Item("Marreg_user_info") Is Nothing Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            Else
                If Not token = Session("CSRF") Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            End If

            Dim aplId As String = If(Not Request("aplId") Is Nothing, DirectCast(Request("aplId"), String).Trim(), "")
            Dim App_Auto_Cd As String = If(Not Request("AppAutoCd") Is Nothing, DirectCast(Request("AppAutoCd"), String).Trim(), "")
            Dim gid As String = If(Not Request("gid") Is Nothing And Request("gid") <> "", DirectCast(Request("gid"), String).Trim(), "0")
            Dim id As Int64 = DirectCast(Session("gid"), Int64)
            If (id < 1 Or id <> Convert.ToInt64(gid)) Then
                response("error") = "Please initiate the process again."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                Exit Sub
            End If

            Dim obj As New WebService_BL
            Dim rets = obj.Update_GRN_Details_Ag_Print(id, aplId, App_Auto_Cd, Convert.ToString(Session.Item("Payment_Reason")))

            If rets = "-3" Then
                json("success") = "OK"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
            Else
                json("error") = "NOT OK"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
            End If
        Catch ex As Exception
            response("exception") = "true"
            response("exception_message") = ex.Message
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
        Finally
            response = If(response.Count < 1, json, response)
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "application/json"
            HttpContext.Current.Response.Write(javaScriptSerializer.Serialize(response))
        End Try
    End Sub
#End Region

#Region "Search Old Data"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub SearchOldData()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
        Dim connection As SqlConnection = New SqlConnection()

        Try
            Dim auth = 0
            Dim ajax, token As String
            Dim method As String
            ajax = HttpContext.Current.Request.Headers("X-Requested-With")
            token = HttpContext.Current.Request.Headers("X-CSRF-TOKEN")
            method = HttpContext.Current.Request.HttpMethod
            If Not (method = "POST" And ajax = "XMLHttpRequest") Then
                response("error") = "Method not allowed"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.MethodNotAllowed, Integer)
                Exit Sub
            End If
            If token = "" Then
                auth = 1
                If Session.Item("Marreg_user_info") Is Nothing Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            Else
                If Not token = Session("CSRF") Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            End If

            'If Session.Item("Marreg_user_info") Is Nothing Then
            '    response("error") = "Invalid request"
            '    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
            '    Exit Sub
            'End If

            Dim draw As String = If(Not Request("draw") Is Nothing, DirectCast(Request("draw"), String), "1")
            Dim start As String = If(Not Request("start") Is Nothing, DirectCast(Request("start"), String), "0")
            Dim length As String = If(Not Request("length") Is Nothing, DirectCast(Request("length"), String), "10")
            Dim value As String = If(Not Request("search[value]") Is Nothing, DirectCast(Request("search[value]"), String).Trim(), "")
            Dim orderByColumn As String = If(Request("order[0][column]") Is Nothing, "", DirectCast(Request("order[0][column]"), String))
            Dim orderByDir As String = If(Request("order[0][dir]") Is Nothing, "", DirectCast(Request("order[0][dir]"), String))
            Dim order_by As String = "Reg_dt"

            Dim groom_name As String = If(Not Request("groom_name") Is Nothing, DirectCast(Request("groom_name"), String).Trim(), Nothing)
            Dim bride_name As String = If(Not Request("bride_name") Is Nothing, DirectCast(Request("bride_name"), String).Trim(), Nothing)
            Dim bride_id As String = If(Not Request("bride_id") Is Nothing, DirectCast(Request("bride_id"), String).Trim(), Nothing)
            Dim groom_id As String = If(Not Request("groom_id") Is Nothing, DirectCast(Request("groom_id"), String).Trim(), Nothing)
            Dim app_id As String = If(Not Request("app_id") Is Nothing, DirectCast(Request("app_id"), String).Trim(), Nothing)
            Dim mo_name As String = If(Not Request("mo_name") Is Nothing, DirectCast(Request("mo_name"), String).Trim(), Nothing)
            Dim marriage_dt As String = If(Not Request("marriage_dt") Is Nothing, DirectCast(Request("marriage_dt").Trim(), String), Nothing)
            Dim act_cd As String = If(Not Request("act_cd") Is Nothing, DirectCast(Request("act_cd"), String).Trim(), Nothing)
            Dim from_dt As String = If(Not Request("from_dt") Is Nothing, DirectCast(Request("from_dt").Trim(), String), Nothing)
            Dim to_dt As String = If(Not Request("to_dt") Is Nothing, DirectCast(Request("to_dt").Trim(), String), Nothing)
            Dim finalize As String = If(Not Request("finalize") Is Nothing, DirectCast(Request("finalize").Trim(), String), "true")
            Dim gid As String = If(Not Request("gid") Is Nothing And Request("gid") <> "", DirectCast(Request("gid"), String).Trim(), "0")

            Dim id As Int64 = DirectCast(Session("gid"), Int64)
            If (id < 1 Or id <> Convert.ToInt64(gid)) Then
                response("error") = "Please initiate the process again."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                Exit Sub
            End If
            If ((groom_name.Length < 3 Or bride_name.Length < 3)) Then
                response("error") = "Please Enter: <br>Groom Name and Bride Name Properly!"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (Not from_dt Is Nothing And Not to_dt Is Nothing) Then
                If ((Not from_dt.Equals("") And to_dt.Equals("")) Or (from_dt.Equals("") And Not to_dt.Equals(""))) Then
                    response("error") = "Please Fill Registry Date Range Properly!"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
            End If


            Dim is_search As Boolean = False
            Dim is_order As Boolean = False
            Select Case orderByColumn
                Case "0"
                    order_by = "Groom_Name"
                Case "1"
                    order_by = "Bride_Name"
                Case "2"
                    order_by = "Reg_dt"
                Case "3"
                    order_by = "Reg_PO"
                Case "4"
                    order_by = "Reg_PS"
                Case "5"
                    order_by = "Reg_Dist"
                Case Else
                    order_by = "Reg_dt"
            End Select
            is_order = True

            Dim sql As String = "SELECT [id],[Scan_App_Id],[Groom_FNam],[Groom_MNam],[Groom_LNam] "
            sql += ",case when [Groom_MNam] is not null and ([Groom_MNam]) <>'' then [Groom_FNam]+' '+[Groom_MNam]+' '+[Groom_LNam] else [Groom_FNam]+' '+[Groom_LNam] end as [Groom_Name] "
            sql += ",[Groom_Father_Nam],[Groom_Mother_Nam],[Groom_Age] "
            sql += ",[Groom_ID_Proof_Type],replace([Groom_ID_No],'''','') as [Groom_ID_No],[Bride_Fnam],[Bride_Mnam],[Bride_Lnam] "
            sql += ",case when [Bride_MNam] is not null and ([Bride_MNam]) <>'' then [Bride_FNam]+' '+[Bride_MNam]+' '+[Bride_LNam] else [Bride_FNam]+' '+[Bride_LNam] end as [Bride_Name] "
            sql += ",[Bride_Father_Nam],[Bride_Mother_Nam] "
            sql += ",[Bride_Age],[Bride_ID_Proof_Type],replace([Bride_ID_No],'''','') as [Bride_ID_No],[Appl_ID],[Cert_Year],[Cert_SlNo],[ACT_cd],[Mo_Name] "
            sql += ",convert(varchar,cast(right(replicate('0',8-len([Reg_dt]))+[Reg_dt], 4)+substring(replicate('0',8-len([Reg_dt]))+[Reg_dt], 3, 2)+left(replicate('0',8-len([Reg_dt]))+[Reg_dt], 2) as date),105) as [Reg_dt] "
            sql += ",[Reg_state],[Reg_Dist],[Reg_PO],[Reg_PS],[Reg_Add],[scan_pages],[Image_Path],[Image_Name] "
            sql += "FROM [scanned_records].[dbo].[MARREG_Old_Records]"

            Dim condition As String = " where [id] is not null"

            If Not (value Is Nothing Or value = "") Then
                condition += " and ("
                condition += "  case when [Groom_MNam] is not null and ([Groom_MNam]) <>'' then lower([Groom_FNam]+' '+[Groom_MNam]+' '+[Groom_LNam]) else lower([Groom_FNam]+' '+[Groom_LNam]) end  like lower('%" + value + "%')"
                condition += " or case when [Bride_MNam] is not null and ([Bride_MNam]) <>'' then lower([Bride_FNam]+' '+[Bride_MNam]+' '+[Bride_LNam]) else lower([Bride_FNam]+' '+[Bride_LNam]) end  like lower('%" + value + "%')"
                condition += " or (lower(Reg_PO)) like lower('%" + value + "%') "
                condition += " or (lower(Reg_PS)) like lower('%" + value + "%') "
                condition += " or (lower(Reg_Dist)) like lower('%" + value + "%') "
                condition += " or convert(varchar, cast(right(replicate('0',8-len([Reg_dt]))+[Reg_dt], 4)+substring(replicate('0',8-len([Reg_dt]))+[Reg_dt], 3, 2)+left(replicate('0',8-len([Reg_dt]))+[Reg_dt], 2) as date), 105) like '%" + value + "%'"
                condition += " )"
                is_search = True
            End If

            If Not (groom_name Is Nothing Or groom_name = "") Then
                If auth = 1 Then
                    condition += " and (case when [Groom_MNam] is not null and ([Groom_MNam]) <>'' then lower([Groom_FNam]+' '+[Groom_MNam]+' '+[Groom_LNam]) else lower([Groom_FNam]+' '+[Groom_LNam]) end  like lower(REPLACE(REPLACE(REPLACE(LTRIM(RTRIM('" + groom_name + "%')),'  ',' '+CHAR(7)),CHAR(7)+' ',''),CHAR(7),'')))"
                Else
                    condition += " and (case when [Groom_MNam] is not null and ([Groom_MNam]) <>'' then lower([Groom_FNam]+' '+[Groom_MNam]+' '+[Groom_LNam]) else lower([Groom_FNam]+' '+[Groom_LNam]) end  = lower(REPLACE(REPLACE(REPLACE(LTRIM(RTRIM('" + groom_name + "')),'  ',' '+CHAR(7)),CHAR(7)+' ',''),CHAR(7),'')))"
                End If
                is_search = True
            End If
            If Not (bride_name Is Nothing Or bride_name = "") Then
                If auth = 1 Then
                    condition += " and (case when [Bride_MNam] is not null and ([Bride_MNam]) <>'' then lower([Bride_FNam]+' '+[Bride_MNam]+' '+[Bride_LNam]) else lower([Bride_FNam]+' '+[Bride_LNam]) end  like lower(REPLACE(REPLACE(REPLACE(LTRIM(RTRIM('" + bride_name + "%')),'  ',' '+CHAR(7)),CHAR(7)+' ',''),CHAR(7),'')))"
                Else
                    condition += " and (case when [Bride_MNam] is not null and ([Bride_MNam]) <>'' then lower([Bride_FNam]+' '+[Bride_MNam]+' '+[Bride_LNam]) else lower([Bride_FNam]+' '+[Bride_LNam]) end  = lower(REPLACE(REPLACE(REPLACE(LTRIM(RTRIM('" + bride_name + "')),'  ',' '+CHAR(7)),CHAR(7)+' ',''),CHAR(7),'')))"
                End If

                is_search = True
            End If
            If Not (bride_id Is Nothing Or bride_id = "") Then
                condition += " and lower(replace([Bride_ID_No],'''','')) = lower('" + bride_id + "')"
                is_search = True
            End If
            If Not (groom_id Is Nothing Or groom_id = "") Then
                condition += "  and lower(replace([Groom_ID_No],'''','')) = lower('" + groom_id + "')"
                is_search = True
            End If
            If Not (app_id Is Nothing Or app_id = "") Then
                condition += " and lower([Appl_ID]) = lower('" + app_id + "')"
                is_search = True
            End If
            If Not (mo_name Is Nothing Or mo_name = "") Then
                If auth = 1 Then
                    condition += " and lower([Mo_Name]) like lower(REPLACE(REPLACE(REPLACE(LTRIM(RTRIM('" + mo_name + "%')),'  ',' '+CHAR(7)),CHAR(7)+' ',''),CHAR(7),''))"
                Else
                    condition += " and lower([Mo_Name]) = lower(REPLACE(REPLACE(REPLACE(LTRIM(RTRIM('" + mo_name + "')),'  ',' '+CHAR(7)),CHAR(7)+' ',''),CHAR(7),''))"
                End If

                is_search = True
            End If
            If Not (marriage_dt Is Nothing Or marriage_dt = "") Then
                condition += " and convert(datetime,convert(varchar,cast(right(replicate('0',8-len([Reg_dt]))+[Reg_dt], 4)+substring(replicate('0',8-len([Reg_dt]))+[Reg_dt], 3, 2)+left(replicate('0',8-len([Reg_dt]))+[Reg_dt], 2) as date),105),105) = convert(datetime,'" + marriage_dt + "',105)"
                is_search = True
            End If
            If Not (act_cd Is Nothing Or act_cd = "") Then
                condition += " and lower([ACT_cd]) = lower('" + act_cd + "')"
                is_search = True
            End If
            If Not (from_dt Is Nothing Or from_dt = "") Then
                condition += " and convert(datetime,convert(varchar,cast(right(replicate('0',8-len([Reg_dt]))+[Reg_dt], 4)+substring(replicate('0',8-len([Reg_dt]))+[Reg_dt], 3, 2)+left(replicate('0',8-len([Reg_dt]))+[Reg_dt], 2) as date),105),105) >= convert(datetime,'" + from_dt + "',105)"
                is_search = True
            End If
            If Not (to_dt Is Nothing Or to_dt = "") Then
                condition += " and convert(datetime,convert(varchar,cast(right(replicate('0',8-len([Reg_dt]))+[Reg_dt], 4)+substring(replicate('0',8-len([Reg_dt]))+[Reg_dt], 3, 2)+left(replicate('0',8-len([Reg_dt]))+[Reg_dt], 2) as date),105),105) <= convert(datetime,'" + to_dt + "',105)"
                is_search = True
            End If

            Dim count_sql As String = "select count(*) as count  FROM [scanned_records].[dbo].[MARREG_Old_Records]"
            If is_order Then
                sql += If(Not condition Is Nothing, condition, "") + " order by " + order_by + " offset " + start + " rows fetch next " + length + " rows only;"
            Else
                sql += If(Not condition Is Nothing, condition, "") + " offset " + start + " rows fetch next " + length + " rows only;"
            End If

            connection.ConnectionString = ConfigurationManager.ConnectionStrings("scanned_records").ConnectionString
            connection.Open()
            Dim adp As SqlDataAdapter = New SqlDataAdapter(sql, connection)
            Dim ds As DataSet = New DataSet()
            adp.Fill(ds)
            Dim dt As New DataTable
            dt = ds.Tables(0)
            Dim count = dt.Rows.Count
            Dim data As New List(Of Dictionary(Of String, String))
            If count >= 1 Then
                For Each drRow As DataRow In dt.Rows
                    Dim img_path = drRow.Item("Image_Path").ToString
                    Dim imp() As String = img_path.Split(New String() {"\"}, StringSplitOptions.None)
                    'img_path = "..\" + ConfigurationManager.AppSettings("MARRIAGE_OLD_DOC_DIR") + "\" + imp(4) + "\" + drRow.Item("Mo_Name").ToString + "\" + drRow.Item("Cert_Year").ToString + "\" + drRow.Item("ACT_cd").ToString + "\" + imp(5)
                    img_path = Common.baseUrl() + ConfigurationManager.AppSettings("MARRIAGE_OLD_DOC_DIR") + "\" + imp(4) + "\" + drRow.Item("Mo_Name").ToString + "\" + drRow.Item("Cert_Year").ToString + "\" + drRow.Item("ACT_cd").ToString + "\" + imp(5)
                    Dim arr As New Dictionary(Of String, String)
                    arr.Add("id", drRow.Item("id").ToString)
                    arr.Add("Groom_Name", drRow.Item("Groom_Name").ToString)
                    arr.Add("Bride_Name", drRow.Item("Bride_Name").ToString)
                    arr.Add("Reg_dt", drRow.Item("Reg_dt").ToString)
                    arr.Add("Reg_PO", drRow.Item("Reg_PO").ToString)
                    arr.Add("Reg_PS", drRow.Item("Reg_PS").ToString)
                    arr.Add("Reg_Dist", drRow.Item("Reg_Dist").ToString)
                    arr.Add("Image_Path", enc.encrypt(img_path))
                    arr.Add("Image_Path2", img_path)
                    arr.Add("Scan_App_Id", drRow.Item("Scan_App_Id").ToString)

                    arr.Add("Groom_Father_Nam", drRow.Item("Groom_Father_Nam").ToString)
                    arr.Add("Groom_Mother_Nam", drRow.Item("Groom_Mother_Nam").ToString)
                    arr.Add("Groom_Age", drRow.Item("Groom_Age").ToString)
                    arr.Add("Groom_ID_Proof_Type", drRow.Item("Groom_ID_Proof_Type").ToString)
                    arr.Add("Groom_ID_No", drRow.Item("Groom_ID_No").ToString)

                    arr.Add("Bride_Father_Nam", drRow.Item("Bride_Father_Nam").ToString)
                    arr.Add("Bride_Mother_Nam", drRow.Item("Bride_Mother_Nam").ToString)
                    arr.Add("Bride_Age", drRow.Item("Bride_Age").ToString)
                    arr.Add("Bride_ID_Proof_Type", drRow.Item("Bride_ID_Proof_Type").ToString)
                    arr.Add("Bride_ID_No", drRow.Item("Bride_ID_No").ToString)

                    arr.Add("Appl_ID", drRow.Item("Appl_ID").ToString)
                    arr.Add("Cert_Year", drRow.Item("Cert_Year").ToString)
                    arr.Add("Cert_SlNo", drRow.Item("Cert_SlNo").ToString)
                    arr.Add("ACT_cd", drRow.Item("ACT_cd").ToString)
                    arr.Add("Mo_Name", drRow.Item("Mo_Name").ToString)
                    arr.Add("Reg_state", drRow.Item("Reg_state").ToString)
                    arr.Add("Reg_Add", drRow.Item("Reg_Add").ToString)

                    data.Add(arr)
                Next
            End If
            sql = count_sql

            Dim adp1 As SqlDataAdapter = New SqlDataAdapter(sql, connection)
            Dim ds1 As DataSet = New DataSet()
            adp1.Fill(ds1)
            Dim dt1 As New DataTable
            dt1 = ds1.Tables(0)
            Dim res = dt1.Rows(0)
            count = If(is_search, count, res("count"))

            If is_search Then
                Dim adp2 As SqlDataAdapter = New SqlDataAdapter(count_sql + condition, connection)
                Dim ds2 As DataSet = New DataSet()
                adp2.Fill(ds2)
                Dim dt2 As New DataTable
                dt2 = ds2.Tables(0)
                count = dt2.Rows(0)("count")
            End If

            json("draw") = draw
            json("recordsTotal") = res("count")
            json("recordsFiltered") = count
            json("search_data") = data

            If (finalize.Equals("true")) Then
                Dim obj As New WebService_BL
                Dim aplId = If(data.Count > 0, data.Item(0)("Appl_ID").ToString(), "")
                Dim rets = obj.Update_GRN_Details_Ag_Print(id, aplId, "", Convert.ToString(Session.Item("Payment_Reason")))
            End If
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
        Catch ex As Exception
            response("exception") = "true"
            response("exception_message") = ex.Message
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
        Finally
            connection.Close()
            response = If(response.Count < 1, json, response)
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "application/json"
            'HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*")
            HttpContext.Current.Response.Write(javaScriptSerializer.Serialize(response))
        End Try
    End Sub
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub SearchRequestInfoValidateOld()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
        Try
            Dim ajax, token As String
            Dim method As String
            ajax = HttpContext.Current.Request.Headers("X-Requested-With")
            token = HttpContext.Current.Request.Headers("X-CSRF-TOKEN")
            method = HttpContext.Current.Request.HttpMethod
            If Not (method = "POST" And ajax = "XMLHttpRequest") Then
                response("error") = "Method not allowed"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.MethodNotAllowed, Integer)
                Exit Sub
            End If
            If token = "" Then
                If Session.Item("Marreg_user_info") Is Nothing Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            Else
                If Not token = Session("CSRF") Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            End If
            Dim groom_name As String = If(Not Request("groom_name") Is Nothing, DirectCast(Request("groom_name"), String).Trim(), "")
            Dim groom_father As String = If(Not Request("groom_father") Is Nothing, DirectCast(Request("groom_father"), String).Trim(), "")
            Dim groom_dob As String = If(Not Request("groom_dob") Is Nothing, DirectCast(Request("groom_dob"), String).Trim(), "")

            Dim bride_name As String = If(Not Request("bride_name") Is Nothing, DirectCast(Request("bride_name"), String).Trim(), "")
            Dim bride_father As String = If(Not Request("bride_father") Is Nothing, DirectCast(Request("bride_father"), String).Trim(), "")
            Dim bride_dob As String = If(Not Request("bride_dob") Is Nothing, DirectCast(Request("bride_dob"), String).Trim(), "")

            Dim mo_name As String = If(Not Request("mo_name") Is Nothing, DirectCast(Request("mo_name"), String).Trim(), "")
            Dim ActCd As String = If(Not Request("ActCd") Is Nothing, DirectCast(Request("ActCd"), String).Trim(), "")
            Dim from_date As String = If(Not Request("from_date") Is Nothing, DirectCast(Request("from_date"), String).Trim(), "")
            Dim to_date As String = If(Not Request("to_date") Is Nothing, DirectCast(Request("to_date"), String).Trim(), "")

            If groom_name <> "" Or groom_father <> "" Or groom_dob <> "" Then
                If groom_name = "" Or groom_name Is Nothing Then
                    response("error") = "Please enter Groom/ Husband Name."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If groom_father = "" Or groom_father Is Nothing Then
                    response("error") = "Please enter Groom/ Husband Father Name."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If groom_dob = "" Or groom_dob Is Nothing Then
                    response("error") = "Please enter Groom/ Husband DOB."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
            ElseIf bride_name <> "" Or bride_father <> "" Or bride_dob <> "" Then
                If bride_name = "" Or bride_name Is Nothing Then
                    response("error") = "Please enter Bride/ Wife Name."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If bride_father = "" Or bride_father Is Nothing Then
                    response("error") = "Please enter Bride/ Wife Father Name."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If bride_dob = "" Or bride_dob Is Nothing Then
                    response("error") = "Please enter Bride/ Wife DOB."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
            Else
                response("error") = "Please enter Groom/ Husband Name, Groom/ Husband Father Name, Groom/ Husband DOB <br>or<br> Bride/ Wife Name, Bride/ Wife Father Name, Bride/ Wife DOB."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If

            'If mo_name = "" Or mo_name Is Nothing Then
            '    response("error") = "Please enter MO Name."
            '    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
            '    Exit Sub
            'End If
            'If ActCd = "" Or ActCd Is Nothing Or ActCd = "0" Then
            '    response("error") = "Please select Act/ Section under which the Marriage was Solemnized/ Registered."
            '    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
            '    Exit Sub
            'End If
            If from_date = "" Or from_date Is Nothing Then
                response("error") = "Please enter Registry Date From."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If to_date = "" Or to_date Is Nothing Then
                response("error") = "Please enter Registry Date To."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If

            Dim obj As New WebService_BL
            Dim rets = obj.SearchRequestInfoValidateOld(groom_name, bride_name, groom_dob, bride_dob, groom_father, bride_father, mo_name, ActCd, from_date, to_date)
            'Dim rets = "1"
            If rets = "-2" Then   'Transaction rollback
                json("error") = "Internal error occured. Please try again later."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
            Else
                json("success") = "Ok"
                json("id") = rets
                Session("validatedId") = rets
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
            End If

        Catch ex As Exception
            response("exception") = "true"
            response("exception_message") = ex.Message
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
        Finally
            response = If(response.Count < 1, json, response)
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "application/json"
            HttpContext.Current.Response.Write(javaScriptSerializer.Serialize(response))
        End Try
    End Sub
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub SearchRequestEmailValidateOld()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
        Try
            Dim ajax, token As String
            Dim method As String
            ajax = HttpContext.Current.Request.Headers("X-Requested-With")
            token = HttpContext.Current.Request.Headers("X-CSRF-TOKEN")
            method = HttpContext.Current.Request.HttpMethod
            If Not (method = "POST" And ajax = "XMLHttpRequest") Then
                response("error") = "Method not allowed"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.MethodNotAllowed, Integer)
                Exit Sub
            End If
            If token = "" Then
                If Session.Item("Marreg_user_info") Is Nothing Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            Else
                If Not token = Session("CSRF") Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            End If
            Dim name As String = If(Not Request("name") Is Nothing, DirectCast(Request("name"), String).Trim(), "")
            Dim mob_no As String = If(Not Request("mob_no") Is Nothing, DirectCast(Request("mob_no"), String).Trim(), "")
            Dim address As String = If(Not Request("address") Is Nothing, DirectCast(Request("address"), String).Trim(), "")
            Dim email As String = If(Not Request("email") Is Nothing, DirectCast(Request("email"), String).Trim(), "")
            Dim validatedId As String = If(Not (Request("validatedId") Is Nothing Or Request("validatedId") = ""), DirectCast(Request("validatedId"), String).Trim(), "0")
            Dim vId = Convert.ToInt64(Session("validatedId"))
            email = a.sqlInj_in(email)
            If name = "" Or name Is Nothing Then
                response("error") = "Please Enter Your Full Name."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If address = "" Or address Is Nothing Then
                response("error") = "Please Enter Your Address."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If mob_no = "" Or mob_no Is Nothing Then
                response("error") = "Please Enter Your Mobile No."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If email = "" Or email Is Nothing Then
                response("error") = "Please Enter Your Email ID."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (vId < 1 Or vId <> Convert.ToInt64(validatedId)) Then
                response("error") = "Please initiate the process again."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                Exit Sub
            End If
            Dim obj As New WebService_BL
            Dim rets = obj.SearchRequestSaveEmailOld(name, address, mob_no, email, validatedId)
            If rets = "" Then
                json("error") = "Please try again."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
            Else
                Dim email_ids As New List(Of String)
                email_ids.Add(a.sqlInj_out(email))

                'Dim sql = "select * from MARREG_Email_Format where lower(email_type)=lower('OTP for Request of Searching of Marriage Record')"
                'Dim d = a.selectdata(sql)
                Dim obj1 As New WS_MasterData_BL()
                Dim ds = obj1.FetchEmailFormat(OTP_for_Request_of_Searching_of_Marriage_Record)
                Dim d = ds.Tables(0)
                If (d.Rows.Count = 0) Then
                    json("error") = "Unable to send OTP. Please contact Administrator."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                Dim subject = d.Rows(0)("email_subject")
                Dim body = d.Rows(0)("email_body")
                Dim oldStr() As String = {"{otp}"}
                Dim newStr() As String = {rets}
                Common.ReplaceEmailBody(body, oldStr, newStr)

                'Dim subject = "OTP for Request of Searching of Marriage Record"
                'Dim body = "<b>DO NOT REPLY TO THIS E-MAIL.USE THE E-MAIL OF MARRIAGE REGISTRAR MENTIONED BELOW </b><br/>" &
                '                "<br/>DEAR MADAM/SIR <br/>" &
                '                "<b>SUBJECT:</b> OTP for Request of Searching of Marriage Record" &
                '                "<br/>OTP TO VERIFY EMAIL FOR REQUEST OF SEARCHING OF MARRIAGE RECORD IS " & rets & "." &
                '                "<br/><br/>WITH REGARDS <br/>REGISTRAR GENERAL OF MARRIAGES <br/>LAW DEPARTMENT <br/>GOVERNMENT OF WEST BENGAL <br/> rgm-wb@nic.in"
                Dim email_stat = Send_Email_Bytes(email_ids, subject, body)
                If (email_stat = "0") Then
                    json("success") = "OTP has been sent successfully to your Email. Please verify the OTP."
                ElseIf (email_stat = "1") Then
                    json("error") = "Error occured in sending OTP. Please try again."
                Else
                    json("error") = "Unknown error occured"
                End If
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
            End If

        Catch ex As Exception
            response("exception") = "true"
            response("exception_message") = ex.Message
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
        Finally
            response = If(response.Count < 1, json, response)
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "application/json"
            HttpContext.Current.Response.Write(javaScriptSerializer.Serialize(response))
        End Try
    End Sub
    Private Function Send_Email_Bytes(email_ids As List(Of String), subject As String, body As String, Optional filebytes As List(Of Byte()) = Nothing) As String
        Dim MS As String
        MS = SendMailMessage_Bytes(email_ids, subject, body, filebytes)
        'MS = eml.send_email_multiple_bytes(email_ids, subject, body, filebytes)
        'If filepath = "" Or filepath Is Nothing Then
        '    MS = eml.send_email(email_id, subject, body)
        'Else
        '    MS = eml.send_email_attachment(email_id, subject, body, filepath)
        'End If
        Return MS
    End Function
    Public Function SendMailMessage_Bytes(ByVal recepients As List(Of String), ByVal subject As String, ByVal body As String, Optional fle As List(Of Byte()) = Nothing) As String
        Dim ret = ""
        Dim mMailMessage As New Net.Mail.MailMessage() With {
            .From = New Net.Mail.MailAddress("utsab.87@gmail.com")
        }
        For Each r As String In recepients
            mMailMessage.To.Add(New Net.Mail.MailAddress(r))
        Next

        'mMailMessage.Subject = Mid(subject, 1, 50)
        mMailMessage.Subject = subject
        mMailMessage.Body = body

        If fle IsNot Nothing Then
            For x As Integer = 0 To fle.Count - 1
                'If Not Byte(fle.Item(x)) Then
                mMailMessage.Attachments.Add(New Net.Mail.Attachment(New MemoryStream(fle.Item(x)), subject.Replace(" ", "_") + "_" + (x + 1).ToString() + ".pdf"))
                'End If
            Next
        End If

        mMailMessage.IsBodyHtml = True
        mMailMessage.Priority = Net.Mail.MailPriority.Normal
        Dim mSmtpClient As New Net.Mail.SmtpClient() With {
            .EnableSsl = True,
            .Host = "smtp.gmail.com",
            .Port = 587,
        .Credentials = New Net.NetworkCredential("utsab.87@gmail.com", "tmnnusptedrtkcdq")
        }
        ' Send the mail message
        Try
            mSmtpClient.Send(mMailMessage)
            ret = "0"
        Catch ex As Exception
            ret = "1"
        End Try
        Return ret
    End Function
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub SearchRequestEmailOTPValidateOld()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
        Try
            Dim ajax, token As String
            Dim method As String
            ajax = HttpContext.Current.Request.Headers("X-Requested-With")
            token = HttpContext.Current.Request.Headers("X-CSRF-TOKEN")
            method = HttpContext.Current.Request.HttpMethod
            If Not (method = "POST" And ajax = "XMLHttpRequest") Then
                response("error") = "Method not allowed"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.MethodNotAllowed, Integer)
                Exit Sub
            End If
            If token = "" Then
                If Session.Item("Marreg_user_info") Is Nothing Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            Else
                If Not token = Session("CSRF") Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            End If
            Dim validatedId As String = If(Not (Request("validatedId") Is Nothing Or Request("validatedId") = ""), DirectCast(Request("validatedId"), String).Trim(), "0")
            Dim otp As String = If(Not Request("otp") Is Nothing, DirectCast(Request("otp"), String).Trim(), "")
            Dim vId = Convert.ToInt64(Session("validatedId"))

            If (vId < 1 Or vId <> Convert.ToInt64(validatedId)) Then
                response("error") = "Please initiate the process again."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                Exit Sub
            End If
            If otp = "" Or otp Is Nothing Then
                response("error") = "Please enter OTP."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            Dim obj As New WebService_BL
            Dim rets = obj.SearchRequestEmailOTPValidateOld(validatedId, otp)
            If rets = 1 Then
                json("success") = "OTP Verified."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
            ElseIf rets = 0 Then
                json("error") = "No OTP found."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
            Else
                json("error") = "Invalid OTP entered. Please try again."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
            End If

        Catch ex As Exception
            response("exception") = "true"
            response("exception_message") = ex.Message
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
        Finally
            response = If(response.Count < 1, json, response)
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "application/json"
            HttpContext.Current.Response.Write(javaScriptSerializer.Serialize(response))
        End Try
    End Sub
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub SearchRequestSendOld()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
        Try
            Dim ajax, token As String
            Dim method As String
            ajax = HttpContext.Current.Request.Headers("X-Requested-With")
            token = HttpContext.Current.Request.Headers("X-CSRF-TOKEN")
            method = HttpContext.Current.Request.HttpMethod
            If Not (method = "POST" And ajax = "XMLHttpRequest") Then
                response("error") = "Method not allowed"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.MethodNotAllowed, Integer)
                Exit Sub
            End If
            If token = "" Then
                If Session.Item("Marreg_user_info") Is Nothing Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            Else
                If Not token = Session("CSRF") Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            End If
            Dim validatedId As String = If(Not (Request("validatedId") Is Nothing Or Request("validatedId") = ""), DirectCast(Request("validatedId"), String).Trim(), "0")
            Dim gid As String = If(Not Request("gid") Is Nothing And Request("gid") <> "", DirectCast(Request("gid"), String).Trim(), "0")
            Dim reason As String = If(Not Request("reason") Is Nothing And Request("reason") <> "", DirectCast(Request("reason"), String).Trim(), "")
            Dim vId = Convert.ToInt64(Session("validatedId"))

            If (vId < 1 Or vId <> Convert.ToInt64(validatedId)) Then
                response("error") = "Please initiate the process again."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                Exit Sub
            End If

            Dim grn_id = Convert.ToInt64(Session("gid"))
            If (grn_id < 1 Or grn_id <> Convert.ToInt64(gid)) Then
                response("error") = "Please initiate the process again."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                Exit Sub
            End If

            Dim obj As New WebService_BL
            Dim rets = obj.SearchRequestSendOld(vId, grn_id, reason)
            Dim row = rets.Rows(0)
            If row("ret") = "-2" Then
                'rollback
                json("error") = "Unknown error occured. Please try again later."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
            ElseIf row("ret") = "-5" Then
                'duplicate
                json("error") = "Invalid Reason Selected."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
            ElseIf row("ret") = "0" Then
                'data missing
                json("error") = "Data missing."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
            Else
                'Ok
                Dim req_id = row("request_id").ToString()
                Dim r_name = row("r_name").ToString().ToUpper()
                Dim r_address = row("r_address").ToString().ToUpper()
                Dim r_mob = row("r_mob").ToString()
                Dim r_email = a.sqlInj_out(row("email").ToString())
                'json("success") = "Request submitted successfully. Please check your registered Email ID from next 3 working days to 15 working days.<br>Please note your Request ID: <b>" + req_id + "</b> for future reference."
                json("success") = "Request submitted successfully. Please check your registered Email ID.<br>Please note your Request ID: <b>" + req_id + "</b> for future reference."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
                'Sending Acknowledgement to the requestee
                Dim l1 As New Dictionary(Of String, Object)
                l1("name") = r_name
                l1("address") = r_address
                l1("email") = r_email
                l1("req_dt") = DateTime.Today.ToString("dd/MM/yyyy")
                l1("req_id") = req_id
                l1("serve_dt") = DateTime.Today.AddDays(90).ToString("dd/MM/yyyy")
                l1("old") = "YS"                                                    'Updated on 28-05-2024 from 'Y' to 'YS'
                Dim data = javaScriptSerializer.Serialize(l1)
                Dim encData = enc.encrypt(data)
                Dim someUrl = baseUrl() + "MARREG_CertifiedCopy/Acknowledgement.ashx?encData=" + encData
                Dim ack_byte As Byte()
                Using webClient = New Net.WebClient()
                    ack_byte = webClient.DownloadData(someUrl)
                End Using
                Dim r_email_ids As New List(Of String)
                'r_email = "utsab.87@gmail.com"
                r_email_ids.Add(r_email)
                Dim ack_bytes As New List(Of Byte())
                ack_bytes.Add(ack_byte)

                'Dim sql = "select * from MARREG_Email_Format where lower(email_type)=lower('Acknowledgement')"
                'Dim d = a.selectdata(sql)
                Dim obj1 As New WS_MasterData_BL()
                Dim ds = obj1.FetchEmailFormat(Acknowledgement)
                Dim d = ds.Tables(0)
                If (d.Rows.Count = 0) Then
                    json("error") = json("success") + " Unable to send Acknowledgement. Please contact Administrator."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                Dim subject = d.Rows(0)("email_subject")
                Dim body = d.Rows(0)("email_body")
                Dim oldStr() As String = {"{req_id}"}
                Dim newStr() As String = {req_id}
                Common.ReplaceEmailBody(body, oldStr, newStr)

                'Dim email_Body = "<b>DO NOT REPLY TO THIS E-MAIL.USE THE E-MAIL OF MARRIAGE REGISTRAR MENTIONED BELOW </b><br/>" &
                '                "<br/>Dear Madam/ Sir <br/>" &
                '                "<b>SUBJECT:</b> Acknowledgement" &
                '                "<br>" + json("success") + "<br><br>With Regards <br/>REGISTRAR GENERAL OF MARRIAGES <br/>LAW DEPARTMENT <br/>GOVERNMENT OF WEST BENGAL <br/> rgm-wb@nic.in"
                Send_Email_Bytes(r_email_ids, subject, body, ack_bytes)
                'Sending Acknowledgement to the requestee ends

                Session.Remove("validatedId")
                Session.Remove("gid")
                Session.Remove("no_copy")
            End If
        Catch ex As Exception
            response("exception") = "true"
            response("exception_message") = ex.Message
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
        Finally
            response = If(response.Count < 1, json, response)
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "application/json"
            HttpContext.Current.Response.Write(javaScriptSerializer.Serialize(response))
        End Try
    End Sub

    Dim mo_auto_cd As String = ""
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub SearchRequestDetailsOld()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
        Dim myDataTable As New MyNestedDataTable()

        Try
            Dim auth = 0
            Dim ajax, token As String
            Dim method As String
            ajax = HttpContext.Current.Request.Headers("X-Requested-With")
            token = HttpContext.Current.Request.Headers("X-CSRF-TOKEN")
            method = HttpContext.Current.Request.HttpMethod
            If Not (method = "POST" And ajax = "XMLHttpRequest") Then
                response("error") = "Method not allowed"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.MethodNotAllowed, Integer)
                Exit Sub
            End If
            If token = "" Then
                auth = 1
                If Session.Item("Marreg_user_info") Is Nothing Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            Else
                If Not token = Session("CSRF") Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            End If
            Dim user_info As New Marreg_Loginfo
            user_info = DirectCast(Session.Item("Marreg_user_info"), Marreg_Loginfo)
            mo_auto_cd = user_info.MO_cd

            Dim status As String = If(Not (Request("status") Is Nothing Or Request("status") = ""), Convert.ToString(Request("status")), "")
            Dim draw As String = If(Not Request("draw") Is Nothing, DirectCast(Request("draw"), String), "1")
            Dim start As String = If(Not Request("start") Is Nothing, DirectCast(Request("start"), String), "0")
            Dim length As String = If(Not Request("length") Is Nothing, DirectCast(Request("length"), String), "10")
            Dim value As String = If(Not Request("search[value]") Is Nothing, DirectCast(Request("search[value]"), String).Trim(), "")
            Dim orderByColumn As String = If(Request("order[0][column]") Is Nothing, "", DirectCast(Request("order[0][column]"), String))
            Dim orderByDir As String = If(Request("order[0][dir]") Is Nothing, "", DirectCast(Request("order[0][dir]"), String))
            Dim order_by As String = "requested_on"

            'Dim marriage_dt As Date = If(Not Request("marriage_dt") Is Nothing, DateConvertion(Request("marriage_dt").Trim()), Convert.ToDateTime("1700-01-01"))
            'Dim cat As String = If(Not Request("cat") Is Nothing, DirectCast(Request("cat").Trim(), String), "")

            Dim is_search As Boolean = False
            Dim is_order As Boolean = False

            is_order = True

            myDataTable = WebService_BL.SearchRequestDetailsOld(status, draw, start, length, value, order_by, orderByDir)
            'If (Finalize.Equals("true")) Then
            '    Dim id As Int64 = DirectCast(Session("gid"), Int64)
            '    Dim obj As New WebService_BL
            '    Dim App_Auto_Cd = If(myDataTable.data.Count > 0, myDataTable.data.Item(0)("App_Auto_Cd").ToString(), "")
            '    Dim rets = obj.Update_GRN_Details_Ag_Print(id, "", App_Auto_Cd, Convert.ToString(Session.Item("Payment_Reason")))
            'End If
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
        Catch ex As Exception
            response("exception") = "true"
            response("exception_message") = ex.Message + "<br>" + "Click <a onclick='location.reload()' href='#'>here</a> to reload"
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
        Finally
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "application/json"
            If response.Count < 1 Then
                HttpContext.Current.Response.Write(javaScriptSerializer.Serialize(myDataTable))
            Else
                HttpContext.Current.Response.Write(javaScriptSerializer.Serialize(response))
            End If
        End Try
    End Sub
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub SearchRequestApproveSendOTP()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
        Try
            Dim ajax, token As String
            Dim method As String
            ajax = HttpContext.Current.Request.Headers("X-Requested-With")
            token = HttpContext.Current.Request.Headers("X-CSRF-TOKEN")
            method = HttpContext.Current.Request.HttpMethod
            If Not (method = "POST" And ajax = "XMLHttpRequest") Then
                response("error") = "Method not allowed"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.MethodNotAllowed, Integer)
                Exit Sub
            End If
            If token = "" Then
                If Session.Item("Marreg_user_info") Is Nothing Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            Else
                If Not token = Session("CSRF") Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            End If
            Dim id As Int64 = If(Not (Request("id") Is Nothing And Request("id") = ""), Convert.ToInt64(Request("id").Trim()), 0)
            Dim reqid As String = If(Not (Request("reqid") Is Nothing Or Request("reqid") = ""), DirectCast(Request("reqid"), String).Trim(), "")
            Dim status As String = If(Not (Request("status") Is Nothing Or Request("status") = ""), Convert.ToString(Request("status")), "")

            Dim status_array = {"02", "06", "09"}
            If Not status_array.Contains(status) Then
                response("error") = "Invalid status"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                Exit Sub
            End If
            Dim dtl = FetchSearchRequestDetailOld(id, "02")
            If (dtl.Tables(0).Rows.Count > 0) Then
                'Send Email Start
                Dim email_stat = GenerateAndSendEmail_SearchApprove(id, reqid, status)
                If (email_stat = "0") Then      'sent
                    json("success") = "0"
                ElseIf (email_stat = "1") Then  'not send
                    json("success") = "1"
                Else
                    json("success") = "-1"
                End If
                'Send Email End
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
            Else
                json("error") = "Request already processed"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
            End If
            'Dim ds = obj.Details_SearchRecordDataOld(id)


        Catch ex As Exception
            response("exception") = "true"
            response("exception_message") = ex.Message
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
        Finally
            response = If(response.Count < 1, json, response)
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "application/json"
            HttpContext.Current.Response.Write(javaScriptSerializer.Serialize(response))
        End Try
    End Sub
    Private Function GenerateAndSendEmail_SearchApprove(id As Int64, reqid As String, status As String) As String
        Dim retVal As String = ""
        Dim r_email As String

        Dim email_ids As New List(Of String)
        Dim obj As New WebService_BL
        Try

            'r_email = "rgm-wb@nic.in"
            'r_email = "destiny.utsab@gmail.com"
            r_email = "wbrgm2021@gmail.com"   'Added on 03-04-2024
            email_ids.Add(r_email)
            Dim otp = ""
            changeOTP(otp)
            Session("ar_otp") = otp
            Session("ar_id") = id
            'Session("ar_otp") = "11111111"
            Dim status_text = ""
            If status = "09" Then
                status_text = "APPROVE"
            ElseIf status = "06" Then
                status_text = "REJECT"
            ElseIf status = "15" Then
                status_text = "ON HOLD"
            End If

            'Dim sql = "select * from MARREG_Email_Format where lower(email_type)=lower('OTP for Process Request')"
            'Dim d = a.selectdata(sql)
            Dim obj1 As New WS_MasterData_BL()
            Dim ds = obj1.FetchEmailFormat(OTP_for_Process_Request)
            Dim d = ds.Tables(0)
            If (d.Rows.Count = 0) Then
                Return "-1"
            End If
            Dim subject = d.Rows(0)("email_subject")
            Dim body = d.Rows(0)("email_body")
            Dim oldStr() As String = {"{status_text}", "{request_id}", "{purpose}", "{otp}"}
            Dim newStr() As String = {status_text, reqid, "SEARCH REQUEST", otp}
            Common.ReplaceEmailBody(body, oldStr, newStr)

            'Dim subject = "OTP"
            'Dim body = "<b>DO NOT REPLY TO THIS E-MAIL.USE THE E-MAIL OF MARRIAGE REGISTRAR MENTIONED BELOW </b><br/>" &
            '                "<br/>DEAR MADAM/SIR <br/>" &
            '                "<br/><b>SUBJECT:</b> OTP" &
            '                "<br/><br/>OTP TO " & status_text & "  REQUEST ID " & reqid & " FOR SEARCH REQUEST IS: " & otp & "." &
            '                "<br/><br/>WITH REGARDS <br/>REGISTRAR GENERAL OF MARRIAGES <br/>LAW DEPARTMENT <br/>GOVERNMENT OF WEST BENGAL <br/> rgm-wb@nic.in"

            retVal = Send_Email_Bytes(email_ids, subject, body)
            'retVal = "0"
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return retVal
    End Function
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub SearchRequestApproveVerifyOTP()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
        Try
            Dim ajax, token As String
            Dim method As String
            ajax = HttpContext.Current.Request.Headers("X-Requested-With")
            token = HttpContext.Current.Request.Headers("X-CSRF-TOKEN")
            method = HttpContext.Current.Request.HttpMethod
            If Not (method = "POST" And ajax = "XMLHttpRequest") Then
                response("error") = "Method not allowed"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.MethodNotAllowed, Integer)
                Exit Sub
            End If
            If token = "" Then
                If Session.Item("Marreg_user_info") Is Nothing Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            Else
                If Not token = Session("CSRF") Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            End If
            Dim id As Int64 = If(Not (Request("id") Is Nothing And Request("id") = ""), Convert.ToInt64(Request("id").Trim()), 0)
            Dim otp As String = If(Not (Request("otp") Is Nothing Or Request("otp") = ""), DirectCast(Request("otp"), String).Trim(), "")
            Dim ar_id = Convert.ToInt64(Session("ar_id"))
            Dim ar_otp = Convert.ToString(Session("ar_otp"))
            If (ar_id < 1 Or ar_id <> Convert.ToInt64(id)) Then
                response("error") = "Please initiate the process again."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                Exit Sub
            End If
            If otp = "" Or otp Is Nothing Then
                response("error") = "Please Enter OTP Received Through Email."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If otp <> ar_otp Then
                response("error") = "Invalid OTP entered. Please try again."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            'Send Email Start
            response("success") = 1
            'Send Email End
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)

        Catch ex As Exception
            response("exception") = "true"
            response("exception_message") = ex.Message
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
        Finally
            response = If(response.Count < 1, json, response)
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "application/json"
            HttpContext.Current.Response.Write(javaScriptSerializer.Serialize(response))
        End Try
    End Sub
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub SearchRequestApproveOld()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
        javaScriptSerializer.MaxJsonLength = Int32.MaxValue
        Dim base64String As String = ""
        Try
            Dim ajax, token As String
            Dim method As String
            ajax = HttpContext.Current.Request.Headers("X-Requested-With")
            token = HttpContext.Current.Request.Headers("X-CSRF-TOKEN")
            method = HttpContext.Current.Request.HttpMethod
            If Not (method = "POST" And ajax = "XMLHttpRequest") Then
                response("error") = "Method not allowed"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.MethodNotAllowed, Integer)
                Exit Sub
            End If
            If token = "" Then
                If Session.Item("Marreg_user_info") Is Nothing Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            Else
                If Not token = Session("CSRF") Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            End If
            Dim id As Int64 = If(Not (Request("id") Is Nothing And Request("id") = ""), Convert.ToInt64(Request("id").Trim()), 0)
            Dim status As String = If(Not (Request("status") Is Nothing Or Request("status") = ""), Convert.ToString(Request("status")), "")
            Dim g_name As String = If(Not (Request("g_name") Is Nothing Or Request("g_name") = ""), Convert.ToString(Request("g_name")), "")
            Dim b_name As String = If(Not (Request("b_name") Is Nothing Or Request("b_name") = ""), Convert.ToString(Request("b_name")), "")
            Dim mo_name As String = If(Not (Request("mo_name") Is Nothing Or Request("mo_name") = ""), Convert.ToString(Request("mo_name")), "")
            Dim act As String = If(Not (Request("act") Is Nothing Or Request("act") = ""), Convert.ToString(Request("act")), "")
            Dim reg_dt As String = If(Not (Request("reg_dt") Is Nothing Or Request("reg_dt") = ""), Convert.ToString(Request("reg_dt")), "")

            Dim file As HttpPostedFile = HttpContext.Current.Request.Files("sfile")

            Dim otp As String = Convert.ToString(Session("ar_otp"))
            Dim user_info As New Marreg_Loginfo
            user_info = DirectCast(Session.Item("Marreg_user_info"), Marreg_Loginfo)
            Dim user_nm = user_info.user_nm
            mo_auto_cd = user_info.MO_cd

            Dim status_array = {"02", "06", "09"}
            If Not status_array.Contains(status) Then
                response("error") = "Invalid status"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                Exit Sub
            End If
            If status = "09" Then       'Approve status
                If g_name = "" Then
                    response("error") = "Please Enter Groom/ Husband Name"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If b_name = "" Then
                    response("error") = "Please Enter Bride/ Wife Name"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If reg_dt = "" Then
                    response("error") = "Please Enter Registration Date"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If act = "" Or act = "0" Then
                    response("error") = "Please Select Act/ Section"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If mo_name = "" Then
                    response("error") = "Please Enter MO Name"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
            End If

            Dim obj As New WebService_BL
            Dim rets = obj.Save_SearchRequestApproveOld(id, status, otp, user_nm, g_name, b_name, reg_dt, act, mo_name)
            If Convert.ToInt64(rets) = 1 Then
                Dim someUrl = baseUrl() + "MARREG_SearchRecords/SearchReportOld.ashx?SearchRecId=" + id.ToString()
                Dim ack_byte As Byte()
                Using webClient = New Net.WebClient()
                    ack_byte = webClient.DownloadData(someUrl)
                End Using
                Dim ret = GenerateAndSendEmailSearchRecOld(id, status, ack_byte)
                If (ret = "0") Then
                    json("success") = "Process done successfully and Email sending success."
                ElseIf (ret = "1") Then
                    json("success") = "Process done successfully and Email sending error."
                Else
                    json("success") = "Process done successfully."
                End If
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
            ElseIf rets = "-1" Then     'Duplicate value
                json("error") = "Please process the request again."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
            ElseIf rets = "-2" Then   'Transaction rollback
                json("error") = "Internal error occured. Please try again later."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
            End If
        Catch ex As Exception
            response("exception") = "true"
            response("exception_message") = ex.Message
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
        Finally
            response = If(response.Count < 1, json, response)
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "application/json"
            HttpContext.Current.Response.Write(javaScriptSerializer.Serialize(response))
        End Try
    End Sub
    Dim obj As New WebService_BL
    Private Function FetchSearchRequestDetailOld(id As Int64, status As String) As DataSet
        Dim requests = obj.FetchSearchRequestDetailOld(id, status)
        Return requests
    End Function
    Private Function GenerateAndSendEmailSearchRecOld(id As Int64, status As String, bytes As Byte()) As String
        Dim retVal As String = ""
        Dim email As String

        Dim objBodySub As New Dictionary(Of String, Object)
        Try
            Dim requests = FetchSearchRequestDetailOld(id, status)
            email = Convert.ToString(requests.Tables(0).Rows(0)("email"))
            email = a.sqlInj_out(email)
            objBodySub = GenSubjectAndBodyEmailOld(status, requests)

            'email = "utsab.87@gmail.com"
            Dim email_ids As New List(Of String)
            Dim filepaths As New List(Of Byte())
            email_ids.Add(email)
            'If (status = "09") Then
            'For i As Integer = 1 To no_copy
            filepaths.Add(bytes)
            'Next
            'End If
            'retVal = Send_Email(email_ids, objBodySub("subject"), objBodySub("body"), filepaths)

            retVal = Send_Email_Bytes(email_ids, objBodySub("subject"), objBodySub("body"), filepaths)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return retVal
    End Function
    Private Function GenSubjectAndBodyEmailOld(status As String, requests As DataSet) As Dictionary(Of String, Object)
        Dim ret As New Dictionary(Of String, Object)
        Try
            Dim groom_name = Convert.ToString(requests.Tables(0).Rows(0)("groom_name"))
            Dim bride_name = Convert.ToString(requests.Tables(0).Rows(0)("bride_name"))
            Dim groom_father = Convert.ToString(requests.Tables(0).Rows(0)("groom_father"))
            Dim bride_father = Convert.ToString(requests.Tables(0).Rows(0)("bride_father"))
            Dim groom_dob = Convert.ToString(requests.Tables(0).Rows(0)("groom_dob"))
            Dim bride_dob = Convert.ToString(requests.Tables(0).Rows(0)("bride_dob"))
            Dim reg_date_from = Convert.ToString(requests.Tables(0).Rows(0)("reg_date_from"))
            Dim reg_date_to = Convert.ToString(requests.Tables(0).Rows(0)("reg_date_to"))
            Dim mo_name = Convert.ToString(requests.Tables(0).Rows(0)("mo_name"))
            Dim req_act = Convert.ToString(requests.Tables(0).Rows(0)("req_act"))
            Dim request_id = Convert.ToString(requests.Tables(0).Rows(0)("request_id"))
            Dim str = "<strong>"
            str += If(groom_name <> "", "Groom/ Husband Name: " + groom_name.ToUpper + "<br>", "")
            str += If(groom_dob <> "", "Groom/ Husband DOB: " + Convert.ToDateTime(groom_dob).ToString("dd/MM/yyyy") + "<br>", "")
            str += If(groom_father <> "", "Groom/ Husband Father Name: " + groom_father.ToUpper + "<br>", "")
            str += If(bride_name <> "", "Bride/ Wife Name: " + bride_name.ToUpper + "<br>", "")
            str += If(bride_dob <> "", "Bride/ Wife DOB: " + Convert.ToDateTime(bride_dob).ToString("dd/MM/yyyy") + "<br>", "")
            str += If(bride_father <> "", "Bride/ Wife Father Name: " + bride_father.ToUpper + "<br>", "")
            str += If(mo_name <> "", "MO Name: " + mo_name.ToUpper + "<br>", "")
            str += If(req_act <> "", "Act/ Section under which the Marriage was Solemnized/ Registered: " + req_act.ToUpper + "<br>", "")
            str += If((reg_date_from <> "" And reg_date_to <> ""), "Registration Date Period: " + Convert.ToDateTime(reg_date_from).ToString("dd/MM/yyyy") + " To " + Convert.ToDateTime(reg_date_to).ToString("dd/MM/yyyy") + "<br>", "")
            str += "</strong>"
            Select Case status
                Case "06"
                    'Dim sql = "select * from MARREG_Email_Format where lower(email_type)=lower('Searching Request Reject')"
                    'Dim d = a.selectdata(sql)
                    Dim obj As New WS_MasterData_BL()
                    Dim ds = obj.FetchEmailFormat(Searching_Request_Reject)
                    Dim d = ds.Tables(0)
                    ret("subject") = d.Rows(0)("email_subject")
                    ret("body") = d.Rows(0)("email_body")
                    Dim oldStr() As String = {"{request_id}", "{request_details}"}
                    Dim newStr() As String = {request_id, str}
                    Common.ReplaceEmailBody(ret("body"), oldStr, newStr)

                    'ret("subject") = "Request for Searching of Marriage Record has been rejected"
                    'ret("body") = "<b>DO NOT REPLY TO THIS E-MAIL.USE THE E-MAIL OF MARRIAGE REGISTRAR MENTIONED BELOW </b><br/>" &
                    '            "<br/>DEAR MADAM/SIR <br/>" &
                    '            "<b>SUBJECT:</b> Request ID " & request_id & " | Request for Searching of Marriage Record has been rejected" &
                    '            "<br/>REQUEST FOR SEARCHING OF THE MARRIAGE RECORD HAS BEEN REJECTED." &
                    '            "<br/>NO RECORD IS FOUND AS PER INFORMATION PROVIDED:" &
                    '            "<br/>" & str &
                    '            "<br/>RESULT IS ATTACHED WITH THIS EMAIL. PLEASE DOWNLOAD AND OPEN THE RESULT." &
                    '            "<br/><br/>WITH REGARDS <br/>REGISTRAR GENERAL OF MARRIAGES <br/>LAW DEPARTMENT <br/>GOVERNMENT OF WEST BENGAL <br/> rgm-wb@nic.in"
                Case "09"

                    'Dim sql = "select f.* from MARREG_Email_Format f inner join MARREG_Email_Type t on t.id=f.email_type_id where f.deleted_on is null and t.deleted_on is null and lower(t.email_type)=lower('" + Searching_Request_Approve + "')"
                    'Dim d = a.selectdata(sql)

                    Dim obj As New WS_MasterData_BL()
                    Dim ds = obj.FetchEmailFormat(Searching_Request_Approve)
                    Dim d = ds.Tables(0)
                    ret("subject") = d.Rows(0)("email_subject")
                    ret("body") = d.Rows(0)("email_body")
                    Dim oldStr() As String = {"{request_id}", "{request_details}"}
                    Dim newStr() As String = {request_id, str}
                    Common.ReplaceEmailBody(ret("body"), oldStr, newStr)

                    'ret("subject") = "Searching of Marriage Record"
                    'ret("body") = "<b>DO NOT REPLY TO THIS E-MAIL.USE THE E-MAIL OF MARRIAGE REGISTRAR MENTIONED BELOW </b><br/>" &
                    '            "<br/>DEAR MADAM/SIR <br/>" &
                    '            "<b>SUBJECT:</b> Request ID " & request_id & " | Searching of Marriage Record" &
                    '            "<br/>REQUEST FOR SEARCHING OF THE MARRIAGE RECORD HAS BEEN ACCEPTED AND PROCESSED AS PER YOUR REQUEST:" &
                    '            "<br/>" & str &
                    '            "<br/>RESULT IS ATTACHED WITH THIS EMAIL. PLEASE DOWNLOAD AND OPEN THE RESULT." &
                    '            "<br/><br/>WITH REGARDS <br/>REGISTRAR GENERAL OF MARRIAGES <br/>LAW DEPARTMENT <br/>GOVERNMENT OF WEST BENGAL <br/> rgm-wb@nic.in"
            End Select
        Catch ex As Exception
            Dim p = ex.Message
            Throw New Exception(ex.Message)
        End Try
        Return ret
    End Function
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub ResendEmailO()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
        Try
            Dim ajax, token As String
            Dim method As String
            ajax = HttpContext.Current.Request.Headers("X-Requested-With")
            token = HttpContext.Current.Request.Headers("X-CSRF-TOKEN")
            method = HttpContext.Current.Request.HttpMethod
            If Not (method = "POST" And ajax = "XMLHttpRequest") Then
                response("error") = "Method not allowed"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.MethodNotAllowed, Integer)
                Exit Sub
            End If
            If token = "" Then
                If Session.Item("Marreg_user_info") Is Nothing Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            Else
                If Not token = Session("CSRF") Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            End If
            Dim id As Int64 = If(Not (Request("id") Is Nothing And Request("id") = ""), Convert.ToInt64(Request("id").Trim()), 0)
            Dim status As String = If(Not (Request("status") Is Nothing Or Request("status") = ""), Convert.ToString(Request("status")), "")
            Dim act_cd As String = If(Not (Request("act") Is Nothing Or Request("act") = ""), DirectCast(Request("act"), String).Trim(), "")

            Dim status_array = {"02", "06", "09"}
            If Not status_array.Contains(status) Then
                response("error") = "Invalid status"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                Exit Sub
            End If
            If status = "09" Then       'Approve status
                Dim act_array = {"01", "02", "03"}
                If Not act_array.Contains(act_cd) Then
                    response("error") = "Invalid Act"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            End If
            'Send Email Start
            'to do get search result byte
            Dim someUrl = baseUrl() + "MARREG_SearchRecords/SearchReportOld.ashx?SearchRecId=" + id.ToString()
            Dim ack_byte As Byte()
            Using webClient = New Net.WebClient()
                ack_byte = webClient.DownloadData(someUrl)
            End Using
            Dim email_stat = GenerateAndSendEmailSearchRecOld(id, status, ack_byte)
            If (email_stat = "0") Then
                json("success") = "Email sent successfully"
            ElseIf (email_stat = "1") Then
                json("success") = "Email could not be sent"
            Else
                json("success") = "Unknown error occured"
            End If
            'Send Email End
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)

        Catch ex As Exception
            response("exception") = "true"
            response("exception_message") = ex.Message
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
        Finally
            response = If(response.Count < 1, json, response)
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "application/json"
            HttpContext.Current.Response.Write(javaScriptSerializer.Serialize(response))
        End Try
    End Sub
#End Region
    Private Function DateConvertion(input As String) As Date
        Dim out_dt As Date = Date.Now
        If (input <> String.Empty And DateTime.TryParse(input, out_dt)) Then
            out_dt = Convert.ToDateTime(input)
        Else
            out_dt = Convert.ToDateTime("1700-01-01")
        End If
        Return out_dt
    End Function
#Region "Search New Data"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)> _
    Public Sub SearchNewData()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
        Dim myDataTable As New MyDataTable()
        Dim connection As SqlConnection = New SqlConnection()

        Try
            Dim auth = 0
            Dim ajax, token As String
            Dim method As String
            ajax = HttpContext.Current.Request.Headers("X-Requested-With")
            token = HttpContext.Current.Request.Headers("X-CSRF-TOKEN")
            method = HttpContext.Current.Request.HttpMethod
            If Not (method = "POST" And ajax = "XMLHttpRequest") Then
                response("error") = "Method not allowed"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.MethodNotAllowed, Integer)
                Exit Sub
            End If
            If token = "" Then
                auth = 1
                If Session.Item("Marreg_user_info") Is Nothing Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            Else
                If Not token = Session("CSRF") Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            End If

            Dim draw As String = If(Not Request("draw") Is Nothing, DirectCast(Request("draw"), String), "1")
            Dim start As String = If(Not Request("start") Is Nothing, DirectCast(Request("start"), String), "0")
            Dim length As String = If(Not Request("length") Is Nothing, DirectCast(Request("length"), String), "10")
            Dim value As String = If(Not Request("search[value]") Is Nothing, DirectCast(Request("search[value]"), String).Trim(), "")
            Dim orderByColumn As String = If(Request("order[0][column]") Is Nothing, "", DirectCast(Request("order[0][column]"), String))
            Dim orderByDir As String = If(Request("order[0][dir]") Is Nothing, "asc", DirectCast(Request("order[0][dir]"), String))
            Dim order_by As String = "Registration_Date"

            Dim groom_name As String = If(Not Request("groom_name") Is Nothing, DirectCast(Request("groom_name"), String).Trim(), Nothing)
            Dim groom_Fname As String = If(Not Request("groom_Fname") Is Nothing, DirectCast(Request("groom_Fname"), String).Trim(), Nothing)
            Dim bride_name As String = If(Not Request("bride_name") Is Nothing, DirectCast(Request("bride_name"), String).Trim(), Nothing)
            Dim bride_Fname As String = If(Not Request("bride_Fname") Is Nothing, DirectCast(Request("bride_Fname"), String).Trim(), Nothing)
            Dim groom_dob As Date = If(Not Request("groom_dob") Is Nothing, DateConvertion(Request("groom_dob").Trim()), Convert.ToDateTime("1700-01-01"))
            Dim bride_dob As Date = If(Not Request("bride_dob") Is Nothing, DateConvertion(Request("bride_dob").Trim()), Convert.ToDateTime("1700-01-01"))
            Dim bride_id As String = If(Not Request("bride_id") Is Nothing, DirectCast(Request("bride_id"), String).Trim(), Nothing)
            Dim groom_id As String = If(Not Request("groom_id") Is Nothing, DirectCast(Request("groom_id"), String).Trim(), Nothing)
            Dim app_no As String = If(Not Request("app_no") Is Nothing, DirectCast(Request("app_no"), String).Trim(), Nothing)
            Dim mo_name As String = If(Not Request("mo_name") Is Nothing, DirectCast(Request("mo_name"), String).Trim(), Nothing)
            Dim marriage_dt As Date = If(Not Request("marriage_dt") Is Nothing, DateConvertion(Request("marriage_dt").Trim()), Convert.ToDateTime("1700-01-01"))
            Dim act_cd As String = If(Not Request("act_cd") Is Nothing, DirectCast(Request("act_cd"), String).Trim(), Nothing)
            Dim from_dt As Date = If(Not Request("from_dt") Is Nothing, DateConvertion(Request("from_dt").Trim()), Convert.ToDateTime("1700-01-01"))
            Dim to_dt As Date = If(Not Request("to_dt") Is Nothing, DateConvertion(Request("to_dt").Trim()), Convert.ToDateTime("1700-01-01"))
            Dim finalize As String = If(Not Request("finalize") Is Nothing, DirectCast(Request("finalize").Trim(), String), "true")
            Dim cat As String = If(Not Request("cat") Is Nothing, DirectCast(Request("cat").Trim(), String), "")
            Dim gid As String = If(Not Request("gid") Is Nothing And Request("gid") <> "", DirectCast(Request("gid"), String).Trim(), "0")
            Dim id As Int64 = Convert.ToInt64(Session("gid"))
            If (id < 1 Or id <> Convert.ToInt64(gid)) Then
                response("error") = "Please initiate the process again."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                Exit Sub
            End If
            Dim is_search As Boolean = False
            Dim is_order As Boolean = False
            'Select Case orderByColumn
            '    Case "0"
            '        order_by = "Groom_Name"
            '    Case "1"
            '        order_by = "Bride_Name"
            '    Case "2"
            '        order_by = "Reg_dt"
            '    Case "3"
            '        order_by = "Reg_PO"
            '    Case "4"
            '        order_by = "Reg_PS"
            '    Case "5"
            '        order_by = "Reg_Dist"
            '    Case Else
            '        order_by = "Registration_Date"
            'End Select
            'is_order = True
            If ((groom_name.Length < 3 Or groom_Fname.Length < 3) And (bride_name.Length < 3 Or bride_Fname.Length < 3)) Then
                response("error") = "Please Enter: <br>Groom/ Husband Name and Father's Name of Groom/ Husband <br>or<br> Bride/ Wife Name and Father's Name of Bride/ Wife Properly!"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (cat = "s") Then
                If ((Not from_dt.Equals("1700-01-01") And to_dt.Equals("1700-01-01")) Or (from_dt.Equals("1700-01-01") And Not to_dt.Equals("1700-01-01"))) Then
                    response("error") = "Please Fill Registry Date Range Properly!"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
            End If

            myDataTable = PublicInfo_BL.NewRecordsListTable(auth, groom_name, groom_Fname, groom_dob, bride_dob, groom_id, bride_name, bride_Fname, bride_id, app_no, mo_name, marriage_dt, act_cd, from_dt, to_dt, draw, start, length, value, order_by, orderByDir)
            If (finalize.Equals("true")) Then
                Dim obj As New WebService_BL
                'Fetching first record App_Auto_Cd
                Dim App_Auto_Cd = If(myDataTable.data.Count > 0, myDataTable.data.Item(0)("App_Auto_Cd").ToString(), "")

                Dim groom_dobs = If(groom_dob.ToString("yyyy-MM-dd").Equals("1700-01-01"), "", groom_dob.ToString("dd-MM-yyyy"))
                Dim bride_dobs = If(bride_dob.ToString("yyyy-MM-dd").Equals("1700-01-01"), "", bride_dob.ToString("dd-MM-yyyy"))
                Dim from_dts = If(from_dt.ToString("yyyy-MM-dd").Equals("1700-01-01"), "", from_dt.ToString("dd-MM-yyyy"))
                Dim to_dts = If(to_dt.ToString("yyyy-MM-dd").Equals("1700-01-01"), "", to_dt.ToString("dd-MM-yyyy"))
                Dim marriage_dts = If(marriage_dt.ToString("yyyy-MM-dd").Equals("1700-01-01"), "", marriage_dt.ToString("dd-MM-yyyy"))
                If (myDataTable.data.Count > 0) Then
                    For Each row As Dictionary(Of String, String) In myDataTable.data
                        Dim AppAutoCd = row("App_Auto_Cd").ToString()
                        Dim SearchRecId = obj.Save_SearchRecordData(groom_name, groom_Fname, bride_name, bride_Fname, act_cd, mo_name, groom_id, bride_id, app_no, groom_dobs, bride_dobs, marriage_dts, from_dts, to_dts, id, AppAutoCd)
                        row.Item("SearchRecId") = SearchRecId.ToString()
                    Next
                Else
                    Dim SearchRec_Id = obj.Save_SearchRecordData(groom_name, groom_Fname, bride_name, bride_Fname, act_cd, mo_name, groom_id, bride_id, app_no, groom_dobs, bride_dobs, marriage_dts, from_dts, to_dts, id, App_Auto_Cd)
                    myDataTable.additional = SearchRec_Id
                End If

                'Update GRN details with first record found
                Dim rets = obj.Update_GRN_Details_Ag_Print(id, "", App_Auto_Cd, Convert.ToString(Session.Item("Payment_Reason")))
            End If
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
        Catch ex As Exception
            response("exception") = "true"
            response("exception_message") = ex.Message + "<br>" + "Click <a onclick='location.reload()' href='#'>here</a> to reload"
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
        Finally
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "application/json"
            If response.Count < 1 Then
                HttpContext.Current.Response.Write(javaScriptSerializer.Serialize(myDataTable))
            Else
                HttpContext.Current.Response.Write(javaScriptSerializer.Serialize(response))
            End If
        End Try
    End Sub
#End Region

#Region "Remote WebService"
    <WebMethod(), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)> _
    Public Sub SaveOtherInfo()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
        Try
            Dim ajax As String
            Dim method As String
            method = HttpContext.Current.Request.HttpMethod
            If Not (method = "POST") Then
                response("error") = "Method not allowed"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If

            Dim apikey As String = If(Not Request("apikey") Is Nothing, DirectCast(Request("apikey"), String).Trim(), "")
            If Not (apikey = ConfigurationManager.AppSettings("api_key").ToString) Then
                response("error") = "Invalid request"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                Exit Sub
            End If

            Dim Mo_Cd As String = If(Not Request("Mo_Cd") Is Nothing, DirectCast(Request("Mo_Cd"), String).Trim(), "")
            Dim Applicant_Cd As String = If(Not Request("Applicant_Cd") Is Nothing, DirectCast(Request("Applicant_Cd"), String).Trim(), "")
            Dim location As String = If(Not Request("location") Is Nothing, DirectCast(Request("location"), String).Trim(), "")
            Dim jointImg As HttpPostedFile = HttpContext.Current.Request.Files("jointImg")

            If Applicant_Cd = "" Then
                response("error") = "Application code needed to process request."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            Else
                Dim obj As New WebService_BL
                Dim ret = obj.ValidateAppCd(Applicant_Cd)
                If ret Is Nothing Or ret <> Mo_Cd Then
                    response("error") = "Invalid application code entered."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
            End If

            If location = "" Then
                response("error") = "Location needed to process request."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If jointImg Is Nothing Then
                response("error") = "No valid file specified. Please select a .JPG, .JPEG or .PNG file to upload."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If


            Dim inputStream As System.IO.Stream = System.IO.Stream.Null
            Dim mStream As IO.MemoryStream
            If Not jointImg Is Nothing Then
                Dim fext = IO.Path.GetExtension(jointImg.FileName).ToLower()
                If fext = ".jpg" Or fext = ".jpeg" Or fext = ".png" Then
                    inputStream = jointImg.InputStream
                Else
                    response("error") = "No valid file specified. Please select a .JPG, .JPEG or .PNG file to upload."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
            End If

            If inputStream.Length <= 512000 Then '100 KB
                If Not inputStream.Length = 0 Then
                    Using photo As New Drawing.Bitmap(Drawing.Image.FromStream(inputStream))
                        Dim count = FaceDetect(photo)
                        If photo IsNot Nothing Then
                            mStream = ResizeImage(photo, 132, 170)
                            Dim gimg_byte As Byte()
                            gimg_byte = New Byte(mStream.Length) {}
                            gimg_byte = mStream.ToArray()
                            'Context.Session("groom_photo") = gimg_byte
                            'Session("groom_photo") = gimg_byte
                            'To do save data into db
                            Dim obj As New WebService_BL
                            Dim ret = obj.SaveOtherInfoMarriage(location, gimg_byte, Applicant_Cd)
                            If ret = 1 Then
                                json("status") = "ok"
                                json("count") = count
                            Else
                                json("status") = "not ok"
                            End If
                        Else
                            response("error") = "No valid file specified. " + inputStream.Length
                            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                            Exit Sub
                        End If
                    End Using
                Else
                    response("error") = "No valid file specified. Please select a .JPG, .JPEG or .PNG file to upload."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
            Else
                response("error") = "No valid file specified. File size must not exceed 50 KB."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            inputStream = System.IO.Stream.Null
        Catch ex As Exception
            response("exception") = "true"
            response("exception_message") = ex.Message
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
        Finally
            response = If(response.Count < 1, json, response)
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "application/json"
            HttpContext.Current.Response.Write(javaScriptSerializer.Serialize(response))
        End Try
    End Sub
#End Region
#Region "FaceDetect"
    Private Function FaceDetect(imgFace As Drawing.Bitmap) As Integer
        Dim detector As Accord.Vision.Detection.HaarObjectDetector
        Dim cascade As New Accord.Vision.Detection.Cascades.FaceHaarCascade
        detector = New Accord.Vision.Detection.HaarObjectDetector(cascade, 12)
        detector.SearchMode = Accord.Vision.Detection.ObjectDetectorSearchMode.Average
        detector.ScalingFactor = 1.5F
        detector.ScalingMode = Accord.Vision.Detection.ObjectDetectorScalingMode.GreaterToSmaller
        detector.UseParallelProcessing = True
        detector.Suppression = 2.5
        Dim faceObjects As Drawing.Rectangle() = detector.ProcessFrame(imgFace)
        Dim g As Drawing.Graphics = Drawing.Graphics.FromImage(imgFace)
        If faceObjects.Length = 0 Then
            detector.SearchMode = Accord.Vision.Detection.ObjectDetectorSearchMode.Average
            detector.ScalingFactor = 1.5F
            detector.ScalingMode = Accord.Vision.Detection.ObjectDetectorScalingMode.SmallerToGreater
            detector.UseParallelProcessing = True
            detector.Suppression = 2.5
            faceObjects = detector.ProcessFrame(imgFace)
            g = Drawing.Graphics.FromImage(imgFace)
        End If
        g.Dispose()
        Return faceObjects.Count
    End Function
#End Region
#Region "Resize Image"
    Public Function ResizeImage(bmSource As Drawing.Bitmap, TargetWidth As Int32, TargetHeight As Int32) As IO.MemoryStream
        Dim bmDest As New Drawing.Bitmap(TargetWidth, TargetHeight, Drawing.Imaging.PixelFormat.Format32bppArgb)
        Dim NewX = 0
        Dim NewY = 0
        Using grDest = Drawing.Graphics.FromImage(bmDest)
            With grDest
                .CompositingQuality = Drawing.Drawing2D.CompositingQuality.HighQuality
                .InterpolationMode = Drawing.Drawing2D.InterpolationMode.HighQualityBicubic
                .PixelOffsetMode = Drawing.Drawing2D.PixelOffsetMode.HighQuality
                .SmoothingMode = Drawing.Drawing2D.SmoothingMode.AntiAlias
                .CompositingMode = Drawing.Drawing2D.CompositingMode.SourceOver
                .DrawImage(bmSource, NewX, NewY, TargetWidth, TargetHeight)
            End With
        End Using

        Dim img As New IO.MemoryStream()
        bmDest.Save(img, System.Drawing.Imaging.ImageFormat.Bmp)
        Return img
    End Function
#End Region

#Region "Test Data"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)> _
    Public Sub GetFileByte()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
        javaScriptSerializer.MaxJsonLength = Int32.MaxValue
        Try
            Dim ajax, token As String
            Dim method As String
            ajax = HttpContext.Current.Request.Headers("X-Requested-With")
            token = HttpContext.Current.Request.Headers("X-CSRF-TOKEN")
            method = HttpContext.Current.Request.HttpMethod
            If Not (method = "POST" And ajax = "XMLHttpRequest") Then
                response("error") = "Method not allowed"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.MethodNotAllowed, Integer)
                Exit Sub
            End If
            If token = "" Then
                If Session.Item("Marreg_user_info") Is Nothing Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            Else
                If Not token = Session("CSRF") Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            End If

            Dim signer As String = If(Not Request("signer") Is Nothing, DirectCast(Request("signer"), String).Trim(), "")
            Dim file As HttpPostedFile = HttpContext.Current.Request.Files("file")
            If file Is Nothing Then
                response("error") = "No valid file specified. Please select a .pdf file to sign."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            Dim inputStream As System.IO.Stream = System.IO.Stream.Null

            If Not file Is Nothing Then
                Dim fext = IO.Path.GetExtension(file.FileName).ToLower()
                If fext = ".pdf" Then
                    inputStream = file.InputStream
                Else
                    response("error") = "No valid file specified. Please select a .JPG, .JPEG or .PNG file to upload."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
            End If
            Dim fileBytes As Byte()
            If Not inputStream.Length = 0 Then
                ReDim fileBytes(0 To inputStream.Length - 1)
            Else
                response("error") = "No valid file specified. Please select a .JPG, .JPEG or .PNG file to upload."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If

            inputStream.Read(fileBytes, 0, fileBytes.Length)
            inputStream.Close()
            Dim base64String As String = Convert.ToBase64String(fileBytes, 0, fileBytes.Length)

            'Dim res = SendPostData(base64String, signer)
            'Dim b = javaScriptSerializer.DeserializeObject(res)
            'response = b

            response("base64String") = base64String
            response("signer") = signer
            'response("sign_date") = DateTime.UtcNow.ToString() 'GMT
            response("sign_date") = DateTime.Now.ToString()
            response("success") = "success"
        Catch ex As Exception
            response("exception") = "true"
            response("exception_message") = ex.Message
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
        Finally
            response = If(response.Count < 1, json, response)
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "application/json"
            HttpContext.Current.Response.Write(javaScriptSerializer.Serialize(response))
        End Try
    End Sub
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)> _
    Public Sub Verify()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
        Try
            Dim ajax, token As String
            Dim method As String
            ajax = HttpContext.Current.Request.Headers("X-Requested-With")
            token = HttpContext.Current.Request.Headers("X-CSRF-TOKEN")
            method = HttpContext.Current.Request.HttpMethod
            If Not (method = "POST" And ajax = "XMLHttpRequest") Then
                response("error") = "Method not allowed"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.MethodNotAllowed, Integer)
                Exit Sub
            End If
            If token = "" Then
                If Session.Item("Marreg_user_info") Is Nothing Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            Else
                If Not token = Session("CSRF") Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            End If

            Dim file As HttpPostedFile = HttpContext.Current.Request.Files("file")
            If file Is Nothing Then
                response("error") = "No valid file specified. Please select a .pdf file to sign."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            Dim inputStream As System.IO.Stream = System.IO.Stream.Null

            If Not file Is Nothing Then
                Dim fext = IO.Path.GetExtension(file.FileName).ToLower()
                If fext = ".pdf" Then
                    inputStream = file.InputStream
                Else
                    response("error") = "No valid file specified. Please select a .JPG, .JPEG or .PNG file to upload."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
            End If
            Dim fileBytes As Byte()
            If Not inputStream.Length = 0 Then
                ReDim fileBytes(0 To inputStream.Length - 1)
            Else
                response("error") = "No valid file specified. Please select a .JPG, .JPEG or .PNG file to upload."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If

            inputStream.Read(fileBytes, 0, fileBytes.Length)
            inputStream.Close()
            Dim base64String As String = Convert.ToBase64String(fileBytes, 0, fileBytes.Length)


            Dim pdfreader = New iTextSharp.text.pdf.PdfReader(fileBytes)
            Dim fields As iTextSharp.text.pdf.AcroFields = pdfreader.AcroFields
            Dim names = fields.GetSignatureNames()
            Dim f = New List(Of Object)
            For Each name As String In names
                'Dim pkcs7 As iTextSharp.text.pdf.security.PdfPKCS7 = fields.VerifySignature(name)
                'Dim pdfSign As Org.BouncyCastle.X509.X509Certificate = pkcs7.SigningCertificate
                Dim pkcs7 As iTextSharp.text.pdf.PdfPKCS7 = fields.VerifySignature(name)
                Dim pdfSign As Org.BouncyCastle.X509.X509Certificate = pkcs7.SigningCertificate
                Dim subjectAlternativeNames = pdfSign.GetSubjectAlternativeNames()
                Dim subjectAlternativeName = ""
                If (Not subjectAlternativeNames Is Nothing) Then
                    For Each n In subjectAlternativeNames
                        subjectAlternativeName = (DirectCast(n, System.Collections.ArrayList))(1).ToString()
                    Next
                End If
                Dim ok = pkcs7.Verify()
                'Dim x509Name = New iTextSharp.text.pdf.security.CertificateInfo.X509Name(pdfSign.SubjectDN.ToString())
                Dim x509Name = New Org.BouncyCastle.Asn1.X509.X509Name(pdfSign.SubjectDN.ToString())
                'Dim pars = x509Name.GetFields()
                Dim pars = x509Name.GetValues()
                Dim cn = pars("CN")(0).ToString()
                Dim altName = subjectAlternativeName
                Dim notBefore = pdfSign.NotBefore.ToLocalTime().ToString()
                Dim notAfter = pdfSign.NotAfter.ToLocalTime().ToString()
                Dim isValidNow = pdfSign.IsValidNow.ToString()
                Dim verified = ok.ToString()
                Dim serialNumber = pdfSign.SerialNumber.ToString(16).ToUpper()
                Dim sigAlgName = pdfSign.SigAlgName.ToString()
                Dim sigHashAlg = pkcs7.GetHashAlgorithm()
                Dim version = pdfSign.Version.ToString()
                Dim signDate = pkcs7.SignDate.ToString()

                Dim t = New With {.cn = cn, .altName = altName, .notBefore = notBefore, .notAfter = notAfter, .isValidNow = isValidNow, .serialNumber = serialNumber,
                                  .sigAlgName = sigAlgName, .sigHashAlg = sigHashAlg, .version = version, .signDate = signDate, .verified = verified}
                f.Add(t)
            Next
            pdfreader.Close()
            If (f.Count > 0) Then
                json("success") = 1
                json("signaturesReports") = f
            Else
                json("error") = "Not a valid digitally signed document"
            End If
        Catch ex As Exception
            response("exception") = "true"
            response("exception_message") = ex.Message
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
        Finally
            response = If(response.Count < 1, json, response)
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "application/json"
            HttpContext.Current.Response.Write(javaScriptSerializer.Serialize(response))
        End Try
    End Sub
    Public Function SendPostData(base64String As String, signer As String) As String
        Try
            Dim myReq As Net.HttpWebRequest
            Dim myResp As Net.HttpWebResponse
            Dim myReader As IO.StreamReader

            'Dim url = "http://localhost/Staging/MARREG_WebService/GRIPS_WebService.asmx/GetParameterFromHeader"
            Dim url = "http://localhost:8083/Sign"
            myReq = Net.HttpWebRequest.Create(url)
            myReq.Method = "POST"
            myReq.ContentType = "application/json"
            myReq.Accept = "application/json"
            'myReq.Headers.Add("Authorization", "Bearer LKJLMLKJLHLMKLJLM839800K=")
            Dim myData As String = "{""filedata"":""" + base64String + """,""signer"":""" + signer + """}"
            myReq.GetRequestStream.Write(System.Text.Encoding.UTF8.GetBytes(myData), 0, System.Text.Encoding.UTF8.GetBytes(myData).Count)
            myResp = myReq.GetResponse
            myReader = New System.IO.StreamReader(myResp.GetResponseStream)
            Return myReader.ReadToEnd
        Catch ex As Exception
            Return "{""error"":""" + ex.Message + """}"
        End Try
    End Function
#End Region
#Region "Email Certificate"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)> _
    Public Sub GenerateAndSendEmail()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
        Try
            Dim ajax, token As String
            Dim method As String
            ajax = HttpContext.Current.Request.Headers("X-Requested-With")
            token = HttpContext.Current.Request.Headers("X-CSRF-TOKEN")
            method = HttpContext.Current.Request.HttpMethod
            If Not (method = "POST" And ajax = "XMLHttpRequest") Then
                response("error") = "Method not allowed"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.MethodNotAllowed, Integer)
                Exit Sub
            End If
            If token = "" Then
                If Session.Item("Marreg_user_info") Is Nothing Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            Else
                If Not token = Session("CSRF") Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            End If

            Dim out_bytes_str As String = If(Not Request("out_bytes_str") Is Nothing, DirectCast(Request("out_bytes_str"), String).Trim(), "")
            Dim app_Auto_Cd As String = If(Not Request("app_Auto_Cd") Is Nothing, DirectCast(Request("app_Auto_Cd"), String).Trim(), "")
            If (out_bytes_str = "") Then
                response("error") = "No valid file specified. Please select a .pdf file to sign."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (app_Auto_Cd = "") Then
                response("error") = "Application No not found"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            'to be done save certificate and send email to parties
            response("success") = "success"
        Catch ex As Exception
            response("exception") = "true"
            response("exception_message") = ex.Message
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
        Finally
            response = If(response.Count < 1, json, response)
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "application/json"
            HttpContext.Current.Response.Write(javaScriptSerializer.Serialize(response))
        End Try
    End Sub
#End Region
    Protected Sub changeOTP(ByRef otp As String)
        Dim secretkey() As Byte = New [Byte](15) {}
        Using rng As New RNGCryptoServiceProvider()
            rng.GetBytes(secretkey)
        End Using
        otp = a.GetTotp(secretkey)
    End Sub
End Class