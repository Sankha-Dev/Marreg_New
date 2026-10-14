Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports Microsoft.VisualBasic
Imports MARREG.App_Code.DataAccessLayer
Imports System.Xml
Imports System.Data

Namespace MARREG.App_Code.BusinessLogicLayer

    Public Class MasterData
        Dim mastrdb As New MasterDB()
        Public Sub New()
        End Sub

#Region "District Master"
        'Private m_District_Cd As String
        'Public Property District_Cd As String
        '    Get
        '        Return m_District_Cd

        '    End Get
        '    Set(value As String)
        '        m_District_Cd = value
        '    End Set
        'End Property
        'Private m_State_Name As String
        'Public Property state_Name As String
        '    Get
        '        Return m_State_Name

        '    End Get
        '    Set(value As String)
        '        m_State_Name = value
        '    End Set
        'End Property
        'Private Property m_EDistrict_Name As String
        'Public Property eDistrict_Name As String
        '    Get
        '        Return m_EDistrict_Name

        '    End Get
        '    Set(value As String)
        '        m_EDistrict_Name = value

        '    End Set
        'End Property

        'Private Property m_VDistrict_Name As String
        'Public Property vDistrict_Name() As String
        '    Get
        '        Return m_VDistrict_Name
        '    End Get
        '    Set(value As String)
        '        m_VDistrict_Name = value

        '    End Set
        'End Property
        'Private Property m_Delete_Status As String
        'Public Property delete_Status() As String
        '    Get
        '        Return m_Delete_Status
        '    End Get
        '    Set(value As String)
        '        m_Delete_Status = value

        '    End Set
        'End Property
        'Private Property m_Entry_Dt As DateTime
        'Public Property entry_Dt() As DateTime
        '    Get
        '        Return m_Entry_Dt
        '    End Get
        '    Set(value As DateTime)
        '        m_Entry_Dt = value

        '    End Set
        'End Property
        'Private Property m_User_Id As String
        'Public Property user_Id() As String
        '    Get
        '        Return m_User_Id
        '    End Get
        '    Set(value As String)
        '        m_User_Id = value

        '    End Set
        'End Property
        'Private Property m_Ip_Address As String
        'Public Property ip_Address() As String
        '    Get
        '        Return m_Ip_Address
        '    End Get
        '    Set(value As String)
        '        m_Ip_Address = value

        '    End Set
        'End Property
        'Public Sub New(District_Cd As String, state_Name As String, eDistrict_Name As String, vDistrict_Name As String,
        '               delete_Status As String, entry_Dt As DateTime, user_Id As String, ip_Address As String)
        '    m_District_Cd = District_Cd
        '    m_State_Name = state_Name
        '    m_EDistrict_Name = eDistrict_Name
        '    m_VDistrict_Name = vDistrict_Name
        '    m_Delete_Status = delete_Status
        '    m_Entry_Dt = entry_Dt
        '    m_User_Id = user_Id

        '    m_Ip_Address = ip_Address

        'End Sub
#End Region

#Region "District master insert update"
        Public Function DistrictSave(District_Cd As String, state_Name As String, eDistrict_Name As String, vDistrict_Name As String, Cen_Distict_cd As String, delete_Status As String, user_Id As String, ip_Address As String, flag As Int16) As String
            If (flag = 1) Then
                Return mastrdb.DistrictSave(District_Cd, state_Name, eDistrict_Name, vDistrict_Name, Cen_Distict_cd, delete_Status, user_Id, ip_Address)
            Else
                Return mastrdb.DistrictUpdate(District_Cd, state_Name, eDistrict_Name, vDistrict_Name, Cen_Distict_cd, delete_Status, user_Id, ip_Address)
            End If
        End Function
#End Region

#Region "Sub Division master insert update"
        Public Function SubDivisionSave(Sub_Division_Cd As String, State_Cd As String, District_Cd As String, eESub_Division_Name As String, vVSub_Division_Name As String, CenSub_Division_Cd As String, delete_Status As String, user_Id As String, ip_Address As String, flag As Int16) As String
            If (flag = 1) Then
                Return mastrdb.SubDivisionSave(State_Cd, Sub_Division_Cd, District_Cd, eESub_Division_Name, vVSub_Division_Name, CenSub_Division_Cd, delete_Status, user_Id, ip_Address)
            Else
                Return mastrdb.SubDivisionUpdate(State_Cd, Sub_Division_Cd, District_Cd, eESub_Division_Name, vVSub_Division_Name, CenSub_Division_Cd, delete_Status, user_Id, ip_Address)
            End If
        End Function

#End Region

#Region "Block master insert update"


        Function BlockSave(Pre_Block_cd As String, sate_cd As String, District_Cd As String, Sub_Division_Cd As String,
                                  Agroclimatic_Zone As String, eblock_nm As String, vblock_nm As String,
                                  Cen_block_cd As String, delete_Status As String,
                                  user_Id As String, ip_Address As String, flag As Int16) As String
            If (flag = 1) Then
                Return mastrdb.BlockSave(Pre_Block_cd, sate_cd, District_Cd, Sub_Division_Cd,
                                  Agroclimatic_Zone,
                                  eblock_nm, vblock_nm, Cen_block_cd, delete_Status,
                                  user_Id, ip_Address)
            Else
                Return mastrdb.BlockUpdate(Pre_Block_cd, sate_cd, District_Cd, Sub_Division_Cd,
                                Agroclimatic_Zone,
                                eblock_nm, vblock_nm, Cen_block_cd, delete_Status,
                                user_Id, ip_Address)
            End If

        End Function
#End Region

#Region "Church master insert update"
        Public Function Church_Name_Save(church_Cd As String, District_Cd As String, Sub_Division_Cd As String, ps_Cd As String, c_name As String, type As String, state As String, flag As Int16) As String
            If (flag = 1) Then
                Return mastrdb.Church_Name_Save(church_Cd, District_Cd, Sub_Division_Cd, ps_Cd, c_name, type, state, flag)
            Else
                Return mastrdb.Church_Name_Update(church_Cd, District_Cd, Sub_Division_Cd, ps_Cd, c_name, type, state, flag)
            End If
        End Function
#End Region

#Region "Employee Master Insert and Update"
        Public Function Employee_Save_Update(emp_Cd As Int32, EmployeeType As Int32, EmployeePost As Int32, EmployeeName As String, EmployeePnF As String, EmployeePayLevel As String, EmployeeOrder As String, EmployeeStatus As String, state As String, flag As Int16) As String
            Return mastrdb.Employee_Details_Save_Update(emp_Cd, EmployeeType, EmployeePost, EmployeeName, EmployeePnF, EmployeePayLevel, EmployeeOrder, EmployeeStatus, state, flag)
        End Function
#End Region

#Region "RTI Authority Master Insert and Update"
        Public Function RTIAuth_Save_Update(rtiauth_cd As String, rtiauthPost As String, rtiauthDesg As String, rtiauthName As String, rtiauthStatus As String, state As String) As String
            Return mastrdb.RTIAuth_Details_Save_Update(rtiauth_cd, rtiauthPost, rtiauthDesg, rtiauthName, rtiauthStatus, state)
        End Function
#End Region

#Region "Portal Feedback Activity Logs"
        Public Function PortalFeedbackActivityLogsSave(user_ip As String, browseragent As String, response As Int32) As String
            Return mastrdb.PortalFeedbackActivityLogsSaveDB(user_ip, browseragent, response)
        End Function
#End Region

