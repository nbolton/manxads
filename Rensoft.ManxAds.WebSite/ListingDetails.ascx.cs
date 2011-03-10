using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Net.Mail;
using System.IO;
using ManxAds;
using SDImage = System.Drawing.Image;
using UIImage = System.Web.UI.WebControls.Image;
using Rensoft.ManxAds;

public partial class ListingDetails : StandardControl
{
    protected string FullListingUrl
    {
        get
        {
            return Accessors.Listing.NavigateUrl.Replace("~", "http://www.manxads.com");
        }
    }

    protected void SetHyperLinkTargets(Control parent, string target)
    {
        foreach (Control child in parent.Controls)
        {
            if (child is HyperLink)
            {
                ((HyperLink)child).Target = target;
            }

            if (child.Controls.Count > 0)
            {
                SetHyperLinkTargets(child, target);
            }
        }
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        // HACK: this can be called from the index page when there is no listing,
        // so just drop out here if no listing was requested.
        if (!Accessors.HasListing)
            return;

        try
        {
            // Force title appendage (default could be false).
            Common.AppendTitle = true;
            Common.Title = Accessors.Listing.Title;

            // put listing id on page so javascript can get to it
            ListingId.Value = Accessors.ListingId.ToString();

            if (Page.Master != null)
            {
                Common.Description = Accessors.Listing.DetailsWithoutHtml;
            }

            SendToFriendHyperLink.NavigateUrl = Request.RawUrl + "&SendToFriend=1";
            SendToFriendBackHyperLink.NavigateUrl = Request.RawUrl.Replace("&SendToFriend=1", null);

            if ((StandardMaster != null) && (Accessors.Listing.ImageCount != 0))
                StandardMaster.LinkImageUrl = Accessors.Listing.SmallImageUrl;

            if ((Auth.IsAuthenticated) &&
                ((Auth.ActiveUser.UserType & WebsiteUserType.AdministratorOnly) != 0))
            {
                // show listing details admin control for users with admin right
                ListingDetailsAdmin1.Visible = true;
                ListingDetailsAdmin1.Initialize(Accessors.Listing);

                ListingDetailsAdmin2.Visible = true;
                ListingDetailsAdmin2.Initialize(Accessors.Listing);
            }

            this.BindBreadcrumbNode(Accessors.Listing);

            if (!IsPostBack)
            {
                if (!Accessors.Listing.Enabled)
                {
                    MultiView.SetActiveView(DeletedView);
                    return;
                }
                
                // User could be redirected from login screen.
                if (Request.QueryString["SendToFriend"] != null)
                {
                    if (Auth.IsAuthenticated)
                    {
                        MultiView.SetActiveView(SendToFriendFormView);
                    }
                    else
                    {
                        FormsAuthentication.RedirectToLoginPage();
                    }
                    return;
                }

                this.BindListing(Accessors.Listing);
                this.BindImages(Accessors.Listing);

                if (Accessors.Listing.IsTraderListing)
                {
                    this.BindTraderDetails(Accessors.Listing);
                }

                // When user visits page, record hit.
                Accessors.Listing.IncrementPageHit();
                PageHitsLabel1.Text = Accessors.Listing.PageHits.ToString();
                PageHitsLabel2.Text = Accessors.Listing.PageHits.ToString();

                MultiView.SetActiveView(DetailsView);

                if (LocalSettings.ListingDetailsPopup)
                {
                    SetHyperLinkTargets(this, "_blank");
                }
            }
        }
        catch (NotFoundException)
        {
            // ID was invalid; listing not found.
            MultiView.SetActiveView(UnavailableView);
        }
        catch (FormatException)
        {
            // ID was not well formed; query string invalid.
            MultiView.SetActiveView(UnavailableView);
        }
    }

    protected void BindTraderDetails(Listing listing)
    {
        TraderPanel.Visible = true;
        TraderWebsiteHyperLink.NavigateUrl = listing.TraderWebsite;
        TraderLogoImage.ImageUrl = listing.TraderLogo;
        TraderLogoHyperLink.NavigateUrl = listing.TraderWebsite;
        TraderNameLabel.Text = listing.TraderName;

        if (File.Exists(Server.MapPath(listing.Seller.TraderLogoRelativePath)))
        {
            TraderLogoImage.ImageUrl = listing.Seller.TraderLogoRelativePath;
        }
        else
        {
            TraderLogoImage.ImageUrl = LocalSettings.TraderLogoPlaceholder;
        }

        switch (listing.Seller.TraderType)
        {
            case TraderType.Corporate:
                TraderTypeLabel.Text = "Corporate Trader";
                break;

            case TraderType.Charity:
                TraderTypeLabel.Text = "Charity Organisation";
                break;
        }
    }

