<%@ Page Language="C#" MasterPageFile="~/Master.master" AutoEventWireup="true" Inherits="AdvertModify" Codebehind="AdvertModify.aspx.cs" ValidateRequest="False" %>
<%@ Register Src="SuccessDialog.ascx" TagName="SuccessDialog" TagPrefix="uc3" %>
<%@ Register Src="AdvertRotator.ascx" TagName="AdvertRotator" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1><asp:Label ID="TitleLabel" runat="server" /></h1>
    <asp:MultiView ID="MultiView" runat="server">
        <asp:View ID="NotFoundView" runat="server">
            <p><asp:Image ID="Image3" runat="server" ImageUrl="~/Images/Static/Layout/ErrorIcon.gif" ImageAlign="TextTop" />&nbsp;The advert you specified was not found.</p>
        </asp:View>
        <asp:View ID="DefaultView" runat="server">
            <asp:Label ID="ValidationErrorLabel" runat="server" Visible="False" ForeColor="Red">
                <p><asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Static/Layout/ErrorIcon.gif" ImageAlign="TextTop" />&nbsp;There was a problem validating the correctness of some fields, please check and try again.</p>
            </asp:Label>
            <br class="GenericFormSeparator" />
            <div class="GenericFormHeader">Specification</div>
            <div class="GenericFormBody">
                <div class="GenericFormSegment">
                    <h1>Title</h1>
                    <p>For your use, choose a title to easily identify this advert again.</p>
                    <asp:TextBox ID="TitleTextBox" Width="250px" MaxLength="200" runat="server" CssClass="GenericFormTextBox" ValidationGroup="Details" />
                </div>
                <div class="GenericFormSegment">
                    <h1>Location</h1>
                    <p>Select the location where the advert should be displayed.</p>
                    <asp:DropDownList ID="PositionTypeDropDownList" runat="server" />
                </div>
                <div class="GenericFormSegment">
                    <h1>Dimensions</h1>
                    <p>Select the dimensions of the advert.</p>
                    <asp:DropDownList ID="SizeTypeDropDownList" runat="server" />
                </div>
                <asp:Panel runat="server" ID="DisplayTimePanel">
                    <div class="GenericFormSegment">
                        <h1>Display Time</h1>
                        <p>Length of time in seconds advert is to be displayed.</p>
                        <asp:TextBox Text="20" ID="RotateFrequencyTextBox" Width="20px" runat="server" CssClass="GenericFormTextBox" ValidationGroup="Details" /> Seconds
                        <asp:RequiredFieldValidator ID="RotateFrequencyRequiredFieldValidator" runat="server" ControlToValidate="RotateFrequencyTextBox" Display="Dynamic" ValidationGroup="Details">
                            <br />Please enter a display time.
                        </asp:RequiredFieldValidator>
                    </div>
                </asp:Panel>
                <div class="GenericFormSegment">
                    <h1>Media Format</h1>
                    <p>The chosen format affects how the media is displayed.</p>
                    <asp:DropDownList ID="FormatTypeDropDownList" runat="server" />
                </div>
                <div class="GenericFormSegment">
                    <h1>Target Hyperlink</h1>
                    <p>The user will be directed to this URL when they click on the advert.</p>
                    <asp:TextBox ID="HyperlinkTextBox" Width="250px" MaxLength="200" runat="server" CssClass="GenericFormTextBox" ValidationGroup="Details" />
                </div>
                <div class="GenericFormSegment">
                    <h1>HTML Snippet</h1>
                    <p>For supported advert types, HTML can be used.</p>
                    <asp:TextBox ID="HtmlTextBox" Width="400px" Height="150px" runat="server" TextMode="MultiLine" CssClass="GenericFormTextBox" ValidationGroup="Details" />
                </div>
            </div>
            <br class="GenericFormSeparator" />
            <div class="GenericFormHeader">Media Upload</div>
            <div class="GenericFormBody">
                <div class="GenericFormSegment">
                    <h1>Upload Media</h1>
                    <p>Click the Browse button to find the media on your computer.</p>
                    <asp:FileUpload ID="MediaFileUpload" runat="server" /> <asp:Button ID="MediaUploadButton" runat="server" Text="Upload" OnClick="MediaUploadButton_Click" CausesValidation="False" />
                    <asp:CustomValidator ID="MediaCustomValidator" runat="server" Display="Dynamic" OnServerValidate="OnRequestDetailsValid" ValidationGroup="Media">
                        <br />You must complete all of the above fields before you can upload a thumbnail.
                    </asp:CustomValidator>
                </div>
                <asp:Panel ID="MediaPreviewPanel" runat="server" Visible="False">
                    <div class="GenericFormSegment">
                        <h1>Uploaded Media</h1>
                        <p>When authorised, this media will be displayed on the website.</p>
                        <p><span class="WarningText">You may need to "force refresh" this web page if the above advert does not appear correctly. To do this, press <b>Ctrl + F5</b> on your keyboard.</span></p>
                        <p>Media URL: <asp:Label runat="server" ID="MediaUrlLabel" /></p>
                        <uc1:AdvertRotator ID="MediaPreview" runat="server" />
                    </div>
                </asp:Panel>
            </div>
            <br class="GenericFormSeparator" />
            <asp:Panel ID="AuthorisedPanel" runat="server">
                <div class="GenericFormHeader">Authorisation</div>
                <div class="GenericFormBody">
                        <div class="GenericFormSegment">
                            <h1>Status</h1>
                            <p><asp:Label ID="AuthorisedStatusLabel" runat="server" /></p>
                        </div>
                    <div class="GenericFormSegment">
                        <h1>Authorise</h1>
                        <p>To protect the website's integrity, an administrator must first authorise this advert before it can be displayed on the website.</p>
                        <asp:CheckBox ID="AuthorisedCheckBox" runat="server" Text="Authorise on Continue" Checked="True" />
                        <p><span class="WarningText">Warning! If you are not an administrator, creating or modifying this advert will automatically cause it to become unauthorised.</span></p>
                    </div>
                </div>
                <br class="GenericFormSeparator" />
            </asp:Panel>
            <asp:ImageButton ID="ContinueButton" runat="server" ImageUrl="~/Images/Static/Layout/ContinueButton.gif" AlternateText="Continue" CausesValidation="False" OnClick="ContinueButton_Click" />
        </asp:View>
        <asp:View ID="RemoveView" runat="server">
            <p>Are you sure you want to delete the selected advert?</p>
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

