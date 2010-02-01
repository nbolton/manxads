using System;
using System.Collections.Generic;
using System.Text;

using System.Net.Mail;
using System.Web;
using System.Net;
using System.Data.SqlClient;
using System.Data;
using ManxAds;

namespace Rensoft.ManxAds
{
    public abstract class ListingNotify
    {
        private Listing listing;
        private ListingChecker checker;

        public Listing Listing
        {
            get { return listing; }
        }

        public ListingChecker Checker
        {
            get { return checker; }
        }

        protected ListingNotify(Listing listing, ListingChecker checker)
        {
            this.listing = listing;
            this.checker = checker;
        }

        protected MailMessage CreateBaseMessage()
        {
            MailAddress from = new MailAddress(
                checker.EmailFromAddress, "ManxAds.com Reminders");

            MailAddress to = new MailAddress(
                listing.GetSeller(checker.ConnectionString).EmailAddress,
                listing.GetSeller(checker.ConnectionString).FullName);

            MailMessage message = new MailMessage(from, to);

            if (!string.IsNullOrEmpty(checker.EmailBccAddress))
            {
                message.Bcc.Add(checker.EmailBccAddress);
            }

            return message;
        }

        public void SendMessage(MailMessage message)
        {
            SmtpClient client = new SmtpClient(checker.EmailServer);

            client.Credentials = new NetworkCredential(
                checker.EmailUsername, checker.EmailPassword);

            client.Send(message);
            writeToLog(message);
        }

        private void writeToLog(MailMessage message)
        {
            using (SqlConnection connection = new SqlConnection(checker.ConnectionString))
            {
                SqlCommand command = new SqlCommand("EmailLogWrite", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("toEmail", (message.To.Count != 0) ? message.To[0].Address : (object)DBNull.Value);
                command.Parameters.AddWithValue("fromEmail", (message.From != null) ? message.From.Address : (object)DBNull.Value);
                command.Parameters.AddWithValue("senderEmail", (message.Sender != null) ? message.Sender.Address : (object)DBNull.Value);
                command.Parameters.AddWithValue("subject", message.Subject);
                command.Parameters.AddWithValue("body", message.Body);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public abstract void Send();
        public abstract void Update();
    }
}
