Imports System.Data
Imports System.Data.Common
Imports System.Data.SqlClient
Imports MARREG.App_Code.DataAccessLayer
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.VisualBasic

Namespace MARREG.App_Code
    Public Class MARREG_ApplyOnline_Webservice_DB


#Region "Declaration"
        Public dbconnctn As New DataBaseHelperDB()
        Dim dbf As New DBFactoryDB()
        Private con As New SqlConnection(dbconnctn.GetConnectionString())
        Public Property id As Int64
        Public Property Reg_Id As String
        Public Property f_name As String
        Public Property m_name As String
        Public Property l_name As String
        Public Property gender As String
        Public Property dob As String
        Public Property dtdob As DateTime
        Public Property name As String
        Public Property mob_no As String
        Public Property email As String

        Public Property type As String
        Public Property proof_type As String
        Public Property child_others As String
        Public Property doc_no As String
        Public Property path As String
        Public Property draft_cd As String
        Public Property img_byte As Byte()

        Public Property start As Integer
        Public Property length As Integer
        Public Property value As String
        Public Property order_by As String
        Public Property orderByDir As String
#End Region
#Region "User Registration"
#Region "Email & Mobile No Validation for Registration"
        Public Function CheckRegEmail() As Integer
            Dim ReturnVal As Integer
            Try
                con.Open()
                Using command As New SqlCommand("WS_CheckRegEmailMobile", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@email", SqlDbType.VarChar).Value = email
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
        Public Function CheckRegMobile() As Integer
            Dim ReturnVal As Integer
            Try
                con.Open()
                Using command As New SqlCommand("WS_CheckRegEmailMobile", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@mob_no", SqlDbType.VarChar).Value = mob_no
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
#Region "Save Data"
        Public Function SaveUserRegistration() As String
            Dim ReturnVal As String
            Try
                con.Open()
                Using command As New SqlCommand("WS_MARREG_SaveUserRegistration", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@f_name", SqlDbType.VarChar).Value = f_name
                    command.Parameters.Add("@m_name", SqlDbType.VarChar).Value = m_name
                    command.Parameters.Add("@l_name", SqlDbType.VarChar).Value = l_name
                    command.Parameters.Add("@gender", SqlDbType.VarChar).Value = gender
                    command.Parameters.Add("@dob", SqlDbType.VarChar).Value = dob
                    command.Parameters.Add("@email_id", SqlDbType.VarChar).Value = email
                    command.Parameters.Add("@mob_no", SqlDbType.VarChar).Value = mob_no
                    command.Parameters.Add("@return_val", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output
                    command.ExecuteNonQuery()
                    ReturnVal = command.Parameters("@return_val").Value.ToString()
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                con.Close()
            End Try
            Return ReturnVal
        End Function
#End Region
#Region "Load Registration Data"
        Public Function LoadRegistrationData() As DataTable
            Dim ds = New DataSet()
            Try
                con.Open()
                Using da As New SqlDataAdapter("WS_MARREG_LoadRegistrationData", con)
                    da.SelectCommand.CommandType = CommandType.StoredProcedure
                    da.SelectCommand.CommandTimeout = 120
                    da.SelectCommand.Parameters.Add("@Reg_Id", SqlDbType.VarChar).Value = Reg_Id
                    da.Fill(ds)
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                con.Close()
            End Try
            Return ds.Tables(0)
        End Function
#End Region
#Region "Login Details Validation"
        Public Function VerifyLogin() As DataTable
            Dim ds = New DataSet()
            Try
                con.Open()
                Using da As New SqlDataAdapter("WS_MARREG_VerifyLogin", con)
                    da.SelectCommand.CommandType = CommandType.StoredProcedure
                    da.SelectCommand.CommandTimeout = 120
                    da.SelectCommand.Parameters.Add("@Reg_Id", SqlDbType.VarChar).Value = Reg_Id
                    da.SelectCommand.Parameters.Add("@dob", SqlDbType.VarChar).Value = dob
                    da.Fill(ds)
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                con.Close()
            End Try
            Return ds.Tables(0)
        End Function
#End Region
#End Region
#Region "Update Initial Details Apply Online"
        Public Function Update_Initial_Details_Apply_Online() As String
            Dim ReturnVal As String
            Try
                con.Open()
                Using command As New SqlCommand("WS_MARREG_Update_Initial_Details_Apply_Online", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@Reg_Id", SqlDbType.VarChar).Value = Reg_Id

                    command.Parameters.Add("@return_val", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output
                    command.ExecuteNonQuery()
                    ReturnVal = command.Parameters("@return_val").Value.ToString()
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                con.Close()
            End Try
            Return ReturnVal
        End Function
#End Region
#Region "Load Apply Online Details"
        Public Function Load_Apply_Online_Details() As DataTable
            Dim ds = New DataSet()
            Try
                con.Open()
                Using da As New SqlDataAdapter("WS_MARREG_Load_Apply_Online_Details", con)
                    da.SelectCommand.CommandType = CommandType.StoredProcedure
                    da.SelectCommand.CommandTimeout = 120
                    da.SelectCommand.Parameters.Add("@Reg_Id", SqlDbType.VarChar).Value = Reg_Id
                    da.Fill(ds)
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                con.Close()
            End Try
            Return ds.Tables(0)
        End Function
#End Region

        Public Function GetDraftApplication(Draft_Cd As String) As DataTable
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim dt = New DataTable()
            Try
                con.Open()
                Dim da As New SqlDataAdapter("WS_MARREG_Get_Detais_MARREG_Draft", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure

                da.SelectCommand.Parameters.Add(New SqlParameter("@Draft_Cd", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@Draft_Cd").Value = Draft_Cd

                da.Fill(dt)
                Return dt
            Catch
                Throw
            Finally
                dt.Dispose()
                con.Close()
                con.Dispose()
            End Try
        End Function

#Region "Groom Details Save"
        Public Function SaveGroomPersonalDetails(Draft_Cd As String, Act As String, groom_fname As String, groom_mname As String, groom_lname As String, groom_guardian_type As String, groom_father_fname As String, groom_father_mname As String, groom_father_lname As String, groom_mother_fname As String, gromm_mother_mname As String, groom_mother_lname As String,
                                                 groom_dob As String, groom_relegion As String, groom_relegion_other As String, groom_nationality As String, groom_mobnum_ctry As String, groom_mobile_no As String, Groom_Email_ID As String, groom_ocupatn As String,
                                                 groom_ocupatn_othr As String, groom_passport_no As String, groom_passport_ctry As String, groom_passport_valid As String, groom_adhar_num As String, Groom_Aadhr_Consent As String, groom_bpl As String, groom_bpl_no As String,
                                                 groom_visa_etaNO As String, groom_visa_purpose As String, groom_visa_valid As String, groom_visa_DtOfEntry As String) As String

            Dim outmsg As String
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Groom_Personal_Details_Save_SP1")

                db.AddInParameter(dbCommand, "@Draft_Cd", DbType.[String], Draft_Cd)
                db.AddInParameter(dbCommand, "@Act_Cd", DbType.[String], Act)
                db.AddInParameter(dbCommand, "@Groom_Fname", DbType.[String], groom_fname)
                db.AddInParameter(dbCommand, "@Groom_MNam", DbType.[String], groom_mname)
                db.AddInParameter(dbCommand, "@Groom_LNam", DbType.[String], groom_lname)
                db.AddInParameter(dbCommand, "@Groom_Guardian_type", DbType.[String], groom_guardian_type)
                db.AddInParameter(dbCommand, "@Groom_Father_FNam", DbType.[String], groom_father_fname)
                db.AddInParameter(dbCommand, "@Groom_Father_MNam", DbType.[String], groom_father_mname)
                db.AddInParameter(dbCommand, "@Groom_Father_LNam", DbType.[String], groom_father_lname)
                db.AddInParameter(dbCommand, "@Groom_Mother_FNam", DbType.[String], groom_mother_fname)
                db.AddInParameter(dbCommand, "@Groom_Mother_MNam", DbType.[String], gromm_mother_mname)
                db.AddInParameter(dbCommand, "@Groom_Mother_LNam", DbType.[String], groom_mother_lname)
                db.AddInParameter(dbCommand, "@Groom_DOB", DbType.String, groom_dob)
                db.AddInParameter(dbCommand, "@Groom_Religion", DbType.[String], groom_relegion)
                db.AddInParameter(dbCommand, "@Groom_Religion_Other_Name", DbType.[String], groom_relegion_other)
                db.AddInParameter(dbCommand, "@Groom_Nationality", DbType.[String], groom_nationality)
                db.AddInParameter(dbCommand, "@Groom_MobNum_Ctry", DbType.[String], groom_mobnum_ctry)
                db.AddInParameter(dbCommand, "@Groom_Mob_Num", DbType.[String], groom_mobile_no)
                db.AddInParameter(dbCommand, "@Groom_Email", DbType.[String], Groom_Email_ID)

                db.AddInParameter(dbCommand, "@Groom_occupation", DbType.[String], groom_ocupatn)
                db.AddInParameter(dbCommand, "@Groom_Occupation_Other_Name", DbType.[String], groom_ocupatn_othr)
                db.AddInParameter(dbCommand, "@Groom_Aadhr_Num", DbType.[String], groom_adhar_num)
                db.AddInParameter(dbCommand, "@Groom_Aadhr_Consent", DbType.[String], Groom_Aadhr_Consent)
                db.AddInParameter(dbCommand, "@Groom_passport_no", DbType.[String], groom_passport_no)
                db.AddInParameter(dbCommand, "@Groom_passport_ctry", DbType.[String], groom_passport_ctry)
                db.AddInParameter(dbCommand, "@Groom_passport_valid", DbType.String, groom_passport_valid)
                db.AddInParameter(dbCommand, "@Groom_bpl_status", DbType.[String], groom_bpl)
                db.AddInParameter(dbCommand, "@Groom_bpl_no", DbType.String, groom_bpl_no)
                db.AddInParameter(dbCommand, "@Groom_visa_etano", DbType.[String], groom_visa_etaNO)
                db.AddInParameter(dbCommand, "@Groom_visa_purpose", DbType.String, groom_visa_purpose)
                db.AddInParameter(dbCommand, "@Groom_visa_valid", DbType.String, groom_visa_valid)
                db.AddInParameter(dbCommand, "@Groom_visa_dateofentry", DbType.String, groom_visa_DtOfEntry)

                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = "0"
            End Try
            Return outmsg
        End Function
        Public Function SaveGroomMaritalDetails(Draft_Cd As String, groom_marital_status As String, groom_case_no As String, groom_court_name As String, groom_Dateof_order As String, groom_issued_by As String, groom_decree_act As String, groom_decree_section As String, groom_country As String, groom_state As String, groom_district As String) As String
            Dim outmsg As String
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Groom_Marital_Details_Save_SP2")

                db.AddInParameter(dbCommand, "@Draft_Cd", DbType.[String], Draft_Cd)
                db.AddInParameter(dbCommand, "@Groom_maritial_status", DbType.[String], groom_marital_status)
                db.AddInParameter(dbCommand, "@Groom_nullity_case_no", DbType.[String], groom_case_no)
                db.AddInParameter(dbCommand, "@Groom_nullity_court_name", DbType.[String], groom_court_name)
                db.AddInParameter(dbCommand, "@Groom_nullity_dateOfOrder", DbType.String, groom_Dateof_order)
                db.AddInParameter(dbCommand, "@Groom_nullity_issuedBy", DbType.[String], groom_issued_by)
                db.AddInParameter(dbCommand, "@Groom_nullity_court_Country", DbType.[String], groom_country)
                db.AddInParameter(dbCommand, "@Groom_nullity_court_state", DbType.[String], groom_state)
                db.AddInParameter(dbCommand, "@Groom_nullity_court_district", DbType.[String], groom_district)
                db.AddInParameter(dbCommand, "@Groom_decree_act", DbType.[String], groom_decree_act)
                db.AddInParameter(dbCommand, "@Groom_decree_section", DbType.[String], groom_decree_section)

                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = "0"
            End Try
            Return outmsg
        End Function

        Public Function SaveGroomAddressDetails(groom_prestadd_location As String, groom_prestadd_houseNo As String, groom_prestadd_locality_nm As String, groom_prestadd_city As String, groom_prestadd_st As String, groom_prestadd_dist As String, groom_prestadd_wa As String,
                                                  groom_prestadd_subdiv As String, groom_prestadd_block As String, groom_prestadd_ps As String, groom_prestadd_munigp As String, groom_prestadd_vill As String, groom_prestadd_txtvill As String,
                                                  groom_prestadd_pin As String, groom_prestadd_po As String, groom_prestadd_txtst As String, groom_prestadd_txtdist As String, groom_prestadd_txtblock As String, groom_prestadd_txtsubdiv As String, groom_prestadd_txtps As String, groom_prestadd_txtmunigp As String, groom_prestadd_txtpo As String,
                                                  groom_pmntadd_location As String, groom_pmntadd_houseNo As String, groom_pmntadd_locality_nm As String, groom_pmntadd_city As String, groom_pmntadd_st As String, groom_pmntadd_dist As String, groom_pmntadd_wa As String, groom_pmntadd_subdiv As String, groom_pmntadd_block As String, groom_pmntadd_ps As String,
                                                  groom_pmntadd_munigp As String, groom_pmntadd_vill As String, groom_pmntadd_txtvill As String, groom_pmntadd_pin As String, groom_pmntadd_po As String, groom_pmntadd_txtst As String, groom_pmntadd_txtdist As String, groom_pmntadd_txtblock As String, groom_pmntadd_txtsubdiv As String,
                                                  groom_pmntadd_txtps As String, groom_pmntadd_txtmunigp As String, groom_pmntadd_txtpo As String, is_same_address As String) As String

            Dim outmsg As String
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Groom_Address_Details_Save_SP3")


                db.AddInParameter(dbCommand, "@Draft_Cd", DbType.[String], "9413")
                db.AddInParameter(dbCommand, "@Groom_prestadd_location", DbType.[String], groom_prestadd_location)
                db.AddInParameter(dbCommand, "@Groom_Present_HNo", DbType.[String], groom_prestadd_houseNo)
                db.AddInParameter(dbCommand, "@Groom_Present_Address", DbType.[String], groom_prestadd_locality_nm)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_Country", DbType.String, groom_prestadd_city)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_State", DbType.String, groom_prestadd_st)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_Dist", DbType.[String], groom_prestadd_dist)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_SubDiv", DbType.[String], groom_prestadd_subdiv)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_Block", DbType.[String], groom_prestadd_block)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_PS", DbType.[String], groom_prestadd_ps)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_WA", DbType.[String], groom_prestadd_wa)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_MuniGp", DbType.[String], groom_prestadd_munigp)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_VilWrd", DbType.[String], groom_prestadd_vill)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_PO", DbType.[String], groom_prestadd_po)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_Pin", DbType.[String], groom_prestadd_pin)
                db.AddInParameter(dbCommand, "@Groom_prestadd_txtvill", DbType.[String], groom_prestadd_txtvill)
                db.AddInParameter(dbCommand, "@Groom_prestadd_txtst", DbType.String, groom_prestadd_txtst)
                db.AddInParameter(dbCommand, "@Groom_prestadd_txtdist", DbType.[String], groom_prestadd_txtdist)
                db.AddInParameter(dbCommand, "@Groom_prestadd_txtblock", DbType.[String], groom_prestadd_txtblock)
                db.AddInParameter(dbCommand, "@Groom_prestadd_txtsubdiv", DbType.[String], groom_prestadd_txtsubdiv)
                db.AddInParameter(dbCommand, "@Groom_prestadd_txtps", DbType.[String], groom_prestadd_txtps)
                db.AddInParameter(dbCommand, "@Groom_prestadd_txtmunigp", DbType.[String], groom_prestadd_txtmunigp)
                db.AddInParameter(dbCommand, "@Groom_prestadd_txtpo", DbType.[String], groom_prestadd_txtpo)
                '22
                db.AddInParameter(dbCommand, "@Groom_pmntadd_location", DbType.[String], groom_pmntadd_location)
                db.AddInParameter(dbCommand, "@Groom_Permanent_HNo", DbType.[String], groom_pmntadd_houseNo)
                db.AddInParameter(dbCommand, "@Groom_Permanent_Address", DbType.[String], groom_pmntadd_locality_nm)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_Country", DbType.String, groom_pmntadd_city)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_State", DbType.String, groom_pmntadd_st)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_Dist", DbType.[String], groom_pmntadd_dist)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_SubDiv", DbType.[String], groom_pmntadd_subdiv)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_Block", DbType.[String], groom_pmntadd_block)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_PS", DbType.[String], groom_pmntadd_ps)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_WA", DbType.[String], groom_pmntadd_wa)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_MuniGp", DbType.[String], groom_pmntadd_munigp)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_VilWrd", DbType.[String], groom_pmntadd_vill)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_PO", DbType.[String], groom_pmntadd_po)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_Pin", DbType.[String], groom_pmntadd_pin)
                db.AddInParameter(dbCommand, "@Groom_pmntadd_txtvill", DbType.[String], groom_pmntadd_txtvill)
                db.AddInParameter(dbCommand, "@Groom_pmntadd_txtst", DbType.String, groom_pmntadd_txtst)
                db.AddInParameter(dbCommand, "@Groom_pmntadd_txtdist", DbType.[String], groom_pmntadd_txtdist)
                db.AddInParameter(dbCommand, "@Groom_pmntadd_txtblock", DbType.[String], groom_pmntadd_txtblock)
                db.AddInParameter(dbCommand, "@Groom_pmntadd_txtsubdiv", DbType.[String], groom_pmntadd_txtsubdiv)
                db.AddInParameter(dbCommand, "@Groom_pmntadd_txtps", DbType.[String], groom_pmntadd_txtps)
                db.AddInParameter(dbCommand, "@Groom_pmntadd_txtmunigp", DbType.[String], groom_pmntadd_txtmunigp)
                db.AddInParameter(dbCommand, "@Groom_pmntadd_txtpo", DbType.[String], groom_pmntadd_txtpo)
                db.AddInParameter(dbCommand, "@Is_same_address", DbType.[String], is_same_address)

                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = "0"
            End Try
            Return outmsg
        End Function
