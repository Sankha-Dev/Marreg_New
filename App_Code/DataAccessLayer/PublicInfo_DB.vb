Imports Microsoft.VisualBasic
Imports System.Data
Imports MARREG.App_Code.DataAccessLayer
Imports System.Data.SqlClient

Namespace MARREG.App_Code
    Public Class PublicInfo_DB
#Region "DECLARATION"
        Public dbconnctn As New DataBaseHelperDB()
        Private con As New SqlConnection(dbconnctn.GetConnectionString())
        Public Property fromDate As Date
        Public Property toDate As Date
        Public Property start As Integer
        Public Property length As Integer
        Public Property value As String
        Public Property order_by As String
        Public Property orderByDir As String

        Public Property groom_name As String
        Public Property bride_name As String
        Public Property groom_Fname As String
        Public Property bride_Fname As String
        Public Property groom_adhr As String
        Public Property bride_adhr As String
        Public Property app_no As String
        Public Property mo_name As String
        Public Property marriage_dt As Date
        Public Property act_cd As String
        Public Property groom_dob As Date
        Public Property bride_dob As Date
        Public Property App_Auto_Cd As String
#End Region
#Region "Notice List"

        Property auth As Integer

        Function getNoticeListing() As IDataReader
            Dim dr As IDataReader = Nothing
            Try
                con.Open()
                Using command As New SqlCommand("MARREG_GetNoticeListing", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@fromDate", SqlDbType.DateTime).Value = fromDate
                    command.Parameters.Add("@toDate", SqlDbType.DateTime).Value = toDate
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

#Region "Marriage New Records List"
        Function getNewRecordsList() As IDataReader
            Dim dr As IDataReader = Nothing
            Try
                con.Open()
                Using command As New SqlCommand("MARREG_GetNewRecordsListing", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@auth", SqlDbType.Int).Value = auth
                    command.Parameters.Add("@groom_name", SqlDbType.NVarChar).Value = groom_name
                    command.Parameters.Add("@groom_Fname", SqlDbType.NVarChar).Value = groom_Fname
                    command.Parameters.Add("@groom_adhr", SqlDbType.NVarChar).Value = groom_adhr
                    command.Parameters.Add("@bride_name", SqlDbType.NVarChar).Value = bride_name
                    command.Parameters.Add("@bride_Fname", SqlDbType.NVarChar).Value = bride_Fname
                    command.Parameters.Add("@bride_adhr", SqlDbType.NVarChar).Value = bride_adhr
                    command.Parameters.Add("@groom_dob", SqlDbType.Date).Value = groom_dob
                    command.Parameters.Add("@bride_dob", SqlDbType.Date).Value = bride_dob
                    command.Parameters.Add("@app_no", SqlDbType.NVarChar).Value = app_no
                    command.Parameters.Add("@mo_name", SqlDbType.NVarChar).Value = mo_name
                    command.Parameters.Add("@marriage_dt", SqlDbType.Date).Value = marriage_dt
                    command.Parameters.Add("@act_cd", SqlDbType.NVarChar).Value = act_cd
                    command.Parameters.Add("@fromDate", SqlDbType.Date).Value = fromDate
                    command.Parameters.Add("@toDate", SqlDbType.Date).Value = toDate
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

#Region "Public Dashboard"
        Public Function GetDashboardDataP1() As DataTable
            Dim ds = New DataSet()
            Try
                con.Open()
                Using da As New SqlDataAdapter("MARREG_GetDashboardData", con)
                    da.SelectCommand.CommandType = CommandType.StoredProcedure
                    da.SelectCommand.CommandTimeout = 120
                    da.SelectCommand.Parameters.Add("@year", SqlDbType.NVarChar).Value = value
                    da.SelectCommand.Parameters.Add("@part", SqlDbType.Int).Value = 1
                    da.Fill(ds)
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                con.Close()
            End Try
            Return ds.Tables(0)
        End Function
        Public Function GetDashboardDataP2() As DataTable
            Dim ds = New DataSet()
            Try
                con.Open()
                Using da As New SqlDataAdapter("MARREG_GetDashboardData", con)
                    da.SelectCommand.CommandType = CommandType.StoredProcedure
                    da.SelectCommand.CommandTimeout = 120
                    da.SelectCommand.Parameters.Add("@year", SqlDbType.NVarChar).Value = value
                    da.SelectCommand.Parameters.Add("@part", SqlDbType.Int).Value = 2
                    da.Fill(ds)
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                con.Close()
            End Try
            Return ds.Tables(0)
        End Function
        Public Function GetDashboardDataP3() As DataTable
            Dim ds = New DataSet()
            Try
                con.Open()
                Using da As New SqlDataAdapter("MARREG_GetDashboardData", con)
                    da.SelectCommand.CommandType = CommandType.StoredProcedure
                    da.SelectCommand.CommandTimeout = 180
                    da.SelectCommand.Parameters.Add("@year", SqlDbType.NVarChar).Value = value
                    da.SelectCommand.Parameters.Add("@part", SqlDbType.Int).Value = 3
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
        Public Function CertificateVerification() As DataTable
            Dim ds = New DataSet()
            Try
                con.Open()
                Using da As New SqlDataAdapter("MARREG_Certificate_Issue", con)
                    da.SelectCommand.CommandType = CommandType.StoredProcedure
                    da.SelectCommand.Parameters.Add("@App_Auto_Cd", SqlDbType.NVarChar).Value = App_Auto_Cd
                    da.SelectCommand.Parameters.Add("@hologram", SqlDbType.NVarChar).Value = "1234"
                    da.Fill(ds)
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                con.Close()
            End Try
            Return ds.Tables(0)
        End Function
    End Class
End Namespace
