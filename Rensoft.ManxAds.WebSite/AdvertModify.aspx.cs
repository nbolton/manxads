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

using System.IO;
using System.Drawing;
using System.Net.Mail;
using ManxAds;

public partial class AdvertModify : StandardPage
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

    /// <summary>
    /// Change page mode to modify if there is a query accessor listing.
    /// </summary>
    /// <param name="accessors"></param>
    protected override void UpdatePageMode(QueryAccessors accessors)
    {
        // Set the default page mode.
        base.PageMode = PageMode.Create;

        // Modify, if listing ID specified.
        if (accessors.AdvertId > 0)
        {
            base.PageMode = PageMode.Modify;
        }

        // Then override with delete, etc.
        base.UpdatePageMode(accessors);
    }

    public AdvertModify() : base(ManxAds.WebsiteUserType.AdvertiserOnly, true, true) { }

    protected override void InitializePage()
    {
        DefaultButton defaultButton = new DefaultButton(ContinueButton);
        defaultButton.AssociateWith(RotateFrequencyTextBox);
        defaultButton.AssociateWith(HyperlinkTextBox);
        defaultButton.AssociateWith(AuthorisedCheckBox);

        DefaultButton defaultUpload = new DefaultButton(MediaUploadButton);
        defaultUpload.AssociateWith(MediaFileUpload);

        if ((PageMode != PageMode.Create) && ((Accessors.Advert == null) ||
               (!Accessors.Advert.AuthoriseEdit(Auth.ActiveUser))))
        {
            TitleLabel.Text = "Error";
            MultiView.SetActiveView(NotFoundView);
            return;
        }

        if (!AdminMode)
        {
            // Hide display time for non-admins.
            DisplayTimePanel.Visible = false;
        }

        if (!IsPostBack)
        {
            switch (PageMode)
            {
                case PageMode.Create:
                    TitleLabel.Text = "New Advert";
                    AuthorisedPanel.Visible = false;
                    PopulateSizeTypeDropDown(0);
                    PopulatePositionTypeDropDown(0);
                    PopulateFormatTypeDropDown(0);
                    MultiView.SetActiveView(DefaultView);
                    break;

                case PageMode.Modify:
                    this.TitleLabel.Text = "Edit Advert";
                    PopulateEverything(Accessors.Advert);
                    MultiView.SetActiveView(DefaultView);
                    break;

                case PageMode.Remove:
                    TitleLabel.Text = "Delete Advert";
                    MultiView.SetActiveView(RemoveView);
                    break;
            }

            Title = TitleLabel.Text;
        }

        base.InitializePage();
    }

    protected void PopulateEverything(Advert advert)
    {
        TitleTextBox.Text = advert.Title;
        AuthorisedStatusLabel.Text = advert.AuthorisedString;
        HyperlinkTextBox.Text = advert.Hyperlink;
        RotateFrequencyTextBox.Text = advert.RotateFrequency.ToString();
        HtmlTextBox.Text = advert.Html;
        PopulateSizeTypeDropDown(advert.SizeType);
        PopulatePositionTypeDropDown(advert.PositionType);
        PopulateFormatTypeDropDown(advert.FormatType);

        if (advert.MediaUrlSupported)
        {
            PopulateMediaPreview(advert);
        }

        if (!AdminMode)
        {
            AuthorisedCheckBox.Visible = false;
            AuthorisedCheckBox.Checked = false;
        }
        else
        {
            AuthorisedCheckBox.Checked = advert.Authorised;
        }

        if (advert.Authorised)
        {
            AuthorisedStatusLabel.ForeColor = Color.Green;
        }
        else
        {
            AuthorisedStatusLabel.ForeColor = Color.Red;
        }
    }

    protected void PopulateMediaPreview(Advert advert)
    {
        // Display a hyperlink to www.manxads.com/{advert-path}.
        const string urlFormat = "http://www.manxads.com/{0}";
        HyperLink hyperLink = new HyperLink();
        string url = hyperLink.ResolveClientUrl(advert.MediaUrl);
        MediaUrlLabel.Text = String.Format(urlFormat, url);

        MediaPreviewPanel.Visible = true;
        MediaPreview.SingleAdvert = advert;
    }

    protected void PopulatePositionTypeDropDown(AdvertPositionType selected)
    {
        ListItem top = new ListItem(
            "Top Leaderboard", ((int)AdvertPositionType.TopLeaderboard).ToString());

        ListItem left1 = new ListItem(
            "Square 1", ((int)AdvertPositionType.Square1).ToString());

        ListItem left2 = new ListItem(
            "Square 2", ((int)AdvertPositionType.Square2).ToString());

        ListItem left3 = new ListItem(
            "Square 3", ((int)AdvertPositionType.Square3).ToString());

        ListItem left4 = new ListItem(
            "Square 4", ((int)AdvertPositionType.Square4).ToString());

        ListItem bottom = new ListItem(
            "Bottom Leaderboard", ((int)AdvertPositionType.BottomLeaderboard).ToString());

        ListItem right = new ListItem(
            "Skyscraper", ((int)AdvertPositionType.Skyscraper).ToString());

        ListItem randomLeaderboard = new ListItem(
            "Random Leaderboard", ((int)AdvertPositionType.RandomLeaderboard).ToString());

        ListItem randomSquareButton = new ListItem(
            "Random Square", ((int)AdvertPositionType.RandomSquareButton).ToString());

        PositionTypeDropDownList.Items.Clear();
        PositionTypeDropDownList.Items.Add(top);
        PositionTypeDropDownList.Items.Add(right);
        PositionTypeDropDownList.Items.Add(left1);
        PositionTypeDropDownList.Items.Add(left2);
        PositionTypeDropDownList.Items.Add(left3);
        PositionTypeDropDownList.Items.Add(left4);
        PositionTypeDropDownList.Items.Add(bottom);
        PositionTypeDropDownList.Items.Add(randomLeaderboard);
        PositionTypeDropDownList.Items.Add(randomSquareButton);

        switch (selected)
        {
            case AdvertPositionType.TopLeaderboard:
                top.Selected = true;
                break;

            case AdvertPositionType.BottomLeaderboard:
                bottom.Selected = true;
                break;

            case AdvertPositionType.Skyscraper:
                right.Selected = true;
                break;

            case AdvertPositionType.Square1:
                left1.Selected = true;
                break;

            case AdvertPositionType.Square2:
                left2.Selected = true;
                break;

            case AdvertPositionType.Square3:
                left3.Selected = true;
                break;

            case AdvertPositionType.Square4:
                left4.Selected = true;
                break;

            case AdvertPositionType.RandomLeaderboard:
                randomLeaderboard.Selected = true;
                break;

            case AdvertPositionType.RandomSquareButton:
                randomSquareButton.Selected = true;
                break;
        }
    }

    protected void PopulateSizeTypeDropDown(AdvertSizeType selected)
    {
        ListItem leaderboard = new ListItem(
            "Leaderboard (728 x 90)",
            ((int)AdvertSizeType.Leaderboard).ToString());

        ListItem skyscraper = new ListItem(
            "Skyscraper (160 x 600)",
            ((int)AdvertSizeType.Skyscraper).ToString());

        ListItem squareButton = new ListItem(
            "Square Button (125 x 125)",
            ((int)AdvertSizeType.SquareButton).ToString());

        ListItem fullBanner = new ListItem(
            "Full Banner (468 x 60)",
            ((int)AdvertSizeType.FullBanner).ToString());

        SizeTypeDropDownList.Items.Clear();
        SizeTypeDropDownList.Items.Add(leaderboard);
        SizeTypeDropDownList.Items.Add(skyscraper);
        SizeTypeDropDownList.Items.Add(squareButton);
        SizeTypeDropDownList.Items.Add(fullBanner);

        switch (selected)
        {
            case AdvertSizeType.FullBanner:
                fullBanner.Selected = true;
                break;

            case AdvertSizeType.Leaderboard:
                leaderboard.Selected = true;
                break;

            case AdvertSizeType.Skyscraper:
                skyscraper.Selected = true;
                break;

            case AdvertSizeType.SquareButton:
                squareButton.Selected = true;
                break;
        }
    }

    protected void PopulateFormatTypeDropDown(AdvertFormatType format)
    {
        ListItem flash = new ListItem(
            "Flash Animation",
            ((int)AdvertFormatType.Flash).ToString());

        ListItem gif = new ListItem(
            "Gif Animation",
            ((int)AdvertFormatType.Gif).ToString());

        ListItem jpeg = new ListItem(
            "Jpeg Still Image",
            ((int)AdvertFormatType.Jpeg).ToString());

        ListItem png = new ListItem(
            "Png Still Image",
            ((int)AdvertFormatType.Png).ToString());

        ListItem html = new ListItem(
            "HTML Snippet",
            ((int)AdvertFormatType.Html).ToString());

        FormatTypeDropDownList.Items.Clear();
        FormatTypeDropDownList.Items.Add(flash);
        FormatTypeDropDownList.Items.Add(gif);
        FormatTypeDropDownList.Items.Add(jpeg);
        FormatTypeDropDownList.Items.Add(png);
        FormatTypeDropDownList.Items.Add(html);

        switch (format)
        {
            case AdvertFormatType.Flash:
                flash.Selected = true;
                break;

            case AdvertFormatType.Gif:
                gif.Selected = true;
                break;

            case AdvertFormatType.Jpeg:
                jpeg.Selected = true;
                break;

            case AdvertFormatType.Png:
                png.Selected = true;
                break;

            case AdvertFormatType.Html:
                html.Selected = true;
                break;
        }
    }

    protected bool CreateOrModify()
    {
        if (!IsValid)
        {
            return false;
        }

        AdvertSizeType sizeType = (AdvertSizeType)int.Parse(
            SizeTypeDropDownList.SelectedValue);

        AdvertPositionType positionType = (AdvertPositionType)int.Parse(
            PositionTypeDropDownList.SelectedValue);

        AdvertFormatType formatType = (AdvertFormatType)int.Parse(
            FormatTypeDropDownList.SelectedValue);

        string hyperlink = HyperlinkTextBox.Text;
        int rotateFrequency = int.Parse(RotateFrequencyTextBox.Text);

        bool authorised = false;
        if (AdminMode)
        {
            authorised = AuthorisedCheckBox.Checked;
        }

        switch (PageMode)
        {
            case PageMode.Create:
                Accessors.Advert = new Advert(
                    Auth.ActiveUser.DatabaseId,
                    sizeType,
                    formatType,
                    positionType,
                    TitleTextBox.Text,
                    true,
                    rotateFrequency,
                    hyperlink,
                    authorised,
                    HtmlTextBox.Text);
                Accessors.Advert.Create();
                break;

            default:
                Accessors.Advert.SizeType = sizeType;
                Accessors.Advert.PositionType = positionType;
                Accessors.Advert.FormatType = formatType;
                Accessors.Advert.RotateFrequency = rotateFrequency;
                Accessors.Advert.Title = TitleTextBox.Text;
                Accessors.Advert.Hyperlink = hyperlink;
                Accessors.Advert.Authorised = authorised;
                Accessors.Advert.Html = HtmlTextBox.Text;
                Accessors.Advert.Modify();
                break;
        }

        return true;
    }

    protected void MediaUploadButton_Click(object sender, EventArgs e)
    {
        Validate("Details");
        Validate("Media");
        if (CreateOrModify())
        {
            UploadMedia(Accessors.Advert);

            AutoFocusControl = MediaFileUpload;
            
            // Always redirect to redraw advert preview.
            Response.Redirect(Accessors.Advert.ModifyUrl);
        }
    }

    protected void ContinueButton_Click(object sender, ImageClickEventArgs e)
    {
        Validate("Details");
        if (CreateOrModify())
        {
            UploadMedia(Accessors.Advert);
            MultiView.SetActiveView(SuccessView);
        }

        if (!AdminMode)
        {
            // Send notify if non-admin.
            SendNotifyEmail(Auth.ActiveUser, Accessors.Advert);
        }
    }

    private void SendNotifyEmail(WebsiteUser user, Advert advert)
    {
        MailMessage email = new MailMessage(
            LocalSettings.MasterSendFromEmail, LocalSettings.AdvertRequestEmail);

        email.Subject = "Advert Request";
        email.Body = "Advert is now pending administrator authorisation.\r\n";
        email.Body += "\r\nUser Name:\t\t" + user.FullName;
        email.Body += "\r\nEmail Address:\t\t" + user.EmailAddress;
        email.Body += "\r\nTrader/Charity:\t\t" + (user.TradingName != null ? user.TradingName : "n/a");
        email.Body += "\r\nDimensions:\t\t" + advert.DimensionsSummary;
        email.Body += "\r\nPosition:\t\t" + advert.PositionTypeString;
        email.Body += "\r\nFormat:\t\t" + advert.FormatType.ToString();

        EmailTools.SendMessage(email, Page);
    }

    protected void UploadMedia(Advert advert)
    {
        if (!MediaFileUpload.HasFile)
        {
            return;
        }

        string savePath = Server.MapPath(advert.MediaUrl);
        MediaFileUpload.SaveAs(savePath);
    }

    protected void RemoveContinueButton_Click(object sender, ImageClickEventArgs e)
    {
        Accessors.Advert.Remove(this.Server);
        MultiView.SetActiveView(SuccessView);
    }

    protected void RemoveCancelButton_Click(object sender, ImageClickEventArgs e)
    {
        RedirectToPreviousPage(true);
    }
}
