Imports Microsoft.VisualBasic
Imports System.Data

Namespace MARREG.App_Code
    Public Class PublicInfo_BL

        Public Shared Function NoticeListTable(fromDate As Date, toDate As Date, strdraw As String, start As String, length As String, searchvalue As String, order_by As String, orderByDir As String) As MyDataTable
            Dim datatable As New MyDataTable
            Dim objPI As New PublicInfo_DB
            Dim noticeList As New List(Of Dictionary(Of String, String))
            Dim objIDR As IDataReader = Nothing
            Dim totalCount As Integer = 0
            Try
                objPI.fromDate = fromDate
                objPI.toDate = toDate
                objPI.start = Convert.ToInt32(start)
                objPI.length = Convert.ToInt32(length)
                objPI.value = Convert.ToString(searchvalue)
                objPI.order_by = Convert.ToString(order_by)
                objPI.orderByDir = Convert.ToString(orderByDir)

                objIDR = objPI.getNoticeListing()

                While objIDR.Read
                    Dim notice As New Dictionary(Of String, String)

                    totalCount = Convert.ToInt32(objIDR("TotalCount"))
                    notice("Applicant_Cd") = Convert.ToString(objIDR("Applicant_Cd"))
                    notice("App_Auto_Cd") = Convert.ToString(objIDR("App_Auto_Cd"))
                    notice("Groom_name") = Convert.ToString(objIDR("Groom_name"))
                    notice("Groom_Father_Nam") = Convert.ToString(objIDR("Groom_Father_Nam"))
                    notice("Bride_name") = Convert.ToString(objIDR("Bride_name"))
                    notice("Bride_Father_Nam") = Convert.ToString(objIDR("Bride_Father_Nam"))
                    notice("Frm_date") = Convert.ToDateTime(objIDR("Frm_date")).ToString("dd-MM-yyyy")
                    notice("To_date") = Convert.ToDateTime(objIDR("To_date")).ToString("dd-MM-yyyy")
                    notice("mo_name") = Convert.ToString(objIDR("mo_name"))

                    noticeList.Add(notice)
                    notice = Nothing
                End While

                datatable.draw = Convert.ToInt32(strdraw)
                datatable.recordsTotal = totalCount
                datatable.recordsFiltered = If(noticeList.Count < totalCount, totalCount, noticeList.Count)
                datatable.data = noticeList
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

        Public Shared Function NewRecordsListTable(auth As Integer, groom_name As String, groom_Fname As String, groom_dob As Date, bride_dob As Date, groom_adhr As String, bride_name As String, bride_Fname As String, bride_adhr As String, app_no As String, mo_name As String, marriage_dt As Date, act_cd As String, from_dt As Date, to_dt As Date, strdraw As String, start As String, length As String, searchvalue As String, order_by As String, orderByDir As String) As MyDataTable
            Dim datatable As New MyDataTable
            Dim objPI As New PublicInfo_DB
            Dim recList As New List(Of Dictionary(Of String, String))
            Dim objIDR As IDataReader = Nothing
            Dim totalCount As Integer = 0
            Try
                objPI.auth = auth
                objPI.groom_name = groom_name
                objPI.groom_Fname = groom_Fname
                objPI.groom_adhr = groom_adhr
                objPI.bride_name = bride_name
                objPI.bride_Fname = bride_Fname
                objPI.groom_dob = groom_dob
                objPI.bride_dob = bride_dob
                objPI.bride_adhr = bride_adhr
                objPI.app_no = app_no
                objPI.mo_name = mo_name
                objPI.marriage_dt = marriage_dt
                objPI.act_cd = act_cd
                objPI.fromDate = from_dt
                objPI.toDate = to_dt
                objPI.start = Convert.ToInt32(start)
                objPI.length = Convert.ToInt32(length)
                objPI.value = Convert.ToString(searchvalue)
                objPI.order_by = Convert.ToString(order_by)
                objPI.orderByDir = Convert.ToString(orderByDir)

                objIDR = objPI.getNewRecordsList()

                While objIDR.Read
                    Dim rec As New Dictionary(Of String, String)

                    totalCount = Convert.ToInt32(objIDR("TotalCount"))
                    rec("cert_no") = Convert.ToString(objIDR("cert_no"))
                    rec("Applicant_Cd") = Convert.ToString(objIDR("Applicant_Cd"))
                    rec("App_Auto_Cd") = Convert.ToString(objIDR("App_Auto_Cd"))
                    rec("Groom_Name") = Convert.ToString(objIDR("Groom_name"))
                    rec("Groom_Father_Name") = Convert.ToString(objIDR("Groom_Father_Nam"))
                    rec("Groom_Mother_Name") = Convert.ToString(objIDR("Groom_Mother_Nam"))
                    rec("Groom_Aadhr_Num") = Convert.ToString(objIDR("Groom_Aadhr_Num"))
                    rec("Groom_DOB") = Convert.ToString(objIDR("Groom_DOB"))
                    rec("Bride_Name") = Convert.ToString(objIDR("Bride_name"))
                    rec("Bride_Father_Name") = Convert.ToString(objIDR("Bride_Father_Nam"))
                    rec("Bride_Mother_Name") = Convert.ToString(objIDR("Bride_Mother_Nam"))
                    rec("Bride_Aadhr_Num") = Convert.ToString(objIDR("Bride_Aadhr_Num"))
                    rec("Bride_DOB") = Convert.ToString(objIDR("Bride_DOB"))
                    rec("Registration_Date") = Convert.ToDateTime(objIDR("Registration_Date")).ToString("dd-MM-yyyy")
                    rec("MO_Cd") = Convert.ToString(objIDR("MO_Autoincrmnt_Cd"))
                    rec("MO_Name") = Convert.ToString(objIDR("MO_Name"))

                    rec("reg_add") = Convert.ToString(objIDR("Place_Reg_Address"))
                    rec("reg_po") = Convert.ToString(objIDR("epo_nm"))
                    rec("reg_ps") = Convert.ToString(objIDR("eps_nm"))
                    rec("reg_dist") = Convert.ToString(objIDR("EDistrict_Name"))
                    rec("reg_pin") = Convert.ToString(objIDR("Pin_No"))

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

        Public Function GetDashboardDataP1(year As String) As DataTable
            Dim objDb As New PublicInfo_DB()
            objDb.value = year
            Return objDb.GetDashboardDataP1()
        End Function
        Public Function GetDashboardDataP2(year As String) As DataTable
            Dim objDb As New PublicInfo_DB()
            objDb.value = year
            Return objDb.GetDashboardDataP2()
        End Function
        Public Function GetDashboardDataP3(year As String) As DataTable
            Dim objDb As New PublicInfo_DB()
            objDb.value = year
            Return objDb.GetDashboardDataP3()
        End Function

        Public Function CertificateVerification(app_cd As String) As Dictionary(Of String, Object)
            Dim objDb As New PublicInfo_DB()
            objDb.App_Auto_Cd = app_cd
            Dim table = objDb.CertificateVerification()
            Dim datas As New Dictionary(Of String, Object)
            If (table.Rows.Count > 0) Then
                datas("Certificate_No") = table.Rows(0)("Certificate_No")
                datas("reg_dt") = table.Rows(0)("reg_dt")
                datas("mo_name") = table.Rows(0)("mo_nam")
                datas("groom_name") = table.Rows(0)("groom_nam")
                datas("bride_name") = table.Rows(0)("bride_nam")
                datas("Mo_Type_Id") = table.Rows(0)("Mo_Type_Id")
            End If

            Return datas
        End Function
    End Class
End Namespace
