using System;
using System.Collections.Generic;
using System.Text;
using ManxAds.Search;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using System.Web;

namespace ManxAds
{
    public class ListingBase : IListingBase
    {
        private int databaseId;
        private string title;
        private string details;
        private TextType detailsType;
        private DateTime createDate;
        private DateTime updateDate;
        private DateTime boostDate;
        private bool expiryNotified;

        private ContextKeywordCollection searchHighlight = ContextKeywordCollection.Empty;

        protected int _DatabaseId
        {
            set { databaseId = value; }
        }

        public int DatabaseId
        {
            get { return databaseId; }
        }

        internal ContextKeywordCollection SearchHighlight
        {
            get { return searchHighlight; }
            set { searchHighlight = value; }
        }

        public string Title
        {
            get
            {
                if (searchHighlight.IsEnabled)
                {
                    return searchHighlight.HighlightContext(title);
                }

                return title;
            }
            set { title = StringTools.StripHtmlTags(value); }
        }

        /// <summary>
        /// Gets a title which can be placed in the URL to help search engines.
        /// </summary>
        public string SearchEngineTitle
        {
            get
            {
                StringBuilder result = new StringBuilder();
                foreach (char c in this.title)
                {
                    if (char.IsLetterOrDigit(c))
                    {
                        result.Append(c);
                    }
                    else if (char.IsWhiteSpace(c))
                    {
                        result.Append('-');
                    }
                }
                return result.ToString();
            }
        }

        public string GetSearchSimilarUrl(HttpServerUtility server)
        {
            string encodedTitle = server.UrlEncode(this.title);
            return String.Format(LocalSettings.SearchListingsUrlFormat, encodedTitle);
        }

        /// <summary>
        /// Gets or sets an unaltered version of the details.
        /// </summary>
        public string Details
        {
            get { return details; }
            set { details = value; }
        }

        /// <summary>
        /// Gets the details with all HTML tags removed.
        /// </summary>
        public string DetailsWithoutHtml
        {
            get { return StringTools.StripHtmlTags(details); }
        }

        /// <summary>
        /// Gets a version of details with public-unsafe content removed.
        /// </summary>
        public string DetailsCleanedWithoutHtml
        {
            get
            {
                return StringTools.MakeTextPublicSafe(
                    DetailsWithoutHtml, true);
            }
        }

        /// <summary>
        /// Gets a version of DetailsMasked which is viewable in
        /// an open context; HTML is not removed, but if DetailsType
        /// is set to PlainText, then HTML break characters are added
        /// to the end of all lines.
        /// </summary>
        public string DetailsFormatted
        {
            get
            {
                if (detailsType == TextType.PlainText)
                {
                    return StringTools.InsertHtmlBreaks(this.DetailsMasked);
                }
                return this.DetailsMasked;
            }
        }

        /// <summary>
        /// Gets a version of details with public-unsafe content masked.
        /// </summary>
        public string DetailsMasked
        {
            get { return StringTools.MakeTextPublicSafe(details, false); }
        }

        /// <summary>
        /// Gets or sets a value indicating the type of text used in the details field.
        /// HTML break tags are automatically added to the end of lines when PlainText
        /// mode is used. The text is returned unchanged when HTML mode is used.
        /// </summary>
        public TextType DetailsType
        {
            get { return detailsType; }
            set { detailsType = value; }
        }

        /// <summary>
        /// Gets the threshold to which the current date
        /// must exceed if the BoostDate property is to be set.
        /// </summary>
        public DateTime BoostDateThreshold
        {
            get
            {
                int sleepHours = LocalSettings.BoostSleepTime;
                return BoostDate.AddHours(sleepHours);
            }
        }

        public bool IsBoosted
        {
            get { return DateTime.Now <= BoostDateThreshold; }
        }

        public TimeSpan BoostLimitExpiry
        {
            get
            {
                long ticks = BoostDateThreshold.Ticks - DateTime.Now.Ticks;
                return new TimeSpan(ticks);
            }
        }

