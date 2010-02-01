using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for BreadcrumbNode
/// </summary>
public class BreadcrumbNode
{
    private string title;
    private string url;
    private BreadcrumbNode parent;
    private BreadcrumbNode child;

    public string Title
    {
        get { return title; }
        set { title = value; }
    }

    public string Url
    {
        get { return url; }
        set { url = value; }
    }

    public BreadcrumbNode Parent
    {
        get { return parent; }
        set { parent = value; }
    }

    public BreadcrumbNode Child
    {
        get { return child; }
        set { child = value; }
    }

    public bool IsLink
    {
        get
        {
            if (String.IsNullOrEmpty(url) || (child == null))
            {
                return false;
            }
            return true;
        }
    }

    public BreadcrumbNode(string title, string url)
    {
        this.title = title;
        this.url = url;
    }

    public BreadcrumbNode(string title, string url, BreadcrumbNode parent)
        : this (title, url)
    {
        this.parent = parent;
    }

    public override string ToString()
    {
        if (IsLink)
        {
            HyperLink hyperlink = new HyperLink();
            string clientUrl = hyperlink.ResolveUrl(this.url);
            return "<a href=\"" + clientUrl + "\">" + title + "</a>";
        }

        return title;
    }
}
