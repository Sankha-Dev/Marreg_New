Imports Microsoft.VisualBasic
Imports System.Web
Imports System.Web.Services
Imports MARREG.App_Code.DataAccessLayer
Imports System.Web.Script.Services
Imports MARREG.App_Code.WebService_BL
Imports MARREG.App_Code
Imports System.Data
Imports System.Web.Script.Serialization



' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class BSKwebService
    Inherits System.Web.Services.WebService

#Region "Declaration"

    Dim mrg As Marreg_Class
    Dim webservice As New WebService_BL()
#End Region


    <WebMethod(), ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=True)> _
    Public Sub MARREG_Service_BSK()
        Dim Request = HttpContext.Current.Request.QueryString
        Dim response As New Dictionary(Of String, Object)
        Dim json As New Dictionary(Of String, Object)
        Dim javaScriptSerializer As New JavaScriptSerializer()
        Try
            Dim method As String
            method = HttpContext.Current.Request.HttpMethod
            If Not (method = "GET") Then
                response("error") = "Method not allowed"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            End If

            Dim Mob_No As String = If(Not (Request("UserId") Is Nothing Or Request("UserId") = ""), DirectCast(Request("UserId"), String).Trim(), "")
            Dim From_date As String = If(Not (Request("from_date") Is Nothing Or Request("from_date") = ""), DirectCast(Request("from_date"), String).Trim(), "")
            Dim To_date As String = If(Not (Request("to_date") Is Nothing Or Request("to_date") = ""), DirectCast(Request("to_date"), String).Trim(), "")

            If From_date = "" Or To_date = "" Then
                response("error") = "Parameter missing"
                HttpContext.Current.Response.StatusCode = DirectCast(System.Net.HttpStatusCode.BadRequest, Integer)
                Exit Sub
            Else
                Dim res = webservice.MARREG_Service_BSK(Mob_No, From_date, To_date)
                response("data") = res
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

End Class