#End Region

#Region "Bride Details Save"

        Public Function SaveBridePersonalDetails(Draft_Cd As String, bride_fname As String, bride_mname As String, bride_lname As String, bride_guardian_type As String, bride_father_fname As String, bride_father_mname As String, bride_father_lname As String, bride_mother_fname As String, bride_mother_mname As String, bride_mother_lname As String,
                                                 bride_dob As String, bride_relegion As String, bride_relegion_other As String, bride_nationality As String, bride_mobnum_ctry As String, bride_mobile_no As String, Bride_Email_ID As String, bride_ocupatn As String,
                                                 bride_ocupatn_othr As String, bride_passport_no As String, bride_passport_ctry As String, bride_passport_valid As String, bride_adhar_num As String, bride_Aadhr_Consent As String, bride_bpl As String, bride_bpl_no As String,
                                                 bride_visa_etaNO As String, bride_visa_purpose As String, bride_visa_valid As String, bride_visa_DtOfEntry As String) As String

            Dim outmsg As String
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_bride_Personal_Details_Save_SP5")
                db.AddInParameter(dbCommand, "@Draft_Cd", DbType.[String], Draft_Cd)
                db.AddInParameter(dbCommand, "@bride_Fname", DbType.[String], bride_fname)
                db.AddInParameter(dbCommand, "@bride_MNam", DbType.[String], bride_mname)
                db.AddInParameter(dbCommand, "@bride_LNam", DbType.[String], bride_lname)
                db.AddInParameter(dbCommand, "@bride_Guardian_type", DbType.[String], bride_guardian_type)
                db.AddInParameter(dbCommand, "@bride_Father_FNam", DbType.[String], bride_father_fname)
                db.AddInParameter(dbCommand, "@bride_Father_MNam", DbType.[String], bride_father_mname)
                db.AddInParameter(dbCommand, "@bride_Father_LNam", DbType.[String], bride_father_lname)
                db.AddInParameter(dbCommand, "@bride_Mother_FNam", DbType.[String], bride_mother_fname)
                db.AddInParameter(dbCommand, "@bride_Mother_MNam", DbType.[String], bride_mother_mname)
                db.AddInParameter(dbCommand, "@bride_Mother_LNam", DbType.[String], bride_mother_lname)
                db.AddInParameter(dbCommand, "@bride_DOB", DbType.String, bride_dob)
                db.AddInParameter(dbCommand, "@bride_Religion", DbType.[String], bride_relegion)
                db.AddInParameter(dbCommand, "@bride_Religion_Other_Name", DbType.[String], bride_relegion_other)
                db.AddInParameter(dbCommand, "@bride_Nationality", DbType.[String], bride_nationality)
                db.AddInParameter(dbCommand, "@bride_MobNum_Ctry", DbType.[String], bride_mobnum_ctry)
                db.AddInParameter(dbCommand, "@bride_Mob_Num", DbType.[String], bride_mobile_no)
                'db.AddInParameter(dbCommand, "@bride_Email", DbType.[String], bride_email)
                db.AddInParameter(dbCommand, "@Bride_Email_ID", DbType.[String], Bride_Email_ID)
                db.AddInParameter(dbCommand, "@bride_occupation", DbType.[String], bride_ocupatn)
                db.AddInParameter(dbCommand, "@bride_Occupation_Other_Name", DbType.[String], bride_ocupatn_othr)
                db.AddInParameter(dbCommand, "@bride_Aadhr_Num", DbType.[String], bride_adhar_num)
                db.AddInParameter(dbCommand, "@bride_Aadhr_Consent", DbType.[String], bride_Aadhr_Consent)
                db.AddInParameter(dbCommand, "@bride_passport_no", DbType.[String], bride_passport_no)
                db.AddInParameter(dbCommand, "@bride_passport_ctry", DbType.[String], bride_passport_ctry)
                db.AddInParameter(dbCommand, "@bride_passport_valid", DbType.String, bride_passport_valid)
                db.AddInParameter(dbCommand, "@bride_bpl_status", DbType.[String], bride_bpl)
                db.AddInParameter(dbCommand, "@bride_bpl_no", DbType.String, bride_bpl_no)
                db.AddInParameter(dbCommand, "@bride_visa_etano", DbType.[String], bride_visa_etaNO)
                db.AddInParameter(dbCommand, "@bride_visa_purpose", DbType.String, bride_visa_purpose)
                db.AddInParameter(dbCommand, "@bride_visa_valid", DbType.String, bride_visa_valid)
                db.AddInParameter(dbCommand, "@bride_visa_dateofentry", DbType.String, bride_visa_DtOfEntry)

                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = "0"
            End Try
            Return outmsg
        End Function

        Public Function SaveBrideMaritalDetails(draft_cd As String, Bride_marital_status As String, Bride_case_no As String, Bride_court_name As String, Bride_Dateof_order As String, Bride_issued_by As String, Bride_decree_act As String, Bride_decree_section As String, Bride_country As String, Bride_state As String, Bride_district As String) As String

            Dim outmsg As String
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Bride_Marital_Details_Save_SP6")

                db.AddInParameter(dbCommand, "@Draft_Cd", DbType.[String], draft_cd)
                db.AddInParameter(dbCommand, "@Bride_maritial_status", DbType.[String], Bride_marital_status)
                db.AddInParameter(dbCommand, "@Bride_nullity_case_no", DbType.[String], Bride_case_no)
                db.AddInParameter(dbCommand, "@Bride_nullity_court_name", DbType.[String], Bride_court_name)
                db.AddInParameter(dbCommand, "@Bride_nullity_dateOfOrder", DbType.[String], Bride_Dateof_order)
                db.AddInParameter(dbCommand, "@Bride_nullity_issuedBy", DbType.[String], Bride_issued_by)
                db.AddInParameter(dbCommand, "@Bride_nullity_court_Country", DbType.[String], Bride_country)
                db.AddInParameter(dbCommand, "@Bride_nullity_court_state", DbType.[String], Bride_state)
                db.AddInParameter(dbCommand, "@Bride_nullity_court_district", DbType.[String], Bride_district)
                db.AddInParameter(dbCommand, "@Bride_decree_act", DbType.[String], Bride_decree_act)
                db.AddInParameter(dbCommand, "@Bride_decree_section", DbType.[String], Bride_decree_section)

                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = "0"
            End Try
            Return outmsg
        End Function

        Public Function SaveBrideAddressDetails(bride_prestadd_location As String, bride_prestadd_houseNo As String, bride_prestadd_locality_nm As String, bride_prestadd_city As String, bride_prestadd_st As String, bride_prestadd_dist As String, bride_prestadd_wa As String,
                                                  bride_prestadd_subdiv As String, bride_prestadd_block As String, bride_prestadd_ps As String, bride_prestadd_munigp As String, bride_prestadd_vill As String, bride_prestadd_txtvill As String,
                                                  bride_prestadd_pin As String, bride_prestadd_po As String, bride_prestadd_txtst As String, bride_prestadd_txtdist As String, bride_prestadd_txtblock As String, bride_prestadd_txtsubdiv As String, bride_prestadd_txtps As String, bride_prestadd_txtmunigp As String, bride_prestadd_txtpo As String,
                                                  bride_pmntadd_location As String, bride_pmntadd_houseNo As String, bride_pmntadd_locality_nm As String, bride_pmntadd_city As String, bride_pmntadd_st As String, bride_pmntadd_dist As String, bride_pmntadd_wa As String, bride_pmntadd_subdiv As String, bride_pmntadd_block As String, bride_pmntadd_ps As String,
                                                  bride_pmntadd_munigp As String, bride_pmntadd_vill As String, bride_pmntadd_txtvill As String, bride_pmntadd_pin As String, bride_pmntadd_po As String, bride_pmntadd_txtst As String, bride_pmntadd_txtdist As String, bride_pmntadd_txtblock As String, bride_pmntadd_txtsubdiv As String,
                                                  bride_pmntadd_txtps As String, bride_pmntadd_txtmunigp As String, bride_pmntadd_txtpo As String, is_same_address As String) As String

            Dim outmsg As String
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_bride_Address_Details_Save_SP7")

                db.AddInParameter(dbCommand, "@Draft_Cd", DbType.[String], "9413")
                db.AddInParameter(dbCommand, "@bride_prestadd_location", DbType.[String], bride_prestadd_location)
                db.AddInParameter(dbCommand, "@bride_Present_HNo", DbType.[String], bride_prestadd_houseNo)
                db.AddInParameter(dbCommand, "@bride_Present_Address", DbType.[String], bride_prestadd_locality_nm)
                db.AddInParameter(dbCommand, "@bride_PrsntAdd_Country", DbType.String, bride_prestadd_city)
                db.AddInParameter(dbCommand, "@bride_PrsntAdd_State", DbType.String, bride_prestadd_st)
                db.AddInParameter(dbCommand, "@bride_PrsntAdd_Dist", DbType.[String], bride_prestadd_dist)
                db.AddInParameter(dbCommand, "@bride_PrsntAdd_SubDiv", DbType.[String], bride_prestadd_subdiv)
                db.AddInParameter(dbCommand, "@bride_PrsntAdd_Block", DbType.[String], bride_prestadd_block)
                db.AddInParameter(dbCommand, "@bride_PrsntAdd_PS", DbType.[String], bride_prestadd_ps)
                db.AddInParameter(dbCommand, "@bride_PrsntAdd_WA", DbType.[String], bride_prestadd_wa)
                db.AddInParameter(dbCommand, "@bride_PrsntAdd_MuniGp", DbType.[String], bride_prestadd_munigp)
                db.AddInParameter(dbCommand, "@bride_PrsntAdd_VilWrd", DbType.[String], bride_prestadd_vill)
                db.AddInParameter(dbCommand, "@bride_PrsntAdd_PO", DbType.[String], bride_prestadd_po)
                db.AddInParameter(dbCommand, "@bride_PrsntAdd_Pin", DbType.[String], bride_prestadd_pin)
                db.AddInParameter(dbCommand, "@bride_prestadd_txtvill", DbType.[String], bride_prestadd_txtvill)
                db.AddInParameter(dbCommand, "@bride_prestadd_txtst", DbType.String, bride_prestadd_txtst)
                db.AddInParameter(dbCommand, "@bride_prestadd_txtdist", DbType.[String], bride_prestadd_txtdist)
                db.AddInParameter(dbCommand, "@bride_prestadd_txtblock", DbType.[String], bride_prestadd_txtblock)
                db.AddInParameter(dbCommand, "@bride_prestadd_txtsubdiv", DbType.[String], bride_prestadd_txtsubdiv)
                db.AddInParameter(dbCommand, "@bride_prestadd_txtps", DbType.[String], bride_prestadd_txtps)
                db.AddInParameter(dbCommand, "@bride_prestadd_txtmunigp", DbType.[String], bride_prestadd_txtmunigp)
                db.AddInParameter(dbCommand, "@bride_prestadd_txtpo", DbType.[String], bride_prestadd_txtpo)
                '22
                db.AddInParameter(dbCommand, "@bride_pmntadd_location", DbType.[String], bride_pmntadd_location)
                db.AddInParameter(dbCommand, "@bride_Permanent_HNo", DbType.[String], bride_pmntadd_houseNo)
                db.AddInParameter(dbCommand, "@bride_Permanent_Address", DbType.[String], bride_pmntadd_locality_nm)
                db.AddInParameter(dbCommand, "@bride_PrmntAdd_Country", DbType.String, bride_pmntadd_city)
                db.AddInParameter(dbCommand, "@bride_PrmntAdd_State", DbType.String, bride_pmntadd_st)
                db.AddInParameter(dbCommand, "@bride_PrmntAdd_Dist", DbType.[String], bride_pmntadd_dist)
                db.AddInParameter(dbCommand, "@bride_PrmntAdd_SubDiv", DbType.[String], bride_pmntadd_subdiv)
                db.AddInParameter(dbCommand, "@bride_PrmntAdd_Block", DbType.[String], bride_pmntadd_block)
                db.AddInParameter(dbCommand, "@bride_PrmntAdd_PS", DbType.[String], bride_pmntadd_ps)
                db.AddInParameter(dbCommand, "@bride_PrmntAdd_WA", DbType.[String], bride_pmntadd_wa)
                db.AddInParameter(dbCommand, "@bride_PrmntAdd_MuniGp", DbType.[String], bride_pmntadd_munigp)
                db.AddInParameter(dbCommand, "@bride_PrmntAdd_VilWrd", DbType.[String], bride_pmntadd_vill)
                db.AddInParameter(dbCommand, "@bride_PrmntAdd_PO", DbType.[String], bride_pmntadd_po)
                db.AddInParameter(dbCommand, "@bride_PrmntAdd_Pin", DbType.[String], bride_pmntadd_pin)
                db.AddInParameter(dbCommand, "@bride_pmntadd_txtvill", DbType.[String], bride_pmntadd_txtvill)
                db.AddInParameter(dbCommand, "@bride_pmntadd_txtst", DbType.String, bride_pmntadd_txtst)
                db.AddInParameter(dbCommand, "@bride_pmntadd_txtdist", DbType.[String], bride_pmntadd_txtdist)
                db.AddInParameter(dbCommand, "@bride_pmntadd_txtblock", DbType.[String], bride_pmntadd_txtblock)
                db.AddInParameter(dbCommand, "@bride_pmntadd_txtsubdiv", DbType.[String], bride_pmntadd_txtsubdiv)
                db.AddInParameter(dbCommand, "@bride_pmntadd_txtps", DbType.[String], bride_pmntadd_txtps)
                db.AddInParameter(dbCommand, "@bride_pmntadd_txtmunigp", DbType.[String], bride_pmntadd_txtmunigp)
                db.AddInParameter(dbCommand, "@bride_pmntadd_txtpo", DbType.[String], bride_pmntadd_txtpo)

                db.AddInParameter(dbCommand, "@Is_same_address", DbType.[String], is_same_address)

                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = "0"
            End Try
            Return outmsg
        End Function
