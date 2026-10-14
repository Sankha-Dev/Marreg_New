Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports MARREG.App_Code.DataAccessLayer
Imports System.Web.Script.Serialization
Imports System.Web.Script.Services
Imports passwordSalt.passwordSalt
Imports MARREG.App_Code
Imports MARREG.App_Code.BusinessLogicLayer
Imports System.Security.Cryptography
Imports System.IO
Imports System.Web.Hosting
Imports System.Text

<WebService(Namespace:="http://tempuri.org/")>
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Public Class AppLoginService
    Inherits System.Web.Services.WebService

    Dim a As New Marreg_Class
    Dim passwordsalt1 As New PassSalt()
    Public dbconnctn As New DataBaseHelperDB()

    Private Sub CreateSalt()
        Try
            Dim rng As New RNGCryptoServiceProvider()
            Dim buff As Byte() = New Byte(5 - 1) {}
            rng.GetBytes(buff)
            Session("MARREG_login_salt") = Convert.ToBase64String(buff)
        Catch ex As Exception
            ' Handle exception (log if needed)
        End Try
    End Sub

#Region "App User LOG IN"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub signinService()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
        Try
            Dim ajax = HttpContext.Current.Request.Headers("X-Requested-With")
            Dim token = HttpContext.Current.Request.Headers("X-CSRF-TOKEN")
            Dim method = HttpContext.Current.Request.HttpMethod
            If Not (method = "POST" And ajax = "XMLHttpRequest") Then
                json("success") = "1"
                response("message") = "Method not allowed"
                HttpContext.Current.Response.StatusCode = 405
                Exit Sub
            End If

            If token = ConfigurationManager.AppSettings("api_key").ToString() Then
                If HttpContext.Current.Session Is Nothing Then
                    json("success") = "1"
                    response("message") = "Session is not initialized."
                    HttpContext.Current.Response.StatusCode = 400
                    Exit Sub
                End If

                If Session("MARREG_login_salt") Is Nothing Then
                    CreateSalt()
                End If

                Dim email As String = If(Not Request("email") Is Nothing, Request("email").Trim().ToLower(), "")
                email = a.sqlInj_in(email)
                Dim pswd As String = If(Not Request("password") Is Nothing, Request("password").Trim(), "")

                ' Convert password using convertme function
                pswd = ConvertPassword(pswd)

                Dim password_db As String = passwordsalt1.password_selecn(email, dbconnctn.GetConnectionString())
                Dim salt_val As String = Session("MARREG_login_salt").ToString()
                Dim pass_encpt As String = a.CreatePasswordHash(salt_val + password_db).ToLower()

                If pass_encpt = pswd Then
                    json("success") = "7"
                    json("enum") = "app"
                    json("userid") = a.sqlInj_out(email)

                    'json("pswd") = pswd
                    'json("pass_encpt") = pass_encpt
                Else
                    json("success") = "1"
                    response("message") = "Incorrect credentials"
                    HttpContext.Current.Response.StatusCode = 400
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            json("success") = "0"
            json("message") = ex.Message
            response("exception_message") =
            HttpContext.Current.Response.StatusCode = 400
        Finally
            response = If(response.Count < 1, json, response)
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "application/json"
            HttpContext.Current.Response.Write(javaScriptSerializer.Serialize(response))
        End Try
    End Sub
#End Region

    Private Function ConvertPassword(password As String) As String
        If String.IsNullOrEmpty(password) Then
            Return ""
        End If

        ' Double SHA-256 hash
        Dim hashedPassword = ComputeSHA256Hash(password)
        hashedPassword = ComputeSHA256Hash(hashedPassword)

        ' Append salt and hash again
        Dim salt = Session("MARREG_login_salt").ToString()
        Return ComputeSHA256Hash(salt + hashedPassword)
    End Function

    Private Function ComputeSHA256Hash(ByVal rawData As String) As String
        Using sha256 As SHA256 = SHA256.Create()
            Dim bytes As Byte() = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData))
            Dim builder As New StringBuilder()
            For Each t As Byte In bytes
                builder.Append(t.ToString("x2"))
            Next
            Return builder.ToString()
        End Using
    End Function
End Class
