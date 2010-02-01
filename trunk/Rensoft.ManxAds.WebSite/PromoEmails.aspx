<%@ Page Language="C#" MasterPageFile="~/Master.master" AutoEventWireup="true" Inherits="PromoEmails" Title="Safe Email Addresses" Codebehind="PromoEmails.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1>Safe Email Addresses</h1>
    <p>This page is displaying a list of email addresses which are safe for mass-mail. The users who own these addresses are happy to accept email from ManxAds.</p>
    <p><asp:Label ID="EmailsLabel" runat="server" /></p>
</asp:Content>

