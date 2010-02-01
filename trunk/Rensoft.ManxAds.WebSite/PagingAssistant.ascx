<%@ Control Language="C#" AutoEventWireup="true" Inherits="PagingAssistant" Codebehind="PagingAssistant.ascx.cs" %>
<asp:Panel ID="PagingPanel" runat="server">
    <div style="width: 100%; text-align: center; margin-top: 5px">
        <asp:HyperLink ID="PreviousHyperLink" runat="server">&lt; Previous</asp:HyperLink> | 
        <asp:Repeater ID="PageNumberRepeater" runat="server">
            <ItemTemplate><asp:HyperLink ID="PageHyperLink" runat="server" Text='<%# Bind("PageNumber") %>' NavigateUrl='<%# Bind("NavigateUrl") %>' /></ItemTemplate>
            <SeparatorTemplate> | </SeparatorTemplate>
        </asp:Repeater>
         | <asp:HyperLink ID="NextHyperLink" runat="server">Next &gt;</asp:HyperLink>
     </div>
 </asp:Panel>