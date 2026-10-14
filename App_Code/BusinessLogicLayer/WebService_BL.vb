Imports Microsoft.VisualBasic
Imports System.Data
Imports MARREG.App_Code.DataAccessLayer

Namespace MARREG.App_Code
    Public Class WebService_BL
        Dim objDB As New WebService_DB()
        Public Function SaveOtherInfoMarriage(location As String, gimg_byte As Byte(), Applicant_Cd As String) As String
            objDB.location = location
            objDB.Applicant_Cd = Applicant_Cd
            objDB.gimg_byte = gimg_byte

            Dim ret = objDB.SaveOtherInfoMarriage()
            Return ret
        End Function
        Public Function ValidateAppCd(Applicant_Cd As String) As String
            objDB.Applicant_Cd = Applicant_Cd

            Return objDB.ValidateAppCd()
        End Function

        Public Function Save_GRN_Details_Ag_Search(GRN_No As String, total_amount As Int64, Payment_Reason As String, reason As String) As String
            objDB.GRN_No = GRN_No
            objDB.total_amount = total_amount
            objDB.Payment_Reason = Payment_Reason
            objDB.reason = reason

            Return objDB.Save_GRN_Details_Ag_Search()
        End Function
        Public Function Update_GRN_Details_Ag_Print(id As Int64, appl_id As String, app_auto_cd As String, Payment_Reason As String) As String
            objDB.id = id
            objDB.appl_id = appl_id
            objDB.App_Auto_Cd = app_auto_cd
            objDB.Payment_Reason = Payment_Reason

            Return objDB.Update_GRN_Details_Ag_Print()
        End Function


#Region "Search Marriage Record"
        Public Function Save_SearchRecordData(groom_name As String, groom_Fname As String, bride_name As String, bride_Fname As String, actCd As String, mo_name As String, groom_aadhaar_no As String, bride_aadhaar_no As String, application_no As String, groom_dob As String, bride_dob As String, reg_date As String, reg_date_from As String, reg_date_to As String, grn_id As Int64, App_Auto_Cd As String) As Int64
            objDB.groom_name = groom_name
            objDB.groom_Fname = groom_Fname
            objDB.bride_name = bride_name
            objDB.bride_Fname = bride_Fname
            objDB.actCd = actCd
            objDB.mo_name = mo_name
            objDB.groom_aadhaar_no = groom_aadhaar_no
            objDB.bride_aadhaar_no = bride_aadhaar_no
            objDB.application_no = application_no
            objDB.groom_dobs = groom_dob
            objDB.bride_dobs = bride_dob
            objDB.reg_dates = reg_date
            objDB.reg_date_from = reg_date_from
            objDB.reg_date_to = reg_date_to
            objDB.grn_id = grn_id
            objDB.App_Auto_Cd = App_Auto_Cd


            Return objDB.Save_SearchRecordData()
        End Function
        Public Function Details_SearchRecordData(SearchRecId As Int64) As DataSet
            objDB.reqid = Convert.ToString(SearchRecId)
            Return objDB.Details_SearchRecordData()
        End Function
