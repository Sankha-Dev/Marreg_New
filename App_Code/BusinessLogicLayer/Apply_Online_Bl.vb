Imports Microsoft.VisualBasic
Imports MARREG.App_Code.DataAccessLayer
Imports System.Data
Imports System.Data.SqlClient

Namespace MARREG.App_Code.BusinessLogicLayer
    Public Class Apply_Online_Bl
        Dim apply_online_db As New Apply_Online_Db()

#Region "FUNCTION"

#Region "Insert Through XML"
        Public Function XMLLoad1(tabname As DataSet) As String
            Return apply_online_db.xmlLoad(tabname)
        End Function
#End Region

#Region "MO View in Apply Online"
        Public Function ViewMO(act As String, district As String, ps As String, subdivision As String, block As String, Work_area As String, Muni_gp As String, Word_Mouza As String) As DataSet
            Return apply_online_db.ViewMO(act, district, ps, subdivision, block, Work_area, Muni_gp, Word_Mouza)
        End Function

        Public Function ViewMOUpdate(act As String, district As String, ps As String, subdivision As String, block As String, Work_area As String, Muni_gp As String, Word_Mouza As String) As DataSet
            Return apply_online_db.ViewMOUpdated(act, district, ps, subdivision, block, Work_area, Muni_gp, Word_Mouza)
        End Function
#End Region

#Region "NewApp Insert"

#Region "Bride & Groom Insert"
#Region "Without draft"
        'Public Function NewAppSave(Form_id As String, Act_cd As String, Groom_FNam As String, Groom_MNam As String, Groom_LNam As String, Groom_Father_Nam As String, Groom_Mother_Nam As String,
        '                           Groom_Present_Address As String, Groom_PrsntAdd_Country As String, Groom_PrsntAdd_State As String, Groom_PrsntAdd_State_flag As String, Groom_PrsntAdd_Dist As String,
        '                           Groom_PrsntAdd_SubDiv As String, Groom_PrsntAdd_Block As String, Groom_PrsntAdd_PS As String, Groom_PrsntAdd_WA As String, Groom_PrsntAdd_MuniGp As String,
        '                           Groom_PrsntAdd_VilWrd As String, Groom_PrsntAdd_PO As String, Groom_PrsntAdd_Pin As String, Groom_Permanent_Address As String, Groom_PrmntAdd_Country As String,
        '                           Groom_PrmntAdd_State As String, Groom_PrmntAdd_State_flag As String, Groom_PrmntAdd_Dist As String, Groom_PrmntAdd_SubDiv As String, Groom_PrmntAdd_Block As String,
        '                           Groom_PrmntAdd_PS As String, Groom_PrmntAdd_WA As String, Groom_PrmntAdd_MuniGp As String, Groom_PrmntAdd_VilWrd As String, Groom_PrmntAdd_PO As String,
        '                           Groom_PrmntAdd_Pin As String, Groom_Mob_Num As String, Groom_dob As DateTime, Groom_Email_Id As String, Groom_Religion As String, Groom_Aadhr_Num As String,
        '                           Groom_occupation As String, Groom_maritial_status As String, Groom_Prev_Mar_Dt As String, Groom_Photo As Byte(), Groom_Sign As Byte(), Groom_Proof_PrsntAdd_Type As String,
        '                           Groom_Proof_PrsntAdd As Byte(), Groom_Proof_PrmntAdd_Type As String, Groom_Proof_PrmntAdd As Byte(), Groom_Proof_DOB_Type As String, Groom_Proof_DOB As Byte(),
        '                           Bride_FNam As String, Bride_MNam As String, Bride_LNam As String, Bride_Father_Nam As String, Bride_Mother_Nam As String, Bride_Present_Address As String,
        '                           Bride_PrsntAdd_Country As String, Bride_PrsntAdd_State As String, Bride_PrsntAdd_State_flag As String, Bride_PrsntAdd_Dist As String, Bride_PrsntAdd_SubDiv As String,
        '                           Bride_PrsntAdd_Block As String, Bride_PrsntAdd_PS As String, Bride_PrsntAdd_WA As String, Bride_PrsntAdd_MuniGp As String, Bride_PrsntAdd_VilWrd As String,
        '                           Bride_PrsntAdd_PO As String, Bride_PrsntAdd_Pin As String, Bride_Permanent_Address As String, Bride_PrmntAdd_Country As String, Bride_PrmntAdd_State As String,
        '                           Bride_PrmntAdd_State_flag As String, Bride_PrmntAdd_Dist As String, Bride_PrmntAdd_SubDiv As String, Bride_PrmntAdd_Block As String, Bride_PrmntAdd_PS As String,
        '                           Bride_PrmntAdd_WA As String, Bride_PrmntAdd_MuniGp As String, Bride_PrmntAdd_VilWrd As String, Bride_PrmntAdd_PO As String, Bride_PrmntAdd_Pin As String,
        '                           Bride_Mob_Num As String, Bride_dob As DateTime, Bride_Email_Id As String, Bride_Religion As String, Bride_Aadhr_Num As String, Bride_Occupation As String,
        '                           Bride_maritial_status As String, Bride_Prev_Mar_Dt As String, Bride_Photo As Byte(), Bride_Sign As Byte(), Bride_Proof_PrsntAdd_Type As String, Bride_Proof_PrsntAdd As Byte(),
        '                           Bride_Proof_PrmntAdd_Type As String, Bride_Proof_PrmntAdd As Byte(), Bride_Proof_DOB_Type As String, Bride_Proof_DOB As Byte(),
        '                           MO_Autoincrement_cd As String, Place_Reg_Address As String, Place_Reg_State As String, Place_Reg_Dist As String, Place_Reg_SubDiv As String, Place_Reg_Block As String,
        '                           Place_Reg_PS As String, Place_Reg_WA As String, Place_Reg_MuniGp As String, Place_Reg_VilWrd As String, Place_Reg_PO As String, Place_Socl_Mrg_Address As String,
        '                           Place_Socl_Mrg_Country As String, Place_Socl_Mrg_State As String, Place_Socl_Mrg_State_flag As String, Place_Socl_Mrg_Dist As String, Place_Socl_Mrg_SubDiv As String,
        '                           Place_Socl_Mrg_Block As String, Place_Socl_Mrg_PS As String, Place_Socl_Mrg_WA As String, Place_Socl_Mrg_MuniGp As String, Place_Socl_Mrg_VilWrd As String,
        '                           Place_Socl_Mrg_PO As String, Place_Socl_Mrg_Pin As String, Socl_Mrg_Dt As String, Mrg_Invi_Card As Byte(), Payment_flag As String, Place_Reg As String, Time_Reg As String, IP_Address As String,
        '                           grm_bpl_prf As Byte(), bride_bpl_prf As Byte(), gbpl_serial_cd As String, bbpl_serial_cd As String,
        '                           grm_bpl_status As String, bride_bpl_status As String, applicant_bpl_status As String, gadhrconsent As String, badhrconsent As String, Groom_Nationality As String, Bride_Nationality As String) As Array

        '    Return apply_online_db.NewAppSave(Form_id, Act_cd, Groom_FNam, Groom_MNam, Groom_LNam, Groom_Father_Nam, Groom_Mother_Nam, Groom_Present_Address, Groom_PrsntAdd_Country, Groom_PrsntAdd_State, Groom_PrsntAdd_State_flag,
        '                                      Groom_PrsntAdd_Dist, Groom_PrsntAdd_SubDiv, Groom_PrsntAdd_Block, Groom_PrsntAdd_PS, Groom_PrsntAdd_WA, Groom_PrsntAdd_MuniGp, Groom_PrsntAdd_VilWrd, Groom_PrsntAdd_PO,
        '                                      Groom_PrsntAdd_Pin, Groom_Permanent_Address, Groom_PrmntAdd_Country, Groom_PrmntAdd_State, Groom_PrmntAdd_State_flag, Groom_PrmntAdd_Dist, Groom_PrmntAdd_SubDiv,
        '                                      Groom_PrmntAdd_Block, Groom_PrmntAdd_PS, Groom_PrmntAdd_WA, Groom_PrmntAdd_MuniGp, Groom_PrmntAdd_VilWrd, Groom_PrmntAdd_PO, Groom_PrmntAdd_Pin, Groom_Mob_Num, Groom_dob,
        '                                      Groom_Email_Id, Groom_Religion, Groom_Aadhr_Num, Groom_occupation, Groom_maritial_status, Groom_Prev_Mar_Dt, Groom_Photo, Groom_Sign, Groom_Proof_PrsntAdd_Type, Groom_Proof_PrsntAdd,
        '                                      Groom_Proof_PrmntAdd_Type, Groom_Proof_PrmntAdd, Groom_Proof_DOB_Type, Groom_Proof_DOB,
        '                                      Bride_FNam, Bride_MNam, Bride_LNam, Bride_Father_Nam, Bride_Mother_Nam, Bride_Present_Address, Bride_PrsntAdd_Country, Bride_PrsntAdd_State, Bride_PrsntAdd_State_flag, Bride_PrsntAdd_Dist,
        '                                      Bride_PrsntAdd_SubDiv, Bride_PrsntAdd_Block, Bride_PrsntAdd_PS, Bride_PrsntAdd_WA, Bride_PrsntAdd_MuniGp, Bride_PrsntAdd_VilWrd, Bride_PrsntAdd_PO, Bride_PrsntAdd_Pin, Bride_Permanent_Address,
        '                                      Bride_PrmntAdd_Country, Bride_PrmntAdd_State, Bride_PrmntAdd_State_flag, Bride_PrmntAdd_Dist, Bride_PrmntAdd_SubDiv, Bride_PrmntAdd_Block, Bride_PrmntAdd_PS, Bride_PrmntAdd_WA,
        '                                      Bride_PrmntAdd_MuniGp, Bride_PrmntAdd_VilWrd, Bride_PrmntAdd_PO, Bride_PrmntAdd_Pin, Bride_Mob_Num, Bride_dob, Bride_Email_Id, Bride_Religion, Bride_Aadhr_Num, Bride_Occupation,
        '                                      Bride_maritial_status, Bride_Prev_Mar_Dt, Bride_Photo, Bride_Sign, Bride_Proof_PrsntAdd_Type, Bride_Proof_PrsntAdd, Bride_Proof_PrmntAdd_Type, Bride_Proof_PrmntAdd, Bride_Proof_DOB_Type,
        '                                      Bride_Proof_DOB,
        '                                      MO_Autoincrement_cd, Place_Reg_Address, Place_Reg_State, Place_Reg_Dist, Place_Reg_SubDiv, Place_Reg_Block, Place_Reg_PS, Place_Reg_WA, Place_Reg_MuniGp, Place_Reg_VilWrd, Place_Reg_PO,
        '                                      Place_Socl_Mrg_Address, Place_Socl_Mrg_Country, Place_Socl_Mrg_State, Place_Socl_Mrg_State_flag, Place_Socl_Mrg_Dist, Place_Socl_Mrg_SubDiv, Place_Socl_Mrg_Block, Place_Socl_Mrg_PS,
        '                                      Place_Socl_Mrg_WA, Place_Socl_Mrg_MuniGp, Place_Socl_Mrg_VilWrd, Place_Socl_Mrg_PO, Place_Socl_Mrg_Pin, Socl_Mrg_Dt, Mrg_Invi_Card, Payment_flag, Place_Reg, Time_Reg, IP_Address,
        '                                      grm_bpl_prf, bride_bpl_prf, gbpl_serial_cd, bbpl_serial_cd, grm_bpl_status, bride_bpl_status, applicant_bpl_status, gadhrconsent, badhrconsent, Groom_Nationality, Bride_Nationality)
        'End Function
