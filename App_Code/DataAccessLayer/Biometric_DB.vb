Imports System.Data
Imports System.Data.SqlClient
Imports MARREG.App_Code.DataAccessLayer
Imports Microsoft.VisualBasic

Public Class Biometric_DB
    Public dbconnctn As New DataBaseHelperDB()
    Dim dbf As New DBFactoryDB()
    Private con As New SqlConnection(dbconnctn.GetConnectionString())
    Public Property Registration_Id As Integer
    Public Property model As String
    Public Property sl_no As String
    Public Property manufacturer As String
    Public Property fpTemplate As String
    Public Property finger As String
    Public Property fpTemplate2 As String
    Public Property finger2 As String
    Public Property fpTemplate3 As String
    Public Property finger3 As String
    Public Property mo_cd As String
    Public Property start As Integer
    Public Property length As Integer
    Public Property value As String
    Public Property order_by As String
    Public Property orderByDir As String

    Public Property flag As String
    Public Property draft_cd As String
    Public Property imageByte As Byte()

    Public Property app_auto_cd As String
    Public Property g_fpTemplate As String
    Public Property g_fpBMPBase64 As String
    Public Property g_bmpBase64 As Byte()
    Public Property b_fpTemplate As String
    Public Property b_fpBMPBase64 As String
    Public Property b_bmpBase64 As Byte()
    Public Property wt1_fpTemplate As String
    Public Property wt1_fpBMPBase64 As String
    Public Property wt1_bmpBase64 As Byte()
    Public Property wt2_fpTemplate As String
    Public Property wt2_fpBMPBase64 As String
    Public Property wt2_bmpBase64 As Byte()
    Public Property wt3_fpTemplate As String
    Public Property wt3_fpBMPBase64 As String
    Public Property wt3_bmpBase64 As Byte()
    Public Property g_verified As String
    Public Property b_verified As String
    'Added on 09-11-2023
    Public Property app_no As String
    Public Property bmpBase64 As Byte()
    Public Property verified As String
    Public Property type As String
    'End Added on 09-11-2023
    Public Property quality As Integer      'Added on 23-11-2023 for customization of capturing procedure
    'Added on 09-01-2024
    Public g_veri_req As Integer
    Public b_veri_req As Integer
    Public verification_score As Integer
    'End Added on 09-01-2024

#Region "Register Device"
    Public Function RegisterDevice() As String

        Dim ReturnVal As String = ""
        Try
            con.Open()
            Using command As New SqlCommand("MARREG_Biometric_Device_Registration_InsertUpdate", con)
                command.CommandType = CommandType.StoredProcedure
                command.Parameters.Add("@REG_ID", SqlDbType.Int).Value = Registration_Id
                command.Parameters.Add("@Device_modelno", SqlDbType.NVarChar).Value = model
                command.Parameters.Add("@Device_slno", SqlDbType.NVarChar).Value = sl_no
                command.Parameters.Add("@manufacturer", SqlDbType.NVarChar).Value = manufacturer
                command.Parameters.Add("@MO_Cd", SqlDbType.NVarChar).Value = mo_cd
                command.Parameters.Add("@fpTemplate", SqlDbType.NVarChar).Value = fpTemplate
                command.Parameters.Add("@finger", SqlDbType.NVarChar).Value = finger
                command.Parameters.Add("@fpTemplate2", SqlDbType.NVarChar).Value = fpTemplate2
                command.Parameters.Add("@finger2", SqlDbType.NVarChar).Value = finger2
                command.Parameters.Add("@capture_quality", SqlDbType.Int).Value = quality       'Added on 23-11-2023 for customization of capturing procedure
                command.Parameters.Add("@Msg", SqlDbType.NVarChar, 4000).Direction = ParameterDirection.Output
                command.ExecuteNonQuery()

                ReturnVal = Convert.ToString(command.Parameters("@Msg").Value.ToString())
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            con.Close()
        End Try
        Return ReturnVal
    End Function
