using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ManxAds;

namespace Rensoft.ManxAds.WebSite
{
    public partial class ListingDetailsAdmin : StandardControl
    {
        public void Initialize(Listing listing)
        {
            EditListingHyperLink.NavigateUrl = listing.ModifyUrl;
            EditUserHyperLink.NavigateUrl = listing.Seller.ModifyUrl;
            DeleteListingHyperLink.NavigateUrl = listing.RemoveUrl;
            BanUserHyperLink.NavigateUrl = listing.Seller.BanUrl;
        }
    }
}