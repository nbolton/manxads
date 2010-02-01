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

public partial class ListingModifySteps : System.Web.UI.UserControl
{
    new protected StandardPage Page
    {
        get { return base.Page as StandardPage; }
    }

    protected MultiView MultiView
    {
        get { return Parent.FindControl("MultiView") as MultiView; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.PageMode != PageMode.Create)
        {
            MainMultiView.SetActiveView(LinkButtonsView);
        }
    }

    protected void DetailsLinkButton_Click(object sender, EventArgs e)
    {
        View details = MultiView.FindControl("DetailsView") as View;
        MultiView.SetActiveView(details);
    }

    protected void CategoriesLinkButton_Click(object sender, EventArgs e)
    {
        View categories = MultiView.FindControl("CategoriesView") as View;
        MultiView.SetActiveView(categories);
    }

    protected void ImagesLinkButton_Click(object sender, EventArgs e)
    {
        View images = MultiView.FindControl("ImagesView") as View;
        MultiView.SetActiveView(images);
    }
}
