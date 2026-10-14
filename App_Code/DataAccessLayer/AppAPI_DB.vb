Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.VisualBasic
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports System.Configuration

Namespace MARREG.App_Code.DataAccessLayer
    Public Class AppAPI_DB
        Private ReadOnly dbconnctn As New DataBaseHelperDB()
        ' Method to get a SQL connection
        Private Function GetSqlConnection() As SqlConnection
            Return New SqlConnection(dbconnctn.GetConnectionString())
        End Function

        ' Fetch user details from stored procedure
        Public Function GetUserDetails(userid As String) As DataTable
            Dim dt As New DataTable()

            If String.IsNullOrWhiteSpace(userid) Then
                Throw New ArgumentException("Invalid Username", "userid")
            End If

            Using con As SqlConnection = GetSqlConnection()
                Using cmd As New SqlCommand("GetAppUserAdditionalInfo", con)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add(New SqlParameter("@UserName", SqlDbType.NVarChar, 255)).Value = userid

                    Try
                        con.Open()
                        Using da As New SqlDataAdapter(cmd)
                            da.Fill(dt)
                        End Using
                    Catch ex As SqlException
                        Throw New Exception("Database error: " & ex.Message, ex)
                    Finally
                        If con.State = ConnectionState.Open Then
                            con.Close()
                        End If
                    End Try
                End Using
            End Using

            Return dt
        End Function

        Public Function GetApplicationList(mocode As String) As DataTable
            Dim dt As New DataTable()

            If String.IsNullOrWhiteSpace(mocode) Then
                Throw New ArgumentException("Invalid Username", "mocode")
            End If

            Using con As SqlConnection = GetSqlConnection()
                Using cmd As New SqlCommand("GetAppApplicationList", con)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add(New SqlParameter("@MO_Autoincrmnt_Cd", SqlDbType.NVarChar, 255)).Value = mocode

                    Try
                        con.Open()
                        Using da As New SqlDataAdapter(cmd)
                            da.Fill(dt)
                        End Using
                    Catch ex As SqlException
                        Throw New Exception("Database error: " & ex.Message, ex)
                    Finally
                        If con.State = ConnectionState.Open Then
                            con.Close()
                        End If
                    End Try
                End Using
            End Using

            Return dt
        End Function


        Public Function GetCertifiedCopyReasons() As DataTable
            Dim dt As New DataTable()

            Using con As SqlConnection = GetSqlConnection()
                Using cmd As New SqlCommand("GetCertifiedCopyReasonList", con)
                    cmd.CommandType = CommandType.StoredProcedure

                    Try
                        con.Open()
                        Using da As New SqlDataAdapter(cmd)
                            da.Fill(dt)
                        End Using
                    Catch ex As SqlException
                        Throw New Exception("Database error: " & ex.Message, ex)
                    Finally
                        If con.State = ConnectionState.Open Then
                            con.Close()
                        End If
                    End Try
                End Using
            End Using

            Return dt
        End Function


    End Class
End Namespace