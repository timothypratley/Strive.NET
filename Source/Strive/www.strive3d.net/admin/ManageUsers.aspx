<%@ Page language="c#" CodeBehind="ManageUsers.aspx.cs" AutoEventWireup="false" Inherits="www.strive3d.net.ManageUsers" %>
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
                    <td width="100">
                        &nbsp;
                    </td>
                    <td>
                        <br>
                        <table width="450" cellspacing="0" cellpadding="4" border="0">
                            <tr height="*" valign="top">
                                <td colspan="2">
                                    <table width="100%" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td align="left">
                                                <span id="title" class="Head" runat="server">Manage User</span>
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
                                <td class="Normal">
                                    Email (or Windows domain name):
                                </td>
                                <td>
                                    <asp:Textbox id="Email" width="200" cssclass="NormalTextBox" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="Normal">
                                    Password:
                                </td>
                                <td>
                                    <asp:Textbox id="Password" width="200" cssclass="NormalTextBox" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:LinkButton Text="Apply Name and Password Changes" cssclass="CommandButton" runat="server" ID="UpdateUserBtn" />
                                    <br>
                                    <br>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:DropDownList id="allRoles" DataTextField="RoleName" DataValueField="RoleID" runat="server" />
                                    &nbsp;<asp:LinkButton id="addExisting" cssclass="CommandButton" Text="Add user to this role" runat="server" />
                                </td>
                            </tr>
                            <tr valign="top">
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:DataList id="userRoles" RepeatColumns="2" DataKeyField="RoleId" runat="server">
                                        <ItemStyle Width="225" />
                                        <ItemTemplate>
                                            &nbsp;&nbsp;
                                            <asp:ImageButton ImageUrl="~/images/delete.gif" CommandName="delete" AlternateText="Remove user from this role" runat="server" ID="Imagebutton1" />
                                            <asp:Label Text='<%# DataBinder.Eval(Container.DataItem, "RoleName") %>' cssclass="Normal" runat="server" ID="Label1" />
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
                                    <asp:LinkButton id="saveBtn" class="CommandButton" Text="Save User Changes" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </form>
    </body>
</HTML>
