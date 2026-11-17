
Imports System.IO

Partial Class Default6
    Inherits System.Web.UI.Page

    Protected Sub btn_upload_Click(sender As Object, e As EventArgs) Handles btn_upload.Click
        Dim inputStream As Stream = Stream.Null
        If file_upload.HasFile Then
            Dim postedfile = file_upload.PostedFile
            If (Not IsPdf(postedfile)) Then
                'ScriptManager.RegisterClientScriptBlock(Me, Me.[GetType](), "JS", "alert('Sorry, the selected file is invalid. Please select only .PDF files.');", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "msg", "<script>showpop6('Sorry, the selected file is invalid. Please select proper pdf file.')</script>", False)
                Return
            End If
            Dim fext = Path.GetExtension(file_upload.FileName)
            Dim fpath_doc, strArr_doc() As String
            Dim cnt_doc As Integer

            fpath_doc = file_upload.FileName
            strArr_doc = fpath_doc.Split(".")
            cnt_doc = strArr_doc.Count()

            If cnt_doc = 2 Then
                If fext = ".pdf" Then
                    inputStream = file_upload.PostedFile.InputStream
                    ViewState("fnam") = file_upload.FileName
                Else
                    ScriptManager.RegisterClientScriptBlock(Me, Me.[GetType](), "JS", "showpop6('Sorry, the selected file is invalid. Please select only .PDF files.');", True)
                    'mdl_cir.Show()
                End If
            Else
                ScriptManager.RegisterClientScriptBlock(Me, Me.[GetType](), "JS", "showpop6('Sorry, the selected file is invalid. Please select file with .PDF extension only. Filename contains dot(.) is not allowed!');", True)
                'mdl_cir.Show()
            End If
            If inputStream.Length > 0 Then
                If inputStream.Length <= 512000 Then    'max 500 kb
                    ViewState("circulardoc") = New Byte(inputStream.Length) {}
                    inputStream.Read(ViewState("circulardoc"), 0, ViewState("circulardoc").Length)
                    Session.Item("doc") = ViewState("circulardoc")
                    Save_Pdf("pdf_temp", ViewState("fnam"), "doc")
                Else
                    ScriptManager.RegisterClientScriptBlock(Me, Me.[GetType](), "JS", "showpop6('File too large. File size must not exceed 500KB.');", True)
                End If
            Else
                'Errormsgbox("Please select a .PDF file to upload.")
                ScriptManager.RegisterClientScriptBlock(Me, Me.[GetType](), "JS", "showpop6('Please select a .PDF file to upload.');", True)
            End If
        Else
            ScriptManager.RegisterClientScriptBlock(Me, Me.[GetType](), "JS", "showpop6('Sorry, no file is selected');", True)
        End If
    End Sub
    Protected Sub Save_Pdf(subfolder As String, filename As String, filename_ss As String)
        Dim path As String
        Dim doc_byte As Byte()

        doc_byte = DirectCast(Session.Item(filename_ss), Byte())
        path = subfolder + "/" + filename
        Using fs As New FileStream(Server.MapPath(path), FileMode.Create)   'FileMode.OpenOrCreate
            fs.Write(Session(filename_ss), 0, doc_byte.Length)
        End Using
        'Session(filename_ss) = Nothing
    End Sub
End Class
