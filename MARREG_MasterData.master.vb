Imports System.Data
Imports MARREG.App_Code.BusinessLogicLayer
Imports System.Data.SqlClient
Imports MARREG.App_Code.DataAccessLayer

Partial Class MARREG_MasterData
    Inherits System.Web.UI.MasterPage

#Region "Declaration"
    Dim role_id As String
    Public dbconnctn As New DataBaseHelperDB()
    Dim c As New Marreg_Class
    Private user_info As New Marreg_Loginfo
    Private logindl As New MARREG_Login_BL()
    Private u_info As Marreg_Loginfo
    'Shared user_info As New Marreg_Loginfo
#End Region

#Region "Page Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Response.AddHeader("pragma", "no-cache")
            Response.AddHeader("cache-control", "private")
            Response.CacheControl = "no-cache"
            Response.Cache.SetExpires(DateTime.Now.AddMinutes(-1))
            Response.Cache.SetCacheability(HttpCacheability.NoCache)
            Response.Cache.SetNoStore()
            If IsPostBack = False Then
                page_validation()

                PopulateRootLevel()
                '---------16.02.2018--------------------------------------
                If Session("ChangePassword") = "1" Then
                    menu_TreeView.Enabled = False
                Else
                    menu_TreeView.Enabled = True
                End If
                Session.Remove("ChangePassword")
                '----------------------------------------------------------
                'If (u_info.role_type_cd <> "02" And u_info.MO_cd <> "1") Then   ' Leave for SA type user
                '    If (Page.Request.Url.AbsoluteUri <> baseUrl() & "MARREG_Admin/MARREG_Change_Password.aspx") Then
                '        check_pass_change_date()
                '    End If
                'End If
            End If
        Catch ex As Exception

        End Try
    End Sub
#End Region
    Private Sub check_pass_change_date()
        Dim username = c.sqlInj_out(u_info.user_nm)
        Dim sql = "select DATEDIFF(day, LastPasswordChangedDate, getdate()) day,LastPasswordChangedDate from aspnet_Membership where email='" + username + "'"
        Dim dt = c.selectdata(sql)
        Dim dates = Convert.ToInt32(dt.Rows(0)("day"))
        If (dates > Convert.ToInt32(ConfigurationManager.AppSettings.Item("pass_change_max_day"))) Then
            Response.Redirect("~/MARREG_Admin/MARREG_Change_Password.aspx", False)
        End If
    End Sub
