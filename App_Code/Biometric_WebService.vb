Imports System.Web
Imports System.Web.Script.Serialization
Imports System.Web.Script.Services
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports MARREG.App_Code
Imports MARREG.App_Code.BusinessLogicLayer

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Public Class Biometric_WebService
    Inherits System.Web.Services.WebService
    'Added on 09-11-2023
    Public encdec As New EncryptionDecryption.EncryptionDecryption()
    'End Added on 09-11-2023
#Region "Register Device"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub RegisterDevice()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
        Try
            Dim ajax, token As String
            Dim method As String
            ajax = HttpContext.Current.Request.Headers("X-Requested-With")
            token = HttpContext.Current.Request.Headers("X-CSRF-TOKEN")
            method = HttpContext.Current.Request.HttpMethod
            If Not (method = "POST" And ajax = "XMLHttpRequest") Then
                response("error") = "Method not allowed"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.MethodNotAllowed, Integer)
                Exit Sub
            End If
            If token = "" Then
                If Session.Item("Marreg_user_info") Is Nothing Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            Else
                If Not token = Session("CSRF") Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            End If

            Dim model As String = If(Not Request("model") Is Nothing, DirectCast(Request("model"), String).Trim(), "")
            Dim sl_no As String = If(Not Request("sl_no") Is Nothing, DirectCast(Request("sl_no"), String).Trim(), "")
            Dim manufacturer As String = If(Not Request("manufacturer") Is Nothing, DirectCast(Request("manufacturer"), String).Trim(), "")
            'Added on 22-11-2023 for customization of capturing procedure
            Dim quality As String = If(Not Request("quality") Is Nothing, DirectCast(Request("quality"), String), "0")
            'End Added On 22-11-2023 For customization Of capturing procedure
            Dim fpTemplate = "", fpTemplate2 = ""
            Dim finger = "", finger2 = ""
            Dim mo_cd As String = If(Not Request("mo_cd") Is Nothing, DirectCast(Request("mo_cd"), String).Trim(), "")

            Dim datalist As String = If(Not (Request("datalist") Is Nothing Or Request("datalist") = ""), DirectCast(Request("datalist"), String).Trim(), "")
            If datalist = "" Or datalist Is Nothing Then
                response("error") = "Please initiate the process again."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            Dim data_list = javaScriptSerializer.DeserializeObject(datalist)
            Dim i As Int32 = 0
            For Each data As Object In data_list
                If (i = 0) Then
                    fpTemplate = data("fpTemplate")
                    finger = data("finger")
                ElseIf (i = 1) Then
                    fpTemplate2 = data("fpTemplate")
                    finger2 = data("finger")
                End If
                i += 1
            Next

            If (model = "" Or sl_no = "" Or manufacturer = "" Or fpTemplate = "" Or finger = "") Then
                response("error") = "Biometric Device not found"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (mo_cd = "" Or mo_cd Is Nothing) Then
                response("error") = "Please select MO Name."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            'Added on 22-11-2023 for customization of capturing procedure
            If (quality = "0" Or mo_cd Is Nothing Or Not (IsNumber(quality))) Then
                response("error") = "Please enter Capture Quality properly."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            'End Added On 22-11-2023 For customization Of capturing procedure
            Dim Registration_Id As Int32 = 0
            Dim m As New Biometric_BL
            'Dim res = m.RegisterDevice(Registration_Id, model, sl_no, manufacturer, mo_cd, fpTemplate, finger, fpTemplate2, finger2)
            'Added on 22-11-2023 for customization of capturing procedure
            Dim res = m.RegisterDevice(Registration_Id, model, sl_no, manufacturer, mo_cd, fpTemplate, finger, fpTemplate2, finger2, Convert.ToInt32(quality))
            'End Added On 22-11-2023 For customization Of capturing procedure
            If res = "1" Then
                response("success") = "Device registered successfully"
            ElseIf res = "2" Then
                response("error") = "Device could not be registered"
            ElseIf res = "3" Then
                response("error") = "You have already valid registered device"
            ElseIf res = "4" Then
                response("error") = "Device already registered"
            Else
                response("error") = res
            End If
        Catch ex As Exception
            response("exception") = "true"
            response("exception_message") = ex.Message
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
        Finally
            response = If(response.Count < 1, json, response)
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "application/json"
            HttpContext.Current.Response.Write(javaScriptSerializer.Serialize(response))
        End Try
    End Sub
#End Region
#Region "Device Details"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub RegisteredDeviceDetails()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
        Try
            Dim auth = 0
            Dim ajax, token As String
            Dim method As String
            ajax = HttpContext.Current.Request.Headers("X-Requested-With")
            token = HttpContext.Current.Request.Headers("X-CSRF-TOKEN")
            method = HttpContext.Current.Request.HttpMethod
            If Not (method = "POST" And ajax = "XMLHttpRequest") Then
                response("error") = "Method not allowed"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.MethodNotAllowed, Integer)
                Exit Sub
            End If
            If token = "" Then
                auth = 1
                If Session.Item("Marreg_user_info") Is Nothing Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            Else
                If Not token = Session("CSRF") Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            End If

            Dim mo_cd As String = If(Not (Request("mocd") Is Nothing Or Request("mocd") = ""), Convert.ToString(Request("mocd").Trim()), "")
            Dim obj As New Biometric_BL()
            Dim DataList = obj.MORegisteredDeviceDetails(mo_cd)
            If (DataList.Capacity > 1) Then
                json("DataList") = DataList
                json("success") = 1
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
            Else
                json("error") = 1
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
            End If

        Catch ex As Exception
            response("exception") = "true"
            response("exception_message") = ex.Message + "<br>" + "Click <a onclick='location.reload()' href='#'>here</a> to reload"
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
        Finally
            response = If(response.Count < 1, json, response)
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "application/json"
            HttpContext.Current.Response.Write(javaScriptSerializer.Serialize(response))
        End Try
    End Sub
