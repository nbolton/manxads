using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Drawing;

namespace ManxAds
{
    public class LocalSettings
    {
        protected static Dictionary<string, string> MergedSettings =
            new Dictionary<string, string>();

        protected static Dictionary<string, string> UserSettings;
        protected static Dictionary<string, string> SiteWideSettings;

        #region Default settings
        public class Default
        {
            public const int BoostSleepTime = 24;
            public const int BoostCountLimit = 3;
            public const bool ListingDetailsPopup = false;
            public const string BadWordsList = "contoso, fabrikam";
            public const bool EnableAdverts = true;
            public const int UserListingLimit = 10;
            public const int TraderListingLimit = 25;
            public const bool AdvertDebug = false;
            public const int WantedCategoryId = 12;
            public const int WelcomeTopListingsLimit = 4;
            public const int WelcomeTopCategoriesLimit = 10;
            public const string ManxAdsIdPrefix = "ma";
            public const string CategoriesZeroListingCount = "None";
            public const string FormattedDateToday = "Today";
            public const string CategoriesLatestListingNever = "Never";
            public const int VerificationTimeoutDays = 7;
            public const int MaximumCategoryCount = -1;
        }
        #endregion

        #region Unchangable settings

        public static List<String> ListingImageTypes
        {
            get { return new List<string>(ListingImageTypesString.Split(',')); }
        }

        public const string EmailLogPath = @"~/Logs/SentEmail.log";

        public const string EmailAddressRegex = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
        public const string ListingEmailReplaceString = "(<em>use 'Contact seller' link</em>)";

        public const string ListingImageTypesString = "png,gif,jpg,jpeg,tiff,bmp";
        public const string TraderLogoPlaceholder = "~/Images/Static/Layout/TraderLogoPlaceholder.png";

        public const float ListingShortTitlePixelLimit = 190f;
        public const float ListingShortTitlePixelSize = 1f;

        public const int SearchQueryWordLimit = 20;

        public const int TraderSearchSegmentCount = 2;
        public const int SellerSearchSegmentCount = 3;

        public const string HelpDocumentsDirectory = "~/Help/";
        public const string HelpDocumentsPathFormat = "~/Help/{0}.xml";
        public const string HelpUserControlPathFormat = "~/Help/{0}.ascx";

        public const string DefaultLandlineArea = "01624";
        public const string DefaultMobileArea = "07624";

        public const string TraderLogoPathFormat = "~/Images/Dynamic/Trader/Logo-{0}.{1}";
        public const string TraderLogoForceExtension = "png";
        public const int TraderLogoWidth = 110;
        public const int TraderLogoHeight = 30;

        /// <summary>
        /// 0: ManxAds ID. 1: Listing title.
        /// </summary>
        public const string ListingContactFormat = "{1} ({0})";
        public static string ManxAdsIdFormat = Default.ManxAdsIdPrefix + "{0}";

        public const string WordHighlightFormat = "<b>{0}</b>";

        public const int BrowserRowPopupWidth = 600;
        public const int BrowserRowPopupHeight = 450;

        public const int DefaultPageRange = 10;
        public const int DefaultPageMaximum = 5000;
        public const int DefaultPageLimit = 10;
        public const int ListingThumbnailWidth = 50;
        public const int ListingThumbnailHeight = 50;
        public const int ListingSmallImageWidth = 110;
        public const int ListingSmallImageHeight = 110;
        public const int ListingFullImageWidth = 440;
        public const int ListingFullImageHeight = 440;
        public const int CategoryThumbnailWidth = 50;
        public const int CategoryThumbnailHeight = 50;

        public const string WebsiteUrl = "http://www.manxads.com";
        public const string ListingViewLinkFormat2 = "{0}/?Title={1}&ListingId={2}";
        public const string ListingViewLinkNoTitleFormat2 = "{0}/?ListingId={1}";
        public const string ListingUpdateLinkFormat2 = "{0}/ListingModify.aspx?Listing={1}";
        public const string ListingDeleteLinkFormat2 = "{0}/ListingModify.aspx?Listing={1}&Remove=1";
        public const string UserBanLinkFormat = "{0}/UserModify.aspx?UserId={1}&Ban=1";

        public const string SellerListingsUrlFormat =
            "~/Listings.aspx?Seller={0}";

        public const string SearchListingsUrlFormat =
            "~/Listings.aspx?Search={0}&Any=1";

        public const string ListingModifyNewRedirect =
            "~/Listings.aspx?Self=1";