#End Region

#Region "With proof As bytearray"
        '26.03.2018
        'Public Function NewAppSave(Draft_No As String, Form_id As String, Act_cd As String, Groom_FNam As String, Groom_MNam As String, Groom_LNam As String, Groom_Father_Nam As String, Groom_Mother_Nam As String,
        '                           Groom_Present_Address As String, Groom_PrsntAdd_Country As String, Groom_PrsntAdd_State As String, Groom_PrsntAdd_State_flag As String, Groom_PrsntAdd_Dist As String,
        '                           Groom_PrsntAdd_SubDiv As String, Groom_PrsntAdd_Block As String, Groom_PrsntAdd_PS As String, Groom_PrsntAdd_WA As String, Groom_PrsntAdd_MuniGp As String,
        '                           Groom_PrsntAdd_VilWrd As String, Groom_PrsntAdd_PO As String, Groom_PrsntAdd_Pin As String, Groom_Permanent_Address As String, Groom_PrmntAdd_Country As String,
        '                           Groom_PrmntAdd_State As String, Groom_PrmntAdd_State_flag As String, Groom_PrmntAdd_Dist As String, Groom_PrmntAdd_SubDiv As String, Groom_PrmntAdd_Block As String,
        '                           Groom_PrmntAdd_PS As String, Groom_PrmntAdd_WA As String, Groom_PrmntAdd_MuniGp As String, Groom_PrmntAdd_VilWrd As String, Groom_PrmntAdd_PO As String,
        '                           Groom_PrmntAdd_Pin As String, Groom_Mob_Num As String, Groom_dob As DateTime, Groom_Email_Id As String, Groom_Religion As String, Groom_Aadhr_Num As String,
        '                           Groom_occupation As String, Groom_maritial_status As String, Groom_Prev_Mar_Dt As String, Groom_Photo As Byte(), Groom_Sign As Byte(), Groom_Proof_PrsntAdd_Type As String,
        '                           Groom_Proof_PrsntAdd As Byte(), Groom_Proof_PrmntAdd_Type As String, Groom_Proof_PrmntAdd As Byte(), Groom_Proof_DOB_Type As String, Groom_Proof_DOB As Byte(),
        '                           Bride_FNam As String, Bride_MNam As String, Bride_LNam As String, Bride_Father_Nam As String, Bride_Mother_Nam As String, Bride_Present_Address As String,
        '                           Bride_PrsntAdd_Country As String, Bride_PrsntAdd_State As String, Bride_PrsntAdd_State_flag As String, Bride_PrsntAdd_Dist As String, Bride_PrsntAdd_SubDiv As String,
        '                           Bride_PrsntAdd_Block As String, Bride_PrsntAdd_PS As String, Bride_PrsntAdd_WA As String, Bride_PrsntAdd_MuniGp As String, Bride_PrsntAdd_VilWrd As String,
        '                           Bride_PrsntAdd_PO As String, Bride_PrsntAdd_Pin As String, Bride_Permanent_Address As String, Bride_PrmntAdd_Country As String, Bride_PrmntAdd_State As String,
        '                           Bride_PrmntAdd_State_flag As String, Bride_PrmntAdd_Dist As String, Bride_PrmntAdd_SubDiv As String, Bride_PrmntAdd_Block As String, Bride_PrmntAdd_PS As String,
        '                           Bride_PrmntAdd_WA As String, Bride_PrmntAdd_MuniGp As String, Bride_PrmntAdd_VilWrd As String, Bride_PrmntAdd_PO As String, Bride_PrmntAdd_Pin As String,
        '                           Bride_Mob_Num As String, Bride_dob As DateTime, Bride_Email_Id As String, Bride_Religion As String, Bride_Aadhr_Num As String, Bride_Occupation As String,
        '                           Bride_maritial_status As String, Bride_Prev_Mar_Dt As String, Bride_Photo As Byte(), Bride_Sign As Byte(), Bride_Proof_PrsntAdd_Type As String, Bride_Proof_PrsntAdd As Byte(),
        '                           Bride_Proof_PrmntAdd_Type As String, Bride_Proof_PrmntAdd As Byte(), Bride_Proof_DOB_Type As String, Bride_Proof_DOB As Byte(),
        '                           MO_Autoincrement_cd As String, Place_Reg_Address As String, Place_Reg_State As String, Place_Reg_Dist As String, Place_Reg_SubDiv As String, Place_Reg_Block As String,
        '                           Place_Reg_PS As String, Place_Reg_WA As String, Place_Reg_MuniGp As String, Place_Reg_VilWrd As String, Place_Reg_PO As String, Place_Socl_Mrg_Address As String,
        '                           Place_Socl_Mrg_Country As String, Place_Socl_Mrg_State As String, Place_Socl_Mrg_State_flag As String, Place_Socl_Mrg_Dist As String, Place_Socl_Mrg_SubDiv As String,
        '                           Place_Socl_Mrg_Block As String, Place_Socl_Mrg_PS As String, Place_Socl_Mrg_WA As String, Place_Socl_Mrg_MuniGp As String, Place_Socl_Mrg_VilWrd As String,
        '                           Place_Socl_Mrg_PO As String, Place_Socl_Mrg_Pin As String, Socl_Mrg_Dt As String, Mrg_Invi_Card As Byte(), Payment_flag As String, Place_Reg As String, Time_Reg As String, IP_Address As String,
        '                           grm_bpl_prf As Byte(), bride_bpl_prf As Byte(), gbpl_serial_cd As String, bbpl_serial_cd As String,
        '                           grm_bpl_status As String, bride_bpl_status As String, applicant_bpl_status As String, gadhrconsent As String, badhrconsent As String, Groom_Nationality As String, Bride_Nationality As String) As Array

        '    Return apply_online_db.NewAppSave(Draft_No, Form_id, Act_cd, Groom_FNam, Groom_MNam, Groom_LNam, Groom_Father_Nam, Groom_Mother_Nam, Groom_Present_Address, Groom_PrsntAdd_Country, Groom_PrsntAdd_State, Groom_PrsntAdd_State_flag,
        '                                      Groom_PrsntAdd_Dist, Groom_PrsntAdd_SubDiv, Groom_PrsntAdd_Block, Groom_PrsntAdd_PS, Groom_PrsntAdd_WA, Groom_PrsntAdd_MuniGp, Groom_PrsntAdd_VilWrd, Groom_PrsntAdd_PO,
        '                                      Groom_PrsntAdd_Pin, Groom_Permanent_Address, Groom_PrmntAdd_Country, Groom_PrmntAdd_State, Groom_PrmntAdd_State_flag, Groom_PrmntAdd_Dist, Groom_PrmntAdd_SubDiv,
        '                                      Groom_PrmntAdd_Block, Groom_PrmntAdd_PS, Groom_PrmntAdd_WA, Groom_PrmntAdd_MuniGp, Groom_PrmntAdd_VilWrd, Groom_PrmntAdd_PO, Groom_PrmntAdd_Pin, Groom_Mob_Num, Groom_dob,
        '                                      Groom_Email_Id, Groom_Religion, Groom_Aadhr_Num, Groom_occupation, Groom_maritial_status, Groom_Prev_Mar_Dt, Groom_Photo, Groom_Sign, Groom_Proof_PrsntAdd_Type, Groom_Proof_PrsntAdd,
        '                                      Groom_Proof_PrmntAdd_Type, Groom_Proof_PrmntAdd, Groom_Proof_DOB_Type, Groom_Proof_DOB,
        '                                      Bride_FNam, Bride_MNam, Bride_LNam, Bride_Father_Nam, Bride_Mother_Nam, Bride_Present_Address, Bride_PrsntAdd_Country, Bride_PrsntAdd_State, Bride_PrsntAdd_State_flag, Bride_PrsntAdd_Dist,
        '                                      Bride_PrsntAdd_SubDiv, Bride_PrsntAdd_Block, Bride_PrsntAdd_PS, Bride_PrsntAdd_WA, Bride_PrsntAdd_MuniGp, Bride_PrsntAdd_VilWrd, Bride_PrsntAdd_PO, Bride_PrsntAdd_Pin, Bride_Permanent_Address,
        '                                      Bride_PrmntAdd_Country, Bride_PrmntAdd_State, Bride_PrmntAdd_State_flag, Bride_PrmntAdd_Dist, Bride_PrmntAdd_SubDiv, Bride_PrmntAdd_Block, Bride_PrmntAdd_PS, Bride_PrmntAdd_WA,
        '                                      Bride_PrmntAdd_MuniGp, Bride_PrmntAdd_VilWrd, Bride_PrmntAdd_PO, Bride_PrmntAdd_Pin, Bride_Mob_Num, Bride_dob, Bride_Email_Id, Bride_Religion, Bride_Aadhr_Num, Bride_Occupation,
        '                                      Bride_maritial_status, Bride_Prev_Mar_Dt, Bride_Photo, Bride_Sign, Bride_Proof_PrsntAdd_Type, Bride_Proof_PrsntAdd, Bride_Proof_PrmntAdd_Type, Bride_Proof_PrmntAdd, Bride_Proof_DOB_Type,
        '                                      Bride_Proof_DOB,
        '                                      MO_Autoincrement_cd, Place_Reg_Address, Place_Reg_State, Place_Reg_Dist, Place_Reg_SubDiv, Place_Reg_Block, Place_Reg_PS, Place_Reg_WA, Place_Reg_MuniGp, Place_Reg_VilWrd, Place_Reg_PO,
        '                                      Place_Socl_Mrg_Address, Place_Socl_Mrg_Country, Place_Socl_Mrg_State, Place_Socl_Mrg_State_flag, Place_Socl_Mrg_Dist, Place_Socl_Mrg_SubDiv, Place_Socl_Mrg_Block, Place_Socl_Mrg_PS,
        '                                      Place_Socl_Mrg_WA, Place_Socl_Mrg_MuniGp, Place_Socl_Mrg_VilWrd, Place_Socl_Mrg_PO, Place_Socl_Mrg_Pin, Socl_Mrg_Dt, Mrg_Invi_Card, Payment_flag, Place_Reg, Time_Reg, IP_Address,
        '                                      grm_bpl_prf, bride_bpl_prf, gbpl_serial_cd, bbpl_serial_cd, grm_bpl_status, bride_bpl_status, applicant_bpl_status, gadhrconsent, badhrconsent, Groom_Nationality, Bride_Nationality)
        'End Function
        ' end 26.03.2018
