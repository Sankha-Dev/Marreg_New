Imports System.IO
Imports System.Net
Imports System.Net.Mail
Imports System.Web.Hosting
Imports System.Web.Script.Serialization
Imports Microsoft.VisualBasic
Imports MARREG.App_Code.DataAccessLayer     'Added on 23-04-2024
Imports System.Data.SqlClient               'Added on 23-04-2024
Imports System.Data                         'Added on 23-04-2024

Public Class AsyncMailMessage_DB
#Region "DECLARATION"
    Public dbconnctn As New DataBaseHelperDB()                          'Added on 23-04-2024
    Private con As New SqlConnection(dbconnctn.GetConnectionString())   'Added on 23-04-2024
    Dim javaScriptSerializer As New JavaScriptSerializer()
    Private Property parameters As Dictionary(Of String, Object)
#End Region
    'Public Function AsyncMailMessage_Bytes(ByVal recepients As List(Of String), ByVal subject As String, ByVal body As String, Optional fle As List(Of Byte()) = Nothing, Optional params As Dictionary(Of String, Object) = Nothing) As String
    '    Dim ret As String = ""
    '    Try
    '        Dim email As New Threading.Thread(Sub() SendAsyncMailMessage_Bytes(recepients, subject, body, fle))
    '        'Dim email As New Threading.Thread(Sub() send_Async_email_multiple_bytes(recepients, subject, body, fle))
    '        If params IsNot Nothing Then
    '            Me.parameters = params
    '        End If
    '        email.IsBackground = True
    '        email.Start()
    '        ret = "0"
    '    Catch ex As Exception
    '        ret = "1"
    '    End Try
    '    Return ret
    'End Function
#Region "Sending Test Email"
    Public Sub AsyncMailMessage_Bytes(ByVal recepients As List(Of String), ByVal subject As String, ByVal body As String, Optional fle As List(Of Byte()) = Nothing, Optional params As Dictionary(Of String, Object) = Nothing)
        Dim ret = ""
        Dim mMailMessage As New MailMessage() With {
        .From = New MailAddress("utsab.87@gmail.com")
    }
        For Each r As String In recepients
            mMailMessage.To.Add(New MailAddress(r))
        Next

        'mMailMessage.Subject = Mid(subject, 1, 50)
        mMailMessage.Subject = subject
        mMailMessage.Body = body

        If fle IsNot Nothing Then
            For x As Integer = 0 To fle.Count - 1
                'If Not Byte(fle.Item(x)) Then
                mMailMessage.Attachments.Add(New Attachment(New MemoryStream(fle.Item(x)), subject.Replace(" ", "_") + "_" + (x + 1).ToString() + ".pdf"))
                'End If
            Next
        End If

        mMailMessage.IsBodyHtml = True
        mMailMessage.Priority = MailPriority.Normal
        Dim mSmtpClient As New SmtpClient() With {
        .EnableSsl = True,
        .Host = "smtp.gmail.com",
        .Port = 587,
    .Credentials = New NetworkCredential("utsab.87@gmail.com", "tmnnusptedrtkcdq")
    }
        ' Send the mail message
        Try
            Dim state As Object = mMailMessage
            AddHandler mSmtpClient.SendCompleted, AddressOf MailDeliveryComplete
            mSmtpClient.SendAsync(mMailMessage, state)
            ret = "0"
        Catch ex As Exception
            ret = "1"
        End Try
    End Sub