    protected void BindBreadcrumbNode(Listing listing)
    {
        if (!LocalSettings.ListingDetailsPopup)
        {
            BreadcrumbTrail.Visible = true;
            this.BreadcrumbTrail.AddNode("Categories", "Categories.aspx");

            if (Accessors.CategoryId != 0)
            {
                Category parent = Accessors.Category;
                this.BreadcrumbTrail.AddNode(parent.Title, parent.NavigateUrl);
            }
            else if (listing.Categories.Count > 0)
            {
                ICategory parent = listing.Categories[0];
                this.BreadcrumbTrail.AddNode(parent.Title, parent.NavigateUrl);
            }

            this.BreadcrumbTrail.AddNode(listing.Title, listing.NavigateUrl);
        }
    }

    protected void BindListing(Listing listing)
    {
        TitleLabel.Text = listing.Title;
        DetailsLabel.Text = listing.DetailsFormatted;
        DateTableCell.Text = listing.CreateDateString;
        SellerNameTableCell.Text = listing.Seller.FullName;
        TermsTableCell.CssClass = listing.PriceColourStyle;
        ListingImage.AlternateText = listing.Title;
        SearchListingsHyperLink.NavigateUrl = listing.GetSearchSimilarUrl(Server);

        // Only show report box if user is logged in.
        if (Auth.IsAuthenticated)
        {
            ListingAbuseReportUtility lau = new ListingAbuseReportUtility(LocalSettings.ConnectionString);
            if (lau.IsReported(listing.DatabaseId, Auth.ActiveUser.DatabaseId))
            {
                ReportAbuseMultiView.SetActiveView(ReportAbuseViewReported);
            }
            else
            {
                ReportAbuseMultiView.SetActiveView(ReportAbuseViewUnreported);
            }
        }

        if (listing.PriceType != PriceType.None)
        {
            PriceTableCell.Text = listing.PriceColoured;
            TermsTableCell.Text = listing.PriceTypeString;
        }

        if (listing.MasterImageId != 0)
        {
            ThumbnailImage.Visible = true;
            ThumbnailImage.ImageUrl = listing.MasterImage.ThumbnailUrl;
        }

        if (listing.ShowLocation)
        {
            if (listing.LocationId > 0)
            {
                LocationTableCell.Text = listing.LocationString;
            }
            if (listing.Seller.LocationId > 0)
            {
                SellerLocationTableCell.Text = listing.Seller.LocationString;
            }
        }

        if (listing.Seller.HasLandlinePhone && listing.ShowLandline)
        {
            LandlinePhoneTableCell.Text = listing.Seller.LandlinePhoneFull;
        }

        if (listing.Seller.HasMobilePhone && listing.ShowMobile)
        {
            MobilePhoneTableCell.Text = listing.Seller.MobilePhoneFull;
        }

        SellerContactHyperLink.NavigateUrl = listing.ContactUrl;
        SellerListingsHyperLink.NavigateUrl = listing.Seller.ListingsUrl;

        if (LocalSettings.ListingDetailsPopup)
        {
            SellerListingsHyperLink.Target = "_blank";
        }
    }

    protected void BindImages(Listing listing)
    {
        if (listing.ImageCount > 0)
        {
            ListingImagePanel.Visible = true;
            ListingImage.ImageUrl = listing.FetchImage(
                listing.MasterImageId).FullImageUrl;

            if (listing.ImageCount > 1)
            {
                //Repeater PreLoadRepeater = (Repeater)ImageUpdatePanel.FindControl("PreLoadRepeater");
                //PreLoadRepeater.DataSource = listing.Images;
                //PreLoadRepeater.DataBind();

                ListingImageRepeater.DataSource = listing.Images;
                ListingImageRepeater.DataBind();
            }
        }
    }

