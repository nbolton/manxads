<%@ Page Language="C#" MasterPageFile="~/Master.master" AutoEventWireup="true" Inherits="ListingModify" Title="Modify Listing" ValidateRequest="False" Codebehind="ListingModify.aspx.cs" %>

<%@ Register Src="ListingModifySteps.ascx" TagName="ListingModifySteps" TagPrefix="uc1" %>
<%@ Register Src="SuccessDialog.ascx" TagName="SuccessDialog" TagPrefix="uc3" %>
<%@ Register src="DonateControl.ascx" tagname="DonateControl" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1><asp:Label ID="TitleLabel" runat="server" /></h1>
    <asp:Label ID="ValidationErrorLabel" runat="server" Visible="False" ForeColor="Red">
        <p><asp:Image ID="Image3" runat="server" ImageUrl="~/Images/Static/Layout/ErrorIcon.gif" ImageAlign="TextTop" />&nbsp;There was a problem validating the correctness of some fields, please check and try again.</p>
    </asp:Label>
    <asp:MultiView ID="MultiView" runat="server">
        <asp:View ID="NotFoundView" runat="server">
            <h1>Not Found</h1>
            <p><asp:Image ID="Image4" runat="server" ImageUrl="~/Images/Static/Layout/ErrorIcon.gif" ImageAlign="TextTop" />&nbsp;The listing you specified was not found.</p>
        </asp:View>
        <asp:View ID="LimitReachedView" runat="server">
            <h1>Limit Reached</h1>
            <p><asp:Image ID="Image2" runat="server" ImageUrl="~/Images/Static/Layout/ErrorIcon.gif" ImageAlign="TextTop" />&nbsp;You have reached the maximum number of listings for your account. Please delete some of your <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Listings.aspx?Self=1">existing listings</asp:HyperLink> if you wish to create a new one.</p>
        </asp:View>
        <asp:View ID="SessionTimeoutView" runat="server">
            <h1>Could not save</h1>
            <p><asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Static/Layout/ErrorIcon.gif" ImageAlign="TextTop" />&nbsp;Your listing could not be fully saved because of a technical problem (although some of your progress may have been saved).</p>
            <h3>Suggestions:</h3>
            <ul>
                <li><asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/Listings.aspx?Self=1">Return to your listings</asp:HyperLink></li>
                <li><asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/UserHome.aspx">Go to your account home</asp:HyperLink></li>
            </ul>
        </asp:View>
        <asp:View ID="DetailsView" runat="server" OnActivate="DetailsView_Activate">
            <uc1:ListingModifySteps ID="ListingModifySteps1" runat="server" />
            <p>Learn how to get the most out of this page and more - <a href="Help.aspx?Topic=Listings" target="_blank">listings help</a>.</p>
            <div class="GenericFormHeader">Details</div>
            <div class="GenericFormBody">
                <div class="GenericFormSegment">
                    <h1>Title</h1>
                    <p>Choose a brief title (maximum <asp:Label ID="TitleMaxLengthLabel" runat="server" /> characters).</p>
                    <asp:TextBox ID="TitleTextBox" Width="230px" runat="server" CssClass="GenericFormTextBox" ValidationGroup="Details" />
                    <asp:RequiredFieldValidator ID="TitleRequiredFieldValidator" runat="server" ControlToValidate="TitleTextBox" Display="Dynamic" ValidationGroup="Details">
                        <br />Please type a title for your listing.
                    </asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="TitleCustomValidator" runat="server" ValidationGroup="Details" ControlToValidate="TitleTextBox" Display="Dynamic" OnServerValidate="OnCheckBadWords">
                        <br />One or more of the words in your title could be considered offencive. Please reword your title so it may be viewied by all audiences.
                    </asp:CustomValidator>
                </div>
                <div class="GenericFormSegment">
                    <h1>Details</h1>
                    <p>Describe the item you are advertising. Advanced users can use HTML.</p>
                    <asp:RadioButtonList ID="DetailsTypeRadioButtonList" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Selected="True" Value="PlainText">Plain Text (Normal)</asp:ListItem>
                        <asp:ListItem Value="Html">HTML (Advanced)</asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:TextBox ID="DetailsTextBox" Width="400px" Height="150px" runat="server" TextMode="MultiLine" CssClass="GenericFormTextBox" ValidationGroup="Details" />
                    <asp:RequiredFieldValidator ID="DetailsRequiredFieldValidator" runat="server" ControlToValidate="DetailsTextBox" Display="Dynamic" ValidationGroup="Details">
                        <br />Please type the details for your listing.
                    </asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="DetailsCustomValidator" runat="server" ValidationGroup="Details" ControlToValidate="DetailsTextBox" Display="Dynamic" OnServerValidate="OnCheckBadWords">
                        <br />One or more of the words in your details could be considered offencive. Please reword your description so it may be viewied by all audiences.
                    </asp:CustomValidator>
                </div>
                <div class="GenericFormSegment">
                    <h1>Price Type</h1>
                    <p>This affects how your price is displayed on the website.</p>
                    <asp:DropDownList ID="PriceTypeDropDownList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="PriceTypeDropDownList_SelectedIndexChanged" ValidationGroup="Details" />
                </div>
                <asp:Panel ID="PriceValuePanel" runat="server">
                    <div class="GenericFormSegment">
                        <h1>Listing Price</h1>
                        &pound; <asp:TextBox ID="PriceValueTextBox" Width="70px" runat="server" CssClass="GenericFormTextBox" ValidationGroup="Details" />
                        <asp:CustomValidator ID="PriceValueCustomValidator" runat="server" ControlToValidate="PriceValueTextBox" Display="Dynamic" OnServerValidate="PriceValueCustomValidator_ServerValidate" ValidateEmptyText="True" ValidationGroup="Details">
                            <br />You have not entered a price, please do so.
                        </asp:CustomValidator>
                        <asp:RegularExpressionValidator ID="PriceValueRegularExpressionValidator" runat="server" ControlToValidate="PriceValueTextBox" Display="Dynamic" ValidationExpression="\d+(,\d+)*(\.\d+)?" ValidationGroup="Details">
                            <br />Make sure you only type a number (currency signs and letters are not allowed).
                        </asp:RegularExpressionValidator>
                    </div>
                </asp:Panel>
                <div class="GenericFormSegment">
                    <h1>Item Location</h1>
                    <asp:DropDownList ID="LocationDropDownList" runat="server" />
                </div>
                <asp:Panel ID="BoostPanel" runat="server" Visible="False">
                    <div class="GenericFormSegment">
                        <h1>Boost Listing</h1>
                        <p>Put this item at the top of your My Listings page.</p>
                        <p><asp:CheckBox ID="BoostCheckBox" runat="server" Text="Boost" Checked="True" /></p>
                    </div>
                </asp:Panel>
            </div>
            <br class="GenericFormSeparator" />
            <div class="GenericFormHeader">Contact</div>
            <div class="GenericFormBody">
                <p><asp:CheckBox ID="ShowLocationCheckBox" runat="server" Text="Display nearest location." Checked="True" /></p>
                <p><asp:CheckBox ID="ShowLandlineCheckBox" runat="server" Text="Display landline telephone number (if available)." Checked="True" /></p>
                <p><asp:CheckBox ID="ShowMobileCheckBox" runat="server" Text="Display mobile telephone number (if available)." Checked="True" /></p>
            </div>
            <br class="GenericFormSeparator" />
            <asp:ImageButton ID="DetailsFinishButton" runat="server" ImageUrl="~/Images/Static/Layout/FinishButton.gif" AlternateText="Finish" CausesValidation="False" OnClick="DetailsFinishButton_Click" Visible="False" />
            <asp:ImageButton ID="DetailsNextButton" runat="server" ImageUrl="~/Images/Static/Layout/NextButton.gif" AlternateText="Next >" CausesValidation="False" OnClick="DetailsNextButton_Click" />
            <p>Click <b>Next</b> to choose categories (and upload images).</p>
        </asp:View>
        <asp:View ID="CategoriesView" runat="server" OnActivate="CategoriesView_Activate">
            <uc1:ListingModifySteps ID="ListingModifySteps2" runat="server" />
            <p>Learn how to get the most out of this page and more. Take a look at our <a href="Help.aspx?Topic=Listings" target="_blank">Listings help article</a>.</p>
            <div class="GenericFormHeader">Categories</div>
            <div class="GenericFormBody">
                <div class="GenericFormSegment">
                    <h1>New Category</h1>
                    <p>You can add your listing to one or more categories.</p>
                    <asp:MultiView ID="CategoryAddMultiView" runat="server">
                        <asp:View ID="CategoryAddDefaultView" runat="server">
                            <asp:DropDownList ID="CategoryDropDownList" runat="server" />
                            <asp:Button ID="CategoryAddButton" runat="server" Text="Add" OnClick="CategoryAddButton_Click" CausesValidation="False" />
                            <asp:CustomValidator ID="CategoryCustomValidator1" runat="server" Display="Dynamic" OnServerValidate="CategoryCustomValidator_ServerValidate" ValidationGroup="Categories">
                                <br />You must add the listing to at least one category.
                            </asp:CustomValidator>
                            <asp:CustomValidator ID="CategoryCustomValidator2" runat="server" Display="Dynamic" OnServerValidate="OnRequestDetailsValid" ValidationGroup="CategoriesRequresDetails">
                                <br />You must complete all of the above fields before you can add categories.
                            </asp:CustomValidator>
                            <asp:Panel ID="MaximumCategoryCountPanel" runat="server" Visible="false">
                                <p style="color: Red"><b>Note:</b> Listings are limited to <asp:Label ID="MaximumCategoryCountLabel" runat="server" /> only.</p>
                            </asp:Panel>
                        </asp:View>
                        <asp:View ID="CategoryAddEmptyView" runat="server">
                            There are no more categories you can add to.
                        </asp:View>
                    </asp:MultiView>
                </div>
                <asp:Panel ID="ListingCategoriesPanel" runat="server" Visible="False">
                    <div class="GenericFormSegment">
                        <h1>Current Categories</h1>
                        <p>Your listing is now in the following categories (<asp:LinkButton ID="RemoveAllCategoriesLinkButton" runat="server" OnClick="RemoveAllCategoriesLinkButton_Click">remove from all</asp:LinkButton>).</p>
                        <asp:DataList ID="ListingCategoryDataList" runat="server" RepeatDirection="Horizontal" RepeatColumns="4" OnItemDataBound="ListingCategoryDataList_ItemDataBound">
                            <ItemTemplate>
                                <asp:Image ID="ThumbnailImage" CssClass="BrowserThumbnailImage" runat="server" BorderWidth="1px" ImageUrl='<%# Bind("ImageUrl") %>' />
                                <p>
                                    <asp:Label ID="TitleLabel" runat="server" Text='<%# Bind("Title") %>' /><br />
                                    <asp:LinkButton ID="CategoryDeleteLinkButton" runat="server" OnClick="CategoryDeleteLinkButton_Click" CommandArgument='<%# Bind("DatabaseId") %>'>Delete</asp:LinkButton>
                                </p>
                            </ItemTemplate>
                            <ItemStyle CssClass="ListingCategoryItem" />
                            <SeparatorStyle Width="20px" />
                            <SeparatorTemplate>&nbsp;</SeparatorTemplate>
                        </asp:DataList>
                        <h1 style="margin-top: 10px">Category spamming</h1>
                        <p>We may delete listings which are added to irrelevant categories.</p>
                    </div>
                </asp:Panel>
            </div>
    
            <br class="GenericFormSeparator" />
            <asp:ImageButton ID="CategoriesPreviousButton" runat="server" ImageUrl="~/Images/Static/Layout/PreviousButton.gif" AlternateText="< Previous" CausesValidation="False" OnClick="CategoriesPreviousButton_Click" />
            <asp:ImageButton ID="CategoriesFinishButton" runat="server" ImageUrl="~/Images/Static/Layout/FinishButton.gif" AlternateText="Finish" CausesValidation="False" OnClick="CategoriesFinishButton_Click" Visible="False" />
            <asp:ImageButton ID="CategoriesNextButton" runat="server" ImageUrl="~/Images/Static/Layout/NextButton.gif" AlternateText="Next >" CausesValidation="False" OnClick="CategoriesNextButton_Click" />
            <p>Click <b>Next</b> to upload images.</p>
        </asp:View>
        <asp:View ID="ImagesView" runat="server" OnActivate="ImagesView_Activate">
            <uc1:ListingModifySteps ID="ListingModifySteps3" runat="server" />
            <p>Learn how to get the most out of this page and more. Take a look at our <a href="Help.aspx?Topic=Listings" target="_blank">Listings help article</a>.</p>
            <div class="GenericFormHeader">Images</div>
            <div class="GenericFormBody">
                <div class="GenericFormSegment">
                    <h1>Upload Image</h1>
                    <p>
                        1. Click the <b>Browse</b> button to find an image on your computer.<br />
                        2. Then, click the <b>Upload</b> button to upload this image to ManxAds.
                    </p>
                    <asp:FileUpload ID="UploadImageFileUpload" runat="server" /> <asp:Button ID="UploadImageUploadButton" runat="server" Text="Upload" OnClick="UploadImageUploadButton_Click" CausesValidation="False" CssClass="UploadButton" />
                    <br /><span style="color: Blue"><b>Note:</b> Large images may take a few minutes to upload.</span>
                    <br /><asp:CheckBox ID="UploadImageThumbnailCheckBox" runat="server" Text="Set the uploaded image as the preview image." Checked="True" />
                    <asp:CustomValidator ID="UploadImageExtensionValidator" runat="server" Display="Dynamic" OnServerValidate="UploadImageExtensionValidator_ServerValidate" ValidationGroup="Images">
                        <br />Sorry, we do not support the chosen type of file. Please make sure you select an <em>image</em> from your comptuer. Supported image types are: <asp:Label ID="SupportedImageTypesLabel" runat="server" />.
                    </asp:CustomValidator>
                </div>
                <asp:Panel ID="UploadedImagesPanel" runat="server" Visible="False">
                    <div class="GenericFormSegment">
                        <h1>Current Images</h1>
                        <p>You can upload as many photos for this listing as you want.</p>
                        <asp:DataList ID="ImagesDataList" runat="server" RepeatDirection="Horizontal" RepeatColumns="3" OnItemDataBound="ImagesDataList_ItemDataBound">
                            <ItemTemplate>
                                <asp:Literal ID="IsMaster" runat="server" Visible="False" Text='<%# Bind("IsMaster") %>' />
                                <asp:Image ID="SmallImage" runat="server" CssClass="BrowserThumbnailImage" BorderWidth="1px" ImageUrl='<%# Bind("SmallImageUrl") %>' />
                                <p><asp:Label ID="PreviewImageLabel" runat="server">Preview On</asp:Label><asp:LinkButton ID="SetPreviewLinkButton" runat="server" OnClick="SetPreviewLinkButton_Click" CommandArgument='<%# Bind("DatabaseId") %>'>Set Preview</asp:LinkButton> | <asp:LinkButton ID="DeleteLinkButton" runat="server" OnClick="DeleteLinkButton_Click" CommandArgument='<%# Bind("DatabaseId") %>'>Delete</asp:LinkButton></p>
                            </ItemTemplate>
                            <ItemStyle CssClass="UploadedImage" />
                            <SeparatorStyle Width="20px" />
                            <SeparatorTemplate>&nbsp;</SeparatorTemplate>
                        </asp:DataList>
                    </div>
                </asp:Panel>
            </div>
            <br class="GenericFormSeparator" />
            <asp:ImageButton ID="ImagesPreviousButton" runat="server" ImageUrl="~/Images/Static/Layout/PreviousButton.gif" AlternateText="< Previous" CausesValidation="False" OnClick="ImagesPreviousButton_Click" />
            <asp:ImageButton ID="ImagesFinishButton" runat="server" ImageUrl="~/Images/Static/Layout/NextButton.gif" AlternateText="Next" CausesValidation="False" OnClick="ImagesFinishButton_Click" />
            <p><b>Note:</b> Large images may take a few minutes to upload.</p>
        </asp:View>
        <asp:View ID="DonateView" runat="server">
            <br class="GenericFormSeparator" />
            <div class="GenericFormHeader">Donate?</div>
            <div class="GenericFormBody">
                <div class="GenericFormSegment">
                    <span class="PayPalDonationListingModify">
                        <uc2:DonateControl ID="DonateControl1" runat="server" DonateID="7152955" />
                        <p>Your listing will still be posted if you click donate.</p>
                    </span>
                </div>
            </div>
            <br class="GenericFormSeparator" />
            <asp:ImageButton ID="DonateFinishImageButton" runat="server" 
                ImageUrl="~/Images/Static/Layout/FinishButton.gif" AlternateText="Next" 
                CausesValidation="False" onclick="DonateFinishImageButton_Click" />
            <p><b>Note:</b> Just click Finish if you don&#39;t want to donate.</p>
        </asp:View>
        <asp:View ID="RemoveView" runat="server">
            <p>Are you sure you want to delete your listing called '<asp:Label ID="RemoveTitleLabel" runat="server" />'?</p>
            <p>
                <asp:ImageButton ID="RemoveContinueButton" runat="server" ImageUrl="~/Images/Static/Layout/DeleteButton.gif" AlternateText="Delete" CausesValidation="False" OnClick="RemoveContinueButton_Click" />
                <asp:ImageButton ID="RemoveCancelButton" runat="server" ImageUrl="~/Images/Static/Layout/CancelButton.gif" AlternateText="Cancel" CausesValidation="False" OnClick="RemoveCancelButton_Click" />
            </p>
        </asp:View>
        <asp:View ID="SuccessView" runat="server" OnActivate="SuccessView_Activate">
            <div id="ImageUploadReminder" runat="server" visible="false" class="ImageUploadReminder">
                You didn't upload any images! Get your listing displayed on the ManxAds home page by uploading one or more images. <asp:LinkButton ID="UploadImagesLinkButton" runat="server" OnClick="UploadImagesLinkButton_Click">Do it now!</asp:LinkButton>
            </div>
            <uc3:SuccessDialog id="SuccessDialog" runat="server"></uc3:SuccessDialog>
        </asp:View>
    </asp:MultiView>
</asp:Content>

