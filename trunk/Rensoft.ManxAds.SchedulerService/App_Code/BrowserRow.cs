using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using ManxAds;


/// <summary>
/// Summary description for BrowserRow
/// </summary>
public class BrowserRow : StandardControl
{
    private bool setImage;
    private string navigateUrl;

    protected bool IsPopup;

    public string NavigateUrl
    {
        get
        {
            // Don't alter origional version.
            string appended = (string)navigateUrl.Clone();

            string categoryId = Request.QueryString["CategoryId"];
            if (categoryId != null)
            {
                // Pass on current category page.
                appended += "&Category=" + categoryId;
            }
            return appended;
        }
        set { navigateUrl = value; }
    }

    public string ImageUrl;
    public string ModifyUrl;
    public string RemoveUrl;

    protected string PopupAttributeValue
    {
        get
        {
            return "openWindow(this.href, " +
                LocalSettings.BrowserRowPopupWidth + ", " +
                LocalSettings.BrowserRowPopupHeight + "); return false";
        }
    }

    protected BrowserRow() : this(true) { }

    protected BrowserRow(bool setImage)
    {
        this.setImage = setImage;
    }

    protected BrowserRow(bool setImage, bool isPopup)
        : this(setImage)
    {
        this.IsPopup = isPopup;
    }

    protected override void OnDataBinding(EventArgs e)
    {
        base.OnDataBinding(e);

        if (setImage)
        {
            HyperLink link = (HyperLink)this.FindControl("ThumbnailHyperLink");
            link.NavigateUrl = NavigateUrl;

            if (IsPopup)
            {
                link.Attributes.Add("onclick", PopupAttributeValue);
            }

            if (!String.IsNullOrEmpty(ImageUrl)
                && File.Exists(Server.MapPath(ImageUrl)))
            {
                Image image = (Image)this.FindControl("ThumbnailImage");
                image.ImageUrl = ImageUrl;
            }
        }
    }

    public void DisplayEditPanel()
    {
        Panel editorPanel = (Panel)FindControl("EditorPanel");
        editorPanel.Visible = true;

        LinkButton modify = (LinkButton)FindControl("ModifyLinkButton");
        modify.CommandArgument = ModifyUrl;

        LinkButton remove = (LinkButton)FindControl("RemoveLinkButton");
        remove.CommandArgument = RemoveUrl;
    }

    protected void EditorLinkButton_Click(object sender, EventArgs e)
    {
        StandardPage.SetAsPreviousPage(Session, Request);
        LinkButton link = sender as LinkButton;
        Response.Redirect(link.CommandArgument);
    }
}
