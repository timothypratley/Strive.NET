<%@ Register TagPrefix="portal" TagName="Banner" Src="~/DesktopPortalBanner.ascx" %>
<%@ Page Language="c#" CodeBehind="EditDocs.aspx.cs" AutoEventWireup="false" Inherits="www.strive3d.net.EditDocs" %>

<HTML>
  <HEAD>
        <link rel="stylesheet" href='portal.css' type="text/css">
  </HEAD>
    <body leftmargin="0" bottommargin="0" rightmargin="0" topmargin="0" marginheight="0" marginwidth="0">
        <form enctype="multipart/form-data" runat="server">
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
                                    <table width="500" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td align="left" class="Head">
                                                Document Details
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <hr noshade size="1">
                                            </td>
                                        </tr>
                                    </table>
                                    <table width="726" cellspacing="0" cellpadding="0" border="0">
                                        <tr valign="top">
                                            <td width="100" class="SubHead">
                                                Name:
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox id="NameField" cssclass="NormalTextBox" width="353" Columns="28" maxlength="150" runat="server" />
                                            </td>
                                            <td width="25" rowspan="6">
                                                &nbsp;
                                            </td>
                                            <td class="Normal" width="250">
                                                <asp:RequiredFieldValidator Display="Static" runat="server" ErrorMessage="You Must Enter a Valid Name" ControlToValidate="NameField" id="RequiredFieldValidator1" />
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td class="SubHead">
                                                Category:
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox id="CategoryField" cssclass="NormalTextBox" width="353" Columns="28" maxlength="50" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td colspan="2">
                                                <hr noshade size="1" width="100%">
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td width="100" class="SubHead">
                                                URL to Browse:
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:textbox id="PathField" cssclass="NormalTextBox" width="353" Columns="28" maxlength="250" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="SubHead">
                                                — or —
                                            </td>
                                            <td colspan="2">
                                                &nbsp;
                                                <br>
                                                <br>
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td nowrap class="SubHead">
                                                Upload to Web Server:&nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:CheckBox id="Upload" Cssclass="Normal" Text="Upload document to server" runat="server" />
                                                <br>
                                                <asp:CheckBox id="storeInDatabase" Cssclass="Normal" Text="Store in database (web farm support)" runat="server" />
                                                <br>
                                                <input type="file" id="FileUpload" width="300" style="WIDTH:353px;FONT-FAMILY:verdana" runat="server" NAME="FileUpload">
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
