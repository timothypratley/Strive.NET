<%@ Control CodeBehind="DesktopModuleTitle.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="www.strive3d.net.DesktopModuleTitle" %>

<%--

   The PortalModuleTitle User Control is responsible for displaying the title of each
   portal module within the portal -- as well as optionally the module's "Edit Page"
   (if such a page has been configured).

--%>


<table width="98%" cellspacing="0" cellpadding="0">
    <tr>
        <td align="left">
            <asp:label id="ModuleTitle" cssclass="Head" EnableViewState="false" runat="server" />
        </td>
        <td align="right">
            <asp:hyperlink id="EditButton" cssclass="CommandButton" EnableViewState="false" runat="server" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <hr noshade size="1">
        </td>
    </tr>
</table>
