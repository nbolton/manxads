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


public partial class RecoverPassword : StandardPage
{
    protected int TempUserId
    {
        set { UserIdLiteral.Text = value.ToString(); }
        get { return int.Parse(UserIdLiteral.Text); }
    }

    protected string AuthCode
    {
        get { return Request.QueryString["AuthCode"]; }
    }

    public RecoverPassword() : base(ManxAds.WebsiteUserType.Public) { }

    protected override void InitializePage()
    {
        DefaultButton step1Default = new DefaultButton(Step1ContinueButton);
        step1Default.AssociateWith(EmailAddressTextBox);
        step1Default.AssociateWith(EmailRetypeAddressTextBox);

        DefaultButton step2Default = new DefaultButton(Step2ContinueButton);
        step2Default.AssociateWith(PasswordTextBox);
        step2Default.AssociateWith(PasswordRetypeTextBox);

        if (!IsPostBack)
        {
            if (AuthCode == null)
            {
                MultiView.SetActiveView(Step1View);
            }
            else
            {
                MultiView.SetActiveView(Step2View);
            }
        }

        base.InitializePage();
    }

    protected void Step1ContinueButton_Click(object sender, ImageClickEventArgs e)
    {
        if (!Security.RequestPasswordReset(EmailAddressTextBox.Text, Page))
        {
            EmailNotFoundLabel.Visible = true;
            return;
        }

        MultiView.SetActiveView(Step1ConfirmationView);
    }

    protected void Step2View_Activate(object sender, EventArgs e)
    {
        TempUserId = Security.CompletePasswordReset(AuthCode);
        if (TempUserId == 0)
        {
            MultiView.SetActiveView(Step2BadAuthView);
        }
    }

    protected void Step2ContinueButton_Click(object sender, ImageClickEventArgs e)
    {
        WebsiteUser user = WebsiteUser.Fetch(TempUserId);
        user.Password = PasswordTextBox.Text;
        user.Modify();
        MultiView.SetActiveView(Step2ConfirmationView);
    }

    protected void LogOnLinkButton_Click(object sender, EventArgs e)
    {
        FormsAuthentication.RedirectFromLoginPage(
            TempUserId.ToString(), false);
    }
}
