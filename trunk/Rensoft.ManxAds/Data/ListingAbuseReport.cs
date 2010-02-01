using System;
using System.Collections.Generic;
using System.Text;

namespace Rensoft.ManxAds.Data
{
    public class ListingAbuseReport
    {
        public int ListingAbuseId { get; set; }
        public int ListingId { get; set; }
        public int ReporterId { get; set; }
        public string ReporterName { get; set; }
        public string Reason { get; set; }
    }
}
