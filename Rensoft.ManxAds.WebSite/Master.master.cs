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

    protected void Page_Load(object sender, EventArgs e)
    {
        // seasonal control changes
        if (DateTime.Now.Month == 12) {
            LayoutHeaderLogoImage.ImageUrl = "~/Images/Static/Layout/ChristmasLogo.gif";
            LayoutHeaderLogoImage.AlternateText = "Merry Christmas from ManxAds!";
        }

        AdvertDebug.Value = LocalSettings.AdvertDebug.ToString();

        leaderboardRotators.Clear();
        leaderboardRotators.AddRange(new AdvertRotator[] {
            TopAdvertRotator,
            BottomAdvertRotator
        });

        squareButtonRotators.Clear();
        squareButtonRotators.AddRange(new AdvertRotator[] {
            LeftAdvertRotator1,
            LeftAdvertRotator2,
            LeftAdvertRotator3,
            LeftAdvertRotator4,
            LeftAdvertRotator5,
            LeftAdvertRotator6
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

        TopAdvertRotator.PositionType = AdvertPositionType.Top;
        LeftAdvertRotator1.PositionType = AdvertPositionType.Left1;
        LeftAdvertRotator2.PositionType = AdvertPositionType.Left2;
        LeftAdvertRotator3.PositionType = AdvertPositionType.Left3;
        LeftAdvertRotator4.PositionType = AdvertPositionType.Left4;
        LeftAdvertRotator5.PositionType = AdvertPositionType.Left5;
        LeftAdvertRotator6.PositionType = AdvertPositionType.Left6;
        RightAdvertRotator.PositionType = AdvertPositionType.Right;
        BottomAdvertRotator.PositionType = AdvertPositionType.Bottom;
    }
}