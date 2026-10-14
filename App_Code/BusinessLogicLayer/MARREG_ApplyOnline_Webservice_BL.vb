Imports System.Data
Imports Microsoft.VisualBasic
Imports MARREG.App_Code.DataAccessLayer

Namespace MARREG.App_Code
    Public Class MARREG_ApplyOnline_Webservice_BL
        Dim obj As New MARREG_ApplyOnline_Webservice_DB()
        Dim objDB As New MARREG_ApplyOnline_Webservice_DB()
        Dim a As New Marreg_Class
#Region "User Registration"
#Region "Email & Mobile No Validation for Registration"
        Public Function CheckRegEmail(email As String) As Integer
            objDB.email = email

            Return objDB.CheckRegEmail()
        End Function
        Public Function CheckRegMobile(mob_no As String) As Integer
            objDB.mob_no = mob_no

            Return objDB.CheckRegMobile()
        End Function
#End Region
#Region "Save Data"
        Public Function SaveUserRegistration(infos As Dictionary(Of String, Object)) As String
            objDB.f_name = Convert.ToString(infos("f_name"))
            objDB.m_name = Convert.ToString(infos("m_name"))
            objDB.l_name = Convert.ToString(infos("l_name"))
            objDB.gender = Convert.ToString(infos("gender"))
            objDB.dob = Convert.ToString(infos("dob"))
            objDB.email = Convert.ToString(a.sqlInj_in(infos("email")))
            objDB.mob_no = Convert.ToString(infos("mob_no"))

            Dim ret = objDB.SaveUserRegistration()
            Return ret
        End Function
#End Region
#Region "Load Registration Data"
        Public Function LoadRegistrationData(Reg_Id As String) As DataTable
            objDB.Reg_Id = Reg_Id
            Return objDB.LoadRegistrationData()
        End Function
#End Region
#Region "Login Details Validation"
        Public Function VerifyLogin(reg_id As String, dob As String) As DataTable
            objDB.Reg_Id = Convert.ToString(reg_id)
            objDB.dob = Convert.ToString(dob)

            Dim ret = objDB.VerifyLogin()
            Return ret
        End Function
#End Region
#End Region
#Region "Update Initial Details Apply Online"
        Public Function Update_Initial_Details_Apply_Online(reg_id As String) As String
            objDB.Reg_Id = reg_id
            Dim ret = objDB.Update_Initial_Details_Apply_Online()
            Return ret
        End Function
#End Region
#Region "Load Apply Online Details"
        Public Function Load_Apply_Online_Details(reg_id As String) As DataTable
            Dim dt As DataTable
            objDB.Reg_Id = reg_id
            dt = objDB.Load_Apply_Online_Details()
            Return dt
        End Function
