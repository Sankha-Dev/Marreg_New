Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports System.Configuration
Imports System.Data.SqlClient

Namespace MARREG.App_Code.DataAccessLayer
    Public Class Apply_Online_Db

#Region "DECLARATION"
        Public dbconnctn As New DataBaseHelperDB()
        Public dbf As New DBFactoryDB()
#End Region

#Region "Function"

#Region "Insert Through XML"
        Public Function xmlLoad(tabname As DataSet) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_New_Applicant_Insert")
                db.AddInParameter(dbCommand, "@XMLdata", DbType.Xml, tabname.GetXml())
                db.AddInParameter(dbCommand, "@Form_id", DbType.String, "1235")
                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
                Return outmsg
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region

#Region "MO View in Apply Online"
        Public Function ViewMO(act As String, district As String, ps As String, subdivision As String, block As String, Work_area As String, Muni_gp As String, Word_Mouza As String) As DataSet
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim ds = New DataSet()
            Try
                con.Open()
                '04.04.2016
                'Dim da As New SqlDataAdapter("Jurisdiction_worknghours_wise_MO_view", con)
                Dim da As New SqlDataAdapter("Jurisdiction_worknghours_wise_MO_view_applyonln", con)
                '04.04.2016
                da.SelectCommand.CommandType = CommandType.StoredProcedure
                da.SelectCommand.Parameters.Add(New SqlParameter("@act_cd", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@act_cd").Value = act
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

        Public Function ViewMOUpdated(act As String, district As String, ps As String, subdivision As String, block As String, Work_area As String, Muni_gp As String, Word_Mouza As String) As DataSet
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim ds = New DataSet()
            Try
                con.Open()
                '04.04.2016
                'Dim da As New SqlDataAdapter("Jurisdiction_worknghours_wise_MO_view", con)
                Dim da As New SqlDataAdapter("Jurisdiction_worknghours_wise_MO_view_applyonln_Updated", con)
                '04.04.2016
                da.SelectCommand.CommandType = CommandType.StoredProcedure
                da.SelectCommand.Parameters.Add(New SqlParameter("@act_cd", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@act_cd").Value = act
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

#Region "New App Insert"

#Region "Groom & Bride Details"

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
        '    Dim outmsg(3) As String
        '    Try
        '        Dim db As Database = dbf.CreateConnection()
        '        Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_New_Applicant_insertBLDB")
        '        db.AddInParameter(dbCommand, "@Form_id", DbType.[String], Form_id)
        '        db.AddInParameter(dbCommand, "@Act_cd", DbType.[String], Act_cd)

        '        db.AddInParameter(dbCommand, "@Groom_FNam", DbType.[String], Groom_FNam)
        '        db.AddInParameter(dbCommand, "@Groom_MNam", DbType.[String], Groom_MNam)
        '        db.AddInParameter(dbCommand, "@Groom_LNam", DbType.[String], Groom_LNam)
        '        db.AddInParameter(dbCommand, "@Groom_Father_Nam", DbType.[String], Groom_Father_Nam)
        '        db.AddInParameter(dbCommand, "@Groom_Mother_Nam", DbType.[String], Groom_Mother_Nam)

        '        db.AddInParameter(dbCommand, "@Groom_Present_Address", DbType.[String], Groom_Present_Address)
        '        db.AddInParameter(dbCommand, "@Groom_PrsntAdd_Country", DbType.[String], Groom_PrsntAdd_Country)
        '        db.AddInParameter(dbCommand, "@Groom_PrsntAdd_State", DbType.[String], Groom_PrsntAdd_State)
        '        db.AddInParameter(dbCommand, "@Groom_PrsntAdd_State_flag", DbType.[String], Groom_PrsntAdd_State_flag)
        '        db.AddInParameter(dbCommand, "@Groom_PrsntAdd_Dist", DbType.[String], Groom_PrsntAdd_Dist)
        '        db.AddInParameter(dbCommand, "@Groom_PrsntAdd_SubDiv", DbType.[String], Groom_PrsntAdd_SubDiv)
        '        db.AddInParameter(dbCommand, "@Groom_PrsntAdd_Block", DbType.[String], Groom_PrsntAdd_Block)
        '        db.AddInParameter(dbCommand, "@Groom_PrsntAdd_PS", DbType.[String], Groom_PrsntAdd_PS)
        '        db.AddInParameter(dbCommand, "@Groom_PrsntAdd_WA", DbType.[String], Groom_PrsntAdd_WA)
        '        db.AddInParameter(dbCommand, "@Groom_PrsntAdd_MuniGp", DbType.[String], Groom_PrsntAdd_MuniGp)
        '        db.AddInParameter(dbCommand, "@Groom_PrsntAdd_VilWrd", DbType.[String], Groom_PrsntAdd_VilWrd)
        '        db.AddInParameter(dbCommand, "@Groom_PrsntAdd_PO", DbType.[String], Groom_PrsntAdd_PO)
        '        db.AddInParameter(dbCommand, "@Groom_PrsntAdd_Pin", DbType.[String], Groom_PrsntAdd_Pin)

        '        db.AddInParameter(dbCommand, "@Groom_Permanent_Address", DbType.[String], Groom_Permanent_Address)
        '        db.AddInParameter(dbCommand, "@Groom_PrmntAdd_Country", DbType.[String], Groom_PrmntAdd_Country)
        '        db.AddInParameter(dbCommand, "@Groom_PrmntAdd_State", DbType.[String], Groom_PrmntAdd_State)
        '        db.AddInParameter(dbCommand, "@Groom_PrmntAdd_State_flag", DbType.[String], Groom_PrmntAdd_State_flag)
        '        db.AddInParameter(dbCommand, "@Groom_PrmntAdd_Dist", DbType.[String], Groom_PrmntAdd_Dist)
        '        db.AddInParameter(dbCommand, "@Groom_PrmntAdd_SubDiv", DbType.[String], Groom_PrmntAdd_SubDiv)
        '        db.AddInParameter(dbCommand, "@Groom_PrmntAdd_Block", DbType.[String], Groom_PrmntAdd_Block)
        '        db.AddInParameter(dbCommand, "@Groom_PrmntAdd_PS", DbType.[String], Groom_PrmntAdd_PS)
        '        db.AddInParameter(dbCommand, "@Groom_PrmntAdd_WA", DbType.[String], Groom_PrmntAdd_WA)
        '        db.AddInParameter(dbCommand, "@Groom_PrmntAdd_MuniGp", DbType.[String], Groom_PrmntAdd_MuniGp)
        '        db.AddInParameter(dbCommand, "@Groom_PrmntAdd_VilWrd", DbType.[String], Groom_PrmntAdd_VilWrd)
        '        db.AddInParameter(dbCommand, "@Groom_PrmntAdd_PO", DbType.[String], Groom_PrmntAdd_PO)
        '        db.AddInParameter(dbCommand, "@Groom_PrmntAdd_Pin", DbType.[String], Groom_PrmntAdd_Pin)

        '        db.AddInParameter(dbCommand, "@Groom_Mob_Num", DbType.[String], Groom_Mob_Num)
        '        db.AddInParameter(dbCommand, "@Groom_dob", DbType.DateTime, Groom_dob)
        '        db.AddInParameter(dbCommand, "@Groom_Email_Id", DbType.[String], Groom_Email_Id)
        '        db.AddInParameter(dbCommand, "@Groom_Religion", DbType.[String], Groom_Religion)
        '        db.AddInParameter(dbCommand, "@Groom_Aadhr_Num", DbType.[String], Groom_Aadhr_Num)
        '        db.AddInParameter(dbCommand, "@Groom_occupation", DbType.[String], Groom_occupation)
        '        db.AddInParameter(dbCommand, "@Groom_maritial_status", DbType.[String], Groom_maritial_status)
        '        db.AddInParameter(dbCommand, "@Groom_Prev_Mar_Dt", DbType.[String], Groom_Prev_Mar_Dt)
        '        db.AddInParameter(dbCommand, "@Groom_Photo", DbType.Binary, Groom_Photo)
        '        db.AddInParameter(dbCommand, "@Groom_Sign", DbType.Binary, Groom_Sign)

        '        db.AddInParameter(dbCommand, "@Groom_Proof_PrsntAdd_Type", DbType.[String], Groom_Proof_PrsntAdd_Type)
        '        db.AddInParameter(dbCommand, "@Groom_Proof_PrsntAdd", DbType.Binary, Groom_Proof_PrsntAdd)
        '        db.AddInParameter(dbCommand, "@Groom_Proof_PrmntAdd_Type", DbType.[String], Groom_Proof_PrmntAdd_Type)
        '        db.AddInParameter(dbCommand, "@Groom_Proof_PrmntAdd", DbType.Binary, Groom_Proof_PrmntAdd)
        '        db.AddInParameter(dbCommand, "@Groom_Proof_DOB_Type", DbType.[String], Groom_Proof_DOB_Type)
        '        db.AddInParameter(dbCommand, "@Groom_Proof_DOB", DbType.Binary, Groom_Proof_DOB)

        '        db.AddInParameter(dbCommand, "@Bride_FNam", DbType.[String], Bride_FNam)
        '        db.AddInParameter(dbCommand, "@Bride_MNam", DbType.[String], Bride_MNam)
        '        db.AddInParameter(dbCommand, "@Bride_LNam", DbType.[String], Bride_LNam)
        '        db.AddInParameter(dbCommand, "@Bride_Father_Nam", DbType.[String], Bride_Father_Nam)
        '        db.AddInParameter(dbCommand, "@Bride_Mother_Nam", DbType.[String], Bride_Mother_Nam)

        '        db.AddInParameter(dbCommand, "@Bride_Present_Address", DbType.[String], Bride_Present_Address)
        '        db.AddInParameter(dbCommand, "@Bride_PrsntAdd_Country", DbType.[String], Bride_PrsntAdd_Country)
        '        db.AddInParameter(dbCommand, "@Bride_PrsntAdd_State", DbType.[String], Bride_PrsntAdd_State)
        '        db.AddInParameter(dbCommand, "@Bride_PrsntAdd_State_flag", DbType.[String], Bride_PrsntAdd_State_flag)
        '        db.AddInParameter(dbCommand, "@Bride_PrsntAdd_Dist", DbType.[String], Bride_PrsntAdd_Dist)
        '        db.AddInParameter(dbCommand, "@Bride_PrsntAdd_SubDiv", DbType.[String], Bride_PrsntAdd_SubDiv)
        '        db.AddInParameter(dbCommand, "@Bride_PrsntAdd_Block", DbType.[String], Bride_PrsntAdd_Block)
        '        db.AddInParameter(dbCommand, "@Bride_PrsntAdd_PS", DbType.[String], Bride_PrsntAdd_PS)
        '        db.AddInParameter(dbCommand, "@Bride_PrsntAdd_WA", DbType.[String], Bride_PrsntAdd_WA)
        '        db.AddInParameter(dbCommand, "@Bride_PrsntAdd_MuniGp", DbType.[String], Bride_PrsntAdd_MuniGp)
        '        db.AddInParameter(dbCommand, "@Bride_PrsntAdd_VilWrd", DbType.[String], Bride_PrsntAdd_VilWrd)
        '        db.AddInParameter(dbCommand, "@Bride_PrsntAdd_PO", DbType.[String], Bride_PrsntAdd_PO)
        '        db.AddInParameter(dbCommand, "@Bride_PrsntAdd_Pin", DbType.[String], Bride_PrsntAdd_Pin)

        '        db.AddInParameter(dbCommand, "@Bride_Permanent_Address", DbType.[String], Bride_Permanent_Address)
        '        db.AddInParameter(dbCommand, "@Bride_PrmntAdd_Country", DbType.[String], Bride_PrmntAdd_Country)
        '        db.AddInParameter(dbCommand, "@Bride_PrmntAdd_State", DbType.[String], Bride_PrmntAdd_State)
        '        db.AddInParameter(dbCommand, "@Bride_PrmntAdd_State_flag", DbType.[String], Bride_PrmntAdd_State_flag)
        '        db.AddInParameter(dbCommand, "@Bride_PrmntAdd_Dist", DbType.[String], Bride_PrmntAdd_Dist)
        '        db.AddInParameter(dbCommand, "@Bride_PrmntAdd_SubDiv", DbType.[String], Bride_PrmntAdd_SubDiv)
        '        db.AddInParameter(dbCommand, "@Bride_PrmntAdd_Block", DbType.[String], Bride_PrmntAdd_Block)
        '        db.AddInParameter(dbCommand, "@Bride_PrmntAdd_PS", DbType.[String], Bride_PrmntAdd_PS)
        '        db.AddInParameter(dbCommand, "@Bride_PrmntAdd_WA", DbType.[String], Bride_PrmntAdd_WA)
        '        db.AddInParameter(dbCommand, "@Bride_PrmntAdd_MuniGp", DbType.[String], Bride_PrmntAdd_MuniGp)
        '        db.AddInParameter(dbCommand, "@Bride_PrmntAdd_VilWrd", DbType.[String], Bride_PrmntAdd_VilWrd)
        '        db.AddInParameter(dbCommand, "@Bride_PrmntAdd_PO", DbType.[String], Bride_PrmntAdd_PO)
        '        db.AddInParameter(dbCommand, "@Bride_PrmntAdd_Pin", DbType.[String], Bride_PrmntAdd_Pin)

        '        db.AddInParameter(dbCommand, "@Bride_Mob_Num", DbType.[String], Bride_Mob_Num)
        '        db.AddInParameter(dbCommand, "@Bride_dob", DbType.DateTime, Bride_dob)
        '        db.AddInParameter(dbCommand, "@Bride_Email_Id", DbType.[String], Bride_Email_Id)
        '        db.AddInParameter(dbCommand, "@Bride_Religion", DbType.[String], Bride_Religion)
        '        db.AddInParameter(dbCommand, "@Bride_Aadhr_Num", DbType.[String], Bride_Aadhr_Num)
        '        db.AddInParameter(dbCommand, "@Bride_Occupation", DbType.[String], Bride_Occupation)
        '        db.AddInParameter(dbCommand, "@Bride_maritial_status", DbType.[String], Bride_maritial_status)
        '        db.AddInParameter(dbCommand, "@Bride_Prev_Mar_Dt", DbType.[String], Bride_Prev_Mar_Dt)
        '        db.AddInParameter(dbCommand, "@Bride_Photo", DbType.Binary, Bride_Photo)
        '        db.AddInParameter(dbCommand, "@Bride_Sign", DbType.Binary, Bride_Sign)

        '        db.AddInParameter(dbCommand, "@Bride_Proof_PrsntAdd_Type", DbType.[String], Bride_Proof_PrsntAdd_Type)
        '        db.AddInParameter(dbCommand, "@Bride_Proof_PrsntAdd", DbType.Binary, Bride_Proof_PrsntAdd)
        '        db.AddInParameter(dbCommand, "@Bride_Proof_PrmntAdd_Type", DbType.[String], Bride_Proof_PrmntAdd_Type)
        '        db.AddInParameter(dbCommand, "@Bride_Proof_PrmntAdd", DbType.Binary, Bride_Proof_PrmntAdd)
        '        db.AddInParameter(dbCommand, "@Bride_Proof_DOB_Type", DbType.[String], Bride_Proof_DOB_Type)
        '        db.AddInParameter(dbCommand, "@Bride_Proof_DOB", DbType.Binary, Bride_Proof_DOB)

        '        db.AddInParameter(dbCommand, "@MO_Autoincrement_cd", DbType.[String], MO_Autoincrement_cd)
        '        db.AddInParameter(dbCommand, "@Place_Reg_Address", DbType.[String], Place_Reg_Address)
        '        db.AddInParameter(dbCommand, "@Place_Reg_State", DbType.[String], Place_Reg_State)
        '        db.AddInParameter(dbCommand, "@Place_Reg_Dist", DbType.[String], Place_Reg_Dist)
        '        db.AddInParameter(dbCommand, "@Place_Reg_SubDiv", DbType.[String], Place_Reg_SubDiv)
        '        db.AddInParameter(dbCommand, "@Place_Reg_Block", DbType.[String], Place_Reg_Block)
        '        db.AddInParameter(dbCommand, "@Place_Reg_PS", DbType.[String], Place_Reg_PS)
        '        db.AddInParameter(dbCommand, "@Place_Reg_WA", DbType.[String], Place_Reg_WA)
        '        db.AddInParameter(dbCommand, "@Place_Reg_MuniGp", DbType.[String], Place_Reg_MuniGp)
        '        db.AddInParameter(dbCommand, "@Place_Reg_VilWrd", DbType.[String], Place_Reg_VilWrd)
        '        db.AddInParameter(dbCommand, "@Place_Reg_PO", DbType.[String], Place_Reg_PO)

        '        db.AddInParameter(dbCommand, "@Place_Socl_Mrg_Address", DbType.[String], Place_Socl_Mrg_Address)
        '        db.AddInParameter(dbCommand, "@Place_Socl_Mrg_Country", DbType.[String], Place_Socl_Mrg_Country)
        '        db.AddInParameter(dbCommand, "@Place_Socl_Mrg_State", DbType.[String], Place_Socl_Mrg_State)
        '        db.AddInParameter(dbCommand, "@Place_Socl_Mrg_State_flag", DbType.[String], Place_Socl_Mrg_State_flag)
        '        db.AddInParameter(dbCommand, "@Place_Socl_Mrg_Dist", DbType.[String], Place_Socl_Mrg_Dist)
        '        db.AddInParameter(dbCommand, "@Place_Socl_Mrg_SubDiv", DbType.[String], Place_Socl_Mrg_SubDiv)
        '        db.AddInParameter(dbCommand, "@Place_Socl_Mrg_Block", DbType.[String], Place_Socl_Mrg_Block)
        '        db.AddInParameter(dbCommand, "@Place_Socl_Mrg_PS", DbType.[String], Place_Socl_Mrg_PS)
        '        db.AddInParameter(dbCommand, "@Place_Socl_Mrg_WA", DbType.[String], Place_Socl_Mrg_WA)
        '        db.AddInParameter(dbCommand, "@Place_Socl_Mrg_MuniGp", DbType.[String], Place_Socl_Mrg_MuniGp)
        '        db.AddInParameter(dbCommand, "@Place_Socl_Mrg_VilWrd", DbType.[String], Place_Socl_Mrg_VilWrd)
        '        db.AddInParameter(dbCommand, "@Place_Socl_Mrg_PO", DbType.[String], Place_Socl_Mrg_PO)
        '        db.AddInParameter(dbCommand, "@Place_Socl_Mrg_Pin", DbType.[String], Place_Socl_Mrg_Pin)

        '        db.AddInParameter(dbCommand, "@Socl_Mrg_Dt", DbType.[String], Socl_Mrg_Dt)
        '        db.AddInParameter(dbCommand, "@Mrg_Invi_Card", DbType.Binary, Mrg_Invi_Card)
        '        db.AddInParameter(dbCommand, "@Payment_flag", DbType.[String], Payment_flag)
        '        'db.AddInParameter(dbCommand, "@Bank_cd", DbType.[String], Bank_cd)
        '        'db.AddInParameter(dbCommand, "@Branch_District_cd", DbType.[String], Branch_District_cd)
        '        'db.AddInParameter(dbCommand, "@Bank_Branch_cd", DbType.[String], Bank_Branch_cd)
        '        'db.AddInParameter(dbCommand, "@Challan_No", DbType.[String], Challan_No)
        '        'db.AddInParameter(dbCommand, "@Challan_Dt", DbType.DateTime, Challan_Dt)
        '        db.AddInParameter(dbCommand, "@Place_Reg", DbType.[String], Place_Reg)
        '        db.AddInParameter(dbCommand, "@Time_Reg", DbType.[String], Time_Reg)
        '        db.AddInParameter(dbCommand, "@Ip_Address", DbType.String, IP_Address)

        '        'db.AddInParameter(dbCommand, "@grm_bpl_holder_nm", DbType.[String], grm_bpl_holder_nm)
        '        'db.AddInParameter(dbCommand, "@bride_bpl_holder_nm", DbType.[String], bride_bpl_holder_nm)
        '        db.AddInParameter(dbCommand, "@grm_bpl_prf", DbType.Binary, grm_bpl_prf)
        '        db.AddInParameter(dbCommand, "@bride_bpl_prf", DbType.Binary, bride_bpl_prf)
        '        db.AddInParameter(dbCommand, "@gbpl_serial_cd", DbType.[String], gbpl_serial_cd)
        '        db.AddInParameter(dbCommand, "@bbpl_serial_cd", DbType.[String], bbpl_serial_cd)
        '        db.AddInParameter(dbCommand, "@grm_bpl_status", DbType.[String], grm_bpl_status)
        '        db.AddInParameter(dbCommand, "@bride_bpl_status", DbType.[String], bride_bpl_status)
        '        db.AddInParameter(dbCommand, "@applicant_bpl_status", DbType.[String], applicant_bpl_status)

        '        db.AddInParameter(dbCommand, "@Groom_Aadhr_Consent", DbType.[String], gadhrconsent)
        '        db.AddInParameter(dbCommand, "@Bride_Aadhr_Consent", DbType.[String], badhrconsent)

        '        db.AddInParameter(dbCommand, "@Groom_Nationality", DbType.[String], Groom_Nationality)
        '        db.AddInParameter(dbCommand, "@Bride_Nationality", DbType.[String], Bride_Nationality)

        '        db.AddOutParameter(dbCommand, "@Applicant_Total_Cd", DbType.[String], 50)
        '        db.AddOutParameter(dbCommand, "@Applicant_auto_Cd", DbType.[String], 10)
        '        'db.AddOutParameter(dbCommand, "@MO_Details", DbType.[String], 1000)
        '        db.ExecuteNonQuery(dbCommand)
        '        outmsg(0) = Convert.ToString(db.GetParameterValue(dbCommand, "@Applicant_Total_Cd"))
        '        outmsg(1) = Convert.ToString(db.GetParameterValue(dbCommand, "@Applicant_auto_Cd"))
        '        'outmsg(2) = Convert.ToString(db.GetParameterValue(dbCommand, "@MO_Details"))
        '        'outmsg(1) = "1"
        '    Catch ex As Exception
        '        'outmsg = ex.Message()
        '        outmsg(0) = "0"
        '    End Try
        '    Return outmsg
        'End Function
#End Region

#Region "With proof As bytearray"
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
        '    Dim outmsg(3) As String
        '    Try
        '        Dim db As Database = dbf.CreateConnection()
        '        Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_New_Applicant_insertBLDB")
        '        db.AddInParameter(dbCommand, "@Draft_No", DbType.[String], Draft_No)
        '        db.AddInParameter(dbCommand, "@Form_id", DbType.[String], Form_id)
        '        db.AddInParameter(dbCommand, "@Act_cd", DbType.[String], Act_cd)

        '        db.AddInParameter(dbCommand, "@Groom_FNam", DbType.[String], Groom_FNam)
        '        db.AddInParameter(dbCommand, "@Groom_MNam", DbType.[String], Groom_MNam)
        '        db.AddInParameter(dbCommand, "@Groom_LNam", DbType.[String], Groom_LNam)
        '        db.AddInParameter(dbCommand, "@Groom_Father_Nam", DbType.[String], Groom_Father_Nam)
        '        db.AddInParameter(dbCommand, "@Groom_Mother_Nam", DbType.[String], Groom_Mother_Nam)

        '        db.AddInParameter(dbCommand, "@Groom_Present_Address", DbType.[String], Groom_Present_Address)
        '        db.AddInParameter(dbCommand, "@Groom_PrsntAdd_Country", DbType.[String], Groom_PrsntAdd_Country)
        '        db.AddInParameter(dbCommand, "@Groom_PrsntAdd_State", DbType.[String], Groom_PrsntAdd_State)
        '        db.AddInParameter(dbCommand, "@Groom_PrsntAdd_State_flag", DbType.[String], Groom_PrsntAdd_State_flag)
        '        db.AddInParameter(dbCommand, "@Groom_PrsntAdd_Dist", DbType.[String], Groom_PrsntAdd_Dist)
        '        db.AddInParameter(dbCommand, "@Groom_PrsntAdd_SubDiv", DbType.[String], Groom_PrsntAdd_SubDiv)
        '        db.AddInParameter(dbCommand, "@Groom_PrsntAdd_Block", DbType.[String], Groom_PrsntAdd_Block)
        '        db.AddInParameter(dbCommand, "@Groom_PrsntAdd_PS", DbType.[String], Groom_PrsntAdd_PS)
        '        db.AddInParameter(dbCommand, "@Groom_PrsntAdd_WA", DbType.[String], Groom_PrsntAdd_WA)
        '        db.AddInParameter(dbCommand, "@Groom_PrsntAdd_MuniGp", DbType.[String], Groom_PrsntAdd_MuniGp)
        '        db.AddInParameter(dbCommand, "@Groom_PrsntAdd_VilWrd", DbType.[String], Groom_PrsntAdd_VilWrd)
        '        db.AddInParameter(dbCommand, "@Groom_PrsntAdd_PO", DbType.[String], Groom_PrsntAdd_PO)
        '        db.AddInParameter(dbCommand, "@Groom_PrsntAdd_Pin", DbType.[String], Groom_PrsntAdd_Pin)

        '        db.AddInParameter(dbCommand, "@Groom_Permanent_Address", DbType.[String], Groom_Permanent_Address)
        '        db.AddInParameter(dbCommand, "@Groom_PrmntAdd_Country", DbType.[String], Groom_PrmntAdd_Country)
        '        db.AddInParameter(dbCommand, "@Groom_PrmntAdd_State", DbType.[String], Groom_PrmntAdd_State)
        '        db.AddInParameter(dbCommand, "@Groom_PrmntAdd_State_flag", DbType.[String], Groom_PrmntAdd_State_flag)
        '        db.AddInParameter(dbCommand, "@Groom_PrmntAdd_Dist", DbType.[String], Groom_PrmntAdd_Dist)
        '        db.AddInParameter(dbCommand, "@Groom_PrmntAdd_SubDiv", DbType.[String], Groom_PrmntAdd_SubDiv)
        '        db.AddInParameter(dbCommand, "@Groom_PrmntAdd_Block", DbType.[String], Groom_PrmntAdd_Block)
        '        db.AddInParameter(dbCommand, "@Groom_PrmntAdd_PS", DbType.[String], Groom_PrmntAdd_PS)
        '        db.AddInParameter(dbCommand, "@Groom_PrmntAdd_WA", DbType.[String], Groom_PrmntAdd_WA)
        '        db.AddInParameter(dbCommand, "@Groom_PrmntAdd_MuniGp", DbType.[String], Groom_PrmntAdd_MuniGp)
        '        db.AddInParameter(dbCommand, "@Groom_PrmntAdd_VilWrd", DbType.[String], Groom_PrmntAdd_VilWrd)
        '        db.AddInParameter(dbCommand, "@Groom_PrmntAdd_PO", DbType.[String], Groom_PrmntAdd_PO)
        '        db.AddInParameter(dbCommand, "@Groom_PrmntAdd_Pin", DbType.[String], Groom_PrmntAdd_Pin)

        '        db.AddInParameter(dbCommand, "@Groom_Mob_Num", DbType.[String], Groom_Mob_Num)
        '        db.AddInParameter(dbCommand, "@Groom_dob", DbType.DateTime, Groom_dob)
        '        db.AddInParameter(dbCommand, "@Groom_Email_Id", DbType.[String], Groom_Email_Id)
        '        db.AddInParameter(dbCommand, "@Groom_Religion", DbType.[String], Groom_Religion)
        '        db.AddInParameter(dbCommand, "@Groom_Aadhr_Num", DbType.[String], Groom_Aadhr_Num)
        '        db.AddInParameter(dbCommand, "@Groom_occupation", DbType.[String], Groom_occupation)
        '        db.AddInParameter(dbCommand, "@Groom_maritial_status", DbType.[String], Groom_maritial_status)
        '        db.AddInParameter(dbCommand, "@Groom_Prev_Mar_Dt", DbType.[String], Groom_Prev_Mar_Dt)
        '        db.AddInParameter(dbCommand, "@Groom_Photo", DbType.Binary, Groom_Photo)
        '        db.AddInParameter(dbCommand, "@Groom_Sign", DbType.Binary, Groom_Sign)

        '        db.AddInParameter(dbCommand, "@Groom_Proof_PrsntAdd_Type", DbType.[String], Groom_Proof_PrsntAdd_Type)
        '        db.AddInParameter(dbCommand, "@Groom_Proof_PrsntAdd", DbType.Binary, Groom_Proof_PrsntAdd)
        '        db.AddInParameter(dbCommand, "@Groom_Proof_PrmntAdd_Type", DbType.[String], Groom_Proof_PrmntAdd_Type)
        '        db.AddInParameter(dbCommand, "@Groom_Proof_PrmntAdd", DbType.Binary, Groom_Proof_PrmntAdd)
        '        db.AddInParameter(dbCommand, "@Groom_Proof_DOB_Type", DbType.[String], Groom_Proof_DOB_Type)
        '        db.AddInParameter(dbCommand, "@Groom_Proof_DOB", DbType.Binary, Groom_Proof_DOB)

        '        db.AddInParameter(dbCommand, "@Bride_FNam", DbType.[String], Bride_FNam)
        '        db.AddInParameter(dbCommand, "@Bride_MNam", DbType.[String], Bride_MNam)
        '        db.AddInParameter(dbCommand, "@Bride_LNam", DbType.[String], Bride_LNam)
        '        db.AddInParameter(dbCommand, "@Bride_Father_Nam", DbType.[String], Bride_Father_Nam)
        '        db.AddInParameter(dbCommand, "@Bride_Mother_Nam", DbType.[String], Bride_Mother_Nam)

        '        db.AddInParameter(dbCommand, "@Bride_Present_Address", DbType.[String], Bride_Present_Address)
        '        db.AddInParameter(dbCommand, "@Bride_PrsntAdd_Country", DbType.[String], Bride_PrsntAdd_Country)
        '        db.AddInParameter(dbCommand, "@Bride_PrsntAdd_State", DbType.[String], Bride_PrsntAdd_State)
        '        db.AddInParameter(dbCommand, "@Bride_PrsntAdd_State_flag", DbType.[String], Bride_PrsntAdd_State_flag)
        '        db.AddInParameter(dbCommand, "@Bride_PrsntAdd_Dist", DbType.[String], Bride_PrsntAdd_Dist)
        '        db.AddInParameter(dbCommand, "@Bride_PrsntAdd_SubDiv", DbType.[String], Bride_PrsntAdd_SubDiv)
        '        db.AddInParameter(dbCommand, "@Bride_PrsntAdd_Block", DbType.[String], Bride_PrsntAdd_Block)
        '        db.AddInParameter(dbCommand, "@Bride_PrsntAdd_PS", DbType.[String], Bride_PrsntAdd_PS)
        '        db.AddInParameter(dbCommand, "@Bride_PrsntAdd_WA", DbType.[String], Bride_PrsntAdd_WA)
        '        db.AddInParameter(dbCommand, "@Bride_PrsntAdd_MuniGp", DbType.[String], Bride_PrsntAdd_MuniGp)
        '        db.AddInParameter(dbCommand, "@Bride_PrsntAdd_VilWrd", DbType.[String], Bride_PrsntAdd_VilWrd)
        '        db.AddInParameter(dbCommand, "@Bride_PrsntAdd_PO", DbType.[String], Bride_PrsntAdd_PO)
        '        db.AddInParameter(dbCommand, "@Bride_PrsntAdd_Pin", DbType.[String], Bride_PrsntAdd_Pin)

        '        db.AddInParameter(dbCommand, "@Bride_Permanent_Address", DbType.[String], Bride_Permanent_Address)
        '        db.AddInParameter(dbCommand, "@Bride_PrmntAdd_Country", DbType.[String], Bride_PrmntAdd_Country)
        '        db.AddInParameter(dbCommand, "@Bride_PrmntAdd_State", DbType.[String], Bride_PrmntAdd_State)
        '        db.AddInParameter(dbCommand, "@Bride_PrmntAdd_State_flag", DbType.[String], Bride_PrmntAdd_State_flag)
        '        db.AddInParameter(dbCommand, "@Bride_PrmntAdd_Dist", DbType.[String], Bride_PrmntAdd_Dist)
        '        db.AddInParameter(dbCommand, "@Bride_PrmntAdd_SubDiv", DbType.[String], Bride_PrmntAdd_SubDiv)
        '        db.AddInParameter(dbCommand, "@Bride_PrmntAdd_Block", DbType.[String], Bride_PrmntAdd_Block)
        '        db.AddInParameter(dbCommand, "@Bride_PrmntAdd_PS", DbType.[String], Bride_PrmntAdd_PS)
        '        db.AddInParameter(dbCommand, "@Bride_PrmntAdd_WA", DbType.[String], Bride_PrmntAdd_WA)
        '        db.AddInParameter(dbCommand, "@Bride_PrmntAdd_MuniGp", DbType.[String], Bride_PrmntAdd_MuniGp)
        '        db.AddInParameter(dbCommand, "@Bride_PrmntAdd_VilWrd", DbType.[String], Bride_PrmntAdd_VilWrd)
        '        db.AddInParameter(dbCommand, "@Bride_PrmntAdd_PO", DbType.[String], Bride_PrmntAdd_PO)
        '        db.AddInParameter(dbCommand, "@Bride_PrmntAdd_Pin", DbType.[String], Bride_PrmntAdd_Pin)

        '        db.AddInParameter(dbCommand, "@Bride_Mob_Num", DbType.[String], Bride_Mob_Num)
        '        db.AddInParameter(dbCommand, "@Bride_dob", DbType.DateTime, Bride_dob)
        '        db.AddInParameter(dbCommand, "@Bride_Email_Id", DbType.[String], Bride_Email_Id)
        '        db.AddInParameter(dbCommand, "@Bride_Religion", DbType.[String], Bride_Religion)
        '        db.AddInParameter(dbCommand, "@Bride_Aadhr_Num", DbType.[String], Bride_Aadhr_Num)
        '        db.AddInParameter(dbCommand, "@Bride_Occupation", DbType.[String], Bride_Occupation)
        '        db.AddInParameter(dbCommand, "@Bride_maritial_status", DbType.[String], Bride_maritial_status)
        '        db.AddInParameter(dbCommand, "@Bride_Prev_Mar_Dt", DbType.[String], Bride_Prev_Mar_Dt)
        '        db.AddInParameter(dbCommand, "@Bride_Photo", DbType.Binary, Bride_Photo)
        '        db.AddInParameter(dbCommand, "@Bride_Sign", DbType.Binary, Bride_Sign)

        '        db.AddInParameter(dbCommand, "@Bride_Proof_PrsntAdd_Type", DbType.[String], Bride_Proof_PrsntAdd_Type)
        '        db.AddInParameter(dbCommand, "@Bride_Proof_PrsntAdd", DbType.Binary, Bride_Proof_PrsntAdd)
        '        db.AddInParameter(dbCommand, "@Bride_Proof_PrmntAdd_Type", DbType.[String], Bride_Proof_PrmntAdd_Type)
        '        db.AddInParameter(dbCommand, "@Bride_Proof_PrmntAdd", DbType.Binary, Bride_Proof_PrmntAdd)
        '        db.AddInParameter(dbCommand, "@Bride_Proof_DOB_Type", DbType.[String], Bride_Proof_DOB_Type)
        '        db.AddInParameter(dbCommand, "@Bride_Proof_DOB", DbType.Binary, Bride_Proof_DOB)

        '        db.AddInParameter(dbCommand, "@MO_Autoincrement_cd", DbType.[String], MO_Autoincrement_cd)
        '        db.AddInParameter(dbCommand, "@Place_Reg_Address", DbType.[String], Place_Reg_Address)
        '        db.AddInParameter(dbCommand, "@Place_Reg_State", DbType.[String], Place_Reg_State)
        '        db.AddInParameter(dbCommand, "@Place_Reg_Dist", DbType.[String], Place_Reg_Dist)
        '        db.AddInParameter(dbCommand, "@Place_Reg_SubDiv", DbType.[String], Place_Reg_SubDiv)
        '        db.AddInParameter(dbCommand, "@Place_Reg_Block", DbType.[String], Place_Reg_Block)
        '        db.AddInParameter(dbCommand, "@Place_Reg_PS", DbType.[String], Place_Reg_PS)
        '        db.AddInParameter(dbCommand, "@Place_Reg_WA", DbType.[String], Place_Reg_WA)
        '        db.AddInParameter(dbCommand, "@Place_Reg_MuniGp", DbType.[String], Place_Reg_MuniGp)
        '        db.AddInParameter(dbCommand, "@Place_Reg_VilWrd", DbType.[String], Place_Reg_VilWrd)
        '        db.AddInParameter(dbCommand, "@Place_Reg_PO", DbType.[String], Place_Reg_PO)

        '        db.AddInParameter(dbCommand, "@Place_Socl_Mrg_Address", DbType.[String], Place_Socl_Mrg_Address)
        '        db.AddInParameter(dbCommand, "@Place_Socl_Mrg_Country", DbType.[String], Place_Socl_Mrg_Country)
        '        db.AddInParameter(dbCommand, "@Place_Socl_Mrg_State", DbType.[String], Place_Socl_Mrg_State)
        '        db.AddInParameter(dbCommand, "@Place_Socl_Mrg_State_flag", DbType.[String], Place_Socl_Mrg_State_flag)
        '        db.AddInParameter(dbCommand, "@Place_Socl_Mrg_Dist", DbType.[String], Place_Socl_Mrg_Dist)
        '        db.AddInParameter(dbCommand, "@Place_Socl_Mrg_SubDiv", DbType.[String], Place_Socl_Mrg_SubDiv)
        '        db.AddInParameter(dbCommand, "@Place_Socl_Mrg_Block", DbType.[String], Place_Socl_Mrg_Block)
        '        db.AddInParameter(dbCommand, "@Place_Socl_Mrg_PS", DbType.[String], Place_Socl_Mrg_PS)
        '        db.AddInParameter(dbCommand, "@Place_Socl_Mrg_WA", DbType.[String], Place_Socl_Mrg_WA)
        '        db.AddInParameter(dbCommand, "@Place_Socl_Mrg_MuniGp", DbType.[String], Place_Socl_Mrg_MuniGp)
        '        db.AddInParameter(dbCommand, "@Place_Socl_Mrg_VilWrd", DbType.[String], Place_Socl_Mrg_VilWrd)
        '        db.AddInParameter(dbCommand, "@Place_Socl_Mrg_PO", DbType.[String], Place_Socl_Mrg_PO)
        '        db.AddInParameter(dbCommand, "@Place_Socl_Mrg_Pin", DbType.[String], Place_Socl_Mrg_Pin)

        '        db.AddInParameter(dbCommand, "@Socl_Mrg_Dt", DbType.[String], Socl_Mrg_Dt)
        '        db.AddInParameter(dbCommand, "@Mrg_Invi_Card", DbType.Binary, Mrg_Invi_Card)
        '        db.AddInParameter(dbCommand, "@Payment_flag", DbType.[String], Payment_flag)
        '        'db.AddInParameter(dbCommand, "@Bank_cd", DbType.[String], Bank_cd)
        '        'db.AddInParameter(dbCommand, "@Branch_District_cd", DbType.[String], Branch_District_cd)
        '        'db.AddInParameter(dbCommand, "@Bank_Branch_cd", DbType.[String], Bank_Branch_cd)
        '        'db.AddInParameter(dbCommand, "@Challan_No", DbType.[String], Challan_No)
        '        'db.AddInParameter(dbCommand, "@Challan_Dt", DbType.DateTime, Challan_Dt)
        '        db.AddInParameter(dbCommand, "@Place_Reg", DbType.[String], Place_Reg)
        '        db.AddInParameter(dbCommand, "@Time_Reg", DbType.[String], Time_Reg)
        '        db.AddInParameter(dbCommand, "@Ip_Address", DbType.String, IP_Address)

        '        'db.AddInParameter(dbCommand, "@grm_bpl_holder_nm", DbType.[String], grm_bpl_holder_nm)
        '        'db.AddInParameter(dbCommand, "@bride_bpl_holder_nm", DbType.[String], bride_bpl_holder_nm)
        '        db.AddInParameter(dbCommand, "@grm_bpl_prf", DbType.Binary, grm_bpl_prf)
        '        db.AddInParameter(dbCommand, "@bride_bpl_prf", DbType.Binary, bride_bpl_prf)
        '        db.AddInParameter(dbCommand, "@gbpl_serial_cd", DbType.[String], gbpl_serial_cd)
        '        db.AddInParameter(dbCommand, "@bbpl_serial_cd", DbType.[String], bbpl_serial_cd)
        '        db.AddInParameter(dbCommand, "@grm_bpl_status", DbType.[String], grm_bpl_status)
        '        db.AddInParameter(dbCommand, "@bride_bpl_status", DbType.[String], bride_bpl_status)
        '        db.AddInParameter(dbCommand, "@applicant_bpl_status", DbType.[String], applicant_bpl_status)

        '        db.AddInParameter(dbCommand, "@Groom_Aadhr_Consent", DbType.[String], gadhrconsent)
        '        db.AddInParameter(dbCommand, "@Bride_Aadhr_Consent", DbType.[String], badhrconsent)

        '        db.AddInParameter(dbCommand, "@Groom_Nationality", DbType.[String], Groom_Nationality)
        '        db.AddInParameter(dbCommand, "@Bride_Nationality", DbType.[String], Bride_Nationality)

        '        db.AddOutParameter(dbCommand, "@Applicant_Total_Cd", DbType.[String], 50)
        '        db.AddOutParameter(dbCommand, "@Applicant_auto_Cd", DbType.[String], 10)
        '        'db.AddOutParameter(dbCommand, "@MO_Details", DbType.[String], 1000)
        '        db.ExecuteNonQuery(dbCommand)
        '        outmsg(0) = Convert.ToString(db.GetParameterValue(dbCommand, "@Applicant_Total_Cd"))
        '        outmsg(1) = Convert.ToString(db.GetParameterValue(dbCommand, "@Applicant_auto_Cd"))
        '        'outmsg(2) = Convert.ToString(db.GetParameterValue(dbCommand, "@MO_Details"))
        '        'outmsg(1) = "1"
        '    Catch ex As Exception
        '        'outmsg = ex.Message()
        '        outmsg(0) = "0"
        '    End Try
        '    Return outmsg
        'End Function
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
            Dim outmsg(3) As String
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_New_Applicant_insertBLDB")
                db.AddInParameter(dbCommand, "@Draft_No", DbType.[String], Draft_No)
                db.AddInParameter(dbCommand, "@Form_id", DbType.[String], Form_id)
                db.AddInParameter(dbCommand, "@Act_cd", DbType.[String], Act_cd)

                db.AddInParameter(dbCommand, "@Groom_FNam", DbType.[String], Groom_FNam)
                db.AddInParameter(dbCommand, "@Groom_MNam", DbType.[String], Groom_MNam)
                db.AddInParameter(dbCommand, "@Groom_LNam", DbType.[String], Groom_LNam)
                db.AddInParameter(dbCommand, "@Groom_Father_Nam", DbType.[String], Groom_Father_Nam)
                db.AddInParameter(dbCommand, "@Groom_Mother_Nam", DbType.[String], Groom_Mother_Nam)

                db.AddInParameter(dbCommand, "@Groom_Present_Address", DbType.[String], Groom_Present_Address)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_Country", DbType.[String], Groom_PrsntAdd_Country)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_State", DbType.[String], Groom_PrsntAdd_State)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_State_flag", DbType.[String], Groom_PrsntAdd_State_flag)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_Dist", DbType.[String], Groom_PrsntAdd_Dist)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_SubDiv", DbType.[String], Groom_PrsntAdd_SubDiv)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_Block", DbType.[String], Groom_PrsntAdd_Block)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_PS", DbType.[String], Groom_PrsntAdd_PS)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_WA", DbType.[String], Groom_PrsntAdd_WA)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_MuniGp", DbType.[String], Groom_PrsntAdd_MuniGp)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_VilWrd", DbType.[String], Groom_PrsntAdd_VilWrd)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_PO", DbType.[String], Groom_PrsntAdd_PO)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_Pin", DbType.[String], Groom_PrsntAdd_Pin)

                db.AddInParameter(dbCommand, "@Groom_Permanent_Address", DbType.[String], Groom_Permanent_Address)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_Country", DbType.[String], Groom_PrmntAdd_Country)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_State", DbType.[String], Groom_PrmntAdd_State)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_State_flag", DbType.[String], Groom_PrmntAdd_State_flag)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_Dist", DbType.[String], Groom_PrmntAdd_Dist)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_SubDiv", DbType.[String], Groom_PrmntAdd_SubDiv)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_Block", DbType.[String], Groom_PrmntAdd_Block)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_PS", DbType.[String], Groom_PrmntAdd_PS)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_WA", DbType.[String], Groom_PrmntAdd_WA)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_MuniGp", DbType.[String], Groom_PrmntAdd_MuniGp)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_VilWrd", DbType.[String], Groom_PrmntAdd_VilWrd)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_PO", DbType.[String], Groom_PrmntAdd_PO)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_Pin", DbType.[String], Groom_PrmntAdd_Pin)

                db.AddInParameter(dbCommand, "@Groom_Mob_Num", DbType.[String], Groom_Mob_Num)
                db.AddInParameter(dbCommand, "@Groom_dob", DbType.DateTime, Groom_dob)
                db.AddInParameter(dbCommand, "@Groom_Email_Id", DbType.[String], Groom_Email_Id)
                db.AddInParameter(dbCommand, "@Groom_Religion", DbType.[String], Groom_Religion)
                db.AddInParameter(dbCommand, "@Groom_Aadhr_Num", DbType.[String], Groom_Aadhr_Num)
                db.AddInParameter(dbCommand, "@Groom_occupation", DbType.[String], Groom_occupation)
                db.AddInParameter(dbCommand, "@Groom_maritial_status", DbType.[String], Groom_maritial_status)
                db.AddInParameter(dbCommand, "@Groom_Prev_Mar_Dt", DbType.[String], Groom_Prev_Mar_Dt)
                db.AddInParameter(dbCommand, "@Groom_Photo", DbType.Binary, Groom_Photo)
                db.AddInParameter(dbCommand, "@Groom_Sign", DbType.Binary, Groom_Sign)

                db.AddInParameter(dbCommand, "@Groom_Proof_PrsntAdd_Type", DbType.[String], Groom_Proof_PrsntAdd_Type)
                'db.AddInParameter(dbCommand, "@Groom_Proof_PrsntAdd", DbType.Binary, Groom_Proof_PrsntAdd)
                db.AddInParameter(dbCommand, "@Groom_Proof_PrmntAdd_Type", DbType.[String], Groom_Proof_PrmntAdd_Type)
                'db.AddInParameter(dbCommand, "@Groom_Proof_PrmntAdd", DbType.Binary, Groom_Proof_PrmntAdd)
                db.AddInParameter(dbCommand, "@Groom_Proof_DOB_Type", DbType.[String], Groom_Proof_DOB_Type)
                ' db.AddInParameter(dbCommand, "@Groom_Proof_DOB", DbType.Binary, Groom_Proof_DOB)

                db.AddInParameter(dbCommand, "@Bride_FNam", DbType.[String], Bride_FNam)
                db.AddInParameter(dbCommand, "@Bride_MNam", DbType.[String], Bride_MNam)
                db.AddInParameter(dbCommand, "@Bride_LNam", DbType.[String], Bride_LNam)
                db.AddInParameter(dbCommand, "@Bride_Father_Nam", DbType.[String], Bride_Father_Nam)
                db.AddInParameter(dbCommand, "@Bride_Mother_Nam", DbType.[String], Bride_Mother_Nam)

                db.AddInParameter(dbCommand, "@Bride_Present_Address", DbType.[String], Bride_Present_Address)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_Country", DbType.[String], Bride_PrsntAdd_Country)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_State", DbType.[String], Bride_PrsntAdd_State)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_State_flag", DbType.[String], Bride_PrsntAdd_State_flag)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_Dist", DbType.[String], Bride_PrsntAdd_Dist)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_SubDiv", DbType.[String], Bride_PrsntAdd_SubDiv)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_Block", DbType.[String], Bride_PrsntAdd_Block)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_PS", DbType.[String], Bride_PrsntAdd_PS)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_WA", DbType.[String], Bride_PrsntAdd_WA)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_MuniGp", DbType.[String], Bride_PrsntAdd_MuniGp)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_VilWrd", DbType.[String], Bride_PrsntAdd_VilWrd)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_PO", DbType.[String], Bride_PrsntAdd_PO)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_Pin", DbType.[String], Bride_PrsntAdd_Pin)

                db.AddInParameter(dbCommand, "@Bride_Permanent_Address", DbType.[String], Bride_Permanent_Address)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_Country", DbType.[String], Bride_PrmntAdd_Country)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_State", DbType.[String], Bride_PrmntAdd_State)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_State_flag", DbType.[String], Bride_PrmntAdd_State_flag)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_Dist", DbType.[String], Bride_PrmntAdd_Dist)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_SubDiv", DbType.[String], Bride_PrmntAdd_SubDiv)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_Block", DbType.[String], Bride_PrmntAdd_Block)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_PS", DbType.[String], Bride_PrmntAdd_PS)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_WA", DbType.[String], Bride_PrmntAdd_WA)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_MuniGp", DbType.[String], Bride_PrmntAdd_MuniGp)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_VilWrd", DbType.[String], Bride_PrmntAdd_VilWrd)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_PO", DbType.[String], Bride_PrmntAdd_PO)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_Pin", DbType.[String], Bride_PrmntAdd_Pin)

                db.AddInParameter(dbCommand, "@Bride_Mob_Num", DbType.[String], Bride_Mob_Num)
                db.AddInParameter(dbCommand, "@Bride_dob", DbType.DateTime, Bride_dob)
                db.AddInParameter(dbCommand, "@Bride_Email_Id", DbType.[String], Bride_Email_Id)
                db.AddInParameter(dbCommand, "@Bride_Religion", DbType.[String], Bride_Religion)
                db.AddInParameter(dbCommand, "@Bride_Aadhr_Num", DbType.[String], Bride_Aadhr_Num)
                db.AddInParameter(dbCommand, "@Bride_Occupation", DbType.[String], Bride_Occupation)
                db.AddInParameter(dbCommand, "@Bride_maritial_status", DbType.[String], Bride_maritial_status)
                db.AddInParameter(dbCommand, "@Bride_Prev_Mar_Dt", DbType.[String], Bride_Prev_Mar_Dt)
                db.AddInParameter(dbCommand, "@Bride_Photo", DbType.Binary, Bride_Photo)
                db.AddInParameter(dbCommand, "@Bride_Sign", DbType.Binary, Bride_Sign)

                db.AddInParameter(dbCommand, "@Bride_Proof_PrsntAdd_Type", DbType.[String], Bride_Proof_PrsntAdd_Type)
                'db.AddInParameter(dbCommand, "@Bride_Proof_PrsntAdd", DbType.Binary, Bride_Proof_PrsntAdd)
                db.AddInParameter(dbCommand, "@Bride_Proof_PrmntAdd_Type", DbType.[String], Bride_Proof_PrmntAdd_Type)
                'db.AddInParameter(dbCommand, "@Bride_Proof_PrmntAdd", DbType.Binary, Bride_Proof_PrmntAdd)
                db.AddInParameter(dbCommand, "@Bride_Proof_DOB_Type", DbType.[String], Bride_Proof_DOB_Type)
                'db.AddInParameter(dbCommand, "@Bride_Proof_DOB", DbType.Binary, Bride_Proof_DOB)

                db.AddInParameter(dbCommand, "@MO_Autoincrement_cd", DbType.[String], MO_Autoincrement_cd)
                db.AddInParameter(dbCommand, "@Place_Reg_Address", DbType.[String], Place_Reg_Address)
                db.AddInParameter(dbCommand, "@Place_Reg_State", DbType.[String], Place_Reg_State)
                db.AddInParameter(dbCommand, "@Place_Reg_Dist", DbType.[String], Place_Reg_Dist)
                db.AddInParameter(dbCommand, "@Place_Reg_SubDiv", DbType.[String], Place_Reg_SubDiv)
                db.AddInParameter(dbCommand, "@Place_Reg_Block", DbType.[String], Place_Reg_Block)
                db.AddInParameter(dbCommand, "@Place_Reg_PS", DbType.[String], Place_Reg_PS)
                db.AddInParameter(dbCommand, "@Place_Reg_WA", DbType.[String], Place_Reg_WA)
                db.AddInParameter(dbCommand, "@Place_Reg_MuniGp", DbType.[String], Place_Reg_MuniGp)
                db.AddInParameter(dbCommand, "@Place_Reg_VilWrd", DbType.[String], Place_Reg_VilWrd)
                db.AddInParameter(dbCommand, "@Place_Reg_PO", DbType.[String], Place_Reg_PO)

                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_Address", DbType.[String], Place_Socl_Mrg_Address)
                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_Country", DbType.[String], Place_Socl_Mrg_Country)
                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_State", DbType.[String], Place_Socl_Mrg_State)
                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_State_flag", DbType.[String], Place_Socl_Mrg_State_flag)
                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_Dist", DbType.[String], Place_Socl_Mrg_Dist)
                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_SubDiv", DbType.[String], Place_Socl_Mrg_SubDiv)
                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_Block", DbType.[String], Place_Socl_Mrg_Block)
                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_PS", DbType.[String], Place_Socl_Mrg_PS)
                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_WA", DbType.[String], Place_Socl_Mrg_WA)
                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_MuniGp", DbType.[String], Place_Socl_Mrg_MuniGp)
                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_VilWrd", DbType.[String], Place_Socl_Mrg_VilWrd)
                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_PO", DbType.[String], Place_Socl_Mrg_PO)
                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_Pin", DbType.[String], Place_Socl_Mrg_Pin)

                db.AddInParameter(dbCommand, "@Socl_Mrg_Dt", DbType.[String], Socl_Mrg_Dt)
                db.AddInParameter(dbCommand, "@Mrg_Invi_Card", DbType.Binary, Mrg_Invi_Card)
                db.AddInParameter(dbCommand, "@Payment_flag", DbType.[String], Payment_flag)
                'db.AddInParameter(dbCommand, "@Bank_cd", DbType.[String], Bank_cd)
                'db.AddInParameter(dbCommand, "@Branch_District_cd", DbType.[String], Branch_District_cd)
                'db.AddInParameter(dbCommand, "@Bank_Branch_cd", DbType.[String], Bank_Branch_cd)
                'db.AddInParameter(dbCommand, "@Challan_No", DbType.[String], Challan_No)
                'db.AddInParameter(dbCommand, "@Challan_Dt", DbType.DateTime, Challan_Dt)
                db.AddInParameter(dbCommand, "@Place_Reg", DbType.[String], Place_Reg)
                db.AddInParameter(dbCommand, "@Time_Reg", DbType.[String], Time_Reg)
                db.AddInParameter(dbCommand, "@Ip_Address", DbType.String, IP_Address)

                'db.AddInParameter(dbCommand, "@grm_bpl_holder_nm", DbType.[String], grm_bpl_holder_nm)
                'db.AddInParameter(dbCommand, "@bride_bpl_holder_nm", DbType.[String], bride_bpl_holder_nm)
                'db.AddInParameter(dbCommand, "@grm_bpl_prf", DbType.Binary, grm_bpl_prf)
                'db.AddInParameter(dbCommand, "@bride_bpl_prf", DbType.Binary, bride_bpl_prf)
                db.AddInParameter(dbCommand, "@gbpl_serial_cd", DbType.[String], gbpl_serial_cd)
                db.AddInParameter(dbCommand, "@bbpl_serial_cd", DbType.[String], bbpl_serial_cd)
                db.AddInParameter(dbCommand, "@grm_bpl_status", DbType.[String], grm_bpl_status)
                db.AddInParameter(dbCommand, "@bride_bpl_status", DbType.[String], bride_bpl_status)
                db.AddInParameter(dbCommand, "@applicant_bpl_status", DbType.[String], applicant_bpl_status)

                db.AddInParameter(dbCommand, "@Groom_Aadhr_Consent", DbType.[String], gadhrconsent)
                db.AddInParameter(dbCommand, "@Bride_Aadhr_Consent", DbType.[String], badhrconsent)

                db.AddInParameter(dbCommand, "@Groom_Nationality", DbType.[String], Groom_Nationality)
                db.AddInParameter(dbCommand, "@Bride_Nationality", DbType.[String], Bride_Nationality)

                db.AddOutParameter(dbCommand, "@Applicant_Total_Cd", DbType.[String], 50)
                db.AddOutParameter(dbCommand, "@Applicant_auto_Cd", DbType.[String], 10)
                db.AddOutParameter(dbCommand, "@file_location", DbType.[String], 100)
                'db.AddOutParameter(dbCommand, "@MO_Details", DbType.[String], 1000)
                db.ExecuteNonQuery(dbCommand)
                outmsg(0) = Convert.ToString(db.GetParameterValue(dbCommand, "@Applicant_Total_Cd"))
                outmsg(1) = Convert.ToString(db.GetParameterValue(dbCommand, "@Applicant_auto_Cd"))
                outmsg(2) = Convert.ToString(db.GetParameterValue(dbCommand, "@file_location"))
                'outmsg(2) = Convert.ToString(db.GetParameterValue(dbCommand, "@MO_Details"))
                'outmsg(1) = "1"
            Catch ex As Exception
                'outmsg = ex.Message()
                outmsg(0) = "0"
            End Try
            Return outmsg
        End Function


