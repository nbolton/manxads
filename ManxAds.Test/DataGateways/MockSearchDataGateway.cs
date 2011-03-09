using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManxAds.DataGateway;
using ManxAds.Search;
using System.Data.SqlTypes;

namespace ManxAds.Test.DataGateways
{
    class MockSearchDataGateway : ISearchDataGateway
    {
        public int UpdateForSingleListing_listingId;
        public SqlXml UpdateForSingleListing_catalogueXml;

        #region ISearchDataGateway Members

        public void OverwriteWithCatalogue(Catalogue catalogue)
        {
            throw new NotImplementedException();
        }

        public void UpdateForSingleListing(int listingId, SqlXml catalogueXml)
        {
            UpdateForSingleListing_listingId = listingId;
            UpdateForSingleListing_catalogueXml = catalogueXml;
        }

        #endregion
    }
}
