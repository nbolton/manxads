using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Rensoft.ManxAds
{
    public class ScheduledPayload
    {
        private DateTime runTime;
        private IScheduledTask task;
        private bool running;

        public event EventHandler TaskRunning;
        public event EventHandler TaskCompleted;

        public DateTime RunTime
        {
            get { return runTime; }
        }

        public ScheduledPayload(DateTime runTime, IScheduledTask task)
        {
            this.runTime = runTime;
            this.task = task;
        }
        
        public void RunIfNeccecary()
        {
            if (!running &&
                (DateTime.Now.Hour == runTime.Hour) &&
                (DateTime.Now.Minute == runTime.Minute))
            {
                // Semaphore to stop it running more than once.
                running = true;

                OnTaskRunning();
                task.Run();
                OnTaskCompleted();

                // Sleep for 1 minuite so task isn't repeated several times during the minuite.
                Thread.Sleep(TimeSpan.FromMinutes(1));

                running = false;
            }
        }

        private void OnTaskRunning()
        {
            if (TaskRunning != null)
            {
                TaskRunning(this, EventArgs.Empty);
            }
        }

        private void OnTaskCompleted()
        {
            if (TaskCompleted != null)
            {
                TaskCompleted(this, EventArgs.Empty);
            }
        }
    }
}
