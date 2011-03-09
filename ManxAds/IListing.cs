using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManxAds
{
    public interface IListing : IListingBase
    {
        List<ICategory> GetCategories(string connectionString);

        ISeller GetSeller(string connectionString);

        int GetImageCount(string connectionString);
    }
}
