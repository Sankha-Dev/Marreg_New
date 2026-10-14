Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports MARREG.App_Code.DataAccessLayer

Public Class GRIPS_DB
#Region "DECLARATION"
    Public dbconnctn As New DataBaseHelperDB()
    '    Public dbf As New DBFactoryDB()
    Private con As New SqlConnection(dbconnctn.GetConnectionString())
    Public Property reference_data As String
    Public Property id As Int64
    Public Property dept_payment_ref_id As String
    Public Property source As String
    Public Property respURL As String
    Public Property payment_amt As Decimal
    Public Property depositor_mob As Double

    Public Property action As String
    Public Property dept_code As String
    Public Property service_code As String

    Public Property dept_ref_no As String
    Public Property identification_no As String
    Public Property total_amt As Decimal
    Public Property depositor_name As String

    Public Property depositor_address As String
    Public Property depositor_email As String
    Public Property onBehalfOf As String
    Public Property depositor_type As String

    Public Property period_from As String
    Public Property period_to As String
    Public Property payment_remarks As String
    Public Property refNo As String
    Public Property ip As String
    Public Property sys_time_stamp As String

    Public Property grips_payment_ref_no As String
    Public Property payment_status As String
    Public Property payment_status_desc As String
    Public Property grn As String
    Public Property grn_time As String
    Public Property grn_status As String
    Public Property grn_status_desc As String
    Public Property bank As String
    Public Property payment_mode As String
    Public Property brn As String
    Public Property brn_time As String
    Public Property status As String
    Public Property status_code As String
    Public Property status_desc As String
    Public Property payment_date As String

    Public Property start As Integer
    Public Property length As Integer
    Public Property value As String
    Public Property order_by As String
    Public Property orderByDir As String
#End Region
#Region "MARREG_TO_GRIPS"



    Public Function SaveGRIPSPaymentRequest() As Dictionary(Of String, String)
        Dim ReturnVal As New Dictionary(Of String, String)
        Try
            con.Open()
            Using command As New SqlCommand("WS_SaveGRIPSPaymentRequest", con)
                command.CommandType = CommandType.StoredProcedure
                command.Parameters.Add("@source", SqlDbType.NVarChar).Value = source
                command.Parameters.Add("@respURL", SqlDbType.NVarChar).Value = respURL
                command.Parameters.Add("@payment_amt", SqlDbType.Decimal).Value = payment_amt
                command.Parameters.Add("@depositor_mob", SqlDbType.NVarChar).Value = depositor_mob
                command.Parameters.Add("@action", SqlDbType.NVarChar).Value = action
                command.Parameters.Add("@dept_code", SqlDbType.NVarChar).Value = dept_code
                command.Parameters.Add("@service_code", SqlDbType.NVarChar).Value = service_code
                command.Parameters.Add("@depositor_name", SqlDbType.NVarChar).Value = depositor_name
                command.Parameters.Add("@depositor_address", SqlDbType.NVarChar).Value = depositor_address
                command.Parameters.Add("@depositor_email", SqlDbType.NVarChar).Value = depositor_email
                command.Parameters.Add("@onBehalfOf", SqlDbType.NVarChar).Value = onBehalfOf
                command.Parameters.Add("@depositor_type", SqlDbType.NVarChar).Value = depositor_type
                command.Parameters.Add("@period_from", SqlDbType.NVarChar).Value = period_from
                command.Parameters.Add("@period_to", SqlDbType.NVarChar).Value = period_to
                command.Parameters.Add("@payment_remarks", SqlDbType.NVarChar).Value = payment_remarks
                command.Parameters.Add("@refNo", SqlDbType.NVarChar).Value = refNo
                command.Parameters.Add("@ip", SqlDbType.NVarChar).Value = ip
                command.Parameters.Add("@sys_time_stamp", SqlDbType.NVarChar).Value = sys_time_stamp

                command.Parameters.Add("@dept_payment_ref_id", SqlDbType.NVarChar, 30).Direction = ParameterDirection.Output
                command.Parameters.Add("@dept_ref_no", SqlDbType.NVarChar, 16).Direction = ParameterDirection.Output
                command.Parameters.Add("@identification_no", SqlDbType.NVarChar, 20).Direction = ParameterDirection.Output
                command.ExecuteNonQuery()

                ReturnVal("dept_payment_ref_id") = command.Parameters("@dept_payment_ref_id").Value.ToString()
                ReturnVal("dept_ref_no") = command.Parameters("@dept_ref_no").Value.ToString()
                ReturnVal("identification_no") = command.Parameters("@identification_no").Value.ToString()
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            con.Close()
        End Try
        Return ReturnVal
    End Function
