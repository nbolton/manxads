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


public partial class UserModify : StandardPage
{
    public UserModify() : base(ManxAds.WebsiteUserType.AdministratorOnly) { }

    protected override void InitializePage()
    {
        DefaultButton defaultButton = new DefaultButton(ContinueButton);
        defaultButton.AssociateWith(FirstNameTextBox);
        defaultButton.AssociateWith(LastNameTextBox);
        defaultButton.AssociateWith(MobileNumberTextBox);
        defaultButton.AssociateWith(LandlineNumberTextBox);
        defaultButton.AssociateWith(EmailAddressTextBox);
        defaultButton.AssociateWith(PasswordTextBox);
        defaultButton.AssociateWith(PasswordRetypeTextBox);
        defaultButton.AssociateWith(ListingLimitTextBox);
        defaultButton.AssociateWith(EnableAdvertsCheckBox);
        defaultButton.AssociateWith(OptOutCheckBox);
        defaultButton.AssociateWith(TraderNameTextBox);
        defaultButton.AssociateWith(TraderWebsiteTextBox);

        if (!IsPostBack)
        {
            switch (PageMode)
            {
                case PageMode.Create:
                    this.TitleLabel.Text = "New User";
                    this.PopulateTitleDropDown(null);
                    this.PopulateTypeDropDown(null);
                    this.PopulateLocationDropDown(-1);
                    this.PopulateTraderTypeDropDown(TraderType.None);
                    this.MultiView.SetActiveView(DefaultView);
                    break;

                case PageMode.Remove:
                    this.TitleLabel.Text = "Delete '" + Accessors.User.FullName + "'";
                    this.RemoveTitleLabel.Text = Accessors.User.FullName;
                    this.MultiView.SetActiveView(RemoveView);
                    break;

                case PageMode.Ban:
                    this.TitleLabel.Text = "Ban user";
                    this.BanTitleLabel.Text = Accessors.User.FullName;
                    this.MultiView.SetActiveView(BanStartView);
                    break;

                default:
                    this.TitleLabel.Text = "Edit '" + Accessors.User.FullName + "'";
                    this.PopulateFields(Accessors.User);
                    this.MultiView.SetActiveView(DefaultView);
                    this.SettingsPanel.Visible = true;
                    break;
            }
            
            this.Title = TitleLabel.Text;
        }

        base.InitializePage();
    }

    private void PopulateTypeDropDown(WebsiteUser user)
    {
        ListItem sellerItem = new ListItem("Seller",
            ((int)WebsiteUserType.SellerInherit).ToString());

        ListItem advertiserItem = new ListItem("Advertiser",
            ((int)WebsiteUserType.AdvertiserInherit).ToString());

        ListItem administratorItem = new ListItem("Administrator",
            ((int)WebsiteUserType.AdministratorInherit).ToString());

        UserTypeDropDownList.Items.Add(sellerItem);
        UserTypeDropDownList.Items.Add(advertiserItem);
        UserTypeDropDownList.Items.Add(administratorItem);

        if (user != null)
        {
            switch (user.UserType)
            {
                case WebsiteUserType.SellerInherit:
                    sellerItem.Selected = true;
                    break;

                case WebsiteUserType.AdvertiserInherit:
                    advertiserItem.Selected = true;
                    break;

                case WebsiteUserType.AdministratorInherit:
                    administratorItem.Selected = true;
                    break;
            }
        }
    }

    private void PopulateTitleDropDown(WebsiteUser user)
    {
        SocialTitleType[] types = new SocialTitleType[] {
            SocialTitleType.Mr,
            SocialTitleType.Mrs,
            SocialTitleType.Ms,
            SocialTitleType.Miss,
            SocialTitleType.Dr
        };

        ListItem tempItem;
        foreach (SocialTitleType type in types)
        {
            tempItem = new ListItem(type.ToString(), ((int)type).ToString());
            if ((user != null) && (user.SocialTitleType == type))
            {
                tempItem.Selected = true;
            }
            TitleDropDownList.Items.Add(tempItem);
        }
    }

