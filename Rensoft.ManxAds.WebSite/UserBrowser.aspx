<%@ Page Language="C#" MasterPageFile="~/Master.master" AutoEventWireup="true" Inherits="UserBrowser" Title="Users" Codebehind="UserBrowser.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1>Users</h1>
    <asp:MultiView ID="UserMultiView" runat="server">
        <asp:View ID="OptionsView" runat="server">
            <div><asp:TextBox ID="SearchTextBox" runat="server" /> <asp:Button ID="SearchButton" runat="server" Text="Search" OnClick="SearchButton_Click" /></div>
            <div><asp:LinkButton ID="ViewBannedLinkButton" runat="server" 
                    onclick="ViewBannedLinkButton_Click">View banned</asp:LinkButton></div>
        </asp:View>
        <asp:View ID="SearchView" runat="server">
            <asp:GridView ID="SearchGridView" runat="server" AutoGenerateColumns="False" CellPadding="4">
                <Columns>
                    <asp:BoundField DataField="FullName" HeaderText="Name" />
                    <asp:BoundField DataField="EmailAddress" HeaderText="Email" />
                    <asp:TemplateField HeaderText="L" ItemStyle-BackColor="#D9DBFF">
                        <ItemTemplate><asp:HyperLink ID="ListingsHyperLink" runat="server" Text='<%# Bind("ListingCount") %>' NavigateUrl='<%# Bind("ListingsUrl") %>' ></asp:HyperLink></ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RecycleBinCount" HeaderText="B" ItemStyle-BackColor="#FFDBBE" />
                    <asp:BoundField DataField="ListingLimitReachedShortString" HeaderText="R" ItemStyle-BackColor="#ECD1FF"     />
                    <asp:BoundField DataField="LastActiveShortString" HeaderText="A" ItemStyle-BackColor="#D7FAFF" />
                    <asp:BoundField DataField="IsVerifiedShortString" HeaderText="V" ItemStyle-BackColor="#B6FF9C" />
                    <asp:BoundField DataField="IsDisabledShortString" HeaderText="D" ItemStyle-BackColor="#FFD1D1" />
                    <asp:BoundField DataField="EmailOptOutShortString" HeaderText="E" ItemStyle-BackColor="#FEFFAF" />
                    <asp:BoundField DataField="LastIp" HeaderText="Last IP" />
                    <asp:TemplateField HeaderText="Actions" ItemStyle-Wrap="False">
                        <ItemTemplate>
                            <asp:HyperLink ID="ModifyHyperLink" runat="server" NavigateUrl='<%# Bind("ModifyUrl") %>'>Edit</asp:HyperLink>
                            <asp:HyperLink ID="RemoveHyperLink" runat="server" NavigateUrl='<%# Bind("RemoveUrl") %>'>Delete</asp:HyperLink>
                            <asp:HyperLink ID="BanHyperLink" runat="server" NavigateUrl='<%# Bind("BanUrl") %>'>Ban</asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <p>
                <span style="background-color: #D9DBFF">L</span> = Listing Count<br />
                <span style="background-color: #FFDBBE">B</span> = Recycle Bin Items<br />
                <span style="background-color: #ECD1FF">R</span> = Listing Limit Reached<br />
                <span style="background-color: #D7FAFF">A</span> = Days Since Active<br />
                <span style="background-color: #B6FF9C">V</span> = User is Verified<br />
                <span style="background-color: #FFD1D1">D</span> = User is Disabled<br />
                <span style="background-color: #FEFFAF">E</span> = Email Opt-Out
            </p>
        </asp:View>
        <asp:View ID="BannedView" runat="server">
            <asp:GridView ID="BannedGridView" runat="server" AutoGenerateColumns="False" CellPadding="4">
                <Columns>
                    <asp:BoundField DataField="FullName" HeaderText="Name" />
                    <asp:BoundField DataField="EmailAddress" HeaderText="Email" />
                    <asp:BoundField DataField="BanUntilString" HeaderText="Ban until" />
                    <asp:BoundField DataField="LastIp" HeaderText="Last IP" />
                    <asp:TemplateField HeaderText="Actions" ItemStyle-Wrap="False">
                        <ItemTemplate>
                            <asp:HyperLink ID="UnbanHyperLink" runat="server" NavigateUrl='<%# Bind("UnbanUrl") %>'>Unban</asp:HyperLink>
                            <asp:HyperLink ID="ModifyHyperLink" runat="server" NavigateUrl='<%# Bind("ModifyUrl") %>'>Edit</asp:HyperLink>
                            <asp:HyperLink ID="RemoveHyperLink" runat="server" NavigateUrl='<%# Bind("RemoveUrl") %>'>Delete</asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </asp:View>
    </asp:MultiView>
</asp:Content>

