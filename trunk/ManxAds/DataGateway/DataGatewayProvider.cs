using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManxAds.DataGateway
{
    public class DataGatewayProvider : IDataGatewayProvider
    {
        private string connectionString;

        public DataGatewayProvider(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IListingDataGateway Listing
        {
            get { return new ListingDataGateway(connectionString); }
        }

        public ISearchDataGateway Search
        {
            get { return new SearchDataGateway(connectionString); }
        }
    }
}
