using System;
using System.Collections.Generic;
using System.Text;
using ManxAds;


namespace Rensoft.ManxAds
{
    public class ListingChecker : NotifierTask
    {
        private const int reminderElapedDays = 90;
        private const int deleteElapsedDays = 104;

        private ListingNotifySet notifySet;

        public ListingNotifySet NotifySet
        {
            get { return notifySet; }
        }

        public ListingChecker(
            string connectionString, 
            string baseUrl, 
            string emailServer,
            string emailUsername,
            string emailPassword,
            string emailFromAddress,
            string emailBccAddress,
            int notifySleep)
            : 
            base(
            connectionString, 
            baseUrl,
            emailServer, 
            emailUsername, 
            emailPassword, 
            emailFromAddress, 
            emailBccAddress)
        {
            this.notifySet = new ListingNotifySet(notifySleep);
        }

        public void RunCheck()
        {
            foreach (ListingBase listingBase in ListingBase.Fetch(ConnectionString))
            {
                if (listingBase.BoostDate.AddDays(deleteElapsedDays) < DateTime.Now)
                {
                    // Fetch full listing again from DB to remove ang get user (design flaw).
                    Listing listing = Listing.Fetch(listingBase.DatabaseId, ConnectionString);
                    notifySet.Add(new ListingDeleteNotify(listing, this));
                }
                else if (!listingBase.ExpiryNotified && // Only send where not sent before.
                    listingBase.BoostDate.AddDays(reminderElapedDays) < DateTime.Now)
                {
                    // Fetch full listing again from DB to get user (design flaw).
                    Listing listing = Listing.Fetch(listingBase.DatabaseId, ConnectionString);
                    notifySet.Add(new ListingExpiryNotify(listing, this));
                }
            }
            notifySet.Execute();
        }
    }
}
