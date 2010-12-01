using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using ManxAds;
using System.Diagnostics;
using System.ComponentModel;
using ManxAds.Search;

namespace Rensoft.ManxAds.Service
{
    public class ServiceHost
    {
        private DateTime abuseReportRunTime;
        private DateTime autoDeleteRunTime;
        private bool checkerRunning;
        private bool crawlerRunning;
        private Timer searchEngineCrawlerTimer;
        private Timer listingCheckerTimer;
        private Timer scheduleTimer;
        private List<ScheduledPayload> scheduledPayloadList;
        private ListingAbuseReportNotifier lar;
        private ListingAutoDeleteTask autoDeleteTask;
        private IEventLogHelper eventLogHelper;
        private IServiceSettings settings;
        private BackgroundWorker initialBackgroundWorker = new BackgroundWorker();

        public ServiceHost(IServiceSettings settings, IEventLogHelper eventLogHelper)
        {
            this.settings = settings;
            this.eventLogHelper = eventLogHelper;

            initialBackgroundWorker.DoWork += new DoWorkEventHandler(initialBackgroundWorker_DoWork);

            abuseReportRunTime = DateTime.Parse(settings.ListingAbuseRunTime);
            autoDeleteRunTime = DateTime.Parse(settings.ListingAutoDeleteRunTime);

            searchEngineCrawlerTimer = new Timer(settings.SearchEngineCrawlInterval);
            listingCheckerTimer = new Timer(settings.ListingCheckInterval);
            scheduleTimer = new Timer(1000);

            searchEngineCrawlerTimer.Elapsed += new ElapsedEventHandler(searchEngineCrawlerTimer_Elapsed);
            listingCheckerTimer.Elapsed += new ElapsedEventHandler(listingCheckerTimer_Elapsed);
            scheduleTimer.Elapsed += new ElapsedEventHandler(scheduleTimer_Elapsed);

            lar = new ListingAbuseReportNotifier(
                settings.ManxAdsDatabase,
                settings.WebSiteBaseUrl,
                settings.EmailServer,
                settings.EmailUsername,
                settings.EmailPassword,
                settings.EmailFromAddress,
                settings.EmailBccAddress,
                settings.WebmasterEmail);

            autoDeleteTask = new ListingAutoDeleteTask(
                settings.ManxAdsDatabase,
                new AbsolutePathUtility(settings.WebsiteAbsolutePath));

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
                    sp.RunIfNecessary();
                }
                catch (Exception ex)
                {
                    reportError(ex);
                }
            }
        }

        public void Start()
        {
            eventLogHelper.Write("Service starting.", EventLogEntryType.Information);

            List<string> timerInfos = new List<string>();

            // Run initial tasks (before timer tick).
            initialBackgroundWorker.RunWorkerAsync();
            
            if (settings.EnableCrawler)
            {
                searchEngineCrawlerTimer.Start();
                timerInfos.Add("Search engine crawler: " + searchEngineCrawlerTimer.Interval);
            }

            if (settings.EnableChecker)
            {
                listingCheckerTimer.Start();
                timerInfos.Add("Listing checker: " + listingCheckerTimer.Interval);
            }

            if (settings.EnableScheduler)
            {
                scheduleTimer.Start();

                var payloadTimes = 
                    from p in scheduledPayloadList
                    select (p.RunTime.Hour + ":" + p.RunTime.Minute);

                string payloadTimesString = string.Join(", ", payloadTimes.ToArray());

                timerInfos.Add(
                    "Scheduler: " + scheduleTimer.Interval + 
                    " (triggers: " + payloadTimesString + ")");
            }

            if (timerInfos.Count > 0)
            {
                string timerInfo = "Timers started...\r\n" + string.Join("\r\n", timerInfos.ToArray());
                eventLogHelper.Write(timerInfo, EventLogEntryType.Information);
            }
            else
            {
                eventLogHelper.Write("No timers started.", EventLogEntryType.Warning);
            }
        }

        public void Stop()
        {
            searchEngineCrawlerTimer.Stop();
            listingCheckerTimer.Stop();

            eventLogHelper.Write("Service stopped.", EventLogEntryType.Information);
        }

        private void reportError(Exception exception)
        {
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

            if (settings.EnableDebugMessages)
            {
                eventLogHelper.Write(
                    "Starting listing checker.", 
                    EventLogEntryType.Information);
            }

            ListingChecker listingChecker = new ListingChecker(
                settings.ManxAdsDatabase,
                settings.WebSiteBaseUrl,
                settings.EmailServer,
                settings.EmailUsername,
                settings.EmailPassword,
                settings.EmailFromAddress,
                settings.EmailBccAddress,
                settings.NotifySleep);

            listingChecker.NotifySet.Notify += new EventHandler<DebugEventArgs>(NotifySet_Notify);
            listingChecker.RunCheck();

            if (settings.EnableDebugMessages)
            {
                eventLogHelper.Write(
                    "Listing checker completed.",
                    EventLogEntryType.Information);
            }

            checkerRunning = false;
        }

        void NotifySet_Notify(object sender, DebugEventArgs e)
        {
            if (settings.EnableDebugMessages)
            {
                eventLogHelper.Write(e.Message, EventLogEntryType.Information);
            }
        }

        private void runSearchEngineCrawler()
        {
            crawlerRunning = true;

            if (settings.EnableDebugMessages)
            {
                eventLogHelper.Write(
                    "Starting search engine crawler.", 
                    EventLogEntryType.Information);
            }

            Catalogue catalogue = Catalogue.GenerateCatalogue(
                settings.ManxAdsDatabase);

            catalogue.UpdateKeywords();

            if (settings.EnableDebugMessages)
            {
                eventLogHelper.Write(
                    "Search engine crawler completed.", 
                    EventLogEntryType.Information);
            }

            crawlerRunning = false;
        }

        private void initialBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (settings.EnableCrawler)
                tryRunSearchEngineCrawler();

            if (settings.EnableChecker)
                tryRunListingChecker();
        }

        private void tryRunSearchEngineCrawler()
        {
            try
            {
                runSearchEngineCrawler();
            }
            catch (Exception ex)
            {
                reportError(ex);
            }
        }

        private void tryRunListingChecker()
        {
            try
            {
                runListingChecker();
            }
            catch (Exception ex)
            {
                reportError(ex);
            }
        }
    }
}
