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
        DefaultButton db = new DefaultButton(SearchButton);
        db.AssociateWith(SearchTextBox);

        if (!IsPostBack)
        {
            /*UsersGridView.DataSource = ManxAds.WebsiteUser.Fetch();
            UsersGridView.DataBind();*/
        }

        base.InitializePage();
    }

    protected void EditorLinkButton_Click(object sender, EventArgs e)
    {
        SetAsPreviousPage();
        LinkButton link = sender as LinkButton;
        Response.Redirect(link.CommandArgument);
    }

    protected void SearchButton_Click(object sender, EventArgs e)
    {
        UsersGridView.DataSource = ManxAds.WebsiteUser.Search(SearchTextBox.Text);
        UsersGridView.DataBind();
    }
}
