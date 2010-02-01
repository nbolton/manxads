<%@ Control Language="C#" AutoEventWireup="true" Inherits="SearchInline" Codebehind="SearchInline.ascx.cs" %>
<div id="LayoutHeaderSearchTitle">
    <span runat="server" visible="false"><span style="color: #2166B1">Search</span> through <asp:Label ID="ListingCountEstimateLabel" runat="server" /> online listings!</span>
    <span runat="server"><span style="color: #2166B1">Search</span> hundreds of online listings!</span>
</div>
<div id="LayoutHeaderSearchControls">
    <div style="float: left; height: 23px"><asp:TextBox CssClass="TextBox" ID="SearchTextBox" runat="server" Width="155px" /></div>
    <div style="float: left; height: 23px"><asp:ImageButton CssClass="ImageButton" ID="SearchButton" runat="server" ImageUrl="~/Images/Static/Layout/SearchButton.gif" OnClick="SearchButton_Click" CausesValidation="False" /></div>
</div>