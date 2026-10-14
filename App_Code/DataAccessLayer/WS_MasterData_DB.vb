Imports Microsoft.VisualBasic
Imports MARREG.App_Code.DataAccessLayer
Imports System.Data.SqlClient
Imports System.Data

Namespace MARREG.App_Code
    Public Class WS_MasterData_DB
#Region "DECLARATION"
        Public dbconnctn As New DataBaseHelperDB()

        Private con As New SqlConnection(dbconnctn.GetConnectionString())
        Public Property id As Int64
        Public Property email_type As String
        Public Property email_subject As String
        Public Property email_body As String
        Public Property email_type_id As Int64
        Public Property start As Integer
        Public Property length As Integer
        Public Property value As String
        Public Property order_by As String
        Public Property orderByDir As String

        Public Property ddl_act As String
        Public Property state_cd As String
        Public Property dist_cd As String
        Public Property subdiv_cd As String
        Public Property block_cd As String
        Public Property work_area As String
        Public Property pincode As String
        Public Property reg_dist As String
        Public Property reg_ps As String
        Public Property munigp As String
        Public Property mo_cd As String
        Public Property doc_type As String
        Public Property nation As String
		Public Property divoceeAct As String
        Public Property maritalStatus As String
#End Region
#Region "Email Type"
        Function EmailTypes() As IDataReader
            Dim dr As IDataReader = Nothing
            Try
                con.Open()
                Using command As New SqlCommand("WS_MARREG_EmailTypes", con)
                    command.CommandType = CommandType.StoredProcedure
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
        Public Function SaveEmailType() As String
            Dim ReturnVal As String
            Try
                con.Open()
                Using command As New SqlCommand("WS_MARREG_SaveEmailType", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@id", SqlDbType.BigInt).Value = id
                    command.Parameters.Add("@email_type", SqlDbType.NVarChar).Value = email_type
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
        Public Function DeleteEmailType() As String
            Dim ReturnVal As String
            Try
                con.Open()
                Using command As New SqlCommand("WS_MARREG_DeleteEmailType", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@id", SqlDbType.BigInt).Value = id
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
#Region "Email Format"
        Function EmailFormats() As IDataReader
            Dim dr As IDataReader = Nothing
            Try
                con.Open()
                Using command As New SqlCommand("WS_MARREG_EmailFormats", con)
                    command.CommandType = CommandType.StoredProcedure
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

        Public Function SaveEmailFormat() As String
            Dim ReturnVal As String
            Try
                con.Open()
                Using command As New SqlCommand("WS_MARREG_SaveEmailFormat", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@id", SqlDbType.BigInt).Value = id
                    command.Parameters.Add("@email_type_id", SqlDbType.BigInt).Value = email_type_id
                    command.Parameters.Add("@email_subject", SqlDbType.NVarChar).Value = email_subject
                    command.Parameters.Add("@email_body", SqlDbType.VarChar).Value = email_body
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
        Public Function DeleteEmailFormat() As String
            Dim ReturnVal As String
            Try
                con.Open()
                Using command As New SqlCommand("WS_MARREG_DeleteEmailFormat", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@id", SqlDbType.BigInt).Value = id
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
        Public Function FetchEmailFormat() As DataSet
            Dim ds As New DataSet
            Try
                con.Open()
                Using da As New SqlDataAdapter("WS_MARREG_FetchEmailFormat", con)
                    da.SelectCommand.CommandType = CommandType.StoredProcedure
                    da.SelectCommand.Parameters.Add("@email_type", SqlDbType.NVarChar).Value = email_type
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
#Region "Master Data"
        Public Function LoadDistrict() As DataSet
            Dim ds As New DataSet
            Try
                con.Open()
                Using da As New SqlDataAdapter("WS_MARREG_LoadDistrict", con)
                    da.SelectCommand.CommandType = CommandType.StoredProcedure
                    da.SelectCommand.Parameters.Add("@state_cd", SqlDbType.NVarChar).Value = state_cd
                    da.Fill(ds)
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                con.Close()
            End Try
            Return ds
        End Function
        Public Function LoadSubDiv() As DataSet
            Dim ds As New DataSet
            Try
                con.Open()
                Using da As New SqlDataAdapter("WS_MARREG_LoadSubDiv", con)
                    da.SelectCommand.CommandType = CommandType.StoredProcedure
                    da.SelectCommand.Parameters.Add("@dist_cd", SqlDbType.NVarChar).Value = dist_cd
                    da.Fill(ds)
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                con.Close()
            End Try
            Return ds
        End Function
        Public Function LoadPS() As DataSet
            Dim ds As New DataSet
            Try
                con.Open()
                Using da As New SqlDataAdapter("WS_MARREG_LoadPS", con)
                    da.SelectCommand.CommandType = CommandType.StoredProcedure
                    da.SelectCommand.Parameters.Add("@dist_cd", SqlDbType.NVarChar).Value = dist_cd
                    da.Fill(ds)
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                con.Close()
            End Try
            Return ds
        End Function
        Public Function LoadBlock() As DataSet
            Dim ds As New DataSet
            Try
                con.Open()
                Using da As New SqlDataAdapter("WS_MARREG_LoadBlock", con)
                    da.SelectCommand.CommandType = CommandType.StoredProcedure
                    da.SelectCommand.Parameters.Add("@dist_cd", SqlDbType.NVarChar).Value = dist_cd
                    da.SelectCommand.Parameters.Add("@subdiv_cd", SqlDbType.NVarChar).Value = subdiv_cd
                    da.Fill(ds)
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                con.Close()
            End Try
            Return ds
        End Function
        Public Function LoadmuniGP() As DataSet
            Dim ds As New DataSet
            Try
                con.Open()
                Using da As New SqlDataAdapter("WS_MARREG_LoadmuniGP", con)
                    da.SelectCommand.CommandType = CommandType.StoredProcedure
                    da.SelectCommand.Parameters.Add("@dist_cd", SqlDbType.NVarChar).Value = dist_cd
                    da.SelectCommand.Parameters.Add("@subdiv_cd", SqlDbType.NVarChar).Value = subdiv_cd
                    da.SelectCommand.Parameters.Add("@block_cd", SqlDbType.NVarChar).Value = block_cd
                    da.SelectCommand.Parameters.Add("@work_area", SqlDbType.NVarChar).Value = work_area
                    da.Fill(ds)
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                con.Close()
            End Try
            Return ds
        End Function
        Public Function LoadPO() As DataSet
            Dim ds As New DataSet
            Try
                con.Open()
                Using da As New SqlDataAdapter("WS_MARREG_LoadPO", con)
                    da.SelectCommand.CommandType = CommandType.StoredProcedure
                    da.SelectCommand.Parameters.Add("@dist_cd", SqlDbType.NVarChar).Value = dist_cd
                    da.SelectCommand.Parameters.Add("@pincode", SqlDbType.NVarChar).Value = pincode
                    da.Fill(ds)
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                con.Close()
            End Try
            Return ds
        End Function
		Public Function LoadDivorceeSection() As DataSet
            Dim ds As New DataSet
            Try
                con.Open()
                Using da As New SqlDataAdapter("WS_MARREG_LoadDivorceeSection", con)
                    da.SelectCommand.CommandType = CommandType.StoredProcedure
                    da.SelectCommand.Parameters.Add("@divoceeAct", SqlDbType.NVarChar).Value = divoceeAct
                    da.SelectCommand.Parameters.Add("@maritalStatus", SqlDbType.NVarChar).Value = maritalStatus
                    da.Fill(ds)
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                con.Close()
            End Try
            Return ds
        End Function

        Function LoadallMO() As IDataReader
            Dim dr As IDataReader = Nothing
            Try
                con.Open()
                Using command As New SqlCommand("WS_MARREG_ApplyOnline_MO_Details", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@start", SqlDbType.Int).Value = start
                    command.Parameters.Add("@length", SqlDbType.Int).Value = length
                    command.Parameters.Add("@value", SqlDbType.NVarChar).Value = value
                    command.Parameters.Add("@order_by", SqlDbType.NVarChar).Value = order_by
                    command.Parameters.Add("@orderByDir", SqlDbType.NVarChar).Value = orderByDir
                    command.Parameters.Add("@act_cd", SqlDbType.NVarChar).Value = ddl_act
                    command.Parameters.Add("@district", SqlDbType.NVarChar).Value = reg_dist
                    command.Parameters.Add("@ps", SqlDbType.NVarChar).Value = reg_ps
                    command.Parameters.Add("@subdivision", SqlDbType.NVarChar).Value = subdiv_cd
                    command.Parameters.Add("@Block", SqlDbType.NVarChar).Value = block_cd
                    command.Parameters.Add("@Muni_gp", SqlDbType.NVarChar).Value = munigp
                    dr = command.ExecuteReader()
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                'con.Close()
            End Try
            Return dr
        End Function

        Function LoadRegMO_other_details() As DataTable
            Dim dt = New DataTable()
            Try
                con.Open()
                Dim da As New SqlDataAdapter("WS_MARREG_ApplyOnline_MO_OtherDetails", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure
                da.SelectCommand.Parameters.Add(New SqlParameter("@mo_cd", SqlDbType.NVarChar))
                da.SelectCommand.Parameters("@mo_cd").Value = mo_cd

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
#Region "Document Proof Type"
        Public Function LoadDocumentProofType() As DataSet
            Dim ds As New DataSet
            Try
                con.Open()
                Using da As New SqlDataAdapter("WS_MARREG_LoadDocumentProofType", con)
                    da.SelectCommand.CommandType = CommandType.StoredProcedure
                    da.SelectCommand.Parameters.Add("@doc_type", SqlDbType.NVarChar).Value = doc_type
                    da.SelectCommand.Parameters.Add("@nation", SqlDbType.NVarChar).Value = nation
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
End Namespace
