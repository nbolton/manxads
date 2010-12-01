using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Rensoft.ManxAds.Service
{
    public interface IEventLogHelper
    {
        void Write(string message, EventLogEntryType eventLogEntryType);
        void Write(Exception exception);
    }
}
