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


/// <summary>
/// Provides a redirect to the Listings.ascx page, with search query.
/// </summary>
public partial class SearchDialog : StandardControl
{
    protected bool Advanced
    {
        get
        {
            string advanced = Request.QueryString["Advanced"];
            if (!String.IsNullOrEmpty(advanced))
            {
                return true;
            }
            return false;
        }
    }

    protected int CategoryId
    {
        get
        {
            if (Common.CategoryPreloaded)
            {
                // Preloaded categories have no ID, but category is accessable.
                return Accessors.Category.DatabaseId;
            }

            int result;
            int.TryParse(Request.QueryString["SearchCategoryId"], out result);
            if (result == 0)
            {
                int result2;
                int.TryParse(Request.QueryString["Category"], out result2);
                return result2;
            }

            return result == 0 ? -1 : result;
        }
    }

    protected int LocationId
    {
        get
        {
            int result;
            int.TryParse(Request.QueryString["LocationId"], out result);
            return result == 0 ? -1 : result;
        }
    }

    protected int SellerId
    {
        get
        {
            int result;
            int.TryParse(Request.QueryString["Seller"], out result);
            return result == 0 ? -1 : result;
        }
    }

    protected string SellerNameLabelText
    {
        get
        {
            try
            {
                WebsiteUser user = WebsiteUser.Fetch(SellerId);
                return "(" + user.FullName + ")";
            }
            catch
            {
                return String.Empty;
            }
        }
    }

    protected string SearchPhrase
    {
        get { return Request.QueryString["Search"]; }
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        DefaultButton simpleDefault = new DefaultButton(SimpleSearchButton);
        simpleDefault.AssociateWith(SimpleCategoryDropDownList);
        simpleDefault.AssociateWith(SimpleSearchTextBox);

        DefaultButton advancedDetault = new DefaultButton(AdvancedSearchButton);
        advancedDetault.AssociateWith(AdvancedCategoryDropDownList);
        advancedDetault.AssociateWith(AdvancedLocationDropDownList);
        advancedDetault.AssociateWith(AdvancedSearchTextBox);
        advancedDetault.AssociateWith(StartDateTextBox);
        advancedDetault.AssociateWith(EndDateTextBox);
        advancedDetault.AssociateWith(StartPriceTextBox);
        advancedDetault.AssociateWith(EndPriceTextBox);

        DateExampleLabel.Text = DateTime.Now.Date.ToShortDateString();

        if (!IsPostBack)
        {
            if (Advanced)
            {
                showAdvancedView();
            }
            else
            {
                showSimpleView();
            }
        }
    }

    private void showAdvancedView()
    {
        TypeMultiView.SetActiveView(AdvancedView);
    }

    protected void PopulateCategoryDropDown(int categoryId, DropDownList dropDownList)
    {
        ListItem tempListItem;
        dropDownList.Items.Clear();
        dropDownList.Items.Insert(0, new ListItem("All Categories", "-1"));

        foreach (Category category in Category.FetchByTitleAscending())
        {
            tempListItem = new ListItem(
                category.Title, category.DatabaseId.ToString());
            dropDownList.Items.Add(tempListItem);

            if (CategoryId == category.DatabaseId)
            {
                tempListItem.Selected = true;
            }
        }
    }

    protected void SimpleSearchButton_Click(object sender, ImageClickEventArgs e)
    {
        string url = "~/Listings.aspx?Search=" + Server.UrlEncode(SimpleSearchTextBox.Text);

        int categoryId = int.Parse(SimpleCategoryDropDownList.SelectedValue);
        if (categoryId > 0)
        {
            url += "&SearchCategoryId=" + categoryId.ToString();
        }

        if (SellerId > 0) url += "&Seller=" + SellerId.ToString();
        if (!String.IsNullOrEmpty(Request.QueryString["Any"])) url += "&Any=1";

        Response.Redirect(url, false);
    }

    protected void AdvancedSearchButton_Click(object sender, ImageClickEventArgs e)
    {
        string url = "~/Listings.aspx?Advanced=1";

        if (!string.IsNullOrEmpty(AdvancedSearchTextBox.Text))
            url += "&Search=" + Server.UrlEncode(AdvancedSearchTextBox.Text);
        else
            url += "&Search=*";

        if (!String.IsNullOrEmpty(Request.QueryString["Any"])) 
            url += "&Any=1";

        int categoryId = int.Parse(AdvancedCategoryDropDownList.SelectedValue);
        int locationId = int.Parse(AdvancedLocationDropDownList.SelectedValue);

        if (categoryId > 0) url += "&SearchCategoryId=" + categoryId.ToString();
        if (locationId > 0) url += "&LocationId=" + locationId.ToString();
        if (SellerId > 0) url += "&Seller=" + SellerId.ToString();

        if (!String.IsNullOrEmpty(StartDateTextBox.Text))
        {
            url += "&StartDate=" + StartDateTextBox.Text;
        }

        if (!String.IsNullOrEmpty(EndDateTextBox.Text))
        {
            url += "&EndDate=" + EndDateTextBox.Text;
        }

        if (!String.IsNullOrEmpty(StartPriceTextBox.Text))
        {
            url += "&StartPrice=" + StartPriceTextBox.Text;
        }

        if (!String.IsNullOrEmpty(EndPriceTextBox.Text))
        {
            url += "&EndPrice=" + EndPriceTextBox.Text;
        }

        Response.Redirect(url, false);
    }

    protected void AdvancedSearchLinkButton_Click(object sender, EventArgs e)
    {
        showAdvancedView();
    }

    protected void SimpleSearchLinkButton_Click(object sender, EventArgs e)
    {
        showSimpleView();
    }

    private void showSimpleView()
    {
        TypeMultiView.SetActiveView(SimpleView);
    }

    protected void SimpleView_Activate(object sender, EventArgs e)
    {
        PopulateCategoryDropDown(CategoryId, SimpleCategoryDropDownList);
        SimpleSearchTextBox.Text = SearchPhrase;
        SellerNameSimpleLabel.Text = SellerNameLabelText;
    }

    protected void AdvancedView_Activate(object sender, EventArgs e)
    {
        AdvancedSearchTextBox.Text = SearchPhrase;
        SellerNameAdvancedLabel.Text = SellerNameLabelText;
        StartDateTextBox.Text = Request.QueryString["StartDate"];
        EndDateTextBox.Text = Request.QueryString["EndDate"];
        StartPriceTextBox.Text = Request.QueryString["StartPrice"];
        EndPriceTextBox.Text = Request.QueryString["EndPrice"];
        PopulateCategoryDropDown(CategoryId, AdvancedCategoryDropDownList);
        Location.DataBind(AdvancedLocationDropDownList, LocationId, "All Locations");
    }
}
