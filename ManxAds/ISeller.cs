using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManxAds
{
    public interface ISeller
    {
        string Forename { get; }
        string FullName { get; }
        string EmailAddress { get; }
        int GetListingCount(string connectionString);
    }
}