#End Region

        Public Function NewAppSave(Draft_No As String, Form_id As String, Act_cd As String, Groom_FNam As String, Groom_MNam As String, Groom_LNam As String, Groom_Father_Nam As String, Groom_Mother_Nam As String,
                                   Groom_Present_Address As String, Groom_PrsntAdd_Country As String, Groom_PrsntAdd_State As String, Groom_PrsntAdd_State_flag As String, Groom_PrsntAdd_Dist As String,
                                   Groom_PrsntAdd_SubDiv As String, Groom_PrsntAdd_Block As String, Groom_PrsntAdd_PS As String, Groom_PrsntAdd_WA As String, Groom_PrsntAdd_MuniGp As String,
                                   Groom_PrsntAdd_VilWrd As String, Groom_PrsntAdd_PO As String, Groom_PrsntAdd_Pin As String, Groom_Permanent_Address As String, Groom_PrmntAdd_Country As String,
                                   Groom_PrmntAdd_State As String, Groom_PrmntAdd_State_flag As String, Groom_PrmntAdd_Dist As String, Groom_PrmntAdd_SubDiv As String, Groom_PrmntAdd_Block As String,
                                   Groom_PrmntAdd_PS As String, Groom_PrmntAdd_WA As String, Groom_PrmntAdd_MuniGp As String, Groom_PrmntAdd_VilWrd As String, Groom_PrmntAdd_PO As String,
                                   Groom_PrmntAdd_Pin As String, Groom_Mob_Num As String, Groom_dob As DateTime, Groom_Email_Id As String, Groom_Religion As String, Groom_Aadhr_Num As String,
                                   Groom_occupation As String, Groom_maritial_status As String, Groom_Prev_Mar_Dt As String, Groom_Photo As Byte(), Groom_Sign As Byte(), Groom_Proof_PrsntAdd_Type As String,
                                   Groom_Proof_PrmntAdd_Type As String, Groom_Proof_DOB_Type As String,
                                   Bride_FNam As String, Bride_MNam As String, Bride_LNam As String, Bride_Father_Nam As String, Bride_Mother_Nam As String, Bride_Present_Address As String,
                                   Bride_PrsntAdd_Country As String, Bride_PrsntAdd_State As String, Bride_PrsntAdd_State_flag As String, Bride_PrsntAdd_Dist As String, Bride_PrsntAdd_SubDiv As String,
                                   Bride_PrsntAdd_Block As String, Bride_PrsntAdd_PS As String, Bride_PrsntAdd_WA As String, Bride_PrsntAdd_MuniGp As String, Bride_PrsntAdd_VilWrd As String,
                                   Bride_PrsntAdd_PO As String, Bride_PrsntAdd_Pin As String, Bride_Permanent_Address As String, Bride_PrmntAdd_Country As String, Bride_PrmntAdd_State As String,
                                   Bride_PrmntAdd_State_flag As String, Bride_PrmntAdd_Dist As String, Bride_PrmntAdd_SubDiv As String, Bride_PrmntAdd_Block As String, Bride_PrmntAdd_PS As String,
                                   Bride_PrmntAdd_WA As String, Bride_PrmntAdd_MuniGp As String, Bride_PrmntAdd_VilWrd As String, Bride_PrmntAdd_PO As String, Bride_PrmntAdd_Pin As String,
                                   Bride_Mob_Num As String, Bride_dob As DateTime, Bride_Email_Id As String, Bride_Religion As String, Bride_Aadhr_Num As String, Bride_Occupation As String,
                                   Bride_maritial_status As String, Bride_Prev_Mar_Dt As String, Bride_Photo As Byte(), Bride_Sign As Byte(), Bride_Proof_PrsntAdd_Type As String,
                                   Bride_Proof_PrmntAdd_Type As String, Bride_Proof_DOB_Type As String,
                                   MO_Autoincrement_cd As String, Place_Reg_Address As String, Place_Reg_State As String, Place_Reg_Dist As String, Place_Reg_SubDiv As String, Place_Reg_Block As String,
                                   Place_Reg_PS As String, Place_Reg_WA As String, Place_Reg_MuniGp As String, Place_Reg_VilWrd As String, Place_Reg_PO As String, Place_Socl_Mrg_Address As String,
                                   Place_Socl_Mrg_Country As String, Place_Socl_Mrg_State As String, Place_Socl_Mrg_State_flag As String, Place_Socl_Mrg_Dist As String, Place_Socl_Mrg_SubDiv As String,
                                   Place_Socl_Mrg_Block As String, Place_Socl_Mrg_PS As String, Place_Socl_Mrg_WA As String, Place_Socl_Mrg_MuniGp As String, Place_Socl_Mrg_VilWrd As String,
                                   Place_Socl_Mrg_PO As String, Place_Socl_Mrg_Pin As String, Socl_Mrg_Dt As String, Mrg_Invi_Card As Byte(), Payment_flag As String, Place_Reg As String, Time_Reg As String, IP_Address As String,
                                   gbpl_serial_cd As String, bbpl_serial_cd As String,
                                   grm_bpl_status As String, bride_bpl_status As String, applicant_bpl_status As String, gadhrconsent As String, badhrconsent As String, Groom_Nationality As String, Bride_Nationality As String) As Array

            Return apply_online_db.NewAppSave(Draft_No, Form_id, Act_cd, Groom_FNam, Groom_MNam, Groom_LNam, Groom_Father_Nam, Groom_Mother_Nam, Groom_Present_Address, Groom_PrsntAdd_Country, Groom_PrsntAdd_State, Groom_PrsntAdd_State_flag,
                                              Groom_PrsntAdd_Dist, Groom_PrsntAdd_SubDiv, Groom_PrsntAdd_Block, Groom_PrsntAdd_PS, Groom_PrsntAdd_WA, Groom_PrsntAdd_MuniGp, Groom_PrsntAdd_VilWrd, Groom_PrsntAdd_PO,
                                              Groom_PrsntAdd_Pin, Groom_Permanent_Address, Groom_PrmntAdd_Country, Groom_PrmntAdd_State, Groom_PrmntAdd_State_flag, Groom_PrmntAdd_Dist, Groom_PrmntAdd_SubDiv,
                                              Groom_PrmntAdd_Block, Groom_PrmntAdd_PS, Groom_PrmntAdd_WA, Groom_PrmntAdd_MuniGp, Groom_PrmntAdd_VilWrd, Groom_PrmntAdd_PO, Groom_PrmntAdd_Pin, Groom_Mob_Num, Groom_dob,
                                              Groom_Email_Id, Groom_Religion, Groom_Aadhr_Num, Groom_occupation, Groom_maritial_status, Groom_Prev_Mar_Dt, Groom_Photo, Groom_Sign, Groom_Proof_PrsntAdd_Type,
                                              Groom_Proof_PrmntAdd_Type, Groom_Proof_DOB_Type,
                                              Bride_FNam, Bride_MNam, Bride_LNam, Bride_Father_Nam, Bride_Mother_Nam, Bride_Present_Address, Bride_PrsntAdd_Country, Bride_PrsntAdd_State, Bride_PrsntAdd_State_flag, Bride_PrsntAdd_Dist,
                                              Bride_PrsntAdd_SubDiv, Bride_PrsntAdd_Block, Bride_PrsntAdd_PS, Bride_PrsntAdd_WA, Bride_PrsntAdd_MuniGp, Bride_PrsntAdd_VilWrd, Bride_PrsntAdd_PO, Bride_PrsntAdd_Pin, Bride_Permanent_Address,
                                              Bride_PrmntAdd_Country, Bride_PrmntAdd_State, Bride_PrmntAdd_State_flag, Bride_PrmntAdd_Dist, Bride_PrmntAdd_SubDiv, Bride_PrmntAdd_Block, Bride_PrmntAdd_PS, Bride_PrmntAdd_WA,
                                              Bride_PrmntAdd_MuniGp, Bride_PrmntAdd_VilWrd, Bride_PrmntAdd_PO, Bride_PrmntAdd_Pin, Bride_Mob_Num, Bride_dob, Bride_Email_Id, Bride_Religion, Bride_Aadhr_Num, Bride_Occupation,
                                              Bride_maritial_status, Bride_Prev_Mar_Dt, Bride_Photo, Bride_Sign, Bride_Proof_PrsntAdd_Type, Bride_Proof_PrmntAdd_Type, Bride_Proof_DOB_Type,
                                              MO_Autoincrement_cd, Place_Reg_Address, Place_Reg_State, Place_Reg_Dist, Place_Reg_SubDiv, Place_Reg_Block, Place_Reg_PS, Place_Reg_WA, Place_Reg_MuniGp, Place_Reg_VilWrd, Place_Reg_PO,
                                              Place_Socl_Mrg_Address, Place_Socl_Mrg_Country, Place_Socl_Mrg_State, Place_Socl_Mrg_State_flag, Place_Socl_Mrg_Dist, Place_Socl_Mrg_SubDiv, Place_Socl_Mrg_Block, Place_Socl_Mrg_PS,
                                              Place_Socl_Mrg_WA, Place_Socl_Mrg_MuniGp, Place_Socl_Mrg_VilWrd, Place_Socl_Mrg_PO, Place_Socl_Mrg_Pin, Socl_Mrg_Dt, Mrg_Invi_Card, Payment_flag, Place_Reg, Time_Reg, IP_Address,
                                              gbpl_serial_cd, bbpl_serial_cd, grm_bpl_status, bride_bpl_status, applicant_bpl_status, gadhrconsent, badhrconsent, Groom_Nationality, Bride_Nationality)
        End Function

