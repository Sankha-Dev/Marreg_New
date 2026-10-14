Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports MARREG.App_Code.DataAccessLayer

Namespace MARREG.App_Code
    Public Class WebService_DB
#Region "DECLARATION"
        Public dbconnctn As New DataBaseHelperDB()
        '    Public dbf As New DBFactoryDB()
        Private con As New SqlConnection(dbconnctn.GetConnectionString())
        Public Property location As String
        Public Property Applicant_Cd As String
        Public Property gimg_byte As Byte()
        Public Property Mo_Cd As String

        Public Property GRN_No As String
        Public Property total_amount As Int64
        Public Property Payment_Reason As String

        Public Property id As Int64
        Public Property appl_id As String
        Public Property App_Auto_Cd As String

        Public Property name As String
        Public Property address As String
        Public Property mob_no As String
        Public Property email As String
        Public Property reason As String

        Public Property groom_name As String
        Public Property bride_name As String
        Public Property certificate_no As String
        Public Property groom_dobs As String
        Public Property bride_dobs As String
        Public Property reg_dates As String
        Public Property grn_id As Int64

        Public Property start As Integer
        Public Property length As Integer
        Public Property value As String
        Public Property order_by As String
        Public Property orderByDir As String

        Public Property status As String
        Public Property mode As Int16
        Public Property no_copy As Int32

        Public Property reqid As String
        Public Property mo_name As String

        Public Property groom_Fname As String
        Public Property bride_Fname As String
        Public Property actCd As String
        Public Property groom_aadhaar_no As String
        Public Property bride_aadhaar_no As String
        Public Property application_no As String
        Public Property reg_date_from As String
        Public Property reg_date_to As String

        Public Property otp As String
        Public Property req_outsider_id As Int64
        Public Property lngReqId As Int64
        Public Property comment As String
        Public Property hearing_dateS As String
        Public Property dobs As String
        Public Property user_name As String
        Public Property mdatas As String

        Public Property req_to As String
        Public Property sm As String
        Public Property hm As String
        Public Property juri_location As String
        Public Property dpr As String
        Public Property docket_no As String
