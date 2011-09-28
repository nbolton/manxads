using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Net.Mail;
using Rensoft.ErrorReporting.WebService.Properties;
using System.Net;

namespace Rensoft.ErrorReporting.WebService
{
    [WebService(Namespace = "http://schemas.rensoft.net/ErrorReporting")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class ReportingService : System.Web.Services.WebService
    {
        [WebMethod]
        public void Report(
            string clientCode, string userComments, string exceptionType,
            string exceptionDetails, string userName, string machineName)
        {
            MailMessage message = new MailMessage(Settings.Default.FromEmail, Settings.Default.ToEmail);
            message.Subject = "Error Report (" + clientCode + ") - " + exceptionType;

            message.Body = "Client: " + clientCode + "\r\n"; 
            message.Body += "Exception: " + exceptionType + "\r\n"; 
            message.Body += "Username: " + userName + "\r\n";
            message.Body += "Machine: " + machineName + "\r\n\r\n";

            if (!string.IsNullOrEmpty(userComments))
            {
                message.Body += "User Comments:\r\n";
                message.Body += userComments + "\r\n\r\n";
            }

            message.Body += exceptionDetails;

            SmtpClient smtpClient = new SmtpClient(Settings.Default.SmtpServer);

            smtpClient.Credentials = new NetworkCredential(
                Settings.Default.SmtpUsername,
                Settings.Default.SmtpPassword);

            smtpClient.Send(message);
        }
    }
}
