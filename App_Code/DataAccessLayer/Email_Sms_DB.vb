Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Collections.Generic
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Net.Mail
Imports System.Net
Imports CDONTS
Imports System.IO
Imports System.Web
Imports System.Web.Security


Imports System.Security.Cryptography.X509Certificates
Imports System.Net.Security

Imports System.Xml


Namespace MARREG.App_Code.DataAccessLayer
    Public Class Email_Sms_DB

#Region "Send Email"
        Public Function send_email(email_id As String, email_sub As String, email_body As String) As String
            Dim outmsg As String
            Dim message As New System.Net.Mail.MailMessage() ' MailMessage 
            '' Dim attach As New System.Net.Mail.Attachment(path)
            ''   message.Attachments.Add(attach)
            Try
                Dim StrMsg As String
                'Dim sb As New StringBuilder
                'sb.AppendLine(email_body)
                'sb.AppendLine("This is a system generated e-mail. Please do not reply.")

                'StrMsg = sb.ToString()

                StrMsg = email_body.ToString()
                'StrMsg = StrMsg + "This is a system generated e-mail. Please do not reply." + Environment.NewLine
                Dim usermessage As String = StrMsg
                'Dim usermessage As String = email_body.ToString()
                Dim senderemailaddress As String = email_id.ToString()
                Dim sendersubject As String = email_sub.ToString()
                message.From = New MailAddress("support.rgm-wb@gov.in")
                message.[To].Add(senderemailaddress)
                message.Subject = sendersubject
                'message.CC.Add(senderemailaddress)
                message.IsBodyHtml = True
                message.Body = usermessage
                'Define the SMTP client host and port number that supports SSL connections.
                Dim mailClient As New System.Net.Mail.SmtpClient("smtp.mail.gov.in", "25")
                'Define your SMTP email server username and password.
                'Dim userCredential As New System.Net.NetworkCredential("support.rgm-wb@gov.in", "*Rm96Qp@H")
                'Dim userCredential As New System.Net.NetworkCredential("support.rgm-wb@gov.in")
                'Define SMTP server host address
                mailClient.Host = "relay.nic.in"
                'Do not use default credentials because authentication to the SMTP server will be using mailClient.Credentials property which is
                'set to use your own username and password
                mailClient.UseDefaultCredentials = False
                'Assign your username and password (stored in userCredential VB variable) to mailclient.Credentials property
                'mailClient.Credentials = userCredential
                'Turn on SSL encryption for the email communication.
                mailClient.EnableSsl = False
                'Finally send the message.
                Try
                    mailClient.Send(message)
                    outmsg = "0"
                    'ScriptManager.RegisterClientScriptBlock(Me, Me.[GetType](), "JS", "alert('Mail sent successfully');", True)
                Catch ex As Exception
                    'Response.Write("Send failure: " & ex.ToString())
                    outmsg = "1"
                End Try
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region
#Region "Send Email with Attachment"
        Public Function send_email_attachment(email_id As String, email_sub As String, email_body As String, filepath As String) As String
            Dim outmsg As String
            Dim message As New System.Net.Mail.MailMessage() ' MailMessage             
            Try
                Dim StrMsg As String
                StrMsg = email_body.ToString()
                Dim usermessage As String = StrMsg
                Dim senderemailaddress As String = email_id.ToString()
                Dim sendersubject As String = email_sub.ToString()
                'message.From = New MailAddress("support.rgm-wb@gov.in")
                message.From = New MailAddress("noreply.rgmwb@gov.in")
                message.[To].Add(senderemailaddress)
                message.[To].Add("support.rgm-wb@gov.in")
                message.Subject = sendersubject
                message.IsBodyHtml = True
                message.Body = usermessage
                filepath = HttpContext.Current.Request.MapPath(filepath)
                message.Attachments.Add(New System.Net.Mail.Attachment(filepath)) '"c:\pdftoattach.pdf"
                Dim mailClient As New System.Net.Mail.SmtpClient("smtp.mail.gov.in", "25")
                mailClient.Host = "relay.nic.in"
                mailClient.UseDefaultCredentials = False
                mailClient.EnableSsl = False
                Try
                    mailClient.Send(message)
                    outmsg = "0"
                Catch ex As Exception
                    outmsg = "1"
                End Try
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region
#Region "Send Email MO"
        Public Function send_email_attachment_mo(email_id As String, email_sub As String, email_body As String, filepath As String, sender_email_id As String) As String
            Dim outmsg As String
            Dim message As New System.Net.Mail.MailMessage() ' MailMessage             
            Try
                Dim StrMsg As String
                StrMsg = email_body.ToString()
                Dim usermessage As String = StrMsg
                Dim senderemailaddress As String = email_id.ToString()
                Dim sendersubject As String = email_sub.ToString()
                Dim sendermoemailaddress = "support.rgm-wb@gov.in"
                message.From = New MailAddress(sendermoemailaddress)
                message.[To].Add(senderemailaddress)
                message.Subject = sendersubject
                message.IsBodyHtml = True
                message.Body = usermessage
                filepath = HttpContext.Current.Request.MapPath(filepath)
                message.Attachments.Add(New System.Net.Mail.Attachment(filepath)) '"c:\pdftoattach.pdf"
                Dim mailClient As New System.Net.Mail.SmtpClient("smtp.mail.gov.in", "25")
                mailClient.Host = "relay.nic.in"
                mailClient.UseDefaultCredentials = False
                mailClient.EnableSsl = False
                Try
                    mailClient.Send(message)
                    outmsg = "0"
                Catch ex As Exception
                    outmsg = "1"
                End Try
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region

