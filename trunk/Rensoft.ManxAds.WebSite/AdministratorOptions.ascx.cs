using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Data.SqlTypes;
using ManxAds;

public partial class AdministratorOptions : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DateTime firstDay = GetFirstWeekDayDate().Date;
        DateTime minDate = SqlDateTime.MinValue.Value;

        int listingWeek = Listing.RunCount(firstDay);
        int listingTotal = Listing.RunCount(minDate);
        int sellerWeek = WebsiteUser.RunCount(firstDay, WebsiteUserType.SellerInherit);
        int sellerTotal = WebsiteUser.RunCount(minDate, WebsiteUserType.SellerInherit);
        int corporateWeek = WebsiteUser.RunCount(firstDay, TraderType.Corporate);
        int corporateTotal = WebsiteUser.RunCount(minDate, TraderType.Corporate);
        int charityWeek = WebsiteUser.RunCount(firstDay, TraderType.Charity);
        int charityTotal = WebsiteUser.RunCount(minDate, TraderType.Charity);

        ListingWeekCell.Text = listingWeek.ToString();
        ListingTotalCell.Text = listingTotal.ToString();
        SellerWeekCell.Text = sellerWeek.ToString();
        SellerTotalCell.Text = sellerTotal.ToString();
        CorporateWeekCell.Text = corporateWeek.ToString();
        CorporateTotalCell.Text = corporateTotal.ToString();
        CharityWeekCell.Text = charityWeek.ToString();
        CharityTotalCell.Text = charityTotal.ToString();
    }

    protected DateTime GetFirstWeekDayDate()
    {
        DateTime firstWeekDay = DateTime.Now;
        DayOfWeek today = DateTime.Now.DayOfWeek;

        switch (today)
        {
            case DayOfWeek.Tuesday:
                return firstWeekDay.AddDays(-1);

            case DayOfWeek.Wednesday:
                return firstWeekDay.AddDays(-2);

            case DayOfWeek.Thursday:
                return firstWeekDay.AddDays(-3);

            case DayOfWeek.Friday:
                return firstWeekDay.AddDays(-4);

            case DayOfWeek.Saturday:
                return firstWeekDay.AddDays(-5);
                
            case DayOfWeek.Sunday:
                return firstWeekDay.AddDays(-6);

            default:
                return firstWeekDay;
        }
    }
}
