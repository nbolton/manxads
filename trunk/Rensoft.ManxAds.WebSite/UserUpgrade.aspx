<%@ Page Language="C#" MasterPageFile="~/Master.master" AutoEventWireup="true" Inherits="UserUpgrade" Title="Upgrade Account" Codebehind="UserUpgrade.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="FormView" runat="server">   
            <h1>Upgrade Account</h1>
            <p>To upgrade your account to trader or charity, please complete this form.</p>
            <div class="GenericFormHeader">Organisation Details</div>
            <div class="GenericFormBody">
                <div class="GenericFormSegment">
                    <h1>Organisation</h1>
                    <p>What is your company or charity called?</p>
                    <asp:TextBox ID="TraderNameTextBox" Width="230px" runat="server" CssClass="GenericFormTextBox" />
                </div>
                <div class="GenericFormSegment">
                    <h1>Website</h1>
                    <p>Drive traffic to your organisation's web site!</p>
                    <asp:TextBox ID="WebsiteTextBox" Width="230px" runat="server" CssClass="GenericFormTextBox" />
                </div>
                <div class="GenericFormSegment">
                    <h1>About</h1>
                    <p>Tell us a little about your organisation - what do you do?</p>
                    <asp:TextBox ID="DetailsTextBox" Width="400px" Height="80px" runat="server" TextMode="MultiLine" CssClass="GenericFormTextBox" />
                </div>
                <div class="GenericFormSegment">
                    <h1>Advertising</h1>
                    <p>Want to get more visitors on your web site? Let us help you by setting up a professional banner advert.</p>
                    <asp:CheckBox ID="AdvertisingCheckBox" runat="server" Text="Contact me about professional advertising." Checked="True" />
                </div>
            </div>
            <br class="GenericFormSeparator" />
            <asp:ImageButton ID="FinishButton" runat="server" ImageUrl="~/Images/Static/Layout/FinishButton.gif" AlternateText="Finish" CausesValidation="False" OnClick="FinishButton_Click" />
        
        </asp:View>
        <asp:View ID="FinishedView" runat="server">   
            <h3>Thank You</h3>
            <p>We've recieved your request, please expect a response soon. Where would you like to go now?</p>
            <ul>
                <li><asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/UserHome.aspx">Back to my account</asp:HyperLink></li>
                <li><asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/Search.aspx">Search for listings</asp:HyperLink></li>
                <li><asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/Listings.aspx">Browse latest listings</asp:HyperLink></li>
            </ul>
        </asp:View>
        <asp:View ID="AcceptedView" runat="server">
            <h3>User Accepted</h3>
            <p>This user's upgrade request has been accepted.</p>
            <ul>
                <li><asp:HyperLink ID="EditUserHyperLink" runat="server">Edit user</asp:HyperLink></li>
                <li><asp:HyperLink ID="HyperLink5" runat="server" NavigateUrl="~/UserHome.aspx">Account home</asp:HyperLink></li>
            </ul>
        </asp:View>
    </asp:MultiView>
</asp:Content>