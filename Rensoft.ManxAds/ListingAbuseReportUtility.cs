using System;
using System.Collections.Generic;
using System.Text;
using ManxAds;
using System.Net.Mail;
using Rensoft.ManxAds.Properties;
using Rensoft.ManxAds.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Data.SqlTypes;
using System.Xml.Linq;
using System.Xml;

namespace Rensoft.ManxAds
{
    public class ListingAbuseReportUtility
    {
        private string connectionString;

        public ListingAbuseReportUtility(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Report(Listing listing, int reporterId, string reason, bool isCustom)
        {
            WebsiteUser reporter = WebsiteUser.Fetch(reporterId);
            WebsiteUser seller = WebsiteUser.Fetch(listing.SellerId);

            using (StoredProceedure sp = new StoredProceedure("ListingAbuseCreate", connectionString))
            {
                sp.AddParam("@ListingId", listing.DatabaseId);
                sp.AddParam("@ReporterId", reporterId);
                sp.AddParam("@Reason", reason);
                sp.Connection.Open();
                sp.Command.ExecuteNonQuery();
            }

            string viewLink = string.Format(
                LocalSettings.ListingViewLinkFormat2, 
                LocalSettings.WebsiteUrl, 
                listing.SearchEngineTitle, 
                listing.DatabaseId);

            string updateLink = string.Format(
                LocalSettings.ListingUpdateLinkFormat2,
                LocalSettings.WebsiteUrl,
                listing.DatabaseId);

            string deleteLink = string.Format(
                LocalSettings.ListingDeleteLinkFormat2,
                LocalSettings.WebsiteUrl,
                listing.DatabaseId);

            string messageToSellerBody = string.Format(
                Resources.ListingAbuseForSeller,
                seller.Forename,
                viewLink,
                listing.Title,
                reason,
                updateLink,
                deleteLink);

            string messageToReporterBody = string.Format(
                Resources.ListingAbuseForReporter,
                reporter.Forename,
                viewLink,
                listing.Title,
                reason);

            MailMessage messageToReporter = new MailMessage(
                LocalSettings.MasterSendFromEmail,
                reporter.EmailAddress,
                "You reported a listing",
                messageToReporterBody);

            messageToReporter.IsBodyHtml = true;

            // Send custom messages to webmaster, in case they're abusive.
            string messageToSellerTo = (isCustom ?
                LocalSettings.WebmasterEmail : seller.EmailAddress);

            MailMessage messageToSeller = new MailMessage(
                LocalSettings.MasterSendFromEmail,
                messageToSellerTo,
                "Your listing has been reported",
                messageToSellerBody);

            messageToSeller.IsBodyHtml = true;

            EmailTools.SendMessage(messageToReporter);
            EmailTools.SendMessage(messageToSeller);
        }

        public bool IsReported(int listingId, int reporterId)
        {
            using (StoredProceedure sp = new StoredProceedure(
                "ListingAbuseExists", connectionString))
            {
                sp.AddParam("@ListingId", listingId);
                sp.AddParam("@ReporterId", reporterId);
                sp.Connection.Open();
                return ((int)sp.Command.ExecuteScalar()) == 1;
            }
        }

        public void SetAdminNotified(int listingId)
        {
            using (StoredProceedure sp = new StoredProceedure(
                "ListingAbuseSetAdminNotified", connectionString))
            {
                sp.AddParam("@ListingId", listingId);
                sp.Connection.Open();
                sp.Command.ExecuteNonQuery();
            }
        }

        public List<ListingAbuseReportGroup> GetReportGroups()
        {
            SqlXml resultXml;
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand("ListingAbuseGetAsXml", connection))
            {
                connection.Open();
                resultXml = new SqlXml(command.ExecuteXmlReader());
            }

            if (string.IsNullOrEmpty(resultXml.Value))
            {
                // Return an empty list when there are no reports.
                return new List<ListingAbuseReportGroup>();
            }

            // Only create document when there is XML data.
            XDocument resultDocument = XDocument.Parse(resultXml.Value);

            var resultQuery =
                from reportGroup in resultDocument.Descendants("ReportGroup")
                select new ListingAbuseReportGroup
                {
                    ListingId = int.Parse(reportGroup.Element("ListingId").Value),
                    ListingTitle = reportGroup.Element("ListingTitle").Value,
                    SellerId = int.Parse(reportGroup.Element("SellerId").Value),
                    TotalReportCount = int.Parse(reportGroup.Element("TotalReportCount").Value),
                    ReportList = (
                        from report in reportGroup.Descendants("Report")
                        select new ListingAbuseReport()
                        {
                            ListingAbuseId = int.Parse(report.Element("ListingAbuseId").Value),
                            ListingId = int.Parse(report.Element("ListingId").Value),
                            ReporterId = int.Parse(report.Element("ReporterId").Value),
                            ReporterName = report.Element("ReporterName").Value,
                            Reason = report.Element("Reason") != null ? report.Element("Reason").Value : null
                        }
                    ).ToList(),
                    CategoryList = (
                        from category in reportGroup.Descendants("Category")
                        select category.Element("Title").Value
                    ).ToList()
                };

            return resultQuery.ToList();
        }

