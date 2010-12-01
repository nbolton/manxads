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
using Rensoft.ManxAds.Service;

namespace Rensoft.ManxAds.SchedulerService
{
    public partial class MainService : ServiceBase
    {
        private ServiceHost host;

        public MainService()
        {
            InitializeComponent();

            host = new ServiceHost(
                Settings.Default,
                new MaEventLogHelper("Scheduler"));
        }

        protected override void OnStart(string[] args)
        {
            host.Start();
        }

        protected override void OnStop()
        {
            host.Stop();
        }
    }
}
