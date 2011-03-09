using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManxAds.Search;
using System.Data.SqlTypes;

namespace ManxAds.DataGateway
{
    public interface ISearchDataGateway
    {
        void OverwriteWithCatalogue(Catalogue catalogue);

        void UpdateForSingleListing(int listingId, SqlXml catalogueXml);
    }
}
