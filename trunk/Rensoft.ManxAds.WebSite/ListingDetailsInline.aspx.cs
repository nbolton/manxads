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


public partial class ListingDetailsInline : StandardPage
{
    protected ListingDetailsInline() : base(ManxAds.WebsiteUserType.Public, true) { }

    protected override void InitializePage()
    {
        base.InitializePage();
    }
}