#Region "Declaration Wipping Bio-Metrics Master Insert"
        Public Function DeclarationWBM_Insert(m_cd As Int32, app_count As Int32, emailbody As String) As String
            Return mastrdb.DeclarationWBM(m_cd, app_count, emailbody)
        End Function
#End Region

#Region "Employee Master Insert and Update"
        Public Function IndexData_Save_Update(dmtCd As String, mo_cd As String, mg_type As String, b_type As String, b_slno As String, fromdate As String, todate As String, bgno As String, remarks As String, state As String, flag As Int16) As String
            Return mastrdb.Index_Data_Save_Update(dmtCd, mo_cd, mg_type, b_type, b_slno, fromdate, todate, bgno, remarks, state, flag)
        End Function
#End Region

#Region "Question Master insert update"
        Public Function Question_Name_Save(ques_cd As String, ques_en As String, ques_bn As String, ques_weight As String, type As String, state As String, flag As Int16) As String
            If (flag = 1) Then
                Return mastrdb.Question_Name_SaveUpdate(ques_cd, ques_en, ques_bn, ques_weight, type, state, flag, "insert")
            Else
                Return mastrdb.Question_Name_SaveUpdate(ques_cd, ques_en, ques_bn, ques_weight, type, state, flag, "update")
            End If
        End Function
#End Region

#Region "Feedback"
        Public Function Save(feedbackList As List(Of Dictionary(Of String, String)), language As String, appAutoCd As String, userIP As String, browserAgent As String) As Boolean
            Return mastrdb.Feedback_Save(feedbackList, language, appAutoCd, userIP, browserAgent)
        End Function

        Public Function GetAllQuestionsJson() As String
            Return mastrdb.GetQuestionnaireJson()
        End Function

#End Region

#Region "Application Acceptance Save"
        Public Function aa_save(drft_cd As String, status As String, remarks As String) As String
            Return mastrdb.AA_Save(drft_cd, status, remarks)
        End Function


#End Region

#Region "Answer Master insert update"
        Public Function Answer_Name_Save(ans_cd As String, ques_cd As Int16, ans_en As String, ans_bn As String, ans_mark As String, type As String, state As String, flag As Int16) As String
            If (flag = 1) Then
                Return mastrdb.Answer_Name_SaveUpdate(ans_cd, ques_cd, ans_en, ans_bn, ans_mark, type, state, flag, "insert")
            Else
                Return mastrdb.Answer_Name_SaveUpdate(ans_cd, ques_cd, ans_en, ans_bn, ans_mark, type, state, flag, "update")
            End If
        End Function
#End Region

#Region "Mouza and Ward master insert update"
        Public Function Mouza_Ward_Save(Mouza_word_Cd As String, state_Cd As String, District_Cd As String, Sub_Division_Cd As String, Block_Cd As String, Gp_Mp_cd As String, E_Name As String, V_Name As String, Census_cd As String, delete_Status As String, user_Id As String, ip_Address As String, type As String, flag As Int16) As String
            If (flag = 1) Then
                Return mastrdb.Mouza_Ward_Save(Mouza_word_Cd, state_Cd, District_Cd, Sub_Division_Cd, Block_Cd, Gp_Mp_cd, E_Name, V_Name, Census_cd, delete_Status, user_Id, ip_Address, type)
            Else
                Return mastrdb.Mouza_Ward_Update(Mouza_word_Cd, state_Cd, District_Cd, Sub_Division_Cd, Block_Cd, Gp_Mp_cd, E_Name, V_Name, Census_cd, delete_Status, user_Id, ip_Address, type)
            End If
        End Function
#End Region

#Region "Municipl and Gp master insert update"
        Public Function Municipl_Gp_Save(Municipl_Gp_Cd As String, state_Cd As String, District_Cd As String, Sub_Division_Cd As String, Block_Cd As String, E_Name As String, V_Name As String, Census_cd As String, delete_Status As String, user_Id As String, ip_Address As String, type As String, flag As Int16) As String
            If (flag = 1) Then
                Return mastrdb.Municipi_Gp_Save(Municipl_Gp_Cd, state_Cd, District_Cd, Sub_Division_Cd, Block_Cd, E_Name, V_Name, Census_cd, delete_Status, user_Id, ip_Address, type)
            Else
                Return mastrdb.Municipi_Gp_Update(Municipl_Gp_Cd, state_Cd, District_Cd, Sub_Division_Cd, Block_Cd, E_Name, V_Name, Census_cd, delete_Status, user_Id, ip_Address, type)
            End If
        End Function
#End Region

#Region "PO master insert update"
        Public Function Po_Save(Pre_PO_cd As String, State_cd As String, District_Cd As String, epo_nm As String, vpo_nm As String, Pin_No As String, category As String, DeleteStatus As String, user_Id As String, ip_Address As String, flag As Int16) As String
            If (flag = 1) Then
                Return mastrdb.Po_Save(Pre_PO_cd, State_cd, District_Cd, epo_nm, vpo_nm, Pin_No, category, DeleteStatus, user_Id, ip_Address)
            Else
                Return mastrdb.Po_Update(Pre_PO_cd, State_cd, District_Cd, epo_nm, vpo_nm, Pin_No, category, DeleteStatus, user_Id, ip_Address)
            End If
        End Function
#End Region

#Region "PS master insert update"
        Public Function Ps_Save(Pre_PO_cd As String, State_cd As String, District_Cd As String, epo_nm As String, vpo_nm As String, DeleteStatus As String, user_Id As String, ip_Address As String, Sub_Division_Cd As String, flag As Int16) As String
            If (flag = 1) Then
                Return mastrdb.PS_Save(Pre_PO_cd, State_cd, District_Cd, epo_nm, vpo_nm, DeleteStatus, user_Id, ip_Address, Sub_Division_Cd)
            Else
                Return mastrdb.Ps_Update(Pre_PO_cd, State_cd, District_Cd, epo_nm, vpo_nm, DeleteStatus, user_Id, ip_Address, Sub_Division_Cd)
            End If
        End Function
#End Region

#Region "Office Details master insert update"
        Public Function Office_Address_Save(Pre_ofc_cd As String, State_cd As String, District_Cd As String, Sub_Div_Cd As String, address As String, lt_deg As String, lt_nrt As String, ln_deg As String, ln_est As String, DeleteStatus As String, user_Id As String, ip_Address As String, flag As Int16) As String
            If (flag = 1) Then
                Return mastrdb.Office_Address_Save(Pre_ofc_cd, State_cd, District_Cd, Sub_Div_Cd, address, lt_deg, lt_nrt, ln_deg, ln_est, DeleteStatus, user_Id, ip_Address)
            Else
                Return mastrdb.Office_Address_Update(Pre_ofc_cd, State_cd, District_Cd, Sub_Div_Cd, address, lt_deg, lt_nrt, ln_deg, ln_est, DeleteStatus, user_Id, ip_Address)
            End If
        End Function
#End Region

#Region "Qualification master insert update"
        Public Function Qualification_Save(Pre_qual_cd As String, qual_desc As String, user_Id As String, ip_Address As String, flag As Int16) As String
            If (flag = 1) Then
                Return mastrdb.Qualification_Save(Pre_qual_cd, qual_desc, user_Id, ip_Address)
            Else
                Return mastrdb.Qualification_Update(Pre_qual_cd, qual_desc, user_Id, ip_Address)
            End If
        End Function
#End Region