#End Region
#Region "Registerd Device List"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub RegisteredDeviceList()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
        Dim myDataTable As New MyNestedDataTable()

        Try
            Dim auth = 0
            Dim ajax, token As String
            Dim method As String
            ajax = HttpContext.Current.Request.Headers("X-Requested-With")
            token = HttpContext.Current.Request.Headers("X-CSRF-TOKEN")
            method = HttpContext.Current.Request.HttpMethod
            If Not (method = "POST" And ajax = "XMLHttpRequest") Then
                response("error") = "Method not allowed"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.MethodNotAllowed, Integer)
                Exit Sub
            End If
            If token = "" Then
                auth = 1
                If Session.Item("Marreg_user_info") Is Nothing Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            Else
                If Not token = Session("CSRF") Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            End If

            'Dim status As String = If(Not (Request("status") Is Nothing Or Request("status") = ""), Convert.ToString(Request("status")), "")
            Dim draw As String = If(Not Request("draw") Is Nothing, DirectCast(Request("draw"), String), "1")
            Dim start As String = If(Not Request("start") Is Nothing, DirectCast(Request("start"), String), "0")
            Dim length As String = If(Not Request("length") Is Nothing, DirectCast(Request("length"), String), "10")
            Dim value As String = If(Not Request("search[value]") Is Nothing, DirectCast(Request("search[value]"), String).Trim(), "")
            Dim orderByColumn As String = If(Request("order[0][column]") Is Nothing, "", DirectCast(Request("order[0][column]"), String))
            Dim orderByDir As String = If(Request("order[0][dir]") Is Nothing, "", DirectCast(Request("order[0][dir]"), String))
            Dim order_by As String = "Entry_dt"

            Dim is_search As Boolean = False
            Dim is_order As Boolean = False
            'Select Case orderByColumn
            '    Case "0"
            '        order_by = "Groom_Name"
            '    Case "1"
            '        order_by = "Bride_Name"
            '    Case "2"
            '        order_by = "Reg_dt"
            '    Case "3"
            '        order_by = "Reg_PO"
            '    Case "4"
            '        order_by = "Reg_PS"
            '    Case "5"
            '        order_by = "Reg_Dist"
            '    Case Else
            '        order_by = "requested_on"
            'End Select
            is_order = True

            myDataTable = Biometric_BL.RegisteredDeviceList(draw, start, length, value, order_by, orderByDir)

            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
        Catch ex As Exception
            response("exception") = "true"
            response("exception_message") = ex.Message + "<br>" + "Click <a onclick='location.reload()' href='#'>here</a> to reload"
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
        Finally
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "application/json"
            If response.Count < 1 Then
                HttpContext.Current.Response.Write(javaScriptSerializer.Serialize(myDataTable))
            Else
                HttpContext.Current.Response.Write(javaScriptSerializer.Serialize(response))
            End If
        End Try
    End Sub
#End Region
#Region "Registerd Device Status Change"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub RegisterdDeviceStatChange()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
        Dim enc As New MARREG_SHA256()
        Try
            Dim auth = 0
            Dim ajax, token As String
            Dim method As String
            ajax = HttpContext.Current.Request.Headers("X-Requested-With")
            token = HttpContext.Current.Request.Headers("X-CSRF-TOKEN")
            method = HttpContext.Current.Request.HttpMethod
            If Not (method = "POST" And ajax = "XMLHttpRequest") Then
                response("error") = "Method not allowed"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.MethodNotAllowed, Integer)
                Exit Sub
            End If
            If token = "" Then
                auth = 1
                If Session.Item("Marreg_user_info") Is Nothing Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            Else
                If Not token = Session("CSRF") Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            End If
            Dim id As Int64 = If(Not (Request("id") Is Nothing Or Request("id") = ""), Convert.ToInt64(Request("id").Trim()), 0)
            Dim param As String = If(Not (Request("param") Is Nothing Or Request("param") = ""), Convert.ToString(Request("param").Trim()), "")
            If (id = 0) Then
                response("error") = "No record selected for change status."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If


            Dim obj = New Biometric_BL()
            Dim msg = obj.RegisterdDeviceStatChange(id)
            If (msg = 1) Then
                json("success") = "Successfully updated status for registration data."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
            Else
                json("error") = "No status has been updated."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
            End If
        Catch ex As Exception
            response("exception") = "true"
            response("exception_message") = ex.Message
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
        Finally
            response = If(response.Count < 1, json, response)
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "application/json"
            HttpContext.Current.Response.Write(javaScriptSerializer.Serialize(response))
        End Try
    End Sub
