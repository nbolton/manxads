<%@ Page Language="C#" MasterPageFile="~/Master.master" AutoEventWireup="true" Inherits="ListingDetailsInline" Codebehind="ListingDetailsInline.aspx.cs" %>

<%@ Register Src="ListingDetails.ascx" TagName="ListingDetails" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <uc1:ListingDetails ID="ListingDetails" runat="server" />
</asp:Content>

