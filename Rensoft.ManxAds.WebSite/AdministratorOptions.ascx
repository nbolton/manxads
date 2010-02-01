<%@ Control Language="C#" AutoEventWireup="true" Inherits="AdministratorOptions" Codebehind="AdministratorOptions.ascx.cs" %>
<div class="GenericFormHeader">Administrator Menu</div>
<div class="GenericFormBody">
    <h3>What do you want to do?</h3>
    <ul>
        <li><asp:HyperLink ID="HyperLink5" NavigateUrl="~/UserModify.aspx?Create=1" runat="server">Create User</asp:HyperLink></li>
        <li><asp:HyperLink ID="HyperLink2" NavigateUrl="~/CategoryModify.aspx?Create=1" runat="server">Create Category</asp:HyperLink></li>
        <li><asp:HyperLink ID="HyperLink1" NavigateUrl="~/UserBrowser.aspx" runat="server">Explore Users</asp:HyperLink></li>
        <li><asp:HyperLink ID="HyperLink3" NavigateUrl="~/Categories.aspx" runat="server">Explore Categories</asp:HyperLink></li>
        <li><asp:HyperLink ID="HyperLink4" NavigateUrl="~/AdvertBrowser.aspx" runat="server">Explore Adverts</asp:HyperLink></li>
        <li><asp:HyperLink ID="HyperLink6" NavigateUrl="~/SettingsEditor.aspx" runat="server">Change Settings</asp:HyperLink></li>
        <li><asp:HyperLink ID="HyperLink7" NavigateUrl="~/PromoEmails.aspx" runat="server">Safe Email Addresses</asp:HyperLink></li>
    </ul>
</div>
<div class="GenericFormBody">
    <h3>Website Statistics</h3>
    <p>These live statistics provide an activity overview of the website. Any type of user can be a corporate or charity trader.</p>
    <asp:Table ID="StatisticsTable" CssClass="AdminStatisticsTable" runat="server">
            <asp:TableHeaderRow>
                <asp:TableHeaderCell />
                <asp:TableHeaderCell>Week</asp:TableHeaderCell>
                <asp:TableHeaderCell>Total</asp:TableHeaderCell>
            </asp:TableHeaderRow>
            <asp:TableRow>
                <asp:TableHeaderCell>Listings</asp:TableHeaderCell>
                <asp:TableCell ID="ListingWeekCell" />
                <asp:TableCell ID="ListingTotalCell" />
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableHeaderCell>Sellers</asp:TableHeaderCell>
                <asp:TableCell ID="SellerWeekCell" />
                <asp:TableCell ID="SellerTotalCell" />
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableHeaderCell>Corporate</asp:TableHeaderCell>
                <asp:TableCell ID="CorporateWeekCell" />
                <asp:TableCell ID="CorporateTotalCell" />
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableHeaderCell>Charity</asp:TableHeaderCell>
                <asp:TableCell ID="CharityWeekCell" />
                <asp:TableCell ID="CharityTotalCell" />
            </asp:TableRow>
        </asp:Table>
</div>
<br class="GenericFormSeparator" />