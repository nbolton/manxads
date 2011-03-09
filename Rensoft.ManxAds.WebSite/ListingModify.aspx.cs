using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using ManxAds;

public partial class ListingModify : StandardPage
{
    private const string tempListingIdName = "tempListingId";

    public ListingModify() : base(WebsiteUserType.SellerOnly) { }

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

    protected bool tempListingExists
    {
        get { return (Accessors.ListingId != 0) || hasListingId(tempListingIdName); }
    }

    private void setTempListingId(int value)
    {
        setListingId(tempListingIdName, value);
    }

    private void setListingId(string name, int value)
    {
        ViewState[name] = value;
    }

    private void clearListingId(string name)
    {
        ViewState[name] = null;
    }

    private int getTempListingId()
    {
        return getListingId(tempListingIdName);
    }

    private bool hasListingId(string name)
    {
        return ViewState[name] != null;
    }

    private int getListingId(string name)
    {
        if (hasListingId(name))
        {
            return (int)ViewState[name];
        }
        else
        {
            throw new Exception("Listing ID (" + name + ") does not exist in viewstate.");
        }
    }

    /// <summary>
    /// Adds attributes to the upload button so that it changes to "Uploading..."
    /// when a user clicks on it.
    /// </summary>
    protected void SetUploadButtonAttributes()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("if (typeof(Page_ClientValidate) == 'function') { ");
        builder.Append("if (Page_ClientValidate() == false) { return false; }} ");
        builder.Append("this.value = 'Uploading...';");
        builder.Append("this.disabled = true;");
        builder.Append(ClientScript.GetPostBackEventReference(UploadImageUploadButton, null));
        builder.Append(";");
        UploadImageUploadButton.Attributes.Add("onclick", builder.ToString());
    }

    /// <summary>
    /// Assign default buttons to fields.
    /// </summary>
    protected void SetDefaultButtons()
    {
        DefaultButton defaultButton = new DefaultButton(DetailsNextButton);
        defaultButton.AssociateWith(TitleTextBox);
        defaultButton.AssociateWith(PriceValueTextBox);
        defaultButton.AssociateWith(ShowLocationCheckBox);
        defaultButton.AssociateWith(ShowLandlineCheckBox);
        defaultButton.AssociateWith(ShowMobileCheckBox);

        DefaultButton defaultUpload = new DefaultButton(UploadImageUploadButton);
        defaultUpload.AssociateWith(UploadImageFileUpload);
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
        if (accessors.ListingId > 0)
        {
            base.PageMode = PageMode.Modify;
        }

        // Then override with delete, etc.
        base.UpdatePageMode(accessors);
    }

    protected override void InitializePage()
    {
        SetDefaultButtons();
        SetUploadButtonAttributes();

        if ((PageMode != PageMode.Create) 
            && (!tempListingExists || !getTempListing().AuthoriseEdit(Auth.ActiveUser)))
        {
            MultiView.SetActiveView(NotFoundView);
            return;
        }

        // Only check the limit when not post back (as to not interrupt mid-creation).
        if ((!IsPostBack) && (PageMode == PageMode.Create) && Auth.ActiveUser.ListingLimitReached)
        {
            MultiView.SetActiveView(LimitReachedView);
            return;
        }

        if (!IsPostBack)
        {
            if ((PageMode == PageMode.Create) || (PageMode == PageMode.Modify))
            {
                this.TitleTextBox.MaxLength = LocalSettings.ListingTitleLimit;
                this.TitleMaxLengthLabel.Text = LocalSettings.ListingTitleLimit.ToString();

                ListItem[] priceTypes = new ListItem[4] {
                    new ListItem("Negotiable", ((int)PriceType.Variable).ToString()),
                    new ListItem("Non-negotiable", ((int)PriceType.Fixed).ToString()),
                    new ListItem("Free Listing", ((int)PriceType.Free).ToString()),
                    new ListItem("No Price", ((int)PriceType.None).ToString())
                };
                PriceTypeDropDownList.Items.AddRange(priceTypes);
            }

            switch (PageMode)
            {
                case PageMode.Create:
                    this.TitleLabel.Text = "New Listing";
                    this.MultiView.SetActiveView(DetailsView);
                    break;

                case PageMode.Modify:
                    this.TitleLabel.Text = "Edit '" + getTempListing().Title + "'";
                    this.PopulateDetailsView();
                    this.MultiView.SetActiveView(DetailsView);
                    break;

                case PageMode.Remove:
                    this.TitleLabel.Text = "Delete '" + getTempListing().Title + "'";
                    this.RemoveTitleLabel.Text = getTempListing().Title;
                    this.MultiView.SetActiveView(RemoveView);
                    break;
            }

            this.Title = TitleLabel.Text;
        }
    }

    private void PopulateLocationDropDown(int locationId)
    {
        Location.DataBind(LocationDropDownList, locationId, "Choose...");
    }

    protected void PopulateDetailsView()
    {
        Listing listing = getTempListing();

        TitleTextBox.Text = listing.Title;
        DetailsTextBox.Text = listing.Details;

        if (listing.PriceType != PriceType.Free)
        {
            NumberFormatInfo format = new NumberFormatInfo();
            format.CurrencySymbol = String.Empty;
            PriceValueTextBox.Text = listing.PriceValue.ToString("C", format);
        }

        if (PageMode == PageMode.Modify)
        {
            BoostPanel.Visible = true;
        }

        foreach (ListItem item in PriceTypeDropDownList.Items)
        {
            if (((PriceType)int.Parse(item.Value) == listing.PriceType))
            {
                item.Selected = true;
            }
        }

        if (!AdminMode)
        {
            // Only apply to non-admins.
            listing.RefreshBoostCountLimitReached();

            if (!listing.CanBoost)
            {
                BoostCheckBox.Enabled = false;
                BoostCheckBox.Checked = false;

                if (listing.IsBoosted)
                {
                    BoostCheckBox.Text = "Boost (Frozen for " + listing.BoostLimitExpiryHoursString + ")";
                }
                else if (listing.BoostLimitReached)
                {
                    BoostCheckBox.Text = "Boost (Limit of " + LocalSettings.BoostCountLimit + " reached)";
                }
            }
        }

        switch (listing.DetailsType)
        {
            case TextType.PlainText:
                DetailsTypeRadioButtonList.SelectedIndex = 0;
                break;

            case TextType.Html:
                DetailsTypeRadioButtonList.SelectedIndex = 1;
                break;
        }

        ShowLandlineCheckBox.Checked = listing.ShowLandline;
        ShowMobileCheckBox.Checked = listing.ShowMobile;
        ShowLocationCheckBox.Checked = listing.ShowLocation;

        UpdatePriceVisibility(listing.PriceType, false);
        PopulateLocationDropDown(listing.LocationId);
    }

    protected void ImagesDataBind(bool setFocus)
    {
        Listing listing = getTempListing();

        UploadImageThumbnailCheckBox.Checked = true;
        List<ListingImage> images = listing.Images;
        if (images.Count > 0)
        {
            ImagesDataList.DataSource = images;
            ImagesDataList.DataBind();
            UploadedImagesPanel.Visible = true;
            foreach (ListingImage image in images)
            {
                if (image.IsMaster)
                {
                    UploadImageThumbnailCheckBox.Checked = false;
                }
            }
        }
        else
        {
            UploadedImagesPanel.Visible = false;
        }

        if (setFocus)
        {
            // Focus just below upload box on next load.
            AutoFocusControl = UploadImageThumbnailCheckBox;
        }
    }

    protected void PriceTypeDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList list = sender as DropDownList;
        UpdatePriceVisibility((PriceType)int.Parse(list.SelectedValue), true);
    }

    protected void UpdatePriceVisibility(PriceType type, bool focus)
    {
        if ((type == PriceType.Free) || (type == PriceType.None))
        {
            PriceValueTextBox.Text = null;
            PriceValuePanel.Visible = false;
            
            if (focus)
            {
                AutoFocusControl = PriceTypeDropDownList;
            }
        }
        else
        {
            PriceValuePanel.Visible = true;

            if (focus)
            {
                AutoFocusControl = PriceValueTextBox;
            }
        }
    }

    protected void PriceValueCustomValidator_ServerValidate(
        object source, ServerValidateEventArgs args)
    {
        PriceType type = (PriceType)int.Parse(PriceTypeDropDownList.SelectedValue);
        if ((type != PriceType.Free) && (type != PriceType.None) && String.IsNullOrEmpty(args.Value))
        {
            args.IsValid = false;
        }
    }

    protected void CreateOrModify()
    {
        decimal priceValue = 0m;
        int sellerId = Auth.ActiveUser.DatabaseId;
        string title = TitleTextBox.Text.Trim();
        string details = DetailsTextBox.Text.Trim();
        PriceType priceType = (PriceType)int.Parse(
            PriceTypeDropDownList.SelectedValue);

        if (!String.IsNullOrEmpty(PriceValueTextBox.Text) &&
            (decimal.Parse(PriceValueTextBox.Text) > 0m))
        {
            // Check for valid positive price.
            priceValue = decimal.Parse(
                PriceValueTextBox.Text, NumberStyles.Currency);
        }
        else if (priceType != PriceType.None)
        {
            // Otherwise presume free item (if not none).
            priceType = PriceType.Free;
        }

        TextType detailsType = TextType.PlainText;
        if (DetailsTypeRadioButtonList.SelectedValue == "Html")
        {
            detailsType = TextType.Html;
        }

        Listing listing;
        if (!tempListingExists)
        {
            listing = new Listing(
                sellerId, title, details, priceValue, priceType,
                DateTime.Now, DateTime.Now, DateTime.Now,
                ShowLandlineCheckBox.Checked,
                ShowMobileCheckBox.Checked,
                ShowLocationCheckBox.Checked,
                int.Parse(LocationDropDownList.SelectedValue),
                true, detailsType);

            // Create disabled so it's invisible.
            listing.Enabled = false;
            listing.Create();

            // Store the created listing so it can be fetched later.
            setTempListingId(listing.DatabaseId);
        }
        else
        {
            // Listing has been created, so simply get it.
            listing = getTempListing();

            // Boost date may be out of sync; so get newest version.
            Listing currentListing = Listing.Fetch(listing.DatabaseId);
            currentListing.RefreshBoostCountLimitReached();
            if (currentListing.CanBoost && BoostCheckBox.Checked)
            {
                listing.BoostDate = DateTime.Now;
            }

            listing.Title = title;
            listing.Details = details;
            listing.PriceValue = priceValue;
            listing.PriceType = priceType;
            listing.ShowLandline = ShowLandlineCheckBox.Checked;
            listing.ShowMobile = ShowMobileCheckBox.Checked;
            listing.ShowLocation = ShowLocationCheckBox.Checked;
            listing.UpdateDate = DateTime.Now;
            listing.LocationId = int.Parse(LocationDropDownList.SelectedValue);
            listing.DetailsType = detailsType;

            // Apply the changes to the existing listing.
            listing.Modify();
        }
    }

    private Listing getListing(int id)
    {
        return Listing.Fetch(id);
    }

    private Listing getTempListing()
    {
        if (Accessors.ListingId != 0)
        {
            return Accessors.Listing;
        }
        else
        {
            return getListing(getTempListingId());
        }
    }

    protected void UploadImages()
    {
        if (!UploadImageFileUpload.HasFile)
        {
            return;
        }

        int imageId = getTempListing().CreateImage(
            UploadImageThumbnailCheckBox.Checked,
            this.Server,
            UploadImageFileUpload.FileContent);
    }

    protected void UploadImageUploadButton_Click(object sender, EventArgs e)
    {
        //if (!HasTempListing)
        //{
        //    showSessionTimeoutView();
        //    return;
        //}

        this.Validate("Images");
        if (IsValid)
        {
            UploadImages();
            ImagesDataBind(true);
        }
    }

    protected void RemoveContinueButton_Click(object sender, ImageClickEventArgs e)
    {
        // Disable update instead of deleting.
        //TempListing.Enabled = false;
        //TempListing.Modify();
        getTempListing().Remove(LocalSettings.ConnectionString);

        MultiView.SetActiveView(SuccessView);
    }

    protected void RemoveCancelButton_Click(object sender, ImageClickEventArgs e)
    {
        RedirectToPreviousPage(true);
    }

    protected void SetPreviewLinkButton_Click(object sender, EventArgs e)
    {
        LinkButton button = sender as LinkButton;
        int imageId = int.Parse(button.CommandArgument);
        getTempListing().SetPreviewImage(imageId);
        ImagesDataBind(true);
    }

    protected void DeleteLinkButton_Click(object sender, EventArgs e)
    {
        LinkButton button = sender as LinkButton;
        int imageId = int.Parse(button.CommandArgument);
        getTempListing().RemoveImage(imageId, this.Server);
        ImagesDataBind(true);
    }

    protected void ImagesDataList_ItemDataBound(
        object sender, DataListItemEventArgs e)
    {
        Label label = e.Item.FindControl("PreviewImageLabel") as Label;
        LinkButton link = e.Item.FindControl("SetPreviewLinkButton") as LinkButton;
        Literal literal = e.Item.FindControl("IsMaster") as Literal;

        if (literal == null)
        {
            return;
        }

        if (bool.Parse(literal.Text))
        {
            label.Visible = true;
            link.Visible = false;
        }
        else
        {
            label.Visible = false;
            link.Visible = true;
        }
    }

    protected void CategoryDataBind(bool setFocus)
    {
        List<ICategory> listingCategories = getTempListing().Categories;
        ListingCategoryDataList.DataSource = listingCategories;
        ListingCategoryDataList.DataBind();

        int mcc = LocalSettings.MaximumCategoryCount;
        CategoryAddButton.Enabled = (mcc == -1) || (mcc > listingCategories.Count);
        MaximumCategoryCountPanel.Visible = (mcc != -1);

        if (mcc != -1)
        {
            MaximumCategoryCountLabel.Text = mcc + (mcc == 1 ? " catagory" : " categories");
        }

        ListingCategoriesPanel.Visible = false;
        if (listingCategories.Count > 0)
        {
            ListingCategoriesPanel.Visible = true;
        }

        // Override set focus to false if add dialog is hidden.
        if (!PopulateCategoryDropDown(listingCategories))
        {
            setFocus = false;
        }

        if (setFocus)
        {
            AutoFocusControl = CategoryDropDownList;
        }
    }
    
    protected void PopulateCategoryDropDown()
    {
        // Use an empty ignore list so all categories are added.
        this.PopulateCategoryDropDown(new List<ICategory>());
    }

    /// <summary>
    /// Populates the 'Add To Category' dropdown list.
    /// </summary>
    /// <param name="ignorables">Category items not to add.</param>
    /// <returns>False if drop down list is not visible.</returns>
    protected bool PopulateCategoryDropDown(List<ICategory> ignorables)
    {
        int categoryCount = 0;
        ListItem tempListItem;
        CategoryDropDownList.Items.Clear();
        CategoryDropDownList.Items.Insert(0, new ListItem("Choose...", "-1"));

        foreach (Category category in Category.FetchByTitleAscending())
        {
            if (!ignorables.Contains(category))
            {
                categoryCount++;
                tempListItem = new ListItem(
                    category.Title, category.DatabaseId.ToString());
                CategoryDropDownList.Items.Add(tempListItem);

                if (Accessors.CategoryId == category.DatabaseId)
                {
                    tempListItem.Selected = true;
                }
            }
        }

        CategoryAddMultiView.SetActiveView(CategoryAddDefaultView);
        if (categoryCount == 0)
        {
            CategoryAddMultiView.SetActiveView(CategoryAddEmptyView);
            return false;
        }

        return true;
    }

    protected void CategoryAddButton_Click(object sender, EventArgs e)
    {
        int categoryId = int.Parse(CategoryDropDownList.SelectedValue);
        if (categoryId > 0)
        {
            getTempListing().AssociateWithCategory(categoryId);
            CategoryDataBind(false);
        }
    }

    protected void CategoryDeleteLinkButton_Click(object sender, EventArgs e)
    {
        LinkButton button = sender as LinkButton;
        int categoryId = int.Parse(button.CommandArgument);
        getTempListing().DisassociateWithCategory(categoryId);

        // In case user forgot to click Upload.
        UploadImages();
        CategoryDataBind(false);
    }

    protected void RemoveAllCategoriesLinkButton_Click(object sender, EventArgs e)
    {
        foreach (Category category in getTempListing().Categories)
        {
            getTempListing().DisassociateWithCategory(category.DatabaseId);
        }

        // In case user forgot to click Upload.
        UploadImages();
        CategoryDataBind(false);
    }

    protected void ListingCategoryDataList_ItemDataBound(
        object sender, DataListItemEventArgs e)
    {
        Image image = e.Item.FindControl("ThumbnailImage") as Image;
        if ((image != null) && (String.IsNullOrEmpty(image.ImageUrl)
            || !File.Exists(Server.MapPath(image.ImageUrl))))
        {
            image.ImageUrl = LocalSettings.PlaceHolderThumbnailPath;
        }
    }

    protected void CategoryCustomValidator_ServerValidate(
        object source, ServerValidateEventArgs args)
    {
        int categoryId = int.Parse(CategoryDropDownList.SelectedValue);

        // Make sure rest of page is valid first.
        if ((this.IsValid)
            && (!tempListingExists || (getTempListing().Categories.Count == 0))
            && (categoryId == -1))
        {
            args.IsValid = false;
        }
    }

    private string getWordPattern(string word)
    {
        return @"(?=\b\w{" + word.Length + @"}\b)\b\w*" + word + @"\b\w*";
    }

    private Regex getWordRegex(string word)
    {
        RegexOptions options = RegexOptions.Multiline | RegexOptions.IgnoreCase;
        string regexPattern = getWordPattern(word);
        return new Regex(regexPattern, options);
    }

    protected void OnCheckBadWords(object source, ServerValidateEventArgs args)
    {
        List<string> badWordList = LocalSettings.BadWordsList;
        foreach (string badWord in badWordList)
        {
            // Create case-irrelevant multiline search.
            Regex pattern = getWordRegex(badWord);

            // For each word, try to find in context by it's self.
            if (pattern.IsMatch(args.Value))
            {
                args.IsValid = false;
                break;
            }
        }
    }

    protected void DetailsNextButton_Click(object sender, ImageClickEventArgs e)
    {
        Validate("Details");
        if (IsValid)
        {
            CreateOrModify();
            MultiView.SetActiveView(CategoriesView);
        }
    }

    protected void DetailsFinishButton_Click(object sender, ImageClickEventArgs e)
    {
        Validate("Details");
        if (IsValid)
        {
            CreateOrModify();
            MultiView.SetActiveView(SuccessView);
        }
    }

    protected void CategoriesPreviousButton_Click(object sender, ImageClickEventArgs e)
    {
        MultiView.SetActiveView(DetailsView);
    }

    protected void CategoriesNextButton_Click(object sender, ImageClickEventArgs e)
    {
        Validate("Categories");

        if (IsValid)
        {
            // If a category has been selected, automatically associate.
            int categoryId = int.Parse(CategoryDropDownList.SelectedValue);
            if (categoryId > 0)
            {
                getTempListing().AssociateWithCategory(categoryId);
                CategoryDataBind(false);
            }

            MultiView.SetActiveView(ImagesView);
        }
    }

    protected void CategoriesFinishButton_Click(object sender, ImageClickEventArgs e)
    {
        Validate("Categories");

        if (IsValid)
        {
            // Apply details changes.
            CreateOrModify();

            // If a category has been selected, automatically associate.
            int categoryId = int.Parse(CategoryDropDownList.SelectedValue);
            if (categoryId > 0)
            {
                getTempListing().AssociateWithCategory(categoryId);
            }
            MultiView.SetActiveView(SuccessView);
        }
    }

    protected void ImagesPreviousButton_Click(object sender, ImageClickEventArgs e)
    {
        MultiView.SetActiveView(CategoriesView);
    }

    protected void ImagesFinishButton_Click(object sender, ImageClickEventArgs e)
    {
        Validate("Images");

        if (IsValid)
        {
            UploadImages();
            finalizeListing();
            MultiView.SetActiveView(DonateView);
        }
    }

    private void finalizeListing()
    {
        Listing listing = getTempListing();
        listing.Enabled = true;
        listing.Modify();
    }

    protected void DetailsView_Activate(object sender, EventArgs e)
    {
        if (tempListingExists)
        {
            this.PopulateLocationDropDown(getTempListing().LocationId);
        }
        else
        {
            this.PopulateLocationDropDown(Auth.ActiveUser.LocationId);
        }

        if (PageMode != PageMode.Create)
        {
            this.DetailsFinishButton.Visible = true;
        }
    }

    protected void CategoriesView_Activate(object sender, EventArgs e)
    {
        PopulateCategoryDropDown();
        CategoryDataBind(false);

        if (PageMode != PageMode.Create)
        {
            this.CategoriesFinishButton.Visible = true;
        }
    }

    protected void ImagesView_Activate(object sender, EventArgs e)
    {
        ImagesDataBind(false);
    }

    [Obsolete]
    private void showSessionTimeoutView()
    {
        TitleLabel.Visible = false;
        MultiView.SetActiveView(SessionTimeoutView);
    }

    protected void SuccessView_Activate(object sender, EventArgs e)
    {
        Listing listing = getTempListing();

        ImageUploadReminder.Visible = false;
        if (listing.Enabled && (listing.ImageCount == 0))
        {
            ImageUploadReminder.Visible = true;
        }

        SuccessDialog.ShowListingHyperLinks(listing.NavigateUrl);
    }

    protected void UploadImageExtensionValidator_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (UploadImageFileUpload.HasFile)
        {
            SupportedImageTypesLabel.Text = String.Join(
                ", ", LocalSettings.ListingImageTypes.ToArray());

            FileInfo file = new FileInfo(UploadImageFileUpload.FileName);
            if (string.IsNullOrEmpty(file.Extension))
            {
                args.IsValid = false;
                return;
            }

            string cleanExtension = file.Extension.Remove(0, 1).ToLower();
            if (!LocalSettings.ListingImageTypes.Contains(cleanExtension))
            {
                args.IsValid = false;
                return;
            }
        }
    }

    protected void UploadImagesLinkButton_Click(object sender, EventArgs e)
    {
        MultiView.SetActiveView(ImagesView);
    }

    protected void DonateFinishImageButton_Click(object sender, ImageClickEventArgs e)
    {
        MultiView.SetActiveView(SuccessView);
    }
}