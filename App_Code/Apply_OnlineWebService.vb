Imports System.Web
Imports System.Web.Script.Serialization
Imports System.Web.Script.Services
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports MARREG.App_Code
Imports MARREG.App_Code.BusinessLogicLayer
Imports System.Security.Cryptography
Imports System.IO
Imports MARREG.App_Code.DataAccessLayer
Imports System.Web.Hosting

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class Apply_OnlineWebService
    Inherits System.Web.Services.WebService
    Dim a As New Marreg_Class

#Region "User Registration"
#Region "Email Validation for Registration"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub CheckRegEmail()
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
            Dim apikey As String = If(Not Request("apikey") Is Nothing, DirectCast(Request("apikey"), String).Trim(), "")
            If Not (method = "POST" And ajax = "XMLHttpRequest") Then
                response("error") = "Method not allowed"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.MethodNotAllowed, Integer)
                Exit Sub
            End If
            If token = "" Then
                If apikey <> "" Then    'added for api testing on 03-06-2024
                    If Not (apikey = ConfigurationManager.AppSettings("api_key").ToString) Then
                        response("error") = "Invalid request"
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                        Exit Sub
                    End If
                Else                    'added for api testing on 03-06-2024
                    If Session.Item("Marreg_user_info") Is Nothing Then
                        response("error") = "Invalid request"
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                        Exit Sub
                    End If
                End If                  'added for api testing on 03-06-2024
            Else
                If Not token = Session("CSRF") Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            End If
            Dim email As String = If(Not Request("email") Is Nothing, DirectCast(Request("email"), String).Trim(), "")
            email = a.sqlInj_in(email)
            Dim obj As New MARREG_ApplyOnline_Webservice_BL
            Dim count = obj.CheckRegEmail(email)
            If count > 0 Then
                Session("email") = Nothing
                response("error") = "Email Id already registered in the portal."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            Else
                Session("email") = a.sqlInj_out(email)
                'Send Email Start
                Dim email_ids As New List(Of String)
                email_ids.Add(Convert.ToString(Session("email")))
                Dim email_stat = GenerateAndSendEmail_Registration(email_ids)
                If (email_stat = "0") Then
                    json("success") = "0"
                ElseIf (email_stat = "1") Then
                    json("success") = "1"
                Else
                    json("success") = "-1"
                End If
                'Send Email End
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
#End Region
#Region "Mobile Validation for Registration"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub CheckRegMobile()
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
            Dim mob_no As String = If(Not Request("mob_no") Is Nothing, DirectCast(Request("mob_no"), String).Trim(), "")
            Dim obj As New MARREG_ApplyOnline_Webservice_BL
            Dim count = obj.CheckRegMobile(mob_no)
            If count > 0 Then
                Session("mob_no") = Nothing
                response("error") = "Mobile No already registered in the portal."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            Else
                Session("mob_no") = mob_no
                'Send SMS Start
                Dim sms_stat = GenerateAndSendSMS_Registration(mob_no)
                If (sms_stat = "0") Then
                    json("success") = "0"
                ElseIf (sms_stat = "1") Then
                    json("success") = "1"
                Else
                    json("success") = "-1"
                End If
                'Send SMS End
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
#End Region

#Region "Login Details Validation"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub VerifyLogin()
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
            Dim reg_id As String = If(Not Request("reg_id") Is Nothing, DirectCast(Request("reg_id"), String).Trim(), "")
            Dim dob As String = If(Not Request("dob") Is Nothing, DirectCast(Request("dob"), String).Trim(), "")
            Dim type As String = If(Not Request("type") Is Nothing, DirectCast(Request("type"), String).Trim(), "")

            Dim obj As New MARREG_ApplyOnline_Webservice_BL
            Dim data = obj.VerifyLogin(reg_id, dob)
            Session("email") = Nothing
            Session("reg_id") = Nothing
            Session("dob") = Nothing
            If data.Rows.Count > 0 Then
                Dim verify = Convert.ToInt16(data.Rows(0).Item("verify"))
                Dim email = Convert.ToString(data.Rows(0).Item("email"))
                If verify = 0 Then
                    response("error") = "Invalid Registration Id entered."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                ElseIf verify = -1 Then
                    response("error") = "Unable to identify your identity."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                ElseIf verify = -2 Then
                    response("error") = "Registration Id and Date of Birth combination not matching."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                Else
                    email = a.sqlInj_out(email)
                    Session("email") = a.sqlInj_out(email)
                    Session("reg_id") = reg_id
                    Session("dob") = dob
                    'Send Email Start
                    Dim email_stat = GenerateAndSendEmail_Login(Convert.ToString(Session("email")))
                    If (email_stat = "0") Then
                        json("success") = "0"
                    ElseIf (email_stat = "1") Then
                        json("success") = "1"
                    Else
                        json("success") = "-1"
                    End If
                    'Send Email End
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
                End If
            Else
                response("error") = "Please try after some time."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
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
    Private Function Send_Email_Bytes(email_ids As List(Of String), subject As String, body As String, Optional filebytes As List(Of Byte()) = Nothing) As String
        Dim MS As String
        MS = SendMailMessage_Bytes(email_ids, subject, body, filebytes)
        'MS = eml.send_email_multiple_bytes(email_ids, subject, body, filebytes)

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
                mMailMessage.Attachments.Add(New Net.Mail.Attachment(New MemoryStream(fle.Item(x)), "acknowledgement.pdf"))
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
#End Region
#Region "Send OTP"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub SendOTP()
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
            Dim type As String = If(Not Request("type") Is Nothing, DirectCast(Request("type"), String).Trim(), "")
            If (type = "email") Then
                Dim email = Convert.ToString(Session("email"))
                'Send Email Start
                Dim email_ids As New List(Of String)
                email_ids.Add(email)
                Dim email_stat = GenerateAndSendEmail_Registration(email_ids)
                If (email_stat = "0") Then
                    json("success") = "0"
                ElseIf (email_stat = "1") Then
                    json("success") = "1"
                Else
                    json("success") = "-1"
                End If
                'Send Email End
            ElseIf (type = "mobile") Then
                Dim mob_no = Convert.ToString(Session("mob_no"))
                'Send SMS Start
                Dim sms_stat = GenerateAndSendSMS_Registration(mob_no)
                If (sms_stat = "0") Then
                    json("success") = "0"
                ElseIf (sms_stat = "1") Then
                    json("success") = "1"
                Else
                    json("success") = "-1"
                End If
                'Send SMS End
            ElseIf (type = "registered email") Then
                Dim email = Convert.ToString(Session("registered_email"))
                'Send Email Start
                Dim email_ids As New List(Of String)
                email_ids.Add(email)
                Dim email_stat = GenerateAndSendEmail_Registration(email_ids)
                If (email_stat = "0") Then
                    json("success") = "0"
                ElseIf (email_stat = "1") Then
                    json("success") = "1"
                Else
                    json("success") = "-1"
                End If
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
    'Private Function GenerateAndSendEmail_Registration(email As String) As String
    Private Function GenerateAndSendEmail_Registration(emails As List(Of String)) As String
        Dim retVal As String = ""
        Dim r_email As String

        Dim email_ids As New List(Of String)
        Dim obj As New WebService_BL
        Try

            'r_email = email
            'r_email = "destiny.utsab@gmail.com"
            email_ids = emails
            Dim otp = ""
            changeOTP(otp)
            Session("reg_otp") = otp

            'Dim sql = "select * from MARREG_Email_Format where lower(email_type)=lower('OTP for Certified Copy')"
            'Dim d = a.selectdata(sql)
            'Dim subject = d.Rows(0)("email_subject")
            'Dim body = d.Rows(0)("email_body")
            'Dim oldStr() As String = {"{status_text}", "{request_id}", "{otp}"}
            'Dim newStr() As String = {status_text, reqid, otp}
            'Common.ReplaceEmailBody(body, oldStr, newStr)

            Dim subject = "OTP"
            Dim body = "<b>DO NOT REPLY TO THIS E-MAIL.USE THE E-MAIL OF MARRIAGE REGISTRAR MENTIONED BELOW </b><br/>" &
                            "<br/>DEAR MADAM/SIR <br/>" &
                            "<br/><b>SUBJECT:</b> OTP" &
                            "<br/><br/>OTP TO VERIFY YOUR EMAIL ID FOR USER REGISTRATION IN MARREG PORTAL IS: " & otp & "." &
                            "<br/><br/>WITH REGARDS <br/>REGISTRAR GENERAL OF MARRIAGES <br/>LAW DEPARTMENT <br/>GOVERNMENT OF WEST BENGAL <br/> rgm-wb@nic.in"

            retVal = Send_Email_Bytes(email_ids, subject, body)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return retVal
    End Function
    Private Function GenerateAndSendSMS_Registration(mob_no As String) As String
        Dim retVal As String = ""
        Dim r_mob_no As String

        Dim obj As New WebService_BL
        Try

            r_mob_no = mob_no
            Dim otp = ""
            changeOTP(otp)
            Session("reg_mob_otp") = otp
            Dim body = "Use " + otp + " as OTP to verify your identity and proceed to submit the page. Do not share this OTP with anyone. O/o RGM,W.B."

            retVal = eml.send_sms_new_template(r_mob_no, body, "1007161658078438818")
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return retVal
    End Function
    Private Function GenerateAndSendEmail_Login(email As String) As String
        Dim retVal As String = ""
        Dim r_email As String

        Dim email_ids As New List(Of String)
        Dim obj As New WebService_BL
        Try

            r_email = email
            'r_email = "destiny.utsab@gmail.com"
            email_ids.Add(r_email)
            Dim otp = ""
            changeOTP(otp)
            Session("reg_email_otp") = otp

            'Dim sql = "select * from MARREG_Email_Format where lower(email_type)=lower('OTP for Certified Copy')"
            'Dim d = a.selectdata(sql)
            'Dim subject = d.Rows(0)("email_subject")
            'Dim body = d.Rows(0)("email_body")
            'Dim oldStr() As String = {"{status_text}", "{request_id}", "{otp}"}
            'Dim newStr() As String = {status_text, reqid, otp}
            'Common.ReplaceEmailBody(body, oldStr, newStr)

            Dim subject = "OTP"
            Dim body = "<b>DO NOT REPLY TO THIS E-MAIL.USE THE E-MAIL OF MARRIAGE REGISTRAR MENTIONED BELOW </b><br/>" &
                            "<br/>DEAR MADAM/SIR <br/>" &
                            "<br/><b>SUBJECT:</b> OTP" &
                            "<br/><br/>OTP TO VERIFY YOUR EMAIL ID FOR LOGIN IN MARREG PORTAL IS: " & otp & "." &
                            "<br/><br/>WITH REGARDS <br/>REGISTRAR GENERAL OF MARRIAGES <br/>LAW DEPARTMENT <br/>GOVERNMENT OF WEST BENGAL <br/> rgm-wb@nic.in"

            retVal = Send_Email_Bytes(email_ids, subject, body)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return retVal
    End Function
    Private Function GenerateAndSendEmail_apply_online(email As String) As String
        Dim retVal As String = ""
        Dim r_email As String

        Dim email_ids As New List(Of String)
        Dim obj As New WebService_BL
        Try
            r_email = email
            'r_email = "destiny.utsab@gmail.com"
            email_ids.Add(r_email)
            Dim otp = ""
            changeOTP(otp)
            Session("applyonline_email_otp") = otp

            'Dim sql = "select * from MARREG_Email_Format where lower(email_type)=lower('OTP for Certified Copy')"
            'Dim d = a.selectdata(sql)
            'Dim subject = d.Rows(0)("email_subject")
            'Dim body = d.Rows(0)("email_body")
            'Dim oldStr() As String = {"{status_text}", "{request_id}", "{otp}"}
            'Dim newStr() As String = {status_text, reqid, otp}
            'Common.ReplaceEmailBody(body, oldStr, newStr)

            Dim subject = "OTP"
            Dim body = "<b>DO NOT REPLY TO THIS E-MAIL.USE THE E-MAIL OF MARRIAGE REGISTRAR MENTIONED BELOW </b><br/>" &
                            "<br/>DEAR MADAM/SIR <br/>" &
                            "<br/><b>SUBJECT:</b> OTP" &
                            "<br/><br/>OTP TO VERIFY YOUR EMAIL ID FOR LOGIN IN MARREG PORTAL IS: " & otp & "." &
                            "<br/><br/>WITH REGARDS <br/>REGISTRAR GENERAL OF MARRIAGES <br/>LAW DEPARTMENT <br/>GOVERNMENT OF WEST BENGAL <br/> rgm-wb@nic.in"

            retVal = Send_Email_Bytes(email_ids, subject, body)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return retVal
    End Function
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub VerifyOTP()
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
            'Dim id As Int64 = If(Not (Request("id") Is Nothing And Request("id") = ""), Convert.ToInt64(Request("id").Trim()), 0)
            Dim otp As String = If(Not (Request("otp") Is Nothing Or Request("otp") = ""), DirectCast(Request("otp"), String).Trim(), "")
            Dim type As String = If(Not (Request("type") Is Nothing Or Request("type") = ""), DirectCast(Request("type"), String).Trim(), "")
            'Dim ar_id = Convert.ToInt64(Session("ar_id"))
            Dim reg_otp = ""
            If (type = "email") Then
                reg_otp = Convert.ToString(Session("reg_otp"))
            ElseIf (type = "mobile") Then
                reg_otp = Convert.ToString(Session("reg_mob_otp"))
            ElseIf (type = "registered email") Then
                reg_otp = Convert.ToString(Session("reg_email_otp"))
            ElseIf (type = "applyOnlineOTP") Then
                reg_otp = Convert.ToString(Session("applyonline_email_otp"))
            End If
            'If (ar_id < 1 Or ar_id <> Convert.ToInt64(id)) Then
            '    response("error") = "Please initiate the process again."
            '    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
            '    Exit Sub
            'End If
            If otp = "" Or otp Is Nothing Then
                response("error") = "Please Enter OTP Received Through " + type + "."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If otp <> reg_otp Then
                response("error") = "Invalid OTP entered. Please try again."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If

            response("success") = 1
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
    Public Sub SendApplyOnlineOTP()
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
            Dim groom_email As String = If(Not Request("g_email") Is Nothing, DirectCast(Request("g_email"), String).Trim(), "")
            Dim bride_email As String = If(Not Request("b_email") Is Nothing, DirectCast(Request("b_email"), String).Trim(), "")
            Dim groom_mobile As String = If(Not Request("g_mobile") Is Nothing, DirectCast(Request("g_mobile"), String).Trim(), "")
            Dim bride_mobile As String = If(Not Request("b_mobile") Is Nothing, DirectCast(Request("b_mobile"), String).Trim(), "")
            If (groom_email = "") Then
                response("error") = "Please enter Email-ID"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (bride_email = "") Then
                response("error") = "Please enter Email-ID"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (groom_mobile = "") Then
                response("error") = "Please enter  mobile no"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (bride_mobile = "") Then
                response("error") = "Please enter mobile no"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            Dim groom_email_sent = Convert.ToString(groom_email)
            Dim bride_email_sent = Convert.ToString(bride_email)
            Dim email_stat_g = GenerateAndSendEmail_apply_online(groom_email_sent)
            Dim email_stat_b = GenerateAndSendEmail_apply_online(bride_email_sent)
            If (email_stat_g = "0" And email_stat_b = "0") Then
                json("success") = "0"
            Else
                json("success") = "-1"
            End If
            Dim mob_no_g = Convert.ToString(groom_mobile)
            Dim mob_no_b = Convert.ToString(groom_mobile)
            'Send SMS Start
            'Dim sms_stat_g = GenerateAndSendSMS_Registration(mob_no_g)
            'Dim sms_stat_b = GenerateAndSendSMS_Registration(mob_no_b)
            'If (sms_stat_g = "0" And sms_stat_b = "0") Then
            '    json("success") = "0"
            'Else
            '    json("success") = "-1"
            'End If
            'Send SMS End
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
    Protected Sub changeOTP(ByRef otp As String)
        Dim secretkey() As Byte = New [Byte](15) {}
        Using rng As New RNGCryptoServiceProvider()
            rng.GetBytes(secretkey)
        End Using
        otp = a.GetTotp(secretkey)
    End Sub
