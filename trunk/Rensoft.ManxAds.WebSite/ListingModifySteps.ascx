<%@ Control Language="C#" AutoEventWireup="true" Inherits="ListingModifySteps" Codebehind="ListingModifySteps.ascx.cs" %>
<asp:MultiView ID="MainMultiView" runat="server">
    <asp:View ID="LinkButtonsView" runat="server">
        <p>
            <asp:LinkButton ID="DetailsLinkButton" runat="server" OnClick="DetailsLinkButton_Click">Details</asp:LinkButton> &gt;
            <asp:LinkButton ID="CategoriesLinkButton" runat="server" OnClick="CategoriesLinkButton_Click">Categories</asp:LinkButton> &gt;
            <asp:LinkButton ID="ImagesLinkButton" runat="server" OnClick="ImagesLinkButton_Click">Images</asp:LinkButton>
        </p>
    </asp:View>
</asp:MultiView>