Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports MARREG.App_Code.DataAccessLayer
Public Class BSK_DB
#Region "Declaration"
    Public dbconnctn As New DataBaseHelperDB()
    Private con As New SqlConnection(dbconnctn.GetConnectionString())
    Public Property dpr As String
    Public Property ticketNo As String
    Public Property userId As String
    Public Property citizenMobile As String
    Public Property citizenEmail As String
    Public Property citizenName As String
    Public Property responseUrl As String
    Public Property serviceCode As String
#End Region
#Region "Methods"
#Region "Save BSK Request"
    Public Function SaveBSK_Request() As String
        Dim ReturnVal As String
        Try
            con.Open()
            Using command As New SqlCommand("WS_SaveBSK_Request", con)
                command.CommandType = CommandType.StoredProcedure
                command.Parameters.Add("@dpr", SqlDbType.NVarChar).Value = dpr
                command.Parameters.Add("@ticketNo", SqlDbType.NVarChar).Value = ticketNo
                command.Parameters.Add("@userId", SqlDbType.NVarChar).Value = userId
                command.Parameters.Add("@citizenMobile", SqlDbType.NVarChar).Value = citizenMobile
                command.Parameters.Add("@citizenEmail", SqlDbType.NVarChar).Value = citizenEmail
                command.Parameters.Add("@citizenName", SqlDbType.NVarChar).Value = citizenName
                command.Parameters.Add("@responseUrl", SqlDbType.NVarChar).Value = responseUrl
                command.Parameters.Add("@serviceCode", SqlDbType.NVarChar).Value = serviceCode

                command.Parameters.Add("@return_value", SqlDbType.NVarChar, 30).Direction = ParameterDirection.Output
                command.ExecuteNonQuery()

                ReturnVal = command.Parameters("@return_value").Value.ToString()
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            con.Close()
        End Try
        Return ReturnVal
    End Function
#End Region
#Region "Fetch BSK Request Data"
    Public Function FetchBSKRequestData() As IDataReader
        Dim dr As IDataReader = Nothing
        Try
            con.Open()
            Using command As New SqlCommand("MARREG_FetchBSKRequestData", con)
                command.CommandType = CommandType.StoredProcedure
                command.Parameters.Add("@dpr", SqlDbType.NVarChar).Value = dpr
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
#End Region
End Class
