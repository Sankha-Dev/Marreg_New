Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Collections.Generic
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Net.Mail
Imports System.Net
Imports CDONTS
Imports System.IO
Imports System.Web
Imports System.Web.Security
Imports System.Security
Imports System.Security.Cryptography
Imports System.Text


Namespace MARREG.App_Code.DataAccessLayer
    Public Class Online_Payment_DB


#Region "Checksum"
        Public Function ChecksumMD5(s As String) As String
            Dim md5Hash As New System.Security.Cryptography.MD5CryptoServiceProvider()
            Dim data As Byte() = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(s))
            Dim sBuilder As New StringBuilder()
            Dim i As Integer
            For i = 0 To data.Length - 1
                sBuilder.Append(data(i).ToString("x2"))
            Next i
            Return sBuilder.ToString()
        End Function
#End Region

#Region "Encryption"
        Public Function encryptFile(s As String, Key As String) As String
            Dim rijndaelCipher As New RijndaelManaged()
            rijndaelCipher.Mode = CipherMode.CBC
            rijndaelCipher.Padding = PaddingMode.PKCS7

            rijndaelCipher.KeySize = &H80
            rijndaelCipher.BlockSize = &H80
            Dim pwdBytes As Byte() = Encoding.UTF8.GetBytes(Key)
            Dim keyBytes As Byte() = New Byte(15) {}
            Dim len As Integer = pwdBytes.Length
            If len > keyBytes.Length Then
                len = keyBytes.Length
            End If
            Array.Copy(pwdBytes, keyBytes, len)
            rijndaelCipher.Key = keyBytes
            rijndaelCipher.IV = keyBytes
            Dim transform As ICryptoTransform = rijndaelCipher.CreateEncryptor()
            Dim plainText As Byte() = Encoding.UTF8.GetBytes(s)
            Return Convert.ToBase64String(transform.TransformFinalBlock(plainText, 0, plainText.Length))
        End Function
#End Region

#Region "Decryption"
        Public Function decryptFile(s As String, Key As String) As String
            Dim rijndaelCipher As New RijndaelManaged()
            rijndaelCipher.Mode = CipherMode.CBC
            rijndaelCipher.Padding = PaddingMode.PKCS7

            rijndaelCipher.KeySize = &H80
            rijndaelCipher.BlockSize = &H80
            Dim encryptedData As Byte() = Convert.FromBase64String(s)
            Dim pwdBytes As Byte() = Encoding.UTF8.GetBytes(Key)
            Dim keyBytes As Byte() = New Byte(15) {}
            Dim len As Integer = pwdBytes.Length
            If len > keyBytes.Length Then
                len = keyBytes.Length
            End If
            Array.Copy(pwdBytes, keyBytes, len)
            rijndaelCipher.Key = keyBytes
            rijndaelCipher.IV = keyBytes
            Dim plainText As Byte() = rijndaelCipher.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length)
            Return Encoding.UTF8.GetString(plainText)
        End Function
#End Region

    End Class
End Namespace