#End Region
#Region "Certified Copy Request"
        Public Function CheakEmailExist(email As String) As DataSet
            objDB.email = email

            Return objDB.CheakEmailExist()
        End Function
        Public Function Save_CertifiedCopyRequest(email As String, groom_name As String, bride_name As String, groom_dob As String, bride_dob As String, reg_date As String, certificate_no As String, grn_id As Int64, no_copy As Int32, mo_name As String) As String
            objDB.email = email
            objDB.groom_name = groom_name
            objDB.bride_name = bride_name
            objDB.groom_dobs = groom_dob
            objDB.bride_dobs = bride_dob
            objDB.reg_dates = reg_date
            objDB.certificate_no = certificate_no
            objDB.grn_id = grn_id
            objDB.no_copy = no_copy
            objDB.mo_name = mo_name

            Return objDB.Save_CertifiedCopyRequest()
        End Function
        Public Function Save_CertifiedCopyRequestRe(email As String, mo_name As String, reqId As String) As String
            objDB.email = email
            objDB.reqid = reqId
            objDB.mo_name = mo_name

            Return objDB.Save_CertifiedCopyRequestRe()
        End Function

        Public Shared Function CertifiedCopyRequestDetails(mo_auto_cd As String, req_to As String, status As String, strdraw As String, start As String, length As String, searchvalue As String, order_by As String, orderByDir As String) As MyNestedDataTable
            Dim datatable As New MyNestedDataTable
            Dim objPI As New WebService_DB
            Dim recList As New List(Of Dictionary(Of String, Object))
            Dim objIDR As IDataReader = Nothing
            Dim totalCount As Integer = 0
            Try
                objPI.Mo_Cd = mo_auto_cd
                objPI.req_to = req_to
                objPI.status = status
                objPI.start = Convert.ToInt32(start)
                objPI.length = Convert.ToInt32(length)
                objPI.value = Convert.ToString(searchvalue)
                objPI.order_by = Convert.ToString(order_by)
                objPI.orderByDir = Convert.ToString(orderByDir)

                objIDR = objPI.CertifiedCopyRequestDetails()

                While objIDR.Read
                    Dim rec As New Dictionary(Of String, Object)

                    totalCount = Convert.ToInt32(objIDR("TotalCount"))
                    rec("id") = Convert.ToString(objIDR("id"))
                    rec("App_Auto_Cd") = Convert.ToString(objIDR("App_Auto_Cd"))
                    rec("Act_cd") = Convert.ToString(objIDR("Act_cd"))
                    rec("req_act") = Convert.ToString(objIDR("req_act"))
                    rec("request_id") = Convert.ToString(objIDR("request_id"))
                    rec("groom_name") = Convert.ToString(objIDR("groom_name"))
                    rec("bride_name") = Convert.ToString(objIDR("bride_name"))
                    rec("groom_dob") = If(Not objIDR("groom_dob") Is DBNull.Value, Convert.ToDateTime(objIDR("groom_dob")).ToString("dd-MM-yyyy"), "")
                    rec("bride_dob") = If(Not objIDR("bride_dob") Is DBNull.Value, Convert.ToDateTime(objIDR("bride_dob")).ToString("dd-MM-yyyy"), "")
                    rec("reg_date") = If(Not objIDR("reg_date") Is DBNull.Value, Convert.ToDateTime(objIDR("reg_date")).ToString("dd-MM-yyyy"), "")
                    rec("certificate_no") = Convert.ToString(objIDR("certificate_no"))
                    rec("email") = Convert.ToString(objIDR("email"))
                    rec("GRN_No") = Convert.ToString(objIDR("GRN_No"))
                    rec("total_amount") = Convert.ToString(objIDR("total_amount"))
                    rec("CertifiedCopy_Reason") = Convert.ToString(objIDR("CertifiedCopy_Reason"))
                    rec("requested_on") = If(Not objIDR("requested_on") Is DBNull.Value, Convert.ToDateTime(objIDR("requested_on")).ToString("dd-MM-yyyy hh:mm:ss tt"), "")
                    rec("status") = Convert.ToString(objIDR("status"))
                    rec("status_ts") = If(Not objIDR("status_ts") Is DBNull.Value, Convert.ToDateTime(objIDR("status_ts")).ToString("dd-MM-yyyy hh:mm:ss tt"), "")
                    rec("status_by") = Convert.ToString(objIDR("status_by"))
                    rec("status_otp") = Convert.ToString(objIDR("status_otp"))
                    rec("mo_name") = Convert.ToString(objIDR("mo_name"))
                    rec("no_copy") = Convert.ToString(objIDR("no_copy"))
                    rec("rectified_on") = If(Not objIDR("rectified_on") Is DBNull.Value, Convert.ToDateTime(objIDR("rectified_on")).ToString("dd-MM-yyyy hh:mm:ss tt"), "")

                    rec("req_outsider_id") = Convert.ToString(objIDR("req_outsider_id"))
                    rec("is_hearing") = Convert.ToString(objIDR("is_hearing"))
                    rec("o_name") = Convert.ToString(objIDR("o_name"))
                    rec("o_mob") = Convert.ToString(objIDR("o_mob"))
                    rec("o_email") = Convert.ToString(objIDR("o_email"))

                    rec("request_to_mocd") = Convert.ToString(objIDR("request_to_mocd"))
                    If (DateDiff(DateInterval.Day, Convert.ToDateTime(rec("requested_on")), DateTime.Now) <= 10 And rec("request_to_mocd") <> "1") Then
                        rec("permission") = "0"
                    Else
                        rec("permission") = "1"
                    End If
                    Dim objSubPI As New WebService_DB
                    Dim recSubList As New List(Of Dictionary(Of String, String))
                    Dim objSubIDR As IDataReader = Nothing
                    objSubPI.lngReqId = Convert.ToInt64(objIDR("id"))
                    objSubIDR = objSubPI.CertifiedCopyRequestCommentDetails()
                    While objSubIDR.Read
                        Dim sub_rec As New Dictionary(Of String, String)
                        sub_rec("c_id") = Convert.ToString(objSubIDR("c_id"))
                        sub_rec("c_full_name") = Convert.ToString(objSubIDR("c_full_name"))
                        sub_rec("c_dob") = If(Not objSubIDR("c_dob") Is DBNull.Value, Convert.ToDateTime(objSubIDR("c_dob")).ToString("dd-MM-yyyy"), "")
                        sub_rec("c_mob_no") = Convert.ToString(objSubIDR("c_mob_no"))
                        sub_rec("c_email") = Convert.ToString(objSubIDR("c_email"))
                        sub_rec("comment") = Convert.ToString(objSubIDR("comment"))
                        sub_rec("comment_date") = If(Not objSubIDR("comment_date") Is DBNull.Value, Convert.ToDateTime(objSubIDR("comment_date")).ToString("dd-MM-yyyy hh:mm:ss tt"), "")
                        sub_rec("c_ref_id") = Convert.ToString(objSubIDR("c_ref_id"))
                        recSubList.Add(sub_rec)
                    End While
                    rec("comment_details") = recSubList

                    objSubIDR = Nothing
                    objSubPI = Nothing

                    Dim objSubPI1 As New WebService_DB
                    Dim recSubList1 As New List(Of Dictionary(Of String, String))
                    objSubPI1.lngReqId = Convert.ToInt64(objIDR("id"))
                    objSubIDR = objSubPI1.CertifiedCopyRequestHearingDetails()
                    While objSubIDR.Read
                        Dim sub_rec As New Dictionary(Of String, String)
                        sub_rec("id") = Convert.ToString(objSubIDR("id"))
                        sub_rec("hearing_date") = If(Not objSubIDR("hearing_date") Is DBNull.Value, Convert.ToDateTime(objSubIDR("hearing_date")).ToString("dd-MM-yyyy"), "")
                        sub_rec("status") = Convert.ToString(objSubIDR("status"))
                        sub_rec("status_ts") = If(Not objSubIDR("status_ts") Is DBNull.Value, Convert.ToDateTime(objSubIDR("status_ts")).ToString("dd-MM-yyyy hh:mm:ss tt"), "")
                        sub_rec("created_at") = If(Not objSubIDR("created_at") Is DBNull.Value, Convert.ToDateTime(objSubIDR("created_at")).ToString("dd-MM-yyyy hh:mm:ss tt"), "")
                        sub_rec("Status_Name") = Convert.ToString(objSubIDR("Status_Name"))
                        recSubList1.Add(sub_rec)
                    End While
                    rec("hearing_details") = recSubList1

                    objSubIDR = Nothing
                    objSubPI1 = Nothing

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
        Public Function Search_CertifiedCopyRequest(id As Int64, mode As Int16, email As String, groom_name As String, bride_name As String, groom_dob As String, bride_dob As String, reg_date As String, certificate_no As String, mo_name As String, req_outsider_id As Int64, strdraw As String, start As String, length As String, searchvalue As String, order_by As String, orderByDir As String) As MyDataTable
            Dim objDS As DataSet = Nothing
            Dim totalCount As Integer = 0
            Dim recList As New List(Of Dictionary(Of String, String))
            Dim datatable As New MyDataTable
            Try
                objDB.id = id
                objDB.mode = mode
                objDB.email = email
                objDB.groom_name = groom_name
                objDB.bride_name = bride_name
                objDB.groom_dobs = groom_dob
                objDB.bride_dobs = bride_dob
                objDB.reg_dates = reg_date
                objDB.certificate_no = certificate_no
                objDB.mo_name = mo_name
                objDB.req_outsider_id = req_outsider_id

                objDB.start = Convert.ToInt32(start)
                objDB.length = Convert.ToInt32(length)
                objDB.value = Convert.ToString(searchvalue)
                objDB.order_by = Convert.ToString(order_by)
                objDB.orderByDir = Convert.ToString(orderByDir)

                objDS = objDB.Search_CertifiedCopyRequest()
                Dim n As Integer = objDS.Tables(0).Rows.Count
                totalCount = n
                While (n > 0)
                    Dim rec As New Dictionary(Of String, String)
                    totalCount = Convert.ToString(objDS.Tables(0).Rows(n - 1)("TotalCount"))
                    rec("App_Auto_Cd") = Convert.ToString(objDS.Tables(0).Rows(n - 1)("App_Auto_Cd"))
                    rec("Groom_Name") = Convert.ToString(objDS.Tables(0).Rows(n - 1)("Groom_Name"))
                    rec("Groom_Father_Name") = Convert.ToString(objDS.Tables(0).Rows(n - 1)("Groom_Father_Nam"))
                    rec("Groom_Mother_Name") = Convert.ToString(objDS.Tables(0).Rows(n - 1)("Groom_Mother_Nam"))
                    rec("Bride_Name") = Convert.ToString(objDS.Tables(0).Rows(n - 1)("Bride_Name"))
                    rec("Bride_Father_Name") = Convert.ToString(objDS.Tables(0).Rows(n - 1)("Bride_Father_Nam"))
                    rec("Bride_Mother_Name") = Convert.ToString(objDS.Tables(0).Rows(n - 1)("Bride_Mother_Nam"))
                    rec("Groom_DOB") = If(Not objDS.Tables(0).Rows(n - 1)("Groom_DOB") Is DBNull.Value, Convert.ToDateTime(objDS.Tables(0).Rows(n - 1)("Groom_DOB")).ToString("dd-MM-yyyy"), "")
                    rec("Bride_DOB") = If(Not objDS.Tables(0).Rows(n - 1)("Bride_DOB") Is DBNull.Value, Convert.ToDateTime(objDS.Tables(0).Rows(n - 1)("Bride_DOB")).ToString("dd-MM-yyyy"), "")
                    rec("Groom_Email_Id") = Convert.ToString(objDS.Tables(0).Rows(n - 1)("Groom_Email_Id"))
                    rec("Bride_Email_Id") = Convert.ToString(objDS.Tables(0).Rows(n - 1)("Bride_Email_Id"))
                    rec("Registration_Date") = If(Not objDS.Tables(0).Rows(n - 1)("Registration_Date") Is DBNull.Value, Convert.ToDateTime(objDS.Tables(0).Rows(n - 1)("Registration_Date")).ToString("dd-MM-yyyy"), "")
                    rec("certificate_no") = Convert.ToString(objDS.Tables(0).Rows(n - 1)("certificate_no"))
                    rec("Act_Cd") = Convert.ToString(objDS.Tables(0).Rows(n - 1)("Act_Cd"))
                    rec("MO_Name") = Convert.ToString(objDS.Tables(0).Rows(n - 1)("MO_Name"))
                    rec("act_name") = Convert.ToString(objDS.Tables(0).Rows(n - 1)("act_name"))

                    recList.Add(rec)
                    rec = Nothing

                    n -= 1
                End While
                datatable.draw = Convert.ToInt32(strdraw)
                datatable.recordsTotal = totalCount
                datatable.recordsFiltered = If(recList.Count < totalCount, totalCount, recList.Count)
                datatable.data = recList
            Catch ex As Exception
                Throw ex
            Finally
                objDS.Dispose()
                objDS = Nothing

            End Try
            Return datatable
        End Function
        Public Function Save_CertifiedCopyRequestApprove(id As Int64, app_auto_cd As String, status As String, otp As String, user_nm As String) As String
            objDB.id = id
            objDB.App_Auto_Cd = app_auto_cd
            objDB.status = status
            objDB.otp = otp
            objDB.user_name = user_nm

            Return objDB.Save_CertifiedCopyRequestApprove()
        End Function
        Public Function FetchRequestDetail(id As Int64, status As String) As DataSet
            objDB.id = id
            objDB.status = status

            Return objDB.FetchRequestDetail()
        End Function
        Public Function ValidateRequestStatus(email As String, reqid As String) As DataSet
            objDB.email = email
            objDB.reqid = reqid

            Return objDB.ValidateRequestStatus()
        End Function
        Public Function CertifiedCopyRequestInfoValidateInsider(email As String, groom_name As String, bride_name As String, groom_dob As String, bride_dob As String, reg_date As String, certificate_no As String, mo_name As String) As String
            objDB.email = email
            objDB.groom_name = groom_name
            objDB.bride_name = bride_name
            objDB.groom_dobs = groom_dob
            objDB.bride_dobs = bride_dob
            objDB.reg_dates = reg_date
            objDB.certificate_no = certificate_no
            objDB.mo_name = mo_name

            Return objDB.CertifiedCopyRequestInfoValidateInsider()
        End Function
        Public Function CertifiedCopyRequestInfoValidate(groom_name As String, bride_name As String, groom_dob As String, bride_dob As String, reg_date As String, mo_name As String, ActCd As String) As String
            objDB.groom_name = groom_name
            objDB.bride_name = bride_name
            objDB.groom_dobs = groom_dob
            objDB.bride_dobs = bride_dob
            objDB.reg_dates = reg_date
            objDB.mo_name = mo_name
            objDB.actCd = ActCd

            Return objDB.CertifiedCopyRequestInfoValidate()
        End Function
        Public Function CertifiedCopyRequestSaveEmailOutsider(name As String, mob_no As String, email As String, validatedId As String) As String
            objDB.name = name
            objDB.mob_no = mob_no
            objDB.email = email
            objDB.id = validatedId

            Return objDB.CertifiedCopyRequestSaveEmailOutsider()
        End Function
        Public Function CertifiedCopyRequestEmailOTPValidate(validatedId As String, otp As String) As Int16
            objDB.otp = otp
            objDB.id = validatedId

            Return objDB.CertifiedCopyRequestEmailOTPValidate()
        End Function
        Public Function CertifiedCopyRequestSendOutsider(validatedId As Int64, grn_id As Int64, no_copy As Int32) As DataTable
            objDB.grn_id = grn_id
            objDB.id = validatedId
            objDB.no_copy = no_copy

            Return objDB.CertifiedCopyRequestSendOutsider()
        End Function
