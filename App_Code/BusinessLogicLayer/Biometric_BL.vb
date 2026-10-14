Imports System
Imports System.Data


Namespace MARREG.App_Code.BusinessLogicLayer
    Public Class Biometric_BL
        Public encdec As New EncryptionDecryption.EncryptionDecryption()
        Shared encdec1 As New EncryptionDecryption.EncryptionDecryption()
#Region "Register Device"
        'Updated on 23-11-2023 for customization of capturing procedure
        Public Function RegisterDevice(Registration_Id As Integer, model As String, sl_no As String, manufacturer As String, mo_auto_cd As String, fpTemplate As String, finger As String, fpTemplate2 As String, finger2 As String, quality As Integer) As String
            Dim obj = New Biometric_DB()
            obj.Registration_Id = Registration_Id
            obj.model = model
            obj.sl_no = sl_no
            obj.manufacturer = manufacturer
            obj.mo_cd = mo_auto_cd
            obj.fpTemplate = encdec.encrypt(fpTemplate)
            obj.finger = finger
            obj.fpTemplate2 = encdec.encrypt(fpTemplate2)
            obj.finger2 = finger2
            obj.quality = quality       'Added on 23-11-2023 for customization of capturing procedure

            Return obj.RegisterDevice()
        End Function
#End Region
#Region "Biometric Details Save final Submission"
        Public Function SaveBiometricDetails(Draft_cd As Integer, flag As String, fpTemplate As String, imageByte As String) As String
            Dim obj = New Biometric_DB()
            obj.flag = flag
            obj.draft_cd = Draft_cd
            obj.fpTemplate = encdec.encrypt(fpTemplate)
            obj.imageByte = MARREG_SHA256.EncryptArray(Convert.FromBase64String(imageByte))

            Return obj.SaveBiometricDetails()
        End Function
#End Region

#Region "Device Details"
        Public Function MORegisteredDeviceDetails(mocd As String) As List(Of Dictionary(Of String, Object))
            Dim recList As New List(Of Dictionary(Of String, Object))
            Dim obj = New Biometric_DB()
            obj.mo_cd = mocd
            Dim objIDR As IDataReader = Nothing
            Try
                obj.mo_cd = mocd

                objIDR = obj.MORegisteredDeviceDetails()

                While objIDR.Read
                    Dim rec As New Dictionary(Of String, Object)

                    rec("id") = Convert.ToString(objIDR("id"))
                    rec("MO_Cd") = Convert.ToString(objIDR("MO_Cd"))
                    rec("manufacturer") = Convert.ToString(objIDR("manufacturer"))
                    rec("Device_sl_no") = Convert.ToString(objIDR("Device_sl_no"))
                    rec("Device_model_no") = Convert.ToString(objIDR("Device_model_no"))
                    rec("Status") = Convert.ToString(objIDR("Status"))
                    rec("MO_Name") = Convert.ToString(objIDR("MO_Name"))
                    Try
                        rec("fpTemplate") = encdec.Decrypt(Convert.ToString(objIDR("fpTemplate")))
                    Catch ex As Exception
                        rec("fpTemplate") = ""
                    End Try
                    rec("finger") = Convert.ToString(objIDR("finger"))
                    Try
                        rec("fpTemplate2") = encdec.Decrypt(Convert.ToString(objIDR("fpTemplate2")))
                    Catch ex As Exception
                        rec("fpTemplate2") = ""
                    End Try
                    rec("finger2") = Convert.ToString(objIDR("finger2"))
                    rec("capture_quality") = Convert.ToString(objIDR("capture_quality"))    'Added on 23-11-2023 for customization of capturing procedure
                    rec("verification_score") = Convert.ToString(objIDR("verification_score"))  'Added on 31-01-2024 for customization of verification procedure
                    rec("Entry_dt") = If(Not objIDR("Entry_dt") Is DBNull.Value, Convert.ToDateTime(objIDR("Entry_dt")).ToString("dd-MM-yyyy hh:mm:ss tt"), "")
                    rec("valid_dt") = If(Not objIDR("valid_dt") Is DBNull.Value, Convert.ToDateTime(objIDR("valid_dt")).ToString("dd-MM-yyyy hh:mm:ss tt"), "")
                    rec("Valid_Status") = If(Not objIDR("valid_dt") Is DBNull.Value, If(Convert.ToDateTime(objIDR("valid_dt")) > DateTime.Now, "Y", "N"), "Y")
                    'rec("Update_On") = If(Not objIDR("Update_On") Is DBNull.Value, Convert.ToDateTime(objIDR("Update_On")).ToString("dd-MM-yyyy hh:mm:ss tt"), "")
                    rec("Deleted_On") = If(Not objIDR("Deleted_On") Is DBNull.Value, Convert.ToDateTime(objIDR("Deleted_On")).ToString("dd-MM-yyyy hh:mm:ss tt"), "")
                    recList.Add(rec)
                    rec = Nothing
                End While

            Catch ex As Exception
                Throw ex
            Finally
                objIDR.Close()
                objIDR.Dispose()
                objIDR = Nothing
                obj = Nothing
            End Try
            Return recList
        End Function
