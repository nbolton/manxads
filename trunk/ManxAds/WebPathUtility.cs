using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ManxAds
{
    public class WebPathUtility : PathUtility
    {
        private HttpServerUtility hsu;

        public WebPathUtility(HttpServerUtility hsu)
        {
            this.hsu = hsu;
        }

        public override string GetAbsolutePath(string webPath)
        {
            return hsu.MapPath(webPath);
        }
    }
}
