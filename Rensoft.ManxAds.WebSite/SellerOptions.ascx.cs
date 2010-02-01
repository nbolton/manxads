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

public partial class SellerOptions : System.Web.UI.UserControl
{
    private const string emptyRecycleBinMessage = 
        "This will permenantly delete all " +
        "items in your recycle bin. Are you sure?";

    private Authentication auth;
    private int recycleBinCount = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        auth = new Authentication(this.Page);
        
        if (auth.IsAuthenticated)
        {
            updateRecycleBinCount();
            ListingCountLabel.Text = auth.ActiveUser.ListingCount.ToString();
        }

        updateRecycleBinControls();

        // Show confirmation when it is used.
        RecycleBinEmptyLinkButton.Attributes.Add(
            "onclick", "return confirm('" + emptyRecycleBinMessage + "')");
    }

    private void updateRecycleBinControls()
    {
        // Show recycle bin only when it can be used.
        RecycleBinEmptyLinkButton.Visible = (recycleBinCount != 0);
        RecycleBinCountLabel.Text = recycleBinCount.ToString();
    }

    private void updateRecycleBinCount()
    {
        recycleBinCount = auth.ActiveUser.RecycleBinCount;
    }

    protected void RecycleBinEmptyLinkButton_Click(object sender, EventArgs e)
    {
        auth.ActiveUser.EmptyRecycleBin(Server);
        updateRecycleBinCount();
        updateRecycleBinControls();
    }
}
