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
Imports System.Data

<WebService(Namespace:="http://tempuri.org/")>
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Public Class AppAPIService
    Inherits System.Web.Services.WebService
    Dim a As New Marreg_Class
    Private ReadOnly db As New AppAPI_DB()
    Dim passwordsalt1 As New PassSalt()
    Public dbconnctn As New DataBaseHelperDB()
    Private user_info As Object

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

    ' Define the UserInfo class
    Public Class UserInfo
        Public Property role_nm As String
        Public Property role_id As String
        Public Property mo_cd As String
        Public Property ph_no As String
    End Class

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
                response("success") = "1"
                response("message") = "Method not allowed"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.MethodNotAllowed, Integer) '405
                Exit Sub
            End If

            If HttpContext.Current.Request.Headers("X-CSRF-TOKEN") = "" Then
                response("success") = "1"
                response("message") = "Method not allowed"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.MethodNotAllowed, Integer) '405
                Exit Sub
            End If

            If token = ConfigurationManager.AppSettings("api_key").ToString() Then
                If HttpContext.Current.Session Is Nothing Then
                    response("success") = "1"
                    response("message") = "Session is not initialized."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.PaymentRequired, Integer) '402
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
                    response("success") = "7"
                    response("enum") = "app"
                    response("message") = "success"
                    response("userid") = a.sqlInj_out(email)
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer) '200

                    'json("pswd") = pswd
                    'json("pass_encpt") = pass_encpt
                Else
                    response("success") = "1"
                    response("message") = "Incorrect credentials"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer) '400
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            response("success") = "0"
            response("message") = ex.Message
            response("exception_message") = "INVALID DETAILS"
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Forbidden, Integer) '403
        Finally
            response = If(response.Count < 1, json, response)
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "application/json"
            HttpContext.Current.Response.Write(javaScriptSerializer.Serialize(response))
        End Try
    End Sub
#End Region

#Region "User Details"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub userDetailsService()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()

        Try
            Dim ajax = HttpContext.Current.Request.Headers("X-Requested-With")
            Dim token = HttpContext.Current.Request.Headers("X-CSRF-TOKEN")
            Dim method = HttpContext.Current.Request.HttpMethod

            If Not (method = "POST" And ajax = "XMLHttpRequest") Then
                json("success") = "0"
                json("message") = "Method not allowed"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.MethodNotAllowed, Integer) '405
                Exit Sub
            End If

            If String.IsNullOrEmpty(token) Then
                json("success") = "0"
                json("message") = "Unauthorized access"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer) '401
                Exit Sub
            End If

            If token = ConfigurationManager.AppSettings("api_key").ToString() Then
                If HttpContext.Current.Session Is Nothing Then
                    json("success") = "0"
                    json("message") = "Session is not initialized."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer) '400
                    Exit Sub
                End If

                Dim email As String = If(Not String.IsNullOrEmpty(Request("email")), Request("email").Trim().ToLower(), "")
                Dim enum_app As String = If(Not String.IsNullOrEmpty(Request("enum")), Request("enum").Trim().ToLower(), "")

                If String.IsNullOrWhiteSpace(email) Then
                    json("success") = "0"
                    json("message") = "Invalid Username"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer) '400
                    Exit Sub
                End If

                If enum_app = "app" Then
                    Dim dtable As DataTable = db.GetUserDetails(a.sqlInj_in(email)) ' Call the function from AppAPI_DB

                    If dtable IsNot Nothing AndAlso dtable.Rows.Count > 0 Then
                        Dim row As DataRow = dtable.Rows(0)
                        If user_info Is Nothing Then
                            user_info = New UserInfo() ' Instantiate UserInfo
                        End If
                        user_info.role_nm = If(row("Name") IsNot DBNull.Value, row("Name").ToString(), "")
                        user_info.role_id = If(row("Designation") IsNot DBNull.Value, row("Designation").ToString(), "")
                        user_info.mo_cd = If(row("MO_cd") IsNot DBNull.Value, row("MO_cd").ToString(), "")
                        user_info.ph_no = If(row("Ph_No") IsNot DBNull.Value, row("Ph_No").ToString(), "")

                        json("success") = "7"
                        json("enum") = "app"
                        json("Name") = user_info.role_nm
                        json("Designation") = user_info.role_id
                        json("MO_cd") = user_info.mo_cd
                        json("Ph_No") = user_info.ph_no
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer) '200
                    Else
                        json("success") = "0"
                        json("message") = "User not found"
                        HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.NotFound, Integer) '404
                    End If
                Else
                    json("success") = "0"
                    json("message") = "Incorrect credentials"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer) '400
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            json("success") = "0"
            json("message") = ex.Message
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer) '400
        Finally
            response = If(response.Count < 1, json, response)
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "application/json"
            HttpContext.Current.Response.Write(javaScriptSerializer.Serialize(response))
        End Try
    End Sub
#End Region

