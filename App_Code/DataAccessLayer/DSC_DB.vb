Imports System.Data
Imports System.Data.Common
Imports System.Data.SqlClient
Imports MARREG.App_Code.DataAccessLayer
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.VisualBasic

Namespace MARREG.App_Code
    Public Class DSC_DB
#Region "Declaration"
        Public dbconnctn As New DataBaseHelperDB()
        Dim dbf As New DBFactoryDB()
        Private con As New SqlConnection(dbconnctn.GetConnectionString())
        Public Property start As Integer
        Public Property length As Integer
        Public Property value As String
        Public Property order_by As String
        Public Property orderByDir As String

        Public Property Registration_Id As Int64
        Public Property Mo_Cd As String
#End Region

        Public Function DSC_Info_Insert(Registration_Id As Integer, Mo_Cd As String, Content_Type As String, Certificate_Verified As String, Subject As String, Issuer_Name As String,
                                        Subject_Name As String, EmailName As String, Signature_Algorithm As String, Public_Key_Length As String, Issue_Date As DateTime,
                                        Valid_Date As DateTime, Public_Key_Str As String, Thumbprint As String, SerialNumber As String,
                                        Version As String, Oid As String) As Array

            Dim outmsg(0) As String
            Try
                Dim db As Database = dbf.CreateConnection()
                Dim dbCommand As DbCommand = db.GetStoredProcCommand("MARREG_DSC_Registration_Insert")
                db.AddInParameter(dbCommand, "@REG_ID", DbType.Int16, Registration_Id)

                db.AddInParameter(dbCommand, "@Mo_Cd", DbType.[String], Mo_Cd)
                db.AddInParameter(dbCommand, "@Content_Type", DbType.[String], Content_Type)
                db.AddInParameter(dbCommand, "@Certificate_Verified", DbType.[String], Certificate_Verified)
                db.AddInParameter(dbCommand, "@Subject", DbType.[String], Subject)
                db.AddInParameter(dbCommand, "@Issuer_Name", DbType.[String], Issuer_Name)
                db.AddInParameter(dbCommand, "@SubjectName", DbType.[String], Subject_Name)
                db.AddInParameter(dbCommand, "@EmailName", DbType.[String], EmailName)
                db.AddInParameter(dbCommand, "@Signature_Algorithm", DbType.String, Signature_Algorithm)
                db.AddInParameter(dbCommand, "@Public_Key_Length", DbType.[String], Public_Key_Length)

                db.AddInParameter(dbCommand, "@Issue_Date", DbType.DateTime, Issue_Date)
                db.AddInParameter(dbCommand, "@Valid_Date", DbType.DateTime, Valid_Date)
                db.AddInParameter(dbCommand, "@Public_Key_Str", DbType.[String], Public_Key_Str)
                db.AddInParameter(dbCommand, "@Thumbprint", DbType.[String], Thumbprint)
                db.AddInParameter(dbCommand, "@SerialNumber", DbType.[String], SerialNumber)
                db.AddInParameter(dbCommand, "@Version", DbType.String, Version)
                db.AddInParameter(dbCommand, "@Oid", DbType.[String], Oid)

                db.AddOutParameter(dbCommand, "@Msg", DbType.[String], 50)

                db.ExecuteNonQuery(dbCommand)
                outmsg(0) = Convert.ToString(db.GetParameterValue(dbCommand, "@Msg"))


            Catch ex As Exception
                outmsg(0) = "0"

            End Try
            Return outmsg
        End Function
        Function DSCRegList() As IDataReader
            Dim dr As IDataReader = Nothing
            Try
                con.Open()
                Using command As New SqlCommand("MARREG_DSCRegList", con)
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
        Public Function DSCDataTransferMO() As Int32
            Dim ReturnVal As New Int32
            Try
                con.Open()
                Using command As New SqlCommand("MARREG_DSCDataTransferMO", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@Registration_Id", SqlDbType.NVarChar).Value = Registration_Id

                    command.Parameters.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.Output
                    command.ExecuteNonQuery()

                    ReturnVal = Convert.ToInt32(command.Parameters("@returnValue").Value.ToString())
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                con.Close()
            End Try
            Return ReturnVal
        End Function
        Public Function DSCDataLost() As Int32
            Dim ReturnVal As New Int32
            Try
                con.Open()
                Using command As New SqlCommand("MARREG_DSCDataLost", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@Registration_Id", SqlDbType.NVarChar).Value = Registration_Id

                    command.Parameters.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.Output
                    command.ExecuteNonQuery()

                    ReturnVal = Convert.ToInt32(command.Parameters("@returnValue").Value.ToString())
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                con.Close()
            End Try
            Return ReturnVal
        End Function
        Public Function DSCStatChange() As Int32
            Dim ReturnVal As New Int32
            Try
                con.Open()
                Using command As New SqlCommand("MARREG_DSCStatChange", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@Registration_Id", SqlDbType.NVarChar).Value = Registration_Id

                    command.Parameters.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.Output
                    command.ExecuteNonQuery()

                    ReturnVal = Convert.ToInt32(command.Parameters("@returnValue").Value.ToString())
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                con.Close()
            End Try
            Return ReturnVal
        End Function
        Public Function DSCDataDelete() As Int32
            Dim ReturnVal As New Int32
            Try
                con.Open()
                Using command As New SqlCommand("MARREG_DSCDataDelete", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@Registration_Id", SqlDbType.NVarChar).Value = Registration_Id

                    command.Parameters.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.Output
                    command.ExecuteNonQuery()

                    ReturnVal = Convert.ToInt32(command.Parameters("@returnValue").Value.ToString())
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                con.Close()
            End Try
            Return ReturnVal
        End Function
        Function LoadMODSCDetails() As IDataReader
            Dim dr As IDataReader = Nothing
            Try
                con.Open()
                Using command As New SqlCommand("MARREG_LoadMODSCDetails", con)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@Mo_Cd", SqlDbType.NVarChar).Value = Mo_Cd
                    dr = command.ExecuteReader()
                End Using
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                'con.Close()
            End Try
            Return dr
        End Function
    End Class
End Namespace