#End Region
#Region "Save Groom Details"
        Public Function SaveGroomPersonalDetails(Draft_Cd As String, Act As String, groom_fname As String, groom_mname As String, groom_lname As String, groom_guardian_type As String, groom_father_fname As String, groom_father_mname As String, groom_father_lname As String, groom_mother_fname As String, gromm_mother_mname As String, groom_mother_lname As String,
                                                groom_dob As String, groom_relegion As String, groom_relegion_other As String, groom_nationality As String, groom_mobnum_ctry As String, groom_mobile_no As String, Groom_Email_ID As String, groom_ocupatn As String,
                                                 groom_ocupatn_othr As String, groom_passport_no As String, groom_passport_ctry As String, groom_passport_valid As String, groom_adhar_num As String, Groom_Aadhr_Consent As String, groom_bpl As String, groom_bpl_no As String, groom_visa_etaNO As String, groom_visa_purpose As String, groom_visa_valid As String, groom_visa_DtOfEntry As String) As String

            Return obj.SaveGroomPersonalDetails(Draft_Cd, Act, groom_fname, groom_mname, groom_lname, groom_guardian_type, groom_father_fname, groom_father_mname, groom_father_lname, groom_mother_fname, gromm_mother_mname, groom_mother_lname,
            groom_dob, groom_relegion, groom_relegion_other, groom_nationality, groom_mobnum_ctry, groom_mobile_no, Groom_Email_ID,
                                                 groom_ocupatn, groom_ocupatn_othr, groom_passport_no, groom_passport_ctry, groom_passport_valid, groom_adhar_num, Groom_Aadhr_Consent, groom_bpl, groom_bpl_no, groom_visa_etaNO, groom_visa_purpose, groom_visa_valid, groom_visa_DtOfEntry)
        End Function
        Public Function SaveGroomMaritalDetails(Draft_Cd As String, groom_marital_status As String, groom_case_no As String, groom_court_name As String, groom_Dateof_order As String, groom_issued_by As String, groom_decree_act As String, groom_decree_section As String, groom_country As String, groom_state As String, groom_district As String) As String

            Return obj.SaveGroomMaritalDetails(Draft_Cd, groom_marital_status, groom_case_no, groom_court_name, groom_Dateof_order, groom_issued_by, groom_decree_act, groom_decree_section, groom_country, groom_state, groom_district)
        End Function
        Public Function SaveGroomAddressDetails(groom_prestadd_location As String, groom_prestadd_houseNo As String, groom_prestadd_locality_nm As String, groom_prestadd_city As String, groom_prestadd_st As String, groom_prestadd_dist As String, groom_prestadd_wa As String,
                                                  groom_prestadd_subdiv As String, groom_prestadd_block As String, groom_prestadd_ps As String, groom_prestadd_munigp As String, groom_prestadd_vill As String, groom_prestadd_txtvill As String,
                                                  groom_prestadd_pin As String, groom_prestadd_po As String, groom_prestadd_txtst As String, groom_prestadd_txtdist As String, groom_prestadd_txtblock As String, groom_prestadd_txtsubdiv As String, groom_prestadd_txtps As String, groom_prestadd_txtmunigp As String, groom_prestadd_txtpo As String,
                                                  groom_pmntadd_location As String, groom_pmntadd_houseNo As String, groom_pmntadd_locality_nm As String, groom_pmntadd_city As String, groom_pmntadd_st As String, groom_pmntadd_dist As String, groom_pmntadd_wa As String, groom_pmntadd_subdiv As String, groom_pmntadd_block As String, groom_pmntadd_ps As String,
                                                  groom_pmntadd_munigp As String, groom_pmntadd_vill As String, groom_pmntadd_txtvill As String, groom_pmntadd_pin As String, groom_pmntadd_po As String, groom_pmntadd_txtst As String, groom_pmntadd_txtdist As String, groom_pmntadd_txtblock As String, groom_pmntadd_txtsubdiv As String,
                                                  groom_pmntadd_txtps As String, groom_pmntadd_txtmunigp As String, groom_pmntadd_txtpo As String, is_same_address As String) As String
            Return obj.SaveGroomAddressDetails(groom_prestadd_location, groom_prestadd_houseNo, groom_prestadd_locality_nm, groom_prestadd_city, groom_prestadd_st, groom_prestadd_dist, groom_prestadd_wa,
                                                  groom_prestadd_subdiv, groom_prestadd_block, groom_prestadd_ps, groom_prestadd_munigp, groom_prestadd_vill, groom_prestadd_txtvill,
                                                  groom_prestadd_pin, groom_prestadd_po, groom_prestadd_txtst, groom_prestadd_txtdist, groom_prestadd_txtblock, groom_prestadd_txtsubdiv, groom_prestadd_txtps, groom_prestadd_txtmunigp, groom_prestadd_txtpo,
                                                  groom_pmntadd_location, groom_pmntadd_houseNo, groom_pmntadd_locality_nm, groom_pmntadd_city, groom_pmntadd_st, groom_pmntadd_dist, groom_pmntadd_wa, groom_pmntadd_subdiv, groom_pmntadd_block, groom_pmntadd_ps,
                                                  groom_pmntadd_munigp, groom_pmntadd_vill, groom_pmntadd_txtvill, groom_pmntadd_pin, groom_pmntadd_po, groom_pmntadd_txtst, groom_pmntadd_txtdist, groom_pmntadd_txtblock, groom_pmntadd_txtsubdiv,
                                                  groom_pmntadd_txtps, groom_pmntadd_txtmunigp, groom_pmntadd_txtpo, is_same_address)
        End Function
