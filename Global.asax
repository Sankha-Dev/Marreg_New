<%@ Application Language="VB" %>
<%@ Import Namespace="System.Data" %> 
<%@ Import Namespace="MARREG.App_Code.BusinessLogicLayer" %> 
<%@ Import Namespace ="System.Data.SqlClient" %> 
<%@ Import Namespace="MARREG.App_Code.DataAccessLayer" %> 

  
<script runat="server">
    Public logindl As New MARREG_Login_BL()
    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application startup

    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application shutdown
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when an unhandled error occurs
        ' Dim CurrentException As Exception = Server.GetLastError()
        ' Dim ErrorDetails As String = CurrentException.ToString()
        'If Response.StatusCode = 403 Then Response.StatusCode = 404
        'If InStr(ErrorDetails.ToLower, "A potentially dangerous Request.QueryString value was detected from the client".ToLower) > 0 Then
        ' Response.Redirect("http://wbdemo21.nic.in/MARREG_Error/Er404.aspx")
        'End If
        Dim ex As Exception = Me.Server.GetLastError().GetBaseException()
        CreateLog(ex)   'Added on 11-03-2024 for error logs
        FormsAuthentication.SignOut()

    End Sub
    'Added on 11-03-2024 for error logs
    Private Sub CreateLog(mex As Exception)
        Dim strFile = ""
        Dim strPath = Server.MapPath("~/MARREG_Logs/") + "Error/" + Date.Today.Year.ToString() + "/" + Date.Today.ToString("MMMM") + "/"
        If Not System.IO.Directory.Exists(strPath) Then
            System.IO.Directory.CreateDirectory(strPath)
        End If
        strFile = strPath + "error_logs_" + DateTime.Now.ToString("dd_MMM_yyyy") + ".log"
        Using sw As New System.IO.StreamWriter(System.IO.File.Open(strFile, IO.FileMode.OpenOrCreate))

        End Using

        Dim message As String = String.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"))
        message += Environment.NewLine
        message += "-----------------------------------------------------------"
        message += Environment.NewLine
        message += String.Format("Message: {0}", mex.Message)
        message += Environment.NewLine
        'message += String.Format("StackTrace: {0}", mex.StackTrace)
        'message += Environment.NewLine
        message += String.Format("Source: {0}", mex.Source)
        message += Environment.NewLine
        message += String.Format("TargetSite: {0}", mex.TargetSite.ToString())
        message += Environment.NewLine
        message += String.Format("IP: {0}", getIPAddress())
        message += Environment.NewLine
        message += "-----------------------------------------------------------"
        message += Environment.NewLine

        Using sw As New IO.StreamWriter(strFile, True)
            sw.WriteLine(message)
            sw.Flush()
            sw.Close()
        End Using
    End Sub
    'End Added on 11-03-2024 for error logs

    Sub Application_BeginRequest(sender As Object, e As EventArgs)
        HttpContext.Current.Response.AddHeader("x-frame-options", "SAMEORIGIN")
        'HttpContext.Current.Response.Headers.Remove("Server")

        'If HttpContext.Current.Request.Url.ToString().ToLower().Contains("http:") Then
        '    Response.Redirect(HttpContext.Current.Request.Url.ToString().ToLower().Replace("http:", "https:"))
        'End If
        If ConfigurationManager.AppSettings("MaintenanceMode") = "true" Then
            'If (Common.getIPAddress() <> "192.168.0.39") Then
            '    HttpContext.Current.RewritePath("~/MARREG_Portal/indexmaintenance.htm")
            'End If
            HttpContext.Current.RewritePath("~/MARREG_Portal/indexmaintenance.htm")
        End If

        'Added on 03-04-2024
        'Try
        '    Dim myCookie = Request.Cookies("ASP.NET_SessionId")
        '    myCookie.Path = "/MARREG_Portal"
        'Catch
        '    Dim myCookie = New HttpCookie("ASP.NET_SessionId")
        '    myCookie.Path = "/MARREG_Portal"
        'End Try
        'End Added on 03-04-2024

    End Sub



    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a new session is started
        'Dim myCookie = Request.Cookies("ASP.NET_SessionId")
        'myCookie.Path = "/MARREG_Portal"
        'Response.Cookies.Remove(myCookie)
        'Response.Cookies.Add(myCookie)

    End Sub
    Sub Session_OnEnd()
        logout("Browser Close")
    End Sub
    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        If Session("Marreg_user_info") IsNot Nothing Then
            logout("Session Expires")
        End If


        ' Code that runs when a session ends. 
        ' Note: The Session_End event is raised only when the sessionstate mode
        ' is set to InProc in the Web.config file. If session mode is set to StateServer 
        ' or SQLServer, the event is not raised.
    End Sub
#Region "Logout"
    Private Sub logout(ByVal status As String)
        Dim user_info As New Marreg_Loginfo
        user_info = Session("Marreg_user_info")
        Try
            'Dim pagenm As String
            'Dim url As String = Request.UrlReferrer.ToString
            'pagenm = ""
            'Dim i As Integer = url.ToString.Length
            'While i > 0
            '    If url.ToString.Substring(i - 1, 1) = "/" Then
            '        pagenm = url.ToString.Substring(i)
            '        Exit While
            '    End If
            '    i = i - 1
            'End While
            'Dim ipadd As String = System.Web.HttpContext.Current.Request.UserHostAddress

            'Dim time As DateTime = Convert.ToDateTime(Format(System.DateTime.Now, "hh:mm:ss"))

            Dim log_status As String = logindl.LogOutHistory(user_info.Login_Status, status, user_info.Ipaddress, "~/MARREG_Login/MARREG_Login.aspx")
            Session.Remove("Marreg_user_info")
            ' Response.Redirect("~/MARREG_Portal/MARREG_Home.aspx")
            Response.Redirect("http://wbdemo21.nic.in/MARREG_Portal/MARREG_Home.aspx")
        Catch ex As Exception

        Finally
        End Try
    End Sub

#End Region
</script>