#End Region
#Region "Registerd Device Delete"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub RegisterdDeviceDelete()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
        Dim enc As New MARREG_SHA256()
        Try
            Dim auth = 0
            Dim ajax, token As String
            Dim method As String
            ajax = HttpContext.Current.Request.Headers("X-Requested-With")
            token = HttpContext.Current.Request.Headers("X-CSRF-TOKEN")
            method = HttpContext.Current.Request.HttpMethod
            If Not (method = "POST" And ajax = "XMLHttpRequest") Then
                response("error") = "Method not allowed"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.MethodNotAllowed, Integer)
                Exit Sub
            End If
            If token = "" Then
                auth = 1
                If Session.Item("Marreg_user_info") Is Nothing Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            Else
                If Not token = Session("CSRF") Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            End If
            Dim id As Int64 = If(Not (Request("id") Is Nothing Or Request("id") = ""), Convert.ToInt64(Request("id").Trim()), 0)
            Dim mo_cd As String = If(Not (Request("mocd") Is Nothing Or Request("mocd") = ""), Convert.ToString(Request("mocd").Trim()), "")
            If (id = 0 Or mo_cd = "") Then
                response("error") = "No record selected for delete."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If

            Dim obj = New Biometric_BL()
            Dim msg = obj.RegisterdDeviceDelete(id, mo_cd)
            If (msg = "5") Then
                json("success") = "Record deleted successfully."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
            Else
                json("error") = "No record has been deleted."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
            End If
        Catch ex As Exception
            response("exception") = "true"
            response("exception_message") = ex.Message
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
        Finally
            response = If(response.Count < 1, json, response)
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "application/json"
            HttpContext.Current.Response.Write(javaScriptSerializer.Serialize(response))
        End Try
    End Sub
#End Region

#Region "Capture Biometric Save"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub SaveBiometricDetails()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
        Try
            Dim ajax, token As String
            Dim method As String
            ajax = HttpContext.Current.Request.Headers("X-Requested-With")
            token = HttpContext.Current.Request.Headers("X-CSRF-TOKEN")
            method = HttpContext.Current.Request.HttpMethod
            If Not (method = "POST" And ajax = "XMLHttpRequest") Then
                response("error") = "Method not allowed"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.MethodNotAllowed, Integer)
                Exit Sub
            End If
            If token = "" Then
                If Session.Item("Marreg_user_info") Is Nothing Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            Else
                If Not token = Session("CSRF") Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            End If
            Dim Draft_cd = Session("Draft_Number")
            Dim flag As String = If(Not Request("flag") Is Nothing, DirectCast(Request("flag"), String).Trim(), "")
            Dim fpTemplate As String = If(Not Request("fpTemplate") Is Nothing, DirectCast(Request("fpTemplate"), String).Trim(), "")
            Dim imageByte As String = If(Not Request("imageByte") Is Nothing, DirectCast(Request("imageByte"), String).Trim(), "")

            'If (model = "" Or sl_no = "" Or manufacturer = "" Or fpTemplate = "" Or finger = "") Then
            '    response("error") = "Biometric Device not found"
            '    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
            '    Exit Sub
            'End If
            'If (mo_cd = "" Or mo_cd Is Nothing) Then
            '    response("error") = "Please select MO Name."
            '    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
            '    Exit Sub
            'End If

            'Dim user_info As New Marreg_Loginfo
            'user_info = DirectCast(Session.Item("Marreg_user_info"), Marreg_Loginfo)
            'Dim mo_auto_cd = user_info.MO_cd
            'If (mo_cd <> mo_auto_cd) Then
            '    response("error") = "Please select MO Name."
            '    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
            '    Exit Sub
            'End If

            Dim Registration_Id As Int32 = 0
            Dim m As New Biometric_BL
            Dim res = m.SaveBiometricDetails(Draft_cd, flag, fpTemplate, imageByte)
            If res = "1" Then
                response("success") = "LTI Captured Successfully"
            ElseIf res = "2" Then
                response("error") = "LTI Already Captured"
            Else
                response("error") = res
            End If
        Catch ex As Exception
            response("exception") = "true"
            response("exception_message") = ex.Message
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
        Finally
            response = If(response.Count < 1, json, response)
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "application/json"
            HttpContext.Current.Response.Write(javaScriptSerializer.Serialize(response))
        End Try
    End Sub
#End Region
#Region "Biometric Functions Handling"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub BiometricFunctionsHandling()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
        Dim enc As New MARREG_SHA256()
        Try
            Dim auth = 0
            Dim ajax, token As String
            Dim method As String
            ajax = HttpContext.Current.Request.Headers("X-Requested-With")
            token = HttpContext.Current.Request.Headers("X-CSRF-TOKEN")
            method = HttpContext.Current.Request.HttpMethod
            If Not (method = "POST" And ajax = "XMLHttpRequest") Then
                response("error") = "Method not allowed"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.MethodNotAllowed, Integer)
                Exit Sub
            End If
            If token = "" Then
                auth = 1
                If Session.Item("Marreg_user_info") Is Nothing Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            Else
                If Not token = Session("CSRF") Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            End If
            Dim flag As String = If(Not (Request("flag") Is Nothing Or Request("flag") = ""), Convert.ToString(Request("flag").Trim()), "")
            json("success") = flag
            Session("mo_bio_verified") = flag
        Catch ex As Exception
            response("exception") = "true"
            response("exception_message") = ex.Message
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
        Finally
            response = If(response.Count < 1, json, response)
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "application/json"
            HttpContext.Current.Response.Write(javaScriptSerializer.Serialize(response))
        End Try
    End Sub