#End Region
#Region "save Bride Details"
        Public Function SaveBridePersonalDetails(Draft_Cd As String, bride_fname As String, bride_mname As String, bride_lname As String, bride_guardian_type As String, bride_father_fname As String, bride_father_mname As String, bride_father_lname As String, bride_mother_fname As String, bride_mother_mname As String, bride_mother_lname As String,
                                                bride_dob As String, bride_relegion As String, bride_relegion_other As String, bride_nationality As String, bride_mobnum_ctry As String, bride_mobile_no As String, Bride_Email_ID As String, bride_ocupatn As String,
                                                 bride_ocupatn_othr As String, bride_passport_no As String, bride_passport_ctry As String, bride_passport_valid As String, bride_adhar_num As String, bride_Aadhr_Consent As String, bride_bpl As String, bride_bpl_no As String, bride_visa_etaNO As String, bride_visa_purpose As String, bride_visa_valid As String, bride_visa_DtOfEntry As String) As String

            Return obj.SaveBridePersonalDetails(Draft_Cd, bride_fname, bride_mname, bride_lname, bride_guardian_type, bride_father_fname, bride_father_mname, bride_father_lname, bride_mother_fname, bride_mother_mname, bride_mother_lname,
                                                 bride_dob, bride_relegion, bride_relegion_other, bride_nationality, bride_mobnum_ctry, bride_mobile_no, Bride_Email_ID,
                                                 bride_ocupatn, bride_ocupatn_othr, bride_passport_no, bride_passport_ctry, bride_passport_valid, bride_adhar_num, bride_Aadhr_Consent, bride_bpl, bride_bpl_no, bride_visa_etaNO, bride_visa_purpose, bride_visa_valid, bride_visa_DtOfEntry)
        End Function
        Public Function SaveBrideMaritalDetails(draft_cd As String, Bride_marital_status As String, Bride_case_no As String, Bride_court_name As String, Bride_Dateof_order As String, Bride_issued_by As String, Bride_decree_act As String, Bride_decree_section As String, Bride_country As String, Bride_state As String, Bride_district As String) As String

            Return obj.SaveBrideMaritalDetails(draft_cd, Bride_marital_status, Bride_case_no, Bride_court_name, Bride_Dateof_order, Bride_issued_by, Bride_decree_act, Bride_decree_section, Bride_country, Bride_state, Bride_district)
        End Function
        Public Function SaveBrideAddressDetails(bride_prestadd_location As String, bride_prestadd_houseNo As String, bride_prestadd_locality_nm As String, bride_prestadd_city As String, bride_prestadd_st As String, bride_prestadd_dist As String, bride_prestadd_wa As String,
                                                  bride_prestadd_subdiv As String, bride_prestadd_block As String, bride_prestadd_ps As String, bride_prestadd_munigp As String, bride_prestadd_vill As String, bride_prestadd_txtvill As String,
                                                  bride_prestadd_pin As String, bride_prestadd_po As String, bride_prestadd_txtst As String, bride_prestadd_txtdist As String, bride_prestadd_txtblock As String, bride_prestadd_txtsubdiv As String, bride_prestadd_txtps As String, bride_prestadd_txtmunigp As String, bride_prestadd_txtpo As String,
                                                  bride_pmntadd_location As String, bride_pmntadd_houseNo As String, bride_pmntadd_locality_nm As String, bride_pmntadd_city As String, bride_pmntadd_st As String, bride_pmntadd_dist As String, bride_pmntadd_wa As String, bride_pmntadd_subdiv As String, bride_pmntadd_block As String, bride_pmntadd_ps As String,
                                                  bride_pmntadd_munigp As String, bride_pmntadd_vill As String, bride_pmntadd_txtvill As String, bride_pmntadd_pin As String, bride_pmntadd_po As String, bride_pmntadd_txtst As String, bride_pmntadd_txtdist As String, bride_pmntadd_txtblock As String, bride_pmntadd_txtsubdiv As String,
                                                  bride_pmntadd_txtps As String, bride_pmntadd_txtmunigp As String, bride_pmntadd_txtpo As String, is_same_address As String) As String
            Return obj.SaveBrideAddressDetails(bride_prestadd_location, bride_prestadd_houseNo, bride_prestadd_locality_nm, bride_prestadd_city, bride_prestadd_st, bride_prestadd_dist, bride_prestadd_wa,
                                                  bride_prestadd_subdiv, bride_prestadd_block, bride_prestadd_ps, bride_prestadd_munigp, bride_prestadd_vill, bride_prestadd_txtvill,
                                                  bride_prestadd_pin, bride_prestadd_po, bride_prestadd_txtst, bride_prestadd_txtdist, bride_prestadd_txtblock, bride_prestadd_txtsubdiv, bride_prestadd_txtps, bride_prestadd_txtmunigp, bride_prestadd_txtpo,
                                                  bride_pmntadd_location, bride_pmntadd_houseNo, bride_pmntadd_locality_nm, bride_pmntadd_city, bride_pmntadd_st, bride_pmntadd_dist, bride_pmntadd_wa, bride_pmntadd_subdiv, bride_pmntadd_block, bride_pmntadd_ps,
                                                  bride_pmntadd_munigp, bride_pmntadd_vill, bride_pmntadd_txtvill, bride_pmntadd_pin, bride_pmntadd_po, bride_pmntadd_txtst, bride_pmntadd_txtdist, bride_pmntadd_txtblock, bride_pmntadd_txtsubdiv,
                                                  bride_pmntadd_txtps, bride_pmntadd_txtmunigp, bride_pmntadd_txtpo, is_same_address)
        End Function