#End Region

#Region "Groom & Bride Child Details"

        'Public Function NewApp_BG_Child_Save(App_Auto_Cd As String, Groom As DataTable, Bride As DataTable) As String
        '    Return apply_online_db.NewApp_BG_Child_Save(App_Auto_Cd, Groom, Bride)
        'End Function

        Public Function NewApp_BG_Child_Save(App_Auto_Cd As String, Groom As DataTable, Bride As DataTable, Tag As String, Choose_Child_By As String) As String
            Return apply_online_db.NewApp_BG_Child_Save(App_Auto_Cd, Groom, Bride, Tag, Choose_Child_By)
        End Function
#End Region

#Region "Draft Insert"

        Public Function christianTestSave(Draft_cd As String, Form_cd As String, Act_cd As String, g_name As String, b_name As String, g_f_name As String, b_f_name As String, g_r_cd As String, b_r_cd As String,
                                          g_c_cd As String, b_c_cd As String, g_sc_name As String, b_sc_name As String) As Array
            Return apply_online_db.ChristianMarriageTest(Draft_cd, Form_cd, Act_cd, g_name, b_name, g_f_name, b_f_name, g_r_cd, b_r_cd, g_c_cd, b_c_cd, g_sc_name, b_sc_name)

        End Function
        Public Function NewAppDraftSave(Draft_Cd As String, Form_id As String, Act_cd As String, Groom_FNam As String, Groom_MNam As String, Groom_LNam As String, Groom_Father_Nam As String, Groom_Mother_Nam As String,
                                   Groom_Present_HNo As String, Groom_Present_Address As String, Groom_PrsntAdd_Country As String, Groom_PrsntAdd_State As String, Groom_PrsntAdd_State_flag As String, Groom_PrsntAdd_Dist As String,
                                   Groom_PrsntAdd_SubDiv As String, Groom_PrsntAdd_Block As String, Groom_PrsntAdd_PS As String, Groom_PrsntAdd_WA As String, Groom_PrsntAdd_MuniGp As String,
                                   Groom_PrsntAdd_VilWrd As String, Groom_PrsntAdd_PO As String, Groom_PrsntAdd_Pin As String, Groom_Permanent_HNo As String, Groom_Permanent_Address As String, Groom_PrmntAdd_Country As String,
                                   Groom_PrmntAdd_State As String, Groom_PrmntAdd_State_flag As String, Groom_PrmntAdd_Dist As String, Groom_PrmntAdd_SubDiv As String, Groom_PrmntAdd_Block As String,
                                   Groom_PrmntAdd_PS As String, Groom_PrmntAdd_WA As String, Groom_PrmntAdd_MuniGp As String, Groom_PrmntAdd_VilWrd As String, Groom_PrmntAdd_PO As String,
                                   Groom_PrmntAdd_Pin As String, Groom_Mob_Num As String, Groom_dob As DateTime, Groom_Email_Id As String, Groom_Religion As String, Groom_Aadhr_Num As String,
                                   Groom_occupation As String, Groom_maritial_status As String, Groom_Prev_Mar_Dt As String, Groom_Photo As Byte(), Groom_Sign As Byte(), Groom_Proof_PrsntAdd_Type As String,
                                   Groom_Proof_PrsntAdd As Byte(), Groom_Proof_PrmntAdd_Type As String, Groom_Proof_PrmntAdd As Byte(), Groom_Proof_DOB_Type As String, Groom_Proof_DOB As Byte(),
                                   Bride_FNam As String, Bride_MNam As String, Bride_LNam As String, Bride_Father_Nam As String, Bride_Mother_Nam As String, Bride_Present_HNo As String, Bride_Present_Address As String,
                                   Bride_PrsntAdd_Country As String, Bride_PrsntAdd_State As String, Bride_PrsntAdd_State_flag As String, Bride_PrsntAdd_Dist As String, Bride_PrsntAdd_SubDiv As String,
                                   Bride_PrsntAdd_Block As String, Bride_PrsntAdd_PS As String, Bride_PrsntAdd_WA As String, Bride_PrsntAdd_MuniGp As String, Bride_PrsntAdd_VilWrd As String,
                                   Bride_PrsntAdd_PO As String, Bride_PrsntAdd_Pin As String, Bride_Permanent_HNo As String, Bride_Permanent_Address As String, Bride_PrmntAdd_Country As String, Bride_PrmntAdd_State As String,
                                   Bride_PrmntAdd_State_flag As String, Bride_PrmntAdd_Dist As String, Bride_PrmntAdd_SubDiv As String, Bride_PrmntAdd_Block As String, Bride_PrmntAdd_PS As String,
                                   Bride_PrmntAdd_WA As String, Bride_PrmntAdd_MuniGp As String, Bride_PrmntAdd_VilWrd As String, Bride_PrmntAdd_PO As String, Bride_PrmntAdd_Pin As String,
                                   Bride_Mob_Num As String, Bride_dob As DateTime, Bride_Email_Id As String, Bride_Religion As String, Bride_Aadhr_Num As String, Bride_Occupation As String,
                                   Bride_maritial_status As String, Bride_Prev_Mar_Dt As String, Bride_Photo As Byte(), Bride_Sign As Byte(), Bride_Proof_PrsntAdd_Type As String, Bride_Proof_PrsntAdd As Byte(),
                                   Bride_Proof_PrmntAdd_Type As String, Bride_Proof_PrmntAdd As Byte(), Bride_Proof_DOB_Type As String, Bride_Proof_DOB As Byte(),
                                   MO_Autoincrement_cd As String, Place_Reg_Address As String, Place_Reg_State As String, Place_Reg_Dist As String, Place_Reg_SubDiv As String, Place_Reg_Block As String,
                                   Place_Reg_PS As String, Place_Reg_WA As String, Place_Reg_MuniGp As String, Place_Reg_VilWrd As String, Place_Reg_PO As String, Place_Socl_Mrg_HNo As String, Place_Socl_Mrg_Address As String,
                                   Place_Socl_Mrg_Country As String, Place_Socl_Mrg_State As String, Place_Socl_Mrg_State_flag As String, Place_Socl_Mrg_Dist As String, Place_Socl_Mrg_SubDiv As String,
                                   Place_Socl_Mrg_Block As String, Place_Socl_Mrg_PS As String, Place_Socl_Mrg_WA As String, Place_Socl_Mrg_MuniGp As String, Place_Socl_Mrg_VilWrd As String,
                                   Place_Socl_Mrg_PO As String, Place_Socl_Mrg_Pin As String, Socl_Mrg_Dt As String, Mrg_Invi_Card As Byte(), Place_Reg As String, Time_Reg As String, IP_Address As String,
                                   grm_bpl_prf As Byte(), bride_bpl_prf As Byte(), gbpl_serial_cd As String, bbpl_serial_cd As String,
                                   grm_bpl_status As String, bride_bpl_status As String, applicant_bpl_status As String, gadhrconsent As String, badhrconsent As String, Groom_Nationality As String, Bride_Nationality As String, MO_Verified As String,
                                   Groom_Religion_Other_Name As String, Groom_Occupation_Other_Name As String, Bride_Religion_Other_Name As String, Bride_Occupation_Other_Name As String,
                                   Groom_IdProofNo_PresentAddress As String, Groom_IdProofNo_Dob As String, Groom_IdProofNo_PermanentAddress As String,
                                   Bride_IdProofNo_PresentAddress As String, Bride_IdProofNo_Dob As String, Bride_IdProofNo_PermanentAddress As String,
                                   Groom_Guardian_type As String, Bride_Guardian_type As String, Groom_length_residence As String, Bride_length_residence As String, Joint_Photo As Byte(),
                                   g_caseno As String, g_courtname As String, g_dateoforder As String, g_petitionername As String, g_respondentname As String,
                                   b_caseno As String, b_courtname As String, b_dateoforder As String, b_petitionername As String, b_respondentname As String,
                                   gdivorce_prf As Byte(), g_txtDivorceCertificateNo As String, bdivorce_prf As Byte(), b_txtDivorceCertificateNo As String,
                                   wife_detCrt_prf As Byte(), WifeDeathCertificateNo As String,
                                   Husband_detCrt_prf As Byte(), HusbandDeathCertificateNo As String, ddlDeathHusbGovId As String, HusbndDtCrt_wh_prf As Byte(), txtWifeGovIdWithHusName As String, IsBrideMaidenName As String, BrideFatherLastName As String,
                                   groom_legalguardian_proof_id As String, bride_legalguardian_proof_id As String, groom_legalguardian_proof_type As String, bride_legalguardian_proof_type As String, ddlgCountryCode As String, ddlbCountryCode As String, IsGroomSameAddr As String, IsBrideSameAddr As String, mo_select As String, propose_reg_dt As DateTime,
                                   groom_nullity_proof_no As String, bride_nullity_proof_no As String,
                                   g_nullity_case_no As String, g_nullity_court_name As String, g_nullity_dateOfOrder As DateTime, g_nullity_issuedBy As String, g_nullity_court_Country As String, g_nullity_court_state As String, g_nullity_court_district As String, g_is_doc_appostile As String, g_null_pre_app_no As String,
                                   b_nullity_case_no As String, b_nullity_court_name As String, b_nullity_dateOfOrder As DateTime, b_nullity_issuedBy As String, b_nullity_court_Country As String, b_nullity_court_state As String, b_nullity_court_district As String, b_is_doc_appostile As String, b_null_pre_app_no As String, g_church_name As String, b_church_name As String) As Array

            Return apply_online_db.NewAppDraftSave(Draft_Cd, Form_id, Act_cd, Groom_FNam, Groom_MNam, Groom_LNam, Groom_Father_Nam, Groom_Mother_Nam, Groom_Present_HNo, Groom_Present_Address, Groom_PrsntAdd_Country, Groom_PrsntAdd_State, Groom_PrsntAdd_State_flag,
                                              Groom_PrsntAdd_Dist, Groom_PrsntAdd_SubDiv, Groom_PrsntAdd_Block, Groom_PrsntAdd_PS, Groom_PrsntAdd_WA, Groom_PrsntAdd_MuniGp, Groom_PrsntAdd_VilWrd, Groom_PrsntAdd_PO,
                                              Groom_PrsntAdd_Pin, Groom_Permanent_HNo, Groom_Permanent_Address, Groom_PrmntAdd_Country, Groom_PrmntAdd_State, Groom_PrmntAdd_State_flag, Groom_PrmntAdd_Dist, Groom_PrmntAdd_SubDiv,
                                              Groom_PrmntAdd_Block, Groom_PrmntAdd_PS, Groom_PrmntAdd_WA, Groom_PrmntAdd_MuniGp, Groom_PrmntAdd_VilWrd, Groom_PrmntAdd_PO, Groom_PrmntAdd_Pin, Groom_Mob_Num, Groom_dob,
                                              Groom_Email_Id, Groom_Religion, Groom_Aadhr_Num, Groom_occupation, Groom_maritial_status, Groom_Prev_Mar_Dt, Groom_Photo, Groom_Sign, Groom_Proof_PrsntAdd_Type, Groom_Proof_PrsntAdd,
                                              Groom_Proof_PrmntAdd_Type, Groom_Proof_PrmntAdd, Groom_Proof_DOB_Type, Groom_Proof_DOB,
                                              Bride_FNam, Bride_MNam, Bride_LNam, Bride_Father_Nam, Bride_Mother_Nam, Bride_Present_HNo, Bride_Present_Address, Bride_PrsntAdd_Country, Bride_PrsntAdd_State, Bride_PrsntAdd_State_flag, Bride_PrsntAdd_Dist,
                                              Bride_PrsntAdd_SubDiv, Bride_PrsntAdd_Block, Bride_PrsntAdd_PS, Bride_PrsntAdd_WA, Bride_PrsntAdd_MuniGp, Bride_PrsntAdd_VilWrd, Bride_PrsntAdd_PO, Bride_PrsntAdd_Pin, Bride_Permanent_HNo, Bride_Permanent_Address,
                                              Bride_PrmntAdd_Country, Bride_PrmntAdd_State, Bride_PrmntAdd_State_flag, Bride_PrmntAdd_Dist, Bride_PrmntAdd_SubDiv, Bride_PrmntAdd_Block, Bride_PrmntAdd_PS, Bride_PrmntAdd_WA,
                                              Bride_PrmntAdd_MuniGp, Bride_PrmntAdd_VilWrd, Bride_PrmntAdd_PO, Bride_PrmntAdd_Pin, Bride_Mob_Num, Bride_dob, Bride_Email_Id, Bride_Religion, Bride_Aadhr_Num, Bride_Occupation,
                                              Bride_maritial_status, Bride_Prev_Mar_Dt, Bride_Photo, Bride_Sign, Bride_Proof_PrsntAdd_Type, Bride_Proof_PrsntAdd, Bride_Proof_PrmntAdd_Type, Bride_Proof_PrmntAdd, Bride_Proof_DOB_Type,
                                              Bride_Proof_DOB,
                                              MO_Autoincrement_cd, Place_Reg_Address, Place_Reg_State, Place_Reg_Dist, Place_Reg_SubDiv, Place_Reg_Block, Place_Reg_PS, Place_Reg_WA, Place_Reg_MuniGp, Place_Reg_VilWrd, Place_Reg_PO,
                                              Place_Socl_Mrg_HNo, Place_Socl_Mrg_Address, Place_Socl_Mrg_Country, Place_Socl_Mrg_State, Place_Socl_Mrg_State_flag, Place_Socl_Mrg_Dist, Place_Socl_Mrg_SubDiv, Place_Socl_Mrg_Block, Place_Socl_Mrg_PS,
                                              Place_Socl_Mrg_WA, Place_Socl_Mrg_MuniGp, Place_Socl_Mrg_VilWrd, Place_Socl_Mrg_PO, Place_Socl_Mrg_Pin, Socl_Mrg_Dt, Mrg_Invi_Card, Place_Reg, Time_Reg, IP_Address,
                                              grm_bpl_prf, bride_bpl_prf, gbpl_serial_cd, bbpl_serial_cd, grm_bpl_status, bride_bpl_status, applicant_bpl_status, gadhrconsent, badhrconsent, Groom_Nationality, Bride_Nationality, MO_Verified,
                                              Groom_Religion_Other_Name, Groom_Occupation_Other_Name, Bride_Religion_Other_Name, Bride_Occupation_Other_Name.Trim,
                                              Groom_IdProofNo_PresentAddress, Groom_IdProofNo_Dob, Groom_IdProofNo_PermanentAddress,
                                              Bride_IdProofNo_PresentAddress, Bride_IdProofNo_Dob, Bride_IdProofNo_PermanentAddress,
                                              Groom_Guardian_type, Bride_Guardian_type, Groom_length_residence, Bride_length_residence, Joint_Photo,
                                              g_caseno, g_courtname, g_dateoforder, g_petitionername, g_respondentname,
                                              b_caseno, b_courtname, b_dateoforder, b_petitionername, b_respondentname,
                                              gdivorce_prf, g_txtDivorceCertificateNo, bdivorce_prf, b_txtDivorceCertificateNo,
                                              wife_detCrt_prf, WifeDeathCertificateNo,
                                              Husband_detCrt_prf, HusbandDeathCertificateNo, ddlDeathHusbGovId, HusbndDtCrt_wh_prf, txtWifeGovIdWithHusName, IsBrideMaidenName, BrideFatherLastName,
                                              groom_legalguardian_proof_id, bride_legalguardian_proof_id, groom_legalguardian_proof_type, bride_legalguardian_proof_type, ddlgCountryCode, ddlbCountryCode, IsGroomSameAddr, IsBrideSameAddr, mo_select, propose_reg_dt,
                                              groom_nullity_proof_no, bride_nullity_proof_no, g_nullity_case_no, g_nullity_court_name, g_nullity_dateOfOrder, g_nullity_issuedBy, g_nullity_court_Country, g_nullity_court_state, g_nullity_court_district, g_is_doc_appostile, g_null_pre_app_no,
                                              b_nullity_case_no, b_nullity_court_name, b_nullity_dateOfOrder, b_nullity_issuedBy, b_nullity_court_Country, b_nullity_court_state, b_nullity_court_district, b_is_doc_appostile, b_null_pre_app_no, g_church_name, b_church_name)
        End Function

        Public Function NewAppDraftSave_Groom(Form_id As String, Act_cd As String, Groom_FNam As String, Groom_MNam As String, Groom_LNam As String, Groom_Father_Nam As String, Groom_Mother_Nam As String,
                                   Groom_Present_HNo As String, Groom_Present_Address As String, Groom_PrsntAdd_Country As String, Groom_PrsntAdd_State As String, Groom_PrsntAdd_State_flag As String, Groom_PrsntAdd_Dist As String,
                                   Groom_PrsntAdd_SubDiv As String, Groom_PrsntAdd_Block As String, Groom_PrsntAdd_PS As String, Groom_PrsntAdd_WA As String, Groom_PrsntAdd_MuniGp As String,
                                   Groom_PrsntAdd_VilWrd As String, Groom_PrsntAdd_PO As String, Groom_PrsntAdd_Pin As String, Groom_Permanent_HNo As String, Groom_Permanent_Address As String, Groom_PrmntAdd_Country As String,
                                   Groom_PrmntAdd_State As String, Groom_PrmntAdd_State_flag As String, Groom_PrmntAdd_Dist As String, Groom_PrmntAdd_SubDiv As String, Groom_PrmntAdd_Block As String,
                                   Groom_PrmntAdd_PS As String, Groom_PrmntAdd_WA As String, Groom_PrmntAdd_MuniGp As String, Groom_PrmntAdd_VilWrd As String, Groom_PrmntAdd_PO As String,
                                   Groom_PrmntAdd_Pin As String, Groom_Mob_Num As String, Groom_dob As DateTime, Groom_Email_Id As String, Groom_Religion As String, Groom_Aadhr_Num As String,
                                   Groom_occupation As String, Groom_maritial_status As String, Groom_Prev_Mar_Dt As String, Groom_Photo As Byte(), Groom_Sign As Byte(), Groom_Proof_PrsntAdd_Type As String,
                                   Groom_Proof_PrsntAdd As Byte(), Groom_Proof_PrmntAdd_Type As String, Groom_Proof_PrmntAdd As Byte(), Groom_Proof_DOB_Type As String, Groom_Proof_DOB As Byte(),
                                   grm_bpl_prf As Byte(), gbpl_serial_cd As String, grm_bpl_status As String, gadhrconsent As String, Groom_Nationality As String) As Array

            Return apply_online_db.NewAppDraftSave_Groom(Form_id, Act_cd, Groom_FNam, Groom_MNam, Groom_LNam, Groom_Father_Nam, Groom_Mother_Nam, Groom_Present_HNo, Groom_Present_Address, Groom_PrsntAdd_Country, Groom_PrsntAdd_State, Groom_PrsntAdd_State_flag,
                                              Groom_PrsntAdd_Dist, Groom_PrsntAdd_SubDiv, Groom_PrsntAdd_Block, Groom_PrsntAdd_PS, Groom_PrsntAdd_WA, Groom_PrsntAdd_MuniGp, Groom_PrsntAdd_VilWrd, Groom_PrsntAdd_PO,
                                              Groom_PrsntAdd_Pin, Groom_Permanent_HNo, Groom_Permanent_Address, Groom_PrmntAdd_Country, Groom_PrmntAdd_State, Groom_PrmntAdd_State_flag, Groom_PrmntAdd_Dist, Groom_PrmntAdd_SubDiv,
                                              Groom_PrmntAdd_Block, Groom_PrmntAdd_PS, Groom_PrmntAdd_WA, Groom_PrmntAdd_MuniGp, Groom_PrmntAdd_VilWrd, Groom_PrmntAdd_PO, Groom_PrmntAdd_Pin, Groom_Mob_Num, Groom_dob,
                                              Groom_Email_Id, Groom_Religion, Groom_Aadhr_Num, Groom_occupation, Groom_maritial_status, Groom_Prev_Mar_Dt, Groom_Photo, Groom_Sign, Groom_Proof_PrsntAdd_Type, Groom_Proof_PrsntAdd,
                                              Groom_Proof_PrmntAdd_Type, Groom_Proof_PrmntAdd, Groom_Proof_DOB_Type, Groom_Proof_DOB,
                                              grm_bpl_prf, gbpl_serial_cd, grm_bpl_status, gadhrconsent, Groom_Nationality)
        End Function

        Public Function NewAppDraftSave_Bride(Form_id As String, Act_cd As String, Bride_FNam As String, Bride_MNam As String, Bride_LNam As String, Bride_Father_Nam As String, Bride_Mother_Nam As String, Bride_Present_HNo As String, Bride_Present_Address As String,
                                   Bride_PrsntAdd_Country As String, Bride_PrsntAdd_State As String, Bride_PrsntAdd_State_flag As String, Bride_PrsntAdd_Dist As String, Bride_PrsntAdd_SubDiv As String,
                                   Bride_PrsntAdd_Block As String, Bride_PrsntAdd_PS As String, Bride_PrsntAdd_WA As String, Bride_PrsntAdd_MuniGp As String, Bride_PrsntAdd_VilWrd As String,
                                   Bride_PrsntAdd_PO As String, Bride_PrsntAdd_Pin As String, Bride_Permanent_HNo As String, Bride_Permanent_Address As String, Bride_PrmntAdd_Country As String, Bride_PrmntAdd_State As String,
                                   Bride_PrmntAdd_State_flag As String, Bride_PrmntAdd_Dist As String, Bride_PrmntAdd_SubDiv As String, Bride_PrmntAdd_Block As String, Bride_PrmntAdd_PS As String,
                                   Bride_PrmntAdd_WA As String, Bride_PrmntAdd_MuniGp As String, Bride_PrmntAdd_VilWrd As String, Bride_PrmntAdd_PO As String, Bride_PrmntAdd_Pin As String,
                                   Bride_Mob_Num As String, Bride_dob As DateTime, Bride_Email_Id As String, Bride_Religion As String, Bride_Aadhr_Num As String, Bride_Occupation As String,
                                   Bride_maritial_status As String, Bride_Prev_Mar_Dt As String, Bride_Photo As Byte(), Bride_Sign As Byte(), Bride_Proof_PrsntAdd_Type As String, Bride_Proof_PrsntAdd As Byte(),
                                   Bride_Proof_PrmntAdd_Type As String, Bride_Proof_PrmntAdd As Byte(), Bride_Proof_DOB_Type As String, Bride_Proof_DOB As Byte(),
                                   bride_bpl_prf As Byte(), bbpl_serial_cd As String, bride_bpl_status As String, badhrconsent As String, Bride_Nationality As String) As Array

            Return apply_online_db.NewAppDraftSave_Bride(Form_id, Act_cd, Bride_FNam, Bride_MNam, Bride_LNam, Bride_Father_Nam, Bride_Mother_Nam, Bride_Present_HNo, Bride_Present_Address, Bride_PrsntAdd_Country, Bride_PrsntAdd_State, Bride_PrsntAdd_State_flag, Bride_PrsntAdd_Dist,
                                              Bride_PrsntAdd_SubDiv, Bride_PrsntAdd_Block, Bride_PrsntAdd_PS, Bride_PrsntAdd_WA, Bride_PrsntAdd_MuniGp, Bride_PrsntAdd_VilWrd, Bride_PrsntAdd_PO, Bride_PrsntAdd_Pin, Bride_Permanent_HNo, Bride_Permanent_Address,
                                              Bride_PrmntAdd_Country, Bride_PrmntAdd_State, Bride_PrmntAdd_State_flag, Bride_PrmntAdd_Dist, Bride_PrmntAdd_SubDiv, Bride_PrmntAdd_Block, Bride_PrmntAdd_PS, Bride_PrmntAdd_WA,
                                              Bride_PrmntAdd_MuniGp, Bride_PrmntAdd_VilWrd, Bride_PrmntAdd_PO, Bride_PrmntAdd_Pin, Bride_Mob_Num, Bride_dob, Bride_Email_Id, Bride_Religion, Bride_Aadhr_Num, Bride_Occupation,
                                              Bride_maritial_status, Bride_Prev_Mar_Dt, Bride_Photo, Bride_Sign, Bride_Proof_PrsntAdd_Type, Bride_Proof_PrsntAdd, Bride_Proof_PrmntAdd_Type, Bride_Proof_PrmntAdd, Bride_Proof_DOB_Type,
                                              Bride_Proof_DOB, bride_bpl_prf, bbpl_serial_cd, bride_bpl_status, badhrconsent, Bride_Nationality)
        End Function