#End Region

#Region "Groom & Bride Child Details"
        Public Function NewApp_BG_Child_Save(App_Auto_Cd As String, Groom As DataTable, Bride As DataTable) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim con As New SqlConnection(dbconnctn.GetConnectionString())
                Dim insertCommand As SqlCommand = New SqlCommand("MARREG_NewApp_BG_Child_Insert", con)
                insertCommand.CommandType = CommandType.StoredProcedure

                Dim app_auto As SqlParameter = insertCommand.Parameters.AddWithValue("@App_Auto_Cd", App_Auto_Cd)
                app_auto.SqlDbType = SqlDbType.NVarChar

                Dim Groom_tab As SqlParameter = insertCommand.Parameters.AddWithValue("@Groom", Groom)
                Groom_tab.SqlDbType = SqlDbType.Structured

                Dim Bride_tab As SqlParameter = insertCommand.Parameters.AddWithValue("@Bride", Bride)
                Bride_tab.SqlDbType = SqlDbType.Structured

                Dim Msg As SqlParameter = insertCommand.Parameters.AddWithValue("@Msg", 50)
                Msg.SqlDbType = SqlDbType.NVarChar
                Msg.Size = 50
                Msg.Direction = ParameterDirection.Output
                con.Open()
                insertCommand.ExecuteNonQuery()
                outmsg = insertCommand.Parameters("@Msg").Value.ToString()
                con.Close()
            Catch ex As Exception
                outmsg = ex.Message
            End Try
            Return outmsg
        End Function

        Public Function NewApp_BG_Child_Save(App_Auto_Cd As String, Groom As DataTable, Bride As DataTable, Tag As String, Choose_Child_By As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim con As New SqlConnection(dbconnctn.GetConnectionString())
                Dim insertCommand As SqlCommand = New SqlCommand("MARREG_NewApp_Child_Insert", con)
                insertCommand.CommandType = CommandType.StoredProcedure

                Dim app_auto As SqlParameter = insertCommand.Parameters.AddWithValue("@App_Auto_Cd", App_Auto_Cd)
                app_auto.SqlDbType = SqlDbType.NVarChar

                Dim Tagg As SqlParameter = insertCommand.Parameters.AddWithValue("@Tag", Tag)
                Tagg.SqlDbType = SqlDbType.NVarChar

                Dim Choose_Child As SqlParameter = insertCommand.Parameters.AddWithValue("@Choose_Child_By", Choose_Child_By)
                Tagg.SqlDbType = SqlDbType.NVarChar

                Dim Groom_tab As SqlParameter = insertCommand.Parameters.AddWithValue("@Groom", Groom)
                Groom_tab.SqlDbType = SqlDbType.Structured

                Dim Bride_tab As SqlParameter = insertCommand.Parameters.AddWithValue("@child", Bride)
                Bride_tab.SqlDbType = SqlDbType.Structured

                Dim Msg As SqlParameter = insertCommand.Parameters.AddWithValue("@Msg", 50)
                Msg.SqlDbType = SqlDbType.NVarChar
                Msg.Size = 50
                Msg.Direction = ParameterDirection.Output

                Dim file As SqlParameter = insertCommand.Parameters.AddWithValue("@filePath", 50)
                file.SqlDbType = SqlDbType.NVarChar
                file.Size = 100
                file.Direction = ParameterDirection.Output

                con.Open()
                insertCommand.ExecuteNonQuery()
                outmsg = insertCommand.Parameters("@Msg").Value.ToString()
                con.Close()
            Catch ex As Exception
                outmsg = ex.Message
            End Try
            Return outmsg
        End Function
#End Region

#Region "Draft Insert"

        Public Function ChristianMarriageTest(Draft_cd As String, Form_cd As String, Act_cd As String, g_name As String, b_name As String, g_f_name As String, b_f_name As String, g_r_cd As String, b_r_cd As String,
                                          g_c_cd As String, b_c_cd As String, g_sc_name As String, b_sc_name As String) As Array
            Dim outmsg(3) As String
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_New_Applicant_Draft_Christian")
                db.AddInParameter(dbCommand, "@Pre_Draft_Cd", DbType.[String], Draft_cd)
                db.AddInParameter(dbCommand, "@Form_cd", DbType.[String], Form_cd)
                db.AddInParameter(dbCommand, "@Act_cd", DbType.[String], Act_cd)
                db.AddInParameter(dbCommand, "@g_name", DbType.[String], g_name)
                db.AddInParameter(dbCommand, "@b_name", DbType.[String], b_name)
                db.AddInParameter(dbCommand, "@g_f_name", DbType.[String], g_f_name)
                db.AddInParameter(dbCommand, "@b_f_name", DbType.[String], b_f_name)
                db.AddInParameter(dbCommand, "@g_r_cd", DbType.[String], g_r_cd)
                db.AddInParameter(dbCommand, "@b_r_cd", DbType.[String], b_r_cd)
                db.AddInParameter(dbCommand, "@g_c_cd", DbType.[String], g_c_cd)
                db.AddInParameter(dbCommand, "@b_c_cd", DbType.[String], b_c_cd)
                db.AddInParameter(dbCommand, "@g_sc_name", DbType.[String], g_sc_name)
                db.AddInParameter(dbCommand, "@b_sc_name", DbType.[String], b_sc_name)
                ''''''''''''''''''''''''''''''''''''

                ''''''''''''''''''''''''''''''''''''
                db.AddOutParameter(dbCommand, "@Applicant_auto_Cd", DbType.[String], 10)

                db.ExecuteNonQuery(dbCommand)

                outmsg(1) = Convert.ToString(db.GetParameterValue(dbCommand, "@Applicant_auto_Cd"))

            Catch ex As Exception
                'outmsg = ex.Message()
                outmsg(0) = "0"
            End Try
            Return outmsg

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
                                   groom_nullity_proof_no As String, bride_nullity_proof_no As String, g_nullity_case_no As String, g_nullity_court_name As String, g_nullity_dateOfOrder As DateTime, g_nullity_issuedBy As String, g_nullity_court_Country As String, g_nullity_court_state As String, g_nullity_court_district As String, g_is_doc_appostile As String, g_null_pre_app_no As String,
                                   b_nullity_case_no As String, b_nullity_court_name As String, b_nullity_dateOfOrder As DateTime, b_nullity_issuedBy As String, b_nullity_court_Country As String, b_nullity_court_state As String, b_nullity_court_district As String, b_is_doc_appostile As String, b_null_pre_app_no As String, g_church_name As String, b_church_name As String) As Array
            Dim outmsg(3) As String
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_New_Applicant_Draft_insertBLDB")
                db.AddInParameter(dbCommand, "@Pre_Draft_Cd", DbType.[String], Draft_Cd)
                db.AddInParameter(dbCommand, "@Form_id", DbType.[String], Form_id)
                db.AddInParameter(dbCommand, "@Act_cd", DbType.[String], Act_cd)

                db.AddInParameter(dbCommand, "@Groom_FNam", DbType.[String], Groom_FNam)
                db.AddInParameter(dbCommand, "@Groom_MNam", DbType.[String], Groom_MNam)
                db.AddInParameter(dbCommand, "@Groom_LNam", DbType.[String], Groom_LNam)
                db.AddInParameter(dbCommand, "@Groom_Father_Nam", DbType.[String], Groom_Father_Nam)
                db.AddInParameter(dbCommand, "@Groom_Mother_Nam", DbType.[String], Groom_Mother_Nam)

                db.AddInParameter(dbCommand, "@Groom_Present_HNo", DbType.[String], Groom_Present_HNo)
                db.AddInParameter(dbCommand, "@Groom_Present_Address", DbType.[String], Groom_Present_Address)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_Country", DbType.[String], Groom_PrsntAdd_Country)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_State", DbType.[String], Groom_PrsntAdd_State)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_State_flag", DbType.[String], Groom_PrsntAdd_State_flag)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_Dist", DbType.[String], Groom_PrsntAdd_Dist)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_SubDiv", DbType.[String], Groom_PrsntAdd_SubDiv)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_Block", DbType.[String], Groom_PrsntAdd_Block)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_PS", DbType.[String], Groom_PrsntAdd_PS)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_WA", DbType.[String], Groom_PrsntAdd_WA)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_MuniGp", DbType.[String], Groom_PrsntAdd_MuniGp)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_VilWrd", DbType.[String], Groom_PrsntAdd_VilWrd)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_PO", DbType.[String], Groom_PrsntAdd_PO)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_Pin", DbType.[String], Groom_PrsntAdd_Pin)

                db.AddInParameter(dbCommand, "@Groom_Permanent_HNo", DbType.[String], Groom_Permanent_HNo)
                db.AddInParameter(dbCommand, "@Groom_Permanent_Address", DbType.[String], Groom_Permanent_Address)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_Country", DbType.[String], Groom_PrmntAdd_Country)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_State", DbType.[String], Groom_PrmntAdd_State)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_State_flag", DbType.[String], Groom_PrmntAdd_State_flag)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_Dist", DbType.[String], Groom_PrmntAdd_Dist)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_SubDiv", DbType.[String], Groom_PrmntAdd_SubDiv)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_Block", DbType.[String], Groom_PrmntAdd_Block)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_PS", DbType.[String], Groom_PrmntAdd_PS)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_WA", DbType.[String], Groom_PrmntAdd_WA)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_MuniGp", DbType.[String], Groom_PrmntAdd_MuniGp)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_VilWrd", DbType.[String], Groom_PrmntAdd_VilWrd)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_PO", DbType.[String], Groom_PrmntAdd_PO)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_Pin", DbType.[String], Groom_PrmntAdd_Pin)

                db.AddInParameter(dbCommand, "@Groom_Mob_Num", DbType.[String], Groom_Mob_Num)
                db.AddInParameter(dbCommand, "@Groom_dob", DbType.DateTime, Groom_dob)
                db.AddInParameter(dbCommand, "@Groom_Email_Id", DbType.[String], Groom_Email_Id)
                db.AddInParameter(dbCommand, "@Groom_Religion", DbType.[String], Groom_Religion)
                db.AddInParameter(dbCommand, "@Groom_Aadhr_Num", DbType.[String], Groom_Aadhr_Num)
                db.AddInParameter(dbCommand, "@Groom_occupation", DbType.[String], Groom_occupation)
                db.AddInParameter(dbCommand, "@Groom_maritial_status", DbType.[String], Groom_maritial_status)
                db.AddInParameter(dbCommand, "@Groom_Prev_Mar_Dt", DbType.[String], Groom_Prev_Mar_Dt)
                db.AddInParameter(dbCommand, "@Groom_Photo", DbType.Binary, Groom_Photo)
                db.AddInParameter(dbCommand, "@Groom_Sign", DbType.Binary, Groom_Sign)

                db.AddInParameter(dbCommand, "@Groom_Proof_PrsntAdd_Type", DbType.[String], Groom_Proof_PrsntAdd_Type)
                ' db.AddInParameter(dbCommand, "@Groom_Proof_PrsntAdd", DbType.Binary, Groom_Proof_PrsntAdd) '26.03.2018
                db.AddInParameter(dbCommand, "@Groom_Proof_PrmntAdd_Type", DbType.[String], Groom_Proof_PrmntAdd_Type)
                'db.AddInParameter(dbCommand, "@Groom_Proof_PrmntAdd", DbType.Binary, Groom_Proof_PrmntAdd) '26.03.2018
                db.AddInParameter(dbCommand, "@Groom_Proof_DOB_Type", DbType.[String], Groom_Proof_DOB_Type)
                'db.AddInParameter(dbCommand, "@Groom_Proof_DOB", DbType.Binary, Groom_Proof_DOB) '26.03.2018

                db.AddInParameter(dbCommand, "@Bride_FNam", DbType.[String], Bride_FNam)
                db.AddInParameter(dbCommand, "@Bride_MNam", DbType.[String], Bride_MNam)
                db.AddInParameter(dbCommand, "@Bride_LNam", DbType.[String], Bride_LNam)
                db.AddInParameter(dbCommand, "@Bride_Father_Nam", DbType.[String], Bride_Father_Nam)
                db.AddInParameter(dbCommand, "@Bride_Mother_Nam", DbType.[String], Bride_Mother_Nam)

                db.AddInParameter(dbCommand, "@Bride_Present_HNo", DbType.[String], Bride_Present_HNo)
                db.AddInParameter(dbCommand, "@Bride_Present_Address", DbType.[String], Bride_Present_Address)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_Country", DbType.[String], Bride_PrsntAdd_Country)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_State", DbType.[String], Bride_PrsntAdd_State)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_State_flag", DbType.[String], Bride_PrsntAdd_State_flag)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_Dist", DbType.[String], Bride_PrsntAdd_Dist)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_SubDiv", DbType.[String], Bride_PrsntAdd_SubDiv)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_Block", DbType.[String], Bride_PrsntAdd_Block)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_PS", DbType.[String], Bride_PrsntAdd_PS)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_WA", DbType.[String], Bride_PrsntAdd_WA)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_MuniGp", DbType.[String], Bride_PrsntAdd_MuniGp)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_VilWrd", DbType.[String], Bride_PrsntAdd_VilWrd)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_PO", DbType.[String], Bride_PrsntAdd_PO)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_Pin", DbType.[String], Bride_PrsntAdd_Pin)

                db.AddInParameter(dbCommand, "@Bride_Permanent_HNo", DbType.[String], Bride_Permanent_HNo)
                db.AddInParameter(dbCommand, "@Bride_Permanent_Address", DbType.[String], Bride_Permanent_Address)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_Country", DbType.[String], Bride_PrmntAdd_Country)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_State", DbType.[String], Bride_PrmntAdd_State)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_State_flag", DbType.[String], Bride_PrmntAdd_State_flag)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_Dist", DbType.[String], Bride_PrmntAdd_Dist)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_SubDiv", DbType.[String], Bride_PrmntAdd_SubDiv)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_Block", DbType.[String], Bride_PrmntAdd_Block)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_PS", DbType.[String], Bride_PrmntAdd_PS)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_WA", DbType.[String], Bride_PrmntAdd_WA)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_MuniGp", DbType.[String], Bride_PrmntAdd_MuniGp)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_VilWrd", DbType.[String], Bride_PrmntAdd_VilWrd)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_PO", DbType.[String], Bride_PrmntAdd_PO)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_Pin", DbType.[String], Bride_PrmntAdd_Pin)

                db.AddInParameter(dbCommand, "@Bride_Mob_Num", DbType.[String], Bride_Mob_Num)
                db.AddInParameter(dbCommand, "@Bride_dob", DbType.DateTime, Bride_dob)
                db.AddInParameter(dbCommand, "@Bride_Email_Id", DbType.[String], Bride_Email_Id)
                db.AddInParameter(dbCommand, "@Bride_Religion", DbType.[String], Bride_Religion)
                db.AddInParameter(dbCommand, "@Bride_Aadhr_Num", DbType.[String], Bride_Aadhr_Num)
                db.AddInParameter(dbCommand, "@Bride_Occupation", DbType.[String], Bride_Occupation)
                db.AddInParameter(dbCommand, "@Bride_maritial_status", DbType.[String], Bride_maritial_status)
                db.AddInParameter(dbCommand, "@Bride_Prev_Mar_Dt", DbType.[String], Bride_Prev_Mar_Dt)
                db.AddInParameter(dbCommand, "@Bride_Photo", DbType.Binary, Bride_Photo)
                db.AddInParameter(dbCommand, "@Bride_Sign", DbType.Binary, Bride_Sign)

                db.AddInParameter(dbCommand, "@Bride_Proof_PrsntAdd_Type", DbType.[String], Bride_Proof_PrsntAdd_Type)
                ' db.AddInParameter(dbCommand, "@Bride_Proof_PrsntAdd", DbType.Binary, Bride_Proof_PrsntAdd) '26.03.2018
                db.AddInParameter(dbCommand, "@Bride_Proof_PrmntAdd_Type", DbType.[String], Bride_Proof_PrmntAdd_Type)
                ' db.AddInParameter(dbCommand, "@Bride_Proof_PrmntAdd", DbType.Binary, Bride_Proof_PrmntAdd) '26.03.2018
                db.AddInParameter(dbCommand, "@Bride_Proof_DOB_Type", DbType.[String], Bride_Proof_DOB_Type)
                ' db.AddInParameter(dbCommand, "@Bride_Proof_DOB", DbType.Binary, Bride_Proof_DOB) '26.03.2018

                db.AddInParameter(dbCommand, "@MO_Autoincrement_cd", DbType.[String], MO_Autoincrement_cd)
                db.AddInParameter(dbCommand, "@Place_Reg_Address", DbType.[String], Place_Reg_Address)
                db.AddInParameter(dbCommand, "@Place_Reg_State", DbType.[String], Place_Reg_State)
                db.AddInParameter(dbCommand, "@Place_Reg_Dist", DbType.[String], Place_Reg_Dist)
                db.AddInParameter(dbCommand, "@Place_Reg_SubDiv", DbType.[String], Place_Reg_SubDiv)
                db.AddInParameter(dbCommand, "@Place_Reg_Block", DbType.[String], Place_Reg_Block)
                db.AddInParameter(dbCommand, "@Place_Reg_PS", DbType.[String], Place_Reg_PS)
                db.AddInParameter(dbCommand, "@Place_Reg_WA", DbType.[String], Place_Reg_WA)
                db.AddInParameter(dbCommand, "@Place_Reg_MuniGp", DbType.[String], Place_Reg_MuniGp)
                db.AddInParameter(dbCommand, "@Place_Reg_VilWrd", DbType.[String], Place_Reg_VilWrd)
                db.AddInParameter(dbCommand, "@Place_Reg_PO", DbType.[String], Place_Reg_PO)

                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_HNo", DbType.[String], Place_Socl_Mrg_HNo)
                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_Address", DbType.[String], Place_Socl_Mrg_Address)
                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_Country", DbType.[String], Place_Socl_Mrg_Country)
                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_State", DbType.[String], Place_Socl_Mrg_State)
                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_State_flag", DbType.[String], Place_Socl_Mrg_State_flag)
                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_Dist", DbType.[String], Place_Socl_Mrg_Dist)
                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_SubDiv", DbType.[String], Place_Socl_Mrg_SubDiv)
                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_Block", DbType.[String], Place_Socl_Mrg_Block)
                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_PS", DbType.[String], Place_Socl_Mrg_PS)
                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_WA", DbType.[String], Place_Socl_Mrg_WA)
                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_MuniGp", DbType.[String], Place_Socl_Mrg_MuniGp)
                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_VilWrd", DbType.[String], Place_Socl_Mrg_VilWrd)
                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_PO", DbType.[String], Place_Socl_Mrg_PO)
                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_Pin", DbType.[String], Place_Socl_Mrg_Pin)

                db.AddInParameter(dbCommand, "@Socl_Mrg_Dt", DbType.[String], Socl_Mrg_Dt)
                db.AddInParameter(dbCommand, "@Mrg_Invi_Card", DbType.Binary, Mrg_Invi_Card)

                db.AddInParameter(dbCommand, "@Place_Reg", DbType.[String], Place_Reg)
                db.AddInParameter(dbCommand, "@Time_Reg", DbType.[String], Time_Reg)
                db.AddInParameter(dbCommand, "@Ip_Address", DbType.String, IP_Address)

                db.AddInParameter(dbCommand, "@gbpl_serial_cd", DbType.[String], gbpl_serial_cd)
                db.AddInParameter(dbCommand, "@bbpl_serial_cd", DbType.[String], bbpl_serial_cd)
                db.AddInParameter(dbCommand, "@grm_bpl_status", DbType.[String], grm_bpl_status)
                db.AddInParameter(dbCommand, "@bride_bpl_status", DbType.[String], bride_bpl_status)
                db.AddInParameter(dbCommand, "@applicant_bpl_status", DbType.[String], applicant_bpl_status)

                db.AddInParameter(dbCommand, "@Groom_Aadhr_Consent", DbType.[String], gadhrconsent)
                db.AddInParameter(dbCommand, "@Bride_Aadhr_Consent", DbType.[String], badhrconsent)

                db.AddInParameter(dbCommand, "@Groom_Nationality", DbType.[String], Groom_Nationality)
                db.AddInParameter(dbCommand, "@Bride_Nationality", DbType.[String], Bride_Nationality)

                db.AddInParameter(dbCommand, "@MO_Verified", DbType.[String], MO_Verified) '26.04.2018 

                db.AddInParameter(dbCommand, "@Groom_Religion_Other_Name", DbType.[String], Groom_Religion_Other_Name)
                db.AddInParameter(dbCommand, "@Groom_Occupation_Other_Name", DbType.[String], Groom_Occupation_Other_Name)
                db.AddInParameter(dbCommand, "@Bride_Religion_Other_Name", DbType.[String], Bride_Religion_Other_Name)
                db.AddInParameter(dbCommand, "@Bride_Occupation_Other_Name", DbType.[String], Bride_Occupation_Other_Name)

                db.AddInParameter(dbCommand, "@Groom_IdProofNo_PresentAddress", DbType.[String], Groom_IdProofNo_PresentAddress)
                db.AddInParameter(dbCommand, "@Groom_IdProofNo_Dob", DbType.[String], Groom_IdProofNo_Dob)
                db.AddInParameter(dbCommand, "@Groom_IdProofNo_PermanentAddress", DbType.[String], Groom_IdProofNo_PermanentAddress)
                db.AddInParameter(dbCommand, "@Bride_IdProofNo_PresentAddress", DbType.[String], Bride_IdProofNo_PresentAddress)
                db.AddInParameter(dbCommand, "@Bride_IdProofNo_Dob", DbType.[String], Bride_IdProofNo_Dob)
                db.AddInParameter(dbCommand, "@Bride_IdProofNo_PermanentAddress", DbType.[String], Bride_IdProofNo_PermanentAddress)
                db.AddInParameter(dbCommand, "@Groom_Guardian_type", DbType.[String], Groom_Guardian_type)
                db.AddInParameter(dbCommand, "@Bride_Guardian_type", DbType.[String], Bride_Guardian_type)
                'anay 23.08.2018---------------------
                db.AddInParameter(dbCommand, "@Groom_Length_Residence", DbType.[String], Groom_length_residence)
                db.AddInParameter(dbCommand, "@Bride_Length_Residence", DbType.[String], Bride_length_residence)
                '------------------------------------

                '05-09-2018-----------------------
                db.AddInParameter(dbCommand, "@Groom_CaseNo", DbType.[String], g_caseno)
                db.AddInParameter(dbCommand, "@Groom_Court_Name", DbType.[String], g_courtname)
                db.AddInParameter(dbCommand, "@Groom_DateOf_Order", DbType.String, g_dateoforder)
                db.AddInParameter(dbCommand, "@Groom_Petitioners_Name", DbType.[String], g_petitionername)
                db.AddInParameter(dbCommand, "@Groom_Respondents_Name", DbType.[String], g_respondentname)

                db.AddInParameter(dbCommand, "@Bride_CaseNo", DbType.[String], b_caseno)
                db.AddInParameter(dbCommand, "@Bride_Court_Name", DbType.[String], b_courtname)
                db.AddInParameter(dbCommand, "@Bride_DateOf_Order", DbType.String, b_dateoforder)
                db.AddInParameter(dbCommand, "@Bride_Petitioners_Name", DbType.[String], b_petitionername)
                db.AddInParameter(dbCommand, "@Bride_Respondents_Name", DbType.[String], b_respondentname)

                'db.AddInParameter(dbCommand, "@Groom_Prf_Divorce_Doc", DbType.[String], gdivorce_prf)
                db.AddInParameter(dbCommand, "@Groom_Prf_Divorce_Doc_No", DbType.[String], g_txtDivorceCertificateNo)
                'db.AddInParameter(dbCommand, "@Bride_Prf_Divorce_Doc", DbType.[String], bdivorce_prf)
                db.AddInParameter(dbCommand, "@Bride_Prf_Divorce_Doc_No", DbType.[String], b_txtDivorceCertificateNo)

                'db.AddInParameter(dbCommand, "@Groom_Prf_Wife_Death_Certificate_Doc", DbType.[String], wife_detCrt_prf)
                db.AddInParameter(dbCommand, "@Groom_Prf_Wife_Death_Certificate_No", DbType.[String], WifeDeathCertificateNo)

                'db.AddInParameter(dbCommand, "@Bride_Prf_Husband_Death_Certificate", DbType.[String], Husband_detCrt_prf)
                db.AddInParameter(dbCommand, "@Bride_Prf_Husband_Death_Certificate_No", DbType.[String], HusbandDeathCertificateNo)
                db.AddInParameter(dbCommand, "@Bride_Prf_With_HusbandName_type", DbType.[String], ddlDeathHusbGovId)
                'db.AddInParameter(dbCommand, "@Bride_Prf_With_HusbandName_Doc", DbType.[String], HusbndDtCrt_wh_prf)
                db.AddInParameter(dbCommand, "@Bride_Pfr_With_HusbandName_IdNumber", DbType.[String], txtWifeGovIdWithHusName)

                db.AddInParameter(dbCommand, "@groom_legalguardian_proof_id_no", DbType.[String], groom_legalguardian_proof_id)
                db.AddInParameter(dbCommand, "@groom_legalguardian_proof_type", DbType.[String], groom_legalguardian_proof_type)
                db.AddInParameter(dbCommand, "@bride_legalguardian_proof_id_no", DbType.[String], bride_legalguardian_proof_id)
                db.AddInParameter(dbCommand, "@bride_legalguardian_proof_type", DbType.[String], bride_legalguardian_proof_type)

                db.AddInParameter(dbCommand, "@Groom_Countrycode", DbType.[String], ddlgCountryCode)
                db.AddInParameter(dbCommand, "@Bride_Countrycode", DbType.[String], ddlbCountryCode)
                '-------------------------
                db.AddInParameter(dbCommand, "@IsBrideMaidenName", DbType.[String], IsBrideMaidenName)
                db.AddInParameter(dbCommand, "@BrideFatherLastName", DbType.[String], BrideFatherLastName)
                '--------------START------------------
                db.AddInParameter(dbCommand, "@IsGroomSameAddr", DbType.[String], IsGroomSameAddr)
                db.AddInParameter(dbCommand, "@IsBrideSameAddr", DbType.[String], IsBrideSameAddr)
                db.AddInParameter(dbCommand, "@mo_select", DbType.[String], mo_select)
                '--------------START------------------
                db.AddInParameter(dbCommand, "@propose_reg_dt", DbType.DateTime, propose_reg_dt)

                'Shaibal 
                db.AddInParameter(dbCommand, "@groom_IDproofNo_nullity", DbType.[String], groom_nullity_proof_no)

                db.AddInParameter(dbCommand, "@Groom_nullity_case_no", DbType.[String], g_nullity_case_no)
                db.AddInParameter(dbCommand, "@Groom_nullity_court_name", DbType.[String], g_nullity_court_name)
                db.AddInParameter(dbCommand, "@Groom_nullity_dateOfOrder", DbType.DateTime, g_nullity_dateOfOrder)
                db.AddInParameter(dbCommand, "@Groom_nullity_issuedBy", DbType.[String], g_nullity_issuedBy)
                db.AddInParameter(dbCommand, "@Groom_nullity_court_Country", DbType.[String], g_nullity_court_Country)
                db.AddInParameter(dbCommand, "@Groom_nullity_court_state", DbType.[String], g_nullity_court_state)
                db.AddInParameter(dbCommand, "@Groom_nullity_court_district", DbType.[String], g_nullity_court_district)
                db.AddInParameter(dbCommand, "@Groom_nullity_doc_appostile", DbType.[String], g_is_doc_appostile)
                db.AddInParameter(dbCommand, "@Groom_nullity_prev_app_no", DbType.[String], g_null_pre_app_no)

                db.AddInParameter(dbCommand, "@bride_IDproofNo_nullity", DbType.[String], bride_nullity_proof_no)

                db.AddInParameter(dbCommand, "@Bride_nullity_case_no", DbType.[String], b_nullity_case_no)
                db.AddInParameter(dbCommand, "@Bride_nullity_court_name", DbType.[String], b_nullity_court_name)
                db.AddInParameter(dbCommand, "@Bride_nullity_dateOfOrder", DbType.DateTime, b_nullity_dateOfOrder)
                db.AddInParameter(dbCommand, "@Bride_nullity_issuedBy", DbType.[String], b_nullity_issuedBy)
                db.AddInParameter(dbCommand, "@Bride_nullity_court_Country", DbType.[String], b_nullity_court_Country)
                db.AddInParameter(dbCommand, "@Bride_nullity_court_state", DbType.[String], b_nullity_court_state)
                db.AddInParameter(dbCommand, "@Bride_nullity_court_district", DbType.[String], b_nullity_court_district)
                db.AddInParameter(dbCommand, "@Bride_nullity_doc_appostile", DbType.[String], b_is_doc_appostile)
                db.AddInParameter(dbCommand, "@Bride_nullity_prev_app_no", DbType.[String], b_null_pre_app_no)
                db.AddInParameter(dbCommand, "@Groom_church_name", DbType.[String], g_church_name)  '25/09/2024 Sankha
                db.AddInParameter(dbCommand, "@Bride_church_name", DbType.[String], b_church_name)      '25/09/2024 Sankha

                '------------------END---------------------------


                db.AddOutParameter(dbCommand, "@Applicant_Total_Cd", DbType.[String], 50)
                db.AddOutParameter(dbCommand, "@Applicant_auto_Cd", DbType.[String], 10)
                db.AddOutParameter(dbCommand, "@file_location", DbType.[String], 100)

                'db.AddOutParameter(dbCommand, "@MO_Details", DbType.[String], 1000)
                db.ExecuteNonQuery(dbCommand)
                outmsg(0) = Convert.ToString(db.GetParameterValue(dbCommand, "@Applicant_Total_Cd"))
                outmsg(1) = Convert.ToString(db.GetParameterValue(dbCommand, "@Applicant_auto_Cd"))
                outmsg(2) = Convert.ToString(db.GetParameterValue(dbCommand, "@file_location"))
                'outmsg(2) = Convert.ToString(db.GetParameterValue(dbCommand, "@MO_Details"))
                'outmsg(1) = "1"

            Catch ex As Exception
                'outmsg = ex.Message()
                outmsg(0) = "0"
            End Try
            Return outmsg
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
            Dim outmsg(3) As String
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_New_Applicant_Draft_Groom")
                db.AddInParameter(dbCommand, "@Form_id", DbType.[String], Form_id)
                db.AddInParameter(dbCommand, "@Act_cd", DbType.[String], Act_cd)

                db.AddInParameter(dbCommand, "@Groom_FNam", DbType.[String], Groom_FNam)
                db.AddInParameter(dbCommand, "@Groom_MNam", DbType.[String], Groom_MNam)
                db.AddInParameter(dbCommand, "@Groom_LNam", DbType.[String], Groom_LNam)
                db.AddInParameter(dbCommand, "@Groom_Father_Nam", DbType.[String], Groom_Father_Nam)
                db.AddInParameter(dbCommand, "@Groom_Mother_Nam", DbType.[String], Groom_Mother_Nam)

                db.AddInParameter(dbCommand, "@Groom_Present_HNo", DbType.[String], Groom_Present_HNo)
                db.AddInParameter(dbCommand, "@Groom_Present_Address", DbType.[String], Groom_Present_Address)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_Country", DbType.[String], Groom_PrsntAdd_Country)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_State", DbType.[String], Groom_PrsntAdd_State)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_State_flag", DbType.[String], Groom_PrsntAdd_State_flag)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_Dist", DbType.[String], Groom_PrsntAdd_Dist)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_SubDiv", DbType.[String], Groom_PrsntAdd_SubDiv)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_Block", DbType.[String], Groom_PrsntAdd_Block)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_PS", DbType.[String], Groom_PrsntAdd_PS)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_WA", DbType.[String], Groom_PrsntAdd_WA)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_MuniGp", DbType.[String], Groom_PrsntAdd_MuniGp)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_VilWrd", DbType.[String], Groom_PrsntAdd_VilWrd)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_PO", DbType.[String], Groom_PrsntAdd_PO)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_Pin", DbType.[String], Groom_PrsntAdd_Pin)

                db.AddInParameter(dbCommand, "@Groom_Permanent_HNo", DbType.[String], Groom_Permanent_HNo)
                db.AddInParameter(dbCommand, "@Groom_Permanent_Address", DbType.[String], Groom_Permanent_Address)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_Country", DbType.[String], Groom_PrmntAdd_Country)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_State", DbType.[String], Groom_PrmntAdd_State)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_State_flag", DbType.[String], Groom_PrmntAdd_State_flag)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_Dist", DbType.[String], Groom_PrmntAdd_Dist)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_SubDiv", DbType.[String], Groom_PrmntAdd_SubDiv)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_Block", DbType.[String], Groom_PrmntAdd_Block)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_PS", DbType.[String], Groom_PrmntAdd_PS)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_WA", DbType.[String], Groom_PrmntAdd_WA)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_MuniGp", DbType.[String], Groom_PrmntAdd_MuniGp)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_VilWrd", DbType.[String], Groom_PrmntAdd_VilWrd)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_PO", DbType.[String], Groom_PrmntAdd_PO)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_Pin", DbType.[String], Groom_PrmntAdd_Pin)

                db.AddInParameter(dbCommand, "@Groom_Mob_Num", DbType.[String], Groom_Mob_Num)
                db.AddInParameter(dbCommand, "@Groom_dob", DbType.DateTime, Groom_dob)
                db.AddInParameter(dbCommand, "@Groom_Email_Id", DbType.[String], Groom_Email_Id)
                db.AddInParameter(dbCommand, "@Groom_Religion", DbType.[String], Groom_Religion)
                db.AddInParameter(dbCommand, "@Groom_Aadhr_Num", DbType.[String], Groom_Aadhr_Num)
                db.AddInParameter(dbCommand, "@Groom_occupation", DbType.[String], Groom_occupation)
                db.AddInParameter(dbCommand, "@Groom_maritial_status", DbType.[String], Groom_maritial_status)
                db.AddInParameter(dbCommand, "@Groom_Prev_Mar_Dt", DbType.[String], Groom_Prev_Mar_Dt)
                db.AddInParameter(dbCommand, "@Groom_Photo", DbType.Binary, Groom_Photo)
                db.AddInParameter(dbCommand, "@Groom_Sign", DbType.Binary, Groom_Sign)

                db.AddInParameter(dbCommand, "@Groom_Proof_PrsntAdd_Type", DbType.[String], Groom_Proof_PrsntAdd_Type)
                db.AddInParameter(dbCommand, "@Groom_Proof_PrsntAdd", DbType.Binary, Groom_Proof_PrsntAdd)
                db.AddInParameter(dbCommand, "@Groom_Proof_PrmntAdd_Type", DbType.[String], Groom_Proof_PrmntAdd_Type)
                db.AddInParameter(dbCommand, "@Groom_Proof_PrmntAdd", DbType.Binary, Groom_Proof_PrmntAdd)
                db.AddInParameter(dbCommand, "@Groom_Proof_DOB_Type", DbType.[String], Groom_Proof_DOB_Type)
                db.AddInParameter(dbCommand, "@Groom_Proof_DOB", DbType.Binary, Groom_Proof_DOB)

                db.AddInParameter(dbCommand, "@grm_bpl_prf", DbType.Binary, grm_bpl_prf)

                db.AddInParameter(dbCommand, "@gbpl_serial_cd", DbType.[String], gbpl_serial_cd)
                db.AddInParameter(dbCommand, "@grm_bpl_status", DbType.[String], grm_bpl_status)
                db.AddInParameter(dbCommand, "@Groom_Aadhr_Consent", DbType.[String], gadhrconsent)
                db.AddInParameter(dbCommand, "@Groom_Nationality", DbType.[String], Groom_Nationality)


                db.AddOutParameter(dbCommand, "@Applicant_Total_Cd", DbType.[String], 50)
                db.AddOutParameter(dbCommand, "@Applicant_auto_Cd", DbType.[String], 10)
                'db.AddOutParameter(dbCommand, "@MO_Details", DbType.[String], 1000)
                db.ExecuteNonQuery(dbCommand)
                outmsg(0) = Convert.ToString(db.GetParameterValue(dbCommand, "@Applicant_Total_Cd"))
                outmsg(1) = Convert.ToString(db.GetParameterValue(dbCommand, "@Applicant_auto_Cd"))
                'outmsg(2) = Convert.ToString(db.GetParameterValue(dbCommand, "@MO_Details"))
                'outmsg(1) = "1"
            Catch ex As Exception
                'outmsg = ex.Message()
                outmsg(0) = "0"
            End Try
            Return outmsg
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
            Dim outmsg(3) As String
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_New_Applicant_Draft_Bride")
                db.AddInParameter(dbCommand, "@Form_id", DbType.[String], Form_id)
                db.AddInParameter(dbCommand, "@Act_cd", DbType.[String], Act_cd)

                db.AddInParameter(dbCommand, "@Bride_FNam", DbType.[String], Bride_FNam)
                db.AddInParameter(dbCommand, "@Bride_MNam", DbType.[String], Bride_MNam)
                db.AddInParameter(dbCommand, "@Bride_LNam", DbType.[String], Bride_LNam)
                db.AddInParameter(dbCommand, "@Bride_Father_Nam", DbType.[String], Bride_Father_Nam)
                db.AddInParameter(dbCommand, "@Bride_Mother_Nam", DbType.[String], Bride_Mother_Nam)

                db.AddInParameter(dbCommand, "@Bride_Present_HNo", DbType.[String], Bride_Present_HNo)
                db.AddInParameter(dbCommand, "@Bride_Present_Address", DbType.[String], Bride_Present_Address)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_Country", DbType.[String], Bride_PrsntAdd_Country)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_State", DbType.[String], Bride_PrsntAdd_State)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_State_flag", DbType.[String], Bride_PrsntAdd_State_flag)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_Dist", DbType.[String], Bride_PrsntAdd_Dist)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_SubDiv", DbType.[String], Bride_PrsntAdd_SubDiv)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_Block", DbType.[String], Bride_PrsntAdd_Block)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_PS", DbType.[String], Bride_PrsntAdd_PS)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_WA", DbType.[String], Bride_PrsntAdd_WA)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_MuniGp", DbType.[String], Bride_PrsntAdd_MuniGp)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_VilWrd", DbType.[String], Bride_PrsntAdd_VilWrd)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_PO", DbType.[String], Bride_PrsntAdd_PO)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_Pin", DbType.[String], Bride_PrsntAdd_Pin)

                db.AddInParameter(dbCommand, "@Bride_Permanent_HNo", DbType.[String], Bride_Permanent_HNo)
                db.AddInParameter(dbCommand, "@Bride_Permanent_Address", DbType.[String], Bride_Permanent_Address)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_Country", DbType.[String], Bride_PrmntAdd_Country)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_State", DbType.[String], Bride_PrmntAdd_State)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_State_flag", DbType.[String], Bride_PrmntAdd_State_flag)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_Dist", DbType.[String], Bride_PrmntAdd_Dist)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_SubDiv", DbType.[String], Bride_PrmntAdd_SubDiv)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_Block", DbType.[String], Bride_PrmntAdd_Block)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_PS", DbType.[String], Bride_PrmntAdd_PS)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_WA", DbType.[String], Bride_PrmntAdd_WA)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_MuniGp", DbType.[String], Bride_PrmntAdd_MuniGp)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_VilWrd", DbType.[String], Bride_PrmntAdd_VilWrd)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_PO", DbType.[String], Bride_PrmntAdd_PO)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_Pin", DbType.[String], Bride_PrmntAdd_Pin)

                db.AddInParameter(dbCommand, "@Bride_Mob_Num", DbType.[String], Bride_Mob_Num)
                db.AddInParameter(dbCommand, "@Bride_dob", DbType.DateTime, Bride_dob)
                db.AddInParameter(dbCommand, "@Bride_Email_Id", DbType.[String], Bride_Email_Id)
                db.AddInParameter(dbCommand, "@Bride_Religion", DbType.[String], Bride_Religion)
                db.AddInParameter(dbCommand, "@Bride_Aadhr_Num", DbType.[String], Bride_Aadhr_Num)
                db.AddInParameter(dbCommand, "@Bride_Occupation", DbType.[String], Bride_Occupation)
                db.AddInParameter(dbCommand, "@Bride_maritial_status", DbType.[String], Bride_maritial_status)
                db.AddInParameter(dbCommand, "@Bride_Prev_Mar_Dt", DbType.[String], Bride_Prev_Mar_Dt)
                db.AddInParameter(dbCommand, "@Bride_Photo", DbType.Binary, Bride_Photo)
                db.AddInParameter(dbCommand, "@Bride_Sign", DbType.Binary, Bride_Sign)

                db.AddInParameter(dbCommand, "@Bride_Proof_PrsntAdd_Type", DbType.[String], Bride_Proof_PrsntAdd_Type)
                db.AddInParameter(dbCommand, "@Bride_Proof_PrsntAdd", DbType.Binary, Bride_Proof_PrsntAdd)
                db.AddInParameter(dbCommand, "@Bride_Proof_PrmntAdd_Type", DbType.[String], Bride_Proof_PrmntAdd_Type)
                db.AddInParameter(dbCommand, "@Bride_Proof_PrmntAdd", DbType.Binary, Bride_Proof_PrmntAdd)
                db.AddInParameter(dbCommand, "@Bride_Proof_DOB_Type", DbType.[String], Bride_Proof_DOB_Type)
                db.AddInParameter(dbCommand, "@Bride_Proof_DOB", DbType.Binary, Bride_Proof_DOB)

                db.AddInParameter(dbCommand, "@bride_bpl_prf", DbType.Binary, bride_bpl_prf)

                db.AddInParameter(dbCommand, "@bbpl_serial_cd", DbType.[String], bbpl_serial_cd)

                db.AddInParameter(dbCommand, "@bride_bpl_status", DbType.[String], bride_bpl_status)

                db.AddInParameter(dbCommand, "@Bride_Aadhr_Consent", DbType.[String], badhrconsent)

                db.AddInParameter(dbCommand, "@Bride_Nationality", DbType.[String], Bride_Nationality)

                db.AddOutParameter(dbCommand, "@Applicant_Total_Cd", DbType.[String], 50)
                db.AddOutParameter(dbCommand, "@Applicant_auto_Cd", DbType.[String], 10)
                'db.AddOutParameter(dbCommand, "@MO_Details", DbType.[String], 1000)
                db.ExecuteNonQuery(dbCommand)
                outmsg(0) = Convert.ToString(db.GetParameterValue(dbCommand, "@Applicant_Total_Cd"))
                outmsg(1) = Convert.ToString(db.GetParameterValue(dbCommand, "@Applicant_auto_Cd"))
                'outmsg(2) = Convert.ToString(db.GetParameterValue(dbCommand, "@MO_Details"))
                'outmsg(1) = "1"
            Catch ex As Exception
                'outmsg = ex.Message()
                outmsg(0) = "0"
            End Try
            Return outmsg
        End Function
