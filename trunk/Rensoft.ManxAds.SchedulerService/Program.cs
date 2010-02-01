using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;

namespace Rensoft.ManxAds.SchedulerService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase.Run(new ServiceBase[] { new MainService() });
        }
    }
}