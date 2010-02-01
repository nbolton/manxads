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


public partial class SearchInline : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DefaultButton defaultButton = new DefaultButton(SearchButton);
        defaultButton.AssociateWith(SearchTextBox);
        ListingCountEstimateLabel.Text = Listing.CountEstimate.ToString();

        if (!IsPostBack)
        {
            SearchTextBox.Text = Request.QueryString["Search"];
        }
    }

    protected void SearchButton_Click(object sender, ImageClickEventArgs e)
    {
        string url = "~/Listings.aspx?Search=" + Server.UrlEncode(SearchTextBox.Text);
        if (!String.IsNullOrEmpty(Request.QueryString["Any"])) url += "&Any=1";
        Response.Redirect(url, false);
    }
}
