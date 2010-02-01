using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using ManxAds;


public partial class FlashAdvertInline : System.Web.UI.UserControl
{
    public int DatabaseId;
    public string LayerId;

    protected void Page_Load(object sender, EventArgs e) { }

    protected override void Render(HtmlTextWriter writer)
    {
        string jsUrl = String.Format(LocalSettings.FlashAdvertUrlFormat, DatabaseId, LayerId);

        HyperLink hyperLink = new HyperLink();
        writer.Write("\x3Cscript type=\"text/javascript\" language=\"javascript\"");
        writer.Write(" src=\"" + hyperLink.ResolveUrl(jsUrl) + "\">\x3C/script>");
    }
}
