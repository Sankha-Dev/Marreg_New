Imports Microsoft.VisualBasic
Imports MARREG.App_Code.DataAccessLayer
Imports System.Data
Imports System.Data.SqlClient


Namespace MARREG.App_Code.BusinessLogicLayer
    Public Class Email_Sms_BL
        Dim Email_Sms_DB As New Email_Sms_DB()

#Region "Send Email"
        Public Function send_email(email_id As String, email_sub As String, email_body As String) As String
            Return Email_Sms_DB.send_email(email_id, email_sub, email_body)
        End Function
#End Region
#Region "Send Email with Attachment"
        Public Function send_email_attachment(email_id As String, email_sub As String, email_body As String, filepath As String) As String
            Return Email_Sms_DB.send_email_attachment(email_id, email_sub, email_body, filepath)
        End Function
#End Region

#Region "Send Email MO"
        Public Function send_email_attachment_mo(email_id As String, email_sub As String, email_body As String, filepath As String, sender_email_id As String) As String
            Return Email_Sms_DB.send_email_attachment_mo(email_id, email_sub, email_body, filepath, sender_email_id)
        End Function
#End Region
#Region "Sent Email with Attachment test"
        Function send_email_attachment_new(email_id As String, email_sub As String, email_body As String) As String
            Return Email_Sms_DB.send_email_attachment_new(email_id, email_sub, email_body)
        End Function
#End Region
#Region "Send SMS"
        Public Function send_sms(mnumber As String, message As String) As String
            Return Email_Sms_DB.send_sms(mnumber,message)
        End Function
#End Region

#Region "Send SMS With Template ID"
        Public Function send_sms_new_template(mnumber As String, message As String, template_id As String) As String
            Return Email_Sms_DB.send_sms_new_template(mnumber, message, template_id)
        End Function
#End Region

#Region "Send Email to Multiple Receipient with Multiple Attachment"
        Public Function send_email_multiple(ByVal email_ids As List(Of String), ByVal email_sub As String, ByVal email_body As String, Optional filepaths As List(Of String) = Nothing) As String
            Return Email_Sms_DB.send_email_multiple(email_ids, email_sub, email_body, filepaths)
        End Function
		Public Function send_email_multiple_bytes(ByVal email_ids As List(Of String), ByVal email_sub As String, ByVal email_body As String, Optional filebytes As List(Of Byte()) = Nothing) As String
            Return Email_Sms_DB.send_email_multiple_bytes(email_ids, email_sub, email_body, filebytes)
        End Function
#End Region
    End Class
End Namespace