        public MailMessage GenerateReportMessage(List<ListingAbuseReportGroup> groupList)
        {
            const string subject = "Today's reported listings";

            const string bodyActionsFormat =
                "<a href='{0}'>Modify</a>, " +
                "<a href='{1}'>Delete</a>, " +
                "<a href='{2}'>Ban user</a>";

            const string bodyRowFormat =
                "<tr bgcolor='{5}'><td><a href='{6}'>{0}</a></td><td>{1}</td><td>{2}</td></tr>" +
                "<tr bgcolor='{5}'><td colspan='3' style='font-size: 0.8em'><b>Categories:</b> {3}</td></tr>" +
                "<tr bgcolor='{5}'><td colspan='3'>" +
                "<ul style='margin: 0; padding-left: 23px; padding-right: 6px'>{4}</ul>" +
                "</td></tr>";

            const string reportRowFormat = "<li>{0}: {1}</li>";

            string bodyRows = string.Empty;
            int rowCounter = 0;

            foreach (ListingAbuseReportGroup group in groupList)
            {
                string modifyLink = string.Format(
                    LocalSettings.ListingUpdateLinkFormat2,
                    LocalSettings.WebsiteUrl,
                    group.ListingId);

                string deleteLink = string.Format(
                    LocalSettings.ListingDeleteLinkFormat2,
                    LocalSettings.WebsiteUrl,
                    group.ListingId);

                string banLink = string.Format(
                    LocalSettings.UserBanLinkFormat,
                    LocalSettings.WebsiteUrl,
                    group.SellerId);

                string viewLink = string.Format(
                    LocalSettings.ListingViewLinkNoTitleFormat2, 
                    LocalSettings.WebsiteUrl,
                    group.ListingId);

                string reportRows = string.Empty;
                foreach (ListingAbuseReport report in group.ReportList)
                {
                    string reportRow = string.Format(
                        reportRowFormat, 
                        report.ReporterName,
                        string.IsNullOrEmpty(report.Reason) ? "No reason" : report.Reason);

                    reportRows += reportRow;
                }

                bodyRows += string.Format(
                    bodyRowFormat,
                    group.ListingTitle,
                    group.TotalReportCount,
                    string.Format(bodyActionsFormat, modifyLink, deleteLink, banLink),
                    string.Join(", ", group.CategoryList.ToArray()),
                    reportRows,
                    ((rowCounter++ % 2) == 0 ? "#ffffff" : "#f3f4ff"),
                    viewLink);
            }

            MailMessage message = new MailMessage();
            message.Body = string.Format(Resources.ListingAbuseReportEmail, bodyRows);
            message.Subject = subject;
            message.IsBodyHtml = true;
            return message;
        }
    }
}
