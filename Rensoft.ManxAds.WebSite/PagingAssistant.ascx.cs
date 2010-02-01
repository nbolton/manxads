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
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ManxAds;


public partial class PagingAssistant : PagingAssistantBase
{
    private string requestUrl;

    public event EventHandler BeforePageLoad;

    protected class PageInfo
    {
        private int pageNumber;
        private string navigateUrl;

        public string PageNumber
        {
            get { return pageNumber.ToString(); }
        }

        public string NavigateUrl
        {
            get { return navigateUrl; }
        }

        public PageInfo(int pageNumber, string requestUrl, bool enable)
        {
            this.pageNumber = pageNumber;
            requestUrl = requestUrl.TrimEnd('?');

            if (enable)
            {
                string separator = (requestUrl.Contains("?") ? "&" : "?");
                this.navigateUrl = requestUrl + separator + "Page=" + pageNumber;
            }
        }
    }

    protected PageInfo[] PageInfoArray
    {
        get
        {
            List<PageInfo> pages = new List<PageInfo>();
            bool enable = false;

            int pageCount = PagingManager.PageCount;
            if (pageCount > LocalSettings.DefaultPageRange)
            {
                pageCount = LocalSettings.DefaultPageRange;
            }

            int halfCount = (int)Math.Floor(pageCount / 2d);
            int pageNumber = PagingManager.SelectedPage - halfCount;

            // Start page can never preceed 1.
            if (pageNumber < 1) { pageNumber = 1; }

            int maxPage = (pageCount + pageNumber - 1);
            if (maxPage > PagingManager.PageCount)
            {
                // Cap max page at most page count.
                maxPage = PagingManager.PageCount;
            }

            for (int i = pageNumber; i <= maxPage; i++)
            {
                enable = (PagingManager.SelectedPage != i);
                pages.Add(new PageInfo(i, requestUrl, enable));
            }
            return pages.ToArray();
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        // Call parent to update paging manager.
        this.BeforePageLoad(sender, e);

        // Only show if we have valid data.
        if ((PagingManager == null) || (PagingManager.PageCount <= 1))
        {
            PagingPanel.Visible = false;
            return;
        }

        Regex pattern = new Regex(@"&?Page=[0-9]*");
        this.requestUrl = pattern.Replace(Request.RawUrl, String.Empty);

        PageNumberRepeater.DataSource = PageInfoArray;
        PageNumberRepeater.DataBind();

        if (PagingManager.SelectedPage > 1)
        {
            PageInfo previous = new PageInfo(
                PagingManager.SelectedPage - 1, requestUrl, true);
            PreviousHyperLink.NavigateUrl = previous.NavigateUrl;
        }

        if (PagingManager.SelectedPage < PagingManager.PageCount)
        {
            PageInfo next = new PageInfo(
                PagingManager.SelectedPage + 1, requestUrl, true);
            NextHyperLink.NavigateUrl = next.NavigateUrl;
        }
    }
}
