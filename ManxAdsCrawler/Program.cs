using System;
using System.Collections.Generic;
using System.Text;

using ManxAds.Search;
using System.Configuration;
using System.Reflection;
using System.Net.Mail;
using System.IO;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;
using ManxAds;

namespace ManxAdsCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            updateKeywords();
#else
            updateKeywordsReportError();
#endif
        }

        private static void updateKeywords()
        {
            Console.WriteLine("ManxAds Crawler " + Assembly.GetExecutingAssembly().GetName().Version);
            Console.WriteLine();

            Console.WriteLine("Indexing listings... ");
            Console.WriteLine();

            Catalogue catalogue = Catalogue.GenerateCatalogue(LocalSettings.ConnectionString);

            Console.WriteLine();
            Console.WriteLine("Got " + catalogue.KeywordCount + " keywords.");

            Console.WriteLine();
            Console.Write("Publishing to database... ");
            catalogue.UpdateKeywords();

            Console.WriteLine("Done");
        }

        private static void updateKeywordsReportError()
        {
            try
            {
                updateKeywords();
            }
            catch (Exception ex)
            {
                string host = ConfigurationManager.AppSettings["DefaultMailServer"];
                string from = ConfigurationManager.AppSettings["DefaultFromEmail"];
                string to = ConfigurationManager.AppSettings["ErrorNotifyEmail"];

                SmtpClient client = new SmtpClient(host);
                client.Send(from, to, "Crawler Error: " + ex.GetType().ToString(), ex.ToString());

                throw ex;
            }
        }
    }
}
