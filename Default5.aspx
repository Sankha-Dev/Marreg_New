<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default5.aspx.vb" Inherits="Default5" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="bootstrap/js/libs/jquery.min.js"></script>
    <script src="bootstrap/crypto-js/aes.js"></script>
    <script src="bootstrap/crypto-js/sha256.js"></script>
    <script src="bootstrap/crypto-js/MARREG_sha256.js"></script>
    <script>
        $(function () {
            let key = CryptoJS.enc.Utf8.parse("<%=ConfigurationManager.AppSettings("bskkey256").ToString()%>");
            let iv = CryptoJS.enc.Utf8.parse("<%=ConfigurationManager.AppSettings("bskiv256").ToString()%>");
            let a = { userId: "8527419636", ticketNo: "20231107151823279", citizenMobile: "9681831219", citizenName: "Utsabendu" };
            $("#normal_text").html(JSON.stringify(a))
            $("#<%=encBSKJS.ClientID%>").click(function (e) {
                e.preventDefault();
                
                $("#enc_text").html(encryptData($("#normal_text").html(), iv, key));
            });
            $("#<%=decBSKJS.ClientID%>").click(function (e) {
                e.preventDefault();

                $("#dec_text").html(decryptData($("#enc_text").html(), iv, key));
            });
            $("#<%=compCheckSumJS.ClientID%>").click(function (e) {
                e.preventDefault();
                var hash = CoputeHash($("#enc_text").html()); alert(hash);
                //$("#chksum_text").html(hash);

                //alert(JSON.stringify({ chk: hash }));
                var da = { enc: $("#enc_text").html(), ha: hash };
                url="http://127.0.0.1/"
                setTimeout(function () { redirectPost(url, da); }, 100);
            });
            var redirectPost = function (url, data = null, target = '_parent', method = 'post') {
                var form = document.createElement('form');
                form.method = method;
                form.action = url;
                form.target = target;
                for (var name in data) {
                    var input = document.createElement('input');
                    input.type = 'hidden';
                    input.name = name;
                    input.value = data[name];
                    form.appendChild(input);
                }
                $('body').append(form);
                form.submit();
            };
        });

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div>
                    <label style="width: 110px; display: inline-block;">Normal Text:</label>
                    <textarea type="text" id="normalText" runat="server" style="width: 500px; height: 50px;"></textarea>
                    <label id="normal_text"></label>
                </div>
                <div>
                    <label style="width: 110px; display: inline-block;">Encrypted Text:</label>
                    <textarea type="text" id="encText" runat="server" style="width: 500px; height: 50px;"></textarea>
                    <label id="enc_text"></label>
                </div>
                <div>
                    <label style="width: 110px; display: inline-block;">Decrypted Text:</label>
                    <textarea type="text" id="decText" runat="server" style="width: 500px; height: 50px;"></textarea>
                    <label id="dec_text"></label>
                </div>
                <div>
                    <select id="tmp_id" runat="server"></select>
                    <select id="col" runat="server">
                        <option value="g_imageByte">g_imageByte</option>
                        <option value="b_imageByte">b_imageByte</option>
                        <option value="wt1_imageByte">wt1_imageByte</option>
                        <option value="wt2_imageByte">wt2_imageByte</option>
                        <option value="wt3_imageByte">wt3_imageByte</option>
                        <option value="testvb">testvb</option>
                    </select>
                    <button id="decByte" runat="server">Decrypt Byte From DB</button>
                </div>
                <div>
                    <label style="width: 110px; display: inline-block;">Base64 Text:</label>
                    <textarea type="text" id="base64Text" runat="server" style="width: 500px; height: 50px;"></textarea>
                    <label id="chksum_text"></label>
                </div>
                <div>
                    <label style="width: 110px; display: inline-block;">Image:</label>
                    <img id="img" runat="server" src="" style="width: 160px; height: 200px; display: inline-block; border: 1px solid;" />
                </div>
                <div>&nbsp;</div>
                <div>
                    <button id="encrypt" runat="server">Encrypt</button>
                    &nbsp;&nbsp;
                    <button id="encByte" runat="server">Encrypt Byte</button>
                    &nbsp;&nbsp;
                    <button id="decrypt" runat="server">Decrypt</button>                    
                    &nbsp;&nbsp;
                    <button id="base64_img" runat="server">Base64 to Image</button>
                    &nbsp;&nbsp;
                    <button id="encBSK" runat="server">Encrypt BSK</button>
                    &nbsp;&nbsp;
                    <button id="decBSK" runat="server">Decrypt BSK</button>
                    &nbsp;&nbsp;
                    <button id="compCheckSum" runat="server">CheckSum</button>
                    &nbsp;&nbsp;
                    <button id="encBSKJS" runat="server">Encrypt BSK JS</button>
                    &nbsp;&nbsp;
                    <button id="decBSKJS" runat="server">Decrypt BSK JS</button>
                    &nbsp;&nbsp;
                    <button id="compCheckSumJS" runat="server">CheckSum JS</button>
                </div>
                <div><label style="display: inline-block;" id="msg" runat="server"></label></div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
