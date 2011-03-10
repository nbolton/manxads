<%@ Application Language="C#" %>

<script runat="server">

    void Application_BeginRequest(object sender, EventArgs e)
    {
        bool redirected = legacyCategoryRedirect();
        
        if (!redirected)
        {
            System.Web.Configuration.HttpRuntimeSection configSection =
                ConfigurationManager.GetSection("system.web/httpRuntime") as
                System.Web.Configuration.HttpRuntimeSection;

            if (Request.ContentLength > (configSection.MaxRequestLength * 1024))
            {
                // Putting code here is pointless, as the response gets terminated anyway.
            }
        }
    }

    private bool checkInvalidRequest(Exception exception)
    {
        string[] invalidRequestMessages = new string[] {
                "A potentially dangerous Request",
                "The state information is invalid",
                "Validation of viewstate MAC failed",
                "This is an invalid webresource request",
                "Invalid postback or callback argument",
                "The client disconnected."
            };

        // Where any message part matches, indicate invalid request.
        foreach (string message in invalidRequestMessages)
        {
            if (exception.Message.Contains(message))
            {
                return true;
            }
        }
        return false;
    }
    
    private bool legacyCategoryRedirect()
    {
        int categoryId = 0;
        if (Request.Url.AbsoluteUri.Contains("/Cars/"))
        {
            categoryId = 1;
        }
        else if (Request.Url.AbsoluteUri.Contains("/Toys/"))
        {
            categoryId = 75;
        }
        else if (Request.Url.AbsoluteUri.Contains("/Animals/"))
        {
            categoryId = 48;
        }
        else if (Request.Url.AbsoluteUri.Contains("/Property/"))
        {
            categoryId = 13;
        }

        if (categoryId != 0)
        {
            Response.Redirect("~/Listings.aspx?Title=Cars&CategoryId=" + categoryId);
            return true;
        }
        else
        {
            return false;
        }
    }
        
    void Application_Error(object sender, EventArgs e)
    {
        bool invalidRequest = checkInvalidRequest(Server.GetLastError());

        if (invalidRequest)
        {
            Response.Redirect("~/InvalidRequest.aspx");
        }
        else
        {
#if !DEBUG
            ErrorReporting.RecordException(Server.GetLastError(), Request, Response);
#endif
        }
    }
       
</script>
