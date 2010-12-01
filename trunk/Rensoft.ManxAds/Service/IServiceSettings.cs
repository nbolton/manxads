using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rensoft.ManxAds.Service
{
    public interface IServiceSettings
    {
        string ListingAbuseRunTime
        {
            get;
        }

        string ListingAutoDeleteRunTime
        {
            get;
        }

        int SearchEngineCrawlInterval
        {
            get;
        }

        int ListingCheckInterval
        {
            get;
        }

        string ManxAdsDatabase
        {
            get;
        }

        string WebSiteBaseUrl
        {
            get;
        }

        string EmailServer
        {
            get;
        }

        string EmailUsername
        {
            get;
        }

        string EmailPassword
        {
            get;
        }

        string EmailFromAddress
        {
            get;
        }

        string EmailBccAddress
        {
            get;
        }

        string WebmasterEmail
        {
            get;
        }

        string WebsiteAbsolutePath
        {
            get;
        }

        bool EnableDebugMessages
        {
            get;
        }

        int NotifySleep
        {
            get;
        }

        bool EnableCrawler
        {
            get;
        }

        bool EnableChecker
        {
            get;
        }

        bool EnableScheduler
        {
            get;
        }
    }
}
