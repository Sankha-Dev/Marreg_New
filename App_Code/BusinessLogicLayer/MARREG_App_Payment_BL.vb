Imports System.Data
Imports Microsoft.VisualBasic


Public Class MARREG_App_Payment_BL
    Dim objDB As New MARREG_App_Payment_DB
    Public Function Save_App_Payment_Init(app_id As String, MO_cd As String, amt As Int64, c_time As DateTime, e_time As DateTime, status As String, draft_cd As String) As String
        objDB.app_id = app_id
        objDB.MO_cd = MO_cd
        objDB.total_amt = amt
        objDB.c_time = c_time
        objDB.e_time = e_time
        objDB.status = status
        objDB.draft_cd = draft_cd

        Dim ret = objDB.Save_App_Payment_Init()
        Return ret
    End Function
    Public Function ValidatePaymentLink(id As String, p_app_id As String, c_time As DateTime) As String
        objDB.id = id
        objDB.app_id = p_app_id
        objDB.c_time = c_time

        Return objDB.ValidatePaymentLink()
    End Function
    Public Function UpdateGRN_App_Payment_Init(pmntInitId As Int64, draft_app_id As String, grn As String, payment_dt As DateTime) As String
        objDB.id = pmntInitId
        objDB.app_id = draft_app_id
        objDB.grn = grn
        objDB.c_time = payment_dt

        Dim ret = objDB.UpdateGRN_App_Payment_Init()
        Return ret
    End Function
    Public Function FetchPaymentStatus(draft_app_id As String, MO_cd As String, draft_cd As String) As DataTable
        objDB.app_id = draft_app_id
        objDB.MO_cd = MO_cd
        objDB.draft_cd = draft_cd

        Dim ret = objDB.FetchPaymentStatus()
        Return ret.Tables(0)
    End Function
End Class

