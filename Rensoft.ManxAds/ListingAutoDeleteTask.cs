using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManxAds;

namespace Rensoft.ManxAds
{
    public class ListingAutoDeleteTask : IScheduledTask
    {
        private const int staleDayCount = 30;

        private string connectionString;
        private PathUtility pathUtility;

        public ListingAutoDeleteTask(string connectionString, PathUtility pathUtility)
        {
            this.connectionString = connectionString;
            this.pathUtility = pathUtility;
        }

        public void Run()
        {
            Listing.RemoveStaleRecycleBinItems(
                connectionString,
                pathUtility, 
                staleDayCount);
        }
    }
}