#End Region

#Region "BSK Insert"
        Public Function BSKInsert(App_cd As String, Draft_Cd As String, Payment_status As String, Ip_address As String, Mac_address As String,
                                  UserId As String, decrypt_UserId As String, mo_cd As String, TicktNo As String) As String
            Dim outmsg As String
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("[Marreg_BSK_count_sp]")
                db.AddInParameter(dbCommand, "@App_cd", DbType.[String], App_cd)
                db.AddInParameter(dbCommand, "@Draft_cd", DbType.[String], Draft_Cd)
                db.AddInParameter(dbCommand, "@Payment_status", DbType.[String], Payment_status)

                db.AddInParameter(dbCommand, "@Ip_address", DbType.[String], Ip_address)
                db.AddInParameter(dbCommand, "@Mac_address", DbType.[String], Mac_address)
                db.AddInParameter(dbCommand, "@UserId", DbType.[String], UserId)
                db.AddInParameter(dbCommand, "@decrypt_UserId", DbType.[String], decrypt_UserId)
                db.AddInParameter(dbCommand, "@mo_cd", DbType.[String], mo_cd)
                db.AddInParameter(dbCommand, "@TicktNo", DbType.[String], TicktNo)

                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))

            Catch ex As Exception
                outmsg = "0"
            End Try
            Return outmsg
        End Function

        Public Function BSK_param1_UserId(UserId As String) As String
            Dim outmsg As String
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("[bsk_test_sp]")
                db.AddInParameter(dbCommand, "@UserId", DbType.[String], UserId)
                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = "0"
            End Try
            Return outmsg
        End Function

