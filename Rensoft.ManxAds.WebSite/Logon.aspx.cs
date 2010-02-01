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
using System.Security.Principal;
using Rensoft.ErrorReporting;
using ManxAds;

public partial class Logon : StandardPage
{
    public Logon() : base(ManxAds.WebsiteUserType.Public) { }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        RobotFlags = RobotFlag.NoIndex | RobotFlag.Follow;
    }

    protected override void InitializePage()
    {
        if (Auth.IsAuthenticated)
        {
            Response.Redirect(FormsAuthentication.DefaultUrl);
        }

        DefaultButton logonDefault = new DefaultButton(LogOnButton);
        logonDefault.AssociateWith(LogonEmailAddressTextBox);
        logonDefault.AssociateWith(LogonPasswordTextBox);
        logonDefault.AssociateWith(PersistentCheckBox);

        DefaultButton registerDefault = new DefaultButton(RegisterButton);
        registerDefault.AssociateWith(RegisterEmailAddress);

        if (!IsPostBack)
        {
            InitializeLocationDropDownList();
        }
    }

    protected void InitializeLocationDropDownList()
    {
        const string unitedKingdomString = "United Kingdom";
        const string dividerString = "-------------------";
        ListItem divider = new ListItem(dividerString, "-1");

        List<ListItem> items = new List<ListItem>();
        items.Add(new ListItem("Choose...", "-1"));

        try
        {
            Location unitedKingdomLocation = Location.FetchByTitleString(unitedKingdomString);
            int unitedKingdomId = unitedKingdomLocation.DatabaseId;
            ListItem unitedKingdom = new ListItem(unitedKingdomString, unitedKingdomId.ToString());

            items.Add(divider);
            items.Add(unitedKingdom);
            items.Add(divider);
            items.Add(new ListItem("Isle of Man...", "-1"));
        }
        catch
        {
            // Purposely ignore errors.
        }

        Location.DataBind(LocationDropDownList, -1, items.ToArray());
    }

    protected void LogOnButton_Click(object sender, ImageClickEventArgs e)
    {
        string emailAddress = LogonEmailAddressTextBox.Text;
        string password = LogonPasswordTextBox.Text;

        WebsiteUser user;
        if (Security.Authenticate(
            emailAddress, password, 
            Request.ServerVariables["REMOTE_ADDR"],  out user))
        {
            FormsAuthentication.RedirectFromLoginPage(
                user.DatabaseId.ToString(), PersistentCheckBox.Checked);
        }
        else
        {
            ErrorPanel.Visible = true;
            ErrorLabel.Text =
                "Sorry, the logon attempt failed. Either your email address " +
                "or password were typed incorrectly. Please check  them, and try " +
                "again. If you are experiencing difficulty, please refer to the " +
                "Troubleshooting area at the bottom of this page.";
        }
    }

    protected void RegisterButton_Click(object sender, ImageClickEventArgs e)
    {
        if (LocationDropDownList.SelectedValue != null)
        {
            Session["RegisterLocation"] = int.Parse(LocationDropDownList.SelectedValue);
        }

        Session["RegisterEmail"] = RegisterEmailAddress.Text;
        Response.Redirect("~/Register.aspx");

        /*if (ResidentCheckBox.Checked)
        {
        }
        else
        {
            ErrorPanel.Visible = true;
            ErrorLabel.Text =
                "Please verify that you are an Isle of Man resident " +
                "by clicking the checkbox above the Register button.";
        }*/
    }
}
