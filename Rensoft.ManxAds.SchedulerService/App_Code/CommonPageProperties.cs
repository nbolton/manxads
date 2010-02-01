using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using ManxAds;


public class CommonPageProperties
{
    private Page _page;
    private HttpRequest _request;
    private HttpResponse _response;

    private bool appendTitle;
    private bool disableCache;
    private WebsiteUserType minimumUserType;

    public event EventHandler TitleChanged;
    public event EventHandler AuthenticationFailed;
    public event EventHandler AuthorizationFailed;

    public PageMode PageMode;
    public Authentication Auth;
    public QueryAccessors Accessors;
    public IPagingManager PagingManager;
    public bool AdminMode;

    /// <summary>
    /// Should be set to true, if the category has been pre-loaded, which
    /// indicates that Accessors.Category is not null and is ready for use.
    /// </summary>
    public bool CategoryPreloaded = false;

    public string Title
    {
        get { return _page.Title; }
        set
        {
            _page.Title = value;
            TitleChanged(this, EventArgs.Empty);
        }
    }

    public bool AppendTitle
    {
        get { return appendTitle; }
        set { appendTitle = value; }
    }

    public int PageNumber
    {
        get
        {
            try
            {
                string page = _request.QueryString["Page"];
                if (String.IsNullOrEmpty(page))
                {
                    return 1;
                }
                return int.Parse(page);
            }
            catch
            {
                return 1;
            }
        }
    }

    public string Description
    {
        get { return ((StandardMaster)_page.Master).Description; }
        set { ((StandardMaster)_page.Master).Description = value; }
    }

    public CommonPageProperties(WebsiteUserType minimumUserType, bool appendTitle, bool disableCache)
    {
        this.minimumUserType = minimumUserType;
        this.appendTitle = appendTitle;
        this.disableCache = disableCache;

        TitleChanged += new EventHandler(OnTitleChanged);
        AuthenticationFailed += new EventHandler(OnAuthenticationFailed);
        AuthorizationFailed += new EventHandler(OnAuthorizationFailed);
    }

    protected virtual void OnAuthorizationFailed(object sender, EventArgs e) { }
    protected virtual void OnAuthenticationFailed(object sender, EventArgs e) { }

    public void UpdatePagingAssistant(object sender, EventArgs e)
    {
        PagingAssistantBase assistant = sender as PagingAssistantBase;
        assistant.PagingManager = PagingManager;
    }

    public void Initialize(Page page, HttpRequest request, HttpResponse response)
    {
        _page = page;
        _request = request;
        _response = response;

        // Verification auth code.
        if (_request.QueryString["VerifyAuthCode"] != null)
        {
            string authCode = _request.QueryString["VerifyAuthCode"];
            _response.Redirect("~/UserVerify.aspx?AuthCode=" + authCode, true);
        }

        this.Auth = new Authentication(_page);

        // If no user logged on and page is not public, redirect to logon.
        if (!Auth.IsAuthenticated && (minimumUserType != WebsiteUserType.Public))
        {
            AuthenticationFailed(this, EventArgs.Empty);
            return;
        }

        // If a user is logged on, check their access.
        if (Auth.IsAuthenticated)
        {
            if ((Auth.ActiveUser.UserType & minimumUserType) == 0)
            {
                AuthorizationFailed(this, EventArgs.Empty);
                return;
            }

            if ((Auth.ActiveUser.UserType & WebsiteUserType.AdministratorOnly) != 0)
            {
                this.AdminMode = true;
            }

            LocalSettings.Populate(Auth.ActiveUser);
        }
        else
        {
            LocalSettings.Populate();
        }

        // Call accessors to assist decide page mode.
        Accessors = new QueryAccessors(_request);

        if (disableCache && (_page.Master != null))
        {
            ((StandardMaster)_page.Master).DisableCache();
        }
    }

    protected void OnTitleChanged(object sender, EventArgs e)
    {
        if (AppendTitle)
        {
            _page.Title += " - ManxAds";
        }
    }

    public void UpdatePageMode(QueryAccessors accessors)
    {
        if (_request.QueryString["Self"] != null)
        {
            PageMode = PageMode.SelfOnly;
        }
        else if (_request.QueryString["Remove"] != null)
        {
            PageMode = PageMode.Remove;
        }
        else if (_request.QueryString["Create"] != null)
        {
            PageMode = PageMode.Create;
        }
        else if (_request.QueryString["Search"] != null)
        {
            PageMode = PageMode.Search;
        }
        else if (_request.QueryString["Seller"] != null)
        {
            PageMode = PageMode.Seller;
        }
    }
}
