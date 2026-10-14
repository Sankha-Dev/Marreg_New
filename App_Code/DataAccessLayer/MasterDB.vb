Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Collections.Generic
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Web.Script.Serialization

Namespace MARREG.App_Code.DataAccessLayer

    Public Class MasterDB

#Region "DECLARATION"
        Public dbconnctn As New DataBaseHelperDB()
        Public dbf As New DBFactoryDB()
#End Region


#Region "District Master Insert Update"

        'Public Function InsertDistrict(district As MasterData) As String
        '    Dim outmsg As String = String.Empty
        '    Try

        '        Dim db As Database = dbf.CreateConnection()
        '        Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_District_Mt_insertupdate")
        '        db.AddInParameter(dbCommand, "@PreDistrict_Cd", DbType.[String], district.District_Cd)
        '        db.AddInParameter(dbCommand, "@State_Name", DbType.[String], district.state_Name)
        '        db.AddInParameter(dbCommand, "@EDistrict_Name", DbType.[String], district.eDistrict_Name)
        '        db.AddInParameter(dbCommand, "@VDistrict_Name", DbType.[String], district.vDistrict_Name)

        '        db.AddInParameter(dbCommand, "@Delete_Status", DbType.[String], district.delete_Status)

        '        db.AddInParameter(dbCommand, "@Entry_Dt", DbType.DateTime, district.entry_Dt)
        '        db.AddInParameter(dbCommand, "@User_Id", DbType.[String], district.user_Id)
        '        db.AddInParameter(dbCommand, "@Ip_Address", DbType.String, district.ip_Address)
        '        db.AddInParameter(dbCommand, "@Tag", DbType.[String], "Insert")
        '        db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
        '        db.ExecuteNonQuery(dbCommand)
        '        outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
        '    Catch ex As Exception

        '        outmsg = ex.Message()
        '    End Try
        '    Return outmsg
        'End Function
#Region "Insert"
        Public Function DistrictSave(District_Cd As String, state_Name As String, eDistrict_Name As String, vDistrict_Name As String, Cen_Distict_cd As String, delete_Status As String, user_Id As String, ip_Address As String) As String
            Dim outmsg As String = String.Empty
            Try

                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_District_Mt_insertupdate")
                db.AddInParameter(dbCommand, "@PreDistrict_Cd", DbType.[String], District_Cd)
                db.AddInParameter(dbCommand, "@State_Name", DbType.[String], state_Name)
                db.AddInParameter(dbCommand, "@EDistrict_Name", DbType.[String], eDistrict_Name)
                db.AddInParameter(dbCommand, "@VDistrict_Name", DbType.[String], vDistrict_Name)
                db.AddInParameter(dbCommand, "@Cen_Distict_cd", DbType.[String], Cen_Distict_cd)
                db.AddInParameter(dbCommand, "@Delete_Status", DbType.[String], delete_Status)


                db.AddInParameter(dbCommand, "@User_Id", DbType.[String], user_Id)
                db.AddInParameter(dbCommand, "@Ip_Address", DbType.String, ip_Address)
                db.AddInParameter(dbCommand, "@Tag", DbType.[String], "Insert")
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
        Public Function DistrictUpdate(District_Cd As String, state_Name As String, eDistrict_Name As String, vDistrict_Name As String, Cen_Distict_cd As String, delete_Status As String, user_Id As String, ip_Address As String) As String
            Dim outmsg As String = String.Empty
            Try

                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_District_Mt_insertupdate")
                db.AddInParameter(dbCommand, "@PreDistrict_Cd", DbType.[String], District_Cd)
                db.AddInParameter(dbCommand, "@State_Name", DbType.[String], state_Name)
                db.AddInParameter(dbCommand, "@EDistrict_Name", DbType.[String], eDistrict_Name)
                db.AddInParameter(dbCommand, "@VDistrict_Name", DbType.[String], vDistrict_Name)
                db.AddInParameter(dbCommand, "@Cen_Distict_cd", DbType.[String], Cen_Distict_cd)
                db.AddInParameter(dbCommand, "@Delete_Status", DbType.[String], delete_Status)


                db.AddInParameter(dbCommand, "@User_Id", DbType.[String], user_Id)
                db.AddInParameter(dbCommand, "@Ip_Address", DbType.String, ip_Address)
                db.AddInParameter(dbCommand, "@Tag", DbType.[String], "Update")
                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception

                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region

