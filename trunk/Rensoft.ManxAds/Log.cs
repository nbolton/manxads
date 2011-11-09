using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rensoft.ManxAds.Service;
using System.Diagnostics;

namespace Rensoft.ManxAds
{
    public static class Log
    {
        public static IEventLogHelper Helper;

        internal static void Error(Exception ex)
        {
            if (Helper != null)
            {
                Helper.Write(ex);
            }
        }
    }
}
