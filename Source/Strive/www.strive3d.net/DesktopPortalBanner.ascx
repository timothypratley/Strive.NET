<%@ Control CodeBehind="DesktopPortalBanner.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="www.strive3d.net.DesktopPortalBanner" %>
<%@ Import Namespace="www.strive3d.net" %>
<%@ Register TagPrefix="Controls" TagName="Login" Src="DesktopModules/Login.ascx" %>

<%--

   The DesktopPortalBanner User Control is responsible for displaying the standard Portal
   banner at the top of each .aspx page.

   The DesktopPortalBanner uses the Portal Configuration System to obtain a list of the
   portal's sitename and tab settings. It then render's this content into the page.

--%>


<table width="100%" cellspacing="0" cellpadding="0" class="HeadBg" border="0">
    <tr class="TabsStrip" >
        <td colspan="3">
            <asp:datalist id="tabs" cssclass="OtherTabsBg" repeatdirection="horizontal" ItemStyle-Height="25" SelectedItemStyle-CssClass="TabBg" ItemStyle-BorderWidth="1" EnableViewState="false" runat="server">
                <ItemTemplate>
                    &nbsp;<a href='<%=www.strive3d.net.Utils.ApplicationPath%>/DesktopDefault.aspx?tabindex=<%# Container.ItemIndex %>&tabid=<%# ((TabStripDetails) Container.DataItem).TabId %>' class="OtherTabs"><%# ((TabStripDetails) Container.DataItem).TabName %></a>&nbsp;
                </ItemTemplate>
                <SelectedItemTemplate>
                    &nbsp;<span class="SelectedTab"><%# ((TabStripDetails) Container.DataItem).TabName %></span>&nbsp;
                </SelectedItemTemplate>
            </asp:datalist>
        </td>
        <td align="right">
            <a href="<%= Utils.ApplicationPath %>" class="SiteLink"><asp:label id="siteName" EnableViewState="false" runat="server" /></a> 
            <span class="Accent"> | </span> <Controls:Login runat="server" /></span></td>         
    </tr>
</table>
