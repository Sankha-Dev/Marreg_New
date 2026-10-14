Imports MARREG.App_Code.DataAccessLayer
Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient

Namespace MARREG.App_Code
    Public Class CertifiedNotice_DB
#Region "Declaration"
        Public dbconnctn As New DataBaseHelperDB()
        Private con As New SqlConnection(dbconnctn.GetConnectionString())
        Public Property email As String
        Public Property groom_name As String
        Public Property bride_name As String
        Public Property groom_Fname As String
        Public Property bride_Fname As String
        Public Property groom_dobs As String
        Public Property bride_dobs As String
        Public Property reg_dates As String
        Public Property mo_name As String
        Public Property actCd As String
        Public Property name As String
        Public Property address As String
        Public Property mob_no As String
        Public Property reason As String
        Public Property id As Int64
        Public Property otp As String
        Public Property grn_id As Int64
        Public Property no_copy As Int32
#End Region
#Region "Methods"
        Public Function CheckEmailExist() As DataSet
            Dim ds = New DataSet()
            Try
                con.Open()
                Using da As New SqlDataAdapter("WS_CheckEmailExist_Notice", con)
                    da.SelectCommand.CommandType = CommandType.StoredProcedure
                    da.SelectCommand.Parameters.Add("@email", SqlDbType.VarChar).Value = email
                    da.Fill(ds)
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                con.Close()
            End Try
            Return ds
        End Function
#Region "Certified Copy Request Old Records"
        Public Function CertifiedCopyRequestInfoValidateOldNotice() As String
            Dim ReturnVal As String
            Try
                con.Open()
                Using command As New SqlCommand("WS_CertifiedCopyRequestInfoValidateOld_Notice", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@groom_name", SqlDbType.NVarChar).Value = groom_name
                    command.Parameters.Add("@bride_name", SqlDbType.NVarChar).Value = bride_name
                    command.Parameters.Add("@groom_Fname", SqlDbType.NVarChar).Value = groom_Fname
                    command.Parameters.Add("@bride_Fname", SqlDbType.NVarChar).Value = bride_Fname
                    command.Parameters.Add("@groom_dobs", SqlDbType.NVarChar).Value = groom_dobs
                    command.Parameters.Add("@bride_dobs", SqlDbType.NVarChar).Value = bride_dobs
                    command.Parameters.Add("@reg_dates", SqlDbType.NVarChar).Value = reg_dates
                    command.Parameters.Add("@mo_name", SqlDbType.NVarChar).Value = mo_name
                    command.Parameters.Add("@ActCd", SqlDbType.NVarChar).Value = actCd
                    command.Parameters.Add("@return_val", SqlDbType.BigInt).Direction = ParameterDirection.Output
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
        Public Function CertifiedCopyRequestSaveEmailOldNotice() As String
            Dim ReturnVal As String
            Try
                con.Open()
                Using command As New SqlCommand("WS_CertifiedCopyRequestSaveEmailOld_Notice", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@name", SqlDbType.NVarChar).Value = name
                    command.Parameters.Add("@address", SqlDbType.NVarChar).Value = address
                    command.Parameters.Add("@mob_no", SqlDbType.NVarChar).Value = mob_no
                    command.Parameters.Add("@email", SqlDbType.NVarChar).Value = email
                    command.Parameters.Add("@id", SqlDbType.BigInt).Value = id
                    command.Parameters.Add("@return_val", SqlDbType.NVarChar, 8).Direction = ParameterDirection.Output
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
        Public Function CertifiedCopyRequestEmailOTPValidateOldNotice() As Int16
            Dim ReturnVal As Int16
            Try
                con.Open()
                Using command As New SqlCommand("WS_CertifiedCopyRequestEmailOTPValidateOldNotice", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@otp", SqlDbType.NVarChar).Value = otp
                    command.Parameters.Add("@id", SqlDbType.BigInt).Value = id
                    command.Parameters.Add("@return_val", SqlDbType.SmallInt).Direction = ParameterDirection.Output
                    command.ExecuteNonQuery()
                    ReturnVal = Convert.ToInt16(command.Parameters("@return_val").Value.ToString())
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                con.Close()
            End Try
            Return ReturnVal
        End Function
        Public Function CertifiedCopyRequestSendOldNotice() As DataTable
            Dim ds = New DataSet()
            Try
                con.Open()
                Using da As New SqlDataAdapter("WS_CertifiedCopyRequestSendOldNotice", con)
                    da.SelectCommand.CommandType = CommandType.StoredProcedure
                    da.SelectCommand.Parameters.Add("@grn_id", SqlDbType.BigInt).Value = grn_id
                    da.SelectCommand.Parameters.Add("@id", SqlDbType.BigInt).Value = id
                    da.SelectCommand.Parameters.Add("@no_copy", SqlDbType.Int).Value = no_copy
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
    End Class
End Namespace
