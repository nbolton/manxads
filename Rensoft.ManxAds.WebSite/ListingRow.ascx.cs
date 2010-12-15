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

using System.IO;
using ManxAds;

public partial class ListingRow : BrowserRow
{
    private Listing listing;

    protected Listing Listing
    {
        get { return listing; }
        set
        {
            listing = value;
            ImageUrl = listing.ThumbnailUrl;
            ModifyUrl = listing.ModifyUrl;
            RemoveUrl = listing.RemoveUrl;
            NavigateUrl = listing.NavigateUrl;
        }
    }

    public delegate void ListingEventArgs(Listing listing);
    public event ListingEventArgs BoostRequest;
    public event ListingEventArgs RestoreListing;
    public event EventHandler ListingUpdated;

    protected ListingRow()
        : base(true, LocalSettings.ListingDetailsPopup)
    {
        BoostRequest += new ListingEventArgs(OnBoostRequest);
        RestoreListing += new ListingEventArgs(OnRestoreListing);
        ListingUpdated += new EventHandler(OnListingUpdated);
    }

    protected virtual void OnListingUpdated(object sender, EventArgs e) { }

    protected virtual void OnRestoreListing(Listing listing)
    {
        Authentication auth = new Authentication(this.Page);
        WebsiteUser user = auth.ActiveUser;

        if ((listing != null) && listing.AuthoriseEdit(user))
        {
            listing.Restore();
        }
    }

    protected virtual void OnBoostRequest(Listing listing)
    {
        Authentication auth = new Authentication(this.Page);
        WebsiteUser user = auth.ActiveUser;

        if ((listing != null) && listing.AuthoriseEdit(user))
        {
            if (Common.AdminMode)
            {
                // Skip data limits for admins.
                listing.SetBoostDateManual(DateTime.Now);
            }
            else
            {
                // Use this property, enforcing limits.
                listing.BoostDate = DateTime.Now;
            }

            listing.Modify();
        }
    }

    protected override void OnDataBinding(EventArgs e)
    {
        // Data item is not always set.
        Listing = (Listing)((DataListItem)Parent).DataItem;

        base.OnDataBinding(e);

        this.TitleHyperLink.Text = Listing.Title;
        this.TitleHyperLink.NavigateUrl = NavigateUrl;
        this.PriceColouredLabel.Text = Listing.PriceColoured;
        this.CreateDateLabel.Text = Listing.FormattedCreateDate;
        this.DescriptionLabel.Text = Listing.LongDetails;
        this.BoostLinkButton.CommandArgument = listing.DatabaseId.ToString();
        this.RestoreLinkButton.CommandArgument = listing.DatabaseId.ToString();

        if (Listing.Seller.IsTrader)
        {
            TraderTagPanel.Visible = true;
            TraderPanel.Visible = true;
            TraderLogoImage.AlternateText = Listing.Seller.TradingName;
            TraderTagHyperLink.NavigateUrl = NavigateUrl;

            if (File.Exists(Server.MapPath(Listing.Seller.TraderLogoRelativePath)))
            {
                TraderLogoImage.ImageUrl = Listing.Seller.TraderLogoRelativePath;
            }
            else
            {
                TraderLogoImage.ImageUrl = LocalSettings.TraderLogoPlaceholder;
            }

            switch (Listing.Seller.TraderType)
            {
                case TraderType.Corporate:
                    TraderTagImage.ImageUrl = "~/Images/Static/Layout/CorporateTraderTag.gif";
                    TraderTagImage.AlternateText = "Trader Listing";
                    break;

                case TraderType.Charity:
                    TraderTagImage.ImageUrl = "~/Images/Static/Layout/CharityTraderTag.gif";
                    TraderTagImage.AlternateText = "Charity Listing";
                    break;
            }
        }
        
        if (!Common.AdminMode && !Listing.CanBoost)
        {
            BoostLinkButton.Enabled = false;

            if (listing.IsBoosted)
            {
                // Indicate it is due to a per-listing time limit.
                BoostLinkButton.Text +=
                    " (Frozen for " + Listing.BoostLimitExpiryHoursString + ")";
            }
            else if (Listing.BoostLimitReached)
            {
                // If explicity not allowed due to limit, show disabled.
                BoostLinkButton.Text += " (Limit of " + LocalSettings.BoostCountLimit + " reached)";
            }
        }

        if (IsPopup)
        {
            TitleHyperLink.Attributes.Add(
                "onclick", PopupAttributeValue);
        }

        bool recycleBinMode = (Request.QueryString["RecycleBin"] != null) ? true : false;
        if (recycleBinMode)
        {
            RestoreLinkButton.Visible = true;
            RemoveLinkButton.Visible = false;

            if (listing.Seller.ListingLimitReached)
            {
                RestoreLinkButton.Enabled = false;
                RestoreLinkButton.Text += " (Limit Reached)";
            }
        }
    }

    protected void BoostLinkButton_Click(object sender, EventArgs e)
    {
        LinkButton linkButton = sender as LinkButton;
        int databaseId = int.Parse(linkButton.CommandArgument);

        // Updates boost time if out of date.
        this.listing = Listing.Fetch(databaseId);

        if (Common.AdminMode || this.listing.CanBoost)
        {
            // CanBoost may have changed since last refresh.
            BoostRequest(this.listing);
        }

        // Tell browser to re-bind list.
        ListingUpdated(this, EventArgs.Empty);
    }

    protected void RestoreLinkButton_Click(object sender, EventArgs e)
    {
        LinkButton linkButton = sender as LinkButton;
        int databaseId = int.Parse(linkButton.CommandArgument);
        RestoreListing(Listing.Fetch(databaseId));
    }
}
