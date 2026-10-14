Imports Microsoft.VisualBasic
Imports System.Data
Imports MARREG.App_Code.DataAccessLayer

Namespace MARREG.App_Code
    Public Class CertifiedNotice_BL
        Dim objDB As New CertifiedNotice_DB()
#Region "Certified Copy Request"
        Public Function CheckEmailExist(email As String) As DataSet
            objDB.email = email
            Return objDB.CheckEmailExist()
        End Function
#End Region
#Region "Certified Copy Request Old Records"
        Public Function CertifiedCopyRequestInfoValidateOldNotice(groom_name As String, bride_name As String, groom_father As String, bride_father As String, groom_dob As String, bride_dob As String, reg_date As String, mo_name As String, ActCd As String) As String
            objDB.groom_name = groom_name
            objDB.bride_name = bride_name
            objDB.groom_Fname = groom_father
            objDB.bride_Fname = bride_father
            objDB.groom_dobs = groom_dob
            objDB.bride_dobs = bride_dob
            objDB.reg_dates = reg_date
            objDB.mo_name = mo_name
            objDB.actCd = ActCd

            Return objDB.CertifiedCopyRequestInfoValidateOldNotice()
        End Function
        Public Function CertifiedCopyRequestSaveEmailOldNotice(name As String, address As String, mob_no As String, email As String, validatedId As String) As String
            objDB.name = name
            objDB.address = address
            objDB.mob_no = mob_no
            objDB.email = email
            objDB.id = validatedId

            Return objDB.CertifiedCopyRequestSaveEmailOldNotice()
        End Function
        Public Function CertifiedCopyRequestEmailOTPValidateOldNotice(validatedId As String, otp As String) As Int16
            objDB.otp = otp
            objDB.id = validatedId

            Return objDB.CertifiedCopyRequestEmailOTPValidateOldNotice()
        End Function
        Public Function CertifiedCopyRequestSendOldNotice(validatedId As Int64, grn_id As Int64, no_copy As Int32) As DataTable
            objDB.grn_id = grn_id
            objDB.id = validatedId
            objDB.no_copy = no_copy

            Return objDB.CertifiedCopyRequestSendOldNotice()
        End Function
#End Region
    End Class
End Namespace
