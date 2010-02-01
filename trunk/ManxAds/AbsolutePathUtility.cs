using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManxAds
{
    public class AbsolutePathUtility : PathUtility
    {
        private string basePath;

        public AbsolutePathUtility(string basePath)
        {
            this.basePath = basePath;
        }

        public override string GetAbsolutePath(string webPath)
        {
            return basePath + "/" + webPath.Replace("~/", null);
        }
    }
}
