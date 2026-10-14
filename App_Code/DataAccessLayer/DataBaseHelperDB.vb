Imports System
Imports System.Configuration
Imports System.Web.Configuration
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports Microsoft.VisualBasic
Imports System.Data

Namespace MARREG.App_Code.DataAccessLayer
    Public Class DataBaseHelperDB

#Region "DatabaseConnection"

        Public Function GetConnectionString(ConnectionStringName As String) As String
            Dim connectionString As String = Nothing
            Dim settings As ConnectionStringSettings = WebConfigurationManager.ConnectionStrings(ConnectionStringName)
            If settings IsNot Nothing Then
                connectionString = settings.ConnectionString
            End If
            Return connectionString
        End Function
        Public Function GetConnectionString() As String
            Dim connectionString As String = Nothing
            Dim settings As ConnectionStringSettings = WebConfigurationManager.ConnectionStrings("marreg")
            If settings IsNot Nothing Then
                connectionString = settings.ConnectionString
            End If
            Return connectionString
        End Function

        Friend Function ExecuteQuery(query As String) As DataTable
            Throw New NotImplementedException()
        End Function
#End Region
    End Class
End Namespace