#End Region
#Region "Device Details"
    Function MORegisteredDeviceDetails() As IDataReader
        Dim dr As IDataReader = Nothing
        Try
            con.Open()
            Using command As New SqlCommand("MARREG_Biometric_Device_GetDetails", con)
                command.CommandType = CommandType.StoredProcedure
                command.Parameters.Add("@Mo_Cd", SqlDbType.NVarChar).Value = mo_cd
                dr = command.ExecuteReader()
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            'con.Close()
        End Try
        Return dr
    End Function
#End Region
#Region "Registered Device List"
    Function RegisteredDeviceList() As IDataReader
        Dim dr As IDataReader = Nothing
        Try
            con.Open()
            Using command As New SqlCommand("MARREG_RegisteredDeviceList", con)
                command.CommandType = CommandType.StoredProcedure
                command.Parameters.Add("@start", SqlDbType.Int).Value = start
                command.Parameters.Add("@length", SqlDbType.Int).Value = length
                command.Parameters.Add("@value", SqlDbType.NVarChar).Value = value
                command.Parameters.Add("@order_by", SqlDbType.NVarChar).Value = order_by
                command.Parameters.Add("@orderByDir", SqlDbType.NVarChar).Value = orderByDir
                dr = command.ExecuteReader()
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            'con.Close()
        End Try
        Return dr
    End Function
#End Region
#Region "Registerd Device Status Change"
    Public Function RegisterdDeviceStatChange() As Int32
        Dim ReturnVal As New Int32
        Try
            con.Open()
            Using command As New SqlCommand("MARREG_RegisterdDeviceStatChange", con)
                command.CommandType = CommandType.StoredProcedure
                command.Parameters.Add("@id", SqlDbType.NVarChar).Value = Registration_Id

                command.Parameters.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.Output
                command.ExecuteNonQuery()

                ReturnVal = Convert.ToInt32(command.Parameters("@returnValue").Value.ToString())
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            con.Close()
        End Try
        Return ReturnVal
    End Function
#End Region
#Region "Registerd Device Delete"
    Public Function RegisterdDeviceDelete() As String
        Dim ReturnVal As String = ""
        Try
            con.Open()
            Using command As New SqlCommand("MARREG_Biometric_Device_Registration_InsertUpdate", con)
                command.CommandType = CommandType.StoredProcedure
                command.Parameters.Add("@REG_ID", SqlDbType.NVarChar).Value = Registration_Id
                command.Parameters.Add("@MO_Cd", SqlDbType.NVarChar).Value = mo_cd

                command.Parameters.Add("@Msg", SqlDbType.NVarChar, 4000).Direction = ParameterDirection.Output
                command.ExecuteNonQuery()

                ReturnVal = Convert.ToString(command.Parameters("@Msg").Value.ToString())
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            con.Close()
        End Try
        Return ReturnVal
    End Function
#End Region
#Region "Biometric details save final submission"
    Public Function SaveBiometricDetails() As String

        Dim ReturnVal As String = ""
        Try
            con.Open()
            Using command As New SqlCommand("MARREG_Biometric_Details_Save", con)
                command.CommandType = CommandType.StoredProcedure
                command.Parameters.Add("@Draft_Cd", SqlDbType.NVarChar).Value = draft_cd
                command.Parameters.Add("@Flag", SqlDbType.NVarChar).Value = flag
                command.Parameters.Add("@FpTemplate", SqlDbType.NVarChar).Value = fpTemplate
                command.Parameters.Add("@ImageByte", SqlDbType.VarBinary).Value = imageByte

                command.Parameters.Add("@Msg", SqlDbType.NVarChar, 10).Direction = ParameterDirection.Output
                command.ExecuteNonQuery()

                ReturnVal = Convert.ToString(command.Parameters("@Msg").Value.ToString())
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            con.Close()
        End Try
        Return ReturnVal
    End Function
#End Region
    'Added on 17-10-2023
