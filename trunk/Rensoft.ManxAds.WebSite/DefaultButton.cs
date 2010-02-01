using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;

/// <summary>
/// Summary description for PairMaster
/// </summary>
public class DefaultButton
{
    private WebControl defaultButton;

    public DefaultButton(WebControl defaultButton)
    {
        this.defaultButton = defaultButton;
    }

    public void AssociateWith(WebControl control)
    {
        control.Attributes.Add("onkeypress",
            "return clickButton(event,'" + defaultButton.ClientID + "')");
    }
}
