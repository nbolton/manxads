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

public partial class SubHeader : System.Web.UI.UserControl
{
    protected Authentication Auth;

    protected void Page_Load(object sender, EventArgs e)
    {
        Auth = new Authentication(this.Page);
        if (Auth.IsAuthenticated)
        {
            UserFullName.Text = Auth.ActiveUser.FullName;
            StatusMultiView.SetActiveView(LoggedOnView);
            RegisterPanel.Visible = false;
        }
        else
        {
            StatusMultiView.SetActiveView(LoggedOutView);
            RegisterPanel.Visible = true;
        }
    }

    protected void LogOnLinkButton_Click(object sender, EventArgs e)
    {
        Auth.LogOff(true);
    }
}
