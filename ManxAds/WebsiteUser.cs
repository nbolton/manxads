using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Net.Mail;
using System.Web;
using System.IO;
using System.Data.SqlTypes;
using System.Net;
using System.Web.UI;
using System.Linq;

namespace ManxAds
{
    public class WebsiteUser : IEquatable<WebsiteUser>, ISeller
    {
        private const int displayNameTrim = 15;
        private const int emailAddressTrim = 25;

        private bool isDead = false;
        private int databaseId;
        private SocialTitleType socialTitleType;
        private string forename;
        private string surname;
        private string emailAddress;
        private Password password;
        private string landlineArea;
        private string landlinePhone;
        private string mobileArea;
        private string mobilePhone;
        private WebsiteUserType userType;
        private bool emailOptOut;
        private DateTime lastActive;
        private DateTime createDate;
        private DateTime updateDate;
        private RegisterType registerType;
        private int locationId;
        private bool isVerified;
        private DateTime verifyDate;
        private string verifyAuthCode;
        private TraderType traderType;
        private DateTime banUntil;

        public DateTime BanUntil
        {
            get { return banUntil; }
            set { banUntil = value; }
        }

        private string _tradingName;
        private string _tradingWebsite;

        private string tradingWebsite
        {
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    if (!value.Contains("http://"))
                    {
                        value = "http://" + value;
                    }
                    _tradingWebsite = value;
                }
            }
            get { return _tradingWebsite; }
        }

        private string tradingName
        {
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _tradingName = value;
                }
            }
            get { return _tradingName; }
        }

        /// <summary>
        /// Gets a new dead user. Can be used in the event of a session panic.
        /// </summary>
        [Obsolete]
        public static WebsiteUser DeadUser
        {
            get
            {
                WebsiteUser user = new WebsiteUser();
                user.isDead = true;
                return user;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating if the user's session is dead.
        /// </summary>
        [Obsolete]
        public bool IsDead
        {
            get { return isDead; }
            set { isDead = value; }
        }

        public int DatabaseId
        {
            get { return databaseId; }
        }

        public SocialTitleType SocialTitleType
        {
            get { return socialTitleType; }
            set { socialTitleType = value; }
        }

        public string Forename
        {
            get { return forename; }
            set { forename = StringTools.StripHtmlTags(value); }
        }

        public string Surname
        {
            get { return surname; }
            set { surname = StringTools.StripHtmlTags(value); }
        }

        public string EmailAddress
        {
            get { return emailAddress; }
            set { emailAddress = StringTools.StripHtmlTags(value.ToLower()); }
        }

        public string EmailAddressTrimmed
        {
            get
            {
                return StringTools.TrimString(
                    emailAddress, 0, emailAddressTrim, false, "...", 2);
            }
        }

        public string Password
        {
            set { password = value; }
        }

        public string LandlineArea
        {
            get
            {
                if (String.IsNullOrEmpty(landlineArea))
                {
                    return LocalSettings.DefaultLandlineArea;
                }
                return landlineArea;
            }
        }

        public string LandlinePhone
        {
            get { return landlinePhone; }
        }

        public bool HasLandlinePhone
        {
            get { return !String.IsNullOrEmpty(landlinePhone) ? true : false; }
        }

        public string LandlinePhoneFull
        {
            get
            {
                if (HasLandlinePhone)
                {
                    return LandlineArea + " " + LandlinePhone;
                }
                return String.Empty;
            }
        }

        public string MobileArea
        {
            get
            {
                if (String.IsNullOrEmpty(mobileArea))
                {
                    return LocalSettings.DefaultMobileArea;
                }
                return mobileArea;
            }
        }

        public string MobilePhone
        {
            get { return mobilePhone; }
        }

        public bool HasMobilePhone
        {
            get { return !String.IsNullOrEmpty(mobilePhone) ? true : false; }
        }

        public string MobilePhoneFull
        {
            get
            {
                if (HasMobilePhone)
                {
                    return MobileArea + " " + MobilePhone;
                }
                return String.Empty;
            }
        }

        public WebsiteUserType UserType
        {
            get { return userType; }
            set { userType = value; }
        }

        public bool EmailOptOut
        {
            get { return emailOptOut; }
            set { emailOptOut = value; }
        }

        public string EmailOptOutString
        {
            get { return emailOptOut ? "Yes" : "No"; }
        }

        public string EmailOptOutShortString
        {
            get { return emailOptOut ? "Y" : "N"; }
        }

        public DateTime LastActive
        {
            get { return lastActive; }
            set { lastActive = value; }
        }

        public bool IsVerified
        {
            get { return isVerified; }
        }

        public string IsVerifiedString
        {
            get { return isVerified ? "Yes" : "No"; }
        }

        public string IsVerifiedShortString
        {
            get { return isVerified ? "Y" : "N"; }
        }

        public TraderType TraderType
        {
            get { return traderType; }
            set { traderType = value; }
        }

        public bool IsTrader
        {
            get { return (traderType != TraderType.None); }
        }

        public DateTime VerifyDate
        {
            get { return verifyDate; }
            set { verifyDate = value; }
        }

        public string TradingName
        {
            get { return tradingName; }
            set { tradingName = value; }
        }

        public string TradingWebsite
        {
            get { return tradingWebsite; }
            set { tradingWebsite = value; }
        }

        public TimeSpan LastActiveTimeSpan
        {
            get { return new TimeSpan(DateTime.Now.Ticks - lastActive.Ticks); }
        }

        public string LastActiveTimeSpanString
        {
            get
            {
                double days = Math.Round(LastActiveTimeSpan.TotalDays, 0);
                
                if (days == 0)
                {
                    return "Today";
                }
                
                if (days == 1)
                {
                    return days + " Day";
                }

                return days + " Days";
            }
        }

        public string LastActiveShortString
        {
            get
            {
                int days = (int)Math.Round(LastActiveTimeSpan.TotalDays, 0);
                return days.ToString();
            }
        }

        public string LastActiveShortDate
        {
            get { return LastActive.ToShortDateString(); }
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

        public int LocationId
        {
            get { return locationId; }
            set { locationId = value; }
        }

        public bool HasLocation
        {
            get { return locationId > 0 ? true : false; }
        }

        public Location Location
        {
            get
            {
                try
                {
                    return Location.Fetch(locationId);
                }
                catch (CannotReadException ex)
                {
                    throw new InvalidOperationException(
                        "Cannot fetch location with ID " + locationId, ex);
                }
            }
        }

        public string LocationString
        {
            get { return Location.Name; }
        }

        public RegisterType RegisterType
        {
            get { return registerType; }
        }

        /// <summary>
        /// Gets the user's forename and surname.
        /// </summary>
        public string FullName
        {
            get
            {
                return String.Format("{0} {1}", forename, surname);
            }
        }

        public string VerifyTimeToLiveWithinDays
        {
            get
            {
                if (VerifyTimeToLive.Days > 0)
                {
                    return "within " + VerifyTimeToLive.Days + " days";
                }

                if (VerifyTimeToLive.Days == 1)
                {
                    return "within 1 day";
                }

                return "today";
            }
        }

        public TimeSpan VerifyTimeToLive
        {
            get
            {
                // Calculate how many ticks since registered.
                long registerTicks = DateTime.Now.Ticks - CreateDate.Ticks;
                TimeSpan registerTime = new TimeSpan(registerTicks);

                // Calculate how many days until validation period expires.
                int timeoutDays = LocalSettings.VerificationTimeoutDays;
                int ttlDays = timeoutDays - registerTime.Days;

                // Convert day count into a TimeSpan.
                return new TimeSpan(ttlDays, 0, 0, 0);
            }
        }

        public bool IsDisabled
        {
            get
            {
                // If timespan has elapsed over the TTL.
                if (VerifyTimeToLive.Ticks >= 0)
                {
                    return false;
                }

                // Explicit false if verified.
                if (IsVerified)
                {
                    return false;
                }

                return true;
            }
        }

        public string IsDisabledShortString
        {
            get { return IsDisabled ? "Y" : "N"; }
        }

        public List<Listing> Listings
        {
            get { return Listing.Fetch(this); }
        }

        public List<Listing> RecycleBin
        {
            get { return Listing.Fetch(this, false); }
        }

        public int ListingCount
        {
            get { return Listing.RunCount(SqlDateTime.MinValue.Value, true, DatabaseId); }
        }

        public int RecycleBinCount
        {
            get { return Listing.RunCount(SqlDateTime.MinValue.Value, false, DatabaseId); }
        }

        public string ModifyUrl
        {
            get { return String.Format(LocalSettings.UserModifyUrlFormat, databaseId); }
        }

        public string RemoveUrl
        {
            get { return String.Format(LocalSettings.UserRemoveUrlFormat, databaseId); }
        }

        public string ListingsUrl
        {
            get { return String.Format(LocalSettings.SellerListingsUrlFormat, databaseId); }
        }

        public string BanUrl
        {
            get { return String.Format(LocalSettings.ListingBanUrlFormat, databaseId); }
        }

        public bool ListingLimitReached
        {
            get
            {
                if (IsTrader)
                {
                    int limit = LocalSettings.Fetch<int>("TraderListingLimit",
                        LocalSettings.Default.TraderListingLimit, this);

                    if (ListingCount >= limit)
                    {
                        return true;
                    }
                }
                else
                {
                    int limit = LocalSettings.Fetch<int>("UserListingLimit",
                        LocalSettings.Default.UserListingLimit, this);

                    if (ListingCount >= limit)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public string ListingLimitReachedShortString
        {
            get { return ListingLimitReached ? "Y" : "N"; }
        }

        public string TraderLogoRelativePath
        {
            get
            {
                if (!this.IsTrader)
                {
                    throw new NotSupportedException(
                        "TraderLogo is only supported for Trader users.");
                }

                return String.Format(LocalSettings.TraderLogoPathFormat,
                    this.DatabaseId, LocalSettings.TraderLogoForceExtension);
            }
        }

        public string DisplayName
        {
            get
            {
                string result = String.Empty;
                if (this.IsTrader)
                {
                    return tradingName;
                }
                return String.Format("{0}, {1}", surname, forename);
            }
        }

        public string DisplayNameTrimmed
        {
            get
            {
                return StringTools.TrimString(
                    DisplayName, 0, displayNameTrim, false, "...", 2);
            }
        }

        protected WebsiteUser() { }
        protected WebsiteUser(int databaseId)
        {
            this.databaseId = databaseId;
        }

        public WebsiteUser(
            SocialTitleType socialTitleType,
            string forename,
            string surname,
            string emailAddress,
            string password,
            string landlineArea,
            string landlinePhone,
            string mobileArea,
            string mobilePhone,
            int locationId,
            WebsiteUserType userType,
            bool emailOptOut,
            DateTime lastActive,
            DateTime createDate,
            DateTime updateDate,
            RegisterType registerType,
            string tradingName,
            string tradingWebsite,
            TraderType traderType)
        {
            this.socialTitleType = socialTitleType;
            this.forename = forename;
            this.surname = surname;
            this.EmailAddress = emailAddress;
            this.password = password;
            this.locationId = locationId;
            this.userType = userType;
            this.emailOptOut = emailOptOut;
            this.lastActive = lastActive;
            this.createDate = createDate;
            this.updateDate = updateDate;
            this.registerType = registerType;
            this.tradingName = tradingName;
            this.tradingWebsite = tradingWebsite;
            this.traderType = traderType;

            this.SetLandlineNumber(landlineArea, landlinePhone);
            this.SetMobileNumber(mobileArea, mobilePhone);
        }

        internal WebsiteUser(
            int databaseId,
            SocialTitleType socialTitleType,
            string forename,
            string surname,
            string emailAddress,
            string password,
            string landlineArea,
            string landlinePhone,
            string mobileArea,
            string mobilePhone,
            int locationId,
            WebsiteUserType userType,
            bool emailOptOut,
            DateTime lastActive,
            DateTime createDate,
            DateTime updateDate,
            RegisterType registerType,
            bool isVerified,
            DateTime verifyDate,
            string verifyAuthCode,
            string tradingName,
            string tradingWebsite,
            TraderType traderType)
            : this(
            socialTitleType,
            forename,
            surname,
            emailAddress,
            password,
            landlineArea,
            landlinePhone,
            mobileArea,
            mobilePhone,
            locationId,
            userType,
            emailOptOut,
            lastActive,
            createDate,
            updateDate,
            registerType,
            tradingName,
            tradingWebsite,
            traderType)
        {
            this.databaseId = databaseId;
            this.isVerified = isVerified;
            this.verifyDate = verifyDate;
            this.verifyAuthCode = verifyAuthCode;
        }

        [Obsolete()]
        internal WebsiteUser(StoredProceedure sp)
            : this(sp, sp.GetReaderValue<int>("UserId")) { }

        [Obsolete()]
        internal WebsiteUser(StoredProceedure sp, int databaseId)
            : this(
            databaseId,
            sp.GetReaderValue<SocialTitleType>("SocialTitleType"),
            sp.GetReaderValue<string>("Forename"),
            sp.GetReaderValue<string>("Surname"),
            sp.GetReaderValue<string>("EmailAddress"),
            null, // Password is encrypted, and so is useless.
            sp.GetReaderValue<string>("LandlineArea"),
            sp.GetReaderValue<string>("LandlinePhone"),
            sp.GetReaderValue<string>("MobileArea"),
            sp.GetReaderValue<string>("MobilePhone"),
            sp.GetReaderValue<int>("LocationId"),
            sp.GetReaderValue<WebsiteUserType>("UserType"),
            sp.GetReaderValue<bool>("EmailOptOut"),
            sp.GetReaderValue<DateTime>("LastActive"),
            sp.GetReaderValue<DateTime>("CreateDate"),
            sp.GetReaderValue<DateTime>("UpdateDate"),
            sp.GetReaderValue<RegisterType>("RegisterType"),
            sp.GetReaderValue<bool>("IsVerified"),
            sp.GetReaderValue<DateTime>("VerifyDate"),
            sp.GetReaderValue<string>("VerifyAuthCode"),
            sp.GetReaderValue<string>("TradingName"),
            sp.GetReaderValue<string>("TradingWebsite"),
            sp.GetReaderValue<TraderType>("TraderType")) { }

        public void SetLandlineNumber(string area, string phone)
        {
            if (!String.IsNullOrEmpty(phone))
            {
                this.landlinePhone = StringTools.StripHtmlTags(phone);
                if (!String.IsNullOrEmpty(area) && area != LocalSettings.DefaultLandlineArea)
                {
                    this.landlineArea = StringTools.StripHtmlTags(area);
                }
            }
            else
            {
                landlinePhone = null;
                landlineArea = null;
            }
        }

        public void SetMobileNumber(string area, string phone)
        {
            if (!String.IsNullOrEmpty(phone))
            {
                this.mobilePhone = StringTools.StripHtmlTags(phone);
                if (!String.IsNullOrEmpty(area) && area != LocalSettings.DefaultMobileArea)
                {
                    this.mobileArea = StringTools.StripHtmlTags(area);
                }
            }
            else
            {
                mobilePhone = null;
                mobileArea = null;
            }
        }

        public void Invalidate()
        {
            this.isVerified = false;
            this.verifyAuthCode = GetUniqueAuthCode(this.emailAddress);
        }

        public bool Create() {
            return Create(null);
        }

        /// <summary>
        /// Creates a database reflection.
        /// </summary>
        /// <returns>False if email address occupied.</returns>
        public bool Create(string userIpAddress)
        {
            // Save verification auth code for use in SendVerificationEmail().
            this.verifyAuthCode = WebsiteUser.GetUniqueAuthCode(emailAddress);

            using (StoredProceedure sp = new StoredProceedure("UserCreate"))
            {
                sp.AddParam("@SocialTitleType", (int)socialTitleType);
                sp.AddParam("@Forename", forename);
                sp.AddParam("@Surname", surname);
                sp.AddParam("@EmailAddress", emailAddress);
                sp.AddParam("@Password", password.Encrypted);
                sp.AddParam("@LandlineArea", landlineArea);
                sp.AddParam("@LandlinePhone", landlinePhone);
                sp.AddParam("@MobileArea", mobileArea);
                sp.AddParam("@MobilePhone", mobilePhone);
                sp.AddParam("@LocationId", locationId);
                sp.AddParam("@UserType", (int)userType);
                sp.AddParam("@EmailOptOut", emailOptOut);
                sp.AddParam("@LastActive", lastActive);
                sp.AddParam("@CreateDate", createDate);
                sp.AddParam("@UpdateDate", updateDate);
                sp.AddParam("@RegisterType", (int)registerType);
                sp.AddParam("@VerifyAuthCode", verifyAuthCode);
                sp.AddParam("@TradingName", tradingName);
                sp.AddParam("@TradingWebsite", tradingWebsite);
                sp.AddParam("@TraderType", traderType);
                sp.AddParam("@LastIp", userIpAddress != null ? (object)userIpAddress : DBNull.Value);
                sp.AddOutParam("@EmailOccupied", SqlDbType.Bit);
                sp.AddOutParam("@InsertId", SqlDbType.Int);

                sp.Connection.Open();
                sp.Command.ExecuteNonQuery();

                this.databaseId = sp.GetParamValue<int>("@InsertId");
                if (sp.GetParamValue<bool>("@EmailOccupied"))
                {
                    return false;
                }
            }

            return true;
        }

        public void Modify()
        {
            using (StoredProceedure sp = new StoredProceedure("UserModify"))
            {
                sp.AddParam("@UserId", databaseId);
                sp.AddParam("@SocialTitleType", (int)socialTitleType);
                sp.AddParam("@Forename", forename);
                sp.AddParam("@Surname", surname);
                sp.AddParam("@EmailAddress", emailAddress);
                sp.AddParam("@LandlineArea", landlineArea);
                sp.AddParam("@LandlinePhone", landlinePhone);
                sp.AddParam("@MobileArea", mobileArea);
                sp.AddParam("@MobilePhone", mobilePhone);
                sp.AddParam("@LocationId", locationId);
                sp.AddParam("@UserType", (int)userType);
                sp.AddParam("@EmailOptOut", emailOptOut);
                sp.AddParam("@LastActive", lastActive);
                sp.AddParam("@CreateDate", createDate);
                sp.AddParam("@UpdateDate", updateDate);
                sp.AddParam("@TradingName", tradingName);
                sp.AddParam("@TradingWebsite", tradingWebsite);
                sp.AddParam("@RegisterType", (int)registerType);
                sp.AddParam("@VerifyAuthCode", verifyAuthCode);
                sp.AddParam("@TraderType", traderType);
                sp.AddParam("@IsVerified", isVerified);

                if (password != null)
                {
                    sp.AddParam("@Password", password.Encrypted);
                }
                else
                {
                    sp.AddParam("@Password", DBNull.Value);
                }

                if (banUntil != default(DateTime))
                {
                    sp.AddParam("@BanUntil", banUntil);
                }
                else
                {
                    sp.AddParam("@BanUntil", DBNull.Value);
                }

                sp.Connection.Open();
                sp.Command.ExecuteNonQuery();
            }
        }

        public void Remove()
        {
            using (StoredProceedure sp = new StoredProceedure("UserRemove"))
            {
                sp.AddParam("@UserId", databaseId);
                sp.Connection.Open();
                sp.Command.ExecuteNonQuery();
            }
        }

        public void SendVerificationEmail(string urlFormat, Page page)
        {
            string authCodeUrl = string.Format(urlFormat, this.verifyAuthCode);

            string messageBody = "Hello " + this.forename + ",\r\n\r\n" +
                "Welcome to ManxAds! When you have a moment, please " +
                "follow this link to confirm your registration.\r\n\r\n" +
                authCodeUrl + "\r\n\r\n" +
                "Please confirm your registration within " +
                LocalSettings.VerificationTimeoutDays + " days of registering, " +
                "or restrictions may be applied on your account.";

            MailMessage message = new MailMessage(
                LocalSettings.MasterSendFromEmail, emailAddress,
                "Registry Confirmation", messageBody);

            EmailTools.SendMessage(message, page);
        }

        public void SendVerificationEmail(Page page)
        {
            SendVerificationEmail(page.Request.Url.OriginalString + "?VerifyAuthCode={0}", page);
        }

        public static string GetUniqueAuthCode(string email)
        {
            // Return an encryption of email and generated passsword.
            string randomCode = ManxAds.Password.Generate();
            Password password = email + randomCode;
            return password.Encrypted;
        }

        public static bool EmailInUse(string emailAddress)
        {
            using (StoredProceedure sp = new StoredProceedure("UserCheckEmailAddress"))
            {
                sp.AddParam("@EmailAddress", emailAddress);
                sp.AddOutParam("@EmailInUse", SqlDbType.Bit);
                sp.Connection.Open();
                sp.Command.ExecuteNonQuery();

                return sp.GetParamValue<bool>("@EmailInUse");
            }
        }

        public static WebsiteUser Verify(string authCode)
        {
            using (StoredProceedure sp = new StoredProceedure("UserVerify"))
            {
                sp.AddParam("@AuthCode", authCode);
                sp.AddOutParam("@UserId", SqlDbType.Int);
                sp.Connection.Open();
                sp.Command.ExecuteNonQuery();

                int userId = sp.GetParamValue<int>("@UserId");
                if (userId == 0)
                {
                    throw new NotFoundException();
                }

                return WebsiteUser.Fetch(userId);
            }
        }

        internal static WebsiteUser Parse(StoredProceedure sp)
        {
            WebsiteUser user = new WebsiteUser();
            user.databaseId = sp.GetReaderValue<int>("UserId");
            user.socialTitleType = sp.GetReaderValue<SocialTitleType>("SocialTitleType");
            user.forename = sp.GetReaderValue<string>("Forename");
            user.surname = sp.GetReaderValue<string>("Surname");
            user.emailAddress = sp.GetReaderValue<string>("EmailAddress");
            user.landlineArea = sp.GetReaderValue<string>("LandlineArea");
            user.landlinePhone = sp.GetReaderValue<string>("LandlinePhone");
            user.mobileArea = sp.GetReaderValue<string>("MobileArea");
            user.mobilePhone = sp.GetReaderValue<string>("MobilePhone");
            user.locationId = sp.GetReaderValue<int>("LocationId");
            user.userType = sp.GetReaderValue<WebsiteUserType>("UserType");
            user.emailOptOut = sp.GetReaderValue<bool>("EmailOptOut");
            user.lastActive = sp.GetReaderValue<DateTime>("LastActive");
            user.createDate = sp.GetReaderValue<DateTime>("CreateDate");
            user.updateDate = sp.GetReaderValue<DateTime>("UpdateDate");
            user.registerType = sp.GetReaderValue<RegisterType>("RegisterType");
            user.isVerified = sp.GetReaderValue<bool>("IsVerified");
            user.verifyDate = sp.GetReaderValue<DateTime>("VerifyDate");
            user.verifyAuthCode = sp.GetReaderValue<string>("VerifyAuthCode");
            user.tradingName = sp.GetReaderValue<string>("TradingName");
            user.tradingWebsite = sp.GetReaderValue<string>("TradingWebsite");
            user.traderType = sp.GetReaderValue<TraderType>("TraderType");
            user.banUntil = sp.GetReaderValue<DateTime>("BanUntil");
            return user;
        }

        public static WebsiteUser Fetch(int databaseId)
        {
            return Fetch(databaseId, LocalSettings.ConnectionString);
        }

        public static WebsiteUser Fetch(int databaseId, string connectionString)
        {
            using (StoredProceedure sp = new StoredProceedure("UserFetchById", connectionString))
            {
                try
                {
                    sp.AddParam("@UserId", databaseId);
                    sp.Connection.Open();
                    sp.Reader = sp.Command.ExecuteReader(CommandBehavior.SingleRow);
                    sp.Reader.Read();

                    return WebsiteUser.Parse(sp);
                }
                catch (CannotReadException ex)
                {
                    throw new NotFoundException(databaseId, ex);
                }
            }
        }

        public static WebsiteUser Fetch(string emailAddress)
        {
            using (StoredProceedure sp = new StoredProceedure("UserFetchByEmail"))
            {
                try
                {
                    sp.AddParam("@EmailAddress", emailAddress);
                    sp.Connection.Open();
                    sp.Reader = sp.Command.ExecuteReader(CommandBehavior.SingleRow);
                    sp.Reader.Read();

                    return WebsiteUser.Parse(sp);
                }
                catch (CannotReadException ex)
                {
                    throw new NotFoundException(ex);
                }
            }
        }

        public static List<WebsiteUser> Fetch()
        {
            List<WebsiteUser> users = new List<WebsiteUser>();
            using (StoredProceedure sp = new StoredProceedure("UserFetch"))
            {
                sp.Connection.Open();
                sp.Reader = sp.Command.ExecuteReader();

                while (sp.Reader.Read())
                {
                    users.Add(WebsiteUser.Parse(sp));
                }
            }
            return users;
        }

        public static List<WebsiteUser> Fetch(TraderType traderType)
        {
            List<WebsiteUser> users = new List<WebsiteUser>();
            using (StoredProceedure sp = new StoredProceedure("UserFetchByTraderType"))
            {
                sp.AddParam("@TraderType", traderType);

                sp.Connection.Open();
                sp.Reader = sp.Command.ExecuteReader();

                while (sp.Reader.Read())
                {
                    users.Add(WebsiteUser.Parse(sp));
                }
            }
            return users;
        }

        public static List<WebsiteUser> Search(string term)
        {
            List<WebsiteUser> users = new List<WebsiteUser>();
            using (StoredProceedure sp = new StoredProceedure("UserSearch"))
            {
                sp.AddParam("@term", term);
                sp.Connection.Open();
                sp.Reader = sp.Command.ExecuteReader();

                while (sp.Reader.Read())
                {
                    users.Add(WebsiteUser.Parse(sp));
                }
            }
            return users;
        }

        public static List<string> FetchPromoEmails()
        {
            List<string> emailList = new List<string>();
            using (StoredProceedure sp = new StoredProceedure("UserFetchPromoEmails"))
            {
                sp.Connection.Open();
                sp.Reader = sp.Command.ExecuteReader();

                while (sp.Reader.Read())
                {
                    emailList.Add(sp.GetReaderValue<string>("EmailAddress"));
                }
            }
            return emailList;
        }

        /// <summary>
        /// Counts the number of users in the database from a specific date.
        /// </summary>
        /// <returns>Number of listings.</returns>
        public static int RunCount(DateTime fromDate, WebsiteUserType userType)
        {
            using (StoredProceedure sp = new StoredProceedure("UserCountByUserType"))
            {
                sp.AddParam("@UserType", userType);
                sp.AddParam("@FromDate", fromDate);
                sp.AddOutParam("@Count", System.Data.SqlDbType.Int);
                sp.Connection.Open();
                sp.Command.ExecuteNonQuery();
                return sp.GetParamValue<int>("@Count");
            }
        }

        /// <summary>
        /// Counts the number of users in the database from a specific date.
        /// </summary>
        /// <returns>Number of listings.</returns>
        public static int RunCount(DateTime fromDate, TraderType traderType)
        {
            using (StoredProceedure sp = new StoredProceedure("UserCountByTraderType"))
            {
                sp.AddParam("@FromDate", fromDate);
                sp.AddParam("@TraderType", traderType);
                sp.AddOutParam("@Count", System.Data.SqlDbType.Int);
                sp.Connection.Open();
                sp.Command.ExecuteNonQuery();
                return sp.GetParamValue<int>("@Count");
            }
        }

        public bool Equals(WebsiteUser other)
        {
            if (other.databaseId == this.databaseId)
            {
                return true;
            }
            return false;
        }

        public int GetListingCount(string connectionString)
        {
            return Listing.RunCount(SqlDateTime.MinValue.Value, true, DatabaseId, connectionString);
        }

        public static bool Exists(int SellerId)
        {
            throw new NotImplementedException();
        }

        public void EmptyRecycleBin(HttpServerUtility server)
        {
            List<Listing> recycleBinListings =
                (from l in RecycleBin
                 where l.ImageCount != 0
                 select l).ToList();

            foreach (Listing l in recycleBinListings)
            {
                foreach (ListingImage li in l.Images)
                {
                    li.Remove(server);
                }
            }

            using (StoredProceedure sp = new StoredProceedure("UserEmptyRecycleBin"))
            {
                sp.AddParam("@UserId", DatabaseId);
                sp.Connection.Open();
                sp.Command.ExecuteNonQuery();
            }
        }

        public void Ban(string body, int periodDays)
        {
            string periodString;
            if (periodDays == -1)
            {
                banUntil = SqlDateTime.MaxValue.Value;
                periodString = "permanently";
            }
            else
            {
                banUntil = DateTime.Now.AddDays(periodDays);
                periodString = string.Format(
                    "for {0} {1}",
                    periodDays,
                    (periodDays == 1 ? "day" : "days"));
            }

            Modify();

            string subject = string.Format("You have been banned {0}", periodString);

            MailMessage mailMessage = new MailMessage(
                LocalSettings.MasterSendFromEmail, emailAddress, subject, body);

            EmailTools.SendMessage(mailMessage);
        }
    }
}