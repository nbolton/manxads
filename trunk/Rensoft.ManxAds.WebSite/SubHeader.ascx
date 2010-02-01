<%@ Control Language="C#" AutoEventWireup="true" Inherits="SubHeader" Codebehind="SubHeader.ascx.cs" %>

<div id="LayoutSubHeaderButtons">
    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Categories.aspx"><asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Static/Layout/BuyButton.gif" AlternateText="Buy" /></asp:HyperLink>
    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/ListingModify.aspx"><asp:Image ID="Image2" runat="server" ImageUrl="~/Images/Static/Layout/SellButton.gif" AlternateText="Sell" /></asp:HyperLink>
    <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/UserHome.aspx"><asp:Image ID="Image3" runat="server" ImageUrl="~/Images/Static/Layout/MyManxAdsButton.gif" AlternateText="My ManxAds" /></asp:HyperLink>
</div>
<div id="LayoutSubHeaderMenu">
    <div id="LayoutSubHeaderMenuLinks">
        <div id="LayoutSubHeaderMenuNavigation">
            <asp:HyperLink ID="HyperLink5" runat="server" NavigateUrl="~/">Home</asp:HyperLink> |
            <asp:HyperLink ID="HyperLink6" runat="server" NavigateUrl="~/Search.aspx?Advanced=1">Search</asp:HyperLink> |
            <asp:HyperLink ID="HyperLink7" runat="server" NavigateUrl="~/Help.aspx">Help</asp:HyperLink> |
            <asp:HyperLink ID="HyperLink8" runat="server" NavigateUrl="~/CharityList.aspx">Charities</asp:HyperLink> |
            <asp:HyperLink ID="HyperLink10" runat="server" NavigateUrl="~/TraderList.aspx">Traders</asp:HyperLink>
            <span id="RegisterPanel" runat="server">
                | <asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl="~/Register.aspx">Register</asp:HyperLink>
            </span>
        </div>
        <div id="LayoutSubHeaderMenuSecurity">
            <asp:MultiView ID="StatusMultiView" runat="server">
                <asp:View ID="LoggedOnView" runat="server">
                    Hello, <asp:Label ID="UserFullName" runat="server" />! (Not you? <asp:LinkButton ID="LogOnLinkButton" runat="server" OnClick="LogOnLinkButton_Click" CausesValidation="False">Log On</asp:LinkButton>).
                </asp:View>
                <asp:View ID="LoggedOutView" runat="server">
                    Welcome to ManxAds! <asp:HyperLink ID="LogOnHyperLink" NavigateUrl="~/Logon.aspx" runat="server">Log on</asp:HyperLink> or <asp:HyperLink ID="RegisterHyperLink" NavigateUrl="~/Register.aspx" runat="server">register</asp:HyperLink>.
                </asp:View>
            </asp:MultiView>
        </div>
    </div>
    <div id="LayoutSubHeaderMenuBar"></div>
</div>