#Region "Save Bride Groom Witness LTI"
    Public Function SaveBrideGroomWitnessLTI() As String
        Dim ReturnVal As String = ""
        Try
            con.Open()
            Using command As New SqlCommand("MARREG_SaveBrideGroomWitnessLTI", con)
                command.CommandType = CommandType.StoredProcedure
                command.Parameters.Add("@id", SqlDbType.NVarChar).Value = Registration_Id
                command.Parameters.Add("@app_auto_cd", SqlDbType.NVarChar).Value = app_auto_cd

                command.Parameters.Add("@g_fpTemplate", SqlDbType.NVarChar).Value = g_fpTemplate
                command.Parameters.Add("@g_imageByte", SqlDbType.VarBinary).Value = g_bmpBase64

                command.Parameters.Add("@b_fpTemplate", SqlDbType.NVarChar).Value = b_fpTemplate
                command.Parameters.Add("@b_imageByte", SqlDbType.VarBinary).Value = b_bmpBase64

                command.Parameters.Add("@wt1_fpTemplate", SqlDbType.NVarChar).Value = wt1_fpTemplate
                command.Parameters.Add("@wt1_imageByte", SqlDbType.VarBinary).Value = wt1_bmpBase64

                command.Parameters.Add("@wt2_fpTemplate", SqlDbType.NVarChar).Value = wt2_fpTemplate
                command.Parameters.Add("@wt2_imageByte", SqlDbType.VarBinary).Value = wt2_bmpBase64

                command.Parameters.Add("@wt3_fpTemplate", SqlDbType.NVarChar).Value = wt3_fpTemplate
                command.Parameters.Add("@wt3_imageByte", SqlDbType.VarBinary).Value = wt3_bmpBase64

                command.Parameters.Add("@g_verified", SqlDbType.NVarChar).Value = g_verified
                command.Parameters.Add("@b_verified", SqlDbType.NVarChar).Value = b_verified

                command.Parameters.Add("@Msg", SqlDbType.NVarChar, 4000).Direction = ParameterDirection.Output
                command.ExecuteNonQuery()

                ReturnVal = Convert.ToString(command.Parameters("@Msg").Value.ToString())
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            con.Close()
        End Try
        Return ReturnVal
    End Function
#End Region
#Region "Load Biom Info Bride Groom Witness LTI"
    Public Function LoadBiomInfoBrideGroomWitnessLTI() As IDataReader
        Dim dr As IDataReader = Nothing
        Try
            con.Open()
            Using command As New SqlCommand("MARREG_LoadBiomInfoBrideGroomWitnessLTI", con)
                command.CommandType = CommandType.StoredProcedure
                command.Parameters.Add("@app_auto_cd", SqlDbType.NVarChar).Value = app_auto_cd
                dr = command.ExecuteReader()
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            'con.Close()
        End Try
        Return dr
    End Function
#End Region
    'End Added on 17-10-2023
    ' Added on 09-11-2023
