<%@ Page Language="c#" CodeBehind="EditAnnouncements.aspx.cs" AutoEventWireup="false" Inherits="www.strive3d.net.EditAnnouncements" %>
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
                                <td width="*">
                                    <table width="520" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td align="left" class="Head">
                                                Announcement Details
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <hr noshade size="1">
                                            </td>
                                        </tr>
                                    </table>
                                    <table width="750" cellspacing="0" cellpadding="0">
                                        <tr valign="top">
                                            <td width="100" class="SubHead">
                                                Title:
                                            </td>
                                            <td rowspan="5">
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox id="TitleField" cssclass="NormalTextBox" width="390" Columns="30" maxlength="100" runat="server" />
                                            </td>
                                            <td width="25" rowspan="5">
                                                &nbsp;
                                            </td>
                                            <td class="Normal" width="250">
                                                <asp:RequiredFieldValidator id="Req1" Display="Static" ErrorMessage="You Must Enter a Valid Title" ControlToValidate="TitleField" runat="server" />
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td class="SubHead">
                                                Read More Link:
                                            </td>
                                            <td>
                                                <asp:TextBox id="MoreLinkField" cssclass="NormalTextBox" width="390" Columns="30" maxlength="100" runat="server" />
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td class="SubHead" nowrap>
                                                Read More (Mobile):
                                            </td>
                                            <td>
                                                <asp:TextBox id="MobileMoreField" cssclass="NormalTextBox" width="390" Columns="30" maxlength="100" runat="server" />
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td class="SubHead">
                                                Description:
                                            </td>
                                            <td>
                                                <asp:TextBox id="DescriptionField" width="390" TextMode="Multiline" Columns="44" Rows="6" runat="server" />
                                            </td>
                                            <td class="Normal">
                                                <asp:RequiredFieldValidator id="Req2" Display="Static" ErrorMessage="You Must Enter a Valid Description" ControlToValidate="DescriptionField" runat="server" />
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td class="SubHead">
                                                Expires:
                                            </td>
                                            <td>
                                                <asp:TextBox id="ExpireField" Text="12/31/2001" cssclass="NormalTextBox" width="100" Columns="8" runat="server" />
                                            </td>
                                            <td class="Normal">
                                                <asp:RequiredFieldValidator Display="Static" id="RequiredExpireDate" runat="server" ErrorMessage="You Must Enter a Valid Expiration Date" ControlToValidate="ExpireField" />
                                                <asp:CompareValidator Display="Static" id="VerifyExpireDate" runat="server" Operator="DataTypeCheck" ControlToValidate="ExpireField" Type="Date" ErrorMessage="You Must Enter a Valid Expiration Date" />
                                            </td>
                                        </tr>
                                    </table>
                                    <p>
                                        <asp:LinkButton id="updateButton" Text="Update" runat="server" CssClass="CommandButton" BorderStyle="none" />
                                        &nbsp;
                                        <asp:LinkButton id="cancelButton" Text="Cancel" CausesValidation="False" runat="server" CssClass="CommandButton" BorderStyle="none" />
                                        &nbsp;
                                        <asp:LinkButton id="deleteButton" Text="Delete this item" CausesValidation="False" runat="server" CssClass="CommandButton" BorderStyle="none" />
                                        <hr noshade size="1" width="520">
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
