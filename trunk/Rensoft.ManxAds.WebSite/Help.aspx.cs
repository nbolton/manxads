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
using System.Xml;

using System.IO;
using System.Text;
using System.Collections.Generic;
using ManxAds;

public partial class Help : StandardPage
{
    protected string Topic
    {
        get { return Request.QueryString["Topic"]; }
    }

    protected string VirtualXmlFilePath
    {
        get { return String.Format(LocalSettings.HelpDocumentsPathFormat, Topic); }
    }

    protected string AbsoluteXmlFilePath
    {
        get { return Server.MapPath(VirtualXmlFilePath); }
    }

    protected string AscxVirtualFilePath
    {
        get
        {
            string format = LocalSettings.HelpUserControlPathFormat;
            return String.Format(format, Topic);
        }
    }

    protected UserControl HelpUserControl
    {
        get
        {
            return LoadControl(AscxVirtualFilePath) as UserControl;
        }
    }

    protected XmlDocument HelpFileXml
    {
        get
        {
            XmlDocument document = new XmlDocument();
            document.Load(AbsoluteXmlFilePath);
            return document;
        }
    }

    protected bool XmlExists
    {
        get
        {
            if (VirtualXmlFilePath.Contains("http://"))
            {
                // Do not try to map spammy paths.
                return false;
            }

            return File.Exists(AbsoluteXmlFilePath);
        }
    }

    protected bool AscxExists
    {
        get { return File.Exists(Server.MapPath(AscxVirtualFilePath)); }
    }

    public Help() : base(ManxAds.WebsiteUserType.Public) { }

    protected override void InitializePage()
    {
        base.InitializePage();

        if (XmlExists)
        {
            HelpMultiView.SetActiveView(DetailsView);
            XmlDocument document = HelpFileXml;
            XmlNode root = document.DocumentElement;

            foreach (XmlNode child in root.ChildNodes)
            {
                switch (child.Name)
                {
                    case "Title":
                        TitleLabel.Text = child.InnerText;
                        this.Title = child.InnerText + " - Help";
                        break;

                    case "Description":
                        DescriptionLabel.Text = child.InnerText;
                        this.Description = child.InnerText;
                        break;

                    case "Body":
                        RenderBody(BodyLiteral, child);
                        DetailsMultiView.SetActiveView(XmlView);
                        break;
                }
            }

            if (AscxExists)
            {
                DetailsMultiView.SetActiveView(ControlView);
                ControlPanel.Controls.Add(HelpUserControl);
            }
        }
        else
        {
            HelpMultiView.SetActiveView(WelcomeView);
        }
    }

    public void RenderBody(Literal target, XmlNode bodyNode)
    {
        StringBuilder sourceCode = new StringBuilder();
        foreach (XmlNode section in bodyNode.ChildNodes)
        {
            string title = section.Attributes["Title"].Value;
            sourceCode.AppendLine("<h2>" + title + "</h2>");
            foreach (XmlNode paragraph in section.ChildNodes)
            {
                sourceCode.AppendLine("<p>" + paragraph.InnerText + "</p>");
            }
        }
        target.Text = sourceCode.ToString();
    }

    protected void WelcomeView_Activate(object sender, EventArgs e)
    {
        string linkFormat = "Help.aspx?Topic={0}";
        string path = Server.MapPath(LocalSettings.HelpDocumentsDirectory);
        List<HelpFileInfo> list = HelpFileInfo.GetHelpFileInfoList(path, linkFormat);

        HelpMenuRepeater.DataSource = list;
        HelpMenuRepeater.DataBind();
    }

    protected void InlineView_Load(object sender, EventArgs e)
    {
        if (ControlPanel.Controls.Count > 0)
        {
            UserControl innerConrol = ControlPanel.Controls[0] as UserControl;
            foreach (Control control in innerConrol.Controls)
            {
                if (control is Image)
                {
                    // Force border width so border shows.
                    Image webImage = control as Image;
                    webImage.BorderWidth = new Unit(1);

                    // ImageUrl has prefixed string "../", so change to "~/".
                    string path = Server.MapPath(webImage.ImageUrl.Replace("..", "~"));

                    // Set web image to width and height of physical image.
                    System.Drawing.Image physicalImage = System.Drawing.Image.FromFile(path);
                    webImage.Width = physicalImage.Width;
                    webImage.Height = physicalImage.Height;
                }
            }
        }
    }
}
