using System;
using System.Collections.Generic;
using System.Text;

namespace Rensoft.ManxAds
{
    public class NotifierTask
    {
        private string connectionString;
        private string baseUrl;
        private string emailServer;
        private string emailUsername;
        private string emailPassword;
        private string emailFromAddress;
        private string emailBccAddress;

        public string ConnectionString
        {
            get { return connectionString; }
        }

        public string BaseUrl
        {
            get { return baseUrl; }
        }

        public string EmailServer
        {
            get { return emailServer; }
        }

        public string EmailUsername
        {
            get { return emailUsername; }
        }

        public string EmailPassword
        {
            get { return emailPassword; }
        }

        public string EmailFromAddress
        {
            get { return emailFromAddress; }
        }

        public string EmailBccAddress
        {
            get { return emailBccAddress; }
        }

        public NotifierTask(
            string connectionString, 
            string baseUrl, 
            string emailServer,
            string emailUsername,
            string emailPassword,
            string emailFromAddress,
            string emailBccAddress)
        {
            this.connectionString = connectionString;
            this.baseUrl = baseUrl;
            this.emailServer = emailServer;
            this.emailUsername = emailUsername;
            this.emailPassword = emailPassword;
            this.emailFromAddress = emailFromAddress;
            this.emailBccAddress = emailBccAddress;
        }
    }
}
