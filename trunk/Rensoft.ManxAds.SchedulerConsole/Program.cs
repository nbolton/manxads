using System;
using System.Collections.Generic;
using System.Text;
using Rensoft.ManxAds.SchedulerConsole.Properties;
using ManxAds.Search;
using Rensoft.ManxAds.Service;
using System.Diagnostics;

namespace Rensoft.ManxAds.SchedulerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host = new ServiceHost(
                Settings.Default, new ConsoleEventLogHelper());

            host.Start();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();

            host.Stop();
        }

        public class ConsoleEventLogHelper : IEventLogHelper
        {
            #region IEventLogHelper Members

            public void Write(string message, EventLogEntryType eventLogEntryType)
            {
                Console.WriteLine(eventLogEntryType.ToString() + ": " + message);
            }

            public void Write(Exception exception)
            {
                Write(exception.ToString(), EventLogEntryType.Error);
            }

            #endregion
        }
    }
}
