using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManxAds
{
    public interface IListingBase
    {
        string Title { get; }
        string Details { get; }
        int DatabaseId { get; }
    }
}
