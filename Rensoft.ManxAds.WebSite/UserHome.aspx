<%@ Page Language="C#" MasterPageFile="~/Master.master" AutoEventWireup="true" Inherits="UserHome" Title="My ManxAds" Codebehind="UserHome.aspx.cs" %>

<%@ Register Src="TraderOptions.ascx" TagName="TraderOptions" TagPrefix="uc1" %>

<%@ Register Src="AdvertiserOptions.ascx" TagName="AdvertiserOptions" TagPrefix="uc4" %>
<%@ Register Src="SellerOptions.ascx" TagName="SellerOptions" TagPrefix="uc5" %>
<%@ Register Src="AdministratorOptions.ascx" TagName="AdministratorOptions" TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:MultiView ID="MultiView" runat="server">
        <asp:View ID="DefaultView" runat="server">
            <h1>My ManxAds</h1>
            <asp:Panel ID="UpgradePanel" CssClass="UpgradeRequestPanel" runat="server" Visible="False">
                <p><b>Thank you</b> for your interest in our Trader &amp; Charity programme. Your upgrade request has been logged on our system. Please wait for us to get in touch with you.</p>
            </asp:Panel>
            <asp:Panel ID="VerifyResentPanel" CssClass="UserVerifyResentPanel" runat="server" Visible="False">
                <p>Your validation email has been resent to <b><asp:Label ID="UserEmailLabel" runat="server" /></b>. Please check your email and follow the link provided.</p>
            </asp:Panel>
            <asp:Panel ID="VerifyPanel" CssClass="UserVerifyPanel" runat="server" Visible="False">
                <h3>Please check your email</h3>
                <p>You should have recieved an email by now which contains a link to confirm your registration. If you do not confirm your account <b><asp:Label ID="VerifyTimeoutDaysLabel" runat="server" /></b>, your listings may become hidden from public view. If you have not yet recieved this email, you could <asp:LinkButton ID="ResendValidationEmailLinkButton" runat="server" OnClick="ResendValidationEmailLinkButton_Click">send the validation email again</asp:LinkButton>.</p>
            </asp:Panel>
            <asp:Panel ID="DisabledPanel" CssClass="UserVerifyPanel" runat="server" Visible="False">
                <h3>Temporarily deactivated</h3>
                <p>You should have recieved an email by now which contains a link to confirm your registration. To activate your account, simply follow the link provided in this email. For now, your listings have now been hidden from public view. Once you validate your account, your listings will be displayed again. <asp:LinkButton ID="LinkButton1" runat="server" OnClick="ResendValidationEmailLinkButton_Click">Send the validation email again</asp:LinkButton>.</p>
            </asp:Panel>
            
            <p>Welcome back, <asp:Label runat="server" ID="FullNameLabel" />!</p>
            <uc3:AdministratorOptions ID="AdministratorOptions" runat="server" Visible="False" />
            <uc4:AdvertiserOptions ID="AdvertiserOptions" runat="server"  Visible="False" />
            <uc5:SellerOptions ID="SellerOptions" runat="server" />
            <uc1:TraderOptions ID="TraderOptions" runat="server" Visible="False" />
            <div class="GenericFormHeader">Personal Menu</div>
            <div class="GenericFormBody">
                <ul>
                    <li><asp:HyperLink ID="HyperLink1" NavigateUrl="~/ProfileEditor.aspx" runat="server">Edit Profile</asp:HyperLink></li>
                    <li><asp:LinkButton ID="LogOffLinkButton" runat="server" OnClick="LogOffLinkButton_Click">Log Off</asp:LinkButton></li>
                </ul>
                <asp:Panel ID="TraderLinksPanel" runat="server">
                    <ul>
                        <li><asp:LinkButton ID="TraderRequestLinkButton" runat="server" OnClick="TraderRequestLinkButton_Click">Upgrade to Trader</asp:LinkButton> (<a href="Help.aspx?Topic=Trader" target="_blank">Help</a>)</li>
                        <li><asp:LinkButton ID="CharityRequestLinkButton" runat="server" OnClick="CharityRequestLinkButton_Click">Upgrade to Charity</asp:LinkButton> (<a href="Help.aspx?Topic=Trader" target="_blank">Help</a>)</li>
                    </ul>
                </asp:Panel>
            </dDatabaseId")")
        </asp:View>
        <asp:View ID="BannedView" runat="server">
            <p>We're sorry, but you have been banned from using ManxAds. Please check your email inbox for full details.</p>
        </asp:View>
    </asp:MultiView>
</asp:Content>