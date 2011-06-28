using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.Common;
using System.Text.RegularExpressions;

using ManxAds.Search;
using System.Data.SqlTypes;
using ManxAds;

public partial class ListingBrowser : StandardControl
{
    protected override void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            foreach (ListItem item in SortDropDownList.Items)
            {
                if (item.Value == Request.QueryString["Sort"])
                {
                    item.Selected = true;
                    break;
                }
            }
        }

        DataBindListings();
    }

    protected void DataBindListings()
    {
        if ((Accessors.CategoryId != 0) || Common.CategoryPreloaded)
        {
            TryLoadCategory();
            return;
        }

        switch (Common.PageMode)
        {
            case PageMode.SelfOnly:
                this.LoadSelfOnlyMode();
                break;

            case PageMode.Search:
                this.LoadSearchMode();
                break;

            case PageMode.Seller:
                this.LoadSellerMode();
                break;

            default:
                this.LoadTopListings();
                break;
        }
    }

    private void TryLoadCategory()
    {
        try
        {
            // Can't rely on PageMode for Category.
            this.LoadCategory();
        }
        catch (NotFoundException)
        {
            BreadcrumbTrail.Visible = false;
            MultiView.SetActiveView(ErrorView);
        }
    }

    private void LoadSearchMode()
    {
        TitleLabel.Visible = false;

        string searchPhrase = Request.QueryString["Search"].Trim();

        // if search terms are empty, use a wildcard char
        if (string.IsNullOrEmpty(searchPhrase))
            searchPhrase = "*";

        // Show extended seach panel.
        SearchPanel.Visible = true;

        // Check for email address search.
        if (searchPhrase.Contains("@"))
        {
            try
            {
                // Consider revising this so false is returned where email not found.
                LoadSellerMode(WebsiteUser.Fetch(searchPhrase.ToLower()));
                return;
            }
            catch (NotFoundException) { }
        }

        Regex tagSearch = new Regex(LocalSettings.ManxAdsIdPrefix + "[0-9]+");
        Match match = tagSearch.Match(searchPhrase);
        if (match.Success)
        {
            int listingId = int.Parse(match.Value.Replace(
                LocalSettings.ManxAdsIdPrefix, String.Empty));

            try
            {
                Listing listing = Listing.Fetch(listingId);
                Response.Redirect(listing.NavigateUrl, false);
                return;
            }
            catch (NotFoundException) { }
        }

        if (searchPhrase != "*")
        {
            this.Common.Title = searchPhrase;
            this.Common.Description = "ManxAds search results for '" + searchPhrase + "'.";
        }
        else
        {
            this.Common.Title = "Search";
            this.Common.Description = "ManxAds search results.";
        }

        this.MultiView.SetActiveView(DefaultView);

        bool advancedMode = !string.IsNullOrEmpty(Request.QueryString["Advanced"]);
        bool anyKeywords = !string.IsNullOrEmpty(Request.QueryString["Any"]);

        if (!advancedMode && (searchPhrase == "*"))
        {
            MultiView.SetActiveView(SimpleSearchErrorView);
            return;
        }

        string catIdString = Request.QueryString["SearchCategoryId"];
        SearchCriteria critera = new SearchCriteria(searchPhrase, anyKeywords);

        if (!String.IsNullOrEmpty(catIdString))
            critera.SetCategoryId(catIdString);

        if (!String.IsNullOrEmpty(Request.QueryString["Seller"]))
            critera.SetSellerId(Request.QueryString["Seller"]);

        if (advancedMode)
        {
            string startDate = Request.QueryString["StartDate"];
            string endDate = Request.QueryString["EndDate"];
            string startPrice = Request.QueryString["StartPrice"];
            string endPrice = Request.QueryString["EndPrice"];

            bool dateOrPriceEmpty = 
                ((string.IsNullOrEmpty(startDate) && string.IsNullOrEmpty(endDate)) &&
                (string.IsNullOrEmpty(startPrice) && string.IsNullOrEmpty(endPrice)));

            if ((searchPhrase == "*") && (string.IsNullOrEmpty(catIdString) || dateOrPriceEmpty))
            {
                MultiView.SetActiveView(AdvancedSearchErrorView);
                return;
            }

            critera.SetStartDate(startDate);
            critera.SetEndDate(endDate);
            critera.SetStartPrice(startPrice);
            critera.SetEndPrice(endPrice);
            critera.SetLocationId(Request.QueryString["LocationId"]);
        }

        // Start the timer for database transaction.
        int startTime = Environment.TickCount;

        // Run search query at database end.
        List<Listing> searchItems = Listing.Search(critera);

        // Calculate elapsed time for database transaction.
        int elapsed = Environment.TickCount - startTime;
        double seconds = Math.Round(((double)elapsed / 1000f), 2);

        try
        {
            Common.PagingManager = new PagingManager<Listing>(
                searchItems, LocalSettings.DefaultPageLimit, Common.PageNumber);
        }
        catch (ArgumentOutOfRangeException)
        {
            MultiView.SetActiveView(PageNotFoundView);
            return;
        }

        if (!Common.PagingManager.HasItems)
        {
            MultiView.SetActiveView(SearchEmptyView);
            return;
        }

        DescriptionLabel.Text = "Results <b>" +
            Common.PagingManager.StartNumber + " - " +
            Common.PagingManager.StopNumber + "</b> " +
            " of <b>" + Common.PagingManager.ItemCount + "</b> " +
            "(<b>" + seconds + "</b> seconds)";

        Common.PagingManager.DataBind(ListingDataList);
        MultiView.SetActiveView(DefaultView);
    }

    protected void LoadCategory()
    {
        // Allow sorting for top listings.
        SortingPanel.Visible = true;

        // Show search (automatically selects category).
        this.SearchPanel.Visible = true;

        this.BreadcrumbTrail.Visible = true;
        this.BreadcrumbTrail.AddNode("Categories", "~/Categories.aspx");
        this.BreadcrumbTrail.AddNode(Accessors.Category.Title, Accessors.Category.NavigateUrl);

        this.Common.Title = Accessors.Category.Title;
        this.Common.Description = Accessors.Category.Description;
        this.TitleLabel.Text = Accessors.Category.Title;
        this.DescriptionLabel.Text = Accessors.Category.Description;
        this.MultiView.SetActiveView(DefaultView);

        try
        {
            Common.PagingManager = new PagingManager<Listing>(
                Listing.Fetch(Accessors.Category, SortDropDownList.SelectedValue),
                LocalSettings.DefaultPageLimit,
                Common.PageNumber);
        }
        catch (ArgumentOutOfRangeException)
        {
            // Nonexistant page has been requested.
            MultiView.SetActiveView(PageNotFoundView);
            return;
        }

        Common.PagingManager.DataBind(ListingDataList);
        MultiView.SetActiveView(DefaultView);

        if (!Common.PagingManager.HasItems)
        {
            MultiView.SetActiveView(EmptyView);
        }
    }

    protected void LoadSellerMode()
    {
        try
        {
            int sellerId;
            int.TryParse(Request.QueryString["Seller"], out sellerId);

            this.LoadSellerMode(ManxAds.WebsiteUser.Fetch(sellerId));
        }
        catch (NotFoundException)
        {
            MultiView.SetActiveView(SellerNotFoundView);
            return;
        }
    }

    protected void LoadSellerMode(WebsiteUser seller)
    {
        string sellerDisplayName;
        if (seller.IsTrader)
        {
            sellerDisplayName = seller.TradingName;
        }
        else
        {
            sellerDisplayName = seller.FullName;
        }

        TitleLabel.Text = "Listings by '" + sellerDisplayName + "'";
        Common.Title = "Listings by '" + sellerDisplayName + "'";
        Common.Description = "View all listings made by '" + sellerDisplayName + "' on ManxAds.";

        this.MultiView.SetActiveView(DefaultView);

        if (seller.IsTrader)
        {
            SearchPanel.Visible = true;
        }

        Common.PagingManager = new PagingManager<Listing>(
            seller.Listings, LocalSettings.DefaultPageLimit, Common.PageNumber);

        Common.PagingManager.DataBind(ListingDataList);
        MultiView.SetActiveView(DefaultView);

        if (!Common.PagingManager.HasItems)
        {
            MultiView.SetActiveView(SellerEmptyView);
        }
    }

    private void LoadSelfOnlyMode()
    {
        DescriptionPanel.Visible = false;
        UserHomePanel.Visible = true;

        if (!Auth.IsAuthenticated)
        {
            // Log off unauthenticated user.
            Auth.LogOff(true);
        }

        bool recycleBinMode = (Request.QueryString["RecycleBin"] != null) ? true : false;
        this.TitleLabel.Text = recycleBinMode ? "Recycle Bin" : "My Listings";
        List<Listing> listings = recycleBinMode ? Auth.ActiveUser.RecycleBin : Auth.ActiveUser.Listings;

        // Disable boosting where neccecary for non-admins.
        Listing.StepDisableBoost(ref listings);

        Common.PagingManager = new PagingManager<Listing>(
            listings, LocalSettings.DefaultPageLimit, Common.PageNumber);

        if (listings.Count > 0)
        {
            Common.PagingManager.DataBind(ListingDataList);
            this.MultiView.SetActiveView(DefaultView);
        }
        else if (recycleBinMode)
        {
            MultiView.SetActiveView(RecycleBinEmptyView);
        }
        else
        {
            MultiView.SetActiveView(UserEmptyView);
        }
    }

    protected void LoadTopListings()
    {
        string path = Server.MapPath(LocalSettings.PageDescriptionsPath);
        PageDescriptions descriptions = new PageDescriptions(path);

        // Allow sorting for top listings.
        SortingPanel.Visible = true;

        string sortColumn;
        string sortDirection;
        switch (SortDropDownList.SelectedValue)
        {
            case "Boosted":
                sortColumn = "BoostDate";
                sortDirection = "DESC";
                break;

            case "Listed":
                sortColumn = "CreateDate";
                sortDirection = "DESC";
                break;

            case "PriceAsc":
                sortColumn = "PriceValue";
                sortDirection = "ASC";
                break;

            case "PriceDesc":
                sortColumn = "PriceValue";
                sortDirection = "DESC";
                break;

            default:
                throw new NotSupportedException("Value of SortDropDownList.SelectedValue not supported.");
        }

        // Fetch a pre-limited list of listings.
        ICollection<IListing> listingList = Listing.Fetch(
            LocalSettings.DefaultPageLimit, Common.PageNumber, sortColumn, sortDirection);

        int totalListings = Listing.RunCount(SqlDateTime.MinValue.Value);

        Common.PagingManager = new PagingManager<IListing>(
            listingList, LocalSettings.DefaultPageLimit,
            Common.PageNumber, totalListings);

        DescriptionLabel.Text = "Showing <b>" +
            Common.PagingManager.StartNumber + "</b> to <b>" +
            Common.PagingManager.StopNumber + "</b> " +
            " of <b>" + Common.PagingManager.ItemCount + "</b> total listings.";

        Common.PagingManager.DataBind(ListingDataList);
        MultiView.SetActiveView(DefaultView);

        if (!Common.PagingManager.HasItems)
        {
            MultiView.SetActiveView(EmptyView);
        }
    }

    protected void ListingDataList_ItemDataBound(
        object sender, DataListItemEventArgs e)
    {
        ListingRow row = e.Item.FindControl("ListingRow") as ListingRow;
        if ((row != null) && (Common.AdminMode || (Common.PageMode == PageMode.SelfOnly)))
        {
            row.DisplayEditPanel();
        }
    }

    public void ListingRow_ListingUpdated(object sender, EventArgs e)
    {
        DataBindListings();
    }

    public void ListingRow_BoostRequest(Listing listing)
    {
        DataBindListings();
    }

    public void ListingRow_RestoreListing(Listing listing)
    {
        DataBindListings();
    }

    protected void EmptyView_Activate(object sender, EventArgs e)
    {
        FirstPostHyperLink.NavigateUrl =
            "~/ListingModify.aspx?Create=1&Category=" +
            Accessors.CategoryId;
    }

    protected void PageNotFoundView_Activate(object sender, EventArgs e)
    {
        // Remove "Page={number}" from all logical combinations of query string.
        Regex pageRegex = new Regex(@"(Page=\d+&?)|(&?Page=\d+)", RegexOptions.IgnoreCase);
        string firstPageUrl = pageRegex.Replace(Request.RawUrl, String.Empty);
        FistPageHyperLink.NavigateUrl = firstPageUrl;
    }

    protected void SortButton_Click(object sender, EventArgs e)
    {
        if ((Accessors.CategoryId != 0) || Common.CategoryPreloaded)
        {
            Response.Redirect(
                "~/ListingBrowser.aspx?CategoryId=" + 
                Accessors.CategoryId + "&Sort=" + SortDropDownList.SelectedValue);
        }
        else
        {
            Response.Redirect("~/ListingBrowser.aspx?Sort=" + SortDropDownList.SelectedValue);
        }
    }
}