#Region "Send Email with Attachment test"
        Public Function send_email_attachment_new(email_id As String, email_sub As String, email_body As String) As String
            ServicePointManager.ServerCertificateValidationCallback = AddressOf TrustAllCertificatesCallback
            Dim outmsg As String
            Dim message As New System.Net.Mail.MailMessage() ' MailMessage 
            '' Dim attach As New System.Net.Mail.Attachment(path)
            ''   message.Attachments.Add(attach)
            Try
                Dim StrMsg As String
                'Dim sb As New StringBuilder
                'sb.AppendLine(email_body)
                'sb.AppendLine("This is a system generated e-mail. Please do not reply.")

                'StrMsg = sb.ToString()

                StrMsg = email_body.ToString()
                'StrMsg = StrMsg + "This is a system generated e-mail. Please do not reply." + Environment.NewLine
                Dim usermessage As String = StrMsg
                'Dim usermessage As String = email_body.ToString()
                Dim senderemailaddress As String = email_id.ToString()
                Dim sendersubject As String = email_sub.ToString()
                message.From = New MailAddress("noreply.rgmwb@gov.in")
                message.[To].Add(senderemailaddress)
                message.[To].Add("nicanaydey@gmail.com")
                message.Subject = sendersubject
                'message.CC.Add(senderemailaddress)
                message.IsBodyHtml = True
                message.Body = usermessage
                'Define the SMTP client host and port number that supports SSL connections.
                Dim mailClient As New System.Net.Mail.SmtpClient("smtp.mail.gov.in", "25")
                'Define your SMTP email server username and password.
                'Dim userCredential As New System.Net.NetworkCredential("support.rgm-wb@gov.in", "*Rm96Qp@H")
                'Dim userCredential As New System.Net.NetworkCredential("support.rgm-wb@gov.in")
                'Define SMTP server host address
                mailClient.Host = "relay.nic.in"
                'Do not use default credentials because authentication to the SMTP server will be using mailClient.Credentials property which is
                'set to use your own username and password
                mailClient.UseDefaultCredentials = False
                'Assign your username and password (stored in userCredential VB variable) to mailclient.Credentials property
                'mailClient.Credentials = userCredential
                'Turn on SSL encryption for the email communication.
                mailClient.EnableSsl = False
                'Finally send the message.
                Try
                    mailClient.Send(message)
                    outmsg = "0"
                    'ScriptManager.RegisterClientScriptBlock(Me, Me.[GetType](), "JS", "alert('Mail sent successfully');", True)
                Catch ex As Exception
                    'Response.Write("Send failure: " & ex.ToString())
                    outmsg = "1"
                End Try
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region

#Region "New Function For SMS"
        Public Function TrustAllCertificatesCallback(ByVal sender As Object, ByVal cert As X509Certificate, ByVal chain As X509Chain, ByVal errors As SslPolicyErrors) As Boolean
            ServicePointManager.Expect100Continue = True
            '' ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls

            Return True
        End Function
#End Region

