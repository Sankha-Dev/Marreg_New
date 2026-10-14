Imports Microsoft.VisualBasic
Imports MARREG.App_Code
Imports System.Data

Public Class WS_MasterData_BL
#Region "Email Type"
    Public Shared Function EmailTypes(strdraw As String, start As String, length As String, searchvalue As String, order_by As String, orderByDir As String) As MyDataTable
        Dim datatable As New MyDataTable
        Dim objPI As New WS_MasterData_DB
        Dim recList As New List(Of Dictionary(Of String, String))
        Dim objIDR As IDataReader = Nothing
        Dim totalCount As Integer = 0
        Try
            objPI.start = Convert.ToInt32(start)
            objPI.length = Convert.ToInt32(length)
            objPI.value = Convert.ToString(searchvalue)
            objPI.order_by = Convert.ToString(order_by)
            objPI.orderByDir = Convert.ToString(orderByDir)

            objIDR = objPI.EmailTypes()

            While objIDR.Read
                Dim rec As New Dictionary(Of String, String)

                totalCount = Convert.ToInt32(objIDR("TotalCount"))
                rec("id") = Convert.ToString(objIDR("id"))
                rec("email_type") = Convert.ToString(objIDR("email_type"))
                rec("update_on") = If(Not objIDR("update_on") Is DBNull.Value, Convert.ToDateTime(objIDR("update_on")).ToString("dd-MM-yyyy hh:mm:ss tt"), "")

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
    Public Function SaveEmailType(id As Int64, email_type As String) As Int16
        Dim objDB As New WS_MasterData_DB
        objDB.id = id
        objDB.email_type = email_type

        Return objDB.SaveEmailType()
    End Function
    Public Function DeleteEmailType(id As Int64) As Int16
        Dim objDB As New WS_MasterData_DB
        objDB.id = id

        Return objDB.DeleteEmailType()
    End Function
#End Region
#Region "Email Format"
    Public Shared Function EmailFormats(strdraw As String, start As String, length As String, searchvalue As String, order_by As String, orderByDir As String) As MyDataTable
        Dim datatable As New MyDataTable
        Dim objPI As New WS_MasterData_DB
        Dim recList As New List(Of Dictionary(Of String, String))
        Dim objIDR As IDataReader = Nothing
        Dim totalCount As Integer = 0
        Try
            objPI.start = Convert.ToInt32(start)
            objPI.length = Convert.ToInt32(length)
            objPI.value = Convert.ToString(searchvalue)
            objPI.order_by = Convert.ToString(order_by)
            objPI.orderByDir = Convert.ToString(orderByDir)

            objIDR = objPI.EmailFormats()

            While objIDR.Read
                Dim rec As New Dictionary(Of String, String)

                totalCount = Convert.ToInt32(objIDR("TotalCount"))
                rec("id") = Convert.ToString(objIDR("id"))
                rec("email_type_id") = Convert.ToString(objIDR("email_type_id"))
                rec("email_type") = Convert.ToString(objIDR("email_type"))
                rec("email_body") = Convert.ToString(objIDR("email_body"))
                rec("email_subject") = Convert.ToString(objIDR("email_subject"))
                rec("update_on") = If(Not objIDR("update_on") Is DBNull.Value, Convert.ToDateTime(objIDR("update_on")).ToString("dd-MM-yyyy hh:mm:ss tt"), "")

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
    Public Function SaveEmailFormat(id As Int64, email_type As Int64, email_subject As String, email_body As String) As Int16
        Dim objDB As New WS_MasterData_DB
        objDB.id = id
        objDB.email_type_id = email_type
        objDB.email_subject = email_subject
        objDB.email_body = email_body

        Return objDB.SaveEmailFormat()
    End Function
    Public Function DeleteEmailFormat(id As Int64) As Int16
        Dim objDB As New WS_MasterData_DB
        objDB.id = id

        Return objDB.DeleteEmailFormat()
    End Function
    Public Function FetchEmailFormat(email_type As String) As DataSet
        Dim objDB As New WS_MasterData_DB
        objDB.email_type = Convert.ToString(email_type)
        Return objDB.FetchEmailFormat()
    End Function
