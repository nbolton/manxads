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
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using ManxAds;

public partial class AdvertRotator : System.Web.UI.UserControl
{
    private List<Advert> advertsList;

    protected MultiView TypeMultiView;
    protected View ImageView;
    protected View FlashView;
    protected View HtmlView;
    protected Literal HtmlLiteral;
    protected HyperLink AdvertImage;
    protected FlashAdvertInline FlashAdvertInline;
    protected Literal LayerStart;
    protected Literal LayerEnd;
    protected int AdvertEnumerator = 0;

    private Random random = new Random(Environment.TickCount);

    public AdvertPositionType PositionType;
    public Advert SingleAdvert;

    public List<Advert> AdvertsList
    {
        get { return advertsList; }
    }

    public AdvertRotator()
    {
        advertsList = new List<Advert>();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (SingleAdvert != null)
        {
            // Show single advert.
            Advert[] singleAdvertArray = new Advert[] { SingleAdvert };
            ModeMultiView.SetActiveView(AdvertView);
            AdvertRepeater.DataSource = singleAdvertArray;
            AdvertRepeater.DataBind();
            return;
        }

        // Do nothing if adverts are disabled.
        if (!LocalSettings.EnableAdverts)
        {
            ModeMultiView.SetActiveView(EmptyView);
            return;
        }
        
        advertsList.AddRange(Advert.Fetch(PositionType));
        if (advertsList.Count > 0)
        {
            Random random = new Random(Environment.TickCount);
            List<RandomAdvert> randomList = new List<RandomAdvert>();

            foreach (Advert advert in advertsList)
            {
                RandomAdvert randomAdvert = new RandomAdvert();
                randomAdvert.Advert = advert;

                int randomWeight = random.Next(0, advertsList.Count);
                float weightFactor = (1 + ((float)advert.Weight / 100));

                // still show advert randomly, but give adverts with a
                // higher weight more chance of appearing first.
                randomAdvert.Weight = (float)randomWeight * weightFactor;

                randomList.Add(randomAdvert);
            }

            // sort list in order of randomised weight
            randomList.Sort(new Comparison<RandomAdvert>(CompareRandomAdvert));

            List<Advert> resultList = new List<Advert>();
            if (randomList[0].Advert.IsExclusive)
            {
                // if ad is exclusive, don't allow any other ads to show on this page load
                resultList.Add(randomList[0].Advert);
            }
            else
            {
                // select non-exclusive ads when the first wasn't exclusive
                resultList.AddRange(randomList.Select(ra => ra.Advert).Where(a => !a.IsExclusive));

                // only use rotator javascript if not in exclusive mode
                AdvertGroupAdd.Text = "<script type=\"text/javascript\" " +
                    "language=\"javascript\">AddGroup('" + ClientID + "')</script>";
                AdvertGroupRotate.Text = "<script type=\"text/javascript\" " +
                    "language=\"javascript\">Rotate('" + ClientID + "')</script>";
            }

            AdvertRepeater.DataSource = resultList;
            AdvertRepeater.DataBind();

            ModeMultiView.SetActiveView(AdvertView);
        }
        else
        {
            ModeMultiView.SetActiveView(EmptyView);
        }
    }

    private int CompareRandomAdvert(RandomAdvert x, RandomAdvert y)
    {
        // reverse the normal sort result (for descending sort)
        return -x.Weight.CompareTo(y.Weight);
    }

    protected void AdvertRepeater_ItemDataBound(
        object sender, RepeaterItemEventArgs e)
    {
        Advert advert = e.Item.DataItem as Advert;
        string group = this.ClientID;
        string layerId = group + "_" + AdvertEnumerator;
        int timer = advert.RotateFrequency;

        switch (advert.FormatType)
        {
            case AdvertFormatType.Flash:
                FlashAdvertInline.DatabaseId = advert.DatabaseId;
                FlashAdvertInline.LayerId = layerId;
                TypeMultiView.SetActiveView(FlashView);
                break;

            case AdvertFormatType.Html:
                HtmlLiteral.Text = advert.Html;
                TypeMultiView.SetActiveView(HtmlView);
                break;

            default:
                AdvertImage.ImageUrl = advert.MediaUrl;
                //AdvertImage.NavigateUrl = advert.NavigateUrl;
                AdvertImage.NavigateUrl = advert.RedirectUrl;
                AdvertImage.Width = advert.Width;
                AdvertImage.Height = advert.Height;
                TypeMultiView.SetActiveView(ImageView);
                break;
        }

        // don't show rotator javascript if in single advert mode,
        // or if current ad is exclusive (there is only 1 shown),
        // which is really the same thing as single advert mode.
        if ((SingleAdvert == null) && !advert.IsExclusive)
        {
            LayerStart.Text = string.Format(
                "<div id='{0}' class='AdvertDefaultState' >", layerId);

            if (LocalSettings.AdvertDebug)
            {
                LayerStart.Text += string.Format(
                    "<div id='{0}_debug' class='AdvertDebug'></div>", layerId);
            }

            LayerEnd.Text = string.Format(
                "</div><script type=\"text/javascript\" " +
                "language=\"javascript\">Add('{0}', {1}, {2})</script>",
                group, AdvertEnumerator, timer);
        }

        AdvertEnumerator++;
    }

    protected int AdvertRotator_AdvertRandomise(Advert x, Advert y)
    {
        // Must be equal when items are the same.
        if (x == y) return 0;

        return random.Next(-1, +1);
    }

    protected void TypeMultiView_Init(object sender, EventArgs e)
    {
        TypeMultiView = sender as MultiView;
    }

    protected void ImageView_Init(object sender, EventArgs e)
    {
        ImageView = sender as View;
    }

    protected void HtmlView_Init(object sender, EventArgs e)
    {
        HtmlView = sender as View;
    }

    protected void HtmlLiteral_Init(object sender, EventArgs e)
    {
        HtmlLiteral = sender as Literal;
    }

    protected void FlashView_Init(object sender, EventArgs e)
    {
        FlashView = sender as View;
    }

    protected void AdvertImage_Init(object sender, EventArgs e)
    {
        AdvertImage = sender as HyperLink;
    }

    protected void FlashAdvertInline_Init(object sender, EventArgs e)
    {
        FlashAdvertInline = sender as FlashAdvertInline;
    }

    protected void LayerStart_Init(object sender, EventArgs e)
    {
        LayerStart = sender as Literal;
    }

    protected void LayerEnd_Init(object sender, EventArgs e)
    {
        LayerEnd = sender as Literal;
    }

    protected void EmptyView_Activate(object sender, EventArgs e)
    {
        switch (PositionType)
        {
            case AdvertPositionType.TopLeaderboard:
                PlaceholderImage.ImageUrl =
                    "~/Images/Static/Layout/PlaceholderLeaderboard.gif";
                break;

            case AdvertPositionType.BottomLeaderboard:
                PlaceholderImage.ImageUrl =
                    "~/Images/Static/Layout/PlaceholderLeaderboard.gif";
                break;

            case AdvertPositionType.Skyscraper:
                PlaceholderImage.ImageUrl =
                    "~/Images/Static/Layout/PlaceholderSkyscraper.gif";
                break;

            default:
                PlaceholderImage.ImageUrl =
                    "~/Images/Static/Layout/PlaceholderSquareButton.gif";
                break;
        }
    }

    class RandomAdvert
    {
        public float Weight
        {
            get;
            set;
        }

        public Advert Advert
        {
            get;
            set;
        }
    }
}
