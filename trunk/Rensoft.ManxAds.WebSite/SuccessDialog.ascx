<%@ Control Language="C#" AutoEventWireup="true" Inherits="SuccessDialog" Codebehind="SuccessDialog.ascx.cs" %>
<h3>Success!</h3>
<p>Where would you like to go now?</p>
<ul>
    <li><b><asp:HyperLink runat="server" ID="ReturnHyperLink">Previous Page</asp:HyperLink></b></li>
    <span ID="ListingsPanel" runat="server" Visible="False">
        <li><asp:HyperLink ID="ListingViewHyperLink" runat="server">View Listing</asp:HyperLink></li>
        <li><asp:HyperLink ID="ListingCreateHyperLink" runat="server" NavigateUrl="~/ListingModify.aspx">Create Another</asp:HyperLink></li>
    </span>
    <li><asp:HyperLink runat="server" ID="UserHomeHyperLink" NavigateUrl="UserHome.aspx">My ManxAds</asp:HyperLink> (your account)</li>
</ul>