#Region "Page Validation"
    Private Sub page_validation()
        Try
            If Session.Item("Marreg_user_info") Is Nothing Then
                'change 07/02/2019
                Dim name As String
                user_info = DirectCast(Session.Item("Marreg_user_info"), Marreg_Loginfo)
                u_info = user_info
                name = "mo-kolkata@nic.in"
                CType(Master.FindControl("lbl_moname"), Label).Text = "mo-kolkata@nic.in"
                mo_image_Url.Visible = False

                btn_logout_master.Visible = False
                btn_home.Visible = True
            Else
                Dim user_info As Marreg_Loginfo
                user_info = DirectCast(Session.Item("Marreg_user_info"), Marreg_Loginfo)
                u_info = user_info
                btn_login_master.Visible = False
                btn_logout_master.Visible = True
                c.role_type_code(user_info.role_nm, role_id)
                btn_home.Visible = False
                'change 07/02/2019
                If Session.Item("mo_image_Url") Is Nothing Then
                Else
                    mo_image_Url.Visible = True

                    mo_image_Url.ImageUrl = Session("mo_image_Url")
                End If

            End If
        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region "Populate tree"
    Private Sub PopulateRootLevel()
        Try
            If Session.Item("Marreg_user_info") IsNot Nothing Then
                Dim objConn As New SqlConnection(dbconnctn.GetConnectionString())
                Dim objCommand As New SqlCommand("select sc.Page_id,(case when sc.Menu_nm like 'US%' then (sc.Menu_nm+' '+rm.Role_Type_nm) else sc.Menu_nm end)Menu_nm,(select count(*) FROM MARREG_Tree_Menu_Master WHERE Page_parentid=sc.Page_id) childnodecount FROM MARREG_Tree_Menu_Master sc ,MARREG_User_Role u,MARREG_RoleType_master rm where rm.Role_Type_cd =u.role_type_cd and u.role_type_cd ='" + role_id.ToString() + "' and sc.page_id =u.page_id and Page_parentID IS NULL  ORDER BY sc.orderby", objConn)
                Dim da As New SqlDataAdapter(objCommand)
                Dim dt As New DataTable()
                da.Fill(dt)
                PopulateNodes(dt, menu_TreeView.Nodes)
                objConn.Dispose()
                objCommand.Dispose()
                da.Dispose()
                dt.Dispose()
            End If
            
        Catch ex As Exception

        End Try
    End Sub
    Private Sub PopulateNodes(ByVal dt As DataTable, ByVal nodes As TreeNodeCollection)
        Try
            For Each dr As DataRow In dt.Rows
                Dim tn As New TreeNode()
                tn.Text = dr("Menu_Nm").ToString()
                tn.Value = dr("Page_ID").ToString()
                nodes.Add(tn)
                'If node has child nodes, then enable on-demand populating
                tn.PopulateOnDemand = (CInt(dr("childnodecount")) > 0)
            Next
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub menu_TreeView_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles menu_TreeView.DataBound
        Try
            If Session("TreeViewState") Is Nothing Then
                ' Record the TreeViews current expand/collapse state. 
                Dim list As ArrayList = New ArrayList
                SaveTreeViewState(menu_TreeView.Nodes, list)
                Session("TreeViewState") = list
            Else
                'Apply the recorded expand/collapse state to the TreeView. 
                Dim list As ArrayList = CType(Session("TreeViewState"), ArrayList)
                RestoreTreeViewState(menu_TreeView.Nodes, list)
            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub menu_TreeView_TreeNodePopulate(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.TreeNodeEventArgs) Handles menu_TreeView.TreeNodePopulate
        Try
            PopulateSubLevel(e.Node.Value, e.Node)
        Catch ex As Exception

        End Try
    End Sub
    Private Sub PopulateSubLevel(ByVal parentid As String, ByVal parentNode As TreeNode)
        Try
            page_validation()
            Dim objConn As New SqlConnection(dbconnctn.GetConnectionString())
            Dim objCommand As New SqlCommand("select sc.Page_id,Menu_Nm,(select count(*) FROM MARREG_Tree_Menu_Master " & _
            " WHERE Page_parentid=sc.Page_id) childnodecount FROM MARREG_Tree_Menu_Master sc, MARREG_User_Role u where sc.page_id =" & _
            " u.page_id and u.role_type_cd='" + role_id.ToString() + "' and sc.Page_parentID='" + parentid + "' ORDER BY sc.orderby ", objConn)
            Dim da As New SqlDataAdapter(objCommand)
            Dim dt As New DataTable()
            da.Fill(dt)
            PopulateNodes(dt, parentNode.ChildNodes)
            objConn.Dispose()
            objCommand.Dispose()
            da.Dispose()
            dt.Dispose()
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub menu_TreeView_TreeNodeCollapsed(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.TreeNodeEventArgs) Handles menu_TreeView.TreeNodeCollapsed
        Try
            If IsPostBack Then
                Dim list As ArrayList = New ArrayList
                SaveTreeViewState(menu_TreeView.Nodes, list)
                Session("TreeViewState") = list
            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub menu_TreeView_TreeNodeExpanded(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.TreeNodeEventArgs) Handles menu_TreeView.TreeNodeExpanded
        Try
            If IsPostBack Then
                Dim list As ArrayList = New ArrayList
                SaveTreeViewState(menu_TreeView.Nodes, list)
                Session("TreeViewState") = list
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub SaveTreeViewState(ByVal nodes As TreeNodeCollection, ByVal list As ArrayList)
        Try
            For Each node As TreeNode In nodes
                If (node.ChildNodes IsNot Nothing And node.ChildNodes.Count <> 0) Then
                    If (node.Expanded.HasValue AndAlso CBool(node.Expanded) AndAlso Not String.IsNullOrEmpty(node.Text)) Then
                        list.Add(node.Text)
                        SaveTreeViewState(node.ChildNodes, list)
                    End If
                End If
            Next
        Catch ex As Exception

        End Try
    End Sub
    Private Sub RestoreTreeViewState(ByVal nodes As TreeNodeCollection, ByVal list As ArrayList)
        Try
            For Each node As TreeNode In nodes
                If list.Contains(node.Text) Then
                    If (node.ChildNodes Is Nothing) Or (node.ChildNodes.Count = 0) Or node.Expanded.HasValue Or Not CBool(node.Expanded) Then
                        node.Expand()
                    End If
                Else
                    If (node.ChildNodes Is Nothing) Or (node.ChildNodes.Count = 0) Or node.Expanded.HasValue Or CBool(node.Expanded) Then
                        node.Collapse()
                    End If
                End If
                If node.ChildNodes Is Nothing Or node.ChildNodes.Count = 0 Then
                    RestoreTreeViewState(node.ChildNodes, list)
                End If
            Next
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub menu_TreeView_SelectedNodeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles menu_TreeView.SelectedNodeChanged
        Try
            If (menu_TreeView.SelectedNode.Value).Length <= 3 Then
                If menu_TreeView.SelectedNode.Value = 1 Then
                    If Session.Item("Marreg_user_info") Is Nothing Then
                        Response.Redirect("~/MARREG_Login/MARREG_About.aspx")
                    'Else
                    '    Response.Redirect("~/MARREG_Portal/MARREG_Home.aspx")
                    End If
                    Exit Sub
                End If
                menu_TreeView.SelectedNode.Expand()
            Else

                Response.Redirect(menu_TreeView.SelectedNode.Value, False)
            End If
        Catch ex As Exception
            Dim p As String = ex.Message()
        End Try
    End Sub