#End Region
#Region "MARREG_FROM_GRIPS"
    Public Function SaveGRIPSPaymentResponse() As DataTable
        Dim dt = New DataTable
        Try
            con.Open()
            Using da As New SqlDataAdapter("WS_SaveGRIPSPaymentResponse", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure
                da.SelectCommand.Parameters.Add("@dept_payment_ref_id", SqlDbType.NVarChar).Value = dept_payment_ref_id
                da.SelectCommand.Parameters.Add("@source", SqlDbType.NVarChar).Value = source
                da.SelectCommand.Parameters.Add("@action", SqlDbType.NVarChar).Value = action
                da.SelectCommand.Parameters.Add("@grips_payment_ref_no", SqlDbType.NVarChar).Value = grips_payment_ref_no
                da.SelectCommand.Parameters.Add("@payment_amt", SqlDbType.Decimal).Value = payment_amt
                da.SelectCommand.Parameters.Add("@payment_status", SqlDbType.NVarChar).Value = payment_status
                da.SelectCommand.Parameters.Add("@payment_status_desc", SqlDbType.NVarChar).Value = payment_status_desc
                da.SelectCommand.Parameters.Add("@grn", SqlDbType.NVarChar).Value = grn
                da.SelectCommand.Parameters.Add("@grn_time", SqlDbType.NVarChar).Value = grn_time
                da.SelectCommand.Parameters.Add("@grn_status", SqlDbType.NVarChar).Value = grn_status
                da.SelectCommand.Parameters.Add("@grn_status_desc", SqlDbType.NVarChar).Value = grn_status_desc
                da.SelectCommand.Parameters.Add("@bank", SqlDbType.NVarChar).Value = bank
                da.SelectCommand.Parameters.Add("@payment_mode", SqlDbType.NVarChar).Value = payment_mode
                da.SelectCommand.Parameters.Add("@brn", SqlDbType.NVarChar).Value = brn
                da.SelectCommand.Parameters.Add("@brn_time", SqlDbType.NVarChar).Value = brn_time
                da.SelectCommand.Parameters.Add("@status", SqlDbType.NVarChar).Value = status
                da.SelectCommand.Parameters.Add("@status_code", SqlDbType.NVarChar).Value = status_code
                da.SelectCommand.Parameters.Add("@status_desc", SqlDbType.NVarChar).Value = status_desc
                da.SelectCommand.Parameters.Add("@ip", SqlDbType.NVarChar).Value = ip
                da.SelectCommand.Parameters.Add("@sys_time_stamp", SqlDbType.NVarChar).Value = sys_time_stamp
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
#Region "GRIPS Payment Details"
    Function GRIPSpaymentDetails() As IDataReader
        Dim dr As IDataReader = Nothing
        Try
            con.Open()
            Using command As New SqlCommand("MARREG_GRIPSpaymentDetails", con)
                command.CommandType = CommandType.StoredProcedure
                command.Parameters.Add("@dept_payment_ref_id", SqlDbType.NVarChar).Value = dept_payment_ref_id
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
#End Region
#Region "GRIPS Payment Details Refresh"
    Public Function GRIPSpaymentStatusFetch() As DataTable
        Dim ds = New DataSet()
        Try
            con.Open()
            Using da As New SqlDataAdapter("WS_GRIPSpaymentStatusFetch", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure
                da.SelectCommand.Parameters.Add("@id", SqlDbType.BigInt).Value = id
                da.Fill(ds)
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            con.Close()
        End Try
        Return ds.Tables(0)
    End Function
    Public Function GRIPSverifyRequestParam() As Int16
        Dim ReturnVal As Int16
        Try
            con.Open()
            Using command As New SqlCommand("WS_GRIPSverifyRequestParam", con)
                command.CommandType = CommandType.StoredProcedure
                command.Parameters.Add("@dept_payment_ref_id", SqlDbType.VarChar).Value = dept_payment_ref_id
                command.Parameters.Add("@payment_date", SqlDbType.VarChar).Value = payment_date
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
    Public Function GRIPSverifyRequestParamStatus() As DataTable
        Dim ds = New DataSet()
        Try
            con.Open()
            Using da As New SqlDataAdapter("WS_GRIPSverifyRequestParamStatus", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure
                da.SelectCommand.Parameters.Add("@service_code", SqlDbType.VarChar).Value = service_code
                da.SelectCommand.Parameters.Add("@dept_payment_ref_id", SqlDbType.VarChar).Value = dept_payment_ref_id
                da.SelectCommand.Parameters.Add("@payment_date", SqlDbType.VarChar).Value = payment_date
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
#Region "MARREG Payment Reference"
    Public Function Save_MARREG_Payment_Reference() As String
        Dim ReturnVal As String
        Try
            con.Open()
            Using command As New SqlCommand("MARREG_Payment_Reference_Save", con)
                command.CommandType = CommandType.StoredProcedure
                command.Parameters.Add("@dept_payment_ref_id", SqlDbType.NVarChar).Value = dept_payment_ref_id
                command.Parameters.Add("@reference_data", SqlDbType.VarChar).Value = reference_data

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
    Public Function Fetch_MARREG_Payment_Reference() As DataTable
        Dim dt = New DataTable
        Try
            con.Open()
            Using da As New SqlDataAdapter("MARREG_Payment_Reference_Fetch", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure
                da.SelectCommand.Parameters.Add("@dept_payment_ref_id", SqlDbType.NVarChar).Value = dept_payment_ref_id
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
End Class
