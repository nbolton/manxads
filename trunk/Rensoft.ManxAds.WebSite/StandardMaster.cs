using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;

/// <summary>
/// Provides helper properties for master pages.
/// </summary>
public class StandardMaster : System.Web.UI.MasterPage
{
    private bool enableAdverts = true;

    private HtmlMeta descriptionMeta
    {
        get { return this.FindControl("descriptionMeta") as HtmlMeta; }
    }

    private HtmlMeta keywordsMeta
    {
        get { return this.FindControl("keywordsMeta") as HtmlMeta; }
    }

    private HtmlMeta cacheMeta
    {
        get { return this.FindControl("cacheMeta") as HtmlMeta; }
    }

    private HtmlMeta robotsMeta
    {
        get { return FindControl("robotsMeta") as HtmlMeta; }
    }

    public string Description
    {
        set { descriptionMeta.Content = value; }
        get { return descriptionMeta.Content; }
    }

    public string Keywords
    {
        set { keywordsMeta.Content = value; }
        get { return keywordsMeta.Content; }
    }

    public RobotFlag RobotFlags
    {
        get { return getRobotFlags(); }
        set { setRobotFlags(value); }
    }

    public string LinkImageUrl
    {
        get;
        set;
    }

    public bool EnableAdverts
    {
        get { return enableAdverts; }
        set { enableAdverts = value; }
    }

    public StandardMaster()
    {
        Error += new EventHandler(StandardMaster_Error);
    }

    void StandardMaster_Error(object sender, EventArgs e)
    {
#if !DEBUG
        // records the current user and requested page - very important!
        if (Page is StandardPage)
            ErrorReporting.RecordLastError((StandardPage)Page);
#endif
    }

    private void setRobotFlags(RobotFlag flags)
    {
        string indexString = string.Empty;
        if ((flags & RobotFlag.Index) != 0)
        {
            indexString = "index";
        }
        else if ((flags & RobotFlag.NoIndex) != 0)
        {
            indexString = "noindex";
        }

        string followString = string.Empty;
        if ((flags & RobotFlag.Follow) != 0)
        {
            followString = "follow";
        }
        else if ((flags & RobotFlag.NoFollow) != 0)
        {
            followString = "nofollow";
        }

        robotsMeta.Content = string.Format("{0}, {1}", indexString, followString);
    }

    private RobotFlag getRobotFlags()
    {
        // Assume no flag set by default.
        RobotFlag flags = RobotFlag.None;

        // Add either index or noindex.
        if (robotsMetaContains("index"))
        {
            flags |= RobotFlag.Index;
        }
        else if (robotsMetaContains("noindex"))
        {
            flags |= RobotFlag.NoIndex;
        }

        // Add either follow or nofollow.
        if (robotsMetaContains("follow"))
        {
            flags |= RobotFlag.Follow;
        }
        else if (robotsMetaContains("nofollow"))
        {
            flags |= RobotFlag.NoFollow;
        }

        return flags;
    }

    private bool robotsMetaContains(string findMode)
    {
        bool modeFound = false;
        foreach (string mode in robotsMeta.Content.Split(','))
        {
            if (mode.Trim() == findMode)
            {
                modeFound = true;
            }
        }
        return modeFound;
    }

    public void DisableCache()
    {
        this.cacheMeta.Content = "no-cache";
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        //RobotFlags = RobotFlag.Index | RobotFlag.Follow;
    }

    // fix for Bug #228 - Old layout is still being cached for some visitors
    public string GetDynamicPath(string virtualPath)
    {
        string mappedPath = MapPath(virtualPath);
        FileInfo styleSheetInfo = new FileInfo(mappedPath);

        // not sure if this is the best approach to resolve virtual paths.
        string relativePath = virtualPath.Replace("~/", Request.ApplicationPath);

        return relativePath + "?" + styleSheetInfo.LastWriteTime.Ticks;
    }
}
