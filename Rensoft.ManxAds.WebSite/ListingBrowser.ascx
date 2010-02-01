<%@ Control Language="C#" AutoEventWireup="true" Inherits="ListingBrowser" Codebehind="ListingBrowser.ascx.cs" %>

<%@ Register Src="SearchDialog.ascx" TagName="SearchDialog" TagPrefix="uc6" %>
<%@ Register Src="TraderListingsTip.ascx" TagName="TraderListingsTip" TagPrefix="uc5" %>
<%@ Register Src="BreadcrumbTrail.ascx" TagName="BreadcrumbTrail" TagPrefix="uc4" %>
<%@ Register Src="PagingAssistant.ascx" TagName="PagingAssistant" TagPrefix="uc3" %>
<%@ Register Src="PriceColoursTip.ascx" TagName="PriceColoursTip" TagPrefix="uc2" %>
<%@ Register Src="ListingRow.ascx" TagName="ListingRow" TagPrefix="uc1" %>
<%@ Register src="DonateControl.ascx" tagname="DonateControl" tagprefix="uc7" %>
    
<uc4:BreadcrumbTrail ID="BreadcrumbTrail" runat="server" Visible="False" />
<h1><asp:Label ID="TitleLabel" runat="server">ManxAds Listings</asp:Label></h1>

<asp:MultiView ID="MultiView" runat="server">
        
    <asp:View ID="DefaultView" runat="server">
        <asp:Panel ID="DescriptionPanel" runat="server">
            <asp:Label ID="DescriptionLabel" runat="server">Showing all ManxAds listings. Select a sort order, or use the <asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl="~/Search.aspx?Advanced=1">Advanced Search</asp:HyperLink>.</asp:Label>
        </asp:Panel>
        <asp:Panel ID="SortingPanel" Visible="False" runat="server">
            <p>
                <asp:DropDownList ID="SortDropDownList" runat="server">
                    <asp:ListItem Value="Boosted">Boost Date</asp:ListItem>
                    <asp:ListItem Value="Listed">Listed Date</asp:ListItem>
                    <asp:ListItem Value="PriceAsc">Price (Lowest First)</asp:ListItem>
                    <asp:ListItem Value="PriceDesc">Price (Highest First)</asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="SortButton" runat="server" Text="Sort" OnClick="SortButton_Click" />
            </p>
        </asp:Panel>
        <asp:Panel ID="SearchPanel" runat="server" Visible="False">
            <uc6:SearchDialog ID="SearchDialog" runat="server" />
            <p><asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/Help.aspx?Topic=Search#result_ranking">Why do my listings appear low in results?</asp:HyperLink></p>
        </asp:Panel>
        <asp:Panel ID="UserHomePanel" runat="server" Visible="False">
            <p>Return to <asp:HyperLink runat="server" ID="UserHomeHyperLink" NavigateUrl="~/UserHome.aspx">My ManxAds</asp:HyperLink> (your account).</p>
        </asp:Panel>
        
        <div class="PayPalDonationListingBrowser">
           <uc7:DonateControl ID="DonateControl1" runat="server" DonateID="7154494" />
        </div>
        
        <asp:DataList ID="ListingDataList" runat="server" Width="100%" OnItemDataBound="ListingDataList_ItemDataBound">
            <HeaderTemplate>
                <table width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="ListingHeaderTitle">Title</td>
                        <td class="ListingHeaderPrice">Price</td>
                        <td class="ListingHeaderDate">Listed</td>
                        <td class="ListingHeaderId">Tag</td>
                    </tr>
                </table>
            </HeaderTemplate>
            <ItemTemplate>
                <uc1:ListingRow ID="ListingRow" runat="server" OnListingUpdated="ListingRow_ListingUpdated" OnBoostRequest="ListingRow_BoostRequest" OnRestoreListing="ListingRow_RestoreListing" />
            </ItemTemplate>
            <AlternatingItemTemplate>
                <uc1:ListingRow ID="ListingRow" runat="server" OnListingUpdated="ListingRow_ListingUpdated" OnBoostRequest="ListingRow_BoostRequest" OnRestoreListing="ListingRow_RestoreListing" />
            </AlternatingItemTemplate>
            <HeaderStyle CssClass="BrowserHeader" />
            <ItemStyle CssClass="BrowserRow" />
            <AlternatingItemStyle CssClass="BrowserRowAlternating" />
            <FooterTemplate>
                <uc3:PagingAssistant ID="PagingAssistant" runat="server" OnBeforePageLoad="UpdatePagingAssistant" />
            </FooterTemplate>
        </asp:DataList>
        <uc2:PriceColoursTip ID="PriceColoursTip1" runat="server" />
        <uc5:TraderListingsTip ID="TraderListingsTip1" runat="server" />
    </asp:View>
    
    <asp:View ID="EmptyView" runat="server" OnActivate="EmptyView_Activate">
        <p>Be the first to <asp:HyperLink ID="FirstPostHyperLink" runat="server">post a listing</asp:HyperLink> in this category.</p>
    </asp:View>
    
    <asp:View ID="UserEmptyView" runat="server">
        <p>You haven't posted any listings yet. Post your <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/ListingModify.aspx?Create=1">first listing</asp:HyperLink> right now.</p>
    </asp:View>
    
    <asp:View ID="RecycleBinEmptyView" runat="server">
        <p>Your recycle bin is empty. When you delete a listing, it will appear here so you can restore it.</p>
        <p>Return to <asp:HyperLink runat="server" ID="HyperLink1" NavigateUrl="~/UserHome.aspx">My ManxAds</asp:HyperLink> (your account).</p>
    </asp:View>
    
    <asp:View ID="ErrorView" runat="server">
        <p>The specified category does not exist.</p>
    </asp:View>
    
    <asp:View ID="SearchErrorView" runat="server">
        <h1>ManxAds Search</h1>
        <p>You did not search for anything. Please type some words to search for.</p>
        <uc6:SearchDialog ID="SearchDialog1" runat="server" />
    </asp:View>
    
    <asp:View ID="SearchEmptyView" runat="server">
        <p>Your search did not match any listings.</p>
        <h3>Suggestions:</h3>
        <ul>
            <li>Make sure all words are spelled correctly.</li>
            <li>Try different more general keywords.</li>
            <li>Search in all categories and locations.</li>
            <li>Use broader price and date ranges.</li>
        </ul>
        <uc6:SearchDialog ID="SearchDialog2" runat="server" />
    </asp:View>
    
    <asp:View ID="SellerNotFoundView" runat="server">
        <p>The seller was not found. It is possible they are no longer a registered user.</p>
    </asp:View>
    
    <asp:View ID="SellerEmptyView" runat="server">
        <p>This seller has no listings. Try searching for another seller.</p>
    </asp:View>
    
    <asp:View ID="PageNotFoundView" runat="server" OnActivate="PageNotFoundView_Activate">
        <p>The page number you requested no longer exists. </p>
        <h3>Suggestions:</h3>
        <ul>
            <li>Return to the <asp:HyperLink ID="FistPageHyperLink" runat="server">first page</asp:HyperLink>.</li>
        </ul>
    </asp:View>
    
</asp:MultiView>