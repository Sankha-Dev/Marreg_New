Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Web.Script.Services
Imports System.Web.Script.Serialization
Imports MARREG.App_Code
Imports MARREG.App_Code.DataAccessLayer
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports System.Data
Imports System.Security.Cryptography
Imports iTextSharp.text.pdf

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")>
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Public Class CertifiedCopy_WebService
    Inherits System.Web.Services.WebService
    Dim a As New Marreg_Class
    Dim m As New WebService_BL
    Dim enc As New EncryptionDecryption.EncryptionDecryption()
#Region "Email Validation"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub CheakEmailExist()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
        Try
            Dim ajax, token As String
            Dim method As String
            Dim enum_app, AppKey As String
            ajax = HttpContext.Current.Request.Headers("X-Requested-With")
            token = HttpContext.Current.Request.Headers("X-CSRF-TOKEN")
            method = HttpContext.Current.Request.HttpMethod
            enum_app = Request("enum")
            AppKey = Request("apikey")

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
                If Not token = Session("CSRF") And Not token = ConfigurationManager.AppSettings("api_key") Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            End If
            Dim email As String = If(Not Request("email") Is Nothing, DirectCast(Request("email"), String).Trim(), "")
            email = a.sqlInj_in(email)
            Dim obj As New WebService_BL
            Dim App_Auto_Cd = obj.CheakEmailExist(email)
            If enum_app = "app" Then
                If App_Auto_Cd.Tables(0).Rows.Count < 1 Then
                    Session("App_Auto_Cd_Arr") = Nothing
                    'response("error") = "Email Id is not registered."
                    json("success") = "failed"
                    json("enum") = "app"
                    json("email_flag") = 0
                    json("message") = "Email Id not registered."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
                    Exit Sub
                ElseIf App_Auto_Cd.Tables(0).Rows(0).Item(0) = "-1" Then
                    Session("App_Auto_Cd_Arr") = Nothing
                    'response("error") = ""
                    json("success") = "failed"
                    json("enum") = "app"
                    json("email_flag") = 0
                    json("message") = "Unable to proceed with this email id"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
                    Exit Sub
                End If
                'response("success") = App_Auto_Cd.Tables(0).Rows.Count
                json("success") = "Ok"
                json("enum") = "app"
                json("email_flag") = 1
                json("message") = "Email Id is registered."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
                Session("App_Auto_Cd_Arr") = App_Auto_Cd
            Else
                If App_Auto_Cd.Tables(0).Rows.Count < 1 Then
                    Session("App_Auto_Cd_Arr") = Nothing
                    response("error") = "Email Id not registered. Please click <a href='" + Common.baseUrl + "MARREG_CertifiedCopy/MARREG_CertifiedCopyOutsider.aspx'>here</a> for alternate process"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                ElseIf App_Auto_Cd.Tables(0).Rows(0).Item(0) = "-1" Then    'Added on 03-09-2024
                    Session("App_Auto_Cd_Arr") = Nothing
                    response("error") = "Unable to proceed with this email id"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                response("success") = App_Auto_Cd.Tables(0).Rows.Count
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
                Session("App_Auto_Cd_Arr") = App_Auto_Cd
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
#Region "Certified Copy Request"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub CertifiedCopyRequestInfoValidateInsider()
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
            Dim email As String = If(Not (Request("email") Is Nothing Or Request("email") = ""), DirectCast(Request("email"), String).Trim(), "")
            Dim groom_name As String = If(Not (Request("groom_name") Is Nothing Or Request("groom_name") = ""), DirectCast(Request("groom_name"), String).Trim(), "")
            Dim bride_name As String = If(Not (Request("bride_name") Is Nothing Or Request("bride_name") = ""), DirectCast(Request("bride_name"), String).Trim(), "")
            Dim groom_dob As String = If(Not (Request("groom_dob") Is Nothing Or Request("groom_dob") = ""), DirectCast(Request("groom_dob"), String).Trim(), "")
            Dim bride_dob As String = If(Not (Request("bride_dob") Is Nothing Or Request("bride_dob") = ""), DirectCast(Request("bride_dob"), String).Trim(), "")
            Dim reg_date As String = If(Not Request("reg_date") Is Nothing, DirectCast(Request("reg_date"), String).Trim(), "")
            Dim mocd As String = If(Not (Request("mocd") Is Nothing Or Request("mocd") = ""), DirectCast(Request("mocd"), String).Trim(), "")
            Dim yr As String = If(Not (Request("yr") Is Nothing Or Request("yr") = ""), DirectCast(Request("yr"), String).Trim(), "")
            Dim no As String = If(Not (Request("no") Is Nothing Or Request("no") = ""), DirectCast(Request("no"), String).Trim(), "")
            Dim certificate_no As String = If(Not (Request("certificate_no") Is Nothing Or Request("certificate_no") = ""), DirectCast(Request("certificate_no"), String).Trim(), "")
            Dim mo_name As String = If(Not (Request("mo_name") Is Nothing Or Request("mo_name") = ""), DirectCast(Request("mo_name"), String).Trim(), "")
            'Dim gid As String = If(Not Request("gid") Is Nothing And Request("gid") <> "", DirectCast(Request("gid"), String).Trim(), "0")
            'Dim nocopy As String = If(Not Request("no_copy") Is Nothing And Request("no_copy") <> "", DirectCast(Request("no_copy"), String).Trim(), "0")

            If email = "" Or email Is Nothing Then
                response("error") = "Please initiate the process again."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If groom_name = "" Or groom_name Is Nothing Then
                response("error") = "Please enter Groom/ Husband Name."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If bride_name = "" Or bride_name Is Nothing Then
                response("error") = "Please enter Bride/ Wife Name."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If groom_dob = "" And bride_dob = "" Then
                response("error") = "Please enter either Groom/ Husband DOB or Bride/ Wife DOB."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            Dim cer_no = ""
            If (mocd <> "" Or yr <> "" Or no <> "") Then
                If (mocd = "" Or yr = "" Or no = "") Then
                    response("error") = "Please enter Certificate No properly."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                Else
                    cer_no = mocd + "-" + yr + "-" + no
                End If
            End If
            If (cer_no <> certificate_no) Then
                response("error") = "Please initiate the process again."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            email = a.sqlInj_in(email)

            Dim obj As New WebService_BL
            Dim rets = obj.CertifiedCopyRequestInfoValidateInsider(email, groom_name, bride_name, groom_dob, bride_dob, reg_date, certificate_no, mo_name)
            If rets = "0" Then
                json("error") = "Request submission error. Please provide valid information associated to your email id."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
            ElseIf rets = "-1" Then     'Duplicate value
                json("error") = "Your previous request is already open."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
            ElseIf rets = "-2" Then   'Transaction rollback
                json("error") = "Internal error occured. Please try again later."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
            ElseIf rets = "-3" Then   'Application has been nullified of divorced
                json("error") = "This application has been dissolved."
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
    Public Sub CertifiedCopyRequest()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
        Try
            Dim ajax, token As String
            Dim method As String
            Dim enum_app, AppKey As String
            enum_app = Request("enum")
            AppKey = Request("apikey")
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
            Dim email As String = If(Not (Request("email") Is Nothing Or Request("email") = ""), DirectCast(Request("email"), String).Trim(), "")
            Dim groom_name As String = If(Not (Request("groom_name") Is Nothing Or Request("groom_name") = ""), DirectCast(Request("groom_name"), String).Trim(), "")
            Dim bride_name As String = If(Not (Request("bride_name") Is Nothing Or Request("bride_name") = ""), DirectCast(Request("bride_name"), String).Trim(), "")
            Dim groom_dob As String = If(Not (Request("groom_dob") Is Nothing Or Request("groom_dob") = ""), DirectCast(Request("groom_dob"), String).Trim(), "")
            Dim bride_dob As String = If(Not (Request("bride_dob") Is Nothing Or Request("bride_dob") = ""), DirectCast(Request("bride_dob"), String).Trim(), "")
            Dim reg_date As String = If(Not Request("reg_date") Is Nothing, DirectCast(Request("reg_date"), String).Trim(), "")
            Dim mocd As String = If(Not (Request("mocd") Is Nothing Or Request("mocd") = ""), DirectCast(Request("mocd"), String).Trim(), "")
            Dim yr As String = If(Not (Request("yr") Is Nothing Or Request("yr") = ""), DirectCast(Request("yr"), String).Trim(), "")
            Dim no As String = If(Not (Request("no") Is Nothing Or Request("no") = ""), DirectCast(Request("no"), String).Trim(), "")
            Dim certificate_no As String = If(Not (Request("certificate_no") Is Nothing Or Request("certificate_no") = ""), DirectCast(Request("certificate_no"), String).Trim(), "")
            Dim mo_name As String = If(Not (Request("mo_name") Is Nothing Or Request("mo_name") = ""), DirectCast(Request("mo_name"), String).Trim(), "")
            Dim gid As String = If(Not Request("gid") Is Nothing And Request("gid") <> "", DirectCast(Request("gid"), String).Trim(), "0")
            Dim nocopy As String = If(Not Request("no_copy") Is Nothing And Request("no_copy") <> "", DirectCast(Request("no_copy"), String).Trim(), "0")

            If email = "" Or email Is Nothing Then
                response("error") = "Please initiate the process again."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If groom_name = "" Or groom_name Is Nothing Then
                response("error") = "Please enter Groom/ Husband Name."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If bride_name = "" Or bride_name Is Nothing Then
                response("error") = "Please enter Bride/ Wife Name."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If groom_dob = "" And bride_dob = "" Then
                response("error") = "Please enter either Groom/ Husband DOB or Bride/ Wife DOB."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            Dim cer_no = ""
            If (mocd <> "" Or yr <> "" Or no <> "") Then
                If (mocd = "" Or yr = "" Or no = "") Then
                    response("error") = "Please enter Certificate No properly."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                Else
                    cer_no = mocd + "-" + yr + "-" + no
                End If
            End If
            If (cer_no <> certificate_no) Then
                response("error") = "Please initiate the process again."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            email = a.sqlInj_in(email)
            Dim obj As New WebService_BL
            Dim grn_id = Convert.ToInt64(Session("gid"))
            Dim no_copy = Convert.ToInt32(Session("no_copy"))
            If (grn_id < 1 Or grn_id <> Convert.ToInt64(gid)) Then
                response("error") = "Please initiate the process again."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                Exit Sub
            End If
            If (no_copy <> Convert.ToInt64(nocopy)) Then
                response("error") = "Please initiate the process again."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                Exit Sub
            End If
            Dim rets = obj.Save_CertifiedCopyRequest(email, groom_name, bride_name, groom_dob, bride_dob, reg_date, certificate_no, grn_id, no_copy, mo_name)
            If rets.Length >= 13 Then
                Dim id As Int64 = DirectCast(Session("gid"), Int64)
                Dim ret = obj.Update_GRN_Details_Ag_Print(id, "", "", Convert.ToString(Session.Item("Payment_Reason")))
                'json("success") = "Request submitted successfully. Please check your registered email id after 3 working days.<br>Please note your Request ID: <b>" + rets + "</b> for future reference."
                'json("success") = "Request submitted successfully. Please check your registered email id after 15 working days.<br>Please note your Request ID: <b>" + rets + "</b> for future reference."
                json("success") = "Request submitted successfully. Please check your registered Email ID.<br>Please note your Request ID: <b>" + rets + "</b> for future reference."
                'Added on 21-11-2023 For Request from BSK
                json("req_id") = rets
                json("req_time") = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                'End Added on 21-11-2023 For Request from BSK
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
                'Sending Acknowledgement to the requestee
                Dim l1 As New Dictionary(Of String, Object)
                l1("name") = groom_name + ", " + bride_name
                l1("email") = a.sqlInj_out(email)
                l1("req_dt") = DateTime.Today.ToString("dd/MM/yyyy")
                l1("req_id") = If(rets.Length >= 13, rets, "")
                l1("serve_dt") = DateTime.Today.AddDays(15).ToString("dd/MM/yyyy")
                l1("old") = "N"
                Dim data = javaScriptSerializer.Serialize(l1)
                Dim encData = enc.encrypt(data)
                Dim someUrl = baseUrl() + "MARREG_CertifiedCopy/Acknowledgement.ashx?encData=" + encData
                Dim ack_byte As Byte()
                Using webClient = New Net.WebClient()
                    ack_byte = webClient.DownloadData(someUrl)
                End Using
                Dim r_email_ids As New List(Of String)
                'email = "utsab.87@gmail.com"
                r_email_ids.Add(a.sqlInj_out(email))
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
                Dim newStr() As String = {rets}
                Common.ReplaceEmailBody(body, oldStr, newStr)

                'Dim email_Body = "<b>DO NOT REPLY TO THIS E-MAIL.USE THE E-MAIL OF MARRIAGE REGISTRAR MENTIONED BELOW </b><br/>" &
                '                "<br/>Dear Madam/ Sir <br/>" &
                '                "<b>SUBJECT:</b> Acknowledgement" &
                '                "<br>" + json("success") + "<br><br>With Regards <br/>REGISTRAR GENERAL OF MARRIAGES <br/>LAW DEPARTMENT <br/>GOVERNMENT OF WEST BENGAL <br/> rgm-wb@nic.in"
                Send_Email_Bytes(r_email_ids, subject, body, ack_bytes)
                'Sending Acknowledgement to the requestee ends
            ElseIf rets = "0" Then     'Requesting others details
                json("error") = "Request submission error. Please provide valid information associated to your email id."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
            ElseIf rets = "-1" Then     'Duplicate value
                json("error") = "Your previous request is already open."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
            ElseIf rets = "-2" Then   'Transaction rollback
                json("error") = "Internal error occured. Please try again later."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
            ElseIf rets = "-3" Then   'Application has been nullified of divorced
                json("error") = "This application has been dissolved."
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
    Public Sub CertifiedCopyRequestRe()
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
            Dim email As String = If(Not Request("reqEmail") Is Nothing, DirectCast(Request("reqEmail"), String).Trim(), "")
            Dim mo_name As String = If(Not Request("mo_name") Is Nothing, DirectCast(Request("mo_name"), String).Trim(), "")
            Dim reqId As String = If(Not Request("reqId") Is Nothing, DirectCast(Request("reqId"), String).Trim(), "")

            If mo_name = "" Or mo_name Is Nothing Then
                response("error") = "Please enter MO Name."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If reqId = "" And email = "" Then
                response("error") = "Please initiate the process again."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            email = a.sqlInj_in(email)
            Dim obj As New WebService_BL

            Dim rets = obj.Save_CertifiedCopyRequestRe(email, mo_name, reqId)
            If rets = "1" Then
                'json("success") = "Request submitted successfully. Please check your registered email id after 3 working days.<br>Please note your Request ID: <b>" + reqId + "</b> for future reference."
                json("success") = "Request submitted successfully. Please check your registered email id after 15 working days.<br>Please note your Request ID: <b>" + reqId + "</b> for future reference."
                'json("success") = "Request submitted successfully. Please check your registered Email ID.<br>Please note your Request ID: <b>" + reqId + "</b> for future reference."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
            ElseIf rets = "-1" Then     'Not valid data supplied
                json("error") = "Your request could not be submitted. Please try again later"
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
#End Region
#Region "Certified Copy Request Details"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub CertifiedCopyRequestDetails()
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
            'Added on 02-04-2024
            If (user_info.role_nm = "DASPL" Or user_info.role_nm = "SO") Then
                mo_auto_cd = "1"
            End If
            'End Added on 02-04-2024

            Dim status As String = If(Not (Request("status") Is Nothing Or Request("status") = ""), Convert.ToString(Request("status")), "")
            Dim req_to As String = If(Not (Request("req_to") Is Nothing Or Request("req_to") = ""), Convert.ToString(Request("req_to")), "")
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

            myDataTable = WebService_BL.CertifiedCopyRequestDetails(mo_auto_cd, req_to, status, draw, start, length, value, order_by, orderByDir)
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
#End Region
#Region "Certified Copy Search"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub Search()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
        Dim myDataTable As New MyDataTable()
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
            Dim id As Int64 = If(Not (Request("id") Is Nothing Or Request("id") = ""), Convert.ToInt64(Request("id")), 0)
            Dim mode As Int16 = If(Not (Request("mode") Is Nothing Or Request("mode") = ""), Convert.ToInt64(Request("mode")), 0)
            Dim email As String = If(Not Request("email") Is Nothing, DirectCast(Request("email"), String).Trim(), "")
            Dim groom_name As String = If(Not Request("groom_name") Is Nothing, DirectCast(Request("groom_name"), String).Trim(), "")
            Dim bride_name As String = If(Not Request("bride_name") Is Nothing, DirectCast(Request("bride_name"), String).Trim(), "")
            Dim groom_dob As String = If(Not Request("groom_dob") Is Nothing, DirectCast(Request("groom_dob"), String).Trim(), "")
            Dim bride_dob As String = If(Not Request("bride_dob") Is Nothing, DirectCast(Request("bride_dob"), String).Trim(), "")
            Dim reg_date As String = If(Not Request("reg_date") Is Nothing, DirectCast(Request("reg_date"), String).Trim(), "")
            Dim certificate_no As String = If(Not Request("certificate_no") Is Nothing, DirectCast(Request("certificate_no"), String).Trim(), "")
            Dim mo_name As String = If(Not Request("mo_name") Is Nothing, DirectCast(Request("mo_name"), String).Trim(), "")
            Dim req_outsider_id As Int64 = If(Not (Request("req_outsider_id") Is Nothing Or Request("req_outsider_id") = ""), Convert.ToInt64(Request("req_outsider_id")), 0)

            Dim draw As String = If(Not Request("draw") Is Nothing, DirectCast(Request("draw"), String), "1")
            Dim start As String = If(Not Request("start") Is Nothing, DirectCast(Request("start"), String), "0")
            Dim length As String = If(Not Request("length") Is Nothing, DirectCast(Request("length"), String), "10")
            Dim value As String = If(Not Request("search[value]") Is Nothing, DirectCast(Request("search[value]"), String).Trim(), "")
            Dim orderByColumn As String = If(Request("order[0][column]") Is Nothing, "", DirectCast(Request("order[0][column]"), String))
            Dim orderByDir As String = If(Request("order[0][dir]") Is Nothing, "", DirectCast(Request("order[0][dir]"), String))
            Dim order_by As String = "Registration_Date"

            email = a.sqlInj_in(email)
            Dim obj As New WebService_BL
            myDataTable = obj.Search_CertifiedCopyRequest(id, mode, email, groom_name, bride_name, groom_dob, bride_dob, reg_date, certificate_no, mo_name, req_outsider_id, draw, start, length, value, order_by, orderByDir)

        Catch ex As Exception
            response("exception") = "true"
            response("exception_message") = ex.Message
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
#Region "Certified Copy Request Approve"
    Dim mo_auto_cd As String = ""
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub CertifiedCopyRequestApprove()
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
            Dim id As Int64 = If(Not (Request("id") Is Nothing And Request("id") = ""), Convert.ToInt64(Request("id").Trim()), 0)
            Dim app_auto_cd As String = If(Not (Request("app_auto_cd") Is Nothing Or Request("app_auto_cd") = ""), DirectCast(Request("app_auto_cd"), String).Trim(), "")
            Dim status As String = If(Not (Request("status") Is Nothing Or Request("status") = ""), Convert.ToString(Request("status")), "")
            Dim act_cd As String = If(Not (Request("act") Is Nothing Or Request("act") = ""), DirectCast(Request("act"), String).Trim(), "")
            Dim otp As String = Convert.ToString(Session("ar_otp"))
            Dim user_info As New Marreg_Loginfo
            user_info = DirectCast(Session.Item("Marreg_user_info"), Marreg_Loginfo)
            Dim user_nm = user_info.user_nm
            mo_auto_cd = user_info.MO_cd

            Dim status_array = {"02", "06", "09", "15"}
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
            Dim obj As New WebService_BL
            Dim rets = obj.Save_CertifiedCopyRequestApprove(id, app_auto_cd, status, otp, user_nm)
            If Convert.ToInt64(rets) = 1 Then
                Dim status_array_n = {"06", "15"}
                If status_array_n.Contains(status) Then
                    Dim ret = GenerateAndSendEmailCert(id, status, Nothing)
                    If (ret = "0") Then
                        json("success") = "Process done successfully and Email sending success."
                    ElseIf (ret = "1") Then
                        json("success") = "Process done successfully and Email sending error."
                    Else
                        json("success") = "Process done successfully."
                    End If
                Else
                    m.Rollback_CertifiedCopyRequestApprove(id)
                    json("data") = Generate_Certificate_Bytes(id, status, app_auto_cd, act_cd)
                End If
                ''Send Email Start
                'Dim email_stat = GenerateAndSendEmail(id, status, app_auto_cd, act_cd)
                'If (email_stat = "0") Then
                '    json("success") = "Process done successfully and Email sending success."
                'ElseIf (email_stat = "1") Then
                '    json("success") = "Process done successfully and Email sending error."
                'Else
                '    json("success") = "Process done successfully."
                'End If
                ''Send Email End

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
#End Region
#Region "Generate Certificate Bytes"
    Private Function GetSignerDetail() As String
        'Load Registered DSC Details
        Dim obj = New MARREG.App_Code.DSC_BL()
        'Dim Data_List = obj.LoadMODSCDetails("363")
        Dim Data_List = obj.LoadMODSCDetails(mo_auto_cd)
        Dim javaScriptSerializer As New Script.Serialization.JavaScriptSerializer()
        Dim Single_Data = Nothing
        For Each dd As Dictionary(Of String, Object) In Data_List
            If (dd("Valid_Status") = "Y" And dd("Active_Status") = "Y") Then
                Single_Data = dd
            End If
        Next
        Dim das = javaScriptSerializer.Serialize(Single_Data)
        'End of Load Registered DSC Details
        Dim encDec As New MARREG_SHA256()
        Return encDec.Encryption(das)
    End Function
    Private Function Generate_Certificate_Bytes(id As Int64, status As String, app_auto_cd As String, act_cd As String) As Dictionary(Of String, String)
        Dim ret As New Dictionary(Of String, String)
        Try
            Dim db As New MasterDB
            Dim f = db.FecthCertificate(app_auto_cd)
            Dim bytFile As Byte()
            Dim base64String = ""
            Dim file_pass = ""
            If (f.Rows.Count > 0) Then
                Dim sqlquery = "select t.Act_cd, UPPER(FORMAT(s.Registration_Date, 'ddMMMyyyy')) as RegDt from MARREG_NewApp_OtherInfo_temp t inner join MARREG_New_applicant_status s on t.App_Auto_Cd=s.App_Auto_Cd where t.App_Auto_Cd='" & app_auto_cd & "'"
                Dim dt As DataTable = a.selectdata(sqlquery)
                Dim regdt_pass = dt.Rows(0)("RegDt").ToString()
                If (regdt_pass = "") Then
                    Return Nothing
                End If

                bytFile = f.Rows(0)("FileStreamCol")
                base64String = Convert.ToBase64String(bytFile, 0, bytFile.Length)
                file_pass = regdt_pass
            Else
                Dim QRCODE_string As Byte() = a.qrcode(app_auto_cd)
                a.update_QR_code(QRCODE_string, app_auto_cd)
                Dim val = SetReportParameters(app_auto_cd, act_cd)
                file_pass = val("regdt_pass")
                base64String = val("base64String")
            End If

            Dim sign_date = DateTime.Now.ToString()
            Dim signer = GetSignerDetail()
            ret("base64String") = base64String
            ret("file_pass") = file_pass
            ret("App_Auto_Cd") = app_auto_cd
            ret("signer") = signer
            ret("sign_date") = sign_date
            ret("id") = id
            ret("status") = status
            Return ret
        Catch ex As Exception
            'To do "Rollback certified copy request process"
            m.Rollback_CertifiedCopyRequestApprove(id)
            Dim p = ex.Message
            Throw New Exception(ex.Message)
        End Try
    End Function
