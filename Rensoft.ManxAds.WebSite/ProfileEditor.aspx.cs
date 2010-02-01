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


public partial class ProfileEditor : StandardPage
{
    /// <summary>
    /// Gets the result of base.IsValid, and shows the
    /// error label if a validation error has occured.
    /// </summary>
    new public bool IsValid
    {
        get
        {
            ValidationErrorLabel.Visible = false;
            if (!base.IsValid)
            {
                ValidationErrorLabel.Visible = true;
            }
            return base.IsValid;
        }
    }

    public ProfileEditor() : base(ManxAds.WebsiteUserType.SellerOnly) { }

    protected override void InitializePage()
    {
        DefaultButton detailsDefault = new DefaultButton(
            PersonalDetailsContinueButton);
        detailsDefault.AssociateWith(FirstNameTextBox);
        detailsDefault.AssociateWith(LastNameTextBox);
        detailsDefault.AssociateWith(MobileNumberTextBox);
        detailsDefault.AssociateWith(MobileAreaTextBox);
        detailsDefault.AssociateWith(LandlineNumberTextBox);
        detailsDefault.AssociateWith(LandlineAreaTextBox);
        detailsDefault.AssociateWith(TraderNameTextBox);
        detailsDefault.AssociateWith(TraderWebsiteTextBox);

        DefaultButton emailDefault = new DefaultButton(
            ChangeEmailAddressContinueButton);
        emailDefault.AssociateWith(EmailAddressTextBox);
        emailDefault.AssociateWith(EmailRetypeAddressTextBox);

        DefaultButton passwordDefault = new DefaultButton(
            ChangePasswordContinueButton);
        passwordDefault.AssociateWith(PasswordTextBox);
        passwordDefault.AssociateWith(PasswordRetypeTextBox);

        if (!IsPostBack)
        {
            MultiView.SetActiveView(DefaultView);
        }

        base.InitializePage();
    }

    protected void PersonalDetailsLinkButton_Click(
        object sender, EventArgs e)
    {
        MultiView.SetActiveView(PersonalDetailsView);
    }

    protected void ChanageEmailLinkButton_Click(
        object sender, EventArgs e)
    {
        MultiView.SetActiveView(ChangeEmailAddressView);
    }

    protected void ChangePasswordLinkButton_Click(
        object sender, EventArgs e)
    {
        MultiView.SetActiveView(ChangePasswordView);
    }

    protected void SettingsLinkButton_Click(object sender, EventArgs e)
    {
        MultiView.SetActiveView(SettingsView);
    }

    protected void MultiView_ActiveViewChanged(object sender, EventArgs e)
    {
        ValidationErrorLabel.Visible = false;
    }

    protected void PersonalDetailsView_Activate(
        object sender, EventArgs e)
    {
        if (Auth.ActiveUser.IsTrader)
        {
            TraderPanel.Visible = true;
            TraderNameTextBox.Text = Auth.ActiveUser.TradingName;
            TraderWebsiteTextBox.Text = Auth.ActiveUser.TradingWebsite;
        }

        FirstNameTextBox.Text = Auth.ActiveUser.Forename;
        LastNameTextBox.Text = Auth.ActiveUser.Surname;
        MobileAreaTextBox.Text = Auth.ActiveUser.MobileArea;
        MobileNumberTextBox.Text = Auth.ActiveUser.MobilePhone;
        LandlineAreaTextBox.Text = Auth.ActiveUser.LandlineArea;
        LandlineNumberTextBox.Text = Auth.ActiveUser.LandlinePhone;
        PopulateTitleDropDown(Auth.ActiveUser.SocialTitleType);
        PopulateLocationDropDown(Auth.ActiveUser.LocationId);
    }

    private void PopulateLocationDropDown(int locationId)
    {
        Location.DataBind(LocationDropDownList, locationId, "Choose...");
    }

    protected void ChangeEmailAddressView_Activate(object sender, EventArgs e)
    {
        EmailAddressTextBox.Text = null;
        EmailRetypeAddressTextBox.Text = null;
    }

    protected void ChangePasswordView_Activate(object sender, EventArgs e)
    {
        PasswordTextBox.Text = null;
        PasswordRetypeTextBox.Text = null;
    }

    protected void SettingsView_Activate(object sender, EventArgs e)
    {
        ListingsPopupCheckBox.Checked = LocalSettings.Fetch<bool>(
            "ListingDetailsPopup",
            LocalSettings.Default.ListingDetailsPopup,
            Auth.ActiveUser);
    }

    private void PopulateTitleDropDown(SocialTitleType selected)
    {
        SocialTitleType[] types = new SocialTitleType[] {
            SocialTitleType.Mr,
            SocialTitleType.Mrs,
            SocialTitleType.Ms,
            SocialTitleType.Miss,
            SocialTitleType.Dr
        };

        ListItem tempItem;
        TitleDropDownList.Items.Clear();

        foreach (SocialTitleType type in types)
        {
            tempItem = new ListItem(type.ToString(), ((int)type).ToString());
            if (selected == type)
            {
                tempItem.Selected = true;
            }
            TitleDropDownList.Items.Add(tempItem);
        }
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

    protected void PersonalDetailsContinueButton_Click(
        object sender, ImageClickEventArgs e)
    {
        Validate();
        if (!IsValid) return;

        Auth.ActiveUser.SocialTitleType = (SocialTitleType)int.Parse(
            TitleDropDownList.SelectedValue);

        Auth.ActiveUser.Forename = FirstNameTextBox.Text;
        Auth.ActiveUser.Surname = LastNameTextBox.Text;
        Auth.ActiveUser.LocationId = int.Parse(LocationDropDownList.SelectedValue);
        Auth.ActiveUser.TradingName = TraderNameTextBox.Text;
        Auth.ActiveUser.TradingWebsite = TraderWebsiteTextBox.Text;

        Auth.ActiveUser.SetLandlineNumber(LandlineAreaTextBox.Text, LandlineNumberTextBox.Text);
        Auth.ActiveUser.SetMobileNumber(MobileAreaTextBox.Text, MobileNumberTextBox.Text);

        Auth.ActiveUser.Modify();

        MultiView.SetActiveView(FinishedView);
    }

    protected void ChangeEmailAddressContinueButton_Click(
        object sender, ImageClickEventArgs e)
    {
        Validate();
        if (!IsValid) return;

        Auth.ActiveUser.EmailAddress = EmailAddressTextBox.Text;
        Auth.ActiveUser.Invalidate();
        Auth.ActiveUser.Modify();

        Auth.ActiveUser.SendVerificationEmail(Page);

        MultiView.SetActiveView(FinishedView);
    }

    protected void ChangePasswordContinueButton_Click(
        object sender, ImageClickEventArgs e)
    {
        Validate();
        if (!IsValid) return;

        Auth.ActiveUser.Password = PasswordTextBox.Text;
        Auth.ActiveUser.Modify();

        MultiView.SetActiveView(FinishedView);
    }

    protected void SettingsContinueButton_Click(object sender, ImageClickEventArgs e)
    {
        LocalSettings.ModifySetting<bool>(
            "ListingDetailsPopup",
            ListingsPopupCheckBox.Checked,
            LocalSettings.Default.ListingDetailsPopup,
            Auth.ActiveUser);

        MultiView.SetActiveView(FinishedView);
    }

    protected void EmailAddressCustomValidator_ServerValidate(
        object source, ServerValidateEventArgs args)
    {
        if (ManxAds.WebsiteUser.EmailInUse(args.Value))
        {
            args.IsValid = false;
        }
    }
}
