﻿<%@ Page Language="C#" MasterPageFile="~/WF/MasterPage.master" AutoEventWireup="true" Inherits="CCFlow.WF.WF_Emps" Title="Untitled Page" Codebehind="Emps.aspx.cs" %>
<%@ Register src="UC/Emps.ascx" tagname="Emps" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <uc1:Emps ID="Emps1" runat="server" />
</asp:Content>