#End Region
#Region "Update Initial Details Apply Online"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub Update_Initial_Details_Apply_Online()
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
            Dim regid As String = If(Not Request("regid") Is Nothing, DirectCast(Request("regid"), String).Trim(), "")
            If Not regid = Session("reg_id") Then
                response("error") = "Invalid Registration ID"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                Exit Sub
            End If
            Dim obj As New MARREG_ApplyOnline_Webservice_BL
            Dim ret = obj.Update_Initial_Details_Apply_Online(regid)
            json("data") = ret
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
#Region "Load Master Data"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub LoadDistrict()
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
            Dim stcd As String = If(Not Request("stcd") Is Nothing, DirectCast(Request("stcd"), String).Trim(), "")

            Dim obj As New WS_MasterData_BL
            Dim data = obj.LoadDistrict(stcd).Tables(0)
            If (data.Rows.Count > 0) Then
                json("success") = 1
            End If
            json("data") = DataTableToJSONWithJavaScriptSerializer(data)
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
    Public Sub LoadSubDiv()
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
            Dim distcd As String = If(Not Request("distcd") Is Nothing, DirectCast(Request("distcd"), String).Trim(), "")

            Dim obj As New WS_MasterData_BL
            Dim data = obj.LoadSubDiv(distcd).Tables(0)
            If (data.Rows.Count > 0) Then
                json("success") = 1
            End If
            json("data") = DataTableToJSONWithJavaScriptSerializer(data)
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
    Public Sub LoadPS()
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
            Dim distcd As String = If(Not Request("distcd") Is Nothing, DirectCast(Request("distcd"), String).Trim(), "")

            Dim obj As New WS_MasterData_BL
            Dim data = obj.LoadPS(distcd).Tables(0)
            If (data.Rows.Count > 0) Then
                json("success") = 1
            End If
            json("data") = DataTableToJSONWithJavaScriptSerializer(data)
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
    Public Sub LoadBlock()
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
            Dim distcd As String = If(Not Request("distcd") Is Nothing, DirectCast(Request("distcd"), String).Trim(), "")
            Dim subdivcd As String = If(Not Request("subdivcd") Is Nothing, DirectCast(Request("subdivcd"), String).Trim(), "")

            Dim obj As New WS_MasterData_BL
            Dim data = obj.LoadBLock(distcd, subdivcd).Tables(0)
            If (data.Rows.Count > 0) Then
                json("success") = 1
            End If
            json("data") = DataTableToJSONWithJavaScriptSerializer(data)
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
    Public Sub LoadmuniGP()
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
            Dim distcd As String = If(Not Request("distcd") Is Nothing, DirectCast(Request("distcd"), String).Trim(), "")
            Dim subdivcd As String = If(Not Request("subdivcd") Is Nothing, DirectCast(Request("subdivcd"), String).Trim(), "")
            Dim blockcd As String = If(Not Request("blockcd") Is Nothing, DirectCast(Request("blockcd"), String).Trim(), "")
            Dim wa As String = If(Not Request("wa") Is Nothing, DirectCast(Request("wa"), String).Trim(), "")

            Dim obj As New WS_MasterData_BL
            Dim data = obj.LoadmuniGP(distcd, subdivcd, blockcd, wa).Tables(0)
            If (data.Rows.Count > 0) Then
                json("success") = 1
            End If
            json("data") = DataTableToJSONWithJavaScriptSerializer(data)
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
    Public Sub LoadPO()
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
            Dim distcd As String = If(Not Request("distcd") Is Nothing, DirectCast(Request("distcd"), String).Trim(), "")
            Dim pincode As String = If(Not Request("pincode") Is Nothing, DirectCast(Request("pincode"), String).Trim(), "")
            Dim obj As New WS_MasterData_BL
            Dim data = obj.LoadPO(distcd, pincode).Tables(0)
            If (data.Rows.Count > 0) Then
                json("success") = 1
            End If
            json("data") = DataTableToJSONWithJavaScriptSerializer(data)
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
    Public Sub LoadDivorceeSection()
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
            Dim divoceeAct As String = If(Not Request("actCd") Is Nothing, DirectCast(Request("actCd"), String).Trim(), "")
            Dim maritalStatus As String = If(Not Request("status") Is Nothing, DirectCast(Request("status"), String).Trim(), "")
            Dim obj As New WS_MasterData_BL
            Dim data = obj.LoadDivorceeSection(divoceeAct, maritalStatus).Tables(0)
            If (data.Rows.Count > 0) Then
                json("success") = 1
            End If
            json("data") = DataTableToJSONWithJavaScriptSerializer(data)
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
    Public Sub LoadVillage()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
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

            Dim districd As String = If(Not Request("district") Is Nothing, DirectCast(Request("district"), String).Trim(), "")
            Dim subdivcd As String = If(Not Request("subdivcd") Is Nothing, DirectCast(Request("subdivcd"), String).Trim(), "")
            Dim blockcd As String = If(Not Request("blockcd") Is Nothing, DirectCast(Request("blockcd"), String).Trim(), "")
            Dim gpcd As String = If(Not Request("gpcd") Is Nothing, DirectCast(Request("gpcd"), String).Trim(), "")
            Dim prefix As String = If(Not Request("prefix") Is Nothing, DirectCast(Request("prefix"), String).Trim(), "")
            If (districd = "" Or districd = "0") Then
                response("error") = "District not selected"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (subdivcd = "" Or subdivcd = "0") Then
                response("error") = "SubDivision not selected"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (blockcd = "" Or blockcd = "0") Then
                response("error") = "Block not selected"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (gpcd = "" Or gpcd = "0") Then
                response("error") = "GP not selected"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            Dim vill As New List(Of Dictionary(Of String, String))()

            Using conn As New Data.SqlClient.SqlConnection()
                conn.ConnectionString = ConfigurationManager.ConnectionStrings("marreg").ConnectionString
                Using cmd As New Data.SqlClient.SqlCommand()
                    cmd.CommandText = "select top 1 Mouza_cd,upper(emouza_nm)emouza_nm from MARREG_Mouza where emouza_nm like @SearchText + '%' and District_Cd='" + districd + "' and sub_division_cd='" + subdivcd + "' and Block_cd='" + blockcd + "' and gp_cd='" + gpcd + "'"
                    cmd.Parameters.AddWithValue("@SearchText", prefix)
                    cmd.Connection = conn
                    conn.Open()
                    Using sdr As Data.SqlClient.SqlDataReader = cmd.ExecuteReader()
                        While sdr.Read()
                            Dim val As New Dictionary(Of String, String)
                            val("Mouza_cd") = sdr("Mouza_cd")
                            val("emouza_nm") = sdr("emouza_nm")
                            vill.Add(val)
                            val = Nothing
                        End While
                    End Using
                    conn.Close()
                End Using
            End Using
            json("vill") = vill
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
        Catch ex As Exception
            response("exception") = "true"
            response("exception_message") = ex.Message
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
        Finally
            'connection.Close()
            response = If(response.Count < 1, json, response)
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "application/json"
            HttpContext.Current.Response.Write(javaScriptSerializer.Serialize(response))
        End Try
    End Sub
#End Region
#Region "REG MO List"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub AllRegMOList()
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
            Dim draw As String = If(Not Request("draw") Is Nothing, DirectCast(Request("draw"), String), "1")
            Dim start As String = If(Not Request("start") Is Nothing, DirectCast(Request("start"), String), "0")
            Dim length As String = If(Not Request("length") Is Nothing, DirectCast(Request("length"), String), "10")
            Dim value As String = If(Not Request("search[value]") Is Nothing, DirectCast(Request("search[value]"), String).Trim(), "")
            Dim orderByDir As String = If(Request("order[0][dir]") Is Nothing, "", DirectCast(Request("order[0][dir]"), String))
            Dim order_by As String = "mo_name"

            Dim act As String = If(Not Request("act_cd") Is Nothing, DirectCast(Request("act_cd"), String).Trim(), "")
            Dim dist As String = If(Not Request("dist_cd") Is Nothing, DirectCast(Request("dist_cd"), String).Trim(), "")
            Dim ps As String = If(Not Request("ps_cd") Is Nothing, DirectCast(Request("ps_cd"), String).Trim(), "")
            Dim subdivision As String = If(Not Request("subdiv_cd") Is Nothing, DirectCast(Request("subdiv_cd"), String).Trim(), "")
            Dim block As String = If(Not Request("block_cd") Is Nothing, DirectCast(Request("block_cd"), String).Trim(), "")
            Dim munigp As String = If(Not Request("munigp_cd") Is Nothing, DirectCast(Request("munigp_cd"), String).Trim(), "")

            Dim obj As New WS_MasterData_BL
            myDataTable = obj.LoadallMO(draw, start, length, value, order_by, orderByDir, act, dist, ps, subdivision, block, munigp)

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
    Public Sub LoadRegMO_other_details()
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
            Dim mo_cd As String = If(Not Request("id") Is Nothing, DirectCast(Request("id"), String).Trim(), "591")
            Dim obj As New WS_MasterData_BL
            Dim res = obj.LoadRegMO_other_details(mo_cd)
            If (res.Rows.Count < 1) Then
                response("error") = "Data Not Found"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                Exit Sub
            End If
            Dim res_mocd = res.Rows(0)("mo_cd").ToString()
            Dim res_moimage = Convert.ToBase64String(res.Rows(0)("MO_image"))
            Dim res_workinghour = res.Rows(0)("workingdays_hour").ToString()
            'Dim res_reg_office_addr = res.Rows(0)("reg_office_address").ToString().ToUpper()
            'Dim res_reg_dist_cd = res.Rows(0)("reg_dist_cd").ToString()
            'Dim res_reg_dist_nm = res.Rows(0)("reg_dist_nm").ToString().ToUpper()
            'Dim res_reg_ps_cd = res.Rows(0)("reg_ps_cd").ToString()
            'Dim res_reg_ps_nm = res.Rows(0)("reg_ps_nm").ToString().ToUpper()
            'Dim res_reg_po_cd = res.Rows(0)("reg_po_cd").ToString()
            'Dim res_reg_po_nm = res.Rows(0)("reg_po_nm").ToString().ToUpper()
            'Dim res_reg_pin = res.Rows(0)("reg_pin_no").ToString()
            'json("data") = DataTableToJSONWithJavaScriptSerializer(res)
            Dim json1 As New Dictionary(Of String, Object)
            json1("mo_cd") = res_mocd
            json1("MO_image") = res_moimage
            json1("workingdays_hour") = res_workinghour
            'json1("reg_address") = res_reg_office_addr
            'json1("reg_dist_cd") = res_reg_dist_cd
            'json1("reg_dist_nm") = res_reg_dist_nm
            'json1("reg_ps_cd") = res_reg_ps_cd
            'json1("reg_ps_nm") = res_reg_ps_nm
            'json1("reg_po_cd") = res_reg_po_cd
            'json1("reg_po_nm") = res_reg_po_nm
            'json1("reg_pin_no") = res_reg_pin
            json("data") = json1
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
        Catch ex As Exception
            response("exception") = "true"
            response("exception_message") = ex.Message + "<br>" + "Click <a onclick='location.reload()' href='#'>here</a> to reload"
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
        Finally
            response = If(response.Count < 1, json, response)
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "application/json"
            HttpContext.Current.Response.Write(javaScriptSerializer.Serialize(response))
        End Try
    End Sub