#End Region
#Region "Save Bride Groom Witness LTI"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub SaveBrideGroomWitnessLTI()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
        Dim enc As New MARREG_SHA256()
        Try
            Dim auth = 0
            Dim ajax, token As String
            Dim method As String
            ajax = HttpContext.Current.Request.Headers("X-Requested-With")
            token = HttpContext.Current.Request.Headers("X-CSRF-TOKEN")
            method = HttpContext.Current.Request.HttpMethod
            If Not (method = "POST" And ajax = "XMLHttpRequest") Then
                response("error") = "Method not allowed"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.MethodNotAllowed, Integer)
                Exit Sub
            End If
            If token = "" Then
                auth = 1
                If Session.Item("Marreg_user_info") Is Nothing Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            Else
                If Not token = Session("CSRF") Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            End If
            Dim app_auto_cd As String = If(Not (Request("app_auto_cd") Is Nothing Or Request("app_auto_cd") = ""), Convert.ToString(Request("app_auto_cd").Trim()), "")

            Dim g_fpTemplate As String = If(Not (Request("g_fpTemplate") Is Nothing Or Request("g_fpTemplate") = ""), Convert.ToString(Request("g_fpTemplate").Trim()), "")
            Dim g_fpBMPBase64 As String = If(Not (Request("g_fpBMPBase64") Is Nothing Or Request("g_fpBMPBase64") = ""), Convert.ToString(Request("g_fpBMPBase64").Trim()), "")

            Dim b_fpTemplate As String = If(Not (Request("b_fpTemplate") Is Nothing Or Request("b_fpTemplate") = ""), Convert.ToString(Request("b_fpTemplate").Trim()), "")
            Dim b_fpBMPBase64 As String = If(Not (Request("b_fpBMPBase64") Is Nothing Or Request("b_fpBMPBase64") = ""), Convert.ToString(Request("b_fpBMPBase64").Trim()), "")

            Dim wt1_fpTemplate As String = If(Not (Request("wt1_fpTemplate") Is Nothing Or Request("wt1_fpTemplate") = ""), Convert.ToString(Request("wt1_fpTemplate").Trim()), "")
            Dim wt1_fpBMPBase64 As String = If(Not (Request("wt1_fpBMPBase64") Is Nothing Or Request("wt1_fpBMPBase64") = ""), Convert.ToString(Request("wt1_fpBMPBase64").Trim()), "")

            Dim wt2_fpTemplate As String = If(Not (Request("wt2_fpTemplate") Is Nothing Or Request("wt2_fpTemplate") = ""), Convert.ToString(Request("wt2_fpTemplate").Trim()), "")
            Dim wt2_fpBMPBase64 As String = If(Not (Request("wt2_fpBMPBase64") Is Nothing Or Request("wt2_fpBMPBase64") = ""), Convert.ToString(Request("wt2_fpBMPBase64").Trim()), "")

            Dim wt3_fpTemplate As String = If(Not (Request("wt3_fpTemplate") Is Nothing Or Request("wt3_fpTemplate") = ""), Convert.ToString(Request("wt3_fpTemplate").Trim()), "")
            Dim wt3_fpBMPBase64 As String = If(Not (Request("wt3_fpBMPBase64") Is Nothing Or Request("wt3_fpBMPBase64") = ""), Convert.ToString(Request("wt3_fpBMPBase64").Trim()), "")

            Dim g_verified As String = If(Not (Request("g_verified") Is Nothing Or Request("g_verified") = ""), Convert.ToString(Request("g_verified").Trim()), "")
            Dim b_verified As String = If(Not (Request("b_verified") Is Nothing Or Request("b_verified") = ""), Convert.ToString(Request("b_verified").Trim()), "")

            If (g_fpTemplate = "" Or g_fpBMPBase64 = "") Then
                response("error") = "Please capture Groom/ Husband LTI."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (b_fpTemplate = "" Or b_fpBMPBase64 = "") Then
                response("error") = "Please capture Bride/ Wife LTI."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (wt1_fpTemplate = "" Or wt1_fpBMPBase64 = "") Then
                response("error") = "Please capture Witness 1 LTI."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (wt2_fpTemplate = "" Or wt2_fpBMPBase64 = "") Then
                response("error") = "Please capture Witness 2 LTI."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (wt3_fpTemplate = "" Or wt3_fpBMPBase64 = "") Then
                response("error") = "Please capture Witness 3 LTI."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (g_verified = "" Or b_verified = "") Then
                response("error") = "Please try again."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            'If (g_verified = "0" And b_verified = "0") Then
            '    response("error") = "Atleast one party need to be verified."
            '    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
            '    Exit Sub
            'End If

            Dim obj As New Biometric_BL
            Dim retval = obj.SaveBrideGroomWitnessLTI(0, app_auto_cd, g_fpTemplate, g_fpBMPBase64, b_fpTemplate, b_fpBMPBase64, wt1_fpTemplate, wt1_fpBMPBase64,
            wt2_fpTemplate, wt2_fpBMPBase64, wt3_fpTemplate, wt3_fpBMPBase64, g_verified, b_verified)
            json("success") = retval
            'Session("mo_bio_verified") = flag
        Catch ex As Exception
            response("exception") = "true"
            response("exception_message") = ex.Message
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
        Finally
            response = If(response.Count < 1, json, response)
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "application/json"
            HttpContext.Current.Response.Write(javaScriptSerializer.Serialize(response))
        End Try
    End Sub
#End Region
    'Added on 09-11-2023
