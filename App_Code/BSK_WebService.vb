Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System
Imports System.Collections.Generic
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Script.Serialization
Imports MARREG.App_Code.DataAccessLayer
Imports Msgbox_ServerSide.Msgbox_name
Imports System.Runtime.Serialization.Json


' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
 Public Class BSK_WebService
    Inherits System.Web.Services.WebService


#Region "Declaration"
    Dim c As New Marreg_Class
    Dim newBox As New MessageBox()
    Dim jsonstr As String
#End Region

    <WebMethod()> _
    Public Function GetBSKData() As String

        BSK_load()
        Return jsonstr
    End Function
  

#Region " table value load"
    Protected Sub BSK_load()
        Try

            'Dim sql As String = "select top 5 Bsk_cd,App_cd  from Marreg_BSK_count "
            Dim sql As String = " select decrypt_UserId as bskuserid , TicktNo as bsktcktno , Draft_cd as appno , '' as deptpayrefno , '' as txn ,'' as bankrefno , convert(varchar,Entrydate,21) as  appdate , 'Submitted' as appstatus from Marreg_BSK_count "
            Dim dt As DataTable = c.selectdata(sql)
            If dt.Rows.Count > 0 Then
                'Dim BSK_id As String = dt.Rows(0)("Bsk_cd")
                'Dim appdate As String = dt.Rows(0)("App_cd")
                jsonstr = DataTableToJSONWithJavaScriptSerializer(dt)
            End If
            'jsonstr = DataTableToJSONWithJavaScriptSerializer(dt)
        Catch ex As Exception

        End Try
    End Sub
#End Region


    Public Function DataTableToJSONWithJavaScriptSerializer(ByVal table As DataTable) As String

        Dim jsSerializer As JavaScriptSerializer = New JavaScriptSerializer()
        Dim parentRow As List(Of Dictionary(Of String, Object)) = New List(Of Dictionary(Of String, Object))()
        Dim childRow As Dictionary(Of String, Object)

        For Each row As DataRow In table.Rows
            childRow = New Dictionary(Of String, Object)()

            For Each col As DataColumn In table.Columns
                childRow.Add(col.ColumnName, row(col))
            Next

            parentRow.Add(childRow)
        Next

        Return jsSerializer.Serialize(parentRow)
    End Function




    '<WebMethod>
    'Public Sub GetBSKDataJson()

    '    Dim sql As String = "select top 5 Bsk_cd,App_cd  from Marreg_BSK_count "
    '    Dim dt As DataTable = c.selectdata(sql)

    '    Dim js = New JavaScriptSerializer()
    '    Context.Response.Write(js.Serialize(dt))
    'End Sub



  

End Class