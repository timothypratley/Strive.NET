<%@ Page language="c#" CodeBehind="DiscussDetails.aspx.cs" AutoEventWireup="false" Inherits="www.strive3d.net.DiscussDetails" %>
<%@ Register TagPrefix="portal" TagName="Banner" Src="~/DesktopPortalBanner.ascx" %>

<HTML>
    <HEAD>
        <link href='portal.css' type="text/css" rel="stylesheet">
    </HEAD>
    <body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" marginwidth="0" marginheight="0">
        <form name="form1" runat="server">
            <table cellSpacing="0" cellPadding="0" width="100%" border="0">
                <tr vAlign="top">
                    <td colSpan="2">
                        <portal:banner id="Banner1" runat="server" ShowTabs="false">
                        </portal:banner>
                    </td>
                </tr>
                <tr vAlign="top">
                    <td width="10%">
                        &nbsp;
                    </td>
                    <td>
                        <br>
                        <table cellSpacing="0" cellPadding="0" width="600">
                            <tr>
                                <td align="left">
                                    <span class="Head">Message Detail</span>
                                </td>
                                <td align="right">
                                    <asp:panel id="ButtonPanel" runat="server"><A class="CommandButton" id="prevItem" title="Previous Message" runat="server">
                                            <IMG src='<%=www.strive3d.net.Utils.ApplicationPath + "~/images/rew.gif"  %>' border="0"></A>&nbsp; <A class="CommandButton" id="nextItem" title="Next Message" runat="server"><IMG src='<%=www.strive3d.net.Utils.ApplicationPath + "~/images/fwd.gif"  %>' border="0"></A>&nbsp; 
                                            <asp:LinkButton id="ReplyBtn" runat="server" EnableViewState="false" Cssclass="CommandButton" Text="Reply to this Message"></asp:LinkButton>
                                    </asp:panel>
                                </td>
                            </tr>
                            <tr>
                                <td colSpan="2">
                                    <hr noShade SIZE="1">
                                </td>
                            </tr>
                        </table>
                        <asp:panel id="EditPanel" runat="server" Visible="false">
                            <TABLE cellSpacing="0" cellPadding="4" width="600" border="0">
                                <TR vAlign="top">
                                    <TD class="SubHead" width="150">
                                        Title:
                                    </TD>
                                    <TD rowSpan="4">
                                        &nbsp;
                                    </TD>
                                    <TD width="*">
                                        <asp:TextBox id="TitleField" runat="server" maxlength="100" columns="40" width="500" cssclass="NormalTextBox"></asp:TextBox>
                                    </TD>
                                </TR>
                                <TR vAlign="top">
                                    <TD class="SubHead">
                                        Body:
                                    </TD>
                                    <TD width="*">
                                        <asp:TextBox id="BodyField" runat="server" columns="59" width="500" Rows="15" TextMode="Multiline"></asp:TextBox>
                                    </TD>
                                </TR>
                                <TR vAlign="top">
                                    <TD>
                                        &nbsp;
                                    </TD>
                                    <TD>
                                        <asp:LinkButton class="CommandButton" id="updateButton" runat="server" Text="Submit"></asp:LinkButton>
                                        &nbsp;
                                        <asp:LinkButton class="CommandButton" id="cancelButton" runat="server" Text="Cancel" CausesValidation="False"></asp:LinkButton>
                                        &nbsp;
                                    </TD>
                                </TR>
                                <TR vAlign="top">
                                    <TD class="SubHead">
                                        Original Message:
                                    </TD>
                                    <TD>
                                        &nbsp;
                                    </TD>
                                </TR>
                            </TABLE>
                        </asp:panel>
                        <table cellSpacing="0" cellPadding="4" width="600" border="0">
                            <tr vAlign="top">
                                <td class="Message" align="left">
                                    <b>Subject: </b>
                                    <asp:label id="Title" runat="server"></asp:label>
                                    <br>
                                    <b>Author: </b>
                                    <asp:label id="CreatedByUser" runat="server"></asp:label>
                                    <br>
                                    <b>Date: </b>
                                    <asp:label id="CreatedDate" runat="server"></asp:label>
                                    <br>
                                    <br>
                                    <asp:label id="Body" runat="server"></asp:label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </form>
    </body>
</HTML>
