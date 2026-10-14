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
    Public Class MARREG_Logib_DB
        ' Declaration of connection helper classes
        Private ReadOnly dbconnctn As New DataBaseHelperDB()
        Private ReadOnly dbf As New DBFactoryDB()

        ' Method to get a SQL connection
        Private Function GetSqlConnection() As SqlConnection
            Return New SqlConnection(dbconnctn.GetConnectionString())
        End Function

        ' Unified method to execute stored procedures and return a DataTable
        Private Function ExecuteStoredProcedure(spName As String, parameters As List(Of SqlParameter)) As DataTable
            Dim dt As New DataTable()
            Using con As SqlConnection = GetSqlConnection()
                Using cmd As New SqlCommand(spName, con)
                    cmd.CommandType = CommandType.StoredProcedure
                    If parameters IsNot Nothing Then
                        cmd.Parameters.AddRange(parameters.ToArray())
                    End If
                    Using da As New SqlDataAdapter(cmd)
                        da.Fill(dt)
                    End Using
                End Using
            End Using
            Return dt
        End Function

        ' User Login
        Public Function LoginUserId(userid As String) As DataTable
            Dim parameters As New List(Of SqlParameter) From {
                New SqlParameter("@userid", SqlDbType.NVarChar) With {.Value = userid},
                New SqlParameter("@Tag", SqlDbType.NVarChar) With {.Value = "role"}
            }
            Return ExecuteStoredProcedure("MARREG_Login", parameters)
        End Function

        Public Function LoginUserDetails(userid As String) As DataTable
            Dim parameters As New List(Of SqlParameter) From {
                New SqlParameter("@userid", SqlDbType.NVarChar) With {.Value = userid},
                New SqlParameter("@Tag", SqlDbType.NVarChar) With {.Value = "userdetls"}
            }
            Return ExecuteStoredProcedure("MARREG_Login", parameters)
        End Function

        Public Function LoginUserCheck(userid As String) As String
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_GET_Online_User")
                db.AddInParameter(dbCommand, "@username", DbType.[String], userid)
                db.AddOutParameter(dbCommand, "@outMsg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                Return Convert.ToString(db.GetParameterValue(dbCommand, "@outMsg"))
            Catch ex As Exception
                Return "Error: " & ex.Message
            End Try
        End Function

        ' Log History Insert
        Public Function LogHistory(loginId As String, userId As String, loginStatus As String, ipAdd As String, macAddress As String,
                                   browser As String, inTime As DateTime) As String
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("Marreg_Login_History")
                db.AddInParameter(dbCommand, "@Logid", DbType.String, loginId)
                db.AddInParameter(dbCommand, "@UserId", DbType.String, userId)
                db.AddInParameter(dbCommand, "@login_status", DbType.String, loginStatus)
                db.AddInParameter(dbCommand, "@ip_add", DbType.String, ipAdd)
                db.AddInParameter(dbCommand, "@Mac_Address", DbType.String, macAddress)
                db.AddInParameter(dbCommand, "@browser", DbType.String, browser)
                db.AddInParameter(dbCommand, "@InTime", DbType.DateTime, inTime)
                db.AddOutParameter(dbCommand, "@login_info", DbType.String, 50)
                db.ExecuteNonQuery(dbCommand)
                Return Convert.ToString(db.GetParameterValue(dbCommand, "@login_info"))
            Catch ex As Exception
                Return "Error: " & ex.Message
            End Try
        End Function

        ' Logout History Insert
        Public Function LogoutHistory(loginId As String, loginStatus As String, ipAdd As String, pageName As String) As String
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Logout_History")
                db.AddInParameter(dbCommand, "@LogIn_Id", DbType.String, loginId)
                db.AddInParameter(dbCommand, "@Logout_Status", DbType.String, loginStatus)
                db.AddInParameter(dbCommand, "@Ip_Address", DbType.String, ipAdd)
                db.AddInParameter(dbCommand, "@page_nm", DbType.String, pageName)
                db.AddOutParameter(dbCommand, "@logout_info", DbType.String, 100)
                db.ExecuteNonQuery(dbCommand)
                Return Convert.ToString(db.GetParameterValue(dbCommand, "@logout_info"))
            Catch ex As Exception
                Return "Error: " & ex.Message
            End Try
        End Function

        ' Count Online Users
        Public Function CountOnlineUsers(typeName As String) As DataTable
            Dim parameters As New List(Of SqlParameter) From {
                New SqlParameter("@Type", SqlDbType.NVarChar) With {.Value = typeName}
            }
            Return ExecuteStoredProcedure("MARREG_Count_OnlineUser", parameters)
        End Function

        Public Function ForceFullyLogOutHistory(username As String, pagename As String) As String
            Dim outmsg As String = String.Empty
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Forcefully_LogOut")
                db.AddInParameter(dbCommand, "@username", DbType.[String], username)
                db.AddInParameter(dbCommand, "@page_nm", DbType.[String], pagename)
                db.AddOutParameter(dbCommand, "@outMsg", DbType.[String], 100)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@outMsg"))
            Catch ex As Exception
                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function

        Public Function loginuserid1(userid As String) As DataTable
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim dt = New DataTable()
            Try
                con.Open()
                Dim da As New SqlDataAdapter("MARREG_Login", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure
                da.SelectCommand.Parameters.Add(New SqlParameter("@userid", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@userid").Value = userid
                da.SelectCommand.Parameters.Add(New SqlParameter("@Tag", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@Tag").Value = "userdetls"
                da.Fill(dt)
                Return dt
            Catch ex As Exception
                Throw
            Finally
                dt.Dispose()
                con.Close()
                con.Dispose()
            End Try
        End Function

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

        Public Function countonlineuser(typename As String) As DataTable
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim dt = New DataTable()
            Try
                con.Open()
                Dim da As New SqlDataAdapter("MARREG_Count_OnlineUser", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure
                da.SelectCommand.Parameters.Add(New SqlParameter("@Type", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@Type").Value = typename
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



    End Class
End Namespace
