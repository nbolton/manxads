<%@ Page Language="C#" AutoEventWireup="true" Inherits="ListingDetailsPopup" Codebehind="ListingDetailsPopup.aspx.cs" %>
<%@ Register Src="ListingDetails.ascx" TagName="ListingDetails" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Listing Details - ManxAds</title>
    <meta name="description" id="descriptionMeta" content="" />
    <meta name="robots" content="noindex,follow" />
	<link rel="stylesheet" type="text/css" href="~/Styles/StyleSheet.css" />
</head>
<body>
    <form id="form1" runat="server">
        <uc1:ListingDetails ID="ListingDetails" runat="server" />
    </form>
</body>
</html>
