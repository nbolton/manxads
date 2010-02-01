<%@ Page Language="C#" MasterPageFile="~/Master.master" AutoEventWireup="true" Inherits="Logon" Title="Log On" Description="Log on to ManxAds, The Isle of Man's Online Marketplace." Codebehind="Logon.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <asp:Panel ID="ErrorPanel" runat="server" Visible="false" CssClass="ErrorPanel" >
        <asp:Image ID="Image3" runat="server" ImageUrl="~/Images/Static/Layout/ErrorIcon.gif" ImageAlign="TextTop" />&nbsp;<asp:Label ID="ErrorLabel" runat="server" CssClass="ErrorLabel" />
    </asp:Panel>

    <div id="LogonOptions">
        
        <div id="LogonExistingUsers">
        
            <div id="LogonExistingUsersTitle">
                <div id="LogonExistingUsersTitleSmall">Existing Users</div>
                <div id="LogonExistingUsersTitleLarge">Log On</div>
            </div>
            
            <div id="LogonExistingUsersControls">
                <div class="LogonControlsTextBoxesContainer">
                    <h1>Email Address</h1>
                    <asp:TextBox Width="153px" CssClass="TextBox" ID="LogonEmailAddressTextBox" runat="server" />
                    <h1>Password</h1>
                    <asp:TextBox Width="153px" CssClass="TextBox" ID="LogonPasswordTextBox" runat="server" TextMode="Password" />
                    <asp:CheckBox ID="PersistentCheckBox" runat="server" Text="Remember my password" />
                </div>
                <div class="LogonControlsButtonContainer">
                    <asp:ImageButton ID="LogOnButton" runat="server" ImageUrl="~/Images/Static/Layout/LogOnButton.gif" OnClick="LogOnButton_Click" AlternateText="Log On" />
                </div>
            </div>
        
        </div>
        
        <div id="LogonNewUsers">
        
            <div id="LogonNewUsersTitle">
                <div id="LogonNewUsersTitleSmall">New to ManxAds</div>
                <div id="LogonNewUsersTitleLarge">Register</div>
            </div>
            
            <div id="LogonNewUsersControls">
                <div class="LogonControlsTextBoxesContainer">
                    <h1>Email Address</h1>
                    <asp:TextBox Width="153px" CssClass="TextBox" ID="RegisterEmailAddress" runat="server" />
                    <h1>Town or Village</h1>
                    <asp:DropDownList ID="LocationDropDownList" runat="server" />
                </div>
                <div class="LogonControlsButtonContainer">
                    <asp:ImageButton ID="RegisterButton" runat="server" ImageUrl="~/Images/Static/Layout/RegisterButton.gif" AlternateText="Register" OnClick="RegisterButton_Click" />
                </div>
            </div>
        
        </div>
        
    </div>
    
    <div id="LogonHelp">
        <h3>Troubleshooting</h3>
        <ul>
            <li><asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/RecoverPassword.aspx">I've forgotten my password.</asp:HyperLink></li>
        </ul>
    </div>

</asp:Content>