#Region "Certified Copy Request Objection"
        Public Function CertifiedCopy_Request_Id_Validate(RequestId As String) As Int64
            objDB.reqid = RequestId
            Return objDB.CertifiedCopy_Request_Id_Validate()
        End Function
        Public Function Validate_CertifiedCopy_Objector(name As String, dob As String, email As String, lngReqId As Int64) As Int64
            objDB.name = name
            objDB.dobs = dob
            objDB.email = email
            objDB.lngReqId = lngReqId
            Return objDB.Validate_CertifiedCopy_Objector()
        End Function
        Public Function CertifiedCopyCommentSubmit(name As String, dob As String, mob_no As String, email As String, comment As String, rId As Int64) As String
            objDB.lngReqId = rId
            objDB.name = name
            objDB.dobs = dob
            objDB.mob_no = mob_no
            objDB.email = email
            objDB.comment = comment
            Return objDB.CertifiedCopyCommentSubmit()
        End Function
#End Region
#Region "Certified Copy Request Hearing"
        Public Function CertifiedCopyRequestHearing(id As Int64) As String
            objDB.id = id
            Return objDB.Save_CertifiedCopyRequestHearing()
        End Function
        Public Function FetchDetailForCertifiedCopyHearing(id As Int64) As DataSet
            objDB.id = id
            Return objDB.FetchDetailForCertifiedCopyHearing()
        End Function
        Public Function CertifiedCopyRequestSaveHearing(id As Int64, hid As Int64, fstat As String, h_date As String) As String
            objDB.lngReqId = id
            objDB.id = hid
            objDB.status = fstat
            objDB.hearing_dateS = h_date
            Return objDB.CertifiedCopyRequestSaveHearing()
        End Function
