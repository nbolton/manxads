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


public partial class UserVerify : StandardPage
{
    public UserVerify() : base(ManxAds.WebsiteUserType.Public) { }

    protected int TempUserId
    {
        set { UserIdLiteral.Text = value.ToString(); }
        get { return int.Parse(UserIdLiteral.Text); }
    }

    protected override void InitializePage()
    {
        try
        {
            Auth.RefreshActiveUser();
            WebsiteUser user = WebsiteUser.Verify(Request.QueryString["AuthCode"]);
            TempUserId = user.DatabaseId;
            ResultMultiView.SetActiveView(SuccessView);
        }
        catch (NotFoundException)
        {
            ResultMultiView.SetActiveView(FailureView);
        }

        base.InitializePage();
    }

    protected void LogOnLinkButton_Click(object sender, EventArgs e)
    {
        FormsAuthentication.RedirectFromLoginPage(
            TempUserId.ToString(), false);
    }
}
