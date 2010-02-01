using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rensoft.ManxAds.WebSite
{
    public partial class RandomNotice : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (DateTime.Now.Month != 12) {
                NoticeMultiView.Views.Remove(NoticeChristmasView);
            }

            Random r = new Random((int)DateTime.Now.Ticks);
            int randomIndex = r.Next(0, NoticeMultiView.Views.Count);
            NoticeMultiView.ActiveViewIndex = randomIndex;
        }
    }
}