using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManxAds
{
    public interface ICategory
    {
        string Title { get; }
        string NavigateUrl { get; }
    }
}
