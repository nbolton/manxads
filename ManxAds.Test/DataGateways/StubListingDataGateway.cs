﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManxAds.DataGateway;

namespace ManxAds.Test.DataGateways
{
    class StubListingDataGateway : IListingDataGateway
    {
        public List<ICategory> StubCategories;

        #region IListingDataGateway Members

        public ICollection<IListingBase> FetchBases()
        {
            throw new NotImplementedException();
        }

        public IListing Fetch(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<ICategory> GetCategories(IListing listing)
        {
            return StubCategories;
        }

        public int GetSellerListingCount(IListing listing)
        {
            return 1;
        }

        public int GetImageCount(IListing listing)
        {
            return 1;
        }

        public void UpdateSearchIndex(IListing listing)
        {
            throw new NotImplementedException();
        }

        public ICollection<IListing> Fetch(int limit, int page)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
