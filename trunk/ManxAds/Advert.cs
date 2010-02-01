using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;
using System.Data;
using System.Web.UI;

namespace ManxAds
{
    public class Advert : IEquatable<Advert>
    {
        private int databaseId;
        private int advertiserId;
        private AdvertSizeType sizeType;
        private AdvertFormatType formatType;
        private AdvertPositionType positionType;
        private string title;
        private bool siteWide;
        private int rotateFrequency;
        private WebsiteUser advertiser;
        private string _hyperlink;
        private bool authorised;

        public int HitsMonth { get; set; }
        public int HitsTotal { get; set; }
        public string Html { get; set; }

        public string HitsMonthString
        {
            get { return HitsSupported ? HitsMonth.ToString() : "-"; }
        }

        public string HitsTotalString
        {
            get { return HitsSupported ? HitsTotal.ToString() : "-"; }
        }

        public bool HitsSupported
        {
            get
            {
                return (formatType != AdvertFormatType.Html) &&
                    (formatType != AdvertFormatType.Flash);
            }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        private string hyperlink
        {
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    if (!value.Contains("http://"))
                    {
                        value = "http://" + value;
                    }
                    _hyperlink = value;
                }
            }
            get { return _hyperlink; }
        }

        public int DatabaseId
        {
            get { return databaseId; }
        }

        public string AdvertiserString
        {
            get
            {
                return Advertiser.FullName;
            }
        }

        public string PositionTypeString
        {
            get
            {
                switch (positionType)
                {
                    case AdvertPositionType.Top:
                        return "Top";

                    case AdvertPositionType.Bottom:
                        return "Bottom";

                    case AdvertPositionType.Right:
                        return "Right";

                    case AdvertPositionType.Left1:
                        return "Left 1";

                    case AdvertPositionType.Left2:
                        return "Left 2";

                    case AdvertPositionType.Left3:
                        return "Left 3";

                    case AdvertPositionType.Left4:
                        return "Left 4";

                    case AdvertPositionType.Left5:
                        return "Left 5";

                    case AdvertPositionType.Left6:
                        return "Left 6";

                    case AdvertPositionType.RandomLeaderboard:
                        return "Random (Leaderboard)";

                    case AdvertPositionType.RandomSquareButton:
                        return "Random (Square Button)";

                    default:
                        throw new NotSupportedException();
                }
            }
        }

        public string DimensionsSummary
        {
            get { return Width + " x " + Height; }
        }

        public string SizeTypeString
        {
            get
            {
                switch (sizeType)
                {
                    case AdvertSizeType.FullBanner:
                        return "Full Banner";

                    case AdvertSizeType.Leaderboard:
                        return "Leaderboard";

                    case AdvertSizeType.Skyscraper:
                        return "Skyscraper";

                    case AdvertSizeType.SquareButton:
                        return "Square Button";

                    default:
                        throw new NotSupportedException();
                }
            }
        }

        public string ModifyUrl
        {
            get { return String.Format(LocalSettings.AdvertModifyUrlFormat, databaseId); }
        }

        public string RemoveUrl
        {
            get { return String.Format(LocalSettings.AdvertRemoveUrlFormat, databaseId); }
        }
    
        /// <summary>
        /// Gets the owning advertiser for this advert.
        /// </summary>
        public WebsiteUser Advertiser
        {
            get
            {
                if (advertiser == null)
                {
                    advertiser = WebsiteUser.Fetch(advertiserId);
                }
                return advertiser;
            }
        }

        /// <summary>
        /// Gets the categories in which this advert is displayed.
        /// </summary>
        public List<CategoryAdvert> CategoryAdverts
        {
            get { return CategoryAdvert.Fetch(this); }
        }

        /// <summary>
        /// Gets the size type of the advert.
        /// </summary>
        public AdvertSizeType SizeType
        {
            get { return sizeType; }
            set { sizeType = value; }
        }

        /// <summary>
        /// Gets the format type of the advert.
        /// </summary>
        public AdvertFormatType FormatType
        {
            get { return formatType; }
            set { formatType = value; }
        }

