using System;
using System.Collections.Generic;
using System.Text;

using System.Net.Mail;
using System.IO;
using System.Web.UI;
using Rensoft.ManxAds.Properties;
using ManxAds;

namespace Rensoft.ManxAds
{
    internal class ListingDeleteNotify : ListingNotify
    {
        public ListingDeleteNotify(Listing listing, ListingChecker checker)
            : base(listing, checker) { }

        public override void Send()
        {
            MailMessage message = base.CreateBaseMessage();
            message.Subject = Listing.Title;

            message.IsBodyHtml = true;
            message.Body = string.Format(
                Resources.ListingDeleteTemplate,
                Listing.Title,
                Listing.ManxAdsId,
                Listing.GetSeller(Checker.ConnectionString).Forename,
                Checker.BaseUrl + "Listings.aspx?Self=1&RecycleBin=1");

            SendMessage(message);
        }

        public override void Update()
        {
            Listing.Remove(Checker.ConnectionString);
        }
    }
}
