using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using ManxAds;
using System.IO;


public partial class Master : StandardMaster
{
    private Queue<Advert> leaderboardQueue;
    private Queue<Advert> squareButtonQueue;
    private List<AdvertRotator> leaderboardRotators;
    private List<AdvertRotator> squareButtonRotators;

    /// <summary>
    /// Gets the full website root for HTML references
    /// which are not controlled.
    /// </summary>
    protected string WebsiteRoot
    {
        get
        {
            if (Request.ApplicationPath != "/")
            {
                return Request.ApplicationPath;
            }

            return null;
        }
    }

    public Master()
    {
        leaderboardQueue = new Queue<Advert>();
        squareButtonQueue = new Queue<Advert>();
        leaderboardRotators = new List<AdvertRotator>();
        squareButtonRotators = new List<AdvertRotator>();
    }

    private List<Advert> randomizeAdverts(List<Advert> advertsList)
    {
        List<int> usedIndexes = new List<int>();
        List<Advert> randomList = new List<Advert>();
        Random random = new Random(Environment.TickCount);

        while (randomList.Count < advertsList.Count)
        {
            int randomIndex;
            do
            {
                // Try to find an unused index.
                randomIndex = random.Next(0, advertsList.Count);
            }
            while (usedIndexes.Contains(randomIndex));

            usedIndexes.Add(randomIndex);
            randomList.Add(advertsList[randomIndex]);
        }
        return randomList;
    }

    protected bool OfflineForMaintenance
    {
        get
        {
            return (Request.QueryString["DisableOfflineForMaintenance"] == null)
                && Convert.ToBoolean(ConfigurationManager.AppSettings["OfflineForMaintenance"]);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        // if offline, hide the "whole page" div and don't do any more load stuff
        if (OfflineForMaintenance)
        {
            MaintenanceView.Visible = true;
            LayoutWholePage.Visible = false;
            return;
        }

        // seasonal control changes
        if (DateTime.Now.Month == 12) {
            LayoutHeaderLogoImage.ImageUrl = "~/Images/Static/Layout/ChristmasLogo.gif";
            LayoutHeaderLogoImage.AlternateText = "Merry Christmas from ManxAds!";
        }

        AdvertDebug.Value = LocalSettings.AdvertDebug.ToString();

        leaderboardRotators.Clear();
        leaderboardRotators.AddRange(new AdvertRotator[] {
            TopLeaderboardAdvertRotator,
            BottomLeaderboardAdvertRotator
        });

        squareButtonRotators.Clear();
        squareButtonRotators.AddRange(new AdvertRotator[] {
            LeftAdvertRotator1,
            LeftAdvertRotator2,
            LeftAdvertRotator3,
            LeftAdvertRotator4
        });

        // Queue random adverts to be semi-evenly distributed top-down.
        foreach (Advert advert in randomizeAdverts(Advert.Fetch(AdvertPositionType.RandomLeaderboard)))
        {
            leaderboardQueue.Enqueue(advert);
        }
        foreach (Advert advert in randomizeAdverts(Advert.Fetch(AdvertPositionType.RandomSquareButton)))
        {
            squareButtonQueue.Enqueue(advert);
        }

        for (int i = 0; i < leaderboardRotators.Count; i++)
        {
            // Break if queue has been emptied.
            if (leaderboardQueue.Count == 0) break;

            leaderboardRotators[i].AdvertsList.Add(leaderboardQueue.Dequeue());

            // Reset back to beginning.
            if (i == (leaderboardRotators.Count - 1)) i = -1;
        }

        for (int i = 0; i < squareButtonRotators.Count; i++)
        {
            // Break if queue has been emptied.
            if (squareButtonQueue.Count == 0) break;

            squareButtonRotators[i].AdvertsList.Add(squareButtonQueue.Dequeue());

            // Reset back to beginning.
            if (i == (squareButtonRotators.Count - 1)) i = -1;
        }

        List<string> keywordsList = new List<string>();
        List<Category> categoryList = Category.Fetch();
        foreach (Category category in categoryList)
        {
            keywordsList.Add(category.Title.ToLower());
        }
        base.Keywords = String.Join(", ", keywordsList.ToArray());

        if (String.IsNullOrEmpty(this.Description))
        {
            string path = Server.MapPath(LocalSettings.PageDescriptionsPath);
            PageDescriptions descriptions = new PageDescriptions(path);
            string[] fileNameParts = Request.FilePath.Split('/');
            string fileName = fileNameParts[fileNameParts.Length - 1];
            base.Description = descriptions[fileName];
        }

        TopLeaderboardAdvertRotator.PositionType = AdvertPositionType.TopLeaderboard;
        LeftAdvertRotator1.PositionType = AdvertPositionType.Square1;
        LeftAdvertRotator2.PositionType = AdvertPositionType.Square2;
        LeftAdvertRotator3.PositionType = AdvertPositionType.Square3;
        LeftAdvertRotator4.PositionType = AdvertPositionType.Square4;
        SkyscraperAdvertRotator.PositionType = AdvertPositionType.Skyscraper;
        BottomLeaderboardAdvertRotator.PositionType = AdvertPositionType.BottomLeaderboard;
    }

    // fix for Bug #228 - Old layout is still being cached for some visitors
    public string GetDynamicPath(string virtualPath)
    {
        string mappedPath = MapPath(virtualPath);
        FileInfo styleSheetInfo = new FileInfo(mappedPath);
        
        // not sure if this is the best approach to resolve virtual paths.
        string relativePath = virtualPath.Replace("~/", Request.ApplicationPath);

        return relativePath + "?" + styleSheetInfo.LastWriteTime.Ticks;
    }
}