        /// <summary>
        /// Gets the position type of the advert.
        /// </summary>
        public AdvertPositionType PositionType
        {
            get { return positionType; }
            set { positionType = value; }
        }

        /// <summary>
        /// Gets the extension of the media file name. This does not
        /// include a prefixed period.
        /// </summary>
        public string MediaUrlExtension
        {
            get
            {
                switch (formatType)
                {
                    case AdvertFormatType.Flash:
                        return "swf";

                    case AdvertFormatType.Gif:
                        return "gif";

                    case AdvertFormatType.Jpeg:
                        return "jpg";

                    case AdvertFormatType.Png:
                        return "png";

                    default:
                        throw new NotSupportedException(
                            "File extension not supported.");
                }
            }
        }

        public bool MediaUrlSupported
        {
            // if we use html snippet, assume no media url
            get { return (FormatType != AdvertFormatType.Html); }
        }

        public string NavigateUrl
        {
            get { return Hyperlink; }
        }

        public string RedirectUrl 
        {
            get { return String.Format(LocalSettings.AdvertRedirectUrlFormat, databaseId); }
        }

        public string MediaUrl
        {
            get
            {
                return String.Format(
                    LocalSettings.AdvertMediaUrlFormat,
                    databaseId, this.MediaUrlExtension);
            }
        }

        public int Width
        {
            get
            {
                switch (sizeType)
                {
                    case AdvertSizeType.Leaderboard:
                        return 728;

                    case AdvertSizeType.FullBanner:
                        return 468;

                    case AdvertSizeType.SquareButton:
                        return 125;

                    case AdvertSizeType.Skyscraper:
                        return 120;

                    default:
                        throw new NotSupportedException(
                            "Advert size type not supported.");
                }
            }
        }

        public int Height
        {
            get
            {
                switch (sizeType)
                {
                    case AdvertSizeType.Leaderboard:
                        return 90;

                    case AdvertSizeType.FullBanner:
                        return 60;

                    case AdvertSizeType.SquareButton:
                        return 125;

                    case AdvertSizeType.Skyscraper:
                        return 600;

                    default:
                        throw new NotSupportedException(
                            "Advert size type not supported.");
                }
            }
        }

        public bool SiteWide
        {
            get { return siteWide; }
            set { siteWide = value; }
        }

        public int RotateFrequency
        {
            get { return rotateFrequency; }
            set { rotateFrequency = value; }
        }

        public string Hyperlink
        {
            get { return hyperlink; }
            set { hyperlink = value; }
        }

        public bool Authorised
        {
            get { return authorised; }
            set { authorised = value; }
        }

        public string AuthorisedString
        {
            get
            {
                if (authorised)
                {
                    return "Authorised";
                }

                return "Unauthorised";
            }
        }

        protected Advert() { }
        protected Advert(int databaseId)
        {
            this.databaseId = databaseId;
        }

        public Advert(
            int advertiserId,
            AdvertSizeType sizeType,
            AdvertFormatType formatType,
            AdvertPositionType positionType,
            string title,
            bool siteWide,
            int rotateFrequency,
            string hyperlink,
            bool authorised,
            string html)
        {
            this.advertiserId = advertiserId;
            this.sizeType = sizeType;
            this.formatType = formatType;
            this.positionType = positionType;
            this.title = title;
            this.siteWide = siteWide;
            this.rotateFrequency = rotateFrequency;
            this.hyperlink = hyperlink;
            this.authorised = authorised;
            this.Html = html;
        }

        internal Advert(
            int databaseId,
            int advertiserId,
            AdvertSizeType sizeType,
            AdvertFormatType formatType,
            AdvertPositionType positionType,
            string title,
            bool siteWide,
            int rotateFrequency,
            string hyperlink,
            bool authorised,
            string html)
            : this(
            advertiserId,
            sizeType,
            formatType,
            positionType,
            title,
            siteWide,
            rotateFrequency,
            hyperlink,
            authorised,
            html)
        {
            this.databaseId = databaseId;
        }

