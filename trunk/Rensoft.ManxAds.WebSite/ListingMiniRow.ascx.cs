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
using ManxAds;


public partial class ListingMiniRow : BrowserRow
{
    protected ListingMiniRow()
        : base(true, LocalSettings.ListingDetailsPopup) { }

    protected override void OnDataBinding(EventArgs e)
    {
        Listing listing = (Listing)((DataListItem)Parent).DataItem;

        // Set these properties for use in overriden method.
        base.NavigateUrl = listing.NavigateUrl;
        base.ImageUrl = listing.MasterImage.ThumbnailUrl;

        // Call overriden once NavigateUrl is set.
        base.OnDataBinding(e);

        this.ShortTitleHyperLink.Text = listing.ShortTitle;
        this.ShortTitleHyperLink.NavigateUrl = NavigateUrl;
        this.DescriptionLabel.Text = listing.ShortDetails;
        this.PriceColouredLabel.Text = listing.PriceColoured;
        
        if (IsPopup)
        {
            ShortTitleHyperLink.Attributes.Add(
                "onclick", PopupAttributeValue);
        }
    }
}
