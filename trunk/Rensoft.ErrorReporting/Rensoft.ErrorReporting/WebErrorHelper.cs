using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Rensoft.ErrorReporting
{
    public class WebErrorHelper : ErrorHelper
    {
        private HttpRequest webRequest;

        public HttpRequest WebRequest
        {
            get { return webRequest; }
            set { webRequest = value; }
        }

        public WebErrorHelper(string serviceUrl, string clientCode, Exception exception, HttpRequest webRequest)
            : base(serviceUrl, clientCode, ((exception is HttpUnhandledException) ? exception.InnerException : exception))
        {
            this.webRequest = webRequest;
        }

        public override string CreateErrorMessage()
        {
            StringBuilder errorDetails = new StringBuilder();
            errorDetails.AppendLine("User Host: " + webRequest.UserHostName);
            errorDetails.AppendLine("User Agent: " + webRequest.UserAgent);
            errorDetails.AppendLine("Request URL: " + webRequest.Url.ToString());

            if (webRequest.UrlReferrer != null)
            {
                errorDetails.AppendLine("Referer URL: " + webRequest.UrlReferrer.ToString());
            }

            errorDetails.AppendLine();
            errorDetails.AppendLine(base.CreateErrorMessage());

            return errorDetails.ToString();
        }
    }
}
