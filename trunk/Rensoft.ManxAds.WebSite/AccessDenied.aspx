<%@ Page Language="C#" MasterPageFile="~/Master.master" AutoEventWireup="true" Inherits="AccessDenied" Title="Access Denied" Codebehind="AccessDenied.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1>Access Denied</h1>
    <p><asp:Image ID="Image3" runat="server" ImageUrl="~/Images/Static/Layout/ErrorIcon.gif" ImageAlign="TextTop" />&nbsp;Sorry, the page you are trying to view is restricted to users with higher security than yourself.</p>
</asp:Content>