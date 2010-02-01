using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class BreadcrumbTrail : System.Web.UI.UserControl
{
    public const string Separator = " &gt; ";

    private List<BreadcrumbNode> nodes;

    public BreadcrumbTrail()
    {
        nodes = new List<BreadcrumbNode>();
    }

    public void AddNode(string title, string url)
    {
        BreadcrumbNode newNode = null, parentNode = null;

        if (nodes.Count > 0)
        {
            parentNode = nodes[nodes.Count - 1];
        }

        if (parentNode != null)
        {
            newNode = new BreadcrumbNode(title, url, parentNode);
            parentNode.Child = newNode;
        }
        else
        {
            newNode = new BreadcrumbNode(title, url);
        }

        nodes.Add(newNode);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        List<string> nodeHtmlParts = new List<string>();
        foreach (BreadcrumbNode node in nodes)
        {
            nodeHtmlParts.Add(node.ToString());
        }
        string[] nodeHtmlArray = nodeHtmlParts.ToArray();
        NodesLiteral.Text = String.Join(Separator, nodeHtmlArray);
    }
}