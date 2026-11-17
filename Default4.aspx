<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default4.aspx.vb" Inherits="Default4" %>

<!DOCTYPE html>
<html lang="en">
  
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content=
        "width=device-width, initial-scale=1.0">
  
    <title>Speech to Text</title>
    <script src="bootstrap/js/libs/jquery.min.js"></script>
    <script>
        //textArea = document.querySelector("textArea");
        //speechBtn = document.querySelector("speak");
        //speechBtn.addEventListener("click", e => {
        //    e.preventDefault();
        //    if (textArea.value != "") {
        //        textToSpeech(textArea.value);
        //    }
        //});

        function speak_speak() {
            textArea = document.getElementById("textArea");
            if (textArea.value != "") {
                textToSpeech(textArea.value);
            }
        }
        function textToSpeech(txt) {
            //let utterence = new SpeechSynthesisUtterance(txt);
            //speechSynthesis.speak(utterence);

            let speakData = new SpeechSynthesisUtterance();
            speakData.volume = 1; // From 0 to 1
            speakData.rate = 1; // From 0.1 to 10
            speakData.pitch = 1.5; // From 0 to 2
            speakData.text = txt;
            speakData.lang = 'en-US';
            speakData.voice = getVoices()[0];

            speechSynthesis.speak(speakData);
        }
        function getVoices() {
            let voices = speechSynthesis.getVoices();
            if (!voices.length) {
                let utterance = new SpeechSynthesisUtterance("");
                speechSynthesis.speak(utterance);
                voices = speechSynthesis.getVoices(); alert(voices.length);
            }            
            return voices;
        }
        $(function () {
            let device = {};
            let url = "http://localhost:11100/rd/info";
            $.ajax({
                crossDomain: true,
                url: url,
                cache: false,
                async: false,
                type: "DEVICEINFO",
                //dataType:"text/xml; encoding='utf-8'",
                dataType: "xml",
                //data: { mo: $(this).val() },
                beforeSend: function () {
                    $('#overlay').show();
                },
                success: function (data) {
                    //dataType="text"
                    //var xmlDoc = $.parseXML(data);
                    //var $xml = $(xmlDoc);
                    //var person = $xml.find('DeviceInfo');
                    //alert(person.attr("dpId"));
                    //=====================================
                    //dataType="xml"
                    var $xml = $(data);
                    var DeviceInfo = $xml.find("DeviceInfo");
                    $.extend(true, device, { model: DeviceInfo.attr("mi")});
                    $(DeviceInfo).find("additional_info > *").each(function () {    
                        if (this.nodeName == "Param") {
                            if ($(this).attr("name") == "srno") {
                                $.extend(true, device, { sl_no: $(this).attr("value") });
                            }
                        }
                    });
                    alert(JSON.stringify(device));
                    $.ajax({
                        crossDomain: false,
                        url: "<%= Common.baseUrl()%>MARREG_WebService/Biometric_WebService.asmx/RegisterDevice";,
                        cache: false,
                        async: false,
                        type: "post",
                        dataType: "json",
                        data: device,
                        beforeSend: function () {
                            $('#overlay').show();
                        },
                        success: function (data) {
                            
                        }
                    }).always(function () {
                        $('#overlay').hide();
                    });
                }, error: function (jqXHR, ajaxOptions, thrownError) {
                    /*Error Block*/
                    if (jqXHR.responseJSON) {
                        if (jqXHR.responseJSON.hasOwnProperty('error')) {
                            alert(jqXHR.responseJSON.error);
                        } else if (jqXHR.responseJSON.hasOwnProperty('exception')) {
                            alert(jqXHR.responseJSON.exception_message);
                        }
                    } else if (jqXHR.status == 0) {
                        alert("Check if RDSERVICE is running");
                    } else {
                        alert("<strong>" + jqXHR.status + ": " + thrownError + "</strong>");
                    }
                }
            }).always(function () {
                $('#overlay').hide();
            });
            function xmlToString(xmlData) {

                var xmlString;
                //IE
                if (window.ActiveXObject) {
                    xmlString = xmlData.xml;
                }
                // code for Mozilla, Firefox, Opera, etc.
                else {
                    xmlString = (new XMLSerializer()).serializeToString(xmlData);
                }
                return xmlString;
            } 
        });
    </script>
    
</head>
  
<body>
    <textarea id="textArea"></textarea>
    <button type="button" id="speak" onclick="speak_speak()">Speak</button>
    
    <div class="words" contenteditable>
        <p id="p"></p>
    </div>
  
    <script>
        var speech = true;
        window.SpeechRecognition = window.SpeechRecognition
                        || window.webkitSpeechRecognition;
  
        const recognition = new SpeechRecognition();
        recognition.interimResults = true;
        const words = document.querySelector('.words');
        words.appendChild(p);
  
        recognition.addEventListener('result', e => {
            const transcript = Array.from(e.results)
                .map(result => result[0])
                .map(result => result.transcript)
                .join('')
  
            document.getElementById("p").innerHTML = transcript;
            console.log(transcript);
        });
          
        if (speech == true) {
            recognition.start();
            recognition.addEventListener('end', recognition.start);
        }
    </script>
    <form runat="server">
    <table border="0" cellpadding="0" cellspacing="0">
        <tr>
        <td style="width: 80px">
            Status
        </td>
        <td>
            <asp:Label ID="lbl_status" runat="server"></asp:Label>
        </td>
        </tr>
    <tr>
        <td style="width: 80px">
            To:
        </td>
        <td>
            <asp:TextBox ID="txtTo" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td>
            Subject:
        </td>
        <td>
            <asp:TextBox ID="txtSubject" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td valign="top">
            Body:
        </td>
        <td>
            <asp:TextBox ID="txtBody" runat="server" TextMode="MultiLine" Height="150" Width="200"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td>
            File Attachment:
        </td>
        <td>
            <asp:FileUpload ID="fuAttachment" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td>
            Gmail Email:
        </td>
        <td>
            <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td>
            Gmail Password:
        </td>
        <td>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Text="ywlbonpgwrjxbhyb"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td>
        </td>
        <td>
            <asp:Button Text="Send" OnClick="SendAsyncEmail" runat="server" />
        </td>
    </tr>
</table>
        </form>
</body>
  
</html>