﻿<%@ Page Title="" Language="C#" MasterPageFile="WinOpen.master" AutoEventWireup="true" Inherits="CCFlow.WF.WF_RuningSmallSingle" Codebehind="RuningSmallSingle.aspx.cs" %>
<%@ Register src="UC/Runing.ascx" tagname="Runing" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script language="JavaScript" src="/WF/Comm/JScript.js"></script>
    <script language=javascript>
        function Do(warning, url) {
            if (window.confirm(warning) == false)
                return;
            window.location.href = url;
        }
		</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <uc1:Runing ID="Runing1" runat="server" />
</asp:Content>