<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RandomNotice.ascx.cs" Inherits="Rensoft.ManxAds.WebSite.RandomNotice" %>

<%@ Register src="DonateControl.ascx" tagname="DonateControl" tagprefix="uc4" %>

<div class="HomeGenericNotice">
    <asp:MultiView ID="NoticeMultiView" runat="server" ActiveViewIndex=0>
        
        <asp:View ID="NoticeAddToFavorites" runat="server">
            <p><b>Share the love!</b></p>
            <p>If you like ManxAds, why not tell your friends?</p>
            
            <div class="AddThis">
                <div class="addthis_toolbox addthis_default_style">
                    <a href="http://www.addthis.com/bookmark.php?v=250&amp;username=manxads" class="addthis_button_compact">Share</a>
                    <a class="addthis_button_preferred_1"></a>
                    <a class="addthis_button_preferred_2"></a>
                </div>
            </div>
            <script type="text/javascript">    var addthis_config = { "data_track_clickback": true };</script>
            <script type="text/javascript" src="http://s7.addthis.com/js/250/addthis_widget.js#username=manxads"></script>
            
        </asp:View>
    
        <asp:View ID="NoticeScamView" runat="server">
            <p><b>Please be aware of scams!</b></p>
            <p>Read about <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Help.aspx?Topic=ListingPolicy">scam listings</asp:HyperLink> so you know how to keep safe on ManxAds. Use the link at the bottom of a listing detail page to report scams.</p>
        </asp:View>
        
        <asp:View ID="NoticeDonationView" runat="server">
            <uc4:DonateControl ID="DonateControl1" runat="server" DonateID="7154453" />
        </asp:View>
        
        <asp:View ID="BugsFixedView" runat="server">
            <p><b>Search engine fixed</b></p>
            <p>The search engine bug was resolved on 17/11/2010, and should now be more reliable. Thanks for letting us know!</p>
        </asp:View>
    
        <asp:View ID="NoticeChristmasView" runat="server">
            <div style="text-align: left">
                <div style="float: left; position: absolute">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Static/Layout/ChristmasHolly.gif" />
                </div>
                <div style="text-align: center">
                    <p><b>Ho ho ho! Merry Christmas!</b></p>
                    <p>Find that special <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/Listings.aspx?Search=christmas">Christmas gift</asp:HyperLink> on ManxAds!</p>
                </div>
            </div>
        </asp:View>
        
    </asp:MultiView>
</div>