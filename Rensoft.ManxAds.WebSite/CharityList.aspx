<%@ Page Language="C#" MasterPageFile="~/Master.master" AutoEventWireup="true" Inherits="CharityList" Title="Charities" Codebehind="CharityList.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1>ManxAds Charites</h1>
    <p>We support Manx charities! If you would like to have your charity or company on ManxAds, simply <a href="Help.aspx?Topic=Trader">find out more</a>!</p>
    <asp:GridView ID="ItemGridView" runat="server" AutoGenerateColumns="False" GridLines="None" OnRowDataBound="ItemGridView_RowDataBound" ShowHeader="False" Width="100%">
        <Columns>
            <asp:TemplateField><ItemTemplate><asp:Label CssClass="TraderGridViewName" ID="NameLabel" runat="server" /></ItemTemplate></asp:TemplateField>
            <asp:TemplateField><ItemTemplate><asp:HyperLink ID="ListingsHyperLink" runat="server">Listings</asp:HyperLink> (<asp:Label ID="ListingCountLabel" runat="server" />)</ItemTemplate></asp:TemplateField>
            <asp:TemplateField><ItemTemplate><asp:HyperLink ID="WebsiteHyperLink" runat="server" Target="_blank">Website</asp:HyperLink></ItemTemplate></asp:TemplateField>
        </Columns>
    </asp:GridView>
    <h3>Not on the list?</h3>
    <p>Register on ManxAds and upgrade to a charity account, then post 5 or more listings - it's that easy!</p>
</asp:Content>

