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

using System.Diagnostics;
using System.Web.SessionState;
using ManxAds;

public class StandardPage : System.Web.UI.Page
{
    private CommonPageProperties _common;
    private bool _abortInitialization;

    /// <summary>
    /// The control client ID to be focused this runtime.
    /// </summary>
    protected string AutoFocusClientId;

    public CommonPageProperties Common
    {
        get { return _common; }
    }

    public PageMode PageMode
    {
        get { return Common.PageMode; }
        set { Common.PageMode = value; }
    }

    protected Authentication Auth
    {
        get { return Common.Auth; }
    }

    protected QueryAccessors Accessors
    {
        get { return Common.Accessors; }
    }

    protected IPagingManager PagingManager
    {
        get { return Common.PagingManager; }
        set { Common.PagingManager = value; }
    }

    protected int PageNumber
    {
        get { return Common.PageNumber; }
    }

    new protected StandardMaster Master
    {
        get { return base.Master as StandardMaster; }
    }

    new public string Title
    {
        get { return Common.Title; }
        set { Common.Title = value; }
    }

    protected string Description
    {
        get { return Common.Description; }
        set { Common.Description = value; }
    }

    protected RobotFlag RobotFlags
    {
        get { return Master.RobotFlags; }
        set { Master.RobotFlags = value; }
    }

    /// <summary>
    /// Sets or gets the auto-focus control from session. Use this
    /// to set the focus for this runtime and the future runtimes.
    /// </summary>
    protected WebControl AutoFocusControl
    {
        set { Session["AutoFocusControl"] = value; }
        get
        {
            object control = Session["AutoFocusControl"];
            if (control != null)
            {
                return control as WebControl;
            }
            return null;
        }
    }

    protected bool AdminMode
    {
        get { return Common.AdminMode; }
    }

    protected StandardPage(WebsiteUserType minimumUserType)
        : this(minimumUserType, true) { }

    protected StandardPage(WebsiteUserType minimumUserType, bool appendTitle)
        : this(minimumUserType, appendTitle, false) { }

    protected StandardPage(
        WebsiteUserType minimumUserType, bool appendTitle, bool disableCache)
    {
        _common = new CommonPageProperties(
            minimumUserType, appendTitle, disableCache);

        this.Common.AuthenticationFailed += new EventHandler(Common_AuthenticationFailed);
        this.Common.AuthorizationFailed += new EventHandler(Common_AuthorizationFailed);

        Error += new EventHandler(StandardPage_Error);
    }

    void StandardPage_Error(object sender, EventArgs e)
    {
#if !DEBUG
        // wtf?!
        //ErrorReporting.Record(ex, Page, Auth);
#endif
    }

    protected void Common_AuthenticationFailed(object sender, EventArgs e)
    {
        _abortInitialization = true;
        FormsAuthentication.RedirectToLoginPage();
    }

    protected void Common_AuthorizationFailed(object sender, EventArgs e)
    {
        _abortInitialization = true;
        Response.Redirect("AccessDenied.aspx");
    }

    protected override void OnLoad(EventArgs e)
    {
        if (!_abortInitialization)
        {
            UpdatePageMode(Accessors);
            InitializePage();
        }

        if (Auth.IsAuthenticated)
        {
            // Warning: May cause undesired side effects!
            // Will increase load on site, but is neccecary to check for ban.
            Auth.RefreshActiveUser();

            // Check if the user's ban is active.
            if (Auth.ActiveUser.BanUntil > DateTime.Now)
            {
                // Only redirect when not already on home page.
                if (!Request.Url.AbsoluteUri.Contains("UserHome.aspx"))
                {
                    // Redirect to home where user will see ban message.
                    Response.Redirect("~/UserHome.aspx");
                }
            }
        }
    }

    /// <summary>
    /// Gets the previous page url and clears the remembered value.
    /// </summary>
    public static string GetPreviousPageUrl(HttpSessionState session, bool forgetCurrent)
    {
        string url = FormsAuthentication.DefaultUrl;
        if (session["PreviousPageUrl"] != null)
        {
            url = session["PreviousPageUrl"].ToString();
            if (forgetCurrent)
            {
                session["PreviousPageUrl"] = null;
            }
        }
        return url;
    }

    /// <summary>
    /// If set, redirects to the previous page. If not set,
    /// redirects to the default forms authentication page.
    /// </summary>
    /// <param name="endResponse"></param>
    protected void RedirectToPreviousPage(bool endResponse)
    {
        Response.Redirect(GetPreviousPageUrl(Session, true), false);
    }

    protected void SetAsPreviousPage()
    {
        StandardPage.SetAsPreviousPage(Session, Request);
    }

    public static void SetAsPreviousPage(
        HttpSessionState session, HttpRequest request)
    {
        session["PreviousPageUrl"] = request.RawUrl;
    }

    protected void ClearAutoFocusControl()
    {
        Session.Remove("AutoFocusControl");
    }

    public class InvalidUploadEventArgs : EventArgs
    {
        private Exception exception;

        public Exception Exception
        {
            get { return exception; }
        }

        public InvalidUploadEventArgs(Exception ex)
        {
            this.exception = ex;
        }
    }

    protected override void FrameworkInitialize()
    {
        Common.Initialize(this, this.Request, this.Response);
        base.FrameworkInitialize();
    }

    protected virtual void UpdatePageMode(QueryAccessors accessors)
    {
        Common.UpdatePageMode(accessors);
    }

    protected virtual void InitializePage() { }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        if (AutoFocusControl != null)
        {
            AutoFocusClientId = AutoFocusControl.ClientID;
        }

        if (!String.IsNullOrEmpty(AutoFocusClientId) &&
            !ClientScript.IsStartupScriptRegistered("SetControlFocus"))
        {
            string script = String.Format(
                "<script language='JavaScript'>document" +
                ".getElementById('{0}').focus();</script>",
                AutoFocusClientId);

            ClientScript.RegisterStartupScript(
                this.GetType(), "SetControlFocus", script);
        }

        ClearAutoFocusControl();
    }

    protected void OnRequestDetailsValid(
        object source, ServerValidateEventArgs args)
    {
        Validate("Details");
        if (!this.IsValid)
        {
            args.IsValid = false;
        }
    }

    protected void UpdatePagingAssistant(object sender, EventArgs e)
    {
        Common.UpdatePagingAssistant(sender, e);
    }
}