#End Region
#Region "Registered Device List"
        Public Shared Function RegisteredDeviceList(strdraw As String, start As String, length As String, searchvalue As String, order_by As String, orderByDir As String) As MyNestedDataTable
            Dim datatable As New MyNestedDataTable
            Dim objPI As New Biometric_DB
            Dim recList As New List(Of Dictionary(Of String, Object))
            Dim objIDR As IDataReader = Nothing
            Dim totalCount As Integer = 0
            Try
                objPI.start = Convert.ToInt32(start)
                objPI.length = Convert.ToInt32(length)
                objPI.value = Convert.ToString(searchvalue)
                objPI.order_by = Convert.ToString(order_by)
                objPI.orderByDir = Convert.ToString(orderByDir)

                objIDR = objPI.RegisteredDeviceList()

                While objIDR.Read
                    Dim rec As New Dictionary(Of String, Object)

                    totalCount = Convert.ToInt32(objIDR("TotalCount"))
                    rec("id") = Convert.ToString(objIDR("id"))
                    rec("Device_sl_no") = Convert.ToString(objIDR("Device_sl_no"))
                    rec("Device_model_no") = Convert.ToString(objIDR("Device_model_no"))
                    rec("Entry_dt") = If(Not objIDR("Entry_dt") Is DBNull.Value, Convert.ToDateTime(objIDR("Entry_dt")).ToString("dd-MM-yyyy hh:mm:ss tt"), "")
                    rec("valid_dt") = If(Not objIDR("valid_dt") Is DBNull.Value, Convert.ToDateTime(objIDR("valid_dt")).ToString("dd-MM-yyyy hh:mm:ss tt"), "")
                    rec("Active_Status") = Convert.ToString(objIDR("Status"))
                    rec("MO_Cd") = Convert.ToString(objIDR("MO_Cd"))
                    rec("MO_Name") = Convert.ToString(objIDR("MO_Name"))
                    rec("manufacturer") = Convert.ToString(objIDR("manufacturer"))
                    'rec("fpTemplate") = Convert.ToString(objIDR("fpTemplate"))
                    Try
                        rec("fpTemplate") = encdec1.Decrypt(Convert.ToString(objIDR("fpTemplate")))
                    Catch ex As Exception
                        rec("fpTemplate") = ""
                    End Try
                    'rec("fpTemplate") = encdec1.Decrypt(Convert.ToString(objIDR("fpTemplate_e")))
                    rec("finger") = Convert.ToString(objIDR("finger"))
                    'rec("fpTemplate2") = Convert.ToString(objIDR("fpTemplate2"))
                    Try
                        rec("fpTemplate2") = encdec1.Decrypt(Convert.ToString(objIDR("fpTemplate2")))
                    Catch ex As Exception
                        rec("fpTemplate2") = ""
                    End Try
                    rec("finger2") = Convert.ToString(objIDR("finger2"))
                    rec("capture_quality") = Convert.ToString(objIDR("capture_quality"))        'Added on 23-11-2023 for customization of capturing procedure
                    rec("verification_score") = Convert.ToString(objIDR("verification_score"))  'Added on 31-01-2024 for customization of verification procedure
                    rec("Valid_Status") = If(Not objIDR("valid_dt") Is DBNull.Value, If(Convert.ToDateTime(objIDR("valid_dt")) > DateTime.Now, "Y", "N"), "Y")
                    rec("Update_On") = If(Not objIDR("Update_On") Is DBNull.Value, Convert.ToDateTime(objIDR("Update_On")).ToString("dd-MM-yyyy hh:mm:ss tt"), "")

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
#End Region
#Region "Registerd Device Status Change"
        Public Function RegisterdDeviceStatChange(id As Int64) As Int32
            Dim obj = New Biometric_DB()
            obj.Registration_Id = id
            Return obj.RegisterdDeviceStatChange()
        End Function