    private void PopulateFields(WebsiteUser user)
    {
        this.FirstNameTextBox.Text = user.Forename;
        this.LastNameTextBox.Text = user.Surname;
        this.MobileNumberTextBox.Text = user.MobilePhone;
        this.LandlineNumberTextBox.Text = user.LandlinePhone;
        this.OptOutCheckBox.Checked = user.EmailOptOut;
        this.EmailAddressTextBox.Text = user.EmailAddress;
        this.TraderNameTextBox.Text = user.TradingName;
        this.TraderWebsiteTextBox.Text = user.TradingWebsite;

        if (Auth.ActiveUser.DatabaseId == Accessors.User.DatabaseId)
        {
            this.UserTypeDropDownList.Enabled = false;
        }

        if (user.Equals(Auth.ActiveUser))
        {
            UserLevelWarningLabel.Visible = true;
        }

        if (!user.IsTrader)
        {
            int siteDefault = LocalSettings.Fetch<int>("UserListingLimit", LocalSettings.Default.UserListingLimit);
            this.ListingLimitTextBox.Text = LocalSettings.Fetch<int>("UserListingLimit", siteDefault, user).ToString();
        }
        else
        {
            TraderPanel.Visible = true;
            int siteDefault = LocalSettings.Fetch<int>("TraderListingLimit", LocalSettings.Default.TraderListingLimit);
            this.ListingLimitTextBox.Text = LocalSettings.Fetch<int>("TraderListingLimit", siteDefault, user).ToString();
        }

        this.EnableAdvertsCheckBox.Checked = LocalSettings.Fetch<bool>(
            "EnableAdverts", LocalSettings.Default.EnableAdverts, user);

        this.PopulateTitleDropDown(user);
        this.PopulateTypeDropDown(user);
        this.PopulateLocationDropDown(user.LocationId);
        this.PopulateTraderTypeDropDown(user.TraderType);
    }

    private void PopulateTraderTypeDropDown(TraderType select)
    {
        TraderType[] typeArray = new TraderType[] {
            TraderType.None,
            TraderType.Corporate,
            TraderType.Charity
        };

        TraderTypeDropDownList.Items.Clear();
        foreach (TraderType type in typeArray)
        {
            ListItem item = new ListItem(type.ToString(), ((int)type).ToString());
            if (type == select)
            {
                item.Selected = true;
            }
            TraderTypeDropDownList.Items.Add(item);
        }
    }

    private void PopulateLocationDropDown(int locationId)
    {
        Location.DataBind(LocationDropDownList, locationId, "Choose...");
    }

