<%@ Control Language="C#" AutoEventWireup="true" Inherits="AdvertRotator" Codebehind="AdvertRotator.ascx.cs" %>
<%@ Register Src="FlashAdvertInline.ascx" TagName="FlashAdvertInline" TagPrefix="uc1" %>

<asp:MultiView ID="ModeMultiView" runat="server">
    <asp:View ID="AdvertView" runat="server">
        <asp:Literal ID="AdvertGroupAdd" runat="server" />
        <asp:Repeater ID="AdvertRepeater" runat="server" OnItemDataBound="AdvertRepeater_ItemDataBound">
            <ItemTemplate>
                <asp:Literal ID="LayerStart" runat="server" OnInit="LayerStart_Init" />
                <asp:MultiView ID="TypeMultiView" runat="server" OnInit="TypeMultiView_Init">
                    <asp:View ID="ImageView" runat="server" OnInit="ImageView_Init">
                        <asp:HyperLink rel="nofollow" ID="AdvertImage" runat="server" Target="_blank" OnInit="AdvertImage_Init" />
                    </asp:View>
                    <asp:View ID="FlashView" runat="server" OnInit="FlashView_Init">
                        <uc1:FlashAdvertInline ID="FlashAdvertInline" runat="server" OnInit="FlashAdvertInline_Init" />
                    </asp:View>
                    <asp:View ID="HtmlView" runat="server" OnInit="HtmlView_Init">
                        <asp:Literal ID="HtmlLiteral" runat="server" OnInit="HtmlLiteral_Init" />
                    </asp:View>
                </asp:MultiView>
                <asp:Literal ID="LayerEnd" runat="server" OnInit="LayerEnd_Init" />
            </ItemTemplate>
        </asp:Repeater>
        <asp:Literal ID="AdvertGroupRotate" runat="server" />
    </asp:View>
    <asp:View ID="EmptyView" runat="server" OnActivate="EmptyView_Activate">
        <asp:Image ID="PlaceholderImage" runat="server" />
    </asp:View>
</asp:MultiView>