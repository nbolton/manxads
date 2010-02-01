<%@ Page Language="C#" MasterPageFile="~/Master.master" AutoEventWireup="true" Inherits="RecoverPassword" Title="Recover Password" Codebehind="RecoverPassword.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1>Password Recovery</h1>
    <asp:MultiView ID="MultiView" runat="server">
        <asp:View ID="Step1View" runat="server">
            <br class="GenericFormSeparator" />
            <asp:Label ID="EmailNotFoundLabel" runat="server" Visible="False" ForeColor="Red">
                <p><asp:Image ID="Image3" runat="server" ImageUrl="~/Images/Static/Layout/ErrorIcon.gif" ImageAlign="TextTop" />&nbsp;The email you specified is not in use on our system. You may have spelt it incorrectly.</p>
            </asp:Label>
            <div class="GenericFormHeader">First Step</div>
            <div class="GenericFormBody">
                <div class="GenericFormSegment">
                    <h1>Email Address</h1>
                    <asp:TextBox ID="EmailAddressTextBox" Width="250px" runat="server" CssClass="GenericFormTextBox" />
                    <asp:RequiredFieldValidator ID="EmailAddressRequiredFieldValidator" runat="server" ControlToValidate="EmailAddressTextBox" Display="Dynamic">
                        <br />You must type your email address.
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="EmailAddressRegularExpressionValidator" runat="server" ControlToValidate="EmailAddressTextBox" Display="Dynamic" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                        <br />The address does not appear to be valid.
                    </asp:RegularExpressionValidator>
                </div>
                <div class="GenericFormSegment">
                    <h1>Re-type Email Address</h1>
                    <p>Please re-type your email address to verify correctness.</p>
                    <asp:TextBox ID="EmailRetypeAddressTextBox" Width="250px" runat="server" CssClass="GenericFormTextBox" />
                    <asp:RequiredFieldValidator ID="EmailRetypeAddressRequiredFieldValidator" runat="server" ControlToValidate="EmailRetypeAddressTextBox" Display="Dynamic">
                        <br />You must re-type your email address.
                    </asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="EmailRetypeAddressCompareValidator" runat="server" ControlToCompare="EmailAddressTextBox" ControlToValidate="EmailRetypeAddressTextBox" Display="Dynamic">
                        <br />Your email addresses do not match, please re-type them.
                    </asp:CompareValidator>
                </div>
            </div>
            <br class="GenericFormSeparator" />
            <asp:ImageButton ID="Step1ContinueButton" runat="server" ImageUrl="~/Images/Static/Layout/ContinueButton.gif" AlternateText="Continue" CausesValidation="True" OnClick="Step1ContinueButton_Click" />
        </asp:View>
        <asp:View ID="Step1ConfirmationView" runat="server">
            <p>You have been sent an email containing a link to reset your password.</p>
        </asp:View>
        <asp:View ID="Step2BadAuthView" runat="server">
            <p>Sorry, the authorisation code was invalid. You cannot reset your password at this time.</p>
            <p>If this is an old password request, you will need to <asp:HyperLink ID="HyperLink1" NavigateUrl="~/RecoverPassword.aspx" runat="server">re-request your password</asp:HyperLink>.</p>
        </asp:View>
        <asp:View ID="Step2View" runat="server" OnActivate="Step2View_Activate">
            <br class="GenericFormSeparator" />
            <div class="GenericFormHeader">Second Step</div>
            <div class="GenericFormBody">
                <asp:Literal ID="UserIdLiteral" runat="server" Visible="False" />
                <div class="GenericFormSegment">
                    <h1>Password</h1>
                    <p>Choose a password containing letters and numbers.</p>
                    <asp:TextBox ID="PasswordTextBox" Width="150px" runat="server" TextMode="Password" CssClass="GenericFormTextBox" />
                    <asp:RequiredFieldValidator ID="PasswordRequiredFieldValidator" runat="server" ControlToValidate="PasswordTextBox" Display="Dynamic">
                        <br />You must type your password.
                    </asp:RequiredFieldValidator>
                </div>
                <div class="GenericFormSegment">
                    <h1>Re-type Password</h1>
                    <p>Please re-type your password to verify correctness.</p>
                    <asp:TextBox ID="PasswordRetypeTextBox" Width="150px" runat="server" TextMode="Password" CssClass="GenericFormTextBox" />
                    <asp:RequiredFieldValidator ID="PasswordRetypeRequiredFieldValidator" runat="server" ControlToValidate="PasswordRetypeTextBox" Display="Dynamic">
                        <br />You must type your password.
                    </asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="PasswordRetypeCompareValidator" runat="server" ControlToCompare="PasswordTextBox" ControlToValidate="PasswordRetypeTextBox" Display="Dynamic">
                        <br />Your passwords do not match, please re-type them.
                    </asp:CompareValidator>
                </div>
            </div>
            <br class="GenericFormSeparator" />
            <asp:ImageButton ID="Step2ContinueButton" runat="server" ImageUrl="~/Images/Static/Layout/ContinueButton.gif" AlternateText="Continue" CausesValidation="True" OnClick="Step2ContinueButton_Click" />
        </asp:View>
        <asp:View ID="Step2ConfirmationView" runat="server">
            <p>Congratulations! Your password has been reset and you can now <asp:LinkButton ID="LogOnLinkButton" runat="server" OnClick="LogOnLinkButton_Click">return to My ManxAds</asp:LinkButton>.</p>
        </asp:View>
    </asp:MultiView>
</asp:Content>