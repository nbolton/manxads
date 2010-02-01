using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;

namespace Rensoft.ManxAds.SchedulerService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        private void serviceInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
            serviceController.Start();
        }
    }
}