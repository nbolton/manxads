using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public interface IPagingManager
{
    bool HasItems { get; }
    int PageCount { get; }
    int SelectedPage { get; set; }
    void DataBind(DataList ListingDataList);
    int StartNumber { get; }
    int StopNumber { get; }
    int ItemCount { get; }
}
