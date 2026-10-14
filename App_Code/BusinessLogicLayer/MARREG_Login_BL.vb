Imports Microsoft.VisualBasic
Imports MARREG.App_Code.DataAccessLayer

Namespace MARREG.App_Code.BusinessLogicLayer
    Public Class MARREG_Login_BL
        Dim marreglogindb As New MARREG_Logib_DB()
        Public Sub New()
        End Sub

#Region "Log In History"
        Public Function LogHistory(LoginId As String, UserId As String, login_status As String,
                                   ip_add As String, Mac_Address As String, browser As String, In_Time As DateTime) As String

            Return marreglogindb.LogHistory(LoginId, UserId, login_status,
                                   ip_add, Mac_Address, browser, In_Time)


        End Function

#End Region


#Region "Log Out History"
        Public Function LogOutHistory(LoginId As String, login_status As String,
                                   ip_add As String, pagename As String) As String

            Return marreglogindb.LogoutHistory(LoginId, login_status,
                                   ip_add, pagename)


        End Function

        Public Function ForceFullyLogOutHistory(username As String, pagename As String) As String
            Return marreglogindb.ForceFullyLogOutHistory(username, pagename)
        End Function

#End Region

    End Class
End Namespace
