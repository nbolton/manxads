<%@ Page Language="C#" AutoEventWireup="true" Inherits="ContactFormPopup" Codebehind="ContactFormPopup.aspx.cs" %>

<%@ Register Src="ContactForm.ascx" TagName="ContactForm" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Contact Seller - ManxAds</title>
    <meta name="description" id="descriptionMeta" content="" />
    <meta name="robots" content="noindex,follow" />
	<link rel="stylesheet" type="text/css" href="~/Styles/StyleSheet.css" />
</head>
<body>
    <form id="form1" runat="server">
        <uc1:ContactForm id="ContactForm1" runat="server" />
    </form>
</body>
</html>
