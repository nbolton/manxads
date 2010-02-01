<%@ Page Language="C#" AutoEventWireup="true" Inherits="GenericError" Codebehind="GenericError.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Application Error</title>
    <meta name="robots" content="noindex,nofollow" />
    <link rel="stylesheet" type="text/css" href="~/Styles/StyleSheet.css" />
</head>
<body>
    <form id="form1" runat="server">
        <h1><span style="color: #2166B2">Manx</span><span style="color: #333333">Ads</span></h1>
        <p>
            An application error has occurred, we're sorry for any inconvenience.<br />
            The error has been logged, and we shall endeavour to solve it as soon as we can.
        </p>
        <h3>Suggestions:</h3>
        <ul>
            <li>Return to the <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Default.aspx">ManxAds home page</asp:HyperLink>.</li>
            <li>Click the back button on your web browser and try again.</li>
        </ul>
    </form>
</body>
</html>
