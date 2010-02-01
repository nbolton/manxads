<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RandomNotice.ascx.cs" Inherits="Rensoft.ManxAds.WebSite.RandomNotice" %>

<%@ Register src="DonateControl.ascx" tagname="DonateControl" tagprefix="uc4" %>

<div class="HomeGenericNotice">
    <asp:MultiView ID="NoticeMultiView" runat="server">
        <asp:View ID="NoticeScamView" runat="server">
            <p><b>Please be aware of scams!</b></p>
            <p>Read about <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Help.aspx?Topic=ListingPolicy">scam listings</asp:HyperLink> so you know how to keep safe on ManxAds. Use the links at the bottom of a listing page to report it as a scam.</p>
        </asp:View>
        <asp:View ID="NoticeDonationView" runat="server">
            <uc4:DonateControl ID="DonateControl1" runat="server" DonateID="7154453" />
        </asp:View>
        <asp:View ID="NoticeChristmasView" runat="server">
            <div style="float: left; position: absolute"><asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Static/Layout/ChristmasHolly.gif" /></div>
            <p><b>Merry Christmas!</b></p>
            <p>Grab a last minute <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/Listings.aspx?Search=christmas">Christmas bargain</asp:HyperLink> on ManxAds!</p>
        </asp:View>
        <asp:View ID="BugsFixedView" runat="server">
            <p><b>We've fixed a bug!</b></p>
            <p>The listing report feature is working again, sorry about the hiccup!</p>
        </asp:View>
    </asp:MultiView>
</div>