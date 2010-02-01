<%@ Page Language="C#" MasterPageFile="~/Master.master" AutoEventWireup="true" Inherits="Categories" Title="Categories" Codebehind="Categories.aspx.cs" %>
<%@ Register Src="CategoryRow.ascx" TagName="CategoryRow" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <h1>Categories</h1>
    <p>Find what you’re looking for by clicking on a category.</p>
        
    <asp:DataList ID="CategoryDataList" runat="server" Width="100%" OnItemDataBound="CategoryDataList_ItemDataBound">
        <HeaderTemplate>
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="CategoryHeaderTitle">Title</td>
                    <td class="CategoryHeaderListingCount">Listings</td>
                    <td class="CategoryHeaderLatestListing">Last Listing</td>
                </tr>
            </table>
        </HeaderTemplate>
        <ItemTemplate>
            <uc1:CategoryRow id="CategoryRow" runat="server" Title='<%# Bind("Title") %>' Description='<%# Bind("Description") %>' NavigateUrl='<%# Bind("NavigateUrl") %>' ImageUrl='<%# Bind("ImageUrl") %>' ListingCount='<%# Bind("ListingCountString") %>' LatestListing='<%# Bind("LatestListing") %>' ModifyUrl='<%# Bind("ModifyUrl") %>' RemoveUrl='<%# Bind("RemoveUrl") %>' Priority='<%# Bind("Priority") %>' DatabaseId='<%# Bind("DatabaseId") %>' OnMultiplePrioritiesUpdated="CategoryRow_MultiplePrioritiesUpdated" OnSinglePriorityUpdated="CategoryRow_SinglePriorityUpdated" />
        </ItemTemplate>
        <AlternatingItemTemplate>
            <uc1:CategoryRow id="CategoryRow" runat="server" Title='<%# Bind("Title") %>' Description='<%# Bind("Description") %>' NavigateUrl='<%# Bind("NavigateUrl") %>' ImageUrl='<%# Bind("ImageUrl") %>' ListingCount='<%# Bind("ListingCountString") %>' LatestListing='<%# Bind("LatestListing") %>' ModifyUrl='<%# Bind("ModifyUrl") %>' RemoveUrl='<%# Bind("RemoveUrl") %>' Priority='<%# Bind("Priority") %>' DatabaseId='<%# Bind("DatabaseId") %>' OnMultiplePrioritiesUpdated="CategoryRow_MultiplePrioritiesUpdated" OnSinglePriorityUpdated="CategoryRow_SinglePriorityUpdated" />
        </AlternatingItemTemplate>
        <HeaderStyle CssClass="BrowserHeader" />
        <ItemStyle CssClass="BrowserRow" />
        <AlternatingItemStyle CssClass="BrowserRowAlternating" />
    </asp:DataList>

</asp:Content>