#End Region

#Region "BSK Insert"
        Public Function BSKInsert(App_cd As String, Draft_Cd As String, Payment_status As String, Ip_address As String, Mac_address As String,
                                  UserId As String, decrypt_UserId As String, mo_cd As String, TicktNo As String) As String
            Return apply_online_db.BSKInsert(App_cd, Draft_Cd, Payment_status, Ip_address, Mac_address,
                                  UserId, decrypt_UserId, mo_cd, TicktNo)

        End Function

        Public Function BSK_param1_UserId(UserId As String) As String    ', mo_cd As String
            Return apply_online_db.BSK_param1_UserId(UserId)                ', mo_cd
        End Function

#End Region

#Region "Duplicate Entry Check"
        Public Function Check_duplicate_entry(marriage_act As String, grom_fname As String, groom_mname As String, groom_lname As String, groom_father_name As String,
                                              groom_mother_name As String, groom_dob As DateTime, groom_mrt_status As String, bride_fnam As String,
                                              bride_mname As String, bride_lname As String, bride_father_name As String, bride_mother_name As String,
                                              bride_dob As DateTime, bride_mrt_status As String) As Array
            Return apply_online_db.Check_duplicate_entry(marriage_act, grom_fname, groom_mname, groom_lname, groom_father_name, groom_mother_name, groom_dob, groom_mrt_status,
                                                         bride_fnam, bride_mname, bride_lname, bride_father_name, bride_mother_name, bride_dob, bride_mrt_status)
        End Function


        Public Function Cheak_email_mobile_exist(marriage_act As String, groom_mrt_status As String, groom_mobile_no As String, groom_email_id As String, bride_mrt_status As String, bride_mobile_no As String, bride_email_id As String) As Array
            Return apply_online_db.Cheak_email_mobile_exist(marriage_act, groom_mrt_status, groom_mobile_no, groom_email_id, bride_mrt_status, bride_mobile_no, bride_email_id)
        End Function
