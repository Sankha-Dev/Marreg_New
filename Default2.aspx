<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default2.aspx.vb" Inherits="Default2" %>

<%--<!DOCTYPE html>--%>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta name="description" content="User Registration on MARREG Portal" />
    <meta name="keywords" content="MARREG, Registration, User Registration" />
    <meta name="viewport" content="width=device-width, height=device-height, initial-scale=1.0, minimum-scale=1.0" />
    <meta name="_token" content="<%= CSRF.Value%>" />
    <title>Test Page</title>
    <script src="bootstrap/js/libs/jquery.min.js"></script>
    <script src="bootstrap/js/libs/bootstrap.bundle.js"></script>
    <script src="bootstrap/js/libs/bootbox.min.js"></script>
    <script src="bootstrap/fontawesome/js/all.js"></script>

    <link rel="stylesheet" type="text/css" href="bootstrap/fontawesome/css/all.min.css" />
    <link rel="stylesheet" type="text/css" href="bootstrap/css/libs/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="bootstrap/css/style.css" />
    
    <script src="bootstrap/crypto-js/aes.js"></script>
    <script src="bootstrap/crypto-js/sha256.js"></script>
    <script src="bootstrap/crypto-js/MARREG_sha256.js"></script>
    <script>
        //$.ajaxSetup({
        //    headers: {
        //        'X-CSRF-TOKEN': $('meta[name="_token"]').attr('content')
        //    }
        //});
        $(function () {
            var data = $('#<%=l1.ClientID%>').html();//Message to Encrypt
            var key = CryptoJS.enc.Utf8.parse('74905859303049556839030328445902');
            var iv = CryptoJS.enc.Utf8.parse('8940046593349576');  
            //var key = CryptoJS.enc.Utf8.parse('12345678123456781234567812345678');
            //var iv = CryptoJS.enc.Utf8.parse('1234567812345678');  
            var encryptedString = encryptData(data, iv, key);
            $('#<%=l4.ClientID%>').html(encryptedString);//genrated encryption String:  swBX2r1Av2tKpdN7CYisMg==
            
            var decrypteddata = decryptData($('#<%=l4.ClientID%>').html(), iv, key);
            $('#<%=l5.ClientID%>').html(decrypteddata);//genrated decryption string:  Example1

            let dsc_obj = null;
            $("#<%=ddlsigner.ClientID%>").change(function (e) {
                e.preventDefault();
                let url = "<%= Common.baseUrl()%>MARREG_WebService/DSC_WebService.asmx/LoadMODSCDetails";
                $.ajax({
                    crossDomain: false,
                    url: url,
                    cache: false,
                    async: true,
                    type: "Post",
                    dataType: "json",
                    data: { mo: $(this).val() },
                    beforeSend: function () {
                        $('#overlay').show();
                    },
                    success: function (data) {
                        if (data.DataList) {
                            if (data.DataList.length > 0) {
                                $.each(data.DataList, function (key,val) {
                                    if (val.Valid_Status === "Y") {
                                        dsc_obj = val;
                                    }
                                });
                            }
                        }
                        alert(JSON.stringify(dsc_obj));
                    }, error: function (jqXHR, ajaxOptions, thrownError) {
                        /*Error Block*/
                        if (jqXHR.responseJSON) {
                            if (jqXHR.responseJSON.hasOwnProperty('error')) {
                                bootbox.alert(jqXHR.responseJSON.error);
                            } else if (jqXHR.responseJSON.hasOwnProperty('exception')) {
                                bootbox.alert(jqXHR.responseJSON.exception_message);
                            }
                        } else {
                            bootbox.alert("<strong>" + jqXHR.status + ": " + thrownError + "</strong>");
                        }
                    }
                }).always(function () {
                    $('#overlay').hide();
                });
            });
            <%--var key = CryptoJS.enc.Utf8.parse('<%=ConfigurationManager.AppSettings.Item("key256").ToString()%>');
            var iv = CryptoJS.enc.Utf8.parse('<%=ConfigurationManager.AppSettings.Item("iv256").ToString()%>');--%>
            var key = CryptoJS.enc.Utf8.parse('74905859303049556839030328445902');
            var iv = CryptoJS.enc.Utf8.parse('8940046593349576');  
            $("#form1").submit(function (e) {
                e.preventDefault();
                alert("button clicked !!");
                let url = "<%= Common.baseUrl()%>MARREG_WebService/WebService.asmx/GetFileByte";
                let formData = new FormData();
                formData.append('signer',$("#signer").val());
                formData.append('file', $("#file")[0].files[0]);
                $.ajax({
                    crossDomain: false,
                    url: url,
                    type: "POST",
                    dataType: "json",
                    data: formData,
                    cache: false,
                    contentType: false,
                    processData: false,
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader("X-CSRF-TOKEN", $('meta[name="_token"]').attr('content'));
                        $('#overlay').show();
                    },
                    success: function (data) {
                        if (data.success) {
                            if(data.base64String){
                                $.ajax({
                                    crossDomain: true,
                                    //url: "http://localhost:8083/CertSign",
                                    //url: "http://localhost:8083/CertifiedCopySign",
                                    url: "http://localhost:8083/OldCertifiedCopySign",
                                    cache: false,
                                    async: true,
                                    type: "POST",
                                    dataType: "json",
                                    data: { filedata: data.base64String, filepass: encryptData($("#filepass").val(), iv, key), signer: encryptData(JSON.stringify(dsc_obj), iv, key), sign_date:data.sign_date},
                                    beforeSend: function () {
                                        $('#overlay').show();
                                    },
                                    success: function (data) {
                                        $('#transaction').show();
                                        if (data.response) {
                                            var resp = JSON.parse(decryptData(data.response, iv, key)); //decode data
                                            //var resp = JSON.parse(data.response);
                                            if (resp.error) {
                                                //show_message_div("error", "Error: <strong>" + resp.error + "</strong>");
                                                $('#transaction_details').html('<div class="alert alert-danger">' + resp.error + '</div>');
                                            } else if (resp.success) {
                                                const blob = base64ToBlob(resp.signaturesReports, 'application/pdf');
                                                const url = URL.createObjectURL(blob);
                                                //const pdfWindow = window.open("");
                                                //pdfWindow.document.write("<iframe width='100%' height='100%' src='" + url + "'></iframe>");
                                                //$('#frame_pdf').attr('src', 'data:application/pdf;base64,' + resp.signaturesReports);
                                                $('#frame_pdf').attr('src', url);
                                                $('#transaction_details').html('<div class="alert alert-success">' + resp.success + '</div>');
                                            }                                            
                                        }
                                        else{
                                            $('#transaction_details').html('<div class="alert alert-danger">No response</div>');
                                        }
                                    },
                                    error: function (jqXHR, ajaxOptions, thrownError) {
                                        /*Error Block*/
                                        if (jqXHR.responseJSON) {
                                            responseJSON = jqXHR.responseJSON;
                                            var resp = JSON.parse(decryptData(responseJSON.response, iv, key));     //decode data
                                            //var resp = JSON.parse(responseJSON.response);
                                            if (resp.hasOwnProperty('error')) {
                                                bootbox.alert(resp.error);
                                            } else if (resp.hasOwnProperty('exception')) {
                                                bootbox.alert(resp.exception_message);
                                            } else if (resp.hasOwnProperty('ExceptionMessage')) {
                                                bootbox.alert(resp.ExceptionMessage);
                                            }
                                        } else if (jqXHR.readyState == 0 && jqXHR.status == 0) {
                                            bootbox.alert("Error: <strong>Error in connecting DSC Signing Service.</strong>");
                                        } else {
                                            //er_msg = errorThrown;
                                            //bootbox.alert("<strong>" + jqXHR.status + ": " + thrownError + "</strong>");
                                            bootbox.alert("<strong>" + thrownError + "</strong>");
                                        }
                                    }
                                }).always(function () {
                                    $('#overlay').hide();
                                });

                                //const blob = base64ToBlob(data.base64String, 'application/pdf');
                                //const url = URL.createObjectURL(blob);
                                ////const pdfWindow = window.open("");
                                ////pdfWindow.document.write("<iframe width='100%' height='100%' src='" + url + "'></iframe>");
                                ////$('#frame_pdf').attr('src', 'data:application/pdf;base64,' + resp.signaturesReports);
                                //$('#frame_pdf').attr('src', url);
                            }
                            //.hide();
                            
                        } else if (data.error) {
                            $('#transaction').show();
                            $('#transaction_details').html('<div class="alert alert-danger">'+data.error+'</div>');//.hide();
                        }
                    },
                    error: function (jqXHR, ajaxOptions, thrownError) {
                        /*Error Block*/
                        if (jqXHR.responseJSON) {
                            if (jqXHR.responseJSON.hasOwnProperty('error')) {
                                bootbox.alert(jqXHR.responseJSON.error);
                            } else if (jqXHR.responseJSON.hasOwnProperty('exception')) {
                                bootbox.alert(jqXHR.responseJSON.exception_message);
                            }
                        } else {
                            bootbox.alert("<strong>" + jqXHR.status + ": " + thrownError + "</strong>");
                        }
                    }
                }).always(function () {
                    $('#overlay').hide();
                });
            });
            

            function base64ToBlob(base64, type = "application/octet-stream") {
                const binStr = atob(base64);
                const len = binStr.length;
                const arr = new Uint8Array(len);
                for (let i = 0; i < len; i++) {
                    arr[i] = binStr.charCodeAt(i);
                }
                return new Blob([arr], { type: type });
            }
            $("#form2").submit(function (e) {
                e.preventDefault();
                let url = "<%= Common.baseUrl()%>MARREG_WebService/WebService.asmx/GetFileByte";
                //let url = "<%= Common.baseUrl()%>MARREG_WebService/WebService.asmx/Verify";
                let formData = new FormData();
                formData.append('file', $("#file1")[0].files[0]);
                $.ajax({
                    crossDomain: false,
                    url: url,
                    type: "POST",
                    dataType: "json",
                    data: formData,
                    cache: false,
                    contentType: false,
                    processData: false,
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader("X-CSRF-TOKEN", $('meta[name="_token"]').attr('content'));
                        $('#overlay').show();
                    },
                    success: function (data) {
                        if (data.success) {
                            if(data.base64String){
                                $.ajax({
                                    crossDomain: true,
                                    url: "http://localhost:8083/Verify",
                                    cache: false,
                                    async: true,
                                    type: "Post",
                                    dataType: "json",
                                    data: { filedata: data.base64String, filepass: encryptData($("#verifypass").val(), iv, key)},
                                    beforeSend: function () {
                                        $('#overlay').show();
                                    },
                                    success: function (data) {
                                        $('#transaction').show();
                                        if(data.success){
                                            $('#transaction_details').html('<div class="alert alert-success">'+JSON.stringify(data)+'</div>');
                                        }
                                        else{
                                            $('#transaction_details').html('<div class="alert alert-danger">'+data.error+'</div>');
                                        }
                                    },
                                    error: function (jqXHR, ajaxOptions, thrownError) {
                                        /*Error Block*/
                                        if (jqXHR.responseJSON) {
                                            if (jqXHR.responseJSON.hasOwnProperty('error')) {
                                                bootbox.alert(jqXHR.responseJSON.error);
                                            } else if (jqXHR.responseJSON.hasOwnProperty('exception')) {
                                                bootbox.alert(jqXHR.responseJSON.exception_message);
                                            }
                                        } else {
                                            bootbox.alert("<strong>" + jqXHR.status + ": " + thrownError + "</strong>");
                                        }
                                    }
                                }).always(function () {
                                    $('#overlay').hide();
                                });
                            }
                            //.hide();
                            //$('#transaction').show();
                            //$('#transaction_details').html('<div class="alert alert-success">'+data.success+'</div>');
                        } else if (data.error) {
                            $('#transaction').show();
                            $('#transaction_details').html('<div class="alert alert-danger">'+data.error+'</div>');//.hide();
                        }
                    },
                    error: function (jqXHR, ajaxOptions, thrownError) {
                        /*Error Block*/
                        if (jqXHR.responseJSON) {
                            if (jqXHR.responseJSON.hasOwnProperty('error')) {
                                bootbox.alert(jqXHR.responseJSON.error);
                            } else if (jqXHR.responseJSON.hasOwnProperty('exception')) {
                                bootbox.alert(jqXHR.responseJSON.exception_message);
                            }
                        } else {
                            bootbox.alert("<strong>" + jqXHR.status + ": " + thrownError + "</strong>");
                        }
                    }
                }).always(function () {
                    $('#overlay').hide();
                });
            });
        });
    </script>