#End Region
#Region "Certified Copy Request Old Records"
        Public Function CertifiedCopyRequestInfoValidateOld(groom_name As String, bride_name As String, groom_father As String, bride_father As String, reg_date As String, mo_name As String, ActCd As String) As String
            objDB.groom_name = groom_name
            objDB.bride_name = bride_name
            objDB.groom_Fname = groom_father
            objDB.bride_Fname = bride_father
            objDB.reg_dates = reg_date
            objDB.mo_name = mo_name
            objDB.actCd = ActCd

            Return objDB.CertifiedCopyRequestInfoValidateOld()
        End Function
        Public Function CertifiedCopyRequestSaveEmailOld(name As String, address As String, mob_no As String, email As String, validatedId As String) As String
            objDB.name = name
            objDB.address = address
            objDB.mob_no = mob_no
            objDB.mob_no = mob_no
            objDB.email = email
            objDB.id = validatedId

            Return objDB.CertifiedCopyRequestSaveEmailOld()
        End Function
        Public Function CertifiedCopyRequestEmailOTPValidateOld(validatedId As String, otp As String) As Int16
            objDB.otp = otp
            objDB.id = validatedId

            Return objDB.CertifiedCopyRequestEmailOTPValidateOld()
        End Function
        Public Function CertifiedCopyRequestSendOld(validatedId As Int64, grn_id As Int64, no_copy As Int32) As DataTable
            objDB.grn_id = grn_id
            objDB.id = validatedId
            objDB.no_copy = no_copy

            Return objDB.CertifiedCopyRequestSendOld()
        End Function
        Public Shared Function CertifiedCopyRequestDetailsOld(mo_auto_cd As String, req_to As String, status As String, strdraw As String, start As String, length As String, searchvalue As String, order_by As String, orderByDir As String) As MyNestedDataTable
            Dim datatable As New MyNestedDataTable
            Dim objPI As New WebService_DB
            Dim recList As New List(Of Dictionary(Of String, Object))
            Dim objIDR As IDataReader = Nothing
            Dim totalCount As Integer = 0
            Try
                objPI.Mo_Cd = mo_auto_cd
                objPI.req_to = req_to
                objPI.status = status
                objPI.start = Convert.ToInt32(start)
                objPI.length = Convert.ToInt32(length)
                objPI.value = Convert.ToString(searchvalue)
                objPI.order_by = Convert.ToString(order_by)
                objPI.orderByDir = Convert.ToString(orderByDir)

                objIDR = objPI.CertifiedCopyRequestDetailsOld()

                While objIDR.Read
                    Dim rec As New Dictionary(Of String, Object)

                    totalCount = Convert.ToInt32(objIDR("TotalCount"))
                    rec("id") = Convert.ToString(objIDR("id"))
                    'rec("App_Auto_Cd") = Convert.ToString(objIDR("App_Auto_Cd"))
                    rec("Act_cd") = Convert.ToString(objIDR("Act_cd"))
                    rec("req_act") = Convert.ToString(objIDR("req_act"))
                    rec("request_id") = Convert.ToString(objIDR("request_id"))
                    rec("groom_name") = Convert.ToString(objIDR("groom_name"))
                    rec("bride_name") = Convert.ToString(objIDR("bride_name"))
                    rec("groom_father") = Convert.ToString(objIDR("groom_father"))
                    rec("bride_father") = Convert.ToString(objIDR("bride_father"))
                    rec("reg_date") = If(Not objIDR("reg_date") Is DBNull.Value, Convert.ToDateTime(objIDR("reg_date")).ToString("dd-MM-yyyy"), "")
                    rec("email") = Convert.ToString(objIDR("email"))
                    rec("GRN_No") = Convert.ToString(objIDR("GRN_No"))
                    rec("total_amount") = Convert.ToString(objIDR("total_amount"))
                    rec("CertifiedCopy_Reason") = Convert.ToString(objIDR("CertifiedCopy_Reason"))
                    rec("requested_on") = If(Not objIDR("requested_on") Is DBNull.Value, Convert.ToDateTime(objIDR("requested_on")).ToString("dd-MM-yyyy hh:mm:ss tt"), "")
                    rec("status") = Convert.ToString(objIDR("status"))
                    rec("status_ts") = If(Not objIDR("status_ts") Is DBNull.Value, Convert.ToDateTime(objIDR("status_ts")).ToString("dd-MM-yyyy hh:mm:ss tt"), "")
                    rec("status_by") = Convert.ToString(objIDR("status_by"))
                    rec("status_otp") = Convert.ToString(objIDR("status_otp"))
                    rec("mo_name") = Convert.ToString(objIDR("mo_name"))
                    rec("no_copy") = Convert.ToString(objIDR("no_copy"))

                    rec("o_name") = Convert.ToString(objIDR("o_name"))
                    rec("o_mob") = Convert.ToString(objIDR("o_mob"))
                    rec("o_address") = Convert.ToString(objIDR("o_address"))

                    rec("request_to_mocd") = Convert.ToString(objIDR("request_to_mocd"))
                    If (DateDiff(DateInterval.Day, Convert.ToDateTime(rec("requested_on")), DateTime.Now) <= 10 And rec("request_to_mocd") <> "1") Then
                        rec("permission") = "0"
                    Else
                        rec("permission") = "1"
                    End If
                    'Dim objSubPI As New WebService_DB
                    'Dim recSubList As New List(Of Dictionary(Of String, String))
                    'Dim objSubIDR As IDataReader = Nothing
                    'objSubPI.lngReqId = Convert.ToInt64(objIDR("id"))
                    'objSubIDR = objSubPI.CertifiedCopyRequestCommentDetails()
                    'While objSubIDR.Read
                    '    Dim sub_rec As New Dictionary(Of String, String)
                    '    sub_rec("c_id") = Convert.ToString(objSubIDR("c_id"))
                    '    sub_rec("c_full_name") = Convert.ToString(objSubIDR("c_full_name"))
                    '    sub_rec("c_dob") = If(Not objSubIDR("c_dob") Is DBNull.Value, Convert.ToDateTime(objSubIDR("c_dob")).ToString("dd-MM-yyyy"), "")
                    '    sub_rec("c_mob_no") = Convert.ToString(objSubIDR("c_mob_no"))
                    '    sub_rec("c_email") = Convert.ToString(objSubIDR("c_email"))
                    '    sub_rec("comment") = Convert.ToString(objSubIDR("comment"))
                    '    sub_rec("comment_date") = If(Not objSubIDR("comment_date") Is DBNull.Value, Convert.ToDateTime(objSubIDR("comment_date")).ToString("dd-MM-yyyy hh:mm:ss tt"), "")
                    '    sub_rec("c_ref_id") = Convert.ToString(objSubIDR("c_ref_id"))
                    '    recSubList.Add(sub_rec)
                    'End While
                    'rec("comment_details") = recSubList

                    'objSubIDR = Nothing
                    'objSubPI = Nothing

                    'Dim objSubPI1 As New WebService_DB
                    'Dim recSubList1 As New List(Of Dictionary(Of String, String))
                    'objSubPI1.lngReqId = Convert.ToInt64(objIDR("id"))
                    'objSubIDR = objSubPI1.CertifiedCopyRequestHearingDetails()
                    'While objSubIDR.Read
                    '    Dim sub_rec As New Dictionary(Of String, String)
                    '    sub_rec("id") = Convert.ToString(objSubIDR("id"))
                    '    sub_rec("hearing_date") = If(Not objSubIDR("hearing_date") Is DBNull.Value, Convert.ToDateTime(objSubIDR("hearing_date")).ToString("dd-MM-yyyy"), "")
                    '    sub_rec("status") = Convert.ToString(objSubIDR("status"))
                    '    sub_rec("status_ts") = If(Not objSubIDR("status_ts") Is DBNull.Value, Convert.ToDateTime(objSubIDR("status_ts")).ToString("dd-MM-yyyy hh:mm:ss tt"), "")
                    '    sub_rec("created_at") = If(Not objSubIDR("created_at") Is DBNull.Value, Convert.ToDateTime(objSubIDR("created_at")).ToString("dd-MM-yyyy hh:mm:ss tt"), "")
                    '    sub_rec("Status_Name") = Convert.ToString(objSubIDR("Status_Name"))
                    '    recSubList1.Add(sub_rec)
                    'End While
                    'rec("hearing_details") = recSubList1

                    'objSubIDR = Nothing
                    'objSubPI1 = Nothing

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
        Public Function Save_CertifiedCopyRequestApproveOld(id As Int64, status As String, otp As String, user_nm As String) As String
            objDB.id = id
            objDB.status = status
            objDB.otp = otp
            objDB.user_name = user_nm

            Return objDB.Save_CertifiedCopyRequestApproveOld()
        End Function
        Public Function FetchRequestDetailOld(id As Int64, status As String) As DataSet
            objDB.id = id
            objDB.status = status

            Return objDB.FetchRequestDetailOld()
        End Function
        Public Function Rollback_CertifiedCopyRequestApproveOld(id As Int64) As String
            objDB.id = id
            Return objDB.Rollback_CertifiedCopyRequestApproveOld()
        End Function
        Public Function Save_CertificateOld(file_byte As Byte(), id As Int64) As String
            objDB.id = id
            objDB.gimg_byte = file_byte
            Return objDB.Save_CertificateOld()
        End Function