#End Region

#Region "Get Saved Draft"
        Public Function GetDraftApplication(Draft_Cd As String) As DataTable
            Return apply_online_db.GetDraftApplication(Draft_Cd)
        End Function
#End Region

#End Region


#Region "Fill vill wrd"
        Public Function Fill_VilWrd_Txt(Dist As String, SubDiv As String, blck As String, gp As String, Mouza_nm As String, ip_addr As String) As Array
            Return apply_online_db.Fill_VilWrd_Txt(Dist, SubDiv, blck, gp, Mouza_nm, ip_addr)
        End Function
#End Region

#Region "Fill Census"
        Public Function Fill_Cencus(Cen_name As String) As String
            Return apply_online_db.Fill_Cencus(Cen_name)
        End Function
#End Region

#Region "MO Transfer"

#Region "Request for Changing MO Insert"
        Public Function MO_Transfer(App_Auto_Cd As String, From_MO_Autoincrement_Cd As String,
                                    Reason_For_Transfer As String, User_Id As String, Ip_Address As String, flag As String) As String

            Return apply_online_db.MO_Transfer_Save(App_Auto_Cd, From_MO_Autoincrement_Cd, Reason_For_Transfer, User_Id, Ip_Address, flag)

        End Function
#End Region

#Region "MARREG Transfer Page Grid View"
        Public Function Transfer_Application(Role_Type_Cd As String) As DataTable
            Return apply_online_db.Transfer_Application(Role_Type_Cd)
        End Function
