<%@ Page CodeBehind="ModuleSettings.aspx.cs" Language="c#" AutoEventWireup="false" Inherits="www.strive3d.net.ModuleSettingsPage" %>
<%@ Register TagPrefix="portal" TagName="Banner" Src="~/DesktopPortalBanner.ascx" %>

<%--
    The ModuleSettings.aspx page is used to enable administrators to view/edit/update
    a portal module's settings (title, output cache properties, edit access)
--%>

<HTML>
  <HEAD>
        <link rel="stylesheet" href='/net/strive3d/portal.css' type="text/css">
  </HEAD>
    <body leftmargin="0" bottommargin="0" rightmargin="0" topmargin="0" marginheight="0" marginwidth="0">
        <form runat="server">
            <table width="100%" cellspacing="0" cellpadding="0" border="0">
                <tr valign="top">
                    <td colspan="2">
                        <portal:Banner ShowTabs="false" runat="server" id="Banner1" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <br>
                        <table width="98%" cellspacing="0" cellpadding="4" border="0">
                            <tr valign="top">
                                <td width="150">
                                    &nbsp;
                                </td>
                                <td width="*">
                                    <table cellpadding="2" cellspacing="1" border="0">
                                        <tr>
                                            <td colspan="4">
                                                <table width="100%" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td align="left" class="Head">
                                                            Module Settings
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <hr noshade size="1">
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="100" class="SubHead">
                                                Module Name:
                                            </td>
                                            <td colspan="3">
                                                &nbsp;<asp:Textbox id="moduleTitle" width="300" cssclass="NormalTextBox" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="SubHead">
                                                Cache Timeout (seconds):
                                            </td>
                                            <td colspan="3">
                                                &nbsp;<asp:Textbox id="cacheTime" width="100" cssclass="NormalTextBox" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td colspan="3">
                                                <hr noshade size="1">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="SubHead">
                                                Roles that can edit content:
                                            </td>
                                            <td colspan="3">
                                                <asp:CheckBoxList id="authEditRoles" RepeatColumns="2" Font-Names="Verdana,Arial" Font-Size="8pt" width="300" cellpadding="0" cellspacing="0" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td colspan="3">
                                                <hr noshade size="1">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="SubHead" nowrap>
                                                Show to mobile users?:
                                            </td>
                                            <td colspan="3">
                                                <asp:Checkbox id="showMobile" Font-Names="Verdana,Arial" Font-Size="8pt" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <hr noshade size="1">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:LinkButton class="CommandButton" Text="Apply Module Changes" runat="server" id="ApplyButton" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </form>
    </body>
</HTML>
