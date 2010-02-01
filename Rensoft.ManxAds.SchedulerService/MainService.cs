using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using Rensoft.ManxAds.SchedulerService.Properties;
using System.Timers;
using ManxAds.Search;
using System.Linq;
using ManxAds;

namespace Rensoft.ManxAds.SchedulerService
{
    public partial class MainService : ServiceBase
    {
        private static DateTime abuseReportRunTime = DateTime.Parse(Settings.Default.ListingAbuseRunTime);
        private static DateTime autoDeleteRunTime = DateTime.Parse(Settings.Default.ListingAutoDeleteRunTime);

        private bool checkerRunning;
        private bool crawlerRunning;
        private Timer searchEngineCrawlerTimer;
        private Timer listingCheckerTimer;
        private Timer scheduleTimer;
        private List<ScheduledPayload> scheduledPayloadList;
        private ListingAbuseReportNotifier lar;
        private ListingAutoDeleteTask autoDeleteTask;

        private MaEventLogHelper eventLogHelper;

        public MainService()
        {
            InitializeComponent();

            eventLogHelper = new MaEventLogHelper("Scheduler");

            searchEngineCrawlerTimer = new Timer(Settings.Default.SearchEngineCrawlInterval);
            listingCheckerTimer = new Timer(Settings.Default.ListingCheckInterval);
            scheduleTimer = new Timer(1000);

            searchEngineCrawlerTimer.Elapsed += new ElapsedEventHandler(searchEngineCrawlerTimer_Elapsed);
            listingCheckerTimer.Elapsed += new ElapsedEventHandler(listingCheckerTimer_Elapsed);
            scheduleTimer.Elapsed += new ElapsedEventHandler(scheduleTimer_Elapsed);

            lar = new ListingAbuseReportNotifier(
                Settings.Default.ManxAdsDatabase,
                Settings.Default.WebSiteBaseUrl,
                Settings.Default.EmailServer,
                Settings.Default.EmailUsername,
                Settings.Default.EmailPassword,
                Settings.Default.EmailFromAddress,
                Settings.Default.EmailBccAddress,
                Settings.Default.WebmasterEmail);

            autoDeleteTask = new ListingAutoDeleteTask(
                Settings.Default.ManxAdsDatabase,
                new AbsolutePathUtility(Settings.Default.WebsiteAbsolutePath));

            ScheduledPayload larPayload = new ScheduledPayload(abuseReportRunTime, lar);
            larPayload.TaskRunning += new EventHandler(larPayload_TaskRunning);
            larPayload.TaskCompleted += new EventHandler(larPayload_TaskCompleted);

            ScheduledPayload autoDeleteTaskPayload = new ScheduledPayload(
                autoDeleteRunTime, autoDeleteTask);
            autoDeleteTaskPayload.TaskRunning += new EventHandler(autoDeleteTaskPayload_TaskRunning);
            autoDeleteTaskPayload.TaskCompleted += new EventHandler(autoDeleteTaskPayload_TaskCompleted);

            scheduledPayloadList = new List<ScheduledPayload>();
            scheduledPayloadList.Add(larPayload);
            scheduledPayloadList.Add(autoDeleteTaskPayload);
        }

        void autoDeleteTaskPayload_TaskCompleted(object sender, EventArgs e)
        {
            eventLogHelper.Write("Auto delete task completed.", EventLogEntryType.Information);
        }

        void autoDeleteTaskPayload_TaskRunning(object sender, EventArgs e)
        {
            eventLogHelper.Write("Auto delete task running.", EventLogEntryType.Information);
        }

        void larPayload_TaskCompleted(object sender, EventArgs e)
        {
            eventLogHelper.Write("Listing abuse reporter completed.", EventLogEntryType.Information);
        }

        void larPayload_TaskRunning(object sender, EventArgs e)
        {
            eventLogHelper.Write("Listing abuse reporter running.", EventLogEntryType.Information);
        }

        void scheduleTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            foreach (ScheduledPayload sp in scheduledPayloadList)
            {
                try
                {
                    sp.RunIfNeccecary();
                }
                catch (Exception ex)
                {
                    reportError(ex);
                }
            }
        }