        //[Obsolete()]
        //internal Advert(StoredProceedure sp, int databaseId)
        //    : this(
        //    databaseId,
        //    sp.GetReaderValue<int>("AdvertiserId"),
        //    sp.GetReaderValue<AdvertSizeType>("SizeType"),
        //    sp.GetReaderValue<AdvertFormatType>("FormatType"),
        //    sp.GetReaderValue<AdvertPositionType>("PositionType"),
        //    sp.GetReaderValue<string>("Title"),
        //    sp.GetReaderValue<bool>("SiteWide"),
        //    sp.GetReaderValue<int>("RotateFrequency"),
        //    sp.GetReaderValue<string>("Hyperlink"),
        //    sp.GetReaderValue<bool>("Authorised")) { }

        //[Obsolete()]
        //internal Advert(StoredProceedure sp)
        //    : this(sp, sp.GetReaderValue<int>("AdvertId")) { }

        internal static Advert Parse(StoredProceedure sp)
        {
            Advert advert = new Advert();
            advert.databaseId = sp.GetReaderValue<int>("AdvertId");
            advert.advertiserId = sp.GetReaderValue<int>("AdvertiserId");
            advert.sizeType = sp.GetReaderValue<AdvertSizeType>("SizeType");
            advert.formatType = sp.GetReaderValue<AdvertFormatType>("FormatType");
            advert.positionType = sp.GetReaderValue<AdvertPositionType>("PositionType");
            advert.title = sp.GetReaderValue<string>("Title");
            advert.siteWide = sp.GetReaderValue<bool>("SiteWide");
            advert.rotateFrequency = sp.GetReaderValue<int>("RotateFrequency");
            advert.hyperlink = sp.GetReaderValue<string>("Hyperlink");
            advert.authorised = sp.GetReaderValue<bool>("Authorised");
            advert.HitsMonth = sp.GetReaderValue<int>("HitsMonth");
            advert.HitsTotal = sp.GetReaderValue<int>("HitsTotal");
            advert.Html = sp.GetReaderValue<string>("Html");
            return advert;
        }

        public void Create()
        {
            using (StoredProceedure sp = new StoredProceedure("AdvertCreate"))
            {
                sp.AddParam("@AdvertiserId", this.advertiserId);
                sp.AddParam("@SizeType", this.sizeType);
                sp.AddParam("@FormatType", this.formatType);
                sp.AddParam("@PositionType", this.positionType);
                sp.AddParam("@Title", this.title);
                sp.AddParam("@SiteWide", this.siteWide);
                sp.AddParam("@RotateFrequency", this.rotateFrequency);
                sp.AddParam("@Hyperlink", this.hyperlink);
                sp.AddParam("@Authorised", this.authorised);
                sp.AddParam("@Html", this.Html);
                sp.AddParam("@Insertid", System.Data.SqlDbType.Int);

                sp.Connection.Open();
                sp.Command.ExecuteNonQuery();

                this.databaseId = sp.GetParamValue<int>("@InsertId");
            }
        }

        public void Modify()
        {
            using (StoredProceedure sp = new StoredProceedure("AdvertModify"))
            {
                sp.AddParam("@AdvertId", this.databaseId);
                sp.AddParam("@AdvertiserId", this.advertiserId);
                sp.AddParam("@SizeType", this.sizeType);
                sp.AddParam("@FormatType", this.formatType);
                sp.AddParam("@PositionType", this.positionType);
                sp.AddParam("@Title", this.title);
                sp.AddParam("@SiteWide", this.siteWide);
                sp.AddParam("@RotateFrequency", this.rotateFrequency);
                sp.AddParam("@Hyperlink", this.hyperlink);
                sp.AddParam("@Authorised", this.authorised);
                sp.AddParam("@Html", this.Html);

                sp.Connection.Open();
                sp.Command.ExecuteNonQuery();
            }
        }