#Region "MO Details insert update"
        'Pre_MO_cd
        Public Function MO_Details_Save(Pre_MO_Autoincrmnt_Cd As String, State_cd As String, District_Cd As String, sub_division_cd As String, PS_cd As String, work_area_cd As String,
                                 Block_Cd As String, Muni_GP_Cd As String, Ward_Mouza_Cd As String, MO_Type_Id As String, MO_FName As String, MO_MName As String, MO_LName As String, MO_Designation As String, MO_office_addr As String,
                                 MO_resi_addr As String, MO_office_po_dist As String, MO_office_po_cd As String, MO_resi_po_dist As String, MO_resi_po_cd As String, MO_MPh_no As String, MO_LPh_no As String, Joining_DT As String, Go_No_NOMO As String, Go_Dt_NOMO As String,
                                 ctgry As String, Email_Id As String, User_Email As String, DOB As String, Ai_Id As String, PAN_No As String,
                                 Bank_cd As String, Bank_Branch_District_Cd As String, Bank_branch_cd As String, Bank_acno As String, Sex As String, Marital_status As String, DeleteStatus As String,
                                  user_Id As String, ip_Address As String, EPIC_no As String, Qulification_cd As String, Marks As String, MO_Imagenam As Byte(), Qulification_Docmnt_name As Byte(),
                                 Signature As Byte(), juris_levl_cd As String, juris_levl As String, flag As Int16, resi_district_cd As String, MO_fthr_Name As String, status_flg As String) As Array
            If (flag = 1) Then
                Return mastrdb.MO_Details_Save(Pre_MO_Autoincrmnt_Cd, State_cd, District_Cd, sub_division_cd, PS_cd, work_area_cd, Block_Cd, Muni_GP_Cd, Ward_Mouza_Cd, MO_Type_Id, MO_FName, MO_MName, MO_LName, MO_Designation, MO_office_addr,
                                               MO_resi_addr, MO_office_po_dist, MO_office_po_cd, MO_resi_po_dist, MO_resi_po_cd, MO_MPh_no, MO_LPh_no, Joining_DT, Go_No_NOMO, Go_Dt_NOMO, ctgry, Email_Id, User_Email, DOB, Ai_Id, PAN_No,
                                               Bank_cd, Bank_Branch_District_Cd, Bank_branch_cd, Bank_acno, Sex, Marital_status, DeleteStatus, user_Id, ip_Address, EPIC_no, Qulification_cd, Marks, MO_Imagenam,
                                               Qulification_Docmnt_name, Signature, juris_levl_cd, juris_levl, resi_district_cd, MO_fthr_Name, status_flg)
            Else
                Return mastrdb.MO_Details_Update(Pre_MO_Autoincrmnt_Cd, State_cd, District_Cd, sub_division_cd, PS_cd, work_area_cd, Block_Cd, Muni_GP_Cd, Ward_Mouza_Cd, MO_Type_Id, MO_FName, MO_MName, MO_LName, MO_Designation, MO_office_addr,
                                               MO_resi_addr, MO_office_po_dist, MO_office_po_cd, MO_resi_po_dist, MO_resi_po_cd, MO_MPh_no, MO_LPh_no, Joining_DT, Go_No_NOMO, Go_Dt_NOMO, ctgry, Email_Id, User_Email, DOB, Ai_Id, PAN_No,
                                               Bank_cd, Bank_Branch_District_Cd, Bank_branch_cd, Bank_acno, Sex, Marital_status, DeleteStatus, user_Id, ip_Address, EPIC_no, Qulification_cd, Marks, MO_Imagenam,
                                               Qulification_Docmnt_name, Signature, juris_levl_cd, juris_levl, resi_district_cd, MO_fthr_Name, status_flg)

            End If
        End Function
#End Region

#Region "MO_workingdays_hour"
        Public Function MO_workingdays_hour(Pre_MO_Autoincrmnt_Cd As String, wrk_dyHrs As DataTable, flag As Int16) As String
            Return mastrdb.MO_workingdays_hour(Pre_MO_Autoincrmnt_Cd, wrk_dyHrs, flag)
        End Function
#End Region

#Region "MO Jurisdiction insert update"
        Public Function MO_Jurisdiction_Save(Pre_Jurisdiction_cd As String, GO_JURIS_NO As String, GO_JURIS_Dt As String, MO_cd As String, Jurisdiction_level_cd As String, Jurisdiction_level As String, User_id As String, Ip_Address As String, flag As Int16) As String
            ' If (flag = 1) Then
            Return mastrdb.MO_Jurisdiction_Save(Pre_Jurisdiction_cd, GO_JURIS_NO, GO_JURIS_Dt, MO_cd, Jurisdiction_level_cd, Jurisdiction_level, User_id, Ip_Address)
            ' Else
            'Return mastrdb.MO_Jurisdiction_Update(Pre_Jurisdiction_cd, MO_cd, Jurisdiction_level_cd, Jurisdiction_level)
            ' End If
        End Function
#End Region

#Region "MO Promotion  Insert "
        Public Function Datatable_MO_promotion(dtPromotion As DataTable, User_Id As String, IP_Address As String) As String

            Return mastrdb.Datatable_MO_promotion(dtPromotion, User_Id, IP_Address)

        End Function
#End Region

#Region "MO Status Flag Entry"
        Public Function MARREG_MO_Status_Flag(dtActive As DataTable, User_Id As String, IP_Address As String) As String

            Return mastrdb.MARREG_MO_Status_Flag(dtActive, User_Id, IP_Address)

        End Function
#End Region

#Region "MO Suspension Page"

#Region "MO Suspension  Insert "
        Public Function datatable_MO_Suspension(dtSuspension As DataTable, User_Id As String, IP_Address As String) As String

            Return mastrdb.datatable_MO_Suspension(dtSuspension, User_Id, IP_Address)

        End Function
#End Region
#Region "MO Inactive  Insert "
        Public Function datatable_MO_Inactivation(dtSuspension As DataTable, User_Id As String, IP_Address As String) As String

            Return mastrdb.datatable_MO_Inactivation(dtSuspension, User_Id, IP_Address)

        End Function
#End Region
#Region "MO Death  Insert "
        Public Function datatable_MO_Death(dtSuspension As DataTable, User_Id As String, IP_Address As String) As String

            Return mastrdb.datatable_MO_Death(dtSuspension, User_Id, IP_Address)

        End Function
#End Region

#Region "MO ACTIVe  Insert "
        Public Function datatable_MO_Active(dtMO_Activation As DataTable, User_Id As String, IP_Address As String) As String

            Return mastrdb.datatable_MO_Active(dtMO_Activation, User_Id, IP_Address)

        End Function
#End Region

#End Region

#Region "Bank insert update"
        Public Function Bank_Save(Pre_bank_cd As String, Bank_nm As String, Bank_abbr As String, User_id As String, Ip_Address As String, flag As Int16) As String
            If (flag = 1) Then
                Return mastrdb.Bank_Save(Pre_bank_cd, Bank_nm, Bank_abbr, User_id, Ip_Address)
            Else

                Return mastrdb.Bank_Update(Pre_bank_cd, Bank_nm, Bank_abbr, User_id, Ip_Address)
            End If
        End Function
#End Region

#Region "Bank Branch insert update"
        Public Function Branch_Save(Pre_brn_cd As String, State_cd As String, District_cd As String, Bank_cd As String, brnEnam As String, brnVnam As String,
                                    IFSC As String, Address As String, flag As Int16) As String
            If (flag = 1) Then
                Return mastrdb.Branch_Save(Pre_brn_cd, State_cd, District_cd, Bank_cd, brnEnam, brnVnam, IFSC, Address)
            Else
                Return mastrdb.Branch_Update(Pre_brn_cd, State_cd, District_cd, Bank_cd, brnEnam, brnVnam, IFSC, Address)
            End If
        End Function
#End Region

