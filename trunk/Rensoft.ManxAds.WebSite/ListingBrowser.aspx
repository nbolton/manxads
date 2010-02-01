<%@ Page Language="C#" MasterPageFile="~/Master.master" AutoEventWireup="true" Inherits="ListingBrowserObsolete" Title="Listings" Codebehind="ListingBrowser.aspx.cs" %>
<%@ Register Src="ListingBrowser.ascx" TagName="ListingBrowser" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <uc1:ListingBrowser ID="ListingBrowser1" runat="server" />
</asp:Content>

