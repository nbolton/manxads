<%@ Page Language="C#" MasterPageFile="~/Master.master" AutoEventWireup="true" Inherits="SettingsEditor" Title="Website Settings" Codebehind="SettingsEditor.aspx.cs" %>

<%@ Register Src="SuccessDialog.ascx" TagName="SuccessDialog" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1>Website Settings</h1>
    <asp:MultiView ID="MultiView" runat="server">
        <asp:View ID="SettingsView" runat="server">
            <asp:Label ID="ValidationErrorLabel" runat="server" Visible="False" ForeColor="Red">
                <p><asp:Image ID="Image3" runat="server" ImageUrl="~/Images/Static/Layout/ErrorIcon.gif" ImageAlign="TextTop" />&nbsp;There was a problem validating the correctness of some fields, please check and try again.</p>
            </asp:Label>
            <p>Values entered here are overridable by individual user settings; these are the default values for the entire website.</p>
            <div class="GenericFormHeader">Welcome Page</div>
            <div class="GenericFormBody">
                <div class="GenericFormSegment">
                    <h1>Top Listings Limit</h1>
                    <p>The maximum number of top listings to show on the welcome page.</p>
                    <asp:TextBox ID="WelcomeTopListingsLimitTextBox" Width="20px" runat="server" CssClass="GenericFormTextBox" />
                </div>
                <div class="GenericFormSegment">
                    <h1>Top Categories Limit</h1>
                    <p>The maximum number of top categories to show on the welcome page.</p>
                    <asp:TextBox ID="WelcomeTopCategoriesLimitTextBox" Width="20px" runat="server" CssClass="GenericFormTextBox" />
                </div>
            </div>
            <br class="GenericFormSeparator" />
            <div class="GenericFormHeader">Adverts</div>
            <div class="GenericFormBody">
                <div class="GenericFormSegment">
                    <h1>Debug Mode</h1>
                    <p>Use with caution. Shows a details summary below all adverts.</p>
                    <asp:CheckBox ID="AdvertsDebugCheckBox" runat="server" Text="Enable" />
                </div>
            </div>
            <br class="GenericFormSeparator" />
            <div class="GenericFormHeader">Listings</div>
            <div class="GenericFormBody">
                <div class="GenericFormSegment">
                    <h1>Maximum Categories</h1>
                    <p>The maximum number of categories a listing can be assigned to (-1 for unlimited).</p>
                    <asp:TextBox ID="MaximumCategoryCountTextBox" Width="20px" runat="server" CssClass="GenericFormTextBox" /> 
                </div>
                <div class="GenericFormSegment">
                    <h1>Boost Count Limit</h1>
                    <p>The maximum number of listings which can be boosted together.</p>
                    <asp:TextBox ID="BoostCountLimitTextBox" Width="20px" runat="server" CssClass="GenericFormTextBox" />
                </div>
                <div class="GenericFormSegment">
                    <h1>Boost Time Limit</h1>
                    <p>The maximum number of hours which a listing can be boosted.</p>
                    <asp:TextBox ID="BoostSleepTimeTextBox" Width="20px" runat="server" CssClass="GenericFormTextBox" />
                </div>
                <div class="GenericFormSegment">
                    <h1>User Listings Limit</h1>
                    <p>Users will be limited to this number of listings.</p>
                    <asp:TextBox ID="UserListingLimitTextBox" Width="20px" runat="server" CssClass="GenericFormTextBox" />
                </div>
                <div class="GenericFormSegment">
                    <h1>Trader Listings Limit</h1>
                    <p>Traders will be limited to this number of listings.</p>
                    <asp:TextBox ID="TraderListingLimitTextBox" Width="20px" runat="server" CssClass="GenericFormTextBox" />
                </div>
                <div class="GenericFormSegment">
                    <h1>Bad Words Dictionary</h1>
                    <p>These words are banned from the listing title and details. This list should be comma-delimited; E.g.: fabrikam, contoso</p>
                    <asp:TextBox ID="BadWordsTextBox" Width="400px" Height="200px" runat="server" CssClass="GenericFormTextBox" TextMode="MultiLine" />
                    <asp:CustomValidator ID="BadWordsCheckForWhitespace" ControlToValidate="BadWordsTextBox" runat="server" Display="Dynamic" OnServerValidate="OnBadWordsCheckForWhitespace">
                        <br />Individual bad words cannot contain spaces (e.g. "Hello World" is not valid).
                    </asp:CustomValidator>
                </div>
            </div>
            <br class="GenericFormSeparator" />
            <asp:ImageButton ID="ContinueButton" runat="server" ImageUrl="~/Images/Static/Layout/ContinueButton.gif" AlternateText="Continue" CausesValidation="False" OnClick="ContinueButton_Click" />
        </asp:View>
        <asp:View ID="SuccessView" runat="server">
            <uc1:SuccessDialog ID="SuccessDialog1" runat="server" />
            
        </asp:View>
    </asp:MultiView>
</asp:Content>

