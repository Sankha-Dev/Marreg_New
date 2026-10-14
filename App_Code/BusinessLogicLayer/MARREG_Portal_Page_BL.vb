Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Web
Imports MARREG.App_Code.DataAccessLayer
Imports System.Xml
Imports System.Data

Namespace MARREG.App_Code.BusinessLogicLayer

    Public Class MARREG_Portal_Page_BL
        Dim portl_page_db As New MARREG_Portal_Page_DB()
        Public Sub New()
        End Sub



#Region "Public Mo view"
        Public Function Public_MO_view(State_Cd As String, district As String, ps As String, subdivision As String, block As String, Work_area As String, Muni_gp As String, Word_Mouza As String, MO_Type_Cd As String) As DataSet
            Return portl_page_db.Public_MO_view(State_Cd, district, ps, subdivision, block, Work_area, Muni_gp, Word_Mouza, MO_Type_Cd)
        End Function
        Public Function Public_MO_view_MONameWise(MO_Name As String) As DataSet
            Return portl_page_db.Public_MO_view_NameWise(MO_Name)
        End Function
#End Region

#Region "Public Mo Non-Functional"
        Public Function Public_MO_NonFunctional(state_cd As String, MO_Type_Cd As String, district As String, ps As String) As DataSet
            Return portl_page_db.Public_MO_NonFunctional(state_cd, MO_Type_Cd, district, ps)
        End Function
#End Region

#Region "Objection Insert in Objection"
        Public Function ObjectionSave(Application_cd As String, Objector_Name As String, Objector_Addr As String, Objector_Country_Cd As String, Objector_State_Flag As String, Objector_State As String,
                                      Objector_Dist As String, Objector_Subdiv As String, Objector_Block As String, Objector_WA As String, Objector_Muni_Gp As String, Objector_Vil_Wrd As String,
                                      Objector_PS As String, Objector_PO As String, Objector_Pin As String, Objector_Applcnt_Reltn_Cd As String, Objector_Mob_No As String, Objector_Email_Id As String,
                                      Objection_Resn_Cd As String, Objector_signature As Byte(), Ip_Address As String, Bank_Cd As String, Bank_Branch_Dist As String, Bank_Branch As String,
                                      Payment_Mode_Cd As String, Payment_Mode_No As String, Payment_Mode_Dt As String, Payable_At As String) As String

            Return portl_page_db.ObjectionSave(Application_cd, Objector_Name, Objector_Addr, Objector_Country_Cd, Objector_State_Flag, Objector_State, Objector_Dist, Objector_Subdiv, Objector_Block,
                                               Objector_WA, Objector_Muni_Gp, Objector_Vil_Wrd, Objector_PS, Objector_PO, Objector_Pin, Objector_Applcnt_Reltn_Cd, Objector_Mob_No, Objector_Email_Id,
                                               Objection_Resn_Cd, Objector_signature, Ip_Address, Bank_Cd, Bank_Branch_Dist, Bank_Branch, Payment_Mode_Cd, Payment_Mode_No, Payment_Mode_Dt, Payable_At)
        End Function
#End Region

#Region "contact Us"
        Public Function ContactUs_Insert(Name As String, Mobile_No As String, Email_Id As String, Subject As String, Message As String) As String
            Return portl_page_db.ContactUs_Insert(Name, Mobile_No, Email_Id, Subject, Message)
        End Function
#End Region
#Region "Feedback"
        Public Function Feedback_Insert(Name As String, Email_Id As String, Satisfaction_rating As String, Experience_rating As String, Expectation As String, ipaddress As String) As String
            Return portl_page_db.Feedback_Insert(Name, Email_Id, Satisfaction_rating, Experience_rating, Expectation, ipaddress)
        End Function
#End Region

#Region "Portal Login insert update"
        Public Function PortalLoginSave(log_id As String, SessionID As String, visit_ipaddr As String, mac_addr As String, status As String, flag As Int16) As String
            If (flag = 1) Then
                Return portl_page_db.PortalLoginSave(log_id, SessionID, visit_ipaddr, mac_addr, status)
            Else
                Return portl_page_db.PortalLoginUpdate(log_id, visit_ipaddr, mac_addr, status)
            End If
        End Function
#End Region

#Region "Login In-time Fetch"
        Public Function Portal_Visitors_count() As DataTable
            Return portl_page_db.Portal_Visitors_count()
        End Function
#End Region

