Imports System.Data
Imports Microsoft.VisualBasic

Public Class BSK_BL
    Dim objDB As New BSK_DB()
#Region "Save BSK Request"
    Public Function SaveBSK_Request(dpr As String, ticketNo As String, userId As String, citizenMobile As String, citizenEmail As String, citizenName As String, responseUrl As String, serviceCode As String) As String
        Dim ret As String
        objDB.dpr = dpr
        objDB.ticketNo = ticketNo
        objDB.userId = userId
        objDB.citizenMobile = citizenMobile
        objDB.citizenEmail = citizenEmail
        objDB.citizenName = citizenName
        objDB.responseUrl = responseUrl
        objDB.serviceCode = serviceCode

        ret = objDB.SaveBSK_Request()
        Return ret
    End Function
#End Region
#Region "Fetch BSK Request Data"
    Public Function FetchBSKRequestData(dpr As String) As Dictionary(Of String, Object)
        Dim ret = New Dictionary(Of String, Object)

        Dim objIDR As IDataReader = Nothing
        Try
            objDB.dpr = dpr

            objIDR = objDB.FetchBSKRequestData()
            While objIDR.Read
                ret("ticketNo") = Convert.ToString(objIDR("ticketNo"))
                ret("userId") = Convert.ToString(objIDR("userId"))
                ret("citizenName") = Convert.ToString(objIDR("citizenName"))        'Added on 08-02-2024
                ret("citizenEmail") = Convert.ToString(objIDR("citizenEmail"))      'Added on 08-02-2024
                ret("citizenMobile") = Convert.ToString(objIDR("citizenMobile"))
                ret("serviceCode") = Convert.ToString(objIDR("serviceCode"))
                ret("dpr") = Convert.ToString(objIDR("dpr"))
                ret("total_amt") = Convert.ToString(objIDR("total_amt"))
                ret("responseUrl") = Convert.ToString(objIDR("responseUrl"))
                ret("brn") = Convert.ToString(objIDR("brn"))
                ret("payment_status") = Convert.ToString(objIDR("payment_status"))
                ret("payment_status_desc") = Convert.ToString(objIDR("payment_status_desc"))
            End While
        Catch ex As Exception
            Throw ex
        Finally
            objIDR.Close()
            objIDR.Dispose()
            objIDR = Nothing
        End Try
        Return ret
    End Function
#End Region
End Class
