<%@ Page Language="C#" MasterPageFile="~/Master.master" AutoEventWireup="true" Inherits="Search" Title="Search ManxAds" Codebehind="Search.aspx.cs" %>

<%@ Register Src="SearchDialog.ascx" TagName="SearchDialog" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1>Search ManxAds</h1>
    <uc1:SearchDialog ID="SearchDialog1" runat="server" />
</asp:Content>

