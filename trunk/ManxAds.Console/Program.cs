using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManxAds.DataGateway;
using ManxAds.Console.Properties;

namespace ManxAds.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Program program = new Program();
            program.run(args);
        }

        private void run(string[] args)
        {
            // override connection string for legacy code
            LocalSettings.ConnectionString = Settings.Default.Database;

            foreach (string arg in args)
            {
                if (arg == "--update-search-index")
                    updateSearchIndex();
            }

            System.Console.Read();
        }

        private void updateSearchIndex()
        {
            double totalElapsed = 0;
            int totalUpdated = 0;

            ListingDataGateway data = new ListingDataGateway(Settings.Default.Database);
            foreach (IListing listing in data.Fetch(int.MaxValue, 1))
            {
                DateTime start = DateTime.Now;

                try
                {
                    data.UpdateSearchIndex(listing);

                    TimeSpan elapsed = DateTime.Now - start;
                    totalElapsed += elapsed.TotalMilliseconds;

                    System.Console.WriteLine(string.Format(
                        "updated id: {0} in {1}ms", listing.DatabaseId, elapsed.TotalMilliseconds));
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(string.Format(
                        "failed to update id: {0}\r\n{1}", listing.DatabaseId, ex));
                }

                totalUpdated++;
            }

            System.Console.WriteLine(
                string.Format("total updated: {0} in {1}ms", totalUpdated, totalElapsed));
        }
    }
}