#Region "Applicant Receipt page "
#Region "New application GridView"
        Public Function New_Application_gridview(MO_Autoincrement_Cd As String, tag As String) As DataTable
            Return mastrdb.New_Application_gridview(MO_Autoincrement_Cd, tag)
        End Function
#End Region

#Region "New application GRN STATUS GridView"
        Public Function New_Application_GRN_gridview(MO_Autoincrement_Cd As String, tag As String) As DataTable
            Return mastrdb.New_Application_GRN_gridview(MO_Autoincrement_Cd, tag)
        End Function
#End Region



#Region "Receipt  GridView"
        Public Function Receipt_gridview(MO_Autoincrement_Cd As String, From_dt As String, To_dt As String) As DataTable
            Return mastrdb.Receipt_gridview(MO_Autoincrement_Cd, From_dt, To_dt)
        End Function
#End Region

#End Region

#Region "Application Status View"
        Public Function Application_status_view(Applicant_Cd As String, objection_cd As String) As DataTable
            Return mastrdb.Application_status_view(Applicant_Cd, objection_cd)
        End Function
#End Region

#Region "Objection Raised Page"
#Region "Objection Raised Gridview"
        Public Function objectn_raised(MO_Autoincrmnt_Cd As String) As DataTable
            Return mastrdb.objectn_raised(MO_Autoincrmnt_Cd)
        End Function
#Region "Objection Hearing Insert "
        Public Function xmlLoad_objctn_hering(DtObjection As DataTable, User_Id As String, IP_Address As String, Mac_Address As String) As String

            Return mastrdb.xmlLoad_objctn_hering(DtObjection, User_Id, IP_Address, Mac_Address)

        End Function
#End Region
#End Region
#End Region

#Region "Objector Details View"
        Public Function objector_Details_View(Objectort_Cd As String) As DataTable
            Return mastrdb.objector_Details_View(Objectort_Cd)
        End Function
#End Region

#Region "Objection Inquiry Insert"
        Public Function Objection_Inquiry_Save(Objection_Cd As String, Applicant_Cd As String, Objection_Reason As String, Attach_Evidence As String,
                                               Evidence_Proof As Byte(), Bank_Cd As String, Bank_Branch_State As String, Bank_Branch_District As String, Bank_Branch_Cd As String, Payment_Mode_Cd As String, Payment_Mode_No As String, Payment_Mode_Dt As String, Payable_At As String, Remarks As String, Status As String,
                                                 IP_Address As String, User_Id As String) As String
            Return mastrdb.Objection_Inquiry_Save(Objection_Cd, Applicant_Cd, Objection_Reason, Attach_Evidence,
                                               Evidence_Proof, Bank_Cd, Bank_Branch_State, Bank_Branch_District, Bank_Branch_Cd, Payment_Mode_Cd, Payment_Mode_No, Payment_Mode_Dt, Payable_At, Remarks, Status,
                                                 IP_Address, User_Id)
        End Function
#End Region

#Region "MO Appoinment Page"
#Region "MO appoinment Gridview"
        Public Function MO_Appoinment_gridview(MO_Autoincrement_Cd As String, from_dt As String, to_dt As String, type As String) As DataTable
            Return mastrdb.MO_Appoinment_gridview(MO_Autoincrement_Cd, from_dt, to_dt, type)
        End Function
#End Region

#End Region

#Region "MO Appoinment Insert & Applicant Status Insert"
        Public Function MO_Status_appointment_Insert(tabname As DataTable, IP_Address As String, User_Id As String, Mac_Address As String, tag As String) As String

            Return mastrdb.MO_Status_appointment_Insert(tabname, IP_Address, User_Id, Mac_Address, tag)

        End Function
#End Region

#Region "GRN Status Insert"
        Public Function GRN_Status_Insert(tabname As DataTable, IP_Address As String, User_Id As String, Mac_Address As String, tag As String) As String

            Return mastrdb.GRN_Status_Insert(tabname, IP_Address, User_Id, Mac_Address, tag)

        End Function
#End Region

#Region "Declaration Insert"
        '        Public Function Declaration_Save(App_Auto_cd As String, Mo_Autoincrement_cd As String, Declaration_Time As String, Bride_FingerPrint As String, Groom_FingerPrint As String,
        '                                         Witness1_Name As String, Witness1_Address As String, Witness1_Sign As Byte(), Witness1_FingerPrint As String, Witness2_Name As String,
        '                                         Witness2_Address As String, Witness2_Sign As Byte(), Witness2_FingerPrint As String, Witness3_Name As String, Witness3_Address As String,
        '                                         Witness3_Sign As Byte(), Witness3_FingerPrint As String, User_Id As String, IP_Address As String) As String
        '            Return mastrdb.Declaration_Save(App_Auto_cd, Mo_Autoincrement_cd, Declaration_Time, Bride_FingerPrint, Groom_FingerPrint, Witness1_Name, Witness1_Address, Witness1_Sign,
        '                                            Witness1_FingerPrint, Witness2_Name, Witness2_Address, Witness2_Sign, Witness2_FingerPrint, Witness3_Name, Witness3_Address, Witness3_Sign,
        '                                            Witness3_FingerPrint, User_Id, IP_Address)
        '        End Function
        '#End Region

        Public Function Declaration_Save(App_Auto_cd As String, Mo_Autoincrement_cd As String, Declaration_Time As String, Bride_FingerPrint As String, Groom_FingerPrint As String,
                                         Witness1_Name As String, Witness1_Address As String, Witness1_Sign As Byte(), Witness1_Adhr As String, Witness1_FingerPrint As String, Witness2_Name As String,
                                         Witness2_Address As String, Witness2_Sign As Byte(), Witness2_Adhr As String, Witness2_FingerPrint As String, Witness3_Name As String, Witness3_Address As String,
                                         Witness3_Sign As Byte(), Witness3_Adhr As String, Witness3_FingerPrint As String, User_Id As String, IP_Address As String, Mac_Address As String,
                                         Witness1_Adhr_Consent As String, Witness2_Adhr_Consent As String, Witness3_Adhr_Consent As String,
                                         doc_prf_wit1 As Byte(), doc_prf_wit2 As Byte(), doc_prf_wit3 As Byte(), doc_prf_type_wit1 As String, doc_prf_type_wit2 As String, doc_prf_type_wit3 As String, Joint_Photo As Byte()) As String
            Return mastrdb.Declaration_Save(App_Auto_cd, Mo_Autoincrement_cd, Declaration_Time, Bride_FingerPrint, Groom_FingerPrint, Witness1_Name, Witness1_Address, Witness1_Sign, Witness1_Adhr,
                                            Witness1_FingerPrint, Witness2_Name, Witness2_Address, Witness2_Sign, Witness2_Adhr, Witness2_FingerPrint, Witness3_Name, Witness3_Address, Witness3_Sign,
                                            Witness3_Adhr, Witness3_FingerPrint, User_Id, IP_Address, Mac_Address, Witness1_Adhr_Consent, Witness2_Adhr_Consent, Witness3_Adhr_Consent,
                                            doc_prf_wit1, doc_prf_wit2, doc_prf_wit3, doc_prf_type_wit1, doc_prf_type_wit2, doc_prf_type_wit3, Joint_Photo)
        End Function

        Public Function Schedule_of_Signature_Witness_Save(App_Auto_cd As String, Mo_Autoincrement_cd As String, Declaration_Time As String, Witness1_Name As String, Witness1_Address As String, Witness1_ps As String,
                                                           Witness2_Name As String, Witness2_Address As String, Witness2_ps As String, Witness3_Name As String, Witness3_Address As String, Witness3_ps As String, IP_Address As String) As String

            Return mastrdb.Schedule_of_Signature_Witness_Save(App_Auto_cd, Mo_Autoincrement_cd, Declaration_Time, Witness1_Name, Witness1_Address, Witness1_ps,
                                                           Witness2_Name, Witness2_Address, Witness2_ps, Witness3_Name, Witness3_Address, Witness3_ps, IP_Address)
        End Function

        Public Function upload_sign(App_auto_cd As String, Tag As String, Sign As Byte()) As String

            Return mastrdb.upload_sign(App_auto_cd, Tag, Sign)

        End Function

        Public Function GetApplication_witness_details(App_auto_cd As String) As DataTable
            Return mastrdb.GetApplication_witness_details(App_auto_cd)
        End Function