#End Region
#End Region
#Region "Store Certified Copy of Marriage Certificate"
        Public Function Save_Certificate(file_byte As Byte(), id As Int64) As String
            objDB.id = id
            objDB.gimg_byte = file_byte
            Return objDB.Save_Certificate()
        End Function
#End Region
#Region "Rollback Certified Copy Request Approve"
        Public Function Rollback_CertifiedCopyRequestApprove(id As Int64) As String
            objDB.id = id
            Return objDB.Rollback_CertifiedCopyRequestApprove()
        End Function
#End Region
#Region "Web Service For BSK"
        Public Function MARREG_Service_BSK(Mobile_no As String, From_dt As String, To_date As String) As List(Of Dictionary(Of String, String))
            Dim objDS As DataSet = Nothing
            Dim totalCount As Integer = 0
            Dim recList As New List(Of Dictionary(Of String, String))
            Dim datatable As New MyDataTable
            Try
                objDB.mob_no = Mobile_no
                objDB.reg_date_from = From_dt
                objDB.reg_date_to = To_date

                objDS = objDB.MARREG_Service_BSK()
                Dim n As Integer = objDS.Tables(0).Rows.Count
                totalCount = n
                While (n > 0)
                    Dim rec As New Dictionary(Of String, String)
                    rec("AppNo") = Convert.ToString(objDS.Tables(0).Rows(n - 1)("App_Cd")) + "/" + Convert.ToString(objDS.Tables(0).Rows(n - 1)("Bsk_cd"))
                    'rec("Applicant_Cd") = Convert.ToString(objDS.Tables(0).Rows(n - 1)("Applicant_Cd"))
                    rec("TicktNo") = Convert.ToString(objDS.Tables(0).Rows(n - 1)("TicktNo"))
                    rec("Appl_Status") = Convert.ToString(objDS.Tables(0).Rows(n - 1)("App_status"))
                    rec("UpdatedOn") = If(Not objDS.Tables(0).Rows(n - 1)("UpdatedOn") Is DBNull.Value, Convert.ToDateTime(objDS.Tables(0).Rows(n - 1)("UpdatedOn")).ToString("dd-MM-yyyy hh:mm:ss tt"), "")
                    rec("deptpayrefno") = Convert.ToString(objDS.Tables(0).Rows(n - 1)("deptpayrefno"))
                    rec("paidamt") = Convert.ToString(objDS.Tables(0).Rows(n - 1)("paidamt"))

                    recList.Add(rec)
                    rec = Nothing

                    n -= 1
                End While
            Catch ex As Exception
                Throw ex
            Finally
                objDS.Dispose()
                objDS = Nothing
            End Try
            Return recList
        End Function