#Region "Load Application Details"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub LoadApplicationDetails()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
        Dim enc As New MARREG_SHA256()
        Try
            Dim auth = 0
            Dim ajax, token As String
            Dim method As String
            ajax = HttpContext.Current.Request.Headers("X-Requested-With")
            token = HttpContext.Current.Request.Headers("X-CSRF-TOKEN")
            method = HttpContext.Current.Request.HttpMethod
            If Not (method = "POST" And ajax = "XMLHttpRequest") Then
                response("error") = "Method not allowed"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.MethodNotAllowed, Integer)
                Exit Sub
            End If
            If token = "" Then
                auth = 1
                If Session.Item("Marreg_user_info") Is Nothing Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            Else
                If Not token = Session("CSRF") Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            End If
            Dim app_no As String = If(Not (Request("app_no") Is Nothing Or Request("app_no") = ""), Convert.ToString(Request("app_no").Trim()), "")

            If (app_no = "") Then
                response("error") = "Please enter Application No."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If

            Dim obj As New Biometric_BL
            Dim retval = obj.LoadApplicationDetails(app_no)
            If (retval.Rows.Count > 0) Then
                'If (retval.Rows(0)("Status_Cd") = "09") Then
                Dim datas = New Dictionary(Of String, Object)
                datas("groom") = Convert.ToString(retval.Rows(0)("G_FullName"))
                datas("bride") = Convert.ToString(retval.Rows(0)("B_Fullname"))
                datas("wt1") = Convert.ToString(retval.Rows(0)("wt1_name"))
                datas("wt2") = Convert.ToString(retval.Rows(0)("wt2_name"))
                datas("wt3") = Convert.ToString(retval.Rows(0)("wt3_name"))
                datas("act") = Convert.ToString(retval.Rows(0)("Act_cd"))
                datas("app_auto_cd") = Convert.ToString(retval.Rows(0)("app_auto_cd"))
                datas("vault_id") = Convert.ToString(retval.Rows(0)("id"))

                datas("fpTemplate_groom") = encdec.Decrypt(Convert.ToString(retval.Rows(0)("fpTemplate_groom")))
                datas("fpTemplate_bride") = encdec.Decrypt(Convert.ToString(retval.Rows(0)("fpTemplate_bride")))
                datas("g_fpTemplate") = encdec.Decrypt(Convert.ToString(retval.Rows(0)("g_fpTemplate")))
                datas("b_fpTemplate") = encdec.Decrypt(Convert.ToString(retval.Rows(0)("b_fpTemplate")))
                datas("wt1_fpTemplate") = encdec.Decrypt(Convert.ToString(retval.Rows(0)("wt1_fpTemplate")))
                datas("wt2_fpTemplate") = encdec.Decrypt(Convert.ToString(retval.Rows(0)("wt2_fpTemplate")))
                datas("wt3_fpTemplate") = encdec.Decrypt(Convert.ToString(retval.Rows(0)("wt3_fpTemplate")))

                datas("wt1_captured") = Convert.ToString(retval.Rows(0)("wt1_captured"))
                datas("wt2_captured") = Convert.ToString(retval.Rows(0)("wt2_captured"))
                datas("wt3_captured") = Convert.ToString(retval.Rows(0)("wt3_captured"))

                datas("g_verified") = Convert.ToString(retval.Rows(0)("g_verified"))
                datas("b_verified") = Convert.ToString(retval.Rows(0)("b_verified"))

                datas("g_veri_req") = Convert.ToString(retval.Rows(0)("g_veri_req"))      'Added on 17-01-2024
                datas("b_veri_req") = Convert.ToString(retval.Rows(0)("b_veri_req"))      'Added on 17-01-2024
                datas("quality") = Convert.ToString(retval.Rows(0)("quality"))      'Added on 17-01-2024
                datas("verification_score") = Convert.ToString(retval.Rows(0)("verification_score"))      'Added on 17-01-2024

                json("success") = 1                         'data found
                    json("data") = datas
                'Else
                '    json("success") = -1                        'status not ok
                'End If
            Else
                json("success") = 0                         'no data found
            End If
            'Dim retval = obj.SaveBrideGroomWitnessLTI(0, app_auto_cd, g_fpTemplate, g_fpBMPBase64, b_fpTemplate, b_fpBMPBase64, wt1_fpTemplate, wt1_fpBMPBase64,
            'wt2_fpTemplate, wt2_fpBMPBase64, wt3_fpTemplate, wt3_fpBMPBase64, g_verified, b_verified)
            'json("success") = retval
            'Session("mo_bio_verified") = flag
        Catch ex As Exception
            response("exception") = "true"
            response("exception_message") = ex.Message
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
        Finally
            response = If(response.Count < 1, json, response)
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "application/json"
            HttpContext.Current.Response.Write(javaScriptSerializer.Serialize(response))
        End Try
    End Sub
