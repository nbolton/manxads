using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManxAds.Test.Listings
{
    class StubCategory : ICategory
    {
        #region ICategory Members

        public string Title
        {
            get;
            set;
        }

        public string NavigateUrl
        {
            get;
            set;
        }

        #endregion
    }
}