#End Region

#Region "MasterData Webservice"
#Region "Update Jurisdiction"
        Public Shared Function FetchJurisdictionList(mo_cd As String, strdraw As String, start As String, length As String, searchvalue As String, order_by As String, orderByDir As String) As MyNestedDataTable
            Dim datatable As New MyNestedDataTable
            Dim objPI As New WebService_DB
            Dim recList As New List(Of Dictionary(Of String, Object))
            Dim objIDR As IDataReader = Nothing
            Dim totalCount As Integer = 0
            Try
                objPI.Mo_Cd = Convert.ToString(mo_cd)
                objPI.start = Convert.ToInt32(start)
                objPI.length = Convert.ToInt32(length)
                objPI.value = Convert.ToString(searchvalue)
                objPI.order_by = Convert.ToString(order_by)
                objPI.orderByDir = Convert.ToString(orderByDir)

                objIDR = objPI.FetchJurisdictionList()

                While objIDR.Read
                    Dim rec As New Dictionary(Of String, Object)

                    totalCount = Convert.ToInt32(objIDR("TotalCount"))
                    rec("Jurisdiction_Details_cd") = Convert.ToString(objIDR("Jurisdiction_Details_cd"))
                    rec("Jurisdctn_location_cd") = Convert.ToString(objIDR("Jurisdctn_location_cd"))
                    rec("eps_nm") = Convert.ToString(objIDR("eps_nm"))
                    rec("hm") = Convert.ToString(objIDR("hm"))
                    rec("sm") = Convert.ToString(objIDR("sm"))
                    rec("MO_Autoincrmnt_Cd") = Convert.ToString(objIDR("MO_Autoincrmnt_Cd"))

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
        Public Function UpdateJurisdiction(id As String, juri_location As String, hm As String, sm As String) As String
            objDB.mdatas = id
            objDB.juri_location = juri_location
            objDB.sm = sm
            objDB.hm = hm

            Return objDB.UpdateJurisdiction()
        End Function
