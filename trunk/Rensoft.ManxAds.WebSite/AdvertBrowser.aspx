<%@ Page Language="C#" MasterPageFile="~/Master.master" AutoEventWireup="true" Inherits="AdvertBrowser" Title="Browse Adverts" Codebehind="AdvertBrowser.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1>Browse Adverts</h1>
    <br class="GenericFormSeparator" />
    <asp:GridView ID="AdvertsGridView" runat="server" AutoGenerateColumns="False" Width="100%">
        <Columns>
            <asp:BoundField DataField="AdvertiserString" HeaderText="Advertiser" ItemStyle-Wrap="false" ItemStyle-VerticalAlign="top" />
            <asp:BoundField DataField="Title" HeaderText="Title" ItemStyle-VerticalAlign="top" />
            <asp:BoundField DataField="AuthorisedString" HeaderText="Authorised" ItemStyle-Wrap="false" ItemStyle-VerticalAlign="top" />
            <asp:BoundField DataField="HitsMonthString" HeaderText="HM" ItemStyle-VerticalAlign="top" />
            <asp:BoundField DataField="HitsTotalString" HeaderText="HT" ItemStyle-VerticalAlign="top" />
            <asp:TemplateField ItemStyle-Wrap="false" ItemStyle-VerticalAlign="top">
                <ItemTemplate>
                    <asp:LinkButton ID="ModifyLinkButton" runat="server" CommandArgument='<%# Bind("ModifyUrl") %>' OnClick="EditorLinkButton_Click">Edit</asp:LinkButton>
                    <asp:LinkButton ID="RemoveLinkButton" runat="server" CommandArgument='<%# Bind("RemoveUrl") %>' OnClick="EditorLinkButton_Click">Delete</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            <p>You currently have no adverts.</p>
        </EmptyDataTemplate>
    </asp:GridView>
    <br class="GenericFormSeparator" />
    <h3>Key/Legend</h3>
    <ul>
        <li><b>HT:</b> Hits in total</li>
        <li><b>HM:</b> Hits this month</li>
    </ul>
    <h3>Authorisation</h3>
    <p>Your advert may appear as unauthorised, because an administrator must first authorise adverts.</p>
</asp:Content>
