<%@ Page language="c#" CodeBehind="ModuleDefinitions.aspx.cs" AutoEventWireup="false" Inherits="www.strive3d.net.ModuleDefinitions" %>
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
                        <portal:Banner id="SiteHeader" runat="server" />
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
                                    <table width="500" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td align="left" class="Head">
                                                Module Type Definition
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <hr noshade size="1">
                                            </td>
                                        </tr>
                                    </table>
                                    <table width="750" cellspacing="0" cellpadding="0" border="0">
                                        <tr>
                                            <td width="100" class="SubHead">
                                                Friendly Name:
                                            </td>
                                            <td rowspan="5">
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox id="FriendlyName" cssclass="NormalTextBox" width="390" Columns="30" maxlength="150" runat="server" />
                                            </td>
                                            <td width="25" rowspan="5">
                                                &nbsp;
                                            </td>
                                            <td class="Normal" width="250">
                                                <asp:RequiredFieldValidator id="Req1" Display="Static" ErrorMessage="Enter a Module NAme" ControlToValidate="FriendlyName" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="SubHead" nowrap>
                                                Desktop Source:
                                            </td>
                                            <td>
                                                <asp:TextBox id="DesktopSrc" cssclass="NormalTextBox" width="390" Columns="30" maxlength="150" runat="server" />
                                            </td>
                                            <td class="Normal">
                                                <asp:RequiredFieldValidator id="Req2" Display="Static" ErrorMessage="You Must Enter Source Path for the Desktop Module" ControlToValidate="DesktopSrc" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="SubHead">
                                                Mobile Source:
                                            </td>
                                            <td>
                                                <asp:TextBox id="MobileSrc" cssclass="NormalTextBox" width="390" Columns="30" maxlength="150" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                    <p>
                                        <asp:LinkButton id="updateButton" Text="Update" runat="server" class="CommandButton" BorderStyle="none" />
                                        &nbsp;
                                        <asp:LinkButton id="cancelButton" Text="Cancel" CausesValidation="False" runat="server" class="CommandButton" BorderStyle="none" />
                                        &nbsp;
                                        <asp:LinkButton id="deleteButton" Text="Delete this module type" CausesValidation="False" runat="server" class="CommandButton" BorderStyle="none" />
                                    </p>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </form>
    </body>
</HTML>
