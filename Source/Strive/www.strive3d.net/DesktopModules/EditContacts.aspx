<%@ Page Language="c#" CodeBehind="EditContacts.aspx.cs" AutoEventWireup="false" Inherits="www.strive3d.net.EditContacts" %>
<%@ Register TagPrefix="portal" TagName="Banner" Src="~/DesktopPortalBanner.ascx" %>

<HTML>
    <HEAD>
        <link rel="stylesheet" href='portal.css' type="text/css">
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
                                <td>
                                    <table width="500" cellspacing="0" cellpadding="0" border="0">
                                        <tr>
                                            <td align="left" class="Head">
                                                Contact Details
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <hr noshade size="1">
                                            </td>
                                        </tr>
                                    </table>
                                    <table width="750" cellspacing="0" cellpadding="0" border="0">
                                        <tr valign="top">
                                            <td width="100" class="SubHead">
                                                Name:
                                            </td>
                                            <td rowspan="5">
                                                &nbsp;
                                            </td>
                                            <td align="left">
                                                <asp:TextBox id="NameField" cssclass="NormalTextBox" width="390" Columns="30" maxlength="50" runat="server" />
                                            </td>
                                            <td width="25" rowspan="5">
                                                &nbsp;
                                            </td>
                                            <td class="Normal" width="250">
                                                <asp:RequiredFieldValidator Display="Static" runat="server" ErrorMessage="You Must Enter a Valid Name" ControlToValidate="NameField" id="RequiredFieldValidator1" />
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td class="SubHead">
                                                Role:
                                            </td>
                                            <td>
                                                <asp:TextBox id="RoleField" cssclass="NormalTextBox" width="390" Columns="30" maxlength="100" runat="server" />
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td class="SubHead">
                                                Email:
                                            </td>
                                            <td>
                                                <asp:TextBox id="EmailField" cssclass="NormalTextBox" width="390" Columns="30" maxlength="100" runat="server" />
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td class="SubHead">
                                                Contact1:
                                            </td>
                                            <td>
                                                <asp:TextBox id="Contact1Field" cssclass="NormalTextBox" width="390" Columns="30" maxlength="250" runat="server" />
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td class="SubHead">
                                                Contact2:
                                            </td>
                                            <td>
                                                <asp:TextBox id="Contact2Field" cssclass="NormalTextBox" width="390" Columns="30" maxlength="250" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                    <p>
                                        <asp:LinkButton id="updateButton" Text="Update" runat="server" class="CommandButton" BorderStyle="none" />
                                        &nbsp;
                                        <asp:LinkButton id="cancelButton" Text="Cancel" CausesValidation="False" runat="server" class="CommandButton" BorderStyle="none" />
                                        &nbsp;
                                        <asp:LinkButton id="deleteButton" Text="Delete this item" CausesValidation="False" runat="server" class="CommandButton" BorderStyle="none" />
                                        <hr noshade size="1" width="500">
                                        <span class="Normal">Created by
                                            <asp:label id="CreatedBy" runat="server" />
                                            on
                                            <asp:label id="CreatedDate" runat="server" />
                                            <br>
                                        </span>
                                    <P>
                                    </P>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </form>
    </body>
</HTML>
