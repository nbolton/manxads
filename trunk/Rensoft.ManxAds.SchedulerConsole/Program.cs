using System;
using System.Collections.Generic;
using System.Text;
using Rensoft.ManxAds.SchedulerConsole.Properties;
using ManxAds.Search;

namespace Rensoft.ManxAds.SchedulerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            if ((args.Length == 0) || (args[0] == "/?"))
            {
                Console.WriteLine(
                    "You must specify a parameter:\r\n" +
                    "/checker\tListing Checker\r\n" +
                    "/crawler\tSearch Engine Crawler.");
            }
            else if (args[0] == "/checker")
            {
                runListingChecker();
            }
            else if (args[0] == "/crawler")
            {
                runSearchEngineCrawler();
            }
            else
            {
                Console.WriteLine("Unrecognised parameter. Use /? for help.");
            }
        }

        private static void runListingChecker()
        {
            ListingChecker listingChecker = new ListingChecker(
                Settings.Default.ManxAdsDatabase,
                Settings.Default.WebSiteBaseUrl,
                Settings.Default.EmailServer,
                Settings.Default.EmailUsername,
                Settings.Default.EmailPassword,
                Settings.Default.EmailFromAddress,
                Settings.Default.EmailBccAddress,
                Settings.Default.NotifySleep);

            listingChecker.NotifySet.Notify += new EventHandler<DebugEventArgs>(NotifySet_Notify);

            Console.WriteLine("Starting listing checker.");
            listingChecker.RunCheck();
            Console.WriteLine("Listing checker completed.");
        }

        static void NotifySet_Notify(object sender, DebugEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

        private static void runSearchEngineCrawler()
        {
            Console.WriteLine("Starting search engine crawler.");

            Catalogue catalogue = Catalogue.GenerateCatalogue(
                Settings.Default.ManxAdsDatabase);

            catalogue.UpdateKeywords();

            Console.WriteLine("Search engine crawler completed.");
        }
    }
}
