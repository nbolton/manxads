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

public partial class Categories : StandardPage
{
    public Categories() : base(WebsiteUserType.Public) { }

    protected override void  InitializePage()
    {
        if (!IsPostBack)
        {
            this.CategoryDataList_DataBind();
        }

        base.InitializePage();
    }

    protected void CategoryDataList_DataBind()
    {
        CategoryDataList.DataSource = Category.Fetch();
        CategoryDataList.DataBind();
    }

    protected void CategoryDataList_ItemDataBound(
        object sender, DataListItemEventArgs e)
    {
        CategoryRow row = e.Item.FindControl("CategoryRow") as CategoryRow;
        if ((row != null) && (AdminMode))
        {
            row.DisplayEditPanel();
        }
    }

    protected void CategoryRow_MultiplePrioritiesUpdated(object sender, EventArgs e)
    {
        this.CategoryDataList_DataBind();
    }

    protected void CategoryRow_SinglePriorityUpdated(object sender, EventArgs e)
    {
        AutoFocusClientId = ((TextBox)sender).ClientID;
    }
}
