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


public partial class FlashAdvert : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e) { }

    protected override void Render(HtmlTextWriter writer)
    {
        try
        {
            QueryAccessors accessors = new QueryAccessors(Request);
            if (accessors.AdvertId != 0)
            {
                writer.Write(accessors.Advert.ToJavaScript(this));
            }
        }
        catch (NotFoundException)
        {
            writer.Write("<!-- Advert not found -->");
        }
    }
}
