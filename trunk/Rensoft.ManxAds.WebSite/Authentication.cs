using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.SessionState;
using System.Security.Principal;
using ManxAds;


public class AuthenticationException : Exception
{
    public Authentication AuthHandler;

    public AuthenticationException(
        string message, Authentication authHandler)
        : base(message)
    {
        this.AuthHandler = authHandler;
    }

    public AuthenticationException(
        string message, Authentication authHandler, Exception innerException)
        : base(message, innerException)
    {
        this.AuthHandler = authHandler;
    }
}

/// <summary>
/// Provides helper methods for authentication using a Page.
/// </summary>
public class Authentication
{
    private HttpSessionState session;
    private FormsIdentity identity;
    private HttpResponse response;

    public bool IsAuthenticated
    {
        get
        {
            if (identity != null)
            {
                return identity.IsAuthenticated;
            }
            return false;
        }
    }

    /// <summary>
    /// Gets or sets the currently authenticated user in session. Should not be
    /// called unless IsAuthenticated is True.
    /// </summary>
    public WebsiteUser ActiveUser
    {
        get
        {
            if (!IsAuthenticated)
            {
                throw new AuthenticationException(
                    "Active user called when not authenticated.", this);
            }

            if (session["AuthenticatedUser"] != null)
            {
                return session["AuthenticatedUser"] as WebsiteUser;
            }

            if (String.IsNullOrEmpty(identity.Name))
            {
                throw new AuthenticationException(
                   "Forms identity was authenticated, " +
                   "while identity name was null or empty.", this);
            }

            try
            {
                int userId = Int32.Parse(identity.Name);
                WebsiteUser user = WebsiteUser.Fetch(userId);
                this.ActiveUser = user;
                return user;
            }
            catch (NotFoundException ex)
            {
                throw new AuthenticationException(
                    "Forms identity (ID: " + identity.Name + ") was not found in the database.", this, ex);
            }
            catch (Exception ex)
            {
                throw new AuthenticationException(
                    "An unexpected exception occured while trying " +
                    "to translate identity name into a website user.", this, ex);
            }
        }
        set
        {
            session["AuthenticatedUser"] = value;
        }
    }

    public Authentication(Page page)
    {
        this.session = page.Session;
        this.response = page.Response;
        this.identity = page.User.Identity as FormsIdentity;
    }

    /// <summary>
    /// Clears the current active user.
    /// </summary>
    public void RefreshActiveUser()
    {
        session.Remove("AuthenticatedUser");
    }

    /// <summary>
    /// Logs user off and redirects to the log off confirmation page.
    /// </summary>
    public void LogOff(bool endResponse)
    {
        // Remove user from session.
        RefreshActiveUser();

        // Set cookie to not authenticated.
        FormsAuthentication.SignOut();

        // Redirect to the login page.
        response.Redirect("~/Logon.aspx", endResponse);
    }
}
