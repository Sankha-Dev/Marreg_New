Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Collections.Generic
Imports System.Web
Imports System.Data.Common
Imports System.Configuration
Imports System.Data.SqlClient
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Namespace MARREG.App_Code.DataAccessLayer

    Public Class MARREG_Portal_Page_DB

#Region "DECLARATION"
        Public dbconnctn As New DataBaseHelperDB()
        Public dbf As New DBFactoryDB()
#End Region



#Region "Public Mo view"
        Public Function Public_MO_view(state_cd As String, district As String, ps As String, subdivision As String, block As String, Work_area As String, Muni_gp As String, Word_Mouza As String, MO_Type_Cd As String) As DataSet
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim ds = New DataSet()
            Try
                con.Open()
                Dim da As New SqlDataAdapter("Public_MO_view_search_mo", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure
                da.SelectCommand.Parameters.Add(New SqlParameter("@State_Cd", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@State_Cd").Value = state_cd
                da.SelectCommand.Parameters.Add(New SqlParameter("@district", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@district").Value = district
                da.SelectCommand.Parameters.Add(New SqlParameter("@Ps", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@Ps").Value = ps
                da.SelectCommand.Parameters.Add(New SqlParameter("@subdivision", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@subdivision").Value = subdivision
                da.SelectCommand.Parameters.Add(New SqlParameter("@Block", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@Block").Value = block
                da.SelectCommand.Parameters.Add(New SqlParameter("@Work_area", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@work_area").Value = Work_area
                da.SelectCommand.Parameters.Add(New SqlParameter("@Muni_gp", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@Muni_gp").Value = Muni_gp
                da.SelectCommand.Parameters.Add(New SqlParameter("@Word_Mouza", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@Word_Mouza").Value = Word_Mouza
                da.SelectCommand.Parameters.Add(New SqlParameter("@MO_Type_Cd", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@MO_Type_Cd").Value = MO_Type_Cd
                da.Fill(ds)
                Return ds
            Catch
                Throw
            Finally
                ds.Dispose()
                con.Close()
                con.Dispose()
            End Try
        End Function

        Public Function Public_MO_view_NameWise(MO_Name As String) As DataSet
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim ds = New DataSet()
            Try
                con.Open()
                Dim da As New SqlDataAdapter("Public_MO_view_NameWise", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure
                da.SelectCommand.Parameters.Add(New SqlParameter("@MO_name", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@MO_name").Value = MO_Name
                da.Fill(ds)
                Return ds
            Catch
                Throw
            Finally
                ds.Dispose()
                con.Close()
                con.Dispose()
            End Try
        End Function
#End Region

#Region "Mo Non Functional"
        Public Function Public_MO_NonFunctional(state_cd As String, MO_Type As String, district As String, ps As String) As DataSet
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim ds = New DataSet()
            Try
                con.Open()
                Dim da As New SqlDataAdapter("MARREG_MO_NonFunctional", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure
                da.SelectCommand.Parameters.Add(New SqlParameter("@State_Cd", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@State_Cd").Value = state_cd
                da.SelectCommand.Parameters.Add(New SqlParameter("@MO_Type", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@MO_Type").Value = MO_Type
                da.SelectCommand.Parameters.Add(New SqlParameter("@district_cd", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@district_cd").Value = district
                da.SelectCommand.Parameters.Add(New SqlParameter("@ps_cd", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@ps_cd").Value = ps
                da.Fill(ds)
                Return ds
            Catch
                Throw
            Finally
                ds.Dispose()
                con.Close()
                con.Dispose()
            End Try
        End Function
#End Region

#Region "Objection Insert"
        Public Function ObjectionSave(Application_cd As String, Objector_Name As String, Objector_Addr As String, Objector_Country_Cd As String, Objector_State_Flag As String, Objector_State As String,
                                      Objector_Dist As String, Objector_Subdiv As String, Objector_Block As String, Objector_WA As String, Objector_Muni_Gp As String, Objector_Vil_Wrd As String,
                                      Objector_PS As String, Objector_PO As String, Objector_Pin As String, Objector_Applcnt_Reltn_Cd As String, Objector_Mob_No As String, Objector_Email_Id As String,
                                      Objection_Resn_Cd As String, Objector_signature As Byte(), Ip_Address As String, Bank_Cd As String, Bank_Branch_Dist As String, Bank_Branch As String,
                                      Payment_Mode_Cd As String, Payment_Mode_No As String, Payment_Mode_Dt As String, Payable_At As String) As String
            Dim outmsg As String
            Try

                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Objector_Details_Insert")
                db.AddInParameter(dbCommand, "@App_Cd", DbType.[String], Application_cd)
                db.AddInParameter(dbCommand, "@Objector_Name", DbType.[String], Objector_Name)
                db.AddInParameter(dbCommand, "@Objector_Addr", DbType.[String], Objector_Addr)

                db.AddInParameter(dbCommand, "@Objector_Country_Cd", DbType.[String], Objector_Country_Cd)
                db.AddInParameter(dbCommand, "@Objector_State_Flag", DbType.[String], Objector_State_Flag)
                db.AddInParameter(dbCommand, "@Objector_State", DbType.[String], Objector_State)
                db.AddInParameter(dbCommand, "@Objector_Dist", DbType.[String], Objector_Dist)
                db.AddInParameter(dbCommand, "@Objector_Subdiv", DbType.[String], Objector_Subdiv)
                db.AddInParameter(dbCommand, "@Objector_Block", DbType.[String], Objector_Block)
                db.AddInParameter(dbCommand, "@Objector_WA", DbType.[String], Objector_WA)
                db.AddInParameter(dbCommand, "@Objector_Muni_Gp", DbType.[String], Objector_Muni_Gp)
                db.AddInParameter(dbCommand, "@Objector_Vil_Wrd", DbType.[String], Objector_Vil_Wrd)
                db.AddInParameter(dbCommand, "@Objector_PS", DbType.[String], Objector_PS)
                db.AddInParameter(dbCommand, "@Objector_PO", DbType.[String], Objector_PO)
                db.AddInParameter(dbCommand, "@Objector_Pin", DbType.[String], Objector_Pin)

                db.AddInParameter(dbCommand, "@Objector_Applcnt_Reltn_Cd", DbType.[String], Objector_Applcnt_Reltn_Cd)
                db.AddInParameter(dbCommand, "@Objector_Mob_No", DbType.[String], Objector_Mob_No)
                db.AddInParameter(dbCommand, "@Objector_Email_Id", DbType.[String], Objector_Email_Id)
                db.AddInParameter(dbCommand, "@Objection_Resn_Cd", DbType.[String], Objection_Resn_Cd)

                db.AddInParameter(dbCommand, "@Objector_signature", DbType.[Binary], Objector_signature)
                db.AddInParameter(dbCommand, "@Bank_Cd", DbType.[String], Bank_Cd)

                db.AddInParameter(dbCommand, "@Bank_Branch_Dist", DbType.[String], Bank_Branch_Dist)
                db.AddInParameter(dbCommand, "@Bank_Branch", DbType.[String], Bank_Branch)
                db.AddInParameter(dbCommand, "@Payment_Mode_Cd", DbType.[String], Payment_Mode_Cd)
                db.AddInParameter(dbCommand, "@Payment_Mode_No", DbType.[String], Payment_Mode_No)
                db.AddInParameter(dbCommand, "@Payment_Mode_Dt", DbType.[String], Payment_Mode_Dt)
                db.AddInParameter(dbCommand, "@Payable_At", DbType.[String], Payable_At)

                db.AddInParameter(dbCommand, "@Ip_Address", DbType.String, Ip_Address)
                db.AddOutParameter(dbCommand, "@Objector_Total_code", DbType.[String], 50)

                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Objector_Total_code"))

            Catch ex As Exception
                outmsg = "0"
            End Try
            Return outmsg
        End Function
#End Region




#Region "Contact Us Insert"
        Public Function ContactUs_Insert(Name As String, Mobile_No As String, Email_Id As String, Subject As String, Message As String) As String
            Dim outmsg As String
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_ContactUs_Insert")
                db.AddInParameter(dbCommand, "@Person_Name", DbType.[String], Name)
                db.AddInParameter(dbCommand, "@Mobile_No", DbType.[String], Mobile_No)
                db.AddInParameter(dbCommand, "@Email_Id", DbType.[String], Email_Id)
                db.AddInParameter(dbCommand, "@Subject", DbType.[String], Subject)
                db.AddInParameter(dbCommand, "@Message", DbType.[String], Message)
                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region

#Region "Feedback Insert"
        Public Function Feedback_Insert(Name As String, Email_Id As String, satisfaction_rating As String, experience_rating As String, expectation As String, Ip_Address As String) As String
            Dim outmsg As String
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Feedback_Insert")
                db.AddInParameter(dbCommand, "@Name", DbType.[String], Name)
                db.AddInParameter(dbCommand, "@Email_Id", DbType.[String], Email_Id)
                db.AddInParameter(dbCommand, "@satisfaction_rating", DbType.[String], satisfaction_rating)
                db.AddInParameter(dbCommand, "@experience_rating", DbType.[String], experience_rating)
                db.AddInParameter(dbCommand, "@expectation", DbType.[String], expectation)
                db.AddInParameter(dbCommand, "@Ip_Address", DbType.[String], Ip_Address)
                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region

#Region "Portal Login Insert Update"
#Region "Insert"
        Public Function PortalLoginSave(log_id As String, SessionID As String, visit_ipaddr As String, mac_addr As String, status As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("Marreg_Portal_Login_History")
                db.AddInParameter(dbCommand, "@Logid", DbType.[String], log_id)
                db.AddInParameter(dbCommand, "@SessionID", DbType.[String], SessionID)
                db.AddInParameter(dbCommand, "@visit_ipaddr", DbType.[String], visit_ipaddr)
                db.AddInParameter(dbCommand, "@mac_addr", DbType.[String], mac_addr)
                db.AddInParameter(dbCommand, "@Status", DbType.[String], status)
                db.AddInParameter(dbCommand, "@Tag", DbType.[String], "Insert")
                db.AddOutParameter(dbCommand, "@login_info", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@login_info"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region
#Region "Update"
        Public Function PortalLoginUpdate(log_id As String, visit_ipaddr As String, mac_addr As String, status As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("Marreg_Portal_Login_History")
                db.AddInParameter(dbCommand, "@Logid", DbType.[String], log_id)
                db.AddInParameter(dbCommand, "@visit_ipaddr", DbType.[String], visit_ipaddr)
                db.AddInParameter(dbCommand, "@mac_addr", DbType.[String], mac_addr)
                db.AddInParameter(dbCommand, "@Status", DbType.[String], status)
                db.AddInParameter(dbCommand, "@Tag", DbType.[String], "Update")
                db.AddOutParameter(dbCommand, "@login_info", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@login_info"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region
#End Region

#Region "Fetch In-Time"
        Public Function Portal_Visitors_count() As DataTable
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim ds = New DataTable()
            Try
                con.Open()
                Dim da As New SqlDataAdapter("MARREG_Portal_Visitors_Count", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure
                da.Fill(ds)
                Return ds
            Catch
                Throw
            Finally
                ds.Dispose()
                con.Close()
                con.Dispose()
            End Try
        End Function
#End Region

#Region "NewApp Payment Insert"
        Public Function NewApp_Payment_Insert(Applicant_Cd As String, Bank_cd As String, Branch_District_cd As String, Bank_Branch_cd As String, Payment_Mode_Cd As String,
                                              Payment_Mode_No As String, Payment_Mode_Dt As DateTime, Payable_At As String) As String
            Dim outmsg As String
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_NewApp_Pymnt_Insert")
                db.AddInParameter(dbCommand, "@Applicant_Cd", DbType.[String], Applicant_Cd)
                db.AddInParameter(dbCommand, "@Bank_cd", DbType.[String], Bank_cd)
                db.AddInParameter(dbCommand, "@Branch_District_cd", DbType.[String], Branch_District_cd)
                db.AddInParameter(dbCommand, "@Bank_Branch_cd", DbType.[String], Bank_Branch_cd)
                db.AddInParameter(dbCommand, "@Payment_Mode_Cd", DbType.[String], Payment_Mode_Cd)
                db.AddInParameter(dbCommand, "@Payment_Mode_No", DbType.[String], Payment_Mode_No)
                db.AddInParameter(dbCommand, "@Payment_Mode_Dt", DbType.DateTime, Payment_Mode_Dt)
                db.AddInParameter(dbCommand, "@Payable_At", DbType.[String], Payable_At)
                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region


#Region "GRIPS Payment"
        Public Function GRIPS_Insert(Applicant_Cd As String, depositor_nam As String, depositor_mob As String, depositor_email As String, pay_amnt As String) As String
            Dim outmsg As String
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_NewApp_GRIPS_Pymnt_Insert")
                db.AddInParameter(dbCommand, "@Applicant_Cd", DbType.[String], Applicant_Cd)
                db.AddInParameter(dbCommand, "@deposited_by", DbType.[String], depositor_nam)
                db.AddInParameter(dbCommand, "@depositor_mobile_no", DbType.[String], depositor_mob)
                db.AddInParameter(dbCommand, "@depositor_email_id", DbType.[String], depositor_email)
                db.AddInParameter(dbCommand, "@total_amount", DbType.[String], pay_amnt)

                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
        ' new 26.04.2018
        Public Function GRIPS_Insert_GRN(Applicant_Cd As String, depositor_nam As String, depositor_mob As String, depositor_email As String, paymnt_dt As DateTime, pay_amnt As String, GRN As String, Mo_CD As String) As Array
            Dim outmsg(4) As String
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_GRIPS_GRN_Insert")
                db.AddInParameter(dbCommand, "@Applicant_Cd", DbType.[String], Applicant_Cd)
                db.AddInParameter(dbCommand, "@deposited_by", DbType.[String], depositor_nam)
                db.AddInParameter(dbCommand, "@depositor_mobile_no", DbType.[String], depositor_mob)
                db.AddInParameter(dbCommand, "@depositor_email_id", DbType.[String], depositor_email)
                db.AddInParameter(dbCommand, "@paymnt_dt", DbType.DateTime, paymnt_dt)
                db.AddInParameter(dbCommand, "@total_amount", DbType.[String], pay_amnt)
                db.AddInParameter(dbCommand, "@pGRN", DbType.[String], GRN)
                db.AddInParameter(dbCommand, "@Mo_CD", DbType.[String], Mo_CD)
                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.AddOutParameter(dbCommand, "@Applicant_Total_Cd", DbType.[String], 50)
                db.AddOutParameter(dbCommand, "@Applicant_auto_Cd", DbType.[String], 10)
                db.AddOutParameter(dbCommand, "@file_location", DbType.[String], 100)

                db.ExecuteNonQuery(dbCommand)
                outmsg(0) = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
                outmsg(1) = Convert.ToString(db.GetParameterValue(dbCommand, "@Applicant_Total_Cd"))
                outmsg(2) = Convert.ToString(db.GetParameterValue(dbCommand, "@Applicant_auto_Cd"))
                outmsg(3) = Convert.ToString(db.GetParameterValue(dbCommand, "@file_location"))
            Catch ex As Exception
                outmsg(0) = ex.Message()
            End Try
            Return outmsg
        End Function

        Public Function GRIPS_Insert_GRN_Additional_Payment(Applicant_Cd As String, depositor_nam As String, depositor_mob As String, depositor_email As String, paymnt_dt As DateTime, pay_amnt As String, GRN As String, Mo_CD As String, dtad_data As DataTable) As String
            'Dim outmsg(4) As Array
            Dim outmsg As String
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim con As New SqlConnection(dbconnctn.GetConnectionString())
                Dim insertCommand As SqlCommand = New SqlCommand("MARREG_GRIPS_GRN_Insert_Additional_Payment", con)
                insertCommand.CommandType = CommandType.StoredProcedure

                Dim app_cd As SqlParameter = insertCommand.Parameters.AddWithValue("@Applicant_Cd", Applicant_Cd)
                app_cd.SqlDbType = SqlDbType.NVarChar
                Dim dpnm As SqlParameter = insertCommand.Parameters.AddWithValue("@deposited_by", depositor_nam)
                dpnm.SqlDbType = SqlDbType.NVarChar
                Dim dpmob As SqlParameter = insertCommand.Parameters.AddWithValue("@depositor_mobile_no", depositor_mob)
                dpmob.SqlDbType = SqlDbType.NVarChar
                Dim dpml As SqlParameter = insertCommand.Parameters.AddWithValue("@depositor_email_id", depositor_email)
                dpml.SqlDbType = SqlDbType.NVarChar
                Dim paydt As SqlParameter = insertCommand.Parameters.AddWithValue("@paymnt_dt", paymnt_dt)
                paydt.SqlDbType = SqlDbType.DateTime
                Dim payamt As SqlParameter = insertCommand.Parameters.AddWithValue("@total_amount", pay_amnt)
                payamt.SqlDbType = SqlDbType.NVarChar
                Dim grnno As SqlParameter = insertCommand.Parameters.AddWithValue("@pGRN", GRN)
                grnno.SqlDbType = SqlDbType.NVarChar
                Dim mocd As SqlParameter = insertCommand.Parameters.AddWithValue("@Mo_CD", Mo_CD)
                mocd.SqlDbType = SqlDbType.NVarChar

                'Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_GRIPS_GRN_Insert_Additional_Payment")
                'db.AddInParameter(dbCommand, "@Applicant_Cd", DbType.[String], Applicant_Cd)
                'db.AddInParameter(dbCommand, "@deposited_by", DbType.[String], depositor_nam)
                'db.AddInParameter(dbCommand, "@depositor_mobile_no", DbType.[String], depositor_mob)
                'db.AddInParameter(dbCommand, "@depositor_email_id", DbType.[String], depositor_email)
                'db.AddInParameter(dbCommand, "@paymnt_dt", DbType.DateTime, paymnt_dt)
                'db.AddInParameter(dbCommand, "@total_amount", DbType.[String], pay_amnt)
                'db.AddInParameter(dbCommand, "@pGRN", DbType.[String], GRN)
                'db.AddInParameter(dbCommand, "@Mo_CD", DbType.[String], Mo_CD)
                'db.AddInParameter(dbCommand, "@dtad_data", SqlDbType.Structured, dtad_data)
                Dim wrkg_hrs As SqlParameter = insertCommand.Parameters.AddWithValue("@dtad_data", dtad_data)
                wrkg_hrs.SqlDbType = SqlDbType.Structured

                Dim Msg As SqlParameter = insertCommand.Parameters.AddWithValue("@Msg", 50)
                Msg.SqlDbType = SqlDbType.NVarChar
                Msg.Size = 50
                Msg.Direction = ParameterDirection.Output
                con.Open()
                insertCommand.ExecuteNonQuery()
                outmsg = insertCommand.Parameters("@Msg").Value
                con.Close()

                'db.AddInParameter(dbCommand, "@MARREG_AdditionalPayment_cd", DbType.[String], MARREG_AdditionalPayment_cd)
                'db.AddInParameter(dbCommand, "@MARREG_Payment_Reason_cd", DbType.[String], MARREG_Payment_Reason_cd)

                'db.AddOutParameter(DbCommand, "@Msg", DbType.[String], 50)
                'db.ExecuteNonQuery(dbCommand)
                'outmsg(0) = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))

            Catch ex As Exception
                'outmsg(0) = ex.Message()
            End Try
            Return outmsg
        End Function

        'Public Function GRIPS_Insert_GRN_Additional_Payment(Applicant_Cd As String, depositor_nam As String, depositor_mob As String, depositor_email As String, paymnt_dt As DateTime, pay_amnt As String, GRN As String, Mo_CD As String, MARREG_AdditionalPayment_cd As String, MARREG_Payment_Reason_cd As String) As Array
        '    Dim outmsg(4) As String
        '    Try
        '        Dim db As Database = dbf.CreateConnection()
        '        Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_GRIPS_GRN_Insert_Additional_Payment")
        '        db.AddInParameter(dbCommand, "@Applicant_Cd", DbType.[String], Applicant_Cd)
        '        db.AddInParameter(dbCommand, "@deposited_by", DbType.[String], depositor_nam)
        '        db.AddInParameter(dbCommand, "@depositor_mobile_no", DbType.[String], depositor_mob)
        '        db.AddInParameter(dbCommand, "@depositor_email_id", DbType.[String], depositor_email)
        '        db.AddInParameter(dbCommand, "@paymnt_dt", DbType.DateTime, paymnt_dt)
        '        db.AddInParameter(dbCommand, "@total_amount", DbType.[String], pay_amnt)
        '        db.AddInParameter(dbCommand, "@pGRN", DbType.[String], GRN)
        '        db.AddInParameter(dbCommand, "@Mo_CD", DbType.[String], Mo_CD)
        '        db.AddInParameter(dbCommand, "@MARREG_AdditionalPayment_cd", DbType.[String], MARREG_AdditionalPayment_cd)
        '        db.AddInParameter(dbCommand, "@MARREG_Payment_Reason_cd", DbType.[String], MARREG_Payment_Reason_cd)

        '        db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
        '        'db.AddOutParameter(dbCommand, "@Applicant_Total_Cd", DbType.[String], 50)
        '        'db.AddOutParameter(dbCommand, "@Applicant_auto_Cd", DbType.[String], 10)
        '        'db.AddOutParameter(dbCommand, "@file_location", DbType.[String], 100)


        '        db.ExecuteNonQuery(dbCommand)
        '        outmsg(0) = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
        '        'outmsg(1) = Convert.ToString(db.GetParameterValue(dbCommand, "@Applicant_Total_Cd"))
        '        'outmsg(2) = Convert.ToString(db.GetParameterValue(dbCommand, "@Applicant_auto_Cd"))
        '        'outmsg(3) = Convert.ToString(db.GetParameterValue(dbCommand, "@file_location"))
        '    Catch ex As Exception
        '        outmsg(0) = ex.Message()
        '    End Try
        '    Return outmsg
        'End Function



#End Region

#Region "Fill Data table for DoughnutChart"
            Public Function Fill_DataTable(State_cd As String) As DataTable
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim dt = New DataTable()
            Try
                con.Open()
                Dim da As New SqlDataAdapter("MARREG_Certificate_Generate_Count_Annually", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure
                da.SelectCommand.Parameters.Add(New SqlParameter("@State_cd", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@State_cd").Value = State_cd
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
#End Region

#Region "fill vill wrd"
        Public Function Fill_VilWrd_Txt(Dist As String, SubDiv As String, blck As String, gp As String, Mouza_nm As String, ip_addr As String) As String
            Dim outmsg As String
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Fill_Vilwrd")

                db.AddInParameter(dbCommand, "@Dis", DbType.[String], Dist)
                db.AddInParameter(dbCommand, "@SubDiv", DbType.[String], SubDiv)
                db.AddInParameter(dbCommand, "@Blck", DbType.[String], blck)
                db.AddInParameter(dbCommand, "@Gp", DbType.[String], gp)
                db.AddInParameter(dbCommand, "@Mouza_nm", DbType.[String], Mouza_nm)
                db.AddInParameter(dbCommand, "@ip_addr", DbType.[String], ip_addr)
                db.AddOutParameter(dbCommand, "@Mouza_cd_out", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Mouza_cd_out"))
            Catch ex As Exception
                outmsg = "0"
            End Try
            Return outmsg
        End Function
#End Region

#Region "MoTrainsaction"
        Public Function MoTrainsaction() As DataSet
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim ds = New DataSet()
            Try
                con.Open()
                Dim da As New SqlDataAdapter("MARREG_Monthly_Performance_Report_Chart", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure
                da.Fill(ds)
                Return ds
            Catch
                Throw
            Finally
                ds.Dispose()
                con.Close()
                con.Dispose()
            End Try
        End Function
#End Region

#Region "Correction GRN"
        Public Function GRIPS_Insert_GRN_For_Correction(application_no As String, no_of_correction As String, grn_amount As String, grn_no As String, deposited_by As String, depositor_email_id As String, depositor_mobile_no As String, Mo_CD As String) As Array

            Dim outmsg(4) As String
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Correction_GRN_Insert")
                db.AddInParameter(dbCommand, "@Applicant_Cd", DbType.[String], application_no)
                db.AddInParameter(dbCommand, "@no_of_correction", DbType.[String], no_of_correction)
                db.AddInParameter(dbCommand, "@total_amount", DbType.[String], grn_amount)
                db.AddInParameter(dbCommand, "@grn_no", DbType.[String], grn_no)
                db.AddInParameter(dbCommand, "@deposited_by", DbType.[String], deposited_by)
                db.AddInParameter(dbCommand, "@depositor_email_id", DbType.[String], depositor_email_id)
                db.AddInParameter(dbCommand, "@depositor_mobile_no", DbType.[String], depositor_mobile_no)
                db.AddInParameter(dbCommand, "@Mo_CD", DbType.[String], Mo_CD)


                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)



                db.ExecuteNonQuery(dbCommand)
                outmsg(0) = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))

            Catch ex As Exception
                outmsg(0) = ex.Message()
            End Try
            Return outmsg
        End Function

        Public Function GRIPS_Insert_GRN_For_Correction_RGM(application_no As String, no_of_correction As String, grn_amount As String, grn_no As String, deposited_by As String, depositor_email_id As String, depositor_mobile_no As String, Mo_CD As String) As Array

            Dim outmsg(4) As String
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Correction_GRN_Insert_RGM")
                db.AddInParameter(dbCommand, "@Applicant_Cd", DbType.[String], application_no)
                db.AddInParameter(dbCommand, "@no_of_correction", DbType.[String], no_of_correction)
                db.AddInParameter(dbCommand, "@total_amount", DbType.[String], grn_amount)
                db.AddInParameter(dbCommand, "@grn_no", DbType.[String], grn_no)
                db.AddInParameter(dbCommand, "@deposited_by", DbType.[String], deposited_by)
                db.AddInParameter(dbCommand, "@depositor_email_id", DbType.[String], depositor_email_id)
                db.AddInParameter(dbCommand, "@depositor_mobile_no", DbType.[String], depositor_mobile_no)
                db.AddInParameter(dbCommand, "@Mo_CD", DbType.[String], Mo_CD)

                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)

                db.ExecuteNonQuery(dbCommand)
                outmsg(0) = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))

            Catch ex As Exception
                outmsg(0) = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region
    End Class
End Namespace
