<%@ Control Language="C#" AutoEventWireup="true" Inherits="ContactForm" Codebehind="ContactForm.ascx.cs" %>
<h1>Contact Seller</h1>
<asp:MultiView ID="ContactMultiView" runat="server">
    <asp:View ID="ContactDefaultView" runat="server">
        <p>You are contacting <b><asp:Label ID="SellerNameLabel" runat="server" /></b> about an item named <b><asp:Label ID="ListingTitleLabel" runat="server" /></b>.</p>
        <asp:Label ID="ValidationErrorLabel" runat="server" Visible="False" ForeColor="Red">
            <p><asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Static/Layout/ErrorIcon.gif" ImageAlign="TextTop" />&nbsp;There was a problem validating the correctness of some fields, please check and try again.</p>
        </asp:Label>
        <div class="GenericFormHeader">Contact Seller</div>
        <div class="GenericFormBody">
            <div class="GenericFormSegment">
                <h1>Your Name</h1>
                <asp:TextBox ID="NameTextBox" Width="200px" runat="server" CssClass="GenericFormTextBox" Enabled="false" />
                <asp:RequiredFieldValidator ID="NameRequiredFieldValidator" runat="server" ControlToValidate="NameTextBox" Display="Dynamic">
                    <br />Please type your name.
                </asp:RequiredFieldValidator>
            </div>
            <div class="GenericFormSegment">
                <h1>Your Phone Number</h1>
                <asp:TextBox ID="PhoneNumberTextBox" Width="150px" runat="server" CssClass="GenericFormTextBox" />
                <asp:CustomValidator ID="PhoneNumberCustomValidator" runat="server" Display="Dynamic" OnServerValidate="CheckPhoneOrEmail" ControlToValidate="PhoneNumberTextBox" ValidateEmptyText="True">
                    <br />Please enter either your phone number or your email address.
                </asp:CustomValidator>
            </div>
            <div class="GenericFormSegment">
                <h1>Your Email Address</h1>
                <asp:TextBox ID="EmailAddressTextBox" Width="250px" runat="server" CssClass="GenericFormTextBox" Enabled="false" />
                <asp:RegularExpressionValidator ID="EmailAddressRegularExpressionValidator" runat="server" ControlToValidate="EmailAddressTextBox" Display="Dynamic" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                    <br />The address does not appear to be valid.
                </asp:RegularExpressionValidator>
                <asp:CustomValidator ID="EmailAddressCustomValidator" runat="server" Display="Dynamic" OnServerValidate="CheckPhoneOrEmail" ControlToValidate="EmailAddressTextBox" ValidateEmptyText="True">
                    <br />Please enter either your phone number or your email address.
                </asp:CustomValidator>
            </div>
            <div class="GenericFormSegment">
                <h1>Your Question</h1>
                <asp:TextBox ID="QuestionTextBox" Width="300px" Height="100px" TextMode="MultiLine" runat="server" CssClass="GenericFormTextBox" />
                <asp:RequiredFieldValidator ID="QuestionRequiredFieldValidator" runat="server" ControlToValidate="QuestionTextBox" Display="Dynamic">
                    <br />Please type a question to ask the seller.
                </asp:RequiredFieldValidator>
            </div>
        </div>
        <br class="GenericFormSeparator" />
        <asp:ImageButton ID="ContactButton" runat="server" ImageUrl="~/Images/Static/Layout/ContinueButton.gif" AlternateText="Continue" OnClick="ContactButton_Click" CausesValidation="False" />
    </asp:View>
    <asp:View ID="ContactFinishedView" runat="server">
        <h3>Success!</h3>
        <p>Your message has been sent. Where would you like to go now?</p>
        <ul>
            <li><asp:HyperLink runat="server" ID="ReturnHyperLink">Return to Listing</asp:HyperLink></li>
            <li><asp:HyperLink runat="server" ID="HyperLink1" NavigateUrl="~/Default.aspx">ManxAds Homepage</asp:HyperLink></li>
        </ul>
    </asp:View>
    <asp:View ID="NotFoundView" runat="server">
        <p>The listing for this contact page has either been removed or does not exist.</p>
        <h3>Suggestions:</h3>
        <ul>
            <li>Return to the <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/Default.aspx">ManxAds homepage</asp:HyperLink>.</li>
        </ul>
    </asp:View>
</asp:MultiView>