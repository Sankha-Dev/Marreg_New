Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Web.Script.Services
Imports System.Web.Script.Serialization
Imports MARREG.App_Code
Imports MARREG.App_Code.DataAccessLayer
Imports System.IO

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class CertifiedNotice_WebService
    Inherits System.Web.Services.WebService
    Dim a As New Marreg_Class
    Dim enc As New EncryptionDecryption.EncryptionDecryption()
#Region "Email Validation"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub CheckEmailExist()
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
            email = a.sqlInj_in(email)
            Dim obj As New CertifiedNotice_BL
            Dim App_Auto_Cd = obj.CheckEmailExist(email)
            If App_Auto_Cd.Tables(0).Rows.Count < 1 Then
                Session("App_Auto_Cd_Arr") = Nothing
                response("error") = "Email Id not registered. Please click <a href='" + Common.baseUrl + "MARREG_CertifiedCopy_Notice/MARREG_CertifiedCopyNoticeOutsider.aspx'>here</a> for alternate process"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            response("success") = App_Auto_Cd.Tables(0).Rows.Count
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
            Session("App_Auto_Cd_Arr") = App_Auto_Cd
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
    Public Sub CertifiedCopyRequestInfoValidateOldNotice()
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
            Dim bride_name As String = If(Not Request("bride_name") Is Nothing, DirectCast(Request("bride_name"), String).Trim(), "")
            Dim groom_father As String = If(Not Request("groom_father") Is Nothing, DirectCast(Request("groom_father"), String).Trim(), "")
            Dim bride_father As String = If(Not Request("bride_father") Is Nothing, DirectCast(Request("bride_father"), String).Trim(), "")
            Dim reg_date As String = If(Not Request("reg_date") Is Nothing, DirectCast(Request("reg_date"), String).Trim(), "")
            Dim groom_dob As String = If(Not Request("groom_dob") Is Nothing, DirectCast(Request("groom_dob"), String).Trim(), "")
            Dim bride_dob As String = If(Not Request("bride_dob") Is Nothing, DirectCast(Request("bride_dob"), String).Trim(), "")
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
            If groom_dob = "" And bride_dob = "" Then
                response("error") = "Please enter either Groom/ Husband DOB or Bride/ Wife DOB."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            'If reg_date = "" Or reg_date Is Nothing Then
            '    response("error") = "Please enter Date of Registration."
            '    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
            '    Exit Sub
            'End If
            If mo_name = "" Or mo_name Is Nothing Then
                response("error") = "Please enter MO Name."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            'If ActCd = "" Or ActCd Is Nothing Or ActCd = "0" Then
            '    response("error") = "Please select Act/ Section under which the Marriage was Solemnized/ Registered."
            '    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
            '    Exit Sub
            'End If

            Dim obj As New CertifiedNotice_BL
            Dim rets = obj.CertifiedCopyRequestInfoValidateOldNotice(groom_name, bride_name, groom_father, bride_father, groom_dob, bride_dob, reg_date, mo_name, ActCd)
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
    Public Sub CertifiedCopyRequestEmailValidateOldNotice()
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
            Dim obj As New CertifiedNotice_BL
            Dim rets = obj.CertifiedCopyRequestSaveEmailOldNotice(name, address, mob_no, email, validatedId)
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
                Dim params As New Dictionary(Of String, Object)
                params = CreateAssocArray(oldStr, newStr)
                Dim asyncEml As New AsyncMailMessage_DB()
                asyncEml.AsyncMailMessage_Bytes(email_ids, subject, body, Nothing, params)

                Dim email_stat = "0"
                If (email_stat = "0") Then
                    'json("success") = "OTP has been sent successfully to your Email. Please verify the OTP."
                    json("success") = "OTP has been sent successfully to your Email. Please verify the OTP." + rets
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
    Public Sub CertifiedCopyRequestEmailOTPValidateOldNotice()
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
            Dim obj As New CertifiedNotice_BL
            Dim rets = obj.CertifiedCopyRequestEmailOTPValidateOldNotice(validatedId, otp)
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
    Public Sub CertifiedCopyRequestSendOldNotice()
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

            Dim obj As New CertifiedNotice_BL
            Dim rets = obj.CertifiedCopyRequestSendOldNotice(vId, grn_id, no_copy)
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
                'Send_Email_Bytes(r_email_ids, subject, body, ack_bytes)
                Dim asyncEml As New AsyncMailMessage_DB()
                asyncEml.AsyncMailMessage_Bytes(r_email_ids, subject, body, ack_bytes, params)
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
#End Region
#Region "Email Send"
    Dim eml As New BusinessLogicLayer.Email_Sms_BL()
    Private Function Send_Email_Bytes(email_ids As List(Of String), subject As String, body As String, Optional filebytes As List(Of Byte()) = Nothing) As String
        Dim MS As String
        'MS = SendMailMessage_Bytes(email_ids, subject, body, filebytes)
        MS = eml.send_email_multiple_bytes(email_ids, subject, body, filebytes)
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
#End Region
End Class