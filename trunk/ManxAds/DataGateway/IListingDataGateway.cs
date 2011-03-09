using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManxAds.DataGateway
{
    public interface IListingDataGateway
    {
        ICollection<IListingBase> FetchBases();

        IListing Fetch(int id);

        ICollection<ICategory> GetCategories(IListing listing);

        int GetSellerListingCount(IListing listing);

        int GetImageCount(IListing listing);

        void UpdateSearchIndex(IListing listing);

        ICollection<IListing> Fetch(int limit, int page);
    }
}
