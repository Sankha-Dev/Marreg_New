Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports Microsoft.VisualBasic
Imports MARREG.App_Code.DataAccessLayer
Imports System.Xml
Imports System.Data
Namespace MARREG.App_Code.BusinessLogicLayer
    Public Class MARREG_Account_BL
        Dim MARREG_AccountDB As New MARREG_Account_DB
        Public Sub New()
        End Sub
#Region "Marreg Account view Gridview"
        Public Function Marreg_Account_View(From_dt As String, To_dt As String) As DataTable
            Return MARREG_AccountDB.Marreg_Account_View(From_dt, To_dt)
        End Function

#End Region
#Region "Marreg  MO Payment  Gridview"
        Public Function Marreg_MO_Payment_GridView() As DataTable
            Return MARREG_AccountDB.Marreg_MO_Payment_GridView()
        End Function

#End Region
#Region "MO Payment ViewDetails  Gridview"
        Public Function View_MOPayment_gridvw() As DataTable
            Return MARREG_AccountDB.View_MOPayment_gridvw()
        End Function

#End Region
#Region "MO Disburse Amount  Insert "
        Public Function Datatable_MO_Amount_Disburse(dtAmountDisburse As DataTable, User_Id As String, IP_Address As String) As String

            Return MARREG_AccountDB.Datatable_MO_Amount_Disburse(dtAmountDisburse, User_Id, IP_Address)

        End Function
#End Region
    End Class
End Namespace