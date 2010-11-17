using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using System.IO;

namespace ManxAds
{
    public class CategoryEmptyException : Exception { }

    public class Category : IEquatable<Category>
    {
        private int databaseId;
        private string title;
        private string description;
        private int priority;
        private bool hasImage;
        private int listingCount;
        private DateTime latestListing;

        public int DatabaseId
        {
            get { return databaseId; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public int Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        public bool HasImage
        {
            get { return hasImage; }
            set { hasImage = value; }
        }

        public string ListingCountWithZeros
        {
            get
            {
                return listingCount.ToString();
            }
        }

        public string ListingCountString
        {
            get
            {
                if (listingCount == 0)
                {
                    return LocalSettings.CategoriesZeroListingCount;
                }

                return listingCount.ToString();
            }
        }

        internal int ListingCount
        {
            get { return listingCount; }
        }

        public string LatestListing
        {
            get
            {
                if (latestListing.Date == DateTime.Now.Date)
                {
                    return LocalSettings.FormattedDateToday;
                }

                if (latestListing == DateTime.MinValue)
                {
                    return LocalSettings.CategoriesLatestListingNever;
                }

                return latestListing.ToShortDateString();
            }
        }

        /// <summary>
        /// Gets the image URL for the category thumbnail.
        /// </summary>
        public string ImageUrl
        {
            get
            {
                // Return null if no image.
                if (!hasImage) return null;

                return String.Format(
                    LocalSettings.CategoryImageUrlFormat, databaseId);
            }
        }

        /// <summary>
        /// Gets the navigate URL to view the listings in the category.
        /// If the StaticTitle property is not null, this is used instead
        /// of the ID.
        /// </summary>
        public string NavigateUrl
        {
            get
            {
                return String.Format(LocalSettings.CategoryNavigateUrlFormat, SearchEngineTitle, databaseId);
            }
        }

        public string ModifyUrl
        {
            get
            {
                return String.Format(LocalSettings.CategoryModifyUrlFormat, databaseId);
            }
        }

        public string RemoveUrl
        {
            get
            {
                return String.Format(LocalSettings.CategoryRemoveUrlFormat, databaseId);
            }
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

        /// <summary>
        /// Gets the listings to be displayed in this category.
        /// </summary>
        public List<Listing> Listings
        {
            get { return Listing.Fetch(this, "Boosted"); }
        }

        protected Category() { }
        protected Category(int databaseId)
        {
            this.databaseId = databaseId;
        }

        internal Category(
            int databaseId,
            string title,
            int listingCount)
        {
            this.databaseId = databaseId;
            this.title = title;
            this.listingCount = listingCount;
        }

        public Category(
            string title,
            string description,
            int priority,
            bool hasImage)
        {
            this.title = title;
            this.description = description;
            this.priority = priority;
            this.hasImage = hasImage;
        }

        internal Category(
            int databaseId,
            string title,
            string description,
            int priority,
            bool hasImage,
            DateTime latestListing,
            int listingCount,
            string staticName)
            : this(title, description, priority, hasImage)
        {
            this.databaseId = databaseId;
            this.latestListing = latestListing;
            this.listingCount = listingCount;
        }

        [Obsolete()]
        internal Category(StoredProceedure sp)
            : this(sp, sp.GetReaderValue<int>("CategoryId")) { }

        [Obsolete()]
        internal Category(StoredProceedure sp, int databaseId)
            : this(
            databaseId,
            sp.GetReaderValue<string>("Title"),
            sp.GetReaderValue<string>("Description"),
            sp.GetReaderValue<int>("Priority"),
            sp.GetReaderValue<bool>("HasImage"),
            sp.GetReaderValue<DateTime>("LatestListing"),
            sp.GetReaderValue<int>("ListingCount"),
            sp.GetReaderValue<string>("StaticName")) { }

        internal static Category Parse(StoredProceedure sp)
        {
            Category category = new Category();
            category.databaseId = sp.GetReaderValue<int>("CategoryId");
            category.title = sp.GetReaderValue<string>("Title");
            category.description = sp.GetReaderValue<string>("Description");
            category.priority = sp.GetReaderValue<int>("Priority");
            category.hasImage = sp.GetReaderValue<bool>("HasImage");
            category.latestListing = sp.GetReaderValue<DateTime>("LatestListing");
            category.listingCount = sp.GetReaderValue<int>("ListingCount");
            return category;
        }

        public void Create()
        {
            using (StoredProceedure sp = new StoredProceedure("CategoryCreate"))
            {
                sp.AddParam("@Title", this.title);
                sp.AddParam("@Priority", this.priority);
                sp.AddParam("@HasImage", this.hasImage);
                sp.AddParam("@Description", this.description);
                sp.AddOutParam("@InsertId", SqlDbType.Int);

                sp.Connection.Open();
                sp.Command.ExecuteNonQuery();

                this.databaseId = sp.GetParamValue<int>("@InsertId");
            }
        }

        public void Modify()
        {
            using (StoredProceedure sp = new StoredProceedure("CategoryModify"))
            {
                sp.AddParam("@CategoryId", this.databaseId);
                sp.AddParam("@Title", this.title);
                sp.AddParam("@Priority", this.priority);
                sp.AddParam("@HasImage", this.hasImage);
                sp.AddParam("@Description", this.description);

                sp.Connection.Open();
                sp.Command.ExecuteNonQuery();
            }
        }

        public static void ModifyPriority(int databaseId, int priority)
        {
            using (StoredProceedure sp = new StoredProceedure("CategoryModifyPriority"))
            {
                sp.AddParam("@CategoryId", databaseId);
                sp.AddParam("@Priority", priority);

                sp.Connection.Open();
                sp.Command.ExecuteNonQuery();
            }
        }

        public void Remove(HttpServerUtility server)
        {
            if (server != null)
            {
                this.RemoveImage(server);
            }

            using (StoredProceedure sp = new StoredProceedure("CategoryRemove"))
            {
                sp.AddParam("@CategoryId", this.databaseId);
                sp.Connection.Open();
                sp.Command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Fetches a specific category.
        /// </summary>
        /// <param name="databaseId">Database ID.</param>
        /// <returns>Fully populated category.</returns>
        public static Category Fetch(int databaseId)
        {
            using (StoredProceedure sp = new StoredProceedure("CategoryFetchById"))
            {
                try
                {
                    sp.AddParam("@CategoryId", databaseId);
                    sp.Connection.Open();
                    sp.Reader = sp.Command.ExecuteReader(CommandBehavior.SingleRow);
                    sp.Reader.Read();

                    return Category.Parse(sp);
                }
                catch (CannotReadException ex)
                {
                    throw new NotFoundException(databaseId, ex);
                }
            }
        }

        /// <summary>
        /// Fetches all categories.
        /// </summary>
        /// <returns>List of populated categories.</returns>
        public static List<Category> Fetch()
        {
            List<Category> categoryList = new List<Category>();
            using (StoredProceedure sp = new StoredProceedure("CategoryFetch"))
            {
                sp.Connection.Open();
                sp.Reader = sp.Command.ExecuteReader();

                while (sp.Reader.Read())
                {
                    categoryList.Add(Category.Parse(sp));
                }
            }
            return categoryList;
        }

        /// <summary>
        /// Fetches all categories in alphabetical order by title.
        /// </summary>
        /// <returns>List of populated categories.</returns>
        public static List<Category> FetchByTitleAscending()
        {
            List<Category> categoryList = new List<Category>();
            using (StoredProceedure sp = new StoredProceedure("CategoryFetchOrderAsc"))
            {
                sp.Connection.Open();
                sp.Reader = sp.Command.ExecuteReader();

                while (sp.Reader.Read())
                {
                    categoryList.Add(Category.Parse(sp));
                }
            }
            return categoryList;
        }

        public static List<Category> Fetch(Listing listing)
        {
            return Fetch(listing, LocalSettings.ConnectionString);
        }

        /// <summary>
        /// Fetches categories where a listing is shown.
        /// </summary>
        /// <param name="listing">Listing to fetch categories for.</param>
        /// <returns>List of populated categories.</returns>
        public static List<Category> Fetch(Listing listing, string connectionString)
        {
            List<Category> categoryList = new List<Category>();
            using (StoredProceedure sp = new StoredProceedure("CategoryFetchByListingId", connectionString))
            {
                sp.AddParam("@ListingId", listing.DatabaseId);
                sp.Connection.Open();
                sp.Reader = sp.Command.ExecuteReader();

                while (sp.Reader.Read())
                {
                    categoryList.Add(Category.Parse(sp));
                }
            }
            return categoryList;
        }

        /// <summary>
        /// Fetches a single category where the StaticName matches.
        /// </summary>
        /// <param name="staticName">Static name that never changes.</param>
        /// <returns>Category from the database.</returns>
        public static Category Fetch(string staticName)
        {
            using (StoredProceedure sp = new StoredProceedure("CategoryFetchByStaticName"))
            {
                sp.AddParam("@StaticName", staticName);

                sp.Connection.Open();
                sp.Reader = sp.Command.ExecuteReader();
                sp.Reader.Read();

                return Category.Parse(sp);
            }
        }

        public static List<Category> FetchTop(int limit)
        {
            Category tempCategory;
            List<Category> categoryList = new List<Category>();

            using (StoredProceedure sp = new StoredProceedure("CategoryFetchTop"))
            {
                sp.AddParam("@Limit", limit);
                sp.Connection.Open();
                sp.Reader = sp.Command.ExecuteReader();

                while (sp.Reader.Read())
                {
                    tempCategory = new Category(
                        sp.GetReaderValue<int>("CategoryId"),
                        sp.GetReaderValue<string>("Title"),
                        sp.GetReaderValue<int>("ListingCount"));
                    categoryList.Add(tempCategory);
                }
            }
            return categoryList;
        }

        public bool Equals(Category other)
        {
            if (this.databaseId == other.databaseId)
            {
                return true;
            }

            return false;
        }

        public void CreateImage(
            HttpServerUtility server, Stream imageStream)
        {
            // Enable now to use ImageUrl property.
            this.hasImage = true;

            if (server != null)
            {
                Imaging.CropAndSave(
                imageStream,
                server.MapPath(this.ImageUrl),
                LocalSettings.CategoryThumbnailWidth,
                LocalSettings.CategoryThumbnailHeight);
            }

            // Call after, incase image create fails.
            this.Modify();
        }

        public void RemoveImage(HttpServerUtility server)
        {
            // Fetch image path and remove from database first.
            string imageUrl = this.ImageUrl;
            this.hasImage = false;
            this.Modify();

            if (server != null)
            {
                string path = server.MapPath(imageUrl);
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }
    }
}
