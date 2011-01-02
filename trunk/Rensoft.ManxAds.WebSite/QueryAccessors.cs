using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using ManxAds;


/// <summary>
/// Provides helper properties to access objects inflicted by the query string.
/// </summary>
public class QueryAccessors
{
    private HttpRequest request;
    private Listing listing;
    private Category category;
    private Advert advert;
    private WebsiteUser user;

    /// <summary>
    /// Because static methods cannot be implemented by interface, each
    /// type has to be tested to reveal the location of it's Fetch method.
    /// </summary>
    /// <typeparam name="T">Type to fetch from, and return as.</typeparam>
    /// <param name="cache">Cached versions are loaded from here.</param>
    /// <param name="id">Database ID for the accessable.</param>
    /// <returns>Returns a newly fetched T.</returns>
    private T getAccessable<T>(ref T cache, int databaseId)
    {
        // Always return catch if available.
        if (cache != null)
        {
            return cache;
        }

        // Test each type to find static Fetch method.
        if (typeof(T) == typeof(Listing))
        {
            cache = (T)(object)Listing.Fetch(databaseId);
        }
        else if (typeof(T) == typeof(Category))
        {
            cache = (T)(object)Category.Fetch(databaseId);
        }
        else if (typeof(T) == typeof(Advert))
        {
            cache = (T)(object)Advert.Fetch(databaseId);
        }
        else if (typeof(T) == typeof(WebsiteUser))
        {
            cache = (T)(object)WebsiteUser.Fetch(databaseId);
        }
        else
        {
            throw new NotSupportedException(
                "Fetching for type '" + typeof(T).Name + "' is not supported.");
        }

        // Fetch found no such item.
        if (cache == null)
        {
            throw new NotFoundException(databaseId);
        }

        return cache;
    }

    private int getQueryStringInt(params string[] keys)
    {
        foreach (string key in keys)
        {
            int value = getQueryStringInt(key);
            if (value != default(int))
            {
                return value;
            }
        }

        return default(int);
    }

    private int getQueryStringInt(string key)
    {
        string queryStringElement = request.QueryString[key];
        if (!String.IsNullOrEmpty(queryStringElement))
        {
            int result;
            int.TryParse(queryStringElement, out result);
            return result;
        }

        return default(int);
    }

    public QueryAccessors(HttpRequest request)
    {
        this.request = request;
    }

    public int CategoryId
    {
        get {  return getQueryStringInt("Category", "CategoryId"); }
    }

    public int ListingId
    {
        get { return getQueryStringInt("Listing", "ListingId"); }
    }

    public int AdvertId
    {
        get { return getQueryStringInt("Advert", "AdvertId"); }
    }

    public int UserId
    {
        get { return getQueryStringInt("User", "UserId"); }
    }

    public bool HasListing
    {
        get { return ListingId != 0; }
    }

    public Category Category
    {
        get
        {
            return getAccessable(ref category, CategoryId);
        }
        set { this.category = value; }
    }

    public Listing Listing
    {
        get
        {
            return getAccessable(ref listing, ListingId);
        }
        set { this.listing = value; }
    }

    public Advert Advert
    {
        get
        {
            return getAccessable(ref advert, AdvertId);
        }
        set { this.advert = value; }
    }

    public WebsiteUser User
    {
        get
        {
            return getAccessable(ref user, UserId);
        }
        set { this.user = value; }
    }
}
