Imports System.Data
Imports Microsoft.VisualBasic
Namespace MARREG.App_Code
    Public Class Report_BL
        Dim objDB As New Report_DB()
#Region "Schedule of Signature B"
        Public Function ScheduleOfSignatureB(AppAutoCd As String) As DataSet
            objDB.AppAutoCd = Convert.ToString(AppAutoCd)
            Return objDB.ScheduleOfSignatureB()
        End Function
#End Region
    End Class
End Namespace
