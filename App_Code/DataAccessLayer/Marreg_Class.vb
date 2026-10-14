Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System
Imports System.IO
Imports System.Web.Security

Imports System.Net
Imports System.Net.Mail
Imports System.Security.Cryptography
Imports System.Xml

Imports ZXing
Imports ZXing.Common
Imports ZXing.QrCode.Internal
Imports ZXing.Rendering

Namespace MARREG.App_Code.DataAccessLayer
    Public Class Marreg_Class
        Protected conn_web As String = ConfigurationManager.ConnectionStrings("marreg").ConnectionString
        'Protected conn_web1 As String = ConfigurationManager.ConnectionStrings("marreg").ConnectionString
        Dim sql_in_out As sql_in_out.Class1 = New sql_in_out.Class1

#Region "MARREG Used Method"


        Public Sub DropDownFillSP(spName As String,
                          ddl As DropDownList,
                          Optional params As List(Of SqlParameter) = Nothing)

            Using conn As New SqlConnection(conn_web)
                Using cmd As New SqlCommand(spName, conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    ddl.Items.Clear()
                    ddl.Items.Add(New ListItem("-- Select --", "0"))

                    'Add parameters only if provided
                    If params IsNot Nothing AndAlso params.Count > 0 Then
                        cmd.Parameters.AddRange(params.ToArray())
                    End If

                    Try
                        conn.Open()
                        Using reader As SqlDataReader = cmd.ExecuteReader()
                            While reader.Read()
                                Dim item As New ListItem(
                            sqlInj_out(reader.GetString(0)),
                            sqlInj_out(reader.GetString(1))
                        )
                                ddl.Items.Add(item)
                            End While
                        End Using
                    Catch
                        ' Optional logging
                    End Try
                End Using
            End Using

        End Sub


        Sub dropDownFill_string_reader(ByRef sql As String, ByRef e As System.Web.UI.WebControls.DropDownList)
            Dim conn As New SqlConnection(conn_web)
            Try
                e.Items.Clear()

                Dim fisrtItem As New ListItem("-- Select --", "0")
                e.Items.Add(fisrtItem)
                Dim cmd1 As New SqlCommand(sql, conn)

                If conn.State = Data.ConnectionState.Open Then
                    conn.Dispose()
                End If
                conn.Open()

                Dim read1 As SqlDataReader = cmd1.ExecuteReader(System.Data.CommandBehavior.CloseConnection)

                Do While read1.Read()
                    Dim NewItem As New ListItem
                    NewItem.Text = sqlInj_out(read1.GetString(0)).ToUpper()
                    NewItem.Value = sqlInj_out(read1.GetString(1))
                    e.Items.Add(NewItem)
                Loop
                read1.Close()
                cmd1.Dispose()

            Catch ex As Exception

            Finally
                conn.Dispose()
            End Try
        End Sub

        Sub dropDownFill_string_reader2(ByRef sql As String, ByRef e As System.Web.UI.WebControls.DropDownList)
            Dim conn As New SqlConnection(conn_web)
            Try
                e.Items.Clear()

                Dim cmd1 As New SqlCommand(sql, conn)

                If conn.State = Data.ConnectionState.Open Then
                    conn.Dispose()
                End If
                conn.Open()

                Dim read1 As SqlDataReader = cmd1.ExecuteReader(System.Data.CommandBehavior.CloseConnection)

                Do While read1.Read()
                    Dim NewItem As New ListItem
                    NewItem.Text = sqlInj_out(read1.GetString(0)).ToUpper()
                    NewItem.Value = sqlInj_out(read1.GetString(1))
                    e.Items.Add(NewItem)
                Loop
                read1.Close()
                cmd1.Dispose()

            Catch ex As Exception

            Finally
                conn.Dispose()
            End Try
        End Sub

        Sub dropDownFill_string_reader3(ByRef sql As String, ByRef e As System.Web.UI.WebControls.DropDownList)
            Dim conn As New SqlConnection(conn_web)
            Try
                e.Items.Clear()

                Dim fisrtItem As New ListItem("-- Select --", "0")
                e.Items.Add(fisrtItem)
                Dim cmd1 As New SqlCommand(sql, conn)

                If conn.State = Data.ConnectionState.Open Then
                    conn.Dispose()
                End If
                conn.Open()

                Dim read1 As SqlDataReader = cmd1.ExecuteReader(System.Data.CommandBehavior.CloseConnection)

                Do While read1.Read()
                    Dim NewItem As New ListItem
                    NewItem.Text = sqlInj_out(read1.GetString(0))
                    NewItem.Value = sqlInj_out(read1.GetString(1))
                    e.Items.Add(NewItem)
                Loop
                read1.Close()
                cmd1.Dispose()

            Catch ex As Exception

            Finally
                conn.Dispose()
            End Try
        End Sub

        Sub dropDownFill_string_reader4(ByRef sql As String, ByRef e As System.Web.UI.WebControls.DropDownList)
            Dim conn As New SqlConnection(conn_web)
            Try
                e.Items.Clear()
                Dim i As Integer
                i = 1
                Dim fisrtItem As New ListItem("-- Select --", "0")
                e.Items.Add(fisrtItem)
                Dim cmd1 As New SqlCommand(sql, conn)

                If conn.State = Data.ConnectionState.Open Then
                    conn.Dispose()
                End If
                conn.Open()

                Dim read1 As SqlDataReader = cmd1.ExecuteReader(System.Data.CommandBehavior.CloseConnection)

                Do While read1.Read()
                    Dim NewItem As New ListItem
                    NewItem.Text = sqlInj_out(read1.GetString(0))
                    NewItem.Value = i
                    e.Items.Add(NewItem)
                    i = i + 1
                Loop
                read1.Close()
                cmd1.Dispose()

            Catch ex As Exception

            Finally
                conn.Dispose()
            End Try
        End Sub

        Sub dropDownFill_string_reader5(ByRef sql As String, ByRef e As System.Web.UI.WebControls.DropDownList)
            Dim conn As New SqlConnection(conn_web)
            Try
                e.Items.Clear()

                'Dim fisrtItem As New ListItem("-- Select --", "0")
                'e.Items.Add(fisrtItem)
                Dim cmd1 As New SqlCommand(sql, conn)

                If conn.State = Data.ConnectionState.Open Then
                    conn.Dispose()
                End If
                conn.Open()

                Dim read1 As SqlDataReader = cmd1.ExecuteReader(System.Data.CommandBehavior.CloseConnection)

                Do While read1.Read()
                    Dim NewItem As New ListItem
                    If (sqlInj_out(read1.GetString(0)) <> "") Then
                        NewItem.Text = sqlInj_out(read1.GetString(0)).ToUpper()
                    Else
                        Continue Do
                    End If
                    NewItem.Value = sqlInj_out(read1.GetString(1))
                    e.Items.Add(NewItem)
                Loop
                read1.Close()
                cmd1.Dispose()
                e.Items.Add(New ListItem("Others", ""))

            Catch ex As Exception

            Finally
                conn.Dispose()
            End Try
        End Sub

#Region "Signature load"
        Public Sub signatureload(imgw1_sign As Image, App_Auto_Cd As String, id As String)
            Dim user_info As New Marreg_Loginfo
            Try
                imgw1_sign.ImageUrl = "Show_Image.ashx?cd=" & App_Auto_Cd & "&id=" & id                'id = "11"
            Catch ex As Exception
            End Try
        End Sub
#End Region

#Region "GridView Load"
        Public Sub GridViewLoad(ByVal sql As String, ByRef e As System.Web.UI.WebControls.GridView)
            Dim conn As New SqlConnection(conn_web)
            Try
                conn.Open()
                Dim objCmd As New SqlDataAdapter(sql, conn)
                Dim oDS As New DataSet
                objCmd.Fill(oDS, "TAB")
                Dim dt As DataTable = oDS.Tables(0)
                Dim rowCount As Integer = dt.Rows.Count
                Dim columnCount As Integer = dt.Columns.Count
                Dim dr As DataRow = dt.NewRow()
                If rowCount = 0 Then
                    e.DataSource = Nothing
                    e.DataBind()
                    e.Focus()
                Else
                    Dim i, j As Integer
                    For i = 0 To rowCount - 1
                        For j = 0 To columnCount - 1
                            If oDS.Tables(0).Rows(i)(j) IsNot DBNull.Value Then
                                oDS.Tables(0).Rows(i)(j) = sqlInj_out(oDS.Tables(0).Rows(i)(j).ToString)
                            End If
                        Next
                    Next
                    e.DataSource = dt
                    e.DataBind()
                End If
                conn.Dispose()
            Catch ex As Exception

            Finally
                conn.Dispose()
            End Try
        End Sub
#End Region

#Region "ListBox Selection"
        Public Sub lst_selecn(ByVal sql As String, ByVal a As ListBox)
            Dim conn As New SqlConnection(conn_web)
            Try
                Dim cmd1 As New SqlCommand(sql, conn)

                If conn.State = Data.ConnectionState.Open Then
                    conn.Dispose()
                End If
                conn.Open()

                Dim read1 As SqlDataReader = cmd1.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                a.Items.Clear()
                Do While read1.Read()
                    Dim NewItem As New ListItem
                    NewItem.Text = sqlInj_out(read1.GetString(0).ToString)
                    NewItem.Value = sqlInj_out(read1.GetString(1).ToString)
                    a.Items.Add(NewItem)
                Loop
                read1.Close()
                cmd1.Dispose()

            Catch ex As Exception

            Finally
                conn.Dispose()
            End Try
        End Sub
#End Region

#Region "SQL Injection"
        Public Function sqlInj_in(ByVal cleanvar As String) As String
            'Try
            Return sql_in_out.sqlInj_in(cleanvar)
            'Catch
            'End Try
        End Function

        Function sqlInj_out(ByVal cleanvar As String) As String
            'Try
            Return sql_in_out.sqlInj_out(cleanvar)
            'Catch
            'End Try
        End Function
#End Region

#Region "Any Special character check in TextBox"
        Sub check_specialSymb(ByRef e As System.Web.UI.WebControls.TextBox)
            Try
                Dim i, j, l As Integer
                ' !@#$%'^*~,<>?;+=`
                Dim specialSymb = ("`") + ("~") + ("!") + ("%") + ("+") + ("'") + ("?") + (">") + ("<") + ("=")
                j = 0
                l = specialSymb.ToString.Length
                If e.Text.Length <> 0 Then
                    While (j < e.Text.Length)
                        For i = 0 To l - 1
                            If (e.Text.Chars(j) = specialSymb(i)) Then
                                e.Text = ""
                                Exit Sub
                            End If
                        Next
                        j = j + 1
                    End While
                End If
                If e.Text = "&nbsp;" Then
                    e.Text = ""
                End If
            Catch ex As Exception
            End Try
        End Sub
#End Region

#Region "Page Visit History By User Record Are stored"
        Sub page_verify(ByVal url As String, ByRef Row_Count As Integer, ByVal Login_Status As String, ByVal role As String)
            Dim conn As New SqlConnection(conn_web)
            Try
                If url Is Nothing Then
                    Row_Count = 0
                    Exit Sub
                End If

                Dim i As Integer
                i = url.ToString.Length
                Dim page_nm As String
                page_nm = ""
                While i > 0
                    If url.ToString.Substring(i - 1, 1) = "/" Then
                        page_nm = url.ToString.Substring(i)
                        Exit While
                    End If
                    i = i - 1
                End While
                If page_nm.ToString = "MARREG_Home.aspx" Or page_nm.ToString = "MARREG_Change_Password.aspx" Then
                    Row_Count = 1
                    Exit Sub
                End If

                conn.Open()
                Dim myTrans As SqlTransaction = conn.BeginTransaction()
                Try
                    Dim myCommand As New SqlCommand("MARREG_User_Visit_History_Details", conn, myTrans)
                    myCommand.CommandType = CommandType.StoredProcedure

                    Dim command_page_nm As New SqlParameter
                    command_page_nm.ParameterName = "@page_nm"
                    command_page_nm.SqlDbType = SqlDbType.NVarChar
                    command_page_nm.Size = 100
                    command_page_nm.Direction = ParameterDirection.Input
                    command_page_nm.Value = page_nm.ToString
                    myCommand.Parameters.Add(command_page_nm)

                    Dim command_role_nm As New SqlParameter
                    command_role_nm.ParameterName = "@role_nm"
                    command_role_nm.SqlDbType = SqlDbType.NVarChar
                    command_role_nm.Size = 100
                    command_role_nm.Direction = ParameterDirection.Input
                    command_role_nm.Value = role.ToString
                    myCommand.Parameters.Add(command_role_nm)

                    Dim command_id As New SqlParameter
                    command_id.ParameterName = "@id"
                    command_id.SqlDbType = SqlDbType.NVarChar
                    command_id.Size = 100
                    command_id.Direction = ParameterDirection.Input
                    command_id.Value = Login_Status.ToString
                    myCommand.Parameters.Add(command_id)

                    Dim command_para_login_info As New SqlParameter
                    command_para_login_info.ParameterName = "@row_count"
                    command_para_login_info.SqlDbType = SqlDbType.NVarChar
                    command_para_login_info.Size = 100
                    command_para_login_info.Direction = ParameterDirection.Output
                    myCommand.Parameters.Add(command_para_login_info)

                    myCommand.ExecuteNonQuery()
                    myTrans.Commit()

                    Row_Count = myCommand.Parameters("@row_count").Value

                    myCommand.Dispose()

                Catch ex As Exception
                    myTrans.Rollback()
                    Row_Count = 0
                Finally
                    conn.Dispose()
                End Try
            Catch ex As Exception

            End Try
        End Sub
#End Region

#Region "Number Check in Textbox"
        Public Function number_ck(ByVal text As System.Web.UI.WebControls.TextBox) As Boolean
            Dim regex As Regex = New Regex("^[-+]?\d*\.?\d*$")
            Return regex.IsMatch(text.Text)
        End Function
#End Region

#Region "Date Field Check"
        Public Function date_ck(ByVal text As System.Web.UI.WebControls.TextBox) As Boolean
            Dim regex As Regex = New Regex("(((0[1-9]|[12][0-9]|3[01])([/])(0[13578]|10|12)([/])(\d{4}))|(([0][1-9]|[12][0-9]|30)([/])(0[469]|11)([/])(\d{4}))|((0[1-9]|1[0-9]|2[0-8])([/])(02)([/])(\d{4}))|((29)(\.|-|\/)(02)([/])([02468][048]00))|((29)([/])(02)([/])([13579][26]00))|((29)([/])(02)([/])([0-9][0-9][0][48]))|((29)([/])(02)([/])([0-9][0-9][2468][048]))|((29)([/])(02)([/])([0-9][0-9][13579][26])))")
            Return regex.IsMatch(text.Text)
        End Function
#End Region

#Region "Email Field Check"
        Public Function email_ck(ByVal text As System.Web.UI.WebControls.TextBox) As Boolean
            Dim regex As Regex = New Regex("\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*")
            Return regex.IsMatch(text.Text)
        End Function
#End Region

#Region "Password Hashing"
        Public Function CreatePasswordHash(ByVal pwd As String) As String
            Dim hashMethod = "SHA256"
            Dim _algorithm As HashAlgorithm
            Select Case hashMethod.ToUpper()
                Case "MD5"
                    _algorithm = MD5.Create()
                    Exit Select
                Case "SHA1"
                    _algorithm = SHA1.Create()
                    Exit Select
                Case "SHA256"
                    _algorithm = SHA256.Create()
                    Exit Select
                Case "SHA512"
                    _algorithm = SHA512.Create()
                    Exit Select
                Case Else
                    Throw New ArgumentException("Invalid password format.", "passwordFormat")
            End Select
            Dim bytes As Byte() = Encoding.UTF8.GetBytes(pwd)
            Dim hash As Byte() = _algorithm.ComputeHash(bytes)
            Dim hashString As String = String.Empty
            For Each x As Byte In hash
                hashString += [String].Format("{0:x2}", x)
            Next
            Return hashString.ToUpperInvariant()

        End Function
       
#End Region

#Region "Find Out the Role type CD"
        Sub role_type_code(ByVal role_type As String, ByRef role_type_cd As String)
            Dim cnn As New SqlConnection(conn_web)
            Try
                cnn.Open()
                Dim sql As String
                sql = " select role_type_cd from " & _
                      " aspnet_Roles a,MARREG_Role_Type_Info b" & _
                      " where a.roleid=b.RoleId " & _
                      " and a.RoleName='" + Trim(role_type.ToString) + "'"
                Dim rs As New SqlDataAdapter(sql, cnn)
                Dim ds As New DataSet
                rs.Fill(ds, "d")
                role_type_cd = ds.Tables(0).Rows(0).Item("role_type_cd").ToString
                rs.Dispose()
                ds.Dispose()
            Catch ex As Exception
            Finally
                cnn.Dispose()
            End Try
        End Sub
#End Region

#Region "Check Retired"
        Public Function Retired(ByVal mo_cd As String) As String
            Dim cnn As New SqlConnection(conn_web)
            Dim ret_date As Date
            Try
                cnn.Open()
                Dim sql As String
                sql = " select CONVERT(nvarchar,DATEADD(DD, -1, DATEADD(YEAR, 68, modtl.DOB)),105) ret_dt " & _
                      " from MARREG_MO_Otherinfo_Details modtl " & _
                      " where MO_Autoincrmnt_Cd='" + mo_cd + "' "
                '" and a.RoleName='" + Trim(role_type.ToString) + "'"
                '-------------------------------------------------
                Dim dt As DataTable
                dt = selectdata(sql)
                If dt.Rows.Count > 0 Then
                    ret_date = dt.Rows(0)("ret_dt").ToString()
                Else
                    ret_date = "13-11-2101"
                End If
                '--------------------------------------------------
                'Dim rs As New SqlDataAdapter(sql, cnn)
                'Dim ds As New DataSet
                'rs.Fill(ds, "d")
                'ret_date = ds.Tables(0).Rows(0).Item("ret_dt").ToString
                'rs.Dispose()
                'ds.Dispose()
            Catch ex As Exception
            Finally
                cnn.Dispose()
            End Try
            Return ret_date
        End Function
#End Region

#Region "Datatatable select"
        Public Function selectdata(query As String) As DataTable
            Dim dt1 As New DataTable()
            Dim con1 As New SqlConnection(conn_web)
            Try
                con1.Open()
                Dim da As New SqlDataAdapter(query, con1)
                da.Fill(dt1)
            Catch
            Finally
                con1.Close()
                con1.Dispose()
            End Try
            Return dt1
        End Function
#End Region

#Region "Dataset select"
        Public Function selectdataset(query As String) As DataSet
            Dim ds As New DataSet()
            Dim con1 As New SqlConnection(conn_web)

            Try
                con1.Open()
                Dim da As New SqlDataAdapter(query, con1)
                da.Fill(ds)
            Catch ex As Exception
                ' Optional: log exception
                ' Throw ex
            Finally
                con1.Close()
                con1.Dispose()
            End Try

            Return ds
        End Function
#End Region


#Region "Insert and delete data"
        Public Sub insert_delete(query As String)
            Dim con2 As New SqlConnection(conn_web)
            Try
                con2.Open()
                Dim com As New SqlCommand(query, con2)
                com.ExecuteNonQuery()
            Catch
            Finally
                con2.Close()
                con2.Dispose()
            End Try
        End Sub
#End Region

#Region "Load Radio Buttin List"
        Public Sub Radiobuttonlist_load(ByVal sql As String, ByVal a As RadioButtonList, ByVal datatextfild As String, ByVal dataveluefild As String)
            Dim conn As New SqlConnection(conn_web)
            Try
                a.Items.Clear()

                Dim ds As DataSet = New DataSet()
                If conn.State = Data.ConnectionState.Open Then
                    conn.Dispose()
                End If
                conn.Open()
                Dim sqldtadptr As SqlDataAdapter = New SqlDataAdapter(sql, conn)
                sqldtadptr.Fill(ds)
                a.DataSource = ds.Tables(0)
                a.DataTextField = datatextfild
                a.DataValueField = dataveluefild

                a.DataBind()
                ds.Dispose()
                sqldtadptr.Dispose()
            Catch ex As Exception

            Finally
                conn.Dispose()
            End Try
        End Sub
#End Region

#Region "SMS Sent"
        Public Function sendsms(msg As String, mobileno As String) As String

            Dim sURL As String = String.Empty
            sURL = "http://smsgw.nic.in/sendsms_nic/sendmsg.php?uname="
            sURL += ConfigurationManager.AppSettings.Item("sms_webpage_user")
            sURL += "&pass="
            sURL += ConfigurationManager.AppSettings.Item("sms_webpage_pwd")
            sURL += "&senderid="
            sURL += "prk "
            sURL += "&message="
            sURL += msg
            sURL += "&dest_mobileno="
            sURL += mobileno
            sURL += "&response=Y"
            '"dastak&passwd=dastak123&mobilenumber=919830106106&message=Welcome To DASTAK TRADECOM (INDIA) PVT. LTD. Your Id :: 123 and Password :: 123 &sid=Dastak&mtype=N"
            Dim request As HttpWebRequest = WebRequest.Create(sURL.ToString())
            'request.Method = "POST"
            Dim response As HttpWebResponse = request.GetResponse()
            Dim reader As StreamReader = New StreamReader(response.GetResponseStream())
            Dim result As String = reader.ReadToEnd()
            Return result
        End Function
#End Region

#Region "Mail Send"
        Protected Sub sendmail(from_id As String, to_id As String, password As String, attachfilepath As String)

            Dim mail As MailMessage = New MailMessage(from_id, to_id)
            mail.Subject = "hi"
            mail.Body = "this is me"
            If attachfilepath <> "" Then
                Dim attachFile As Attachment = New Attachment(attachfilepath)
                mail.Attachments.Add(attachFile)
            End If

            Dim smtp As SmtpClient = New SmtpClient()
            smtp.Host = "smtp.gmail.com"
            smtp.Port = 587
            smtp.UseDefaultCredentials = False
            smtp.Credentials = New System.Net.NetworkCredential(from_id, password)
            smtp.EnableSsl = True
            smtp.Send(mail)
        End Sub
#End Region

#Region "Get OTP"
        Public Function GetHotp(base32EncodedSecret As Byte(), counter As Long) As String
            Dim message As Byte() = BitConverter.GetBytes(counter).Reverse().ToArray()
            Dim secret As Byte() = base32EncodedSecret
            Dim hmac As New HMACSHA1(secret, True)
            Dim hash As Byte() = hmac.ComputeHash(message)
            Dim offset As Integer = hash(hash.Length - 1) And &HF
            Dim truncatedHash As Integer = ((hash(offset) And &H7F) << 24) Or ((hash(offset + 1) And &HFF) << 16) Or ((hash(offset + 2) And &HFF) << 8) Or (hash(offset + 3) And &HFF)
            Dim hotp As Integer = truncatedHash Mod 100000000
            Return hotp.ToString().PadLeft(8, "0"c)
        End Function

        Public Function GetTotp(base32EncodedSecret As Byte()) As String
            Dim epochStart As New DateTime(1970, 1, 1, 0, 0, 0, _
                0, DateTimeKind.Utc)
            Dim counter As Long = CLng(Math.Floor((DateTime.UtcNow - epochStart).TotalSeconds / 30))
            Return GetHotp(base32EncodedSecret, counter)
        End Function
#End Region

#Region "Check GRN"
        Public Function checkGrn(ByVal grnno As String) As String
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            Dim HEAD_OF_ACCOUNT As String = "0070-60-108-001-12"
            Dim grn As String
            Dim hoa1 As String = ""
            grn = ("https://wbifms.gov.in/GRIPS//ChallanDetails/query.do?GRN_NO=" + grnno + "")
            Dim xmlDoc As XmlDocument = New XmlDocument
            xmlDoc.Load(grn)
            Dim nodeList As XmlNodeList = xmlDoc.DocumentElement.SelectNodes("/GRIPS_EPAYMENT")
            Dim nodeListChallan As XmlNodeList = xmlDoc.DocumentElement.SelectNodes("/GRIPS_EPAYMENT/CHALLAN_DETAILS")

            For Each node As XmlNode In nodeListChallan
                Try
                    Dim AMOUNT1 As String = node.SelectSingleNode("AMOUNT1").InnerText     'ViewState("HEAD_OF_ACCOUNT1")
                    hoa1 = node.SelectSingleNode("HEAD_OF_ACCOUNT1").InnerText
                Catch ex As Exception

                End Try
            Next
            If HEAD_OF_ACCOUNT = hoa1 Then
                Return "MARREG HEAD"
            Else
                Return "NOT MARREG HEAD"
            End If
        End Function
#End Region
#Region "QR code generation"
        'Public Function qrcode(app_no As String) As Byte()
        '    Dim act = "SELECT act.act_id act_id,act.act_nm act_nm from MARREG_Act act,MARREG_NewApp_OtherInfo_temp othr where act.act_id = othr.Act_cd And othr.App_Auto_Cd = '" & app_no & "'"
        '    Dim dtn As DataTable = selectdata(act)
        '    Dim act_id As String = dtn.Rows(0)("act_id").ToString()
        '    Dim act_nm As String = dtn.Rows(0)("act_nm").ToString()
        '    Dim regSolmn_dt As String
        '    If act_id = "01" Then       '"The Hindu Marriage Act, 1955" Then
        '        regSolmn_dt = "Reg"
        '    ElseIf act_id = "03" Then   '"The Special Marriage Act, 1954 (Under Section 16)" Then
        '        act_nm = "The Special Marriage Act,1954(Section 16)"
        '        regSolmn_dt = "Reg"
        '    ElseIf act_id = "05" Then   '"The Parsi Marriage & Divorce Act, 1936" Then
        '        regSolmn_dt = "Reg"
        '    ElseIf act_id = "02" Then   '"The Parsi Marriage & Divorce Act, 1936" Then
        '        act_nm = "The Special Marriage Act,1954(Section 13)"
        '        regSolmn_dt = "Slmn"
        '    Else
        '        regSolmn_dt = "Slmn"
        '    End If
        '    Dim sqlquery = "select ('" & act_nm & "'+'.'+'Bride-'+UPPER(bride .Bride_FNam +' '+bride.Bride_LNam )+','+'Groom-'+UPPER(groom .Groom_FNam +' '+groom.Groom_LNam)+'.'+'" & regSolmn_dt & "'+'Dt-'+convert(nvarchar,newapp_sts.Registration_Date,105)+'.MO Name-'+upper(mo.MO_FName+' '+(case mo.MO_MName when NULL then '' when '' then '' else  mo.MO_MName + ' ' END)+mo.MO_LName + '.'))name" & _
        '             " from MARREG_NewApp_Bride_temp bride,MARREG_NewApp_Groom_temp groom ,MARREG_NewApp_OtherInfo_temp othr,MARREG_Act act, MARREG_New_applicant_status newapp_sts, MARREG_MO_Details mo" & _
        '             " where act.act_id = othr.Act_cd And othr.App_Auto_Cd = groom.App_Auto_Cd And othr.App_Auto_Cd = bride.App_Auto_Cd And newapp_sts.App_Auto_Cd=othr.App_Auto_Cd And othr.App_Auto_Cd = '" & app_no & "' and othr.MO_Autoincrement_cd=mo.MO_Autoincrmnt_Cd"

        '    Dim dt As DataTable = selectdata(sqlquery)
        '    Dim encoding = dt.Rows(0)("name").ToString()

        '    Dim QRCodeEncoder As ThoughtWorks.QRCode.Codec.QRCodeEncoder = New ThoughtWorks.QRCode.Codec.QRCodeEncoder()
        '    QRCodeEncoder.QRCodeEncodeMode = ThoughtWorks.QRCode.Codec.QRCodeEncoder.ENCODE_MODE.BYTE
        '    QRCodeEncoder.QRCodeScale = Len(encoding)
        '    QRCodeEncoder.QRCodeVersion = 0
        '    Dim photo As System.Drawing.Bitmap = QRCodeEncoder.Encode(encoding)
        '    Dim p As MemoryStream = ResizeImage(photo, 132, 170)

        '    Dim imgbyte As Byte() = p.ToArray()
        '    'Dim base64String As String = Convert.ToBase64String(imgbyte)
        '    Return imgbyte
        'End Function


        Public Function qrcode(app_no As String) As Byte()
            Dim en As New MARREG_SHA256()
            Dim encdata = en.Encryption(app_no)
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
            Dim bitmap As Drawing.Bitmap = barcodeWriter.Write(baseUrl() + "MARREG_Portal/MARREG_Certificate_Verification.aspx?a=" + HttpContext.Current.Server.UrlEncode(encdata) + "&cs=" + cs)
            Dim logo = New Drawing.Bitmap(HttpContext.Current.Server.MapPath("~/MARREG_Images/Portal/logo.png"))
            Dim g As Drawing.Graphics = Drawing.Graphics.FromImage(bitmap)
            g.DrawImage(logo, New Drawing.Point((bitmap.Width - logo.Width) / 2, (bitmap.Height - logo.Height) / 2))
            Dim p As MemoryStream = ResizeImage(bitmap, 170, 170)

            Dim imgbyte As Byte() = p.ToArray()
            Return imgbyte
        End Function

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

            Dim mStream As New MemoryStream()
            bmDest.Save(mStream, System.Drawing.Imaging.ImageFormat.Bmp)
            Return mStream
        End Function

#End Region
#End Region
#Region "CheckListBox Bind"
        Public Sub CheckListBox_Bind(ByVal sql As String, ByVal a As CheckBoxList)
            Dim conn As New SqlConnection(conn_web)
            Try
                Dim cmd1 As New SqlCommand(sql, conn)

                If conn.State = Data.ConnectionState.Open Then
                    conn.Dispose()
                End If
                conn.Open()

                Dim read1 As SqlDataReader = cmd1.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                a.Items.Clear()
                Do While read1.Read()
                    Dim NewItem As New ListItem
                    NewItem.Text = sqlInj_out(read1.GetString(0).ToString)
                    NewItem.Value = sqlInj_out(read1.GetString(1).ToString)
                    a.Items.Add(NewItem)
                Loop
                read1.Close()
                cmd1.Dispose()

            Catch ex As Exception

            Finally
                conn.Dispose()
            End Try
        End Sub
#End Region
#Region "Checkbox"             'added 16.08.2018 anay
        Sub chkbox_fill(ByRef sql As String, ByRef e As System.Web.UI.WebControls.CheckBox)
            Dim conn As New SqlConnection(conn_web)
            Try
                e.Controls.Clear()
                Dim cmd1 As New SqlCommand(sql, conn)
                If conn.State = Data.ConnectionState.Open Then
                    conn.Dispose()
                End If
                conn.Open()
                Dim read1 As SqlDataReader = cmd1.ExecuteReader(System.Data.CommandBehavior.CloseConnection)

                'Do While read1.Read()
                '    Dim NewItem As New CheckBoxList
                '    NewItem.Text = sqlInj_out(read1.GetString(0))
                '    'NewItem.Value = sqlInj_out(read1.GetString(1))
                '    e.Controls.Add(NewItem)
                'Loop
                read1.Close()
                cmd1.Dispose()
            Catch
            Finally
                conn.Dispose()
            End Try
        End Sub
#End Region

#Region "Unused Method"
        Sub chk_fill_string(ByRef sql As String, ByRef e As System.Web.UI.WebControls.CheckBoxList)
            Dim conn As New SqlConnection(conn_web)
            Try
                e.Items.Clear()
                Dim cmd1 As New SqlCommand(sql, conn)
                If conn.State = Data.ConnectionState.Open Then
                    conn.Dispose()
                End If
                conn.Open()
                Dim read1 As SqlDataReader = cmd1.ExecuteReader(System.Data.CommandBehavior.CloseConnection)

                Do While read1.Read()
                    Dim NewItem As New ListItem
                    NewItem.Text = sqlInj_out(read1.GetString(0))
                    NewItem.Value = sqlInj_out(read1.GetString(1))
                    e.Items.Add(NewItem)
                Loop
                read1.Close()
                cmd1.Dispose()
            Catch
            Finally
                conn.Dispose()
            End Try
        End Sub

        Public Sub ddl_selecn(ByVal sql As String, ByVal a As DropDownList)
            Dim conn As New SqlConnection(conn_web)
            Try
                a.Items.Clear()
                a.Items.Add("Select")

                Dim cmd1 As New SqlCommand(sql, conn)

                If conn.State = Data.ConnectionState.Open Then
                    conn.Dispose()
                End If
                conn.Open()

                Dim read1 As SqlDataReader = cmd1.ExecuteReader(System.Data.CommandBehavior.CloseConnection)

                Do While read1.Read()
                    Dim NewItem As New ListItem
                    NewItem.Text = sqlInj_out(read1.GetString(0).ToString)
                    a.Items.Add(NewItem)
                Loop
                read1.Close()
                cmd1.Dispose()

            Catch ex As Exception

            Finally
                conn.Dispose()
            End Try
        End Sub



        Public Sub row_count(ByVal sql As String, ByRef a As Integer)
            Dim conn As New SqlConnection(conn_web)
            Try
                Dim cmd1 As New SqlCommand(sql, conn)

                If conn.State = Data.ConnectionState.Open Then
                    conn.Dispose()
                End If
                conn.Open()
                a = 0
                Dim read1 As SqlDataReader = cmd1.ExecuteReader(System.Data.CommandBehavior.CloseConnection)

                Do While read1.Read()
                    a = a + 1
                Loop
                read1.Close()
                cmd1.Dispose()

            Catch ex As Exception

            Finally
                conn.Dispose()
            End Try
        End Sub

        Public Sub text_selecn(ByVal sql As String, ByRef a As TextBox)
            Dim conn As New SqlConnection(conn_web)
            Try
                Dim cmd1 As New SqlCommand(sql, conn)

                If conn.State = Data.ConnectionState.Open Then
                    conn.Dispose()
                End If
                conn.Open()

                Dim read1 As SqlDataReader = cmd1.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                a.Text = ""
                Do While read1.Read()
                    Dim NewItem As New ListItem
                    NewItem.Text = sqlInj_out(read1.GetString(0).ToString)
                    a.Text = NewItem.Text
                Loop
                read1.Close()
                cmd1.Dispose()

            Catch ex As Exception

            Finally
                conn.Dispose()
            End Try
        End Sub

        Public Sub lbl_selecn(ByVal sql As String, ByRef a As Label)
            Dim conn As New SqlConnection(conn_web)
            Try
                Dim cmd1 As New SqlCommand(sql, conn)

                If conn.State = Data.ConnectionState.Open Then
                    conn.Dispose()
                End If
                conn.Open()

                Dim read1 As SqlDataReader = cmd1.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                a.Text = ""
                Do While read1.Read()
                    Dim NewItem As New ListItem
                    NewItem.Text = sqlInj_out(read1.GetString(0).ToString)
                    a.Text = NewItem.Text
                Loop
                read1.Close()
                cmd1.Dispose()

            Catch ex As Exception

            Finally
                conn.Dispose()
            End Try
        End Sub

        Public Sub array_selecn(ByVal sql As String, ByRef a As ArrayList)
            Dim conn As New SqlConnection(conn_web)
            Try
                Dim cmd1 As New SqlCommand(sql, conn)

                If conn.State = Data.ConnectionState.Open Then
                    conn.Dispose()
                End If
                conn.Open()

                Dim read1 As SqlDataReader = cmd1.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                a.Clear()
                Do While read1.Read()
                    Dim NewItem As New ListItem
                    NewItem.Text = sqlInj_out(read1.GetString(0).ToString)
                    a.Add(NewItem)
                Loop
                read1.Close()
                cmd1.Dispose()

            Catch ex As Exception

            Finally
                conn.Dispose()
            End Try
        End Sub

        Public Sub var_selecn(ByVal sql As String, ByRef a As String)
            Dim conn As New SqlConnection(conn_web)
            Try
                Dim cmd1 As New SqlCommand(sql, conn)

                If conn.State = Data.ConnectionState.Open Then
                    conn.Dispose()
                End If
                conn.Open()
                a = ""
                Dim read1 As SqlDataReader = cmd1.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                Do While read1.Read()
                    Dim NewItem As New ListItem
                    NewItem.Text = sqlInj_out(read1.GetString(0).ToString)
                    a = NewItem.Text
                Loop
                read1.Close()
                cmd1.Dispose()

            Catch ex As Exception

            Finally
                conn.Dispose()
            End Try
        End Sub

        Sub dropDownFill_unique_reader(ByRef sql As String, ByRef e As System.Web.UI.WebControls.DropDownList)
            Dim conn As New SqlConnection(conn_web)
            Try
                e.Items.Clear()
                Dim fisrtItem As New ListItem("-- Select --", "0")
                e.Items.Add(fisrtItem)
                Dim cmd1 As New SqlCommand(sql, conn)

                If conn.State = Data.ConnectionState.Open Then
                    conn.Dispose()
                End If
                conn.Open()

                Dim read1 As SqlDataReader = cmd1.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                Do While read1.Read()

                    Dim NewItem As New ListItem
                    NewItem.Text = sqlInj_out(read1.GetString(0).ToString).ToUpper()
                    NewItem.Value = sqlInj_out(read1.GetGuid(1).ToString)
                    e.Items.Add(NewItem)
                Loop
                read1.Close()
                cmd1.Dispose()

            Catch ex As Exception

            Finally
                conn.Dispose()
            End Try
        End Sub

        Sub html_dropDownFill_string_reader(ByRef sql As String, ByRef e As System.Web.UI.HtmlControls.HtmlSelect)
            Dim conn As New SqlConnection(conn_web)
            Try
                e.Items.Clear()
                Dim fisrtItem As New ListItem("-- Select --", "0")
                e.Items.Add(fisrtItem)

                Dim cmd1 As New SqlCommand(sql, conn)

                If conn.State = Data.ConnectionState.Open Then
                    conn.Dispose()
                End If
                conn.Open()

                Dim read1 As SqlDataReader = cmd1.ExecuteReader(System.Data.CommandBehavior.CloseConnection)

                Do While read1.Read()
                    Dim NewItem As New ListItem
                    NewItem.Text = sqlInj_out(read1.GetString(0)).ToUpper()
                    NewItem.Value = sqlInj_out(read1.GetString(1))
                    e.Items.Add(NewItem)
                Loop
                read1.Close()
                cmd1.Dispose()

            Catch ex As Exception

            Finally
                conn.Dispose()
            End Try
        End Sub

        Sub lstFill_string_reader(ByRef sql As String, ByRef e As System.Web.UI.WebControls.ListBox)
            Dim conn As New SqlConnection(conn_web)
            Try
                e.Items.Clear()

                Dim cmd1 As New SqlCommand(sql, conn)

                If conn.State = Data.ConnectionState.Open Then
                    conn.Dispose()
                End If
                conn.Open()

                Dim read1 As SqlDataReader = cmd1.ExecuteReader(System.Data.CommandBehavior.CloseConnection)

                Do While read1.Read()

                    Dim NewItem As New ListItem
                    NewItem.Text = sqlInj_out(read1.GetString(0))
                    NewItem.Value = sqlInj_out(read1.GetString(1))
                    e.Items.Add(NewItem)
                Loop
                read1.Close()
                cmd1.Dispose()

            Catch ex As Exception

            Finally
                conn.Dispose()
            End Try
        End Sub

        Public Sub DUPLICATE_CHK(ByVal sql As String, ByRef Row_Count As Integer)
            Dim conn As New SqlConnection(conn_web)
            Try
                Dim cmd1 As New SqlCommand(sql, conn)

                If conn.State = Data.ConnectionState.Open Then
                    conn.Dispose()
                End If
                conn.Open()
                Row_Count = 0
                Dim read1 As SqlDataReader = cmd1.ExecuteReader(System.Data.CommandBehavior.CloseConnection)

                Do While read1.Read()
                    Row_Count = Row_Count + 1
                Loop
                read1.Close()
                cmd1.Dispose()

            Catch ex As Exception

            Finally
                conn.Dispose()
            End Try
        End Sub

        Sub CHK_FILL(ByRef sql As String, ByRef e As System.Web.UI.WebControls.CheckBoxList)
            Dim cnn As New SqlConnection(conn_web)
            Try
                e.Items.Clear()
                Dim cmd1 As New SqlCommand(sql, cnn)
                If cnn.State = ConnectionState.Closed Then
                    cnn.Open()
                End If

                Dim read1 As SqlDataReader = cmd1.ExecuteReader(System.Data.CommandBehavior.CloseConnection)

                Do While read1.Read()
                    Dim NewItem As New ListItem
                    NewItem.Text = read1.GetString(0)
                    NewItem.Value = read1.GetGuid(1).ToString
                    e.Items.Add(NewItem)
                Loop
                cmd1.Dispose()
                read1.Close()
            Catch ex As Exception
            Finally
                cnn.Dispose()
            End Try
        End Sub

        Public Function validate_field(ByVal field As TextBox) As Boolean
            Try
                Dim flag As Integer = 0
                Dim arr As New ArrayList
                Dim txt_value = field.Text
                arr.Clear()
                Dim invalids = "!@#$%'^*~,<>?;+=`"
                For i As Integer = 0 To Len(txt_value) - 1
                    If InStr(1, invalids, Mid(txt_value, i + 1, 1)) > 0 Then
                        Dim tst As Integer = i + 1
                        arr.Add(tst)
                        flag = flag + 1
                        If i <> 0 And arr.Count >= 2 Then
                            If arr(flag - 1) = arr(flag - 2) + 1 Or arr(flag - 1) + 1 = arr(flag - 2) Then
                                Return True
                                Exit For
                            End If
                        End If

                    End If
                Next
            Catch ex As Exception

            End Try
        End Function

        Public Function text_validation(ByRef e As System.Web.UI.WebControls.TextBox) As String
            e.Text = e.Text.Trim()
            Dim regck As String
            Dim regck1 As String
            If String.IsNullOrEmpty(e.Text) Then
                Return ("Please Enter Input ")
                Exit Function
            End If
            'regck = "^\d*$"
            'If Regex.IsMatch(e.Text, regck) Then
            '    Return ("Only Numbers are not allowed")
            'End If
            If regck1 = "{45,95}" Then

            End If

            regck = "^[,-.()&\\/]{1,250}$"



            If Not Regex.IsMatch(e.Text, regck) Then
                Return ("Special Characters are not allowed ")
            End If

            Return "validate"
        End Function

#End Region
#Region "Update QR Code"
        Public Sub update_QR_code(qrcode As Byte(), app_auto_cd As String)
            Dim con2 As New SqlConnection(conn_web)
            Try
                con2.Open()
                Using cmd As SqlCommand = New SqlCommand("update MARREG_Certificate_New set qrcode=@qrcode where App_Auto_Cd=@App_Auto_Cd", con2)
                    cmd.Parameters.Add("@qrcode", SqlDbType.VarBinary, qrcode.Length).Value = qrcode
                    cmd.Parameters.Add("@App_Auto_Cd", SqlDbType.NVarChar, 20).Value = app_auto_cd
                    cmd.ExecuteNonQuery()
                End Using
            Catch
            Finally
                con2.Close()
                con2.Dispose()
            End Try
        End Sub
#End Region

    End Class
End Namespace