#End Region
#Region "Reupload Bride Groom Witness LTI"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub ReuploadBrideGroomWitnessLTI()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
        Dim enc As New MARREG_SHA256()
        Try
            Dim auth = 0
            Dim ajax, token As String
            Dim method As String
            ajax = HttpContext.Current.Request.Headers("X-Requested-With")
            token = HttpContext.Current.Request.Headers("X-CSRF-TOKEN")
            method = HttpContext.Current.Request.HttpMethod
            If Not (method = "POST" And ajax = "XMLHttpRequest") Then
                response("error") = "Method not allowed"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.MethodNotAllowed, Integer)
                Exit Sub
            End If
            If token = "" Then
                auth = 1
                If Session.Item("Marreg_user_info") Is Nothing Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            Else
                If Not token = Session("CSRF") Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            End If
            Dim app_auto_cd As String = If(Not (Request("app_auto_cd") Is Nothing Or Request("app_auto_cd") = ""), Convert.ToString(Request("app_auto_cd").Trim()), "")
            Dim vault_id As String = If(Not (Request("vault_id") Is Nothing Or Request("vault_id") = ""), Convert.ToString(Request("vault_id").Trim()), "")
            Dim fpTemplate As String = If(Not (Request("fpTemplate") Is Nothing Or Request("fpTemplate") = ""), Convert.ToString(Request("fpTemplate").Trim()), "")
            Dim fpBMPBase64 As String = If(Not (Request("fpBMPBase64") Is Nothing Or Request("fpBMPBase64") = ""), Convert.ToString(Request("fpBMPBase64").Trim()), "")


            Dim verified As String = If(Not (Request("verified") Is Nothing Or Request("verified") = ""), Convert.ToString(Request("verified").Trim()), "")
            Dim type As String = If(Not (Request("type") Is Nothing Or Request("type") = ""), Convert.ToString(Request("type").Trim()), "")

            If (app_auto_cd = "") Then
                response("error") = "Invalid application no."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (vault_id = "") Then
                response("error") = "Invalid application no."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (fpTemplate = "" Or fpBMPBase64 = "") Then
                response("error") = "Please capture LTI."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (verified = "") Then
                response("error") = "Verified data missing."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If

            Dim obj As New Biometric_BL
            Dim retval = obj.ReuploadBrideGroomWitnessLTI(vault_id, app_auto_cd, fpTemplate, fpBMPBase64, verified, type)
            If (retval = "1") Then          'updated successfully
                json("msg") = "Successfully updated"
            ElseIf (retval = "2") Then      'invalid type specified
                json("msg") = "Invalid type specified"
            ElseIf (retval = "3") Then      'blank template
                json("msg") = "Please capture the LTI properly"
            ElseIf (retval = "-1") Then     'No such record found
                json("msg") = "Nothing to update"
            Else                            'other issue
                json("msg") = retval
            End If
            json("success") = retval
        Catch ex As Exception
            response("exception") = "true"
            response("exception_message") = ex.Message
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
        Finally
            response = If(response.Count < 1, json, response)
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "application/json"
            HttpContext.Current.Response.Write(javaScriptSerializer.Serialize(response))
        End Try
    End Sub
#End Region
    'End Added on 09-11-2023
    'Added on 09-01-2024
#Region "Load Permission Details"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub LoadPermissionDetails()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
        Dim enc As New MARREG_SHA256()
        Try
            Dim auth = 0
            Dim ajax, token As String
            Dim method As String
            ajax = HttpContext.Current.Request.Headers("X-Requested-With")
            token = HttpContext.Current.Request.Headers("X-CSRF-TOKEN")
            method = HttpContext.Current.Request.HttpMethod
            If Not (method = "POST" And ajax = "XMLHttpRequest") Then
                response("error") = "Method not allowed"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.MethodNotAllowed, Integer)
                Exit Sub
            End If
            If token = "" Then
                auth = 1
                If Session.Item("Marreg_user_info") Is Nothing Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            Else
                If Not token = Session("CSRF") Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            End If
            Dim app_no As String = If(Not (Request("app_no") Is Nothing Or Request("app_no") = ""), Convert.ToString(Request("app_no").Trim()), "")

            If (app_no = "") Then
                response("error") = "Please enter Application No."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            Dim obj As New Biometric_BL
            Dim retval = obj.LoadPermissionDetails(app_no)
            If (retval.Rows.Count > 0) Then
                Dim datas = New Dictionary(Of String, Object)
                datas("groom") = Convert.ToString(retval.Rows(0)("G_FullName"))
                datas("bride") = Convert.ToString(retval.Rows(0)("B_Fullname"))
                datas("app_auto_cd") = Convert.ToString(retval.Rows(0)("app_auto_cd"))
                datas("act") = Convert.ToString(retval.Rows(0)("Act_cd"))
                datas("groom_captured_f") = Convert.ToString(retval.Rows(0)("groom_captured_f"))
                datas("Entry_dt_groom") = If(Not retval.Rows(0)("Entry_dt_groom") Is DBNull.Value, Convert.ToDateTime(retval.Rows(0)("Entry_dt_groom")).ToString("dd-MM-yyyy"), "")
                datas("bride_captured_f") = Convert.ToString(retval.Rows(0)("bride_captured_f"))
                datas("Entry_dt_bride") = If(Not retval.Rows(0)("Entry_dt_bride") Is DBNull.Value, Convert.ToDateTime(retval.Rows(0)("Entry_dt_bride")).ToString("dd-MM-yyyy"), "")
                datas("groom_captured_d") = Convert.ToString(retval.Rows(0)("groom_captured_d"))
                datas("bride_captured_d") = Convert.ToString(retval.Rows(0)("bride_captured_d"))

                json("success") = 1                         'data found
                json("data") = datas
            Else
                json("success") = 0                         'no data found
            End If
        Catch ex As Exception
            response("exception") = "true"
            response("exception_message") = ex.Message
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
        Finally
            response = If(response.Count < 1, json, response)
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "application/json"
            HttpContext.Current.Response.Write(javaScriptSerializer.Serialize(response))
        End Try
    End Sub
