using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Rensoft.ManxAds
{
    public class ListingNotifySet
    {
        private int waitTime;
        private List<ListingNotify> list;

        public event EventHandler<DebugEventArgs> Notify;

        public ListingNotifySet(int waitTime)
        {
            this.waitTime = waitTime;
            this.list = new List<ListingNotify>();
        }

        public void Add(ListingNotify notify)
        {
            list.Add(notify);
        }

        public void Execute()
        {
            foreach (ListingNotify notify in list)
            {
                if (notify is ListingExpiryNotify)
                {
                    OnNotify(new DebugEventArgs(
                        "Sending expiry notification for listing " + 
                        "with ID " + notify.Listing.DatabaseId + "."));
                }
                else if (notify is ListingDeleteNotify)
                {
                    OnNotify(new DebugEventArgs(
                        "Sending delete notification for listing " +
                        "with ID " + notify.Listing.DatabaseId + "."));
                }
                else
                {
                    throw new NotSupportedException(
                        "Support not available for derrived type (" + notify.GetType().Name + ").");
                }

                // Send the email.
                notify.Send();

                // Update the listing.
                notify.Update();

                // Do not hammer mail server.
                Thread.Sleep(waitTime);
            }
        }

        protected virtual void OnNotify(DebugEventArgs e)
        {
            if (Notify != null) Notify(this, e);
        }
    }
}
