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

        public List<IListingBase> FetchBases()
        {
            return ListingBase.Fetch(connectionString);
        }

        public IListing Fetch(int id)
        {
            return Listing.Fetch(id, connectionString);
        }

        public List<ICategory> GetCategories(IListing listing)
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

        #endregion
    }
}