#End Region
#Region "Social Marriage Address"
        Public Function SaveSocialMarriageAddressDetails(Draft_Cd As String, Place_Socl_Mrg_HNo As String, Place_Socl_Mrg_Address As String, Place_Socl_Mrg_Country As String, Place_Socl_Mrg_State As String, Place_Socl_Mrg_Dist As String,
                                                         Place_Socl_Mrg_PS As String, Place_Socl_Mrg_PO As String, Socl_Mrg_Dt As String, Mrg_Invi_Card As Byte, IP_Address As String, Place_Socl_Mrg_Pin As String, Place_Socl_Mrg_txtPS As String, Place_Socl_Mrg_txtPO As String) As String

            Return obj.SaveSocialMarriageAddressDetails(Draft_Cd, Place_Socl_Mrg_HNo, Place_Socl_Mrg_Address, Place_Socl_Mrg_Country, Place_Socl_Mrg_State, Place_Socl_Mrg_Dist,
                                                        Place_Socl_Mrg_PS, Place_Socl_Mrg_PO, Socl_Mrg_Dt, Mrg_Invi_Card, IP_Address, Place_Socl_Mrg_Pin, Place_Socl_Mrg_txtPS, Place_Socl_Mrg_txtPO)
        End Function
#End Region
#Region "Registration Address Save"
        Public Function SaveRegistrationDetails(reg_mo_cd As String, registration_place As String, registration_address As String, registration_ctry As String,
                                                  registration_state As String, registartion_dist As String, registration_ps As String,
                                                registration_pin As String, registration_po As String, proposed_registration_dt As DateTime, registartion_time As String) As String

            Return obj.SaveRegistrationDetails(reg_mo_cd, registration_place, registration_address, registration_ctry,
                                                  registration_state, registartion_dist, registration_ps, registration_pin, registration_po, proposed_registration_dt, registartion_time)
        End Function
#End Region
#Region "Get Saved Draft"
        Public Function GetDraftApplication(Draft_Cd As String) As DataTable
            Return obj.GetDraftApplication(Draft_Cd)
        End Function
#End Region
#Region "File Upload"
        Public Function UploadDocProof(infos As Dictionary(Of String, Object)) As String
            obj.type = Convert.ToString(infos("type"))
            obj.proof_type = Convert.ToString(infos("proof_type"))
            obj.doc_no = Convert.ToString(infos("doc_no"))
            obj.path = Convert.ToString(infos("path"))
            obj.draft_cd = Convert.ToString(infos("draft_cd"))

            Dim ret = obj.UploadDocProof()
            Return ret
        End Function
