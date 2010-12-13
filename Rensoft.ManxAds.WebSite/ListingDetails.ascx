<%@ Control Language="C#" AutoEventWireup="true" Inherits="ListingDetails" Codebehind="ListingDetails.ascx.cs" %>
<%@ Register Assembly="System.Web.DynamicData, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.DynamicData" TagPrefix="cc1" %>
<%@ Register Src="BreadcrumbTrail.ascx" TagName="BreadcrumbTrail" TagPrefix="uc1" %>
<%@ Register src="DonateControl.ascx" tagname="DonateControl" tagprefix="uc2" %>

<asp:MultiView ID="MultiView" runat="server">
    <asp:View ID="DetailsView" runat="server">
        
        <input ID="ListingId" runat="Server" type="hidden" />
        <script type="text/javascript">
            var listing_id = 'listing_' + document.getElementById('ctl00_ContentPlaceHolder1_ListingDetails1_ListingId').value
		</script>
        
        <asp:Image ID="ThumbnailImage" runat="server" ImageAlign="Left" CssClass="ListingDetailsThumbnail" BorderWidth="1px" Visible="False" />
        <h1 style="margin-bottom: 5px"><asp:Label ID="TitleLabel" runat="server" /></h1>
        <uc1:BreadcrumbTrail ID="BreadcrumbTrail" runat="server" Visible="False" />
        <div id="ListingSummary">
            <asp:Table ID="SummaryTable" runat="server" CellPadding="0" CellSpacing="0">
                <asp:TableRow>
                    <asp:TableHeaderCell>Price:</asp:TableHeaderCell>
                    <asp:TableCell ID="PriceTableCell">n/a</asp:TableCell>
                    <asp:TableHeaderCell>Contact Name:</asp:TableHeaderCell>
                    <asp:TableCell ID="SellerNameTableCell" />
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableHeaderCell>Terms:</asp:TableHeaderCell>
                    <asp:TableCell ID="TermsTableCell" CssClass="NoTextWrap">n/a</asp:TableCell>
                    <asp:TableHeaderCell>Seller Location:</asp:TableHeaderCell>
                    <asp:TableCell ID="SellerLocationTableCell" CssClass="NoTextWrap">n/a</asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableHeaderCell>Location:</asp:TableHeaderCell>
                    <asp:TableCell ID="LocationTableCell" CssClass="NoTextWrap">n/a</asp:TableCell>
                    <asp:TableHeaderCell>Mobile Phone:</asp:TableHeaderCell>
                    <asp:TableCell ID="MobilePhoneTableCell" CssClass="NoTextWrap">n/a</asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableHeaderCell>Listed:</asp:TableHeaderCell>
                    <asp:TableCell ID="DateTableCell"  CssClass="NoTextWrap"/>
                    <asp:TableHeaderCell>Landline Phone:</asp:TableHeaderCell>
                    <asp:TableCell ID="LandlinePhoneTableCell" CssClass="NoTextWrap">n/a</asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
        <asp:Panel ID="TraderPanel" runat="server" Visible="False" CssClass="ListingDetailsTraderInfo">
            <h3><asp:Label ID="TraderTypeLabel" runat="server" /></h3>
            <asp:HyperLink ID="TraderLogoHyperLink" runat="server" Target="_blank">
                <asp:Image ID="TraderLogoImage" runat="server" ImageAlign="Left" />
            </asp:HyperLink>
            <p>
                <asp:Label ID="TraderNameLabel" runat="server" /><br />
                <asp:HyperLink ID="TraderWebsiteHyperLink" runat="server" Target="_blank">Visit Website</asp:HyperLink>
            </p>
        </asp:Panel>
		<table cellpadding="0" cellspacing="0" border="0">
			<tr>
				<td style="padding-right: 10px">
					<ul class="ListingDetailsOptions" style="padding: 0px">
						<li>Listing has had <b><asp:Label ID="PageHitsLabel1" runat="server" /></b> hits.</li>
						<li><asp:HyperLink ID="SellerContactHyperLink" runat="server">Contact seller</asp:HyperLink> about this listing.</li>
						<li><asp:HyperLink ID="SellerListingsHyperLink" runat="server">More listings</asp:HyperLink> from this seller.</li>
						<li><asp:HyperLink ID="SearchListingsHyperLink" runat="server">Similar listings</asp:HyperLink> from all sellers.</li>
						<li><asp:HyperLink ID="SendToFriendHyperLink" runat="server">Send this listing</asp:HyperLink> to someone.</li>
					</ul>
				</td>
			</tr>
		</table>
		
		<div style="margin-top: 10px; margin-bottom: 10px">
            <script type="text/javascript"><!--
                google_ad_client = "ca-pub-3305986762497110";
                /* ManxAds filler */
                google_ad_slot = "6014498430";
                google_ad_width = 234;
                google_ad_height = 60;
            //-->
            </script>
            <script type="text/javascript"
            src="http://pagead2.googlesyndication.com/pagead/show_ads.js">
            </script>
		</div>
		
        <p><asp:Label ID="DetailsLabel" runat="server" /></p>
        
        <asp:Panel ID="ListingImagePanel" runat="server" Visible="False">
            
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            
            <asp:UpdatePanel ID="ImageUpdatePanel" runat="server" ChildrenAsTriggers="true">
                <ContentTemplate>
                    
                    <div class="ListingDetailsImageThumbnails">
                        <asp:Repeater ID="ListingImageRepeater" runat="server" OnItemDataBound="ListingImageRepeater_ItemDataBound">
                            <ItemTemplate>
                                <span class="ListingDetailsEnlargeThumbnail">
                                    <asp:LinkButton ID="ThumnbnailLinkButton" runat="server" OnClick="ThumbnailLinkButton_Click" >
                                        <asp:Image ID="ThumnbnailImage" runat="server" Width="50" Height="50" ImageUrl='<%# Bind("ThumbnailUrl") %>' CssClass="BrowserThumbnailImage" BorderWidth="1px" />
                                    </asp:LinkButton>
                                </span>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                    
                    <asp:UpdateProgress ID="ImageUpdateProgress" runat="server" AssociatedUpdatePanelID="ImageUpdatePanel" >
                        <ProgressTemplate>
                            <div class="ListingImageProgress">
                                <div style="margin-bottom: 5px"><img src="Images/Static/Layout/Loading.gif" /></div>
                                <div><b>Loading...</b></div>
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    
                    <div class="ListingDetailsImage">
                        <asp:Image ID="ListingImage" runat="server"  />
                    </div>
                    
                </ContentTemplate>
            </asp:UpdatePanel>
            
        </asp:Panel>
        <div class="ListingHitCounter">
            <asp:Label ID="PageHitsLabel2" runat="server" /> hits
        </div>
        <asp:MultiView ID="ReportAbuseMultiView" runat="server">
            <asp:View ID="ReportAbuseViewUnreported" runat="server">
                <div class="ReportAbuse">
                    <p><span style="color: Red; font-weight: bold">Is this listing fraudulent, or in the wrong category?</span> Please report this listing if it's a violation of the <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="Help.aspx?Topic=ListingPolicy">Listing Policy</asp:HyperLink>. Don't worry, your name and contact details will be kept anonymous from the seller.</p>
                    <p><asp:LinkButton ID="ReportAbuseButton" runat="server" onclick="ReportAbuseButton_Click">Report this listing</asp:LinkButton></p>
                </div>
            </asp:View>
            <asp:View ID="ReportAbuseViewReported" runat="server">
                <div class="ReportAbuse">
                    <p>You have reported this listing. Thanks for cleaning up ManxAds!</p>
                </div>
            </asp:View>
        </asp:MultiView>
    </asp:View>
    <asp:View ID="DeletedView" runat="server" OnActivate="DeletedView_Activate">
        <h1>Listing Deleted</h1>
        <p>This listing has been deleted by it's seller.</p>
        <h3>Suggestions:</h3>
        <ul>
            <li>Try <asp:HyperLink ID="SearchHyperLink1" runat="server">searching</asp:HyperLink> for similar listings.</li>
            <li>View <asp:HyperLink ID="OtherListingsHyperLink" runat="server">other listings</asp:HyperLink> from this seller.</li>
            <li>Browse the website by <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Categories.aspx">category</asp:HyperLink>.</li>
            <li>Visit the <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/Listings.aspx">Top Listings</asp:HyperLink> page.</li>
        </ul>
    </asp:View>
    <asp:View ID="UnavailableView" runat="server" OnActivate="UnavailableView_Activate">
        <h1>Listing Unavailable</h1>
        <p>This listing has been removed or is no longer available. The hyperlink you are using may be invalid.</p>
        <h3>Suggestions:</h3>
        <ul>
            <li>Try <asp:HyperLink ID="SearchHyperLink2" runat="server" NavigateUrl="~/Search.aspx">searching</asp:HyperLink> for your listing instead.</li>
            <li>Browse the website by <asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl="~/Categories.aspx">category</asp:HyperLink>.</li>
            <li>Visit the <asp:HyperLink ID="HyperLink5" runat="server" NavigateUrl="~/Listings.aspx">Top Listings</asp:HyperLink> page.</li>
        </ul>
    </asp:View>
    <asp:View ID="SendToFriendFormView" runat="server" OnActivate="SendToFriendFormView_Activate">
        <h1>Send to a Friend</h1>
        <p>Spread the word! Tell your friends, family or collegues about this listing.</p>
        <asp:Label ID="SendToFriendErrorLabel" runat="server" Visible="False" ForeColor="Red">
            <p><asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Static/Layout/ErrorIcon.gif" ImageAlign="TextTop" />&nbsp;There was a problem validating the correctness of some fields, please check and try again.</p>
        </asp:Label>
        <div class="GenericFormHeader">Information</div>
        <div class="GenericFormBody">
            <div class="GenericFormSegment">
                <h1>Friend's Email</h1>
                <asp:TextBox ID="SendToFriendEmailTextBox" Width="250px" runat="server" CssClass="GenericFormTextBox" />
                <asp:RequiredFieldValidator ID="SendToFriendEmailRequiredValidator" runat="server" ControlToValidate="SendToFriendEmailTextBox" Display="Dynamic" ValidationGroup="SendToFriend">
                    <br />Please type your friends email address (e.g. joe.bloggs@example.com).
                </asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="SendToFriendEmailRegexValidator" runat="server" ControlToValidate="SendToFriendEmailTextBox" Display="Dynamic" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="SendToFriend">
                    <br />The address does not appear to be valid (e.g. joe.bloggs@example.com).
                </asp:RegularExpressionValidator>
            </div>
            <div class="GenericFormSegment">
                <h1>Short Message</h1>
                <asp:TextBox ID="SendToFriendMessageTextBox" Width="300px" Height="100px" TextMode="MultiLine" runat="server" CssClass="GenericFormTextBox" />
                <asp:RequiredFieldValidator ID="SendToFriendMessageRequiredValidator" runat="server" ControlToValidate="SendToFriendMessageTextBox" Display="Dynamic" ValidationGroup="SendToFriend">
                    <br />Please type a question to ask the seller.
                </asp:RequiredFieldValidator>
            </div>
        </div>
        <br class="GenericFormSeparator" />
        <asp:ImageButton ID="SendToFriendButton" runat="server" ImageUrl="~/Images/Static/Layout/ContinueButton.gif" AlternateText="Continue" CausesValidation="False" OnClick="SendToFriendButton_Click" />
    </asp:View>
    <asp:View ID="SendToFriendDoneView" runat="server">
        <h1>Message Sent</h1>
        <p>Thank you for sending this listing to someone.</p>
        <h3>Where would you like to go?</h3>
        <ul>
            <li><asp:HyperLink ID="SendToFriendBackHyperLink" runat="server">Back to listing</asp:HyperLink></li>
        </ul>
    </asp:View>
    <asp:View ID="ReportAbuseStartView" runat="server">
        <h1>Report Violation</h1>
        <p>Are you sure you want to report this listing as a violation? Please make sure you read the <asp:HyperLink ID="HyperLink6" runat="server" NavigateUrl="Help.aspx?Topic=ListingPolicy">Listing Policy</asp:Hyperlink> carefully before hand.</p>
        <asp:Label ID="ReportAbuseErrorLabel" runat="server" Visible="False" ForeColor="Red">
            <p><asp:Image ID="Image2" runat="server" ImageUrl="~/Images/Static/Layout/ErrorIcon.gif" ImageAlign="TextTop" />&nbsp;There was a problem validating the correctness of some fields, please check and try again.</p>
        </asp:Label>
        <div class="GenericFormHeader">Details</div>
        <div class="GenericFormBody">
            <div class="GenericFormSegment">
                <h1>Reason</h1>
                <p>Please select a reason, so the seller knows what to fix.</p>
                <p>
                    <asp:RadioButton ID="ReportAbuseRadioButton1" runat="server" GroupName="ReportAbuseReasons" Text="It's in the wrong category" /><br />
                    <asp:RadioButton ID="ReportAbuseRadioButton2" runat="server" GroupName="ReportAbuseReasons" Text="It looks like a scam, or looks fraudulent" /><br />
                    <asp:RadioButton ID="ReportAbuseRadioButton3" runat="server" GroupName="ReportAbuseReasons" Text="Other:" />&nbsp;<asp:TextBox ID="ReportAbuseOtherTextBox" Width="250px" runat="server" CssClass="GenericFormTextBox" />
                </p>
                <asp:CustomValidator ID="ReportAbuseRadioButtonCustomValidator" runat="server" 
                    ValidationGroup="ReportAbuse" 
                    onservervalidate="ReportAbuseRadioButtonCustomValidator_ServerValidate" Display="Dynamic">
                    <p>No reason was selected, please select a radio button.</p>
                </asp:CustomValidator>
                <asp:CustomValidator ID="ReportAbuseOtherTextBoxCustomValidator" runat="server" 
                    ValidationGroup="ReportAbuse" 
                    onservervalidate="ReportAbuseOtherTextBoxCustomValidator_ServerValidate" Display="Dynamic">
                    <p>The other radio button is selected, but no reason was provided.</p>
                </asp:CustomValidator>
            </div>
        </div>
        <br class="GenericFormSeparator" />
        <asp:ImageButton ID="ReportAbuseContinueImageButton" runat="server" 
            ImageUrl="~/Images/Static/Layout/ContinueButton.gif" AlternateText="Continue" 
            CausesValidation="False" onclick="ReportAbuseContinueImageButton_Click" />&nbsp;<asp:ImageButton 
            ID="ReportAbuseCancelImageButton" runat="server" 
            ImageUrl="~/Images/Static/Layout/CancelButton.gif" AlternateText="Cancel" 
            CausesValidation="False" onclick="ReportAbuseCancelImageButton_Click" />
    </asp:View>
    <asp:View ID="ReportAbuseDoneView" runat="server">
        <h1>Report Violation</h1>
        <p>Thanks for reporting this listing, you've helped to make ManxAds better. We will investigate this as soon as we can.</p>
        <ul>
            <li><asp:HyperLink ID="HyperLink7" runat="server" NavigateUrl="~/Default.aspx">Go to home page</asp:HyperLink></li>
        </ul>
    </asp:View>
</asp:MultiView>