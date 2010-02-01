using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Web.SessionState;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Web.UI;

namespace ManxAds
{
    public class Security
    {
        public static bool Authenticate(
            string emailAddress,
            string password,
            string ipAddress,
            out WebsiteUser user)
        {
            int userId = 0;
            WebsiteUserType userType;

            using (StoredProceedure sp = new StoredProceedure("Authenticate"))
            {
                sp.AddParam("@EmailAddress", emailAddress);
                sp.AddParam("@Password", ((Password)password).Encrypted);
                sp.AddParam("@IpAddress", ipAddress);
                sp.AddParam("@UserId", SqlDbType.Int);
                sp.AddParam("@UserType", SqlDbType.Int);

                sp.Connection.Open();
                sp.Command.ExecuteNonQuery();

                userId = sp.GetParamValue<int>("@UserId");
                userType = sp.GetParamValue<WebsiteUserType>("@UserType");
            }

            if (userId != 0)
            {
                // Set user out param for session.
                user = WebsiteUser.Fetch(userId);
                return true;
            }

            user = null;
            return false;
        }

        public static bool RequestPasswordReset(
            string emailAddress, Page page)
        {
            if (!ManxAds.WebsiteUser.EmailInUse(emailAddress))
            {
                return false;
            }

            string authCode = WebsiteUser.GetUniqueAuthCode(emailAddress);
            string hostname = page.Request.UserHostAddress;
            string ipAddress = page.Request.UserHostName;

            using (StoredProceedure sp = new StoredProceedure("PasswordResetRequest"))
            {
                sp.AddParam("@AuthCode", authCode);
                sp.AddParam("@Hostname", hostname);
                sp.AddParam("@IPAddress", ipAddress);
                sp.AddParam("@EmailAddress", emailAddress);

                sp.Connection.Open();
                if (sp.Command.ExecuteNonQuery() <= 0)
                {
                    return false;
                }
            }

            string messageBody = "Hello,\r\n\r\n" +
                
                "You have been send this message from the Password Recovery " +
                "service on the ManxAds website. If you did not request this " +
                "email, then you can safely ignore it.\r\n\r\n" +
                "If you would like to reset your ManxAds password, " +
                "please follow this link...\r\n\r\n" +
                page.Request.Url.OriginalString + "?AuthCode=" + authCode;

            MailMessage message = new MailMessage(
                LocalSettings.MasterSendFromEmail, emailAddress,
                "Password Recovery", messageBody);

            EmailTools.SendMessage(message, page);
            
            return true;
        }

        /// <summary>
        /// Returns a user where an authentication code is found.
        /// </summary>
        /// <param name="authCode">Authentication code.</param>
        /// <exception cref="NotFoundException">
        /// Thrown if the authentication code was invalid.
        /// </exception>
        /// <returns>
        /// The fetched user ID where the auth code was valid.
        /// </returns>
        public static int CompletePasswordReset(string authCode)
        {
            using (StoredProceedure sp = new StoredProceedure("PasswordResetComplete"))
            {
                sp.AddParam("@AuthCode", authCode);
                sp.AddParam("@UserId", SqlDbType.Int);

                sp.Connection.Open();
                sp.Command.ExecuteNonQuery();

                return sp.GetParamValue<int>("@UserId");
            }
        }
    }
}