#Region "Send SMS"
        Public Function send_sms(mnumber As String, message As String) As String

            ServicePointManager.ServerCertificateValidationCallback = AddressOf TrustAllCertificatesCallback
            'Dim msg, uname, pin, mobile, senderid, lurl As String
            'msg = message
            'mobile = mnumber
            'uname = ConfigurationManager.AppSettings("sms_uname").ToString()
            'pin = ConfigurationManager.AppSettings("sms_pwd").ToString()
            'senderid = ConfigurationManager.AppSettings("sms_senderid").ToString()
            'lurl = "https://smsgw.sms.gov.in/failsafe/HttpLink?username=" & uname & "&pin=" & pin & "&message=" & message & "&mnumber=" & mobile & "&signature=" & senderid
            'Try
            '    Dim Request As HttpWebRequest = WebRequest.Create(lurl)

            '    Dim Response As HttpWebResponse = Request.GetResponse()
            '    Dim reader As New StreamReader(Response.GetResponseStream())

            '    Dim tmp As String = reader.ReadToEnd()
            '    Response.Close()

            '    Return "0"
            'Catch ex As Exception
            '    Return "1"
            'End Try
            Dim msg, uname, pin, mobile, senderid, lurl, dtl_entry_id As String
            msg = message
            mobile = mnumber
            uname = ConfigurationManager.AppSettings("sms_uname").ToString()
            pin = ConfigurationManager.AppSettings("sms_pwd").ToString()
            senderid = ConfigurationManager.AppSettings("sms_senderid").ToString()
            dtl_entry_id = ConfigurationManager.AppSettings("dlt_entity_id").ToString
            lurl = "https://smsgw.sms.gov.in/failsafe/HttpLink?username=" & uname & "&pin=" & pin & "&message=" & message & "&mnumber=" & mobile & "&signature=" & senderid & "&dlt_entity_id=" & dtl_entry_id
            'lurl = "https://smsgw.sms.gov.in/failsafe/HttpLink?username=" & uname & "&pin=" & pin & "&message=" & message & "&mnumber=" & mobile & "&signature=" & senderid
            Dim textboxError As String
            Try
                Dim myReq As HttpWebRequest = CType(WebRequest.Create(lurl), HttpWebRequest)
                Dim myResp As HttpWebResponse = CType(myReq.GetResponse(), HttpWebResponse)
                Dim respStreamReader As System.IO.StreamReader = New System.IO.StreamReader(myResp.GetResponseStream())
                Dim responseString As String = respStreamReader.ReadToEnd()
                respStreamReader.Close()
                myResp.Close()
                textboxError = responseString
                'textboxError.Visible = True
                Return textboxError
            Catch __unusedException1__ As Exception
                'textboxError = "The SMS Gateway Server is not running!"
                'textboxError.Visible = True
                'Return textboxError
                textboxError = __unusedException1__.Message()
            End Try
            Return textboxError
        End Function

        
#End Region

#Region "Send SMS NEW TEMPLATE"
        Public Function send_sms_new_template(mnumber As String, message As String, template_id As String) As String

            ServicePointManager.ServerCertificateValidationCallback = AddressOf TrustAllCertificatesCallback
            'Added on 07-03-2024 for customization of sms sending option
            Dim sms_flag = Convert.ToString(ConfigurationManager.AppSettings.Item("send_sms"))
            If (sms_flag = "0") Then
                Return sms_flag
            End If
            'End Added on 07-03-2024 for customization of sms sending option
            Dim msg, uname, pin, mobile, senderid, lurl, dtl_entry_id, dlt_template_id As String
            msg = message
            mobile = mnumber
            dlt_template_id = template_id
            uname = ConfigurationManager.AppSettings("sms_uname").ToString()
            pin = ConfigurationManager.AppSettings("sms_pwd").ToString()
            senderid = ConfigurationManager.AppSettings("sms_senderid").ToString()
            dtl_entry_id = ConfigurationManager.AppSettings("dlt_entity_id").ToString
            lurl = "https://smsgw.sms.gov.in/failsafe/HttpLink?username=" & uname & "&pin=" & pin & "&message=" & message & "&mnumber=" & mobile & "&signature=" & senderid & "&dlt_entity_id=" & dtl_entry_id & "&dlt_template_id=" & dlt_template_id
            'lurl = "https://smsgw.sms.gov.in/failsafe/HttpLink?username=" & uname & "&pin=" & pin & "&message=" & message & "&mnumber=" & mobile & "&signature=" & senderid & "&dlt_entity_id=" & dtl_entry_id
            'lurl = "https://smsgw.sms.gov.in/failsafe/HttpLink?username=" & uname & "&pin=" & pin & "&message=" & message & "&mnumber=" & mobile & "&signature=" & senderid
            Dim textboxError As String

            Try
                Dim myReq As HttpWebRequest = CType(WebRequest.Create(lurl), HttpWebRequest)
                Dim myResp As HttpWebResponse = CType(myReq.GetResponse(), HttpWebResponse)
                Dim respStreamReader As System.IO.StreamReader = New System.IO.StreamReader(myResp.GetResponseStream())
                Dim responseString As String = respStreamReader.ReadToEnd()
                respStreamReader.Close()
                myResp.Close()
                textboxError = responseString
                'textboxError.Visible = True
                Return textboxError
            Catch __unusedException1__ As Exception
                'textboxError = "The SMS Gateway Server is not running!"
                'textboxError.Visible = True
                'Return textboxError
                textboxError = __unusedException1__.Message()
            End Try
            Return textboxError
        End Function