#End Region
#End Region
#Region "Search Old Data"
        Public Function SearchRequestInfoValidateOld(groom_name As String, bride_name As String, groom_dobs As String, bride_dobs As String, groom_father As String, bride_father As String, mo_name As String, ActCd As String, reg_date_from As String, reg_date_to As String) As String
            objDB.groom_name = groom_name
            objDB.bride_name = bride_name
            objDB.groom_dobs = groom_dobs
            objDB.bride_dobs = bride_dobs
            objDB.groom_Fname = groom_father
            objDB.bride_Fname = bride_father
            objDB.mo_name = mo_name
            objDB.actCd = ActCd
            objDB.reg_date_from = reg_date_from
            objDB.reg_date_to = reg_date_to

            Return objDB.SearchRequestInfoValidateOld()
        End Function
        Public Function SearchRequestSaveEmailOld(name As String, address As String, mob_no As String, email As String, validatedId As String) As String
            objDB.name = name
            objDB.address = address
            objDB.mob_no = mob_no
            objDB.mob_no = mob_no
            objDB.email = email
            objDB.id = validatedId

            Return objDB.SearchRequestSaveEmailOld()
        End Function
        Public Function SearchRequestEmailOTPValidateOld(validatedId As String, otp As String) As Int16
            objDB.otp = otp
            objDB.id = validatedId

            Return objDB.SearchRequestEmailOTPValidateOld()
        End Function
        Public Function SearchRequestSendOld(validatedId As Int64, grn_id As Int64, reason As String) As DataTable
            objDB.grn_id = grn_id
            objDB.id = validatedId
            objDB.reason = reason
            Return objDB.SearchRequestSendOld()
        End Function
        Public Shared Function SearchRequestDetailsOld(status As String, strdraw As String, start As String, length As String, searchvalue As String, order_by As String, orderByDir As String) As MyNestedDataTable
            Dim datatable As New MyNestedDataTable
            Dim objPI As New WebService_DB
            Dim recList As New List(Of Dictionary(Of String, Object))
            Dim objIDR As IDataReader = Nothing
            Dim totalCount As Integer = 0
            Try
                'objPI.Mo_Cd = mo_auto_cd
                'objPI.req_to = req_to
                objPI.status = status
                objPI.start = Convert.ToInt32(start)
                objPI.length = Convert.ToInt32(length)
                objPI.value = Convert.ToString(searchvalue)
                objPI.order_by = Convert.ToString(order_by)
                objPI.orderByDir = Convert.ToString(orderByDir)

                objIDR = objPI.SearchRequestDetailsOld()

                While objIDR.Read
                    Dim rec As New Dictionary(Of String, Object)

                    totalCount = Convert.ToInt32(objIDR("TotalCount"))
                    rec("id") = Convert.ToString(objIDR("id"))
                    rec("Act_cd") = Convert.ToString(objIDR("Act_cd"))
                    rec("req_act") = Convert.ToString(objIDR("req_act"))
                    rec("request_id") = Convert.ToString(objIDR("request_id"))
                    rec("groom_name") = Convert.ToString(objIDR("groom_name"))
                    rec("bride_name") = Convert.ToString(objIDR("bride_name"))
                    rec("groom_father") = Convert.ToString(objIDR("groom_father"))
                    rec("bride_father") = Convert.ToString(objIDR("bride_father"))
                    rec("groom_dob") = If(Not objIDR("groom_dob") Is DBNull.Value, Convert.ToDateTime(objIDR("groom_dob")).ToString("dd-MM-yyyy"), "")
                    rec("bride_dob") = If(Not objIDR("bride_dob") Is DBNull.Value, Convert.ToDateTime(objIDR("bride_dob")).ToString("dd-MM-yyyy"), "")
                    rec("reg_date_from") = If(Not objIDR("reg_date_from") Is DBNull.Value, Convert.ToDateTime(objIDR("reg_date_from")).ToString("dd-MM-yyyy"), "")
                    rec("reg_date_to") = If(Not objIDR("reg_date_to") Is DBNull.Value, Convert.ToDateTime(objIDR("reg_date_to")).ToString("dd-MM-yyyy"), "")
                    rec("r_email") = Convert.ToString(objIDR("r_email"))
                    rec("r_name") = Convert.ToString(objIDR("r_name"))
                    rec("r_mob") = Convert.ToString(objIDR("r_mob"))
                    rec("r_address") = Convert.ToString(objIDR("r_address"))
                    rec("GRN_No") = Convert.ToString(objIDR("GRN_No"))
                    rec("total_amount") = Convert.ToString(objIDR("total_amount"))
                    rec("Searching_Reason") = Convert.ToString(objIDR("Searching_Reason"))
                    rec("requested_on") = If(Not objIDR("requested_on") Is DBNull.Value, Convert.ToDateTime(objIDR("requested_on")).ToString("dd-MM-yyyy hh:mm:ss tt"), "")
                    rec("status") = Convert.ToString(objIDR("status"))
                    rec("status_ts") = If(Not objIDR("status_ts") Is DBNull.Value, Convert.ToDateTime(objIDR("status_ts")).ToString("dd-MM-yyyy hh:mm:ss tt"), "")
                    rec("status_by") = Convert.ToString(objIDR("status_by"))
                    rec("status_otp") = Convert.ToString(objIDR("status_otp"))
                    rec("mo_name") = Convert.ToString(objIDR("mo_name"))

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
        Public Function Save_SearchRequestApproveOld(id As Int64, status As String, otp As String, user_nm As String, g_name As String, b_name As String, reg_dt As String, act As String, mo_name As String) As String
            objDB.id = id
            objDB.status = status
            objDB.otp = otp
            objDB.user_name = user_nm
            objDB.groom_name = g_name
            objDB.bride_name = b_name
            objDB.reg_dates = reg_dt
            objDB.actCd = act
            objDB.mo_name = mo_name

            Return objDB.Save_SearchRequestApproveOld()
        End Function
        Public Function FetchSearchRequestDetailOld(id As Int64, status As String) As DataSet
            objDB.id = id
            objDB.status = status

            Return objDB.FetchSearchRequestDetailOld()
        End Function
        Public Function Details_SearchRecordDataOld(SearchRecId As Int64) As DataSet
            objDB.reqid = Convert.ToString(SearchRecId)
            Return objDB.Details_SearchRecordDataOld()
        End Function
