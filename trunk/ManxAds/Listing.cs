using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using System.Web;
using System.IO;
using ManxAds.Search;
using System.Xml;
using System.Data.SqlTypes;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;

namespace ManxAds
{
    public class Listing : ListingBase
    {
        private int sellerId;
        private int masterImageId;
        private WebsiteUser seller;
        private decimal priceValue;
        private PriceType priceType;
        private bool showLandline;
        private bool showMobile;
        private bool showLocation;
        private int locationId;
        private bool enabled;
        private bool boostLimitReached;
        private int pageHits;

        public int SellerId
        {
            get { return sellerId; }
            set { sellerId = value; }
        }

        public string ShortTitle
        {
            get
            {
                float sizeEm = LocalSettings.ListingShortTitlePixelSize;

                // Get origional limit, then chop as per price length.
                float limit = LocalSettings.ListingShortTitlePixelLimit;
                limit -= StringTools.GetStringWeight(PriceFormatted, sizeEm);

                return StringTools.TrimToPixels(base.Title, 0, limit, sizeEm, "...");
            }
        }

        public string ShortDetails
        {
            get
            {
                int limit = LocalSettings.ListingShortDetailsLimit;
                return StringTools.TrimString(DetailsCleanedWithoutHtml, 0, limit, true, "...", 2);
            }
        }

        public string LongDetails
        {
            get
            {
                // Set defaults to seller limits.
                int overallLimit = LocalSettings.ListingLongDetailsLimit;
                int segmentCount = LocalSettings.SellerSearchSegmentCount;

                // Override with trader if neccecary.
                if (this.IsTraderListing)
                {
                    overallLimit = LocalSettings.ListingTraderDetailsLimit;
                    segmentCount = LocalSettings.TraderSearchSegmentCount;
                }

                if (SearchHighlight.IsEnabled)
                {
                    string highlighted = SearchHighlight.HighlightContext(
                        DetailsCleanedWithoutHtml, overallLimit, segmentCount);

                    if (!String.IsNullOrEmpty(highlighted))
                    {
                        return highlighted;
                    }
                }

                return StringTools.TrimString(DetailsCleanedWithoutHtml, 0, overallLimit, true, "...", 2);
            }
        }

        /// <summary>
        /// Gets the length of PriceFormatted, only counting
        /// alpha numeric characters and ignoring symbles.
        /// </summary>
        public int AlphaNumericPriceLength
        {
            get
            {
                int charCount = 0;
                foreach (char thisChar in PriceFormatted.ToCharArray())
                {
                    if ((thisChar != '.') && (thisChar != ','))
                    {
                        charCount++;
                    }
                }
                return charCount;
            }
        }

        /// <summary>
        /// Gets a currency formatted version of the price without CSS.
        /// </summary>
        public string PriceFormatted
        {
            get
            {
                if (priceType == PriceType.None)
                {
                    return String.Empty;
                }

                if (priceType == PriceType.Free)
                {
                    return LocalSettings.ListingPriceFreeText;
                }

                if (priceValue < 1m)
                {
                    int pennies = (int)Math.Round(priceValue * 100m, 2);
                    return String.Format(
                        LocalSettings.ListingPricePeniesFormat, pennies);
                }

                string formatted = PriceValue.ToString(
                    LocalSettings.ListingPriceFormat);

                if (formatted.Contains(".00"))
                {
                    formatted = formatted.Replace(".00", String.Empty);
                }

                return formatted;
            }
        }

        /// <summary>
        /// Gets the CSS style name for the current price.
        /// </summary>
        public string PriceColourStyle
        {
            get
            {
                switch (priceType)
                {
                    case PriceType.Fixed:
                        return "ColouredPriceFixed";

                    case PriceType.Variable:
                        return "ColouredPriceVariable";

                    case PriceType.Free:
                        return "ColouredPriceFree";

                    case PriceType.None:
                        return String.Empty;
                }

                throw new NotSupportedException(
                    "The supplied price type is not supported.");
            }
        }

        /// <summary>
        /// Gets the formatted price wrapped with CSS span tags.
        /// </summary>
        public string PriceColoured
        {
            get
            {
                if (priceType == PriceType.None)
                {
                    return String.Empty;
                }

                return string.Format(
                    LocalSettings.ListingColouredPriceFormat,
                    this.PriceColourStyle, this.PriceFormatted);
            }
        }

