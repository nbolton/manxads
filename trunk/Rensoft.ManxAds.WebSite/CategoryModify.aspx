<%@ Page Language="C#" MasterPageFile="~/Master.master" AutoEventWireup="true" Inherits="CategoryModify" Title="Modify Category" Codebehind="CategoryModify.aspx.cs" %>
<%@ Register Src="SuccessDialog.ascx" TagName="SuccessDialog" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1><asp:Label ID="TitleLabel" runat="server" /></h1>
    <asp:MultiView ID="MultiView" runat="server">
        <asp:View ID="NotFoundView" runat="server">
            <p><asp:Image ID="Image3" runat="server" ImageUrl="~/Images/Static/Layout/ErrorIcon.gif" ImageAlign="TextTop" />&nbsp;The category you specified was not found.</p>
        </asp:View>
        <asp:View ID="DefaultView" runat="server">
            <asp:Label ID="ValidationErrorLabel" runat="server" Visible="False" ForeColor="Red">
                <p><asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Static/Layout/ErrorIcon.gif" ImageAlign="TextTop" />&nbsp;There was a problem validating the correctness of some fields, please check and try again.</p>
            </asp:Label>
            <br class="GenericFormSeparator" />
            <div class="GenericFormHeader">Category Details</div>
            <div class="GenericFormBody">
                <div class="GenericFormSegment">
                    <h1>Title</h1>
                    <p>Very short titles work best (maximum <asp:Label ID="TitleMaxLengthLabel" runat="server" /> characters).</p>
                    <asp:TextBox ID="TitleTextBox" Width="160px" runat="server" CssClass="GenericFormTextBox" ValidationGroup="Details" />
                    <asp:RequiredFieldValidator ID="TitleRequiredFieldValidator" runat="server" ControlToValidate="TitleTextBox" Display="Dynamic" ValidationGroup="Details">
                        <br />Please type a title for the category.
                    </asp:RequiredFieldValidator>
                </div>
                <div class="GenericFormSegment">
                    <h1>Details <em>(optional)</em></h1>
                    <p>This will be shown on full category list page.</p>
                    <asp:TextBox ID="DetailsTextBox" Width="400px" Height="40px" runat="server" TextMode="MultiLine" CssClass="GenericFormTextBox"  ValidationGroup="Details"/>
                </div>
            </div>
            <br class="GenericFormSeparator" />
            <div class="GenericFormHeader">Thumbnail</div>
            <div class="GenericFormBody">
                <asp:Panel ID="UploadedThumbnailPanel" runat="server" Visible="False">
                    <div class="GenericFormSegment">
                        <h1>Uploaded Thumbnail</h1>
                        <p>This thumbnail now on the website and displayed next to the category.</p>
                        <asp:Image ID="UploadedThumbnailImage" runat="server" CssClass="BrowserThumbnailImage" BorderWidth="1px" />
                        <br /><asp:LinkButton ID="DeleteLinkButton" runat="server" OnClick="DeleteLinkButton_Click">Delete</asp:LinkButton>
                    </div>
                </asp:Panel>
                <div class="GenericFormSegment">
                    <h1>Upload Thumbnail</h1>
                    <p>Click the Browse button to find an image on your computer.</p>
                    <asp:FileUpload ID="ThumbnailFileUpload" runat="server" /> <asp:Button ID="ThumbnailUploadButton" runat="server" Text="Upload" OnClick="ThumbnailUploadButton_Click" CausesValidation="False" />
                    <asp:CustomValidator ID="ThumbnailCustomValidator" runat="server" Display="Dynamic" OnServerValidate="OnRequestDetailsValid" ValidationGroup="Images">
                        <br />You must complete all of the above fields before you can upload a thumbnail.
                    </asp:CustomValidator>
                </div>
            </div>
            <br class="GenericFormSeparator" />
            <asp:ImageButton ID="ContinueButton" runat="server" ImageUrl="~/Images/Static/Layout/ContinueButton.gif" AlternateText="Continue" CausesValidation="False" OnClick="ContinueButton_Click" />
        </asp:View>
        <asp:View ID="RemoveView" runat="server">
            <p>Are you sure you want to delete the category called '<asp:Label ID="RemoveTitleLabel" runat="server" />'?</p>
            <p>
                <asp:ImageButton ID="RemoveContinueButton" runat="server" ImageUrl="~/Images/Static/Layout/DeleteButton.gif" AlternateText="Delete" CausesValidation="False" OnClick="RemoveContinueButton_Click" />
                <asp:ImageButton ID="RemoveCancelButton" runat="server" ImageUrl="~/Images/Static/Layout/CancelButton.gif" AlternateText="Cancel" CausesValidation="False" OnClick="RemoveCancelButton_Click" />
            </p>
        </asp:View>
        <asp:View ID="SuccessView" runat="server">
            <uc3:SuccessDialog id="SuccessDialog1" runat="server"></uc3:SuccessDialog>
        </asp:View>
    </asp:MultiView>
</asp:Content>

