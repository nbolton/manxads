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

public partial class CategoryRow : BrowserRow
{
    public string Title;
    public string Description;
    public string ListingCount;
    public string LatestListing;
    public string Priority;
    public string DatabaseId;

    public event EventHandler SinglePriorityUpdated;
    public event EventHandler MultiplePrioritiesUpdated;

    protected override void Page_Load(object sender, EventArgs e)
    {
        SinglePriorityUpdated += new EventHandler(OnSinglePriorityUpdated);
        MultiplePrioritiesUpdated += new EventHandler(OnMultiplePrioritiesUpdated);
    }

    protected void OnMultiplePrioritiesUpdated(object sender, EventArgs e) { }
    protected void OnSinglePriorityUpdated(object sender, EventArgs e) { }

    protected override void OnDataBinding(EventArgs e)
    {
        base.OnDataBinding(e);

        Category category = (Category)((DataListItem)Parent).DataItem;
        this.TitleHyperLink.Text = category.Title;
        this.TitleHyperLink.NavigateUrl = category.NavigateUrl;
        this.DescriptionLabel.Text = category.Description;
        this.ListingCountLabel.Text = category.ListingCountString;
        this.LatestListingLabel.Text = category.LatestListing;
        this.DatabaseIdLiteral.Text = category.DatabaseId.ToString();
        this.PriorityTextBox.Text = category.Priority.ToString();
        this.PriorityTextBox.Attributes.Add("onclick", "select()");
    }

    protected void PriorityTextBox_TextChanged(object sender, EventArgs e)
    {
        int databaseId = int.Parse(this.DatabaseIdLiteral.Text);
        int priority = int.Parse(this.PriorityTextBox.Text);
        Category.ModifyPriority(databaseId, priority);
        SinglePriorityUpdated(sender, e);
    }

    protected void UpdateLinkButton_Click(object sender, EventArgs e)
    {
        MultiplePrioritiesUpdated(sender, e);
    }
}