#End Region
#Region "Image Upload"
        Public Function UploadImage(infos As Dictionary(Of String, Object)) As String
            obj.type = Convert.ToString(infos("type"))
            obj.img_byte = infos("img_byte")
            obj.draft_cd = Convert.ToString(infos("draft_cd"))

            Dim ret = obj.UploadImage()
            Return ret
        End Function
#End Region
#Region "Load Groom Documents and Image"
        Public Function LoadGroomDocumentsImage(draft_cd As String) As DataSet
            obj.draft_cd = draft_cd
            Return obj.LoadGroomDocumentsImage()
        End Function
#End Region
#Region "Child Details Upload"
        Public Function UploadChildDetails(infos As Dictionary(Of String, Object)) As String
            obj.type = Convert.ToString(infos("type"))
            obj.name = Convert.ToString(infos("c_name"))
            obj.gender = Convert.ToString(infos("c_gender"))
            obj.dob = Convert.ToString(infos("c_dob"))
            obj.proof_type = Convert.ToString(infos("proof_type"))
            obj.child_others = Convert.ToString(infos("child_others"))
            obj.doc_no = Convert.ToString(infos("doc_no"))
            obj.path = Convert.ToString(infos("path"))
            obj.draft_cd = Convert.ToString(infos("draft_cd"))

            Dim ret = obj.UploadChildDetails()
            Return ret
        End Function
        Public Function ChildDetailsList(strdraw As String, start As String, length As String, searchvalue As String, order_by As String, orderByDir As String, draft_cd As String) As MyNestedDataTable

            Dim datatable As New MyNestedDataTable
            Dim recList As New List(Of Dictionary(Of String, Object))
            Dim obj As New WS_MasterData_DB()
            Dim objPI As New MARREG_ApplyOnline_Webservice_DB
            Dim objIDR As IDataReader = Nothing
            Dim totalCount As Integer = 0
            Try

                objPI.start = Convert.ToInt32(start)
                objPI.length = Convert.ToInt32(length)
                objPI.value = Convert.ToString(searchvalue)
                objPI.order_by = Convert.ToString(order_by)
                objPI.orderByDir = Convert.ToString(orderByDir)
                objPI.draft_cd = Convert.ToString(draft_cd)
                objIDR = objPI.ChildDetailsList()

                While objIDR.Read
                    Dim rec As New Dictionary(Of String, Object)
                    totalCount = Convert.ToInt32(objIDR("TotalCount"))
                    rec("Draft_Cd") = Convert.ToString(objIDR("Draft_Cd"))
                    rec("Child_Cd") = Convert.ToString(objIDR("Child_Cd"))
                    rec("Child_Of") = Convert.ToString(objIDR("Child_Of"))
                    rec("Name") = Convert.ToString(objIDR("Name"))
                    rec("DOB") = Convert.ToString(objIDR("DOB"))
                    rec("Gender") = Convert.ToString(objIDR("Gender"))
                    rec("Choose_Child_By") = Convert.ToString(objIDR("Choose_Child_By"))
                    rec("Child_proof_type") = Convert.ToString(objIDR("Child_proof_type"))
                    rec("DOB_Proof") = Convert.ToString(objIDR("DOB_Proof"))
                    rec("Child_fileNamePath") = Convert.ToString(objIDR("Child_fileNamePath"))
                    rec("Child_proof_no") = Convert.ToString(objIDR("Child_proof_no"))
                    rec("Other") = Convert.ToString(objIDR("Other"))
                    recList.Add(rec)
                    rec = Nothing
                End While
                datatable.draw = Convert.ToInt32(strdraw)
                datatable.recordsTotal = totalCount
                datatable.recordsFiltered = If(recList.Count < totalCount, totalCount, recList.Count)
                datatable.data = recList
            Catch ex As Exception
                Throw ex
            Finally
                objIDR.Close()
                objIDR.Dispose()
                objIDR = Nothing
                obj = Nothing
            End Try
            Return datatable
        End Function

#End Region
    End Class

End Namespace