#End Region
#Region "Save Permission Details"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub SaveBiometricPermission()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
        Dim enc As New MARREG_SHA256()
        Try
            Dim auth = 0
            Dim ajax, token As String
            Dim method As String
            ajax = HttpContext.Current.Request.Headers("X-Requested-With")
            token = HttpContext.Current.Request.Headers("X-CSRF-TOKEN")
            method = HttpContext.Current.Request.HttpMethod
            If Not (method = "POST" And ajax = "XMLHttpRequest") Then
                response("error") = "Method not allowed"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.MethodNotAllowed, Integer)
                Exit Sub
            End If
            If token = "" Then
                auth = 1
                If Session.Item("Marreg_user_info") Is Nothing Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            Else
                If Not token = Session("CSRF") Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            End If
            Dim app_auto_cd As String = If(Not (Request("app_auto_cd") Is Nothing Or Request("app_auto_cd") = ""), Convert.ToString(Request("app_auto_cd").Trim()), "")
            Dim groom_captured_f As String = If(Not (Request("groom_captured_f") Is Nothing Or Request("groom_captured_f") = ""), Convert.ToString(Request("groom_captured_f").Trim()), "")
            Dim bride_captured_f As String = If(Not (Request("bride_captured_f") Is Nothing Or Request("bride_captured_f") = ""), Convert.ToString(Request("bride_captured_f").Trim()), "")
            Dim g_veri_req As String = If(Not (Request("g_veri_req") Is Nothing Or Request("g_veri_req") = ""), Convert.ToString(Request("g_veri_req").Trim()), "")
            Dim b_veri_req As String = If(Not (Request("b_veri_req") Is Nothing Or Request("b_veri_req") = ""), Convert.ToString(Request("b_veri_req").Trim()), "")
            Dim groom_captured_d As String = If(Not (Request("groom_captured_d") Is Nothing Or Request("groom_captured_d") = ""), Convert.ToString(Request("groom_captured_d").Trim()), "")
            Dim bride_captured_d As String = If(Not (Request("bride_captured_d") Is Nothing Or Request("bride_captured_d") = ""), Convert.ToString(Request("bride_captured_d").Trim()), "")
            Dim cap_qty_d As String = If(Not (Request("cap_qty_d") Is Nothing Or Request("cap_qty_d") = ""), Convert.ToString(Request("cap_qty_d").Trim()), "")
            Dim veri_score_d As String = If(Not (Request("veri_score_d") Is Nothing Or Request("veri_score_d") = ""), Convert.ToString(Request("veri_score_d").Trim()), "")

            If (app_auto_cd = "") Then
                response("error") = "Invalid application no."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (groom_captured_f = "" Or Not IsNumber(groom_captured_f)) Then
                response("error") = "Groom final captured status invalid."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (bride_captured_f = "" Or Not IsNumber(bride_captured_f)) Then
                response("error") = "Bride final captured status invalid."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (g_veri_req = "" Or Not IsNumber(g_veri_req)) Then
                response("error") = "Groom verification data invalid."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (b_veri_req = "" Or Not IsNumber(b_veri_req)) Then
                response("error") = "Bride verification data invalid."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (groom_captured_d = "" Or Not IsNumber(groom_captured_d)) Then
                response("error") = "Groom captured status invalid."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (bride_captured_d = "" Or Not IsNumber(bride_captured_d)) Then
                response("error") = "Bride captured status invalid."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (cap_qty_d = "" Or Not IsNumber(cap_qty_d)) Then
                response("error") = "Capture quality is invalid."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (veri_score_d = "" Or Not IsNumber(veri_score_d)) Then
                response("error") = "Verification score is invalid."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (Convert.ToInt32(groom_captured_f) = 0 And Convert.ToInt32(g_veri_req) = 1) Then
                response("error") = "No need to verify groom biometric."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (Convert.ToInt32(bride_captured_f) = 0 And Convert.ToInt32(b_veri_req) = 1) Then
                response("error") = "No need to verify bride biometric."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (Convert.ToInt32(groom_captured_f) = 1 And Convert.ToInt32(bride_captured_f) = 1) Then
                If (Convert.ToInt32(g_veri_req) = 0 And Convert.ToInt32(b_veri_req) = 0) Then
                    response("error") = "Atleast one party needs to verify biometric."
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                    Exit Sub
                End If
            End If
            If (Convert.ToInt32(groom_captured_d) = 1 And Convert.ToInt32(bride_captured_d) = 1) Then
                response("error") = "No need to give permission."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If

            Dim obj As New Biometric_BL
            Dim retval = obj.SaveBiometricPermission(app_auto_cd, g_veri_req, b_veri_req, cap_qty_d, veri_score_d)
            If (retval = "1") Then          'updated successfully
                json("msg") = "Successfully saved"
            ElseIf (retval = "-1") Then     'No such record found
                json("msg") = "Duplicate permission found"
            Else                            'other issue
                json("msg") = retval
            End If
            json("success") = retval
        Catch ex As Exception
            response("exception") = "true"
            response("exception_message") = ex.Message
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
        Finally
            response = If(response.Count < 1, json, response)
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "application/json"
            HttpContext.Current.Response.Write(javaScriptSerializer.Serialize(response))
        End Try
    End Sub
#End Region
    'End Added on 09-01-2024
    'Added on 16-01-2024