#End Region

#Region "Request for Changing MO Updare"
        Public Function MO_Transfer_update(dtMOTransfer As DataTable, User_id As String, Ip_Address As String, Mac_Address As String) As String

            Return apply_online_db.MO_Transfer_Update(dtMOTransfer, User_id, Ip_Address, Mac_Address)

        End Function
#End Region
#End Region
#Region "EDIT"
        Public Function GetApplication(Draft_Cd As String) As DataTable
            Return apply_online_db.GetApplication(Draft_Cd)
        End Function

        Public Function NewAppDraftSaveEdit(Draft_Cd As String, Form_id As String, Act_cd As String, Groom_FNam As String, Groom_MNam As String, Groom_LNam As String, Groom_Father_Nam As String, Groom_Mother_Nam As String,
                                   Groom_Present_HNo As String, Groom_Present_Address As String, Groom_PrsntAdd_Country As String, Groom_PrsntAdd_State As String, Groom_PrsntAdd_State_flag As String, Groom_PrsntAdd_Dist As String,
                                   Groom_PrsntAdd_SubDiv As String, Groom_PrsntAdd_Block As String, Groom_PrsntAdd_PS As String, Groom_PrsntAdd_WA As String, Groom_PrsntAdd_MuniGp As String,
                                   Groom_PrsntAdd_VilWrd As String, Groom_PrsntAdd_PO As String, Groom_PrsntAdd_Pin As String, Groom_Permanent_HNo As String, Groom_Permanent_Address As String, Groom_PrmntAdd_Country As String,
                                   Groom_PrmntAdd_State As String, Groom_PrmntAdd_State_flag As String, Groom_PrmntAdd_Dist As String, Groom_PrmntAdd_SubDiv As String, Groom_PrmntAdd_Block As String,
                                   Groom_PrmntAdd_PS As String, Groom_PrmntAdd_WA As String, Groom_PrmntAdd_MuniGp As String, Groom_PrmntAdd_VilWrd As String, Groom_PrmntAdd_PO As String,
                                   Groom_PrmntAdd_Pin As String, Groom_Mob_Num As String, Groom_dob As DateTime, Groom_Email_Id As String, Groom_Religion As String, Groom_Aadhr_Num As String,
                                   Groom_occupation As String, Groom_maritial_status As String, Groom_Prev_Mar_Dt As String, Groom_Photo As Byte(), Groom_Sign As Byte(), Groom_Proof_PrsntAdd_Type As String,
                                   Groom_Proof_PrsntAdd As Byte(), Groom_Proof_PrmntAdd_Type As String, Groom_Proof_PrmntAdd As Byte(), Groom_Proof_DOB_Type As String, Groom_Proof_DOB As Byte(),
                                   Bride_FNam As String, Bride_MNam As String, Bride_LNam As String, Bride_Father_Nam As String, Bride_Mother_Nam As String, Bride_Present_HNo As String, Bride_Present_Address As String,
                                   Bride_PrsntAdd_Country As String, Bride_PrsntAdd_State As String, Bride_PrsntAdd_State_flag As String, Bride_PrsntAdd_Dist As String, Bride_PrsntAdd_SubDiv As String,
                                   Bride_PrsntAdd_Block As String, Bride_PrsntAdd_PS As String, Bride_PrsntAdd_WA As String, Bride_PrsntAdd_MuniGp As String, Bride_PrsntAdd_VilWrd As String,
                                   Bride_PrsntAdd_PO As String, Bride_PrsntAdd_Pin As String, Bride_Permanent_HNo As String, Bride_Permanent_Address As String, Bride_PrmntAdd_Country As String, Bride_PrmntAdd_State As String,
                                   Bride_PrmntAdd_State_flag As String, Bride_PrmntAdd_Dist As String, Bride_PrmntAdd_SubDiv As String, Bride_PrmntAdd_Block As String, Bride_PrmntAdd_PS As String,
                                   Bride_PrmntAdd_WA As String, Bride_PrmntAdd_MuniGp As String, Bride_PrmntAdd_VilWrd As String, Bride_PrmntAdd_PO As String, Bride_PrmntAdd_Pin As String,
                                   Bride_Mob_Num As String, Bride_dob As DateTime, Bride_Email_Id As String, Bride_Religion As String, Bride_Aadhr_Num As String, Bride_Occupation As String,
                                   Bride_maritial_status As String, Bride_Prev_Mar_Dt As String, Bride_Photo As Byte(), Bride_Sign As Byte(), Bride_Proof_PrsntAdd_Type As String, Bride_Proof_PrsntAdd As Byte(),
                                   Bride_Proof_PrmntAdd_Type As String, Bride_Proof_PrmntAdd As Byte(), Bride_Proof_DOB_Type As String, Bride_Proof_DOB As Byte(),
                                   MO_Autoincrement_cd As String, Place_Reg_Address As String, Place_Reg_State As String, Place_Reg_Dist As String, Place_Reg_SubDiv As String, Place_Reg_Block As String,
                                   Place_Reg_PS As String, Place_Reg_WA As String, Place_Reg_MuniGp As String, Place_Reg_VilWrd As String, Place_Reg_PO As String, Place_Socl_Mrg_HNo As String, Place_Socl_Mrg_Address As String,
                                   Place_Socl_Mrg_Country As String, Place_Socl_Mrg_State As String, Place_Socl_Mrg_State_flag As String, Place_Socl_Mrg_Dist As String, Place_Socl_Mrg_SubDiv As String,
                                   Place_Socl_Mrg_Block As String, Place_Socl_Mrg_PS As String, Place_Socl_Mrg_WA As String, Place_Socl_Mrg_MuniGp As String, Place_Socl_Mrg_VilWrd As String,
                                   Place_Socl_Mrg_PO As String, Place_Socl_Mrg_Pin As String, Socl_Mrg_Dt As String, Mrg_Invi_Card As Byte(), Place_Reg As String, Time_Reg As String, IP_Address As String,
                                   grm_bpl_prf As Byte(), bride_bpl_prf As Byte(), gbpl_serial_cd As String, bbpl_serial_cd As String,
                                   grm_bpl_status As String, bride_bpl_status As String, applicant_bpl_status As String, gadhrconsent As String, badhrconsent As String, Groom_Nationality As String, Bride_Nationality As String, MO_Verified As String,
                                   Groom_Religion_Other_Name As String, Groom_Occupation_Other_Name As String, Bride_Religion_Other_Name As String, Bride_Occupation_Other_Name As String,
                                   Groom_IdProofNo_PresentAddress As String, Groom_IdProofNo_Dob As String, Groom_IdProofNo_PermanentAddress As String,
                                   Bride_IdProofNo_PresentAddress As String, Bride_IdProofNo_Dob As String, Bride_IdProofNo_PermanentAddress As String,
                                   Groom_Guardian_type As String, Bride_Guardian_type As String, Groom_length_residence As String, Bride_length_residence As String, Joint_Photo As Byte(),
                                   g_caseno As String, g_courtname As String, g_dateoforder As String, g_petitionername As String, g_respondentname As String,
                                   b_caseno As String, b_courtname As String, b_dateoforder As String, b_petitionername As String, b_respondentname As String,
                                   gdivorce_prf As Byte(), g_txtDivorceCertificateNo As String, bdivorce_prf As Byte(), b_txtDivorceCertificateNo As String,
                                   wife_detCrt_prf As Byte(), WifeDeathCertificateNo As String,
                                   Husband_detCrt_prf As Byte(), HusbandDeathCertificateNo As String, ddlDeathHusbGovId As String, HusbndDtCrt_wh_prf As Byte(), txtWifeGovIdWithHusName As String, IsBrideMaidenName As String, BrideFatherLastName As String,
                                   groom_legalguardian_proof_id As String, bride_legalguardian_proof_id As String, groom_legalguardian_proof_type As String, bride_legalguardian_proof_type As String, IsCerificateReopen As String,
                                   userId As String, IPAddress As String, IsGroomSameAddr As String, IsBrideSameAddr As String) As Array

            Return apply_online_db.NewAppDraftSaveEdit(Draft_Cd, Form_id, Act_cd, Groom_FNam, Groom_MNam, Groom_LNam, Groom_Father_Nam, Groom_Mother_Nam, Groom_Present_HNo, Groom_Present_Address, Groom_PrsntAdd_Country, Groom_PrsntAdd_State, Groom_PrsntAdd_State_flag,
                                              Groom_PrsntAdd_Dist, Groom_PrsntAdd_SubDiv, Groom_PrsntAdd_Block, Groom_PrsntAdd_PS, Groom_PrsntAdd_WA, Groom_PrsntAdd_MuniGp, Groom_PrsntAdd_VilWrd, Groom_PrsntAdd_PO,
                                              Groom_PrsntAdd_Pin, Groom_Permanent_HNo, Groom_Permanent_Address, Groom_PrmntAdd_Country, Groom_PrmntAdd_State, Groom_PrmntAdd_State_flag, Groom_PrmntAdd_Dist, Groom_PrmntAdd_SubDiv,
                                              Groom_PrmntAdd_Block, Groom_PrmntAdd_PS, Groom_PrmntAdd_WA, Groom_PrmntAdd_MuniGp, Groom_PrmntAdd_VilWrd, Groom_PrmntAdd_PO, Groom_PrmntAdd_Pin, Groom_Mob_Num, Groom_dob,
                                              Groom_Email_Id, Groom_Religion, Groom_Aadhr_Num, Groom_occupation, Groom_maritial_status, Groom_Prev_Mar_Dt, Groom_Photo, Groom_Sign, Groom_Proof_PrsntAdd_Type, Groom_Proof_PrsntAdd,
                                              Groom_Proof_PrmntAdd_Type, Groom_Proof_PrmntAdd, Groom_Proof_DOB_Type, Groom_Proof_DOB,
                                              Bride_FNam, Bride_MNam, Bride_LNam, Bride_Father_Nam, Bride_Mother_Nam, Bride_Present_HNo, Bride_Present_Address, Bride_PrsntAdd_Country, Bride_PrsntAdd_State, Bride_PrsntAdd_State_flag, Bride_PrsntAdd_Dist,
                                              Bride_PrsntAdd_SubDiv, Bride_PrsntAdd_Block, Bride_PrsntAdd_PS, Bride_PrsntAdd_WA, Bride_PrsntAdd_MuniGp, Bride_PrsntAdd_VilWrd, Bride_PrsntAdd_PO, Bride_PrsntAdd_Pin, Bride_Permanent_HNo, Bride_Permanent_Address,
                                              Bride_PrmntAdd_Country, Bride_PrmntAdd_State, Bride_PrmntAdd_State_flag, Bride_PrmntAdd_Dist, Bride_PrmntAdd_SubDiv, Bride_PrmntAdd_Block, Bride_PrmntAdd_PS, Bride_PrmntAdd_WA,
                                              Bride_PrmntAdd_MuniGp, Bride_PrmntAdd_VilWrd, Bride_PrmntAdd_PO, Bride_PrmntAdd_Pin, Bride_Mob_Num, Bride_dob, Bride_Email_Id, Bride_Religion, Bride_Aadhr_Num, Bride_Occupation,
                                              Bride_maritial_status, Bride_Prev_Mar_Dt, Bride_Photo, Bride_Sign, Bride_Proof_PrsntAdd_Type, Bride_Proof_PrsntAdd, Bride_Proof_PrmntAdd_Type, Bride_Proof_PrmntAdd, Bride_Proof_DOB_Type,
                                              Bride_Proof_DOB,
                                              MO_Autoincrement_cd, Place_Reg_Address, Place_Reg_State, Place_Reg_Dist, Place_Reg_SubDiv, Place_Reg_Block, Place_Reg_PS, Place_Reg_WA, Place_Reg_MuniGp, Place_Reg_VilWrd, Place_Reg_PO,
                                              Place_Socl_Mrg_HNo, Place_Socl_Mrg_Address, Place_Socl_Mrg_Country, Place_Socl_Mrg_State, Place_Socl_Mrg_State_flag, Place_Socl_Mrg_Dist, Place_Socl_Mrg_SubDiv, Place_Socl_Mrg_Block, Place_Socl_Mrg_PS,
                                              Place_Socl_Mrg_WA, Place_Socl_Mrg_MuniGp, Place_Socl_Mrg_VilWrd, Place_Socl_Mrg_PO, Place_Socl_Mrg_Pin, Socl_Mrg_Dt, Mrg_Invi_Card, Place_Reg, Time_Reg, IP_Address,
                                              grm_bpl_prf, bride_bpl_prf, gbpl_serial_cd, bbpl_serial_cd, grm_bpl_status, bride_bpl_status, applicant_bpl_status, gadhrconsent, badhrconsent, Groom_Nationality, Bride_Nationality, MO_Verified,
                                              Groom_Religion_Other_Name, Groom_Occupation_Other_Name, Bride_Religion_Other_Name, Bride_Occupation_Other_Name.Trim,
                                              Groom_IdProofNo_PresentAddress, Groom_IdProofNo_Dob, Groom_IdProofNo_PermanentAddress,
                                              Bride_IdProofNo_PresentAddress, Bride_IdProofNo_Dob, Bride_IdProofNo_PermanentAddress,
                                              Groom_Guardian_type, Bride_Guardian_type, Groom_length_residence, Bride_length_residence, Joint_Photo,
                                              g_caseno, g_courtname, g_dateoforder, g_petitionername, g_respondentname,
                                              b_caseno, b_courtname, b_dateoforder, b_petitionername, b_respondentname,
                                              gdivorce_prf, g_txtDivorceCertificateNo, bdivorce_prf, b_txtDivorceCertificateNo,
                                              wife_detCrt_prf, WifeDeathCertificateNo,
                                              Husband_detCrt_prf, HusbandDeathCertificateNo, ddlDeathHusbGovId, HusbndDtCrt_wh_prf, txtWifeGovIdWithHusName, IsBrideMaidenName, BrideFatherLastName,
                                              groom_legalguardian_proof_id, bride_legalguardian_proof_id, groom_legalguardian_proof_type, bride_legalguardian_proof_type, IsCerificateReopen, userId, IPAddress, IsGroomSameAddr, IsBrideSameAddr)
        End Function

        Public Function NewApp_BG_Child_Save_Edit(App_Auto_Cd As String, Groom As DataTable, Bride As DataTable, Tag As String, Choose_Child_By As String) As String
            Return apply_online_db.NewApp_BG_Child_Save_Edit(App_Auto_Cd, Groom, Bride, Tag, Choose_Child_By)
        End Function

        Public Function GetCorrectionTblData(App_cd As String) As DataTable
            Return apply_online_db.GetCorrectionTblData(App_cd)
        End Function

        Public Function MO_Correction_Insert(app_cd As String, application_no As String, g_lnm_flag As String, groom_lastname As String, g_mnm_flag As String, groom_mothernm As String,
                                             b_lnm_flag As String, bride_lname As String, bride_mnm_flag As String, bride_m_fullname As String,
                                              Witness1_doc As Byte(), Witness2_doc As Byte(), Witness1_Sign As Byte(), Witness2_Sign As Byte(), Witness1_name As String, Witness2_name As String, witnss1_relation As String, witnss2_relation As String, witnss1_proof_identity As String, witnss2_proof_identity As String,
                                             Groom_present_addr_flag As String, gprstadd_hno As String, gprstadd_subdiv As String, gprstadd_wa As String, gprstadd_block As String, gprstadd_gp_or_municipal As String, gprstadd_vill As String, gprstadd_po As String, gprstadd_pin As String,
                                             Groom_Permanent_addr_flag As String, gpmntadd_hno As String, gpmntadd_subdiv As String, gpmntadd_wa As String, gpmntadd_block As String, gpmntadd_gp_or_municipal As String, gpmntadd_vill As String, gpmntadd_po As String, gpmntadd_pin As String,
                                             Bride_present_addr_flag As String, bprstadd_hno As String, bprstadd_subdiv As String, bprstadd_wa As String, bprstadd_block As String, bprstadd_gp_or_municipal As String, bprstadd_vill As String, bprstadd_po As String, bprstadd_pin As String,
                                             Bride_Permanent_addr_flag As String, bpmntadd_hno As String, bpmntadd_subdiv As String, bpmntadd_wa As String, bpmntadd_block As String, bpmntadd_gp_or_municipal As String, bpmntadd_vill As String, bpmntadd_po As String,
                                             bpmntadd_pin As String, Child_Final_Insert As String, ip_address As String, MAC_Address As String) As Array

            Return apply_online_db.MO_Correction_Insert(app_cd, application_no, g_lnm_flag, groom_lastname, g_mnm_flag, groom_mothernm, b_lnm_flag, bride_lname, bride_mnm_flag, bride_m_fullname,
                                                        Witness1_doc, Witness2_doc, Witness1_Sign, Witness2_Sign, Witness1_name, Witness2_name, witnss1_relation, witnss2_relation, witnss1_proof_identity, witnss2_proof_identity,
                                                        Groom_present_addr_flag, gprstadd_hno, gprstadd_subdiv, gprstadd_wa, gprstadd_block, gprstadd_gp_or_municipal, gprstadd_vill, gprstadd_po, gprstadd_pin,
                                                        Groom_Permanent_addr_flag, gpmntadd_hno, gpmntadd_subdiv, gpmntadd_wa, gpmntadd_block, gpmntadd_gp_or_municipal, gpmntadd_vill, gpmntadd_po, gpmntadd_pin,
                                                        Bride_present_addr_flag, bprstadd_hno, bprstadd_subdiv, bprstadd_wa, bprstadd_block, bprstadd_gp_or_municipal, bprstadd_vill, bprstadd_po, bprstadd_pin,
                                                        Bride_Permanent_addr_flag, bpmntadd_hno, bpmntadd_subdiv, bpmntadd_wa, bpmntadd_block, bpmntadd_gp_or_municipal, bpmntadd_vill, bpmntadd_po, bpmntadd_pin,
                                                         Child_Final_Insert, ip_address, MAC_Address)
        End Function
#End Region


#Region "Post Office Update for outside WB"
        Public Function Address_Save(Application_no As String, app_auto_cd As String, state As String, district As String, post_office As String, pin_no As String, IsAddres_Same As String, DateAndTime As String, Updated_PO_For As String, Address_type As String) As Array

            Return apply_online_db.Address_Save(Application_no, app_auto_cd, state, district, post_office, pin_no, IsAddres_Same, DateAndTime, Updated_PO_For, Address_type)
        End Function
#End Region


#End Region

    End Class
End Namespace