#Region "Load Application Details"
    Public Function LoadApplicationDetails() As DataSet
        Dim ds As New DataSet
        Try
            con.Open()
            Using da As New SqlDataAdapter("WS_MARREG_LoadApplicationDetails", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure
                da.SelectCommand.Parameters.Add("@app_no", SqlDbType.NVarChar).Value = app_no
                da.Fill(ds)
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            con.Close()
        End Try
        Return ds
    End Function
#End Region
#Region "Reupload Bride Groom Witness LTI"
    Public Function ReuploadBrideGroomWitnessLTI() As String
        Dim ReturnVal As String = ""
        Try
            con.Open()
            Using command As New SqlCommand("MARREG_ReuploadBrideGroomWitnessLTI", con)
                command.CommandType = CommandType.StoredProcedure
                command.Parameters.Add("@id", SqlDbType.NVarChar).Value = Registration_Id
                command.Parameters.Add("@app_auto_cd", SqlDbType.NVarChar).Value = app_auto_cd

                command.Parameters.Add("@fpTemplate", SqlDbType.NVarChar).Value = fpTemplate
                command.Parameters.Add("@imageByte", SqlDbType.VarBinary).Value = bmpBase64

                command.Parameters.Add("@verified", SqlDbType.NVarChar).Value = verified
                command.Parameters.Add("@type", SqlDbType.NVarChar).Value = type

                command.Parameters.Add("@Msg", SqlDbType.NVarChar, 4000).Direction = ParameterDirection.Output
                command.ExecuteNonQuery()

                ReturnVal = Convert.ToString(command.Parameters("@Msg").Value.ToString())
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            con.Close()
        End Try
        Return ReturnVal
    End Function
#End Region
    'End Added on 09-11-2023
    'Added on 09-01-2024
#Region "Load Application Details"
    Public Function LoadPermissionDetails() As DataSet
        Dim ds As New DataSet
        Try
            con.Open()
            Using da As New SqlDataAdapter("WS_MARREG_LoadPermissionDetails", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure
                da.SelectCommand.Parameters.Add("@app_no", SqlDbType.NVarChar).Value = app_no
                da.Fill(ds)
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            con.Close()
        End Try
        Return ds
    End Function
#End Region
#Region "Save Biometric Permission"
    Public Function SaveBiometricPermission() As String
        Dim ReturnVal As String = ""
        Try
            con.Open()
            Using command As New SqlCommand("WS_MARREG_SaveBiometricPermission", con)
                command.CommandType = CommandType.StoredProcedure
                command.Parameters.Add("@app_auto_cd", SqlDbType.NVarChar).Value = app_auto_cd

                command.Parameters.Add("@g_veri_req", SqlDbType.Int).Value = g_veri_req
                command.Parameters.Add("@b_veri_req", SqlDbType.Int).Value = b_veri_req

                command.Parameters.Add("@quality", SqlDbType.Int).Value = quality
                command.Parameters.Add("@verification_score", SqlDbType.Int).Value = verification_score

                command.Parameters.Add("@Msg", SqlDbType.NVarChar, 4000).Direction = ParameterDirection.Output
                command.ExecuteNonQuery()

                ReturnVal = Convert.ToString(command.Parameters("@Msg").Value.ToString())
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            con.Close()
        End Try
        Return ReturnVal
    End Function
#End Region
    'End Added on 09-01-2024
    'Added on 16-01-2024
#Region "Biometric Permission List"
    Function BiometricPermissionList() As IDataReader
        Dim dr As IDataReader = Nothing
        Try
            con.Open()
            Using command As New SqlCommand("WS_MARREG_BiometricPermissionList", con)
                command.CommandType = CommandType.StoredProcedure
                command.Parameters.Add("@start", SqlDbType.Int).Value = start
                command.Parameters.Add("@length", SqlDbType.Int).Value = length
                command.Parameters.Add("@value", SqlDbType.NVarChar).Value = value
                command.Parameters.Add("@order_by", SqlDbType.NVarChar).Value = order_by
                command.Parameters.Add("@orderByDir", SqlDbType.NVarChar).Value = orderByDir
                dr = command.ExecuteReader()
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            'con.Close()
        End Try
        Return dr
    End Function
#End Region
#Region "Biometric Permission Delete"
    Public Function BiometricPermissionDelete() As String
        Dim ReturnVal As String = ""
        Try
            con.Open()
            Using command As New SqlCommand("WS_MARREG_BiometricPermissionDelete", con)
                command.CommandType = CommandType.StoredProcedure
                command.Parameters.Add("@id", SqlDbType.NVarChar).Value = Registration_Id

                command.Parameters.Add("@Msg", SqlDbType.Int, 10).Direction = ParameterDirection.Output
                command.ExecuteNonQuery()

                ReturnVal = Convert.ToString(command.Parameters("@Msg").Value.ToString())
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            con.Close()
        End Try
        Return ReturnVal
    End Function
#End Region
    'End Added on 16-01-2024
    'Added on 31-01-2024
#Region "Update MO Bio Verification Score"
    Public Function UpdateMO_BioVerificationScore() As String
        Dim ReturnVal As String = ""
        Try
            con.Open()
            Using command As New SqlCommand("WS_MARREG_UpdateMO_BioVerificationScore", con)
                command.CommandType = CommandType.StoredProcedure
                command.Parameters.Add("@id", SqlDbType.BigInt).Value = Registration_Id

                command.Parameters.Add("@verification_score", SqlDbType.Int).Value = verification_score

                command.Parameters.Add("@Msg", SqlDbType.NVarChar, 4000).Direction = ParameterDirection.Output
                command.ExecuteNonQuery()

                ReturnVal = Convert.ToString(command.Parameters("@Msg").Value.ToString())
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            con.Close()
        End Try
        Return ReturnVal
    End Function
#End Region
    'End Added on 31-01-2024
End Class