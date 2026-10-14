Imports Microsoft.VisualBasic
Imports MARREG.App_Code.DataAccessLayer

Namespace MARREG.App_Code.BusinessLogicLayer
    Public Class AdminBL
        Dim admindb As New AdminDB()
        Public Sub New()
        End Sub

#Region "FUNCTION"
#Region "Role Access"
        Public Function Roleaccess(page_id As String, role_type_cd As String) As String

            Return admindb.Roleaccess(page_id, role_type_cd)

        End Function

#End Region



#End Region

    End Class
End Namespace
