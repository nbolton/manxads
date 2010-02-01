<%@ Page Language="C#" MasterPageFile="~/Master.master" AutoEventWireup="true" Inherits="Register" Title="Register" Codebehind="Register.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1>Register</h1>
    <p>To register on ManxAds for free, fill in the boxes below. If you have forgotten your password, please use the <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/RecoverPassword.aspx">Password Recovery</asp:HyperLink> tool.</p>
    <asp:Label ID="ValidationErrorLabel" runat="server" Visible="False" ForeColor="Red">
        <p><asp:Image ID="Image3" runat="server" ImageUrl="~/Images/Static/Layout/ErrorIcon.gif" ImageAlign="TextTop" />&nbsp;There was a problem validating the correctness of some fields, please check and try again.</p>
    </asp:Label>
    <asp:Label ID="EmailInUseLabel" runat="server" Visible="False" ForeColor="Red">
        <p><asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Static/Layout/ErrorIcon.gif" ImageAlign="TextTop" />&nbsp;The email address you chose was already in use. If you have forgotten your password, please use the <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~\RecoverPassword.aspx">Password Recovery</asp:HyperLink> tool.</p>
    </asp:Label>
    
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
            <h1>Re-type Email Address</h1>
            <p>Please re-type your email address to verify correctness.</p>
            <asp:TextBox ID="EmailRetypeAddressTextBox" Width="250px" runat="server" CssClass="GenericFormTextBox" />
            <asp:RequiredFieldValidator ID="EmailRetypeAddressRequiredFieldValidator" runat="server" ControlToValidate="EmailRetypeAddressTextBox" Display="Dynamic">
                <br />Please re-type your email address.
            </asp:RequiredFieldValidator>
            <asp:CompareValidator ID="EmailRetypeAddressCompareValidator" runat="server" ControlToCompare="EmailAddressTextBox" ControlToValidate="EmailRetypeAddressTextBox" Display="Dynamic">
                <br />Your email addresses do not match, please re-type them.
            </asp:CompareValidator>
        </div>
        <div class="GenericFormSegment">
            <h1>Password</h1>
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
                <br />Please re-type your password.
            </asp:RequiredFieldValidator>
            <asp:CompareValidator ID="PasswordRetypeCompareValidator" runat="server" ControlToCompare="PasswordTextBox" ControlToValidate="PasswordRetypeTextBox" Display="Dynamic">
                <br />Your passwords do not match, please re-type them.
            </asp:CompareValidator>
        </div>
    </div>
    
    <br class="GenericFormSeparator" />
    
    <div class="GenericFormHeader">Personal Information</div>
    <div class="GenericFormBody">
        <div class="GenericFormSegment">
            <h1>Your First &amp; Last Names</h1>
            <asp:DropDownList ID="TitleDropDownList" runat="server">
                <asp:ListItem>Mr</asp:ListItem>
                <asp:ListItem>Mrs</asp:ListItem>
                <asp:ListItem>Ms</asp:ListItem>
                <asp:ListItem>Miss</asp:ListItem>
                <asp:ListItem>Dr</asp:ListItem>
            </asp:DropDownList>
            <asp:TextBox ID="FirstNameTextBox" Width="70px" runat="server" CssClass="GenericFormTextBox" /> 
            <asp:TextBox ID="LastNameTextBox" Width="70px" runat="server" CssClass="GenericFormTextBox" />
            <asp:RequiredFieldValidator ID="FirstNameRequiredFieldValidator" runat="server" ControlToValidate="FirstNameTextBox" Display="Dynamic">
                <br />Please enter your first name in the first box.
            </asp:RequiredFieldValidator>
            <asp:RequiredFieldValidator ID="LastNameRequiredFieldValidator" runat="server" ControlToValidate="LastNameTextBox" Display="Dynamic">
                <br />Please enter your last name in the second box.
            </asp:RequiredFieldValidator>
        </div>
        <div class="GenericFormSegment">
            <h1>Mobile Telephone Number</h1>
            <p>Please enter either a mobile or a landline number.</p>
            <asp:TextBox ID="MobileAreaTextBox" Width="40px" runat="server" CssClass="GenericFormTextBox" Text="07624" /> <asp:TextBox ID="MobileNumberTextBox" Width="60px" runat="server" CssClass="GenericFormTextBox" />
            <asp:RegularExpressionValidator ID="MobileNumberRegularExpressionValidator" runat="server" ControlToValidate="MobileNumberTextBox" ValidationExpression="^[ \d]+$" Display="Dynamic">
                <br />The phone number you entered can only contain numbers and spaces.
            </asp:RegularExpressionValidator>
            <asp:CustomValidator ID="MobileNumberCustomValidator" runat="server" Display="Dynamic" OnServerValidate="ValidateLandlineAndMobile">
                <br />You must enter either a mobile or a landline number.
            </asp:CustomValidator>
        </div>
        <div class="GenericFormSegment">
            <h1>Landline Telephone Number</h1>
            <p>Please enter either a landline or mobile number.</p>
            <asp:TextBox ID="LandlineAreaTextBox" Width="40px" runat="server" CssClass="GenericFormTextBox" Text="01624" /> <asp:TextBox ID="LandlineNumberTextBox" Width="60px" runat="server" CssClass="GenericFormTextBox" />
            <asp:RegularExpressionValidator ID="LandlineNumberRegularExpressionValidator" runat="server" ControlToValidate="LandlineNumberTextBox" ValidationExpression="^[ \d]+$" Display="Dynamic">
                <br />The phone number you entered can only contain numbers and spaces.
            </asp:RegularExpressionValidator>
            <asp:CustomValidator ID="LandlineNumberCustomValidator" runat="server" Display="Dynamic" OnServerValidate="ValidateLandlineAndMobile">
                <br />You must enter either a landline or mobile number.
            </asp:CustomValidator>
        </div>
        <div class="GenericFormSegment">
            <h1>Town or Village</h1>
            <p>Select your town or village from this list.</p>
            <asp:DropDownList ID="LocationDropDownList" runat="server" />
        </div>
    </div>
    
    <br class="GenericFormSeparator" />
    
    <div class="GenericFormHeader">Small Print</div>
    <div class="GenericFormBody">
        <asp:CheckBox ID="OptOutCheckBox" runat="server" Text="I do not wish to receive email from ManxAds." /><br />
        <asp:CheckBox ID="TermsCheckBox" runat="server" Text="I have read and agree to the <a href='Terms.aspx' target='_blank'>ManxAds Terms &amp Conditions</a>." />
        <asp:CustomValidator ID="TermsCustomValidator" runat="server" Display="Dynamic" OnServerValidate="ValidateTermsCheckBox">
            <br />Please check the above box to confirm that you have read, and that you agree to the ManxAds Terms &amp Conditions.
        </asp:CustomValidator>
    </div>
    
    <br class="GenericFormSeparator" />
    <asp:ImageButton ID="RegisterButton" runat="server" ImageUrl="~/Images/Static/Layout/RegisterButton.gif" AlternateText="Register" OnClick="RegisterButton_Click" CausesValidation="False" />
    
</asp:Content>