    protected void ThumbnailImageButton_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton imageButton = sender as ImageButton;
        int imageId = int.Parse(imageButton.CommandArgument);
        setListingImage(Accessors.Listing.FetchImage(imageId).FullImageUrl);
    }

    protected void DeletedView_Activate(object sender, EventArgs e)
    {
        SearchHyperLink1.NavigateUrl = Accessors.Listing.GetSearchSimilarUrl(Server);
        OtherListingsHyperLink.NavigateUrl = Accessors.Listing.Seller.ListingsUrl;
    }

    protected void UnavailableView_Activate(object sender, EventArgs e)
    {
        if (Request.QueryString["Title"] != null)
        {
            string keywords = Request.QueryString["Title"].Replace('-', ' ');
            SearchHyperLink2.NavigateUrl = String.Format(LocalSettings.SearchListingsUrlFormat, keywords);
        }
    }

    protected void SendToFriendFormView_Activate(object sender, EventArgs e)
    {
        SendToFriendMessageTextBox.Text =
            "Hi, I thought you would be interested in a " +
            "listing I found on ManxAds... \r\n\r\n" + FullListingUrl;

        DefaultButton defaultButton = new DefaultButton(SendToFriendButton);
        defaultButton.AssociateWith(SendToFriendEmailTextBox);
    }

    protected void SendToFriendButton_Click(object sender, ImageClickEventArgs e)
    {
        Page.Validate("SendToFriend");
        if (!Page.IsValid)
        {
            SendToFriendErrorLabel.Visible = true;
            return;
        }

        MailAddress senderEmail = new MailAddress(LocalSettings.MasterSendFromEmail, "ManxAds.com");
        MailAddress fromEmail = new MailAddress(Auth.ActiveUser.EmailAddress, Auth.ActiveUser.FullName);
        MailAddress toEmail = new MailAddress(SendToFriendEmailTextBox.Text);

        MailMessage message = new MailMessage(fromEmail, toEmail);
        message.Sender = senderEmail;

        message.Subject = String.Format(
            LocalSettings.ListingContactFormat,
            Accessors.Listing.ManxAdsId,
            Accessors.Listing.Title);

        message.Body = "Hello!\r\n\r\n";
        message.Body += "This is a message from ManxAds on behalf of " + Auth.ActiveUser.FullName + ". ";
        message.Body += "ManxAds is not responsible for the contents of this email.\r\n\r\n";
        message.Body += SendToFriendMessageTextBox.Text + "\r\n\r\n";
        message.Body += "ManxAds - The Manx Classifieds Website - www.manxads.com";

        EmailTools.SendMessage(message, Page);

        MultiView.SetActiveView(SendToFriendDoneView);
    }

    protected void SendToFriendBackLinkButton_Click(object sender, EventArgs e)
    {
        MultiView.SetActiveView(DetailsView);
    }
    protected void ListingImageRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if ((e.Item.ItemType == ListItemType.Item) ||
            (e.Item.ItemType == ListItemType.AlternatingItem))
        {
            ListingImage listingImage = e.Item.DataItem as ListingImage;

            //string onClick = "return setMainImage('" + ListingImage.ClientID + "', this);";
            //HyperLink hyperLink = e.Item.FindControl("ThumnbnailHyperLink") as HyperLink;
            //hyperLink.NavigateUrl = listingImage.FullImageUrl;
            //hyperLink.Attributes["onclick"] = onClick;

            LinkButton tlb = (LinkButton)e.Item.FindControl("ThumnbnailLinkButton");
            tlb.CommandArgument = listingImage.FullImageUrl;
        }
    }

    protected void ThumbnailLinkButton_Click(object sender, EventArgs e)
    {
        LinkButton ThumbnailLinkButton = (LinkButton)sender;
        setListingImage(ThumbnailLinkButton.CommandArgument);
    }

    private void setListingImage(string imageUrl)
    {
        ListingImage.ImageUrl = imageUrl;

        // Where the file exists, find out the image dimensions.
        if (File.Exists(getImageFilePath(imageUrl)))
        {
            SDImage sdImage = SDImage.FromFile(getImageFilePath(imageUrl));
            ListingImage.Width = sdImage.Width;
            ListingImage.Height = sdImage.Height;
        }
    }

    private string getImageFilePath(string imageUrl)
    {
        return Server.MapPath(imageUrl);
    }

    protected void ReportAbuseButton_Click(object sender, EventArgs e)
    {
        MultiView.SetActiveView(ReportAbuseStartView);
    }

    protected void ReportAbuseContinueImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Page.Validate("ReportAbuse");
        if (!Page.IsValid)
        {
            ReportAbuseErrorLabel.Visible = true;
            return;
        }

        bool isCustom = false;
        string reason;

        if (ReportAbuseRadioButton1.Checked)
        {
            reason = ReportAbuseRadioButton1.Text;
        }
        else if (ReportAbuseRadioButton2.Checked)
        {
            reason = ReportAbuseRadioButton2.Text;
        }
        else if (ReportAbuseRadioButton3.Checked)
        {
            isCustom = true;
            reason = ReportAbuseOtherTextBox.Text;
        }
        else
        {
            throw new Exception("No reason radio button was selected.");
        }

        ListingAbuseReportUtility lau = new ListingAbuseReportUtility(LocalSettings.ConnectionString);
        lau.Report(Accessors.Listing, Auth.ActiveUser.DatabaseId, reason, isCustom);
        MultiView.SetActiveView(ReportAbuseDoneView);
    }

    protected void ReportAbuseCancelImageButton_Click(object sender, ImageClickEventArgs e)
    {
        ReportAbuseErrorLabel.Visible = false;
        ReportAbuseRadioButton1.Checked = false;
        ReportAbuseRadioButton2.Checked = false;
        ReportAbuseRadioButton3.Checked = false;

        MultiView.SetActiveView(DetailsView);
    }

    protected void ReportAbuseRadioButtonCustomValidator_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid =
            ReportAbuseRadioButton1.Checked ||
            ReportAbuseRadioButton2.Checked ||
            ReportAbuseRadioButton3.Checked;
    }

    protected void ReportAbuseOtherTextBoxCustomValidator_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid =
            !ReportAbuseRadioButton3.Checked ||
            (ReportAbuseRadioButton3.Checked && !string.IsNullOrEmpty(ReportAbuseOtherTextBox.Text));
    }
}