#End Region

#Region "Witness Edit"
        Public Function Witness_Update(App_Auto_cd As String, Witness1_Name As String, Witness1_Address As String, doc_prf_type_wit1 As String,
                                        doc_prf_wit1 As Byte(), Witness1_Sign As Byte(), tag As String) As String
            Return mastrdb.Witness_Update(App_Auto_cd, Witness1_Name, Witness1_Address, doc_prf_type_wit1, doc_prf_wit1, Witness1_Sign, tag)

        End Function
#End Region
        '18-01-2019
#Region "Additional Payment"
        Public Function AdditionalPayment_Save(App_Auto_cd As String, Mo_Autoincrement_cd As String, MARREG_Payment_Reason_cd As String, AdditionalPayment_Status As String, Print_Status As String) As String
            Return mastrdb.AdditionalPayment_Save(App_Auto_cd, Mo_Autoincrement_cd, MARREG_Payment_Reason_cd, AdditionalPayment_Status, Print_Status)
        End Function
#End Region

#Region "Generate Certificate"
        Public Function Gen_Certificate_Save(Hologram_No As String, App_Auto_Cd As String, qrcode As Byte(), Bank_Cd As String, Branch_Dist As String,
                                        Branch_Cd As String, Payment_Mode_Cd As String, Payment_Mode_No As String, Payment_Mode_Dt As String,
                                        Payable_At As String, User_Id As String, Ip_Address As String, Type As String, OTP As String) As String
            Return mastrdb.Gen_Certificate_Save(Hologram_No, App_Auto_Cd, qrcode, Bank_Cd, Branch_Dist,
                                        Branch_Cd, Payment_Mode_Cd, Payment_Mode_No, Payment_Mode_Dt,
                                        Payable_At, User_Id, Ip_Address, Type, OTP) 'OTP 05.05.2017
        End Function
#End Region

#Region "MARREG Hologram New Stock Insert "
        'Public Function Marreg_Hologram_Insert(PusrchsDate As String, Strt_hologramNo As String, End_hologramno As String) As String

        '    Return mastrdb.Marreg_Hologram_Insert(PusrchsDate, Strt_hologramNo, End_hologramno)

        'End Function
        Public Function Marreg_Holo_GO_Insert(state_cd As String, GONm As String, GODate As String, Begin_hologramNo As String, End_hologramno As String) As String

            Return mastrdb.Marreg_Holo_GO_Insert(state_cd, GONm, GODate, Begin_hologramNo, End_hologramno)

        End Function
#End Region

#Region "MARREG Hologram Stock Issue Insert "
        Public Function Marreg_Holo_Stk_Issue_Insert(User_Id As String, Ip_Address As String, GO_Cd As String, Strt_hologramno As String, End_hologramno As String) As String

            Return mastrdb.Marreg_Holo_Stk_Issue_Insert(User_Id, Ip_Address, GO_Cd, Strt_hologramno, End_hologramno)

        End Function
#End Region

#Region "MARREG Hologram Stock Count"
        Public Function Marreg_Holo_Stk_Count(state_cd As String) As String

            Return mastrdb.Marreg_Holo_Stk_Count(state_cd)

        End Function
#End Region

#Region "GO Hologram Start End View"
        Public Function GO_detail_view(GO_cd As String) As DataTable
            Return mastrdb.GO_detail_view(GO_cd)
        End Function

#End Region

#Region "Marreg Hologram Stock view Gridview"
        Public Function Marreg_Holgram_stock_view(From_dt As String, To_dt As String) As DataTable
            Return mastrdb.Marreg_Holgram_stock_view(From_dt, To_dt)
        End Function

#End Region

#Region "Scanned Record Entry"
        Public Function Scan_Recrd_Save(Groom_FNam As String, Groom_MNam As String, Groom_LNam As String, Groom_Father_Nam As String, Groom_Mother_Nam As String, Groom_DOB As String,
                                        Bride_FNam As String, Bride_MNam As String, Bride_LNam As String, Bride_Father_Nam As String, Bride_Mother_Nam As String, Bride_DOB As String,
                                        Act_cd As String, Reg_Dt As DateTime, MO_Name As String, Reg_Add As String, Reg_Dist As String, Reg_SubDiv As String, Reg_Block As String,
                                        Reg_PS As String, Reg_WA As String, Reg_MuniGp As String, Reg_VilWrd As String, Reg_PO As String, Notice As Byte(), Declrtn As Byte(),
                                        Cert As Byte(), User_Id As String, IP_Address As String) As String
            Return mastrdb.Scan_Recrd_Save(Groom_FNam, Groom_MNam, Groom_LNam, Groom_Father_Nam, Groom_Mother_Nam, Groom_DOB, Bride_FNam, Bride_MNam, Bride_LNam, Bride_Father_Nam, Bride_Mother_Nam,
                                           Bride_DOB, Act_cd, Reg_Dt, MO_Name, Reg_Add, Reg_Dist, Reg_SubDiv, Reg_Block, Reg_PS, Reg_WA, Reg_MuniGp, Reg_VilWrd, Reg_PO, Notice, Declrtn, Cert,
                                           User_Id, IP_Address)
        End Function
#End Region

#Region "RTI insert update"
        Public Function RTI_Save(RTI_Id As String, RTI_name As String, RTI_Autority As String, RTI_designation As String, RTI_addr As String, RTI_contct As String, RTIFax As String, RTIEMail As String, User_Id As String, IP_Address As String, flag As Int16, RTI_Status As String) As String
            If (flag = 1) Then
                Return mastrdb.RTI_Save(RTI_Id, RTI_name, RTI_Autority, RTI_designation, RTI_addr, RTI_contct, RTIFax, RTIEMail, User_Id, IP_Address, RTI_Status)
            Else
                Return mastrdb.RTI_Save_Update(RTI_Id, RTI_name, RTI_Autority, RTI_designation, RTI_addr, RTI_contct, RTIFax, RTIEMail, User_Id, IP_Address, RTI_Status)
            End If
        End Function
#End Region

#Region "Who's who insert update"
        Public Function Who_is_Who_Save(wh_Id As String, wh_name As String, wh_degnation As String, wh_addr As String, wh_contct As String, whFax As String, whEMail As String, User_Id As String, IP_Address As String, flag As Int16, wh_status As String, wh_image As Byte()) As String
            If (flag = 1) Then
                Return mastrdb.Who_is_Who_Save(wh_Id, wh_name, wh_degnation, wh_addr, wh_contct, whFax, whEMail, User_Id, IP_Address, wh_status, wh_image)
            Else
                Return mastrdb.Who_is_Who_Save_Update(wh_Id, wh_name, wh_degnation, wh_addr, wh_contct, whFax, whEMail, User_Id, IP_Address, wh_status, wh_image)
            End If
        End Function
#End Region