        public const string AdvertModifyNewRedirect =
            "~/AdvertBrowser.aspx?Self=1";

        public const string UserModifyRedirect =
            "~/UserBrowser.aspx";

        public const string CategoryModifyNewRedirect =
            "~/Categories.aspx?Self=1";

        public const string CategoryModifyUrlFormat =
            "~/CategoryModify.aspx?Category={0}";

        public const string CategoryRemoveUrlFormat =
            "~/CategoryModify.aspx?Category={0}&Remove=1";

        public const string ListingModifyUrlFormat =
            "~/ListingModify.aspx?Listing={0}";

        public const string ListingRemoveUrlFormat =
            "~/ListingModify.aspx?Listing={0}&Remove=1";

        public const string AdvertModifyUrlFormat =
            "~/AdvertModify.aspx?Advert={0}";

        public const string AdvertRemoveUrlFormat =
            "~/AdvertModify.aspx?Advert={0}&Remove=1";

        public const string AdvertRedirectUrlFormat =
            "~/AdvertRedirect.aspx?AdvertId={0}";

        public const string ListingPricePeniesFormat = "{0:G}p";

        public const string ListingPriceFormat = "C";

        public const string ListingPriceFreeText = "Free!";

        public static char[] ShortenTrimChars = new char[] {
            ' ', ',', '\'', '.', '-', '*', '&'
        };

        public const string ListingColouredPriceFormat =
            "<span class=\"{0}\">{1}</span>";

        public const int CategoryTitleLimit = 20;

        /// <summary>
        /// The limit to which listing titles can be typed (in the listings editor).
        /// </summary>
        public const int ListingTitleLimit = 35;

        /// <summary>
        /// Number of listings loaded into the (paged) top listings page.
        /// </summary>
        public const int TopListingsLimit = 150;

        /// <summary>
        /// Number of characters to trim to for listing titles on the welcome page.
        /// </summary>
        [Obsolete("Title is now shortened by pixels.")]
        public const int ListingShortTitleLimit = 26;

        /// <summary>
        /// Number of characters to trim to for listing details on the welcome page.
        /// </summary>
        public const int ListingShortDetailsLimit = 70;

        /// <summary>
        /// Number of characters to trim to for listing details belonging to sellers on normal pages.
        /// </summary>
        public const int ListingLongDetailsLimit = 120;

        /// <summary>
        /// Number of characters to trim to for listing details belonging to traders on normal pages.
        /// </summary>
        public const int ListingTraderDetailsLimit = 80;

        public const string PlaceHolderThumbnailPath =
            "~/Images/Static/Layout/PlaceholderThumbnail.gif";

        public const string PageDescriptionsPath = "~/PageDescriptions.xml";

        public const string ListingFullImageUrlFormat =
            "~/Images/Dynamic/Listing/Full-{0}-{1}.jpg";

        public const string ListingSmallImageUrlFormat =
            "~/Images/Dynamic/Listing/Small-{0}-{1}.jpg";

        public const string ListingThumbnailUrlFormat =
            "~/Images/Dynamic/Listing/Thumb-{0}-{1}.gif";

        public const string ListingInlineNavigateUrlFormat =
            "~/?Title={0}&ListingId={1}";

        public const string ListingPopupNavigateUrlFormat =
            "~/ListingDetailsPopup.aspx?Title={0}&Listing={1}";

        public const string ListingInlineContactUrlFormat =
            "~/ContactFormInline.aspx?Listing={0}";

        public const string ListingPopupContactUrlFormat =
            "~/ContactFormPopup.aspx?Listing={0}";

        public const string CategoryNavigateUrlFormat =
            "~/Listings.aspx?Title={0}&CategoryId={1}";

        public const string FlashAdvertUrlFormat =
            "~/FlashAdvert.aspx?Advert={0}&LayerId={1}";

        public const string CategoryImageUrlFormat =
            "~/Images/Dynamic/Category/{0}.gif";

        public const string AdvertMediaUrlFormat =
            "~/Images/Dynamic/Advert/{0}.{1}";

        public const string UserModifyUrlFormat =
            "~/UserModify.aspx?User={0}";

