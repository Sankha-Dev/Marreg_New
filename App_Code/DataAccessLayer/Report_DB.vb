Imports System.Data
Imports System.Data.SqlClient
Imports MARREG.App_Code.DataAccessLayer
Imports Microsoft.VisualBasic
Namespace MARREG.App_Code
    Public Class Report_DB
#Region "DECLARATION"
        Public dbconnctn As New DataBaseHelperDB()
        Private con As New SqlConnection(dbconnctn.GetConnectionString())
        Public Property AppAutoCd As String
#End Region
#Region "Schedule of Signature B"
        Public Function ScheduleOfSignatureB() As DataSet
            Dim ds = New DataSet()
            Try
                con.Open()
                Using da As New SqlDataAdapter("MARREG_ScheduleofSignatureB", con)
                    da.SelectCommand.CommandType = CommandType.StoredProcedure
                    da.SelectCommand.Parameters.Add("@App_Auto_Cd", SqlDbType.NVarChar).Value = AppAutoCd
                    da.SelectCommand.Parameters.Add("@hologram", SqlDbType.NVarChar).Value = ""
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