#End Region

#Region "Get Saved Draft"
        Public Function GetDraftApplication(Draft_Cd As String) As DataTable
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim dt = New DataTable()
            Try
                con.Open()
                Dim da As New SqlDataAdapter("MARREG_View_NewApp_Draft", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure

                da.SelectCommand.Parameters.Add(New SqlParameter("@App_Auto_Cd", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@App_Auto_Cd").Value = Draft_Cd

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

#End Region

#Region "fill vill wrd"
        Public Function Fill_VilWrd_Txt(Dist As String, SubDiv As String, blck As String, gp As String, Mouza_nm As String, ip_addr As String) As Array
            Dim outmsg(3) As String
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
                outmsg(0) = Convert.ToString(db.GetParameterValue(dbCommand, "@Mouza_cd_out"))
            Catch ex As Exception
                outmsg(0) = "0"
            End Try
            Return outmsg
        End Function
#End Region

#Region "Fill Census"
        Public Function Fill_Cencus(Cen_name As String) As String
            Dim outmsg As String
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Fill_Census")
                db.AddInParameter(dbCommand, "@Census_nm", DbType.[String], Cen_name)
                db.AddOutParameter(dbCommand, "@Census_out", DbType.[String], 10)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Census_out"))
            Catch ex As Exception
                outmsg = "0"
            End Try
            Return outmsg
        End Function
#End Region

#Region "MO Transfer"

#Region "Request for Changing MO"

#Region "Insert"
        Public Function MO_Transfer_Save(App_Auto_Cd As String, From_MO_Autoincrement_Cd As String,
                                         Reason_For_Transfer As String, User_Id As String, Ip_Address As String, tag As String) As String
            Dim outmsg As String
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_MO_Transfer_Insert")
                db.AddInParameter(dbCommand, "@App_Auto_Cd", DbType.[String], App_Auto_Cd)
                db.AddInParameter(dbCommand, "@From_MO_Autoincrmnt_Cd", DbType.[String], From_MO_Autoincrement_Cd)


                db.AddInParameter(dbCommand, "@Reason_For_Transfer", DbType.[String], Reason_For_Transfer)


                db.AddInParameter(dbCommand, "@User_Id", DbType.[String], User_Id)
                db.AddInParameter(dbCommand, "@Ip_Address", DbType.String, Ip_Address)
                db.AddInParameter(dbCommand, "@Tag", DbType.String, tag)
                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region

#Region "Update"
        Public Function MO_Transfer_Update(tabname As DataTable, User_Id As String, IP_Address As String) As String
            Dim outmsg As String = String.Empty

            Try
                Dim db As Database = dbf.CreateConnection()
                Dim con As New SqlConnection(dbconnctn.GetConnectionString())
                Dim insertCommand As SqlCommand = New SqlCommand("MARREG_MO_Transfer_Update", con)
                insertCommand.CommandType = CommandType.StoredProcedure
                Dim tvpParam As SqlParameter = insertCommand.Parameters.AddWithValue("@tab_value", tabname)
                tvpParam.SqlDbType = SqlDbType.Structured
                Dim UserId As SqlParameter = insertCommand.Parameters.AddWithValue("@User_Id", User_Id)
                UserId.SqlDbType = SqlDbType.NVarChar
                Dim IPAddress As SqlParameter = insertCommand.Parameters.AddWithValue("@IP_Address", IP_Address)
                IPAddress.SqlDbType = SqlDbType.NVarChar
                Dim Msg As SqlParameter = insertCommand.Parameters.AddWithValue("@Msg", 50)
                Msg.SqlDbType = SqlDbType.NVarChar
                Msg.Size = 50
                Msg.Direction = ParameterDirection.Output
                con.Open()
                insertCommand.ExecuteNonQuery()
                'outmsg = insertCommand.Parameters["@Msg"].Value.ToString()
                outmsg = insertCommand.Parameters("@Msg").Value.ToString()
                con.Close()
                Return outmsg
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region
#End Region
#Region "Action taken against Request for Changing MO"
#Region "Update"
        Public Function MO_Transfer_Update(tabname As DataTable, User_Id As String, IP_Address As String, Mac_Address As String) As String
            Dim outmsg As String = String.Empty

            Try
                Dim db As Database = dbf.CreateConnection()
                Dim con As New SqlConnection(dbconnctn.GetConnectionString())
                Dim insertCommand As SqlCommand = New SqlCommand("MARREG_MO_Transfer_Update", con)
                insertCommand.CommandType = CommandType.StoredProcedure
                Dim tvpParam As SqlParameter = insertCommand.Parameters.AddWithValue("@tab_value", tabname)
                tvpParam.SqlDbType = SqlDbType.Structured
                Dim UserId As SqlParameter = insertCommand.Parameters.AddWithValue("@User_Id", User_Id)
                UserId.SqlDbType = SqlDbType.NVarChar
                Dim IPAddress As SqlParameter = insertCommand.Parameters.AddWithValue("@IP_Address", IP_Address)
                IPAddress.SqlDbType = SqlDbType.NVarChar
                Dim MacAddress As SqlParameter = insertCommand.Parameters.AddWithValue("@Mac_Address", Mac_Address)
                MacAddress.SqlDbType = SqlDbType.NVarChar
                Dim Msg As SqlParameter = insertCommand.Parameters.AddWithValue("@Msg", 50)
                Msg.SqlDbType = SqlDbType.NVarChar
                Msg.Size = 50
                Msg.Direction = ParameterDirection.Output
                con.Open()
                insertCommand.ExecuteNonQuery()
                'outmsg = insertCommand.Parameters["@Msg"].Value.ToString()
                outmsg = insertCommand.Parameters("@Msg").Value.ToString()
                con.Close()
                Return outmsg
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region

#End Region

#Region "MARREG Transfer Page Grid View"

        Public Function Transfer_Application(Role_Type_Cd As String) As DataTable
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim dt = New DataTable()
            Try
                con.Open()
                Dim da As New SqlDataAdapter("MARREG_MO_Transfer_Gridview", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure

                da.SelectCommand.Parameters.Add(New SqlParameter("@Role_Type_Cd", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@Role_Type_Cd").Value = Role_Type_Cd

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
#End Region

#Region "EDIT"
        Public Function GetApplication(Draft_Cd As String) As DataTable
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim dt = New DataTable()
            Try
                con.Open()
                Dim da As New SqlDataAdapter("MARREG_View_NewApp_temp", con)
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
            Dim outmsg(3) As String
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_New_Applicant_temp_insertBLDB")
                db.AddInParameter(dbCommand, "@Pre_Draft_Cd", DbType.[String], Draft_Cd)
                db.AddInParameter(dbCommand, "@Form_id", DbType.[String], Form_id)
                db.AddInParameter(dbCommand, "@Act_cd", DbType.[String], Act_cd)

                db.AddInParameter(dbCommand, "@Groom_FNam", DbType.[String], Groom_FNam)
                db.AddInParameter(dbCommand, "@Groom_MNam", DbType.[String], Groom_MNam)
                db.AddInParameter(dbCommand, "@Groom_LNam", DbType.[String], Groom_LNam)
                db.AddInParameter(dbCommand, "@Groom_Father_Nam", DbType.[String], Groom_Father_Nam)
                db.AddInParameter(dbCommand, "@Groom_Mother_Nam", DbType.[String], Groom_Mother_Nam)

                db.AddInParameter(dbCommand, "@Groom_Present_HNo", DbType.[String], Groom_Present_HNo)
                db.AddInParameter(dbCommand, "@Groom_Present_Address", DbType.[String], Groom_Present_Address)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_Country", DbType.[String], Groom_PrsntAdd_Country)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_State", DbType.[String], Groom_PrsntAdd_State)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_State_flag", DbType.[String], Groom_PrsntAdd_State_flag)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_Dist", DbType.[String], Groom_PrsntAdd_Dist)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_SubDiv", DbType.[String], Groom_PrsntAdd_SubDiv)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_Block", DbType.[String], Groom_PrsntAdd_Block)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_PS", DbType.[String], Groom_PrsntAdd_PS)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_WA", DbType.[String], Groom_PrsntAdd_WA)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_MuniGp", DbType.[String], Groom_PrsntAdd_MuniGp)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_VilWrd", DbType.[String], Groom_PrsntAdd_VilWrd)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_PO", DbType.[String], Groom_PrsntAdd_PO)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_Pin", DbType.[String], Groom_PrsntAdd_Pin)

                db.AddInParameter(dbCommand, "@Groom_Permanent_HNo", DbType.[String], Groom_Permanent_HNo)
                db.AddInParameter(dbCommand, "@Groom_Permanent_Address", DbType.[String], Groom_Permanent_Address)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_Country", DbType.[String], Groom_PrmntAdd_Country)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_State", DbType.[String], Groom_PrmntAdd_State)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_State_flag", DbType.[String], Groom_PrmntAdd_State_flag)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_Dist", DbType.[String], Groom_PrmntAdd_Dist)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_SubDiv", DbType.[String], Groom_PrmntAdd_SubDiv)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_Block", DbType.[String], Groom_PrmntAdd_Block)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_PS", DbType.[String], Groom_PrmntAdd_PS)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_WA", DbType.[String], Groom_PrmntAdd_WA)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_MuniGp", DbType.[String], Groom_PrmntAdd_MuniGp)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_VilWrd", DbType.[String], Groom_PrmntAdd_VilWrd)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_PO", DbType.[String], Groom_PrmntAdd_PO)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_Pin", DbType.[String], Groom_PrmntAdd_Pin)

                db.AddInParameter(dbCommand, "@Groom_Mob_Num", DbType.[String], Groom_Mob_Num)
                db.AddInParameter(dbCommand, "@Groom_dob", DbType.DateTime, Groom_dob)
                db.AddInParameter(dbCommand, "@Groom_Email_Id", DbType.[String], Groom_Email_Id)
                db.AddInParameter(dbCommand, "@Groom_Religion", DbType.[String], Groom_Religion)
                db.AddInParameter(dbCommand, "@Groom_Aadhr_Num", DbType.[String], Groom_Aadhr_Num)
                db.AddInParameter(dbCommand, "@Groom_occupation", DbType.[String], Groom_occupation)
                db.AddInParameter(dbCommand, "@Groom_maritial_status", DbType.[String], Groom_maritial_status)
                db.AddInParameter(dbCommand, "@Groom_Prev_Mar_Dt", DbType.[String], Groom_Prev_Mar_Dt)
                db.AddInParameter(dbCommand, "@Groom_Photo", DbType.Binary, Groom_Photo)
                db.AddInParameter(dbCommand, "@Groom_Sign", DbType.Binary, Groom_Sign)

                db.AddInParameter(dbCommand, "@Groom_Proof_PrsntAdd_Type", DbType.[String], Groom_Proof_PrsntAdd_Type)
                ' db.AddInParameter(dbCommand, "@Groom_Proof_PrsntAdd", DbType.Binary, Groom_Proof_PrsntAdd) '26.03.2018
                db.AddInParameter(dbCommand, "@Groom_Proof_PrmntAdd_Type", DbType.[String], Groom_Proof_PrmntAdd_Type)
                'db.AddInParameter(dbCommand, "@Groom_Proof_PrmntAdd", DbType.Binary, Groom_Proof_PrmntAdd) '26.03.2018
                db.AddInParameter(dbCommand, "@Groom_Proof_DOB_Type", DbType.[String], Groom_Proof_DOB_Type)
                'db.AddInParameter(dbCommand, "@Groom_Proof_DOB", DbType.Binary, Groom_Proof_DOB) '26.03.2018

                db.AddInParameter(dbCommand, "@Bride_FNam", DbType.[String], Bride_FNam)
                db.AddInParameter(dbCommand, "@Bride_MNam", DbType.[String], Bride_MNam)
                db.AddInParameter(dbCommand, "@Bride_LNam", DbType.[String], Bride_LNam)
                db.AddInParameter(dbCommand, "@Bride_Father_Nam", DbType.[String], Bride_Father_Nam)
                db.AddInParameter(dbCommand, "@Bride_Mother_Nam", DbType.[String], Bride_Mother_Nam)

                db.AddInParameter(dbCommand, "@Bride_Present_HNo", DbType.[String], Bride_Present_HNo)
                db.AddInParameter(dbCommand, "@Bride_Present_Address", DbType.[String], Bride_Present_Address)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_Country", DbType.[String], Bride_PrsntAdd_Country)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_State", DbType.[String], Bride_PrsntAdd_State)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_State_flag", DbType.[String], Bride_PrsntAdd_State_flag)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_Dist", DbType.[String], Bride_PrsntAdd_Dist)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_SubDiv", DbType.[String], Bride_PrsntAdd_SubDiv)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_Block", DbType.[String], Bride_PrsntAdd_Block)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_PS", DbType.[String], Bride_PrsntAdd_PS)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_WA", DbType.[String], Bride_PrsntAdd_WA)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_MuniGp", DbType.[String], Bride_PrsntAdd_MuniGp)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_VilWrd", DbType.[String], Bride_PrsntAdd_VilWrd)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_PO", DbType.[String], Bride_PrsntAdd_PO)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_Pin", DbType.[String], Bride_PrsntAdd_Pin)

                db.AddInParameter(dbCommand, "@Bride_Permanent_HNo", DbType.[String], Bride_Permanent_HNo)
                db.AddInParameter(dbCommand, "@Bride_Permanent_Address", DbType.[String], Bride_Permanent_Address)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_Country", DbType.[String], Bride_PrmntAdd_Country)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_State", DbType.[String], Bride_PrmntAdd_State)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_State_flag", DbType.[String], Bride_PrmntAdd_State_flag)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_Dist", DbType.[String], Bride_PrmntAdd_Dist)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_SubDiv", DbType.[String], Bride_PrmntAdd_SubDiv)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_Block", DbType.[String], Bride_PrmntAdd_Block)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_PS", DbType.[String], Bride_PrmntAdd_PS)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_WA", DbType.[String], Bride_PrmntAdd_WA)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_MuniGp", DbType.[String], Bride_PrmntAdd_MuniGp)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_VilWrd", DbType.[String], Bride_PrmntAdd_VilWrd)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_PO", DbType.[String], Bride_PrmntAdd_PO)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_Pin", DbType.[String], Bride_PrmntAdd_Pin)

                db.AddInParameter(dbCommand, "@Bride_Mob_Num", DbType.[String], Bride_Mob_Num)
                db.AddInParameter(dbCommand, "@Bride_dob", DbType.DateTime, Bride_dob)
                db.AddInParameter(dbCommand, "@Bride_Email_Id", DbType.[String], Bride_Email_Id)
                db.AddInParameter(dbCommand, "@Bride_Religion", DbType.[String], Bride_Religion)
                db.AddInParameter(dbCommand, "@Bride_Aadhr_Num", DbType.[String], Bride_Aadhr_Num)
                db.AddInParameter(dbCommand, "@Bride_Occupation", DbType.[String], Bride_Occupation)
                db.AddInParameter(dbCommand, "@Bride_maritial_status", DbType.[String], Bride_maritial_status)
                db.AddInParameter(dbCommand, "@Bride_Prev_Mar_Dt", DbType.[String], Bride_Prev_Mar_Dt)
                db.AddInParameter(dbCommand, "@Bride_Photo", DbType.Binary, Bride_Photo)
                db.AddInParameter(dbCommand, "@Bride_Sign", DbType.Binary, Bride_Sign)

                db.AddInParameter(dbCommand, "@Bride_Proof_PrsntAdd_Type", DbType.[String], Bride_Proof_PrsntAdd_Type)
                ' db.AddInParameter(dbCommand, "@Bride_Proof_PrsntAdd", DbType.Binary, Bride_Proof_PrsntAdd) '26.03.2018
                db.AddInParameter(dbCommand, "@Bride_Proof_PrmntAdd_Type", DbType.[String], Bride_Proof_PrmntAdd_Type)
                ' db.AddInParameter(dbCommand, "@Bride_Proof_PrmntAdd", DbType.Binary, Bride_Proof_PrmntAdd) '26.03.2018
                db.AddInParameter(dbCommand, "@Bride_Proof_DOB_Type", DbType.[String], Bride_Proof_DOB_Type)
                ' db.AddInParameter(dbCommand, "@Bride_Proof_DOB", DbType.Binary, Bride_Proof_DOB) '26.03.2018

                db.AddInParameter(dbCommand, "@MO_Autoincrement_cd", DbType.[String], MO_Autoincrement_cd)
                db.AddInParameter(dbCommand, "@Place_Reg_Address", DbType.[String], Place_Reg_Address)
                db.AddInParameter(dbCommand, "@Place_Reg_State", DbType.[String], Place_Reg_State)
                db.AddInParameter(dbCommand, "@Place_Reg_Dist", DbType.[String], Place_Reg_Dist)
                db.AddInParameter(dbCommand, "@Place_Reg_SubDiv", DbType.[String], Place_Reg_SubDiv)
                db.AddInParameter(dbCommand, "@Place_Reg_Block", DbType.[String], Place_Reg_Block)
                db.AddInParameter(dbCommand, "@Place_Reg_PS", DbType.[String], Place_Reg_PS)
                db.AddInParameter(dbCommand, "@Place_Reg_WA", DbType.[String], Place_Reg_WA)
                db.AddInParameter(dbCommand, "@Place_Reg_MuniGp", DbType.[String], Place_Reg_MuniGp)
                db.AddInParameter(dbCommand, "@Place_Reg_VilWrd", DbType.[String], Place_Reg_VilWrd)
                db.AddInParameter(dbCommand, "@Place_Reg_PO", DbType.[String], Place_Reg_PO)

                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_HNo", DbType.[String], Place_Socl_Mrg_HNo)
                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_Address", DbType.[String], Place_Socl_Mrg_Address)
                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_Country", DbType.[String], Place_Socl_Mrg_Country)
                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_State", DbType.[String], Place_Socl_Mrg_State)
                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_State_flag", DbType.[String], Place_Socl_Mrg_State_flag)
                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_Dist", DbType.[String], Place_Socl_Mrg_Dist)
                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_SubDiv", DbType.[String], Place_Socl_Mrg_SubDiv)
                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_Block", DbType.[String], Place_Socl_Mrg_Block)
                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_PS", DbType.[String], Place_Socl_Mrg_PS)
                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_WA", DbType.[String], Place_Socl_Mrg_WA)
                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_MuniGp", DbType.[String], Place_Socl_Mrg_MuniGp)
                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_VilWrd", DbType.[String], Place_Socl_Mrg_VilWrd)
                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_PO", DbType.[String], Place_Socl_Mrg_PO)
                db.AddInParameter(dbCommand, "@Place_Socl_Mrg_Pin", DbType.[String], Place_Socl_Mrg_Pin)

                db.AddInParameter(dbCommand, "@Socl_Mrg_Dt", DbType.[String], Socl_Mrg_Dt)
                db.AddInParameter(dbCommand, "@Mrg_Invi_Card", DbType.Binary, Mrg_Invi_Card)

                db.AddInParameter(dbCommand, "@Place_Reg", DbType.[String], Place_Reg)
                db.AddInParameter(dbCommand, "@Time_Reg", DbType.[String], Time_Reg)
                db.AddInParameter(dbCommand, "@Ip_Address", DbType.String, IP_Address)

                db.AddInParameter(dbCommand, "@gbpl_serial_cd", DbType.[String], gbpl_serial_cd)
                db.AddInParameter(dbCommand, "@bbpl_serial_cd", DbType.[String], bbpl_serial_cd)
                db.AddInParameter(dbCommand, "@grm_bpl_status", DbType.[String], grm_bpl_status)
                db.AddInParameter(dbCommand, "@bride_bpl_status", DbType.[String], bride_bpl_status)
                db.AddInParameter(dbCommand, "@applicant_bpl_status", DbType.[String], applicant_bpl_status)

                db.AddInParameter(dbCommand, "@Groom_Aadhr_Consent", DbType.[String], gadhrconsent)
                db.AddInParameter(dbCommand, "@Bride_Aadhr_Consent", DbType.[String], badhrconsent)

                db.AddInParameter(dbCommand, "@Groom_Nationality", DbType.[String], Groom_Nationality)
                db.AddInParameter(dbCommand, "@Bride_Nationality", DbType.[String], Bride_Nationality)

                db.AddInParameter(dbCommand, "@MO_Verified", DbType.[String], MO_Verified) '26.04.2018 

                db.AddInParameter(dbCommand, "@Groom_Religion_Other_Name", DbType.[String], Groom_Religion_Other_Name)
                db.AddInParameter(dbCommand, "@Groom_Occupation_Other_Name", DbType.[String], Groom_Occupation_Other_Name)
                db.AddInParameter(dbCommand, "@Bride_Religion_Other_Name", DbType.[String], Bride_Religion_Other_Name)
                db.AddInParameter(dbCommand, "@Bride_Occupation_Other_Name", DbType.[String], Bride_Occupation_Other_Name)

                db.AddInParameter(dbCommand, "@Groom_IdProofNo_PresentAddress", DbType.[String], Groom_IdProofNo_PresentAddress)
                db.AddInParameter(dbCommand, "@Groom_IdProofNo_Dob", DbType.[String], Groom_IdProofNo_Dob)
                db.AddInParameter(dbCommand, "@Groom_IdProofNo_PermanentAddress", DbType.[String], Groom_IdProofNo_PermanentAddress)
                db.AddInParameter(dbCommand, "@Bride_IdProofNo_PresentAddress", DbType.[String], Bride_IdProofNo_PresentAddress)
                db.AddInParameter(dbCommand, "@Bride_IdProofNo_Dob", DbType.[String], Bride_IdProofNo_Dob)
                db.AddInParameter(dbCommand, "@Bride_IdProofNo_PermanentAddress", DbType.[String], Bride_IdProofNo_PermanentAddress)
                db.AddInParameter(dbCommand, "@Groom_Guardian_type", DbType.[String], Groom_Guardian_type)
                db.AddInParameter(dbCommand, "@Bride_Guardian_type", DbType.[String], Bride_Guardian_type)
                'anay 23.08.2018---------------------
                db.AddInParameter(dbCommand, "@Groom_Length_Residence", DbType.[String], Groom_length_residence)
                db.AddInParameter(dbCommand, "@Bride_Length_Residence", DbType.[String], Bride_length_residence)
                '------------------------------------

                '05-09-2018-----------------------
                db.AddInParameter(dbCommand, "@Groom_CaseNo", DbType.[String], g_caseno)
                db.AddInParameter(dbCommand, "@Groom_Court_Name", DbType.[String], g_courtname)
                db.AddInParameter(dbCommand, "@Groom_DateOf_Order", DbType.String, g_dateoforder)
                db.AddInParameter(dbCommand, "@Groom_Petitioners_Name", DbType.[String], g_petitionername)
                db.AddInParameter(dbCommand, "@Groom_Respondents_Name", DbType.[String], g_respondentname)

                db.AddInParameter(dbCommand, "@Bride_CaseNo", DbType.[String], b_caseno)
                db.AddInParameter(dbCommand, "@Bride_Court_Name", DbType.[String], b_courtname)
                db.AddInParameter(dbCommand, "@Bride_DateOf_Order", DbType.String, b_dateoforder)
                db.AddInParameter(dbCommand, "@Bride_Petitioners_Name", DbType.[String], b_petitionername)
                db.AddInParameter(dbCommand, "@Bride_Respondents_Name", DbType.[String], b_respondentname)

                'db.AddInParameter(dbCommand, "@Groom_Prf_Divorce_Doc", DbType.[String], gdivorce_prf)
                db.AddInParameter(dbCommand, "@Groom_Prf_Divorce_Doc_No", DbType.[String], g_txtDivorceCertificateNo)
                'db.AddInParameter(dbCommand, "@Bride_Prf_Divorce_Doc", DbType.[String], bdivorce_prf)
                db.AddInParameter(dbCommand, "@Bride_Prf_Divorce_Doc_No", DbType.[String], b_txtDivorceCertificateNo)

                'db.AddInParameter(dbCommand, "@Groom_Prf_Wife_Death_Certificate_Doc", DbType.[String], wife_detCrt_prf)
                db.AddInParameter(dbCommand, "@Groom_Prf_Wife_Death_Certificate_No", DbType.[String], WifeDeathCertificateNo)

                'db.AddInParameter(dbCommand, "@Bride_Prf_Husband_Death_Certificate", DbType.[String], Husband_detCrt_prf)
                db.AddInParameter(dbCommand, "@Bride_Prf_Husband_Death_Certificate_No", DbType.[String], HusbandDeathCertificateNo)
                db.AddInParameter(dbCommand, "@Bride_Prf_With_HusbandName_type", DbType.[String], ddlDeathHusbGovId)
                'db.AddInParameter(dbCommand, "@Bride_Prf_With_HusbandName_Doc", DbType.[String], HusbndDtCrt_wh_prf)
                db.AddInParameter(dbCommand, "@Bride_Pfr_With_HusbandName_IdNumber", DbType.[String], txtWifeGovIdWithHusName)

                db.AddInParameter(dbCommand, "@groom_legalguardian_proof_id_no", DbType.[String], groom_legalguardian_proof_id)
                db.AddInParameter(dbCommand, "@groom_legalguardian_proof_type", DbType.[String], groom_legalguardian_proof_type)
                db.AddInParameter(dbCommand, "@bride_legalguardian_proof_id_no", DbType.[String], bride_legalguardian_proof_id)
                db.AddInParameter(dbCommand, "@bride_legalguardian_proof_type", DbType.[String], bride_legalguardian_proof_type)
                '-------------------------
                db.AddInParameter(dbCommand, "@IsBrideMaidenName", DbType.[String], IsBrideMaidenName)
                db.AddInParameter(dbCommand, "@BrideFatherLastName", DbType.[String], BrideFatherLastName)
                db.AddInParameter(dbCommand, "@IsCerificateReopen", DbType.[String], IsCerificateReopen)
                db.AddInParameter(dbCommand, "@userId", DbType.[String], userId)
                '--------------START------------------
                db.AddInParameter(dbCommand, "@IsGroomSameAddr", DbType.[String], IsGroomSameAddr)
                db.AddInParameter(dbCommand, "@IsBrideSameAddr", DbType.[String], IsBrideSameAddr)
                '------------------END---------------------------
                db.AddOutParameter(dbCommand, "@Applicant_Total_Cd", DbType.[String], 50)
                db.AddOutParameter(dbCommand, "@Applicant_auto_Cd", DbType.[String], 10)
                db.AddOutParameter(dbCommand, "@file_location", DbType.[String], 100)

                'db.AddOutParameter(dbCommand, "@MO_Details", DbType.[String], 1000)
                db.ExecuteNonQuery(dbCommand)
                outmsg(0) = Convert.ToString(db.GetParameterValue(dbCommand, "@Applicant_Total_Cd"))
                outmsg(1) = Convert.ToString(db.GetParameterValue(dbCommand, "@Applicant_auto_Cd"))
                outmsg(2) = Convert.ToString(db.GetParameterValue(dbCommand, "@file_location"))
                'outmsg(2) = Convert.ToString(db.GetParameterValue(dbCommand, "@MO_Details"))
                'outmsg(1) = "1"

            Catch ex As Exception
                'outmsg = ex.Message()
                outmsg(0) = "0"
            End Try
            Return outmsg
        End Function

        Public Function NewApp_BG_Child_Save_Edit(App_Auto_Cd As String, Groom As DataTable, Bride As DataTable, Tag As String, Choose_Child_By As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim con As New SqlConnection(dbconnctn.GetConnectionString())
                Dim insertCommand As SqlCommand = New SqlCommand("MARREG_NewApp_Child_Insert_temp", con)
                insertCommand.CommandType = CommandType.StoredProcedure

                Dim app_auto As SqlParameter = insertCommand.Parameters.AddWithValue("@App_Auto_Cd", App_Auto_Cd)
                app_auto.SqlDbType = SqlDbType.NVarChar

                Dim Tagg As SqlParameter = insertCommand.Parameters.AddWithValue("@Tag", Tag)
                Tagg.SqlDbType = SqlDbType.NVarChar

                Dim Choose_Child As SqlParameter = insertCommand.Parameters.AddWithValue("@Choose_Child_By", Choose_Child_By)
                Tagg.SqlDbType = SqlDbType.NVarChar

                Dim Groom_tab As SqlParameter = insertCommand.Parameters.AddWithValue("@Groom", Groom)
                Groom_tab.SqlDbType = SqlDbType.Structured

                Dim Bride_tab As SqlParameter = insertCommand.Parameters.AddWithValue("@child", Bride)
                Bride_tab.SqlDbType = SqlDbType.Structured

                Dim Msg As SqlParameter = insertCommand.Parameters.AddWithValue("@Msg", 50)
                Msg.SqlDbType = SqlDbType.NVarChar
                Msg.Size = 50
                Msg.Direction = ParameterDirection.Output

                Dim file As SqlParameter = insertCommand.Parameters.AddWithValue("@filePath", 50)
                file.SqlDbType = SqlDbType.NVarChar
                file.Size = 100
                file.Direction = ParameterDirection.Output

                con.Open()
                insertCommand.ExecuteNonQuery()
                outmsg = insertCommand.Parameters("@Msg").Value.ToString()
                con.Close()
            Catch ex As Exception
                outmsg = ex.Message
            End Try
            Return outmsg
        End Function

        Public Function GetCorrectionTblData(App_Cd As String) As DataTable
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim dt = New DataTable()
            Try
                con.Open()
                Dim da As New SqlDataAdapter("MARREG_Correction_TblInfo", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure

                da.SelectCommand.Parameters.Add(New SqlParameter("@App_Cd", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@App_Cd").Value = App_Cd

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

        Public Function MO_Correction_Insert(app_cd As String, application_no As String, g_lnm_flag As String, groom_lastname As String, g_mnm_flag As String, groom_mothernm As String,
                                            b_lnm_flag As String, bride_lname As String, bride_mnm_flag As String, bride_m_fullname As String,
                                             Witness1_doc As Byte(), Witness2_doc As Byte(), Witness1_Sign As Byte(), Witness2_Sign As Byte(), Witness1_name As String, Witness2_name As String, witnss1_relation As String, witnss2_relation As String, witnss1_proof_identity As String, witnss2_proof_identity As String,
                                            Groom_present_addr_flag As String, gprstadd_hno As String, gprstadd_subdiv As String, gprstadd_wa As String, gprstadd_block As String, gprstadd_gp_or_municipal As String, gprstadd_vill As String, gprstadd_po As String, gprstadd_pin As String,
                                            Groom_Permanent_addr_flag As String, gpmntadd_hno As String, gpmntadd_subdiv As String, gpmntadd_wa As String, gpmntadd_block As String, gpmntadd_gp_or_municipal As String, gpmntadd_vill As String, gpmntadd_po As String, gpmntadd_pin As String,
                                              Bride_present_addr_flag As String, bprstadd_hno As String, bprstadd_subdiv As String, bprstadd_wa As String, bprstadd_block As String, bprstadd_gp_or_municipal As String, bprstadd_vill As String, bprstadd_po As String, bprstadd_pin As String,
                                             Bride_Permanent_addr_flag As String, bpmntadd_hno As String, bpmntadd_subdiv As String, bpmntadd_wa As String, bpmntadd_block As String, bpmntadd_gp_or_municipal As String, bpmntadd_vill As String, bpmntadd_po As String, bpmntadd_pin As String,
                                              Child_Final_Insert As String, ip_address As String, MAC_Address As String) As Array

            Dim outmsg(3) As String
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Correction_sp")
                db.AddInParameter(dbCommand, "@App_Cd", DbType.[String], app_cd)
                db.AddInParameter(dbCommand, "@Application_no", DbType.[String], application_no)
                db.AddInParameter(dbCommand, "@Groom_LNam_FLAG", DbType.[String], g_lnm_flag)
                db.AddInParameter(dbCommand, "@Groom_LNam", DbType.[String], groom_lastname)
                db.AddInParameter(dbCommand, "@Groom_Mother_Nam_FLAG", DbType.[String], g_mnm_flag)
                db.AddInParameter(dbCommand, "@Groom_Mother_Nam", DbType.[String], groom_mothernm)

                db.AddInParameter(dbCommand, "@Bride_LNam_FLAG", DbType.[String], b_lnm_flag)
                db.AddInParameter(dbCommand, "@Bride_LNam", DbType.[String], bride_lname)
                db.AddInParameter(dbCommand, "@Bride_Mother_Nam_FLAG", DbType.[String], bride_mnm_flag)
                db.AddInParameter(dbCommand, "@Bride_Mother_Nam", DbType.[String], bride_m_fullname)

                db.AddInParameter(dbCommand, "@Witness1_doc", DbType.Binary, Witness1_doc)
                db.AddInParameter(dbCommand, "@Witness2_doc", DbType.Binary, Witness2_doc)
                db.AddInParameter(dbCommand, "@Witness1_Sign", DbType.Binary, Witness1_Sign)
                db.AddInParameter(dbCommand, "@Witness2_Sign", DbType.Binary, Witness2_Sign)
                db.AddInParameter(dbCommand, "@Witness1_name", DbType.[String], Witness1_name)
                db.AddInParameter(dbCommand, "@Witness2_name", DbType.[String], Witness2_name)
                db.AddInParameter(dbCommand, "@witnss1_relation", DbType.[String], witnss1_relation)
                db.AddInParameter(dbCommand, "@witnss2_relation", DbType.[String], witnss2_relation)
                db.AddInParameter(dbCommand, "@witnss1_proof_identity", DbType.[String], witnss1_proof_identity)
                db.AddInParameter(dbCommand, "@witnss2_proof_identity", DbType.[String], witnss2_proof_identity)


                'groom_present_address @

                db.AddInParameter(dbCommand, "@Groom_Present_Address_FLAG", DbType.[String], Groom_present_addr_flag)
                db.AddInParameter(dbCommand, "@Groom_prsntAddress", DbType.[String], gprstadd_hno)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_SubDiv", DbType.[String], gprstadd_subdiv)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_WA", DbType.[String], gprstadd_wa)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_Block", DbType.[String], gprstadd_block)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_MuniGp", DbType.[String], gprstadd_gp_or_municipal)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_VilWrd", DbType.[String], gprstadd_vill)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_PO", DbType.[String], gprstadd_po)
                db.AddInParameter(dbCommand, "@Groom_PrsntAdd_Pin", DbType.[String], gprstadd_pin)

                'bride_present_address 
                db.AddInParameter(dbCommand, "@Bride_Present_Address_FLAG", DbType.[String], Bride_present_addr_flag)
                db.AddInParameter(dbCommand, "@Bride_PresentAddress", DbType.[String], bprstadd_hno)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_SubDiv", DbType.[String], bprstadd_subdiv)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_WA", DbType.[String], bprstadd_wa)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_Block", DbType.[String], bprstadd_block)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_MuniGp", DbType.[String], bprstadd_gp_or_municipal)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_VilWrd", DbType.[String], bprstadd_vill)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_PO", DbType.[String], bprstadd_po)
                db.AddInParameter(dbCommand, "@Bride_PrsntAdd_Pin", DbType.[String], bprstadd_pin)

                'groom_permanent_address
                db.AddInParameter(dbCommand, "@Groom_Permanent_Address_FLAG", DbType.[String], Groom_Permanent_addr_flag)
                db.AddInParameter(dbCommand, "@groom_peranentAddress", DbType.[String], gpmntadd_hno)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_SubDiv", DbType.[String], gpmntadd_subdiv)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_WA", DbType.[String], gpmntadd_wa)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_Block", DbType.[String], gpmntadd_block)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_MuniGp", DbType.[String], gpmntadd_gp_or_municipal)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_VilWrd", DbType.[String], gpmntadd_vill)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_PO", DbType.[String], gpmntadd_po)
                db.AddInParameter(dbCommand, "@Groom_PrmntAdd_Pin", DbType.[String], gpmntadd_pin)

                'Bride_permanent_address

                db.AddInParameter(dbCommand, "@Bride_Permanent_Address_FLAG", DbType.[String], Bride_Permanent_addr_flag)
                db.AddInParameter(dbCommand, "@Bride_peranentAddress", DbType.[String], bpmntadd_hno)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_SubDiv", DbType.[String], bpmntadd_subdiv)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_WA", DbType.[String], bpmntadd_wa)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_Block", DbType.[String], bpmntadd_block)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_MuniGp", DbType.[String], bpmntadd_gp_or_municipal)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_VilWrd", DbType.[String], bpmntadd_vill)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_PO", DbType.[String], bpmntadd_po)
                db.AddInParameter(dbCommand, "@Bride_PrmntAdd_Pin", DbType.[String], bpmntadd_pin)

                db.AddInParameter(dbCommand, "@Child_insert_flag", DbType.[String], Child_Final_Insert)
                db.AddInParameter(dbCommand, "@ip_address", DbType.[String], ip_address)
                db.AddInParameter(dbCommand, "@MAC_Address", DbType.[String], MAC_Address)

                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 100)

                db.ExecuteNonQuery(dbCommand)
                outmsg(0) = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))

            Catch ex As Exception

                outmsg(0) = "0"
            End Try
            Return outmsg
        End Function
