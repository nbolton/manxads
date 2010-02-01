<%@ Page Language="C#" MasterPageFile="~/Master.master" AutoEventWireup="true" Inherits="ProfileEditor" Title="Edit Profile" Codebehind="ProfileEditor.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1>Edit Profile</h1>
    <p>
        <asp:LinkButton ID="PersonalDetailsLinkButton" runat="server" OnClick="PersonalDetailsLinkButton_Click" CausesValidation="false">My Details</asp:LinkButton> |
        <asp:LinkButton ID="SettingsLinkButton" runat="server" OnClick="SettingsLinkButton_Click" CausesValidation="false">My Settings</asp:LinkButton> | 
        <asp:LinkButton ID="ChanageEmailLinkButton" runat="server" OnClick="ChanageEmailLinkButton_Click" CausesValidation="false">Change Email Address</asp:LinkButton> |
        <asp:LinkButton ID="ChangePasswordLinkButton" runat="server" OnClick="ChangePasswordLinkButton_Click" CausesValidation="false">Change Password</asp:LinkButton>
    </p>
    <asp:Label ID="ValidationErrorLabel" runat="server" Visible="False" ForeColor="Red">
        <p><asp:Image ID="Image3" runat="server" ImageUrl="~/Images/Static/Layout/ErrorIcon.gif" ImageAlign="TextTop" />&nbsp;There was a problem validating the correctness of some fields, please check and try again.</p>
    </asp:Label>
    <asp:MultiView ID="MultiView" runat="server" OnActiveViewChanged="MultiView_ActiveViewChanged">
        <asp:View ID="DefaultView" runat="server">
            <p>From the above, select the area of your profile your want to edit.</p>
        </asp:View>
        <asp:View ID="FinishedView" runat="server">
            <p>Thank you for updating your profile. Return to <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/UserHome.aspx">my homepage</asp:HyperLink>.</p>
        </asp:View>
        <asp:View ID="PersonalDetailsView" runat="server" OnActivate="PersonalDetailsView_Activate">
            <div class="GenericFormHeader">Personal Details</div>
            <div class="GenericFormBody">
                <asp:Panel ID="TraderPanel" runat="server" Visible="False">
                    <div class="GenericFormSegment">
                        <h1>Trader Name</h1>
                        <asp:TextBox ID="TraderNameTextBox" Width="200px" runat="server" CssClass="GenericFormTextBox" />
                    </div>
                    <div class="GenericFormSegment">
                        <h1>Trader Website</h1>
                        <asp:TextBox ID="TraderWebsiteTextBox" Width="300px" runat="server" CssClass="GenericFormTextBox" />
                    </div>
                </asp:Panel>
                <div class="GenericFormSegment">
                    <h1>First &amp; Last Names</h1>
                    <asp:DropDownList ID="TitleDropDownList" runat="server" />
                    <asp:TextBox ID="FirstNameTextBox" Width="70px" runat="server" CssClass="GenericFormTextBox" /> 
                    <asp:TextBox ID="LastNameTextBox" Width="70px" runat="server" CssClass="GenericFormTextBox" />
                    <asp:RequiredFieldValidator ID="FirstNameRequiredFieldValidator" runat="server" ControlToValidate="FirstNameTextBox" Display="Dynamic">
                        <br />You must enter the first name in the first box.
                    </asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="LastNameRequiredFieldValidator" runat="server" ControlToValidate="LastNameTextBox" Display="Dynamic">
                        <br />You must enter the last name in the second box.
                    </asp:RequiredFieldValidator>
                </div>
                <div class="GenericFormSegment">
                    <h1>Mobile Telephone Number</h1>
                    <asp:TextBox ID="MobileAreaTextBox" Width="40px" runat="server" CssClass="GenericFormTextBox" Text="07624" /> <asp:TextBox ID="MobileNumberTextBox" Width="60px" runat="server" CssClass="GenericFormTextBox" />
                    <asp:RegularExpressionValidator ID="MobileNumberRegularExpressionValidator" runat="server" ControlToValidate="MobileNumberTextBox" ValidationExpression="^[ \d]+$" Display="Dynamic">
                        <br />The phone number you entered can only contain numbers and spaces.
                    </asp:RegularExpressionValidator>
                    <asp:CustomValidator ID="MobileNumberCustomValidator" runat="server" Display="Dynamic" OnServerValidate="ValidateLandlineAndMobile">
                        <br />You must enter either a mobile or a landline telephone number.
                    </asp:CustomValidator>
                </div>
                <div class="GenericFormSegment">
                    <h1>Landline Telephone Number</h1>
                    <asp:TextBox ID="LandlineAreaTextBox" Width="40px" runat="server" CssClass="GenericFormTextBox" Text="01624" /> <asp:TextBox ID="LandlineNumberTextBox" Width="60px" runat="server" CssClass="GenericFormTextBox" />
                    <asp:RegularExpressionValidator ID="LandlineNumberRegularExpressionValidator" runat="server" ControlToValidate="LandlineNumberTextBox" ValidationExpression="^[ \d]+$" Display="Dynamic">
                        <br />The phone number you entered can only contain numbers and spaces.
                    </asp:RegularExpressionValidator>
                    <asp:CustomValidator ID="LandlineNumberCustomValidator" runat="server" Display="Dynamic" OnServerValidate="ValidateLandlineAndMobile">
                        <br />You must enter either a mobile or a landline telephone number.
                    </asp:CustomValidator>
                </div>
                <div class="GenericFormSegment">
                    <h1>Nearest Location</h1>
                    <p>Select your location (or nearest) from this list.</p>
                    <asp:DropDownList ID="LocationDropDownList" runat="server" />
                </div>
            </div>
            <br class="GenericFormSeparator" />
            <asp:ImageButton ID="PersonalDetailsContinueButton" runat="server" ImageUrl="~/Images/Static/Layout/ContinueButton.gif" AlternateText="Continue" CausesValidation="False" OnClick="PersonalDetailsContinueButton_Click" />
        </asp:View>
        <asp:View ID="ChangeEmailAddressView" runat="server" OnActivate="ChangeEmailAddressView_Activate">
            <div class="GenericFormHeader">Change Email Address</div>
            <div class="GenericFormBody">
                <div class="GenericFormSegment">
                    <h1>New Email Address</h1>
                    <asp:TextBox ID="EmailAddressTextBox" Width="250px" runat="server" CssClass="GenericFormTextBox" />
                    <asp:RequiredFieldValidator ID="EmailAddressRequiredFieldValidator" runat="server" ControlToValidate="EmailAddressTextBox" Display="Dynamic">
                        <br />You must type your email address.
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="EmailAddressRegularExpressionValidator" runat="server" ControlToValidate="EmailAddressTextBox" Display="Dynamic" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                        <br />The address does not appear to be valid.
                    </asp:RegularExpressionValidator>
                    <asp:CustomValidator ID="EmailAddressCustomValidator" runat="server" ControlToValidate="EmailAddressTextBox" Display="Dynamic" OnServerValidate="EmailAddressCustomValidator_ServerValidate">
                        <br />This email address is already in use either by yourself or another user.
                    </asp:CustomValidator>
                </div>
                <div class="GenericFormSegment">
                    <h1>Retype Email Address</h1>
                    <p>Please retype your email address to verify correctness.</p>
                    <asp:TextBox ID="EmailRetypeAddressTextBox" Width="250px" runat="server" CssClass="GenericFormTextBox" />
                    <asp:RequiredFieldValidator ID="EmailRetypeAddressRequiredFieldValidator" runat="server" ControlToValidate="EmailRetypeAddressTextBox" Display="Dynamic">
                        <br />You must retype your email address.
                    </asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="EmailRetypeAddressCompareValidator" runat="server" ControlToCompare="EmailAddressTextBox" ControlToValidate="EmailRetypeAddressTextBox" Display="Dynamic">
                        <br />Your email addresses do not match, please re-type them.
                    </asp:CompareValidator>
                </div>
            </div>
            <br class="GenericFormSeparator" />
            <asp:ImageButton ID="ChangeEmailAddressContinueButton" runat="server" ImageUrl="~/Images/Static/Layout/ContinueButton.gif" AlternateText="Continue" CausesValidation="False" OnClick="ChangeEmailAddressContinueButton_Click" />
        </asp:View>
        <asp:View ID="ChangePasswordView" runat="server" OnActivate="ChangePasswordView_Activate">
            <div class="GenericFormHeader">Change Password</div>
            <div class="GenericFormBody">
                <div class="GenericFormSegment">
                    <h1>New Password</h1>
                    <p>Choose a password containing letters and numbers.</p>
                    <asp:TextBox ID="PasswordTextBox" Width="150px" runat="server" TextMode="Password" CssClass="GenericFormTextBox" />
                    <asp:RequiredFieldValidator ID="PasswordRequiredFieldValidator" runat="server" ControlToValidate="PasswordTextBox" Display="Dynamic">
                        <br />Please type your password.
                    </asp:RequiredFieldValidator>
                </div>
                <div class="GenericFormSegment">
                    <h1>Re-type Password</h1>
                    <p>Please re-type your password to verify correctness.</p>
                    <asp:TextBox ID="PasswordRetypeTextBox" Width="150px" runat="server" TextMode="Password" CssClass="GenericFormTextBox" />
                    <asp:RequiredFieldValidator ID="PasswordRetypeRequiredFieldValidator" runat="server" ControlToValidate="PasswordRetypeTextBox" Display="Dynamic">
                        <br />Please type your password.
                    </asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="PasswordRetypeCompareValidator" runat="server" ControlToCompare="PasswordTextBox" ControlToValidate="PasswordRetypeTextBox" Display="Dynamic">
                        <br />Your passwords do not match, please re-type them.
                    </asp:CompareValidator>
                </div>
            </div>
            <br class="GenericFormSeparator" />
            <asp:ImageButton ID="ChangePasswordContinueButton" runat="server" ImageUrl="~/Images/Static/Layout/ContinueButton.gif" AlternateText="Continue" CausesValidation="False" OnClick="ChangePasswordContinueButton_Click" />
        </asp:View>
        <asp:View ID="SettingsView" runat="server" OnActivate="SettingsView_Activate">
            <div class="GenericFormHeader">Settings</div>
            <div class="GenericFormBody">
                <div class="GenericFormSegment">
                    <h1>Listings Display Mode</h1>
                    <asp:CheckBox ID="ListingsPopupCheckBox" runat="server" Text="Open listings in a new window." />
                </div>
            </div>
            <br class="GenericFormSeparator" />
            <asp:ImageButton ID="SettingsContinueButton" runat="server" ImageUrl="~/Images/Static/Layout/ContinueButton.gif" AlternateText="Continue" CausesValidation="False" OnClick="SettingsContinueButton_Click" />
        </asp:View>
    </asp:MultiView>
</asp:Content>

