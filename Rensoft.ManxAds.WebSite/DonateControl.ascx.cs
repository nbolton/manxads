using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rensoft.ManxAds.WebSite
{
    public partial class DonateControl : System.Web.UI.UserControl
    {
        private string donateID;

        public string DonateID
        {
            get { return donateID; }
            set { donateID = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}