#Region "NewApp Payment Insert"
        Public Function NewApp_Payment_Insert(Applicant_Cd As String, Bank_cd As String, Branch_District_cd As String, Bank_Branch_cd As String, Payment_Mode_Cd As String,
                                              Payment_Mode_No As String, Payment_Mode_Dt As DateTime, Payable_At As String) As String
            Return portl_page_db.NewApp_Payment_Insert(Applicant_Cd, Bank_cd, Branch_District_cd, Bank_Branch_cd, Payment_Mode_Cd, Payment_Mode_No, Payment_Mode_Dt, Payable_At)
        End Function
#End Region

#Region "GRIPS Payment"
        Public Function GRIPS_Insert(Applicant_Cd As String, depositor_nam As String, depositor_mob As String, depositor_email As String, pay_amnt As String) As String

            Return portl_page_db.GRIPS_Insert(Applicant_Cd, depositor_nam, depositor_mob, depositor_email, pay_amnt)
        End Function

        'new 26.04.2018
        Public Function GRIPS_Insert_GRN(Applicant_Cd As String, depositor_nam As String, depositor_mob As String, depositor_email As String, paymnt_dt As DateTime, pay_amnt As String, GRN As String,Mo_CD As String) As Array
            Return portl_page_db.GRIPS_Insert_GRN(Applicant_Cd, depositor_nam, depositor_mob, depositor_email, paymnt_dt, pay_amnt, GRN, Mo_CD)
        End Function
        '18.01.2019
        'Public Function GRIPS_Insert_GRN_Additional_Payment(Applicant_Cd As String, depositor_nam As String, depositor_mob As String, depositor_email As String, paymnt_dt As DateTime, pay_amnt As String, GRN As String, Mo_CD As String, MARREG_AdditionalPayment_cd As String, MARREG_Payment_Reason_cd As String) As Array
        '    Return portl_page_db.GRIPS_Insert_GRN_Additional_Payment(Applicant_Cd, depositor_nam, depositor_mob, depositor_email, paymnt_dt, pay_amnt, GRN, Mo_CD, MARREG_AdditionalPayment_cd, MARREG_Payment_Reason_cd)
        'End Function
        Public Function GRIPS_Insert_GRN_Additional_Payment(Applicant_Cd As String, depositor_nam As String, depositor_mob As String, depositor_email As String, paymnt_dt As DateTime, pay_amnt As String, GRN As String, Mo_CD As String, dtad_data As DataTable) As String
            Return portl_page_db.GRIPS_Insert_GRN_Additional_Payment(Applicant_Cd, depositor_nam, depositor_mob, depositor_email, paymnt_dt, pay_amnt, GRN, Mo_CD, dtad_data)
        End Function
#End Region

#Region "Fill Data table for DoughnutChart"
        Public Function Fill_DataTable(State_cd As String) As DataTable
            Return portl_page_db.Fill_DataTable(State_cd)
        End Function
#End Region

#Region "Fill vill wrd"
        Public Function Fill_VilWrd_Txt(Dist As String, SubDiv As String, blck As String, gp As String, Mouza_nm As String, ip_addr As String) As String
            Return portl_page_db.Fill_VilWrd_Txt(Dist, SubDiv, blck, gp, Mouza_nm, ip_addr)
        End Function
#End Region

#Region "MoTrainsaction"
        Public Function MoTrainsaction() As DataSet
            Return portl_page_db.MoTrainsaction()
        End Function
#End Region

#Region "Correction GRN"
        Public Function GRIPS_Insert_GRN_For_Correction(application_no As String, no_of_correction As String, grn_amount As String, grn_no As String, deposited_by As String, depositor_email_id As String, depositor_mobile_no As String, Mo_CD As String) As Array
            Return portl_page_db.GRIPS_Insert_GRN_For_Correction(application_no, no_of_correction, grn_amount, grn_no, deposited_by, depositor_email_id, depositor_mobile_no, Mo_CD)
        End Function
        Public Function GRIPS_Insert_GRN_For_Correction_RGM(application_no As String, no_of_correction As String, grn_amount As String, grn_no As String, deposited_by As String, depositor_email_id As String, depositor_mobile_no As String, Mo_CD As String) As Array
            Return portl_page_db.GRIPS_Insert_GRN_For_Correction_RGM(application_no, no_of_correction, grn_amount, grn_no, deposited_by, depositor_email_id, depositor_mobile_no, Mo_CD)
        End Function
#End Region

    End Class
End Namespace
