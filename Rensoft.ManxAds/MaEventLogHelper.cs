using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rensoft.ManxAds.Service;

namespace Rensoft.ManxAds
{
    public class MaEventLogHelper : EventLogHelper, IEventLogHelper
    {
        public MaEventLogHelper(string component) :
            base("ManxAds " + component, "ManxAds") { }
    }
}
