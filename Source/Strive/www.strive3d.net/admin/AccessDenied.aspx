<%@ Page CodeBehind="AccessDenied.aspx.cs" Language="c#" AutoEventWireup="false" Inherits="www.strive3d.net.AccessDeniedPage" %>
<%@ Register TagPrefix="portal" TagName="Banner" Src="~/DesktopPortalBanner.ascx" %>
<%@ OutputCache Duration="36000" VaryByParam="none" %>

<html>
    <head>
        <title>ASP.NET Portal</title>
        <link rel="stylesheet" href='<%=www.strive3d.net.Utils.ApplicationPath%>/portal.css' type="text/css" />
    </head>
    <body leftmargin="0" bottommargin="0" rightmargin="0" topmargin="0" marginheight="0" marginwidth="0">
        <form runat="server">
            <table width="100%" cellspacing="0" cellpadding="0" border="0">
                <tr valign="top">
                    <td colspan="2">
                        <portal:Banner runat="server" />
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <center>
                            <br>
                            <table width="500" border="0">
                                <tr>
                                    <td class="Normal">
                                        <br>
                                        <br>
                                        <br>
                                        <br>
                                        <span class="Head">Access Denied</span>
                                        <br>
                                        <br>
                                        <hr noshade size="1pt">
                                        <br>
                                        Either you are not currently logged in, or you do not have access to this tab 
                                        page within the portal. Please contact the portal administrator to obtain 
                                        access.
                                        <br>
                                        <br>
                                        <a href="<%=www.strive3d.net.Utils.ApplicationPath%>/DesktopDefault.aspx">Return to Portal Home</a>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </td>
                </tr>
            </table>
        </form>
    </body>
</html>
