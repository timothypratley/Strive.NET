<%@ Page language="c#" CodeBehind="TabLayout.aspx.cs" AutoEventWireup="false" Inherits="www.strive3d.net.TabLayout" %>
<%@ Register TagPrefix="portal" TagName="Banner" Src="~/DesktopPortalBanner.ascx" %>

<HTML>
  <HEAD>

<%--
     The TabLayout.aspx page is used to control the layout settings of an
     individual tab within the portal.
--%>
        <link rel="stylesheet" href='<%= Utils.ApplicationPath + "/net/strive3d/Portal.css" %>' type="text/css">
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
                        <table width="98%" cellspacing="0" cellpadding="4">
                            <tr valign="top">
                                <td width="150">
                                    &nbsp;
                                </td>
                                <td width="*">
                                    <table border="0" cellpadding="2" cellspacing="1">
                                        <tr>
                                            <td colspan="4">
                                                <table width="100%" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td align="left" class="Head">
                                                            Tab Name and Layout
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
                                            <td width="100" class="Normal">
                                                Tab Name:
                                            </td>
                                            <td colspan="3">
                                                <asp:Textbox id="tabName" width="300" cssclass="NormalTextBox" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="Normal" nowrap>
                                                Authorized Roles:
                                            </td>
                                            <td colspan="3">
                                                <asp:CheckBoxList id="authRoles" RepeatColumns="2" Font-Names="Verdana,Arial" Font-Size="8pt" width="300" runat="server" />
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
                                            <td class="Normal" nowrap>
                                                Show to mobile users?:
                                            </td>
                                            <td colspan="3">
                                                <asp:Checkbox id="showMobile" Font-Names="Verdana,Arial" Font-Size="8pt" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="Normal" nowrap>
                                                Mobile Tab Name:
                                            </td>
                                            <td colspan="3">
                                                <asp:Textbox id="mobileTabName" width="300" cssclass="NormalTextBox" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <hr noshade size="1">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="Normal">
                                                Add Module:
                                            </td>
                                            <td class="Normal">
                                                Module Type
                                            </td>
                                            <td colspan="2">
                                                <asp:DropDownList id="moduleType" DataValueField="ModuleDefID" DataTextField="FriendlyName" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td class="Normal">
                                                Module Name:
                                            </td>
                                            <td colspan="2">
                                                <asp:Textbox id="moduleTitle" EnableViewState="false" Text="New Module Name" cssclass="NormalTextBox" width="250" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td colspan="3">
                                                <asp:LinkButton class="CommandButton" Text='<img src="../images/dn.gif" border=0> Add to "Organize Modules" Below' runat="server" id="AddModuleBtn" />
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
                                        <tr valign="top">
                                            <td class="Normal">
                                                Organize Modules:
                                            </td>
                                            <td width="120">
                                                <table border="0" cellspacing="0" cellpadding="2" width="100%">
                                                    <tr>
                                                        <td class="NormalBold">
                                                            &nbsp;Left Mini Pane
                                                        </td>
                                                    </tr>
                                                    <tr valign="top">
                                                        <td>
                                                            <table border="0" cellspacing="2" cellpadding="0">
                                                                <tr valign="top">
                                                                    <td rowspan="2">
                                                                        <asp:ListBox id="leftPane" DataSource="<%# leftList %>" DataTextField="ModuleTitle" DataValueField="ModuleId" width="110" rows="7" runat="server" />
                                                                    </td>
                                                                    <td valign="top" nowrap>
                                                                        <asp:ImageButton ImageUrl="~/images/up.gif" CommandName="up" CommandArgument="leftPane" AlternateText="Move selected module up in list" runat="server" id="LeftUpBtn" />
                                                                        <br>
                                                                        <asp:ImageButton ImageUrl="~/images/rt.gif" CommandName="right" sourcepane="leftPane" targetpane="contentPane" AlternateText="Move selected module to the content pane" runat="server" id="LeftRightBtn" />
                                                                        <br>
                                                                        <asp:ImageButton ImageUrl="~/images/dn.gif" CommandName="down" CommandArgument="leftPane" AlternateText="Move selected module down in list" runat="server" id="LeftDownBtn" />
                                                                        &nbsp;&nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="bottom" nowrap>
                                                                        <asp:ImageButton ImageUrl="~/images/edit.gif" CommandName="edit" CommandArgument="leftPane" AlternateText="Edit this item" runat="server" id="LeftEditBtn" />
                                                                        <br>
                                                                        <asp:ImageButton ImageUrl="~/images/delete.gif" CommandName="delete" CommandArgument="leftPane" AlternateText="Delete this item" runat="server" id="LeftDeleteBtn" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td width="*">
                                                <table border="0" cellspacing="0" cellpadding="2" width="100%">
                                                    <tr>
                                                        <td class="NormalBold">
                                                            &nbsp;Content Pane
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="middle">
                                                            <table border="0" cellspacing="2" cellpadding="0">
                                                                <tr valign="top">
                                                                    <td rowspan="2">
                                                                        <asp:ListBox id="contentPane" DataSource="<%# contentList %>" DataTextField="ModuleTitle" DataValueField="ModuleId" width="170" rows="7" runat="server" />
                                                                    </td>
                                                                    <td valign="top" nowrap>
                                                                        <asp:ImageButton ImageUrl="~/images/up.gif" CommandName="up" CommandArgument="contentPane" AlternateText="Move selected module up in list" runat="server" id="ContentUpBtn" />
                                                                        <br>
                                                                        <asp:ImageButton ImageUrl="~/images/lt.gif" sourcepane="contentPane" targetpane="leftPane" AlternateText="Move selected module to the left pane" runat="server" id="ContentLeftBtn" />
                                                                        <br>
                                                                        <asp:ImageButton ImageUrl="~/images/rt.gif" sourcepane="contentPane" targetpane="rightPane" AlternateText="Move selected module to the right pane" runat="server" id="ContentRightBtn" />
                                                                        <br>
                                                                        <asp:ImageButton ImageUrl="~/images/dn.gif" CommandName="down" CommandArgument="contentPane" AlternateText="Move selected module down in list" runat="server" id="ContentDownBtn" />
                                                                        &nbsp;&nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="bottom" nowrap>
                                                                        <asp:ImageButton ImageUrl="~/images/edit.gif" CommandName="edit" CommandArgument="contentPane" AlternateText="Edit this item" runat="server" id="ContentEditBtn" />
                                                                        <br>
                                                                        <asp:ImageButton ImageUrl="~/images/delete.gif" CommandName="delete" CommandArgument="contentPane" AlternateText="Delete this item" runat="server" id="ContentDeleteBtn" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td width="120">
                                                <table border="0" cellspacing="0" cellpadding="2" width="100%">
                                                    <tr>
                                                        <td class="NormalBold">
                                                            &nbsp;Right Mini Pane
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <table border="0" cellspacing="2" cellpadding="0">
                                                                <tr valign="top">
                                                                    <td rowspan="2">
                                                                        <asp:ListBox id="rightPane" DataSource="<%# rightList %>" DataTextField="ModuleTitle" DataValueField="ModuleId" width="110" rows="7" runat="server" />
                                                                    </td>
                                                                    <td valign="top" nowrap>
                                                                        <asp:ImageButton ImageUrl="~/images/up.gif" CommandName="up" CommandArgument="rightPane" AlternateText="Move selected module up in list" runat="server" id="RightUpBtn" />
                                                                        <br>
                                                                        <asp:ImageButton ImageUrl="~/images/lt.gif" sourcepane="rightPane" targetpane="contentPane" AlternateText="Move selected module to the left pane" runat="server" id="RightLeftBtn" />
                                                                        <br>
                                                                        <asp:ImageButton ImageUrl="~/images/dn.gif" CommandName="down" CommandArgument="rightPane" AlternateText="Move selected module down in list" runat="server" id="RightDownBtn" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="bottom" nowrap>
                                                                        <asp:ImageButton ImageUrl="~/images/edit.gif" CommandName="edit" CommandArgument="rightPane" AlternateText="Edit this item" runat="server" id="RightEditBtn" />
                                                                        <br>
                                                                        <asp:ImageButton ImageUrl="~/images/delete.gif" CommandName="delete" CommandArgument="rightPane" AlternateText="Delete this item" runat="server" id="RightDeleteBtn" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <hr noshade size="1">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:LinkButton id="applyBtn" class="CommandButton" Text="Apply Changes" runat="server" />
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
