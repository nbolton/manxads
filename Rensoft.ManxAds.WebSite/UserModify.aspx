<%@ Page Language="C#" MasterPageFile="~/Master.master" AutoEventWireup="true" Inherits="UserModify" Title="Edit User" Codebehind="UserModify.aspx.cs" %>
<%@ Register Src="SuccessDialog.ascx" TagName="SuccessDialog" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1><asp:Label ID="TitleLabel" runat="server" /></h1>
    <asp:MultiView ID="MultiView" runat="server">
        <asp:View ID="NotFoundView" runat="server">
            <p><asp:Image ID="Image3" runat="server" ImageUrl="~/Images/Static/Layout/ErrorIcon.gif" ImageAlign="TextTop" />&nbsp;The user you specified was not found.</p>
        </asp:View>
        <asp:View ID="DefaultView" runat="server">
            <asp:Label ID="ValidationErrorLabel" runat="server" Visible="False" ForeColor="Red">
                <p><asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Static/Layout/ErrorIcon.gif" ImageAlign="TextTop" />&nbsp;There was a problem validating the correctness of some fields, please check and try again.</p>
            </asp:Label>
            <br class="GenericFormSeparator" />
            <div class="GenericFormHeader">Login Information</div>
            <div class="GenericFormBody">
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
                    <h1>Town or Village</h1>
                    <p>Select the user's town or village from this list.</p>
                    <asp:DropDownList ID="LocationDropDownList" runat="server" />
                </div>
                <div class="GenericFormSegment">
                    <h1>User Type</h1>
                    <asp:DropDownList ID="UserTypeDropDownList" runat="server" />
                    <asp:Label ID="UserLevelWarningLabel" runat="server" Visible="false">
                        <p style="color: Red">You cannot change your own user type.</p>
                    </asp:Label>
                </div>
                <div class="GenericFormSegment">
                    <h1>Trader Type</h1>
                    <asp:DropDownList ID="TraderTypeDropDownList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="TraderTypeDropDownList_SelectedIndexChanged" />
                </div>
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
            </div>
            <br class="GenericFormSeparator" />
            <div class="GenericFormHeader">Login Information</div>
            <div class="GenericFormBody">
                <div class="GenericFormSegment">
                    <h1>Email Address</h1>
                    <asp:TextBox ID="EmailAddressTextBox" Width="250px" runat="server" CssClass="GenericFormTextBox" />
                    <asp:RequiredFieldValidator ID="EmailAddressRequiredFieldValidator" runat="server" ControlToValidate="EmailAddressTextBox" Display="Dynamic">
                        <br />Please type your email address.
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="EmailAddressRegularExpressionValidator" runat="server" ControlToValidate="EmailAddressTextBox" Display="Dynamic" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                        <br />The address does not appear to be valid.
                    </asp:RegularExpressionValidator>
                </div>
                <div class="GenericFormSegment">
                    <h1>Change Password</h1>
                    <p>Type a new password if you wish to change the existing one.</p>
                    <asp:TextBox ID="PasswordTextBox" Width="150px" runat="server" TextMode="Password" CssClass="GenericFormTextBox" />
                </div>
                <div class="GenericFormSegment">
                    <h1>Re-type Password</h1>
                    <p>Please re-type the password to verify correctness.</p>
                    <asp:TextBox ID="PasswordRetypeTextBox" Width="150px" runat="server" TextMode="Password" CssClass="GenericFormTextBox" />
                    <asp:CompareValidator ID="PasswordRetypeCompareValidator" runat="server" ControlToCompare="PasswordTextBox" ControlToValidate="PasswordRetypeTextBox" Display="Dynamic">
                        <br />Your passwords do not match, please re-type them.
                    </asp:CompareValidator>
                </div>
            </div>
            <br class="GenericFormSeparator" />
            <asp:Panel ID="SettingsPanel" runat="server" Visible="false">
                <div class="GenericFormHeader">User Settings</div>
                <div class="GenericFormBody">
                    <div class="GenericFormSegment">
                        <h1>Listing Limit</h1>
                        <p>The number of listings the user can post will be limited to this number.</p>
                        <asp:TextBox ID="ListingLimitTextBox" Width="20px" runat="server" CssClass="GenericFormTextBox" />
                        <asp:LinkButton ID="DefaultListingLimitLinkButton" runat="server" OnClick="DefaultListingLimitLinkButton_Click">Default</asp:LinkButton>
                    </div>
                    <div class="GenericFormSegment">
                        <h1>Display Adverts</h1>
                        <asp:CheckBox ID="EnableAdvertsCheckBox" runat="server" Text="Yes" />
                    </div>
                </div>
                <br class="GenericFormSeparator" />
            </asp:Panel>
            <div class="GenericFormHeader">Small Print</div>
            <div class="GenericFormBody">
                <asp:CheckBox ID="OptOutCheckBox" runat="server" Text="The user would like to opt-out of any marketing or special offer emails." />
            </div>
            <br class="GenericFormSeparator" />
            <div class="GenericFormHeader">Validation Email</div>
            <div class="GenericFormBody">
                <div class="GenericFormSegment">
                    <asp:Button ID="ValidationEmailButton" runat="server" Text="Resend Validation Email" OnClick="ValidationEmailButton_Click" />
                    <asp:Label ID="ValidationEmailSentLabel" runat="server" ForeColor="Green" Visible="False">Validation email has been resent.</asp:Label>
                </div>
            </div>
            <br class="GenericFormSeparator" />
            <asp:ImageButton ID="ContinueButton" runat="server" ImageUrl="~/Images/Static/Layout/ContinueButton.gif" AlternateText="Continue" CausesValidation="False" OnClick="ContinueButton_Click" />
        </asp:View>
        <asp:View ID="RemoveView" runat="server">
            <p>Are you sure you want to delete the user called '<asp:Label ID="RemoveTitleLabel" runat="server" />'?</p>
            <p>
                <asp:ImageButton ID="RemoveContinueButton" runat="server" ImageUrl="~/Images/Static/Layout/DeleteButton.gif" AlternateText="Delete" CausesValidation="False" OnClick="RemoveContinueButton_Click" />
                <asp:ImageButton ID="RemoveCancelButton" runat="server" ImageUrl="~/Images/Static/Layout/CancelButton.gif" AlternateText="Cancel" CausesValidation="False" OnClick="RemoveCancelButton_Click" />
            </p>
        </asp:View>
        <asp:View ID="SuccessView" runat="server">
            <uc3:SuccessDialog id="SuccessDialog1" runat="server"></uc3:SuccessDialog>
        </asp:View>
        <asp:View ID="BanStartView" runat="server">
            <p>To ban the user '<asp:Label ID="BanTitleLabel" runat="server" />', choose a message and a ban period.</p>
            
            <div class="GenericFormHeader">Options</div>
            <div class="GenericFormBody">
                <div class="GenericFormSegment">
                    <h1>Message</h1>
                    <p>The email to the user will contain this message.</p>
                    <asp:TextBox ID="BanMessageTextBox" Text="You have been banned from ManxAds for violating the Listing Policy, and will be unable to log in until the ban expires." Width="350px" Height="100px" runat="server" CssClass="GenericFormTextBox" TextMode="MultiLine" />
                </div>
                <div class="GenericFormSegment">
                    <h1>Ban period (days)</h1>
                    <p>Set the number to -1 to make the ban permenant.</p>
                    <asp:TextBox ID="BanPeriodTextBox" Text="7" Width="20px" runat="server" CssClass="GenericFormTextBox" />
                </div>
            </div>
            <br class="GenericFormSeparator" />
            <asp:ImageButton ID="BanContinueImageButton" runat="server" 
                ImageUrl="~/Images/Static/Layout/ContinueButton.gif" AlternateText="Delete" 
                CausesValidation="False" onclick="BanContinueImageButton_Click" />
        </asp:View>
        <asp:View ID="BanFinishView" runat="server">
            <h3>User banned</h3>
            <p>You have banned the user <asp:Label ID="BanUntilDateLabel" runat="server" />. The user cannot login until after this date.</p>
        </asp:View>
    </asp:MultiView>
</asp:Content>