#End Region
#Region "Master Data"
    Public Function LoadDistrict(state_cd As String) As DataSet
        Dim objDB As New WS_MasterData_DB
        objDB.state_cd = Convert.ToString(state_cd)
        Return objDB.LoadDistrict()
    End Function
    Public Function LoadSubDiv(dist_cd As String) As DataSet
        Dim objDB As New WS_MasterData_DB
        objDB.dist_cd = Convert.ToString(dist_cd)
        Return objDB.LoadSubDiv()
    End Function
    Public Function LoadPS(dist_cd As String) As DataSet
        Dim objDB As New WS_MasterData_DB
        objDB.dist_cd = Convert.ToString(dist_cd)
        Return objDB.LoadPS()
    End Function
    Public Function LoadBLock(dist_cd As String, subdiv_cd As String) As DataSet
        Dim objDB As New WS_MasterData_DB
        objDB.dist_cd = Convert.ToString(dist_cd)
        objDB.subdiv_cd = Convert.ToString(subdiv_cd)
        Return objDB.LoadBLock()
    End Function
    Public Function LoadmuniGP(dist_cd As String, subdiv_cd As String, blockcd As String, wa As String) As DataSet
        Dim objDB As New WS_MasterData_DB
        objDB.dist_cd = Convert.ToString(dist_cd)
        objDB.subdiv_cd = Convert.ToString(subdiv_cd)
        objDB.block_cd = Convert.ToString(blockcd)
        objDB.work_area = Convert.ToString(wa)
        Return objDB.LoadmuniGP()
    End Function
    Public Function LoadPO(dist_cd As String, pincode As String) As DataSet
        Dim objDB As New WS_MasterData_DB
        objDB.dist_cd = Convert.ToString(dist_cd)
        objDB.pincode = Convert.ToString(pincode)
        Return objDB.LoadPO()
    End Function
	Public Function LoadDivorceeSection(divoceeAct As String, maritalStatus As String) As DataSet
        Dim objDB As New WS_MasterData_DB
        objDB.divoceeAct = Convert.ToString(divoceeAct)
        objDB.maritalStatus = Convert.ToString(maritalStatus)
        Return objDB.LoadDivorceeSection()
    End Function

    Public Function LoadallMO(strdraw As String, start As String, length As String, searchvalue As String, order_by As String, orderByDir As String, act As String,
                              dist As String, ps As String, subdivision As String, block As String, munigp As String) As MyNestedDataTable

        Dim datatable As New MyNestedDataTable
        Dim recList As New List(Of Dictionary(Of String, Object))
        Dim obj As New WS_MasterData_DB()
        Dim objPI As New WS_MasterData_DB
        Dim objIDR As IDataReader = Nothing
        Dim totalCount As Integer = 0
        Try

            objPI.start = Convert.ToInt32(start)
            objPI.length = Convert.ToInt32(length)
            objPI.value = Convert.ToString(searchvalue)
            objPI.order_by = Convert.ToString(order_by)
            objPI.orderByDir = Convert.ToString(orderByDir)
            objPI.ddl_act = Convert.ToString(act)
            objPI.reg_dist = Convert.ToString(dist)
            objPI.reg_ps = Convert.ToString(ps)
            objPI.subdiv_cd = Convert.ToString(subdivision)
            objPI.block_cd = Convert.ToString(block)
            objPI.munigp = Convert.ToString(munigp)
            objIDR = objPI.LoadallMO()

            While objIDR.Read
                Dim rec As New Dictionary(Of String, Object)
                totalCount = Convert.ToInt32(objIDR("TotalCount"))
                rec("MO_Cd") = Convert.ToString(objIDR("mo_auto_cd"))
                rec("MO_Name") = Convert.ToString(objIDR("moname"))
                rec("MOb_No") = Convert.ToString(objIDR("contactno"))
                rec("Email_Id") = Convert.ToString(objIDR("email_id"))
                'rec("Mo_Image") = Convert.ToBase64String(objIDR("mo_image"))
                rec("Mo_Address") = Convert.ToString(objIDR("addresss"))
				rec("reg_office_address") = Convert.ToString(objIDR("reg_office_address"))
                rec("reg_dist_cd") = Convert.ToString(objIDR("reg_dist_cd"))
                rec("reg_dist_nm") = Convert.ToString(objIDR("reg_dist_nm"))
                rec("reg_ps_cd") = Convert.ToString(objIDR("reg_ps_cd"))
                rec("reg_ps_nm") = Convert.ToString(objIDR("reg_ps_nm"))
                rec("reg_po_cd") = Convert.ToString(objIDR("reg_po_cd"))
                rec("reg_po_nm") = Convert.ToString(objIDR("reg_po_nm"))
                rec("reg_pin_no") = Convert.ToString(objIDR("reg_pin_no"))
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
            obj = Nothing
        End Try
        Return datatable
    End Function

    Public Function LoadRegMO_other_details(mo_cd As String) As DataTable
        Dim objDB As New WS_MasterData_DB
        objDB.mo_cd = Convert.ToString(mo_cd)
        Return objDB.LoadRegMO_other_details()
    End Function
#End Region
#Region "Document Proof Type"
    Public Function LoadDocumentProofType(state_cd As String, nation As String) As DataSet
        Dim objDB As New WS_MasterData_DB
        objDB.doc_type = Convert.ToString(state_cd)
        objDB.nation = Convert.ToString(nation)
        Return objDB.LoadDocumentProofType()
    End Function
#End Region
End Class
