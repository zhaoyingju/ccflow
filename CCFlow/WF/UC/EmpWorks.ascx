﻿<%@ Control Language="C#" AutoEventWireup="true" Inherits="CCFlow.WF.UC.EmpWorks" Codebehind="EmpWorks.ascx.cs" %>
<%@ Register src="/WF/Pub.ascx" tagname="Pub" tagprefix="uc1" %>
    <div style="width:100%;" >
    <uc1:Pub ID="Pub1" runat="server" />
    </div>
    <script   type="text/javascript">
        var NS4 = (document.layers);
        var IE4 = (document.all);
        var win = window;
        var n = 0;
        function findInPage(str) {

            alert(document.getElementById('string1'));
            str = document.getElementById('string1').value;
        //    alert(str);
            var txt, i, found;
            if (str == "")
                return false;
            if (NS4) {
                if (!win.find(str))
                    while (win.find(str, false, true))
                        n++;
                else
                    n++;
                if (n == 0)
                    alert("对不起！没有你要找的内容。");
            }
            if (IE4) {
                txt = win.document.body.createTextRange();
                for (i = 0; i <= n && (found = txt.findText(str)) != false; i++) {
                    txt.moveStart("character", 1);
                    txt.moveEnd("textedit");
                }
                if (found) {
                    txt.moveStart("character", -1);
                    txt.findText(str);
                    txt.select();
                    txt.scrollIntoView();
                    n++;
                }
                else {
                    if (n > 0) {
                        n = 0;
                        findInPage(str);
                    }
                    else
                        alert("对不起！没有你要找的内容。");
                }
            }
            return false;
        }

        function SetImg(appPath,id) {
            document.getElementById(id).src = appPath + 'WF/Img/Mail_Read.png';
        }

        function GroupBarClick(appPath, rowIdx) {
            var alt = document.getElementById('Img' + rowIdx).alert;
            var sta = 'block';
            if (alt == 'Max') {
                sta = 'block';
                alt = 'Min';
            } else {
                sta = 'none';
                alt = 'Max';
            }
            document.getElementById('Img' + rowIdx).src = appPath + 'WF/Img/' + alt + '.gif';
            document.getElementById('Img' + rowIdx).alert = alt;
            var i = 0
            for (i = 0; i <= 5000; i++) {
                if (document.getElementById(rowIdx + '_' + i) == null)
                    continue;
                document.getElementById(rowIdx + '_' + i).style.display = sta;
            }
        }
        function WinOpenIt(url) {
            var newWindow = window.open(url, '_blank', 'height=600,width=850,top=50,left=50,toolbar=no,menubar=no,scrollbars=yes, resizable=yes,location=no, status=no');
            newWindow.focus();
            return;
        }
    </script>
    <style>
        .TTD
        {
          word-wrap: break-word; 
      　　word-break: normal; 
        }
        .Icon
        {
            width:16px;
            height:16px;
            border:0px;
        }
          
    </style>
