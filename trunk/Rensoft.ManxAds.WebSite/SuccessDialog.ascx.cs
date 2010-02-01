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

public partial class SuccessDialog : StandardControl
{
    protected override void Render(HtmlTextWriter writer)
    {
        string url = StandardPage.GetPreviousPageUrl(Session, true);
        ReturnHyperLink.NavigateUrl = url;

        if (!Auth.IsAuthenticated)
        {
            UserHomeHyperLink.Visible = false;
        }

        base.Render(writer);
    }

    public void ShowListingHyperLinks(string navigateUrl)
    {
        ListingsPanel.Visible = true;
        ListingViewHyperLink.NavigateUrl = navigateUrl;
    }
}
