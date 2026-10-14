Imports System.Web
Imports System.Web.Services
Imports System.Web.Script.Services
Imports System.Web.Script.Serialization
Imports MARREG.App_Code
Imports System.Net
Imports System.IO
Imports System.Data
Imports MARREG.App_Code.DataAccessLayer
Imports System.Security.Cryptography.X509Certificates
Imports System.Net.Security

'Imports System.Text

'Imports System
'Imports System.Net.Http
'Imports System.Net.Http.Json
'Imports System.Runtime.Serialization.Json
'Imports System.Threading
' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class GRIPS_WebService
    Inherits System.Web.Services.WebService
    Dim a As New Marreg_Class
#Region "GRIPS Payment Details"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)> _
    Public Sub GRIPSpaymentDetails()
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

            Dim dpr As String = If(Not (Request("dpr") Is Nothing Or Request("dpr") = ""), Convert.ToString(Request("dpr")), "")
            Dim draw As String = If(Not Request("draw") Is Nothing, DirectCast(Request("draw"), String), "1")
            Dim start As String = If(Not Request("start") Is Nothing, DirectCast(Request("start"), String), "0")
            Dim length As String = If(Not Request("length") Is Nothing, DirectCast(Request("length"), String), "10")
            Dim value As String = If(Not Request("search[value]") Is Nothing, DirectCast(Request("search[value]"), String).Trim(), "")
            Dim orderByColumn As String = If(Request("order[0][column]") Is Nothing, "", DirectCast(Request("order[0][column]"), String))
            Dim orderByDir As String = If(Request("order[0][dir]") Is Nothing, "", DirectCast(Request("order[0][dir]"), String))
            Dim order_by As String = "created_at"

            Dim is_search As Boolean = False
            Dim is_order As Boolean = False

            is_order = True

            myDataTable = GRIPS_BL.GRIPSpaymentDetails(dpr, draw, start, length, value, order_by, orderByDir)

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
#Region "GRIPS Payment Details Refresh"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)> _
    Public Sub GRIPSpaymentStatusRefresh()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
        Dim encDec As New EncryptionDecryption.EncryptionDecryption()
        Dim enc As New MARREG_SHA256()
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
            Dim id As Int64 = If(Not (Request("id") Is Nothing Or Request("id") = ""), Convert.ToInt64(Request("id").Trim()), 0)
            Dim obj As New GRIPS_BL
            Dim rets = obj.GRIPSpaymentStatusFetch(id)
            Dim dpr = rets.Rows(0)("dept_payment_ref_id")
            Dim src = rets.Rows(0)("source")
            Dim action = ConfigurationManager.AppSettings("gi_dv_action").ToString()
            Dim paymentDate = Convert.ToDateTime(rets.Rows(0)("sys_time_stamp")).ToString("dd/MM/yyyy")
            Dim ip = Common.getIPAddress()
            Dim sysTimeStamp = DateTime.Now().ToString("dd/MM/yyyy HH:mm:ss")

            Dim l1 As New Dictionary(Of String, Object)
            Dim lReq As New Dictionary(Of String, Object)
            lReq.Add("dpr", dpr) 'unique for each packet
            lReq.Add("src", src)
            lReq.Add("action", action)
            lReq.Add("paymentDate", paymentDate)
            l1.Add("req", lReq)
            Dim lsrc As New Dictionary(Of String, String)
            lsrc.Add("srcId", src)
            lsrc.Add("userId", "")
            lsrc.Add("ip", ip)
            lsrc.Add("mac", "")
            lsrc.Add("sysTimeStamp", sysTimeStamp)
            l1.Add("src", lsrc)
            Dim data = javaScriptSerializer.Serialize(l1)
            Dim encData = enc.Encryption(data)
            Dim chkSum = enc.CoputeHash(data)
            Dim res = ""
            res = SendPostData(encData, chkSum, src)
            Dim b = javaScriptSerializer.DeserializeObject(res)
            Dim dc = b
            Dim rEncData = dc("encData")
            Dim rcs = dc("cs")
            Dim rsrc = dc("src")

            Dim datas = enc.Decryption(rEncData)
            Dim datacs = enc.CoputeHash(datas)
            If (datacs <> rcs) Then
                response("error") = "Invalid Response"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
            Else
                Dim robj = javaScriptSerializer.DeserializeObject(datas)
                Dim rdata As New DataTable
                SaveResponseFromGRIPS(robj, rdata)
                Dim resp As New Dictionary(Of String, Object)
                If (rdata.Rows.Count > 0 And rdata.Columns.Count > 1) Then
                    If rdata.Rows(0)("payment_status") = "S" Then
                        Dim reference_data = Common.FetchPaymentRefData(dpr)

                        SaveGRN_Details(rdata, reference_data, resp)
                        If (resp.ContainsKey("error")) Then
                            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                            json("error") = resp("error")
                        End If
                    End If
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
                    response("success") = "1"
                Else
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
                    response("success") = "0"
                End If
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
    Private Sub SaveResponseFromGRIPS(obj As Object, ByRef rdata As DataTable)
        Try
            Dim objD As New Dictionary(Of String, Object)
            objD("dpr") = Convert.ToString(obj("res")("dpr"))
            objD("src") = Convert.ToString(obj("res")("src"))
            objD("action") = Convert.ToString(obj("res")("action"))
            objD("gpr") = Convert.ToString(obj("res")("gpr"))
            objD("paymentAmt") = Convert.ToDecimal(obj("res")("paymentAmt"))
            objD("paymentStatus") = Convert.ToString(obj("res")("paymentStatus"))
            objD("paymentStatusDesc") = Convert.ToString(obj("res")("paymentStatusDesc"))
            If (objD("paymentStatus") <> "NA") Then
                objD("grn") = Convert.ToString(obj("res")("paymentDtls")(0)("grn"))
                objD("grnTime") = Convert.ToString(obj("res")("paymentDtls")(0)("grnTime"))
                objD("grnStatus") = Convert.ToString(obj("res")("paymentDtls")(0)("grnStatus"))
                objD("grnStatusDesc") = Convert.ToString(obj("res")("paymentDtls")(0)("grnStatusDesc"))
                objD("bank") = Convert.ToString(obj("res")("paymentDtls")(0)("bank"))
                objD("paymentMode") = Convert.ToString(obj("res")("paymentDtls")(0)("paymentMode"))
                objD("brn") = Convert.ToString(obj("res")("paymentDtls")(0)("brn"))
                objD("brnTime") = Convert.ToString(obj("res")("paymentDtls")(0)("brnTime"))
            Else
                objD("grn") = ""
                objD("grnTime") = ""
                objD("grnStatus") = ""
                objD("grnStatusDesc") = ""
                objD("bank") = ""
                objD("paymentMode") = ""
                objD("brn") = ""
                objD("brnTime") = ""
            End If
            objD("status") = Convert.ToString(obj("sts")("status"))
            objD("statusCode") = Convert.ToString(obj("sts")("statusCode"))
            objD("statusDesc") = Convert.ToString(obj("sts")("statusDesc"))
            objD("ip") = Convert.ToString(obj("src")("ip"))
            objD("sysTimeStamp") = Convert.ToString(obj("src")("sysTimeStamp"))
            Dim objG As New GRIPS_BL
            Dim ret = objG.SaveGRIPSPaymentResponse(objD)
            rdata = ret
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Function SendPostData(encData As String, chkSum As String, src As String) As String
        Try
            Dim myReq As HttpWebRequest
            Dim myResp As HttpWebResponse
            Dim myReader As StreamReader
            'Dim url = "http://localhost/Staging/MARREG_WebService/GRIPS_WebService.asmx/GetParameterFromHeader"
            Dim url = ConfigurationManager.AppSettings("gi_dv_url").ToString()
            myReq = HttpWebRequest.Create(url)
            myReq.Method = "POST"
            myReq.ContentType = "application/json"
            myReq.Accept = "application/json"
            'myReq.Headers.Add("Authorization", "Bearer LKJLMLKJLHLMKLJLM839800K=")
            Dim myData As String = "{""encData"":""" + encData + """,""cs"":""" + chkSum + """,""src"":""" + src + """}"
            Net.ServicePointManager.SecurityProtocol = Net.SecurityProtocolType.Tls12
            System.Net.ServicePointManager.ServerCertificateValidationCallback = AddressOf ValidateRemoteCertificate
            myReq.GetRequestStream.Write(System.Text.Encoding.UTF8.GetBytes(myData), 0, System.Text.Encoding.UTF8.GetBytes(myData).Count)
            myResp = myReq.GetResponse
            myReader = New System.IO.StreamReader(myResp.GetResponseStream)
            Return myReader.ReadToEnd
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub GRIPSVerifyPaymentStatus()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
        Dim encDec As New EncryptionDecryption.EncryptionDecryption()
        Dim enc As New MARREG_SHA256()
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
            Dim dpr As String = If(Not (Request("dpr") Is Nothing Or Request("dpr") = ""), Convert.ToString(Request("dpr").Trim()), "")
            Dim paymentDt As String = If(Not (Request("paymentDt") Is Nothing Or Request("paymentDt") = ""), Convert.ToString(Request("paymentDt").Trim()), "")
            If dpr = "" Or dpr Is Nothing Then
                response("error") = "Please enter Department Payment Reference ID."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If paymentDt = "" And paymentDt = "" Then
                response("error") = "Please enter Payment Date."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            Dim src = ConfigurationManager.AppSettings("gi_pmt_src").ToString()
            Dim action = ConfigurationManager.AppSettings("gi_dv_action").ToString()
            Dim paymentDate = Convert.ToDateTime(paymentDt).ToString("dd/MM/yyyy")
            Dim ip = Common.getIPAddress()
            Dim sysTimeStamp = DateTime.Now().ToString("dd/MM/yyyy HH:mm:ss")
            'Verify request params
            Dim objG As New GRIPS_BL
            Dim ret = objG.GRIPSverifyRequestParam(dpr, paymentDate)
            If (ret = 0) Then
                json("error") = "0"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
                Exit Sub
            End If
            'end of Verification
            Dim l1 As New Dictionary(Of String, Object)
            Dim lReq As New Dictionary(Of String, Object)
            lReq.Add("dpr", dpr) 'unique for each packet
            lReq.Add("src", src)
            lReq.Add("action", action)
            lReq.Add("paymentDate", paymentDate)
            l1.Add("req", lReq)
            Dim lsrc As New Dictionary(Of String, String)
            lsrc.Add("srcId", src)
            lsrc.Add("userId", "")
            lsrc.Add("ip", ip)
            lsrc.Add("mac", "")
            lsrc.Add("sysTimeStamp", sysTimeStamp)
            l1.Add("src", lsrc)
            Dim data = javaScriptSerializer.Serialize(l1)
            Dim encData = enc.Encryption(data)
            Dim chkSum = enc.CoputeHash(data)
            Dim res = ""
            res = SendPostData(encData, chkSum, src)
            Dim b = javaScriptSerializer.DeserializeObject(res)
            Dim dc = b
            Dim rEncData = dc("encData")
            Dim rcs = dc("cs")
            Dim rsrc = dc("src")

            Dim datas = enc.Decryption(rEncData)
            Dim datacs = enc.CoputeHash(datas)

            If (datacs <> rcs) Then
                response("error") = "Invalid Response"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
            Else
                Dim robj = javaScriptSerializer.DeserializeObject(datas)
                Dim rdata As New DataTable
                SaveResponseFromGRIPS(robj, rdata)
                If (rdata.Rows.Count > 0 And rdata.Columns.Count > 1) Then
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
                    json("success") = "1"
                    json("data") = DataTableToJSONWithJavaScriptSerializer(rdata)
                Else
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
                    json("error") = "0"
                End If
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
    Public Sub MARREGPaymentStatus()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
        Dim encDec As New EncryptionDecryption.EncryptionDecryption()
        Dim enc As New MARREG_SHA256()
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
            Dim remarks As String = If(Not (Request("remarks") Is Nothing Or Request("remarks") = ""), Convert.ToString(Request("remarks").Trim()), "")
            Dim dpr As String = If(Not (Request("dpr") Is Nothing Or Request("dpr") = ""), Convert.ToString(Request("dpr").Trim()), "")
            Dim paymentDt As String = If(Not (Request("paymentDt") Is Nothing Or Request("paymentDt") = ""), Convert.ToString(Request("paymentDt").Trim()), "")
            Dim captcha As String = If(Not (Request("captcha") Is Nothing Or Request("captcha") = ""), Convert.ToString(Request("captcha").Trim()), "")
            If remarks = "" Or remarks Is Nothing Then
                response("error") = "Please select Service Type."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If dpr = "" Or dpr Is Nothing Then
                response("error") = "Please enter Department Payment Reference ID."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If paymentDt = "" And paymentDt Is Nothing Then
                response("error") = "Please enter Payment Date."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If captcha = "" And captcha Is Nothing Then
                response("error") = "Please enter Captcha Text."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If Not captcha = Session("captchaString") Then
                response("error") = "Invalid Captcha Entered."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If

            'To Do Refresh Captcha String

            Dim src = ConfigurationManager.AppSettings("gi_pmt_src").ToString()
            Dim action = ConfigurationManager.AppSettings("gi_dv_action").ToString()
            Dim paymentDate = Convert.ToDateTime(paymentDt).ToString("dd/MM/yyyy")
            Dim ip = Common.getIPAddress()
            Dim sysTimeStamp = DateTime.Now().ToString("dd/MM/yyyy HH:mm:ss")
            'Verify request params
            Dim objG As New GRIPS_BL
            'Dim ret = objG.GRIPSverifyRequestParam(dpr, paymentDate)
            Dim svcCode = "3" + Mid(remarks, Len(remarks) - 1, 2)
            Dim ret = objG.GRIPSverifyRequestParamStatus(svcCode, dpr, paymentDate)
            If (ret.Rows.Count = 0) Then
                json("error") = "0"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
                Exit Sub
            Else
                Dim payment_status = Convert.ToString(ret.Rows(0)("payment_status"))
                If (payment_status = "P" Or payment_status = "") Then
                    'to do check from remote site
                    Dim l1 As New Dictionary(Of String, Object)
                    Dim lReq As New Dictionary(Of String, Object)
                    lReq.Add("dpr", dpr) 'unique for each packet
                    lReq.Add("src", src)
                    lReq.Add("action", action)
                    lReq.Add("paymentDate", paymentDate)
                    l1.Add("req", lReq)
                    Dim lsrc As New Dictionary(Of String, String)
                    lsrc.Add("srcId", src)
                    lsrc.Add("userId", "")
                    lsrc.Add("ip", ip)
                    lsrc.Add("mac", "")
                    lsrc.Add("sysTimeStamp", sysTimeStamp)
                    l1.Add("src", lsrc)
                    Dim data = javaScriptSerializer.Serialize(l1)
                    Dim encData = enc.Encryption(data)
                    Dim chkSum = enc.CoputeHash(data)
                    Dim res = ""
                    res = SendPostData(encData, chkSum, src)
                    Dim b = javaScriptSerializer.DeserializeObject(res)
                    Dim dc = b
                    Dim rEncData = dc("encData")
                    Dim rcs = dc("cs")
                    Dim rsrc = dc("src")

                    Dim datas = enc.Decryption(rEncData)
                    Dim datacs = enc.CoputeHash(datas)

                    If (datacs <> rcs) Then
                        response("error") = "Invalid Response"
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Else
                        Dim robj = javaScriptSerializer.DeserializeObject(datas)
                        Dim rdata As New DataTable
                        SaveResponseFromGRIPS(robj, rdata)
                        Dim resp As New Dictionary(Of String, Object)
                        If (rdata.Rows.Count > 0 And rdata.Columns.Count > 1) Then
                            If rdata.Rows(0)("payment_status") = "S" Then
                                Dim reference_data = Common.FetchPaymentRefData(dpr)

                                SaveGRN_Details(rdata, reference_data, resp)
                                If (resp.ContainsKey("error")) Then
                                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                                    json("error") = resp("error")
                                End If
                            End If
                            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
                            json("success") = "1"
                            json("data") = DataTableToJSONWithJavaScriptSerializer(rdata)
                            json("resp") = resp
                        Else
                            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
                            json("error") = "0"
                        End If
                        'To Do Code for Sending Email Intimation for Payment Success
                        If rdata.Rows(0)("grn_status") <> "" Then
                            Dim depName = rdata.Rows(0)("depositor_name").ToString
                            remarks = rdata.Rows(0)("payment_remarks").ToString
                            Dim totalAmt = rdata.Rows(0)("total_amt").ToString
                            Dim depEmail = a.sqlInj_out(rdata.Rows(0)("depositor_email").ToString).ToString
                            Dim paymentStatus = rdata.Rows(0)("payment_status_desc").ToString
                            Dim grn = rdata.Rows(0)("grn").ToString
                            paymentDate = Convert.ToDateTime(rdata.Rows(0)("grn_time")).ToString("dd/MM/yyyy")
                            Dim obj1 As New WS_MasterData_BL()
                            Dim ds = obj1.FetchEmailFormat(Intimation_for_Payment_Complete)
                            Dim d = ds.Tables(0)
                            If (d.Rows.Count = 0) Then
                            Else
                                Dim subject = d.Rows(0)("email_subject")
                                Dim body = d.Rows(0)("email_body")
                                Dim oldStr() As String = {"{depName}", "{remarks}", "{totalAmt}", "{dpr}", "{paymentStatus}", "{grn}", "{paymentDate}"}
                                Dim newStr() As String = {depName.ToUpper, remarks.ToUpper, "Rs. " + totalAmt.ToString(), dpr, paymentStatus.ToUpper, grn, paymentDate}
                                Dim params As New Dictionary(Of String, Object)
                                params = CreateAssocArray(oldStr, newStr)
                                Common.ReplaceEmailBody(body, oldStr, newStr)
                                Dim eml As New BusinessLogicLayer.Email_Sms_BL
                                Dim r_email_ids As New List(Of String)
                                r_email_ids.Add(depEmail)
                                'Dim email_stat = eml.send_email_multiple_bytes(r_email_ids, subject, body)
                                Dim asyncEml As New AsyncMailMessage_DB()
                                asyncEml.AsyncMailMessage_Bytes(r_email_ids, subject, body, Nothing, params)
                                'SendMailMessage_Bytes(r_email_ids, subject, body)
                            End If
                        End If
                    End If

                ElseIf (payment_status = "S") Then
                    Dim reference_data = Common.FetchPaymentRefData(dpr)
                    If (reference_data Is Nothing) Then
                        json("success") = "0"
                        json("data") = DataTableToJSONWithJavaScriptSerializer(ret)
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
                        Exit Sub
                    End If
                    Dim resp As New Dictionary(Of String, Object)
                    SaveGRN_Details(ret, reference_data, resp)
                    If (resp.ContainsKey("error")) Then
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        json("error") = resp("error")
                    End If
                    json("success") = "1"
                    json("data") = DataTableToJSONWithJavaScriptSerializer(ret)
                    json("resp") = resp
                ElseIf (payment_status = "F") Then
                    json("success") = "1"
                    json("data") = DataTableToJSONWithJavaScriptSerializer(ret)
                Else
                    json("success") = "1"
                    json("data") = DataTableToJSONWithJavaScriptSerializer(ret)
                End If
            End If
            'end of Verification
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
    Private Sub SaveGRN_Details(rdata As DataTable, reference_data As Object, ByRef resp As Dictionary(Of String, Object))
        Dim obj As New WebService_BL
        Dim Payment_Reason = ""
        Dim reason = ""
        If reference_data.ContainsKey("Payment_Reason") Then
            Payment_Reason = reference_data("Payment_Reason")
        Else
            resp("error") = "Invalid payment reference found"
        End If
        If reference_data.ContainsKey("reason") Then
            reason = reference_data("reason")
        End If
        'Added on 18-12-2023 for Registration Payment Integration
        If (reference_data("PaymentRemarks") = "0001" Or reference_data("PaymentRemarks") = "0002") Then     ' Case Registration of Marriage
            Dim objP As New MARREG_App_Payment_BL()
            Dim pmntInitId = Convert.ToInt64(reference_data("pmntInitId"))
            Dim draft_app_id = Convert.ToString(reference_data("draft_app_id"))
            Dim grn = Convert.ToString(rdata.Rows(0)("grn"))
            Dim payment_dt = Convert.ToString(rdata.Rows(0)("payment_completed"))
            Dim retP = objP.UpdateGRN_App_Payment_Init(pmntInitId, draft_app_id, grn, payment_dt)
            If (retP = "1") Then
                resp("succ") = " Please proceed the Application further."
            Else
                resp("error") = " Please contact O/o RGM, WB."
            End If
            Exit Sub
        End If
        'End Added on 18-12-2023 for Registration Payment Integration
        Session("Payment_Reason") = Payment_Reason
        Session("gid") = Nothing
        Dim rets = obj.Save_GRN_Details_Ag_Search(rdata.Rows(0)("grn"), Convert.ToInt64(rdata.Rows(0)("payment_amt")), Payment_Reason, reason)
        If rets = "-1" Then     'Duplicate value/ Process completed
            resp("error") = "Request has already been submitted."
            'HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
        ElseIf rets = "-2" Then   'Transaction rollback
            resp("error") = "Internal error occured. Please try again later."
            'HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
        Else
            If Convert.ToInt64(rets) >= 1 Then
                If reference_data.ContainsKey("ServiceDocketNo") Then
                    Dim ret = obj.Update_GRN_Details_Ag_Print(rets, "", "", Convert.ToString(Session.Item("Payment_Reason")))
                    Dim dpr = rdata.Rows(0)("dept_payment_ref_id")
                    Dim docket_no = reference_data("ServiceDocketNo")
                    obj.UpdateServiceDocketStatus(dpr, docket_no)
                    Exit Sub
                End If
                resp("PaymentRemarks") = reference_data("PaymentRemarks")
                If (reference_data("PaymentRemarks") = "0005") Then     ' Case Searching
                    resp("gid") = Convert.ToInt64(rets)
                    resp("cat") = reference_data("cat")
                    resp("period") = reference_data("period")
                    resp("postUrl") = reference_data("postUrl")
                    Session("gid") = Convert.ToInt64(rets)
                ElseIf (reference_data("PaymentRemarks") = "0003") Then     ' Case Certified Copy of Certificate
                    resp("gid") = Convert.ToInt64(rets)
                    resp("no_copy") = reference_data("no_copy")
                    resp("validatedId") = reference_data("validatedId")
                    If (resp("validatedId") = "" Or resp("validatedId") Is Nothing Or resp("validatedId") = "0") Then
                        resp("vdata") = reference_data("vdata")
                    End If
                    resp("postUrl") = reference_data("postUrl")

                    Session("gid") = Convert.ToInt64(rets)
                    Session("no_copy") = Convert.ToInt64(reference_data("no_copy"))
                    Session("validatedId") = Convert.ToInt64(reference_data("validatedId"))
                End If
                resp("next") = "true"
            End If
        End If
    End Sub
    Public Function DataTableToJSONWithJavaScriptSerializer(ByVal table As DataTable) As List(Of Dictionary(Of String, Object))
        Dim jsSerializer As JavaScriptSerializer = New JavaScriptSerializer()
        Dim parentRow As List(Of Dictionary(Of String, Object)) = New List(Of Dictionary(Of String, Object))()
        Dim childRow As Dictionary(Of String, Object)

        For Each row As DataRow In table.Rows
            childRow = New Dictionary(Of String, Object)()

            For Each col As DataColumn In table.Columns
                If TypeOf row(col) Is DateTime Then
                    childRow.Add(col.ColumnName, Convert.ToDateTime(row(col)).ToString("dd-MM-yyyy hh:mm:ss tt"))
                ElseIf TypeOf row(col) Is Date Then
                    childRow.Add(col.ColumnName, Convert.ToDateTime(row(col)).ToString("dd-MM-yyyy"))
                ElseIf TypeOf row(col) Is DBNull Then
                    childRow.Add(col.ColumnName, "NA")
                ElseIf row(col).ToString() = "" Then
                    childRow.Add(col.ColumnName, "")
                Else
                    childRow.Add(col.ColumnName, a.sqlInj_out(row(col)).ToString())
                End If
            Next
            parentRow.Add(childRow)
        Next
        Return parentRow
        'Return jsSerializer.Serialize(parentRow)
    End Function
    <WebMethod> _
    Public Function GetParameterFromHeader() As Object
        Dim headerList As NameValueCollection = HttpContext.Current.Request.Headers
        Dim javaScriptSerializer As New JavaScriptSerializer()
        Dim response As New Dictionary(Of String, Object)
        Dim encDec As New EncryptionDecryption.EncryptionDecryption()
        Dim enc As New MARREG_SHA256()
        Dim stream = HttpContext.Current.Request.InputStream
        stream.Position = 0
        Dim sr = New StreamReader(stream).ReadToEnd()
        Dim Request = javaScriptSerializer.DeserializeObject(sr)

        Dim encData As String = If(Not (Request("encData") Is Nothing Or Request("encData") = ""), Convert.ToString(Request("encData").Trim()), "")
        Dim cs As String = If(Not (Request("chkSum") Is Nothing Or Request("chkSum") = ""), Convert.ToString(Request("chkSum").Trim()), "")
        Dim src As String = If(Not (Request("src") Is Nothing Or Request("src") = ""), Convert.ToString(Request("src").Trim()), "")

        'Dim datas = encDec.Decrypt(encData)
        'Dim datacs = getCheckSum(datas)
        'encData = "JeCUdYjlaKyVj5ZxrbdtXbHkorZ85D8LS7Oaqo7SEMwkYdoA3EStkLrIjxSr/rIa"
        Dim datas = enc.Decryption(encData)
        Dim datacs = enc.CoputeHash(datas)
        If (datacs <> cs) Then
            response("error") = "Invalid Response"
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
        Else
            Dim obj = javaScriptSerializer.DeserializeObject(datas)

            Dim data = javaScriptSerializer.Serialize(dd())
            'response("encData") = encDec.encrypt(data)
            'response("cs") = getCheckSum(data)
            response("encData") = enc.Encryption(data)
            response("cs") = enc.CoputeHash(data)
            response("src") = "rgmwb"
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
            'Dim rdata As New DataTable
            'SaveResponseFromGRIPS(obj, rdata)
            'setDatas(rdata)
        End If





        Return response
        'HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)

        'HttpContext.Current.Response.Clear()
        'HttpContext.Current.Response.ContentType = "application/json"
        'HttpContext.Current.Response.Write(javaScriptSerializer.Serialize(response))
    End Function
    Private Function dd() As Object
        Dim ip = Common.getIPAddress()
        Dim sysTimeStamp = DateTime.Now().ToString("dd/MM/yyyy HH:mm:ss")
        Dim l1 As New Dictionary(Of String, Object)
        Dim lReq As New Dictionary(Of String, Object)
        lReq.Add("dpr", "RGM2022090900000002") 'unique for each packet
        lReq.Add("src", "rgmwb")
        lReq.Add("action", "DV")
        lReq.Add("gpr", "1236547890")
        lReq.Add("paymentAmt", "200")
        lReq.Add("paymentStatus", "S")
        lReq.Add("paymentStatusDesc", "Success")
        'paymentDtls
        Dim lPaymentDtls As New Dictionary(Of String, Object)
        lPaymentDtls.Add("deptCode", "083")
        lPaymentDtls.Add("svcCode", "001")
        lPaymentDtls.Add("drn", "083202200000002")                   'unique for a GRN
        lPaymentDtls.Add("identificationNo", "0832022000300000002")     'unique for a GRN
        lPaymentDtls.Add("totalAmt", "200")
        lPaymentDtls.Add("grn", "192012587412596545")
        lPaymentDtls.Add("grnTime", DateTime.Now().ToString("dd/MM/yyyy HH:mm:ss"))
        lPaymentDtls.Add("grnStatus", "S")
        lPaymentDtls.Add("grnStatusDesc", "Success")
        lPaymentDtls.Add("bank", "State Bank of India")
        lPaymentDtls.Add("paymentMode", "1")
        lPaymentDtls.Add("brn", "IX123456")
        lPaymentDtls.Add("brnTime", DateTime.Now().ToString("dd/MM/yyyy HH:mm:ss"))

        lReq.Add("paymentDtls", lPaymentDtls)
        l1.Add("res", lReq)

        Dim lsts As New Dictionary(Of String, String)
        lsts.Add("status", "S")
        lsts.Add("statusCode", "0")
        lsts.Add("statusDesc", "No error in the flow")

        l1.Add("sts", lsts)

        Dim lsrc As New Dictionary(Of String, String)
        lsrc.Add("srcId", "rgmwb")
        lsrc.Add("userId", "")
        lsrc.Add("ip", ip)
        lsrc.Add("mac", "")
        lsrc.Add("sysTimeStamp", sysTimeStamp)

        l1.Add("src", lsrc)
        Return l1
    End Function
#End Region
    Public Function getCheckSum(ByVal msg As String) As String
        Dim sum As Integer = 0

        For i As Integer = 0 To msg.Length() - 1
            sum += Convert.ToUInt32(msg.Chars(i))
        Next

        sum += 16
        sum = sum Mod 256
        Dim checksum As String = Hex(sum).ToString().ToUpper()

        If checksum.Length() = 1 Then
            checksum = "0" & checksum
        End If

        Return checksum
    End Function

#Region "GRN"
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
    Public Sub GRN_Status()
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

            Dim GRN_NO As String = If(Not Request("grn") Is Nothing, DirectCast(Request("grn"), String).Trim(), "")
            Dim captcha As String = If(Not (Request("captcha") Is Nothing Or Request("captcha") = ""), Convert.ToString(Request("captcha").Trim()), "")
            If GRN_NO = "" Or GRN_NO Is Nothing Then
                response("error") = "Please enter GRN."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If Not captcha = Session("captchaString") Then
                response("error") = "Invalid Captcha Entered."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If

            Net.ServicePointManager.SecurityProtocol = Net.SecurityProtocolType.Tls12
            System.Net.ServicePointManager.ServerCertificateValidationCallback = AddressOf ValidateRemoteCertificate
            'Dim grn = ("https://wbifms.gov.in/GRIPS/ChallanDetails/query.do?GRN_NO=" + GRN_NO + "")
            Dim grn = String.Format(ConfigurationManager.AppSettings("gi_grncheck_url").ToString(), GRN_NO)
            Dim xmlDoc As System.Xml.XmlDocument = New System.Xml.XmlDocument
            xmlDoc.Load(grn)
            Dim nodeList As System.Xml.XmlNodeList = xmlDoc.DocumentElement.SelectNodes("/GRIPS_EPAYMENT")
            Dim nodeListChallan As System.Xml.XmlNodeList = xmlDoc.DocumentElement.SelectNodes("/GRIPS_EPAYMENT/CHALLAN_DETAILS")
            Dim grn_stat = check_Grn(nodeListChallan)
            'grn_stat = "MARREG HEAD"

            If grn_stat = "MARREG HEAD" Then
                json("xml_data") = xmlDoc.InnerXml
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
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
#End Region
End Class