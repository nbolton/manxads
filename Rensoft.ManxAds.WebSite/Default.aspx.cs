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

public partial class _Default : StandardPage
{
    public _Default() : base(WebsiteUserType.Public, false) { }

    protected override void InitializePage()
    {
        if (!string.IsNullOrEmpty(Request.QueryString["TestError"]))
        {
            throw new Exception("Test exception.");
        }

        if (!String.IsNullOrEmpty(Request.QueryString["Listing"]) ||
            !String.IsNullOrEmpty(Request.QueryString["ListingId"]))
        {
            LoadDetails();
        }
        else
        {
            LoadHome();
        }

        base.InitializePage();
    }

    protected void LoadHome()
    {
        int listingsLimit = LocalSettings.WelcomeTopListingsLimit;
        ListingsDataList.DataSource = Listing.FetchTop(listingsLimit, true);
        ListingsDataList.DataBind();

        int categoriesLimit = LocalSettings.WelcomeTopCategoriesLimit;
        CategoriesDataList.DataSource = Category.FetchTop(categoriesLimit);
        CategoriesDataList.DataBind();

        MultiView.SetActiveView(HomeView);
    }

    protected void LoadDetails()
    {
        MultiView.SetActiveView(DetailsView);
    }
}