#End Region
#Region "Service Docket"
        Public Function SaveServiceDocket(service As String, stext As String, username As String) As String
            objDB.reason = service
            objDB.mdatas = stext
            objDB.user_name = username
            Return objDB.SaveServiceDocket()
        End Function
        Public Function FetchDocketDetails(docket_no As String) As DataTable
            objDB.docket_no = docket_no
            Return objDB.FetchDocketDetails().Tables(0)
        End Function
        Public Shared Function DocketList(dno As String, strdraw As String, start As String, length As String, searchvalue As String, order_by As String, orderByDir As String) As MyNestedDataTable
            Dim datatable As New MyNestedDataTable
            Dim objPI As New WebService_DB
            Dim recList As New List(Of Dictionary(Of String, Object))
            Dim objIDR As IDataReader = Nothing
            Dim totalCount As Integer = 0
            Dim a As New Marreg_Class
            Try
                objPI.docket_no = Convert.ToString(dno)
                objPI.start = Convert.ToInt32(start)
                objPI.length = Convert.ToInt32(length)
                objPI.value = Convert.ToString(searchvalue)
                objPI.order_by = Convert.ToString(order_by)
                objPI.orderByDir = Convert.ToString(orderByDir)

                objIDR = objPI.DocketList()

                While objIDR.Read
                    Dim rec As New Dictionary(Of String, Object)

                    totalCount = Convert.ToInt32(objIDR("TotalCount"))
                    rec("id") = Convert.ToString(objIDR("id"))
                    rec("docket_no") = Convert.ToString(objIDR("docket_no"))
                    rec("status_cd") = Convert.ToString(objIDR("status_cd"))
                    rec("Status_Name") = Convert.ToString(objIDR("Status_Name"))
                    rec("docket_dt") = Convert.ToDateTime(objIDR("created_at")).ToString("dd-MM-yyyy")
                    rec("created_at") = Convert.ToDateTime(objIDR("created_at")).ToString("dd-MM-yyyy hh:mm:ss tt")
                    rec("updated_at") = Convert.ToDateTime(objIDR("updated_at")).ToString("dd-MM-yyyy hh:mm:ss tt")
                    rec("created_by") = a.sqlInj_out(Convert.ToString(objIDR("created_by")))
                    rec("service_code") = Convert.ToString(objIDR("service_code"))
                    rec("service_name") = Common.PaymentRemarks().Item(Convert.ToString(objIDR("service_code")))
                    rec("dept_payment_ref_id") = Convert.ToString(objIDR("dept_payment_ref_id"))
                    rec("payment_amt") = Convert.ToString(objIDR("payment_amt"))
                    rec("payment_status") = Convert.ToString(objIDR("payment_status"))
                    rec("payment_status_desc") = Convert.ToString(objIDR("payment_status_desc"))
                    rec("sys_time_stamp") = If(Not objIDR("sys_time_stamp") Is DBNull.Value, Convert.ToDateTime(objIDR("sys_time_stamp")).ToString("dd-MM-yyyy hh:mm:ss tt"), "")
                    rec("mtgid") = Convert.ToString(objIDR("mtgid"))

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
        Public Function UpdateServiceDocketDPR(dpr As String, docket_no As String) As String
            objDB.dpr = dpr
            objDB.docket_no = docket_no
            Return objDB.UpdateServiceDocketDPR()
        End Function
        Public Function UpdateServiceDocketStatus(dpr As String, docket_no As String) As String
            objDB.dpr = dpr
            objDB.docket_no = docket_no

            Return objDB.UpdateServiceDocketStatus()
        End Function
#End Region
    End Class
End Namespace
