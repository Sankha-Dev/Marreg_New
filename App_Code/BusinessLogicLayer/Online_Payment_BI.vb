Imports Microsoft.VisualBasic
Imports MARREG.App_Code.DataAccessLayer
Imports System.Data
Imports System.Data.SqlClient
Imports System.Security.Cryptography


Namespace MARREG.App_Code.BusinessLogicLayer
    Public Class Online_Payment_BI
        Dim pay As New Online_Payment_DB()

#Region "Checksum"
        Public Function ChecksumMD5(s As String) As String
            Return pay.ChecksumMD5(s)
        End Function
#End Region

#Region "Encryption"
        Public Function encryptFile(s As String, Key As String) As String
            Return pay.encryptFile(s, Key)
        End Function
#End Region

#Region "Decryption"
        Public Function decryptFile(s As String, Key As String) As String
            Return pay.decryptFile(s, Key)
        End Function
#End Region

    End Class
End Namespace