        public void Remove(HttpServerUtility server)
        {
            if ((server != null) && (FormatType != AdvertFormatType.Html))
            {
                // Delete the media.
                File.Delete(server.MapPath(this.MediaUrl));
            }

            using (StoredProceedure sp = new StoredProceedure("AdvertRemove"))
            {
                sp.AddParam("@AdvertId", this.databaseId);
                sp.Connection.Open();
                sp.Command.ExecuteNonQuery();
            }
        }

        public int AssociateWithCategory(int categoryId, int rotateFrequency)
        {
            using (StoredProceedure sp = new StoredProceedure("CategoryAdvertAssociate"))
            {
                sp.AddParam("@AdvertId", this.databaseId);
                sp.AddParam("@CategoryId", categoryId);
                sp.AddParam("@RotateFrequency", rotateFrequency);
                sp.AddParam("@Insertid", System.Data.SqlDbType.Int);

                sp.Connection.Open();
                sp.Command.ExecuteNonQuery();

                return sp.GetParamValue<int>("@InsertId");
            }
        }

        public void DisassociateWithCategory(int categoryAdvertId)
        {
            using (StoredProceedure sp = new StoredProceedure("CategoryAdvertDisassociate"))
            {
                sp.AddParam("@CategoryAdvertId", categoryAdvertId);
                sp.Connection.Open();
                sp.Command.ExecuteNonQuery();
            }
        }

        public static List<Advert> Fetch()
        {
            List<Advert> advertList = new List<Advert>();
            using (StoredProceedure sp = new StoredProceedure("AdvertFetch"))
            {
                sp.Connection.Open();
                sp.Reader = sp.Command.ExecuteReader();

                while (sp.Reader.Read())
                {
                    advertList.Add(Advert.Parse(sp));
                }
            }
            return advertList;
        }

        public static Advert Fetch(int databaseId)
        {
            using (StoredProceedure sp = new StoredProceedure("AdvertFetchById"))
            {
                try
                {
                    sp.AddParam("@AdvertId", databaseId);
                    sp.Connection.Open();
                    sp.Reader = sp.Command.ExecuteReader(CommandBehavior.SingleRow);
                    sp.Reader.Read();

                    return Advert.Parse(sp);
                }
                catch (CannotReadException ex)
                {
                    throw new NotFoundException(databaseId, ex);
                }
            }
        }

        public static List<Advert> Fetch(WebsiteUser advertiser)
        {
            List<Advert> advertList = new List<Advert>();
            using (StoredProceedure sp = new StoredProceedure("AdvertFetchByAdvertiserId"))
            {
                sp.AddParam("@AdvertiserId", advertiser.DatabaseId);
                sp.Connection.Open();
                sp.Reader = sp.Command.ExecuteReader();

                while (sp.Reader.Read())
                {
                    advertList.Add(Advert.Parse(sp));
                }
            }
            return advertList;
        }

        public static List<Advert> Fetch(AdvertPositionType position)
        {
            List<Advert> advertList = new List<Advert>();
            using (StoredProceedure sp = new StoredProceedure("AdvertFetchByPosition"))
            {
                sp.AddParam("@PositionType", position);
                sp.Connection.Open();
                sp.Reader = sp.Command.ExecuteReader();

                while (sp.Reader.Read())
                {
                    advertList.Add(Advert.Parse(sp));
                }
            }
            return advertList;
        }