#End Region

#Region "Socila Marriage Address Save"

        Public Function SaveSocialMarriageAddressDetails(Draft_Cd As String, Place_Socl_Mrg_HNo As String, Place_Socl_Mrg_Address As String, Place_Socl_Mrg_Country As String, Place_Socl_Mrg_State As String, Place_Socl_Mrg_Dist As String,
            Place_Socl_Mrg_PS As String, Place_Socl_Mrg_PO As String, Socl_Mrg_Dt As String, Mrg_Invi_Card As Byte, IP_Address As String, Place_Socl_Mrg_Pin As String, Place_Socl_Mrg_txtPS As String, Place_Socl_Mrg_txtPO As String) As String

            Dim outmsg As String
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_SocialMarriage_Details_Save_SP8")

                db.AddInParameter(dbCommand, "@Draft_cd", DbType.[String], Draft_Cd)
                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_HNo", DbType.[String], Place_Socl_Mrg_HNo)
                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_Address", DbType.[String], Place_Socl_Mrg_Address)
                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_Country", DbType.[String], Place_Socl_Mrg_Country)
                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_State", DbType.[String], Place_Socl_Mrg_State)
                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_Dist", DbType.[String], Place_Socl_Mrg_Dist)
                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_PS", DbType.[String], Place_Socl_Mrg_PS)
                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_PO", DbType.[String], Place_Socl_Mrg_PO)
                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_Pin", DbType.String, Place_Socl_Mrg_Pin)
                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_txtPS", DbType.[String], Place_Socl_Mrg_txtPS)
                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_txtPO", DbType.String, Place_Socl_Mrg_txtPO)

                db.AddInParameter(dbCommand, "@Socl_Mrg_Dt", DbType.String, Socl_Mrg_Dt)
                db.AddInParameter(dbCommand, "@Mrg_Invi_Card", DbType.Byte, Mrg_Invi_Card)
                db.AddInParameter(dbCommand, "@IP_Address", DbType.[String], IP_Address)

                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)

                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = "0"
            End Try
            Return outmsg
        End Function
