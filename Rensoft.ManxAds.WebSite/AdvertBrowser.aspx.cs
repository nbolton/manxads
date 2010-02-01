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


public partial class AdvertBrowser : StandardPage
{
    public AdvertBrowser() : base(ManxAds.WebsiteUserType.AdvertiserOnly) { }

    protected override void InitializePage()
    {
        base.InitializePage();

        if (!IsPostBack)
        {
            if ((PageMode != PageMode.SelfOnly) && AdminMode)
            {
                // If admin and not in self-only mode, show all.
                AdvertsGridView.DataSource = Advert.Fetch();
            }
            else
            {
                AdvertsGridView.DataSource = Advert.Fetch(Auth.ActiveUser);
            }
            AdvertsGridView.DataBind();
        }
    }

    protected void EditorLinkButton_Click(object sender, EventArgs e)
    {
        SetAsPreviousPage();
        LinkButton link = sender as LinkButton;
        Response.Redirect(link.CommandArgument);
    }
}