#End Region

        Public Function SaveOtherInfoMarriage() As String
            Dim ReturnVal As String
            Try
                con.Open()
                Using command As New SqlCommand("WS_SaveOtherInfoMarriage", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.AddWithValue("@Location", DirectCast(location, String))
                    command.Parameters.Add("@Applicant_Cd", SqlDbType.VarChar).Value = Applicant_Cd
                    command.Parameters.Add("@Joint_PhotoWithMo", SqlDbType.Binary).Value = gimg_byte
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

        Public Function ValidateAppCd() As String
            Dim ReturnVal As String
            Try
                con.Open()
                Using command As New SqlCommand("WS_ValidateAppCd", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@Applicant_Cd", SqlDbType.VarChar).Value = Applicant_Cd
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

        Public Function Save_GRN_Details_Ag_Search() As String
            Dim ReturnVal As String
            Try
                con.Open()
                Using command As New SqlCommand("WS_Save_GRN_Details_Ag_Search", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@GRN_No", SqlDbType.NVarChar).Value = GRN_No
                    command.Parameters.Add("@total_amount", SqlDbType.BigInt).Value = total_amount
                    command.Parameters.Add("@MARREG_Payment_Reason", SqlDbType.NVarChar).Value = Payment_Reason
                    command.Parameters.Add("@reason", SqlDbType.NVarChar).Value = reason
                    command.Parameters.Add("@client_ip", SqlDbType.NVarChar).Value = Common.getIPAddress()
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

        Public Function Update_GRN_Details_Ag_Print() As String
            Dim ReturnVal As String
            Try
                con.Open()
                Using command As New SqlCommand("WS_Save_GRN_Details_Ag_Search", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@id", SqlDbType.BigInt).Value = id
                    command.Parameters.Add("@old_appl_id", SqlDbType.NVarChar).Value = appl_id
                    command.Parameters.Add("@App_Auto_Cd", SqlDbType.NVarChar).Value = App_Auto_Cd
                    command.Parameters.Add("@MARREG_Payment_Reason", SqlDbType.NVarChar).Value = Payment_Reason
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

        Public Function CheakEmailExist() As DataSet
            Dim ds = New DataSet()
            Try
                con.Open()
                Using da As New SqlDataAdapter("WS_CheakEmailExist", con)
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
#Region "Search Marriage Record"
        Public Function Save_SearchRecordData() As Int64
            Dim ReturnVal As Int64
            Try
                con.Open()
                Using command As New SqlCommand("WS_Save_SearchRecordData", con)
                    command.CommandType = CommandType.StoredProcedure

                    command.Parameters.Add("@groom_name", SqlDbType.NVarChar).Value = groom_name
                    command.Parameters.Add("@groom_Fname", SqlDbType.NVarChar).Value = groom_Fname
                    command.Parameters.Add("@bride_name", SqlDbType.NVarChar).Value = bride_name
                    command.Parameters.Add("@bride_Fname", SqlDbType.NVarChar).Value = bride_Fname
                    command.Parameters.Add("@actCd", SqlDbType.NVarChar).Value = actCd
                    command.Parameters.Add("@mo_name", SqlDbType.NVarChar).Value = mo_name
                    command.Parameters.Add("@groom_aadhaar_no", SqlDbType.NVarChar).Value = groom_aadhaar_no
                    command.Parameters.Add("@bride_aadhaar_no", SqlDbType.NVarChar).Value = bride_aadhaar_no
                    command.Parameters.Add("@application_no", SqlDbType.NVarChar).Value = application_no
                    command.Parameters.Add("@groom_dobs", SqlDbType.NVarChar).Value = groom_dobs
                    command.Parameters.Add("@bride_dobs", SqlDbType.NVarChar).Value = bride_dobs
                    command.Parameters.Add("@reg_dates", SqlDbType.NVarChar).Value = reg_dates
                    command.Parameters.Add("@reg_date_from", SqlDbType.NVarChar).Value = reg_date_from
                    command.Parameters.Add("@reg_date_to", SqlDbType.NVarChar).Value = reg_date_to
                    command.Parameters.Add("@grn_id", SqlDbType.BigInt).Value = grn_id
                    command.Parameters.Add("@App_Auto_Cd", SqlDbType.NVarChar).Value = App_Auto_Cd

                    command.Parameters.Add("@return_val", SqlDbType.BigInt).Direction = ParameterDirection.Output
                    command.ExecuteNonQuery()
                    ReturnVal = Convert.ToInt64(command.Parameters("@return_val").Value.ToString())
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                con.Close()
            End Try
            Return ReturnVal
        End Function
        Public Function Details_SearchRecordData() As DataSet
            Dim ds = New DataSet()
            Try
                con.Open()
                Using da As New SqlDataAdapter("WS_Details_SearchRecordData", con)
                    da.SelectCommand.CommandType = CommandType.StoredProcedure
                    da.SelectCommand.Parameters.Add("@SearchRecId", SqlDbType.BigInt).Value = reqid
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
#Region "Search Old Data"
        Public Function SearchRequestInfoValidateOld() As String
            Dim ReturnVal As String
            Try
                con.Open()
                Using command As New SqlCommand("WS_SearchRequestInfoValidateOld", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@groom_name", SqlDbType.NVarChar).Value = groom_name
                    command.Parameters.Add("@bride_name", SqlDbType.NVarChar).Value = bride_name
                    command.Parameters.Add("@groom_dobs", SqlDbType.NVarChar).Value = groom_dobs
                    command.Parameters.Add("@bride_dobs", SqlDbType.NVarChar).Value = bride_dobs
                    command.Parameters.Add("@groom_Fname", SqlDbType.NVarChar).Value = groom_Fname
                    command.Parameters.Add("@bride_Fname", SqlDbType.NVarChar).Value = bride_Fname
                    command.Parameters.Add("@mo_name", SqlDbType.NVarChar).Value = mo_name
                    command.Parameters.Add("@ActCd", SqlDbType.NVarChar).Value = actCd
                    command.Parameters.Add("@reg_date_from", SqlDbType.NVarChar).Value = reg_date_from
                    command.Parameters.Add("@reg_date_to", SqlDbType.NVarChar).Value = reg_date_to
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
        Public Function SearchRequestSaveEmailOld() As String
            Dim ReturnVal As String
            Try
                con.Open()
                Using command As New SqlCommand("WS_SearchRequestSaveEmailOld", con)
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
        Public Function SearchRequestEmailOTPValidateOld() As Int16
            Dim ReturnVal As Int16
            Try
                con.Open()
                Using command As New SqlCommand("WS_SearchRequestEmailOTPValidateOld", con)
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
        Public Function SearchRequestSendOld() As DataTable
            Dim ds = New DataSet()
            Try
                con.Open()
                Using da As New SqlDataAdapter("WS_SearchRequestSendOld", con)
                    da.SelectCommand.CommandType = CommandType.StoredProcedure
                    da.SelectCommand.Parameters.Add("@grn_id", SqlDbType.BigInt).Value = grn_id
                    da.SelectCommand.Parameters.Add("@id", SqlDbType.BigInt).Value = id
                    da.SelectCommand.Parameters.Add("@reason", SqlDbType.NVarChar).Value = reason
                    da.Fill(ds)
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                con.Close()
            End Try
            Return ds.Tables(0)
        End Function
        Function SearchRequestDetailsOld() As IDataReader
            Dim dr As IDataReader = Nothing
            Try
                con.Open()
                Using command As New SqlCommand("MARREG_SearchRequestDetailsOld", con)
                    command.CommandType = CommandType.StoredProcedure
                    'command.Parameters.Add("@Mo_Cd", SqlDbType.NVarChar).Value = Mo_Cd
                    'command.Parameters.Add("@req_to", SqlDbType.NVarChar).Value = req_to
                    command.Parameters.Add("@status", SqlDbType.NVarChar).Value = status
                    command.Parameters.Add("@start", SqlDbType.Int).Value = start
                    command.Parameters.Add("@length", SqlDbType.Int).Value = length
                    command.Parameters.Add("@value", SqlDbType.NVarChar).Value = value
                    command.Parameters.Add("@order_by", SqlDbType.NVarChar).Value = order_by
                    command.Parameters.Add("@orderByDir", SqlDbType.NVarChar).Value = orderByDir
                    dr = command.ExecuteReader()
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                'con.Close()
            End Try
            Return dr
        End Function
        Public Function Save_SearchRequestApproveOld() As String
            Dim ReturnVal As String
            Try
                con.Open()
                Using command As New SqlCommand("WS_Save_SearchRequestApproveOld", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@id", SqlDbType.BigInt).Value = id
                    command.Parameters.Add("@status", SqlDbType.NVarChar).Value = status
                    command.Parameters.Add("@otp", SqlDbType.NVarChar).Value = otp
                    command.Parameters.Add("@user_name", SqlDbType.NVarChar).Value = user_name
                    command.Parameters.Add("@groom_name", SqlDbType.NVarChar).Value = groom_name
                    command.Parameters.Add("@bride_name", SqlDbType.NVarChar).Value = bride_name
                    command.Parameters.Add("@reg_dates", SqlDbType.NVarChar).Value = reg_dates
                    command.Parameters.Add("@actCd", SqlDbType.NVarChar).Value = actCd
                    command.Parameters.Add("@mo_name", SqlDbType.NVarChar).Value = mo_name
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
        Public Function FetchSearchRequestDetailOld() As DataSet
            Dim ds As New DataSet
            Try
                con.Open()
                Using da As New SqlDataAdapter("WS_FetchSearchRequestDetailOld", con)
                    da.SelectCommand.CommandType = CommandType.StoredProcedure
                    da.SelectCommand.Parameters.Add("@id", SqlDbType.BigInt).Value = id
                    da.SelectCommand.Parameters.Add("@status", SqlDbType.NVarChar).Value = status
                    da.Fill(ds)
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                con.Close()
            End Try
            Return ds
        End Function
        Public Function Details_SearchRecordDataOld() As DataSet
            Dim ds = New DataSet()
            Try
                con.Open()
                Using da As New SqlDataAdapter("WS_Details_SearchRecordDataOld", con)
                    da.SelectCommand.CommandType = CommandType.StoredProcedure
                    da.SelectCommand.Parameters.Add("@SearchRecId", SqlDbType.BigInt).Value = reqid
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
#Region "Certified Copy Request"
        Public Function CertifiedCopyRequestInfoValidateInsider() As String
            Dim ReturnVal As String
            Try
                con.Open()
                Using command As New SqlCommand("WS_CertifiedCopyRequestInfoValidateInsider", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@email", SqlDbType.NVarChar).Value = email
                    command.Parameters.Add("@groom_name", SqlDbType.NVarChar).Value = groom_name
                    command.Parameters.Add("@bride_name", SqlDbType.NVarChar).Value = bride_name
                    command.Parameters.Add("@mo_name", SqlDbType.NVarChar).Value = mo_name
                    command.Parameters.Add("@groom_dobs", SqlDbType.NVarChar).Value = groom_dobs
                    command.Parameters.Add("@bride_dobs", SqlDbType.NVarChar).Value = bride_dobs
                    command.Parameters.Add("@reg_dates", SqlDbType.NVarChar).Value = reg_dates
                    command.Parameters.Add("@certificate_no", SqlDbType.NVarChar).Value = certificate_no

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
        Public Function Save_CertifiedCopyRequest() As String
            Dim ReturnVal As String
            Try
                con.Open()
                Using command As New SqlCommand("WS_Save_CertifiedCopyRequest", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@email", SqlDbType.NVarChar).Value = email
                    command.Parameters.Add("@groom_name", SqlDbType.NVarChar).Value = groom_name
                    command.Parameters.Add("@bride_name", SqlDbType.NVarChar).Value = bride_name
                    command.Parameters.Add("@groom_dobs", SqlDbType.NVarChar).Value = groom_dobs
                    command.Parameters.Add("@bride_dobs", SqlDbType.NVarChar).Value = bride_dobs
                    command.Parameters.Add("@reg_dates", SqlDbType.NVarChar).Value = reg_dates
                    command.Parameters.Add("@certificate_no", SqlDbType.NVarChar).Value = certificate_no
                    command.Parameters.Add("@grn_id", SqlDbType.BigInt).Value = grn_id
                    command.Parameters.Add("@no_copy", SqlDbType.Int).Value = no_copy
                    command.Parameters.Add("@mo_name", SqlDbType.NVarChar).Value = mo_name
                    command.Parameters.Add("@return_val", SqlDbType.NVarChar, 20).Direction = ParameterDirection.Output
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
        Public Function Save_CertifiedCopyRequestRe() As String
            Dim ReturnVal As String
            Try
                con.Open()
                Using command As New SqlCommand("WS_Save_CertifiedCopyRequestRe", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@email", SqlDbType.NVarChar).Value = email
                    command.Parameters.Add("@mo_name", SqlDbType.NVarChar).Value = mo_name
                    command.Parameters.Add("@reqid", SqlDbType.NVarChar).Value = reqid
                    command.Parameters.Add("@return_val", SqlDbType.NVarChar, 20).Direction = ParameterDirection.Output
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
#Region "Certified Copy Outsider"
        Public Function CertifiedCopyRequestInfoValidate() As String
            Dim ReturnVal As String
            Try
                con.Open()
                Using command As New SqlCommand("WS_CertifiedCopyRequestInfoValidate", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@groom_name", SqlDbType.NVarChar).Value = groom_name
                    command.Parameters.Add("@bride_name", SqlDbType.NVarChar).Value = bride_name
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
        Public Function CertifiedCopyRequestSaveEmailOutsider() As String
            Dim ReturnVal As String
            Try
                con.Open()
                Using command As New SqlCommand("WS_CertifiedCopyRequestSaveEmailOutsider", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@name", SqlDbType.NVarChar).Value = name
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
        Public Function CertifiedCopyRequestEmailOTPValidate() As Int16
            Dim ReturnVal As Int16
            Try
                con.Open()
                Using command As New SqlCommand("WS_CertifiedCopyRequestEmailOTPValidate", con)
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
        Public Function CertifiedCopyRequestSendOutsider() As DataTable
            Dim ds = New DataSet()
            Try
                con.Open()
                Using da As New SqlDataAdapter("WS_CertifiedCopyRequestSendOutsider", con)
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
#Region "Certified Copy Request Objection"
        Public Function CertifiedCopy_Request_Id_Validate() As Int64
            Dim ReturnVal As Int64
            Try
                con.Open()
                Using command As New SqlCommand("MARREG_CertifiedCopy_Request_Id_Validate", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@RequestId", SqlDbType.NVarChar).Value = reqid
                    command.Parameters.Add("@return_val", SqlDbType.BigInt).Direction = ParameterDirection.Output
                    command.ExecuteNonQuery()
                    ReturnVal = Convert.ToInt64(command.Parameters("@return_val").Value.ToString())
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                con.Close()
            End Try
            Return ReturnVal
        End Function
        Public Function Validate_CertifiedCopy_Objector() As Int32
            Dim ReturnVal As Int32
            Try
                con.Open()
                Using command As New SqlCommand("WS_Validate_CertifiedCopy_Objector", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@name", SqlDbType.NVarChar).Value = name
                    command.Parameters.Add("@dobs", SqlDbType.NVarChar).Value = dobs
                    command.Parameters.Add("@email", SqlDbType.NVarChar).Value = email
                    command.Parameters.Add("@reqId", SqlDbType.BigInt).Value = lngReqId
                    command.Parameters.Add("@return_val", SqlDbType.Int).Direction = ParameterDirection.Output
                    command.ExecuteNonQuery()
                    ReturnVal = Convert.ToInt32(command.Parameters("@return_val").Value.ToString())
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                con.Close()
            End Try
            Return ReturnVal
        End Function
        Public Function CertifiedCopyCommentSubmit() As String
            Dim ReturnVal As String
            Try
                con.Open()
                Using command As New SqlCommand("WS_CertifiedCopyCommentSave", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@ReqId", SqlDbType.BigInt).Value = lngReqId
                    command.Parameters.Add("@name", SqlDbType.NVarChar).Value = name
                    command.Parameters.Add("@dobs", SqlDbType.NVarChar).Value = dobs
                    command.Parameters.Add("@mob_no", SqlDbType.NVarChar).Value = mob_no
                    command.Parameters.Add("@email", SqlDbType.NVarChar).Value = email
                    command.Parameters.Add("@comment", SqlDbType.NVarChar).Value = comment
                    command.Parameters.Add("@client_ip", SqlDbType.NVarChar).Value = Common.getIPAddress()
                    command.Parameters.Add("@return_val", SqlDbType.NVarChar, 20).Direction = ParameterDirection.Output
                    command.ExecuteNonQuery()
                    ReturnVal = Convert.ToString(command.Parameters("@return_val").Value.ToString())
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                con.Close()
            End Try
            Return ReturnVal
        End Function

        Function CertifiedCopyRequestDetails() As IDataReader
            Dim dr As IDataReader = Nothing
            Try
                con.Open()
                Using command As New SqlCommand("MARREG_CertifiedCopyRequestDetails", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@Mo_Cd", SqlDbType.NVarChar).Value = Mo_Cd
                    command.Parameters.Add("@req_to", SqlDbType.NVarChar).Value = req_to
                    command.Parameters.Add("@status", SqlDbType.NVarChar).Value = status
                    command.Parameters.Add("@start", SqlDbType.Int).Value = start
                    command.Parameters.Add("@length", SqlDbType.Int).Value = length
                    command.Parameters.Add("@value", SqlDbType.NVarChar).Value = value
                    command.Parameters.Add("@order_by", SqlDbType.NVarChar).Value = order_by
                    command.Parameters.Add("@orderByDir", SqlDbType.NVarChar).Value = orderByDir
                    dr = command.ExecuteReader()
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                'con.Close()
            End Try
            Return dr
        End Function
        Function CertifiedCopyRequestCommentDetails() As IDataReader
            Dim dr As IDataReader = Nothing
            Try
                con.Open()
                Using command As New SqlCommand("MARREG_CertifiedCopyRequestCommentDetails", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@ReqId", SqlDbType.BigInt).Value = lngReqId
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
#Region "Certified Copy Request Hearing"
        Public Function Save_CertifiedCopyRequestHearing() As String
            Dim ReturnVal As String
            Try
                con.Open()
                Using command As New SqlCommand("WS_Save_CertifiedCopyRequestHearing", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@id", SqlDbType.BigInt).Value = id
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
        Public Function FetchDetailForCertifiedCopyHearing() As DataSet
            Dim ds As New DataSet
            Try
                con.Open()
                Using da As New SqlDataAdapter("WS_FetchDetailForCertifiedCopyHearing", con)
                    da.SelectCommand.CommandType = CommandType.StoredProcedure
                    da.SelectCommand.Parameters.Add("@id", SqlDbType.BigInt).Value = id
                    da.Fill(ds)
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                con.Close()
            End Try
            Return ds
        End Function
        Function CertifiedCopyRequestHearingDetails() As IDataReader
            Dim dr As IDataReader = Nothing
            Try
                con.Open()
                Using command As New SqlCommand("MARREG_CertifiedCopyRequestHearingDetails", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@ReqId", SqlDbType.BigInt).Value = lngReqId
                    dr = command.ExecuteReader()
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                'con.Close()
            End Try
            Return dr
        End Function
        Public Function CertifiedCopyRequestSaveHearing() As String
            Dim ReturnVal As String
            Try
                con.Open()
                Using command As New SqlCommand("WS_CertifiedCopyRequestSaveHearing", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@reqId", SqlDbType.BigInt).Value = lngReqId
                    command.Parameters.Add("@id", SqlDbType.BigInt).Value = id
                    command.Parameters.Add("@status", SqlDbType.NVarChar).Value = status
                    command.Parameters.Add("@hearing_dateS", SqlDbType.NVarChar).Value = hearing_dateS
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
#End Region
#End Region
#Region "Certified Copy Request Old Records"
        Public Function CertifiedCopyRequestInfoValidateOld() As String
            Dim ReturnVal As String
            Try
                con.Open()
                Using command As New SqlCommand("WS_CertifiedCopyRequestInfoValidateOld", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@groom_name", SqlDbType.NVarChar).Value = groom_name
                    command.Parameters.Add("@bride_name", SqlDbType.NVarChar).Value = bride_name
                    command.Parameters.Add("@groom_Fname", SqlDbType.NVarChar).Value = groom_Fname
                    command.Parameters.Add("@bride_Fname", SqlDbType.NVarChar).Value = bride_Fname
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
        Public Function CertifiedCopyRequestSaveEmailOld() As String
            Dim ReturnVal As String
            Try
                con.Open()
                Using command As New SqlCommand("WS_CertifiedCopyRequestSaveEmailOld", con)
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
        Public Function CertifiedCopyRequestEmailOTPValidateOld() As Int16
            Dim ReturnVal As Int16
            Try
                con.Open()
                Using command As New SqlCommand("WS_CertifiedCopyRequestEmailOTPValidateOld", con)
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
        Public Function CertifiedCopyRequestSendOld() As DataTable
            Dim ds = New DataSet()
            Try
                con.Open()
                Using da As New SqlDataAdapter("WS_CertifiedCopyRequestSendOld", con)
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
        Function CertifiedCopyRequestDetailsOld() As IDataReader
            Dim dr As IDataReader = Nothing
            Try
                con.Open()
                Using command As New SqlCommand("MARREG_CertifiedCopyRequestDetailsOld", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@Mo_Cd", SqlDbType.NVarChar).Value = Mo_Cd
                    command.Parameters.Add("@req_to", SqlDbType.NVarChar).Value = req_to
                    command.Parameters.Add("@status", SqlDbType.NVarChar).Value = status
                    command.Parameters.Add("@start", SqlDbType.Int).Value = start
                    command.Parameters.Add("@length", SqlDbType.Int).Value = length
                    command.Parameters.Add("@value", SqlDbType.NVarChar).Value = value
                    command.Parameters.Add("@order_by", SqlDbType.NVarChar).Value = order_by
                    command.Parameters.Add("@orderByDir", SqlDbType.NVarChar).Value = orderByDir
                    dr = command.ExecuteReader()
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                'con.Close()
            End Try
            Return dr
        End Function
        Public Function Save_CertifiedCopyRequestApproveOld() As String
            Dim ReturnVal As String
            Try
                con.Open()
                Using command As New SqlCommand("WS_Save_CertifiedCopyRequestApproveOld", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@id", SqlDbType.BigInt).Value = id
                    command.Parameters.Add("@status", SqlDbType.NVarChar).Value = status
                    command.Parameters.Add("@otp", SqlDbType.NVarChar).Value = otp
                    command.Parameters.Add("@user_name", SqlDbType.NVarChar).Value = user_name
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
        Public Function FetchRequestDetailOld() As DataSet
            Dim ds As New DataSet
            Try
                con.Open()
                Using da As New SqlDataAdapter("WS_FetchRequestDetailOld", con)
                    da.SelectCommand.CommandType = CommandType.StoredProcedure
                    da.SelectCommand.Parameters.Add("@id", SqlDbType.BigInt).Value = id
                    da.SelectCommand.Parameters.Add("@status", SqlDbType.NVarChar).Value = status
                    da.Fill(ds)
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                con.Close()
            End Try
            Return ds
        End Function
        Public Function Rollback_CertifiedCopyRequestApproveOld() As String
            Dim outmsg As String = String.Empty
            Try
                Using cnn As New SqlConnection(dbconnctn.GetConnectionString())
                    cnn.Open()
                    Dim cmd = New SqlCommand("WS_Rollback_CertifiedCopyRequestApproveOld", cnn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add(New SqlParameter("@id", id))
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
        Public Function Save_CertificateOld() As String
            Dim outmsg As String = String.Empty
            Try
                Using cnn As New SqlConnection(dbconnctn.GetConnectionString())
                    cnn.Open()
                    Dim cmd = New SqlCommand("MARREG_SaveCertifiedCopyByteOld", cnn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add(New SqlParameter("@id", id))
                    cmd.Parameters.Add(New SqlParameter("@Content", gimg_byte))
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
        Public Function FecthCertifiedCopyOld(id As Int64) As DataTable
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim dt = New DataTable
            Try
                con.Open()
                Using da As New SqlDataAdapter("MARREG_GetCertifiedCopyByteOld", con)
                    da.SelectCommand.CommandType = CommandType.StoredProcedure
                    da.SelectCommand.Parameters.Add("@id", SqlDbType.NVarChar).Value = id
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
#End Region
        Public Function Search_CertifiedCopyRequest() As DataSet
            Dim ds = New DataSet()
            Try
                con.Open()
                Using da As New SqlDataAdapter("WS_Search_CertifiedCopyRequest", con)
                    da.SelectCommand.CommandType = CommandType.StoredProcedure
                    da.SelectCommand.Parameters.Add("@id", SqlDbType.BigInt).Value = id
                    da.SelectCommand.Parameters.Add("@mode", SqlDbType.SmallInt).Value = mode
                    da.SelectCommand.Parameters.Add("@emails", SqlDbType.NVarChar).Value = email
                    da.SelectCommand.Parameters.Add("@groom_names", SqlDbType.NVarChar).Value = groom_name
                    da.SelectCommand.Parameters.Add("@bride_names", SqlDbType.NVarChar).Value = bride_name
                    da.SelectCommand.Parameters.Add("@groom_dobs", SqlDbType.NVarChar).Value = groom_dobs
                    da.SelectCommand.Parameters.Add("@bride_dobs", SqlDbType.NVarChar).Value = bride_dobs
                    da.SelectCommand.Parameters.Add("@reg_dates", SqlDbType.NVarChar).Value = reg_dates
                    da.SelectCommand.Parameters.Add("@certificate_nos", SqlDbType.NVarChar).Value = certificate_no
                    da.SelectCommand.Parameters.Add("@mo_names", SqlDbType.NVarChar).Value = mo_name
                    da.SelectCommand.Parameters.Add("@req_outsider_id", SqlDbType.BigInt).Value = req_outsider_id

                    da.SelectCommand.Parameters.Add("@start", SqlDbType.Int).Value = start
                    da.SelectCommand.Parameters.Add("@length", SqlDbType.Int).Value = length
                    da.SelectCommand.Parameters.Add("@value", SqlDbType.NVarChar).Value = value
                    da.SelectCommand.Parameters.Add("@order_by", SqlDbType.NVarChar).Value = order_by
                    da.SelectCommand.Parameters.Add("@orderByDir", SqlDbType.NVarChar).Value = orderByDir
                    da.Fill(ds)
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                con.Close()
            End Try
            Return ds
        End Function
        Public Function Save_CertifiedCopyRequestApprove() As String
            Dim ReturnVal As String
            Try
                con.Open()
                Using command As New SqlCommand("WS_Save_CertifiedCopyRequestApprove", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@id", SqlDbType.BigInt).Value = id
                    command.Parameters.Add("@App_Auto_Cd", SqlDbType.NVarChar).Value = App_Auto_Cd
                    command.Parameters.Add("@status", SqlDbType.NVarChar).Value = status
                    command.Parameters.Add("@otp", SqlDbType.NVarChar).Value = otp
                    command.Parameters.Add("@user_name", SqlDbType.NVarChar).Value = user_name
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
        Public Function FetchRequestDetail() As DataSet
            Dim ds As New DataSet
            Try
                con.Open()
                Using da As New SqlDataAdapter("WS_FetchRequestDetail", con)
                    da.SelectCommand.CommandType = CommandType.StoredProcedure
                    da.SelectCommand.Parameters.Add("@id", SqlDbType.BigInt).Value = id
                    da.SelectCommand.Parameters.Add("@status", SqlDbType.NVarChar).Value = status
                    da.Fill(ds)
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                con.Close()
            End Try
            Return ds
        End Function
        Public Function ValidateRequestStatus() As DataSet
            Dim ds = New DataSet()
            Try
                con.Open()
                Using da As New SqlDataAdapter("WS_ValidateRequestStatus", con)
                    da.SelectCommand.CommandType = CommandType.StoredProcedure
                    da.SelectCommand.Parameters.Add("@email", SqlDbType.VarChar).Value = email
                    da.SelectCommand.Parameters.Add("@reqid", SqlDbType.VarChar).Value = reqid
                    da.Fill(ds)
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                con.Close()
            End Try
            Return ds
        End Function
#Region "Store and Retrieve Certified Copy of  Marriage Certificate"
        Public Function Save_Certificate() As String
            Dim outmsg As String = String.Empty
            Try
                Using cnn As New SqlConnection(dbconnctn.GetConnectionString())
                    cnn.Open()
                    Dim cmd = New SqlCommand("MARREG_SaveCertifiedCopyByte", cnn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add(New SqlParameter("@id", id))
                    cmd.Parameters.Add(New SqlParameter("@Content", gimg_byte))
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
        Public Function FecthCertifiedCopy(id As Int64) As DataTable
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim dt = New DataTable
            Try
                con.Open()
                Using da As New SqlDataAdapter("MARREG_GetCertifiedCopyByte", con)
                    da.SelectCommand.CommandType = CommandType.StoredProcedure
                    da.SelectCommand.Parameters.Add("@id", SqlDbType.NVarChar).Value = id
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
#Region "Rollback Certified Copy Request Approve"
        Public Function Rollback_CertifiedCopyRequestApprove() As String
            Dim outmsg As String = String.Empty
            Try
                Using cnn As New SqlConnection(dbconnctn.GetConnectionString())
                    cnn.Open()
                    Dim cmd = New SqlCommand("WS_Rollback_CertifiedCopyRequestApprove", cnn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add(New SqlParameter("@id", id))
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
#End Region
#Region "Web Service For BSK"
        Public Function MARREG_Service_BSK() As DataSet
            Dim con As New SqlConnection(dbconnctn.GetConnectionString())
            Dim ds = New DataSet()
            Try
                con.Open()
                Dim da As New SqlDataAdapter("MARREG_BSK_API_SP", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure

                da.SelectCommand.Parameters.Add(New SqlParameter("@Mobile_no", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@Mobile_No").Value = mob_no

                da.SelectCommand.Parameters.Add(New SqlParameter("@From_dt", SqlDbType.DateTime))
                da.SelectCommand.Parameters("@From_dt").Value = Convert.ToDateTime(reg_date_from)

                da.SelectCommand.Parameters.Add(New SqlParameter("@To_dt", SqlDbType.DateTime))
                da.SelectCommand.Parameters("@To_dt").Value = Convert.ToDateTime(reg_date_to)

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

#Region "MasterData Webservice"
#Region "Update Jurisdiction"
        Function FetchJurisdictionList() As IDataReader
            Dim dr As IDataReader = Nothing
            Try
                con.Open()
                Using command As New SqlCommand("WS_MO_JurisdictionList", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@Mo_Cd", SqlDbType.NVarChar).Value = Mo_Cd
                    command.Parameters.Add("@start", SqlDbType.Int).Value = start
                    command.Parameters.Add("@length", SqlDbType.Int).Value = length
                    command.Parameters.Add("@value", SqlDbType.NVarChar).Value = value
                    command.Parameters.Add("@order_by", SqlDbType.NVarChar).Value = order_by
                    command.Parameters.Add("@orderByDir", SqlDbType.NVarChar).Value = orderByDir
                    dr = command.ExecuteReader()
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                'con.Close()
            End Try
            Return dr
        End Function
        Public Function UpdateJurisdiction() As String
            Dim ReturnVal As String
            Try
                con.Open()
                Using command As New SqlCommand("WS_UpdateJurisdiction", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@Jurisdiction_Details_cd", SqlDbType.NVarChar).Value = mdatas
                    command.Parameters.Add("@Jurisdctn_location_cd", SqlDbType.NVarChar).Value = juri_location
                    command.Parameters.Add("@sm", SqlDbType.NVarChar).Value = sm
                    command.Parameters.Add("@hm", SqlDbType.NVarChar).Value = hm
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
#End Region
#End Region
#Region "Service Docket"
        Public Function SaveServiceDocket() As String
            Dim ReturnVal As String
            Try
                con.Open()
                Using command As New SqlCommand("WS_SaveServiceDocket", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@service_code", SqlDbType.NVarChar).Value = reason
                    command.Parameters.Add("@service_type", SqlDbType.NVarChar).Value = mdatas
                    command.Parameters.Add("@user_name", SqlDbType.NVarChar).Value = user_name
                    command.Parameters.Add("@return_val", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output
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
        Public Function FetchDocketDetails() As DataSet
            Dim ds As New DataSet
            Try
                con.Open()
                Using da As New SqlDataAdapter("WS_FetchDocketDetails", con)
                    da.SelectCommand.CommandType = CommandType.StoredProcedure
                    da.SelectCommand.Parameters.Add("@docket_no", SqlDbType.NVarChar).Value = docket_no
                    da.Fill(ds)
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                con.Close()
            End Try
            Return ds
        End Function
        Function DocketList() As IDataReader
            Dim dr As IDataReader = Nothing
            Try
                con.Open()
                Using command As New SqlCommand("WS_ServiceDocketList", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@dno", SqlDbType.NVarChar).Value = docket_no
                    command.Parameters.Add("@start", SqlDbType.Int).Value = start
                    command.Parameters.Add("@length", SqlDbType.Int).Value = length
                    command.Parameters.Add("@value", SqlDbType.NVarChar).Value = value
                    command.Parameters.Add("@order_by", SqlDbType.NVarChar).Value = order_by
                    command.Parameters.Add("@orderByDir", SqlDbType.NVarChar).Value = orderByDir
                    dr = command.ExecuteReader()
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                'con.Close()
            End Try
            Return dr
        End Function
        Public Function UpdateServiceDocketDPR() As String
            Dim ReturnVal As String
            Try
                con.Open()
                Using command As New SqlCommand("WS_UpdateServiceDocketDPR", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@dept_payment_ref_id", SqlDbType.NVarChar).Value = dpr
                    command.Parameters.Add("@docket_no", SqlDbType.VarChar).Value = docket_no

                    command.Parameters.Add("@returnValue", SqlDbType.NVarChar, 1).Direction = ParameterDirection.Output
                    command.ExecuteNonQuery()

                    ReturnVal = command.Parameters("@returnValue").Value.ToString()
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                con.Close()
            End Try
            Return ReturnVal
        End Function
        Public Function UpdateServiceDocketStatus() As String
            Dim ReturnVal As String
            Try
                con.Open()
                Using command As New SqlCommand("WS_UpdateServiceDocketStatus", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@dept_payment_ref_id", SqlDbType.NVarChar).Value = dpr
                    command.Parameters.Add("@docket_no", SqlDbType.VarChar).Value = docket_no

                    command.Parameters.Add("@returnValue", SqlDbType.NVarChar, 1).Direction = ParameterDirection.Output
                    command.ExecuteNonQuery()

                    ReturnVal = command.Parameters("@returnValue").Value.ToString()
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                con.Close()
            End Try
            Return ReturnVal
        End Function
#End Region
    End Class
End Namespace
