using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManxAds.DataGateway
{
    public interface IDataGatewayProvider
    {
        IListingDataGateway Listing { get; }
        ISearchDataGateway Search { get; }
    }
}
