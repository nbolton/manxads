using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ManxAds.Test.Listings;
using ManxAds.Search;
using ManxAds.Test.DataGateways;

namespace ManxAds.Test
{
    [TestClass]
    public class SearchIndexUpdaterTests
    {
        [TestMethod]
        public void Update_SimpleListing_GotExpectedCatalogueXml()
        {
            StubListing listing = new StubListing();
            listing.DatabaseId = 1;
            listing.Title = "test title";
            listing.Details = "test details";

            List<ICategory> categories = new List<ICategory>();
            categories.Add(new StubCategory());

            StubListingDataGateway listingData = new StubListingDataGateway();
            listingData.StubCategories = categories;

            MockSearchDataGateway searchData = new MockSearchDataGateway();

            StubDataGatewayProvider data = new StubDataGatewayProvider();
            data.Listing = listingData;
            data.Search = searchData;

            SearchIndexUpdater updater = new SearchIndexUpdater(data);

            updater.Update(listing);

            string expected =
                "<Catalogue>" + 
                    "<Keyword Name=\"test\" Weight=\"54\" />" +
                    "<Keyword Name=\"title\" Weight=\"26.5\" />" + 
                    "<Keyword Name=\"detail\" Weight=\"1.766667\" />" +
                    "<Keyword Name=\"details\" Weight=\"0.8833333\" />" + 
                "</Catalogue>";

            Assert.AreEqual(1, searchData.UpdateForSingleListing_listingId);
            Assert.AreEqual(expected, searchData.UpdateForSingleListing_catalogueXml.Value);
        }
    }
}
