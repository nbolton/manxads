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

public partial class TraderList : StandardPage
{
    public TraderList() : base(WebsiteUserType.Public) { }

    protected override void InitializePage()
    {
        base.InitializePage();
        ItemGridView.DataSource = WebsiteUser.Fetch(TraderType.Corporate);
        ItemGridView.DataBind();
    }

    protected void ItemGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            WebsiteUser user = e.Row.DataItem as WebsiteUser;

            Label nameLabel = e.Row.FindControl("NameLabel") as Label;
            Label listingCountLabel = e.Row.FindControl("ListingCountLabel") as Label;
            HyperLink websiteHyperLink = e.Row.FindControl("WebsiteHyperLink") as HyperLink;
            HyperLink listingsHyperLink = e.Row.FindControl("ListingsHyperLink") as HyperLink;

            nameLabel.Text = user.TradingName;
            listingCountLabel.Text = user.ListingCount.ToString();
            listingsHyperLink.NavigateUrl = "~/Listings.aspx?Seller=" + user.DatabaseId;

            websiteHyperLink.NavigateUrl = user.TradingWebsite;
            if (string.IsNullOrEmpty(user.TradingWebsite))
            {
                websiteHyperLink.Enabled = false;
            }
        }
    }
}