</head>
<body>
    
    <div>
        <asp:image runat="server" id="qr_image" name="qr_image"></asp:image>
    </div>
    <div class="row">
        <div class="col-6"><asp:label ID="l1" runat="server"></asp:label><br /><asp:label ID="l2" runat="server"></asp:label><br /><asp:label ID="l3" runat="server"></asp:label>
            <br /><asp:label ID="l4" runat="server"></asp:label><br /><asp:label ID="l5" runat="server"></asp:label>
        </div>
        <div class="col-6"><button id="lblLink" runat="server">Click to view file</button></div>
    </div>
        <div class="form-inline">
        <div id="transaction" class="col-12">
            <div id="transaction_details">

            </div>
        </div>
        
        <div class="border-primary card col-5 m-1">
            <div class="card-header"><h5 class="card-title">Signing</h5></div>
            <div class="card-body">
                <form id="form1" runat="server" class="container-fluid" enctype="multipart/form-data">
                    <div class="form-group row col">
                        <label class="col-4">Signer</label>
                        <div class="col-8">
                            <input type="text" id="signer" class="form-control form-control-sm col-5" name="signer" /> or
                            <asp:DropDownList ID="ddlsigner" runat="server" CssClass="form-control-sm form-control col-6"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-4">Choose File</label>
                        <input type="file" id="file" name="file" class="form-control-file form-control-sm col-8" />
                    </div>
                    <div class="form-group row col">
                        <label class="col-4">File Password</label>
                        <div class="col-8">
                            <input type="text" id="filepass" class="form-control form-control-sm" name="filepass" />
                        </div>
                    </div>
                    <div class="form-group row">
                        <input type="submit" class="btn-dark btn col" value="Sign" />
                    </div>
                </form>
            </div>
            <%--<asp:TextBox ID="txtgdob" runat="server"></asp:TextBox>
            <asp:TextBox ID="txtbdob" runat="server"></asp:TextBox>
            <asp:TextBox ID="txtsclmardt" runat="server"></asp:TextBox>
            <asp:Button ID="checkbtn" runat="server" Text="Check" />
            <asp:label ID="lblmsg" runat="server"></asp:label>--%>
        </div>
            <%--<div class="w-25"></div>--%>
        <div class="border-primary card col-5 m-1">
            <div class="card-header"><h5 class="card-title">Verifying</h5></div>
            <div class="card-body">                
                <form id="form2" class="container-fluid" enctype="multipart/form-data">
                    <div class="form-group row">
                        <label class="col-4">Choose File</label>
                        <input type="file" id="file1" name="file" class="form-control-file form-control-sm col-8" />
                    </div>
                    <div class="form-group row col">
                        <label class="col-4">File Password</label>
                        <div class="col-8">
                            <input type="text" id="verifypass" class="form-control form-control-sm" name="verifypass" />
                        </div>
                    </div>
                    <div class="form-group row">
                        <input type="submit" class="btn-block btn col" value="Verify" />
                    </div>
                </form>
            </div>
        </div>
        <div class="border-success card col-11 m-1">
            <iframe id="frame_pdf" class="h-50"></iframe>
        </div>
        </div>
    
    <div id="overlay" style="display: none;">
        <div id="loading"></div>
    </div>
</body>
</html>