#End Region

#Region "Logout"
    Private Sub logout(ByVal status As String)
        Dim user_info As New Marreg_Loginfo
        user_info = Session("Marreg_user_info")
        Try
            Dim pagenm As String
            Dim url As String = Request.UrlReferrer.ToString
            pagenm = ""
            Dim i As Integer = url.ToString.Length
            While i > 0
                If url.ToString.Substring(i - 1, 1) = "/" Then
                    pagenm = url.ToString.Substring(i)
                    Exit While
                End If
                i = i - 1
            End While
            Dim ipadd As String = System.Web.HttpContext.Current.Request.UserHostAddress

            Dim time As DateTime = Convert.ToDateTime(Format(System.DateTime.Now, "hh:mm:ss"))
            Dim log_status As String = logindl.LogOutHistory(user_info.Login_Status, status, ipadd, pagenm)
            Session.Remove("Marreg_user_info")
            'change 07.02.19
            Session.Remove("name")
            Session.Remove("mo_image_Url")

            Session.Remove("Draft_Number") '07032018
            Response.Redirect("~/MARREG_Portal/MARREG_Home.aspx")
        Catch ex As Exception

        Finally
        End Try
    End Sub
    Protected Sub btn_logout_master_Click(sender As Object, e As ImageClickEventArgs) Handles btn_logout_master.Click
        logout("Logout Successful")
    End Sub
#End Region

#Region "Home Button Click"
    Protected Sub btn_home_Click(sender As Object, e As ImageClickEventArgs) Handles btn_home.Click
        Response.Redirect("~/MARREG_Portal/MARREG_Home.aspx")
    End Sub
#End Region

End Class

