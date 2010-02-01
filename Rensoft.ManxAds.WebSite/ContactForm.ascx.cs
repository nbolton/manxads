using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Net.Mail;
using System.Net;
using System.IO;
using ManxAds;

public partial class ContactForm : StandardControl
{
    private const string emailTemplate = "~/Email/ContactTemplate.html";

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        if (!Auth.IsAuthenticated)
        {
            // Do not run page unauthenticated.
            return;
        }

        try
        {
            ContactMultiView.SetActiveView(ContactDefaultView);
            ListingTitleLabel.Text = Accessors.Listing.Title;
            SellerNameLabel.Text = Accessors.Listing.Seller.FullName;

            ReturnHyperLink.NavigateUrl = Accessors.Listing.NavigateUrl;

            if (Accessors.Listing.Seller.IsTrader)
            {
                SellerNameLabel.Text += "</b> from <b>" + Accessors.Listing.TraderName;
            }

            if (!IsPostBack && Auth.IsAuthenticated)
            {
                this.BindContactForm(Auth.ActiveUser);
            }
        }
        catch (NotFoundException)
        {
            ContactMultiView.SetActiveView(NotFoundView);
        }
    }

    protected void BindContactForm(WebsiteUser user)
    {
        NameTextBox.Text = user.FullName;
        EmailAddressTextBox.Text = user.EmailAddress;

        if (!String.IsNullOrEmpty(user.MobilePhone))
        {
            PhoneNumberTextBox.Text = user.MobilePhoneFull;
        }
        else
        {
            PhoneNumberTextBox.Text = user.LandlinePhoneFull;
        }

        ContactButton.PostBackUrl = Request.RawUrl + "#Contact";
    }

    protected void ContactButton_Click(object sender, ImageClickEventArgs e)
    {
        Page.Validate();
        if (!Page.IsValid)
        {
            ValidationErrorLabel.Visible = true;
            return;
        }

        //SendPlainTextEmail();
        SendHtmlEmail();

        ContactMultiView.SetActiveView(ContactFinishedView);
    }

    private void SendHtmlEmail()
    {
        string template = File.ReadAllText(Server.MapPath(emailTemplate));

        string baseUrl = "http://" + Request.Url.Host;
        baseUrl += (Request.Url.Port != 80 ? ":" + Request.Url.Port : string.Empty);

        string body = string.Format(
            template,
            Accessors.Listing.Title,
            Accessors.Listing.ManxAdsId,
            Accessors.Listing.Seller.Forename,
            baseUrl + (new Control().ResolveUrl(Accessors.Listing.NavigateUrl)),
            QuestionTextBox.Text,
            string.IsNullOrEmpty(NameTextBox.Text) ? "n/a" : NameTextBox.Text,
            string.IsNullOrEmpty(PhoneNumberTextBox.Text) ? "n/a" : PhoneNumberTextBox.Text,
            string.IsNullOrEmpty(EmailAddressTextBox.Text) ? "n/a" : EmailAddressTextBox.Text);

        MailMessage message = CreateBaseMessage();
        message.Body = body;
        message.IsBodyHtml = true;

        EmailTools.SendMessage(message, Page);
    }

    protected MailMessage CreateBaseMessage()
    {
        string fromEmail = EmailAddressTextBox.Text;
        if (String.IsNullOrEmpty(fromEmail))
        {
            fromEmail = LocalSettings.MasterSendFromEmail;
        }

        MailAddress from = new MailAddress(
            fromEmail, NameTextBox.Text);

        MailAddress to = new MailAddress(
            Accessors.Listing.Seller.EmailAddress,
            Accessors.Listing.Seller.FullName);

        MailAddress sender = new MailAddress(
            LocalSettings.MasterSendFromEmail, "ManxAds.com");

        MailMessage message = new MailMessage(from, to);
        message.Sender = sender;

        message.Subject = String.Format(
            LocalSettings.ListingContactFormat,
            Accessors.Listing.ManxAdsId,
            Accessors.Listing.Title);

        return message;
    }

    protected void CheckPhoneOrEmail(object source, ServerValidateEventArgs args)
    {
        if (String.IsNullOrEmpty(EmailAddressTextBox.Text) &&
            String.IsNullOrEmpty(PhoneNumberTextBox.Text))
        {
            args.IsValid = false;
        }
    }
}