#Region "ExOfficioRGM Insert"
        Public Function ExOfficioRGM_Transfer_Save(ExOff_id As String, MO_Cd As String, ExOff_FName As String, ExOff_MName As String, ExOff_LName As String,
         GO_No As String, GO_dt As DateTime, Joining_dt As DateTime,
        Transfer_GO_No As String, Transfer_GO_dt As DateTime, Transfer_dt As DateTime,
        ExOff_Image As Byte(), ExOff_Signature As Byte(),
        doc_prf_app As Byte(), doc_prf_trans As Byte(),
        ip_address As String, designation As String) As String

            Return mastrdb.ExOfficioRGM_Transfer_Save(ExOff_id, MO_Cd, ExOff_FName, ExOff_MName, ExOff_LName,
            GO_No, GO_dt, Joining_dt,
            Transfer_GO_No, Transfer_GO_dt, Transfer_dt,
            ExOff_Image, ExOff_Signature,
            doc_prf_app, doc_prf_trans,
            ip_address, designation)

        End Function
#End Region

#Region "FAQ Insert"
        Public Function Datatable_DT_FAQ(dtFAQ As DataTable, User_Id As String, IP_Address As String) As String

            Return mastrdb.Datatable_DT_FAQ(dtFAQ, User_Id, IP_Address)

        End Function
#End Region

#Region "Circulars Details insert"
        Public Function Circular_insert(Circular_Type As String, CircularName As String, CircularDesc As String,
                                   Publish_Dt As String, CircularDoc As Byte()) As String
            Return mastrdb.Circular_insert(Circular_Type, CircularName, CircularDesc, Publish_Dt, CircularDoc)
        End Function
#End Region

#Region "Query Ans insert"
        Public Function Query_Ans_insert(Message As String, Answer As String, User_Id As String, IP_Address As String, Person_Id As String, flag As Int16) As String

            If (flag = 1) Then
                Return mastrdb.Query_Ans_insert(Message, Answer, User_Id, IP_Address, Person_Id)
            Else
                Return mastrdb.Query_Ans_update(Message, Answer, User_Id, IP_Address, Person_Id)
            End If
        End Function
#End Region

#Region "Objection Enquiry Insert"
        Public Function Objection_Enquiry_Save(Objection_Cd As String, Applicant_Cd As String, Objection_Reason As String, Attach_Evidence As String, Evidence As Byte(), Bank_Cd As String,
                                              Bank_Branch_District As String, Bank_Branch_Cd As String, Payment_Mode_Cd As String, Payment_Mode_No As String, Payment_Mode_Dt As String,
                                              Payable_At As String, Remarks As String, Status As String, IP_Address As String, User_Id As String, Mac_Address As String) As String
            Return mastrdb.Objection_Enquiry_Save(Objection_Cd, Applicant_Cd, Objection_Reason, Attach_Evidence, Evidence, Bank_Cd, Bank_Branch_District, Bank_Branch_Cd, Payment_Mode_Cd,
                                                  Payment_Mode_No, Payment_Mode_Dt, Payable_At, Remarks, Status, IP_Address, User_Id, Mac_Address)
        End Function
#End Region

#Region "GRIPS Response"
        Public Function GRIPS_Response_Insert(dept_ref_no As String, transaction_id As String, bank_transaction_id As String, bank_cd As String, bank_tran_ts As DateTime, challan_amnt As Integer, challan_ref_id As String, challan_refId_dt As DateTime, bank_tran_status As String, bank_tran_msg As String) As String
            Return mastrdb.GRIPS_Response_Insert(dept_ref_no, transaction_id, bank_transaction_id, bank_cd, bank_tran_ts, challan_amnt, challan_ref_id, challan_refId_dt, bank_tran_status, bank_tran_msg)
        End Function
#End Region

#Region "Application Details view"

        Public Function application_details_view(Applicant_cd As String, tag As String) As DataTable
            Return mastrdb.application_details_view(Applicant_cd, tag)
        End Function

#End Region

#Region "Application details view for Objector"

        Public Function application_details_view_forObjector(Applicant_cd As String) As DataTable
            Return mastrdb.application_details_view_forObjector(Applicant_cd)
        End Function

#End Region

        '06.04.2016
#Region "New Application Update"
        Public Function NewAppUpdate(App_cd As String, Groom_FNam As String, Groom_MNam As String, Groom_LNam As String, Groom_Father_Nam As String, Groom_Mother_Nam As String,
                                     Groom_dob As DateTime, Groom_Mob_Num As String, Groom_Aadhr_Num As String, Groom_Email_Id As String, Groom_bpl_status As String, Groom_occupation As String,
                                     Bride_FNam As String, Bride_MNam As String, Bride_LNam As String, Bride_Father_Nam As String, Bride_Mother_Nam As String,
                                     Bride_dob As DateTime, Bride_Mob_Num As String, Bride_Aadhr_Num As String, Bride_Email_Id As String, Bride_bpl_status As String, Bride_occupation As String,
                                     applicant_bpl_status As String, Place_Socl_Mrg_Address As String, Place_Socl_Mrg_Country As String, Place_Socl_Mrg_State As String, Place_Socl_Mrg_State_flag As String,
                                     Place_Socl_Mrg_Dist As String, Place_Socl_Mrg_SubDiv As String, Place_Socl_Mrg_Block As String, Place_Socl_Mrg_PS As String, Place_Socl_Mrg_WA As String,
                                     Place_Socl_Mrg_MuniGp As String, Place_Socl_Mrg_VilWrd As String, Place_Socl_Mrg_PO As String, Place_Socl_Mrg_Pin As String,
                                     Groom_dobprf_type As String, Groom_dob_prf As String, Groom_prst_prf_type As String, Groom_prstadd_prf As String, Groom_pmnt_prf_type As String, Groom_pmntadd_prf As String, Groom_bpl_prf As String,
                                     Bride_dobprf_type As String, Bride_dob_prf As String, Bride_prst_prf_type As String, Bride_prstadd_prf As String, Bride_pmnt_prf_type As String, Bride_pmntadd_prf As String, Bride_bpl_prf As String,
                                     gbpl_numbr As String, bbpl_numbr As String, User_Id As String, IP_Address As String, Mac_Address As String) As String
            Return mastrdb.NewAppUpdate(App_cd, Groom_FNam, Groom_MNam, Groom_LNam, Groom_Father_Nam, Groom_Mother_Nam,
                                        Groom_dob, Groom_Mob_Num, Groom_Aadhr_Num, Groom_Email_Id, Groom_bpl_status, Groom_occupation,
                                        Bride_FNam, Bride_MNam, Bride_LNam, Bride_Father_Nam, Bride_Mother_Nam,
                                        Bride_dob, Bride_Mob_Num, Bride_Aadhr_Num, Bride_Email_Id, Bride_bpl_status, Bride_occupation, applicant_bpl_status,
                                        Place_Socl_Mrg_Address, Place_Socl_Mrg_Country, Place_Socl_Mrg_State,
                                        Place_Socl_Mrg_State_flag, Place_Socl_Mrg_Dist, Place_Socl_Mrg_SubDiv,
                                        Place_Socl_Mrg_Block, Place_Socl_Mrg_PS, Place_Socl_Mrg_WA, Place_Socl_Mrg_MuniGp,
                                        Place_Socl_Mrg_VilWrd, Place_Socl_Mrg_PO, Place_Socl_Mrg_Pin,
                                        Groom_dobprf_type, Groom_dob_prf, Groom_prst_prf_type, Groom_prstadd_prf, Groom_pmnt_prf_type, Groom_pmntadd_prf, Groom_bpl_prf,
                                        Bride_dobprf_type, Bride_dob_prf, Bride_prst_prf_type, Bride_prstadd_prf, Bride_pmnt_prf_type, Bride_pmntadd_prf, Bride_bpl_prf,
                                        gbpl_numbr, bbpl_numbr, User_Id, IP_Address, Mac_Address)
        End Function

        Public Function save_cert_no_update(App_cd As String, cert_num As String, flag As String, groom_or_bride As String) As String
            Return mastrdb.save_cert_no_update(App_cd, cert_num, flag, groom_or_bride)
        End Function
