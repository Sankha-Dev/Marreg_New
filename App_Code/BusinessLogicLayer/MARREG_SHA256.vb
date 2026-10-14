Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports System.Security.Cryptography
Imports System.Text

Public Class MARREG_SHA256
    Public cAesIV256 As String = ConfigurationManager.AppSettings.Item("civ256").ToString()
    Public cAesKey256 As String = ConfigurationManager.AppSettings.Item("ckey256").ToString()

    Dim AesIV256 As String = ConfigurationManager.AppSettings.Item("iv256").ToString()
    Dim AesKey256 As String = ConfigurationManager.AppSettings.Item("key256").ToString()

    Dim aes As New AesCryptoServiceProvider

    Public Sub New()
        aes.BlockSize = 128
        aes.KeySize = 256
        aes.IV = Encoding.UTF8.GetBytes(AesIV256)
        aes.Key = Encoding.UTF8.GetBytes(AesKey256)
        aes.Mode = CipherMode.CBC
        aes.Padding = PaddingMode.PKCS7
    End Sub
    Public Sub newaes()
        aes.BlockSize = 128
        aes.KeySize = 256
        aes.IV = Encoding.UTF8.GetBytes(cAesIV256)
        aes.Key = Encoding.UTF8.GetBytes(cAesKey256)
        aes.Mode = CipherMode.CBC
        aes.Padding = PaddingMode.PKCS7
    End Sub

    Public Function Encryption(plainText As String) As String
        Dim src As Byte() = Encoding.ASCII.GetBytes(plainText)
        Using encrypt As ICryptoTransform = aes.CreateEncryptor
            Dim dest As Byte() = encrypt.TransformFinalBlock(src, 0, src.Length)
            Dim ENCDATA = Convert.ToBase64String(dest)
            Return ENCDATA
        End Using
    End Function
    Public Function Decryption(encryptData As String) As String
        Dim src As Byte() = System.Convert.FromBase64String(encryptData)
        Try
            Using decrypt As ICryptoTransform = aes.CreateDecryptor
                Dim dest As Byte() = decrypt.TransformFinalBlock(src, 0, src.Length)
                Dim plainText = Encoding.ASCII.GetString(dest)
                Return plainText
            End Using
        Catch ex As Exception
            Dim exceptionType As Type = ex.GetType
            If (exceptionType.Name = "CryptographicException") Then
                newaes()
            End If
            Using decrypt As ICryptoTransform = aes.CreateDecryptor
                Dim dest As Byte() = decrypt.TransformFinalBlock(src, 0, src.Length)
                Dim plainText = Encoding.ASCII.GetString(dest)
                Return plainText
            End Using
        End Try
    End Function
    Public Function CoputeHash(plainText As String) As String
        Using sha256Hash As SHA256 = SHA256.Create
            Dim bytes As Byte() = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(plainText))
            Dim builder As StringBuilder = New StringBuilder()
            For i As Integer = 0 To bytes.Length() - 1
                builder.Append(bytes(i).ToString("x2"))
            Next i
            Return builder.ToString()
        End Using
    End Function
    'Added on 17-10-2023 for image byte encryption
    Shared sAesIV256 As String = ConfigurationManager.AppSettings.Item("civ256").ToString()
    Shared sAesKey256 As String = ConfigurationManager.AppSettings.Item("ckey256").ToString()
    Public Shared Function EncryptArray(ByRef data As Byte()) As Byte()
        Dim key() As Byte = Encoding.UTF8.GetBytes(sAesKey256)
        Dim IV() As Byte = Encoding.UTF8.GetBytes(sAesIV256)
        Using cp As New AesCryptoServiceProvider
            Using ms As New MemoryStream
                Using cs As New CryptoStream(ms, cp.CreateEncryptor(key, IV), CryptoStreamMode.Write)
                    cs.Write(data, 0, data.Length)
                    cs.FlushFinalBlock()
                    Return ms.ToArray()
                End Using
            End Using
        End Using
    End Function

    Public Shared Sub DecryptArray(ByRef data As Byte())
        Dim key() As Byte = Encoding.UTF8.GetBytes(sAesKey256)
        Dim IV() As Byte = Encoding.UTF8.GetBytes(sAesIV256)
        Using cp As New AesCryptoServiceProvider
            Using ms As New MemoryStream(data)
                Using cs As New CryptoStream(ms, cp.CreateDecryptor(key, IV), CryptoStreamMode.Read)
                    Using ms2 As New MemoryStream()
                        cs.CopyTo(ms2)
                        data = ms2.ToArray()
                    End Using
                End Using
            End Using
        End Using
    End Sub
    'End Added on 17-10-2023
End Class
