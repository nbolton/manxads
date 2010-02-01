<%@ Control Language="C#" AutoEventWireup="true" Inherits="SellerOptions" Codebehind="SellerOptions.ascx.cs" %>
<div class="GenericFormHeader">Seller Menu</div>
<div class="GenericFormBody">
    <h3>What do you want to do?</h3>
    <ul>
        <li><asp:HyperLink ID="HyperLink3" NavigateUrl="~/ListingModify.aspx?Create=1" runat="server">New Listing</asp:HyperLink></li>
        <li><asp:HyperLink ID="HyperLink6" NavigateUrl="~/Listings.aspx?Self=1" runat="server">My Listings</asp:HyperLink> (<asp:Label ID="ListingCountLabel" runat="server" />)</li>
        <li><asp:HyperLink ID="HyperLink1" NavigateUrl="~/Listings.aspx?Self=1&RecycleBin=1" runat="server">Recycle Bin</asp:HyperLink> (<asp:Label ID="RecycleBinCountLabel" runat="server" />) <asp:LinkButton ID="RecycleBinEmptyLinkButton" runat="server" onclick="RecycleBinEmptyLinkButton_Click">Empty</asp:LinkButton></li>
    </ul>
</div>
<br class="GenericFormSeparator" />