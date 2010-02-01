using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ManxAds;

namespace Rensoft.ManxAds.WebSite
{
    public partial class AdvertRedirect : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int advertId;
            if (int.TryParse(Request.QueryString["AdvertId"], out advertId))
            {
                Advert advert = Advert.Fetch(advertId);
                advert.Hit();
                Response.Redirect(advert.NavigateUrl);
            }
            Response.End();
        }
    }
}
