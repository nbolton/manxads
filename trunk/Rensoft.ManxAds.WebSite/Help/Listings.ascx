<%@ Control Language="C#" AutoEventWireup="true" Inherits="Help_WebUserControl" Codebehind="Listings.ascx.cs" %>
<h2>Getting Started</h2>
<p>Creating your own free classified listings couldn't be easier. The quickest way
    is to click <strong>Sell</strong>
    at the top of any page. Alternatively you could access it from My ManxAds,
    by clicking on <strong>New Listing</strong>.</p>
<ul>
    <li><a href="#details">Details</a></li>
    <li><a href="#price_types">Price Types</a></li>
    <li><a href="#categories">Categories</a></li>
    <li><a href="#boosting">Boosting</a></li>
    <li><a href="#editing">Editing</a></li>
</ul>
<a id="details" />
<h2>Details</h2>
<p>Once on the new listing screen, choose a title which makes your item stand out. It is a good idea not to use all capital letters. This is because they take up more room on the ManxAds home page, and when 'trimmed' less of the letters will appear than if lower case were used.</p>
<p>
    <asp:Image ID="Image1" runat="server" ImageUrl="../Images/Static/Help/Listing-Details.png" /></p>
    <p>
        Some users are often tempted to add asterisks (*) and other symbols to the title to make it stand out. It is acceptable to do this, but remember, it may not work in your favour when the listing is on the ManxAds home page.</p>

<a id="price_types" />
<h2>
    Price Types</h2>
    <p>
        ManxAds lets you choose between 4 price types. Negotiable, non-negotiable, free
        listing or no price. The first two require you to enter a price, where the last
        two do not. For example, if you are hosting a free event, or giving something away
        (like an old sofa, or garden shed) choose "free listing".</p>
        <h3>Negotiable Example</h3>
    <p>
        <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/Static/Help/Listing-Price-A.png" />
    </p>
        <h3>Free Listing Example</h3>
    <p>
        <asp:Image ID="Image3" runat="server" ImageUrl="~/Images/Static/Help/Listing-Price-B.png" /></p>
    <a id="categories" />
    <h2>
        Categories</h2>
<p>You can add your listing to as many categories as you like. But, it is a good idea to choose only the categories which apply.</p>
<p>
    <asp:Image ID="Image4" runat="server" ImageUrl="~/Images/Static/Help/Listing-Categories-A.png" />&nbsp;</p>
<p>To add a category, simply choose a category from the list and click the <strong>Add</strong>
    button.</p>
<p>
    <asp:Image ID="Image5" runat="server" ImageUrl="~/Images/Static/Help/Listing-Categories-B.png" />&nbsp;</p>
<p>
    Categories you add are shown below. To remove the listing from a category, click
    the <strong>Delete</strong> link below the category. A handy tip to remember is that, the more categories you add your item to, the lower it will appear in search results.</p>
<h2>Images &amp; Photos</h2>
<p>Images are optional, but when you add images to your listing, we display it on the ManxAds home page. So listings with pictures get more attention.</p>
<p>The thumbnail of your picture which we display in the listing browser and search results is a square image cut out of the original picture (this is called 'cropping'), so it's a good idea to make sure what ever is in the picture is in the middle. The full (un-cropped) version of your image is displayed on your listing below the details text, and is shrunk to fit.</p>
<p>
    To upload an image, simply navigate to the <em>Images</em> step and click the <strong>
        Browse</strong> button. The file browser for your Internet Browser should open
    up for you to choose an image on your computer. In this example we have used the
    Microsoft Internet Explorer file browser, so remember it may look a little different
    on your computer.</p>
<p>
    <asp:Image ID="Image6" runat="server" ImageUrl="~/Images/Static/Help/Listing-Images-A.png" />&nbsp;</p>
<p>
    If you are using Microsoft Internet Explorer, part of the file browser may look
    like the above. We recommend you change to the <strong>Thumbnails</strong> view
    so you can see which image you are uploading.</p>
<p>
    <asp:Image ID="Image7" runat="server" ImageUrl="~/Images/Static/Help/Listing-Images-B.png" />&nbsp;</p>
<p>
    Once you have clicked on your image, simply click on the <strong>Open</strong> button,
    which will tell ManxAds which image you would like to upload.</p>
<p>
    <asp:Image ID="Image8" runat="server" ImageUrl="~/Images/Static/Help/Listing-Images-C.png" />&nbsp;</p>
<p>
    Now that you have selected an image to upload, you now need to click the <strong>Upload</strong>
    button to upload the image to your ManxAds listing.</p>
<p>
    <asp:Image ID="Image9" runat="server" ImageUrl="~/Images/Static/Help/Listing-Images-D.png" />&nbsp;</p>
<p>
    To delete an image, simply click the <strong>Delete</strong> link beneath that image.
    If you would like to change the preview thumbnail, simply click <strong>Set Preview</strong>
    beneath 
    that image. When an image is the preview image, it will be marked as <em>Preview
        On</em>. Your preview thumbnail is the smaller image which shows up next to your
    listing on the ManxAds home page, categories and search results.</p>
<a id="boosting" />
<h2>Boosting</h2>
<p>Want your item to be on the ManxAds home page? Then you should try <em>boosting</em>.
    To boost a listing, simply go to My ManxAds and choose <strong>My Listings</strong>,
    then click the <strong>Boost</strong> link (if it is enabled).</p>
<p>
    <asp:Image ID="Image10" runat="server" ImageUrl="~/Images/Static/Help/Listing-Boosting.png" />&nbsp;</p>
<p>There is a time limit on boosting. Once you boost your listing it becomes locked in it's current position until the time limit has passed. When a listing is disabled from boosting, the boost link is disabled, and the time until you can boost next is displayed.</p>
<a id="editing" />
<h2>Editing</h2>
<p>To edit your listings, first, go to My ManxAds, then <strong>My Listings</strong>.
    Click the <strong>Edit</strong> link, which takes you to the listing editor wizard.
    If you're finished editing at any time, simply click <strong>Finish</strong> button
    at the bottom of any step.</p>
<p>Also, you can jump straight to any step of the wizard by using the navigation links at the top of each step. You can even change text on the details page, go straight to the images or categories steps, and when you click Finish, all of your changes will be applied.</p>