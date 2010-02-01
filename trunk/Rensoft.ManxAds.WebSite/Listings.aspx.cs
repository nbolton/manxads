using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.Common;
using System.Text.RegularExpressions;
using ManxAds.Search;
using ManxAds;

/// <summary>
/// Page to expose the ListingBrowser.ascx control.
/// </summary>
public partial class Listings : StandardPage
{
    public Listings() : base(WebsiteUserType.Public) { }
}