#End Region
#Region "Registerd Device Delete"
        Public Function RegisterdDeviceDelete(Registration_Id As Int64, mo_auto_cd As String) As String
            Dim obj = New Biometric_DB()
            obj.Registration_Id = Registration_Id
            obj.mo_cd = mo_auto_cd
            Return obj.RegisterdDeviceDelete()
        End Function
#End Region
        'Added on 17-10-2023
#Region "Save Bride Groom Witness LTI"
        Public Function SaveBrideGroomWitnessLTI(id As Int64, app_auto_cd As String, g_fpTemplate As String, g_fpBMPBase64 As String, b_fpTemplate As String, b_fpBMPBase64 As String,
                                                 wt1_fpTemplate As String, wt1_fpBMPBase64 As String, wt2_fpTemplate As String, wt2_fpBMPBase64 As String,
                                                 wt3_fpTemplate As String, wt3_fpBMPBase64 As String,
                                                 g_verified As String, b_verified As String) As String
            Dim obj = New Biometric_DB()
            obj.Registration_Id = id
            obj.app_auto_cd = app_auto_cd
            obj.g_fpTemplate = encdec.encrypt(g_fpTemplate)
            obj.g_bmpBase64 = MARREG_SHA256.EncryptArray(Convert.FromBase64String(g_fpBMPBase64))
            obj.b_fpTemplate = encdec.encrypt(b_fpTemplate)
            obj.b_bmpBase64 = MARREG_SHA256.EncryptArray(Convert.FromBase64String(b_fpBMPBase64))
            obj.wt1_fpTemplate = encdec.encrypt(wt1_fpTemplate)
            obj.wt1_bmpBase64 = MARREG_SHA256.EncryptArray(Convert.FromBase64String(wt1_fpBMPBase64))
            obj.wt2_fpTemplate = encdec.encrypt(wt2_fpTemplate)
            obj.wt2_bmpBase64 = MARREG_SHA256.EncryptArray(Convert.FromBase64String(wt2_fpBMPBase64))
            obj.wt3_fpTemplate = encdec.encrypt(wt3_fpTemplate)
            obj.wt3_bmpBase64 = MARREG_SHA256.EncryptArray(Convert.FromBase64String(wt3_fpBMPBase64))
            obj.g_verified = g_verified
            obj.b_verified = b_verified

            Return obj.SaveBrideGroomWitnessLTI()
        End Function
#End Region
#Region "Load Biom Info Bride Groom Witness LTI"
        Public Function LoadBiomInfoBrideGroomWitnessLTI(app_auto_cd As String) As Dictionary(Of String, Object)
            Dim ret As New Dictionary(Of String, Object)
            Dim obj = New Biometric_DB()
            Dim objIDR As IDataReader = Nothing
            Try
                obj.app_auto_cd = app_auto_cd

                objIDR = obj.LoadBiomInfoBrideGroomWitnessLTI()
                While objIDR.Read
                    ret("fpTemplate_groom") = encdec.Decrypt(Convert.ToString(objIDR("fpTemplate_groom")))
                    ret("fpTemplate_bride") = encdec.Decrypt(Convert.ToString(objIDR("fpTemplate_bride")))
                    ret("wt1_captured") = Convert.ToString(objIDR("wt1_captured"))
                    ret("wt2_captured") = Convert.ToString(objIDR("wt2_captured"))
                    ret("wt3_captured") = Convert.ToString(objIDR("wt3_captured"))
                    ret("g_verified") = Convert.ToString(objIDR("g_verified"))
                    ret("b_verified") = Convert.ToString(objIDR("b_verified"))
                    ret("g_veri_req") = Convert.ToString(objIDR("g_veri_req"))      'Added on 09-01-2024
                    ret("b_veri_req") = Convert.ToString(objIDR("b_veri_req"))      'Added on 09-01-2024
                    ret("quality") = Convert.ToString(objIDR("quality"))      'Added on 15-01-2024
                    ret("verification_score") = Convert.ToString(objIDR("verification_score"))      'Added on 15-01-2024
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
        'End Added on 17-10-2023
        'Added on 09-11-2023
#Region "Load Application Details"
        Public Function LoadApplicationDetails(app_no As String) As DataTable
            Dim dt As New DataTable
            Dim obj As New Biometric_DB
            obj.app_no = app_no
            Dim ret = obj.LoadApplicationDetails()
            dt = ret.Tables(0)
            Return dt
        End Function