#End Region

#Region "Dulicate Entry"
        Public Function Check_duplicate_entry(marriage_act As String, grom_fname As String, groom_mname As String, groom_lname As String, groom_father_name As String,
                                              groom_mother_name As String, groom_dob As String, groom_mrt_status As String, bride_fnam As String,
                                              bride_mname As String, bride_lname As String, bride_father_name As String, bride_mother_name As String,
                                              bride_dob As String, bride_mrt_status As String) As Array

            Dim outmsg(2) As String
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Check_Duplicate_Applicantion")
                db.AddInParameter(dbCommand, "@Act_cd", DbType.[String], marriage_act)

                db.AddInParameter(dbCommand, "@Groom_FNam", DbType.[String], grom_fname)
                db.AddInParameter(dbCommand, "@Groom_MNam", DbType.[String], groom_mname)
                db.AddInParameter(dbCommand, "@Groom_LNam", DbType.[String], groom_lname)
                db.AddInParameter(dbCommand, "@Groom_Father_Nam", DbType.[String], groom_father_name)
                db.AddInParameter(dbCommand, "@Groom_Mother_Nam", DbType.[String], groom_mother_name)
                db.AddInParameter(dbCommand, "@Groom_dob", DbType.DateTime, groom_dob)
                db.AddInParameter(dbCommand, "@Groom_maritial_status", DbType.[String], groom_mrt_status)

                db.AddInParameter(dbCommand, "@Bride_FNam", DbType.[String], bride_fnam)
                db.AddInParameter(dbCommand, "@Bride_MNam", DbType.[String], bride_mname)
                db.AddInParameter(dbCommand, "@Bride_LNam", DbType.[String], bride_lname)
                db.AddInParameter(dbCommand, "@Bride_Father_Nam", DbType.[String], bride_father_name)
                db.AddInParameter(dbCommand, "@Bride_Mother_Nam", DbType.[String], bride_mother_name)
                db.AddInParameter(dbCommand, "@Bride_dob", DbType.DateTime, bride_dob)
                db.AddInParameter(dbCommand, "@Bride_maritial_status", DbType.[String], bride_mrt_status)

                db.AddOutParameter(dbCommand, "@application_status", DbType.[String], 25)

                db.ExecuteNonQuery(dbCommand)
                outmsg(0) = Convert.ToString(db.GetParameterValue(dbCommand, "@application_status"))


            Catch ex As Exception
                'outmsg(0) = "0"
                outmsg(0) = "Fresh Entry"

            End Try
            Return outmsg
        End Function

        Public Function Cheak_email_mobile_exist(marriage_act As String, groom_mrt_status As String, groom_mobile_no As String, groom_email_id As String, bride_mrt_status As String, bride_mobile_no As String, bride_email_id As String) As Array

            Dim outmsg(5) As String
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Cheak_email_mobile_exist")
                db.AddInParameter(dbCommand, "@Act_cd", DbType.[String], marriage_act)


                db.AddInParameter(dbCommand, "@Groom_maritial_status", DbType.[String], groom_mrt_status)
                db.AddInParameter(dbCommand, "@Groom_mobile_no", DbType.[String], groom_mobile_no)
                db.AddInParameter(dbCommand, "@Groom_email_id", DbType.[String], groom_email_id)


                db.AddInParameter(dbCommand, "@Bride_maritial_status", DbType.[String], bride_mrt_status)
                db.AddInParameter(dbCommand, "@Bride_mobile_no", DbType.[String], bride_mobile_no)
                db.AddInParameter(dbCommand, "@Bride_email_id", DbType.[String], bride_email_id)

                db.AddOutParameter(dbCommand, "@email_status", DbType.[String], 10)
                db.AddOutParameter(dbCommand, "@mobile_status", DbType.[String], 10)


                db.ExecuteNonQuery(dbCommand)
                outmsg(0) = Convert.ToString(db.GetParameterValue(dbCommand, "@email_status"))
                outmsg(1) = Convert.ToString(db.GetParameterValue(dbCommand, "@mobile_status"))


            Catch ex As Exception
                outmsg(0) = "0"


            End Try
            Return outmsg
        End Function