        public DateTime BoostDate
        {
            get { return boostDate; }
            set
            {
                if (IsBoosted)
                {
                    throw new ArgumentException(
                        "BoostSleepTime forbids this action for " +
                        BoostLimitExpiry.TotalHours + " more hours.");
                }
                boostDate = value;
            }
        }

        public DateTime CreateDate
        {
            get { return createDate; }
            set { createDate = value; }
        }

        public DateTime UpdateDate
        {
            get { return updateDate; }
            set { updateDate = value; }
        }

        /// <summary>
        /// Returns the concatenated short date and short time strings.
        /// </summary>
        public string CreateDateString
        {
            get
            {
                return CreateDate.ToShortDateString() + " " +
                    CreateDate.ToShortTimeString();
            }
        }

        public string FormattedCreateDate
        {
            get
            {
                if (CreateDate.Date == DateTime.Now.Date)
                {
                    return LocalSettings.FormattedDateToday;
                }

                return CreateDate.ToShortDateString();
            }
        }

        protected ListingBase() { }

        [Obsolete()]
        internal ListingBase(StoredProceedure sp)
            : this(
            sp.GetReaderValue<int>("ListingId"),
            sp.GetReaderValue<string>("Title"),
            sp.GetReaderValue<string>("Details"),
            sp.GetReaderValue<TextType>("DetailsType"),
            sp.GetReaderValue<DateTime>("CreateDate"),
            sp.GetReaderValue<DateTime>("UpdateDate"),
            sp.GetReaderValue<DateTime>("BoostDate")) { }

        internal static ListingBase Parse(StoredProceedure sp)
        {
            ListingBase listing = new ListingBase();
            listing.databaseId = sp.GetReaderValue<int>("ListingId");
            listing.title = sp.GetReaderValue<string>("Title");
            listing.details = sp.GetReaderValue<string>("Details");
            listing.detailsType = sp.GetReaderValue<TextType>("DetailsType");
            listing.createDate = sp.GetReaderValue<DateTime>("CreateDate");
            listing.updateDate = sp.GetReaderValue<DateTime>("UpdateDate");
            listing.boostDate = sp.GetReaderValue<DateTime>("BoostDate");
            listing.ExpiryNotified = sp.GetReaderValue<bool>("ExpiryNotified");
            return listing;
        }

        public ListingBase(int databaseId)
        {
            this.databaseId = databaseId;
        }

        public bool ExpiryNotified
        {
            get { return expiryNotified; }
            set { expiryNotified = value; }
        }

        public ListingBase(
            int databaseId, 
            string title, 
            string details, 
            TextType detailsType,
            DateTime createDate,
            DateTime updateDate,
            DateTime boostDate)
            : this(databaseId)
        {
            this.title = title;
            this.details = details;
            this.detailsType = detailsType;
            this.createDate = createDate;
            this.updateDate = updateDate;
            this.boostDate = boostDate;
        }

        public static List<IListingBase> Fetch()
        {
            return Fetch(LocalSettings.ConnectionString);
        }

        public static List<IListingBase> Fetch(string connectionString)
        {
            List<IListingBase> listingList = new List<IListingBase>();
            using (StoredProceedure sp = new StoredProceedure("ListingFetchBase", connectionString))
            {
                sp.Connection.Open();
                sp.Reader = sp.Command.ExecuteReader();

                while (sp.Reader.Read())
                {
                    listingList.Add(ListingBase.Parse(sp));
                }
            }
            return listingList;
        }

        public static List<IListingBase> FetchNoBan(string connectionString)
        {
            List<IListingBase> listingList = new List<IListingBase>();
            using (StoredProceedure sp = new StoredProceedure("ListingFetchBaseNoBan", connectionString))
            {
                sp.Connection.Open();
                sp.Reader = sp.Command.ExecuteReader();

                while (sp.Reader.Read())
                {
                    listingList.Add(ListingBase.Parse(sp));
                }
            }
            return listingList;
        }

        public void SetExpiryNotified(string connectionString)
        {
            using (StoredProceedure sp = new StoredProceedure("ListingExpiryNotified", connectionString))
            {
                sp.AddParam("@ListingId", this.DatabaseId);

                sp.Connection.Open();
                sp.Command.ExecuteNonQuery();
            }
        }
    }
}
