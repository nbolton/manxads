using System;
using System.Collections.Generic;
using System.Text;

namespace ManxAds
{
    [Flags]
    public enum WebsiteUserType
    {
        None                = 0x00,

        Public              = 0x01,

        SellerOnly          = 0x02,

        AdvertiserOnly      = 0x04,

        AdministratorOnly   = 0x08,

        /// <summary>
        /// Seller also assumes Public role.
        /// </summary>
        SellerInherit = Public | SellerOnly,

        /// <summary>
        /// Advertiser also assumes Trader, Seller and Public roles.
        /// </summary>
        AdvertiserInherit = SellerInherit | AdvertiserOnly,

        /// <summary>
        /// Administrator also assumes Advertiser, Trader, Seller and Public roles.
        /// </summary>
        AdministratorInherit = AdvertiserInherit | AdministratorOnly
    }
}
