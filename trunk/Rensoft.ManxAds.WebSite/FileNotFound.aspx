<%@ Page Language="C#" AutoEventWireup="true" Inherits="FileNotFound" Codebehind="FileNotFound.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>404 (File Not Found)</title>
    <meta name="robots" content="noindex,nofollow" />
    <link rel="stylesheet" type="text/css" href="~/Styles/StyleSheet.css" />
</head>
<body>
    <form id="form1" runat="server">
        <h1><span style="color: #2166B2">Manx</span><span style="color: #333333">Ads</span></h1>
        <p>The page or file you requested does not exist or has been removed.</p>
        <h3>Suggestions:</h3>
        <ul>
            <li>Return to the <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Default.aspx">ManxAds home page</asp:HyperLink>.</li>
            <li>Try <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/Search.aspx">searching</asp:HyperLink> for what you want.</li>
        </ul>
    </form>
</body>
</html>
