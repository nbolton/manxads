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

public partial class PromoEmails : StandardPage
{
    private string emailSeparator = "; ";

    public PromoEmails() : base(ManxAds.WebsiteUserType.AdministratorOnly) { }

    protected override void InitializePage()
    {
        string[] emails = WebsiteUser.FetchPromoEmails().ToArray();
        EmailsLabel.Text = String.Join(emailSeparator, emails);

        base.InitializePage();
    }
}
