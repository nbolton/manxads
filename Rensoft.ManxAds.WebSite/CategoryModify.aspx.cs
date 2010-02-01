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


public partial class CategoryModify : StandardPage
{
    public CategoryModify() :
        base(ManxAds.WebsiteUserType.AdministratorOnly, true, true) { }

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
        if (accessors.CategoryId > 0)
        {
            base.PageMode = PageMode.Modify;
        }

        // Then override with delete, etc.
        base.UpdatePageMode(accessors);
    }

    protected override void InitializePage()
    {
        DefaultButton defaultButton = new DefaultButton(ContinueButton);
        defaultButton.AssociateWith(TitleTextBox);
        defaultButton.AssociateWith(DetailsTextBox);

        DefaultButton uploadDefault = new DefaultButton(ThumbnailUploadButton);
        defaultButton.AssociateWith(ThumbnailFileUpload);

        if (IsPostBack)
        {
            return;
        }

        this.TitleTextBox.MaxLength = LocalSettings.CategoryTitleLimit;
        this.TitleMaxLengthLabel.Text = LocalSettings.CategoryTitleLimit.ToString();

        switch (PageMode)
        {
            case PageMode.Create:
                this.TitleLabel.Text = "New Category";
                this.MultiView.SetActiveView(DefaultView);
                break;

            case PageMode.Modify:
                this.TitleLabel.Text = "Edit '" + Accessors.Category.Title + "'";
                this.PopulateFields(Accessors.Category);
                this.MultiView.SetActiveView(DefaultView);
                break;

            case PageMode.Remove:
                this.TitleLabel.Text = "Delete '" + Accessors.Category.Title + "'";
                this.RemoveTitleLabel.Text = Accessors.Category.Title;
                this.MultiView.SetActiveView(RemoveView);
                break;
        }

        this.Title = this.TitleLabel.Text;
        base.InitializePage();
    }

    private void PopulateFields(Category category)
    {
        this.TitleTextBox.Text = category.Title;
        this.DetailsTextBox.Text = category.Description;

        if (category.HasImage)
        {
            UploadedThumbnailPanel.Visible = true;
            UploadedThumbnailImage.ImageUrl = category.ImageUrl;
        }
    }

    protected void ContinueButton_Click(object sender, ImageClickEventArgs e)
    {
        this.Validate("Details");
        if (CreateOrModify())
        {
            // First upload pending images.
            UploadImages(false);
            MultiView.SetActiveView(SuccessView);
        }
    }

    private void UploadImages(bool focus)
    {
        if (!ThumbnailFileUpload.HasFile)
        {
            return;
        }

        if (focus)
        {
            AutoFocusControl = ThumbnailFileUpload;
        }

        Accessors.Category.CreateImage(
            this.Server, ThumbnailFileUpload.FileContent);
    }

    protected bool CreateOrModify()
    {
        if (!IsValid)
        {
            return false;
        }

        string title = TitleTextBox.Text.Trim();
        string details = DetailsTextBox.Text.Trim();

        switch (PageMode)
        {
            case PageMode.Create:
                Accessors.Category = new Category(
                    title,
                    details,
                    0, false);
                Accessors.Category.Create();
                break;

            default:
                Accessors.Category.Title = title;
                Accessors.Category.Description = details;
                Accessors.Category.Modify();
                break;
        }

        return true;
    }

    protected void RemoveContinueButton_Click(object sender, ImageClickEventArgs e)
    {
        Accessors.Category.Remove(this.Server);
        MultiView.SetActiveView(SuccessView);
    }

    protected void RemoveCancelButton_Click(object sender, ImageClickEventArgs e)
    {
        RedirectToPreviousPage(true);
    }

    protected void ThumbnailUploadButton_Click(object sender, EventArgs e)
    {
        this.Validate("Details");
        this.Validate("Images");
        if (CreateOrModify())
        {
            UploadImages(true);

            switch (PageMode)
            {
                case PageMode.Create:
                    // If in create mode, switch to modify mode.
                    Response.Redirect(Accessors.Category.ModifyUrl);
                    break;

                default:
                    PopulateFields(Accessors.Category);
                    break;
            }
        }
    }

    protected void DeleteLinkButton_Click(object sender, EventArgs e)
    {
        Accessors.Category.RemoveImage(this.Server);
        UploadedThumbnailPanel.Visible = false;
        AutoFocusControl = ThumbnailFileUpload;
    }
}
