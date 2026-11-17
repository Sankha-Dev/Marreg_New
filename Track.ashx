<%@ WebHandler Language="VB" Class="Track" %>

Imports System.IO
Imports MARREG.App_Code.BusinessLogicLayer
Imports Newtonsoft.Json
Public Class Track : Implements IHttpHandler

    Private mastrdata As New MasterData()

    Public Sub ProcessRequest(context As HttpContext) Implements IHttpHandler.ProcessRequest
        Try
            context.Response.ContentType = "text/plain"

            Dim body As String
            Using reader As New StreamReader(context.Request.InputStream)
                body = reader.ReadToEnd()
            End Using

            Dim data = JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(body)
            Dim dataType As String = data("type").ToString()
            Dim ip As String = context.Request.UserHostAddress
            Dim agent As String = context.Request.UserAgent
            Dim msg As String = ""

            If dataType = "feedback" Then
                Dim smiley As String = data("smiley").ToString()
                Dim smileyValue As Integer = 0

                Select Case smiley.ToLower()
                    Case "excellent"
                        smileyValue = 1
                    Case "good"
                        smileyValue = 2
                    Case "ok"
                        smileyValue = 3
                    Case Else
                        smileyValue = 0
                End Select

                msg = mastrdata.PortalFeedbackActivityLogsSave(ip, agent, smileyValue)

            ElseIf dataType = "duration" Then
                msg = "Duration logged"
            End If

            context.Response.Write(msg)

        Catch ex As Exception
            context.Response.Write("Error: " & ex.Message)
        End Try
    End Sub

    Public ReadOnly Property IsReusable As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class
