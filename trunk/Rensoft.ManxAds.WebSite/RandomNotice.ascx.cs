using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Configuration;

namespace Rensoft.ManxAds.WebSite
{
    public partial class RandomNotice : System.Web.UI.UserControl
    {
        protected DateTime BugNoticeExpiryDate
        {
            get
            {
                string setting = ConfigurationManager.AppSettings["BugNoticeExpiryDate"];
                return string.IsNullOrEmpty(setting) ? DateTime.MinValue : Convert.ToDateTime(setting);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (DateTime.Now.Month != 12)
                NoticeMultiView.Views.Remove(NoticeChristmasView);

            if (DateTime.Now > BugNoticeExpiryDate)
                NoticeMultiView.Views.Remove(BugsFixedView);
            
            Random r = new Random(Environment.TickCount);
            NoticeMultiView.ActiveViewIndex = r.Next() % NoticeMultiView.Views.Count;
        }
    }
}