using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManxAds.Test.Listings
{
    class StubListing : IListing
    {
        #region IListing Members

        public string Title
        {
            get;
            set;
        }

        public string Details
        {
            get;
            set;
        }

        public int DatabaseId
        {
            get;
            set;
        }

        public List<ICategory> GetCategories(string connectionString)
        {
            throw new NotImplementedException();
        }

        public ISeller GetSeller(string connectionString)
        {
            throw new NotImplementedException();
        }

        public int GetImageCount(string connectionString)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