#End Region
#Region "Sending NIC Email"
    Private Shared Function AcceptAllCertifications(sender As Object, certification As System.Security.Cryptography.X509Certificates.X509Certificate, chain As System.Security.Cryptography.X509Certificates.X509Chain, sslPolicyErrors As System.Net.Security.SslPolicyErrors) As Boolean
        Return True
    End Function
    Public Function send_Async_email_multiple_bytes(ByVal email_ids As List(Of String), ByVal email_sub As String, ByVal email_body As String, Optional filebytes As List(Of Byte()) = Nothing) As String
        Dim outmsg As String
        Dim message As New MailMessage() ' MailMessage
        Try
            Dim StrMsg As String
            StrMsg = email_body.ToString()
            Dim usermessage As String = StrMsg
            Dim sendersubject As String = email_sub.ToString()
            message.From = New MailAddress("noreply.rgmwb@gov.in")
            For Each r As String In email_ids
                message.To.Add(New Net.Mail.MailAddress(r))
            Next
            If (sendersubject.ToLower() <> Convert.ToString("Marriage Certificate").ToLower() And sendersubject.ToLower() <> Convert.ToString("Login OTP").ToLower()) Then
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
                Net.ServicePointManager.SecurityProtocol = Net.SecurityProtocolType.Tls12
                System.Net.ServicePointManager.ServerCertificateValidationCallback = AddressOf AcceptAllCertifications

                'mailClient.Send(message)
                Dim state As Object = message
                AddHandler mailClient.SendCompleted, AddressOf MailDeliveryComplete
                mailClient.SendAsync(message, state)
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
#Region "Post Mail Delivery and Create Log"
    Sub MailDeliveryComplete(sender As Object, e As System.ComponentModel.AsyncCompletedEventArgs)
        Try
            'Console.WriteLine("Message {0}.", e.UserState)
            Dim mail = DirectCast(e.UserState, Net.Mail.MailMessage)
            If (Not e.Cancelled And e.Error Is Nothing) Then
            End If
            CreateLog(e)
        Catch ex As Exception
            'Throw ex
        End Try
    End Sub
    Private Sub CreateLog(mail As Object)
        Dim strFile = ""
        'Added on 11-03-2024 for saving the logs in different folders
        Dim strPath = HostingEnvironment.MapPath("~/MARREG_Logs/") + Date.Today.Year.ToString() + "/" + Date.Today.ToString("MMMM") + "/"
        If Not System.IO.Directory.Exists(strPath) Then
            System.IO.Directory.CreateDirectory(strPath)
        End If
        'End Added on 11-03-2024 for saving the logs in different folders
        strFile = strPath + "email_logs_" + DateTime.Now.ToString("dd_MMM_yyyy") + ".log"
        Using sw As New StreamWriter(File.Open(strFile, IO.FileMode.OpenOrCreate))

        End Using
        Dim rec = ""
        For i As Int16 = 0 To mail.UserState.To.Count - 1
            Dim d = ""
            If i < mail.UserState.To.Count - 1 Then
                d = ","
            End If
            rec += mail.UserState.To.Item(i).ToString + d
        Next
        Dim err = ""
        If Not mail.Error Is Nothing Then
            Dim RegexSentenceParse As String = Regex.Replace(mail.Error.Message.ToString, "(^.*[\.\?!]|^\S[^.\?!]*)", "")
            If (RegexSentenceParse = "") Then
                err = mail.Error.Message.ToString
            Else
                err = mail.Error.Message.ToString.Replace(RegexSentenceParse, "")
            End If
        End If

        Dim str = "From: " + mail.UserState.From.ToString + "; To: " + rec + "; Subject: " + mail.UserState.Subject + "; Status:" + IIf(err.Length = 0, "Email Sent", "Email Not Sent") + "; Error: " + IIf(err.Length = 0, "No Error;", err + ";") + IIf(parameters IsNot Nothing, " Data:" + javaScriptSerializer.Serialize(parameters), "")
        Using sw As New IO.StreamWriter(strFile, True)
            sw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") & " -- " & str)
            sw.Flush()      'Added on 13-03-2024
            sw.Close()      'Added on 13-03-2024
        End Using
    End Sub
#End Region

#Region "Post Mail Delivery and Save into DB"
    Private Function save_emails(email_from As String, email_to As String, email_sub As String, email_body As String, sending_time As DateTime, sending_status As String, error_string As String) As Integer
        Dim ReturnVal As New Int32
        Try
            con.Open()
            Using command As New SqlCommand("MARREG_DSCDataTransferMO", con)
                command.CommandType = CommandType.StoredProcedure
                command.Parameters.Add("@email_from", SqlDbType.NVarChar).Value = email_from
                command.Parameters.Add("@email_to", SqlDbType.NVarChar).Value = email_to
                command.Parameters.Add("@email_sub", SqlDbType.NVarChar).Value = email_sub
                command.Parameters.Add("@email_body", SqlDbType.NVarChar).Value = email_body
                command.Parameters.Add("@sending_time", SqlDbType.DateTime).Value = sending_time
                command.Parameters.Add("@sending_status", SqlDbType.NVarChar).Value = sending_status
                command.Parameters.Add("@error_string", SqlDbType.NVarChar).Value = error_string

                command.Parameters.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.Output
                command.ExecuteNonQuery()

                ReturnVal = Convert.ToInt32(command.Parameters("@returnValue").Value.ToString())
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            con.Close()
        End Try
        Return ReturnVal
    End Function
#End Region
End Class