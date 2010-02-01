using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


namespace ManxAds
{
    /// <summary>
    /// Contains information about a listing, where all information is safe for public view.
    /// </summary>
    public struct PublicListing
    {
        public string Title;
        public string Details;
        public decimal PriceValue;
        public PriceType PriceType;

        private DateTime CreateDate;
        private DateTime UpdateDate;
        private DateTime BoostDate;

        public string NavigateUrl;
        public string ThumbnailUrl;
        public string SmallImageUrl;
        public string FullImageUrl;

        public PublicListing(
            string title,
            string details,
            decimal priceValue,
            PriceType PriceType,
            DateTime createDate,
            DateTime updateDate,
            DateTime boostDate,
            string navigateUrl,
            string thumbnailUrl,
            string smallImageUrl,
            string fullImageUrl)
        {
            this.Title = title;
            this.Details = details;
            this.PriceValue = priceValue;
            this.PriceType = PriceType;

            this.CreateDate = createDate;
            this.UpdateDate = updateDate;
            this.BoostDate = boostDate;

            this.NavigateUrl = navigateUrl;
            this.ThumbnailUrl = thumbnailUrl;
            this.SmallImageUrl = smallImageUrl;
            this.FullImageUrl = fullImageUrl;
        }
    }
}