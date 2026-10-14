Imports MARREG.App_Code.DataAccessLayer
Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient

Public Class MARREG_App_Payment_DB
#Region "Declaration"
    Public dbconnctn As New DataBaseHelperDB()
    Private con As New SqlConnection(dbconnctn.GetConnectionString())
    Public Property id As Int64
    Public Property app_id As String
    Public Property MO_cd As String
    Public Property total_amt As Int64
    Public Property c_time As DateTime
    Public Property e_time As DateTime
    Public Property status As String
    Public Property grn As String
    Public Property draft_cd As String
#End Region
#Region "Methods"
    Public Function Save_App_Payment_Init() As String
        Dim ReturnVal As String
        Try
            con.Open()
            Using command As New SqlCommand("MARREG_Save_App_Payment_Init", con)
                command.CommandType = CommandType.StoredProcedure
                command.Parameters.Add("@id", SqlDbType.BigInt).Value = 0
                command.Parameters.Add("@Draft_Applicant_Cd", SqlDbType.NVarChar).Value = app_id
                command.Parameters.Add("@MO_Autoincrement_cd", SqlDbType.NVarChar).Value = MO_cd
                command.Parameters.Add("@total_amount", SqlDbType.BigInt).Value = total_amt
                command.Parameters.Add("@Link_Sent_On", SqlDbType.DateTime).Value = c_time
                command.Parameters.Add("@Link_Valid_Upto", SqlDbType.DateTime).Value = e_time
                command.Parameters.Add("@PaymentStatus", SqlDbType.NVarChar).Value = status
                command.Parameters.Add("@draft_cd", SqlDbType.NVarChar).Value = draft_cd

                command.Parameters.Add("@return_val", SqlDbType.SmallInt).Direction = ParameterDirection.Output
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
    Public Function ValidatePaymentLink() As String
        Dim ReturnVal As String
        Try
            con.Open()
            Using command As New SqlCommand("MARREG_ValidatePaymentLink", con)
                command.CommandType = CommandType.StoredProcedure
                command.Parameters.Add("@id", SqlDbType.BigInt).Value = id
                command.Parameters.Add("@Draft_Applicant_Cd", SqlDbType.NVarChar).Value = app_id
                command.Parameters.Add("@c_time", SqlDbType.DateTime).Value = c_time

                command.Parameters.Add("@return_val", SqlDbType.SmallInt).Direction = ParameterDirection.Output
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
    Public Function UpdateGRN_App_Payment_Init() As String
        Dim ReturnVal As String
        Try
            con.Open()
            Using command As New SqlCommand("MARREG_UpdateGRN_App_Payment_Init", con)
                command.CommandType = CommandType.StoredProcedure
                command.Parameters.Add("@id", SqlDbType.BigInt).Value = id
                command.Parameters.Add("@Draft_Applicant_Cd", SqlDbType.NVarChar).Value = app_id
                command.Parameters.Add("@GRN", SqlDbType.NVarChar).Value = grn
                command.Parameters.Add("@Payment_Date", SqlDbType.DateTime).Value = c_time

                command.Parameters.Add("@return_val", SqlDbType.SmallInt).Direction = ParameterDirection.Output
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
    Public Function FetchPaymentStatus() As DataSet
        Dim ds As New DataSet
        Try
            con.Open()
            Using da As New SqlDataAdapter("MARREG_FetchPaymentStatus", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure
                da.SelectCommand.Parameters.Add("@Draft_Applicant_Cd", SqlDbType.NVarChar).Value = app_id
                da.SelectCommand.Parameters.Add("@draft_cd", SqlDbType.NVarChar).Value = draft_cd
                da.SelectCommand.Parameters.Add("@MO_cd", SqlDbType.NVarChar).Value = MO_cd

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
End Class