        public decimal PriceValue
        {
            get { return priceValue; }
            set { priceValue = value; }
        }

        /// <summary>
        /// Gets the price type for this listing.
        /// </summary>
        public PriceType PriceType
        {
            get { return priceType; }
            set { priceType = value; }
        }

        /// <summary>
        /// Gets a string representation of PriceType.
        /// </summary>
        public string PriceTypeString
        {
            get
            {
                switch (priceType)
                {
                    case PriceType.Fixed:
                        return "Non-negotiable";

                    case PriceType.Variable:
                        return "Negotiable";

                    case PriceType.Free:
                        return "Free Listing";
                }

                throw new NotSupportedException(
                    "String conversion of price type not supported.");
            }
        }

        public string BoostLimitExpiryHoursString
        {
            get
            {
                double totalHours = BoostLimitExpiry.TotalHours;
                int hours = (int)Math.Ceiling(totalHours);
                if (hours == 1)
                {
                    return "1 hour";
                }
                return hours + " hours";
            }
        }

        public bool CanBoost
        {
            get { return !IsBoosted & !boostLimitReached; }
        }

        public bool BoostLimitReached
        {
            get { return boostLimitReached; }
        }

        public bool ShowLandline
        {
            get { return showLandline; }
            set { showLandline = value; }
        }

        public bool ShowMobile
        {
            get { return showMobile; }
            set { showMobile = value; }
        }

        public bool ShowLocation
        {
            get { return showLocation; }
            set { showLocation = value; }
        }

