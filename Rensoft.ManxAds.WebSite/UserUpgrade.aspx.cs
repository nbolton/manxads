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

using System.Net.Mail;
using System.Net;
using ManxAds;

public partial class UserUpgrade : StandardPage
{
    protected TraderType TraderType
    {
        get { return (TraderType)int.Parse(Request.QueryString["TraderType"]); }
    }

    protected int AcceptUserId
    {
        get
        {
            int i;
            int.TryParse(Request.QueryString["AcceptUserId"], out i);
            return i;
        }
    }

    public UserUpgrade() : base(WebsiteUserType.SellerOnly, false, true) { }

    protected override void InitializePage()
    {
        base.InitializePage();

        if (AcceptUserId != 0)
        {
            WebsiteUser user = WebsiteUser.Fetch(AcceptUserId);

            user.TraderType = TraderType;
            user.Modify();

            EditUserHyperLink.NavigateUrl = "~/UserModify.aspx?UserId=" + AcceptUserId;
            MultiView1.SetActiveView(AcceptedView);
        }
        else if (!IsPostBack)
        {
            MultiView1.SetActiveView(FormView);
        }
    }

    protected void FinishButton_Click(object sender, ImageClickEventArgs e)
    {
        Auth.ActiveUser.TradingName = TraderNameTextBox.Text;
        Auth.ActiveUser.TradingWebsite = WebsiteTextBox.Text;
        Auth.ActiveUser.Modify();

        SendUpgradeRequestEmail(
            Auth.ActiveUser, TraderType, AdvertisingCheckBox.Checked, DetailsTextBox.Text);

        MultiView1.SetActiveView(FinishedView);
    }

    private void SendUpgradeRequestEmail(
        WebsiteUser user, TraderType upgradeType, bool sellAdvertising, string about)
    {
        MailMessage email = new MailMessage();
        email.From = new MailAddress(LocalSettings.MasterSendFromEmail);

        switch (upgradeType)
        {
            case TraderType.Charity:
                email.To.Add(LocalSettings.CharityRequestEmail);
                email.Subject = "Charity Upgrade Request";
                email.Body = "This user would like to become a charity.\r\n";
                break;

            case TraderType.Corporate:
                email.To.Add(LocalSettings.TraderRequestEmail);
                email.Subject = "Trader Upgrade Request";
                email.Body = "This user would like to become a corporate trader.\r\n";
                break;
        }

        email.Body += "\r\nUser Name:\t\t" + user.FullName;
        email.Body += "\r\nEmail Address:\t\t" + user.EmailAddress;
        email.Body += "\r\nRegistered:\t\t" + user.CreateDate.ToShortDateString();

        email.Body += "\r\n\r\nOrganisation:\t\t" + user.TradingName;
        email.Body += !string.IsNullOrEmpty(user.TradingWebsite) ? ("\r\nWebsite:\t\t" + user.TradingWebsite) : null;
        email.Body += "\r\nSell Advertising:\t" + (sellAdvertising ? "Yes" : "No");
        email.Body += "\r\n\r\n" + about;

        const string urlFormat = "http://www.manxads.com/UserUpgrade.aspx?AcceptUserId={0}&TraderType={1}";
        string acceptUrl = string.Format(urlFormat, user.DatabaseId, (int)upgradeType);
        email.Body += "\r\n\r\nTo accept this request, follow this link:\r\n" + acceptUrl;

        EmailTools.SendMessage(email, Page);
    }
}