#End Region
#Region "Save Groom Details"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub SaveGroomPersonalDetails()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
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
            Dim Act As String = If(Not (Request("act_cd") Is Nothing Or Request("act_cd") = ""), DirectCast(Request("act_cd"), String).Trim(), "")
            Dim groom_fname As String = If(Not (Request("g_first_name") Is Nothing Or Request("g_first_name") = ""), DirectCast(Request("g_first_name"), String).Trim(), "")
            Dim groom_mname As String = If(Not (Request("g_middle_name") Is Nothing Or Request("g_middle_name") = ""), DirectCast(Request("g_middle_name"), String).Trim(), "")
            Dim groom_lname As String = If(Not (Request("g_last_name") Is Nothing Or Request("g_last_name") = ""), DirectCast(Request("g_last_name"), String).Trim(), "")
            Dim groom_guardian_type As String = If(Not (Request("g_guardian_type") Is Nothing Or Request("g_guardian_type") = ""), DirectCast(Request("g_guardian_type"), String).Trim(), "")
            Dim groom_father_fname As String = If(Not (Request("gf_first_name") Is Nothing Or Request("gf_first_name") = ""), DirectCast(Request("gf_first_name"), String).Trim(), "")
            Dim groom_father_mname As String = If(Not (Request("gf_middle_name") Is Nothing Or Request("gf_middle_name") = ""), DirectCast(Request("gf_middle_name"), String).Trim(), "")
            Dim groom_father_lname As String = If(Not (Request("gf_last_name") Is Nothing Or Request("gf_last_name") = ""), DirectCast(Request("gf_last_name"), String).Trim(), "")
            Dim groom_mother_fname As String = If(Not (Request("gm_first_name") Is Nothing Or Request("gm_first_name") = ""), DirectCast(Request("gm_first_name"), String).Trim(), "")
            Dim groom_mother_mname As String = If(Not (Request("gm_middle_name") Is Nothing Or Request("gm_middle_name") = ""), DirectCast(Request("gm_middle_name"), String).Trim(), "")
            Dim gromm_mother_lname As String = If(Not (Request("gm_last_name") Is Nothing Or Request("gm_last_name") = ""), DirectCast(Request("gm_last_name"), String).Trim(), "")
            Dim groom_dob As String = If(Not (Request("g_dob") Is Nothing Or Request("g_dob") = ""), DirectCast(Request("g_dob"), String).Trim(), "")
            Dim groom_relegion As String = If(Not (Request("g_religion") Is Nothing Or Request("g_religion") = ""), DirectCast(Request("g_religion"), String).Trim(), "")
            Dim groom_relegion_other As String = If(Not (Request("g_religion_othr") Is Nothing Or Request("g_religion_othr") = ""), DirectCast(Request("g_religion_othr"), String).Trim(), "")
            Dim groom_nationality As String = If(Not (Request("g_nationality") Is Nothing Or Request("g_nationality") = ""), DirectCast(Request("g_nationality"), String).Trim(), "")
            Dim groom_mobnum_ctry As String = If(Not (Request("g_mobctry_code") Is Nothing Or Request("g_mobctry_code") = ""), DirectCast(Request("g_mobctry_code"), String).Trim(), "")
            Dim groom_mobile_no As String = If(Not (Request("g_mob_no") Is Nothing Or Request("g_mob_no") = ""), DirectCast(Request("g_mob_no"), String).Trim(), "")
            Dim groom_email As String = If(Not (Request("g_email") Is Nothing Or Request("g_email") = ""), DirectCast(Request("g_email"), String).Trim(), "")
            Dim groom_email_dn As String = If(Not (Request("g_emaildn") Is Nothing Or Request("g_emaildn") = ""), DirectCast(Request("g_emaildn"), String).Trim(), "")
            Dim groom_ocupatn As String = If(Not (Request("g_occupation") Is Nothing Or Request("g_occupation") = ""), DirectCast(Request("g_occupation"), String).Trim(), "")
            Dim groom_ocupatn_othr As String = If(Not (Request("g_occupation_othr") Is Nothing Or Request("g_occupation_othr") = ""), DirectCast(Request("g_occupation_othr"), String).Trim(), "")
            Dim groom_bpl As String = If(Not (Request("g_bpl") Is Nothing Or Request("g_bpl") = ""), DirectCast(Request("g_bpl"), String).Trim(), "")
            Dim groom_bpl_no As String = If(Not (Request("g_bpl_no") Is Nothing Or Request("g_bpl_no") = ""), DirectCast(Request("g_bpl_no"), String).Trim(), "")
            Dim groom_passport_no As String = If(Not (Request("g_pasport_no") Is Nothing Or Request("g_pasport_no") = ""), DirectCast(Request("g_pasport_no"), String).Trim(), "")
            Dim groom_passport_ctry As String = If(Not (Request("g_pasport_ctry") Is Nothing Or Request("g_pasport_ctry") = ""), DirectCast(Request("g_pasport_ctry"), String).Trim(), "")
            Dim groom_passport_valid As String = If(Not (Request("g_pasport_valid") Is Nothing Or Request("g_pasport_valid") = ""), DirectCast(Request("g_pasport_valid"), String).Trim(), "")
            Dim groom_adhar_num As String = If(Not (Request("groom_adhar_num") Is Nothing Or Request("groom_adhar_num") = ""), DirectCast(Request("groom_adhar_num"), String).Trim(), "")
            Dim groom_Aadhr_Consent As String = If(Not (Request("groom_adhar_con") Is Nothing Or Request("groom_adhar_con") = ""), DirectCast(Request("groom_adhar_con"), String).Trim(), "")
            Dim groom_visa_etaNO As String = If(Not (Request("g_visa_etaNo") Is Nothing Or Request("g_visa_etaNo") = ""), DirectCast(Request("g_visa_etaNo"), String).Trim(), "")
            Dim groom_visa_purpose As String = If(Not (Request("g_visa_Purpose") Is Nothing Or Request("g_visa_Purpose") = ""), DirectCast(Request("g_visa_Purpose"), String).Trim(), "")
            Dim groom_visa_valid As String = If(Not (Request("g_visa_valid") Is Nothing Or Request("g_visa_valid") = ""), DirectCast(Request("g_visa_valid"), String).Trim(), "")
            Dim groom_visa_DtOfEntry As String = If(Not (Request("g_visa_Indiaentry") Is Nothing Or Request("g_visa_Indiaentry") = ""), DirectCast(Request("g_visa_Indiaentry"), String).Trim(), "")
            Dim f_draft_cd As String = If(Not Request("draft_cd") Is Nothing, DirectCast(Request("draft_cd"), String).Trim(), "")
            Dim draft_cd = If(Not (Session.Item("draft_cd") Is Nothing Or Session.Item("draft_cd") = ""), DirectCast(Session.Item("draft_cd"), String).Trim(), "")
            If (draft_cd <> f_draft_cd) Then
                response("error") = "Bad request"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            Dim Groom_Email_ID As String = ""
            If (groom_email = "" And groom_email_dn = "") Then
                response("error") = "Please enter Email ID"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            Else
                Groom_Email_ID = a.sqlInj_in(groom_email.Trim() + "@" + groom_email_dn.Trim())
            End If
            If (Act = "") Then
                response("error") = "Please select Act"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (groom_fname = "") Then
                response("error") = "Please enter name of groom/husband"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (groom_guardian_type = "") Then
                response("error") = "Please select guardian type of groom/husband"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (groom_father_fname = "") Then
                response("error") = "Please enter father's first name of groom/husband"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (groom_dob = "") Then
                response("error") = "Please enter DOB"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (groom_mother_fname = "") Then
                response("error") = "Please select mothers of groom/husband"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (groom_relegion = "0") Then
                response("error") = "Please select religion"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            Else
                If (groom_relegion = "05") Then
                    If (groom_relegion_other = "") Then
                        response("error") = "Please enter Other name of religion"
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                End If
            End If
            If (groom_mobile_no = "") Then
                response("error") = "Please enter Mobile No."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (groom_email = "" Or groom_email_dn = "") Then
                response("error") = "Please enter Email-ID."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (groom_ocupatn = "0") Then
                response("error") = "Please select Occupation"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            Else
                If (groom_ocupatn = "05") Then
                    If (groom_ocupatn_othr = "") Then
                        response("error") = "Please enter Other name of Occupation"
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                End If
            End If
            If (groom_nationality = "1") Then
                If groom_adhar_num = "" Then
                    response("error") = "Please Enter Adhaar No."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
            Else
                If groom_passport_no = "" Then
                    response("error") = "Please Enter Passport No."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If groom_passport_ctry = "0" Then
                    response("error") = "Please Select Passport Country"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If groom_passport_valid = "" Then
                    response("error") = "Please Enter Validity Date of Passport"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If (groom_nationality = "3") Then
                    If groom_visa_etaNO = "" Then
                        response("error") = "Please Enter Visa Eta No."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                    If groom_visa_purpose = "" Then
                        response("error") = "Please Enter Purpose of Visa "
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                    If groom_visa_valid = "" Then
                        response("error") = "Please Enter Validity of Visa"
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                    If groom_visa_DtOfEntry = "" Then
                        response("error") = "Please Enter Date of Entry in INDIA for Visa"
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                End If
            End If

            '-------------------------------------------------------------

            Dim obj = New MARREG_ApplyOnline_Webservice_BL()

            Dim msg = obj.SaveGroomPersonalDetails(draft_cd, Act, groom_fname, groom_mname, groom_lname, groom_guardian_type, groom_father_fname, groom_father_mname, groom_father_lname, groom_mother_fname, groom_mother_mname, gromm_mother_lname,
                                                    groom_dob, groom_relegion, groom_relegion_other, groom_nationality, groom_mobnum_ctry, groom_mobile_no, Groom_Email_ID,
                                                    groom_ocupatn, groom_ocupatn_othr, groom_passport_no, groom_passport_ctry, groom_passport_valid, groom_adhar_num, groom_Aadhr_Consent, groom_bpl, groom_bpl_no,
                                                    groom_visa_etaNO, groom_visa_purpose, groom_visa_valid, groom_visa_DtOfEntry)
            If (msg = "1") Then
                json("success") = "Details Inserted Successfully "
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
            Else
                json("error") = "Something went wrong"
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
    Public Sub SaveGroomMaritalDetails()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
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
            Dim groom_marital_status As String = If(Not (Request("g_marital_status") Is Nothing Or Request("g_marital_status") = ""), DirectCast(Request("g_marital_status"), String).Trim(), "")
            Dim groom_case_no As String = If(Not (Request("g_nullity_case_no") Is Nothing Or Request("g_nullity_case_no") = ""), DirectCast(Request("g_nullity_case_no"), String).Trim(), "")
            Dim groom_court_name As String = If(Not (Request("g_nullity_court_name") Is Nothing Or Request("g_nullity_court_name") = ""), DirectCast(Request("g_nullity_court_name"), String).Trim(), "")
            Dim groom_Dateof_order As String = If(Not (Request("g_nullity_date_ofOrder") Is Nothing Or Request("g_nullity_date_ofOrder") = ""), DirectCast(Request("g_nullity_date_ofOrder"), String).Trim(), "")
            Dim groom_issued_by As String = If(Not (Request("g_nullity_issued") Is Nothing Or Request("g_nullity_issued") = ""), DirectCast(Request("g_nullity_issued"), String).Trim(), "")
            Dim groom_decree_act As String = If(Not (Request("g_nullity_issued_act") Is Nothing Or Request("g_nullity_issued_act") = ""), DirectCast(Request("g_nullity_issued_act"), String).Trim(), "")
            Dim groom_decree_section As String = If(Not (Request("g_decree_section") Is Nothing Or Request("g_decree_section") = ""), DirectCast(Request("g_decree_section"), String).Trim(), "")
            Dim groom_country As String = If(Not (Request("g_nullity_country") Is Nothing Or Request("g_nullity_country") = ""), DirectCast(Request("g_nullity_country"), String).Trim(), "")
            Dim groom_state As String = If(Not (Request("g_nullity_state") Is Nothing Or Request("g_nullity_state") = ""), DirectCast(Request("g_nullity_state"), String).Trim(), "")
            Dim groom_district As String = If(Not (Request("g_nullity_dist") Is Nothing Or Request("g_nullity_dist") = ""), DirectCast(Request("g_nullity_dist"), String).Trim(), "")
            Dim f_draft_cd As String = If(Not Request("draft_cd") Is Nothing, DirectCast(Request("draft_cd"), String).Trim(), "")
            Dim draft_cd = If(Not (Session.Item("draft_cd") Is Nothing Or Session.Item("draft_cd") = ""), DirectCast(Session.Item("draft_cd"), String).Trim(), "")
            If (draft_cd <> f_draft_cd) Then
                response("error") = "Bad request"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If

            If (groom_marital_status = "03" Or groom_marital_status = "06") Then
                If groom_case_no = "" Then
                    response("error") = "Please Enter Case No."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If groom_court_name = "" Then
                    response("error") = "Please Enter Name of the Court "
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If groom_decree_act = "0" Then
                    response("error") = "Please Enter Decree Act"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
            End If
            Dim obj = New MARREG_ApplyOnline_Webservice_BL()
            Dim msg = obj.SaveGroomMaritalDetails(draft_cd, groom_marital_status, groom_case_no, groom_court_name, groom_Dateof_order, groom_issued_by, groom_decree_act, groom_decree_section, groom_country, groom_state, groom_district)
            If (msg = 1) Then
                json("success") = "Details Inserted Successfully "
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
            Else
                json("error") = "Something went wrong"
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
    Public Sub SaveGroomAddressDetails()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
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
            Dim groom_prestadd_location As String = If(Not (Request("g_prestadd_location") Is Nothing Or Request("g_prestadd_location") = ""), DirectCast(Request("g_prestadd_location"), String).Trim(), "")
            Dim groom_prestadd_houseNo As String = If(Not (Request("g_prestadd_house") Is Nothing Or Request("g_prestadd_house") = ""), DirectCast(Request("g_prestadd_house"), String).Trim(), "")
            Dim groom_prestadd_locality_nm As String = If(Not (Request("g_prestadd_locality_nm") Is Nothing Or Request("g_prestadd_locality_nm") = ""), DirectCast(Request("g_prestadd_locality_nm"), String).Trim(), "")
            Dim groom_prestadd_city As String = If(Not (Request("g_prestadd_city") Is Nothing Or Request("g_prestadd_city") = ""), DirectCast(Request("g_prestadd_city"), String).Trim(), "")
            Dim groom_prestadd_st As String = If(Not (Request("g_prestadd_st") Is Nothing Or Request("g_prestadd_st") = ""), DirectCast(Request("g_prestadd_st"), String).Trim(), "")
            Dim groom_prestadd_dist As String = If(Not (Request("g_prestadd_dist") Is Nothing Or Request("g_prestadd_dist") = ""), DirectCast(Request("g_prestadd_dist"), String).Trim(), "")
            Dim groom_prestadd_wa As String = If(Not (Request("g_prestadd_wa") Is Nothing Or Request("g_prestadd_wa") = ""), DirectCast(Request("g_prestadd_wa"), String).Trim(), "")
            Dim groom_prestadd_subdiv As String = If(Not (Request("g_prestadd_subdiv") Is Nothing Or Request("g_prestadd_subdiv") = ""), DirectCast(Request("g_prestadd_subdiv"), String).Trim(), "")
            Dim groom_prestadd_block As String = If(Not (Request("g_prestadd_block") Is Nothing Or Request("g_prestadd_block") = ""), DirectCast(Request("g_prestadd_block"), String).Trim(), "")
            Dim groom_prestadd_ps As String = If(Not (Request("g_prestadd_ps") Is Nothing Or Request("g_prestadd_ps") = ""), DirectCast(Request("g_prestadd_ps"), String).Trim(), "")
            Dim groom_prestadd_munigp As String = If(Not (Request("g_prestadd_munigp") Is Nothing Or Request("g_prestadd_munigp") = ""), DirectCast(Request("g_prestadd_munigp"), String).Trim(), "")
            Dim groom_prestadd_vill As String = If(Not (Request("g_prestadd_vill") Is Nothing Or Request("g_prestadd_vill") = ""), DirectCast(Request("g_prestadd_vill"), String).Trim(), "")
            Dim groom_prestadd_txtvill As String = If(Not (Request("g_prestadd_txtvill") Is Nothing Or Request("g_prestadd_txtvill") = ""), DirectCast(Request("g_prestadd_txtvill"), String).Trim(), "")
            Dim groom_prestadd_pin As String = If(Not (Request("g_prestadd_pin") Is Nothing Or Request("g_prestadd_pin") = ""), DirectCast(Request("g_prestadd_pin"), String).Trim(), "")
            Dim groom_prestadd_po As String = If(Not (Request("g_prestadd_po") Is Nothing Or Request("g_prestadd_po") = ""), DirectCast(Request("g_prestadd_po"), String).Trim(), "")
            Dim groom_prestadd_txtst As String = If(Not (Request("g_prestadd_txtst") Is Nothing Or Request("g_prestadd_txtst") = ""), DirectCast(Request("g_prestadd_txtst"), String).Trim(), "")
            Dim groom_prestadd_txtdist As String = If(Not (Request("g_prestadd_txtdist") Is Nothing Or Request("g_prestadd_txtdist") = ""), DirectCast(Request("g_prestadd_txtdist"), String).Trim(), "")
            Dim groom_prestadd_txtblock As String = If(Not (Request("g_prestadd_txtblock") Is Nothing Or Request("g_prestadd_txtblock") = ""), DirectCast(Request("g_prestadd_txtblock"), String).Trim(), "")
            Dim groom_prestadd_txtsubdiv As String = If(Not (Request("g_prestadd_txtsubdiv") Is Nothing Or Request("g_prestadd_txtsubdiv") = ""), DirectCast(Request("g_prestadd_txtsubdiv"), String).Trim(), "")
            Dim groom_prestadd_txtps As String = If(Not (Request("g_prestadd_txtps") Is Nothing Or Request("g_prestadd_txtps") = ""), DirectCast(Request("g_prestadd_txtps"), String).Trim(), "")
            Dim groom_prestadd_txtmunigp As String = If(Not (Request("g_prestadd_txtmunigp") Is Nothing Or Request("g_prestadd_txtmunigp") = ""), DirectCast(Request("g_prestadd_txtmunigp"), String).Trim(), "")
            Dim groom_prestadd_txtpo As String = If(Not (Request("g_prestadd_txtpo") Is Nothing Or Request("g_prestadd_txtpo") = ""), DirectCast(Request("g_prestadd_txtpo"), String).Trim(), "")

            Dim groom_pmntadd_location As String = If(Not (Request("g_pmntadd_location") Is Nothing Or Request("g_pmntadd_location") = ""), DirectCast(Request("g_pmntadd_location"), String).Trim(), "")
            Dim groom_pmntadd_houseNo As String = If(Not (Request("g_pmntadd_house") Is Nothing Or Request("g_pmntadd_house") = ""), DirectCast(Request("g_pmntadd_house"), String).Trim(), "")
            Dim groom_pmntadd_locality_nm As String = If(Not (Request("g_pmntadd_locality_nm") Is Nothing Or Request("g_pmntadd_locality_nm") = ""), DirectCast(Request("g_pmntadd_locality_nm"), String).Trim(), "")
            Dim groom_pmntadd_city As String = If(Not (Request("g_pmntadd_city") Is Nothing Or Request("g_pmntadd_city") = ""), DirectCast(Request("g_pmntadd_city"), String).Trim(), "")
            Dim groom_pmntadd_st As String = If(Not (Request("g_pmntadd_st") Is Nothing Or Request("g_pmntadd_st") = ""), DirectCast(Request("g_pmntadd_st"), String).Trim(), "")
            Dim groom_pmntadd_dist As String = If(Not (Request("g_pmntadd_dist") Is Nothing Or Request("g_pmntadd_dist") = ""), DirectCast(Request("g_pmntadd_dist"), String).Trim(), "")
            Dim groom_pmntadd_wa As String = If(Not (Request("g_pmntadd_wa") Is Nothing Or Request("g_pmntadd_wa") = ""), DirectCast(Request("g_pmntadd_wa"), String).Trim(), "")
            Dim groom_pmntadd_subdiv As String = If(Not (Request("g_pmntadd_subdiv") Is Nothing Or Request("g_pmntadd_subdiv") = ""), DirectCast(Request("g_pmntadd_subdiv"), String).Trim(), "")
            Dim groom_pmntadd_block As String = If(Not (Request("g_pmntadd_block") Is Nothing Or Request("g_pmntadd_block") = ""), DirectCast(Request("g_pmntadd_block"), String).Trim(), "")
            Dim groom_pmntadd_ps As String = If(Not (Request("g_pmntadd_ps") Is Nothing Or Request("g_pmntadd_ps") = ""), DirectCast(Request("g_pmntadd_ps"), String).Trim(), "")
            Dim groom_pmntadd_munigp As String = If(Not (Request("g_pmntadd_munigp") Is Nothing Or Request("g_pmntadd_munigp") = ""), DirectCast(Request("g_pmntadd_munigp"), String).Trim(), "")
            Dim groom_pmntadd_vill As String = If(Not (Request("g_pmntadd_vill") Is Nothing Or Request("g_pmntadd_vill") = ""), DirectCast(Request("g_pmntadd_vill"), String).Trim(), "")

            Dim groom_pmntadd_txtvill As String = If(Not (Request("g_pmntadd_txtvill") Is Nothing Or Request("g_pmntadd_txtvill") = ""), DirectCast(Request("g_pmntadd_txtvill"), String).Trim(), "")
            Dim groom_pmntadd_pin As String = If(Not (Request("g_pmntadd_pin") Is Nothing Or Request("g_pmntadd_pin") = ""), DirectCast(Request("g_pmntadd_pin"), String).Trim(), "")
            Dim groom_pmntadd_po As String = If(Not (Request("g_pmntadd_po") Is Nothing Or Request("g_pmntadd_po") = ""), DirectCast(Request("g_pmntadd_po"), String).Trim(), "")
            Dim groom_pmntadd_txtst As String = If(Not (Request("g_pmntadd_txtst") Is Nothing Or Request("g_pmntadd_txtst") = ""), DirectCast(Request("g_pmntadd_txtst"), String).Trim(), "")
            Dim groom_pmntadd_txtdist As String = If(Not (Request("g_pmntadd_txtdist") Is Nothing Or Request("g_pmntadd_txtdist") = ""), DirectCast(Request("g_pmntadd_txtdist"), String).Trim(), "")
            Dim groom_pmntadd_txtblock As String = If(Not (Request("g_pmntadd_txtblock") Is Nothing Or Request("g_pmntadd_txtblock") = ""), DirectCast(Request("g_pmntadd_txtblock"), String).Trim(), "")
            Dim groom_pmntadd_txtsubdiv As String = If(Not (Request("g_pmntadd_txtsubdiv") Is Nothing Or Request("g_pmntadd_txtsubdiv") = ""), DirectCast(Request("g_pmntadd_txtsubdiv"), String).Trim(), "")
            Dim groom_pmntadd_txtps As String = If(Not (Request("g_pmntadd_txtps") Is Nothing Or Request("g_pmntadd_txtps") = ""), DirectCast(Request("g_pmntadd_txtps"), String).Trim(), "")
            Dim groom_pmntadd_txtmunigp As String = If(Not (Request("g_pmntadd_txtmunigp") Is Nothing Or Request("g_pmntadd_txtmunigp") = ""), DirectCast(Request("g_pmntadd_txtmunigp"), String).Trim(), "")
            Dim groom_pmntadd_txtpo As String = If(Not (Request("g_pmntadd_txtpo") Is Nothing Or Request("g_pmntadd_txtpo") = ""), DirectCast(Request("g_pmntadd_txtpo"), String).Trim(), "")

            Dim is_same_address As String = If(Not (Request("same_address") Is Nothing Or Request("same_address") = ""), DirectCast(Request("same_address"), String).Trim(), "")


            If groom_prestadd_city Is Nothing Or groom_prestadd_city = "0" Then
                response("error") = "Please Select Present Address Country."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If groom_prestadd_pin = "" Then
                response("error") = "Please Enter Present Address PIN Code."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If groom_prestadd_location = "16" Then
                If groom_prestadd_st Is Nothing Or groom_prestadd_st = "0" Then
                    response("error") = "Please Select Present Address State."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If groom_prestadd_dist = "" Or groom_prestadd_dist = "0" Then
                    response("error") = "Please Select Present Address District."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If groom_prestadd_subdiv = "" Or groom_prestadd_subdiv = "0" Then
                    response("error") = "Please Select Present Address Sub-Division."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If groom_prestadd_ps = "" Or groom_prestadd_ps = "0" Then
                    response("error") = "Please Select Present Address PS."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If groom_prestadd_po = "" Or groom_prestadd_po = "0" Then
                    response("error") = "Please Select Present Address Post Office."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If groom_prestadd_wa = "02" Then
                    If groom_prestadd_munigp = "" Or groom_prestadd_munigp = "0" Then
                        response("error") = "Please Select Present Address Municipality."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                Else
                    If groom_prestadd_block = "" Or groom_prestadd_block = "0" Then
                        response("error") = "Please Select Present Address Block."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                    If groom_prestadd_vill = "" Or groom_prestadd_vill = "0" Then
                        response("error") = "Please Enter Present Address Village."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                    If groom_prestadd_munigp = "" Or groom_prestadd_munigp = "0" Then
                        response("error") = "Please Select Present Address GP."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                End If
            Else
                If groom_prestadd_location = "17" Then
                    If groom_prestadd_st Is Nothing Or groom_prestadd_st = "0" Then
                        response("error") = "Please Select Present Address State."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                    If groom_prestadd_dist = "" Or groom_prestadd_dist = "0" Then
                        response("error") = "Please Select Present Address District."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                End If
                If groom_prestadd_location = "18" Then
                    If groom_prestadd_txtst = "" Then
                        response("error") = "Please Select Present Address State."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                    If groom_prestadd_txtdist = "" Then
                        response("error") = "Please Select Present Address District."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                End If

                If groom_prestadd_txtsubdiv = "" Then
                    response("error") = "Please Select Present Address Sub-Division."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If groom_prestadd_txtps = "" Then
                    response("error") = "Please Select Present Address PS."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If groom_prestadd_txtpo = "" Then
                    response("error") = "Please Select Present Address Post Office."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If groom_prestadd_wa = "01" Then
                    If groom_prestadd_txtmunigp = "" Then
                        response("error") = "Please Select Present Address Municipality."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                Else
                    If groom_prestadd_txtblock = "" Then
                        response("error") = "Please Select Present Address Block."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                    If groom_prestadd_vill = "" Or groom_prestadd_vill = "0" Then
                        response("error") = "Please Enter Present Address Village."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                    If groom_prestadd_txtmunigp = "" Then
                        response("error") = "Please Select Present Address GP."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                End If
            End If

            'Permanent Address
            If groom_pmntadd_city Is Nothing Or groom_pmntadd_city = "0" Then
                response("error") = "Please Select Permanent Address Country."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If groom_pmntadd_pin = "" Then
                response("error") = "Please Enter Permanent Address PIN Code."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If groom_pmntadd_location = "16" Then
                If groom_pmntadd_st Is Nothing Or groom_pmntadd_st = "0" Then
                    response("error") = "Please Select Permanent Address State."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If groom_pmntadd_dist = "" Or groom_pmntadd_dist = "0" Then
                    response("error") = "Please Select Permanent Address District."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If groom_pmntadd_subdiv = "" Or groom_pmntadd_subdiv = "0" Then
                    response("error") = "Please Select Permanent Address Sub-Division."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If groom_pmntadd_ps = "" Or groom_pmntadd_ps = "0" Then
                    response("error") = "Please Select Permanent Address PS."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If groom_pmntadd_po = "" Or groom_pmntadd_po = "0" Then
                    response("error") = "Please Select Permanent Address Post Office."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If groom_pmntadd_wa = "02" Then
                    If groom_pmntadd_munigp = "" Or groom_pmntadd_munigp = "0" Then
                        response("error") = "Please Select Permanent Address Municipality."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                Else
                    If groom_pmntadd_block = "" Or groom_pmntadd_block = "0" Then
                        response("error") = "Please Select Permanent Address Block."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                    If groom_pmntadd_vill = "" Or groom_pmntadd_vill = "0" Then
                        response("error") = "Please Enter Permanent Address Village."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                    If groom_pmntadd_munigp = "" Or groom_pmntadd_munigp = "0" Then
                        response("error") = "Please Select Permanent Address GP."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                End If
            Else
                If groom_pmntadd_location = "17" Then
                    If groom_pmntadd_st Is Nothing Or groom_pmntadd_st = "0" Then
                        response("error") = "Please Select Permanent Address State."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                    If groom_pmntadd_dist = "" Or groom_pmntadd_dist = "0" Then
                        response("error") = "Please Select Permanent Address District."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                End If
                If groom_pmntadd_location = "18" Then
                    If groom_pmntadd_txtst Is Nothing Or groom_pmntadd_txtst = "" Then
                        response("error") = "Please Select Permanent Address State."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                    If groom_pmntadd_txtdist = "" Then
                        response("error") = "Please Select Permanent Address District."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                End If

                If groom_pmntadd_txtsubdiv = "" Then
                    response("error") = "Please Select Permanent Address Sub-Division."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If groom_pmntadd_txtps = "" Then
                    response("error") = "Please Select Permanent Address PS."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If groom_pmntadd_txtpo = "" Then
                    response("error") = "Please Select Permanent Address Post Office."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If groom_pmntadd_wa = "01" Then
                    If groom_pmntadd_txtmunigp = "" Then
                        response("error") = "Please Select Permanent Address Municipality."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                Else
                    If groom_pmntadd_txtblock = "" Then
                        response("error") = "Please Select Permanent Address Block."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                    If groom_pmntadd_vill = "" Or groom_pmntadd_vill = "0" Then
                        response("error") = "Please Enter Permanent Address Village."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                    If groom_pmntadd_txtmunigp = "" Then
                        response("error") = "Please Select Permanent Address GP."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                End If
            End If

            Dim obj = New MARREG_ApplyOnline_Webservice_BL()
            Dim msg = obj.SaveGroomAddressDetails(groom_prestadd_location, groom_prestadd_houseNo, groom_prestadd_locality_nm, groom_prestadd_city, groom_prestadd_st, groom_prestadd_dist, groom_prestadd_wa,
                                                  groom_prestadd_subdiv, groom_prestadd_block, groom_prestadd_ps, groom_prestadd_munigp, groom_prestadd_vill, groom_prestadd_txtvill,
                                                  groom_prestadd_pin, groom_prestadd_po, groom_prestadd_txtst, groom_prestadd_txtdist, groom_prestadd_txtblock, groom_prestadd_txtsubdiv, groom_prestadd_txtps, groom_prestadd_txtmunigp, groom_prestadd_txtpo,
                                                  groom_pmntadd_location, groom_pmntadd_houseNo, groom_pmntadd_locality_nm, groom_pmntadd_city, groom_pmntadd_st, groom_pmntadd_dist, groom_pmntadd_wa, groom_pmntadd_subdiv, groom_pmntadd_block, groom_pmntadd_ps,
                                                  groom_pmntadd_munigp, groom_pmntadd_vill, groom_pmntadd_txtvill, groom_pmntadd_pin, groom_pmntadd_po, groom_pmntadd_txtst, groom_pmntadd_txtdist, groom_pmntadd_txtblock, groom_pmntadd_txtsubdiv,
                                                  groom_pmntadd_txtps, groom_pmntadd_txtmunigp, groom_pmntadd_txtpo, is_same_address)

            If (msg = 1) Then
                json("success") = "Details Inserted Successfully "
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
            Else
                json("error") = "Something went wrong"
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
#Region "Save bride Details"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub SaveBridePersonalDetails()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
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
            'Dim Act As String = If(Not (Request("act_cd") Is Nothing Or Request("act_cd") = ""), DirectCast(Request("act_cd"), String).Trim(), "")
            'Dim Draft_Cd As String = "9413"
            Dim bride_fname As String = If(Not (Request("b_first_name") Is Nothing Or Request("b_first_name") = ""), DirectCast(Request("b_first_name"), String).Trim(), "")
            Dim bride_mname As String = If(Not (Request("b_middle_name") Is Nothing Or Request("b_middle_name") = ""), DirectCast(Request("b_middle_name"), String).Trim(), "")
            Dim bride_lname As String = If(Not (Request("b_last_name") Is Nothing Or Request("b_last_name") = ""), DirectCast(Request("b_last_name"), String).Trim(), "")
            Dim bride_guardian_type As String = If(Not (Request("b_guardian_type") Is Nothing Or Request("b_guardian_type") = ""), DirectCast(Request("b_guardian_type"), String).Trim(), "")
            Dim bride_father_fname As String = If(Not (Request("bf_first_name") Is Nothing Or Request("bf_first_name") = ""), DirectCast(Request("bf_first_name"), String).Trim(), "")
            Dim bride_father_mname As String = If(Not (Request("bf_middle_name") Is Nothing Or Request("bf_middle_name") = ""), DirectCast(Request("bf_middle_name"), String).Trim(), "")
            Dim bride_father_lname As String = If(Not (Request("bf_last_name") Is Nothing Or Request("bf_last_name") = ""), DirectCast(Request("bf_last_name"), String).Trim(), "")
            Dim bride_mother_fname As String = If(Not (Request("bm_first_name") Is Nothing Or Request("bm_first_name") = ""), DirectCast(Request("bm_first_name"), String).Trim(), "")
            Dim bride_mother_mname As String = If(Not (Request("bm_middle_name") Is Nothing Or Request("bm_middle_name") = ""), DirectCast(Request("bm_middle_name"), String).Trim(), "")
            Dim bride_mother_lname As String = If(Not (Request("bm_last_name") Is Nothing Or Request("bm_last_name") = ""), DirectCast(Request("bm_last_name"), String).Trim(), "")
            Dim bride_dob As String = If(Not (Request("b_dob") Is Nothing Or Request("b_dob") = ""), DirectCast(Request("b_dob"), String).Trim(), "")
            Dim bride_relegion As String = If(Not (Request("b_religion") Is Nothing Or Request("b_religion") = ""), DirectCast(Request("b_religion"), String).Trim(), "")
            Dim bride_relegion_other As String = If(Not (Request("b_religion_othr") Is Nothing Or Request("b_religion_othr") = ""), DirectCast(Request("b_religion_othr"), String).Trim(), "")
            Dim bride_nationality As String = If(Not (Request("b_nationality") Is Nothing Or Request("b_nationality") = ""), DirectCast(Request("b_nationality"), String).Trim(), "")
            Dim bride_mobnum_ctry As String = If(Not (Request("b_mobctry_code") Is Nothing Or Request("b_mobctry_code") = ""), DirectCast(Request("b_mobctry_code"), String).Trim(), "")
            Dim bride_mobile_no As String = If(Not (Request("b_mob_no") Is Nothing Or Request("b_mob_no") = ""), DirectCast(Request("b_mob_no"), String).Trim(), "")
            Dim bride_email As String = If(Not (Request("b_email") Is Nothing Or Request("b_email") = ""), DirectCast(Request("b_email"), String).Trim(), "")
            Dim bride_email_dn As String = If(Not (Request("b_emaildn") Is Nothing Or Request("b_emaildn") = ""), DirectCast(Request("b_emaildn"), String).Trim(), "")
            Dim bride_ocupatn As String = If(Not (Request("b_occupation") Is Nothing Or Request("b_occupation") = ""), DirectCast(Request("b_occupation"), String).Trim(), "")
            Dim bride_ocupatn_othr As String = If(Not (Request("b_occupation_othr") Is Nothing Or Request("b_occupation_othr") = ""), DirectCast(Request("b_occupation_othr"), String).Trim(), "")
            Dim bride_bpl As String = If(Not (Request("b_bpl") Is Nothing Or Request("b_bpl") = ""), DirectCast(Request("b_bpl"), String).Trim(), "")
            Dim bride_bpl_no As String = If(Not (Request("b_bpl_no") Is Nothing Or Request("b_bpl_no") = ""), DirectCast(Request("b_bpl_no"), String).Trim(), "")
            Dim bride_passport_no As String = If(Not (Request("b_pasport_no") Is Nothing Or Request("b_pasport_no") = ""), DirectCast(Request("b_pasport_no"), String).Trim(), "")
            Dim bride_passport_ctry As String = If(Not (Request("b_pasport_ctry") Is Nothing Or Request("b_pasport_ctry") = ""), DirectCast(Request("b_pasport_ctry"), String).Trim(), "")
            Dim bride_passport_valid As String = If(Not (Request("b_pasport_valid") Is Nothing Or Request("b_pasport_valid") = ""), DirectCast(Request("b_pasport_valid"), String).Trim(), "")
            Dim bride_adhar_num As String = If(Not (Request("bride_adhar_num") Is Nothing Or Request("bride_adhar_num") = ""), DirectCast(Request("bride_adhar_num"), String).Trim(), "")
            Dim bride_Aadhr_Consent As String = If(Not (Request("bride_adhar_con") Is Nothing Or Request("bride_adhar_con") = ""), DirectCast(Request("bride_adhar_con"), String).Trim(), "")
            Dim bride_visa_etaNO As String = If(Not (Request("b_visa_etaNo") Is Nothing Or Request("b_visa_etaNo") = ""), DirectCast(Request("b_visa_etaNo"), String).Trim(), "")
            Dim bride_visa_purpose As String = If(Not (Request("b_visa_Purpose") Is Nothing Or Request("b_visa_Purpose") = ""), DirectCast(Request("b_visa_Purpose"), String).Trim(), "")
            Dim bride_visa_valid As String = If(Not (Request("b_visa_valid") Is Nothing Or Request("b_visa_valid") = ""), DirectCast(Request("b_visa_valid"), String).Trim(), "")
            Dim bride_visa_DtOfEntry As String = If(Not (Request("b_visa_Indiaentry") Is Nothing Or Request("b_visa_Indiaentry") = ""), DirectCast(Request("b_visa_Indiaentry"), String).Trim(), "")
            Dim f_draft_cd As String = If(Not Request("draft_cd") Is Nothing, DirectCast(Request("draft_cd"), String).Trim(), "")
            Dim draft_cd = If(Not (Session.Item("draft_cd") Is Nothing Or Session.Item("draft_cd") = ""), DirectCast(Session.Item("draft_cd"), String).Trim(), "")
            If (draft_cd <> f_draft_cd) Then
                response("error") = "Bad request"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            Dim Bride_Email_ID As String = ""
            If (bride_email = "" And bride_email_dn = "") Then
                response("error") = "Please enter Email ID"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            Else
                Bride_Email_ID = a.sqlInj_in(bride_email.Trim() + "@" + bride_email_dn.Trim())
            End If
            If (bride_fname = "") Then
                response("error") = "Please enter Name"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (bride_guardian_type = "") Then
                response("error") = "Please select guardian type"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (bride_father_fname = "") Then
                response("error") = "Please enter father's first name"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (bride_dob = "") Then
                response("error") = "Please enter DOB"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (bride_mother_fname = "") Then
                response("error") = "Please enter mother's name"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            '------------------------------------------------------------------
            If (bride_relegion = "0") Then
                response("error") = "Please select religion"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            Else
                If (bride_relegion = "05") Then
                    If (bride_relegion_other = "") Then
                        response("error") = "Please enter Other name of religion"
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                End If
            End If
            If (bride_mobile_no = "") Then
                response("error") = "Please enter Mobile No."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (bride_email = "" Or bride_email_dn = "") Then
                response("error") = "Please enter Email-ID."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (bride_ocupatn = "0") Then
                response("error") = "Please select Occupation"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            Else
                If (bride_ocupatn = "05") Then
                    If (bride_ocupatn_othr = "") Then
                        response("error") = "Please enter Other name of Occupation"
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                End If
            End If
            If (bride_nationality = "1") Then
                If bride_adhar_num = "" Then
                    response("error") = "Please Enter Adhaar No."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
            Else
                If bride_passport_no = "" Then
                    response("error") = "Please Enter Passport No."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If bride_passport_ctry = "0" Then
                    response("error") = "Please Select Passport Country"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If bride_passport_valid = "" Then
                    response("error") = "Please Enter Validity Date of Passport"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If (bride_nationality = "3") Then
                    If bride_visa_etaNO = "" Then
                        response("error") = "Please Enter Visa Eta No."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                    If bride_visa_purpose = "" Then
                        response("error") = "Please Enter Purpose of Visa "
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                    If bride_visa_valid = "" Then
                        response("error") = "Please Enter Validity of Visa"
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                    If bride_visa_DtOfEntry = "" Then
                        response("error") = "Please Enter Date of Entry in INDIA for Visa"
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                End If
            End If

            'If bride_dob <> "" Then
            '    bride_dob = Convert.ToDateTime(bride_dob)
            'End If
            'If bride_passport_valid <> "" Then
            '    bride_passport_valid = Convert.ToDateTime(bride_passport_valid)
            'End If
            'If bride_visa_valid <> "" Then
            '    bride_visa_valid = Convert.ToDateTime(bride_visa_valid)
            'End If
            'If bride_visa_DtOfEntry <> "" Then
            '    bride_visa_DtOfEntry = Convert.ToDateTime(bride_visa_DtOfEntry)
            'End If

            Dim obj = New MARREG_ApplyOnline_Webservice_BL()
            Dim msg = obj.SaveBridePersonalDetails(draft_cd, bride_fname, bride_mname, bride_lname, bride_guardian_type, bride_father_fname, bride_father_mname, bride_father_lname, bride_mother_fname, bride_mother_mname, bride_mother_lname,
                                                    bride_dob, bride_relegion, bride_relegion_other, bride_nationality, bride_mobnum_ctry, bride_mobile_no, Bride_Email_ID,
                                                    bride_ocupatn, bride_ocupatn_othr, bride_passport_no, bride_passport_ctry, bride_passport_valid, bride_adhar_num, bride_Aadhr_Consent, bride_bpl, bride_bpl_no,
                                                    bride_visa_etaNO, bride_visa_purpose, bride_visa_valid, bride_visa_DtOfEntry)
            If (msg = 1) Then
                json("success") = "Details Inserted Successfully "
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
            Else
                json("error") = "Something went wrong"
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
    Public Sub SaveBrideMaritalDetails()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
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
            Dim Bride_marital_status As String = If(Not (Request("b_marital_status") Is Nothing Or Request("b_marital_status") = ""), DirectCast(Request("b_marital_status"), String).Trim(), "")
            Dim Bride_case_no As String = If(Not (Request("b_nullity_case_no") Is Nothing Or Request("b_nullity_case_no") = ""), DirectCast(Request("b_nullity_case_no"), String).Trim(), "")
            Dim Bride_court_name As String = If(Not (Request("b_nullity_court_name") Is Nothing Or Request("b_nullity_court_name") = ""), DirectCast(Request("b_nullity_court_name"), String).Trim(), "")
            Dim Bride_Dateof_order As String = If(Not (Request("b_nullity_date_ofOrder") Is Nothing Or Request("b_nullity_date_ofOrder") = ""), DirectCast(Request("b_nullity_date_ofOrder"), String).Trim(), "")
            Dim Bride_issued_by As String = If(Not (Request("b_nullity_issued") Is Nothing Or Request("b_nullity_issued") = ""), DirectCast(Request("b_nullity_issued"), String).Trim(), "")
            Dim Bride_decree_act As String = If(Not (Request("b_nullity_issued_act") Is Nothing Or Request("b_nullity_issued_act") = ""), DirectCast(Request("b_nullity_issued_act"), String).Trim(), "")
            Dim Bride_decree_section As String = If(Not (Request("b_decree_section") Is Nothing Or Request("b_decree_section") = ""), DirectCast(Request("b_decree_section"), String).Trim(), "")
            Dim Bride_country As String = If(Not (Request("b_nullity_country") Is Nothing Or Request("b_nullity_country") = ""), DirectCast(Request("b_nullity_country"), String).Trim(), "")
            Dim Bride_state As String = If(Not (Request("b_nullity_state") Is Nothing Or Request("b_nullity_state") = ""), DirectCast(Request("b_nullity_state"), String).Trim(), "")
            Dim Bride_district As String = If(Not (Request("b_nullity_dist") Is Nothing Or Request("b_nullity_dist") = ""), DirectCast(Request("b_nullity_dist"), String).Trim(), "")
            Dim f_draft_cd As String = If(Not Request("draft_cd") Is Nothing, DirectCast(Request("draft_cd"), String).Trim(), "")
            Dim draft_cd = If(Not (Session.Item("draft_cd") Is Nothing Or Session.Item("draft_cd") = ""), DirectCast(Session.Item("draft_cd"), String).Trim(), "")
            If (draft_cd <> f_draft_cd) Then
                response("error") = "Bad request"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If

            If (Bride_marital_status = "03" Or Bride_marital_status = "06") Then
                If Bride_case_no = "" Then
                    response("error") = "Please Enter Case No."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If Bride_court_name = "" Then
                    response("error") = "Please Enter Name of the Court "
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If Bride_decree_act = "0" Then
                    response("error") = "Please Enter Decree Act"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
            End If
            Dim obj = New MARREG_ApplyOnline_Webservice_BL()
            Dim msg = obj.SaveBrideMaritalDetails(draft_cd, Bride_marital_status, Bride_case_no, Bride_court_name, Bride_Dateof_order, Bride_issued_by, Bride_decree_act, Bride_decree_section, Bride_country, Bride_state, Bride_district)
            If (msg = 1) Then
                json("success") = "Details Inserted Successfully "
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
            Else
                json("error") = "Something went wrong"
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
    Public Sub SaveBrideAddressDetails()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
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
            Dim bride_prestadd_location As String = If(Not (Request("b_prestadd_location") Is Nothing Or Request("b_prestadd_location") = ""), DirectCast(Request("b_prestadd_location"), String).Trim(), "")
            Dim bride_prestadd_houseNo As String = If(Not (Request("b_prestadd_house") Is Nothing Or Request("b_prestadd_house") = ""), DirectCast(Request("b_prestadd_house"), String).Trim(), "")
            Dim bride_prestadd_locality_nm As String = If(Not (Request("b_prestadd_locality_nm") Is Nothing Or Request("b_prestadd_locality_nm") = ""), DirectCast(Request("b_prestadd_locality_nm"), String).Trim(), "")
            Dim bride_prestadd_city As String = If(Not (Request("b_prestadd_city") Is Nothing Or Request("b_prestadd_city") = ""), DirectCast(Request("b_prestadd_city"), String).Trim(), "")
            Dim bride_prestadd_st As String = If(Not (Request("b_prestadd_st") Is Nothing Or Request("b_prestadd_st") = ""), DirectCast(Request("b_prestadd_st"), String).Trim(), "")
            Dim bride_prestadd_dist As String = If(Not (Request("b_prestadd_dist") Is Nothing Or Request("b_prestadd_dist") = ""), DirectCast(Request("b_prestadd_dist"), String).Trim(), "")
            Dim bride_prestadd_wa As String = If(Not (Request("b_prestadd_wa") Is Nothing Or Request("b_prestadd_wa") = ""), DirectCast(Request("b_prestadd_wa"), String).Trim(), "")
            Dim bride_prestadd_subdiv As String = If(Not (Request("b_prestadd_subdiv") Is Nothing Or Request("b_prestadd_subdiv") = ""), DirectCast(Request("b_prestadd_subdiv"), String).Trim(), "")
            Dim bride_prestadd_block As String = If(Not (Request("b_prestadd_block") Is Nothing Or Request("b_prestadd_block") = ""), DirectCast(Request("b_prestadd_block"), String).Trim(), "")
            Dim bride_prestadd_ps As String = If(Not (Request("b_prestadd_ps") Is Nothing Or Request("b_prestadd_ps") = ""), DirectCast(Request("b_prestadd_ps"), String).Trim(), "")
            Dim bride_prestadd_munigp As String = If(Not (Request("b_prestadd_munigp") Is Nothing Or Request("b_prestadd_munigp") = ""), DirectCast(Request("b_prestadd_munigp"), String).Trim(), "")
            Dim bride_prestadd_vill As String = If(Not (Request("b_prestadd_vill") Is Nothing Or Request("b_prestadd_vill") = ""), DirectCast(Request("b_prestadd_vill"), String).Trim(), "")
            Dim bride_prestadd_txtvill As String = If(Not (Request("b_prestadd_txtvill") Is Nothing Or Request("b_prestadd_txtvill") = ""), DirectCast(Request("b_prestadd_txtvill"), String).Trim(), "")
            Dim bride_prestadd_pin As String = If(Not (Request("b_prestadd_pin") Is Nothing Or Request("b_prestadd_pin") = ""), DirectCast(Request("b_prestadd_pin"), String).Trim(), "")
            Dim bride_prestadd_po As String = If(Not (Request("b_prestadd_po") Is Nothing Or Request("b_prestadd_po") = ""), DirectCast(Request("b_prestadd_po"), String).Trim(), "")
            Dim bride_prestadd_txtst As String = If(Not (Request("b_prestadd_txtst") Is Nothing Or Request("b_prestadd_txtst") = ""), DirectCast(Request("b_prestadd_txtst"), String).Trim(), "")
            Dim bride_prestadd_txtdist As String = If(Not (Request("b_prestadd_txtdist") Is Nothing Or Request("b_prestadd_txtdist") = ""), DirectCast(Request("b_prestadd_txtdist"), String).Trim(), "")
            Dim bride_prestadd_txtblock As String = If(Not (Request("b_prestadd_txtblock") Is Nothing Or Request("b_prestadd_txtblock") = ""), DirectCast(Request("b_prestadd_txtblock"), String).Trim(), "")
            Dim bride_prestadd_txtsubdiv As String = If(Not (Request("b_prestadd_txtsubdiv") Is Nothing Or Request("b_prestadd_txtsubdiv") = ""), DirectCast(Request("b_prestadd_txtsubdiv"), String).Trim(), "")
            Dim bride_prestadd_txtps As String = If(Not (Request("b_prestadd_txtps") Is Nothing Or Request("b_prestadd_txtps") = ""), DirectCast(Request("b_prestadd_txtps"), String).Trim(), "")
            Dim bride_prestadd_txtmunigp As String = If(Not (Request("b_prestadd_txtmunigp") Is Nothing Or Request("b_prestadd_txtmunigp") = ""), DirectCast(Request("b_prestadd_txtmunigp"), String).Trim(), "")
            Dim bride_prestadd_txtpo As String = If(Not (Request("b_prestadd_txtpo") Is Nothing Or Request("b_prestadd_txtpo") = ""), DirectCast(Request("b_prestadd_txtpo"), String).Trim(), "")

            Dim bride_pmntadd_location As String = If(Not (Request("b_pmntadd_location") Is Nothing Or Request("b_pmntadd_location") = ""), DirectCast(Request("b_pmntadd_location"), String).Trim(), "")
            Dim bride_pmntadd_houseNo As String = If(Not (Request("b_pmntadd_house") Is Nothing Or Request("b_pmntadd_house") = ""), DirectCast(Request("b_pmntadd_house"), String).Trim(), "")
            Dim bride_pmntadd_locality_nm As String = If(Not (Request("b_pmntadd_locality_nm") Is Nothing Or Request("b_pmntadd_locality_nm") = ""), DirectCast(Request("b_pmntadd_locality_nm"), String).Trim(), "")
            Dim bride_pmntadd_city As String = If(Not (Request("b_pmntadd_city") Is Nothing Or Request("b_pmntadd_city") = ""), DirectCast(Request("b_pmntadd_city"), String).Trim(), "")
            Dim bride_pmntadd_st As String = If(Not (Request("b_pmntadd_st") Is Nothing Or Request("b_pmntadd_st") = ""), DirectCast(Request("b_pmntadd_st"), String).Trim(), "")
            Dim bride_pmntadd_dist As String = If(Not (Request("b_pmntadd_dist") Is Nothing Or Request("b_pmntadd_dist") = ""), DirectCast(Request("b_pmntadd_dist"), String).Trim(), "")
            Dim bride_pmntadd_wa As String = If(Not (Request("b_pmntadd_wa") Is Nothing Or Request("b_pmntadd_wa") = ""), DirectCast(Request("b_pmntadd_wa"), String).Trim(), "")
            Dim bride_pmntadd_subdiv As String = If(Not (Request("b_pmntadd_subdiv") Is Nothing Or Request("b_pmntadd_subdiv") = ""), DirectCast(Request("b_pmntadd_subdiv"), String).Trim(), "")
            Dim bride_pmntadd_block As String = If(Not (Request("b_pmntadd_block") Is Nothing Or Request("b_pmntadd_block") = ""), DirectCast(Request("b_pmntadd_block"), String).Trim(), "")
            Dim bride_pmntadd_ps As String = If(Not (Request("b_pmntadd_ps") Is Nothing Or Request("b_pmntadd_ps") = ""), DirectCast(Request("b_pmntadd_ps"), String).Trim(), "")
            Dim bride_pmntadd_munigp As String = If(Not (Request("b_pmntadd_munigp") Is Nothing Or Request("b_pmntadd_munigp") = ""), DirectCast(Request("b_pmntadd_munigp"), String).Trim(), "")
            Dim bride_pmntadd_vill As String = If(Not (Request("b_pmntadd_vill") Is Nothing Or Request("b_pmntadd_vill") = ""), DirectCast(Request("b_pmntadd_vill"), String).Trim(), "")

            Dim bride_pmntadd_txtvill As String = If(Not (Request("b_pmntadd_txtvill") Is Nothing Or Request("b_pmntadd_txtvill") = ""), DirectCast(Request("b_pmntadd_txtvill"), String).Trim(), "")
            Dim bride_pmntadd_pin As String = If(Not (Request("b_pmntadd_pin") Is Nothing Or Request("b_pmntadd_pin") = ""), DirectCast(Request("b_pmntadd_pin"), String).Trim(), "")
            Dim bride_pmntadd_po As String = If(Not (Request("b_pmntadd_po") Is Nothing Or Request("b_pmntadd_po") = ""), DirectCast(Request("b_pmntadd_po"), String).Trim(), "")
            Dim bride_pmntadd_txtst As String = If(Not (Request("b_pmntadd_txtst") Is Nothing Or Request("b_pmntadd_txtst") = ""), DirectCast(Request("b_pmntadd_txtst"), String).Trim(), "")
            Dim bride_pmntadd_txtdist As String = If(Not (Request("b_pmntadd_txtdist") Is Nothing Or Request("b_pmntadd_txtdist") = ""), DirectCast(Request("b_pmntadd_txtdist"), String).Trim(), "")
            Dim bride_pmntadd_txtblock As String = If(Not (Request("b_pmntadd_txtblock") Is Nothing Or Request("b_pmntadd_txtblock") = ""), DirectCast(Request("b_pmntadd_txtblock"), String).Trim(), "")
            Dim bride_pmntadd_txtsubdiv As String = If(Not (Request("b_pmntadd_txtsubdiv") Is Nothing Or Request("b_pmntadd_txtsubdiv") = ""), DirectCast(Request("b_pmntadd_txtsubdiv"), String).Trim(), "")
            Dim bride_pmntadd_txtps As String = If(Not (Request("b_pmntadd_txtps") Is Nothing Or Request("b_pmntadd_txtps") = ""), DirectCast(Request("b_pmntadd_txtps"), String).Trim(), "")
            Dim bride_pmntadd_txtmunigp As String = If(Not (Request("b_pmntadd_txtmunigp") Is Nothing Or Request("b_pmntadd_txtmunigp") = ""), DirectCast(Request("b_pmntadd_txtmunigp"), String).Trim(), "")
            Dim bride_pmntadd_txtpo As String = If(Not (Request("b_pmntadd_txtpo") Is Nothing Or Request("b_pmntadd_txtpo") = ""), DirectCast(Request("b_pmntadd_txtpo"), String).Trim(), "")

            Dim is_same_address As String = If(Not (Request("same_address") Is Nothing Or Request("same_address") = ""), DirectCast(Request("same_address"), String).Trim(), "")


            If bride_prestadd_city Is Nothing Or bride_prestadd_city = "0" Then
                response("error") = "Please Select Present Address Country."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If bride_prestadd_pin = "" Then
                response("error") = "Please Enter Present Address PIN Code."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If bride_prestadd_location = "16" Then
                If bride_prestadd_st Is Nothing Or bride_prestadd_st = "0" Then
                    response("error") = "Please Select Present Address State."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If bride_prestadd_dist = "" Or bride_prestadd_dist = "0" Then
                    response("error") = "Please Select Present Address District."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If bride_prestadd_subdiv = "" Or bride_prestadd_subdiv = "0" Then
                    response("error") = "Please Select Present Address Sub-Division."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If bride_prestadd_ps = "" Or bride_prestadd_ps = "0" Then
                    response("error") = "Please Select Present Address PS."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If bride_prestadd_po = "" Or bride_prestadd_po = "0" Then
                    response("error") = "Please Select Present Address Post Office."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If bride_prestadd_wa = "02" Then
                    If bride_prestadd_munigp = "" Or bride_prestadd_munigp = "0" Then
                        response("error") = "Please Select Present Address Municipality."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                Else
                    If bride_prestadd_block = "" Or bride_prestadd_block = "0" Then
                        response("error") = "Please Select Present Address Block."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                    If bride_prestadd_vill = "" Or bride_prestadd_vill = "0" Then
                        response("error") = "Please Enter Present Address Village."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                    If bride_prestadd_munigp = "" Or bride_prestadd_munigp = "0" Then
                        response("error") = "Please Select Present Address GP."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                End If
            Else
                If bride_prestadd_location = "17" Then
                    If bride_prestadd_st Is Nothing Or bride_prestadd_st = "0" Then
                        response("error") = "Please Select Present Address State."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                    If bride_prestadd_dist = "" Or bride_prestadd_dist = "0" Then
                        response("error") = "Please Select Present Address District."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                End If
                If bride_prestadd_location = "18" Then
                    If bride_prestadd_txtst = "" Then
                        response("error") = "Please Select Present Address State."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                    If bride_prestadd_txtdist = "" Then
                        response("error") = "Please Select Present Address District."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                End If

                If bride_prestadd_txtsubdiv = "" Then
                    response("error") = "Please Select Present Address Sub-Division."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If bride_prestadd_txtps = "" Then
                    response("error") = "Please Select Present Address PS."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If bride_prestadd_txtpo = "" Then
                    response("error") = "Please Select Present Address Post Office."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If bride_prestadd_wa = "01" Then
                    If bride_prestadd_txtmunigp = "" Then
                        response("error") = "Please Select Present Address Municipality."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                Else
                    If bride_prestadd_txtblock = "" Then
                        response("error") = "Please Select Present Address Block."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                    If bride_prestadd_vill = "" Or bride_prestadd_vill = "0" Then
                        response("error") = "Please Enter Present Address Village."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                    If bride_prestadd_txtmunigp = "" Then
                        response("error") = "Please Select Present Address GP."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                End If
            End If

            'Permanent Address
            If bride_pmntadd_city Is Nothing Or bride_pmntadd_city = "0" Then
                response("error") = "Please Select Permanent Address Country."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If bride_pmntadd_pin = "" Then



                response("error") = "Please Enter Permanent Address PIN Code."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If bride_pmntadd_location = "16" Then
                If bride_pmntadd_st Is Nothing Or bride_pmntadd_st = "0" Then
                    response("error") = "Please Select Permanent Address State."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If bride_pmntadd_dist = "" Or bride_pmntadd_dist = "0" Then
                    response("error") = "Please Select Permanent Address District."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If bride_pmntadd_subdiv = "" Or bride_pmntadd_subdiv = "0" Then
                    response("error") = "Please Select Permanent Address Sub-Division."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If bride_pmntadd_ps = "" Or bride_pmntadd_ps = "0" Then
                    response("error") = "Please Select Permanent Address PS."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If bride_pmntadd_po = "" Or bride_pmntadd_po = "0" Then
                    response("error") = "Please Select Permanent Address Post Office."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If bride_pmntadd_wa = "02" Then
                    If bride_pmntadd_munigp = "" Or bride_pmntadd_munigp = "0" Then
                        response("error") = "Please Select Permanent Address Municipality."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                Else
                    If bride_pmntadd_block = "" Or bride_pmntadd_block = "0" Then
                        response("error") = "Please Select Permanent Address Block."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                    If bride_pmntadd_vill = "" Or bride_pmntadd_vill = "0" Then
                        response("error") = "Please Enter Permanent Address Village."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                    If bride_pmntadd_munigp = "" Or bride_pmntadd_munigp = "0" Then
                        response("error") = "Please Select Permanent Address GP."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                End If
            Else
                If bride_pmntadd_location = "17" Then
                    If bride_pmntadd_st Is Nothing Or bride_pmntadd_st = "0" Then
                        response("error") = "Please Select Permanent Address State."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                    If bride_pmntadd_dist = "" Or bride_pmntadd_dist = "0" Then
                        response("error") = "Please Select Permanent Address District."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                End If
                If bride_pmntadd_location = "18" Then
                    If bride_pmntadd_txtst Is Nothing Or bride_pmntadd_txtst = "" Then
                        response("error") = "Please Select Permanent Address State."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                    If bride_pmntadd_txtdist = "" Then
                        response("error") = "Please Select Permanent Address District."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                End If

                If bride_pmntadd_txtsubdiv = "" Then
                    response("error") = "Please Select Permanent Address Sub-Division."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If bride_pmntadd_txtps = "" Then
                    response("error") = "Please Select Permanent Address PS."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If bride_pmntadd_txtpo = "" Then
                    response("error") = "Please Select Permanent Address Post Office."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If bride_pmntadd_wa = "01" Then
                    If bride_pmntadd_txtmunigp = "" Then


                        response("error") = "Please Select Permanent Address Municipality."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                Else
                    If bride_pmntadd_txtblock = "" Then
                        response("error") = "Please Select Permanent Address Block."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                    If bride_pmntadd_vill = "" Or bride_pmntadd_vill = "0" Then
                        response("error") = "Please Enter Permanent Address Village."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                    If bride_pmntadd_txtmunigp = "" Then
                        response("error") = "Please Select Permanent Address GP."
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                End If
            End If

            Dim obj = New MARREG_ApplyOnline_Webservice_BL()
            Dim msg = obj.SaveBrideAddressDetails(bride_prestadd_location, bride_prestadd_houseNo, bride_prestadd_locality_nm, bride_prestadd_city, bride_prestadd_st, bride_prestadd_dist, bride_prestadd_wa,
                                                  bride_prestadd_subdiv, bride_prestadd_block, bride_prestadd_ps, bride_prestadd_munigp, bride_prestadd_vill, bride_prestadd_txtvill,
                                                  bride_prestadd_pin, bride_prestadd_po, bride_prestadd_txtst, bride_prestadd_txtdist, bride_prestadd_txtblock, bride_prestadd_txtsubdiv, bride_prestadd_txtps, bride_prestadd_txtmunigp, bride_prestadd_txtpo,
                                                  bride_pmntadd_location, bride_pmntadd_houseNo, bride_pmntadd_locality_nm, bride_pmntadd_city, bride_pmntadd_st, bride_pmntadd_dist, bride_pmntadd_wa, bride_pmntadd_subdiv, bride_pmntadd_block, bride_pmntadd_ps,
                                                  bride_pmntadd_munigp, bride_pmntadd_vill, bride_pmntadd_txtvill, bride_pmntadd_pin, bride_pmntadd_po, bride_pmntadd_txtst, bride_pmntadd_txtdist, bride_pmntadd_txtblock, bride_pmntadd_txtsubdiv,
                                                  bride_pmntadd_txtps, bride_pmntadd_txtmunigp, bride_pmntadd_txtpo, is_same_address)

            If (msg = 1) Then
                json("success") = "Details Inserted Successfully "
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
            Else
                json("error") = "Something went wrong"
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
#Region "Save Social Marriage Address Details"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub SaveSocialMarriageAddressDetails()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
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
            Dim Draft_Cd As String = "9413"
            Dim Place_Socl_Mrg_HNo As String = If(Not (Request("socl_mrg_houseNo") Is Nothing Or Request("socl_mrg_houseNo") = ""), DirectCast(Request("socl_mrg_houseNo"), String).Trim(), "")
            Dim Place_Socl_Mrg_Address As String = If(Not (Request("socl_mrg_locality_nm") Is Nothing Or Request("socl_mrg_locality_nm") = ""), DirectCast(Request("socl_mrg_locality_nm"), String).Trim(), "")
            Dim Place_Socl_Mrg_Country As String = If(Not (Request("socl_mrg_city") Is Nothing Or Request("socl_mrg_city") = ""), DirectCast(Request("socl_mrg_city"), String).Trim(), "")
            Dim Place_Socl_Mrg_State As String = If(Not (Request("socl_mrg_st") Is Nothing Or Request("socl_mrg_st") = ""), DirectCast(Request("socl_mrg_st"), String).Trim(), "")
            Dim Place_Socl_Mrg_Dist As String = If(Not (Request("socl_mrg_dist") Is Nothing Or Request("socl_mrg_dist") = ""), DirectCast(Request("socl_mrg_dist"), String).Trim(), "")
            Dim Place_Socl_Mrg_PS As String = If(Not (Request("socl_mrg_ps") Is Nothing Or Request("socl_mrg_ps") = "0"), DirectCast(Request("socl_mrg_ps"), String).Trim(), "")
            Dim Place_Socl_Mrg_txtPS As String = If(Not (Request("socl_mrg_txtps") Is Nothing Or Request("socl_mrg_txtps") = "0"), DirectCast(Request("socl_mrg_txtps"), String).Trim(), "")
            Dim Place_Socl_Mrg_PO As String = If(Not (Request("socl_mrg_po") Is Nothing Or Request("socl_mrg_po") = ""), DirectCast(Request("socl_mrg_po"), String).Trim(), "")
            Dim Place_Socl_Mrg_txtPO As String = If(Not (Request("socl_mrg_txtpo") Is Nothing Or Request("socl_mrg_txtpo") = ""), DirectCast(Request("socl_mrg_txtpo"), String).Trim(), "")
            Dim Place_Socl_Mrg_Pin As String = If(Not (Request("socl_mrg_pin") Is Nothing Or Request("socl_mrg_pin") = ""), DirectCast(Request("socl_mrg_pin"), String).Trim(), "")

            Dim Socl_Mrg_Dt As String = If(Not (Request("socl_mrg_dt") Is Nothing Or Request("socl_mrg_dt") = ""), DirectCast(Request("socl_mrg_dt"), String).Trim(), "")
            'Dim socl_mrg_card As Byte = If(Not (Request("socl_mrg_card") Is Nothing Or Request("socl_mrg_card") = ""), DirectCast(Request("socl_mrg_card"), String).Trim(), "")
            Dim IP_Address As String = Common.getIPAddress()

            Dim Mrg_Invi_Card As Byte = (Request("socl_mrg_card"))
            If (Place_Socl_Mrg_Country = "0" Or Place_Socl_Mrg_Country = "") Then
                response("error") = "Please select Country"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (Place_Socl_Mrg_State = "0" Or Place_Socl_Mrg_State = "") Then
                response("error") = "Please select State"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (Place_Socl_Mrg_Dist = "0" Or Place_Socl_Mrg_Dist = "") Then
                response("error") = "Please select District"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If Place_Socl_Mrg_State = "19" Then
                If (Place_Socl_Mrg_PS = "0" Or Place_Socl_Mrg_PS = "") Then
                    response("error") = "Please select PS"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If (Place_Socl_Mrg_PO = "0" Or Place_Socl_Mrg_PO = "") Then
                    response("error") = "Please select PO"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
            Else
                If (Place_Socl_Mrg_txtPS = "") Then
                    response("error") = "Please select PS"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If (Place_Socl_Mrg_txtPO = "") Then
                    response("error") = "Please select PO"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
            End If
            If (Place_Socl_Mrg_Pin = "") Then
                response("error") = "Please select PIN NO"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (Socl_Mrg_Dt = "") Then
                response("error") = "Please enter Social Marriage Date"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If

            'If (Mrg_Invi_Card <> "") Then
            '    Mrg_Invi_Card = Convert.ToByte(Mrg_Invi_Card)
            'Else

            'End If

            Dim obj = New MARREG_ApplyOnline_Webservice_BL()
            Dim msg = obj.SaveSocialMarriageAddressDetails(Draft_Cd, Place_Socl_Mrg_HNo, Place_Socl_Mrg_Address, Place_Socl_Mrg_Country, Place_Socl_Mrg_State, Place_Socl_Mrg_Dist,
                                                   Place_Socl_Mrg_PS, Place_Socl_Mrg_PO, Socl_Mrg_Dt, Mrg_Invi_Card, IP_Address, Place_Socl_Mrg_Pin, Place_Socl_Mrg_txtPS, Place_Socl_Mrg_txtPO)
            If (msg = 1) Then
                json("success") = "Details Inserted Successfully "
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
            Else
                json("error") = "Something went wrong"
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
#Region "Save Registration Details"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub SaveRegistrationDetails()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
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
            Dim reg_mo_cd As String = If(Not (Request("reg_mo_cd") Is Nothing Or Request("reg_mo_cd") = ""), DirectCast(Request("reg_mo_cd"), String).Trim(), "")
            Dim registration_place As String = If(Not (Request("reg_palce") Is Nothing Or Request("reg_palce") = ""), DirectCast(Request("reg_palce"), String).Trim(), "")
            Dim registration_address As String = If(Not (Request("reg_address") Is Nothing Or Request("reg_address") = ""), DirectCast(Request("reg_address"), String).Trim(), "")
            Dim registration_ctry As String = If(Not (Request("reg_ctry") Is Nothing Or Request("reg_ctry") = ""), DirectCast(Request("reg_ctry"), String).Trim(), "")
            Dim registration_state As String = If(Not (Request("reg_st") Is Nothing Or Request("reg_st") = ""), DirectCast(Request("reg_st"), String).Trim(), "")
            Dim registartion_dist As String = If(Not (Request("reg_dist") Is Nothing Or Request("reg_dist") = "0"), DirectCast(Request("reg_dist"), String).Trim(), "")
            Dim registration_ps As String = If(Not (Request("reg_ps") Is Nothing Or Request("reg_ps") = ""), DirectCast(Request("reg_ps"), String).Trim(), "")
            Dim registration_pin As String = If(Not (Request("reg_pin") Is Nothing Or Request("reg_pin") = ""), DirectCast(Request("reg_pin"), String).Trim(), "")
            Dim registration_po As String = If(Not (Request("reg_po") Is Nothing Or Request("reg_po") = ""), DirectCast(Request("reg_po"), String).Trim(), "")
            Dim proposed_registration_dt As String = If(Not (Request("reg_dt") Is Nothing Or Request("reg_dt") = ""), DirectCast(Request("reg_dt"), String).Trim(), "")
            Dim registartion_time As String = If(Not (Request("reg_time") Is Nothing Or Request("reg_time") = ""), DirectCast(Request("reg_time"), String).Trim(), "")


            If (registartion_dist = "") Then
                response("error") = "Please select District"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (registration_pin = "") Then
                response("error") = "Please enter PIN No"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If

            Dim obj = New MARREG_ApplyOnline_Webservice_BL()
            Dim msg = obj.SaveRegistrationDetails(reg_mo_cd, registration_place, registration_address, registration_ctry,
                                                  registration_state, registartion_dist, registration_ps, registration_pin, registration_po, proposed_registration_dt, registartion_time)
            If (msg = 1) Then
                json("success") = "Details Inserted Successfully "
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
            Else
                json("error") = "Something went wrong"
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
#Region "Document Proof Type"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub LoadDocumentProofType()
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
            Dim type As String = If(Not Request("type") Is Nothing, DirectCast(Request("type"), String).Trim(), "")
            Dim nation As String = If(Not Request("nation") Is Nothing, DirectCast(Request("nation"), String).Trim(), "")
            If (type <> "1" And type <> "2" And type <> "3" And type <> "4") Then
                json("data") = Nothing
            Else
                Dim obj As New WS_MasterData_BL
                Dim data = obj.LoadDocumentProofType(type, nation).Tables(0)
                If (data.Rows.Count > 0) Then
                    json("success") = 1
                End If
                json("data") = DataTableToJSONWithJavaScriptSerializer(data)
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
#Region "File Upload"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub UploadDocProof()
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
            Dim type As String = If(Not Request("type") Is Nothing, DirectCast(Request("type"), String).Trim(), "")
            Dim proof_type As String = If(Not Request("proof_type") Is Nothing, DirectCast(Request("proof_type"), String).Trim(), "")
            Dim doc_no As String = If(Not Request("doc_no") Is Nothing, DirectCast(Request("doc_no"), String).Trim(), "")
            Dim f_draft_cd As String = If(Not Request("draft_cd") Is Nothing, DirectCast(Request("draft_cd"), String).Trim(), "")
            Dim file_doc As HttpPostedFile = HttpContext.Current.Request.Files("file_doc")
            Dim Draft_Applicant_Cd = If(Not Session("Draft_Applicant_Cd") Is Nothing, DirectCast(Session("Draft_Applicant_Cd"), String).Trim(), "")
            Dim draft_cd = If(Not (Session.Item("draft_cd") Is Nothing Or Session.Item("draft_cd") = ""), DirectCast(Session.Item("draft_cd"), String).Trim(), "")
            If (draft_cd <> f_draft_cd) Then
                response("error") = "Bad request"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (type = "" Or type = "0") Then
                response("error") = "Type of document not found"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (type <> "1" And type <> "2" And type <> "3") Then
                'No proof type is required
            Else
                If (proof_type = "" Or proof_type = "0") Then
                    response("error") = "Please choose Proof of Document"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
            End If
            If (doc_no = "") Then
                response("error") = "Please enter ID Proof No"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If file_doc Is Nothing Or Not IsPdf(file_doc) Then
                response("error") = "No valid file specified. Please select a .pdf file to upload."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            ElseIf (file_doc.ContentLength > 300 * 1024) Then       'Maximim file size should be 300 KB
                response("error") = "Maximum 300 KB file allowed to upload."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            Dim sFileName As String = "doc_proof.pdf"
            Select Case type
                Case "1"
                    sFileName = "groom_dob_proof.pdf"
                Case "2"
                    sFileName = "groom_present_addr_proof.pdf"
                Case "3"
                    sFileName = "groom_permanent_addr_proof.pdf"
                Case "5"
                    sFileName = "groom_bpl_proof.pdf"
                Case "6"
                    sFileName = "groom_unmarried_proof.pdf"
                Case "7"
                    sFileName = "groom_Divorce_certifiacte.pdf"
                Case "8"
                    sFileName = "bride_death_certifiacte.pdf"
                Case "9"
                    sFileName = "groom_nullity_proof.pdf"
                Case "10"
                    sFileName = "groom_previous_marriage_cert.pdf"
            End Select
            If (Draft_Applicant_Cd = "") Then
                response("error") = "Bad request"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            Dim dPath = "~/MARREG_Documents/MARREG_Files_BrideGroom_Draft/" + Date.Today.Year.ToString() + "/" + Draft_Applicant_Cd + "/"
            Dim strPath = HostingEnvironment.MapPath(dPath)

            Dim path As String = dPath + sFileName
            Dim obj = New MARREG_ApplyOnline_Webservice_BL()
            Dim infos = New Dictionary(Of String, Object)
            infos("type") = type
            infos("proof_type") = proof_type
            infos("doc_no") = doc_no
            infos("path") = path
            infos("draft_cd") = draft_cd
            Dim ret = obj.UploadDocProof(infos)
            If (ret = "1") Then         'success
                If Not System.IO.Directory.Exists(strPath) Then
                    System.IO.Directory.CreateDirectory(strPath)
                End If
                file_doc.SaveAs(strPath + sFileName)
                json("success") = ret
                json("p") = path
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
            Else
                json("error") = "Something went wrong"
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
#Region "Image Upload"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub UploadImage()
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
            Dim type As String = If(Not Request("type") Is Nothing, DirectCast(Request("type"), String).Trim(), "")
            Dim f_draft_cd As String = If(Not Request("draft_cd") Is Nothing, DirectCast(Request("draft_cd"), String).Trim(), "")
            Dim img_file As HttpPostedFile = HttpContext.Current.Request.Files("img_file")
            Dim Draft_Applicant_Cd = If(Not Session("Draft_Applicant_Cd") Is Nothing, DirectCast(Session("Draft_Applicant_Cd"), String).Trim(), "")
            Dim draft_cd = If(Not (Session.Item("draft_cd") Is Nothing Or Session.Item("draft_cd") = ""), DirectCast(Session.Item("draft_cd"), String).Trim(), "")
            If (draft_cd <> f_draft_cd) Then
                response("error") = "Bad request"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (type = "" Or type = "0") Then
                response("error") = "Please state whether Groom/ Husband image or Bride/ Wife image"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            Dim a = ""
            If (type = "1") Then
                a = "Groom/ Husband"
            ElseIf (type = "2") Then
                a = "Bride/ Wife"
            Else
                response("error") = "Please state whether Groom/ Husband image or Bride/ Wife image"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            Dim inputStream As Stream = Stream.Null
            Dim mStream As MemoryStream
            Dim gimg_byte As Byte()
            If img_file Is Nothing Then
                response("error") = "No valid file specified."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            Else
                Dim fext = System.IO.Path.GetExtension(img_file.FileName).ToLower()
                If fext = ".jpg" Or fext = ".jpeg" Or fext = ".png" Then
                    inputStream = img_file.InputStream
                Else
                    response("error") = "Please select a .JPG, .JPEG or .PNG file to upload."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
                If (img_file.ContentLength > 50 * 1024) Then       'Maximim file size should be 50 KB
                    response("error") = "Maximum 50 KB file allowed to upload."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
            End If
            If Not inputStream.Length = 0 Then
                Using photo As New Drawing.Bitmap(Drawing.Image.FromStream(inputStream))
                    Dim count = Common.FaceDetect(photo)
                    If count <> 0 Then
                        mStream = Common.ResizeImage(photo, 132, 170)
                        gimg_byte = New Byte(mStream.Length) {}
                        gimg_byte = mStream.ToArray()
                    Else
                        response("error") = "Please Upload proper Photograph of " + a
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                        Exit Sub
                    End If
                End Using
            Else
                response("error") = "Please select a .JPG, .JPEG or .PNG file to upload."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (Draft_Applicant_Cd = "") Then
                response("error") = "Bad request"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If

            Dim obj = New MARREG_ApplyOnline_Webservice_BL()
            Dim infos = New Dictionary(Of String, Object)
            infos("type") = type
            infos("img_byte") = gimg_byte
            infos("draft_cd") = draft_cd
            Dim ret = obj.UploadImage(infos)
            If (ret = "1") Then         'success
                json("success") = ret

                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
            Else
                json("error") = "Something went wrong"
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
#Region "Load Groom Documents and Image"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub LoadGroomDocumentsImage()
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
            Dim f_draft_cd As String = If(Not Request("draft_cd") Is Nothing, DirectCast(Request("draft_cd"), String).Trim(), "")
            Dim draft_cd = If(Not (Session.Item("draft_cd") Is Nothing Or Session.Item("draft_cd") = ""), DirectCast(Session.Item("draft_cd"), String).Trim(), "")
            If (draft_cd <> f_draft_cd) Then
                response("error") = "Bad request"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If

            Dim obj As New MARREG_ApplyOnline_Webservice_BL
            Dim sf = New Dictionary(Of String, Object)
            Dim df = New Dictionary(Of String, Dictionary(Of String, Object))
            Dim data = obj.LoadGroomDocumentsImage(draft_cd).Tables(0)
            If (data.Rows.Count > 0) Then
                json("success") = 1
                sf("Groom_Proof_DOB_Type") = Convert.ToString(data.Rows(0)("Groom_Proof_DOB_Type"))
                sf("Groom_IdProofNo_Dob") = Convert.ToString(data.Rows(0)("Groom_IdProofNo_Dob"))
                sf("Groom_Proof_DOB") = Convert.ToString(data.Rows(0)("Groom_Proof_DOB"))
                df("1") = sf
                sf = New Dictionary(Of String, Object)
                sf("Groom_Proof_PrsntAdd_Type") = Convert.ToString(data.Rows(0)("Groom_Proof_PrsntAdd_Type"))
                sf("Groom_IdProofNo_PrsntAdd") = Convert.ToString(data.Rows(0)("Groom_IdProofNo_PrsntAdd"))
                sf("Groom_Proof_PrsntAdd") = Convert.ToString(data.Rows(0)("Groom_Proof_PrsntAdd"))
                df("2") = sf
                sf = New Dictionary(Of String, Object)
                sf("Groom_Proof_PrmntAdd_Type") = Convert.ToString(data.Rows(0)("Groom_Proof_PrmntAdd_Type"))
                sf("Groom_IdProofNo_pmntAdd") = Convert.ToString(data.Rows(0)("Groom_IdProofNo_pmntAdd"))
                sf("Groom_Proof_PrmntAdd") = Convert.ToString(data.Rows(0)("Groom_Proof_PrmntAdd"))
                df("3") = sf
                sf = New Dictionary(Of String, Object)
                sf("gbpl_serial_cd") = Convert.ToString(data.Rows(0)("gbpl_serial_cd"))
                sf("grm_bpl_prf") = Convert.ToString(data.Rows(0)("grm_bpl_prf"))
                df("5") = sf
                sf = New Dictionary(Of String, Object)
                sf("Unmarried_Prf_Doc") = Convert.ToString(data.Rows(0)("Unmarried_Prf_Doc"))
                sf("Unmarried_Prf_Doc_No") = Convert.ToString(data.Rows(0)("Unmarried_Prf_Doc_No"))
                df("6") = sf
                sf = New Dictionary(Of String, Object)
                sf("Groom_Prf_Divorce_Doc") = Convert.ToString(data.Rows(0)("Groom_Prf_Divorce_Doc"))
                sf("Groom_Prf_Divorce_Doc_No") = Convert.ToString(data.Rows(0)("Groom_Prf_Divorce_Doc_No"))
                df("7") = sf
                sf = New Dictionary(Of String, Object)
                sf("Groom_Prf_Wife_Death_Certificate_Doc") = Convert.ToString(data.Rows(0)("Groom_Prf_Wife_Death_Certificate_Doc"))
                sf("Groom_Prf_Wife_Death_Certificate_No") = Convert.ToString(data.Rows(0)("Groom_Prf_Wife_Death_Certificate_No"))
                df("8") = sf
                sf = New Dictionary(Of String, Object)
                sf("Groom_Nullity_Proof") = Convert.ToString(data.Rows(0)("Groom_Nullity_Proof"))
                sf("Groom_IdProofNo_Nullity") = Convert.ToString(data.Rows(0)("Groom_IdProofNo_Nullity"))
                df("9") = sf
                sf = New Dictionary(Of String, Object)
                sf("Groom_Nullity_PreMrg_Proof") = Convert.ToString(data.Rows(0)("Groom_Nullity_PreMrg_Proof"))
                sf("Groom_nullity_prev_app_no") = Convert.ToString(data.Rows(0)("Groom_nullity_prev_app_no"))
                df("10") = sf
                sf = New Dictionary(Of String, Object)
                Dim gp = If(Not data.Rows(0)("Groom_Photo") Is DBNull.Value, Convert.ToBase64String(data.Rows(0)("Groom_Photo")), "")
                sf("Groom_Photo") = gp
                df("p") = sf
            End If
            'json("data") = DataTableToJSONWithJavaScriptSerializer(data)
            json("data") = df
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
#Region "Child Details Upload"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub UploadChildDetails()
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
            Dim type As String = If(Not Request("type") Is Nothing, DirectCast(Request("type"), String).Trim(), "")
            Dim c_name As String = If(Not Request("c_name") Is Nothing, DirectCast(Request("c_name"), String).Trim(), "")
            Dim c_gender As String = If(Not Request("c_gender") Is Nothing, DirectCast(Request("c_gender"), String).Trim(), "")
            Dim c_dob As String = If(Not Request("c_dob") Is Nothing, DirectCast(Request("c_dob"), String).Trim(), "")
            Dim proof_type As String = If(Not Request("proof_type") Is Nothing, DirectCast(Request("proof_type"), String).Trim(), "")
            Dim child_others As String = If(Not Request("child_others") Is Nothing, DirectCast(Request("child_others"), String).Trim(), "")
            Dim doc_no As String = If(Not Request("doc_no") Is Nothing, DirectCast(Request("doc_no"), String).Trim(), "")
            Dim f_draft_cd As String = If(Not Request("draft_cd") Is Nothing, DirectCast(Request("draft_cd"), String).Trim(), "")
            Dim r As String = If(Not Request("r") Is Nothing, DirectCast(Request("r"), String).Trim(), "")
            Dim file_doc As HttpPostedFile = HttpContext.Current.Request.Files("file_doc")
            Dim Draft_Applicant_Cd = If(Not Session("Draft_Applicant_Cd") Is Nothing, DirectCast(Session("Draft_Applicant_Cd"), String).Trim(), "")
            Dim draft_cd = If(Not (Session.Item("draft_cd") Is Nothing Or Session.Item("draft_cd") = ""), DirectCast(Session.Item("draft_cd"), String).Trim(), "")
            If (draft_cd <> f_draft_cd) Then
                response("error") = "Bad request"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (type = "" Or type = "0") Then
                response("error") = "Type of document not found"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (c_name = "") Then
                response("error") = "Please enter Name of Child"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (c_gender = "") Then
                response("error") = "Please choose Child Gender"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (c_dob = "") Then
                response("error") = "Please enter DOB of Child"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (proof_type = "" Or proof_type = "0") Then
                response("error") = "Please choose Proof of DOB"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            ElseIf (proof_type = "04" And child_others = "") Then
                response("error") = "Please enter Other Proof of DOB"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (doc_no = "") Then
                response("error") = "Please enter Document Proof No"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If file_doc Is Nothing Or Not IsPdf(file_doc) Then
                response("error") = "No valid file specified. Please select a .pdf file to upload."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            ElseIf (file_doc.ContentLength > 300 * 1024) Then       'Maximim file size should be 300 KB
                response("error") = "Maximum 300 KB file allowed to upload."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (r = "") Then
                response("error") = "Please refresh the page and try again"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            Dim sFileName As String = "child" + (Convert.ToInt32(r) + 1).ToString() + ".pdf"
            If (Draft_Applicant_Cd = "") Then
                response("error") = "Bad request"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            Dim dPath = "~/MARREG_Documents/MARREG_Files_Child_Draft/" + Date.Today.Year.ToString() + "/" + Draft_Applicant_Cd + "/"
            Dim strPath = HostingEnvironment.MapPath(dPath)

            Dim path As String = dPath + sFileName
            Dim obj = New MARREG_ApplyOnline_Webservice_BL()
            Dim infos = New Dictionary(Of String, Object)
            infos("type") = type
            infos("c_name") = c_name
            infos("c_gender") = c_gender
            infos("c_dob") = c_dob
            infos("proof_type") = proof_type
            infos("child_others") = child_others
            infos("doc_no") = doc_no
            infos("path") = path
            infos("draft_cd") = draft_cd
            Dim ret = obj.UploadChildDetails(infos)
            If (Convert.ToInt64(ret) > 0) Then         'success
                If Not System.IO.Directory.Exists(strPath) Then
                    System.IO.Directory.CreateDirectory(strPath)
                End If
                file_doc.SaveAs(strPath + sFileName)
                json("success") = 1
                json("cid") = ret
                json("p") = path
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
            Else
                json("error") = "Something went wrong"
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
    Public Sub ChildDetailsList()
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
            Dim draw As String = If(Not Request("draw") Is Nothing, DirectCast(Request("draw"), String), "1")
            Dim start As String = If(Not Request("start") Is Nothing, DirectCast(Request("start"), String), "0")
            Dim length As String = If(Not Request("length") Is Nothing, DirectCast(Request("length"), String), "10")
            Dim value As String = If(Not Request("search[value]") Is Nothing, DirectCast(Request("search[value]"), String).Trim(), "")
            Dim orderByDir As String = If(Request("order[0][dir]") Is Nothing, "", DirectCast(Request("order[0][dir]"), String))
            Dim order_by As String = "Child_Cd"

            Dim f_draft_cd As String = If(Not Request("draft_cd") Is Nothing, DirectCast(Request("draft_cd"), String).Trim(), "")
            Dim draft_cd = If(Not (Session.Item("draft_cd") Is Nothing Or Session.Item("draft_cd") = ""), DirectCast(Session.Item("draft_cd"), String).Trim(), "")
            If (draft_cd <> f_draft_cd) Then
                response("error") = "Bad request"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If

            Dim obj As New MARREG_ApplyOnline_Webservice_BL
            myDataTable = obj.ChildDetailsList(draw, start, length, value, order_by, orderByDir, draft_cd)

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
    Public Sub ChildDetailsDelete()
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
            Dim ccd As String = If(Not Request("ccd") Is Nothing, DirectCast(Request("ccd"), String).Trim(), "")
            Dim f_draft_cd As String = If(Not Request("dcd") Is Nothing, DirectCast(Request("dcd"), String).Trim(), "")
            Dim draft_cd = If(Not (Session.Item("draft_cd") Is Nothing Or Session.Item("draft_cd") = ""), DirectCast(Session.Item("draft_cd"), String).Trim(), "")
            If (draft_cd <> f_draft_cd) Then
                response("error") = "Bad request"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (ccd = "" Or ccd = "0") Then
                response("error") = "Bad request"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            'Dim dPath = "~/MARREG_Documents/MARREG_Files_Child_Draft/" + Date.Today.Year.ToString() + "/" + Draft_Applicant_Cd + "/"
            'Dim strPath = HostingEnvironment.MapPath(dPath)

            'Dim path As String = dPath + sFileName
            'Dim obj = New MARREG_ApplyOnline_Webservice_BL()
            'Dim infos = New Dictionary(Of String, Object)
            'infos("type") = type
            'infos("c_name") = c_name
            'infos("c_gender") = c_gender
            'infos("c_dob") = c_dob
            'infos("proof_type") = proof_type
            'infos("child_others") = child_others
            'infos("doc_no") = doc_no
            'infos("path") = path
            'infos("draft_cd") = draft_cd
            'Dim ret = obj.UploadChildDetails(infos)
            'If (Convert.ToInt64(ret) > 0) Then         'success
            '    If Not System.IO.Directory.Exists(strPath) Then
            '        System.IO.Directory.CreateDirectory(strPath)
            '    End If
            '    file_doc.SaveAs(strPath + sFileName)
            '    json("success") = 1
            '    json("cid") = ret
            '    json("p") = path
            '    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
            'Else
            '    json("error") = "Something went wrong"
            '    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
            'End If
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