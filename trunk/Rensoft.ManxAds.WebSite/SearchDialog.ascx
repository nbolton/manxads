<%@ Control Language="C#" AutoEventWireup="true" Inherits="SearchDialog" Codebehind="SearchDialog.ascx.cs" %>
<div class="SearchDialog">
    <asp:MultiView ID="TypeMultiView" runat="server">
        <asp:View ID="SimpleView" runat="server" OnActivate="SimpleView_Activate">
            <h3>Simple Search <asp:Label ID="SellerNameSimpleLabel" runat="server" /></h3>
            <p style="margin-bottom: 0px; padding-bottom: 0px">
                <asp:TextBox CssClass="TextBox" ID="SimpleSearchTextBox" Width="200px" runat="server" />
                <asp:DropDownList ID="SimpleCategoryDropDownList" CssClass="SearchCategoryDropDown" runat="server" />
                <asp:ImageButton CssClass="ImageButton" ID="SimpleSearchButton" runat="server" ImageUrl="~/Images/Static/Layout/SearchButton.gif" OnClick="SimpleSearchButton_Click" CausesValidation="False" />
            </p>
            <p style="margin-top: 0px; padding-top: 0px">
                <asp:LinkButton ID="AdvancedSearchLinkButton" runat="server" OnClick="AdvancedSearchLinkButton_Click">Advanced Search</asp:LinkButton>
            </p>
        </asp:View>
        <asp:View ID="AdvancedView" runat="server" OnActivate="AdvancedView_Activate">
            <h3>Advanced Search <asp:Label ID="SellerNameAdvancedLabel" runat="server" /></h3>
            <asp:Table ID="Table1" runat="server">
                <asp:TableRow>
                    <asp:TableHeaderCell>All the words</asp:TableHeaderCell>
                    <asp:TableCell><asp:TextBox ID="AdvancedSearchTextBox" Width="200px" runat="server" /></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableHeaderCell>Price range</asp:TableHeaderCell>
                    <asp:TableCell>&pound; <asp:TextBox ID="StartPriceTextBox" Width="60px" runat="server" /> to &pound; <asp:TextBox ID="EndPriceTextBox" Width="60px" runat="server" /> <em>optional</em> (e.g. 10 or 99.99)</asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableHeaderCell>Date range</asp:TableHeaderCell>
                    <asp:TableCell><asp:TextBox ID="StartDateTextBox" Width="80px" runat="server" /> to <asp:TextBox ID="EndDateTextBox" Width="80px" runat="server" /> <em>optional</em> (e.g. <asp:Label ID="DateExampleLabel" runat="server" />)</asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableHeaderCell>In category</asp:TableHeaderCell>
                    <asp:TableCell><asp:DropDownList ID="AdvancedCategoryDropDownList" runat="server" /></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableHeaderCell>At location</asp:TableHeaderCell>
                    <asp:TableCell><asp:DropDownList ID="AdvancedLocationDropDownList" runat="server" /></asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <p><asp:ImageButton CssClass="ImageButton" ID="AdvancedSearchButton" runat="server" ImageUrl="~/Images/Static/Layout/SearchButton.gif" OnClick="AdvancedSearchButton_Click" CausesValidation="False" /></p>
            <p style="margin-top: 0px; padding-top: 0px">
                <asp:LinkButton ID="SimpleSearchLinkButton" runat="server" OnClick="SimpleSearchLinkButton_Click">Simple Search</asp:LinkButton>
            </p>
        </asp:View>
    </asp:MultiView>
</div>