
Imports System.Data
Imports System.Data.SqlClient
Imports MARREG.App_Code.DataAccessLayer

Partial Class Default5
    Inherits System.Web.UI.Page
    Public encdec As New EncryptionDecryption.EncryptionDecryption()
    Dim connection As SqlConnection = New SqlConnection()
    Dim a As New Marreg_Class
    Private Sub form1_Load(sender As Object, e As EventArgs) Handles form1.Load
        If (Not IsPostBack) Then
            Dim sql = "select cast(id as nvarchar),cast(id as nvarchar) from MARREG_FP_Vault_Temp"
            a.html_dropDownFill_string_reader(sql, tmp_id)
        End If

        'Dim encdec As New EncryptionDecryption.EncryptionDecryption()
        'Using cond As New Data.SqlClient.SqlConnection()
        '    cond.ConnectionString = ConfigurationManager.ConnectionStrings("marreg1").ConnectionString
        '    Using cmd As New Data.SqlClient.SqlCommand()
        '        cmd.CommandText = "SELECT id,copy_fpTemplate,copy_fpTemplate2 FROM MARREG_Biometric_DeviceInfo "
        '        cmd.Connection = cond
        '        cond.Open()
        '        Using sdr As Data.SqlClient.SqlDataReader = cmd.ExecuteReader()
        '            While sdr.Read()
        '                Dim val As New Dictionary(Of String, String)
        '                val("fpTemplate") = encdec.encrypt(sdr("copy_fpTemplate"))
        '                val("fpTemplate2") = encdec.encrypt(sdr("copy_fpTemplate2"))
        '                val("id") = sdr("id")

        '                Using cmd1 As New Data.SqlClient.SqlCommand()
        '                    cmd1.CommandText = String.Format("update MARREG_Biometric_DeviceInfo set fpTemplate='{0}',fpTemplate2='{1}' where id={2}", val("fpTemplate"), val("fpTemplate2"), val("id"))
        '                    cmd1.Connection = cond
        '                    cmd1.ExecuteNonQuery()
        '                End Using
        '            End While
        '        End Using
        '        cond.Close()
        '    End Using
        'End Using
    End Sub

    Private Sub encrypt_ServerClick(sender As Object, e As EventArgs) Handles encrypt.ServerClick
        Try
            encText.InnerText = encdec.encrypt(normalText.InnerText)
        Catch ex As Exception
            With msg
                .InnerText = ex.Message
                .Attributes.CssStyle.Item("color") = "red"
            End With
        End Try
    End Sub

    Private Sub decrypt_ServerClick(sender As Object, e As EventArgs) Handles decrypt.ServerClick
        Try
            decText.InnerText = encdec.Decrypt(encText.InnerText)
        Catch ex As Exception
            With msg
                .InnerText = ex.Message
                .Attributes.CssStyle.Item("color") = "red"
            End With
        End Try
    End Sub

    Private Sub decByte_ServerClick(sender As Object, e As EventArgs) Handles decByte.ServerClick
        Try
            Dim bte As Byte()
            'bte = Encoding.Default.GetBytes(encText.InnerText)
            'MARREG_SHA256.DecryptArray(bte)
            'decText.InnerText = System.Convert.ToBase64String(bte)

            Dim sql = "select * from MARREG_FP_Vault_Temp where id='" + tmp_id.Value + "'"
            connection.ConnectionString = ConfigurationManager.ConnectionStrings("marreg").ConnectionString
            connection.Open()
            Dim adp As SqlDataAdapter = New SqlDataAdapter(sql, connection)
            Dim ds As DataSet = New DataSet()
            adp.Fill(ds)
            Dim dt As New DataTable
            dt = ds.Tables(0)
            bte = dt.Rows(0)(col.Value)
            MARREG_SHA256.DecryptArray(bte)
            decText.InnerText = System.Convert.ToBase64String(bte)
        Catch ex As Exception
            With msg
                .InnerText = ex.Message
                .Attributes.CssStyle.Item("color") = "red"
            End With
        End Try
    End Sub

    Private Sub encByte_ServerClick(sender As Object, e As EventArgs) Handles encByte.ServerClick
        Try
            Dim bte As Byte()
            bte = System.Convert.FromBase64String(normalText.InnerText.ToString)
            encText.InnerText = Encoding.Default.GetString(MARREG_SHA256.EncryptArray(bte))
        Catch ex As Exception
            With msg
                .InnerText = ex.Message
                .Attributes.CssStyle.Item("color") = "red"
            End With
        End Try
    End Sub

    Private Sub base64_img_ServerClick(sender As Object, e As EventArgs) Handles base64_img.ServerClick
        img.Src = "data:image/png;base64," + base64Text.InnerText.ToString
    End Sub

    Private Sub encBSK_ServerClick(sender As Object, e As EventArgs) Handles encBSK.ServerClick
        Dim m As New MARREG_SHA256
        Dim javaScriptSerializer As New Script.Serialization.JavaScriptSerializer
        Dim l1 As New Dictionary(Of String, String)
        l1("userId") = "8527419636"
        l1("ticketNo") = "20231107151823279"
        l1("citizenMobile") = "9681831219"
        l1("citizenName") = "Utsabendu"
        Dim txt = javaScriptSerializer.Serialize(l1)
        'txt = "{""userid"":""8527419636"",""ticketno"":""20221202153322882"",""itizenmobile"":""9963258963"",""citizenname"":""Anupam Ghosh""}"
        m.cAesKey256 = "IP16TDW0L5AQB41V6S6J8QLTPLRXBV2W"
        m.cAesIV256 = "V0ZMZO6WZ45KY2PL"
        m.newaes()
        encText.InnerText = m.Encryption(txt)
    End Sub

    Private Sub decBSK_ServerClick(sender As Object, e As EventArgs) Handles decBSK.ServerClick
        Dim m As New MARREG_SHA256
        m.cAesKey256 = "IP16TDW0L5AQB41V6S6J8QLTPLRXBV2W"
        m.cAesIV256 = "V0ZMZO6WZ45KY2PL"
        m.newaes()
        decText.InnerText = m.Decryption(encText.InnerText)
    End Sub

    Private Sub compCheckSum_ServerClick(sender As Object, e As EventArgs) Handles compCheckSum.ServerClick
        Dim m As New MARREG_SHA256
        m.cAesKey256 = "IP16TDW0L5AQB41V6S6J8QLTPLRXBV2W"
        m.cAesIV256 = "V0ZMZO6WZ45KY2PL"
        m.newaes()
        base64Text.InnerText = m.CoputeHash(encText.InnerText)
    End Sub
End Class
