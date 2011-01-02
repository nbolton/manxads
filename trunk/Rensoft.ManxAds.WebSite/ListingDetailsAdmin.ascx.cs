using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rensoft.ManxAds.WebSite
{
    public partial class ListingDetailsAdmin : StandardControl
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            // HACK: this can be called from the index page when there is no listing,
            // so just drop out here if no listing was requested.
            if (!Accessors.HasListing)
                return;

            EditListingHyperLink.NavigateUrl = Accessors.Listing.ModifyUrl;
            EditUserHyperLink.NavigateUrl = Accessors.Listing.Seller.ModifyUrl;
        }
    }
}