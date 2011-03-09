using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManxAds.DataGateway
{
    public interface IListingDataGateway
    {
        List<IListingBase> FetchBases();

        IListing Fetch(int id);

        List<ICategory> GetCategories(IListing listing);

        int GetSellerListingCount(IListing listing);

        int GetImageCount(IListing listing);
    }
}
