<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DonateControl.ascx.cs" Inherits="Rensoft.ManxAds.WebSite.DonateControl" %>
<p><b>Donate to ManxAds</b></p>
<p>Your donations help us improve ManxAds. Thank you!</p>
<p>
    <input type="hidden" name="cmd" value="_s-xclick" />
    <input type="hidden" name="hosted_button_id" value="<%=DonateID%>" />
    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="https://www.paypal.com/en_GB/i/btn/btn_donate_SM.gif" PostBackUrl="https://www.paypal.com/cgi-bin/webscr" AlternateText="PayPal - The safer, easier way to pay online." />
</p>