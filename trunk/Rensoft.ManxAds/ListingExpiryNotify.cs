using System;
using System.Collections.Generic;
using System.Text;

using System.Net.Mail;
using System.Web.UI;
using System.IO;
using Rensoft.ManxAds.Properties;
using ManxAds;

namespace Rensoft.ManxAds
{
    internal class ListingExpiryNotify : ListingNotify
    {
        public ListingExpiryNotify(Listing listing, ListingChecker checker)
            : base(listing, checker) { }

        public override void Send()
        {
            MailMessage message = base.CreateBaseMessage();
            message.Subject = Listing.Title;
            
            message.IsBodyHtml = true;
            message.Body = string.Format(
                Resources.ListingExpiryTemplate,
                Listing.Title,
                Listing.ManxAdsId,
                Listing.GetSeller(Checker.ConnectionString).Forename,
                Checker.BaseUrl + (new Control().ResolveUrl(Listing.NavigateUrl)).Replace("~/", null),
                Checker.BaseUrl + (new Control().ResolveUrl(Listing.RemoveUrl)).Replace("~/", null),
                Checker.BaseUrl + (new Control().ResolveUrl(Listing.ModifyUrl)).Replace("~/", null));

            SendMessage(message);
        }

        public override void Update()
        {
            Listing.SetExpiryNotified(Checker.ConnectionString);
        }
    }
}
