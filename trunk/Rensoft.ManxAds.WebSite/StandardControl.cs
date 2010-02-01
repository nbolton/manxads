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
/// Summary description for StandardControl
/// </summary>
public class StandardControl : UserControl
{
    protected CommonPageProperties Common
    {
        get { return ((StandardPage)Page).Common; }
    }

    protected Authentication Auth
    {
        get { return Common.Auth; }
        set { Common.Auth = value; }
    }

    protected QueryAccessors Accessors
    {
        get { return Common.Accessors; }
        set { Common.Accessors = value; }
    }

    protected virtual void Page_Load(object sender, EventArgs e) { }

    protected void UpdatePagingAssistant(object sender, EventArgs e)
    {
        Common.UpdatePagingAssistant(sender, e);
    }
}