#End Region
#Region "Reupload Bride Groom Witness LTI"
        Public Function ReuploadBrideGroomWitnessLTI(id As Int64, app_auto_cd As String, fpTemplate As String, fpBMPBase64 As String,
                                                 verified As String, type As String) As String
            Dim obj = New Biometric_DB()
            obj.Registration_Id = id
            obj.app_auto_cd = app_auto_cd
            obj.fpTemplate = encdec.encrypt(fpTemplate)
            obj.bmpBase64 = MARREG_SHA256.EncryptArray(Convert.FromBase64String(fpBMPBase64))
            obj.verified = verified
            obj.type = type

            Return obj.ReuploadBrideGroomWitnessLTI()
        End Function
#End Region
        'End Added on 09-11-2023
        'Added on 09-01-2024
#Region "Load Permission Details"
        Public Function LoadPermissionDetails(app_no As String) As DataTable
            Dim dt As New DataTable
            Dim obj As New Biometric_DB
            obj.app_no = app_no
            Dim ret = obj.LoadPermissionDetails()
            dt = ret.Tables(0)
            Return dt
        End Function
#End Region
#Region "Save Biometric Permission"
        Public Function SaveBiometricPermission(app_auto_cd As String, g_veri_req As Int32, b_veri_req As Int32, cap_qty_d As Int32, veri_score_d As Int32) As String
            Dim obj = New Biometric_DB()
            obj.app_auto_cd = app_auto_cd
            obj.g_veri_req = g_veri_req
            obj.b_veri_req = b_veri_req
            obj.quality = cap_qty_d
            obj.verification_score = veri_score_d

            Return obj.SaveBiometricPermission()
        End Function
#End Region
        'End Added on 09-01-2024
        'Added on 16-01-2024
#Region "Biometric Permission List"
        Public Shared Function BiometricPermissionList(strdraw As String, start As String, length As String, searchvalue As String, order_by As String, orderByDir As String) As MyNestedDataTable
            Dim datatable As New MyNestedDataTable
            Dim objPI As New Biometric_DB
            Dim recList As New List(Of Dictionary(Of String, Object))
            Dim objIDR As IDataReader = Nothing
            Dim totalCount As Integer = 0
            Try
                objPI.start = Convert.ToInt32(start)
                objPI.length = Convert.ToInt32(length)
                objPI.value = Convert.ToString(searchvalue)
                objPI.order_by = Convert.ToString(order_by)
                objPI.orderByDir = Convert.ToString(orderByDir)

                objIDR = objPI.BiometricPermissionList()

                While objIDR.Read
                    Dim rec As New Dictionary(Of String, Object)

                    totalCount = Convert.ToInt32(objIDR("TotalCount"))
                    rec("id") = Convert.ToString(objIDR("id"))
                    rec("Applicant_Cd") = Convert.ToString(objIDR("Applicant_Cd"))
                    rec("app_auto_cd") = Convert.ToString(objIDR("app_auto_cd"))
                    rec("g_veri_req") = Convert.ToString(objIDR("g_veri_req"))
                    rec("b_veri_req") = Convert.ToString(objIDR("b_veri_req"))
                    rec("quality") = Convert.ToString(objIDR("quality"))
                    rec("verification_score") = Convert.ToString(objIDR("verification_score"))
                    rec("declaration_captured") = If(Not objIDR("declaration_captured_at") Is DBNull.Value, Convert.ToDateTime(objIDR("declaration_captured_at")).ToString("dd-MM-yyyy hh:mm:ss tt"), "")
                    rec("created_at") = If(Not objIDR("created_at") Is DBNull.Value, Convert.ToDateTime(objIDR("created_at")).ToString("dd-MM-yyyy hh:mm:ss tt"), "")
                    rec("Bride_Name") = Convert.ToString(objIDR("Bride_Name"))
                    rec("Groom_Name") = Convert.ToString(objIDR("Groom_Name"))

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
#End Region
#Region "Biometric Permission Delete"
        Public Function BiometricPermissionDelete(id As Int64) As String
            Dim obj = New Biometric_DB()
            obj.Registration_Id = id
            Return obj.BiometricPermissionDelete()
        End Function
#End Region
        'End Added on 16-01-2024
        'Added on 31-01-2024
#Region "Update MO Bio Verification Score"
        Public Function UpdateMO_BioVerificationScore(id As Int32, score As Int32) As String
            Dim obj = New Biometric_DB()
            obj.Registration_Id = id
            obj.verification_score = score

            Return obj.UpdateMO_BioVerificationScore()
        End Function
#End Region
        'End Added on 31-01-2024
    End Class
End Namespace