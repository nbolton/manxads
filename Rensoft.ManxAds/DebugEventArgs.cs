using System;
using System.Collections.Generic;
using System.Text;

namespace Rensoft.ManxAds
{
    public class DebugEventArgs : EventArgs
    {
        private string message;

        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        public DebugEventArgs(string message)
        {
            this.message = message;
        }
    }
}
