using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;
using System.Data;

namespace ManxAds
{
    public class ListingImage
    {
        private int databaseId;
        private int listingId;
        private bool isMaster;

        public int DatabaseId
        {
            get { return databaseId; }
        }

        public bool IsMaster
        {
            get { return isMaster; }
        }

        /// <summary>
        /// Gets the relative path to the listing image.
        /// </summary>
        public string FullImageUrl
        {
            get
            {
                return String.Format(
                    LocalSettings.ListingFullImageUrlFormat,
                    listingId, databaseId);
            }
        }

        /// <summary>
        /// Gets the relative path to the listing image.
        /// </summary>
        public string SmallImageUrl
        {
            get
            {
                return String.Format(
                    LocalSettings.ListingSmallImageUrlFormat,
                    listingId, databaseId);
            }
        }

        /// <summary>
        /// Gets the relative path to the listing image.
        /// </summary>
        public string ThumbnailUrl
        {
            get
            {
                return String.Format(
                    LocalSettings.ListingThumbnailUrlFormat,
                    listingId, databaseId);
            }
        }

        protected ListingImage() { }

        public ListingImage(int listingId, bool isMaster)
        {
            this.listingId = listingId;
            this.isMaster = isMaster;
        }

        internal ListingImage(int databaseId, int listingId, bool isMaster)
            : this(listingId, isMaster)
        {
            this.databaseId = databaseId;
        }

        [Obsolete()]
        internal ListingImage(StoredProceedure sp, int listingId)
            : this(
            sp.GetReaderValue<int>("ListingImageId"),
            listingId,
            sp.GetReaderValue<bool>("Master")) { }

        internal static ListingImage Parse(StoredProceedure sp, int listingId)
        {
            ListingImage image = new ListingImage();
            image.databaseId = sp.GetReaderValue<int>("ListingImageId");
            image.isMaster = sp.GetReaderValue<bool>("Master");
            image.listingId = listingId;
            return image;
        }

        public int Create(HttpServerUtility server, Stream imageStream)
        {
            using (StoredProceedure sp = new StoredProceedure("ListingImageCreate"))
            {
                sp.AddParam("@ListingId", this.listingId);
                sp.AddParam("@Master", this.isMaster);
                sp.AddOutParam("@InsertId", SqlDbType.Int);

                sp.Connection.Open();
                sp.Command.ExecuteNonQuery();

                this.databaseId = sp.GetParamValue<int>("@InsertId");
            }

            if (server == null)
            {
                // Test case mode, no server.
                return databaseId;
            }

            string thumbnailPath = server.MapPath(this.ThumbnailUrl);
            string smallImagePath = server.MapPath(this.SmallImageUrl);
            string fullImagePath = server.MapPath(this.FullImageUrl);

            Imaging.CropAndSave(
                imageStream, thumbnailPath,
                LocalSettings.ListingThumbnailWidth,
                LocalSettings.ListingThumbnailHeight);

            Imaging.CropAndSave(
                imageStream, smallImagePath,
                LocalSettings.ListingSmallImageWidth,
                LocalSettings.ListingSmallImageHeight);

            Imaging.ShrinkAndSave(
                imageStream, fullImagePath,
                LocalSettings.ListingFullImageWidth,
                LocalSettings.ListingFullImageHeight);

            return databaseId;
        }

        public void Remove(HttpServerUtility server)
        {
            if (server != null)
            {
                Remove(new WebPathUtility(server));
            }
            else
            {
                Remove((PathUtility)null);
            }
        }

        public void Remove(PathUtility pathUtility)
        {
            Remove(pathUtility, LocalSettings.ConnectionString);
        }

        public void Remove(PathUtility pathUtility, string connectionString)
        {
            if (pathUtility != null)
            {
                File.Delete(pathUtility.GetAbsolutePath(this.ThumbnailUrl));
                File.Delete(pathUtility.GetAbsolutePath(this.SmallImageUrl));
                File.Delete(pathUtility.GetAbsolutePath(this.FullImageUrl));
            }

            using (StoredProceedure sp = new StoredProceedure(
                "ListingImageRemove", connectionString))
            {
                sp.AddParam("@ListingImageId", this.databaseId);
                sp.Connection.Open();
                sp.Command.ExecuteNonQuery();
            }
        }

        public void SetMaster()
        {
            using (StoredProceedure sp = new StoredProceedure("ListingImageSetMaster"))
            {
                sp.AddParam("@ListingId", this.listingId);
                sp.AddParam("@ListingImageId", this.databaseId);
                sp.Connection.Open();
                sp.Command.ExecuteNonQuery();
            }
        }

        internal static List<ListingImage> Fetch(Listing listing)
        {
            return Fetch(listing, LocalSettings.ConnectionString);
        }

        public static List<ListingImage> Fetch(Listing listing, string connectionString)
        {
            List<ListingImage> listingImageList = new List<ListingImage>();
            using (StoredProceedure sp = new StoredProceedure("ListingImageFetchByListingId", connectionString))
            {
                sp.AddParam("@ListingId", listing.DatabaseId);
                sp.Connection.Open();
                sp.Reader = sp.Command.ExecuteReader();

                while (sp.Reader.Read())
                {
                    listingImageList.Add(ListingImage.Parse(sp, listing.DatabaseId));
                }
            }
            return listingImageList;
        }
    }
}
