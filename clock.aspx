<%@ Page Language="VB" AutoEventWireup="false" CodeFile="clock.aspx.vb" Inherits="clock" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Clock</title>
            <meta name="clock" http-equiv="refresh" content="1" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="lbl_clock" runat="server"></asp:Label>
        </div>
    </form>
</body>
</html>
