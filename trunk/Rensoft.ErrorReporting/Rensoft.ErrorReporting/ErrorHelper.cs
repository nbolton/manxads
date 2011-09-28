using System;
using System.Net;
using Rensoft.ErrorReporting.ErrorReportingWebService;
using System.Diagnostics;

namespace Rensoft.ErrorReporting
{
    public class ErrorHelper
    {
        private string serviceUrl;
        private string clientCode;
        private Exception exception;
        private string userName;
        private string machineName;
        private IWebProxy proxy;
        private string userComments;
        private string customMessage;
        private bool useSystemProxy;

        public string ServiceUrl
        {
            get { return serviceUrl; }
        }

        public string ClientCode
        {
            get { return clientCode; }
        }

        public Exception Exception
        {
            get { return exception; }
        }

        public string ErrorMessage
        {
            get
            {
                if (string.IsNullOrEmpty(customMessage))
                {
                    return CreateErrorMessage();
                }
                return customMessage;
            }
            set { customMessage = value; }
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        public string MachineName
        {
            get { return machineName; }
            set { machineName = value; }
        }

        public IWebProxy Proxy
        {
            get { return proxy; }
        }

        public string UserComments
        {
            get { return userComments; }
            set { userComments = value; }
        }

        public bool UseSystemProxy
        {
            get { return useSystemProxy; }
        }

        public ErrorHelper(string serviceUrl, string clientCode, Exception exception)
            : this(serviceUrl, clientCode, exception, WebRequest.GetSystemWebProxy())
        {
            this.useSystemProxy = true;
        }

        public ErrorHelper(string serviceUrl, string clientCode, Exception exception, IWebProxy proxy)
        {
            this.serviceUrl = serviceUrl;
            this.clientCode = clientCode;
            this.exception = exception;
            this.proxy = proxy;

            this.userName = Environment.UserName;
            this.machineName = Environment.MachineName;
        }

        [Obsolete("Create an instance of ErrorHelper instead.")]
        public static string CreateExceptionDetails(Exception exception)
        {
            ErrorHelper helper = new ErrorHelper(null, null, exception);
            return helper.CreateErrorMessage();
        }

        public virtual string CreateErrorMessage()
        {
            string errorDetails = string.Empty;
            Exception currentException = exception;
            while (currentException != null)
            {
                errorDetails += "Type: " + currentException.GetType().Name + "\r\n";
                errorDetails += "Message: " + currentException.Message + "\r\n";
                errorDetails += "Stack Trace:\r\n" + currentException.StackTrace + "\r\n\r\n";

                currentException = currentException.InnerException;
            }
            return errorDetails;
        }

        public void WriteToEventLog(string logName, string appName) 
        {
            if (!EventLog.SourceExists(appName))
            {
                EventLog.CreateEventSource(appName, logName);
            }

            EventLog.WriteEntry(
                appName, CreateErrorMessage(),
                EventLogEntryType.Error, 1);
        }

        public void SendReport()
        {
            ReportingService service = new ReportingService();
            service.Url = serviceUrl;

            service.Report(
                clientCode, 
                userComments, 
                exception.GetType().Name,
                ErrorMessage, 
                userName, 
                machineName);
        }

        [Obsolete("Create instance of ErrorHelper instead.")]
        public static void LaunchReporter(
            Exception exception, string clientCode, string serviceUrl)
        {
            LaunchReporter(exception, clientCode, serviceUrl, null);
        }

        [Obsolete("Create instance of ErrorHelper instead.")]
        public static void LaunchReporter(
            Exception exception, string clientCode, string serviceUrl, IWebProxy proxy)
        {
            FormErrorHelper helper = new FormErrorHelper(serviceUrl, clientCode, exception);
            helper.proxy = proxy;
            helper.LaunchReporter();
        }

        [Obsolete("Create instance of ErrorHelper instead.")]
        public static void LaunchReporter(
            Exception exception, string clientCode, string serviceUrl, bool useSystemProxy)
        {
            if (!useSystemProxy)
            {
                throw new ArgumentException("Use system proxy cannot be false.", "useSystemProxy");
            }

            LaunchReporter(exception, clientCode, serviceUrl, WebRequest.DefaultWebProxy);
        }
    }
}
