<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default6.aspx.vb" Inherits="Default6" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>File Upload</title>
    <script type="text/javascript" src="bootstrap/js/libs/jquery.min.js"></script>
    <link rel="stylesheet" type="text/css" href="bootstrap/toast/toastr.css" />
<script type="text/javascript" src="bootstrap/toast/toastr.js"></script>
<script type="text/javascript">
    function showpop6(msg, title) {
        toastr.options = {
            "closeButton": true,
            "debug": false,
            "newestOnTop": false,
            "progressBar": true,
            "positionClass": "toast-bottom-left",
            "preventDuplicates": true,
            "onclick": null,
            "showDuration": "300",
            "hideDuration": "1000",
            "timeOut": "6000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        }
        // toastr['success'](msg, title);
        var d = Date();
        toastr.error(msg, title);
        return false;
    }
    function showpop5(msg, title) {
        toastr.options = {
            "closeButton": true,
            "debug": false,
            "newestOnTop": false,
            "progressBar": true,
            "positionClass": "toast-bottom-left",
            "preventDuplicates": true,
            "onclick": null,
            "showDuration": "300",
            "hideDuration": "1000",
            "timeOut": "6000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        }
        // toastr['success'](msg, title);
        var d = Date();
        toastr.success(msg, title);
        return false;
    }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table width="30%" >
                <tr>
                    <td width="20%"><asp:Label ID="lbl_upload" runat="server">File</asp:Label></td>
                    <td><asp:FileUpload ID="file_upload" runat="server" /></td>
                </tr>
                <tr><td colspan="2">&nbsp;</td></tr>
                <tr><td>&nbsp;</td><td><asp:Button ID="btn_upload" runat="server" Text="Upload" /></td></tr>
            </table>
        </div>
    </form>
</body>
</html>
