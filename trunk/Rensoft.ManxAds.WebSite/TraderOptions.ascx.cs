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

using System.IO;
using ManxAds;

public partial class TraderOptions : StandardControl
{
    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        if (this.Visible)
        {
            this.BindTraderLogoImage();
            TraderTypeLabel.Text = TraderType.Corporate.ToString();
        }
    }

    protected void LogoUploadButton_Click(object sender, EventArgs e)
    {
        if (LogoFileUpload.HasFile)
        {
            Imaging.ShrinkAndSave(
                LogoFileUpload.FileContent,
                Server.MapPath(Auth.ActiveUser.TraderLogoRelativePath),
                LocalSettings.TraderLogoWidth,
                LocalSettings.TraderLogoHeight);

            BindTraderLogoImage();
        }
    }

    protected void BindTraderLogoImage()
    {
        LogoImagePanel.Visible = false;
        if (File.Exists(Server.MapPath(Auth.ActiveUser.TraderLogoRelativePath)))
        {
            LogoImagePanel.Visible = true;
            LogoImage.ImageUrl = Auth.ActiveUser.TraderLogoRelativePath;
        }
    }

    protected void LogoDeleteLinkButton_Click(object sender, EventArgs e)
    {
        File.Delete(Server.MapPath(Auth.ActiveUser.TraderLogoRelativePath));
        BindTraderLogoImage();
    }
}