#End Region

#Region "Social Marriage view"
        Public Function SclMarriage_Addr_View(App_cd As String) As Array
            Return mastrdb.SclMarriage_Addr_View(App_cd)
        End Function
#End Region

#Region "Application Address view"
        Public Function New_Appln_Addr_View(App_cd As String) As Array
            Return mastrdb.New_Appln_Addr_View(App_cd)
        End Function
#End Region
        '06.04.2016

        '30.05.2016
#Region "Save Sent Mail"
        Public Function Sent_Email_Save(state_Cd As String, District_Cd As String, Sub_Division_Cd As String, PS_Cd As String, MO_Type As String, Email_Subject As String, Email_Content As String, user_Id As String, ip_Address As String) As Array
            Return mastrdb.Sent_Email_Save(state_Cd, District_Cd, Sub_Division_Cd, PS_Cd, MO_Type, Email_Subject, Email_Content, user_Id, ip_Address)
        End Function

        Public Function MO_Sent_Email_Save(Eml_Auto_cd As String, MO_Cd As String, Status As String) As String
            Return mastrdb.MO_Sent_Email_Save(Eml_Auto_cd, MO_Cd, Status)
        End Function
#End Region
        '30.05.2016

        '20.09.2016
#Region "Save Sent SMS"
        Public Function Sent_SMS_Save(state_Cd As String, District_Cd As String, Sub_Division_Cd As String, PS_Cd As String, MO_Type As String, SMS_Content As String, user_Id As String, ip_Address As String) As Array
            Return mastrdb.Sent_SMS_Save(state_Cd, District_Cd, Sub_Division_Cd, PS_Cd, MO_Type, SMS_Content, user_Id, ip_Address)
        End Function

        Public Function MO_Sent_SMS_Save(SMS_Auto_cd As String, MO_Cd As String, Status As String) As String
            Return mastrdb.MO_Sent_SMS_Save(SMS_Auto_cd, MO_Cd, Status)
        End Function
#End Region
        'end 20.09.2016
        '02.02.2017
#Region "Generate qrcode print sticker"
        Public Function Gen_qrcode(qrcode As Byte(), MO_Autoincrmnt_Cd As String, tag As String) As DataTable
            Return mastrdb.Gen_qrcode(qrcode, MO_Autoincrmnt_Cd, tag)
        End Function
#End Region
        '02.02.2017
        '22.02.2017 anita
#Region "DA Details insert update"
        Public Function DA_Details_Save(Pre_DA_Cd As String, state_cd As String, DA_FName As String, DA_MName As String, DA_LName As String, DA_MPh_no As String, DA_LPh_no As String,
                                 Email_Id As String, User_Email As String, Design As String, DA_dos As String, juris_levl As String, user_Id As String, ip_Address As String, status_flg As String, reason As String, flag As String) As Array
            If (flag = 1) Then
                Return mastrdb.DA_Details_Save(Pre_DA_Cd, state_cd, DA_FName, DA_MName, DA_LName, DA_MPh_no, DA_LPh_no,
                                 Email_Id, User_Email, Design, DA_dos, juris_levl, user_Id, ip_Address, status_flg, reason)
            Else
                Return mastrdb.DA_Details_Update(Pre_DA_Cd, state_cd, DA_FName, DA_MName, DA_LName, DA_MPh_no, DA_LPh_no,
                                 Email_Id, User_Email, Design, DA_dos, juris_levl, user_Id, ip_Address, status_flg, reason)

            End If
        End Function
#End Region

#Region "Marriage Registration Approval Page"

#Region "load gridview"
        Public Function Regtrn_Approval_gridview(user_id As String, dist As String, subdiv As String, role_type As String, app_status As String) As DataSet
            Return mastrdb.Regtrn_Approval_gridview(user_id, dist, subdiv, role_type, app_status)
        End Function
#End Region

#Region "MO Details in DA Approval Page"
        Public Function MO_view(MO_Cd As String) As DataSet
            Return mastrdb.MO_view(MO_Cd)
        End Function
#End Region

#Region "Registration verified by DA,SO,RGM,MO Insert"
        Public Function Registration_Approval(DtVerification As DataTable, User_Nm As String, User_Id As String, IP_Address As String, Mac_Address As String, role_type As String, app_status As String) As String
            Return mastrdb.Registration_Approval(DtVerification, User_Nm, User_Id, IP_Address, Mac_Address, role_type, app_status)
        End Function
#End Region

#Region "Registration verified by SO and RGM Update with sent sms_id and email_id"
        Public Function Registration_Approval_Update(DtUpdateVeriftn As DataTable, User_Id As String, role_type As String, app_status As String) As String
            Return mastrdb.Registration_Approval_Update(DtUpdateVeriftn, User_Id, role_type, app_status)
        End Function
#End Region

#End Region

#Region "RGM/SO Details insert update"
        Public Function RGM_SO_Details_Save(Pre_RGM_SO_Cd As String, state_cd As String, RGM_SO_FName As String, RGM_SO_MName As String, RGM_SO_LName As String, RGM_SO_MPh_no As String, RGM_SO_LPh_no As String,
                                Design As String, RGM_SO_dos As String, user_Id As String, ip_Address As String, status_flg As String, reason As String, flag As String) As String
            If (flag = 1) Then
                Return mastrdb.RGM_SO_Details_Save(Pre_RGM_SO_Cd, state_cd, RGM_SO_FName, RGM_SO_MName, RGM_SO_LName, RGM_SO_MPh_no, RGM_SO_LPh_no,
                                                    Design, RGM_SO_dos, user_Id, ip_Address, status_flg, reason)
            Else
                Return mastrdb.RGM_SO_Details_Update(Pre_RGM_SO_Cd, state_cd, RGM_SO_FName, RGM_SO_MName, RGM_SO_LName, RGM_SO_MPh_no, RGM_SO_LPh_no,
                                                    Design, RGM_SO_dos, user_Id, ip_Address, status_flg, reason)
            End If
        End Function
#End Region

        'end 22.02.2017
#Region "Document upload Details view" '27.04.2018
        Public Function Document_Upload_View(App_cd As String, applicnt_type As String, doc_type As String, app_type As String) As DataTable
            Return mastrdb.Document_Upload_View(App_cd, applicnt_type, doc_type, app_type)
        End Function
#End Region

#Region "Print Cert MO"
        Public Function MO_print_status_update(MO_print_status As String, App_Auto_Cd As String, QRCODE_string As Byte()) As String
            Return mastrdb.MO_print_status_update(MO_print_status, App_Auto_Cd, QRCODE_string)
        End Function
        Public Function MO_Reprint_status_update(Aplication_No As String, Ip_Address As String) As String
            Return mastrdb.MO_Reprint_status_update(Aplication_No, Ip_Address)
        End Function
        Public Function MO_print_reprint_status(MO_print_status As String, MO_reprint_status As String, App_Auto_Cd As String) As String
            Return mastrdb.MO_print_reprint_status(MO_print_status, MO_reprint_status, App_Auto_Cd)
        End Function
#End Region