#End Region

#Region "Registration Address  Save"
        Public Function SaveRegistrationDetails(reg_mo_cd As String, registration_place As String, registration_address As String, registration_ctry As String,
                                                  registration_state As String, registartion_dist As String, registration_ps As String,
                                                registration_pin As String, registration_po As String, proposed_registration_dt As DateTime, registartion_time As String) As String

            Dim outmsg As String
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Registration_Details_Save_SP9")

                db.AddInParameter(dbCommand, "@Mo_Cd", DbType.[String], reg_mo_cd)
                db.AddInParameter(dbCommand, "@Place_of_Registration", DbType.[String], registration_place)
                db.AddInParameter(dbCommand, "@Place_Reg_Address", DbType.[String], registration_address)
                db.AddInParameter(dbCommand, "@Place_Reg_State", DbType.[String], registration_state)
                db.AddInParameter(dbCommand, "@Place_Reg_Dist", DbType.[String], registartion_dist)
                db.AddInParameter(dbCommand, "@Place_Reg_PS", DbType.[String], registration_ps)
                db.AddInParameter(dbCommand, "@Place_Reg_PO", DbType.[String], registration_po)
                ' db.AddInParameter(dbCommand, "@Groom_Father_MNam", DbType.[String], registration_pin)
                db.AddInParameter(dbCommand, "@Proposed_Reg_Dt", DbType.DateTime, proposed_registration_dt)
                db.AddInParameter(dbCommand, "@Registration_Time", DbType.[String], registartion_time)
                db.AddInParameter(dbCommand, "@Draft_cd", DbType.[String], "9413")
                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)

                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))


            Catch ex As Exception
                outmsg = "0"

            End Try
            Return outmsg
        End Function
