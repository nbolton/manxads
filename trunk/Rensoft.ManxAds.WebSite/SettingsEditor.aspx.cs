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


public partial class SettingsEditor : StandardPage
{
    public SettingsEditor() : base(ManxAds.WebsiteUserType.AdministratorOnly) { }

    protected override void InitializePage()
    {
        DefaultButton defaultButton = new DefaultButton(ContinueButton);
        defaultButton.AssociateWith(WelcomeTopListingsLimitTextBox);
        defaultButton.AssociateWith(WelcomeTopCategoriesLimitTextBox);
        defaultButton.AssociateWith(AdvertsDebugCheckBox);
        defaultButton.AssociateWith(UserListingLimitTextBox);
        defaultButton.AssociateWith(TraderListingLimitTextBox);
        defaultButton.AssociateWith(BoostCountLimitTextBox);
        defaultButton.AssociateWith(BoostSleepTimeTextBox);
        defaultButton.AssociateWith(MaximumCategoryCountTextBox);

        if (!IsPostBack)
        {
            WelcomeTopListingsLimitTextBox.Text =
                LocalSettings.Fetch<int>("WelcomeTopListingsLimit",
                LocalSettings.Default.WelcomeTopListingsLimit).ToString();

            WelcomeTopCategoriesLimitTextBox.Text =
                LocalSettings.Fetch<int>("WelcomeTopCategoriesLimit",
                LocalSettings.Default.WelcomeTopCategoriesLimit).ToString();

            AdvertsDebugCheckBox.Checked =
                LocalSettings.Fetch<bool>("AdvertDebug", 
                LocalSettings.Default.AdvertDebug);

            UserListingLimitTextBox.Text =
                LocalSettings.Fetch<int>("UserListingLimit",
                LocalSettings.Default.UserListingLimit).ToString();

            TraderListingLimitTextBox.Text =
                LocalSettings.Fetch<int>("TraderListingLimit", 
                LocalSettings.Default.TraderListingLimit).ToString();

            BadWordsTextBox.Text =
                LocalSettings.Fetch<string>("BadWordsList", 
                LocalSettings.Default.BadWordsList).ToString();

            BoostSleepTimeTextBox.Text =
                LocalSettings.Fetch<int>("BoostSleepTime",
                LocalSettings.Default.BoostSleepTime).ToString();

            BoostCountLimitTextBox.Text =
                LocalSettings.Fetch<int>("BoostCountLimit",
                LocalSettings.Default.BoostCountLimit).ToString();

            MaximumCategoryCountTextBox.Text =
                LocalSettings.Fetch<int>("MaximumCategoryCount",
                LocalSettings.Default.MaximumCategoryCount).ToString();

            MultiView.SetActiveView(SettingsView);
        }

        base.InitializePage();
    }

    protected void ContinueButton_Click(object sender, ImageClickEventArgs e)
    {
        Validate();
        if (!IsValid)
        {
            ValidationErrorLabel.Visible = true;
            return;
        }

        LocalSettings.ModifySetting<int>(
            "WelcomeTopListingsLimit",
            int.Parse(WelcomeTopListingsLimitTextBox.Text),
            LocalSettings.Default.WelcomeTopListingsLimit);

        LocalSettings.ModifySetting<int>(
            "WelcomeTopCategoriesLimit",
            int.Parse(WelcomeTopCategoriesLimitTextBox.Text),
            LocalSettings.Default.WelcomeTopCategoriesLimit);

        LocalSettings.ModifySetting<bool>(
            "AdvertDebug",
            AdvertsDebugCheckBox.Checked,
            LocalSettings.Default.AdvertDebug);

        LocalSettings.ModifySetting<int>(
            "UserListingLimit",
            int.Parse(UserListingLimitTextBox.Text),
            LocalSettings.Default.UserListingLimit);

        LocalSettings.ModifySetting<int>(
            "TraderListingLimit",
            int.Parse(TraderListingLimitTextBox.Text),
            LocalSettings.Default.TraderListingLimit);

        LocalSettings.ModifySetting<string>(
            "BadWordsList",
            BadWordsTextBox.Text,
            LocalSettings.Default.BadWordsList);

        LocalSettings.ModifySetting<int>(
            "BoostSleepTime",
            BoostSleepTimeTextBox.Text,
            LocalSettings.Default.BoostSleepTime);

        LocalSettings.ModifySetting<int>(
            "BoostCountLimit",
            BoostCountLimitTextBox.Text,
            LocalSettings.Default.BoostCountLimit);

        LocalSettings.ModifySetting<int>(
            "MaximumCategoryCount",
            MaximumCategoryCountTextBox.Text,
            LocalSettings.Default.MaximumCategoryCount);

        MultiView.SetActiveView(SuccessView);
    }

    protected void OnBadWordsCheckForWhitespace(object sender, ServerValidateEventArgs e)
    {
        char[] split = new char[] { ',' };
        foreach (string badWord in e.Value.Split(split))
        {
            if (badWord.Trim().Contains(" "))
            {
                e.IsValid = false;
                return;
            }
        }
    }
}
