Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Collections.Generic
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports System.Configuration
Imports System.Data.SqlClient
Namespace MARREG.App_Code.DataAccessLayer
    Public Class MARREG_Account_DB
#Region "DECLARATION"
        Public dbconnctn As New DataBaseHelperDB()
        Public dbf As New DBFactoryDB()
#End Region

#Region "Marreg  Account View Gridview"
        Public Function Marreg_Account_View(From_dt As String, To_dt As String) As DataTable
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim dt = New DataTable()
            Try
                con.Open()
                Dim da As New SqlDataAdapter("MARREG_Account_Details_Gridview", con)
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
#Region "Marreg  MO Payment Gridview"
        Public Function Marreg_MO_Payment_GridView() As DataTable
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim dt = New DataTable()
            Try
                con.Open()
                Dim da As New SqlDataAdapter("MARREG_Account_Disburse_Gridview", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure
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
#Region "MO Payment ViewDetails  Gridview"
        Public Function View_MOPayment_gridvw() As DataTable
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim dt = New DataTable()
            Try
                con.Open()
                Dim da As New SqlDataAdapter("MARREG_MO_Payment_ViewDetails_Gridview", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure
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
#Region "MO Disburse Amount  Insert "
        Public Function Datatable_MO_Amount_Disburse(tabname As DataTable, User_Id As String, IP_Address As String) As String
            'Dim outmsg As String = String.Empty
            'Try
            '    Dim db As Database = dbf.CreateConnection()
            '    Dim dbCommand As DbCommand = db.GetStoredProcCommand("MO_Disburse_Amount_Insert_To_MARREG_Transaction_Details")
            '    db.AddInParameter(dbCommand, "@XMLdata", DbType.Xml, tabname)

            '    db.AddInParameter(dbCommand, "@User_Id", DbType.String, User_Id)
            '    db.AddInParameter(dbCommand, "@IP_Address", DbType.String, IP_Address)
            '    db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
            '    db.ExecuteNonQuery(dbCommand)
            '    outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            '    Return outmsg
            'Catch ex As Exception
            '    outmsg = ex.Message()
            'End Try
            'Return outmsg

            Dim outmsg As String = String.Empty

            Try
                Dim db As Database = dbf.CreateConnection()
                Dim con As New SqlConnection(dbconnctn.GetConnectionString())
                Dim insertCommand As SqlCommand = New SqlCommand("MO_Disburse_Amount_Insert_To_MARREG_Transaction_Details", con)
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
                outmsg = insertCommand.Parameters("@Msg").Value.ToString()
                con.Close()
                Return outmsg
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg



        End Function
#End Region
    End Class
End Namespace
