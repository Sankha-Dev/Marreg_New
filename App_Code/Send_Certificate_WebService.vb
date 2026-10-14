Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Web.Script.Services
Imports System.Web.Script.Serialization
Imports System.Data
Imports MARREG.App_Code.BusinessLogicLayer
Imports System.IO

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class Send_Certificate_WebService
    Inherits System.Web.Services.WebService
    Dim eml As New Email_Sms_BL()

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

            Dim m As New MasterData
            Dim res = m.Save_Certificate(file_byte, app_Auto_Cd)
            If res = "1" Then
                Dim ret = GenerateAndSendEmailCert(file_byte, app_Auto_Cd)
                'Dim ret = 0
                If (ret = "0") Then
                    response("success") = "Certificate generated and email sent successfully."
                Else
                    response("success") = "Certificate generated and email could not be sent."
                End If
            Else
                response("error") = "Please generate the Certificate once again."
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
#Region "Send Certificate as Attachment"
    Private Function FecthBrideGroomDetails(App_Auto_Cd As String) As DataRow
        Dim obj As New MasterData
        Dim dt = obj.FecthBrideGroomDetails(App_Auto_Cd)
        Return dt.Rows(0)
    End Function
    Private Function GenerateAndSendEmailCert(bytes As Byte(), app_Auto_Cd As String) As String
        Dim data = FecthBrideGroomDetails(Convert.ToString(app_Auto_Cd))
        Dim r_email_ids As New List(Of String)
        'Dim email = "shaibaldas15@gmail.com"
        'r_email_ids.Add(email)
        'r_email_ids.Add("destiny.utsab@gmail.com")
        r_email_ids.Add(data("bride_email_address"))
        r_email_ids.Add(data("groom_email_address"))
        If (r_email_ids.Count > 0) Then
            Dim ack_bytes As New List(Of Byte())
            ack_bytes.Add(bytes)
            Dim email_Body = "<b>DO NOT REPLY TO THIS E-MAIL.USE THE E-MAIL OF MARRIAGE REGISTRAR MENTIONED BELOW </b><br/>" &
                            "<br/>Dear Mrs. " & data("bride_full_name") & "<br/>" &
                            "<br/>Dear Mr.  " & data("groom_full_name") & "<br/>" &
                            "<br/><br/><b>SUBJECT:</b> Marriage Certificate<br/>" &
                            "<br/><b>CONGRATULATIONS!!</b>" &
                            "<br>Your Marriage Certificate is digitally signed by the " & data("mo_full_name") & " which is attached herewith. Please download and open by entering password. " &
                            "Your password will be the date of Solemnization/ Registration like 23APR2023 if your date of Solemnization/ Registration is 23/04/2023." &
                            "You will also get two copies of Marriage Certificates from the respective Marriage Officer without any additional cost." &
                            "<br><br>With Best Wishes.<br/><br/>REGISTRAR GENERAL OF MARRIAGES <br/>LAW DEPARTMENT <br/>GOVERNMENT OF WEST BENGAL"
            Dim ret = Send_Email_Bytes(r_email_ids, "Marriage Certificate", email_Body, ack_bytes)
            Return ret
        Else
            Return ""
        End If

    End Function

    Private Function Send_Email_Bytes(email_ids As List(Of String), subject As String, body As String, Optional filebytes As List(Of Byte()) = Nothing) As String
        Dim MS As String
        'MS = SendMailMessage_Bytes(email_ids, subject, body, filebytes)
        MS = eml.send_email_multiple_bytes(email_ids, subject, body, filebytes)
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
                mMailMessage.Attachments.Add(New Net.Mail.Attachment(New MemoryStream(fle.Item(x)), "certificate.pdf"))
                'End If
            Next
        End If

        mMailMessage.IsBodyHtml = True
        mMailMessage.Priority = Net.Mail.MailPriority.Normal
        Dim mSmtpClient As New Net.Mail.SmtpClient() With {
            .EnableSsl = True,
            .Host = "smtp.gmail.com",
            .Port = 587,
        .Credentials = New Net.NetworkCredential("utsab.87@gmail.com", "ywlbonpgwrjxbhyb")
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
#Region "Print Reprint Status"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub print_reprint_status()
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

            Dim app_Auto_Cd As String = If(Not Request("app_Auto_Cd") Is Nothing, DirectCast(Request("app_Auto_Cd"), String).Trim(), "")

            If (app_Auto_Cd = "") Then
                response("error") = "Application No not found."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If

            Dim m As New MasterData
            Dim res = m.MO_print_reprint_status("N", "N", app_Auto_Cd)
            If res = "1" Then
                response("success") = "1"
            Else
                response("error") = "0"
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