Imports Microsoft.VisualBasic

Namespace MARREG.App_Code
    Public Class MyDataTable
#Region "DECLARATION"
        Public Property draw As Integer
        Public Property recordsTotal As Integer
        Public Property recordsFiltered As Integer
        Public Property data As List(Of Dictionary(Of String, String))
        Public Property additional As String
#End Region

#Region "Constructor"
        Public Sub New()
            draw = 1
            recordsTotal = New Integer
            recordsFiltered = New Integer
            data = New List(Of Dictionary(Of String, String))
        End Sub
#End Region

    End Class
    Public Class MyNestedDataTable
#Region "DECLARATION"
        Public Property draw As Integer
        Public Property recordsTotal As Integer
        Public Property recordsFiltered As Integer
        Public Property data As List(Of Dictionary(Of String, Object))
        Public Property additional As String
#End Region

#Region "Constructor"
        Public Sub New()
            draw = 1
            recordsTotal = New Integer
            recordsFiltered = New Integer
            data = New List(Of Dictionary(Of String, Object))
        End Sub
#End Region

    End Class
End Namespace