#End Region
#Region "Email Certificate"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
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
            Dim id As Int64 = If(Not (Request("id") Is Nothing And Request("id") = ""), Convert.ToInt64(Request("id").Trim()), 0)
            Dim status As String = If(Not Request("status") Is Nothing, DirectCast(Request("status"), String).Trim(), "")
            If (out_bytes_str = "") Then
                response("error") = "No valid file specified. Please generate the certificate again to sign."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (app_Auto_Cd = "") Then
                response("error") = "Application No not found."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            Dim file_byte = Convert.FromBase64String(out_bytes_str)

            Dim res = m.Save_Certificate(file_byte, id)
            If res = "1" Then
                Dim ret = GenerateAndSendEmailCert(id, status, file_byte)
                'Dim ret = 0
                If (ret = "0") Then
                    response("success") = "Process done successfully and Email sending success."
                ElseIf (ret = "1") Then
                    response("success") = "Process done successfully and Email sending error."
                Else
                    response("success") = "Process done successfully."
                End If
            Else
                'To do "Rollback certified copy request process"
                m.Rollback_CertifiedCopyRequestApprove(id)
                response("error") = "Please process the request once again."
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
#Region "Send Email"
    Dim eml As New BusinessLogicLayer.Email_Sms_BL()
    Private Function FetchRequestDetail(id As Int64, status As String) As DataSet
        Dim obj As New WebService_BL
        Dim requests = obj.FetchRequestDetail(id, status)
        Return requests
    End Function
    Private Function GenerateAndSendEmailCert(id As Int64, status As String, bytes As Byte()) As String
        Dim retVal As String = ""
        Dim email As String

        Dim objBodySub As New Dictionary(Of String, Object)
        Try
            Dim requests = FetchRequestDetail(id, status)
            Dim no_copy = Convert.ToInt32(requests.Tables(0).Rows(0)("no_copy"))
            email = Convert.ToString(requests.Tables(0).Rows(0)("email"))
            email = a.sqlInj_out(email)
            objBodySub = GenSubjectAndBodyEmail(status, no_copy, requests)

            'email = "utsab.87@gmail.com"
            Dim email_ids As New List(Of String)
            Dim filepaths As New List(Of Byte())
            email_ids.Add(email)
            If (status = "09") Then
                For i As Integer = 1 To no_copy
                    filepaths.Add(bytes)
                Next
            End If
            'retVal = Send_Email(email_ids, objBodySub("subject"), objBodySub("body"), filepaths)

            retVal = Send_Email_Bytes(email_ids, objBodySub("subject"), objBodySub("body"), filepaths)

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return retVal
    End Function
    Private Function FetchRequestDetailOld(id As Int64, status As String) As DataSet
        Dim obj As New WebService_BL
        Dim requests = obj.FetchRequestDetailOld(id, status)
        Return requests
    End Function
    Private Function GenerateAndSendEmailCertOld(id As Int64, status As String, bytes As Byte()) As String
        Dim retVal As String = ""
        Dim email As String

        Dim objBodySub As New Dictionary(Of String, Object)
        Try
            Dim requests = FetchRequestDetailOld(id, status)
            Dim no_copy = Convert.ToInt32(requests.Tables(0).Rows(0)("no_copy"))
            email = Convert.ToString(requests.Tables(0).Rows(0)("email"))
            email = a.sqlInj_out(email)
            objBodySub = GenSubjectAndBodyEmailOld(status, no_copy, requests)

            'email = "utsab.87@gmail.com"
            Dim email_ids As New List(Of String)
            Dim filepaths As New List(Of Byte())
            email_ids.Add(email)
            If (status = "09") Then
                For i As Integer = 1 To no_copy
                    filepaths.Add(bytes)
                Next
            End If
            'retVal = Send_Email(email_ids, objBodySub("subject"), objBodySub("body"), filepaths)

            retVal = Send_Email_Bytes(email_ids, objBodySub("subject"), objBodySub("body"), filepaths)

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return retVal
    End Function
    Private Function GenSubjectAndBodyEmail(status As String, no_copy As Integer, requests As DataSet) As Dictionary(Of String, Object)
        Dim ret As New Dictionary(Of String, Object)
        Try
            Dim groom_name = Convert.ToString(requests.Tables(0).Rows(0)("groom_name"))
            Dim bride_name = Convert.ToString(requests.Tables(0).Rows(0)("bride_name"))
            Dim groom_dob = Convert.ToString(requests.Tables(0).Rows(0)("groom_dob"))
            Dim bride_dob = Convert.ToString(requests.Tables(0).Rows(0)("bride_dob"))
            Dim reg_date = Convert.ToString(requests.Tables(0).Rows(0)("reg_date"))
            Dim certificate_no = Convert.ToString(requests.Tables(0).Rows(0)("certificate_no"))
            Dim mo_name = Convert.ToString(requests.Tables(0).Rows(0)("mo_name"))
            Dim req_act = Convert.ToString(requests.Tables(0).Rows(0)("req_act"))
            Dim request_id = Convert.ToString(requests.Tables(0).Rows(0)("request_id"))
            Dim memo_no = Convert.ToString(requests.Tables(0).Rows(0)("memo_no"))     'ADDED ON 19-04-2024
            Dim str = "<strong>"
            str += If(groom_name <> "", "Groom Name: " + groom_name.ToUpper + "<br>", "")
            str += If(bride_name <> "", "Bride Name: " + bride_name.ToUpper + "<br>", "")
            str += If(groom_dob <> "", "Groom DOB: " + Convert.ToDateTime(groom_dob).ToString("dd/MM/yyyy") + "<br>", "")
            str += If(bride_dob <> "", "Bride DOB: " + Convert.ToDateTime(bride_dob).ToString("dd/MM/yyyy") + "<br>", "")
            str += If(reg_date <> "", "Registration Date: " + Convert.ToDateTime(reg_date).ToString("dd/MM/yyyy") + "<br>", "")
            str += If(certificate_no <> "", "Certificate No.: " + certificate_no.ToUpper + "<br>", "")
            str += If(mo_name <> "", "MO Name: " + mo_name.ToUpper + "<br>", "")
            str += If(req_act <> "", "Act/ Section under which the Marriage was Solemnized/ Registered: " + req_act.ToUpper + "<br>", "")
            str += "</strong>"
            Select Case status
                Case "06"
                    'Dim sql = "select * from MARREG_Email_Format where lower(email_type)=lower('')"
                    'Dim d = a.selectdata(sql)
                    Dim obj1 As New WS_MasterData_BL()
                    Dim ds = obj1.FetchEmailFormat(Certified_Copy_Request_Reject)
                    Dim d = ds.Tables(0)
                    ret("subject") = d.Rows(0)("email_subject")
                    ret("body") = d.Rows(0)("email_body")
                    Dim oldStr() As String = {"{request_id}", "{request_details}"}
                    Dim newStr() As String = {request_id, str}
                    Common.ReplaceEmailBody(ret("body"), oldStr, newStr)

                    'ret("subject") = "Request for Certified Copy of Marriage Certificate has been rejected"
                    'ret("body") = "<b>DO NOT REPLY TO THIS E-MAIL.USE THE E-MAIL OF MARRIAGE REGISTRAR MENTIONED BELOW </b><br/>" &
                    '            "<br/>DEAR MADAM/SIR <br/>" &
                    '            "<b>SUBJECT:</b> Request ID " & request_id & " | Request for Certified Copy of Marriage Certificate has been rejected" &
                    '            "<br/>REQUEST FOR CERTIFIED COPY OF THE MARRIAGE CERTIFICATE HAS BEEN REJECTED." &
                    '            "<br/>NO RECORD IS MATCHING WITH YOUR REQUEST:" &
                    '            "<br/>" & str &
                    '            "<br/><br/>WITH REGARDS <br/>REGISTRAR GENERAL OF MARRIAGES <br/>LAW DEPARTMENT <br/>GOVERNMENT OF WEST BENGAL <br/> rgm-wb@nic.in"
                    ret("filepath") = ""
                Case "09"
                    'Dim sql = "select * from MARREG_Email_Format where lower(email_type)=lower('Certified Copy Request Approve')"
                    'Dim d = a.selectdata(sql)
                    Dim obj1 As New WS_MasterData_BL()
                    Dim ds = obj1.FetchEmailFormat(Certified_Copy_Request_Approve)
                    Dim d = ds.Tables(0)
                    ret("subject") = d.Rows(0)("email_subject")
                    ret("body") = d.Rows(0)("email_body")
                    Dim oldStr() As String = {"{request_id}", "{memo_no}", "{request_details}", "{no_of_copy}"}     'UPDATED ON 19-04-2024
                    Dim newStr() As String = {request_id, memo_no, str, no_copy}                                    'UPDATED ON 19-04-2024
                    Common.ReplaceEmailBody(ret("body"), oldStr, newStr)

                    'ret("subject") = "Certified Copy of Marriage Certificate"
                    'ret("body") = "<b>DO NOT REPLY TO THIS E-MAIL.USE THE E-MAIL OF MARRIAGE REGISTRAR MENTIONED BELOW </b><br/>" &
                    '            "<br/>DEAR MADAM/SIR <br/>" &
                    '            "<b>SUBJECT:</b> Request ID " & request_id & " | Certified Copy of Marriage Certificate" &
                    '            "<br/>REQUEST FOR CERTIFIED COPY OF THE MARRIAGE CERTIFICATE HAS BEEN ACCEPTED AND PROCESSED AS PER YOUR REQUEST:" &
                    '            "<br/>" & str &
                    '            "<br/>COPIES ARE ATTACHED WITH THIS EMAIL (" & no_copy & " COPIES). PLEASE DOWNLOAD AND OPEN BY ENTERING PASSWORD." &
                    '            "YOUR PASSWORD WILL BE THE DATE OF SOLEMNIZATION/ REGISTRATION LIKE 23APR2023 IF YOUR DATE OF SOLEMNIZATION/ REGISTRATION IS 23/04/2023." &
                    '            "<br/><br/>WITH REGARDS <br/>REGISTRAR GENERAL OF MARRIAGES <br/>LAW DEPARTMENT <br/>GOVERNMENT OF WEST BENGAL <br/> rgm-wb@nic.in"
                    'SetReportParameters(app_auto_cd, act_cd)
                    ret("filepath") = Session("pdfnm_ccmc")
                Case "15"
                    'Dim sql = "select * from MARREG_Email_Format where lower(email_type)=lower('Certified Copy Request On Hold')"
                    'Dim d = a.selectdata(sql)
                    Dim obj1 As New WS_MasterData_BL()
                    Dim ds = obj1.FetchEmailFormat(Certified_Copy_Request_On_Hold)
                    Dim d = ds.Tables(0)
                    ret("subject") = d.Rows(0)("email_subject")
                    ret("body") = d.Rows(0)("email_body")
                    Dim oldStr() As String = {"{request_id}", "{request_details}", "{link}"}
                    Dim newStr() As String = {request_id, str, "<a href='" & Common.baseUrl() & "MARREG_CertifiedCopy/MARREG_CertifiedCopyOfCertificates.aspx?mode=verify' target='_blank'>HERE</a>"}
                    Common.ReplaceEmailBody(ret("body"), oldStr, newStr)

                    'ret("subject") = "Request for Certified Copy of Marriage Certificate is misleading"
                    'ret("body") = "<b>DO NOT REPLY TO THIS E-MAIL.USE THE E-MAIL OF MARRIAGE REGISTRAR MENTIONED BELOW </b><br/>" &
                    '            "<br/>DEAR MADAM/SIR <br/>" &
                    '            "<b>SUBJECT:</b> Request ID " & request_id & " | Request for Certified Copy of Marriage Certificate is misleading" &
                    '            "<br/>REQUEST FOR CERTIFIED COPY OF MARRIAGE CERTIFICATE IS NOT CONCLUSIVE. PLEASE PROVIDE MARRIAGE OFFICER'S NAME ALONG WITH THE REQUEST PROVIDED." &
                    '            "<br/>PLEASE CLICK <a href='" & Common.baseUrl() & "MARREG_CertifiedCopy/MARREG_CertifiedCopyOfCertificates.aspx?mode=verify'>HERE</a> AND PUT YOUR REQUEST ID: <b>" & request_id & "</b> AND REGISTERED EMAIL ID IN THE RELEVANT FIELDS:" &
                    '            "<br/>" & str &
                    '            "<br/><br/>WITH REGARDS <br/>REGISTRAR GENERAL OF MARRIAGES <br/>LAW DEPARTMENT <br/>GOVERNMENT OF WEST BENGAL <br/> rgm-wb@nic.in"
                    ret("filepath") = ""
            End Select
        Catch ex As Exception
            Dim p = ex.Message
            Throw New Exception(ex.Message)
        End Try
        Return ret
    End Function
    Private Function Send_Email(email_ids As List(Of String), subject As String, body As String, Optional filepaths As List(Of String) = Nothing) As String
        Dim MS As String
        'MS = SendMailMessage(email_ids, subject, body, filepaths)
        MS = eml.send_email_multiple(email_ids, subject, body, filepaths)
        'If filepath = "" Or filepath Is Nothing Then
        '    MS = eml.send_email(email_id, subject, body)
        'Else
        '    MS = eml.send_email_attachment(email_id, subject, body, filepath)
        'End If
        Return MS
    End Function
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
    'For Testing Purpose
    Public Function SendMailMessage_Bytes(ByVal recepients As List(Of String), ByVal subject As String, ByVal body As String, Optional fle As List(Of Byte()) = Nothing) As String
        Dim ret = ""
        Dim mMailMessage As New Net.Mail.MailMessage() With {
            .From = New Net.Mail.MailAddress("utsab.87@gmail.com")
        }
        For Each r As String In recepients
            mMailMessage.To.Add(New Net.Mail.MailAddress(r))
        Next

        mMailMessage.Subject = Mid(subject, 1, 50)
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

    Public Function SendMailMessage(ByVal recepients As List(Of String), ByVal subject As String, ByVal body As String, Optional fle As List(Of String) = Nothing) As String
        Dim ret = ""
        Dim mMailMessage As New Net.Mail.MailMessage() With {
            .From = New Net.Mail.MailAddress("utsab.87@gmail.com")
        }
        For Each r As String In recepients
            mMailMessage.To.Add(New Net.Mail.MailAddress(r))
        Next

        mMailMessage.Subject = Mid(subject, 1, 50)
        mMailMessage.Body = body

        If fle IsNot Nothing Then
            For x As Integer = 0 To fle.Count - 1
                If Not String.IsNullOrEmpty(fle.Item(x)) Then
                    mMailMessage.Attachments.Add(New Net.Mail.Attachment(Server.MapPath(fle.Item(x))))
                End If
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
    Private Function SetReportParameters(App_Auto_Cd As String, act_cd As String) As Dictionary(Of String, String)
        Dim ret As New Dictionary(Of String, String)
        Dim RptViewer As New ReportViewer
        Try
            RptViewer.ProcessingMode = ProcessingMode.Remote

            RptViewer.ServerReport.ReportServerUrl = New Uri(ConfigurationManager.AppSettings("reportserver").ToString())
            Dim strUserName As String = ConfigurationManager.AppSettings("uid").ToString()
            Dim strPassword As String = ConfigurationManager.AppSettings("pwd").ToString()
            Dim strDomain As String = ConfigurationManager.AppSettings("domain").ToString()
            RptViewer.ServerReport.ReportServerCredentials = New ReportViewerCredentials(strUserName, strPassword, strDomain)

            Dim sqlquery = "select t.Act_cd, UPPER(FORMAT(s.Registration_Date, 'ddMMMyyyy')) as RegDt from MARREG_NewApp_OtherInfo_temp t inner join MARREG_New_applicant_status s on t.App_Auto_Cd=s.App_Auto_Cd where t.App_Auto_Cd='" & App_Auto_Cd & "'"
            Dim dt As DataTable = a.selectdata(sqlquery)
            Dim regdt_pass = dt.Rows(0)("RegDt").ToString()
            If (regdt_pass = "") Then
                'ScriptManager.RegisterClientScriptBlock(Me, Me.[GetType](), "JS", "showpop6('Registration/ Solemnization Date not available','Error');", True)
                Return Nothing
            End If

            If act_cd = "01" Then
                RptViewer.ServerReport.ReportPath =
                "/" + ConfigurationManager.AppSettings("reportdir").ToString() + "/" + "Certified_HinduA4_appr"
            ElseIf act_cd = "02" Then
                RptViewer.ServerReport.ReportPath =
                "/" + ConfigurationManager.AppSettings("reportdir").ToString() + "/" + "Certified_Spcl13A4_appr"
            ElseIf act_cd = "03" Then
                RptViewer.ServerReport.ReportPath =
                "/" + ConfigurationManager.AppSettings("reportdir").ToString() + "/" + "Certified_Spcl16A4_appr"
            End If

            RptViewer.ServerReport.Refresh()
            Dim paramList As New Generic.List(Of ReportParameter)

            paramList.Add(New ReportParameter("App_Auto_Cd", App_Auto_Cd))
            RptViewer.ServerReport.SetParameters(paramList)
            'pdf generaation-----
            Dim warnings As Warning() = Nothing
            Dim streamids As String() = Nothing
            Dim mimeType As String = String.Empty, encoding As String = String.Empty, extension As String = String.Empty, deviceInfo As String
            deviceInfo = "True"
            Dim bytes As Byte() = RptViewer.ServerReport.Render("PDF", Nothing, mimeType, encoding, extension, streamids, warnings)



            Dim byte1() As Byte = Nothing
            Dim reader = New PdfReader(bytes)

            Dim base64String As String = ""
            Using ms As New MemoryStream()
                Dim pdfStamper = New PdfStamper(reader, ms)

                Dim encryptionParams As New PdfEncryption()
                Dim filepass = regdt_pass
                Dim ownerPass = System.Text.Encoding.ASCII.GetBytes(filepass)

                ' Set the encryption parameters for the PDF stamper
                pdfStamper.SetEncryption(ownerPass, ownerPass, PdfWriter.AllowPrinting Or PdfWriter.AllowCopy Or PdfWriter.AllowScreenReaders, PdfWriter.ENCRYPTION_AES_128)
                pdfStamper.FormFlattening = True
                pdfStamper.SetFullCompression()
                ' Close the PDF stamper and the PDF reader
                pdfStamper.Close()
                reader.Close()
                byte1 = ms.ToArray()

                base64String = Convert.ToBase64String(byte1, 0, byte1.Length)

                'ScriptManager.RegisterClientScriptBlock(Me, Me.[GetType](), "JS", "signCertificate('" + base64String + "','" + filepass + "','" + signer + "','" + sign_date + "','" + App_Auto_Cd + "');", True)
            End Using
            ret("base64String") = base64String
            ret("regdt_pass") = regdt_pass
            Return ret
            'Dim filenm = "MARREG_Certified_Copy_" + App_Auto_Cd + ".pdf"
            'Dim filepath = "~/pdf_temp/" + filenm
            'Using fs As New FileStream(Server.MapPath(filepath), FileMode.OpenOrCreate)
            '    fs.Write(bytes, 0, bytes.Length)
            '    'Session("pdfnm_ccmc") = filepath
            '    fs.Close()
            'End Using

            'Dim filepaths As New List(Of String)
            'For i As Integer = 0 To no_copy
            '    filepaths.Add(filepath)
            '    i += 1
            'Next

            'Dim outputPath = "~/pdf_temp/" + "Final_" + filenm
            'Dim doc = New iTextSharp.text.Document()
            'Dim writer = New iTextSharp.text.pdf.PdfCopy(doc, New FileStream(Server.MapPath(outputPath), FileMode.Create))
            'If (writer Is Nothing) Then
            '    Return
            'End If
            'doc.Open()
            'For Each filename In filepaths
            '    Dim reader = New iTextSharp.text.pdf.PdfReader(Server.MapPath(filename))
            '    reader.ConsolidateNamedDestinations()
            '    For i As Integer = 1 To reader.NumberOfPages
            '        Dim page As iTextSharp.text.pdf.PdfImportedPage = writer.GetImportedPage(reader, i)
            '        writer.AddPage(page)
            '    Next
            '    reader.Close()
            'Next
            'writer.Close()
            'doc.Close()
            'Session("pdfnm_ccmc") = outputPath

        Catch ex As Exception
            Dim p = ex.Message
            Throw New Exception(ex.Message)
        End Try
    End Function
