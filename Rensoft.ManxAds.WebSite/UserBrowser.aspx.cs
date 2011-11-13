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

public partial class UserBrowser : StandardPage
{
    public UserBrowser() : base(WebsiteUserType.AdministratorOnly) { }

    protected override void InitializePage()
    {
        Master.EnableAdverts = false;

        DefaultButton db = new DefaultButton(SearchButton);
        db.AssociateWith(SearchTextBox);

        if (!IsPostBack)
        {
            UserMultiView.SetActiveView(OptionsView);
        }

        base.InitializePage();
    }

    protected void SearchButton_Click(object sender, EventArgs e)
    {
        UserMultiView.SetActiveView(SearchView);
        SearchGridView.DataSource = ManxAds.WebsiteUser.Search(SearchTextBox.Text);
        SearchGridView.DataBind();
    }

    protected void ViewBannedLinkButton_Click(object sender, EventArgs e)
    {
        UserMultiView.SetActiveView(BannedView);
        BannedGridView.DataSource = ManxAds.WebsiteUser.FetchBanned();
        BannedGridView.DataBind();
    }
}
