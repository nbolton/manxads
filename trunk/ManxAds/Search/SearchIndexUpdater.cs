using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManxAds.DataGateway;
using System.Xml;
using System.Diagnostics;

namespace ManxAds.Search
{
    public class SearchIndexUpdater
    {
        private IDataGatewayProvider data;

        public SearchIndexUpdater(string connectionString)
            : this(new DataGatewayProvider(connectionString)) { }

        public SearchIndexUpdater(IDataGatewayProvider data)
        {
            this.data = data;
        }

        public void Update(IListing listing)
        {
            DateTime start = DateTime.Now;

            Catalogue catalogue = new Catalogue(data);
            catalogue.GenerateFromSingle(listing);
            data.Search.UpdateForSingleListing(listing.DatabaseId, catalogue.GetSingleListingXml());

            TimeSpan elapsed = DateTime.Now - start;
            Debug.WriteLine("search index updated in: " + elapsed.TotalMilliseconds + "ms");
        }
    }
}
