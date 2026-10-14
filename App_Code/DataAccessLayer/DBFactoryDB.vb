Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports Microsoft.VisualBasic

Namespace MARREG.App_Code.DataAccessLayer
    Public Class DBFactoryDB
        Private _sqlConnectionString As String
        Public con As New DataBaseHelperDB()
        Public Function CreateConnection() As Database
            _sqlConnectionString = con.GetConnectionString()
            Dim db As New SqlDatabase(_sqlConnectionString)
            Return db
        End Function

        Public Function CreateConnection(ConnectionString As String) As Database
            _sqlConnectionString = con.GetConnectionString(ConnectionString)
            Dim db As New SqlDatabase(_sqlConnectionString)
            Return db
        End Function

    End Class
End Namespace