        public int LocationId
        {
            get { return locationId; }
            set { locationId = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating wether or not the listing is enabled.
        /// When a listing is not enabled, it can be interprated as "deleted".
        /// </summary>
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public string LocationString
        {
            get { return Location.Fetch(locationId).Name; }
        }

        public bool IsTraderListing
        {
            get { return Seller.IsTrader; }
        }

        public string TraderName
        {
            get { return Seller.TradingName; }
        }

        public string TraderWebsite
        {
            get { return Seller.TradingWebsite; }
        }

        public string TraderLogo
        {
            get
            {
                if (Seller.IsTrader)
                {
                    return Seller.TraderLogoRelativePath;
                }
                return null;
            }
        }

        public int MasterImageId
        {
            get { return masterImageId; }
        }

        public ListingImage MasterImage
        {
            get { return this.FetchImage(masterImageId); }
        }

        /// <summary>
        /// Gets a list of images for this listing.
        /// </summary>
        public List<ListingImage> Images
        {
            get { return ListingImage.Fetch(this); }
        }

        public int ImageCount
        {
            get { return this.Images.Count; }
        }

        public string ThumbnailUrl
        {
            get { return MasterImage.ThumbnailUrl; }
        }

        public string SmallImageUrl
        {
            get { return MasterImage.SmallImageUrl; }
        }

        public string FullImageUrl
        {
            get { return MasterImage.FullImageUrl; }
        }

        public string NavigateUrl
        {
            get
            {
                string format = LocalSettings.ListingInlineNavigateUrlFormat;
                if (LocalSettings.ListingDetailsPopup)
                {
                    format = LocalSettings.ListingPopupNavigateUrlFormat;
                }

                return String.Format(format, SearchEngineTitle, DatabaseId);
            }
        }

        public string ContactUrl
        {
            get
            {
                string format = LocalSettings.ListingInlineContactUrlFormat;
                if (LocalSettings.ListingDetailsPopup)
                {
                    format = LocalSettings.ListingPopupContactUrlFormat;
                }
                return String.Format(format, base.DatabaseId);
            }
        }

        public string ModifyUrl
        {
            get
            {
                return String.Format(LocalSettings.ListingModifyUrlFormat, base.DatabaseId);
            }
        }

        public string RemoveUrl
        {
            get
            {
                return String.Format(LocalSettings.ListingRemoveUrlFormat, base.DatabaseId);
            }
        }

        /// <summary>
        /// Gets the cached seller who owns this listing.
        /// </summary>
        public WebsiteUser Seller
        {
            get
            {
                if (seller == null)
                {
                    seller = WebsiteUser.Fetch(sellerId);
                }
                return seller;
            }
        }

        /// <summary>
        /// Gets the categories where this listing is displayed.
        /// </summary>
        public List<Category> Categories
        {
            get { return Category.Fetch(this); }
        }

        /// <summary>
        /// Returns the database ID formatted as a ManxAds ID.
        /// </summary>
        public string ManxAdsId
        {
            get { return String.Format(LocalSettings.ManxAdsIdFormat, base.DatabaseId); }
        }

        public static int CountEstimate
        {
            get { return RunCount(SqlDateTime.MinValue.Value); }
        }

        public int PageHits
        {
            get { return pageHits; }
            set { pageHits = value; }
        }

        protected Listing() : base() { }

        protected Listing(
            int databaseId,
            int sellerId,
            int masterImageId,
            string title,
            string details,
            decimal priceValue,
            PriceType priceType,
            DateTime boostDate,
            DateTime createDate,
            TextType detailsType)
            : base(databaseId, title, details, detailsType, createDate, createDate, boostDate)
        {
            this.sellerId = sellerId;
            this.masterImageId = masterImageId;
            this.priceValue = priceValue;
            this.priceType = priceType;
        }

        public Listing(
            int sellerId,
            string title,
            string details,
            decimal priceValue,
            PriceType priceType,
            DateTime createDate,
            DateTime updateDate,
            DateTime boostDate,
            bool showLandline,
            bool showMobile,
            bool showLocation,
            int locationId,
            bool enabled,
            TextType detailsType)
        {
            this.Title = title;
            this.Details = details;
            this.DetailsType = detailsType;
            this.CreateDate = createDate;
            this.UpdateDate = updateDate;
            this.BoostDate = boostDate;

            this.sellerId = sellerId;
            this.priceValue = priceValue;
            this.priceType = priceType;
            this.showLandline = showLandline;
            this.showMobile = showMobile;
            this.showLocation = showLocation;
            this.locationId = locationId;
            this.enabled = enabled;
        }

        internal Listing(
            int databaseId,
            int sellerId,
            int masterImageId,
            string title,
            string details,
            decimal priceValue,
            PriceType priceType,
            DateTime createDate,
            DateTime updateDate,
            DateTime boostDate,
            bool showLandline,
            bool showMobile,
            bool showLocation,
            int locationId,
            bool enabled,
            TextType detailsType)
            : this
            (sellerId,
            title,
            details,
            priceValue,
            priceType,
            createDate,
            updateDate,
            boostDate,
            showLandline,
            showMobile,
            showLocation,
            locationId,
            enabled,
            detailsType)
        {
            this._DatabaseId = databaseId;
            this.masterImageId = masterImageId;
        }

        /// <summary>
        /// Used for testing purposes.
        /// </summary>
        protected Listing(int databaseId) : base(databaseId) { }

        [Obsolete()]
        internal Listing(StoredProceedure sp)
            : this(sp, sp.GetReaderValue<int>("ListingId"),
            ContextKeywordCollection.Empty) { }

        [Obsolete()]
        internal Listing(
            StoredProceedure sp,
            int databaseId,
            ContextKeywordCollection searchHighlight)
            : this(
            databaseId,
            sp.GetReaderValue<int>("SellerId"),
            sp.GetReaderValue<int>("MasterImageId"),
            sp.GetReaderValue<string>("Title"),
            sp.GetReaderValue<string>("Details"),
            sp.GetReaderValue<decimal>("PriceValue"),
            sp.GetReaderValue<PriceType>("PriceType"),
            sp.GetReaderValue<DateTime>("CreateDate"),
            sp.GetReaderValue<DateTime>("UpdateDate"),
            sp.GetReaderValue<DateTime>("BoostDate"),
            sp.GetReaderValue<bool>("ShowLandline"),
            sp.GetReaderValue<bool>("ShowMobile"),
            sp.GetReaderValue<bool>("ShowLocation"),
            sp.GetReaderValue<int>("LocationId"),
            sp.GetReaderValue<bool>("Enabled"),
            sp.GetReaderValue<TextType>("DetailsType"))
        {
            base.SearchHighlight = searchHighlight;   
        }

        new internal static Listing Parse(StoredProceedure sp)
        {
            Listing listing = new Listing();
            listing._DatabaseId = sp.GetReaderValue<int>("ListingId");
            listing.sellerId = sp.GetReaderValue<int>("SellerId");
            listing.masterImageId = sp.GetReaderValue<int>("MasterImageId");
            listing.Title = sp.GetReaderValue<string>("Title");
            listing.Details = sp.GetReaderValue<string>("Details");
            listing.CreateDate = sp.GetReaderValue<DateTime>("CreateDate");
            listing.UpdateDate = sp.GetReaderValue<DateTime>("UpdateDate");
            listing.BoostDate = sp.GetReaderValue<DateTime>("BoostDate");
            listing.priceValue = sp.GetReaderValue<decimal>("PriceValue");
            listing.priceType = sp.GetReaderValue<PriceType>("PriceType");
            listing.showLandline = sp.GetReaderValue<bool>("ShowLandline");
            listing.showMobile = sp.GetReaderValue<bool>("ShowMobile");
            listing.showLocation = sp.GetReaderValue<bool>("ShowLocation");
            listing.locationId = sp.GetReaderValue<int>("LocationId");
            listing.enabled = sp.GetReaderValue<bool>("Enabled");
            listing.DetailsType = sp.GetReaderValue<TextType>("DetailsType");
            listing.PageHits = sp.GetReaderValue<int>("PageHits");
            return listing;
        }

        public void Create()
        {
            using (StoredProceedure sp = new StoredProceedure("ListingCreate"))
            {
                sp.AddParam("@SellerId", this.sellerId);
                sp.AddParam("@Title", this.Title);
                sp.AddParam("@Details", this.Details);
                sp.AddParam("@DetailsType", this.DetailsType);
                sp.AddParam("@PriceValue", this.priceValue);
                sp.AddParam("@PriceType", this.priceType);
                sp.AddParam("@CreateDate", this.CreateDate);
                sp.AddParam("@UpdateDate", this.UpdateDate);
                sp.AddParam("@BoostDate", this.BoostDate);
                sp.AddParam("@ShowLandline", this.showLandline);
                sp.AddParam("@ShowMobile", this.showMobile);
                sp.AddParam("@ShowLocation", this.showLocation);
                sp.AddParam("@LocationId", this.locationId);
                sp.AddParam("@Enabled", this.enabled);
                sp.AddOutParam("@InsertId", SqlDbType.Int);

                sp.Connection.Open();
                sp.Command.ExecuteNonQuery();

                _DatabaseId = sp.GetParamValue<int>("@InsertId");
            }
        }

        public void Modify()
        {
            Modify(LocalSettings.ConnectionString);
        }

        public void Modify(string connectionString)
        {
            using (StoredProceedure sp = new StoredProceedure("ListingModify", connectionString))
            {
                sp.AddParam("@ListingId", this.DatabaseId);
                sp.AddParam("@SellerId", this.sellerId);
                sp.AddParam("@Title", this.Title);
                sp.AddParam("@Details", this.Details);
                sp.AddParam("@DetailsType", this.DetailsType);
                sp.AddParam("@PriceValue", this.priceValue);
                sp.AddParam("@PriceType", this.priceType);
                sp.AddParam("@CreateDate", this.CreateDate);
                sp.AddParam("@UpdateDate", this.UpdateDate);
                sp.AddParam("@BoostDate", this.BoostDate);
                sp.AddParam("@ShowLandline", this.showLandline);
                sp.AddParam("@ShowMobile", this.showMobile);
                sp.AddParam("@ShowLocation", this.showLocation);
                sp.AddParam("@LocationId", this.locationId);
                sp.AddParam("@Enabled", this.enabled);

                sp.Connection.Open();
                sp.Command.ExecuteNonQuery();
            }
        }

        public void Restore()
        {
            Restore(LocalSettings.ConnectionString);
        }

        public void Restore(string connectionString)
        {
            using (StoredProceedure sp = new StoredProceedure("ListingRestore", connectionString))
            {
                sp.AddParam("@ListingId", this.DatabaseId);

                sp.Connection.Open();
                sp.Command.ExecuteNonQuery();
            }
        }

        public void IncrementPageHit()
        {
            using (StoredProceedure sp = new StoredProceedure("ListingPageHitIncrement"))
            {
                sp.AddParam("@ListingId", this.DatabaseId);

                sp.Connection.Open();
                sp.Command.ExecuteNonQuery();
            }
        }

        [Obsolete("Use Remove(string connectionString) instead.")]
        public void Remove()
        {
            Remove(LocalSettings.ConnectionString);
        }

        /// <summary>
        /// Marks listing as not enabled, then updates the database. 
        /// </summary>
        public void Remove(string connectionString)
        {
            Enabled = false;

            // HACK: set the update date to now, to use it as a way of 
            // tracking when the listing was removed
            // TODO: implement a DeleteDate property
            UpdateDate = DateTime.Now;

            Modify(connectionString);
        }

        public void RemoveFromRecycleBin(
            string connectionString, PathUtility pathUtility)
        {
            foreach (ListingImage image in GetImages(connectionString))
            {
                image.Remove(pathUtility, connectionString);
            }

            using (StoredProceedure sp = new StoredProceedure(
                "ListingRemoveFromRecycleBin", connectionString))
            {
                sp.AddParam("listingId", DatabaseId);
                sp.Connection.Open();
                sp.Command.ExecuteNonQuery();
            }
        }

        public IEnumerable<ListingImage> GetImages(string connectionString)
        {
            return ListingImage.Fetch(this, connectionString);
        }

        public void AssociateWithCategory(Category category)
        {
            AssociateWithCategory(category.DatabaseId);
        }

        public void DisassociateWithCategory(Category category)
        {
            DisassociateWithCategory(category.DatabaseId);
        }

        /// <summary>
        /// Creates an association with a category.
        /// </summary>
        /// <param name="categoryId">Category ID to associate with.</param>
        public void AssociateWithCategory(int categoryId)
        {
            using (StoredProceedure sp = new StoredProceedure("ListingCategoryAssociate"))
            {
                sp.AddParam("@ListingId", this.DatabaseId);
                sp.AddParam("@CategoryId", categoryId);

                sp.Connection.Open();
                sp.Command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Removes an association from a category.
        /// </summary>
        /// <param name="categoryId">Category ID to disassociate with.</param>
        public void DisassociateWithCategory(int categoryId)
        {
            using (StoredProceedure sp = new StoredProceedure("ListingCategoryDisassociate"))
            {
                sp.AddParam("@ListingId", this.DatabaseId);
                sp.AddParam("@CategoryId", categoryId);

                sp.Connection.Open();
                sp.Command.ExecuteNonQuery();
            }
        }

        public int CreateImage(
            bool isMaster, HttpServerUtility server, Stream imageStream)
        {
            // Create the initial image (dosen't create in database).
            ListingImage image = new ListingImage(this.DatabaseId, isMaster);

            // Adds to list for databind in current session.
            this.Images.Add(image);

            // Creates image on server and database.
            return image.Create(server, imageStream);
        }

        public void RemoveImage(int imageId, HttpServerUtility server)
        {
            // Recreate image for image and database deletion.
            ListingImage image = new ListingImage(imageId, this.DatabaseId, false);

            // Remove from list for databind in current session.
            this.Images.Remove(image);

            // Remove image and database record.
            image.Remove(server);
        }

        public void SetPreviewImage(int imageId)
        {
            ListingImage image = new ListingImage(
                imageId, this.DatabaseId, false);
            image.SetMaster();
        }

        public static List<Listing> FetchTop(int limit, bool onlyImages)
        {
            Listing tempListing;
            List<Listing> listingList = new List<Listing>();

            using (StoredProceedure sp = new StoredProceedure("ListingFetchTop"))
            {
                sp.AddParam("@Limit", limit);
                sp.AddParam("@OnlyImages", onlyImages);

                sp.Connection.Open();
                sp.Reader = sp.Command.ExecuteReader();

                while (sp.Reader.Read())
                {
                    tempListing = new Listing(
                        sp.GetReaderValue<int>("ListingId"),
                        sp.GetReaderValue<int>("SellerId"),
                        sp.GetReaderValue<int>("MasterImageId"),
                        sp.GetReaderValue<string>("Title"),
                        sp.GetReaderValue<string>("Details"),
                        sp.GetReaderValue<decimal>("PriceValue"),
                        sp.GetReaderValue<PriceType>("PriceType"),
                        sp.GetReaderValue<DateTime>("BoostDate"),
                        sp.GetReaderValue<DateTime>("CreateDate"),
                        sp.GetReaderValue<TextType>("DetailsType"));

                    listingList.Add(tempListing);
                }
            }
            return listingList;
        }

        public static Listing Fetch(int databaseId)
        {
            return Fetch(databaseId, ContextKeywordCollection.Empty, LocalSettings.ConnectionString);
        }

        public static Listing Fetch(int databaseId, string connectionString)
        {
            return Fetch(databaseId, ContextKeywordCollection.Empty, connectionString);
        }

        public static Listing Fetch(int databaseId, ContextKeywordCollection keywords)
        {
            return Fetch(databaseId, keywords, LocalSettings.ConnectionString);
        }

        public static Listing Fetch(int databaseId, ContextKeywordCollection keywords, string connectionString)
        {
            using (StoredProceedure sp = new StoredProceedure("ListingFetchById", connectionString))
            {
                try
                {
                    sp.AddParam("@ListingId", databaseId);

                    sp.Connection.Open();
                    sp.Reader = sp.Command.ExecuteReader(CommandBehavior.SingleRow);
                    sp.Reader.Read();

                    return Listing.Parse(sp);
                }
                catch (CannotReadException ex)
                {
                    throw new NotFoundException(databaseId, ex);
                }
            }
        }

        public static List<Listing> Fetch(int limit, int page)
        {
            return Fetch(limit, page, "BoostDate", "DESC");
        }

        public static List<Listing> Fetch(int limit, int page, string sortColumn, string sortDirection)
        {
            string queryFormat =
                "SELECT * FROM ( " +
                    "SELECT TOP (@Length) * FROM ( " +

                        "SELECT TOP (@Length + @Index) * " +
                        "FROM VW_ListingFetch " +
                        "WHERE Enabled = 1 " +
                        "{3} " +
                        "ORDER BY {0} {1} " +

                    ") AS InnerOrder " +
                    "ORDER BY {0} {2} " +

                ") AS OuterOrder " +
                "ORDER BY {0} {1}";

            string priceExlude = string.Empty;
            if (sortColumn == "PriceValue")
            {
                priceExlude = "AND PriceType != 3";
            }

            string oppositDir = (sortDirection == "ASC") ? "DESC" : "ASC";
            string query = string.Format(queryFormat, sortColumn, sortDirection, oppositDir, priceExlude);

            List<Listing> listingList = new List<Listing>();
            using (SqlConnection connection = new SqlConnection(LocalSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                // Zero based value from page number.
                int index = (page - 1) * limit;

                command.Parameters.AddWithValue("@Index", index);
                command.Parameters.AddWithValue("@Length", limit);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                // Create stored proc handler for reader value parsing.
                StoredProceedure sp = StoredProceedure.FromReader(reader);

                while (reader.Read())
                {
                    listingList.Add(Listing.Parse(sp));
                }

                connection.Close();
            }
            return listingList;
        }

        public static List<Listing> Fetch(Category category, string sortMode)
        {
            List<Listing> listingList = new List<Listing>();
            using (StoredProceedure sp = new StoredProceedure("ListingFetchByCategoryId"))
            {
                sp.AddParam("@CategoryId", category.DatabaseId);
                sp.AddParam("@SortMode", sortMode);

                sp.Connection.Open();
                sp.Reader = sp.Command.ExecuteReader();

                while (sp.Reader.Read())
                {
                    listingList.Add(Listing.Parse(sp));
                }
            }
            return listingList;
        }

        public static List<Listing> Fetch(WebsiteUser seller)
        {
            return Fetch(seller, true);
        }

        public static List<Listing> Fetch(WebsiteUser seller, bool enabled)
        {
            List<Listing> listingList = new List<Listing>();
            using (StoredProceedure sp = new StoredProceedure("ListingFetchBySellerId"))
            {
                sp.AddParam("@SellerId", seller.DatabaseId);
                sp.AddParam("@Enabled", enabled);

                sp.Connection.Open();
                sp.Reader = sp.Command.ExecuteReader();

                while (sp.Reader.Read())
                {
                    listingList.Add(Listing.Parse(sp));
                }
            }
            return listingList;
        }

        public static List<Listing> FetchWhereHasAbuseVotes(string connectionString, bool adminNotified)
        {
            List<Listing> listingList = new List<Listing>();
            using (StoredProceedure sp = new StoredProceedure("ListingFetchWhereHasAbuseVotes", connectionString))
            {
                sp.AddParam("@AdminNotified", adminNotified);
                sp.Connection.Open();
                sp.Reader = sp.Command.ExecuteReader();

                while (sp.Reader.Read())
                {
                    listingList.Add(Listing.Parse(sp));
                }
            }
            return listingList;
        }

        public static List<Listing> FetchRecycleBinItems(
            string connectionString, DateTime updateBeforeDateTime)
        {
            List<Listing> listingList = new List<Listing>();
            using (StoredProceedure sp = new StoredProceedure(
                "ListingFetchRecycleBinItems", connectionString))
            {
                sp.AddParam("@updateBeforeDateTime", updateBeforeDateTime);
                sp.Connection.Open();
                sp.Reader = sp.Command.ExecuteReader();

                while (sp.Reader.Read())
                {
                    listingList.Add(Listing.Parse(sp));
                }
            }
            return listingList;
        }

        public static int RunCount(DateTime fromDate)
        {
            return RunCount(fromDate, true, 0);
        }

        internal static int RunCount(DateTime fromDate, bool enabled, int sellerId)
        {
            return RunCount(fromDate, enabled, sellerId, LocalSettings.ConnectionString);
        }

        /// <summary>
        /// Counts the number of listings in the database from a specific date
        /// </summary>
        /// <returns>Number of listings.</returns>
        public static int RunCount(DateTime fromDate, bool enabled, int sellerId, string connectionString)
        {
            using (StoredProceedure sp = new StoredProceedure("ListingCount", connectionString))
            {
                sp.AddParam("@FromDate", fromDate);
                sp.AddParam("@Enabled", enabled);
                sp.AddParam("@SellerId", sellerId);
                sp.AddOutParam("@Count", System.Data.SqlDbType.Int);

                sp.Connection.Open();
                sp.Command.ExecuteNonQuery();

                return sp.GetParamValue<int>("@Count");
            }
        }

        public ListingImage FetchImage(int listingImageId)
        {
            bool isMaster = false;
            if (listingImageId == this.masterImageId)
            {
                isMaster = true;
            }

            return new ListingImage(
                listingImageId, this.DatabaseId, isMaster);
        }

        public static List<Listing> Search(SearchCriteria critera)
        {
            List<Listing> listingList = null;
            WordList wordList = new WordList(
                critera.Phrase, false,
                LocalSettings.SearchQueryWordLimit,
                true, critera.AnyKeywords, true);

            using (StoredProceedure sp = new StoredProceedure("ListingSearch"))
            {
                sp.AddParam("@wordsXml", wordList.ToXml());
                sp.AddParam("@anyKeywords", true); // Why does this not use critera.AnyKeywords?
                sp.AddParam("@maxDate", critera.EndDate);
                sp.AddParam("@minDate", critera.StartDate);
                sp.AddParam("@maxPrice", critera.EndPrice);
                sp.AddParam("@minPrice", critera.StartPrice);
                sp.AddParam("@categoryId", critera.CategoryId);
                sp.AddParam("@locationId", critera.LocationId);
                sp.AddParam("@sellerId", critera.SellerId);
                sp.AddParam("@resultsLimit", -1);
                sp.AddParam("@startIndex", -1);

                sp.Connection.Open();
                sp.ExecuteReader();

                if (!sp.Reader.Read())
                    throw new InvalidOperationException("Could not get search results XML.");

                string xml = sp.GetReaderValue<string>("ListingSearch");
                listingList = ParseXmlListingSearch(xml);
            }

            // don't sort wildcard searches
            if (critera.Phrase != "*")
            {
                Comparison<Listing> weightSort = new Comparison<Listing>(CompareWeights);
                listingList.Sort(weightSort);
            }

            return listingList;
        }

        protected static int CompareWeights(Listing x, Listing y)
        {
            float xWeight = x.SearchHighlight.TotalWeight;
            float yWeight = y.SearchHighlight.TotalWeight;

            if (xWeight < yWeight)
            {
                return +1;
            }
            else if (xWeight > yWeight)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        public static List<Listing> ParseXmlListingSearch(string xml)
        {
            List<Listing> listingList = new List<Listing>();
            XmlDocument document = new XmlDocument();
            document.LoadXml(xml);
            XmlNode root = document.DocumentElement;

            foreach (XmlNode listingNode in root.ChildNodes)
            {
                Listing listing = new Listing(
                    int.Parse(listingNode.Attributes["ListingId"].Value),
                    int.Parse(listingNode.Attributes["SellerId"].Value),
                    int.Parse(listingNode.Attributes["MasterImageId"].Value),
                    listingNode.Attributes["Title"].Value,
                    listingNode.Attributes["Details"].Value,
                    decimal.Parse(listingNode.Attributes["PriceValue"].Value),
                    (PriceType)int.Parse(listingNode.Attributes["PriceType"].Value),
                    DateTime.Parse(listingNode.Attributes["BoostDate"].Value),
                    DateTime.Parse(listingNode.Attributes["CreateDate"].Value),
                    (TextType)int.Parse(listingNode.Attributes["DetailsType"].Value));

                foreach (XmlNode childNode in listingNode.ChildNodes)
                {
                    if (childNode.Name == "RelevantKeywords")
                    {
                        foreach (XmlNode keywordNode in childNode.ChildNodes)
                        {
                            ContextKeyword keyword = new ContextKeyword(
                                keywordNode.Attributes["Name"].Value,
                                float.Parse(keywordNode.Attributes["Weight"].Value));
                            listing.SearchHighlight.Add(keyword);
                        }
                    }
                }

                listingList.Add(listing);
            }
            return listingList;
        }

        public bool AuthoriseEdit(WebsiteUser user)
        {
            if ((user.UserType & WebsiteUserType.AdministratorOnly) != 0)
            {
                return true;
            }

            if (user.DatabaseId == this.sellerId)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Uses the BoostCountLimit setting to determin if listings can be boosted.
        /// </summary>
        /// <param name="list"></param>
        public static void StepDisableBoost(ref List<Listing> list)
        {
            int boostedCount = 0;
            foreach (Listing listing in list)
            {
                // Where listings cannot be bosted, this is because they are
                if (listing.IsBoosted)
                {
                    boostedCount++;
                }

                listing.boostLimitReached = boostedCount >= LocalSettings.BoostCountLimit;
            }
        }

        public void RefreshBoostCountLimitReached()
        {
            // Calculate date where listings cannot fall behind to be not boosted.
            DateTime boostThreshold = DateTime.Now.AddHours(-LocalSettings.BoostSleepTime);

            SqlConnection connection = new SqlConnection(LocalSettings.ConnectionString);
            SqlCommand command = new SqlCommand(
                "SELECT COUNT(ListingId) FROM Listings " +
                "WHERE BoostDate > @boostThreshold " +
                "AND UserId = @userId", connection);

            command.Parameters.AddWithValue("boostThreshold", boostThreshold);
            command.Parameters.AddWithValue("userId", sellerId);

            connection.Open();
            int boostedCount = (int)command.ExecuteScalar();
            connection.Close();

            this.boostLimitReached = boostedCount >= LocalSettings.BoostCountLimit;
        }

        public static implicit operator PublicListing(Listing listing)
        {
            return new PublicListing(
                listing.Title,
                listing.Details,
                listing.PriceValue,
                listing.PriceType,
                listing.CreateDate,
                listing.UpdateDate,
                listing.BoostDate,
                listing.NavigateUrl,
                listing.ThumbnailUrl,
                listing.SmallImageUrl,
                listing.FullImageUrl);
        }

        public void SetBoostDateManual(DateTime dateTime)
        {
            this.BoostDate = dateTime;
        }

        public WebsiteUser GetSeller(string connectionString)
        {
            return WebsiteUser.Fetch(sellerId, connectionString);
        }

        public List<Category> GetCategories(string connectionString)
        {
            return Category.Fetch(this, connectionString);
        }

        public int GetImageCount(string connectionString)
        {
            return ListingImage.Fetch(this, connectionString).Count;
        }

        public static void RemoveStaleRecycleBinItems(
            string connectionString, 
            PathUtility pathUtility,
            int staleDayCount)
        {
            List<Listing> listingList = Listing.FetchRecycleBinItems(
                connectionString, DateTime.Now.AddDays(-staleDayCount));

            foreach (Listing listing in listingList)
            {
                listing.RemoveFromRecycleBin(connectionString, pathUtility);
            }
        }
    }
}
