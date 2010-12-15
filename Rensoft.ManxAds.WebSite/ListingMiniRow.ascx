<%@ Control Language="C#" AutoEventWireup="true" Inherits="ListingMiniRow" Codebehind="ListingMiniRow.ascx.cs" %>

<asp:Panel ID="TraderTagPanel" runat="server" CssClass="ListingTraderTag" Visible="False">
    <asp:HyperLink ID="TraderTagHyperLink" runat="server">
        <asp:Image ID="TraderTagImage" runat="server" ImageUrl="~/Images/Static/Layout/TraderTag.gif" AlternateText="Trader Listing" />
    </asp:HyperLink>
</asp:Panel>
<table width="100%" cellpadding="0" cellspacing="0">
    <tr>
        <td class="BrowserThumbnail">
            <asp:HyperLink ID="ThumbnailHyperLink" runat="server">
                <asp:Image ID="ThumbnailImage" runat="server" CssClass="BrowserThumbnailImage" BorderWidth="1px" ImageUrl="~/Images/Static/Layout/PlaceholderThumbnail.gif" />
            </asp:HyperLink>
        </td>
        <td class="BrowserDetails">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr class="BrowserDetailsSummary">
                    <td class="ListingsMiniRowTitle"><h2><asp:HyperLink ID="ShortTitleHyperLink" runat="server" /></h2></td>
                    <td class="ListingsMiniRowPrice"><asp:Label ID="PriceColouredLabel" runat="server" /></td>
                </tr>
                <tr>
                    <td class="BrowserDetailsSeparator"></td>
                </tr>
                <tr class="BrowserDetailsDescription">
                    <td colspan="3"><p><asp:Label ID="DescriptionLabel" runat="server" CssClass="ListingsMiniRowDetails" /></p></td>
                </tr>
            </table>
        </td>
    </tr>
</table>