        public const string UserRemoveUrlFormat =
            "~/UserModify.aspx?User={0}&Remove=1";

        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager.
                    ConnectionStrings["Pimpmaster"].ConnectionString;
            }
        }

        public static string WebmasterEmail
        {
            get
            {
                return ConfigurationManager.
                    AppSettings["WebmasterEmail"];
            }
        }

        public static string MasterSendFromEmail
        {
            get
            {
                return ConfigurationManager.
                    AppSettings["MasterSendFromEmail"];
            }
        }

        public static string MasterSmtpUsername
        {
            get
            {
                return ConfigurationManager.
                    AppSettings["MasterSmtpUsername"];
            }
        }

        public static string MasterSmtpPassword
        {
            get
            {
                return ConfigurationManager.
                    AppSettings["MasterSmtpPassword"];
            }
        }

        public static string MasterSmtpServer
        {
            get
            {
                return ConfigurationManager.
                    AppSettings["MasterSmtpServer"];
            }
        }

        public static string ErrorReportingRecipient
        {
            get
            {
                return ConfigurationManager.
                    AppSettings["ErrorReportingRecipient"];
            }
        }

        public static string AdvertRequestEmail
        {
            get
            {
                return ConfigurationManager.
                    AppSettings["AdvertRequestEmail"];
            }
        }

        public static string CharityRequestEmail
        {
            get
            {
                return ConfigurationManager.
                    AppSettings["CharityRequestEmail"];
            }
        }

        public static string TraderRequestEmail
        {
            get
            {
                return ConfigurationManager.
                    AppSettings["TraderRequestEmail"];
            }
        }

        #endregion

        #region Dynamic settings

        public static int MaximumCategoryCount
        {
            get
            {
                return FetchMerged<int>("MaximumCategoryCount", Default.MaximumCategoryCount);
            }
        }

        public static int BoostCountLimit
        {
            get
            {
                return FetchMerged<int>("BoostCountLimit", Default.BoostCountLimit);
            }
        }

        public static int BoostSleepTime
        {
            get
            {
                return FetchMerged<int>("BoostSleepTime", Default.BoostSleepTime);
            }
        }

        public static int VerificationTimeoutDays
        {
            get
            {
                return FetchMerged<int>("VerificationTimeoutDays", Default.VerificationTimeoutDays);
            }
        }

        public static bool ListingDetailsPopup
        {
            get
            {
                return FetchMerged<bool>("ListingDetailsPopup", Default.ListingDetailsPopup);
            }
        }

        public static string ManxAdsIdPrefix
        {
            get
            {
                return FetchMerged<string>("ManxAdsIdPrefix", Default.ManxAdsIdPrefix);
            }
        }

        public static List<string> BadWordsList
        {
            get
            {
                string raw = FetchMerged<string>(
                    "BadWordsList", Default.BadWordsList);

                string[] split = raw.Split(',');
                List<string> newList = new List<string>();
                foreach (string word in split)
                {
                    newList.Add(word.Trim());
                }
                return newList;
            }
        }

        public static bool EnableAdverts
        {
            get
            {
                return FetchMerged<bool>("EnableAdverts", Default.EnableAdverts);
            }
        }

        public static int UserListingLimit
        {
            get
            {
                return FetchMerged<int>("UserListingLimit", Default.UserListingLimit);
            }
        }

        public static int TraderListingLimit
        {
            get
            {
                return FetchMerged<int>("TraderListingLimit", Default.TraderListingLimit);
            }
        }

        public static bool AdvertDebug
        {
            get
            {
                return FetchMerged<bool>("AdvertDebug", Default.AdvertDebug);
            }
        }

        [Obsolete("Cannot guarentee the value's validity")]
        public static int WantedCategoryId
        {
            get
            {
                return FetchMerged<int>(
                    "WantedCategoryId", Default.WantedCategoryId);
            }
        }

        public static string FormattedDateToday
        {
            get
            {
                return FetchMerged<string>(
                    "FormattedDateToday",
                    Default.FormattedDateToday);
            }
        }

        public static int WelcomeTopListingsLimit
        {
            get
            {
                return FetchMerged<int>(
                    "WelcomeTopListingsLimit",
                    Default.WelcomeTopListingsLimit);
            }
        }

        public static int WelcomeTopCategoriesLimit
        {
            get
            {
                return FetchMerged<int>(
                    "WelcomeTopCategoriesLimit",
                    Default.WelcomeTopCategoriesLimit);
            }
        }

        public static string CategoriesZeroListingCount
        {
            get
            {
                return FetchMerged<string>(
                    "CategoriesZeroListingCount",
                    Default.CategoriesZeroListingCount);
            }
        }

        public static string CategoriesLatestListingNever
        {
            get
            {
                return FetchMerged<string>(
                    "CategoriesLatestListingNever",
                    Default.CategoriesLatestListingNever);
            }
        }

        #endregion

        #region Methods

        public static void Populate()
        {
            // Fetch the default settings.
            SiteWideSettings = Fetch(new Dictionary<string, string>());
            MergedSettings = Fetch(MergedSettings);
        }

        public static void Populate(WebsiteUser user)
        {
            Populate();

            // Then overide with the user settings.
            UserSettings = Fetch(new Dictionary<string, string>(), user);
            MergedSettings = Fetch(MergedSettings, user);
        }

        /// <summary>
        /// Fetches a user-specific setting where the user paramater is not null. If
        /// the user parameter is null, then the site-wide settings are used. If the
        /// key is not found in the user's settings database table, then the default
        /// value provided is retuened.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static T Fetch<T>(string key, T defaultValue, WebsiteUser user)
        {
            Dictionary<string, string> dictionary;
            if (user != null)
            {
                // Fetch merged user and site-wide settings.
                dictionary = Fetch(SiteWideSettings, user);
            }
            else
            {
                dictionary = SiteWideSettings;
            }

            if (dictionary.ContainsKey(key) && (dictionary[key] != null))
            {
                if (typeof(T) == typeof(int))
                {
                    return (T)(object)int.Parse(dictionary[key]);
                }

                if (typeof(T) == typeof(bool))
                {
                    return (T)(object)bool.Parse(dictionary[key]);
                }

                return (T)(object)dictionary[key];
            }

            return defaultValue;
        }

        /// <summary>
        /// Fetches the site-wide settings for a particular key. If no key
        /// is found in the site-wide settings database table, then the provided
        /// default value is then returned.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T Fetch<T>(string key, T defaultValue)
        {
            return Fetch<T>(key, defaultValue, null);
        }

        protected static T FetchMerged<T>(string key, object defaultValue)
        {
            string databaseValue = null;
            if ((MergedSettings != null) && MergedSettings.ContainsKey(key))
            {
                databaseValue = MergedSettings[key];
            }

            if (databaseValue == null)
            {
                return (T)defaultValue;
            }

            if (typeof(T) == typeof(int))
            {
                return (T)(object)int.Parse(databaseValue);
            }

            if (typeof(T) == typeof(bool))
            {
                return (T)(object)bool.Parse(databaseValue);
            }

            return (T)(object)databaseValue;
        }

        /// <summary>
        /// Fetches all site-wide settings.
        /// </summary>
        /// <returns></returns>
        protected static Dictionary<string, string> Fetch(
            Dictionary<string, string> settings)
        {
            return Fetch(settings, null);
        }

        /// <summary>
        /// Fetches user specific settings for a user.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected static Dictionary<string, string> Fetch(
            Dictionary<string, string> settings, WebsiteUser user)
        {
            string key;
            string value;

            string spName = "SettingsFetch";
            if (user != null)
            {
                spName = "SettingsFetchByUserId";
            }

            using (StoredProceedure sp = new StoredProceedure(spName))
            {
                if (user != null)
                {
                    sp.AddParam("@UserId", user.DatabaseId);
                }

                sp.Connection.Open();
                sp.Reader = sp.Command.ExecuteReader();

                while (sp.Reader.Read())
                {
                    key = sp.GetReaderValue<string>("KeyName");
                    value = sp.GetReaderValue<string>("KeyValue");
                    settings[key] = value;
                }
            }

            return settings;
        }

        public static bool ModifySetting<T>(
            string key,
            object newValue,
            object defaultValue)
        {
            return ModifySetting<T>(
                key, newValue, defaultValue, null);
        }

        public static bool ModifySetting<T>(
            string key,
            object newValue,
            object defaultValue,
            WebsiteUser user)
        {
            object currentValue = null;
            if (user == null)
            {
                currentValue = Fetch<T>(key, (T)defaultValue);
            }
            else
            {
                currentValue = Fetch<T>(key, (T)defaultValue, user);
            }

            if (newValue.ToString() == currentValue.ToString())
            {
                // No action needed when no change.
                return false;
            }

            if (newValue.ToString() == defaultValue.ToString())
            {
                // TODO: Remove database key.
            }

            using (StoredProceedure sp = new StoredProceedure("SettingsModify"))
            {
                if (user != null)
                {
                    sp.AddParam("@UserId", user.DatabaseId);
                }
                else
                {
                    sp.AddParam("@UserId", -1);
                }

                sp.AddParam("@KeyName", key.ToString());
                sp.AddParam("@KeyValue", newValue.ToString());

                sp.Connection.Open();
                sp.Command.ExecuteNonQuery();
            }

            return true;
        }

        #endregion
    }
}
