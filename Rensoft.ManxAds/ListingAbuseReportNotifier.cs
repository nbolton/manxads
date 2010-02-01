using System;
using System.Collections.Generic;
using System.Text;
using ManxAds;
using System.Net.Mail;
using System.Net;
using Rensoft.ManxAds.Data;

namespace Rensoft.ManxAds
{
    public class ListingAbuseReportNotifier : NotifierTask, IScheduledTask
    {
        private string adminEmail;

        public string AdminEmail
        {
            get { return adminEmail; }
            set { adminEmail = value; }
        }

        public ListingAbuseReportNotifier(
            string connectionString, 
            string baseUrl, 
            string emailServer,
            string emailUsername,
            string emailPassword,
            string emailFromAddress,
            string emailBccAddress,
            string adminEmail)
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
            this.adminEmail = adminEmail;
        }

        public void Run()
        {
            ListingAbuseReportUtility utility = new ListingAbuseReportUtility(ConnectionString);
            List<ListingAbuseReportGroup> groupList = utility.GetReportGroups();

            if (groupList.Count != 0)
            {
                MailMessage message = utility.GenerateReportMessage(groupList);
                message.From = new MailAddress(EmailFromAddress);
                message.To.Add(new MailAddress(AdminEmail));

                SmtpClient client = new SmtpClient(EmailServer);
                client.Credentials = new NetworkCredential(EmailUsername, EmailPassword);
                client.Send(message);

                // Only after sending the email can we mark as notified.
                foreach (ListingAbuseReportGroup group in groupList)
                {
                    /* Warning: A race condition may occur here, since
                     * further votes my have been cast since the message was
                     * sent, and this marks all 'listing abuse' rows as
                     * notified for the particular listing. This isn't such
                     * a big deal since the admin is being notified anyway. */
                    utility.SetAdminNotified(group.ListingId);
                }
            }
        }
    }
}
