Imports Microsoft.VisualBasic

Imports System
Imports System.Data
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Imports System.Configuration
Imports System.Data.SqlClient

Namespace MARREG.App_Code.DataAccessLayer
    Public Class AdminDB
#Region "DECLARATION"
        Public dbconnctn As New DataBaseHelperDB()
        Public dbf As New DBFactoryDB()
#End Region

#Region "FUNCTION"
#Region "Role Access Insert Update"
#Region "Insert"
        Public Function Roleaccess(page_id As String, role_type_cd As String) As String
            Dim outmsg As String = String.Empty
            Try

                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_Role_Access_insertupdate")
                db.AddInParameter(dbCommand, "@page_id", DbType.[String], page_id)
                db.AddInParameter(dbCommand, "@role_type_cd", DbType.[String], role_type_cd)
              
                db.AddInParameter(dbCommand, "@Tag", DbType.[String], "Insert")
                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)
                db.ExecuteNonQuery(dbCommand)
                outmsg = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))
            Catch ex As Exception

                outmsg = ex.Message()
            End Try
            Return outmsg
        End Function
#End Region
#End Region





#End Region

    End Class
End Namespace