#End Region

#Region "Address SAve for outside westbengal"
        Public Function Address_Save(Application_no As String, app_auto_cd As String, state As String, district As String, post_office As String, pin_no As String, IsAddres_Same As String, DateAndTime As String, Updated_PO_For As String, Address_type As String) As Array

            Dim outmsg(3) As String
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("Update_Address_for_Outdide_WB")
                db.AddInParameter(dbCommand, "@Application_no", DbType.[String], Application_no)
                db.AddInParameter(dbCommand, "@app_auto_cd", DbType.[String], app_auto_cd)
                db.AddInParameter(dbCommand, "@state", DbType.[String], state)
                db.AddInParameter(dbCommand, "@district", DbType.[String], district)
                db.AddInParameter(dbCommand, "@post_office", DbType.[String], post_office)
                db.AddInParameter(dbCommand, "@pin_no", DbType.[String], pin_no)
                db.AddInParameter(dbCommand, "@IsAddres_Same", DbType.[String], IsAddres_Same)
                db.AddInParameter(dbCommand, "@DateAndTime", DbType.[String], DateAndTime)
                db.AddInParameter(dbCommand, "@Updated_PO_For", DbType.[String], Updated_PO_For)
                db.AddInParameter(dbCommand, "@Address_type", DbType.[String], Address_type)

                db.AddOutParameter(dbCommand, "@Result_Msg", DbType.[String], 10)
                db.ExecuteNonQuery(dbCommand)
                outmsg(0) = Convert.ToString(db.GetParameterValue(dbCommand, "@Result_Msg"))
            Catch ex As Exception

            End Try
            Return outmsg
        End Function

#End Region

#End Region

    End Class
End Namespace
