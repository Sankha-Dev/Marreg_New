Imports Microsoft.VisualBasic
Imports System.Data
Imports MARREG.App_Code.DataAccessLayer

Namespace MARREG.App_Code
    Public Class GRIPS_BL
        Dim objDB As New GRIPS_DB()
        Public Function SaveGRIPSPaymentRequest(src As String, respURL As String, totalAmt As Decimal, depMob As Double, action As String, deptCode As String, svcCode As String, depName As String, depAddress As String, depEmail As String, onBehalfOf As String, depType As String, periodFrom As String, periodTo As String, remarks As String, refNo As String, ip As String, sysTimeStamp As String) As Dictionary(Of String, String)
            Dim ret As Dictionary(Of String, String)
            objDB.source = src
            objDB.respURL = respURL
            objDB.payment_amt = totalAmt
            objDB.depositor_mob = depMob
            objDB.action = action
            objDB.dept_code = deptCode
            objDB.service_code = svcCode
            objDB.depositor_name = depName
            objDB.depositor_address = depAddress
            objDB.depositor_email = depEmail
            objDB.onBehalfOf = onBehalfOf
            objDB.depositor_type = depType
            objDB.period_from = periodFrom
            objDB.period_to = periodTo
            objDB.payment_remarks = remarks
            objDB.refNo = refNo
            objDB.ip = ip
            objDB.sys_time_stamp = sysTimeStamp

            ret = objDB.SaveGRIPSPaymentRequest()
            Return ret
        End Function

        Public Function SaveGRIPSPaymentResponse(objD As Object) As DataTable
            Dim ret As DataTable
            objDB.dept_payment_ref_id = objD("dpr")
            objDB.source = objD("src")
            objDB.action = objD("action")
            objDB.grips_payment_ref_no = objD("gpr")
            objDB.payment_amt = Convert.ToDecimal(objD("paymentAmt"))
            objDB.payment_status = objD("paymentStatus")
            objDB.payment_status_desc = objD("paymentStatusDesc")
            objDB.grn = objD("grn")
            objDB.grn_time = objD("grnTime")
            objDB.grn_status = objD("grnStatus")
            objDB.grn_status_desc = objD("grnStatusDesc")
            objDB.bank = objD("bank")
            objDB.payment_mode = objD("paymentMode")
            objDB.brn = objD("brn")
            objDB.brn_time = objD("brnTime")
            objDB.status = objD("status")
            objDB.status_code = objD("statusCode")
            objDB.status_desc = objD("statusDesc")
            objDB.ip = objD("ip")
            objDB.sys_time_stamp = objD("sysTimeStamp")

            ret = objDB.SaveGRIPSPaymentResponse()
            Return ret
        End Function
        Public Shared Function GRIPSpaymentDetails(dpr As String, strdraw As String, start As String, length As String, searchvalue As String, order_by As String, orderByDir As String) As MyNestedDataTable
            Dim datatable As New MyNestedDataTable
            Dim objPI As New GRIPS_DB
            Dim recList As New List(Of Dictionary(Of String, Object))
            Dim objIDR As IDataReader = Nothing
            Dim totalCount As Integer = 0
            Try
                objPI.dept_payment_ref_id = dpr
                objPI.start = Convert.ToInt32(start)
                objPI.length = Convert.ToInt32(length)
                objPI.value = Convert.ToString(searchvalue)
                objPI.order_by = Convert.ToString(order_by)
                objPI.orderByDir = Convert.ToString(orderByDir)

                objIDR = objPI.GRIPSpaymentDetails()

                While objIDR.Read
                    Dim rec As New Dictionary(Of String, Object)

                    totalCount = Convert.ToInt32(objIDR("TotalCount"))
                    rec("id") = Convert.ToString(objIDR("id"))
                    rec("dept_payment_ref_id") = Convert.ToString(objIDR("dept_payment_ref_id"))
                    rec("payment_amt") = Convert.ToString(objIDR("payment_amt"))
                    rec("action") = Convert.ToString(objIDR("action"))
                    rec("dept_ref_no") = Convert.ToString(objIDR("dept_ref_no"))
                    rec("identification_no") = Convert.ToString(objIDR("identification_no"))
                    rec("depositor_name") = Convert.ToString(objIDR("depositor_name"))
                    rec("depositor_address") = Convert.ToString(objIDR("depositor_address"))
                    rec("depositor_mob") = Convert.ToString(objIDR("depositor_mob"))
                    rec("depositor_email") = Convert.ToString(objIDR("depositor_email"))
                    rec("onBehalfOf") = Convert.ToString(objIDR("onBehalfOf"))
                    rec("depositor_type") = Convert.ToString(objIDR("depositor_type"))
                    rec("period_from") = If(Not objIDR("period_from") Is DBNull.Value, Convert.ToDateTime(objIDR("period_from")).ToString("dd-MM-yyyy"), "")
                    rec("period_to") = If(Not objIDR("period_to") Is DBNull.Value, Convert.ToDateTime(objIDR("period_to")).ToString("dd-MM-yyyy"), "")
                    rec("payment_remarks") = Convert.ToString(objIDR("payment_remarks"))
                    rec("ip") = Convert.ToString(objIDR("ip"))
                    rec("sys_time_stamp") = If(Not objIDR("sys_time_stamp") Is DBNull.Value, Convert.ToDateTime(objIDR("sys_time_stamp")).ToString("dd-MM-yyyy hh:mm:ss tt"), "")
                    rec("created_at") = If(Not objIDR("created_at") Is DBNull.Value, Convert.ToDateTime(objIDR("created_at")).ToString("dd-MM-yyyy hh:mm:ss tt"), "")
                    rec("res_id") = Convert.ToString(objIDR("res_id"))
                    rec("res_action") = Convert.ToString(objIDR("res_action"))
                    rec("grips_payment_ref_no") = Convert.ToString(objIDR("grips_payment_ref_no"))
                    rec("res_payment_amt") = Convert.ToString(objIDR("res_payment_amt"))
                    rec("payment_status") = Convert.ToString(objIDR("payment_status"))
                    rec("payment_status_desc") = Convert.ToString(objIDR("payment_status_desc"))
                    rec("grn") = Convert.ToString(objIDR("grn"))
                    rec("grn_time") = If(Not objIDR("grn_time") Is DBNull.Value, Convert.ToDateTime(objIDR("grn_time")).ToString("dd-MM-yyyy hh:mm:ss tt"), "")
                    rec("grn_status") = Convert.ToString(objIDR("grn_status"))
                    rec("grn_status_desc") = Convert.ToString(objIDR("grn_status_desc"))
                    rec("bank") = Convert.ToString(objIDR("bank"))
                    rec("payment_mode") = Convert.ToString(objIDR("payment_mode"))
                    rec("brn") = Convert.ToString(objIDR("brn"))
                    rec("brn_time") = If(Not objIDR("brn_time") Is DBNull.Value, Convert.ToDateTime(objIDR("brn_time")).ToString("dd-MM-yyyy hh:mm:ss tt"), "NA")
                    rec("status") = Convert.ToString(objIDR("status"))
                    rec("status_desc") = Convert.ToString(objIDR("status_desc"))
                    rec("res_ip") = Convert.ToString(objIDR("res_ip"))
                    rec("res_sys_time_stamp") = If(Not objIDR("res_sys_time_stamp") Is DBNull.Value, Convert.ToDateTime(objIDR("res_sys_time_stamp")).ToString("dd-MM-yyyy hh:mm:ss tt"), "")
                    rec("res_created_at") = If(Not objIDR("res_created_at") Is DBNull.Value, Convert.ToDateTime(objIDR("res_created_at")).ToString("dd-MM-yyyy hh:mm:ss tt"), "")

                    recList.Add(rec)
                    rec = Nothing
                End While

                datatable.draw = Convert.ToInt32(strdraw)
                datatable.recordsTotal = totalCount
                datatable.recordsFiltered = If(recList.Count < totalCount, totalCount, recList.Count)
                datatable.data = recList
            Catch ex As Exception
                Throw ex
            Finally
                objIDR.Close()
                objIDR.Dispose()
                objIDR = Nothing
                objPI = Nothing
            End Try
            Return datatable
        End Function
