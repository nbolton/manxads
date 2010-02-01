<%@ Page Language="C#" MasterPageFile="~/Master.master" AutoEventWireup="true" Inherits="Help" Title="Help" Codebehind="Help.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:MultiView ID="HelpMultiView" runat="server">
        <asp:View ID="WelcomeView" runat="server" OnActivate="WelcomeView_Activate">    
            <h1>ManxAds Help</h1>
            <p>Make your ManxAds experience better by absorbing usefull information in our help library. If you can't find what you're looking for, then you should feel free to contact us!</p>
            <h2>Help Topics</h2>
            <ul class="HelpMenu">
                <asp:Repeater ID="HelpMenuRepeater" runat="server">
                    <ItemTemplate>
                        <li>
                            <asp:HyperLink ID="TitleHyperlink" runat="server" Text='<%# Bind("Title") %>' NavigateUrl='<%# Bind("HyperLink") %>' /><br />
                            <asp:Label ID="DescriptionLabel" runat="server" Text='<%# Bind("Description") %>' />
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
        </asp:View>
        <asp:View ID="DetailsView" runat="server">
            <h1><asp:Label ID="TitleLabel" runat="server" /></h1>
            <p><asp:Label ID="DescriptionLabel" runat="server" /></p>
            <asp:MultiView ID="DetailsMultiView" runat="server">
                <asp:View ID="XmlView" runat="server">
                    <asp:Literal ID="BodyLiteral" runat="server" />
                </asp:View>
                <asp:View ID="ControlView" runat="server">
                    <asp:Panel ID="ControlPanel" runat="server" CssClass="HelpControl" />
                </asp:View>
            </asp:MultiView>
            <p>Back to <a href="Help.aspx">Help Topics</a>.</p>
        </asp:View>
        <asp:View ID="InlineView" runat="server" OnLoad="InlineView_Load" />
    </asp:MultiView>
</asp:Content>

