using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using ManxAds;


public partial class Register : StandardPage
{
    public Register() : base(ManxAds.WebsiteUserType.Public) { }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        RobotFlags = RobotFlag.NoIndex | RobotFlag.Follow;
    }

    protected override void InitializePage()
    {
        DefaultButton defaultButton = new DefaultButton(RegisterButton);
        defaultButton.AssociateWith(EmailAddressTextBox);
        defaultButton.AssociateWith(EmailRetypeAddressTextBox);
        defaultButton.AssociateWith(PasswordTextBox);
        defaultButton.AssociateWith(PasswordRetypeTextBox);
        defaultButton.AssociateWith(TitleDropDownList);
        defaultButton.AssociateWith(FirstNameTextBox);
        defaultButton.AssociateWith(LastNameTextBox);
        defaultButton.AssociateWith(MobileNumberTextBox);
        defaultButton.AssociateWith(LandlineNumberTextBox);
        defaultButton.AssociateWith(OptOutCheckBox);

        if (!IsPostBack)
        {
            InitializeLocationDropDownList();
            InitializeEmailAddressTextBox();
        }

        base.InitializePage();
    }

    protected void InitializeEmailAddressTextBox()
    {
        if (Session["RegisterEmail"] != null)
        {
            EmailAddressTextBox.Text = Session["RegisterEmail"].ToString();
        }
    }

    protected void InitializeLocationDropDownList()
    {
        int registerLocationId = -1;
        if (Session["RegisterLocation"] != null)
        {
            registerLocationId = (int)Session["RegisterLocation"];
        }

        const string unitedKingdomString = "United Kingdom";
        const string dividerString = "-------------------";
        ListItem divider = new ListItem(dividerString, "-1");

        List<ListItem> items = new List<ListItem>();
        items.Add(new ListItem("Choose...", "-1"));

        try
        {
            Location unitedKingdomLocation = Location.FetchByTitleString(unitedKingdomString);
            int unitedKingdomId = unitedKingdomLocation.DatabaseId;
            ListItem unitedKingdom = new ListItem(unitedKingdomString, unitedKingdomId.ToString());

            items.Add(divider);
            items.Add(unitedKingdom);
            items.Add(divider);
            items.Add(new ListItem("Isle of Man...", "-1"));
        }
        catch
        {
            // Purposely ignore errors.
        }

        Location.DataBind(LocationDropDownList, registerLocationId, items.ToArray());
    }

    protected void RegisterButton_Click(object sender, ImageClickEventArgs e)
    {
        ValidationErrorLabel.Visible = false;
        EmailInUseLabel.Visible = false;

        if (ManxAds.WebsiteUser.EmailInUse(EmailAddressTextBox.Text))
        {
            EmailInUseLabel.Visible = true;
            return;
        }

        Validate();

        if (!IsValid)
        {
            ValidationErrorLabel.Visible = true;
            return;
        }

        SocialTitleType titleType = SocialTitleType.Mr;
        switch (TitleDropDownList.SelectedValue)
        {
            case "Mrs":
                titleType = SocialTitleType.Mrs;
                break;

            case "Ms":
                titleType = SocialTitleType.Ms;
                break;

            case "Miss":
                titleType = SocialTitleType.Miss;
                break;

            case "Dr":
                titleType = SocialTitleType.Dr;
                break;
        }

        WebsiteUser newUser = new WebsiteUser(
            titleType,
            FirstNameTextBox.Text,
            LastNameTextBox.Text,
            EmailAddressTextBox.Text,
            PasswordTextBox.Text,
            LandlineAreaTextBox.Text,
            LandlineNumberTextBox.Text,
            MobileAreaTextBox.Text,
            MobileNumberTextBox.Text,
            int.Parse(LocationDropDownList.SelectedValue),
            WebsiteUserType.SellerInherit,
            OptOutCheckBox.Checked,
            DateTime.Now,
            DateTime.Now,
            DateTime.Now,
            RegisterType.SelfRegistered,
            null, null, TraderType.None);

        newUser.Create(Request.ServerVariables["REMOTE_ADDR"]);
        newUser.SendVerificationEmail(Page);

        FormsAuthentication.RedirectFromLoginPage(
            newUser.DatabaseId.ToString(), false);
    }

    protected void ValidateLandlineAndMobile(
        object source, ServerValidateEventArgs args)
    {
        CustomValidator validator = source as CustomValidator;
        if (String.IsNullOrEmpty(MobileNumberTextBox.Text)
            && String.IsNullOrEmpty(LandlineNumberTextBox.Text))
        {
            args.IsValid = false;
        }
    }

    protected void ValidateTermsCheckBox(object sender, ServerValidateEventArgs e)
    {
        e.IsValid = TermsCheckBox.Checked;
    }
}
