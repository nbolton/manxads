<%@ Control Language="C#" AutoEventWireup="true" Inherits="ListingRow" Codebehind="ListingRow.ascx.cs" %>

<asp:Panel ID="TraderTagPanel" runat="server" CssClass="ListingTraderTag" Visible="False">
    <asp:HyperLink ID="TraderTagHyperLink" runat="server">
        <asp:Image ID="TraderTagImage" runat="server" />
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
                    <td class="ListingRowTitle"><asp:HyperLink ID="TitleHyperLink" runat="server" /></td>
                    <td class="ListingRowPrice"><asp:Label ID="PriceColouredLabel" runat="server" /></td>
                    <td class="ListingRowDate"><asp:Label ID="CreateDateLabel" runat="server" /></td>
                    <td class="ListingRowId"><asp:Label ID="ManxAdsIdLabel" runat="server" /></td>
                </tr>
                <tr>
                    <td class="BrowserDetailsSeparator"></td>
                </tr>
                <tr class="BrowserDetailsDescription">
                    <td colspan="4">
                        <asp:Panel ID="TraderPanel" runat="server" CssClass="TraderLayer" Visible="False">
                            <asp:Image ID="TraderLogoImage" runat="server" />
                        </asp:Panel>
                        <asp:Label ID="DescriptionLabel" runat="server" />
                        <asp:Panel ID="EditorPanel" runat="server" Visible="False">
                            <p>
                                <asp:LinkButton ID="ModifyLinkButton" runat="server" OnClick="EditorLinkButton_Click">Edit</asp:LinkButton> |
                                <asp:LinkButton ID="RemoveLinkButton" runat="server" OnClick="EditorLinkButton_Click">Delete</asp:LinkButton>
                                <asp:LinkButton ID="RestoreLinkButton" runat="server" Visible="False" OnClick="RestoreLinkButton_Click">Restore</asp:LinkButton> |
                                <asp:LinkButton ID="BoostLinkButton" runat="server" OnClick="BoostLinkButton_Click">Boost</asp:LinkButton>
                            </p>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>