#End Region

#Region "Send Email to Multiple Receipient with Multiple Attachment"
        
        Public Function send_email_multiple(ByVal email_ids As List(Of String), ByVal email_sub As String, ByVal email_body As String, Optional filepaths As List(Of String) = Nothing) As String
            Dim outmsg As String
            Dim message As New System.Net.Mail.MailMessage() ' MailMessage
            'ServicePointManager.ServerCertificateValidationCallback += (s, ce, ca, p) => true
            ServicePointManager.ServerCertificateValidationCallback = New System.Net.Security.RemoteCertificateValidationCallback(AddressOf AcceptAllCertifications)
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            Try
                Dim StrMsg As String
                StrMsg = email_body.ToString()
                Dim usermessage As String = StrMsg
                Dim sendersubject As String = email_sub.ToString()
                message.From = New MailAddress("noreply.rgmwb@gov.in")
                For Each r As String In email_ids
                    message.To.Add(New Net.Mail.MailAddress(r))
                Next
                message.To.Add(New Net.Mail.MailAddress("support.rgm-wb@gov.in"))
                message.Subject = sendersubject
                message.IsBodyHtml = True
                message.Body = usermessage
                If filepaths IsNot Nothing Then
                    For x As Integer = 0 To filepaths.Count - 1
                        If Not String.IsNullOrEmpty(filepaths.Item(x)) Then
                            Dim path = HttpContext.Current.Request.MapPath(filepaths.Item(x))
                            message.Attachments.Add(New Net.Mail.Attachment(path))
                        End If
                    Next
                End If

                Dim mailClient As New Net.Mail.SmtpClient() With {
                    .EnableSsl = False,
                    .Host = "relay.nic.in",
                    .Port = 25,
                    .UseDefaultCredentials = False
                }

                Try
                    mailClient.Send(message)
                    outmsg = "0"
                Catch ex As Exception
                    outmsg = "1"
                End Try
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
        Private Shared Function AcceptAllCertifications(sender As Object, certification As System.Security.Cryptography.X509Certificates.X509Certificate, chain As System.Security.Cryptography.X509Certificates.X509Chain, sslPolicyErrors As System.Net.Security.SslPolicyErrors) As Boolean
            Return True
        End Function
        Public Function send_email_multiple_bytes(ByVal email_ids As List(Of String), ByVal email_sub As String, ByVal email_body As String, Optional filebytes As List(Of Byte()) = Nothing) As String
            Dim outmsg As String
            Dim message As New System.Net.Mail.MailMessage() ' MailMessage
            Try
                Dim StrMsg As String
                StrMsg = email_body.ToString()
                Dim usermessage As String = StrMsg
                Dim sendersubject As String = email_sub.ToString()
                message.From = New MailAddress("noreply.rgmwb@gov.in")
                For Each r As String In email_ids
                    message.To.Add(New Net.Mail.MailAddress(r))
                Next
                If (sendersubject.ToLower() <> Convert.ToString("Marriage Certificate").ToLower()) Then
                    message.To.Add("support.rgm-wb@gov.in")
                End If
                message.Subject = sendersubject
                message.IsBodyHtml = True
                message.Body = usermessage
                If filebytes IsNot Nothing Then
                    For x As Integer = 0 To filebytes.Count - 1
                        If Not filebytes.Item(x).Length = 0 Then
                            message.Attachments.Add(New Net.Mail.Attachment(New MemoryStream(filebytes.Item(x)), Replace(email_sub, " ", "_") + "_" + DateTime.Now.ToFileTime().ToString() + ".pdf"))
                        End If
                    Next
                End If

                Dim mailClient As New Net.Mail.SmtpClient() With {
                    .EnableSsl = False,
                    .Host = "relay.nic.in",
                    .Port = 25,
                    .UseDefaultCredentials = False
                }

                Try
                    mailClient.Send(message)
                    outmsg = "0"
                Catch ex As Exception
                    outmsg = "1"
                End Try
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region
    End Class
End Namespace
