<%@ Page language="c#" CodeBehind="SecurityRoles.aspx.cs" AutoEventWireup="false" Inherits="www.strive3d.net.SecurityRoles" %>
<%@ Register TagPrefix="portal" TagName="Banner" Src="~/DesktopPortalBanner.ascx" %>

<%--
    The SecurityRoles.aspx page is used to create and edit security roles within
    the Portal application.
--%>

<HTML>
    <HEAD>
        <link rel="stylesheet" href='/portal.css' type="text/css">
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
                            <tr height="*" valign="top">
                                <td width="100">
                                    &nbsp;
                                </td>
                                <td width="*">
                                    <table width="450" cellpadding="2" cellspacing="4" border="0">
                                        <tr>
                                            <td colspan="2">
                                                <table width="100%" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td align="left">
                                                            <span id="title" class="Head" runat="server">Role Membership</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <hr noshade size="1">
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:Label id="Message" CssClass="NormalRed" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <table width="100%" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox id="windowsUserName" Text="DOMAIN\username" Visible="False" runat="server" />
                                                        </td>
                                                        <td class="Normal">
                                                            <asp:LinkButton id="addNew" cssclass="CommandButton" Text="Create new user and add to role" Visible="False" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:DropDownList id="allUsers" DataTextField="Email" DataValueField="UserID" runat="server" />
                                                        </td>
                                                        <td>
                                                            <asp:LinkButton id="addExisting" cssclass="CommandButton" Text="Add existing user to role" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:DataList id="usersInRole" RepeatColumns="2" DataKeyField="UserId" runat="server">
                                                    <ItemStyle Width="225" />
                                                    <ItemTemplate>
                                                        &nbsp;&nbsp;
                                                        <asp:ImageButton ImageUrl="~/images/delete.gif" CommandName="delete" AlternateText="Remove this user from role" runat="server" />
                                                        <asp:Label Text='<%# DataBinder.Eval(Container.DataItem, "Email") %>' cssclass="Normal" runat="server" />
                                                    </ItemTemplate>
                                                </asp:DataList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <hr noshade size="1">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:LinkButton id="saveBtn" class="CommandButton" Text="Save Role Changes" runat="server" />
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
