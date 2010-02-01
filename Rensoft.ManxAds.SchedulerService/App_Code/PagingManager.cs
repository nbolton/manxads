using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

public class PagingManager<T> : IPagingManager
{
    private List<T> items;
    private int limit;
    private int page;
    private bool itemLimitEnable;
    private int itemLimitCount;
    private bool autoLimitPages;

    protected int TotalItems
    {
        get { return itemLimitEnable ? itemLimitCount : items.Count; }
    }

    public bool ItemsLimited
    {
        get { return itemLimitEnable; }
    }

    public bool HasItems
    {
        get
        {
            if (TotalItems > 0)
            {
                return true;
            }
            return false;
        }
    }

    public int SelectedPage
    {
        get
        {
            // Where page auto limit on, return page count if selected page exceeds it.
            return (autoLimitPages && (page > PageCount)) ? PageCount : page;
        }
        set { page = value; }
    }

    public int PageCount
    {
        get
        {
            return (int)Math.Ceiling((double)TotalItems / (double)limit);
        }
    }

    public int ItemsLeft
    {
        get { return TotalItems - Index; }
    }

    public int CappedLimit
    {
        get
        {
            if (TotalItems < limit)
            {
                return TotalItems;
            }

            if (limit > ItemsLeft)
            {
                return ItemsLeft;
            }

            return limit;
        }
    }

    public int Index
    {
        get
        {
            if (SelectedPage <= 1)
            {
                // Just return 0 in this case.
                return 0;
            }

            return limit * (SelectedPage - 1);
        }
    }

    public List<T> Range
    {
        get
        {
            if (itemLimitEnable)
            {
                // Items already limited.
                return items;
            }

            // Limit items from full list.
            return items.GetRange(Index, CappedLimit);
        }
    }

    public int StartNumber
    {
        get { return Index + 1; }
    }

    public int StopNumber
    {
        get { return Index + CappedLimit; }
    }

    public int ItemCount
    {
        get { return TotalItems; }
    }

    /// <summary>
    /// Creates an instance where the page is automatically limited.
    /// </summary>
    public PagingManager(List<T> items, int limit, int page)
        : this(items, limit, page, true, false) { }

    /// <summary>
    /// Creates an instance where the page is automatically limited, and an item limit is applied.
    /// </summary>
    public PagingManager(List<T> items, int limit, int page, int itemLimit)
        : this(items, limit, page, true, itemLimit) { }

    /// <summary>
    /// Creates an instance where the automatic page limit can be enabled or disabled.
    /// </summary>
    public PagingManager(List<T> items, int limit, int page, bool autoLimitPages)
        : this(items, limit, page, autoLimitPages, false) { }

    /// <summary>
    /// Creates an instance where the automatic page limit can be enabled or disabled, and an 
    /// item limit is applied.
    /// </summary>
    public PagingManager(List<T> items, int limit, int page, bool autoLimitPages, int itemLimit)
        : this(items, limit, page, autoLimitPages, true)
    {
        this.itemLimitCount = itemLimit;
    }

    protected PagingManager(List<T> items, int limit, int page, bool autoLimitPages, bool limitItems)
    {
        this.items = items;
        this.limit = limit;
        this.page = page;
        this.itemLimitEnable = limitItems;
        this.autoLimitPages = autoLimitPages;
    }

    public void DataBind(DataList ListingDataList)
    {
        // Do nothing if items empty.
        if (!HasItems && (page == 1)) return;

        // Ensure page number is in range.
        if (SelectedPage > PageCount)
        {
            throw new ArgumentOutOfRangeException(
                "page", "Page " + page + " does not exist.");
        }

        ListingDataList.DataSource = Range;
        ListingDataList.DataBind();
    }
}
