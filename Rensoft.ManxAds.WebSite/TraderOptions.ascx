<%@ Control Language="C#" AutoEventWireup="true" Inherits="TraderOptions" Codebehind="TraderOptions.ascx.cs" %>
<div class="GenericFormHeader"><asp:Label ID="TraderTypeLabel" runat="server" /> Trader Options</div>
<div class="GenericFormBody">
    <asp:Panel ID="LogoImagePanel" runat="server" Visible="False">
        <div class="GenericFormSegment">
            <h1>Your Logo</h1>
            <p>This logo will now be displayed next to your listings.</p>
            <asp:Image ID="LogoImage" runat="server" />
            <p><asp:LinkButton ID="LogoDeleteLinkButton" runat="server" OnClick="LogoDeleteLinkButton_Click">Delete</asp:LinkButton></p>
        </div>
    </asp:Panel>
    <div class="GenericFormSegment">
        <h1>Upload Logo</h1>
        <p>Click the Browse button to find an image on your computer.</p>
        <p><em>Note:</em> Your logo should have an approximate <a href="http://en.wikipedia.org/wiki/Aspect_ratio_(image)" target="_blank">aspect ratio</a> of 3:1.</p>
        <asp:FileUpload ID="LogoFileUpload" runat="server" /> <asp:Button ID="LogoUploadButton" runat="server" Text="Upload" CausesValidation="False" OnClick="LogoUploadButton_Click" />
    </div>
</div>
<br class="GenericFormSeparator" />