#Region "Grid Fill District"
        Public Function Viewdistrict(tabname As String, sate_Cd As String) As DataSet
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim ds = New DataSet()
            Try
                con.Open()
                Dim da As New SqlDataAdapter("MARREG_All_GridView_Load_insertupdate", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure
                'SqlParameter param = new SqlParameter("@flag", 2);
                da.SelectCommand.Parameters.Add(New SqlParameter("@TableName", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@TableName").Value = tabname
                da.SelectCommand.Parameters.Add(New SqlParameter("@State_Cd", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@State_Cd").Value = sate_Cd
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
        Public Function ViewNullity_Application(value As String) As DataSet
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim ds = New DataSet()
            Try
                con.Open()
                Dim da As New SqlDataAdapter("MARREG_All_GridView_Load_nullity", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure
                da.SelectCommand.Parameters.Add(New SqlParameter("@value", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@value").Value = value
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

        Public Function ViewCorrection_Application(value As String, da_cd As String) As DataSet
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim ds = New DataSet()
            Try
                con.Open()
                Dim da As New SqlDataAdapter("MARREG_All_Correction_Request_View", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure
                da.SelectCommand.Parameters.Add(New SqlParameter("@value", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@value").Value = value

                da.SelectCommand.Parameters.Add(New SqlParameter("@da_cd", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@da_cd").Value = da_cd
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

        Public Function allow_correction_application(sl_no As String) As String
            Dim outmsg As String = String.Empty
            Try

                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_correction_allow")
                db.AddInParameter(dbCommand, "@sl_no", DbType.[String], sl_no)

                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception

                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function

#End Region
#End Region



#Region "Sub Division Master Insert Update"

#Region "Insert"
        Public Function SubDivisionSave(State_Cd As String, Sub_Division_Cd As String, District_Cd As String, eESub_Division_Name As String,
                                        vVSub_Division_Name As String, CenSub_Division_Cd As String, delete_Status As String,
                                        user_Id As String, ip_Address As String) As String
            Dim outmsg As String = String.Empty
            Try

                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_SubDivision_Mt_insertupdate")
                db.AddInParameter(dbCommand, "@PreSub_Division_Cd", DbType.[String], Sub_Division_Cd)
                db.AddInParameter(dbCommand, "@State_Cd", DbType.[String], State_Cd)
                db.AddInParameter(dbCommand, "@District_Cd", DbType.[String], District_Cd)
                db.AddInParameter(dbCommand, "@ESub_Division_Name", DbType.[String], eESub_Division_Name)
                db.AddInParameter(dbCommand, "@VSub_Division_Name", DbType.[String], vVSub_Division_Name)
                db.AddInParameter(dbCommand, "@CenSub_Division_Cd", DbType.[String], CenSub_Division_Cd)
                db.AddInParameter(dbCommand, "@DeleteStatus", DbType.[String], delete_Status)


                db.AddInParameter(dbCommand, "@User_Id", DbType.[String], user_Id)
                db.AddInParameter(dbCommand, "@Ip_Address", DbType.String, ip_Address)
                db.AddInParameter(dbCommand, "@Tag", DbType.[String], "Insert")
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
        Public Function SubDivisionUpdate(State_Cd As String, Sub_Division_Cd As String, District_Cd As String, eESub_Division_Name As String,
                                        vVSub_Division_Name As String, CenSub_Division_Cd As String, delete_Status As String,
                                        user_Id As String, ip_Address As String) As String
            Dim outmsg As String = String.Empty
            Try

                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_SubDivision_Mt_insertupdate")
                db.AddInParameter(dbCommand, "@PreSub_Division_Cd", DbType.[String], Sub_Division_Cd)
                db.AddInParameter(dbCommand, "@State_Cd", DbType.[String], State_Cd)
                db.AddInParameter(dbCommand, "@District_Cd", DbType.[String], District_Cd)
                db.AddInParameter(dbCommand, "@ESub_Division_Name", DbType.[String], eESub_Division_Name)
                db.AddInParameter(dbCommand, "@VSub_Division_Name", DbType.[String], vVSub_Division_Name)
                db.AddInParameter(dbCommand, "@CenSub_Division_Cd", DbType.[String], CenSub_Division_Cd)
                db.AddInParameter(dbCommand, "@DeleteStatus", DbType.[String], delete_Status)


                db.AddInParameter(dbCommand, "@User_Id", DbType.[String], user_Id)
                db.AddInParameter(dbCommand, "@Ip_Address", DbType.String, ip_Address)
                db.AddInParameter(dbCommand, "@Tag", DbType.[String], "Update")
                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception

                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region
#End Region

#Region "Block Master Insert Update"
#Region "Insert"
        Public Function BlockSave(Pre_Block_cd As String, sate_cd As String, District_Cd As String, Sub_Division_Cd As String,
                                  Agroclimatic_Zone As String,
                                  eblock_nm As String, vblock_nm As String, Cen_block_cd As String, delete_Status As String,
                                  user_Id As String, ip_Address As String) As String



            Dim outmsg As String = String.Empty
            Try

                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Block_Mt_insertupdate")
                db.AddInParameter(dbCommand, "@Pre_Block_cd", DbType.[String], Pre_Block_cd)
                db.AddInParameter(dbCommand, "@state_cd", DbType.[String], sate_cd)
                db.AddInParameter(dbCommand, "@dist_cd", DbType.[String], District_Cd)

                db.AddInParameter(dbCommand, "@sub_division_cd", DbType.[String], Sub_Division_Cd)
                db.AddInParameter(dbCommand, "@Agroclimatic_Zone", DbType.[String], Agroclimatic_Zone)

                db.AddInParameter(dbCommand, "@eblock_nm", DbType.[String], eblock_nm)

                db.AddInParameter(dbCommand, "@vblock_nm", DbType.[String], vblock_nm)
                db.AddInParameter(dbCommand, "@Cen_block_cd", DbType.[String], Cen_block_cd)
                db.AddInParameter(dbCommand, "@Delete_Status", DbType.[String], delete_Status)

                db.AddInParameter(dbCommand, "@User_id", DbType.String, user_Id)
                db.AddInParameter(dbCommand, "@Ip_Address", DbType.[String], ip_Address)
                db.AddInParameter(dbCommand, "@Tag", DbType.[String], "Insert")
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
        Public Function BlockUpdate(Pre_Block_cd As String, sate_cd As String, District_Cd As String, Sub_Division_Cd As String,
                                         Agroclimatic_Zone As String,
                                         eblock_nm As String, vblock_nm As String, Cen_block_cd As String, delete_Status As String,
                                         user_Id As String, ip_Address As String) As String



            Dim outmsg As String = String.Empty
            Try

                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Block_Mt_insertupdate")
                db.AddInParameter(dbCommand, "@Pre_Block_cd", DbType.[String], Pre_Block_cd)
                db.AddInParameter(dbCommand, "@state_cd", DbType.[String], sate_cd)
                db.AddInParameter(dbCommand, "@dist_cd", DbType.[String], District_Cd)

                db.AddInParameter(dbCommand, "@sub_division_cd", DbType.[String], Sub_Division_Cd)
                db.AddInParameter(dbCommand, "@Agroclimatic_Zone", DbType.[String], Agroclimatic_Zone)

                db.AddInParameter(dbCommand, "@eblock_nm", DbType.[String], eblock_nm)

                db.AddInParameter(dbCommand, "@vblock_nm", DbType.[String], vblock_nm)
                db.AddInParameter(dbCommand, "@Cen_block_cd", DbType.[String], Cen_block_cd)
                db.AddInParameter(dbCommand, "@Delete_Status", DbType.[String], delete_Status)

                db.AddInParameter(dbCommand, "@User_id", DbType.String, user_Id)
                db.AddInParameter(dbCommand, "@Ip_Address", DbType.[String], ip_Address)
                db.AddInParameter(dbCommand, "@Tag", DbType.[String], "Update")
                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception

                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function

#End Region
#End Region



#Region "Mouza Word Master Insert Update"

#Region "Insert"
        Public Function Mouza_Ward_Save(Mouza_word_Cd As String, state_Cd As String, District_Cd As String, Sub_Division_Cd As String, Block_Cd As String,
                                        Gp_Mp_cd As String, E_Name As String,
                                        V_Name As String, Census_cd As String, delete_Status As String,
                                        user_Id As String, ip_Address As String, type As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Ward_Mouza_Mt_insertupdate")
                db.AddInParameter(dbCommand, "@PrewordMouza_Cd", DbType.[String], Mouza_word_Cd)
                db.AddInParameter(dbCommand, "@State_Cd", DbType.[String], state_Cd)
                db.AddInParameter(dbCommand, "@District_Cd", DbType.[String], District_Cd)
                db.AddInParameter(dbCommand, "@sub_division_cd", DbType.[String], Sub_Division_Cd)
                db.AddInParameter(dbCommand, "@Block_cd", DbType.[String], Block_Cd)
                db.AddInParameter(dbCommand, "@gp_MP_cd", DbType.[String], Gp_Mp_cd)
                db.AddInParameter(dbCommand, "@emouza_nm", DbType.[String], E_Name)
                db.AddInParameter(dbCommand, "@vmouza_nm", DbType.[String], V_Name)
                db.AddInParameter(dbCommand, "@Census_cd", DbType.[String], Census_cd)
                db.AddInParameter(dbCommand, "@DeleteStatus", DbType.[String], delete_Status)

                db.AddInParameter(dbCommand, "@User_Id", DbType.[String], user_Id)
                db.AddInParameter(dbCommand, "@Ip_Address", DbType.[String], ip_Address)
                db.AddInParameter(dbCommand, "@Type", DbType.[String], type)

                db.AddInParameter(dbCommand, "@Tag", DbType.[String], "Insert")
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
        Public Function Mouza_Ward_Update(Mouza_word_Cd As String, state_Cd As String, District_Cd As String, Sub_Division_Cd As String, Block_Cd As String,
                                        Gp_Mp_cd As String, E_Name As String,
                                        V_Name As String, Census_cd As String, delete_Status As String,
                                        user_Id As String, ip_Address As String, type As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Ward_Mouza_Mt_insertupdate")
                db.AddInParameter(dbCommand, "@PrewordMouza_Cd", DbType.[String], Mouza_word_Cd)
                db.AddInParameter(dbCommand, "@State_Cd", DbType.[String], state_Cd)
                db.AddInParameter(dbCommand, "@District_Cd", DbType.[String], District_Cd)
                db.AddInParameter(dbCommand, "@sub_division_cd", DbType.[String], Sub_Division_Cd)
                db.AddInParameter(dbCommand, "@Block_cd", DbType.[String], Block_Cd)
                db.AddInParameter(dbCommand, "@gp_MP_cd", DbType.[String], Gp_Mp_cd)
                db.AddInParameter(dbCommand, "@emouza_nm", DbType.[String], E_Name)
                db.AddInParameter(dbCommand, "@vmouza_nm", DbType.[String], V_Name)
                db.AddInParameter(dbCommand, "@Census_cd", DbType.[String], Census_cd)
                db.AddInParameter(dbCommand, "@DeleteStatus", DbType.[String], delete_Status)

                db.AddInParameter(dbCommand, "@User_Id", DbType.[String], user_Id)
                db.AddInParameter(dbCommand, "@Ip_Address", DbType.[String], ip_Address)
                db.AddInParameter(dbCommand, "@Type", DbType.[String], type)

                db.AddInParameter(dbCommand, "@Tag", DbType.[String], "Update")
                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region

#End Region

#Region "Municiplity Gp Master Insert Update"

#Region "Insert"
        Public Function Municipi_Gp_Save(Municipl_Gp_Cd As String, state_Cd As String, District_Cd As String, Sub_Division_Cd As String, Block_Cd As String,
                                         E_Name As String,
                                        V_Name As String, Census_cd As String, delete_Status As String,
                                        user_Id As String, ip_Address As String, type As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Municipal_Gp_Mt_insertupdate")
                db.AddInParameter(dbCommand, "@PreMunicpl_Gp_Cd", DbType.[String], Municipl_Gp_Cd)
                db.AddInParameter(dbCommand, "@State_Cd", DbType.[String], state_Cd)
                db.AddInParameter(dbCommand, "@District_Cd", DbType.[String], District_Cd)
                db.AddInParameter(dbCommand, "@sub_division_cd", DbType.[String], Sub_Division_Cd)
                db.AddInParameter(dbCommand, "@Block_cd", DbType.[String], Block_Cd)
                db.AddInParameter(dbCommand, "@egp_nm", DbType.[String], E_Name)
                db.AddInParameter(dbCommand, "@vgp_nm", DbType.[String], V_Name)
                db.AddInParameter(dbCommand, "@Census_cd", DbType.[String], Census_cd)
                db.AddInParameter(dbCommand, "@DeleteStatus", DbType.[String], delete_Status)

                db.AddInParameter(dbCommand, "@User_Id", DbType.[String], user_Id)
                db.AddInParameter(dbCommand, "@Ip_Address", DbType.[String], ip_Address)
                db.AddInParameter(dbCommand, "@Type", DbType.[String], type)

                db.AddInParameter(dbCommand, "@Tag", DbType.[String], "Insert")
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
        Public Function Municipi_Gp_Update(Municipl_Gp_Cd As String, state_Cd As String, District_Cd As String, Sub_Division_Cd As String, Block_Cd As String,
                                         E_Name As String,
                                        V_Name As String, Census_cd As String, delete_Status As String,
                                        user_Id As String, ip_Address As String, type As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Municipal_Gp_Mt_insertupdate")
                db.AddInParameter(dbCommand, "@PreMunicpl_Gp_Cd", DbType.[String], Municipl_Gp_Cd)
                db.AddInParameter(dbCommand, "@State_Cd", DbType.[String], state_Cd)
                db.AddInParameter(dbCommand, "@District_Cd", DbType.[String], District_Cd)
                db.AddInParameter(dbCommand, "@sub_division_cd", DbType.[String], Sub_Division_Cd)
                db.AddInParameter(dbCommand, "@Block_cd", DbType.[String], Block_Cd)
                db.AddInParameter(dbCommand, "@egp_nm", DbType.[String], E_Name)
                db.AddInParameter(dbCommand, "@vgp_nm", DbType.[String], V_Name)
                db.AddInParameter(dbCommand, "@Census_cd", DbType.[String], Census_cd)
                db.AddInParameter(dbCommand, "@DeleteStatus", DbType.[String], delete_Status)

                db.AddInParameter(dbCommand, "@User_Id", DbType.[String], user_Id)
                db.AddInParameter(dbCommand, "@Ip_Address", DbType.[String], ip_Address)
                db.AddInParameter(dbCommand, "@Type", DbType.[String], type)

                db.AddInParameter(dbCommand, "@Tag", DbType.[String], "Update")
                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region
#End Region

#Region "PO Master Insert Update"

#Region "Insert"
        Public Function Po_Save(Pre_PO_cd As String, State_cd As String, District_Cd As String, epo_nm As String, vpo_nm As String, Pin_No As String, category As String,
                               DeleteStatus As String, user_Id As String, ip_Address As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Po_insertupdate")
                db.AddInParameter(dbCommand, "@Pre_PO_cd", DbType.[String], Pre_PO_cd)
                db.AddInParameter(dbCommand, "@State_cd", DbType.[String], State_cd)
                db.AddInParameter(dbCommand, "@District_Cd", DbType.[String], District_Cd)
                db.AddInParameter(dbCommand, "@epo_nm", DbType.[String], epo_nm)
                db.AddInParameter(dbCommand, "@vpo_nm", DbType.[String], vpo_nm)
                db.AddInParameter(dbCommand, "@Pin_No", DbType.[String], Pin_No)
                db.AddInParameter(dbCommand, "@category", DbType.[String], category)
                db.AddInParameter(dbCommand, "@Delete_Status", DbType.[String], DeleteStatus)

                db.AddInParameter(dbCommand, "@User_id", DbType.String, user_Id)
                db.AddInParameter(dbCommand, "@Ip_Address", DbType.[String], ip_Address)
                db.AddInParameter(dbCommand, "@Tag", DbType.[String], "Insert")
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
        Public Function Po_Update(Pre_PO_cd As String, State_cd As String, District_Cd As String, epo_nm As String, vpo_nm As String, Pin_No As String, category As String,
                               DeleteStatus As String, user_Id As String, ip_Address As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Po_insertupdate")
                db.AddInParameter(dbCommand, "@Pre_PO_cd", DbType.[String], Pre_PO_cd)
                db.AddInParameter(dbCommand, "@State_cd", DbType.[String], State_cd)
                db.AddInParameter(dbCommand, "@District_Cd", DbType.[String], District_Cd)
                db.AddInParameter(dbCommand, "@epo_nm", DbType.[String], epo_nm)
                db.AddInParameter(dbCommand, "@vpo_nm", DbType.[String], vpo_nm)
                db.AddInParameter(dbCommand, "@Pin_No", DbType.[String], Pin_No)
                db.AddInParameter(dbCommand, "@category", DbType.[String], category)
                db.AddInParameter(dbCommand, "@Delete_Status", DbType.[String], DeleteStatus)

                db.AddInParameter(dbCommand, "@User_id", DbType.String, user_Id)
                db.AddInParameter(dbCommand, "@Ip_Address", DbType.[String], ip_Address)
                db.AddInParameter(dbCommand, "@Tag", DbType.[String], "Update")
                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region

#End Region

#Region "PS Master Insert Update"

#Region "Insert"
        Public Function PS_Save(Pre_PS_cd As String, State_cd As String, District_Cd As String, eps_nm As String, vps_nm As String,
                               DeleteStatus As String, user_Id As String, ip_Address As String, Sub_Division_Cd As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_PS_insertupdate")
                db.AddInParameter(dbCommand, "@Pre_PS_cd", DbType.[String], Pre_PS_cd)
                db.AddInParameter(dbCommand, "@State_cd", DbType.[String], State_cd)
                db.AddInParameter(dbCommand, "@District_Cd", DbType.[String], District_Cd)
                db.AddInParameter(dbCommand, "@eps_nm", DbType.[String], eps_nm)
                db.AddInParameter(dbCommand, "@vps_nm", DbType.[String], vps_nm)
                db.AddInParameter(dbCommand, "@DeleteStatus", DbType.[String], DeleteStatus)

                db.AddInParameter(dbCommand, "@User_id", DbType.String, user_Id)
                db.AddInParameter(dbCommand, "@Ip_Address", DbType.[String], ip_Address)
                db.AddInParameter(dbCommand, "@Sub_Division_Cd", DbType.[String], Sub_Division_Cd)
                db.AddInParameter(dbCommand, "@Tag", DbType.[String], "Insert")
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
        Public Function Ps_Update(Pre_PS_cd As String, State_cd As String, District_Cd As String, eps_nm As String, vps_nm As String,
                               DeleteStatus As String, user_Id As String, ip_Address As String, Sub_Division_Cd As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_PS_insertupdate")
                db.AddInParameter(dbCommand, "@Pre_PS_cd", DbType.[String], Pre_PS_cd)
                db.AddInParameter(dbCommand, "@State_cd", DbType.[String], State_cd)
                db.AddInParameter(dbCommand, "@District_Cd", DbType.[String], District_Cd)
                db.AddInParameter(dbCommand, "@eps_nm", DbType.[String], eps_nm)
                db.AddInParameter(dbCommand, "@vps_nm", DbType.[String], vps_nm)
                db.AddInParameter(dbCommand, "@DeleteStatus", DbType.[String], DeleteStatus)

                db.AddInParameter(dbCommand, "@User_id", DbType.String, user_Id)
                db.AddInParameter(dbCommand, "@Ip_Address", DbType.[String], ip_Address)
                db.AddInParameter(dbCommand, "@Sub_Division_Cd", DbType.[String], Sub_Division_Cd)
                db.AddInParameter(dbCommand, "@Tag", DbType.[String], "Update")
                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region

#End Region

#Region "EMPLOYEE Master Insert Update"
#Region "Insert/ Update"
        Public Function Employee_Details_Save_Update(emp_Cd As Int32, EmployeeType As Int32, EmployeePost As Int32, EmployeeName As String, EmployeePnF As String, EmployeePayLevel As String, EmployeeOrder As String, EmployeeStatus As String, state As String, flag As Int16) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Employee_Master")
                db.AddInParameter(dbCommand, "@e_Cd", DbType.Int32, emp_Cd)
                db.AddInParameter(dbCommand, "@e_type", DbType.Int32, EmployeeType)
                db.AddInParameter(dbCommand, "@e_post", DbType.Int32, EmployeePost)
                db.AddInParameter(dbCommand, "@e_name", DbType.[String], EmployeeName)
                db.AddInParameter(dbCommand, "@e_pnf", DbType.[String], EmployeePnF)
                db.AddInParameter(dbCommand, "@e_pl", DbType.[String], EmployeePayLevel)
                db.AddInParameter(dbCommand, "@e_ord", DbType.[String], EmployeeOrder)
                db.AddInParameter(dbCommand, "@e_sts", DbType.[String], EmployeeStatus)
                db.AddInParameter(dbCommand, "@state", DbType.[String], state)

                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region
#End Region

#Region "RTI Authority Master Insert Update"
#Region "Insert"
        Public Function RTIAuth_Details_Save_Update(rtiauth_cd As String, rtiauthPost As String, rtiauthDesg As String, rtiauthName As String, rtiaStatus As String, state As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_RTIAuth_Master")
                db.AddInParameter(dbCommand, "@rtia_cd", DbType.[String], rtiauth_cd)
                db.AddInParameter(dbCommand, "@rtia_post", DbType.[String], rtiauthPost)
                db.AddInParameter(dbCommand, "@rtia_desg", DbType.[String], rtiauthDesg)
                db.AddInParameter(dbCommand, "@rtia_name", DbType.[String], rtiauthName)
                db.AddInParameter(dbCommand, "@rtia_sts", DbType.[String], rtiaStatus)
                db.AddInParameter(dbCommand, "@state", DbType.[String], state)
                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region

#End Region

#Region "Portal Feedback Activity Logs"
#Region "Insert"
        Public Function PortalFeedbackActivityLogsSaveDB(user_ip As String, browser_agent As String, response As Int32) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_PortalFeedback_ActivityLog")
                db.AddInParameter(dbCommand, "@ip", DbType.[String], user_ip)
                db.AddInParameter(dbCommand, "@agent", DbType.[String], browser_agent)
                db.AddInParameter(dbCommand, "@response", DbType.Int32, response)

                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region

#End Region

#Region "Declaration Wipping Bio-Metrics Master Insert"
#Region "Insert/ Update"
        Public Function DeclarationWBM(mo_cd As Int32, app_count As Int32, emailbody As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_SubmitDeclaration_DataSecurity")
                db.AddInParameter(dbCommand, "@mo_cd", DbType.Int32, mo_cd)
                db.AddInParameter(dbCommand, "@app_count", DbType.Int32, app_count)
                db.AddInParameter(dbCommand, "@email_body", DbType.String, emailbody)

                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region
#End Region

#Region "Application Acceptance Save"
#Region "Insert"
        Public Function AA_Save(draft_cd As String, status As String, remarks As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Save_ApplicationAcceptance")
                db.AddInParameter(dbCommand, "@draft_cd", DbType.[String], draft_cd)
                db.AddInParameter(dbCommand, "@status", DbType.[String], status)
                db.AddInParameter(dbCommand, "@remarks", DbType.String, remarks)

                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region

#End Region

#Region "INDEX DATA INSERT UPDATE "
#Region "Insert/ Update"
        Public Function Index_Data_Save_Update(dmtCd As String, mo_cd As String, mg_type As String, b_type As String, b_slno As String, fromdate As String, todate As String, bgno As String, remarks As String, state As String, flag As Int16) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("INDEX_DATAMASTER_InsertUpdate")
                db.AddInParameter(dbCommand, "@dmt_cd", DbType.Int32, dmtCd)
                db.AddInParameter(dbCommand, "@mo_cd", DbType.Int32, mo_cd)
                db.AddInParameter(dbCommand, "@marriage_cd", DbType.Int32, mg_type)
                db.AddInParameter(dbCommand, "@dpb_cd", DbType.Int32, b_type)
                db.AddInParameter(dbCommand, "@sl_no", DbType.[String], b_slno)
                db.AddInParameter(dbCommand, "@from_date", DbType.[String], fromdate)
                db.AddInParameter(dbCommand, "@to_date", DbType.[String], todate)
                db.AddInParameter(dbCommand, "@bag_no", DbType.[String], bgno)
                db.AddInParameter(dbCommand, "@remarks", DbType.[String], remarks)
                db.AddInParameter(dbCommand, "@state", DbType.[String], state)

                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region

#End Region

#Region "Church Master Insert Update"

#Region "Insert"
        Public Function Church_Name_Save(church_Cd As String, District_Cd As String, Sub_Division_Cd As String, ps_Cd As String, c_name As String, type As String, state As String, flag As Int16) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_ChurchName_Master")
                db.AddInParameter(dbCommand, "@c_Cd", DbType.[String], church_Cd)
                db.AddInParameter(dbCommand, "@District_Cd", DbType.[String], District_Cd)
                db.AddInParameter(dbCommand, "@sub_division_cd", DbType.[String], Sub_Division_Cd)
                db.AddInParameter(dbCommand, "@PS_cd", DbType.[String], ps_Cd)
                db.AddInParameter(dbCommand, "@c_name", DbType.[String], c_name)

                db.AddInParameter(dbCommand, "@Type", DbType.[String], type)
                db.AddInParameter(dbCommand, "@state", DbType.[String], state)

                db.AddInParameter(dbCommand, "@Tag", DbType.[String], "Insert")
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
        Public Function Church_Name_Update(church_Cd As String, District_Cd As String, Sub_Division_Cd As String, ps_Cd As String, c_name As String, type As String, state As String, flag As Int16) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_ChurchName_Master")
                db.AddInParameter(dbCommand, "@c_Cd", DbType.[String], church_Cd)
                db.AddInParameter(dbCommand, "@District_Cd", DbType.[String], District_Cd)
                db.AddInParameter(dbCommand, "@sub_division_cd", DbType.[String], Sub_Division_Cd)
                db.AddInParameter(dbCommand, "@PS_cd", DbType.[String], ps_Cd)
                db.AddInParameter(dbCommand, "@c_name", DbType.[String], c_name)

                db.AddInParameter(dbCommand, "@Type", DbType.[String], type)
                db.AddInParameter(dbCommand, "@state", DbType.[String], state)

                db.AddInParameter(dbCommand, "@Tag", DbType.[String], "Update")
                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region


#Region "Insert"
        Public Function Question_Name_SaveUpdate(ques_cd As String, ques_en As String, ques_bn As String, ques_weight As String, type As String, state As String, flag As Int16, job As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Question_Master")
                db.AddInParameter(dbCommand, "@ques_cd", DbType.[String], ques_cd)
                db.AddInParameter(dbCommand, "@ques_en", DbType.[String], ques_en)
                db.AddInParameter(dbCommand, "@ques_bn", DbType.[String], ques_bn)
                db.AddInParameter(dbCommand, "@ques_weight", DbType.[String], ques_weight)

                db.AddInParameter(dbCommand, "@state", DbType.[String], state)

                db.AddInParameter(dbCommand, "@Tag", DbType.[String], job)
                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region

#Region "Insert"
        Public Function Answer_Name_SaveUpdate(ans_cd As String, ques_cd As Int16, ans_en As String, ans_bn As String, ans_mark As String, type As String, state As String, flag As Int16, job As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Answer_Master")
                db.AddInParameter(dbCommand, "@ans_cd", DbType.[String], ans_cd)
                db.AddInParameter(dbCommand, "@ques_cd", DbType.[String], ques_cd)
                db.AddInParameter(dbCommand, "@ans_en", DbType.[String], ans_en)
                db.AddInParameter(dbCommand, "@ans_bn", DbType.[String], ans_bn)
                db.AddInParameter(dbCommand, "@ans_mark", DbType.[String], ans_mark)

                db.AddInParameter(dbCommand, "@state", DbType.[String], state)

                db.AddInParameter(dbCommand, "@Tag", DbType.[String], job)
                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region

#Region "Office Details Master Insert Update"
#Region "Insert"
        Public Function Office_Address_Save(Pre_ofc_cd As String, State_cd As String, District_Cd As String, Sub_Div_Cd As String, address As String, lt_deg As String, lt_nrt As String,
                                            ln_deg As String, ln_est As String, DeleteStatus As String, user_Id As String, ip_Address As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Office_Details_insertupdate")
                db.AddInParameter(dbCommand, "@Pre_office_cd", DbType.[String], Pre_ofc_cd)
                db.AddInParameter(dbCommand, "@State_cd", DbType.[String], State_cd)
                db.AddInParameter(dbCommand, "@District_Cd", DbType.[String], District_Cd)
                db.AddInParameter(dbCommand, "@Sub_Division_cd", DbType.[String], Sub_Div_Cd)
                db.AddInParameter(dbCommand, "@address", DbType.[String], address)
                db.AddInParameter(dbCommand, "@latitude", DbType.[String], lt_deg)
                db.AddInParameter(dbCommand, "@noth", DbType.[String], lt_nrt)
                db.AddInParameter(dbCommand, "@longitude", DbType.[String], ln_deg)
                db.AddInParameter(dbCommand, "@east", DbType.[String], ln_est)
                db.AddInParameter(dbCommand, "@DeleteStatus", DbType.[String], DeleteStatus)

                db.AddInParameter(dbCommand, "@User_id", DbType.String, user_Id)
                db.AddInParameter(dbCommand, "@Ip_Address", DbType.[String], ip_Address)
                db.AddInParameter(dbCommand, "@Tag", DbType.[String], "Insert")
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
        Public Function Office_Address_Update(Pre_ofc_cd As String, State_cd As String, District_Cd As String, Sub_Div_Cd As String, address As String, lt_deg As String, lt_nrt As String,
                                              ln_deg As String, ln_est As String, DeleteStatus As String, user_Id As String, ip_Address As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Office_Details_insertupdate")
                db.AddInParameter(dbCommand, "@Pre_office_cd", DbType.[String], Pre_ofc_cd)
                db.AddInParameter(dbCommand, "@State_cd", DbType.[String], State_cd)
                db.AddInParameter(dbCommand, "@District_Cd", DbType.[String], District_Cd)
                db.AddInParameter(dbCommand, "@Sub_Division_cd", DbType.[String], Sub_Div_Cd)
                db.AddInParameter(dbCommand, "@address", DbType.[String], address)
                db.AddInParameter(dbCommand, "@latitude", DbType.[String], lt_deg)
                db.AddInParameter(dbCommand, "@noth", DbType.[String], lt_nrt)
                db.AddInParameter(dbCommand, "@longitude", DbType.[String], ln_deg)
                db.AddInParameter(dbCommand, "@east", DbType.[String], ln_est)
                db.AddInParameter(dbCommand, "@DeleteStatus", DbType.[String], DeleteStatus)

                db.AddInParameter(dbCommand, "@User_id", DbType.String, user_Id)
                db.AddInParameter(dbCommand, "@Ip_Address", DbType.[String], ip_Address)
                db.AddInParameter(dbCommand, "@Tag", DbType.[String], "Update")
                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region

#End Region

#Region "Qualification Master Insert Update"
#Region "Insert"
        Public Function Qualification_Save(Pre_qual_cd As String, qual_desc As String, user_Id As String, ip_Address As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Qualification_Mt_insertupdate")
                db.AddInParameter(dbCommand, "@Pre_qual_cd", DbType.[String], Pre_qual_cd)
                db.AddInParameter(dbCommand, "@qual_desc", DbType.[String], qual_desc)
                'db.AddInParameter(dbCommand, "@DeleteStatus", DbType.[String], DeleteStatus)

                db.AddInParameter(dbCommand, "@User_id", DbType.String, user_Id)
                db.AddInParameter(dbCommand, "@Ip_Address", DbType.[String], ip_Address)
                db.AddInParameter(dbCommand, "@Tag", DbType.[String], "Insert")
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
        Public Function Qualification_Update(Pre_qual_cd As String, qual_desc As String, user_Id As String, ip_Address As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Qualification_Mt_insertupdate")
                db.AddInParameter(dbCommand, "@Pre_qual_Cd", DbType.[String], Pre_qual_cd)
                db.AddInParameter(dbCommand, "@qual_desc", DbType.[String], qual_desc)
                'db.AddInParameter(dbCommand, "@DeleteStatus", DbType.[String], DeleteStatus)

                db.AddInParameter(dbCommand, "@User_id", DbType.String, user_Id)
                db.AddInParameter(dbCommand, "@Ip_Address", DbType.[String], ip_Address)
                db.AddInParameter(dbCommand, "@Tag", DbType.[String], "Update")
                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region

#End Region

#Region "MO Details Insert Update"

#Region "Insert"
        Public Function MO_Details_Save(Pre_MO_Autoincrmnt_Cd As String, State_cd As String, District_Cd As String, sub_division_cd As String, PS_cd As String, work_area_cd As String,
                                            Block_Cd As String, Muni_GP_Cd As String, Ward_Mouza_Cd As String, MO_Type_Id As String, MO_FName As String, MO_MName As String, MO_LName As String, MO_Designation As String, MO_office_addr As String,
                                            MO_resi_addr As String, MO_office_po_dist As String, MO_office_po_cd As String, MO_resi_po_dist As String, MO_resi_po_cd As String, MO_MPh_no As String, MO_LPh_no As String, Joining_DT As String, Go_No_NOMO As String, Go_Dt_NOMO As String,
                                            ctgry As String, Email_Id As String, User_Email As String, DOB As String, Ai_Id As String, PAN_No As String,
                                            Bank_cd As String, Bank_Branch_District_Cd As String, Bank_branch_cd As String, Bank_acno As String, Sex As String, Marital_status As String, DeleteStatus As String,
                                             user_Id As String, ip_Address As String, EPIC_no As String, Qulification_cd As String, Marks As String, MO_Imagename As Byte(), Qulification_Docmnt_name As Byte(),
                                            Signature As Byte(), juris_levl_cd As String, juris_levl As String, resi_district_cd As String, MO_fthr_Name As String, status_flg As String) As Array
            Dim outmsg(1) As String
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_MO_insertupdate")
                db.AddInParameter(dbCommand, "@Pre_MO_Autoincrmnt_Cd", DbType.[String], Pre_MO_Autoincrmnt_Cd)
                'db.AddInParameter(dbCommand, "@Pre_MO_cd", DbType.[String], Pre_MO_cd)
                db.AddInParameter(dbCommand, "@State_cd", DbType.[String], State_cd)
                db.AddInParameter(dbCommand, "@District_Cd", DbType.[String], District_Cd)
                db.AddInParameter(dbCommand, "@sub_division_cd", DbType.[String], sub_division_cd)
                db.AddInParameter(dbCommand, "@PS_cd", DbType.[String], PS_cd)
                db.AddInParameter(dbCommand, "@work_area_cd", DbType.[String], work_area_cd)
                db.AddInParameter(dbCommand, "@Block_cd", DbType.[String], Block_Cd)
                db.AddInParameter(dbCommand, "@Muni_GP_cd", DbType.[String], Muni_GP_Cd)
                db.AddInParameter(dbCommand, "@Ward_Mouza_cd", DbType.[String], Ward_Mouza_Cd)
                db.AddInParameter(dbCommand, "@Mo_Type_Id", DbType.[String], MO_Type_Id)
                db.AddInParameter(dbCommand, "@MO_FName", DbType.[String], MO_FName)
                db.AddInParameter(dbCommand, "@MO_MName", DbType.[String], MO_MName)
                db.AddInParameter(dbCommand, "@MO_LName", DbType.[String], MO_LName)
                db.AddInParameter(dbCommand, "@MO_Designation", DbType.[String], MO_Designation)
                db.AddInParameter(dbCommand, "@MO_office_addr", DbType.[String], MO_office_addr)
                db.AddInParameter(dbCommand, "@MO_resi_addr", DbType.[String], MO_resi_addr)
                db.AddInParameter(dbCommand, "@MO_Office_PO_Dist", DbType.[String], MO_office_po_dist) '21052018
                db.AddInParameter(dbCommand, "@MO_Office_PinCode", DbType.[String], MO_office_po_cd)
                db.AddInParameter(dbCommand, "@MO_Resi_PO_Dist", DbType.[String], MO_resi_po_dist) '21052018
                db.AddInParameter(dbCommand, "@MO_Resi_PinCode", DbType.[String], MO_resi_po_cd)
                db.AddInParameter(dbCommand, "@MO_MPh_no", DbType.[String], MO_MPh_no)
                db.AddInParameter(dbCommand, "@MO_LPh_no", DbType.[String], MO_LPh_no)
                db.AddInParameter(dbCommand, "@Joining_DT", DbType.[String], Joining_DT)
                db.AddInParameter(dbCommand, "@Go_No_NOMO", DbType.[String], Go_No_NOMO)
                db.AddInParameter(dbCommand, "@Go_Dt_NOMO", DbType.[String], Go_Dt_NOMO)

                db.AddInParameter(dbCommand, "@Category", DbType.[String], ctgry)
                db.AddInParameter(dbCommand, "@Email_Id", DbType.[String], Email_Id)
                db.AddInParameter(dbCommand, "@User_Email_Id", DbType.[String], User_Email)
                db.AddInParameter(dbCommand, "@DOB", DbType.[String], DOB)
                db.AddInParameter(dbCommand, "@Ai_Id", DbType.[String], Ai_Id)
                db.AddInParameter(dbCommand, "@PAN_No", DbType.[String], PAN_No)
                'db.AddInParameter(dbCommand, "@Working_days", DbType.[String], work_day)
                'db.AddInParameter(dbCommand, "@Working_From_Hours", DbType.[String], work_hrs_from)
                'db.AddInParameter(dbCommand, "@Working_To_Hours", DbType.[String], work_hrs_to)
                db.AddInParameter(dbCommand, "@Bank_cd", DbType.[String], Bank_cd)
                db.AddInParameter(dbCommand, "@Bank_brn_dist_cd", DbType.[String], Bank_Branch_District_Cd)
                db.AddInParameter(dbCommand, "@Bank_brn_cd", DbType.[String], Bank_branch_cd)
                db.AddInParameter(dbCommand, "@Bank_Ac_no", DbType.[String], Bank_acno)
                db.AddInParameter(dbCommand, "@Sex", DbType.[String], Sex)
                db.AddInParameter(dbCommand, "@Marital_Status", DbType.[String], Marital_status)
                db.AddInParameter(dbCommand, "@DeleteStatus", DbType.[String], DeleteStatus)

                db.AddInParameter(dbCommand, "@User_id", DbType.[String], user_Id)
                db.AddInParameter(dbCommand, "@Ip_Address", DbType.[String], ip_Address)
                db.AddInParameter(dbCommand, "@epic_no", DbType.[String], EPIC_no)
                db.AddInParameter(dbCommand, "@Qulification_cd", DbType.[String], Qulification_cd)
                db.AddInParameter(dbCommand, "@Marks", DbType.[String], Marks)
                db.AddInParameter(dbCommand, "@MO_image", DbType.Binary, MO_Imagename)
                db.AddInParameter(dbCommand, "@Qulification_Docmnt", DbType.Binary, Qulification_Docmnt_name)
                db.AddInParameter(dbCommand, "@MO_Signature", DbType.Binary, Signature)
                db.AddInParameter(dbCommand, "@Jurisdctn_level_cd", DbType.[String], juris_levl_cd)
                db.AddInParameter(dbCommand, "@Jurisdctn_level", DbType.[String], juris_levl)
                db.AddInParameter(dbCommand, "@Tag", DbType.[String], "Insert")

                db.AddInParameter(dbCommand, "@Sts_Flag", DbType.[String], status_flg) '26.08.16 anita
                '23_05_16 anita
                db.AddInParameter(dbCommand, "@MO_resi_dist_cd", DbType.[String], resi_district_cd)
                db.AddInParameter(dbCommand, "@MO_Father_Name", DbType.[String], MO_fthr_Name)
                'end anita
                db.AddOutParameter(dbCommand, "@MO_total_cd", DbType.[String], 50)
                db.AddOutParameter(dbCommand, "@MOAutoincrmntCd", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg(0) = Convert.ToString(db.GetParameterValue(dbCommand, "@MO_total_cd"))
                outmsg(1) = Convert.ToString(db.GetParameterValue(dbCommand, "@MOAutoincrmntCd"))
            Catch ex As Exception
                outmsg(0) = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region

#Region "Update"
        Public Function MO_Details_Update(Pre_MO_Autoincrmnt_Cd As String, State_cd As String, District_Cd As String, sub_division_cd As String, PS_cd As String, work_area_cd As String,
                                          Block_Cd As String, Muni_GP_Cd As String, Ward_Mouza_Cd As String, MO_Type_Id As String, MO_FName As String, MO_MName As String, MO_LName As String, MO_Designation As String, MO_office_addr As String,
                                          MO_resi_addr As String, MO_office_po_dist As String, MO_office_po_cd As String, MO_resi_po_dist As String, MO_resi_po_cd As String, MO_MPh_no As String, MO_LPh_no As String, Joining_DT As String, Go_No_NOMO As String, Go_Dt_NOMO As String,
                                          ctgry As String, Email_Id As String, User_Email As String, DOB As String, Ai_Id As String, PAN_No As String,
                                          Bank_cd As String, Bank_Branch_District_Cd As String, Bank_branch_cd As String, Bank_acno As String, Sex As String, Marital_status As String, DeleteStatus As String,
                                          user_Id As String, ip_Address As String, EPIC_no As String, Qulification_cd As String, Marks As String, MO_Imagename As Byte(), Qulification_Docmnt_name As Byte(),
                                          Signature As Byte(), juris_levl_cd As String, juris_levl As String, resi_district_cd As String, MO_fthr_Name As String, status_flg As String) As Array
            Dim outmsg(2) As String
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_MO_insertupdate")
                db.AddInParameter(dbCommand, "@Pre_MO_Autoincrmnt_Cd", DbType.[String], Pre_MO_Autoincrmnt_Cd)
                'db.AddInParameter(dbCommand, "@Pre_MO_cd", DbType.[String], Pre_MO_cd)
                db.AddInParameter(dbCommand, "@State_cd", DbType.[String], State_cd)
                db.AddInParameter(dbCommand, "@District_Cd", DbType.[String], District_Cd)
                db.AddInParameter(dbCommand, "@sub_division_cd", DbType.[String], sub_division_cd)
                db.AddInParameter(dbCommand, "@PS_cd", DbType.[String], PS_cd)
                db.AddInParameter(dbCommand, "@work_area_cd", DbType.[String], work_area_cd)
                db.AddInParameter(dbCommand, "@Block_cd", DbType.[String], Block_Cd)
                db.AddInParameter(dbCommand, "@Muni_GP_cd", DbType.[String], Muni_GP_Cd)
                db.AddInParameter(dbCommand, "@Ward_Mouza_cd", DbType.[String], Ward_Mouza_Cd)
                db.AddInParameter(dbCommand, "@Mo_Type_Id", DbType.[String], MO_Type_Id)
                db.AddInParameter(dbCommand, "@MO_FName", DbType.[String], MO_FName)
                db.AddInParameter(dbCommand, "@MO_MName", DbType.[String], MO_MName)
                db.AddInParameter(dbCommand, "@MO_LName", DbType.[String], MO_LName)
                db.AddInParameter(dbCommand, "@MO_Designation", DbType.[String], MO_Designation)
                db.AddInParameter(dbCommand, "@MO_office_addr", DbType.[String], MO_office_addr)
                db.AddInParameter(dbCommand, "@MO_resi_addr", DbType.[String], MO_resi_addr)
                db.AddInParameter(dbCommand, "@MO_Office_PO_Dist", DbType.[String], MO_office_po_dist) '21052018
                db.AddInParameter(dbCommand, "@MO_Office_PinCode", DbType.[String], MO_office_po_cd)
                db.AddInParameter(dbCommand, "@MO_Resi_PO_Dist", DbType.[String], MO_resi_po_dist) '21052018
                db.AddInParameter(dbCommand, "@MO_Resi_PinCode", DbType.[String], MO_resi_po_cd)
                db.AddInParameter(dbCommand, "@MO_MPh_no", DbType.[String], MO_MPh_no)
                db.AddInParameter(dbCommand, "@MO_LPh_no", DbType.[String], MO_LPh_no)
                db.AddInParameter(dbCommand, "@Joining_DT", DbType.[String], Joining_DT)
                db.AddInParameter(dbCommand, "@Go_No_NOMO", DbType.[String], Go_No_NOMO)
                db.AddInParameter(dbCommand, "@Go_Dt_NOMO", DbType.[String], Go_Dt_NOMO)

                db.AddInParameter(dbCommand, "@Category", DbType.[String], ctgry)
                db.AddInParameter(dbCommand, "@Email_Id", DbType.[String], Email_Id)
                db.AddInParameter(dbCommand, "@User_Email_Id", DbType.[String], User_Email)
                db.AddInParameter(dbCommand, "@DOB", DbType.[String], DOB)
                db.AddInParameter(dbCommand, "@Ai_Id", DbType.[String], Ai_Id)
                db.AddInParameter(dbCommand, "@PAN_No", DbType.[String], PAN_No)
                'db.AddInParameter(dbCommand, "@Working_days", DbType.[String], work_day)
                'db.AddInParameter(dbCommand, "@Working_From_Hours", DbType.[String], work_hrs_from)
                'db.AddInParameter(dbCommand, "@Working_To_Hours", DbType.[String], work_hrs_to)
                db.AddInParameter(dbCommand, "@Bank_cd", DbType.[String], Bank_cd)
                db.AddInParameter(dbCommand, "@Bank_brn_dist_cd", DbType.[String], Bank_Branch_District_Cd)
                db.AddInParameter(dbCommand, "@Bank_brn_cd", DbType.[String], Bank_branch_cd)
                db.AddInParameter(dbCommand, "@Bank_Ac_no", DbType.[String], Bank_acno)
                db.AddInParameter(dbCommand, "@Sex", DbType.[String], Sex)
                db.AddInParameter(dbCommand, "@Marital_Status", DbType.[String], Marital_status)
                db.AddInParameter(dbCommand, "@DeleteStatus", DbType.[String], DeleteStatus)

                db.AddInParameter(dbCommand, "@User_id", DbType.[String], user_Id)
                db.AddInParameter(dbCommand, "@Ip_Address", DbType.[String], ip_Address)
                db.AddInParameter(dbCommand, "@epic_no", DbType.[String], EPIC_no)
                db.AddInParameter(dbCommand, "@Qulification_cd", DbType.[String], Qulification_cd)
                db.AddInParameter(dbCommand, "@Marks", DbType.[String], Marks)
                db.AddInParameter(dbCommand, "@MO_image", DbType.Binary, MO_Imagename)
                db.AddInParameter(dbCommand, "@Qulification_Docmnt", DbType.Binary, Qulification_Docmnt_name)
                db.AddInParameter(dbCommand, "@MO_Signature", DbType.Binary, Signature)
                db.AddInParameter(dbCommand, "@Jurisdctn_level_cd", DbType.[String], juris_levl_cd)
                db.AddInParameter(dbCommand, "@Jurisdctn_level", DbType.[String], juris_levl)
                db.AddInParameter(dbCommand, "@Tag", DbType.[String], "Update")
                db.AddInParameter(dbCommand, "@Sts_Flag", DbType.[String], status_flg) '26.08.16 anita
                '23_05_16
                db.AddInParameter(dbCommand, "@MO_resi_dist_cd", DbType.[String], resi_district_cd)
                db.AddInParameter(dbCommand, "@MO_Father_Name", DbType.[String], MO_fthr_Name)

                db.AddOutParameter(dbCommand, "@MO_total_cd", DbType.[String], 50)
                db.AddOutParameter(dbCommand, "@MOAutoincrmntCd", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg(0) = Convert.ToString(db.GetParameterValue(dbCommand, "@MO_total_cd"))
            Catch ex As Exception
                outmsg(0) = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region

#Region "MO_workingdays_hour"
        Public Function MO_workingdays_hour(Pre_MO_Autoincrmnt_Cd As String, wrk_dyHrs As DataTable, flag As Int16) As String
            Dim outmsg As String = String.Empty
            Dim op As String
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim con As New SqlConnection(dbconnctn.GetConnectionString())
                Dim insertCommand As SqlCommand = New SqlCommand("MARREG_MO_workingdays_hour", con)
                insertCommand.CommandType = CommandType.StoredProcedure

                Dim mo_cd As SqlParameter = insertCommand.Parameters.AddWithValue("@MO_Autoincrmnt_Cd", Pre_MO_Autoincrmnt_Cd)
                mo_cd.SqlDbType = SqlDbType.NVarChar

                Dim wrkg_hrs As SqlParameter = insertCommand.Parameters.AddWithValue("@MO_WrkDay", wrk_dyHrs)
                wrkg_hrs.SqlDbType = SqlDbType.Structured

                If flag = 1 Then
                    op = "Insert"
                Else
                    op = "Update"
                End If

                Dim tag As SqlParameter = insertCommand.Parameters.AddWithValue("@Tag", op)
                tag.SqlDbType = SqlDbType.NVarChar

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
#End Region

#Region "MO usercreation"
        Public Function updatemembership(pwd As String, userid As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("update_user_membership")
                db.AddInParameter(dbCommand, "@password", DbType.[String], pwd)
                db.AddInParameter(dbCommand, "@userid", DbType.[String], userid)
                db.ExecuteNonQuery(dbCommand)
                outmsg = "update"
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
        Public Function Mo_create_user(roleid As String, userid As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("user_creation_for_marreg")
                db.AddInParameter(dbCommand, "@rolename", DbType.[String], roleid)
                db.AddInParameter(dbCommand, "@username", DbType.[String], userid)
                db.ExecuteNonQuery(dbCommand)
                outmsg = "saved"
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
        Public Function creatuser(UserName As String, state_cd As String, district_cd As String, Sub_division_cd As String, block As String,
                                  Designation As String, Ph_no As String, email As String, Mo_cd As String, Name As String, DOB As String, valid_from As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Create_User")
                db.AddInParameter(dbCommand, "@UserName", DbType.[String], UserName)
                db.AddInParameter(dbCommand, "@state_cd", DbType.[String], state_cd)
                db.AddInParameter(dbCommand, "@district_cd", DbType.[String], district_cd)
                db.AddInParameter(dbCommand, "@Sub_division_cd", DbType.[String], Sub_division_cd)
                db.AddInParameter(dbCommand, "@block_cd", DbType.[String], block)

                db.AddInParameter(dbCommand, "@Designation", DbType.[String], Designation)
                db.AddInParameter(dbCommand, "@Ph_no", DbType.[String], Ph_no)
                db.AddInParameter(dbCommand, "@Email", DbType.[String], email)
                db.AddInParameter(dbCommand, "@MO_cd", DbType.[String], Mo_cd)
                db.AddInParameter(dbCommand, "@Name", DbType.[String], Name)

                db.AddInParameter(dbCommand, "@DOB", DbType.[String], DOB) '10.08.2017
                db.AddInParameter(dbCommand, "@valid_from", DbType.[String], valid_from) '10.08.2017

                db.ExecuteNonQuery(dbCommand)
                outmsg = "saved"
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region

#End Region

#Region "MO Jurisdiction Insert Update"

#Region "Insert"
        Public Function MO_Jurisdiction_Save(Pre_Jurisdiction_cd As String, GO_JURIS_NO As String, GO_JURIS_Dt As String, MO_cd As String,
                                              Jurisdiction_level_cd As String,
                                             Jurisdiction_level As String, User_id As String, Ip_Address As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_MO_Jurisdiction_insertupdate")
                db.AddInParameter(dbCommand, "@PreJurisdiction_Cd", DbType.[String], Pre_Jurisdiction_cd)
                db.AddInParameter(dbCommand, "@GO_JURIS_NO", DbType.[String], GO_JURIS_NO)
                db.AddInParameter(dbCommand, "@GO_JURIS_Dt", DbType.[String], GO_JURIS_Dt)

                db.AddInParameter(dbCommand, "@MO_cd", DbType.[String], MO_cd)


                db.AddInParameter(dbCommand, "@Jurisdctn_level", DbType.[String], Jurisdiction_level)


                db.AddInParameter(dbCommand, "@User_id", DbType.[String], User_id)
                db.AddInParameter(dbCommand, "@Ip_Address", DbType.[String], Ip_Address)
                db.AddInParameter(dbCommand, "@Tag", DbType.[String], "Insert")
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

#End Region

#End Region

#Region "MO Promotion  Insert "
        Public Function Datatable_MO_promotion(tabname As DataTable, User_Id As String, IP_Address As String) As String
            Dim outmsg As String = String.Empty

            Try
                Dim db As Database = dbf.CreateConnection()
                Dim con As New SqlConnection(dbconnctn.GetConnectionString())
                Dim insertCommand As SqlCommand = New SqlCommand("MARREG_MO_Promotion_Insert", con)
                insertCommand.CommandType = CommandType.StoredProcedure
                Dim tvpParam As SqlParameter = insertCommand.Parameters.AddWithValue("@tab_value", tabname)
                tvpParam.SqlDbType = SqlDbType.Structured
                Dim UserId As SqlParameter = insertCommand.Parameters.AddWithValue("@User_Id", User_Id)
                UserId.SqlDbType = SqlDbType.NVarChar
                Dim IPAddress As SqlParameter = insertCommand.Parameters.AddWithValue("@IP_Address", IP_Address)
                IPAddress.SqlDbType = SqlDbType.NVarChar
                Dim Msg As SqlParameter = insertCommand.Parameters.AddWithValue("@Msg", 150)
                Msg.SqlDbType = SqlDbType.NVarChar
                Msg.Size = 150
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
#Region "MO Status Flag Entry"
        Public Function MARREG_MO_Status_Flag(tabname As DataTable, User_Id As String, IP_Address As String) As String
            Dim outmsg As String = String.Empty

            Try
                Dim db As Database = dbf.CreateConnection()
                Dim con As New SqlConnection(dbconnctn.GetConnectionString())
                Dim insertCommand As SqlCommand = New SqlCommand("MARREG_MO_Status_Flag_sp", con)
                insertCommand.CommandType = CommandType.StoredProcedure
                Dim tvpParam As SqlParameter = insertCommand.Parameters.AddWithValue("@sts_flag", tabname)
                tvpParam.SqlDbType = SqlDbType.Structured
                Dim UserId As SqlParameter = insertCommand.Parameters.AddWithValue("@User_Id", User_Id)
                UserId.SqlDbType = SqlDbType.NVarChar
                Dim IPAddress As SqlParameter = insertCommand.Parameters.AddWithValue("@IP_Address", IP_Address)
                IPAddress.SqlDbType = SqlDbType.NVarChar

                con.Open()
                insertCommand.ExecuteNonQuery()

                con.Close()
                Return outmsg
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region
#Region "MO Suspension  page "

#Region "MO Suspension  Insert "
        Public Function datatable_MO_Suspension(tabname As DataTable, User_Id As String, IP_Address As String) As String
            Dim outmsg As String = String.Empty

            Try
                Dim db As Database = dbf.CreateConnection()
                Dim con As New SqlConnection(dbconnctn.GetConnectionString())
                Dim insertCommand As SqlCommand = New SqlCommand("MARREG_MO_Suspension_Insert", con)
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

#Region "MO Inactive Insert "
        Public Function datatable_MO_Inactivation(tabname As DataTable, User_Id As String, IP_Address As String) As String
            Dim outmsg As String = String.Empty

            Try
                Dim db As Database = dbf.CreateConnection()
                Dim con As New SqlConnection(dbconnctn.GetConnectionString())
                Dim insertCommand As SqlCommand = New SqlCommand("[MARREG_MO_Inactivation_Insert]", con)
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

#Region "MO Death Insert "
        Public Function datatable_MO_Death(tabname As DataTable, User_Id As String, IP_Address As String) As String
            Dim outmsg As String = String.Empty

            Try
                Dim db As Database = dbf.CreateConnection()
                Dim con As New SqlConnection(dbconnctn.GetConnectionString())
                Dim insertCommand As SqlCommand = New SqlCommand("[MARREG_MO_Death_Insert]", con)
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

#Region "MO Active  Insert "
        Public Function datatable_MO_Active(tabname As DataTable, User_Id As String, IP_Address As String) As String
            Dim outmsg As String = String.Empty

            Try
                Dim db As Database = dbf.CreateConnection()
                Dim con As New SqlConnection(dbconnctn.GetConnectionString())
                Dim insertCommand As SqlCommand = New SqlCommand("MARREG_MO_Active_Insert_Update", con)
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




        '#Region "MO Jurisdiction Details Insert Update"

        '#Region "Insert"
        '        Public Function MO_Jurisdiction_Details_Save(Pre_Jurisdiction_Details_cd As String, Jurisdiction_cd As String, Jurisdiction_location_cd As String) As String
        '            Dim outmsg As String = String.Empty
        '            Try
        '                Dim db As Database = dbf.CreateConnection()
        '                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_MO_Jurisdiction_Details_insertupdate")
        '                db.AddInParameter(dbCommand, "@PreJurisdiction_Details_cd", DbType.[String], Pre_Jurisdiction_Details_cd)
        '                db.AddInParameter(dbCommand, "@Jurisdiction_Cd", DbType.[String], Jurisdiction_cd)
        '                db.AddInParameter(dbCommand, "@Jurisdctn_location_cd", DbType.[String], Jurisdiction_location_cd)
        '                db.AddInParameter(dbCommand, "@Tag", DbType.[String], "Insert")
        '                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
        '                db.ExecuteNonQuery(dbCommand)
        '                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
        '            Catch ex As Exception
        '                outmsg = ex.Message()
        '            End Try
        '            Return outmsg
        '        End Function
        '#End Region

        '#Region "Update"
        '        Public Function MO_Jurisdiction_Details_Update(Pre_Jurisdiction_Details_cd As String, Jurisdiction_cd As String, Jurisdiction_location_cd As String) As String
        '            Dim outmsg As String = String.Empty
        '            Try
        '                Dim db As Database = dbf.CreateConnection()
        '                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_MO_Jurisdiction_Details_insertupdate")
        '                db.AddInParameter(dbCommand, "@PreJurisdiction_Details_cd", DbType.[String], Pre_Jurisdiction_Details_cd)
        '                db.AddInParameter(dbCommand, "@Jurisdiction_Cd", DbType.[String], Jurisdiction_cd)
        '                db.AddInParameter(dbCommand, "@Jurisdctn_location_cd", DbType.[String], Jurisdiction_location_cd)
        '                db.AddInParameter(dbCommand, "@Tag", DbType.[String], "Update")
        '                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
        '                db.ExecuteNonQuery(dbCommand)
        '                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
        '            Catch ex As Exception
        '                outmsg = ex.Message()
        '            End Try
        '            Return outmsg
        '        End Function
        '#End Region
        '#End Region

#Region "Bank Branch Insert Update"

#Region "Insert"
        Public Function Bank_Save(Pre_bank_cd As String, Bank_nm As String, Bank_abbr As String, User_id As String, Ip_Address As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Bank_Mt_insertupdate")
                db.AddInParameter(dbCommand, "@Pre_bank_cd", DbType.[String], Pre_bank_cd)
                db.AddInParameter(dbCommand, "@Bank_nm", DbType.[String], Bank_nm)
                db.AddInParameter(dbCommand, "@Bank_abbr", DbType.[String], Bank_abbr)
                db.AddInParameter(dbCommand, "@User_id", DbType.[String], User_id)
                db.AddInParameter(dbCommand, "@Ip_Address", DbType.[String], Ip_Address)
                db.AddInParameter(dbCommand, "@Tag", DbType.[String], "Insert")
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
        Public Function Bank_Update(Pre_bank_cd As String, Bank_nm As String, Bank_abbr As String, User_id As String, Ip_Address As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Bank_Mt_insertupdate")
                db.AddInParameter(dbCommand, "@Pre_bank_cd", DbType.[String], Pre_bank_cd)
                db.AddInParameter(dbCommand, "@Bank_nm", DbType.[String], Bank_nm)
                db.AddInParameter(dbCommand, "@Bank_abbr", DbType.[String], Bank_abbr)
                db.AddInParameter(dbCommand, "@User_id", DbType.[String], User_id)
                db.AddInParameter(dbCommand, "@Ip_Address", DbType.[String], Ip_Address)
                db.AddInParameter(dbCommand, "@Tag", DbType.[String], "Update")
                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region

#Region "Grid Fill Bank"
        Public Function Viewbank(tabname As String, sate_Cd As String) As DataSet
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim ds = New DataSet()
            Try
                con.Open()
                Dim da As New SqlDataAdapter("MARREG_All_GridView_Load_insertupdate", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure
                'SqlParameter param = new SqlParameter("@flag", 2);
                da.SelectCommand.Parameters.Add(New SqlParameter("@TableName", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@TableName").Value = tabname
                da.SelectCommand.Parameters.Add(New SqlParameter("@State_Cd", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@State_Cd").Value = sate_Cd
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
#End Region



#Region "Bank Branch Insert Update"

#Region "Insert"
        Public Function Branch_Save(Pre_brn_cd As String, State_cd As String, District_cd As String, Bank_cd As String, brnEnam As String, brnVnam As String,
                                    IFSC As String, Address As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Bank_Branch_Mt_insertupdate")
                db.AddInParameter(dbCommand, "@PreBank_brn_cd", DbType.[String], Pre_brn_cd)
                db.AddInParameter(dbCommand, "@State_cd", DbType.[String], State_cd)
                db.AddInParameter(dbCommand, "@District_cd", DbType.[String], District_cd)
                db.AddInParameter(dbCommand, "@Bank_cd", DbType.[String], Bank_cd)
                db.AddInParameter(dbCommand, "@Branch_EName", DbType.[String], brnEnam)
                db.AddInParameter(dbCommand, "@Branch_VName", DbType.[String], brnVnam)
                db.AddInParameter(dbCommand, "@IFSC", DbType.[String], IFSC)
                db.AddInParameter(dbCommand, "@Bank_Address", DbType.[String], Address)
                db.AddInParameter(dbCommand, "@Tag", DbType.[String], "Insert")
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
        Public Function Branch_Update(Pre_brn_cd As String, State_cd As String, District_cd As String, Bank_cd As String, brnEnam As String, brnVnam As String,
                                    IFSC As String, Address As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Bank_Branch_Mt_insertupdate")
                db.AddInParameter(dbCommand, "@PreBank_brn_cd", DbType.[String], Pre_brn_cd)
                db.AddInParameter(dbCommand, "@State_cd", DbType.[String], State_cd)
                db.AddInParameter(dbCommand, "@District_cd", DbType.[String], District_cd)
                db.AddInParameter(dbCommand, "@Bank_cd", DbType.[String], Bank_cd)
                db.AddInParameter(dbCommand, "@Branch_EName", DbType.[String], brnEnam)
                db.AddInParameter(dbCommand, "@Branch_VName", DbType.[String], brnVnam)
                db.AddInParameter(dbCommand, "@IFSC", DbType.[String], IFSC)
                db.AddInParameter(dbCommand, "@Bank_Address", DbType.[String], Address)
                db.AddInParameter(dbCommand, "@Tag", DbType.[String], "Update")
                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region

#End Region

#Region "Mo Signature load"
#Region "Applicant Receipt page Gridview"
        Public Function MO_signature_load(MO_Autoincrement_Cd As String) As DataTable
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim dt = New DataTable()
            Try
                con.Open()
                Dim da As New SqlDataAdapter("MARREG_MO_Signature_load", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure
                'SqlParameter param = new SqlParameter("@flag", 2);
                da.SelectCommand.Parameters.Add(New SqlParameter("@MO_Autoincrement_Cd", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@MO_Autoincrement_Cd").Value = MO_Autoincrement_Cd

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


#Region "Applicant Receipt page "

#Region "New application GridView"
        Public Function New_Application_gridview(MO_Autoincrement_Cd As String, tag As String) As DataTable
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim dt = New DataTable()
            Try
                con.Open()
                Dim da As New SqlDataAdapter("Resp_MARREG_NEW_Application_Gridview", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure
                'SqlParameter param = new SqlParameter("@flag", 2);
                da.SelectCommand.Parameters.Add(New SqlParameter("@MO_Autoincrement_Cd", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@MO_Autoincrement_Cd").Value = MO_Autoincrement_Cd
                da.SelectCommand.Parameters.Add(New SqlParameter("@tag", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@tag").Value = tag
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

        Public Function New_Application_GRN_gridview(MO_Autoincrement_Cd As String, tag As String) As DataTable
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim dt = New DataTable()
            Try
                con.Open()
                Dim da As New SqlDataAdapter("MARREG_NEW_Application_GRN", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure
                'SqlParameter param = new SqlParameter("@flag", 2);
                da.SelectCommand.Parameters.Add(New SqlParameter("@MO_Autoincrement_Cd", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@MO_Autoincrement_Cd").Value = MO_Autoincrement_Cd
                da.SelectCommand.Parameters.Add(New SqlParameter("@tag", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@tag").Value = tag
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


#Region "Receipt  GridView"
        Public Function Receipt_gridview(MO_Autoincrement_Cd As String, From_dt As String, To_dt As String) As DataTable
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim dt = New DataTable()
            Try
                con.Open()
                Dim da As New SqlDataAdapter("MARREG_Application_Receipt_Gridview", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure
                da.SelectCommand.Parameters.Add(New SqlParameter("@MO_Autoincrement_Cd", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@MO_Autoincrement_Cd").Value = MO_Autoincrement_Cd
                da.SelectCommand.Parameters.Add(New SqlParameter("@From_dt", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@From_dt").Value = From_dt
                da.SelectCommand.Parameters.Add(New SqlParameter("@To_dt", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@To_dt").Value = To_dt
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

#Region "Application Status View"
        Public Function Application_status_view(Applicant_Cd As String, objection_cd As String) As DataTable
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim dt = New DataTable()
            Try
                con.Open()
                Dim da As New SqlDataAdapter("MARREG_Application_Status_view", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure
                'SqlParameter param = new SqlParameter("@flag", 2);
                da.SelectCommand.Parameters.Add(New SqlParameter("@Applicant_Cd", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@Applicant_Cd").Value = Applicant_Cd
                da.SelectCommand.Parameters.Add(New SqlParameter("@Objector_auto_cd", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@Objector_auto_cd").Value = objection_cd
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

#Region "Objection Raised Page"
#Region "Objection Raised Gridview"
        Public Function objectn_raised(MO_Autoincrmnt_Cd As String) As DataTable
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim dt = New DataTable()
            Try
                con.Open()
                Dim da As New SqlDataAdapter("MARREG_Objection_Raised_Gridview", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure
                'SqlParameter param = new SqlParameter("@flag", 2);
                da.SelectCommand.Parameters.Add(New SqlParameter("@MO_Autoincrmnt_Cd", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@MO_Autoincrmnt_Cd").Value = MO_Autoincrmnt_Cd
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

#Region "Objection Hearing Insert "
        Public Function xmlLoad_objctn_hering(tabname As DataTable, User_Id As String, IP_Address As String, Mac_Address As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim con As New SqlConnection(dbconnctn.GetConnectionString())
                Dim insertCommand As SqlCommand = New SqlCommand("MARREG_Objection_Hearing_Insert", con)
                insertCommand.CommandType = CommandType.StoredProcedure
                Dim tvpParam As SqlParameter = insertCommand.Parameters.AddWithValue("@tab_value", tabname)
                tvpParam.SqlDbType = SqlDbType.Structured
                Dim UserId As SqlParameter = insertCommand.Parameters.AddWithValue("@User_Id", User_Id)
                UserId.SqlDbType = SqlDbType.NVarChar
                Dim IPAddress As SqlParameter = insertCommand.Parameters.AddWithValue("@IP_Address", IP_Address)
                IPAddress.SqlDbType = SqlDbType.NVarChar
                Dim MACAddress As SqlParameter = insertCommand.Parameters.AddWithValue("@Mac_Address", Mac_Address)
                MACAddress.SqlDbType = SqlDbType.NVarChar
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

#Region "Objector Details View"
        Public Function objector_Details_View(Objectort_Cd As String) As DataTable
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim dt = New DataTable()
            Try
                con.Open()
                Dim da As New SqlDataAdapter("MARREG_Objector_Details_view", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure
                'SqlParameter param = new SqlParameter("@flag", 2);
                da.SelectCommand.Parameters.Add(New SqlParameter("@Ojector_Cd", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@Ojector_Cd").Value = Objectort_Cd

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

#Region "Objection Inquiry Insert"
        Public Function Objection_Inquiry_Save(Objection_Cd As String, Applicant_Cd As String, Objection_Reason As String, Attach_Evidence As String,
                                               Evidence_Proof As Byte(), Bank_Cd As String, Bank_Branch_State As String, Bank_Branch_District As String, Bank_Branch_Cd As String, Payment_Mode_Cd As String, Payment_Mode_No As String, Payment_Mode_Dt As String, Payable_At As String, Remarks As String, Status As String,
                                                 IP_Address As String, User_Id As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Objection_Inquiry_Insert")
                db.AddInParameter(dbCommand, "@Objector_auto_cd", DbType.[String], Objection_Cd)

                db.AddInParameter(dbCommand, "@App_Auto_Cd", DbType.[String], Applicant_Cd)
                db.AddInParameter(dbCommand, "@Objection_Reason", DbType.[String], Objection_Reason)
                db.AddInParameter(dbCommand, "@Attach_Evidence", DbType.[String], Attach_Evidence)
                db.AddInParameter(dbCommand, "@Evidence_Proof", DbType.[Binary], Evidence_Proof)
                db.AddInParameter(dbCommand, "@Bank_Cd", DbType.[String], Bank_Cd)
                db.AddInParameter(dbCommand, "@Bank_Branch_state", DbType.[String], Bank_Branch_State)
                db.AddInParameter(dbCommand, "@Bank_Branch_District", DbType.[String], Bank_Branch_District)
                db.AddInParameter(dbCommand, "@Bank_Branch_Cd", DbType.[String], Bank_Branch_Cd)
                db.AddInParameter(dbCommand, "@Payment_Mode_Cd", DbType.[String], Payment_Mode_Cd)
                db.AddInParameter(dbCommand, "@Payment_Mode_No", DbType.[String], Payment_Mode_No)
                db.AddInParameter(dbCommand, "@Payment_Mode_Dt", DbType.[String], Payment_Mode_Dt)
                db.AddInParameter(dbCommand, "@Payable_At", DbType.String, Payable_At)
                db.AddInParameter(dbCommand, "@Remarks", DbType.[String], Remarks)
                db.AddInParameter(dbCommand, "@Status", DbType.[String], Status)

                db.AddInParameter(dbCommand, "@Ip_Address", DbType.[String], IP_Address)
                db.AddInParameter(dbCommand, "@user_id", DbType.[String], User_Id)
                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region

#Region "MO Appoinment Page "
#Region "MO Appoinment grid veiw "
        Public Function MO_Appoinment_gridview(MO_Autoincrement_Cd As String, from_dt As String, to_dt As String, type As String) As DataTable
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim dt = New DataTable()
            Try
                con.Open()
                Dim da As New SqlDataAdapter("MARREG_MO_Appoinment_Grid_view", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure
                da.SelectCommand.Parameters.Add(New SqlParameter("@MO_Autoincrmnt_Cd", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@MO_Autoincrmnt_Cd").Value = MO_Autoincrement_Cd
                da.SelectCommand.Parameters.Add(New SqlParameter("@from_dt", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@from_dt").Value = from_dt
                da.SelectCommand.Parameters.Add(New SqlParameter("@to_dt", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@to_dt").Value = to_dt
                da.SelectCommand.Parameters.Add(New SqlParameter("@type", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@type").Value = type
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

#Region "MO Appoinment Insert & Application Status Insert"
        Public Function MO_Status_appointment_Insert(tabname As DataTable, IP_Address As String, User_Id As String, Mac_Address As String, tag As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim con As New SqlConnection(dbconnctn.GetConnectionString())
                Dim insertCommand As SqlCommand = New SqlCommand("Resp_MARREG_New_Applicant_Status_Insert", con)
                insertCommand.CommandType = CommandType.StoredProcedure
                Dim tvpParam As SqlParameter = insertCommand.Parameters.AddWithValue("@tab_value", tabname)
                tvpParam.SqlDbType = SqlDbType.Structured
                Dim IPAddress As SqlParameter = insertCommand.Parameters.AddWithValue("@IP_Address", IP_Address)
                IPAddress.SqlDbType = SqlDbType.NVarChar
                Dim MACAddress As SqlParameter = insertCommand.Parameters.AddWithValue("@Mac_Address", Mac_Address)
                MACAddress.SqlDbType = SqlDbType.NVarChar
                Dim UserId As SqlParameter = insertCommand.Parameters.AddWithValue("@User_Id", User_Id)
                UserId.SqlDbType = SqlDbType.NVarChar
                Dim tag1 As SqlParameter = insertCommand.Parameters.AddWithValue("@tag", tag)
                tag1.SqlDbType = SqlDbType.NVarChar

                Dim Msg As SqlParameter = insertCommand.Parameters.AddWithValue("@Msg", 50)
                Msg.SqlDbType = SqlDbType.NVarChar
                Msg.Size = 50
                Msg.Direction = ParameterDirection.Output
                con.Open()
                insertCommand.ExecuteNonQuery()
                outmsg = insertCommand.Parameters("@Msg").Value.ToString()
                con.Close()
                Return outmsg

            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region

#Region "GRN STATUS INSERT"
        Public Function GRN_Status_Insert(tabname As DataTable, IP_Address As String, User_Id As String, Mac_Address As String, tag As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim con As New SqlConnection(dbconnctn.GetConnectionString())
                Dim insertCommand As SqlCommand = New SqlCommand("MARREG_GRN_Status_Insert", con)
                insertCommand.CommandType = CommandType.StoredProcedure
                Dim tvpParam As SqlParameter = insertCommand.Parameters.AddWithValue("@tab_value", tabname)
                tvpParam.SqlDbType = SqlDbType.Structured
                Dim IPAddress As SqlParameter = insertCommand.Parameters.AddWithValue("@IP_Address", IP_Address)
                IPAddress.SqlDbType = SqlDbType.NVarChar
                Dim MACAddress As SqlParameter = insertCommand.Parameters.AddWithValue("@Mac_Address", Mac_Address)
                MACAddress.SqlDbType = SqlDbType.NVarChar
                Dim UserId As SqlParameter = insertCommand.Parameters.AddWithValue("@User_Id", User_Id)
                UserId.SqlDbType = SqlDbType.NVarChar
                Dim tag1 As SqlParameter = insertCommand.Parameters.AddWithValue("@tag", tag)
                tag1.SqlDbType = SqlDbType.NVarChar

                Dim Msg As SqlParameter = insertCommand.Parameters.AddWithValue("@Msg", 50)
                Msg.SqlDbType = SqlDbType.NVarChar
                Msg.Size = 50
                Msg.Direction = ParameterDirection.Output
                con.Open()
                insertCommand.ExecuteNonQuery()
                outmsg = insertCommand.Parameters("@Msg").Value.ToString()
                con.Close()
                Return outmsg

            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region

#Region "Declaration Insert"
        'Public Function Declaration_Save(App_Auto_cd As String, Mo_Autoincrement_cd As String, Declaration_Time As String, Bride_FingerPrint As String, Groom_FingerPrint As String,
        '                                 Witness1_Name As String, Witness1_Address As String, Witness1_Sign As Byte(), Witness1_FingerPrint As String, Witness2_Name As String,
        '                                 Witness2_Address As String, Witness2_Sign As Byte(), Witness2_FingerPrint As String, Witness3_Name As String, Witness3_Address As String,
        '                                 Witness3_Sign As Byte(), Witness3_FingerPrint As String, User_Id As String, IP_Address As String) As String
        '    Dim outmsg As String = String.Empty
        '    Try
        '        Dim db As Database = dbf.CreateConnection()
        '        Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Declaration_insert")
        '        db.AddInParameter(dbCommand, "@App_Auto_cd", DbType.[String], App_Auto_cd)
        '        db.AddInParameter(dbCommand, "@Mo_Autoincrement_cd", DbType.[String], Mo_Autoincrement_cd)
        '        db.AddInParameter(dbCommand, "@Declaration_Time", DbType.[String], Declaration_Time)
        '        db.AddInParameter(dbCommand, "@Bride_FingerPrint", DbType.[String], Bride_FingerPrint)
        '        db.AddInParameter(dbCommand, "@Groom_FingerPrint", DbType.[String], Groom_FingerPrint)
        '        db.AddInParameter(dbCommand, "@Witness1_Name", DbType.[String], Witness1_Name)
        '        db.AddInParameter(dbCommand, "@Witness1_Address", DbType.[String], Witness1_Address)
        '        db.AddInParameter(dbCommand, "@Witness1_Sign", DbType.Binary, Witness1_Sign)
        '        db.AddInParameter(dbCommand, "@Witness1_FingerPrint", DbType.[String], Witness1_FingerPrint)
        '        db.AddInParameter(dbCommand, "@Witness2_Name", DbType.[String], Witness2_Name)
        '        db.AddInParameter(dbCommand, "@Witness2_Address", DbType.[String], Witness2_Address)
        '        db.AddInParameter(dbCommand, "@Witness2_Sign", DbType.Binary, Witness2_Sign)
        '        db.AddInParameter(dbCommand, "@Witness2_FingerPrint", DbType.[String], Witness2_FingerPrint)
        '        db.AddInParameter(dbCommand, "@Witness3_Name", DbType.[String], Witness3_Name)
        '        db.AddInParameter(dbCommand, "@Witness3_Address", DbType.[String], Witness3_Address)
        '        db.AddInParameter(dbCommand, "@Witness3_Sign", DbType.Binary, Witness3_Sign)
        '        db.AddInParameter(dbCommand, "@Witness3_FingerPrint", DbType.[String], Witness3_FingerPrint)
        '        db.AddInParameter(dbCommand, "@User_Id", DbType.[String], User_Id)
        '        db.AddInParameter(dbCommand, "@Ip_Address", DbType.[String], IP_Address)
        '        db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
        '        db.ExecuteNonQuery(dbCommand)
        '        outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
        '    Catch ex As Exception
        '        outmsg = ex.Message()
        '    End Try
        '    Return outmsg
        'End Function

        Public Function Declaration_Save(App_Auto_cd As String, Mo_Autoincrement_cd As String, Declaration_Time As String, Bride_FingerPrint As String, Groom_FingerPrint As String,
                                         Witness1_Name As String, Witness1_Address As String, Witness1_Sign As Byte(), Witness1_Adhr As String, Witness1_FingerPrint As String, Witness2_Name As String,
                                         Witness2_Address As String, Witness2_Sign As Byte(), Witness2_Adhr As String, Witness2_FingerPrint As String, Witness3_Name As String, Witness3_Address As String,
                                         Witness3_Sign As Byte(), Witness3_Adhr As String, Witness3_FingerPrint As String, User_Id As String, IP_Address As String, Mac_Address As String,
                                         Witness1_Adhr_Consent As String, Witness2_Adhr_Consent As String, Witness3_Adhr_Consent As String,
                                         doc_prf_wit1 As Byte(), doc_prf_wit2 As Byte(), doc_prf_wit3 As Byte(), doc_prf_type_wit1 As String, doc_prf_type_wit2 As String, doc_prf_type_wit3 As String, Joint_Photo As Byte()) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Declaration_insert")
                db.AddInParameter(dbCommand, "@App_Auto_cd", DbType.[String], App_Auto_cd)
                db.AddInParameter(dbCommand, "@Mo_Autoincrement_cd", DbType.[String], Mo_Autoincrement_cd)
                db.AddInParameter(dbCommand, "@Declaration_Time", DbType.[String], Declaration_Time)
                db.AddInParameter(dbCommand, "@Bride_FingerPrint", DbType.[String], Bride_FingerPrint)
                db.AddInParameter(dbCommand, "@Groom_FingerPrint", DbType.[String], Groom_FingerPrint)
                db.AddInParameter(dbCommand, "@Witness1_Name", DbType.[String], Witness1_Name)
                db.AddInParameter(dbCommand, "@Witness1_Address", DbType.[String], Witness1_Address)
                db.AddInParameter(dbCommand, "@Witness1_Sign", DbType.Binary, Witness1_Sign)
                db.AddInParameter(dbCommand, "@Witness1_Adhr_Nm", DbType.[String], Witness1_Adhr)
                db.AddInParameter(dbCommand, "@Witness1_FingerPrint", DbType.[String], Witness1_FingerPrint)

                db.AddInParameter(dbCommand, "@Witness2_Name", DbType.[String], Witness2_Name)
                db.AddInParameter(dbCommand, "@Witness2_Address", DbType.[String], Witness2_Address)
                db.AddInParameter(dbCommand, "@Witness2_Sign", DbType.Binary, Witness2_Sign)
                db.AddInParameter(dbCommand, "@Witness2_Adhr_Nm", DbType.[String], Witness2_Adhr)
                db.AddInParameter(dbCommand, "@Witness2_FingerPrint", DbType.[String], Witness2_FingerPrint)

                db.AddInParameter(dbCommand, "@Witness3_Name", DbType.[String], Witness3_Name)
                db.AddInParameter(dbCommand, "@Witness3_Address", DbType.[String], Witness3_Address)
                db.AddInParameter(dbCommand, "@Witness3_Sign", DbType.Binary, Witness3_Sign)
                db.AddInParameter(dbCommand, "@Witness3_Adhr_Nm", DbType.[String], Witness3_Adhr)

                db.AddInParameter(dbCommand, "@Witness3_FingerPrint", DbType.[String], Witness3_FingerPrint)
                db.AddInParameter(dbCommand, "@User_Id", DbType.[String], User_Id)
                db.AddInParameter(dbCommand, "@Ip_Address", DbType.[String], IP_Address)
                db.AddInParameter(dbCommand, "@Mac_Address", DbType.[String], Mac_Address)

                db.AddInParameter(dbCommand, "@Witness1_Adhr_Consent", DbType.[String], Witness1_Adhr_Consent)
                db.AddInParameter(dbCommand, "@Witness2_Adhr_Consent", DbType.[String], Witness2_Adhr_Consent)
                db.AddInParameter(dbCommand, "@Witness3_Adhr_Consent", DbType.[String], Witness3_Adhr_Consent)

                db.AddInParameter(dbCommand, "@doc_prf_wit1", DbType.Binary, doc_prf_wit1)
                db.AddInParameter(dbCommand, "@doc_prf_wit2", DbType.Binary, doc_prf_wit2)
                db.AddInParameter(dbCommand, "@doc_prf_wit3", DbType.Binary, doc_prf_wit3)
                db.AddInParameter(dbCommand, "@doc_prf_type_wit1", DbType.[String], doc_prf_type_wit1)
                db.AddInParameter(dbCommand, "@doc_prf_type_wit2", DbType.[String], doc_prf_type_wit2)
                db.AddInParameter(dbCommand, "@doc_prf_type_wit3", DbType.[String], doc_prf_type_wit3)

                db.AddInParameter(dbCommand, "@Joint_Photo", DbType.Binary, Joint_Photo)

                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function

        Public Function Schedule_of_Signature_Witness_Save(App_Auto_cd As String, Mo_Autoincrement_cd As String, Declaration_Time As String, Witness1_Name As String, Witness1_Address As String, Witness1_ps As String,
                                                           Witness2_Name As String, Witness2_Address As String, Witness2_ps As String, Witness3_Name As String, Witness3_Address As String, Witness3_ps As String, IP_Address As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Schedule_of_Signature_Witness_insert")
                db.AddInParameter(dbCommand, "@App_Auto_cd", DbType.[String], App_Auto_cd)
                db.AddInParameter(dbCommand, "@Mo_Autoincrement_cd", DbType.[String], Mo_Autoincrement_cd)
                db.AddInParameter(dbCommand, "@Declaration_Time", DbType.[String], Declaration_Time)

                db.AddInParameter(dbCommand, "@Witness1_Name", DbType.[String], Witness1_Name)
                db.AddInParameter(dbCommand, "@Witness1_Address", DbType.[String], Witness1_Address)
                db.AddInParameter(dbCommand, "@Witness1_ps", DbType.[String], Witness1_ps)

                db.AddInParameter(dbCommand, "@Witness2_Name", DbType.[String], Witness2_Name)
                db.AddInParameter(dbCommand, "@Witness2_Address", DbType.[String], Witness2_Address)
                db.AddInParameter(dbCommand, "@Witness2_ps", DbType.[String], Witness2_ps)

                db.AddInParameter(dbCommand, "@Witness3_Name", DbType.[String], Witness3_Name)
                db.AddInParameter(dbCommand, "@Witness3_Address", DbType.[String], Witness3_Address)
                db.AddInParameter(dbCommand, "@Witness3_ps", DbType.[String], Witness3_ps)
                db.AddInParameter(dbCommand, "@Ip_Address", DbType.[String], IP_Address)

                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function

        Public Function upload_sign(App_Auto_cd As String, Tag As String, Sign As Byte()) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Sign_Insert_InDeclaration")
                db.AddInParameter(dbCommand, "@App_Auto_cd", DbType.[String], App_Auto_cd)
                db.AddInParameter(dbCommand, "@Tag", DbType.[String], Tag)
                db.AddInParameter(dbCommand, "@GroomBride_Sign", DbType.Binary, Sign)


                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function

        Public Function GetApplication_witness_details(App_auto_cd As String) As DataTable
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim dt = New DataTable()
            Try
                con.Open()
                Dim da As New SqlDataAdapter("MARREG_View_witness_details", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure

                da.SelectCommand.Parameters.Add(New SqlParameter("@App_auto_cd", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@App_auto_cd").Value = App_auto_cd

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

#Region "Witness Update"         '27.06.2019anay
        Public Function Witness_Update(App_Auto_cd As String, Witness1_Name As String, Witness1_Address As String, doc_prf_type_wit1 As String,
                                        doc_prf_wit1 As Byte(), Witness1_Sign As Byte(), tag As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Witness_Update")
                db.AddInParameter(dbCommand, "@App_Auto_cd", DbType.[String], App_Auto_cd)
                db.AddInParameter(dbCommand, "@Witness1_Name", DbType.[String], Witness1_Name)
                db.AddInParameter(dbCommand, "@Witness1_Address", DbType.[String], Witness1_Address)
                db.AddInParameter(dbCommand, "@doc_prf_type_wit1", DbType.[String], doc_prf_type_wit1)
                db.AddInParameter(dbCommand, "@doc_prf_wit1", DbType.Binary, doc_prf_wit1)
                db.AddInParameter(dbCommand, "@Witness1_Sign", DbType.Binary, Witness1_Sign)
                db.AddInParameter(dbCommand, "@tag", DbType.[String], tag)

                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region

        '18-01-2019
#Region "Additional Payment"
        Public Function AdditionalPayment_Save(App_Auto_cd As String, Mo_Autoincrement_cd As String, MARREG_Payment_Reason_cd As String, AdditionalPayment_Status As String, Print_Status As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_AdditionalPayment_sp")
                db.AddInParameter(dbCommand, "@App_Auto_cd", DbType.[String], App_Auto_cd)
                db.AddInParameter(dbCommand, "@Mo_Autoincrement_cd", DbType.[String], Mo_Autoincrement_cd)
                'db.AddInParameter(dbCommand, "@MARREG_party_residence_flag", DbType.[String], MARREG_party_residence_flag)
                'db.AddInParameter(dbCommand, "@MARREG_solemnisation_place_flag", DbType.[String], MARREG_solemnisation_place_flag)
                'db.AddInParameter(dbCommand, "@MARREG_solemnisation_time_flag", DbType.[String], MARREG_solemnisation_time_flag)
                'db.AddInParameter(dbCommand, "@MARREG_certified_copy_flag", DbType.[String], MARREG_certified_copy_flag)
                db.AddInParameter(dbCommand, "@MARREG_Payment_Reason_cd", DbType.[String], MARREG_Payment_Reason_cd)
                db.AddInParameter(dbCommand, "@AdditionalPayment_Status", DbType.[String], AdditionalPayment_Status)
                db.AddInParameter(dbCommand, "@Print_Status", DbType.[String], Print_Status)
                'db.AddInParameter(dbCommand, "@Entrydate", DbType.[String], Entrydate)
                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region

#Region "Generate Certificate"
        Public Function Gen_Certificate_Save(Hologram_No As String, App_Auto_Cd As String, qrcode As Byte(), Bank_Cd As String, Branch_Dist As String,
                                        Branch_Cd As String, Payment_Mode_Cd As String, Payment_Mode_No As String, Payment_Mode_Dt As String,
                                        Payable_At As String, User_Id As String, Ip_Address As String, Type As String, OTP As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Certificate_Generate")
                db.AddInParameter(dbCommand, "@Hologram_No", DbType.[String], Hologram_No)
                db.AddInParameter(dbCommand, "@App_Auto_Cd", DbType.[String], App_Auto_Cd)
                db.AddInParameter(dbCommand, "@qrcode", DbType.Binary, qrcode)
                db.AddInParameter(dbCommand, "@Bank_Cd", DbType.[String], Bank_Cd)
                db.AddInParameter(dbCommand, "@Branch_Dist", DbType.[String], Branch_Dist)
                db.AddInParameter(dbCommand, "@Branch_Cd", DbType.[String], Branch_Cd)
                db.AddInParameter(dbCommand, "@Payment_Mode_Cd", DbType.[String], Payment_Mode_Cd)
                db.AddInParameter(dbCommand, "@Payment_Mode_No", DbType.[String], Payment_Mode_No)
                db.AddInParameter(dbCommand, "@Payment_Mode_Dt", DbType.[String], Payment_Mode_Dt)
                db.AddInParameter(dbCommand, "@Payable_At", DbType.[String], Payable_At)

                db.AddInParameter(dbCommand, "@User_Id", DbType.[String], User_Id)
                db.AddInParameter(dbCommand, "@Ip_Address", DbType.[String], Ip_Address)
                db.AddInParameter(dbCommand, "@Type", DbType.[String], Type)
                db.AddInParameter(dbCommand, "@Authen_OTP", DbType.[String], OTP)
                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region

#Region "Marreg Hologram New Stock Insert"
        'Public Function Marreg_Hologram_Insert(PusrchsDate As String, Strt_hologramNo As String, End_hologramno As String) As String
        '    Dim outmsg As String = String.Empty
        '    Try
        '        Dim db As Database = dbf.CreateConnection()
        '        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Marreg_Hologram_Insert")
        '        db.AddInParameter(dbCommand, "@PusrchsDate", DbType.String, PusrchsDate)
        '        db.AddInParameter(dbCommand, "@Strt_hologramNo", DbType.String, Strt_hologramNo)
        '        db.AddInParameter(dbCommand, "@End_hologramno", DbType.String, End_hologramno)
        '        db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
        '        db.ExecuteNonQuery(dbCommand)
        '        outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
        '        Return outmsg
        '    Catch ex As Exception
        '        outmsg = ex.Message()
        '    End Try
        '    Return outmsg
        'End Function

        Public Function Marreg_Holo_GO_Insert(state_cd As String, GONm As String, GODate As String, Begin_hologramNo As String, End_hologramno As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("Marreg_Hologram_Stock_Insert")
                db.AddInParameter(dbCommand, "@State_cd", DbType.String, state_cd)
                db.AddInParameter(dbCommand, "@GO_Cd", DbType.String, GONm)
                db.AddInParameter(dbCommand, "@GODate", DbType.String, GODate)
                db.AddInParameter(dbCommand, "@Begin_hologramNo", DbType.String, Begin_hologramNo)
                db.AddInParameter(dbCommand, "@End_hologramno", DbType.String, End_hologramno)
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

#Region "Marreg Hologram Stock Issue Insert"
        Public Function Marreg_Holo_Stk_Issue_Insert(User_Id As String, Ip_Address As String, GO_Cd As String, Strt_hologramno As String, End_hologramno As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("Marreg_Hologram_Stock_Issue_Insert")
                db.AddInParameter(dbCommand, "@GO_Cd", DbType.String, GO_Cd)
                db.AddInParameter(dbCommand, "@User_Id", DbType.[String], User_Id)
                db.AddInParameter(dbCommand, "@Ip_Address", DbType.[String], Ip_Address)
                db.AddInParameter(dbCommand, "@Begin_holo_Iss_No", DbType.String, Strt_hologramno)
                db.AddInParameter(dbCommand, "@End_holo_Iss_no", DbType.String, End_hologramno)
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

#Region "Marreg Hologram Stock Count"
        Public Function Marreg_Holo_Stk_Count(state_cd As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("Marreg_Hologram_Stock_Count")
                db.AddInParameter(dbCommand, "@State_cd", DbType.String, state_cd)
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

#Region "GO Hologram Start End View"
        Public Function GO_detail_view(GO_cd As String) As DataTable
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim dt = New DataTable()
            Try
                con.Open()
                Dim da As New SqlDataAdapter("MARREG_GO_Num_view", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure
                'SqlParameter param = new SqlParameter("@flag", 2);
                da.SelectCommand.Parameters.Add(New SqlParameter("@GO_Auto_Cd", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@GO_Auto_Cd").Value = GO_cd
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

#Region "Marreg  Hologram View Gridview"
        Public Function Marreg_Holgram_stock_view(From_dt As String, To_dt As String) As DataTable
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim dt = New DataTable()
            Try
                con.Open()
                Dim da As New SqlDataAdapter("Marreg_Holgram_stock_view", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure
                da.SelectCommand.Parameters.Add(New SqlParameter("@From_dt", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@From_dt").Value = From_dt
                da.SelectCommand.Parameters.Add(New SqlParameter("@To_dt", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@To_dt").Value = To_dt
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

#Region "Scanned Record Entry"
        Public Function Scan_Recrd_Save(Groom_FNam As String, Groom_MNam As String, Groom_LNam As String, Groom_Father_Nam As String, Groom_Mother_Nam As String, Groom_DOB As String,
                                        Bride_FNam As String, Bride_MNam As String, Bride_LNam As String, Bride_Father_Nam As String, Bride_Mother_Nam As String, Bride_DOB As String,
                                        Act_cd As String, Reg_Dt As DateTime, MO_Name As String, Reg_Add As String, Reg_Dist As String, Reg_SubDiv As String, Reg_Block As String,
                                        Reg_PS As String, Reg_WA As String, Reg_MuniGp As String, Reg_VilWrd As String, Reg_PO As String, Notice As Byte(), Declrtn As Byte(),
                                        Cert As Byte(), User_Id As String, IP_Address As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Scanned_Record_insert")
                db.AddInParameter(dbCommand, "@Groom_FNam", DbType.[String], Groom_FNam)
                db.AddInParameter(dbCommand, "@Groom_MNam", DbType.[String], Groom_MNam)
                db.AddInParameter(dbCommand, "@Groom_LNam", DbType.[String], Groom_LNam)
                db.AddInParameter(dbCommand, "@Groom_Father_Nam", DbType.[String], Groom_Father_Nam)
                db.AddInParameter(dbCommand, "@Groom_Mother_Nam", DbType.[String], Groom_Mother_Nam)
                db.AddInParameter(dbCommand, "@Groom_DOB", DbType.[String], Groom_DOB)

                db.AddInParameter(dbCommand, "@Bride_FNam", DbType.[String], Bride_FNam)
                db.AddInParameter(dbCommand, "@Bride_MNam", DbType.[String], Bride_MNam)
                db.AddInParameter(dbCommand, "@Bride_LNam", DbType.[String], Bride_LNam)
                db.AddInParameter(dbCommand, "@Bride_Father_Nam", DbType.[String], Bride_Father_Nam)
                db.AddInParameter(dbCommand, "@Bride_Mother_Nam", DbType.[String], Bride_Mother_Nam)
                db.AddInParameter(dbCommand, "@Bride_DOB", DbType.[String], Bride_DOB)

                db.AddInParameter(dbCommand, "@Act_cd", DbType.[String], Act_cd)
                db.AddInParameter(dbCommand, "@Reg_Dt", DbType.DateTime, Reg_Dt)
                db.AddInParameter(dbCommand, "@MO_Name", DbType.[String], MO_Name)

                db.AddInParameter(dbCommand, "@Reg_Add", DbType.[String], Reg_Add)
                db.AddInParameter(dbCommand, "@Reg_Dist", DbType.[String], Reg_Dist)
                db.AddInParameter(dbCommand, "@Reg_SubDiv", DbType.[String], Reg_SubDiv)
                db.AddInParameter(dbCommand, "@Reg_Block", DbType.[String], Reg_Block)
                db.AddInParameter(dbCommand, "@Reg_PS", DbType.[String], Reg_PS)
                db.AddInParameter(dbCommand, "@Reg_WA", DbType.[String], Reg_WA)
                db.AddInParameter(dbCommand, "@Reg_MuniGp", DbType.[String], Reg_MuniGp)
                db.AddInParameter(dbCommand, "@Reg_VilWrd", DbType.[String], Reg_VilWrd)
                db.AddInParameter(dbCommand, "@Reg_PO", DbType.[String], Reg_PO)

                db.AddInParameter(dbCommand, "@Notice", DbType.Binary, Notice)
                db.AddInParameter(dbCommand, "@Declaration", DbType.Binary, Declrtn)
                db.AddInParameter(dbCommand, "@Certificate", DbType.Binary, Cert)

                db.AddInParameter(dbCommand, "@User_Id", DbType.[String], User_Id)
                db.AddInParameter(dbCommand, "@Ip_Address", DbType.[String], IP_Address)
                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region

#Region "Who's who Insert Update"

#Region "Insert"
        Public Function Who_is_Who_Save(wh_Id As String, wh_name As String, wh_degnation As String, wh_addr As String, wh_contct As String, whFax As String, whEMail As String, User_Id As String, IP_Address As String, wh_status As String, wh_image As Byte()) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Who_is_Who_Mt_insertupdate")
                db.AddInParameter(dbCommand, "@Prewh_Id", DbType.[String], wh_Id)
                db.AddInParameter(dbCommand, "@wh_name", DbType.[String], wh_name)
                db.AddInParameter(dbCommand, "@wh_degnation", DbType.[String], wh_degnation)
                db.AddInParameter(dbCommand, "@wh_addr", DbType.[String], wh_addr)
                db.AddInParameter(dbCommand, "@wh_contct", DbType.[String], wh_contct)
                db.AddInParameter(dbCommand, "@whFax", DbType.[String], whFax)
                db.AddInParameter(dbCommand, "@whEMail", DbType.[String], whEMail)
                db.AddInParameter(dbCommand, "@User_Id", DbType.[String], User_Id)
                db.AddInParameter(dbCommand, "@IP_Address", DbType.[String], IP_Address)
                db.AddInParameter(dbCommand, "@wh_status", DbType.[String], wh_status)
                db.AddInParameter(dbCommand, "@wh_image", DbType.Binary, wh_image)
                db.AddInParameter(dbCommand, "@Tag", DbType.[String], "Insert")
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
        Public Function Who_is_Who_Save_Update(wh_Id As String, wh_name As String, wh_degnation As String, wh_addr As String, wh_contct As String, whFax As String, whEMail As String, User_Id As String, IP_Address As String, wh_status As String, wh_image As Byte()) As String

            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Who_is_Who_Mt_insertupdate")
                db.AddInParameter(dbCommand, "@Prewh_Id", DbType.[String], wh_Id)
                db.AddInParameter(dbCommand, "@wh_name", DbType.[String], wh_name)
                db.AddInParameter(dbCommand, "@wh_degnation", DbType.[String], wh_degnation)
                db.AddInParameter(dbCommand, "@wh_addr", DbType.[String], wh_addr)
                db.AddInParameter(dbCommand, "@wh_contct", DbType.[String], wh_contct)
                db.AddInParameter(dbCommand, "@whFax", DbType.[String], whFax)
                db.AddInParameter(dbCommand, "@whEMail", DbType.[String], whEMail)
                db.AddInParameter(dbCommand, "@User_Id", DbType.[String], User_Id)
                db.AddInParameter(dbCommand, "@IP_Address", DbType.[String], IP_Address)
                db.AddInParameter(dbCommand, "@wh_status", DbType.[String], wh_status)
                db.AddInParameter(dbCommand, "@wh_image", DbType.Binary, wh_image)
                db.AddInParameter(dbCommand, "@Tag", DbType.[String], "Update")
                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region

#End Region

#Region "ExOfficioRGM Insert"

        Public Function ExOfficioRGM_Transfer_Save(ExOff_id As String, MO_Cd As String, ExOff_FName As String, ExOff_MName As String, ExOff_LName As String,
         GO_No As String, GO_dt As DateTime, Joining_dt As DateTime,
        Transfer_GO_No As String, Transfer_GO_dt As DateTime, Transfer_dt As DateTime,
        ExOff_Image As Byte(), ExOff_Signature As Byte(),
        doc_prf_app As Byte(), doc_prf_trans As Byte(),
        ip_address As String, designation As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_ExOfficioRGM_insert")
                db.AddInParameter(dbCommand, "@PreExOff_id", DbType.[String], ExOff_id)
                db.AddInParameter(dbCommand, "@MO_Cd", DbType.[String], MO_Cd)
                db.AddInParameter(dbCommand, "@ExOff_FName", DbType.[String], ExOff_FName)
                db.AddInParameter(dbCommand, "@ExOff_MName", DbType.[String], ExOff_MName)
                db.AddInParameter(dbCommand, "@ExOff_LName", DbType.[String], ExOff_LName)


                db.AddInParameter(dbCommand, "@GO_No", DbType.[String], GO_No)
                db.AddInParameter(dbCommand, "@GO_dt", DbType.DateTime, GO_dt)
                db.AddInParameter(dbCommand, "@Joining_dt", DbType.DateTime, Joining_dt)

                db.AddInParameter(dbCommand, "@Transfer_GO_No", DbType.[String], Transfer_GO_No)
                db.AddInParameter(dbCommand, "@TransferGO_dt", DbType.DateTime, Transfer_GO_dt)
                db.AddInParameter(dbCommand, "@Transfer_dt", DbType.DateTime, Transfer_dt)

                db.AddInParameter(dbCommand, "@ExOff_Image", DbType.Binary, ExOff_Image)
                db.AddInParameter(dbCommand, "@ExOff_Signature", DbType.Binary, ExOff_Signature)

                db.AddInParameter(dbCommand, "@doc_prf_app", DbType.Binary, doc_prf_app)
                db.AddInParameter(dbCommand, "@doc_prf_trans", DbType.Binary, doc_prf_trans)

                db.AddInParameter(dbCommand, "@IP_Address", DbType.[String], ip_address)
                db.AddInParameter(dbCommand, "@designation", DbType.[String], designation)


                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region

#Region "RTI Insert Update"

#Region "Insert"
        Public Function RTI_Save(RTI_Id As String, RTI_name As String, RTI_Autority As String, RTI_designation As String, RTI_addr As String, RTI_contct As String, RTIFax As String, RTIEMail As String, User_Id As String, IP_Address As String, RTI_Status As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_RTI_Mt_insertupdate")
                db.AddInParameter(dbCommand, "@PreRTI_Id", DbType.[String], RTI_Id)
                db.AddInParameter(dbCommand, "@RTI_name", DbType.[String], RTI_name)
                db.AddInParameter(dbCommand, "@RTI_Autority", DbType.[String], RTI_Autority)
                db.AddInParameter(dbCommand, "@RTI_designation", DbType.[String], RTI_designation)
                db.AddInParameter(dbCommand, "@RTI_addr", DbType.[String], RTI_addr)
                db.AddInParameter(dbCommand, "@RTI_contct", DbType.[String], RTI_contct)
                db.AddInParameter(dbCommand, "@RTIFax", DbType.[String], RTIFax)
                db.AddInParameter(dbCommand, "@RTIEMail", DbType.[String], RTIEMail)
                db.AddInParameter(dbCommand, "@User_Id", DbType.[String], User_Id)
                db.AddInParameter(dbCommand, "@IP_Address", DbType.[String], IP_Address)
                db.AddInParameter(dbCommand, "@Tag", DbType.[String], "Insert")
                db.AddInParameter(dbCommand, "@RTI_Status", DbType.[String], RTI_Status)
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
        Public Function RTI_Save_Update(RTI_Id As String, RTI_name As String, RTI_Autority As String, RTI_designation As String, RTI_addr As String, RTI_contct As String, RTIFax As String, RTIEMail As String, User_Id As String, IP_Address As String, RTI_Status As String) As String

            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_RTI_Mt_insertupdate")
                db.AddInParameter(dbCommand, "@PreRTI_Id", DbType.[String], RTI_Id)
                db.AddInParameter(dbCommand, "@RTI_name", DbType.[String], RTI_name)
                db.AddInParameter(dbCommand, "@RTI_Autority", DbType.[String], RTI_Autority)
                db.AddInParameter(dbCommand, "@RTI_designation", DbType.[String], RTI_designation)
                db.AddInParameter(dbCommand, "@RTI_addr", DbType.[String], RTI_addr)
                db.AddInParameter(dbCommand, "@RTI_contct", DbType.[String], RTI_contct)
                db.AddInParameter(dbCommand, "@RTIFax", DbType.[String], RTIFax)
                db.AddInParameter(dbCommand, "@RTIEMail", DbType.[String], RTIEMail)
                db.AddInParameter(dbCommand, "@User_Id", DbType.[String], User_Id)
                db.AddInParameter(dbCommand, "@IP_Address", DbType.[String], IP_Address)
                db.AddInParameter(dbCommand, "@Tag", DbType.[String], "Update")
                db.AddInParameter(dbCommand, "@RTI_Status", DbType.[String], RTI_Status)
                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region

#End Region

#Region "FAQ Insert"
        Public Function Datatable_DT_FAQ(tabname As DataTable, User_Id As String, IP_Address As String) As String

            Dim outmsg As String = String.Empty

            Try
                Dim db As Database = dbf.CreateConnection()
                Dim con As New SqlConnection(dbconnctn.GetConnectionString())
                Dim insertCommand As SqlCommand = New SqlCommand("MARREG_FAQ_Insert", con)
                insertCommand.CommandType = CommandType.StoredProcedure
                Dim tvpParam As SqlParameter = insertCommand.Parameters.AddWithValue("@tab_value", tabname)
                tvpParam.SqlDbType = SqlDbType.Structured
                Dim UserId As SqlParameter = insertCommand.Parameters.AddWithValue("@User_Id", User_Id)
                UserId.SqlDbType = SqlDbType.NVarChar
                Dim IPAddress As SqlParameter = insertCommand.Parameters.AddWithValue("@IP_Address", IP_Address)
                IPAddress.SqlDbType = SqlDbType.NVarChar
                Dim Msg As SqlParameter = insertCommand.Parameters.AddWithValue("@Msg", 100)
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

#Region "Circular Details Insert"
        Public Function Circular_insert(Circular_Type As String, CircularName As String, CircularDesc As String, Publish_Dt As String, CircularDoc As Byte()) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Circular_Details_Insert")
                db.AddInParameter(dbCommand, "@Circular_Type", DbType.[String], Circular_Type)
                db.AddInParameter(dbCommand, "@Circular_Name", DbType.[String], CircularName)
                db.AddInParameter(dbCommand, "@CircularDesc", DbType.[String], CircularDesc)
                db.AddInParameter(dbCommand, "@Publish_Dt", DbType.[String], Publish_Dt)
                db.AddInParameter(dbCommand, "@CircularDoc", DbType.Binary, CircularDoc)
                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region

#Region "Query Answer Insert"

#Region "Query insert into FAQ"
        Public Function Query_Ans_insert(Message As String, Answer As String, User_Id As String, IP_Address As String, Person_Id As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Query_Answer_InsertUpdate")
                db.AddInParameter(dbCommand, "@Message", DbType.[String], Message)
                db.AddInParameter(dbCommand, "@Answer", DbType.[String], Answer)
                db.AddInParameter(dbCommand, "@User_Id", DbType.[String], User_Id)
                db.AddInParameter(dbCommand, "@IP_Address", DbType.[String], IP_Address)
                db.AddInParameter(dbCommand, "@Person_Id", DbType.[String], Person_Id)
                db.AddInParameter(dbCommand, "@Tag", DbType.[String], "FAQ")
                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region

#Region "Update into Contact Us"
        Public Function Query_Ans_update(Message As String, Answer As String, User_Id As String, IP_Address As String, Person_Id As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Query_Answer_InsertUpdate")
                db.AddInParameter(dbCommand, "@Message", DbType.[String], Message)
                db.AddInParameter(dbCommand, "@Answer", DbType.[String], Answer)
                db.AddInParameter(dbCommand, "@User_Id", DbType.[String], User_Id)
                db.AddInParameter(dbCommand, "@IP_Address", DbType.[String], IP_Address)
                db.AddInParameter(dbCommand, "@Person_Id", DbType.[String], Person_Id)
                db.AddInParameter(dbCommand, "@Tag", DbType.[String], "Contact_Us")
                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region
#End Region

#Region "Objection Enquiry Insert"
        Public Function Objection_Enquiry_Save(Objection_Cd As String, Applicant_Cd As String, Objection_Reason As String, Attach_Evidence As String, Evidence As Byte(), Bank_Cd As String, Bank_Branch_District As String,
                                               Bank_Branch_Cd As String, Payment_Mode_Cd As String, Payment_Mode_No As String, Payment_Mode_Dt As String, Payable_At As String, Remarks As String, Status As String,
                                               IP_Address As String, User_Id As String, Mac_Address As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Objection_Enquiry_Insert")
                db.AddInParameter(dbCommand, "@Objector_auto_cd", DbType.[String], Objection_Cd)

                db.AddInParameter(dbCommand, "@App_Auto_Cd", DbType.[String], Applicant_Cd)
                db.AddInParameter(dbCommand, "@Objection_Reason", DbType.[String], Objection_Reason)
                db.AddInParameter(dbCommand, "@Attach_Evidence", DbType.[String], Attach_Evidence)
                db.AddInParameter(dbCommand, "@Evidence", DbType.[Binary], Evidence)
                db.AddInParameter(dbCommand, "@Bank_Cd", DbType.[String], Bank_Cd)
                db.AddInParameter(dbCommand, "@Bank_Branch_District", DbType.[String], Bank_Branch_District)
                db.AddInParameter(dbCommand, "@Bank_Branch_Cd", DbType.[String], Bank_Branch_Cd)
                db.AddInParameter(dbCommand, "@Payment_Mode_Cd", DbType.[String], Payment_Mode_Cd)
                db.AddInParameter(dbCommand, "@Payment_Mode_No", DbType.[String], Payment_Mode_No)
                db.AddInParameter(dbCommand, "@Payment_Mode_Dt", DbType.[String], Payment_Mode_Dt)
                db.AddInParameter(dbCommand, "@Payable_At", DbType.String, Payable_At)
                db.AddInParameter(dbCommand, "@Remarks", DbType.[String], Remarks)
                db.AddInParameter(dbCommand, "@Status", DbType.[String], Status)

                db.AddInParameter(dbCommand, "@Ip_Address", DbType.[String], IP_Address)
                db.AddInParameter(dbCommand, "@user_id", DbType.[String], User_Id)
                db.AddInParameter(dbCommand, "@Mac_Address", DbType.[String], Mac_Address)
                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region

#Region "GRIPS Response Insert"
        Public Function GRIPS_Response_Insert(dept_ref_no As String, transaction_id As String, bank_transaction_id As String, bank_cd As String, bank_tran_ts As DateTime, challan_amnt As Integer, challan_ref_id As String, challan_refId_dt As DateTime, bank_tran_status As String, bank_tran_msg As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_GRIPS_Response_Insert")
                db.AddInParameter(dbCommand, "@dept_ref_no", DbType.[String], dept_ref_no)
                db.AddInParameter(dbCommand, "@transaction_id", DbType.[String], transaction_id)
                db.AddInParameter(dbCommand, "@bank_transaction_id", DbType.[String], bank_transaction_id)
                db.AddInParameter(dbCommand, "@bank_cd", DbType.[String], bank_cd)
                db.AddInParameter(dbCommand, "@bank_tran_ts", DbType.DateTime, bank_tran_ts)
                db.AddInParameter(dbCommand, "@challan_amnt", DbType.Int16, challan_amnt)
                db.AddInParameter(dbCommand, "@challan_ref_id", DbType.[String], challan_ref_id)
                db.AddInParameter(dbCommand, "@challan_refId_dt", DbType.DateTime, challan_refId_dt)
                db.AddInParameter(dbCommand, "@bank_tran_status", DbType.[String], bank_tran_status)
                db.AddInParameter(dbCommand, "@bank_tran_msg", DbType.[String], bank_tran_msg)

                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try

            Return outmsg
        End Function
#End Region

#Region "Application Details View"
        Public Function application_details_view(Applicant_cd As String, tag As String) As DataTable
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim dt = New DataTable()
            Try
                con.Open()
                Dim da As New SqlDataAdapter("MARREG_Application_DetailsView", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure

                da.SelectCommand.Parameters.Add(New SqlParameter("@Applicant_cd", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@Applicant_cd").Value = Applicant_cd
                da.SelectCommand.Parameters.Add(New SqlParameter("@tag", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@tag").Value = tag

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

#Region "Application Details View for Objector"
        Public Function application_details_view_forObjector(Applicant_cd As String) As DataTable
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim dt = New DataTable()
            Try
                con.Open()
                Dim da As New SqlDataAdapter("MARREG_Application_DetailsView_forObjector", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure

                da.SelectCommand.Parameters.Add(New SqlParameter("@Applicant_cd", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@Applicant_cd").Value = Applicant_cd
                'da.SelectCommand.Parameters.Add(New SqlParameter("@tag", SqlDbType.NVarChar))
                'da.SelectCommand.Parameters("@tag").Value = tag

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

        '06.04.16
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
            ', Socl_Mrg_Dt As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_New_Application_Update")
                db.AddInParameter(dbCommand, "@App_Auto_cd", DbType.[String], App_cd)
                db.AddInParameter(dbCommand, "@Groom_FNam", DbType.[String], Groom_FNam)
                db.AddInParameter(dbCommand, "@Groom_MNam", DbType.[String], Groom_MNam)
                db.AddInParameter(dbCommand, "@Groom_LNam", DbType.[String], Groom_LNam)
                db.AddInParameter(dbCommand, "@Groom_Father_Nam", DbType.[String], Groom_Father_Nam)
                db.AddInParameter(dbCommand, "@Groom_Mother_Nam", DbType.[String], Groom_Mother_Nam)
                db.AddInParameter(dbCommand, "@Groom_dob", DbType.DateTime, Groom_dob)
                db.AddInParameter(dbCommand, "@Groom_Mob_Num", DbType.[String], Groom_Mob_Num)
                db.AddInParameter(dbCommand, "@Groom_Aadhr_Num", DbType.[String], Groom_Aadhr_Num)
                db.AddInParameter(dbCommand, "@Groom_Email_Id", DbType.[String], Groom_Email_Id)
                db.AddInParameter(dbCommand, "@Groom_bpl_status", DbType.[String], Groom_bpl_status)
                db.AddInParameter(dbCommand, "@Groom_Occupation", DbType.[String], Groom_occupation)

                db.AddInParameter(dbCommand, "@Bride_FNam", DbType.[String], Bride_FNam)
                db.AddInParameter(dbCommand, "@Bride_MNam", DbType.[String], Bride_MNam)
                db.AddInParameter(dbCommand, "@Bride_LNam", DbType.[String], Bride_LNam)
                db.AddInParameter(dbCommand, "@Bride_Father_Nam", DbType.[String], Bride_Father_Nam)
                db.AddInParameter(dbCommand, "@Bride_Mother_Nam", DbType.[String], Bride_Mother_Nam)
                db.AddInParameter(dbCommand, "@Bride_dob", DbType.DateTime, Bride_dob)
                db.AddInParameter(dbCommand, "@Bride_Mob_Num", DbType.[String], Bride_Mob_Num)
                db.AddInParameter(dbCommand, "@Bride_Aadhr_Num", DbType.[String], Bride_Aadhr_Num)
                db.AddInParameter(dbCommand, "@Bride_Email_Id", DbType.[String], Bride_Email_Id)
                db.AddInParameter(dbCommand, "@Bride_bpl_status", DbType.[String], Bride_bpl_status)
                db.AddInParameter(dbCommand, "@Bride_Occupation", DbType.[String], Bride_occupation)

                db.AddInParameter(dbCommand, "@applicant_bpl_status", DbType.[String], applicant_bpl_status)

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

                db.AddInParameter(dbCommand, "@Groom_Proof_PrsntAdd_Type", DbType.[String], Groom_prst_prf_type)
                db.AddInParameter(dbCommand, "@Groom_Proof_PrsntAdd", DbType.[String], Groom_prstadd_prf)
                db.AddInParameter(dbCommand, "@Groom_Proof_PrmntAdd_Type", DbType.[String], Groom_pmnt_prf_type)
                db.AddInParameter(dbCommand, "@Groom_Proof_PrmntAdd", DbType.[String], Groom_pmntadd_prf)
                db.AddInParameter(dbCommand, "@Groom_Proof_DOB_Type", DbType.[String], Groom_dobprf_type)
                db.AddInParameter(dbCommand, "@Groom_Proof_DOB", DbType.[String], Groom_dob_prf)

                db.AddInParameter(dbCommand, "@Bride_Proof_PrsntAdd_Type", DbType.[String], Bride_prst_prf_type)
                db.AddInParameter(dbCommand, "@Bride_Proof_PrsntAdd", DbType.[String], Bride_prstadd_prf)
                db.AddInParameter(dbCommand, "@Bride_Proof_PrmntAdd_Type", DbType.[String], Bride_pmnt_prf_type)
                db.AddInParameter(dbCommand, "@Bride_Proof_PrmntAdd", DbType.[String], Bride_pmntadd_prf)
                db.AddInParameter(dbCommand, "@Bride_Proof_DOB_Type", DbType.[String], Bride_dobprf_type)
                db.AddInParameter(dbCommand, "@Bride_Proof_DOB", DbType.[String], Bride_dob_prf)

                db.AddInParameter(dbCommand, "@grm_bpl_prf", DbType.[String], Groom_bpl_prf)
                db.AddInParameter(dbCommand, "@bride_bpl_prf", DbType.[String], Bride_bpl_prf)

                db.AddInParameter(dbCommand, "@gbpl_serial_cd", DbType.[String], gbpl_numbr)
                db.AddInParameter(dbCommand, "@bbpl_serial_cd", DbType.[String], bbpl_numbr)

                db.AddInParameter(dbCommand, "@User_Id", DbType.[String], User_Id)
                db.AddInParameter(dbCommand, "@IP_Address", DbType.[String], IP_Address)
                db.AddInParameter(dbCommand, "@Mac_Address", DbType.[String], Mac_Address)

                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function

        Public Function save_cert_no_update(App_cd As String, cert_num As String, flag As String, groom_or_bride As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Cert_Num_Update")
                db.AddInParameter(dbCommand, "@App_Auto_cd", DbType.[String], App_cd)
                db.AddInParameter(dbCommand, "@Cert_num", DbType.[String], cert_num)
                db.AddInParameter(dbCommand, "@Flag", DbType.[String], flag)
                db.AddInParameter(dbCommand, "@Groom_or_Bride", DbType.[String], groom_or_bride)

                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region

#Region "Social Marriage view"
        Public Function SclMarriage_Addr_View(App_cd As String) As Array
            Dim outmsg(2) As String
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_SclMarriageAddr_View")
                db.AddInParameter(dbCommand, "@App_Auto_cd", DbType.[String], App_cd)
                db.AddOutParameter(dbCommand, "@Socl_Mrg_Dt", DbType.[String], 10)
                db.AddOutParameter(dbCommand, "@Socl_Mrg_Addr", DbType.[String], 255)
                db.ExecuteNonQuery(dbCommand)
                outmsg(0) = Convert.ToString(db.GetParameterValue(dbCommand, "@Socl_Mrg_Dt"))
                outmsg(1) = Convert.ToString(db.GetParameterValue(dbCommand, "@Socl_Mrg_Addr"))
            Catch ex As Exception
                'outmsg(0) = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region

#Region "Application Address view"
        Public Function New_Appln_Addr_View(App_cd As String) As Array
            Dim outmsg(5) As String
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Application_Addr_View")
                db.AddInParameter(dbCommand, "@App_Auto_cd", DbType.[String], App_cd)
                db.AddOutParameter(dbCommand, "@Groom_Prsnt_Addr", DbType.[String], 255)
                db.AddOutParameter(dbCommand, "@Groom_Prmnt_Addr", DbType.[String], 255)
                db.AddOutParameter(dbCommand, "@Bride_Prsnt_Addr", DbType.[String], 255)
                db.AddOutParameter(dbCommand, "@Bride_Prmnt_Addr", DbType.[String], 255)
                db.AddOutParameter(dbCommand, "@Plc_Regtrn_Addr", DbType.[String], 255)
                db.ExecuteNonQuery(dbCommand)
                outmsg(0) = Convert.ToString(db.GetParameterValue(dbCommand, "@Groom_Prsnt_Addr"))
                outmsg(1) = Convert.ToString(db.GetParameterValue(dbCommand, "@Groom_Prmnt_Addr"))
                outmsg(2) = Convert.ToString(db.GetParameterValue(dbCommand, "@Bride_Prsnt_Addr"))
                outmsg(3) = Convert.ToString(db.GetParameterValue(dbCommand, "@Bride_Prmnt_Addr"))
                outmsg(4) = Convert.ToString(db.GetParameterValue(dbCommand, "@Plc_Regtrn_Addr"))
            Catch ex As Exception
                'outmsg(0) = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region
        '06.04.16

        '30.05.2016
#Region "Save Sent Mail"
        Public Function Sent_Email_Save(state_Cd As String, District_Cd As String, Sub_Division_Cd As String, PS_Cd As String, MO_Type As String, Email_Subject As String, Email_Content As String, user_Id As String, ip_Address As String) As Array
            Dim outmsg(5) As String
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Email_insert")
                db.AddInParameter(dbCommand, "@State_cd", DbType.[String], state_Cd)
                db.AddInParameter(dbCommand, "@District_Cd", DbType.[String], District_Cd)
                db.AddInParameter(dbCommand, "@sub_division_cd", DbType.[String], Sub_Division_Cd)
                db.AddInParameter(dbCommand, "@PS_cd", DbType.[String], PS_Cd)
                db.AddInParameter(dbCommand, "@Mo_Type_Id", DbType.[String], MO_Type)
                db.AddInParameter(dbCommand, "@Email_subj", DbType.[String], Email_Subject)
                db.AddInParameter(dbCommand, "@Email_content", DbType.[String], Email_Content)
                db.AddInParameter(dbCommand, "@User_id", DbType.[String], user_Id)
                db.AddInParameter(dbCommand, "@Ip_Address", DbType.[String], ip_Address)
                db.AddOutParameter(dbCommand, "@msg", DbType.[String], 50)
                db.AddOutParameter(dbCommand, "@email_cd", DbType.[String], 10)
                db.ExecuteNonQuery(dbCommand)
                outmsg(0) = Convert.ToString(db.GetParameterValue(dbCommand, "@msg"))
                outmsg(1) = Convert.ToString(db.GetParameterValue(dbCommand, "@email_cd"))
            Catch ex As Exception
                'outmsg(0) = ex.Message()
            End Try
            Return outmsg
        End Function

        Public Function MO_Sent_Email_Save(Eml_Auto_cd As String, MO_Cd As String, Status As String) As String
            Dim outmsg As String
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Email_OthrInfo_insert")
                db.AddInParameter(dbCommand, "@Email_cd", DbType.[String], Eml_Auto_cd)
                db.AddInParameter(dbCommand, "@MO_Autoincrmnt_Cd", DbType.[String], MO_Cd)
                db.AddInParameter(dbCommand, "@Email_Status", DbType.[String], Status)
                db.AddOutParameter(dbCommand, "@msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@msg"))
            Catch ex As Exception
                'outmsg(0) = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region

        '20.09.2016
#Region "Save Sent SMS"
        Public Function Sent_SMS_Save(state_Cd As String, District_Cd As String, Sub_Division_Cd As String, PS_Cd As String, MO_Type As String, SMS_Content As String, user_Id As String, ip_Address As String) As Array
            Dim outmsg(5) As String
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_SMS_insert")
                db.AddInParameter(dbCommand, "@State_cd", DbType.[String], state_Cd)
                db.AddInParameter(dbCommand, "@District_Cd", DbType.[String], District_Cd)
                db.AddInParameter(dbCommand, "@sub_division_cd", DbType.[String], Sub_Division_Cd)
                db.AddInParameter(dbCommand, "@PS_cd", DbType.[String], PS_Cd)
                db.AddInParameter(dbCommand, "@Mo_Type_Id", DbType.[String], MO_Type)
                db.AddInParameter(dbCommand, "@SMS_content", DbType.[String], SMS_Content)
                db.AddInParameter(dbCommand, "@User_id", DbType.[String], user_Id)
                db.AddInParameter(dbCommand, "@Ip_Address", DbType.[String], ip_Address)
                db.AddOutParameter(dbCommand, "@msg", DbType.[String], 50)
                db.AddOutParameter(dbCommand, "@sms_cd", DbType.[String], 10)
                db.ExecuteNonQuery(dbCommand)
                outmsg(0) = Convert.ToString(db.GetParameterValue(dbCommand, "@msg"))
                outmsg(1) = Convert.ToString(db.GetParameterValue(dbCommand, "@SMS_cd"))
            Catch ex As Exception
                'outmsg(0) = ex.Message()
            End Try
            Return outmsg
        End Function

        Public Function MO_Sent_SMS_Save(SMS_Auto_cd As String, MO_Cd As String, Status As String) As String
            Dim outmsg As String
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_SMS_OthrInfo_insert")
                db.AddInParameter(dbCommand, "@SMS_cd", DbType.[String], SMS_Auto_cd)
                db.AddInParameter(dbCommand, "@MO_Autoincrmnt_Cd", DbType.[String], MO_Cd)
                db.AddInParameter(dbCommand, "@SMS_Status", DbType.[String], Status)
                db.AddOutParameter(dbCommand, "@msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@msg"))
            Catch ex As Exception
                'outmsg(0) = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region
        'end 20.09.2016
        '02.02.2017
#Region "QR code generate"
        Public Function Gen_qrcode(qrcode As Byte(), MO_Autoincrmnt_Cd As String, tag As String) As DataTable
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim dt = New DataTable()
            Try
                con.Open()
                Dim da As New SqlDataAdapter("MARREG_MO_Qrcode_Generate", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure
                da.SelectCommand.Parameters.Add(New SqlParameter("@qrcode", SqlDbType.Binary))
                da.SelectCommand.Parameters("@qrcode").Value = qrcode
                da.SelectCommand.Parameters.Add(New SqlParameter("@MO_Autoincrmnt_Cd", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@MO_Autoincrmnt_Cd").Value = MO_Autoincrmnt_Cd
                da.SelectCommand.Parameters.Add(New SqlParameter("@tag", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@tag").Value = tag
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
        '02.02.2017
        '22.02.2017 anita
#Region "DA Details Insert Update"
#Region "Insert"
        Public Function DA_Details_Save(Pre_DA_Cd As String, state_cd As String, DA_FName As String, DA_MName As String, DA_LName As String, DA_MPh_no As String, DA_LPh_no As String,
                                 Email_Id As String, User_Email As String, Design As String, DA_dos As String, juris_levl As String, user_Id As String, ip_Address As String, status_flg As String, reason As String) As Array
            Dim outmsg(1) As String
            Try
                Dim db As Database = dbf.CreateConnection()

                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_DA_insertupdate")
                db.AddInParameter(dbCommand, "@Pre_DA_Cd", DbType.[String], Pre_DA_Cd)
                db.AddInParameter(dbCommand, "@State_cd", DbType.[String], state_cd)

                db.AddInParameter(dbCommand, "@DA_FName", DbType.[String], DA_FName)
                db.AddInParameter(dbCommand, "@DA_MName", DbType.[String], DA_MName)
                db.AddInParameter(dbCommand, "@DA_LName", DbType.[String], DA_LName)
                db.AddInParameter(dbCommand, "@DA_MPh_no", DbType.[String], DA_MPh_no)
                db.AddInParameter(dbCommand, "@DA_LPh_no", DbType.[String], DA_LPh_no)
                db.AddInParameter(dbCommand, "@Email_Id", DbType.[String], Email_Id)
                db.AddInParameter(dbCommand, "@User_Email_Id", DbType.[String], User_Email)

                db.AddInParameter(dbCommand, "@Design_Type_Id", DbType.[String], Design)
                db.AddInParameter(dbCommand, "@DOS", DbType.[String], DA_dos)
                db.AddInParameter(dbCommand, "@Status_Cd", DbType.[String], status_flg)
                db.AddInParameter(dbCommand, "@Reason", DbType.[String], reason)
                db.AddInParameter(dbCommand, "@Jurisdctn_level", DbType.[String], juris_levl)

                db.AddInParameter(dbCommand, "@User_id", DbType.[String], user_Id)
                db.AddInParameter(dbCommand, "@Ip_Address", DbType.[String], ip_Address)
                db.AddInParameter(dbCommand, "@Tag", DbType.[String], "Insert")
                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.AddOutParameter(dbCommand, "@DACd", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg(0) = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
                outmsg(1) = Convert.ToString(db.GetParameterValue(dbCommand, "@DACd"))
            Catch ex As Exception
                outmsg(0) = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region

#Region "Update"
        Public Function DA_Details_Update(Pre_DA_Cd As String, state_cd As String, DA_FName As String, DA_MName As String, DA_LName As String, DA_MPh_no As String, DA_LPh_no As String,
                                 Email_Id As String, User_Email As String, Design As String, DA_dos As String, juris_levl As String, user_Id As String, ip_Address As String, status_flg As String, reason As String) As Array
            Dim outmsg(1) As String
            Try
                Dim db As Database = dbf.CreateConnection()

                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_DA_insertupdate")
                db.AddInParameter(dbCommand, "@Pre_DA_Cd", DbType.[String], Pre_DA_Cd)
                db.AddInParameter(dbCommand, "@State_cd", DbType.[String], state_cd)

                db.AddInParameter(dbCommand, "@DA_FName", DbType.[String], DA_FName)
                db.AddInParameter(dbCommand, "@DA_MName", DbType.[String], DA_MName)
                db.AddInParameter(dbCommand, "@DA_LName", DbType.[String], DA_LName)
                db.AddInParameter(dbCommand, "@DA_MPh_no", DbType.[String], DA_MPh_no)
                db.AddInParameter(dbCommand, "@DA_LPh_no", DbType.[String], DA_LPh_no)
                db.AddInParameter(dbCommand, "@Email_Id", DbType.[String], Email_Id)
                db.AddInParameter(dbCommand, "@User_Email_Id", DbType.[String], User_Email)

                db.AddInParameter(dbCommand, "@Design_Type_Id", DbType.[String], Design)
                db.AddInParameter(dbCommand, "@DOS", DbType.[String], DA_dos)
                db.AddInParameter(dbCommand, "@Status_Cd", DbType.[String], status_flg)
                db.AddInParameter(dbCommand, "@Reason", DbType.[String], reason)
                db.AddInParameter(dbCommand, "@Jurisdctn_level", DbType.[String], juris_levl)

                db.AddInParameter(dbCommand, "@User_id", DbType.[String], user_Id)
                db.AddInParameter(dbCommand, "@Ip_Address", DbType.[String], ip_Address)
                db.AddInParameter(dbCommand, "@Tag", DbType.[String], "Update")
                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.AddOutParameter(dbCommand, "@DACd", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg(0) = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
                outmsg(1) = Convert.ToString(db.GetParameterValue(dbCommand, "@DACd"))
            Catch ex As Exception
                outmsg(0) = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region
#End Region

#Region "Marriage Registration Approval Page"
#Region "Registration approval grid veiw "
        Public Function Regtrn_Approval_gridview(user_id As String, dist As String, subdiv As String, role_type As String, app_status As String) As DataSet
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim ds = New DataSet()
            Try
                con.Open()
                Dim da As New SqlDataAdapter("MARREG_Registration_Approval_Grid_view", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure
                da.SelectCommand.Parameters.Add(New SqlParameter("@user_id", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@user_id").Value = user_id
                da.SelectCommand.Parameters.Add(New SqlParameter("@dist_cd", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@dist_cd").Value = dist
                da.SelectCommand.Parameters.Add(New SqlParameter("@subdiv_cd", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@subdiv_cd").Value = subdiv
                da.SelectCommand.Parameters.Add(New SqlParameter("@role_type", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@role_type").Value = role_type
                da.SelectCommand.Parameters.Add(New SqlParameter("@app_status", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@app_status").Value = app_status
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

#Region "MO Details in DA Approval Page"
        Public Function MO_view(MO_Cd As String) As DataSet
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim ds = New DataSet()
            Try
                con.Open()
                Dim da As New SqlDataAdapter("MARREG_MO_Detail_rpt", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure
                da.SelectCommand.Parameters.Add(New SqlParameter("@MO_Autoincrmnt_Cd", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@MO_Autoincrmnt_Cd").Value = MO_Cd
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

#Region "Registration verified by DA,SO,RGM,MO Insert"
        Public Function Registration_Approval(tabname As DataTable, User_Nm As String, User_Id As String, IP_Address As String, Mac_Address As String, role_type As String, app_status As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim con As New SqlConnection(dbconnctn.GetConnectionString())
                Dim insertCommand As SqlCommand = New SqlCommand("MARREG_Registration_Verification_Insert", con)
                insertCommand.CommandType = CommandType.StoredProcedure
                Dim tvpParam As SqlParameter = insertCommand.Parameters.AddWithValue("@tab_value", tabname)
                tvpParam.SqlDbType = SqlDbType.Structured
                Dim roletype As SqlParameter = insertCommand.Parameters.AddWithValue("@role_type", role_type)
                roletype.SqlDbType = SqlDbType.NVarChar
                Dim UserNm As SqlParameter = insertCommand.Parameters.AddWithValue("@User_Nm", User_Nm)
                UserNm.SqlDbType = SqlDbType.NVarChar
                Dim UserId As SqlParameter = insertCommand.Parameters.AddWithValue("@User_Id", User_Id)
                UserId.SqlDbType = SqlDbType.NVarChar
                Dim IPAddress As SqlParameter = insertCommand.Parameters.AddWithValue("@IP_Address", IP_Address)
                IPAddress.SqlDbType = SqlDbType.NVarChar
                Dim MACAddress As SqlParameter = insertCommand.Parameters.AddWithValue("@Mac_Address", Mac_Address)
                MACAddress.SqlDbType = SqlDbType.NVarChar
                Dim appstatus As SqlParameter = insertCommand.Parameters.AddWithValue("@app_status", app_status)
                appstatus.SqlDbType = SqlDbType.NVarChar
                Dim Msg As SqlParameter = insertCommand.Parameters.AddWithValue("@Msg", 50)
                Msg.SqlDbType = SqlDbType.NVarChar
                Msg.Size = 50
                Msg.Direction = ParameterDirection.Output
                con.Open()
                insertCommand.ExecuteNonQuery()
                outmsg = insertCommand.Parameters("@Msg").Value.ToString()
                con.Close()
                Return outmsg
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region

#Region "Registration verified by SO and RGM Update with sent sms_id and email_id"
        Public Function Registration_Approval_Update(tabname As DataTable, User_Id As String, role_type As String, app_status As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim con As New SqlConnection(dbconnctn.GetConnectionString())
                Dim insertCommand As SqlCommand = New SqlCommand("MARREG_Registration_Verification_Update", con)
                insertCommand.CommandType = CommandType.StoredProcedure
                Dim tvpParam As SqlParameter = insertCommand.Parameters.AddWithValue("@tab_value", tabname)
                tvpParam.SqlDbType = SqlDbType.Structured
                Dim UserId As SqlParameter = insertCommand.Parameters.AddWithValue("@User_Id", User_Id)
                UserId.SqlDbType = SqlDbType.NVarChar
                Dim roletype As SqlParameter = insertCommand.Parameters.AddWithValue("@role_type", role_type)
                roletype.SqlDbType = SqlDbType.NVarChar
                Dim appstatus As SqlParameter = insertCommand.Parameters.AddWithValue("@app_status", app_status)
                appstatus.SqlDbType = SqlDbType.NVarChar
                Dim Msg As SqlParameter = insertCommand.Parameters.AddWithValue("@Msg", 50)
                Msg.SqlDbType = SqlDbType.NVarChar
                Msg.Size = 50
                Msg.Direction = ParameterDirection.Output
                con.Open()
                insertCommand.ExecuteNonQuery()
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

#Region "RGM/SO Details Insert Update"
#Region "Insert"
        Public Function RGM_SO_Details_Save(Pre_RGM_SO_Cd As String, state_cd As String, RGM_SO_FName As String, RGM_SO_MName As String, RGM_SO_LName As String, RGM_SO_MPh_no As String, RGM_SO_LPh_no As String,
                                Design As String, RGM_SO_dos As String, user_Id As String, ip_Address As String, status_flg As String, reason As String) As String
            Dim outmsg As String
            Try
                Dim db As Database = dbf.CreateConnection()

                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_RGM_SO_insertupdate")
                db.AddInParameter(dbCommand, "@Pre_RGM_SO_Cd", DbType.[String], Pre_RGM_SO_Cd)
                db.AddInParameter(dbCommand, "@State_cd", DbType.[String], state_cd)

                db.AddInParameter(dbCommand, "@RGM_SO_FName", DbType.[String], RGM_SO_FName)
                db.AddInParameter(dbCommand, "@RGM_SO_MName", DbType.[String], RGM_SO_MName)
                db.AddInParameter(dbCommand, "@RGM_SO_LName", DbType.[String], RGM_SO_LName)
                db.AddInParameter(dbCommand, "@RGM_SO_MPh_no", DbType.[String], RGM_SO_MPh_no)
                db.AddInParameter(dbCommand, "@RGM_SO_LPh_no", DbType.[String], RGM_SO_LPh_no)

                db.AddInParameter(dbCommand, "@Design_Type_Id", DbType.[String], Design)
                db.AddInParameter(dbCommand, "@DOS", DbType.[String], RGM_SO_dos)
                db.AddInParameter(dbCommand, "@Status_Cd", DbType.[String], status_flg)
                db.AddInParameter(dbCommand, "@Reason", DbType.[String], reason)

                db.AddInParameter(dbCommand, "@User_id", DbType.[String], user_Id)
                db.AddInParameter(dbCommand, "@Ip_Address", DbType.[String], ip_Address)
                db.AddInParameter(dbCommand, "@Tag", DbType.[String], "Insert")
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
        Public Function RGM_SO_Details_Update(Pre_RGM_SO_Cd As String, state_cd As String, RGM_SO_FName As String, RGM_SO_MName As String, RGM_SO_LName As String, RGM_SO_MPh_no As String, RGM_SO_LPh_no As String,
                                Design As String, RGM_SO_dos As String, user_Id As String, ip_Address As String, status_flg As String, reason As String) As String
            Dim outmsg As String
            Try
                Dim db As Database = dbf.CreateConnection()

                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_RGM_SO_insertupdate")
                db.AddInParameter(dbCommand, "@Pre_RGM_SO_Cd", DbType.[String], Pre_RGM_SO_Cd)
                db.AddInParameter(dbCommand, "@State_cd", DbType.[String], state_cd)

                db.AddInParameter(dbCommand, "@RGM_SO_FName", DbType.[String], RGM_SO_FName)
                db.AddInParameter(dbCommand, "@RGM_SO_MName", DbType.[String], RGM_SO_MName)
                db.AddInParameter(dbCommand, "@RGM_SO_LName", DbType.[String], RGM_SO_LName)
                db.AddInParameter(dbCommand, "@RGM_SO_MPh_no", DbType.[String], RGM_SO_MPh_no)
                db.AddInParameter(dbCommand, "@RGM_SO_LPh_no", DbType.[String], RGM_SO_LPh_no)

                db.AddInParameter(dbCommand, "@Design_Type_Id", DbType.[String], Design)
                db.AddInParameter(dbCommand, "@DOS", DbType.[String], RGM_SO_dos)
                db.AddInParameter(dbCommand, "@Status_Cd", DbType.[String], status_flg)
                db.AddInParameter(dbCommand, "@Reason", DbType.[String], reason)

                db.AddInParameter(dbCommand, "@User_id", DbType.[String], user_Id)
                db.AddInParameter(dbCommand, "@Ip_Address", DbType.[String], ip_Address)
                db.AddInParameter(dbCommand, "@Tag", DbType.[String], "Update")
                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region
#End Region
        'end 22.02.2017

#Region "Document upload Details view" 'updated 27.04.2018
        Public Function Document_Upload_View(App_cd As String, applicnt_type As String, doc_type As String, Tag As String) As DataTable
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim dt = New DataTable()
            Try
                con.Open()
                Dim da As New SqlDataAdapter("MARREG_UplDoc_History_View", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure

                da.SelectCommand.Parameters.Add(New SqlParameter("@App_Auto_Cd", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@App_Auto_Cd").Value = App_cd
                da.SelectCommand.Parameters.Add(New SqlParameter("@Applicant_type", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@Applicant_type").Value = applicnt_type
                da.SelectCommand.Parameters.Add(New SqlParameter("@Type_of_Proof_Id", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@Type_of_Proof_Id").Value = doc_type
                da.SelectCommand.Parameters.Add(New SqlParameter("@Tag", SqlDbType.NVarChar)) '27.04.2018
                da.SelectCommand.Parameters("@Tag").Value = Tag
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

#Region "Print Cert MO"
        Public Function MO_print_status_update(MO_print_status As String, App_Auto_Cd As String, QRCODE_string As Byte()) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_MO_print_status_SP")
                db.AddInParameter(dbCommand, "@MO_print_status", DbType.[String], MO_print_status)
                db.AddInParameter(dbCommand, "@App_Auto_Cd", DbType.[String], App_Auto_Cd)
                db.AddInParameter(dbCommand, "@QRCODE_string", DbType.Binary, QRCODE_string)

                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function

        Public Function MO_Reprint_status_update(Application_No As String, Ip_Address As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_MO_Allow_Reprint_SP")
                db.AddInParameter(dbCommand, "@Application_No", DbType.[String], Application_No)
                db.AddInParameter(dbCommand, "@Ip_Address", DbType.[String], Ip_Address)

                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
        Public Function MO_print_reprint_status(MO_print_status As String, MO_reprint_status As String, App_Auto_Cd As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_MO_print_reprint_status_SP")
                db.AddInParameter(dbCommand, "@MO_print_status", DbType.[String], MO_print_status)
                db.AddInParameter(dbCommand, "@MO_reprint_status", DbType.[String], MO_reprint_status)
                db.AddInParameter(dbCommand, "@App_Auto_Cd", DbType.[String], App_Auto_Cd)

                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region
#Region "Print Correction Cert MO"
        Public Function MO_Correction_print_status_update(MO_print_status As String, App_Auto_Cd As String, QRCODE_string As Byte()) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("[MARREG_MO_Correction_print_status_SP]")
                db.AddInParameter(dbCommand, "@MO_print_status", DbType.[String], MO_print_status)
                db.AddInParameter(dbCommand, "@App_Auto_Cd", DbType.[String], App_Auto_Cd)
                db.AddInParameter(dbCommand, "@QRCODE_string", DbType.Binary, QRCODE_string)

                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region
#Region "Reprint Cert"
        Public Function MO_Reprint_status(App_Auto_Cd As String, Reprint_Reson As String, Ip_Address As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_MO_Reprint_status_SP")
                db.AddInParameter(dbCommand, "@App_Auto_Cd", DbType.[String], App_Auto_Cd)
                db.AddInParameter(dbCommand, "@Reprint_Reson", DbType.[String], Reprint_Reson)
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

#Region "Upload Image"
        Public Function NewAppSave(gimg_byte As Byte()) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_upload_cert_photo")
                db.AddInParameter(dbCommand, "@gimg_byte", DbType.Binary, gimg_byte)
                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception

                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region

#Region "User Status"
        Public Function User_Status(User_cd As String, User_type As String) As String
            Dim outmsg As String
            Try
                Dim db As Database = dbf.CreateConnection()

                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_User_Status")
                db.AddInParameter(dbCommand, "@User_cd", DbType.[String], User_cd)
                db.AddInParameter(dbCommand, "@User_design", DbType.[String], User_type)
                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region

        ' 05.04.2018
#Region "View Scanned Record"
        Public Function Scanned_Record_View(Certificate_Nbr As String, Groom_FName As String, Groom_MName As String, Groom_LName As String,
                                            Groom_DOB As String, Groom_Father_Name As String, Groom_Aadhr As String,
                                            Bride_FName As String, Bride_MName As String, Bride_LName As String,
                                            Bride_DOB As String, Bride_Father_Name As String, Bride_Aadhr As String) As DataSet
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim ds = New DataSet()
            Try
                con.Open()
                Dim da As New SqlDataAdapter("MARREG_Scanned_Record_Search", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure
                da.SelectCommand.Parameters.Add(New SqlParameter("@Certificate_Nbr", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@Certificate_Nbr").Value = Certificate_Nbr

                da.SelectCommand.Parameters.Add(New SqlParameter("@Groom_FNam", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@Groom_FNam").Value = Groom_FName

                da.SelectCommand.Parameters.Add(New SqlParameter("@Groom_MNam", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@Groom_MNam").Value = Groom_MName

                da.SelectCommand.Parameters.Add(New SqlParameter("@Groom_LNam", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@Groom_LNam").Value = Groom_LName

                da.SelectCommand.Parameters.Add(New SqlParameter("@Groom_DOB", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@Groom_DOB").Value = Groom_DOB

                da.SelectCommand.Parameters.Add(New SqlParameter("@Groom_Father_Nam", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@Groom_Father_Nam").Value = Groom_Father_Name

                da.SelectCommand.Parameters.Add(New SqlParameter("@Groom_Aadhaar", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@Groom_Aadhaar").Value = Groom_Aadhr

                da.SelectCommand.Parameters.Add(New SqlParameter("@Bride_Fnam", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@Bride_Fnam").Value = Bride_FName

                da.SelectCommand.Parameters.Add(New SqlParameter("@Bride_Mnam", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@Bride_Mnam").Value = Bride_MName

                da.SelectCommand.Parameters.Add(New SqlParameter("@Bride_Lnam", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@Bride_Lnam").Value = Bride_LName

                da.SelectCommand.Parameters.Add(New SqlParameter("@Bride_DOB", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@Bride_DOB").Value = Bride_DOB

                da.SelectCommand.Parameters.Add(New SqlParameter("@Bride_Father_Nam", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@Bride_Father_Nam").Value = Bride_Father_Name

                da.SelectCommand.Parameters.Add(New SqlParameter("@Bride_Aadhaar", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@Bride_Aadhaar").Value = Bride_Aadhr
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
        'end 05.04.2018

        'RGMCertificateSearch start 21.11.2018
#Region "Print Certificate Search by Groom and Bride Name"
        Public Function Cert_Serch_grmBride(groom_name As String, bride_name As String, groom_father_name As String, bride_father_name As String) As DataSet
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim ds = New DataSet()
            Try
                con.Open()
                Dim da As New SqlDataAdapter("MARREG_Certificate_Search_BrideGroom_Name", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure
                da.SelectCommand.Parameters.Add(New SqlParameter("@GroomName", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@GroomName").Value = groom_name
                da.SelectCommand.Parameters.Add(New SqlParameter("@BrideName", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@BrideName").Value = bride_name
                da.SelectCommand.Parameters.Add(New SqlParameter("@GroomFatherName", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@GroomFatherName").Value = groom_father_name
                da.SelectCommand.Parameters.Add(New SqlParameter("@BrideFatherName", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@BrideFatherName").Value = bride_father_name
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
        'end 21.11.2018

        'RGMCertificateSearch start 21.11.2018
#Region "MO Serach by MO CD"
        Public Function mo_search_by_mocd(mo_cd As String) As DataSet
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim ds = New DataSet()
            Try
                con.Open()
                Dim da As New SqlDataAdapter("Public_MO_view_By_MOCD", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure
                da.SelectCommand.Parameters.Add(New SqlParameter("@MO_CD", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@MO_CD").Value = mo_cd

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
        'end 21.11.2018
#Region "Pending Application And Action Against"
        Function PendingApplicationDetails() As DataSet
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim ds = New DataSet()
            Try
                con.Open()
                Dim da As New SqlDataAdapter("MARREG_GET_pendingApplication", con)
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

        Function Remove_Unused_Application(UserId As String, ip_add As String, Mac_Address As String, removeString As String) As DataSet
            Dim ds = New DataSet()
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Try
                con.Open()
                Dim da As New SqlDataAdapter("Marreg_Remove_AllUnUsed_Application", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure
                da.SelectCommand.Parameters.Add(New SqlParameter("@UserId", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@UserId").Value = UserId
                da.SelectCommand.Parameters.Add(New SqlParameter("@ip_add", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@ip_add").Value = ip_add
                da.SelectCommand.Parameters.Add(New SqlParameter("@Mac_Address", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@Mac_Address").Value = Mac_Address
                da.SelectCommand.Parameters.Add(New SqlParameter("@removeString", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@removeString").Value = removeString
                da.Fill(ds)
                Return ds
            Catch ex As Exception
                ds = Nothing
                Return ds
            End Try
        End Function

#End Region

#Region "District Registrar Email Entry"
        Public Function District_Registrar_Entry(Country_cd As String, state_Cd As String, District_Cd As String, Email As String, Registrar_type As String, delete_Status As String, user_Id As String, ip_Address As String, type As String, tag As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("District_Registrar_Email_Entry")
                db.AddInParameter(dbCommand, "@Country_cd", DbType.[String], Country_cd)
                db.AddInParameter(dbCommand, "@State_Cd", DbType.[String], state_Cd)
                db.AddInParameter(dbCommand, "@District_Cd", DbType.[String], District_Cd)
                db.AddInParameter(dbCommand, "@Email", DbType.[String], Email)
                db.AddInParameter(dbCommand, "@Registrar_type", DbType.[String], Registrar_type)
                db.AddInParameter(dbCommand, "@DeleteStatus", DbType.[String], delete_Status)

                db.AddInParameter(dbCommand, "@User_Id", DbType.[String], user_Id)
                db.AddInParameter(dbCommand, "@Ip_Address", DbType.[String], ip_Address)
                db.AddInParameter(dbCommand, "@Type", DbType.[String], type)

                db.AddInParameter(dbCommand, "@Tag", DbType.[String], tag)

                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 500)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function


#End Region

#Region "Active Mo view"
        Public Function RGM_ActiveMO_View(district As String, MO_Type_Cd As String) As DataSet
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim ds = New DataSet()
            Try
                con.Open()
                Dim da As New SqlDataAdapter("MARREG_ActiveMO_View", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure
                da.SelectCommand.Parameters.Add(New SqlParameter("@district", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@district").Value = district
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
#End Region

        '29.07.2019
#Region "DA Check Status Insert"   '12.06.2019
        Public Function DA_Check_Status(app_auto_cd As String, da_cd As String, app_status As String, remarks As String, Ip_Address As String, sts_flag As String) As String

            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_DA_Verification_Insert")
                db.AddInParameter(dbCommand, "@app_auto_cd", DbType.[String], app_auto_cd)
                db.AddInParameter(dbCommand, "@da_cd", DbType.[String], da_cd)
                db.AddInParameter(dbCommand, "@app_status", DbType.[String], app_status)
                db.AddInParameter(dbCommand, "@remarks", DbType.[String], remarks)
                db.AddInParameter(dbCommand, "@Ip_Address", DbType.[String], Ip_Address)
                db.AddInParameter(dbCommand, "@sts_flag", DbType.[String], sts_flag)
                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception

                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function

#End Region

        '29.07.2019
#Region "DA Verification GridView"        '14.06.2019
        Public Function DA_Verification_gridview(DA_Cd As String, District_cd As String, mo_cd As String, frmdt As String, todt As String, app_no As String) As DataTable
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim dt = New DataTable()
            Try
                con.Open()
                Dim da As New SqlDataAdapter("MARREG_DA_Verification_Gridview_rpt", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure
                'SqlParameter param = new SqlParameter("@flag", 2);
                da.SelectCommand.Parameters.Add(New SqlParameter("@DA_Cd", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@DA_Cd").Value = DA_Cd

                da.SelectCommand.Parameters.Add(New SqlParameter("@District_cd", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@District_cd").Value = District_cd

                da.SelectCommand.Parameters.Add(New SqlParameter("@mo_cd", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@mo_cd").Value = mo_cd

                da.SelectCommand.Parameters.Add(New SqlParameter("@frmdt", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@frmdt").Value = frmdt

                da.SelectCommand.Parameters.Add(New SqlParameter("@todt", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@todt").Value = todt

                da.SelectCommand.Parameters.Add(New SqlParameter("@App", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@App").Value = app_no



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

#Region "Joint Photo Update"
        Public Function jointPhoto_update(App_Auto_cd As String, Joint_photo As Byte()) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_JointPhoto_Update")
                db.AddInParameter(dbCommand, "@App_Auto_cd", DbType.[String], App_Auto_cd)
                db.AddInParameter(dbCommand, "@Joint_photo", DbType.Binary, Joint_photo)

                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region

        Public Function Grn_search_test(grn_no As String) As DataTable
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim dt = New DataTable()
            Try
                con.Open()
                Dim da As New SqlDataAdapter("MARREG_GRN_no_Search", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure

                da.SelectCommand.Parameters.Add(New SqlParameter("@grn_no", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@grn_no").Value = grn_no

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

#Region "Correction_Request"
        Public Function Mo_Correction_Request(Application_No As String, Mo_Autoincrement_cd As String, No_of_Correction As String, Mail_Date As String, Mail_Sent_to As String, Correction_Place As String,
                                               Incorrect As String, Correct As String, Requested_by As String, Correction_Place2 As String, Incorrect2 As String, Correct2 As String, Doc_Upload_Req As String, IP_Address As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_MO_Correction_Request_insert")
                db.AddInParameter(dbCommand, "@Application_No", DbType.[String], Application_No)
                db.AddInParameter(dbCommand, "@Mo_Autoincrement_cd", DbType.[String], Mo_Autoincrement_cd)
                db.AddInParameter(dbCommand, "@No_of_Correction", DbType.[String], No_of_Correction)
                db.AddInParameter(dbCommand, "@Mail_Date", DbType.[String], Mail_Date)
                db.AddInParameter(dbCommand, "@Mail_Sent_to", DbType.[String], Mail_Sent_to)

                db.AddInParameter(dbCommand, "@Correction_Place", DbType.[String], Correction_Place)
                db.AddInParameter(dbCommand, "@Incorrect", DbType.[String], Incorrect)
                db.AddInParameter(dbCommand, "@Correct", DbType.[String], Correct)

                db.AddInParameter(dbCommand, "@Requested_by", DbType.[String], Requested_by)

                db.AddInParameter(dbCommand, "@Correction_Place2", DbType.[String], Correction_Place2)
                db.AddInParameter(dbCommand, "@Incorrect2", DbType.[String], Incorrect2)
                db.AddInParameter(dbCommand, "@Correct2", DbType.[String], Correct2)
                db.AddInParameter(dbCommand, "@Doc_Upload_Req", DbType.[String], Doc_Upload_Req)
                db.AddInParameter(dbCommand, "@IP_Address", DbType.[String], IP_Address)

                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function

        Public Function Correction_Field_entry(Correction_field_id As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Correction_Fields_update")
                db.AddInParameter(dbCommand, "@Correction_field_id", DbType.[String], Correction_field_id)

                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function

        Public Function Correction_Field_display() As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Correction_Fields_view")

                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function

        Public Function update_photo_sign(App_Auto_cd As String, gr_photo As String, g_photo As Byte(), gr_sign As String, g_sign As Byte(), br_photo As String, b_photo As Byte(), br_sign As String, b_sign As Byte(),
                                         wt1_signn As String, wt1_sign As Byte(), wt2_signn As String, wt2_sign As Byte(), wt3_signn As String, wt3_sign As Byte(), grn_amount As String, grn_no As String, comment As String, file_path As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Sign_photo_update")
                db.AddInParameter(dbCommand, "@App_Auto_cd", DbType.[String], App_Auto_cd)
                db.AddInParameter(dbCommand, "@gr_photo", DbType.String, gr_photo)
                db.AddInParameter(dbCommand, "@groom_photo", DbType.Binary, g_photo)

                db.AddInParameter(dbCommand, "@gr_sign", DbType.String, gr_sign)
                db.AddInParameter(dbCommand, "@groom_Sign", DbType.Binary, g_sign)

                db.AddInParameter(dbCommand, "@br_photo", DbType.String, br_photo)
                db.AddInParameter(dbCommand, "@bride_photo", DbType.Binary, b_photo)

                db.AddInParameter(dbCommand, "@br_sign", DbType.String, br_sign)
                db.AddInParameter(dbCommand, "@bride_Sign", DbType.Binary, b_sign)

                db.AddInParameter(dbCommand, "@wt1_signn", DbType.String, wt1_signn)
                db.AddInParameter(dbCommand, "@wt1_sign", DbType.Binary, wt1_sign)
                db.AddInParameter(dbCommand, "@wt2_signn", DbType.String, wt2_signn)
                db.AddInParameter(dbCommand, "@wt2_sign", DbType.Binary, wt2_sign)
                db.AddInParameter(dbCommand, "@wt3_signn", DbType.String, wt3_signn)
                db.AddInParameter(dbCommand, "@wt3_sign", DbType.Binary, wt3_sign)

                db.AddInParameter(dbCommand, "@comment", DbType.String, comment)
                db.AddInParameter(dbCommand, "@grn_amount", DbType.String, grn_amount)
                db.AddInParameter(dbCommand, "@grn_no", DbType.String, grn_no)

                db.AddInParameter(dbCommand, "@file_path", DbType.String, file_path)

                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function

        Public Function Get_PhotoSign(App_auto_cd As String) As DataTable
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim dt = New DataTable()
            Try
                con.Open()
                Dim da As New SqlDataAdapter("MARREG_Photo_sign_fetch", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure

                da.SelectCommand.Parameters.Add(New SqlParameter("@App_auto_cd", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@App_auto_cd").Value = App_auto_cd

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
#Region "Update Details"
        Public Function Exofficio_Details_Update(Mo_cd As String, Mobile_no As String, Date_of_Birth As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Exofficio_Details_Update")
                db.AddInParameter(dbCommand, "@Mo_cd", DbType.[String], Mo_cd)
                db.AddInParameter(dbCommand, "@Mobile_no", DbType.[String], Mobile_no)
                db.AddInParameter(dbCommand, "@Date_of_Birth", DbType.[String], Date_of_Birth)


                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region
#Region "Fecth Bride Groom Details"
        Public Function FecthBrideGroomDetails(App_Auto_Cd As String) As DataTable
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim dt = New DataTable
            Try
                con.Open()
                Using da As New SqlDataAdapter("MARREG_FetchBrideGroomEmail", con)
                    da.SelectCommand.CommandType = CommandType.StoredProcedure
                    da.SelectCommand.Parameters.Add("@App_Auto_Cd", SqlDbType.NVarChar).Value = App_Auto_Cd
                    da.Fill(dt)
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                con.Close()
            End Try
            Return dt
        End Function
#End Region
#Region "Store Marriage Certificate"
        Public Function Save_Certificate(file_byte As Byte(), App_Auto_Cd As String) As String
            Dim outmsg As String = String.Empty
            Try
                Using cnn As New SqlConnection(dbconnctn.GetConnectionString())
                    cnn.Open()
                    Dim cmd = New SqlCommand("MARREG_SaveCertificateByte", cnn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add(New SqlParameter("@App_Auto_Cd", App_Auto_Cd))
                    cmd.Parameters.Add(New SqlParameter("@Content", file_byte))
                    cmd.Parameters.Add("@return_val", SqlDbType.SmallInt).Direction = ParameterDirection.Output
                    Try

                        cmd.ExecuteNonQuery()

                    Catch
                        Throw
                    End Try
                    outmsg = cmd.Parameters("@return_val").Value.ToString()
                    cnn.Close()
                End Using
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
        Public Function FecthCertificate(App_Auto_Cd As String) As DataTable
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim dt = New DataTable
            Try
                con.Open()
                Using da As New SqlDataAdapter("MARREG_GetCertificateByte", con)
                    da.SelectCommand.CommandType = CommandType.StoredProcedure
                    da.SelectCommand.Parameters.Add("@App_Auto_Cd", SqlDbType.NVarChar).Value = App_Auto_Cd
                    da.Fill(dt)
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                con.Close()
            End Try
            Return dt
        End Function
#End Region
#Region "MO_Check_Login_Details"
        Public Function MO_Check_Login_Details(mocd As String) As IDataReader
            Dim con2 As New SqlConnection(dbconnctn.GetConnectionString())
            Dim dr As IDataReader = Nothing
            Try
                con2.Open()
                Using cmd As SqlCommand = New SqlCommand("select count(*) count from Marreg_User_Additional_Info ua inner join aspnet_Membership m on ua.UserId=m.UserId where  MO_cd=@mocd", con2)
                    cmd.Parameters.Add("@mocd", SqlDbType.NVarChar, 20).Value = mocd
                    dr = cmd.ExecuteReader()
                End Using
            Catch
            Finally

            End Try
            Return dr
        End Function
#End Region

#Region "Feedback"

        Public Function Feedback_Save(feedbackList As List(Of Dictionary(Of String, String)), language As String, appAutoCd As String, userIP As String, browserAgent As String) As Boolean
            Dim success As Boolean = True
            Dim db As Database = dbf.CreateConnection()
            Dim cmd As DbCommand = Nothing

            Try
                For Each item In feedbackList
                    cmd = db.GetStoredProcCommand("InsertSingleFeedbackQnA")

                    db.AddInParameter(cmd, "@AppAutoCd", DbType.String, appAutoCd)
                    db.AddInParameter(cmd, "@QCd", DbType.String, item("q_cd"))
                    db.AddInParameter(cmd, "@ACd", DbType.String, item("a_cd"))
                    db.AddInParameter(cmd, "@LangCd", DbType.String, language)
                    'db.AddInParameter(cmd, "@SubmitAt", DbType.DateTime, DateTime.UtcNow)
                    db.AddInParameter(cmd, "@Remarks", DbType.String, item("remark"))
                    db.AddInParameter(cmd, "@UserIP", DbType.String, userIP)
                    db.AddInParameter(cmd, "@BrowserAgent", DbType.String, browserAgent)

                    db.ExecuteNonQuery(cmd)
                    If cmd IsNot Nothing Then CType(cmd, IDisposable).Dispose()
                Next

            Catch ex As Exception
                success = False
                ' Optionally log ex.Message
            Finally
                cmd.Dispose()
            End Try

            Return success
        End Function

        Public Function GetQuestionnaireJson() As String
            Dim resultJson As String = String.Empty
            Dim db As Database = Nothing
            Dim cmd As DbCommand = Nothing

            Try
                db = dbf.CreateConnection()
                cmd = db.GetStoredProcCommand("GetMarriageRegistrationQuestionnaire")
                resultJson = Convert.ToString(db.ExecuteScalar(cmd))
            Catch ex As Exception
                ' Optionally log the error
            Finally
                If cmd IsNot Nothing Then CType(cmd, IDisposable).Dispose()
            End Try

            Return resultJson
        End Function



        Public Function FetchQuestionnaireJson() As String
            Dim resultJson As String = String.Empty
            Dim db As Database = Nothing
            Dim cmd As DbCommand = Nothing

            Try
                db = dbf.CreateConnection()
                cmd = db.GetStoredProcCommand("GetMarriageRegistrationQuestionnaire")
                resultJson = Convert.ToString(db.ExecuteScalar(cmd))
            Catch ex As Exception
                resultJson = ""
            Finally
                If cmd IsNot Nothing Then CType(cmd, IDisposable).Dispose()
            End Try

            Return resultJson
        End Function

#End Region
    End Class
End Namespace