#End Region
#Region "Resend Email"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub ResendEmail()
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
            Dim app_auto_cd As String = If(Not (Request("app_auto_cd") Is Nothing Or Request("app_auto_cd") = ""), DirectCast(Request("app_auto_cd"), String).Trim(), "")
            Dim status As String = If(Not (Request("status") Is Nothing Or Request("status") = ""), Convert.ToString(Request("status")), "")
            Dim act_cd As String = If(Not (Request("act") Is Nothing Or Request("act") = ""), DirectCast(Request("act"), String).Trim(), "")

            Dim status_array = {"02", "06", "09", "15"}
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
            'to do get certified copy byte
            Dim db As New WebService_DB
            Dim f = db.FecthCertifiedCopy(id)
            Dim bytFile As Byte() = Nothing
            Dim base64String = ""
            If (f.Rows.Count > 0) Then
                bytFile = f.Rows(0)("FileStreamCol")
            End If

            Dim email_stat = GenerateAndSendEmailCert(id, status, bytFile)
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
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub ResendHEmail()
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

            'Send Email Start
            Dim email_stat = GenerateAndSendEmail_Hearing(id)
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
#Region "Validate Request Status"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub ValidateRequestStatus()
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
            Dim reqid As String = If(Not Request("reqid") Is Nothing, DirectCast(Request("reqid"), String).Trim(), "")
            Dim email As String = If(Not Request("email") Is Nothing, DirectCast(Request("email"), String).Trim(), "")
            Dim mode As String = If(Not Request("mode") Is Nothing, DirectCast(Request("mode"), String).Trim(), "")
            email = a.sqlInj_in(email)
            Dim obj As New WebService_BL
            Dim ReqStatus = obj.ValidateRequestStatus(email, reqid)
            Dim dtReqStatus = ReqStatus.Tables(0)
            If dtReqStatus.Rows.Count < 1 Then
                response("error") = "You are not authorized to proceed."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (mode = "verify") Then
                response("success") = dtReqStatus.Rows.Count
            ElseIf (mode = "view") Then
                Dim rows As New List(Of Dictionary(Of String, Object))()
                Dim row As Dictionary(Of String, Object)
                For Each dr As DataRow In dtReqStatus.Rows
                    row = New Dictionary(Of String, Object)()
                    For Each col As DataColumn In dtReqStatus.Columns
                        Dim dataVal = dr(col)
                        If (col.DataType = System.Type.GetType("System.DateTime")) Then
                            dataVal = If(dr(col) Is DBNull.Value, "", Convert.ToDateTime(dataVal).ToString("dd/MM/yyyy"))
                        End If
                        row.Add(col.ColumnName, dataVal)
                    Next
                    rows.Add(row)
                Next
                response("success") = rows
            End If
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
#Region "Certified Copy Request Outsider"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub CertifiedCopyRequestInfoValidate()
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
            Dim email As String = If(Not Request("email") Is Nothing, DirectCast(Request("email"), String).Trim(), "")
            Dim groom_name As String = If(Not Request("groom_name") Is Nothing, DirectCast(Request("groom_name"), String).Trim(), "")
            Dim bride_name As String = If(Not Request("bride_name") Is Nothing, DirectCast(Request("bride_name"), String).Trim(), "")
            Dim groom_dob As String = If(Not Request("groom_dob") Is Nothing, DirectCast(Request("groom_dob"), String).Trim(), "")
            Dim bride_dob As String = If(Not Request("bride_dob") Is Nothing, DirectCast(Request("bride_dob"), String).Trim(), "")
            Dim reg_date As String = If(Not Request("reg_date") Is Nothing, DirectCast(Request("reg_date"), String).Trim(), "")
            Dim mo_name As String = If(Not Request("mo_name") Is Nothing, DirectCast(Request("mo_name"), String).Trim(), "")
            Dim ActCd As String = If(Not Request("ActCd") Is Nothing, DirectCast(Request("ActCd"), String).Trim(), "")


            If groom_name = "" Or groom_name Is Nothing Then
                response("error") = "Please enter Groom/ Husband Name."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If bride_name = "" Or bride_name Is Nothing Then
                response("error") = "Please enter Bride/ Wife Name."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If groom_dob = "" And bride_dob = "" Then
                response("error") = "Please enter either Groom/ Husband DOB or Bride/ Wife DOB."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If reg_date = "" Or reg_date Is Nothing Then
                response("error") = "Please enter Date of Registration."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If mo_name = "" Or mo_name Is Nothing Then
                response("error") = "Please enter MO Name."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If ActCd = "" Or ActCd Is Nothing Or ActCd = "0" Then
                response("error") = "Please select Act/ Section under which the Marriage was Solemnized/ Registered."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If

            Dim obj As New WebService_BL
            Dim rets = obj.CertifiedCopyRequestInfoValidate(groom_name, bride_name, groom_dob, bride_dob, reg_date, mo_name, ActCd)
            If rets = "0" Then
                json("error") = "No such information found."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
            ElseIf rets = "-1" Then     'Duplicate value
                json("error") = "Please fill all the fields properly."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
            ElseIf rets = "-2" Then   'Transaction rollback
                json("error") = "Internal error occured. Please try again later."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
            ElseIf rets = "-3" Then   'Application has been nullified of divorced
                json("error") = "This application has been dissolved."
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
    Public Sub CertifiedCopyRequestEmailValidate()
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
            Dim email As String = If(Not Request("email") Is Nothing, DirectCast(Request("email"), String).Trim(), "")
            Dim validatedId As String = If(Not (Request("validatedId") Is Nothing Or Request("validatedId") = ""), DirectCast(Request("validatedId"), String).Trim(), "0")
            Dim vId = Convert.ToInt64(Session("validatedId"))
            email = a.sqlInj_in(email)
            If name = "" Or name Is Nothing Then
                response("error") = "Please Enter Your Full Name."
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
            Dim rets = obj.CertifiedCopyRequestSaveEmailOutsider(name, mob_no, email, validatedId)
            If rets = "" Then
                json("error") = "Please try again."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
            Else
                Dim email_ids As New List(Of String)
                email_ids.Add(a.sqlInj_out(email))

                'Dim sql = "select * from MARREG_Email_Format where lower(email_type)=lower('OTP for Request of Certified Copy of Marriage Certificate')"
                'Dim d = a.selectdata(sql)
                Dim obj1 As New WS_MasterData_BL()
                Dim ds = obj1.FetchEmailFormat(OTP_for_Request_of_Certified_Copy_of_Marriage_Certificate)
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

                'Dim subject = "OTP for Request of Certified Copy"
                'Dim body = "<b>DO NOT REPLY TO THIS E-MAIL.USE THE E-MAIL OF MARRIAGE REGISTRAR MENTIONED BELOW </b><br/>" &
                '                "<br/>DEAR MADAM/SIR <br/>" &
                '                "<b>SUBJECT:</b> OTP for Request of Certified Copy of Marriage Certificate" &
                '                "<br/>OTP TO VERIFY EMAIL FOR REQUEST OF CERTIFIED COPY OF MARRIAGE CERTIFICATE IS " & rets & "." &
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
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub CertifiedCopyRequestEmailOTPValidate()
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
            Dim rets = obj.CertifiedCopyRequestEmailOTPValidate(validatedId, otp)
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
    Public Sub CertifiedCopyRequestSendOutsider()
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
            Dim nocopy As String = If(Not Request("no_copy") Is Nothing And Request("no_copy") <> "", DirectCast(Request("no_copy"), String).Trim(), "0")
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

            Dim no_copy = Convert.ToInt32(Session("no_copy"))
            If (no_copy <> Convert.ToInt64(nocopy)) Then
                response("error") = "Please initiate the process again."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                Exit Sub
            End If

            Dim obj As New WebService_BL
            Dim rets = obj.CertifiedCopyRequestSendOutsider(vId, grn_id, no_copy)
            Dim row = rets.Rows(0)
            If row("ret") = "-2" Then
                'rollback
                json("error") = "Unknown error occured. Please try again later."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
            ElseIf row("ret") = "-1" Then
                'duplicate
                json("error") = "Your previous request is already open."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
            ElseIf row("ret") = "0" Then
                'data missing
                json("error") = "Data missing."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
            Else
                'Ok
                Dim req_id = row("request_id").ToString()
                Dim Groom_Email_Id = row("Groom_Email_Id").ToString()
                Dim Bride_Email_Id = row("Bride_Email_Id").ToString()
                Dim certificate_no = row("certificate_no").ToString()
                Dim r_name = row("r_name").ToString().ToUpper()
                Dim r_mob = row("r_mob").ToString()
                Dim r_email = a.sqlInj_out(row("email").ToString())
                'json("success") = "Request submitted successfully. Please check your registered Email ID from next 3 working days to 15 working days.<br>Please note your Request ID: <b>" + req_id + "</b> for future reference."
                'json("success") = "Request submitted successfully. Please check your registered Email ID after 15 working days.<br>Please note your Request ID: <b>" + req_id + "</b> for future reference."
                json("success") = "Request submitted successfully. Please check your registered Email ID.<br>Please note your Request ID: <b>" + req_id + "</b> for future reference."
                'Added on 21-11-2023 For Request from BSK
                json("req_id") = req_id
                json("req_time") = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                'End Added on 21-11-2023 For Request from BSK
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
                'Sending Acknowledgement to the requestee
                Dim l1 As New Dictionary(Of String, Object)
                l1("name") = r_name
                l1("email") = r_email
                l1("req_dt") = DateTime.Today.ToString("dd/MM/yyyy")
                l1("req_id") = req_id
                l1("serve_dt") = DateTime.Today.AddDays(15).ToString("dd/MM/yyyy")
                l1("old") = "N"
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

                'Groom_Email_Id = "utsab.87@gmail.com"
                'Bride_Email_Id = "destiny.utsab@gmail.com"
                Dim email_ids As New List(Of String)
                If (Groom_Email_Id <> "") Then
                    email_ids.Add(a.sqlInj_out(Groom_Email_Id))
                End If
                If (Bride_Email_Id <> "") Then
                    email_ids.Add(a.sqlInj_out(Bride_Email_Id))
                End If

                Dim comment_link = enc.encrypt("rid=" & req_id)

                'Dim sql1 = "select * from MARREG_Email_Format where lower(email_type)=lower('Intimation for Comment')"
                'Dim d1 = a.selectdata(sql1)
                'Dim obj1 As New WS_MasterData_BL()
                Dim ds1 = obj1.FetchEmailFormat(Intimation_for_Comment)
                Dim d1 = ds1.Tables(0)
                Dim subject1 = d1.Rows(0)("email_subject")
                Dim body1 = d1.Rows(0)("email_body")
                Dim oldStr1() As String = {"{certificate_no}", "{requestee_name}", "{requestee_email}", "{requestee_mobile}", "{comment_link}"}
                Dim newStr1() As String = {certificate_no, r_name, r_email, r_mob, "<a href='" & Common.baseUrl() & "MARREG_CertifiedCopy/MARREG_CertifiedCopyComment.aspx?" & comment_link & "'>HERE</a>"}
                Common.ReplaceEmailBody(body1, oldStr1, newStr1)

                'subject = "Intimation"
                'body = "<b>DO NOT REPLY TO THIS E-MAIL.USE THE E-MAIL OF MARRIAGE REGISTRAR MENTIONED BELOW</b><br/>" &
                '                "<br/>DEAR MADAM/SIR <br/>" &
                '                "<b>SUBJECT:</b> INTIMATION" &
                '                "<br/>THE CERTIFIED COPY OF YOUR MARRIAGE CERTIFICATE (CERTIFICATE NO.: " & certificate_no & ") IS APPLIED BY " & r_name & ", EMAIL ID: " & r_email & ", MOBILE NO.: " & r_mob & "." &
                '                "<br/>YOU MAY COMMENT BY CLICK <a href='" & Common.baseUrl() & "MARREG_CertifiedCopy/MARREG_CertifiedCopyComment.aspx?" & comment_link & "'>HERE</a>, BY NEXT 48 HOURS (IF ANY)." &
                '                "<br/><br/>WITH REGARDS <br/>REGISTRAR GENERAL OF MARRIAGES <br/>LAW DEPARTMENT <br/>GOVERNMENT OF WEST BENGAL <br/> rgm-wb@nic.in"
                If email_ids.Count > 0 Then
                    Dim email_stat = Send_Email(email_ids, subject1, body1)
                End If
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
#End Region
#Region "Certified Copy Request Comment"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub CertifiedCopyCommentInit()
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
            Dim reqid As String = If(Not (Request("reqid") Is Nothing Or Request("reqid") = ""), DirectCast(Request("reqid"), String).Trim(), "0")
            Dim name As String = If(Not Request("name") Is Nothing And Request("name") <> "", DirectCast(Request("name"), String).Trim(), "")
            Dim dob As String = If(Not Request("dob") Is Nothing And Request("dob") <> "", DirectCast(Request("dob"), String).Trim(), "")
            Dim mob_no As String = If(Not Request("mob_no") Is Nothing And Request("mob_no") <> "", DirectCast(Request("mob_no"), String).Trim(), "")
            Dim email As String = If(Not Request("email") Is Nothing And Request("email") <> "", DirectCast(Request("email"), String).Trim(), "")
            Dim comment As String = If(Not Request("comment") Is Nothing And Request("comment") <> "", HttpUtility.HtmlEncode(DirectCast(Request("comment"), String).Trim()), "")
            Dim requestId As String = If(Not Request("requestId") Is Nothing And Request("requestId") <> "", DirectCast(Request("requestId"), String).Trim(), "")
            Dim rId = Convert.ToInt64(Session("lngReqId"))
            If (rId < 1 Or rId <> Convert.ToInt64(reqid)) Then
                response("error") = "Please initiate the process again."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                Exit Sub
            End If
            If name = "" Or name Is Nothing Then
                response("error") = "Please Enter Your Full Name."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If dob = "" Or dob Is Nothing Then
                response("error") = "Please Enter Your Date of Birth."
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
            If comment = "" Or comment Is Nothing Then
                response("error") = "Please Enter Your Proper Comment."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            Dim obj As New WebService_BL
            email = a.sqlInj_in(email)
            Dim ret = obj.Validate_CertifiedCopy_Objector(name, dob, email, rId)
            If (ret = 1) Then
                Dim otp As String = ""
                changeOTP(otp)
                Session("comment_otp") = otp
                Dim params As New Dictionary(Of String, String)
                params("reqid") = reqid
                params("name") = name
                params("dob") = dob
                params("mob_no") = mob_no
                params("email") = email
                params("comment") = HttpUtility.HtmlDecode(comment)
                params("requestId") = requestId

                Dim email_ids As New List(Of String)
                'email = "utsab.87@gmail.com"
                email_ids.Add(a.sqlInj_out(email))

                'Dim sql = "select * from MARREG_Email_Format where lower(email_type)=lower('OTP for Comment')"
                'Dim d = a.selectdata(sql)
                Dim obj1 As New WS_MasterData_BL()
                Dim ds = obj1.FetchEmailFormat(OTP_for_Comment)
                Dim d = ds.Tables(0)
                If (d.Rows.Count = 0) Then
                    json("error") = "Unable to send OTP. Please contact Administrator."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                Dim subject = d.Rows(0)("email_subject")
                Dim body = d.Rows(0)("email_body")
                Dim oldStr() As String = {"{request_id}", "{otp}"}
                Dim newStr() As String = {requestId, otp}
                Common.ReplaceEmailBody(body, oldStr, newStr)

                'Dim subject = "OTP for Comment"
                'Dim body = "<b>DO NOT REPLY TO THIS E-MAIL.USE THE E-MAIL OF MARRIAGE REGISTRAR MENTIONED BELOW </b><br/>" &
                '                "<br/>DEAR MADAM/SIR <br/>" &
                '                "<b>SUBJECT:</b> OTP for Comment" &
                '                "<br/>OTP TO VERIFY EMAIL FOR SUBMISSION OF COMMENT FOR CERTIFIED COPY OF MARRIAGE CERTIFICATE (REQUEST ID: " & requestId & ") IS " & otp & "." &
                '                "<br/><br/>WITH REGARDS <br/>REGISTRAR GENERAL OF MARRIAGES <br/>LAW DEPARTMENT <br/>GOVERNMENT OF WEST BENGAL <br/> rgm-wb@nic.in"
                Dim email_stat = Send_Email(email_ids, subject, body)
                If (email_stat = "0") Then
                    json("success") = "OTP has been sent successfully to your Email. Please verify the OTP."
                ElseIf (email_stat = "1") Then
                    json("error") = "Error occured in sending OTP. Please try again."
                Else
                    json("error") = "Unknown error occured"
                End If
                json("params") = enc.encrypt(javaScriptSerializer.Serialize(params))
                Session("params") = json("params")
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
            ElseIf (ret = -1) Then
                response("error") = "You Have Already Submitted Your Comment."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
            Else
                response("error") = "Please Enter Your Required Details Properly."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
            End If
            'Dim rets = obj.CertifiedCopyObjectionInit(name, mob_no, email, objection, rId)

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
    Public Sub CertifiedCopyCommentSubmit()
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
            Dim reqid As String = If(Not (Request("reqid") Is Nothing Or Request("reqid") = ""), DirectCast(Request("reqid"), String).Trim(), "0")
            Dim otp As String = If(Not (Request("otp") Is Nothing Or Request("otp") = ""), DirectCast(Request("otp"), String).Trim(), "")
            Dim params As String = If(Not Request("params") Is Nothing And Request("params") <> "", DirectCast(Request("params"), String).Trim(), "")
            Dim rId = Convert.ToInt64(Session("lngReqId"))
            Dim sparams = Convert.ToString(Session("params"))
            Dim sotp = Convert.ToString(Session("comment_otp"))
            If (rId < 1 Or rId <> Convert.ToInt64(reqid)) Then
                response("error") = "Please initiate the process again."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                Exit Sub
            End If
            If otp = "" Or otp Is Nothing Then
                response("error") = "Please Enter OTP Received Through Email."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If params = "" Or params Is Nothing Or (params <> sparams) Then
                response("error") = "Data has been Tampered. Please initiate the process again."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If otp <> sotp Then
                response("error") = "Invalid OTP entered. Please try again."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            Dim pparams = javaScriptSerializer.DeserializeObject(enc.Decrypt(params))
            Dim obj As New WebService_BL
            Dim rets = obj.CertifiedCopyCommentSubmit(pparams("name"), pparams("dob"), pparams("mob_no"), pparams("email"), pparams("comment"), Convert.ToInt64(pparams("reqid")))
            If rets.Length >= 13 Then
                json("success") = "Your comment submitted successfully."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
                Session.Remove("lngReqId")
                Session.Remove("params")
                Session.Remove("comment_otp")
            ElseIf rets = "-1" Then     'Duplicate value
                json("error") = "You have already submitted your comment."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
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
#End Region
#Region "Certified Copy Request Hearing"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub CertifiedCopyRequestCallHearing()
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

            'Dim status_array = {"02", "06", "09", "15"}
            'If Not status_array.Contains(status) Then
            '    response("error") = "Invalid status"
            '    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
            '    Exit Sub
            'End If
            'If status = "09" Then       'Approve status
            '    Dim act_array = {"01", "02", "03"}
            '    If Not act_array.Contains(act_cd) Then
            '        response("error") = "Invalid Act"
            '        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
            '        Exit Sub
            '    End If
            'End If
            Dim obj As New WebService_BL
            Dim rets = obj.CertifiedCopyRequestHearing(id)
            If Convert.ToInt64(rets) > 0 Then
                'Send Email Start
                Dim email_stat = GenerateAndSendEmail_Hearing(id)
                If (email_stat = "0") Then
                    json("success") = "Process done successfully and Email sending success."
                ElseIf (email_stat = "1") Then
                    json("success") = "Process done successfully and Email sending error."
                Else
                    json("success") = "Process done successfully."
                End If
                'Send Email End
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
    Private Function GenerateAndSendEmail_Hearing(id As Int64) As String
        Dim retVal As String = ""
        Dim r_email As String
        Dim r_name As String
        Dim request_id As String
        Dim com_name As New List(Of String)
        Dim com_email As New List(Of String)
        Dim email_ids As New List(Of String)
        Dim obj As New WebService_BL
        Try
            Dim requests = obj.FetchDetailForCertifiedCopyHearing(id)
            request_id = Convert.ToString(requests.Tables(0).Rows(0)("request_id"))
            r_name = Convert.ToString(requests.Tables(0).Rows(0)("r_name")).ToUpper()
            r_email = Convert.ToString(requests.Tables(0).Rows(0)("r_email"))
            r_email = a.sqlInj_out(r_email)
            'r_email = "destiny.utsab@gmail.com"
            email_ids.Add(r_email)

            Dim req_tab = requests.Tables(0)
            Dim coms = ""
            For i As Integer = 0 To req_tab.Rows.Count - 1
                coms += If(coms.Length > 0, ", <br/>", "")
                coms += Convert.ToString(req_tab.Rows(i)("full_name")).ToUpper() & " (" & Convert.ToString(req_tab.Rows(i)("ref_id")) & ")"
                'com_name.Add(req_tab.Rows(i)("full_name"))
                Dim eml = a.sqlInj_out(req_tab.Rows(i)("email"))
                'eml = "utsab.87@gmail.com"
                'eml = "utsab87@gmail.com"
                email_ids.Add(eml)
            Next

            'Dim sql = "select * from MARREG_Email_Format where lower(email_type)=lower('Intimation for Hearing')"
            'Dim d = a.selectdata(sql)
            Dim obj1 As New WS_MasterData_BL()
            Dim ds = obj1.FetchEmailFormat(Intimation_for_Hearing)
            Dim d = ds.Tables(0)
            If (d.Rows.Count = 0) Then
                Return "-1"
            End If
            Dim subject = d.Rows(0)("email_subject")
            Dim body = d.Rows(0)("email_body")
            Dim oldStr() As String = {"{r_name}", "{request_id}", "{commenter}"}
            Dim newStr() As String = {r_name, request_id, coms}
            Common.ReplaceEmailBody(body, oldStr, newStr)

            'Dim subject = "Issuance of Certified Copy of Marriage Certificate"
            'Dim body = "<b>DO NOT REPLY TO THIS E-MAIL.USE THE E-MAIL OF MARRIAGE REGISTRAR MENTIONED BELOW </b><br/>" &
            '                "<br/>DEAR MADAM/SIR <br/>" &
            '                "<br/><b>SUBJECT:</b> Issuance of Certified Copy of Marriage Certificate" &
            '                "<br/><b>REFERENCE:</b> APPLICATION OF CERTIFIED COPY OF MARRIAGE CERTIFICATE BY <i>" & r_name & " (" & request_id & ")</i>" &
            '                "<br/><br/>YOU ARE REQUESTED TO APPEAR PERSONALLY OR THROUGH AUTHORISED REPRESENTATIVE BY NEXT 7 WORKING DAYS WITHIN 11AM TO 2PM " &
            '                "AT <br/><b>THE CHAMBER OF THE SPECIAL OFFICER (OFFICE OF THE REGISTRAR GENERAL OF MARRIAGES, WEST BENGAL, " &
            '                "<br/>K.I.T. BUILDING (4TH FLOOR), P-16, INDIA EXCHANGE PLACE EXTENSION, KOLKATA-700073, WEST BENGAL, INDIA).</b>" &
            '                "<br/>YOU'LL BE HEARD RAGARDING THE MATTER." &
            '                "<br/>YOUR REFERENCE ID NO FOR THIS HEARING IS AS FOLLOWS:" &
            '                "<br/><i>" & coms & "</i>" &
            '                "<br/><br/>WITH REGARDS <br/>REGISTRAR GENERAL OF MARRIAGES <br/>LAW DEPARTMENT <br/>GOVERNMENT OF WEST BENGAL <br/> rgm-wb@nic.in"

            retVal = Send_Email(email_ids, subject, body)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return retVal
    End Function
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub CertifiedCopyRequestSaveHearing()
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
            Dim hid As Int64 = If(Not (Request("hid") Is Nothing And Request("hid") = ""), Convert.ToInt64(Request("hid").Trim()), 0)
            Dim fstat As String = If(Not (Request("fstat") Is Nothing And Request("fstat") = ""), Convert.ToString(Request("fstat").Trim()), "")
            Dim h_date As String = If(Not (Request("h_date") Is Nothing And Request("h_date") = ""), Convert.ToString(Request("h_date").Trim()), "")

            Dim status_array = {"d", "w"}
            If Not status_array.Contains(fstat) Then
                response("error") = "Invalid status"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                Exit Sub
            End If

            Dim obj As New WebService_BL
            Dim rets = obj.CertifiedCopyRequestSaveHearing(id, hid, fstat, h_date)
            If rets = 1 Then
                json("success") = "Hearing details saved successfully. Please process the request further."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
            ElseIf rets = "-1" Then     'value not found
                json("error") = "Hearing details could not be saved."
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
#End Region
#Region "Certified Copy Status Change OTP"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub CertifiedCopyRequestApproveSendOTP()
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

            Dim status_array = {"02", "06", "09", "15"}
            If Not status_array.Contains(status) Then
                response("error") = "Invalid status"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                Exit Sub
            End If

            'Send Email Start
            Dim email_stat = GenerateAndSendEmail_CertifiedApprove(id, reqid, status)
            If (email_stat = "0") Then      'sent
                json("success") = "0"
            ElseIf (email_stat = "1") Then  'not send
                json("success") = "1"
            Else
                json("success") = "-1"
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
    Private Function GenerateAndSendEmail_CertifiedApprove(id As Int64, reqid As String, status As String) As String
        Dim retVal As String = ""
        Dim r_email As String

        Dim email_ids As New List(Of String)
        Dim obj As New WebService_BL
        Try
            '-------added on 04-09-2023------------------
            Dim user_info As New Marreg_Loginfo
            user_info = DirectCast(Session.Item("Marreg_user_info"), Marreg_Loginfo)
            mo_auto_cd = user_info.user_nm
            r_email = a.sqlInj_out(user_info.user_nm)
            '--------------------------------------------
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
            Dim newStr() As String = {status_text, reqid, "CERTIFIED COPY OF MARRIAGE CERTIFICATE", otp}
            Common.ReplaceEmailBody(body, oldStr, newStr)

            'Dim subject = "OTP"
            'Dim body = "<b>DO NOT REPLY TO THIS E-MAIL.USE THE E-MAIL OF MARRIAGE REGISTRAR MENTIONED BELOW </b><br/>" & _
            '                "<br/>DEAR MADAM/SIR <br/>" & _
            '                "<br/><b>SUBJECT:</b> OTP" & _
            '                "<br/><br/>OTP TO " & status_text & "  REQUEST ID " & reqid & " FOR CERTIFIED COPY OF MARRIAGE CERTIFICATE IS: " & otp & "." & _
            '                "<br/><br/>WITH REGARDS <br/>REGISTRAR GENERAL OF MARRIAGES <br/>LAW DEPARTMENT <br/>GOVERNMENT OF WEST BENGAL <br/> rgm-wb@nic.in"

            'retVal = Send_Email(email_ids, subject, body)
            Dim params As New Dictionary(Of String, Object)
            params = CreateAssocArray(oldStr, newStr)
            Dim asyncEml As New AsyncMailMessage_DB()
            asyncEml.AsyncMailMessage_Bytes(email_ids, subject, body, Nothing, params)
            retVal = "0"
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return retVal
    End Function
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)> _
    Public Sub CertifiedCopyRequestApproveVerifyOTP()
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
#End Region
#Region "Certified Copy Request Old Records"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub CertifiedCopyRequestInfoValidateOld()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
        Try
            Dim ajax, token As String
            Dim method As String
            Dim enum_app, AppKey As String
            ajax = HttpContext.Current.Request.Headers("X-Requested-With")
            token = HttpContext.Current.Request.Headers("X-CSRF-TOKEN")
            method = HttpContext.Current.Request.HttpMethod
            enum_app = Request("enum")
            AppKey = Request("apikey")
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
                If Not token = Session("CSRF") And AppKey Is Nothing Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            End If
            If Not token = ConfigurationManager.AppSettings("api_key").ToString() Or enum_app = "" Then
                json("success") = "0"
                json("message") = "Invalid API Key"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Forbidden, Integer) '403
                Exit Sub
            End If
            Dim groom_name As String = If(Not Request("groom_name") Is Nothing, DirectCast(Request("groom_name"), String).Trim(), "")
            Dim bride_name As String = If(Not Request("bride_name") Is Nothing, DirectCast(Request("bride_name"), String).Trim(), "")
            Dim groom_father As String = If(Not Request("groom_father") Is Nothing, DirectCast(Request("groom_father"), String).Trim(), "")
            Dim bride_father As String = If(Not Request("bride_father") Is Nothing, DirectCast(Request("bride_father"), String).Trim(), "")
            Dim reg_date As String = If(Not Request("reg_date") Is Nothing, DirectCast(Request("reg_date"), String).Trim(), "")
            Dim mo_name As String = If(Not Request("mo_name") Is Nothing, DirectCast(Request("mo_name"), String).Trim(), "")
            Dim ActCd As String = If(Not Request("ActCd") Is Nothing, DirectCast(Request("ActCd"), String).Trim(), "")


            If groom_name = "" Or groom_name Is Nothing Then
                response("error") = "Please enter Groom/ Husband Name."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If bride_name = "" Or bride_name Is Nothing Then
                response("error") = "Please enter Bride/ Wife Name."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If groom_father = "" Or groom_father Is Nothing Then
                response("error") = "Please enter Groom/ Husband Father Name."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If bride_father = "" Or bride_father Is Nothing Then
                response("error") = "Please enter Bride/ Wife Father Name."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If reg_date = "" Or reg_date Is Nothing Then
                response("error") = "Please enter Date of Registration."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If mo_name = "" Or mo_name Is Nothing Then
                response("error") = "Please enter MO Name."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If ActCd = "" Or ActCd Is Nothing Or ActCd = "0" Then
                response("error") = "Please select Act/ Section under which the Marriage was Solemnized/ Registered."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If

            Dim obj As New WebService_BL
            Dim rets = obj.CertifiedCopyRequestInfoValidateOld(groom_name, bride_name, groom_father, bride_father, reg_date, mo_name, ActCd)
            'Dim rets = "1"
            If rets = "-2" Then   'Transaction rollback
                json("error") = "Internal error occured. Please try again later."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
            Else
                json("success") = "Ok"
                json("enum") = "app"
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

    '<WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    'Public Sub SaveGRIPSPaymentData()
    '    Dim Request = HttpContext.Current.Request.Form
    '    Dim response As New Dictionary(Of String, Object)()
    '    Dim serializer As New JavaScriptSerializer()
    '    Try
    '        Dim ajax = HttpContext.Current.Request.Headers("X-Requested-With")
    '        Dim token = HttpContext.Current.Request.Headers("X-CSRF-TOKEN")
    '        Dim method = HttpContext.Current.Request.HttpMethod
    '        Dim enum_app, AppKey As String

    '        enum_app = Request("enum")
    '        AppKey = Request("apikey")
    '        If Not (method = "POST" And ajax = "XMLHttpRequest") Then
    '            response("error") = "Method not allowed"
    '            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.MethodNotAllowed, Integer)
    '            Exit Sub
    '        End If

    '        If String.IsNullOrEmpty(token) OrElse token <> ConfigurationManager.AppSettings("api_key") Then
    '            response("error") = "Invalid request"
    '            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
    '            Exit Sub
    '        End If

    '        Dim encData = Request("encData")
    '        If String.IsNullOrEmpty(encData) Then
    '            response("error") = "Internal error occured. Please try again later."
    '            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
    '            Exit Sub
    '        End If

    '        Dim obj = serializer.DeserializeObject(encData)
    '        Dim rdata As New DataTable()
    '        SaveResponseFromGRIPS(obj, rdata)

    '        Dim jsonData = New List(Of Dictionary(Of String, Object))()
    '        For Each row As DataRow In rdata.Rows
    '            Dim rowData As New Dictionary(Of String, Object)()
    '            For Each col As DataColumn In rdata.Columns
    '                rowData(col.ColumnName) = row(col)
    '            Next
    '            jsonData.Add(rowData)
    '        Next

    '        response("success") = "true"
    '        response("data") = jsonData
    '        response("enum") = "app"
    '        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)

    '    Catch ex As Exception
    '        response("error") = ex.Message
    '        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
    '    Finally
    '        HttpContext.Current.Response.Clear()
    '        HttpContext.Current.Response.ContentType = "application/json"
    '        HttpContext.Current.Response.Write(serializer.Serialize(response))
    '    End Try
    'End Sub

    Private Sub SaveResponseFromGRIPS(obj As Object, ByRef rdata As DataTable)
        Try
            Dim objD As New Dictionary(Of String, Object)()
            objD("dpr") = Convert.ToString(obj("res")("dpr"))
            objD("src") = Convert.ToString(obj("res")("src"))
            objD("action") = Convert.ToString(obj("res")("action"))
            objD("gpr") = Convert.ToString(obj("res")("gpr"))
            objD("paymentAmt") = Convert.ToDecimal(obj("res")("paymentAmt"))
            objD("paymentStatus") = Convert.ToString(obj("res")("paymentStatus"))
            objD("paymentStatusDesc") = Convert.ToString(obj("res")("paymentStatusDesc"))

            If objD("paymentStatus") <> "NA" Then
                Dim payDtls = obj("res")("paymentDtls")(0)
                objD("grn") = Convert.ToString(payDtls("grn"))
                objD("grnTime") = Convert.ToString(payDtls("grnTime"))
                objD("grnStatus") = Convert.ToString(payDtls("grnStatus"))
                objD("grnStatusDesc") = Convert.ToString(payDtls("grnStatusDesc"))
                objD("bank") = Convert.ToString(payDtls("bank"))
                objD("paymentMode") = Convert.ToString(payDtls("paymentMode"))
                objD("brn") = Convert.ToString(payDtls("brn"))
                objD("brnTime") = Convert.ToString(payDtls("brnTime"))
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

            Dim objG As New GRIPS_BL()
            Dim ret = objG.SaveGRIPSPaymentResponse(objD)
            rdata = ret

        Catch ex As Exception
            'ShowMessage(ex.Message, "error")
        End Try
    End Sub

    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub FetchPaymentReferenceData()
        Dim response As New Dictionary(Of String, Object)()
        Dim serializer As New JavaScriptSerializer()
        Try
            Dim ajax = HttpContext.Current.Request.Headers("X-Requested-With")
            Dim token = HttpContext.Current.Request.Headers("X-CSRF-TOKEN")
            Dim method = HttpContext.Current.Request.HttpMethod

            If Not (method = "POST" And ajax = "XMLHttpRequest") Then
                response("error") = "Method not allowed"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.MethodNotAllowed, Integer)
                Exit Sub
            End If

            If String.IsNullOrEmpty(token) OrElse token <> ConfigurationManager.AppSettings("api_key") Then
                response("error") = "Invalid request"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                Exit Sub
            End If

            Dim deptPaymentRefId = HttpContext.Current.Request.Form("dept_payment_ref_id")
            If String.IsNullOrEmpty(deptPaymentRefId) Then
                response("error") = "Internal error occured. Please try again later."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If

            Dim referenceData = FetchPaymentRefData(deptPaymentRefId)
            If referenceData Is Nothing Then
                response("success") = "true"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.NotFound, Integer)
                Exit Sub
            End If


            response("success") = "true"
            response("data") = referenceData
            response("enum") = "app"

        Catch ex As Exception
            response("error") = ex.Message
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
        Finally
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "application/json"
            HttpContext.Current.Response.Write(serializer.Serialize(response))
        End Try
    End Sub

    Private Function FetchPaymentRefData(dept_payment_ref_id As String) As Object
        Try
            Dim objG As New MARREG.App_Code.GRIPS_BL()
            Dim data = objG.Fetch_MARREG_Payment_Reference(dept_payment_ref_id)
            If data.Rows.Count = 0 Then Return Nothing

            Dim reference_data_str = data.Rows(0)("reference_data").ToString()
            Dim serializer As New JavaScriptSerializer()
            Dim reference_data = serializer.DeserializeObject(reference_data_str)
            Return reference_data
        Catch ex As Exception
            Return Nothing
        End Try
    End Function


    '<WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    'Public Sub SaveGRNDetailsAPI()
    '    Dim response As New Dictionary(Of String, Object)()
    '    Dim serializer As New JavaScriptSerializer()
    '    Try
    '        Dim ajax = HttpContext.Current.Request.Headers("X-Requested-With")
    '        Dim token = HttpContext.Current.Request.Headers("X-CSRF-TOKEN")
    '        Dim method = HttpContext.Current.Request.HttpMethod

    '        If Not (method = "POST" And ajax = "XMLHttpRequest") Then
    '            response("error") = "Method not allowed"
    '            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.MethodNotAllowed, Integer)
    '            Exit Sub
    '        End If

    '        If String.IsNullOrEmpty(token) OrElse token <> ConfigurationManager.AppSettings("api_key") Then
    '            response("error") = "Invalid request"
    '            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
    '            Exit Sub
    '        End If

    '        Dim rdataJson = HttpContext.Current.Request.Form("rdata")
    '        Dim referenceDataJson = HttpContext.Current.Request.Form("reference_data")

    '        If String.IsNullOrEmpty(rdataJson) OrElse String.IsNullOrEmpty(referenceDataJson) Then
    '            response("error") = "Missing input data"
    '            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
    '            Exit Sub
    '        End If

    '        ' Deserialize inputs
    '        Dim rdataObj = serializer.DeserializeObject(rdataJson)
    '        Dim referenceData = serializer.DeserializeObject(referenceDataJson)

    '        ' Convert rdataObj to DataTable
    '        Dim rdata As New DataTable()
    '        If TypeOf rdataObj Is ArrayList Then
    '            Dim rows = CType(rdataObj, ArrayList)
    '            If rows.Count > 0 Then
    '                ' Build columns
    '                For Each key As String In CType(rows(0), Dictionary(Of String, Object)).Keys
    '                    rdata.Columns.Add(key)
    '                Next
    '                ' Fill rows
    '                For Each row As Dictionary(Of String, Object) In rows
    '                    Dim dr = rdata.NewRow()
    '                    For Each key As String In row.Keys
    '                        dr(key) = row(key)
    '                    Next
    '                    rdata.Rows.Add(dr)
    '                Next
    '            End If
    '        End If

    '        ' Call SaveGRN_Details logic
    '        Dim resultMsg As String = RunSaveGRNDetails(rdata, referenceData)
    '        response("success") = True
    '        response("message") = resultMsg

    '        ' If redirection HTML is generated, add to response
    '        If HttpContext.Current.Items("postHtml") IsNot Nothing Then
    '            response("html") = HttpContext.Current.Items("postHtml").ToString()
    '        End If

    '        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)

    '    Catch ex As Exception
    '        response("error") = ex.Message
    '        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.InternalServerError, Integer)
    '    Finally
    '        HttpContext.Current.Response.Clear()
    '        HttpContext.Current.Response.ContentType = "application/json"
    '        HttpContext.Current.Response.Write(serializer.Serialize(response))
    '    End Try
    'End Sub

    'Private Function RunSaveGRNDetails(rdata As DataTable, reference_data As Object) As String
    '    Dim message As String = ""
    '    Dim v1 As String = ""
    '    Try
    '        Dim obj As New WebService_BL
    '        Dim Payment_Reason = If(reference_data.ContainsKey("Payment_Reason"), reference_data("Payment_Reason"), "")
    '        Dim reason = If(reference_data.ContainsKey("reason"), reference_data("reason"), "")

    '        If Payment_Reason = "" Then
    '            Return "Invalid payment reference found"
    '        End If

    '        ' Case Registration of Marriage
    '        If (reference_data("PaymentRemarks") = "0001" Or reference_data("PaymentRemarks") = "0002") Then
    '            Dim objP As New MARREG_App_Payment_BL()
    '            Dim pmntInitId = Convert.ToInt64(reference_data("pmntInitId"))
    '            Dim draft_app_id = Convert.ToString(reference_data("draft_app_id"))
    '            Dim grn = Convert.ToString(rdata.Rows(0)("grn"))
    '            Dim payment_dt = Convert.ToString(rdata.Rows(0)("payment_completed"))
    '            Dim retP = objP.UpdateGRN_App_Payment_Init(pmntInitId, draft_app_id, grn, payment_dt)
    '            If (retP = "1") Then
    '                Return "Please proceed the Application further."
    '            Else
    '                Return "Please contact O/o RGM, WB."
    '            End If
    '        End If

    '        ' Default Save
    '        Dim rets = obj.Save_GRN_Details_Ag_Search(rdata.Rows(0)("grn"), Convert.ToInt64(rdata.Rows(0)("payment_amt")), Payment_Reason, reason)
    '        If rets = "-1" Then
    '            message = "GRN is already used"
    '        ElseIf rets = "-2" Then
    '            message = "Internal error occurred. Please try again later."
    '        ElseIf Convert.ToInt64(rets) >= 1 Then
    '            ' Case Searching
    '            If (reference_data("PaymentRemarks") = "0005") Then
    '                Dim html = BuildPostForm("0005", reference_data, rets, rdata)
    '                HttpContext.Current.Items("postHtml") = html
    '                message = "Redirect required"
    '            ElseIf (reference_data("PaymentRemarks") = "0003") Then
    '                Dim html = BuildPostForm("0003", reference_data, rets, rdata)
    '                HttpContext.Current.Items("postHtml") = html
    '                v1 = Convert.ToInt64(rets)
    '                message = "Redirect required"
    '            Else
    '                v1 = Convert.ToInt64(rets)
    '                message = "GRN details saved successfully"
    '            End If
    '        End If
    '    Catch ex As Exception
    '        message = ex.Message
    '    End Try
    '    Return message
    'End Function

    'Private Function BuildPostForm(paymentRemarks As String, reference_data As Object, rets As String, rdata As DataTable) As String
    '    Dim html As String = "<html><head>"
    '    html += "</head><body onload='document.forms[0].submit()'>"
    '    html += String.Format("<form name='PostForm' id='PostForm' method='POST' action='{0}' enctype='multipart/form-data'>", baseUrl() + reference_data("postUrl"))

    '    If paymentRemarks = "0005" Then
    '        html += String.Format("<input name='cat' type='hidden' value='{0}'>", reference_data("cat"))
    '        html += String.Format("<input name='period' type='hidden' value='{0}'>", reference_data("period"))
    '        html += String.Format("<input name='gid' type='hidden' value='{0}'>", rets)
    '    ElseIf paymentRemarks = "0003" Then
    '        html += String.Format("<input name='no_copy' type='hidden' value='{0}'>", reference_data("no_copy"))
    '        html += String.Format("<input name='validatedId' type='hidden' value='{0}'>", reference_data("validatedId"))
    '        If (reference_data("validatedId") = "" Or reference_data("validatedId") Is Nothing Or reference_data("validatedId") = "0") Then
    '            Dim serializer As New JavaScriptSerializer()
    '            html += String.Format("<input name='vdata' type='hidden' value='{0}'>", serializer.Serialize(reference_data("vdata")))
    '        End If
    '        html += String.Format("<input name='gid' type='hidden' value='{0}'>", rets)
    '    End If

    '    html += String.Format("<input name='dpr' type='hidden' value='{0}'>", rdata.Rows(0)("dept_payment_ref_id"))
    '    html += "</form></body></html>"
    '    Return html
    'End Function



    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub CertifiedCopyRequestEmailValidateOld()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
        Try
            Dim ajax, token As String
            Dim method As String
            Dim enum_app, AppKey As String
            ajax = HttpContext.Current.Request.Headers("X-Requested-With")
            token = HttpContext.Current.Request.Headers("X-CSRF-TOKEN")
            method = HttpContext.Current.Request.HttpMethod
            enum_app = Request("enum")
            AppKey = Request("apikey")
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
                If Not token = Session("CSRF") And AppKey Is Nothing Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            End If
            If Not token = ConfigurationManager.AppSettings("api_key").ToString() Or enum_app = "" Then
                json("success") = "0"
                json("message") = "Invalid API Key"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Forbidden, Integer) '403
                Exit Sub
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
            If ((vId < 1 Or vId <> Convert.ToInt64(validatedId)) And (enum_app Is Nothing)) Then
                response("error") = "Please initiate the process again."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                Exit Sub
            End If
            Dim obj As New WebService_BL
            Dim rets = obj.CertifiedCopyRequestSaveEmailOld(name, address, mob_no, email, validatedId)
            If rets = "" Then
                json("error") = "Please try again."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
            Else
                Dim email_ids As New List(Of String)
                email_ids.Add(a.sqlInj_out(email))

                'Dim sql = "select * from MARREG_Email_Format where lower(email_type)=lower('OTP for Request of Certified Copy of Marriage Certificate')"
                'Dim d = a.selectdata(sql)
                Dim obj1 As New WS_MasterData_BL()
                Dim ds = obj1.FetchEmailFormat(OTP_for_Request_of_Certified_Copy_of_Marriage_Certificate)
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

                'Dim subject = "OTP for Request of Certified Copy"
                'Dim body = "<b>DO NOT REPLY TO THIS E-MAIL.USE THE E-MAIL OF MARRIAGE REGISTRAR MENTIONED BELOW </b><br/>" &
                '                "<br/>DEAR MADAM/SIR <br/>" &
                '                "<b>SUBJECT:</b> OTP for Request of Certified Copy of Marriage Certificate" &
                '                "<br/>OTP TO VERIFY EMAIL FOR REQUEST OF CERTIFIED COPY OF MARRIAGE CERTIFICATE IS " & rets & "." &
                '                "<br/><br/>WITH REGARDS <br/>REGISTRAR GENERAL OF MARRIAGES <br/>LAW DEPARTMENT <br/>GOVERNMENT OF WEST BENGAL <br/> rgm-wb@nic.in"
                'Dim email_stat = Send_Email_Bytes(email_ids, subject, body)
                Dim email_stat = "0"
                If (email_stat = "0") Then
                    'json("success") = "OTP has been sent successfully to your Email. Please verify the OTP."
                    json("success") = "OTP has been sent successfully to your Email. Please verify the OTP." + rets
                    json("enum") = "app"
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
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub CertifiedCopyRequestEmailOTPValidateOld()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
        Try
            Dim ajax, token As String
            Dim method As String
            Dim enum_app, AppKey As String
            ajax = HttpContext.Current.Request.Headers("X-Requested-With")
            token = HttpContext.Current.Request.Headers("X-CSRF-TOKEN")
            method = HttpContext.Current.Request.HttpMethod
            enum_app = Request("enum")
            AppKey = Request("apikey")
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
                If Not token = Session("CSRF") And AppKey Is Nothing Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            End If
            If Not token = ConfigurationManager.AppSettings("api_key").ToString() Or enum_app = "" Then
                json("success") = "0"
                json("message") = "Invalid API Key"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Forbidden, Integer) '403
                Exit Sub
            End If
            Dim validatedId As String = If(Not (Request("validatedId") Is Nothing Or Request("validatedId") = ""), DirectCast(Request("validatedId"), String).Trim(), "0")
            Dim otp As String = If(Not Request("otp") Is Nothing, DirectCast(Request("otp"), String).Trim(), "")
            Dim vId = Convert.ToInt64(Session("validatedId"))

            If ((vId < 1 Or vId <> Convert.ToInt64(validatedId)) And (enum_app Is Nothing)) Then
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
            Dim rets = obj.CertifiedCopyRequestEmailOTPValidateOld(validatedId, otp)
            If rets = 1 Then
                json("success") = "OTP Verified."
                json("enum") = "app"
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
    Public Sub SaveGRIPSPaymentRequestAPI()
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
        Try
            Dim request = HttpContext.Current.Request.Form
            Dim ajax = HttpContext.Current.Request.Headers("X-Requested-With")
            Dim token = HttpContext.Current.Request.Headers("X-CSRF-TOKEN")
            Dim method = HttpContext.Current.Request.HttpMethod
            Dim enum_app = request("enum")
            Dim AppKey = request("apikey")

            If Not (method = "POST" And ajax = "XMLHttpRequest") Then
                response("error") = "Method not allowed"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.MethodNotAllowed, Integer) '405
                Exit Sub
            End If

            If token = "" Then
                If Session.Item("Marreg_user_info") Is Nothing Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer) '401
                    Exit Sub
                End If
            Else
                If Not token = Session("CSRF") And AppKey Is Nothing Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer) '401
                    Exit Sub
                End If
            End If

            If Not token = ConfigurationManager.AppSettings("api_key").ToString() Or enum_app = "" Then
                json("success") = "0"
                json("message") = "Invalid API Key"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Forbidden, Integer) '403
                Exit Sub
            End If

            ' Mandatory fields from request
            Dim depName = request("depName")
            Dim depMob = Convert.ToDouble(request("depMob"))
            Dim depEmail = request("depEmail") '            a.sqlInj_in(request("depEmail"))
            Dim depAddress = request("depAddress")
            Dim onBehalfOf = request("onBehalfOf")
            Dim remarks = request("remarks")
            Dim refNo = request("refNo")
            Dim totalAmt = Convert.ToDecimal(request("totalAmt"))
            'Dim headOfAc = request("hoa")

            Dim src = ConfigurationManager.AppSettings("gi_pmt_src").ToString()
            Dim respURL = baseUrl() + ConfigurationManager.AppSettings("gi_resp_url").ToString()
            Dim action = ConfigurationManager.AppSettings("gi_pmt_action").ToString()
            Dim deptCode = ConfigurationManager.AppSettings("gi_dept_code").ToString()
            Dim svcCode = "3" + Mid(refNo, Len(refNo) - 1, 2)
            Dim depType = "Citizen"
            Dim periodFrom = Date.Now().ToString("dd/MM/yyyy")
            Dim periodTo = Date.Now().ToString("dd/MM/yyyy")
            Dim ip = Common.getIPAddress()
            Dim sysTimeStamp = DateTime.Now().ToString("dd/MM/yyyy HH:mm:ss")

            Dim obj As New GRIPS_BL
            Dim ret = obj.SaveGRIPSPaymentRequest(src, respURL, totalAmt, depMob, action, deptCode, svcCode, depName, depAddress, depEmail, onBehalfOf, depType, periodFrom, periodTo, remarks, refNo, ip, sysTimeStamp)

            Dim dpr = ret("dept_payment_ref_id")
            Dim drn = ret("dept_ref_no")
            Dim identificationNo = ret("identification_no")

            json("success") = "Ok"
            json("enum") = "app"
            json("dept_payment_ref_id") = dpr
            json("dept_ref_no") = drn
            json("identification_no") = identificationNo

            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer) '200

        Catch ex As Exception
            response("exception") = True
            response("exception_message") = ex.Message
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer) '400
        Finally
            response = If(response.Count < 1, json, response)
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "application/json"
            HttpContext.Current.Response.Write(javaScriptSerializer.Serialize(response))
        End Try
    End Sub


    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub savePaymentReferenceAPI()
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()

        Try
            Dim request = HttpContext.Current.Request.Form
            Dim ajax = HttpContext.Current.Request.Headers("X-Requested-With")
            Dim token = HttpContext.Current.Request.Headers("X-CSRF-TOKEN")
            Dim method = HttpContext.Current.Request.HttpMethod
            Dim enum_app = request("enum")
            Dim AppKey = request("apikey")

            If Not (method = "POST" And ajax = "XMLHttpRequest") Then
                response("error") = "Method not allowed"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.MethodNotAllowed, Integer) '405
                Exit Sub
            End If

            If token = "" Then
                If Session.Item("Marreg_user_info") Is Nothing Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer) '401
                    Exit Sub
                End If
            Else
                If Not token = Session("CSRF") And AppKey Is Nothing Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer) '401
                    Exit Sub
                End If
            End If

            If Not token = ConfigurationManager.AppSettings("api_key").ToString() Or enum_app = "" Then
                json("success") = "0"
                json("message") = "Invalid API Key"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Forbidden, Integer) '403
                Exit Sub
            End If

            ' Mandatory fields from request
            Dim dpr = request("dept_payment_ref_id")
            Dim reference_data = request("reference_data")

            Dim obj As New GRIPS_BL
            Dim ret1 = obj.Save_MARREG_Payment_Reference(dpr, reference_data)

            If ret1 = "-2" Then
                json("success") = "0"
                json("message") = "Please try again."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.InternalServerError, Integer) '500 
                Exit Sub
            ElseIf ret1 <> "1" Then
                json("success") = "0"
                json("message") = ret1
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.InternalServerError, Integer) '500
                Exit Sub
            End If
            json("success") = "Ok"
            json("enum") = "app"
            json("message") = "Payment reference saved successfully."
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer) '200

        Catch ex As Exception
            response("exception") = True
            response("exception_message") = ex.Message
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer) '400
        Finally
            response = If(response.Count < 1, json, response)
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "application/json"
            HttpContext.Current.Response.Write(javaScriptSerializer.Serialize(response))
        End Try
    End Sub

    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub SaveGRIPSPaymentDetailsData()
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim serializer As New JavaScriptSerializer()
        Dim javaScriptSerializer As New JavaScriptSerializer()
        Dim requestId As String

        Try
            Dim request = HttpContext.Current.Request.Form
            Dim ajax = HttpContext.Current.Request.Headers("X-Requested-With")
            Dim token = HttpContext.Current.Request.Headers("X-CSRF-TOKEN")
            Dim method = HttpContext.Current.Request.HttpMethod
            Dim enum_app = request("enum")
            Dim AppKey = request("apikey")

            ' Validate method
            If Not (method = "POST" And ajax = "XMLHttpRequest" And enum_app = "app") Then
                response("error") = "Method not allowed"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.MethodNotAllowed, Integer) '405 DirectCast(System.Net.HttpStatusCode.Forbidden, Integer)
                Exit Sub
            End If

            ' Validate CSRF and Session
            If String.IsNullOrEmpty(token) Then
                If Session.Item("Marreg_user_info") Is Nothing Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer) '401
                    Exit Sub
                End If
            Else
                If Not token = Session("CSRF") AndAlso AppKey Is Nothing Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer) '401
                    Exit Sub
                End If
            End If

            ' API Key check
            If Not token = ConfigurationManager.AppSettings("api_key").ToString() OrElse String.IsNullOrEmpty(enum_app) Then
                json("success") = "0"
                json("message") = "Invalid API Key"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Forbidden, Integer) '403
                Exit Sub
            End If

            ' Get encData
            Dim encData = request("encData")
            If String.IsNullOrEmpty(encData) Then
                response("error") = "Missing encData"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer) '400
                Exit Sub
            End If

            ' Deserialize and save payment response
            Dim obj = serializer.DeserializeObject(encData)
            Dim rdata As New DataTable()
            SaveResponseFrom_GRIPS(obj, rdata)

            If rdata.Rows.Count = 0 Then
                json("success") = "0"
                json("enum") = "app"
                json("requestId") = requestId
                json("message") = "No response saved"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer) '400
                Exit Sub
            End If

            ' Extract fields
            Dim dpr = rdata.Rows(0)("dept_payment_ref_id").ToString()
            Dim status = rdata.Rows(0)("payment_status").ToString()
            Dim statusDesc = rdata.Rows(0)("payment_status_desc").ToString()
            Dim emailSent As Boolean = False
            Dim emailAckSent As Boolean = False
            Dim grnSaveResult As String = ""

            Dim depoName As String = rdata.Rows(0)("depositor_name").ToString()
            Dim depAddr As String = rdata.Rows(0)("depositor_address").ToString()
            Dim grn_id As String = rdata.Rows(0)("grn").ToString()
            Dim grnTime As String = rdata.Rows(0)("grn_time").ToString()
            Dim grnStatus As String = rdata.Rows(0)("grn_status_desc").ToString()

            Dim validatedId As String = ""
            Dim gId As Int64
            Dim noOfCopy As String = ""

            ' On success: Save GRN + Send Email
            If status = "S" Then
                Dim reference_data = Common.FetchPaymentRefData(dpr)
                grnSaveResult = SaveGRN_Details(rdata, reference_data)
                validatedId = reference_data("validatedId").ToString()
                gId = Convert.ToInt64(grnSaveResult)
                noOfCopy = reference_data("no_copy").ToString()

                Try
                    ' Fetch template from WS_MasterData_BL
                    Dim obj1 As New WS_MasterData_BL()
                    Dim ds As DataSet = obj1.FetchEmailFormat("Intimation_for_Payment_Complete")
                    Dim d As DataTable = ds.Tables(0)

                    If d.Rows.Count > 0 Then
                        Dim subject As String = d.Rows(0)("email_subject").ToString()
                        Dim body As String = d.Rows(0)("email_body").ToString()

                        ' Replace placeholders
                        Dim depName As String = reference_data("depName").ToString()
                        Dim remarks As String = reference_data("remarks").ToString()
                        Dim totalAmt As String = reference_data("totalAmt").ToString()
                        Dim depEmail As String = reference_data("depEmail").ToString()
                        Dim grn As String = rdata.Rows(0)("grn").ToString()
                        Dim paymentDate As String = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")

                        Dim oldStr() As String = {"{depName}", "{remarks}", "{totalAmt}", "{dpr}", "{paymentStatus}", "{grn}", "{paymentDate}"}
                        Dim newStr() As String = {depName.ToUpper(), remarks.ToUpper(), "Rs. " & totalAmt, dpr, status.ToUpper(), grn, paymentDate}

                        Dim params As Dictionary(Of String, Object) = CreateAssocArray(oldStr, newStr)
                        Common.ReplaceEmailBody(body, oldStr, newStr)

                        ' Recipients
                        Dim r_email_ids As New List(Of String) From {depEmail}

                        ' Async send
                        Dim asyncEml As New AsyncMailMessage_DB()
                        asyncEml.AsyncMailMessage_Bytes(r_email_ids, subject, body, Nothing, params)

                        emailSent = True
                    End If
                Catch exMail As Exception
                    emailSent = False
                End Try
            End If

            Dim vId = Convert.ToInt64(validatedId)

            If (vId < 1 Or vId <> Convert.ToInt64(validatedId)) Then
                response("error") = "Please initiate the process again."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                Exit Sub
            End If


            If (gId < 1 Or gId <> Convert.ToInt64(gId)) Then
                response("error") = "Please initiate the process again."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                Exit Sub
            End If

            Dim no_copy = Convert.ToInt32(noOfCopy)
            If (no_copy <> Convert.ToInt64(noOfCopy)) Then
                response("error") = "Please initiate the process again."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                Exit Sub
            End If

            Dim obj2 As New WebService_BL
            Dim rets = obj2.CertifiedCopyRequestSendOld(vId, gId, no_copy)
            Dim row = rets.Rows(0)
            If row("ret") = "-2" Then
                'rollback
                json("error") = "Unknown error occured. Please try again later."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
            ElseIf row("ret") = "-1" Then
                'duplicate
                json("error") = "Your previous request is already open."
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
                'Added on 10-11-2023 For Request from BSK
                json("req_id") = req_id
                json("req_time") = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                'End Added on 10-11-2023 For Request from BSK
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
                'Sending Acknowledgement to the requestee
                Dim l1 As New Dictionary(Of String, Object)
                l1("name") = r_name
                l1("address") = r_address
                l1("email") = r_email
                l1("req_dt") = DateTime.Today.ToString("dd/MM/yyyy")
                l1("req_id") = req_id
                l1("serve_dt") = DateTime.Today.AddDays(90).ToString("dd/MM/yyyy")
                l1("old") = "Y"
                Dim data = JavaScriptSerializer.Serialize(l1)
                Dim encData_ = enc.encrypt(data)
                Dim someUrl = baseUrl() + "MARREG_CertifiedCopy/Acknowledgement.ashx?encData=" + encData_
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
                Dim params As New Dictionary(Of String, Object)
                params = CreateAssocArray(oldStr, newStr)
                Common.ReplaceEmailBody(body, oldStr, newStr)

                'Dim email_Body = "<b>DO NOT REPLY TO THIS E-MAIL.USE THE E-MAIL OF MARRIAGE REGISTRAR MENTIONED BELOW </b><br/>" &
                '                "<br/>Dear Madam/ Sir <br/>" &
                '                "<b>SUBJECT:</b> Acknowledgement" &
                '                "<br>" + json("success") + "<br><br>With Regards <br/>REGISTRAR GENERAL OF MARRIAGES <br/>LAW DEPARTMENT <br/>GOVERNMENT OF WEST BENGAL <br/> rgm-wb@nic.in"
                Send_Email_Bytes(r_email_ids, subject, body, ack_bytes)
                emailAckSent = True
                requestId = req_id
            End If


            ' JSON response
            json("success") = "true"
            json("enum") = "app"
            json("requestId") = requestId
            json("dept_payment_ref_id") = dpr
            json("payment_status") = status
            json("payment_status_desc") = statusDesc
            json("message") = "Payment " & statusDesc & ". Department Payment Reference ID: " & dpr & "; Request ID: " & requestId
            json("email_sent") = emailSent
            json("emailAck_sent") = emailAckSent
            json("grn_save") = grnSaveResult
            json("depName") = depoName
            json("depAddr") = depAddr
            json("grn") = grn_id
            json("grnTime") = grnTime

            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer) '200

        Catch ex As Exception
            response("exception") = True
            response("requestId") = requestId
            response("exception_message") = ex.Message
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer) '400
        Finally
            response = If(response.Count < 1, json, response)
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "application/json"
            HttpContext.Current.Response.Write(serializer.Serialize(response))
        End Try
    End Sub

    ' ------------------------
    ' Helper Functions
    ' ------------------------
    Private Sub SaveResponseFrom_GRIPS(obj As Object, ByRef rdata As DataTable)
        Try
            Dim objD As New Dictionary(Of String, Object)()
            objD("dpr") = Convert.ToString(obj("res")("dpr"))
            objD("src") = Convert.ToString(obj("res")("src"))
            objD("action") = Convert.ToString(obj("res")("action"))
            objD("gpr") = Convert.ToString(obj("res")("gpr"))
            objD("paymentAmt") = Convert.ToDecimal(obj("res")("paymentAmt"))
            objD("paymentStatus") = Convert.ToString(obj("res")("paymentStatus"))
            objD("paymentStatusDesc") = Convert.ToString(obj("res")("paymentStatusDesc"))

            If objD("paymentStatus") <> "NA" Then
                Dim payDtls = obj("res")("paymentDtls")(0)
                objD("grn") = Convert.ToString(payDtls("grn"))
                objD("grnTime") = Convert.ToString(payDtls("grnTime"))
                objD("grnStatus") = Convert.ToString(payDtls("grnStatus"))
                objD("grnStatusDesc") = Convert.ToString(payDtls("grnStatusDesc"))
                objD("bank") = Convert.ToString(payDtls("bank"))
                objD("paymentMode") = Convert.ToString(payDtls("paymentMode"))
                objD("brn") = Convert.ToString(payDtls("brn"))
                objD("brnTime") = Convert.ToString(payDtls("brnTime"))
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

            Dim objG As New GRIPS_BL()
            Dim ret = objG.SaveGRIPSPaymentResponse(objD)
            rdata = ret

        Catch ex As Exception
            ' Optional: log or show error for debugging
            ' ShowMessage(ex.Message, "error")
        End Try
    End Sub


    Private Function SaveGRN_Details(rdata As DataTable, reference_data As Object) As String
        Try
            Dim obj As New WebService_BL
            Dim grn = rdata.Rows(0)("grn").ToString()
            Dim amt = Convert.ToInt64(rdata.Rows(0)("payment_amt"))
            Dim payment_reason = reference_data("Payment_Reason")
            Dim reason = If(reference_data.ContainsKey("reason"), reference_data("reason").ToString(), "")
            Return obj.Save_GRN_Details_Ag_Search(grn, amt, payment_reason, reason)
        Catch ex As Exception
            Return "-2"
        End Try
    End Function


    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub CertifiedCopyRequestSendOld()
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
            Dim nocopy As String = If(Not Request("no_copy") Is Nothing And Request("no_copy") <> "", DirectCast(Request("no_copy"), String).Trim(), "0")
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

            Dim no_copy = Convert.ToInt32(Session("no_copy"))
            If (no_copy <> Convert.ToInt64(nocopy)) Then
                response("error") = "Please initiate the process again."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                Exit Sub
            End If

            Dim obj As New WebService_BL
            Dim rets = obj.CertifiedCopyRequestSendOld(vId, grn_id, no_copy)
            Dim row = rets.Rows(0)
            If row("ret") = "-2" Then
                'rollback
                json("error") = "Unknown error occured. Please try again later."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
            ElseIf row("ret") = "-1" Then
                'duplicate
                json("error") = "Your previous request is already open."
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
                'Added on 10-11-2023 For Request from BSK
                json("req_id") = req_id
                json("req_time") = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                'End Added on 10-11-2023 For Request from BSK
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
                'Sending Acknowledgement to the requestee
                Dim l1 As New Dictionary(Of String, Object)
                l1("name") = r_name
                l1("address") = r_address
                l1("email") = r_email
                l1("req_dt") = DateTime.Today.ToString("dd/MM/yyyy")
                l1("req_id") = req_id
                l1("serve_dt") = DateTime.Today.AddDays(90).ToString("dd/MM/yyyy")
                l1("old") = "Y"
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
                Dim params As New Dictionary(Of String, Object)
                params = CreateAssocArray(oldStr, newStr)
                Common.ReplaceEmailBody(body, oldStr, newStr)

                'Dim email_Body = "<b>DO NOT REPLY TO THIS E-MAIL.USE THE E-MAIL OF MARRIAGE REGISTRAR MENTIONED BELOW </b><br/>" &
                '                "<br/>Dear Madam/ Sir <br/>" &
                '                "<b>SUBJECT:</b> Acknowledgement" &
                '                "<br>" + json("success") + "<br><br>With Regards <br/>REGISTRAR GENERAL OF MARRIAGES <br/>LAW DEPARTMENT <br/>GOVERNMENT OF WEST BENGAL <br/> rgm-wb@nic.in"
                Send_Email_Bytes(r_email_ids, subject, body, ack_bytes)
                'Dim asyncEml As New AsyncMailMessage_DB()
                'asyncEml.AsyncMailMessage_Bytes(r_email_ids, subject, body, ack_bytes, params)
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

    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub CertifiedCopyRequestDetailsOld()
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
            'Added on 02-04-2024
            If (user_info.role_nm = "DASPL" Or user_info.role_nm = "SO") Then
                mo_auto_cd = "1"
            End If
            'End Added on 02-04-2024

            Dim status As String = If(Not (Request("status") Is Nothing Or Request("status") = ""), Convert.ToString(Request("status")), "")
            Dim req_to As String = If(Not (Request("req_to") Is Nothing Or Request("req_to") = ""), Convert.ToString(Request("req_to")), "")
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

            myDataTable = WebService_BL.CertifiedCopyRequestDetailsOld(mo_auto_cd, req_to, status, draw, start, length, value, order_by, orderByDir)
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
    Public Sub CertifiedCopyRequestApproveOld()
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
                If file Is Nothing Or Not IsPdf(file) Then
                    response("error") = "No valid file specified. Please select a .pdf file to sign."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                ElseIf (file.ContentLength > 512 * 1024) Then       'Maximim file size should be 500 KB
                    response("error") = "Maximum 500 KB file allowed to upload."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
            End If

            Dim obj As New WebService_BL
            Dim rets = obj.Save_CertifiedCopyRequestApproveOld(id, status, otp, user_nm)
            If Convert.ToInt64(rets) = 1 Then
                Dim status_array_n = {"06", "15"}
                If status_array_n.Contains(status) Then
                    Dim ret = GenerateAndSendEmailCertOld(id, status, Nothing)
                    If (ret = "0") Then
                        json("success") = "Process done successfully and Email sending success."
                    ElseIf (ret = "1") Then
                        json("success") = "Process done successfully and Email sending error."
                    Else
                        json("success") = "Process done successfully."
                    End If
                Else
                    m.Rollback_CertifiedCopyRequestApproveOld(id)
                    json("data") = Generate_Certificate_BytesOld(id, status, file)
                End If
                ''Send Email Start
                'Dim email_stat = GenerateAndSendEmail(id, status, app_auto_cd, act_cd)
                'If (email_stat = "0") Then
                '    json("success") = "Process done successfully and Email sending success."
                'ElseIf (email_stat = "1") Then
                '    json("success") = "Process done successfully and Email sending error."
                'Else
                '    json("success") = "Process done successfully."
                'End If
                ''Send Email End

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
    Private Function GenSubjectAndBodyEmailOld(status As String, no_copy As Integer, requests As DataSet) As Dictionary(Of String, Object)
        Dim ret As New Dictionary(Of String, Object)
        Try
            Dim groom_name = Convert.ToString(requests.Tables(0).Rows(0)("groom_name"))
            Dim bride_name = Convert.ToString(requests.Tables(0).Rows(0)("bride_name"))
            Dim groom_father = Convert.ToString(requests.Tables(0).Rows(0)("groom_father"))
            Dim bride_father = Convert.ToString(requests.Tables(0).Rows(0)("bride_father"))
            Dim reg_date = Convert.ToString(requests.Tables(0).Rows(0)("reg_date"))
            Dim mo_name = Convert.ToString(requests.Tables(0).Rows(0)("mo_name"))
            Dim req_act = Convert.ToString(requests.Tables(0).Rows(0)("req_act"))
            Dim request_id = Convert.ToString(requests.Tables(0).Rows(0)("request_id"))
            Dim memo_no = Convert.ToString(requests.Tables(0).Rows(0)("memo_no"))     'ADDED ON 19-04-2024
            Dim str = "<strong>"
            str += If(groom_name <> "", "Groom Name: " + groom_name.ToUpper + "<br>", "")
            str += If(groom_father <> "", "Groom Father Name: " + groom_father.ToUpper + "<br>", "")
            str += If(bride_name <> "", "Bride Name: " + bride_name.ToUpper + "<br>", "")
            str += If(bride_father <> "", "Bride Father Name: " + bride_father.ToUpper + "<br>", "")
            str += If(reg_date <> "", "Registration Date: " + Convert.ToDateTime(reg_date).ToString("dd/MM/yyyy") + "<br>", "")
            str += If(mo_name <> "", "MO Name: " + mo_name.ToUpper + "<br>", "")
            str += If(req_act <> "", "Act/ Section under which the Marriage was Solemnized/ Registered: " + req_act.ToUpper + "<br>", "")
            str += "</strong>"
            Select Case status
                Case "06"
                    'Dim sql = "select * from MARREG_Email_Format where lower(email_type)=lower('')"
                    'Dim d = a.selectdata(sql)
                    Dim obj1 As New WS_MasterData_BL()
                    Dim ds = obj1.FetchEmailFormat(Certified_Copy_Request_Reject)
                    Dim d = ds.Tables(0)
                    ret("subject") = d.Rows(0)("email_subject")
                    ret("body") = d.Rows(0)("email_body")
                    Dim oldStr() As String = {"{request_id}", "{request_details}"}
                    Dim newStr() As String = {request_id, str}
                    Common.ReplaceEmailBody(ret("body"), oldStr, newStr)

                    'ret("subject") = "Request for Certified Copy of Marriage Certificate has been rejected"
                    'ret("body") = "<b>DO NOT REPLY TO THIS E-MAIL.USE THE E-MAIL OF MARRIAGE REGISTRAR MENTIONED BELOW </b><br/>" &
                    '            "<br/>DEAR MADAM/SIR <br/>" &
                    '            "<b>SUBJECT:</b> Request ID " & request_id & " | Request for Certified Copy of Marriage Certificate has been rejected" &
                    '            "<br/>REQUEST FOR CERTIFIED COPY OF THE MARRIAGE CERTIFICATE HAS BEEN REJECTED." &
                    '            "<br/>NO RECORD IS MATCHING WITH YOUR REQUEST:" &
                    '            "<br/>" & str &
                    '            "<br/><br/>WITH REGARDS <br/>REGISTRAR GENERAL OF MARRIAGES <br/>LAW DEPARTMENT <br/>GOVERNMENT OF WEST BENGAL <br/> rgm-wb@nic.in"
                    ret("filepath") = ""
                Case "09"
                    'Dim sql = "select * from MARREG_Email_Format where lower(email_type)=lower('Certified Copy Request Approve')"
                    'Dim d = a.selectdata(sql)
                    Dim obj1 As New WS_MasterData_BL()
                    Dim ds = obj1.FetchEmailFormat(Certified_Copy_Request_Approve_Old)
                    Dim d = ds.Tables(0)
                    ret("subject") = d.Rows(0)("email_subject")
                    ret("body") = d.Rows(0)("email_body")
                    Dim oldStr() As String = {"{request_id}", "{memo_no}", "{request_details}", "{no_of_copy}"}     'UPDATED ON 19-04-2024
                    Dim newStr() As String = {request_id, memo_no, str, no_copy}                                    'UPDATED ON 19-04-2024
                    Common.ReplaceEmailBody(ret("body"), oldStr, newStr)

                    'ret("subject") = "Certified Copy of Marriage Certificate"
                    'ret("body") = "<b>DO NOT REPLY TO THIS E-MAIL.USE THE E-MAIL OF MARRIAGE REGISTRAR MENTIONED BELOW </b><br/>" &
                    '            "<br/>DEAR MADAM/SIR <br/>" &
                    '            "<b>SUBJECT:</b> Request ID " & request_id & " | Certified Copy of Marriage Certificate" &
                    '            "<br/>REQUEST FOR CERTIFIED COPY OF THE MARRIAGE CERTIFICATE HAS BEEN ACCEPTED AND PROCESSED AS PER YOUR REQUEST:" &
                    '            "<br/>" & str &
                    '            "<br/>COPIES ARE ATTACHED WITH THIS EMAIL (" & no_copy & " COPIES). PLEASE DOWNLOAD AND OPEN BY ENTERING PASSWORD." &
                    '            "YOUR PASSWORD WILL BE THE FIRST FOUR CHARACTER (WITHOUT BLANK SPACE) OF YOUR (APPLICANT) NAME FOLLOWED BY LAST FOUR CHARACTER OF YOUR REQUEST ID." &
                    '            "FOR EXAMPLE :<br/>1) IF YOUR NAME IS <b>DIPAK SARKAR</b> AND YOUR REQUEST ID IS <b>O/1234/56789032</b> THEN PASSWORD TO OPEN THE FILE WILL BE <b>DIPA9032</b>.<br/>" &
                    '            "2) IF YOUR NAME IS <b>ANU MONDAL</b> AND YOUR REQUEST ID IS <b>O/1234/56789032</b> THEN PASSWORD TO OPEN THE FILE WILL BE <b>ANUM9032</b>.<br/>" &
                    '            "3) IF YOUR NAME IS <b>PIU</b> AND YOUR REQUEST ID IS <b>O/1234/56789032</b> THEN PASSWORD TO OPEN THE FILE WILL BE <b>PIU$9032</b> (IN THIS CASE FOURTH CHARACTER OF NAME WILL BE $).<br/>" &
                    '            "<br/><br/>WITH REGARDS <br/>REGISTRAR GENERAL OF MARRIAGES <br/>LAW DEPARTMENT <br/>GOVERNMENT OF WEST BENGAL <br/> rgm-wb@nic.in"
                    'SetReportParameters(app_auto_cd, act_cd)
                    ret("filepath") = Session("pdfnm_ccmc")
            End Select
        Catch ex As Exception
            Dim p = ex.Message
            Throw New Exception(ex.Message)
        End Try
        Return ret
    End Function
    Private Function Generate_Certificate_BytesOld(id As Int64, status As String, file As HttpPostedFile) As Dictionary(Of String, String)
        Dim ret As New Dictionary(Of String, String)
        Try
            Dim db As New MasterDB
            Dim file_pass = ""
            Dim base64String = ""
            Dim requests = FetchRequestDetailOld(id, "")
            Dim r_id = Convert.ToString(requests.Tables(0).Rows(0)("request_id"))
            r_id = r_id.Substring(r_id.Length - 4, 4)
            Dim r_n = Convert.ToString(requests.Tables(0).Rows(0)("r_name")).ToUpper()
            r_n = r_n.Replace(" ", "")
            While (r_n.Length < 4)
                r_n += "$"
            End While
            r_n = r_n.Substring(0, 4)
            file_pass = r_n + r_id
            Dim inputStream As System.IO.Stream = System.IO.Stream.Null
            file.InputStream.Position = 0
            If Not file Is Nothing Then
                Dim fext = IO.Path.GetExtension(file.FileName).ToLower()
                If fext = ".pdf" Then
                    inputStream = file.InputStream
                Else
                    ret("error") = "No valid file specified. Please select a .pdf file to upload."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Return ret
                End If
            End If
            Dim fileBytes As Byte()
            If Not inputStream.Length = 0 Then
                ReDim fileBytes(0 To inputStream.Length - 1)
            Else
                ret("error") = "No valid file specified. Please select a .pdf file to upload."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Return ret
            End If
            inputStream.Read(fileBytes, 0, fileBytes.Length)
            inputStream.Close()

            Dim byte1() As Byte = Nothing
            Dim reader = New PdfReader(fileBytes)

            Using ms As New MemoryStream()
                Dim pdfStamper = New PdfStamper(reader, ms)

                Dim encryptionParams As New PdfEncryption()
                Dim filepass = file_pass
                Dim ownerPass = System.Text.Encoding.ASCII.GetBytes(filepass)

                ' Set the encryption parameters for the PDF stamper
                pdfStamper.SetEncryption(ownerPass, ownerPass, PdfWriter.AllowPrinting Or PdfWriter.AllowCopy Or PdfWriter.AllowScreenReaders, PdfWriter.ENCRYPTION_AES_128)
                pdfStamper.FormFlattening = True
                pdfStamper.SetFullCompression()
                ' Close the PDF stamper and the PDF reader
                pdfStamper.Close()
                reader.Close()
                byte1 = ms.ToArray()

                base64String = Convert.ToBase64String(byte1, 0, byte1.Length)
            End Using

            Dim sign_date = DateTime.Now.ToString()
            Dim signer = GetSignerDetail()
            ret("base64String") = base64String
            ret("file_pass") = file_pass
            ret("signer") = signer
            ret("sign_date") = sign_date
            ret("id") = id
            ret("status") = status
            Return ret
        Catch ex As Exception
            'To do "Rollback certified copy request process"
            m.Rollback_CertifiedCopyRequestApproveOld(id)
            Dim p = ex.Message
            Throw New Exception(ex.Message)
        End Try
    End Function
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub GenerateAndSendEmailOld()
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
            Dim id As Int64 = If(Not (Request("id") Is Nothing And Request("id") = ""), Convert.ToInt64(Request("id").Trim()), 0)
            Dim status As String = If(Not Request("status") Is Nothing, DirectCast(Request("status"), String).Trim(), "")
            If (out_bytes_str = "") Then
                response("error") = "No valid file specified. Please generate the certificate again to sign."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            Dim file_byte = Convert.FromBase64String(out_bytes_str)

            Dim res = m.Save_CertificateOld(file_byte, id)
            If res = "1" Then
                Dim ret = GenerateAndSendEmailCertOld(id, status, file_byte)
                'Dim ret = 0
                If (ret = "0") Then
                    response("success") = "Process done successfully and Email sending success."
                ElseIf (ret = "1") Then
                    response("success") = "Process done successfully and Email sending error."
                Else
                    response("success") = "Process done successfully."
                End If
            Else
                'To do "Rollback certified copy request process"
                m.Rollback_CertifiedCopyRequestApprove(id)
                response("error") = "Please process the request once again."
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

            Dim status_array = {"02", "06", "09", "15"}
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
            'to do get certified copy byte
            Dim db As New WebService_DB
            Dim f = db.FecthCertifiedCopyOld(id)
            Dim bytFile As Byte() = Nothing
            Dim base64String = ""
            If (f.Rows.Count > 0) Then
                bytFile = f.Rows(0)("FileStreamCol")
            End If

            Dim email_stat = GenerateAndSendEmailCertOld(id, status, bytFile)
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

    '<WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>

#End Region
    Protected Sub changeOTP(ByRef otp As String)
        Dim secretkey() As Byte = New [Byte](15) {}
        Using rng As New RNGCryptoServiceProvider()
            rng.GetBytes(secretkey)
        End Using
        otp = a.GetTotp(secretkey)
    End Sub
End Class