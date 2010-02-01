using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rensoft.ManxAds
{
    public class MaEventLogHelper : EventLogHelper
    {
        public MaEventLogHelper(string component) :
            base("ManxAds " + component, "ManxAds") { }
    }
}
