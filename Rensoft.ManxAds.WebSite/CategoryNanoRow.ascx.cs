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


public partial class CategoryNanoRow : BrowserRow
{
    public string Title;
    public string ListingCountWithZeros;

    public CategoryNanoRow() : base(false) { }

    protected override void OnDataBinding(EventArgs e)
    {
        base.OnDataBinding(e);

        Category category = (Category)((DataListItem)Parent).DataItem;
        TitleHyperLink.Text = category.Title;
        TitleHyperLink.NavigateUrl = category.NavigateUrl;
        ListingCountWithZerosLabel.Text = category.ListingCountWithZeros;
    }
}