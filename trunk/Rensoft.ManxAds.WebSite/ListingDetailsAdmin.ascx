<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ListingDetailsAdmin.ascx.cs" Inherits="Rensoft.ManxAds.WebSite.ListingDetailsAdmin" %>
<div class="ListingAdminPanel">
    <p>
        <b>Admin actions:</b>
        <asp:HyperLink ID="EditListingHyperLink" runat="server">Edit listing</asp:HyperLink> | 
        <asp:HyperLink ID="DeleteListingHyperLink" runat="server">Delete listing</asp:HyperLink> |
        <asp:HyperLink ID="EditUserHyperLink" runat="server">Edit user</asp:HyperLink> |
        <asp:HyperLink ID="BanUserHyperLink" runat="server">Ban user</asp:HyperLink>
    </p>
    <p>
        <b>Last known IP:</b> <asp:Label ID="LastKnownIpLabel" runat="server" ></asp:Label> 
    </p>
</div>