#Region "GRIPS Payment Details Refresh"
        Public Function GRIPSpaymentStatusFetch(id As Int64) As DataTable
            objDB.id = id

            Return objDB.GRIPSpaymentStatusFetch()
        End Function
        Public Function GRIPSverifyRequestParam(dpr As String, paymentDate As String) As Int16
            objDB.dept_payment_ref_id = dpr
            objDB.payment_date = paymentDate

            Return objDB.GRIPSverifyRequestParam()
        End Function
        Public Function GRIPSverifyRequestParamStatus(svcCode As String, dpr As String, paymentDate As String) As DataTable
            objDB.service_code = svcCode
            objDB.dept_payment_ref_id = dpr
            objDB.payment_date = paymentDate

            Return objDB.GRIPSverifyRequestParamStatus()
        End Function
#End Region
#Region "MARREG Payment Reference"
        Public Function Save_MARREG_Payment_Reference(dept_payment_ref_id As String, reference_data As String) As String
            Dim ret As String
            objDB.dept_payment_ref_id = dept_payment_ref_id
            objDB.reference_data = reference_data

            ret = objDB.Save_MARREG_Payment_Reference()
            Return ret
        End Function
        Public Function Fetch_MARREG_Payment_Reference(dept_payment_ref_id As String) As DataTable
            Dim ret As DataTable
            objDB.dept_payment_ref_id = dept_payment_ref_id

            ret = objDB.Fetch_MARREG_Payment_Reference()
            Return ret
        End Function

        Public Function FetchPaymentResponse(dpr As String) As DataTable
            Dim objPI As New GRIPS_DB
            Dim objIDR As IDataReader = Nothing
            Dim dt As New DataTable()

            Try
                objPI.dept_payment_ref_id = dpr
                objIDR = objPI.GRIPSpaymentDetails()  ' Assuming this SP returns payment response rows

                dt.Load(objIDR)   ' Convert DataReader → DataTable
            Catch ex As Exception
                Throw ex
            Finally
                If objIDR IsNot Nothing Then
                    objIDR.Close()
                    objIDR.Dispose()
                End If
                objPI = Nothing
            End Try
            Return dt
        End Function

#End Region
    End Class
End Namespace
