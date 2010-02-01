<%@ Page Language="C#" MasterPageFile="~/Master.master" AutoEventWireup="true" Inherits="Listings" Title="Listings" Codebehind="Listings.aspx.cs" %>
<%@ Register Src="ListingBrowser.ascx" TagName="ListingBrowser" TagPrefix="uc7" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <uc7:ListingBrowser ID="ListingBrowser1" runat="server" />
</asp:Content>