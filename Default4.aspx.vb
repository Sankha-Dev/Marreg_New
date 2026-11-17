Imports System.IO
Imports System.Net
Imports System.Net.Mail
Imports System.Threading
Partial Class Default4
    Inherits System.Web.UI.Page
    Protected Sub SendAsyncEmail(sender As Object, e As EventArgs)
        lbl_status.Text = ""
        Dim [to] As String = txtTo.Text
        Dim from As String = txtEmail.Text
        Dim password As String = txtPassword.Text
        Dim subject As String = txtSubject.Text
        Dim body As String = txtBody.Text
        Dim postedFile As HttpPostedFile = fuAttachment.PostedFile
        'Dim email As New Thread(Sub() SendEmail([to], from, password, subject, body, postedFile))
        'email.IsBackground = False
        'email.Start()
        'lbl_status.Text = "Email sent"
        'lbl_status.Attributes.Add("style", "color:green;")
        Dim email As New Thread(Sub() SendEmailAsync([to], from, password, subject, body, postedFile))
        email.IsBackground = True
        email.Start()
        'SendEmailAsync([to], from, password, subject, body, postedFile)
        ClientScript.RegisterStartupScript(Me.GetType(), "alert", "alert('Email sent.');", True)
    End Sub

    'Private Sub SendEmail([to] As String, from As String, password As String, subject As String, body As String, postedFile As HttpPostedFile)
    '    Try

    '        Using mm As New MailMessage(from, [to])
    '            mm.Subject = subject
    '            mm.Body = body
    '            If postedFile.ContentLength > 0 Then
    '                Dim fileName As String = Path.GetFileName(postedFile.FileName)
    '                mm.Attachments.Add(New Attachment(postedFile.InputStream, fileName))
    '            End If
    '            mm.IsBodyHtml = False
    '            Dim smtp As New SmtpClient()
    '            smtp.Host = "smtp.gmail.com"
    '            smtp.EnableSsl = True
    '            Dim NetworkCred As New NetworkCredential(from, password)
    '            smtp.UseDefaultCredentials = True
    '            smtp.Credentials = NetworkCred
    '            smtp.Port = 587
    '            AddHandler smtp.SendCompleted, AddressOf MailDeliveryComplete
    '            smtp.Send(mm)
    '        End Using
    '    Catch ex As Exception
    '        ClientScript.RegisterStartupScript(Me.GetType(), "alert", "alert('" + ex.Message + "');", True)
    '    End Try
    'End Sub
    Private Sub SendEmailAsync([to] As String, from As String, password As String, subject As String, body As String, postedFile As HttpPostedFile)
        'Try
        Dim message = New MailMessage()
        message.To.Add([to])
        message.From = New MailAddress(from)
        message.Subject = subject
        message.Body = body + ". " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss:fffffffK")
        If postedFile.ContentLength > 0 Then
            Dim fileName As String = Path.GetFileName(postedFile.FileName)
            message.Attachments.Add(New Attachment(postedFile.InputStream, fileName))
        End If

        Dim client As New SmtpClient()
        client.Host = "smtp.gmail.com"
        client.Port = 587
        client.EnableSsl = True

        Dim cred = New NetworkCredential()
        cred.UserName = from
        cred.Password = password
        client.Credentials = cred

        Dim state As Object = message

        AddHandler client.SendCompleted, AddressOf MailDeliveryComplete
        'client.SendCompleted = AddressOf MailDeliveryComplete
        client.SendAsync(message, state)
        ClientScript.RegisterStartupScript(Me.GetType(), "alert", "alert('Email sent...');", True)
        'Console.WriteLine("Sending finished, press Enter.")
        'Console.ReadLine()

        'Using mm As New MailMessage(from, [to])
        '    mm.Subject = subject
        '    mm.Body = body
        '    If postedFile.ContentLength > 0 Then
        '        Dim fileName As String = Path.GetFileName(postedFile.FileName)
        '        mm.Attachments.Add(New Attachment(postedFile.InputStream, fileName))
        '    End If
        '    mm.IsBodyHtml = False
        '    Dim smtp As New SmtpClient()
        '    smtp.Host = "smtp.gmail.com"
        '    smtp.EnableSsl = True
        '    Dim NetworkCred As New NetworkCredential(from, password)
        '    smtp.UseDefaultCredentials = True
        '    smtp.Credentials = NetworkCred
        '    smtp.Port = 587
        '    'Try

        '    AddHandler smtp.SendCompleted, AddressOf MailDeliveryComplete
        '    smtp.SendAsync(mm, "Test")
        '    'Catch ex As Exception
        '    '    lbl_status.Text = ex.Message
        '    '    lbl_status.Attributes.Add("style", "color:red;")
        '    'End Try

        'End Using

        'Catch ex As Exception
        '    ClientScript.RegisterStartupScript(Me.GetType(), "alert", "alert('" + ex.Message + "');", True)
        'End Try
    End Sub
    Sub MailDeliveryComplete(sender As Object, e As System.ComponentModel.AsyncCompletedEventArgs)
        Console.WriteLine("Message {0}.", e.UserState)
        Dim mail = DirectCast(e.UserState, MailMessage)
        If (Not e.Cancelled And e.Error Is Nothing) Then
            lbl_status.Text = "Email finally sent"
            lbl_status.Attributes.Add("style", "color:green;")
        End If
        CreateLog(e)
        'If e.Error IsNot Nothing Then
        '    lbl_status.Text = "Email not sent"
        '    lbl_status.Attributes.Add("style", "color:red;")
        '    'Console.WriteLine("Error sending email.")
        'ElseIf e.Cancelled Then
        '    lbl_status.Text = "Sending of email cancelled"
        '    lbl_status.Attributes.Add("style", "color:orange;")
        '    'Console.WriteLine("Sending of email cancelled.")
        'Else
        '    lbl_status.Text = "Email finally sent"
        '    lbl_status.Attributes.Add("style", "color:green;")
        '    'Console.WriteLine("Message sent.")
        'End If
    End Sub
    Private Sub CreateLog(mail As Object)
        Dim strFile As String = Server.MapPath("~/MARREG_Logs/") + "email_logs_" + DateTime.Now.ToString("MMM_yyyy") + ".log"

        Using sw As New StreamWriter(File.Open(strFile, FileMode.OpenOrCreate))

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

        Dim str = "From: " + mail.UserState.From.ToString + "; To: " + rec + "; Subject: " + mail.UserState.Subject + "; Status:" + IIf(err.Length = 0, "Email Sent", "Email Not Sent") + "; Error: " + IIf(err.Length = 0, "No Error", err)
        Using sw As New StreamWriter(strFile, True)
            'sw = File.AppendText(strFile)
            sw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") & " -- " & str)
        End Using
    End Sub

    Private Sub Default4_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim a = DateTime.Parse("20/09/2023").DayOfWeek
        a = 7 - a + 2
        Dim d = DateAndTime.DateAdd(DateInterval.Day, a, Convert.ToDateTime("2023-09-20"))
        lbl_status.Text = d.ToString("dd/MM/yyyy")
    End Sub
End Class
