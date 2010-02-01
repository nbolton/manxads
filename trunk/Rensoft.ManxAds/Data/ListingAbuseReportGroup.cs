using System;
using System.Collections.Generic;
using System.Text;

namespace Rensoft.ManxAds.Data
{
    public class ListingAbuseReportGroup
    {
        public int ListingId { get; set; }
        public string ListingTitle { get; set; }
        public int SellerId { get; set; }
        public int TotalReportCount { get; set; }
        public List<ListingAbuseReport> ReportList { get; set; }
        public List<string> CategoryList { get; set; }
    }
}
