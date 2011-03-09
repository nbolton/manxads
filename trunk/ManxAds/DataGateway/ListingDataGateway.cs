using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManxAds.DataGateway
{
    public class ListingDataGateway : IListingDataGateway
    {
        private string connectionString;

        public ListingDataGateway(string connectionString)
        {
            this.connectionString = connectionString;
        }

        #region IListingDataGateway Members

        public ICollection<IListingBase> FetchBases()
        {
            return ListingBase.Fetch(connectionString);
        }

        public IListing Fetch(int id)
        {
            return Listing.Fetch(id, connectionString);
        }

        public ICollection<ICategory> GetCategories(IListing listing)
        {
            return listing.GetCategories(connectionString);
        }

        public int GetSellerListingCount(IListing listing)
        {
            return listing.GetSeller(connectionString).GetListingCount(connectionString);
        }

        public int GetImageCount(IListing listing)
        {
            return listing.GetImageCount(connectionString);
        }

        public void UpdateSearchIndex(IListing listing)
        {
            listing.UpdateSearchIndex(connectionString);
        }

        public ICollection<IListing> Fetch(int limit, int page)
        {
            return Listing.Fetch(limit, page);
        }

        #endregion
    }
}
