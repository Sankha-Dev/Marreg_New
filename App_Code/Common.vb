Imports System.IO
Imports System.Web.Script.Serialization
Imports Accord.Vision.Detection
Imports Accord.Vision.Detection.Cascades
Imports Microsoft.VisualBasic

Public Module Common
    Dim javaScriptSerializer As New JavaScriptSerializer()
    Public Function baseUrl() As String
        Dim url As String
        url = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath.TrimEnd("/") + "/"
        Return url
    End Function
    Function getIPAddress() As String
        Dim ipaddress As String
        ipaddress = Convert.ToString(HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR"))
        If ipaddress = "" Or ipaddress Is Nothing Then
            ipaddress = Convert.ToString(HttpContext.Current.Request.ServerVariables("REMOTE_ADDR"))
        End If
        Return ipaddress
    End Function
    Function PaymentRemarks() As NameValueCollection
        Dim datas As New NameValueCollection
        datas.Add("0001", "Solemnization/ Registration (Special Marriage)")
        datas.Add("0002", "Registration (Hindu Marriage)")
        datas.Add("0003", "Certified Copy")
        datas.Add("0004", "Inspection")
        datas.Add("0005", "Searching")
        datas.Add("0006", "Correction")
        datas.Add("0007", "Authentication")
        datas.Add("0008", "Forms & Stationery")
        Return datas
    End Function
    Public Certified_Copy_Request_Reject As String = "Certified Copy Request Reject"
    Public Intimation_for_Comment As String = "Intimation for Comment"
    Public Certified_Copy_Request_Approve As String = "Certified Copy Request Approve"
    Public OTP_for_Process_Request As String = "OTP for Process Request"
    Public OTP_for_Request_of_Certified_Copy_of_Marriage_Certificate As String = "OTP for Request of Certified Copy of Marriage Certificate"
    Public OTP_for_Comment As String = "OTP for Comment"
    Public Intimation_for_Hearing As String = "Intimation for Hearing"
    Public Certified_Copy_Request_On_Hold As String = "Certified Copy Request On Hold"
    Public OTP_for_Request_of_Searching_of_Marriage_Record As String = "OTP for Request of Searching of Marriage Record"
    Public Searching_Request_Approve As String = "Searching Request Approve"
    Public Searching_Request_Reject As String = "Searching Request Reject"
    Public Acknowledgement As String = "Acknowledgement"
    Public Intimation_for_Payment_Initiate As String = "Intimation for Payment Initiate"
    Public Certified_Copy_Request_Approve_Old As String = "Certified Copy Request Approve Old"
    Public Intimation_for_Payment_Complete As String = "Intimation for Payment Complete"
    Public Payment_Link_for_Solemn_Reg As String = "Payment Link for Solemnization/Registration"

    Function EmailTypes() As NameValueCollection
        Dim datas As New NameValueCollection
        datas.Add(1, "Certified Copy Request Reject")
        datas.Add(2, "Intimation for Comment")
        datas.Add(3, "Certified Copy Request Approve")
        datas.Add(4, "OTP for Process Request")
        datas.Add(5, "OTP for Request of Certified Copy of Marriage Certificate")
        datas.Add(6, "OTP for Comment")
        datas.Add(7, "Intimation for Hearing")
        datas.Add(8, "Certified Copy Request On Hold")
        datas.Add(9, "OTP for Request of Searching of Marriage Record")
        datas.Add(10, "Searching Request Approve")
        datas.Add(11, "Searching Request Reject")
        datas.Add(12, "Acknowledgement")
        Return datas
    End Function
    Function ReplaceEmailBody(ByRef Body As String, oldVal() As String, newVal() As String) As String
        If (oldVal.Count <> newVal.Count) Then
            Return Body
        End If
        For i As Integer = 0 To oldVal.Count - 1
            Body = Body.Replace(oldVal(i), newVal(i))
        Next
        Return Body
    End Function
    Function CreateAssocArray(oldVal() As String, newVal() As String) As Dictionary(Of String, Object)
        Dim arr As New Dictionary(Of String, Object)
        If (oldVal.Count = newVal.Count) Then
            For i As Integer = 0 To oldVal.Count - 1
                arr(oldVal(i).Replace("{", "").Replace("}", "")) = newVal(i)
            Next
        End If
        Return arr
    End Function
    Function IsEmail(ByVal email As String) As Boolean
        Static emailExpression As New Regex("^[a-zA-Z0-9][-._a-zA-Z0-9]*[a-zA-Z0-9]@[-.a-zA-Z0-9]+(\.[-.a-zA-Z0-9]+)*\.(com|edu|info|gov|int|mil|net|org|biz|name|museum|coop|aero|pro|tv|[a-zA-Z]{2})$")
        Return emailExpression.IsMatch(email)
    End Function
    Function IsPhoneFax(ByVal phone_fax As String) As Boolean
        Static faxExpression As New Regex("^[0-9]{3}[-0-9]{9}$")
        Return faxExpression.IsMatch(phone_fax)
    End Function
    Function IsAlpha(ByVal text As String) As Boolean
        Static textExpression As New Regex("^[a-zA-Z]*$")
        Return textExpression.IsMatch(text)
    End Function
    Function IsAlphaWithSpace(ByVal text As String) As Boolean
        Static textExpression As New Regex("^[a-zA-Z0-9 -.]*$")
        Return textExpression.IsMatch(text)
    End Function
    Function IsApplictionNo(ByVal text As String) As Boolean
        Static textExpression As New Regex("^[0-9-]+$")
        Return textExpression.IsMatch(text)
    End Function
    Function IsQualification(ByVal text As String) As Boolean
        Static textExpression As New Regex("^[a-zA-Z0-9 -+/.()]+$")
        Return textExpression.IsMatch(text)
    End Function
    Function IsNumber(ByVal text As String) As Boolean
        Static textExpression As New Regex("^[0-9]+$")
        Return textExpression.IsMatch(text)
    End Function
    Function IsPinCode(ByVal text As String) As Boolean
        Static textExpression As New Regex("^[0-9]{6}$")
        Return textExpression.IsMatch(text)
    End Function
    Function IsServiceDocket(ByVal text As String) As Boolean
        Static textExpression As New Regex("^[a-zA-Z0-9/]+$")
        Return textExpression.IsMatch(text)
    End Function
    Function IsPdf(ByVal file As System.Web.HttpPostedFile) As Boolean
        Dim rdr = New System.IO.BinaryReader(file.InputStream)
        Dim buffer As Byte
        Dim fileclass = ""
        buffer = rdr.ReadByte()
        fileclass = buffer.ToString()
        buffer = rdr.ReadByte()
        fileclass += buffer.ToString()
        rdr.BaseStream.Position = 0
        'rdr.Close()
        If (fileclass <> "3780") Then
            Return False
        Else
            Return True
        End If
    End Function
    Function FetchPaymentRefData(dept_payment_ref_id As String) As Object
        Try
            Dim objG As New MARREG.App_Code.GRIPS_BL
            Dim data = objG.Fetch_MARREG_Payment_Reference(dept_payment_ref_id)
            Dim reference_data_str = data.Rows(0)("reference_data").ToString()
            Dim reference_data = javaScriptSerializer.DeserializeObject(reference_data_str)
            Return reference_data
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Public Function DataTableToJSONWithJavaScriptSerializer(ByVal table As Data.DataTable) As List(Of Dictionary(Of String, Object))
        Dim jsSerializer As JavaScriptSerializer = New JavaScriptSerializer()
        Dim parentRow As List(Of Dictionary(Of String, Object)) = New List(Of Dictionary(Of String, Object))()
        Dim childRow As Dictionary(Of String, Object)

        For Each row As Data.DataRow In table.Rows
            childRow = New Dictionary(Of String, Object)()

            For Each col As Data.DataColumn In table.Columns
                childRow.Add(col.ColumnName, row(col))
            Next

            parentRow.Add(childRow)
        Next

        Return parentRow
    End Function
#Region "FaceDetect"
    Public Function FaceDetect(imgFace As Drawing.Bitmap) As Integer
        Dim detector As HaarObjectDetector
        Dim cascade As New FaceHaarCascade
        detector = New HaarObjectDetector(cascade, 12)
        detector.SearchMode = ObjectDetectorSearchMode.Average
        detector.ScalingFactor = 1.5F
        detector.ScalingMode = ObjectDetectorScalingMode.GreaterToSmaller
        detector.UseParallelProcessing = True
        detector.Suppression = 2.5
        Dim faceObjects As Drawing.Rectangle() = detector.ProcessFrame(imgFace)
        Dim g As Drawing.Graphics = Drawing.Graphics.FromImage(imgFace)
        If faceObjects.Length = 0 Then
            detector.SearchMode = ObjectDetectorSearchMode.Average
            detector.ScalingFactor = 1.5F
            detector.ScalingMode = ObjectDetectorScalingMode.SmallerToGreater
            detector.UseParallelProcessing = True
            detector.Suppression = 2.5
            faceObjects = detector.ProcessFrame(imgFace)
            g = Drawing.Graphics.FromImage(imgFace)
        End If
        g.Dispose()
        Return faceObjects.Count
    End Function
#End Region
#Region "Resize Image"
    Public Function ResizeImage(bmSource As Drawing.Bitmap, TargetWidth As Int32, TargetHeight As Int32) As MemoryStream
        Dim bmDest As New Drawing.Bitmap(TargetWidth, TargetHeight, Drawing.Imaging.PixelFormat.Format32bppArgb)
        Dim NewX = 0
        Dim NewY = 0
        Using grDest = Drawing.Graphics.FromImage(bmDest)
            With grDest
                .CompositingQuality = Drawing.Drawing2D.CompositingQuality.HighQuality
                .InterpolationMode = Drawing.Drawing2D.InterpolationMode.HighQualityBicubic
                .PixelOffsetMode = Drawing.Drawing2D.PixelOffsetMode.HighQuality
                .SmoothingMode = Drawing.Drawing2D.SmoothingMode.AntiAlias
                .CompositingMode = Drawing.Drawing2D.CompositingMode.SourceOver
                .DrawImage(bmSource, NewX, NewY, TargetWidth, TargetHeight)
            End With
        End Using

        Dim img As New MemoryStream()
        bmDest.Save(img, System.Drawing.Imaging.ImageFormat.Bmp)
        Return img
    End Function
#End Region
End Module
