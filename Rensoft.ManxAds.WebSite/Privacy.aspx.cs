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

public partial class Privacy : StandardPage
{
    public Privacy() : base(ManxAds.WebsiteUserType.Public) { }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        RobotFlags = RobotFlag.NoIndex | RobotFlag.Follow;
    }
}
