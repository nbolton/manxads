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
using System.IO;
using System.Xml;

/// <summary>
/// Describes a help article.
/// </summary>
public class HelpFileInfo
{
    private string _fileName;
    private string _title;
    private string _description;
    private string _linkFormat;

    public string Title
    {
        get { return _title; }
    }

    public string Description
    {
        get { return _description; }
    }

    public string HyperLink
    {
        get
        {
            FileInfo info = new FileInfo(_fileName);

            // Remove the file extension from the name to get topic ID.
            string id = info.Name.Replace(info.Extension, String.Empty);

            return String.Format(_linkFormat, id);
        }
    }

    protected HelpFileInfo(string fileName, string linkFormat)
    {
        _fileName = fileName;
        _linkFormat = linkFormat;
    }

    public static HelpFileInfo Parse(string fileName, string linkFormat)
    {
        HelpFileInfo fileInfo = new HelpFileInfo(fileName, linkFormat);
        XmlDocument document = new XmlDocument();

        document.Load(fileName);
        XmlNode root = document.DocumentElement;
        XmlNode title = root.FirstChild;
        XmlNode description = title.NextSibling;

        fileInfo._title = title.InnerText.Trim();
        fileInfo._description = description.InnerText.Trim();

        return fileInfo;
    }

    /// <summary>
    /// Traverses a directory and creates a 
    /// </summary>
    /// <param name="directory"></param>
    /// <returns></returns>
    public static List<HelpFileInfo> GetHelpFileInfoList(
        string directory, string linkFormat)
    {
        string[] fileNameArray = Directory.GetFiles(directory);
        List<HelpFileInfo> infoList = new List<HelpFileInfo>();

        foreach (string fileName in fileNameArray)
        {
            // Index only XML files.
            FileInfo fileInfo = new FileInfo(fileName);
            if (fileInfo.Extension == ".xml")
            {
                infoList.Add(HelpFileInfo.Parse(fileName, linkFormat));
            }
        }

        return infoList;
    }
}
