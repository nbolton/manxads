using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManxAds.DataGateway;

namespace ManxAds.Test.DataGateways
{
    class StubDataGatewayProvider : IDataGatewayProvider
    {
        #region IDataGatewayProvider Members

        public IListingDataGateway Listing
        {
            get;
            set;
        }

        public ISearchDataGateway Search
        {
            get;
            set;
        }

        #endregion
    }
}
