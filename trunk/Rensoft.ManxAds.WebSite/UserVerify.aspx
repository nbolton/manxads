<%@ Page Language="C#" MasterPageFile="~/Master.master" AutoEventWireup="true" Inherits="UserVerify" Title="Registry Confirmation" Codebehind="UserVerify.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Literal ID="UserIdLiteral" runat="server" Visible="False" />
    <h1>Registry Confirmation</h1>
    <asp:MultiView ID="ResultMultiView" runat="server">
        <asp:View ID="SuccessView" runat="server">
            <p>Congratulations, your ManxAds account is now verified!</p>
            <p>Please, <asp:LinkButton ID="LogOnLinkButton" runat="server" OnClick="LogOnLinkButton_Click">proceed to My ManxAds</asp:LinkButton>.</p>
        </asp:View>
        <asp:View ID="FailureView" runat="server">
            <p>We're sorry. Your ManxAds account could not be verified.</p>
            <h3>Suggestions:</h3>
            <ul>
                <li>If you have copied and pased the link, please make sure it is typed correctly.</li>
                <li>Make sure hyperlink in the confirmation email is not corrupt or incomplete.</li>
                <li>Try clicking 'Resend Validation Email' on your My ManxAds webpage.</li>
                <li>Still having problems? Please <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/Contact.aspx">contact us</asp:HyperLink>.</li>
            </ul>
        </asp:View>
    </asp:MultiView>
</asp:Content>