    protected void ContinueButton_Click(object sender, ImageClickEventArgs e)
    {
        Validate();
        ValidationErrorLabel.Visible = false;
        if (!IsValid)
        {
            ValidationErrorLabel.Visible = true;
            return;
        }

        SocialTitleType titleType = (SocialTitleType)int.Parse(
            TitleDropDownList.SelectedValue);
        
        WebsiteUserType userType = (WebsiteUserType)int.Parse(
            UserTypeDropDownList.SelectedValue);

        TraderType traderType = (TraderType)int.Parse(
            TraderTypeDropDownList.SelectedValue);

        string traderName = null, traderWebsite = null;

        if (!String.IsNullOrEmpty(TraderNameTextBox.Text))
        {
            traderName = TraderNameTextBox.Text;
        }

        if (!String.IsNullOrEmpty(TraderWebsiteTextBox.Text))
        {
            traderWebsite = TraderWebsiteTextBox.Text;
        }

        switch (PageMode)
        {
            case PageMode.Create:
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
                    userType,
                    OptOutCheckBox.Checked,
                    DateTime.Now,
                    DateTime.Now,
                    DateTime.Now,
                    RegisterType.AdministratorRegistered,
                    traderName,
                    traderWebsite,
                    traderType);
                newUser.Create();
                Response.Redirect(LocalSettings.UserModifyRedirect);
                break;

            default:
                Accessors.User.UpdateDate = DateTime.Now;
                Accessors.User.Forename = FirstNameTextBox.Text;
                Accessors.User.Surname = LastNameTextBox.Text;
                Accessors.User.EmailAddress = EmailAddressTextBox.Text;
                Accessors.User.SocialTitleType = titleType;
                Accessors.User.LocationId = int.Parse(LocationDropDownList.SelectedValue);
                Accessors.User.EmailOptOut = OptOutCheckBox.Checked;
                Accessors.User.TradingName = traderName;
                Accessors.User.TradingWebsite = traderWebsite;
                Accessors.User.TraderType = traderType;

                Accessors.User.SetLandlineNumber(LandlineAreaTextBox.Text, LandlineNumberTextBox.Text);
                Accessors.User.SetMobileNumber(MobileAreaTextBox.Text, MobileNumberTextBox.Text);

                if (!Accessors.User.IsTrader)
                {
                    LocalSettings.ModifySetting<int>(
                        "UserListingLimit",
                        int.Parse(ListingLimitTextBox.Text),
                        LocalSettings.Default.UserListingLimit, Accessors.User);
                }
                else
                {
                    LocalSettings.ModifySetting<int>(
                        "TraderListingLimit",
                        int.Parse(ListingLimitTextBox.Text),
                        LocalSettings.Default.TraderListingLimit, Accessors.User);
                }

                LocalSettings.ModifySetting<bool>(
                    "EnableAdverts",
                    EnableAdvertsCheckBox.Checked,
                    LocalSettings.Default.EnableAdverts, Accessors.User);

                if (!String.IsNullOrEmpty(PasswordTextBox.Text))
                {
                    Accessors.User.Password = PasswordTextBox.Text;
                }

                if (Auth.ActiveUser.DatabaseId == Accessors.User.DatabaseId)
                {
                    // Update self in session if editing self.
                    Auth.ActiveUser = Accessors.User;
                }
                else
                {
                    // Only change user type for other users.
                    Accessors.User.UserType = userType;
                }

                Accessors.User.Modify();
                MultiView.SetActiveView(SuccessView);
                break;
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

    protected void RemoveContinueButton_Click(object sender, ImageClickEventArgs e)
    {
        Accessors.User.Remove();
        Response.Redirect(LocalSettings.UserModifyRedirect);
    }

    protected void RemoveCancelButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect(LocalSettings.UserModifyRedirect);
    }

    protected TraderType GetSelectedTraderType(DropDownList dropDown)
    {
        return (TraderType)int.Parse(dropDown.SelectedValue);
    }

    protected void TraderTypeDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        TraderType type = GetSelectedTraderType(sender as DropDownList);
        SetListingLimitDefault(ListingLimitTextBox, type);

        if (type != TraderType.None)
        {
            TraderPanel.Visible = true;
            AutoFocusControl = TraderNameTextBox;
        }
        else
        {
            TraderPanel.Visible = false;
            AutoFocusControl = sender as DropDownList;
        }
    }

    protected void SetListingLimitDefault(TextBox target, TraderType type)
    {
        if (type == TraderType.None)
        {
            target.Text = LocalSettings.Fetch<int>(
                "UserListingLimit",
                LocalSettings.Default.UserListingLimit).ToString();
        }
        else
        {
            target.Text = LocalSettings.Fetch<int>(
                "TraderListingLimit",
                LocalSettings.Default.TraderListingLimit).ToString();
        }
    }

    protected void DefaultListingLimitLinkButton_Click(object sender, EventArgs e)
    {
        TraderType type = GetSelectedTraderType(TraderTypeDropDownList);
        SetListingLimitDefault(ListingLimitTextBox, type);
        AutoFocusControl = sender as LinkButton;
    }

    protected void ValidationEmailButton_Click(object sender, EventArgs e)
    {
        string url = this.ResolveUrl("~/UserHome.aspx?VerifyAuthCode={0}");
        Accessors.User.SendVerificationEmail(
            "http://" + Request.Url.Host + ":" + Request.Url.Port + url, Page);

        ValidationEmailSentLabel.Visible = true;
    }

    protected void BanContinueImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Accessors.User.Ban(BanMessageTextBox.Text, int.Parse(BanPeriodTextBox.Text));

        if (BanPeriodTextBox.Text != "-1")
        {
            BanUntilDateLabel.Text = "until " + Accessors.User.BanUntil.ToShortDateString();
        }
        else
        {
            BanUntilDateLabel.Text = "permanently";
        }

        MultiView.SetActiveView(BanFinishView);
    }
}
