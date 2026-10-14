Imports System.Data
Imports Microsoft.VisualBasic

Namespace MARREG.App_Code
    Public Class DSC_BL
        Public Function DSC_Info_Insert(Registration_Id As Integer, Mo_Cd As String, Content_Type As String, Certificate_Verified As String, Subject As String, Issuer_Name As String,
                                            Subject_Name As String, EmailName As String, Signature_Algorithm As String, Public_Key_Length As String, Issue_Date As DateTime,
                                            Valid_Date As DateTime, Public_Key_Str As String, Thumbprint As String, SerialNumber As String,
                                            Version As String, Oid As String) As Array
            Dim obj = New DSC_DB()
            Return obj.DSC_Info_Insert(Registration_Id, Mo_Cd, Content_Type, Certificate_Verified, Subject, Issuer_Name,
                                        Subject_Name, EmailName, Signature_Algorithm, Public_Key_Length, Issue_Date,
                                        Valid_Date, Public_Key_Str, Thumbprint, SerialNumber,
                                        Version, Oid)
        End Function

        Public Shared Function DSCRegList(strdraw As String, start As String, length As String, searchvalue As String, order_by As String, orderByDir As String) As MyNestedDataTable
            Dim datatable As New MyNestedDataTable
            Dim objPI As New DSC_DB
            Dim recList As New List(Of Dictionary(Of String, Object))
            Dim objIDR As IDataReader = Nothing
            Dim totalCount As Integer = 0
            Try
                objPI.start = Convert.ToInt32(start)
                objPI.length = Convert.ToInt32(length)
                objPI.value = Convert.ToString(searchvalue)
                objPI.order_by = Convert.ToString(order_by)
                objPI.orderByDir = Convert.ToString(orderByDir)

                objIDR = objPI.DSCRegList()

                While objIDR.Read
                    Dim rec As New Dictionary(Of String, Object)

                    totalCount = Convert.ToInt32(objIDR("TotalCount"))
                    rec("Registration_ID") = Convert.ToString(objIDR("Registration_ID"))
                    rec("MO_Autoincrmnt_Cd") = Convert.ToString(objIDR("MO_Autoincrmnt_Cd"))
                    rec("full_name") = Convert.ToString(objIDR("full_name"))
                    rec("Subject") = Convert.ToString(objIDR("Subject"))
                    rec("SubjectName") = Convert.ToString(objIDR("SubjectName"))
                    rec("ISSUER_NAME") = Convert.ToString(objIDR("ISSUER_NAME"))
                    rec("ISSUE_DATE") = If(Not objIDR("ISSUE_DATE") Is DBNull.Value, Convert.ToDateTime(objIDR("ISSUE_DATE")).ToString("dd-MM-yyyy hh:mm:ss tt"), "")
                    rec("VALID_DATE") = If(Not objIDR("VALID_DATE") Is DBNull.Value, Convert.ToDateTime(objIDR("VALID_DATE")).ToString("dd-MM-yyyy hh:mm:ss tt"), "")
                    rec("Content_Type") = Convert.ToString(objIDR("Content_Type"))
                    rec("Certificate_Verified") = Convert.ToString(objIDR("Certificate_Verified"))
                    rec("EmailName") = Convert.ToString(objIDR("EmailName"))
                    rec("Signature_Algorithm") = Convert.ToString(objIDR("Signature_Algorithm"))
                    rec("Public_Key_Length") = Convert.ToString(objIDR("Public_Key_Length"))
                    rec("Public_Key_Str") = Convert.ToString(objIDR("Public_Key_Str"))
                    rec("Thumbprint") = Convert.ToString(objIDR("Thumbprint"))
                    rec("SerialNumber") = Convert.ToString(objIDR("SerialNumber"))
                    rec("Version") = Convert.ToString(objIDR("Version"))
                    rec("Oid") = Convert.ToString(objIDR("Oid"))
                    rec("Entry_Date") = If(Not objIDR("Entry_Date") Is DBNull.Value, Convert.ToDateTime(objIDR("Entry_Date")).ToString("dd-MM-yyyy hh:mm:ss tt"), "")
                    rec("Active_Status") = Convert.ToString(objIDR("Active_Status"))
                    rec("Status") = If(Convert.ToDateTime(objIDR("VALID_DATE")) > DateTime.Now, "Y", "N")
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
        Public Function DSCDataTransferMO(Registration_Id As Int64) As Int32
            Dim obj = New DSC_DB()
            obj.Registration_Id = Registration_Id
            Return obj.DSCDataTransferMO()
        End Function
        Public Function DSCDataLost(Registration_Id As Int64) As Int32
            Dim obj = New DSC_DB()
            obj.Registration_Id = Registration_Id
            Return obj.DSCDataLost()
        End Function
        Public Function DSCStatChange(Registration_Id As Int64) As Int32
            Dim obj = New DSC_DB()
            obj.Registration_Id = Registration_Id
            Return obj.DSCStatChange()
        End Function
        Public Function DSCDataDelete(Registration_Id As Int64) As Int32
            Dim obj = New DSC_DB()
            obj.Registration_Id = Registration_Id
            Return obj.DSCDataDelete()
        End Function
        Public Function LoadMODSCDetails(mocd As String) As List(Of Dictionary(Of String, Object))
            Dim recList As New List(Of Dictionary(Of String, Object))
            Dim obj = New DSC_DB()
            obj.Mo_Cd = mocd
            Dim objIDR As IDataReader = Nothing
            Try
                obj.Mo_Cd = mocd

                objIDR = obj.LoadMODSCDetails()

                While objIDR.Read
                    Dim rec As New Dictionary(Of String, Object)

                    rec("Registration_ID") = Convert.ToString(objIDR("Registration_ID"))
                    rec("MO_Cd") = Convert.ToString(objIDR("MO_Cd"))
                    rec("Content_Type") = Convert.ToString(objIDR("Content_Type"))
                    rec("Certificate_Verified") = Convert.ToString(objIDR("Certificate_Verified"))
                    rec("Issuer_Name") = Convert.ToString(objIDR("Issuer_Name"))
                    rec("EmailName") = Convert.ToString(objIDR("EmailName"))
                    rec("Signature_Algorithm") = Convert.ToString(objIDR("Signature_Algorithm"))
                    rec("Public_Key_Length") = Convert.ToString(objIDR("Public_Key_Length"))
                    rec("Issue_Date") = If(Not objIDR("Issue_Date") Is DBNull.Value, Convert.ToDateTime(objIDR("Issue_Date")).ToString("dd-MM-yyyy hh:mm:ss tt"), "")
                    rec("Valid_Date") = If(Not objIDR("Valid_Date") Is DBNull.Value, Convert.ToDateTime(objIDR("Valid_Date")).ToString("dd-MM-yyyy hh:mm:ss tt"), "")
                    rec("Public_Key_Str") = Convert.ToString(objIDR("Public_Key_Str"))
                    rec("Thumbprint") = Convert.ToString(objIDR("Thumbprint"))
                    rec("SerialNumber") = Convert.ToString(objIDR("SerialNumber"))
                    rec("Version") = Convert.ToString(objIDR("Version"))
                    rec("Oid") = Convert.ToString(objIDR("Oid"))
                    rec("Version") = Convert.ToString(objIDR("Version"))
                    rec("Entry_Date") = If(Not objIDR("Entry_Date") Is DBNull.Value, Convert.ToDateTime(objIDR("Entry_Date")).ToString("dd-MM-yyyy hh:mm:ss tt"), "")
                    rec("Active_Status") = Convert.ToString(objIDR("Active_Status"))
                    rec("Valid_Status") = If(Convert.ToDateTime(objIDR("Valid_Date")) > DateTime.Now, "Y", "N")
                    rec("Update_On") = If(Not objIDR("Update_On") Is DBNull.Value, Convert.ToDateTime(objIDR("Update_On")).ToString("dd-MM-yyyy hh:mm:ss tt"), "")
                    rec("SubjectName") = Convert.ToString(objIDR("SubjectName"))
                    rec("Deleted_On") = If(Not objIDR("Deleted_On") Is DBNull.Value, Convert.ToDateTime(objIDR("Deleted_On")).ToString("dd-MM-yyyy hh:mm:ss tt"), "")
                    rec("Subject") = Convert.ToString(objIDR("Subject"))

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
    End Class
End Namespace
