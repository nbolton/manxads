<%@ Control Language="C#" AutoEventWireup="true" Inherits="AdvertiserOptions" Codebehind="AdvertiserOptions.ascx.cs" %>
<div class="GenericFormHeader">Advertiser Menu</div>
<div class="GenericFormBody">
    <h3>What do you want to do?</h3>
    <ul>
        <li><asp:HyperLink ID="HyperLink3" NavigateUrl="~/AdvertModify.aspx?Create=1" runat="server">New Advert</asp:HyperLink></li>
        <li><asp:HyperLink ID="HyperLink6" NavigateUrl="~/AdvertBrowser.aspx?Self=1" runat="server">My Adverts</asp:HyperLink></li>
    </ul>
</div>
<br class="GenericFormSeparator" />