#Region "Biometric Permission List"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub BiometricPermissionList()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
        Dim myDataTable As New MyNestedDataTable()

        Try
            Dim auth = 0
            Dim ajax, token As String
            Dim method As String
            ajax = HttpContext.Current.Request.Headers("X-Requested-With")
            token = HttpContext.Current.Request.Headers("X-CSRF-TOKEN")
            method = HttpContext.Current.Request.HttpMethod
            If Not (method = "POST" And ajax = "XMLHttpRequest") Then
                response("error") = "Method not allowed"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.MethodNotAllowed, Integer)
                Exit Sub
            End If
            If token = "" Then
                auth = 1
                If Session.Item("Marreg_user_info") Is Nothing Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            Else
                If Not token = Session("CSRF") Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            End If

            Dim draw As String = If(Not Request("draw") Is Nothing, DirectCast(Request("draw"), String), "1")
            Dim start As String = If(Not Request("start") Is Nothing, DirectCast(Request("start"), String), "0")
            Dim length As String = If(Not Request("length") Is Nothing, DirectCast(Request("length"), String), "10")
            Dim value As String = If(Not Request("search[value]") Is Nothing, DirectCast(Request("search[value]"), String).Trim(), "")
            Dim orderByColumn As String = If(Request("order[0][column]") Is Nothing, "", DirectCast(Request("order[0][column]"), String))
            Dim orderByDir As String = If(Request("order[0][dir]") Is Nothing, "", DirectCast(Request("order[0][dir]"), String))
            Dim order_by As String = "created_at"

            Dim is_search As Boolean = False
            Dim is_order As Boolean = False
            Select Case orderByColumn
                '    Case "0"
                '        order_by = "Groom_Name"
                '    Case "1"
                '        order_by = "Bride_Name"
                Case "2"
                    order_by = "created_at"
                    '    Case "3"
                    '        order_by = "Reg_PO"
                    '    Case "4"
                    '        order_by = "Reg_PS"
                    '    Case "5"
                    '        order_by = "Reg_Dist"
                Case Else
                    order_by = "created_at"
            End Select
            is_order = True

            myDataTable = Biometric_BL.BiometricPermissionList(draw, start, length, value, order_by, orderByDir)

            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
        Catch ex As Exception
            response("exception") = "true"
            response("exception_message") = ex.Message + "<br>" + "Click <a onclick='location.reload()' href='#'>here</a> to reload"
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
        Finally
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "application/json"
            If response.Count < 1 Then
                HttpContext.Current.Response.Write(javaScriptSerializer.Serialize(myDataTable))
            Else
                HttpContext.Current.Response.Write(javaScriptSerializer.Serialize(response))
            End If
        End Try
    End Sub
#End Region
#Region "Biometric Permission Delete"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub BiometricPermissionDelete()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
        Dim enc As New MARREG_SHA256()
        Try
            Dim auth = 0
            Dim ajax, token As String
            Dim method As String
            ajax = HttpContext.Current.Request.Headers("X-Requested-With")
            token = HttpContext.Current.Request.Headers("X-CSRF-TOKEN")
            method = HttpContext.Current.Request.HttpMethod
            If Not (method = "POST" And ajax = "XMLHttpRequest") Then
                response("error") = "Method not allowed"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.MethodNotAllowed, Integer)
                Exit Sub
            End If
            If token = "" Then
                auth = 1
                If Session.Item("Marreg_user_info") Is Nothing Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            Else
                If Not token = Session("CSRF") Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            End If
            Dim id As Int64 = If(Not (Request("id") Is Nothing Or Request("id") = ""), Convert.ToInt64(Request("id").Trim()), 0)
            If (id = 0) Then
                response("error") = "No record selected for delete."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If

            Dim obj = New Biometric_BL()
            Dim msg = obj.BiometricPermissionDelete(id)
            If (msg = "1") Then
                json("success") = "Record deleted successfully."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.OK, Integer)
            ElseIf (msg = "0") Then
                json("error") = "Error in delete record."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
            ElseIf (msg = "-1") Then
                json("error") = "Delete not possible for this record."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
            Else
                json("error") = "No record has been deleted."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
            End If
        Catch ex As Exception
            response("exception") = "true"
            response("exception_message") = ex.Message
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
        Finally
            response = If(response.Count < 1, json, response)
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "application/json"
            HttpContext.Current.Response.Write(javaScriptSerializer.Serialize(response))
        End Try
    End Sub
#End Region
    'End Added on 16-01-2024
    'Added on 31-01-2024
#Region "Update MO Bio Verification Score"
    <WebMethod(EnableSession:=True), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)>
    Public Sub UpdateMO_BioVerificationScore()
        Dim Request = HttpContext.Current.Request.Form
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
        Dim enc As New MARREG_SHA256()
        Try
            Dim auth = 0
            Dim ajax, token As String
            Dim method As String
            ajax = HttpContext.Current.Request.Headers("X-Requested-With")
            token = HttpContext.Current.Request.Headers("X-CSRF-TOKEN")
            method = HttpContext.Current.Request.HttpMethod
            If Not (method = "POST" And ajax = "XMLHttpRequest") Then
                response("error") = "Method not allowed"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.MethodNotAllowed, Integer)
                Exit Sub
            End If
            If token = "" Then
                auth = 1
                If Session.Item("Marreg_user_info") Is Nothing Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            Else
                If Not token = Session("CSRF") Then
                    response("error") = "Invalid request"
                    HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.Unauthorized, Integer)
                    Exit Sub
                End If
            End If
            Dim id As String = If(Not (Request("id") Is Nothing Or Request("id") = ""), Convert.ToString(Request("id").Trim()), "")
            Dim score As String = If(Not (Request("score") Is Nothing Or Request("score") = ""), Convert.ToString(Request("score").Trim()), "")

            If (id = "" Or Not IsNumber(id)) Then
                response("error") = "Selected record is invalid."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (score = "" Or Not IsNumber(score)) Then
                response("error") = "Verification score is invalid."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            If (score > 199 Or score < 10) Then
                response("error") = "Verification score is out of range."
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If
            Dim obj As New Biometric_BL
            Dim retval = obj.UpdateMO_BioVerificationScore(id, score)
            If (retval = "1") Then          'updated successfully
                json("msg") = "Verification score successfully updated"
            Else                            'other issue
                json("msg") = retval
            End If
            json("success") = retval
        Catch ex As Exception
            response("exception") = "true"
            response("exception_message") = ex.Message
            HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
        Finally
            response = If(response.Count < 1, json, response)
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "application/json"
            HttpContext.Current.Response.Write(javaScriptSerializer.Serialize(response))
        End Try
    End Sub
#End Region
    'End Added on 31-01-2024
End Class