<%@ Control Language="C#" AutoEventWireup="true" Inherits="CategoryRow" Codebehind="CategoryRow.ascx.cs" %>

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
                    <td class="CategoryRowTitle"><h2><asp:HyperLink ID="TitleHyperLink" runat="server" /></h2></td>
                    <td class="CategoryRowListingCount"><asp:Label ID="ListingCountLabel" runat="server" /></td>
                    <td class="CategoryRowLatestListing"><asp:Label ID="LatestListingLabel" runat="server" /></td>
                </tr>
                <tr>
                    <td class="BrowserDetailsSeparator"></td>
                </tr>
                <tr class="BrowserDetailsDescription">
                    <td colspan="3">
                        <p><asp:Label ID="DescriptionLabel" runat="server" /></p>
                        <asp:Panel ID="EditorPanel" runat="server" Visible="False">
                            <asp:Literal ID="DatabaseIdLiteral" runat="server" Visible="False" />
                            <asp:LinkButton ID="ModifyLinkButton" runat="server" OnClick="EditorLinkButton_Click">Edit</asp:LinkButton> |
                            <asp:LinkButton ID="RemoveLinkButton" runat="server" OnClick="EditorLinkButton_Click">Delete</asp:LinkButton> |
                            Priority <asp:TextBox ID="PriorityTextBox" Width="15px" runat="server" CssClass="GenericFormTextBox" EnableTheming="True" OnTextChanged="PriorityTextBox_TextChanged" /> <asp:LinkButton ID="UpdateLinkButton" runat="server" OnClick="UpdateLinkButton_Click">Update</asp:LinkButton>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>