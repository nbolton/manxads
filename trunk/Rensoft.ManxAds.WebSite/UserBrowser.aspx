<%@ Page Language="C#" MasterPageFile="~/Master.master" AutoEventWireup="true" Inherits="UserBrowser" Title="Users" Codebehind="UserBrowser.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1>Users</h1>
    
    <div><asp:TextBox ID="SearchTextBox" runat="server" /> <asp:Button ID="SearchButton" runat="server" Text="Search" OnClick="SearchButton_Click" /></div>
    
    <p>L = Listing Count, B = Recycle Bin Items, R = Listing Limit Reached, A = Days Since Active, V = User is Verified, D = User is Disabled, E = Email Opt-Out</p>
    <asp:GridView ID="UsersGridView" runat="server" AutoGenerateColumns="False" Width="100%">
        <Columns>
            <asp:BoundField DataField="EmailAddress" HeaderText="Email Address" />
            <asp:TemplateField HeaderText="L" ItemStyle-BackColor="#D9DBFF">
                <ItemTemplate><asp:HyperLink ID="ListingsHyperLink" runat="server" Text='<%# Bind("ListingCount") %>' NavigateUrl='<%# Bind("ListingsUrl") %>' ></asp:HyperLink></ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="RecycleBinCount" HeaderText="B" ItemStyle-BackColor="#FFDBBE" />
            <asp:BoundField DataField="ListingLimitReachedShortString" HeaderText="R" ItemStyle-BackColor="#ECD1FF"     />
            <asp:BoundField DataField="LastActiveShortString" HeaderText="A" ItemStyle-BackColor="#D7FAFF" />
            <asp:BoundField DataField="IsVerifiedShortString" HeaderText="V" ItemStyle-BackColor="#B6FF9C" />
            <asp:BoundField DataField="IsDisabledShortString" HeaderText="D" ItemStyle-BackColor="#FFD1D1" />
            <asp:BoundField DataField="EmailOptOutShortString" HeaderText="E" ItemStyle-BackColor="#FEFFAF" />
            <asp:TemplateField HeaderText="Actions" ItemStyle-Wrap="False">
                <ItemTemplate>
                    <asp:LinkButton ID="ModifyLinkButton" runat="server" CommandArgument='<%# Bind("ModifyUrl") %>' OnClick="EditorLinkButton_Click">Edit</asp:LinkButton>
                    <asp:LinkButton ID="RemoveLinkButton" runat="server" CommandArgument='<%# Bind("RemoveUrl") %>' OnClick="EditorLinkButton_Click">Delete</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>

