Imports ZXing
Imports ZXing.Common
Imports ZXing.QrCode.Internal
Imports ZXing.Rendering
Imports System.Drawing
Imports System.IO
Imports MARREG.App_Code.DataAccessLayer
Imports System.Data.SqlClient

Partial Public Class Default2
    Inherits System.Web.UI.Page
    Dim mc As New Marreg_Class()
    Dim en As New MARREG_SHA256()
    Dim a As New Marreg_Class()
    Protected CSRF As New HiddenField
#Region "Page Init"
    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        If Not Page.IsPostBack Then
            Dim milliseconds = CLng(DateTime.UtcNow.Subtract(New DateTime(1970, 1, 1)).TotalMilliseconds)
            Session("CSRF") = Guid.NewGuid.ToString() + milliseconds.ToString()
            CSRF.Value = Session("CSRF").ToString()
        End If
    End Sub
#End Region
    'Protected Overrides Sub OnPreRender(e As EventArgs)
    '    Response.ContentType = "audio/wav"
    '    Dim mstream = GetAudio("I, the holder of Aadhaar, hereby give my consent to MeitY/NIC/Directorate of Registrar General of Marriages, West Bengal to record my Aadhaar number for generation of Digital Marriage Certificate (DMC). I also give my consent for sharing of my Aadhaar number and my DMC with the Registrar General of Marriages, West Bengal for processing and intimation. MeitY/ N.I.C. have informed me that my Biometric will not be stored/shared.")
    '    mstream.Position = 0
    '    mstream.CopyTo(Response.OutputStream)
    '    Response.End()
    'End Sub
    'Public Shared Function GetAudio(input As String) As MemoryStream

    '    Dim mem = New MemoryStream()

    '    'Dim t As Threading.Thread()
    '    Dim t As New Threading.Thread(
    '      Sub()
    '          Dim synth As New Speech.Synthesis.SpeechSynthesizer
    '          synth.Rate = -5
    '          synth.SetOutputToWaveStream(mem)
    '          synth.Speak(input)
    '      End Sub
    '    )

    '    t.Start()
    '    t.Join()
    '    Return mem
    'End Function

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Data.SqlClient.SqlConnection.ClearAllPools()
        BarCodeGeneration()
        loadMO()
        Dim g_uid = Guid.NewGuid()
        Dim byt As Byte() = System.Text.Encoding.UTF8.GetBytes(g_uid.ToString())
        l1.Text = "123"
        l2.Text = encrypt(l1.Text.ToString())
        l3.Text = decrypt(l2.Text.ToString())

        'Dim b = DirectCast(Convert.FromBase64String(l2.Text), Guid)
        'l3.Text = System.Text.Encoding.UTF8.GetString(b)
        'l3.Text = b.ToString()

        Dim encdec As New EncryptionDecryption.EncryptionDecryption()
        l3.Text = encdec.encrypt("Rk1SACAyMAAAAADYAAABLAGQAMUAxQEAAABIH0C1ABEAAIBWABcVAEBsACOWAIBDAC6eAIBBAD8cAEDJAEn1AECnAFkAAECzAG70AIBCAHovAECXAH/+AEBAAIqzAEB6AI8fAEEMAJPeAICjAJXrAEBIAJgzAEBSAJm1AIDeAK/QAEECALjWAIDVALjNAECcALnHAEDOALrHAIB6AMs5AEDSAM9HAEC+ANPGAIEDAN/JAEBYAOXDAEC6AO5DAIDAAP+/AEBpAQdDAIDLAQy5AIB2ARk4AAAA")

    End Sub
    Private Sub loadMO()
        Dim sql As String = "SELECT       (case when mo.MO_MName is not null and mo.MO_MName<>'' then mo.MO_FName+' '+mo.MO_MName+' '+mo.MO_LName else mo.MO_FName+' '+mo.MO_LName end) mo_name, mo.MO_Autoincrmnt_Cd" &
                            " From            dbo.MARREG_MO_Details AS mo INNER JOIN" &
                         " dbo.MARREG_DSC_Registration_Info AS dsc ON mo.MO_Autoincrmnt_Cd = dsc.MO_Cd where dsc.Deleted_On is null"
        a.dropDownFill_string_reader(sql, ddlsigner)
    End Sub
    Private Function encrypt(text As String) As String
        Dim enc As New MARREG_SHA256()
        Return enc.Encryption(text)
    End Function
    Private Function decrypt(text As String) As String
        Dim enc As New MARREG_SHA256()
        Return enc.Decryption(text)
    End Function
    Private Sub BarCodeGeneration()
        Dim app_cd = "144"
        Dim encdata = en.Encryption(app_cd)
        Dim cs = en.CoputeHash(encdata)

        Dim barcodeWriter As New BarcodeWriter()
        Dim encodingOptions = New EncodingOptions()
        encodingOptions.Width = 300
        encodingOptions.Height = 300
        encodingOptions.Margin = 0
        encodingOptions.PureBarcode = False
        encodingOptions.Hints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.H)
        barcodeWriter.Renderer = New BitmapRenderer()
        barcodeWriter.Options = encodingOptions
        barcodeWriter.Format = BarcodeFormat.QR_CODE
        Dim bitmap As Bitmap = barcodeWriter.Write(baseUrl() + "MARREG_Portal/MARREG_Certificate_Verification.aspx?a=" + Server.UrlEncode(encdata) + "&cs=" + cs)
        Dim logo = New Bitmap(Server.MapPath("~/MARREG_Images/Portal/logo.png"))
        Dim g As Graphics = Graphics.FromImage(bitmap)
        g.DrawImage(logo, New Point((bitmap.Width - logo.Width) / 2, (bitmap.Height - logo.Height) / 2))
        Dim p As MemoryStream = mc.ResizeImage(bitmap, 120, 120)

        Dim imgbyte As Byte() = p.ToArray()
        Dim base64String As String = Convert.ToBase64String(imgbyte)
        qr_image.ImageUrl = "data:image/png;base64, " + base64String
    End Sub
    Private Sub lblLink_ServerClick(sender As Object, e As EventArgs) Handles lblLink.ServerClick
        Dim db As New MasterDB
        Dim f = db.FecthCertificate("15")
        If (f.Rows.Count > 0) Then
            Dim bytFile As Byte()
            bytFile = f.Rows(0)("FileStreamCol")
            Response.Clear()
            Response.Buffer = True
            Response.ContentType = "application/pdf"
            Response.AddHeader("content-disposition", "attachment;filename=Tr.pdf")
            Response.Charset = ""
            Response.Cache.SetCacheability(HttpCacheability.NoCache)
            Response.BinaryWrite(bytFile)
            Response.End()
        End If
    End Sub
    'Protected Sub checkbtn_click(sender As Object, e As EventArgs) Handles checkbtn.Click
    '    Try
    '        If (CheckBrideGroomAge()) Then
    '            lblmsg.Text = "OK"
    '        Else
    '            lblmsg.Text = "NOT OK"
    '        End If
    '    Catch ex As Exception
    '        lblmsg.Text = ex.Message
    '        lblmsg.CssClass = "text-danger"
    '    End Try
    'End Sub
    'Private Function CheckBrideGroomAge() As Boolean
    '    Dim groom_dob = txtgdob.Text
    '    Dim bride_dob = txtbdob.Text
    '    Dim socl_mrg_dt = txtsclmardt.Text
    '    Dim act = "01"
    '    Dim G_MinAge As Integer = 0
    '    Dim B_MinAge As Integer = 0
    '    If act = "01" Or act = "03" Then
    '        If (Convert.ToDateTime(socl_mrg_dt) <= Convert.ToDateTime(ConfigurationManager.AppSettings("child_mrg_act_1978").ToString())) Then
    '            G_MinAge = 18
    '            B_MinAge = 15
    '        Else
    '            G_MinAge = 21
    '            B_MinAge = 18
    '        End If
    '    ElseIf act = "02" Then
    '        Return True
    '    End If
    '    Dim groom_age As Integer
    '    groom_age = Convert.ToDateTime(socl_mrg_dt).Year - Convert.ToDateTime(groom_dob).Year
    '    If (Convert.ToDateTime(groom_dob) > Convert.ToDateTime(socl_mrg_dt).AddYears(-groom_age)) Then groom_age -= 1

    '    Dim bride_age As Integer
    '    bride_age = Convert.ToDateTime(socl_mrg_dt).Year - Convert.ToDateTime(bride_dob).Year
    '    If (Convert.ToDateTime(bride_dob) > Convert.ToDateTime(socl_mrg_dt).AddYears(-bride_age)) Then bride_age -= 1

    '    If (groom_age < G_MinAge) Then
    '        ScriptManager.RegisterClientScriptBlock(Me, Me.[GetType](), "JS", "alert('Groom Age less than " + G_MinAge.ToString() + " years');", True)
    '        Return False
    '    End If
    '    If (bride_age < B_MinAge) Then
    '        ScriptManager.RegisterClientScriptBlock(Me, Me.[GetType](), "JS", "alert('Bride Age less than " + B_MinAge.ToString() + " years');", True)
    '        Return False
    '    End If
    '    Return True
    'End Function
End Class