#Region "Print Correction Cert MO"
        Public Function MO_Correction_print_status_update(MO_print_status As String, App_Auto_Cd As String, QRCODE_string As Byte()) As String
            Return mastrdb.MO_Correction_print_status_update(MO_print_status, App_Auto_Cd, QRCODE_string)
        End Function
#End Region

#Region "Reprint Cert"
        Public Function MO_Reprint_status(App_Auto_Cd As String, Reprint_Reson As String, Ip_Address As String) As String
            Return mastrdb.MO_Reprint_status(App_Auto_Cd, Reprint_Reson, Ip_Address)
        End Function
#End Region

#Region "Upload Image"
        Public Function NewAppSave(gimg_byte As Byte()) As String
            Return mastrdb.NewAppSave(gimg_byte)
        End Function
#End Region

#Region "User Status"
        Public Function User_Status(User_cd As String, User_type As String) As String
            Return mastrdb.User_Status(User_cd, User_type)
        End Function
#End Region

        ' 05.04.2018
#Region "View Scanned Record"
        Public Function Scanned_Record_View(Certificate_Nbr As String, Groom_FName As String, Groom_MName As String, Groom_LName As String,
                                            Groom_DOB As String, Groom_Father_Name As String, Groom_Aadhr As String,
                                            Bride_FName As String, Bride_MName As String, Bride_LName As String,
                                            Bride_DOB As String, Bride_Father_Name As String, Bride_Aadhr As String) As DataSet
            Return mastrdb.Scanned_Record_View(Certificate_Nbr, Groom_FName, Groom_MName, Groom_LName, Groom_DOB, Groom_Father_Name, Groom_Aadhr,
                                               Bride_FName, Bride_MName, Bride_LName, Bride_DOB, Bride_Father_Name, Bride_Aadhr)
        End Function
#End Region
        'end 05.04.2018

        'RGMCertificateSearch start 21.11.2018
#Region "Print Certificate Search by Groom and Bride Name"
        Public Function Cert_Serch_grmBride(groom_name As String, bride_name As String, groom_father_name As String, bride_father_name As String) As DataSet
            Return mastrdb.Cert_Serch_grmBride(groom_name, bride_name, groom_father_name, bride_father_name)
        End Function
#End Region
        'end 21.11.2018

        'RGMCertificateSearch start 21.11.2018
#Region "MO Serach by MO CD"
        Public Function mo_search_by_mocd(mo_cd As String) As DataSet
            Return mastrdb.mo_search_by_mocd(mo_cd)
        End Function
#End Region
        'end 21.11.2018
#Region "Pending Application And Action Against"
        Public Function PendingApplicationDetails() As DataSet
            Return mastrdb.PendingApplicationDetails()
        End Function

        Public Function Remove_Unused_Application(UserId As String, ip_add As String, Mac_Address As String, removeString As String) As DataSet
            Return mastrdb.Remove_Unused_Application(UserId, ip_add, Mac_Address, removeString)
        End Function
#End Region

#Region "District Registrar Insert"
        Public Function District_Registrar_Entry(Country_cd As String, state_Cd As String, District_Cd As String, Email As String, Registrar_type As String, delete_Status As String, user_Id As String, ip_Address As String, type As String, tag As String) As String
            Return mastrdb.District_Registrar_Entry(Country_cd, state_Cd, District_Cd, Email, Registrar_type, delete_Status, user_Id, ip_Address, type, tag)
        End Function
#End Region

#Region "Active Mo view"
        Public Function RGM_ActiveMO_View(district As String, MO_Type_Cd As String) As DataSet
            Return mastrdb.RGM_ActiveMO_View(district, MO_Type_Cd)
        End Function
#End Region

#Region "DA Check Status Insert"   '12.06.2019


        Function DA_Check_Status(app_auto_cd As String, da_cd As String, app_status As String, remarks As String, Ip_Address As String, sts_flag As String) As String

            Return mastrdb.DA_Check_Status(app_auto_cd, da_cd, app_status, remarks, Ip_Address, sts_flag)

        End Function
#End Region

#Region "DA VERIFICATION GRIDVIEW"
        Public Function DA_Verification_gridview(DA_Cd As String, District_cd As String, mo_cd As String, frmdt As String, todt As String, app_no As String) As DataTable
            Return mastrdb.DA_Verification_gridview(DA_Cd, District_cd, mo_cd, frmdt, todt, app_no)
        End Function
#End Region

#Region "joint Photo Update"
        Public Function jointPhoto_update(App_auto_cd As String, joint_photo As Byte()) As String
            Return mastrdb.jointPhoto_update(App_auto_cd, joint_photo)
        End Function
#End Region

#Region "GRN SEARCH"
        Public Function Grn_search_test(grn_no As String) As DataTable
            Return mastrdb.Grn_search_test(grn_no)
        End Function
#End Region

#Region "Correction_Request"
        Public Function Mo_Correction_Request(Application_No As String, Mo_Autoincrement_cd As String, No_of_Correction As String, Mail_Date As String, Mail_Sent_to As String, Correction_Place As String,
                                                                  Incorrect As String, Correct As String, Requested_by As String, Correction_Place2 As String, Incorrect2 As String, Correct2 As String, Doc_Upload_Req As String, IP_Address As String) As String

            Return mastrdb.Mo_Correction_Request(Application_No, Mo_Autoincrement_cd, No_of_Correction, Mail_Date, Mail_Sent_to, Correction_Place, Incorrect, Correct, Requested_by, Correction_Place2, Incorrect2, Correct2, Doc_Upload_Req, IP_Address)
        End Function
#End Region

#Region "Correction Fields"
        Public Function Correction_Field_entry(Correction_field_id As String) As String
            Return mastrdb.Correction_Field_entry(Correction_field_id)
        End Function
        Public Function Correction_Field_display() As String
            Return mastrdb.Correction_Field_display()
        End Function
        Public Function Get_PhotoSign(Draft_Cd As String) As DataTable
            Return mastrdb.Get_PhotoSign(Draft_Cd)
        End Function

        Public Function update_photo_sign(App_Auto_cd As String, gr_photo As String, g_photo As Byte(), gr_sign As String, g_sign As Byte(), br_photo As String, b_photo As Byte(), br_sign As String, b_sign As Byte(), wt1_signn As String, wt1_sign As Byte(), wt2_signn As String, wt2_sign As Byte(), wt3_signn As String, wt3_sign As Byte(), grn_amount As String, grn_no As String, comment As String, file_path As String) As String

            Return mastrdb.update_photo_sign(App_Auto_cd, gr_photo, g_photo, gr_sign, g_sign, br_photo, b_photo, br_sign, b_sign, wt1_signn, wt1_sign, wt2_signn, wt2_sign, wt3_signn, wt3_sign, grn_amount, grn_no, comment, file_path)

        End Function
#End Region

#Region "Update Details"
        Public Function Exofficio_Details_Update(Mo_cd As String, Mobile_no As String, Date_of_Birth As String) As String
            Return mastrdb.Exofficio_Details_Update(Mo_cd, Mobile_no, Date_of_Birth)
        End Function
#End Region
#Region "Fecth Bride Groom Details"
        Public Function FecthBrideGroomDetails(App_Auto_Cd As String) As DataTable
            Return mastrdb.FecthBrideGroomDetails(App_Auto_Cd)
        End Function
#End Region
#Region "Store Marriage Certificate"
        Public Function Save_Certificate(file_byte As Byte(), app_Auto_Cd As String) As String
            Return mastrdb.Save_Certificate(file_byte, app_Auto_Cd)
        End Function
#End Region
    End Class
End Namespace