        protected override void OnStart(string[] args)
        {
            eventLogHelper.Write("Service starting.", EventLogEntryType.Information);

            // Run initial tasks (before timer tick).
            initialBackgroundWorker.RunWorkerAsync();

            // Start timers (delay of 1 interval).
            searchEngineCrawlerTimer.Start();
            listingCheckerTimer.Start();
            scheduleTimer.Start();

            string searchEngineCrawlerEnabled = searchEngineCrawlerTimer.Enabled ? "Enabled" : "Disabled";
            string listingCheckerEnabled = listingCheckerTimer.Enabled ? "Enabled" : "Disabled";

            var payloadTimes = (from p in scheduledPayloadList select 
                                    (p.RunTime.Hour + ":" + p.RunTime.Minute));
            string payloadTimesString = string.Join(", ", payloadTimes.ToArray());

            string timerInfo =
                "Timers started...\r\n" +
                "Search Engine Crawler: " + searchEngineCrawlerTimer.Interval +
                " (" + searchEngineCrawlerEnabled + ")\r\n" +
                "Listing Checker: " + listingCheckerTimer.Interval +
                " (" + listingCheckerEnabled + ")\r\n" +
                "Scheduler: " + scheduleTimer.Interval +
                " (Trigger times: " + payloadTimesString + ")";

            eventLogHelper.Write(timerInfo, EventLogEntryType.Information);
        }

        protected override void OnStop()
        {
            searchEngineCrawlerTimer.Stop();
            listingCheckerTimer.Stop();

            eventLogHelper.Write("Service stopped.", EventLogEntryType.Information);
        }

        private void reportError(Exception exception)
        {
            /*ErrorHelper errorHelper = new ErrorHelper(
                Settings.Default.ErrorHelperWebServiceUrl, "MNXD", exception);

            // Write to event log first (less likely to fail).
            this.EventLog.WriteEntry(errorHelper.ErrorMessage, EventLogEntryType.Error, 4);

            errorHelper.SendReport();*/

            eventLogHelper.Write(exception);
        }

        void searchEngineCrawlerTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (!crawlerRunning)
                {
                    runSearchEngineCrawler();
                }
                else
                {
                    eventLogHelper.Write(
                        "Search Engine Crawler skipped (already running).",
                        EventLogEntryType.Warning);
                }
            }
            catch (Exception ex)
            {
                reportError(ex);
            }
        }

        void listingCheckerTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (!checkerRunning)
                {
                    runListingChecker();
                }
                else
                {
                    eventLogHelper.Write(
                        "Listing Checker skipped (already running).", 
                        EventLogEntryType.Warning);
                }
            }
            catch (Exception ex)
            {
                reportError(ex);
            }
        }

        private void runListingChecker()
        {
            checkerRunning = true;

            if (Settings.Default.EnableDebugMessages)
            {
                eventLogHelper.Write(
                    "Starting listing checker.", 
                    EventLogEntryType.Information);
            }

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
            listingChecker.RunCheck();

            if (Settings.Default.EnableDebugMessages)
            {
                eventLogHelper.Write(
                    "Listing checker completed.",
                    EventLogEntryType.Information);
            }

            checkerRunning = false;
        }

        void NotifySet_Notify(object sender, DebugEventArgs e)
        {
            if (Settings.Default.EnableDebugMessages)
            {
                eventLogHelper.Write(e.Message, EventLogEntryType.Information);
            }
        }

        private void runSearchEngineCrawler()
        {
            crawlerRunning = true;

            if (Settings.Default.EnableDebugMessages)
            {
                eventLogHelper.Write(
                    "Starting search engine crawler.", 
                    EventLogEntryType.Information);
            }

            Catalogue catalogue = Catalogue.GenerateCatalogue(
                Settings.Default.ManxAdsDatabase);

            catalogue.UpdateKeywords();

            if (Settings.Default.EnableDebugMessages)
            {
                eventLogHelper.Write(
                    "Search engine crawler completed.", 
                    EventLogEntryType.Information);
            }

            crawlerRunning = false;
        }

        private void initialBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Run both for the first time on start.
            runSearchEngineCrawler();
            runListingChecker();
        }

        private void initialBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                reportError(e.Error);
                Stop();
            }
        }
    }
}
