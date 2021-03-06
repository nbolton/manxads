using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net.Mail;

using System.Text;
using Rensoft.ErrorReporting;
using System.Diagnostics;
using ManxAds;

/// <summary>
/// Provides a means of reporting errors by email.
/// </summary>
public class ErrorReporting
{
    /// <summary>
    /// Gets the recipient of the error report.
    /// </summary>
    protected static string ErrorReportingRecipient
    {
        get
        {
            return ConfigurationManager.AppSettings["ErrorReportingRecipient"];
        }
    }

    /// <summary>
    /// Gets the enabled status of error reporting.
    /// </summary>
    protected static bool EnableErrorReporting
    {
        get
        {
            return bool.Parse(
                ConfigurationManager.AppSettings["EnableErrorReporting"]);
        }
    }

    /// <summary>
    /// If enabled in the application configuration, a report of
    /// the exception will be emailed to the ErrorReportingRecipient
    /// found in the application configuration.
    /// </summary>
    /// <param name="exception">Exception which to report.</param>
    /// <param name="request">Server request for extra details.</param>
    /// <returns>True if error was reported.</returns>
    [Obsolete()]
    public static bool ReportErrorByEmail(
        Exception exception, Page page)
    {
        return ReportErrorByEmail(exception, page, null);
    }
    

    /// <summary>
    /// If enabled in the application configuration, a report of
    /// the exception will be emailed to the ErrorReportingRecipient
    /// found in the application configuration.
    /// </summary>
    /// <param name="exception">Exception which to report.</param>
    /// <param name="request">Server request for extra details.</param>
    /// <param name="auth">Provider of user information.</param>
    /// <returns>True if error was reported.</returns>
    public static bool ReportErrorByEmail(
        Exception exception, Page page, Authentication auth)
    {
        // Return if error reporting disabled.
        if (!EnableErrorReporting) return false;

        // Save origional reference for later use.
        Exception origionalException = exception;
        StringBuilder errorMessage = new StringBuilder();

        errorMessage.AppendLine("User Host: " + page.Request.UserHostName);
        errorMessage.AppendLine("User Agent: " + page.Request.UserAgent);
        errorMessage.AppendLine("Request URL: " + page.Request.Url.ToString());

        if (page.Request.UrlReferrer != null)
        {
            errorMessage.AppendLine("Referer URL: " + page.Request.UrlReferrer.ToString());
        }
        
        if ((auth != null) && auth.IsAuthenticated)
        {
            errorMessage.AppendLine(
                "Active User: " + auth.ActiveUser.FullName +
                " (" + auth.ActiveUser.EmailAddress + ")");
        }

        while (exception != null)
        {
            errorMessage.AppendLine();
            errorMessage.AppendLine("Error Message: " + exception.Message.ToString());

            if (exception.StackTrace != null)
            {
                errorMessage.AppendLine("Stack Trace:");
                errorMessage.AppendLine(exception.StackTrace.ToString());
            }

            // Change reference to inner ex to recurse.
            exception = exception.InnerException;
        }

        MailMessage message = new MailMessage(
            LocalSettings.MasterSendFromEmail,
            LocalSettings.ErrorReportingRecipient,
            "Website Error: " + origionalException.GetType().Name,
            errorMessage.ToString());

        EmailTools.SendMessage(message, page);

        return true;
    }

    public static void ReportErrorByWebService(
        Exception exception, HttpRequest request, HttpResponse response)
    {
        if (exception is HttpUnhandledException)
        {
            // Replace exception sender with the actual excepton.
            exception = exception.InnerException;
        }

        if (checkInvalidRequest(exception))
        {
            response.Redirect("~/InvalidRequest.aspx", true);
            return;
        }

        if (exception is AuthenticationException)
        {
            AuthenticationException authEx = exception as AuthenticationException;
            string user = authEx.AuthHandler.ActiveUser.DisplayName;
            exception = new Exception("User '" + user + "' was redirected to login page.", authEx);

            // Log off and redirect to login page instead of error page.
            authEx.AuthHandler.LogOff(false);
        }

        string url = ConfigurationManager.AppSettings["ErrorReportServiceUrl"];
        WebErrorHelper helper = new WebErrorHelper(url, "MNXD", exception, request);
        helper.SendReport();
    }

    private static bool checkInvalidRequest(Exception exception)
    {
        string[] invalidRequestMessages = new string[] {
            "A potentially dangerous Request.Form value",
            "The state information is invalid",
            "Validation of viewstate MAC failed"
        };

        // Where any message part matches, indicate invalid request.
        foreach (string message in invalidRequestMessages)
        {
            if (exception.Message.Contains(message))
            {
                return true;
            }
        }
        return false;
    }
}