#Region "Application Details"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub getApplicantDetails()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()

        Try
            Dim ajax = HttpContext.Current.Request.Headers("X-Requested-With")
            Dim token = HttpContext.Current.Request.Headers("X-CSRF-TOKEN")
            Dim method = HttpContext.Current.Request.HttpMethod

            If Not (method = "POST" And ajax = "XMLHttpRequest") Then
                json("success") = "0"
                json("message") = "Method not allowed"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.MethodNotAllowed, Integer) '405
                Exit Sub
            End If

            If String.IsNullOrEmpty(token) Then
                json("success") = "0"
                json("message") = "Unauthorized access"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer) '401
                Exit Sub
            End If

            Dim enum_app As String = If(Not String.IsNullOrEmpty(Request("enum")), Request("enum").Trim().ToLower(), "")

            If Not token = ConfigurationManager.AppSettings("api_key").ToString() And enum_app = "app" Then
                json("success") = "0"
                json("message") = "Invalid API Key"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Forbidden, Integer) '403
                Exit Sub
            End If

            If HttpContext.Current.Session Is Nothing Then
                json("success") = "0"
                json("message") = "Session is not initialized."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer) '400
                Exit Sub
            End If

            Dim mocode As String = If(Not String.IsNullOrEmpty(Request("m_cd")), Request("m_cd").Trim().ToLower(), "")

            Dim dtable As DataTable = db.GetApplicationList(mocode)

            If dtable IsNot Nothing AndAlso dtable.Rows.Count > 0 Then
                Dim applicants As New List(Of Object)
                For Each row As DataRow In dtable.Rows
                    applicants.Add(New With {
                        .application_cd = row("application_cd").ToString(),
                        .App_Auto_Cd = row("App_Auto_Cd").ToString()
                    })
                Next
                json("success") = "7"
                json("enum") = "app"
                json("applications") = applicants
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer) '200
            Else
                json("success") = "0"
                json("message") = "No applicants found"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.NotFound, Integer) '404
            End If

        Catch ex As Exception
            json("success") = "0"
            json("message2") = ex.Message
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer) '400
        Finally
            response = If(response.Count < 1, json, response)
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "application/json"
            HttpContext.Current.Response.Write(javaScriptSerializer.Serialize(response))
        End Try
    End Sub
#End Region

#Region "Certified Copy Reason"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub getCertifiedCopyReasons()
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()

        Try
            Dim ajax = HttpContext.Current.Request.Headers("X-Requested-With")
            Dim token = HttpContext.Current.Request.Headers("X-CSRF-TOKEN")
            Dim method = HttpContext.Current.Request.HttpMethod

            If Not (method = "POST" And ajax = "XMLHttpRequest") Then
                json("success") = "0"
                json("message") = "Method not allowed"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.MethodNotAllowed, Integer) '405
                Exit Sub
            End If

            If String.IsNullOrEmpty(token) Then
                json("success") = "0"
                json("message") = "Unauthorized access"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer) '401
                Exit Sub
            End If

            Dim enum_app As String = If(Not String.IsNullOrEmpty(HttpContext.Current.Request.Form("enum")), HttpContext.Current.Request.Form("enum").Trim().ToLower(), "")

            If Not token = ConfigurationManager.AppSettings("api_key").ToString() And enum_app = "app" Then
                json("success") = "0"
                json("message") = "Invalid API Key"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Forbidden, Integer) '403
                Exit Sub
            End If

            If HttpContext.Current.Session Is Nothing Then
                json("success") = "0"
                json("message") = "Session is not initialized."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer) '400
                Exit Sub
            End If

            Dim dtable As DataTable = db.GetCertifiedCopyReasons()

            If dtable IsNot Nothing AndAlso dtable.Rows.Count > 0 Then
                Dim reasons As New List(Of Object)
                For Each row As DataRow In dtable.Rows
                    reasons.Add(New With {
                    .CertifiedCopy_Reason_cd = row("CertifiedCopy_Reason_cd").ToString(),
                    .CertifiedCopy_Reason = row("CertifiedCopy_Reason").ToString()
                })
                Next
                json("success") = "7"
                json("enum") = "app"
                json("reasons") = reasons
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer) '200
            Else
                json("success") = "0"
                json("message") = "No data found"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.NotFound, Integer) '404
            End If

        Catch ex As Exception
            json("success") = "0"
            json("message") = ex.Message
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer) '400
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

    Public Function LoadUserDetails(userid As String)
        Try
            Dim dtable As DataTable = db.GetUserDetails(userid) ' Call the function from AppAPI_DB

            If dtable.Rows.Count > 0 Then
                Dim row As DataRow = dtable.Rows(0)
                user_info.role_nm = row("Name").ToString()
                user_info.role_id = row("Designation").ToString()
                user_info.mo_cd = row("MO_cd").ToString()
                user_info.ph_no = row("Ph_No").ToString()
            End If
            a.role_type_code(user_info.role_nm, user_info.role_type_cd)
        Catch ex As Exception
            ' Log the error if needed
            Throw New Exception("Error loading user details: " & ex.Message, ex)
        End Try
    End Function
End Class