        /// <summary>
        /// Running 2 SQL queries, fetches the site-wide adverts,
        /// with the additional category-specific adverts.
        /// </summary>
        /// <param name="category">Category-specific specifier.</param>
        /// <param name="position">Position on the page.</param>
        /// <returns>Merged set of site-wide and category-specific ads.</returns>
        public static List<Advert> Fetch(
            Category category, AdvertPositionType position)
        {
            // Fetch the initial site-wide adverts.
            List<Advert> advertList = Advert.Fetch(position);

            // Convert list to dictionary to check for duplicates.
            Dictionary<int, Advert> dictionary = new Dictionary<int, Advert>();
            foreach (Advert advert in advertList)
            {
                dictionary.Add(advert.databaseId, advert);
            }

            using (StoredProceedure sp = new StoredProceedure("AdvertFetchByCategoryId"))
            {
                sp.AddParam("@PositionType", position);
                sp.AddParam("@CategoryId", category.DatabaseId);
                sp.Connection.Open();
                sp.Reader = sp.Command.ExecuteReader();

                while (sp.Reader.Read())
                {
                    int tempAdvertId = sp.GetReaderValue<int>("AdvertId");
                    Advert tempAdvert = Advert.Parse(sp);

                    if (dictionary.ContainsKey(tempAdvertId))
                    {
                        dictionary[tempAdvertId] = tempAdvert;
                    }
                    else
                    {
                        dictionary.Add(tempAdvertId, tempAdvert);
                    }
                }
            }

            // Createa new advert list with possible merges.
            List<Advert> appendedAdvertList = new List<Advert>();
            foreach (KeyValuePair<int, Advert> kvp in dictionary)
            {
                appendedAdvertList.Add(kvp.Value);
            }

            return appendedAdvertList;
        }

        /// <summary>
        /// Running 2 SQL queries, fetches the site-wide adverts,
        /// with the additional listing-category-specific adverts.
        /// </summary>
        /// <param name="category">Listing-specific specifier.</param>
        /// <param name="position">Position on the page.</param>
        /// <returns>Merged set of site-wide and listing-specific ads.</returns>
        public static List<Advert> Fetch(
            Listing listing, AdvertPositionType position)
        {
            // Fetch the initial site-wide adverts.
            List<Advert> advertList = Advert.Fetch(position);

            // Convert list to dictionary to check for duplicates.
            Dictionary<int, Advert> dictionary = new Dictionary<int, Advert>();
            foreach (Advert advert in advertList)
            {
                dictionary.Add(advert.databaseId, advert);
            }

            using (StoredProceedure sp = new StoredProceedure("AdvertFetchByListingId"))
            {
                sp.AddParam("@PositionType", position);
                sp.AddParam("@ListingId", listing.DatabaseId);
                sp.Connection.Open();
                sp.Reader = sp.Command.ExecuteReader();

                while (sp.Reader.Read())
                {
                    int tempAdvertId = sp.GetReaderValue<int>("AdvertId");
                    Advert tempAdvert = Advert.Parse(sp);

                    if (dictionary.ContainsKey(tempAdvertId))
                    {
                        dictionary[tempAdvertId] = tempAdvert;
                    }
                    else
                    {
                        dictionary.Add(tempAdvertId, tempAdvert);
                    }
                }
            }

            // Createa new advert list with possible merges.
            List<Advert> appendedAdvertList = new List<Advert>();
            foreach (KeyValuePair<int, Advert> kvp in dictionary)
            {
                appendedAdvertList.Add(kvp.Value);
            }

            return appendedAdvertList;
        }
        
        public bool Equals(Advert other)
        {
            if (other.databaseId == this.databaseId)
            {
                return true;
            }

            return false;
        }

        public bool AuthoriseEdit(WebsiteUser user)
        {
            if ((user.UserType & WebsiteUserType.AdministratorOnly) != 0)
            {
                return true;
            }

            if (user.DatabaseId == this.advertiserId)
            {
                return true;
            }

            return false;
        }

        public string ToJavaScript(Page caller)
        {
            return "document.write('" +
                "<object type=\"application/x-shockwave-flash\" " +
                "id=\"" + caller.Request.QueryString["LayerId"] + "_Flash\" " +
                "data=\"" + caller.ResolveUrl(MediaUrl) + "\" " +
                "width=\"" + Width + "\" " +
                "height=\"" + Height + "\">" +
                "<param name=\"Movie\" value=\"" + caller.ResolveUrl(MediaUrl) + "\" />" +
                "</object>')";
        }

        public void Hit()
        {
            using (StoredProceedure sp = new StoredProceedure("AdvertMonthHit"))
            {
                sp.AddParam("AdvertId", databaseId);
                sp.Connection.Open();
                sp.Command.ExecuteNonQuery();
                sp.Connection.Close();
            }
        }
    }
}