#End Region
#Region "File Upload"
        Public Function UploadDocProof() As String
            Dim ReturnVal As String
            Try
                con.Open()
                Using command As New SqlCommand("WS_MARREG_UploadDocProof", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@type", SqlDbType.VarChar).Value = type
                    command.Parameters.Add("@proof_type", SqlDbType.VarChar).Value = proof_type
                    command.Parameters.Add("@doc_no", SqlDbType.VarChar).Value = doc_no
                    command.Parameters.Add("@path", SqlDbType.VarChar).Value = path
                    command.Parameters.Add("@draft_cd", SqlDbType.VarChar).Value = draft_cd
                    command.Parameters.Add("@return_val", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output
                    command.ExecuteNonQuery()
                    ReturnVal = command.Parameters("@return_val").Value.ToString()
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                con.Close()
            End Try
            Return ReturnVal
        End Function
#End Region
#Region "Image Upload"
        Public Function UploadImage() As String
            Dim ReturnVal As String
            Try
                con.Open()
                Using command As New SqlCommand("WS_MARREG_UploadImageProof", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@type", SqlDbType.VarChar).Value = type
                    command.Parameters.Add("@img_byte", SqlDbType.VarBinary).Value = img_byte
                    command.Parameters.Add("@draft_cd", SqlDbType.VarChar).Value = draft_cd
                    command.Parameters.Add("@return_val", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output
                    command.ExecuteNonQuery()
                    ReturnVal = command.Parameters("@return_val").Value.ToString()
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                con.Close()
            End Try
            Return ReturnVal
        End Function
#End Region
#Region "Load Groom Documents and Image"
        Public Function LoadGroomDocumentsImage() As DataSet
            Dim ds As New DataSet
            Try
                con.Open()
                Using da As New SqlDataAdapter("WS_LoadGroomDocumentsImage", con)
                    da.SelectCommand.CommandType = CommandType.StoredProcedure
                    da.SelectCommand.Parameters.Add("@draft_cd", SqlDbType.NVarChar).Value = draft_cd
                    da.Fill(ds)
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                con.Close()
            End Try
            Return ds
        End Function
#End Region
#Region "Child Details Upload"
        Public Function UploadChildDetails() As String
            Dim ReturnVal As String
            Try
                con.Open()
                Using command As New SqlCommand("WS_MARREG_UploadChildDetails", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@type", SqlDbType.VarChar).Value = type
                    command.Parameters.Add("@Name", SqlDbType.VarChar).Value = name
                    command.Parameters.Add("@DOB", SqlDbType.VarChar).Value = dob
                    command.Parameters.Add("@Gender", SqlDbType.VarChar).Value = gender
                    command.Parameters.Add("@proof_type", SqlDbType.VarChar).Value = proof_type
                    command.Parameters.Add("@child_others", SqlDbType.VarChar).Value = child_others
                    command.Parameters.Add("@doc_no", SqlDbType.VarChar).Value = doc_no
                    command.Parameters.Add("@path", SqlDbType.VarChar).Value = path
                    command.Parameters.Add("@draft_cd", SqlDbType.VarChar).Value = draft_cd
                    command.Parameters.Add("@return_val", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output
                    command.ExecuteNonQuery()
                    ReturnVal = command.Parameters("@return_val").Value.ToString()
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                con.Close()
            End Try
            Return ReturnVal
        End Function
        Function ChildDetailsList() As IDataReader
            Dim dr As IDataReader = Nothing
            Try
                con.Open()
                Using command As New SqlCommand("WS_MARREG_ChildDetailsList", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@start", SqlDbType.Int).Value = start
                    command.Parameters.Add("@length", SqlDbType.Int).Value = length
                    command.Parameters.Add("@value", SqlDbType.NVarChar).Value = value
                    command.Parameters.Add("@order_by", SqlDbType.NVarChar).Value = order_by
                    command.Parameters.Add("@orderByDir", SqlDbType.NVarChar).Value = orderByDir
                    command.Parameters.Add("@draft_cd", SqlDbType.NVarChar).Value = draft_cd
                    dr = command.ExecuteReader()
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                'con.Close()
            End Try
            Return dr
        End Function
#End Region
    End Class
End Namespace
