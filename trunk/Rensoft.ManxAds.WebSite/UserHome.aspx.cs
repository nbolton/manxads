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
using ManxAds;

public partial class UserHome : StandardPage
{
    public UserHome() : base(WebsiteUserType.SellerOnly, false, true) { }

    protected override void InitializePage()
    {
        if (Auth.ActiveUser.BanUntil == null)
        {
            MultiView.SetActiveView(DefaultView);

            if ((Auth.ActiveUser.UserType & WebsiteUserType.AdministratorOnly) != 0)
            {
                AdministratorOptions.Visible = true;
            }

            if ((Auth.ActiveUser.UserType & WebsiteUserType.AdvertiserOnly) != 0)
            {
                AdvertiserOptions.Visible = true;
            }

            if (Auth.ActiveUser.IsTrader)
            {
                TraderOptions.Visible = true;
            }

            FullNameLabel.Text = Auth.ActiveUser.FullName;
            UserEmailLabel.Text = Auth.ActiveUser.EmailAddress;
            VerifyTimeoutDaysLabel.Text = Auth.ActiveUser.VerifyTimeToLiveWithinDays;

            if (Auth.ActiveUser.IsDisabled)
            {
                DisabledPanel.Visible = true;
            }
            else if (!Auth.ActiveUser.IsVerified)
            {
                long ticks = DateTime.Now.Date.Ticks - Auth.ActiveUser.CreateDate.Date.Ticks;
                TimeSpan registered = new TimeSpan(ticks);
                if (registered.TotalDays >= 1)
                {
                    // Only show if older then 1 day.
                    VerifyPanel.Visible = true;
                }
            }

            if (Auth.ActiveUser.IsTrader)
            {
                TraderLinksPanel.Visible = false;
            }
        }
        else
        {
            MultiView.SetActiveView(BannedView);
        }

        base.InitializePage();
    }

    protected void LogOffLinkButton_Click(object sender, EventArgs e)
    {
        Auth.LogOff(true);
    }

    protected void ResendValidationEmailLinkButton_Click(object sender, EventArgs e)
    {
        string url = this.ResolveUrl("~/UserHome.aspx?VerifyAuthCode={0}");
        Auth.ActiveUser.SendVerificationEmail(
            "http://" + Request.Url.Host + ":" + Request.Url.Port + url, Page);
        VerifyResentPanel.Visible = true;
    }

    protected void CharityRequestLinkButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/UserUpgrade.aspx?TraderType=" + (int)TraderType.Charity);
    }

    protected void TraderRequestLinkButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/UserUpgrade.aspx?TraderType=" + (int)TraderType.Corporate);
    }
}
