<%@ Page Language="C#" MasterPageFile="~/Master.master" AutoEventWireup="true" Inherits="ContactFormInline" Title="Contact Seller" Codebehind="ContactFormInline.aspx.cs" %>

<%@ Register Src="ContactForm.ascx" TagName="ContactForm" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <uc1:ContactForm id="ContactForm1" runat="server" />
</asp:Content>

