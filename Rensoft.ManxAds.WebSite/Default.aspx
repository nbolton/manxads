<%@ Page Language="C#" MasterPageFile="~/Master.master" AutoEventWireup="true" Inherits="_Default" Codebehind="Default.aspx.cs" %>

<%@ Register Src="ListingDetails.ascx" TagName="ListingDetails" TagPrefix="uc3" %>

<%@ Register Src="CategoryNanoRow.ascx" TagName="CategoryNanoRow" TagPrefix="uc2" %>
<%@ Register Src="ListingMiniRow.ascx" TagName="ListingMiniRow" TagPrefix="uc1" %>

<%@ Register src="RandomNotice.ascx" tagname="RandomNotice" tagprefix="uc5" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:MultiView ID="MultiView" runat="server">
        <asp:View ID="HomeView" runat="server">
        
            <uc5:RandomNotice ID="RandomNotice1" runat="server" />
            
            <table width="100%">
                <tr>
                    <td id="WelcomeColumnLeft">
                        <asp:DataList ID="ListingsDataList" runat="server" Width="100%" CssClass="BrowserMini">
                            <HeaderTemplate>
                                Classifieds with Pictures (<asp:HyperLink ID="HyperLink" runat="server" NavigateUrl="~/Listings.aspx">all</asp:HyperLink>)</HeaderTemplate><ItemTemplate>
                                <uc1:ListingMiniRow ID="ListingMiniRow" runat="server" />
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <uc1:ListingMiniRow ID="ListingMiniRow" runat="server" />
                            </AlternatingItemTemplate>
                            <HeaderStyle CssClass="BrowserMiniHeader" />
                            <ItemStyle CssClass="BrowserRow" />
                            <AlternatingItemStyle CssClass="BrowserRowAlternating" />
                        </asp:DataList>
                    </td>
                    <td id="WelcomeColumnRight">
                        <asp:DataList ID="CategoriesDataList" runat="server" Width="100%" CssClass="BrowserNano">
                            <HeaderTemplate>
                                Categories (<asp:HyperLink ID="HyperLink" runat="server" NavigateUrl="~/Categories.aspx">all</asp:HyperLink>)</HeaderTemplate><ItemTemplate>
                                <uc2:CategoryNanoRow ID="CategoryNanoRow" runat="server" Title='<%# Bind("Title") %>' NavigateUrl='<%# Bind("NavigateUrl") %>' ListingCountWithZeros='<%# Bind("ListingCountWithZeros") %>' />
                            </ItemTemplate>
                            <HeaderStyle CssClass="BrowserMiniHeader" />
                            <ItemStyle CssClass="BrowserNanoRow" />
                        </asp:DataList>
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="DetailsView" runat="server">
            <uc3:ListingDetails ID="ListingDetails1" runat="server" />
        </asp:View>
    </asp:MultiView>
</asp:Content>

