<%@ Page language="c#" CodeBehind="Register.aspx.cs" AutoEventWireup="false" Inherits="www.strive3d.net.Register" %>
<%@ Register TagPrefix="portal" TagName="Banner" Src="~/DesktopPortalBanner.ascx" %>

<%--

   The Register.aspx page is used to enable clients to register a new unique username
   and password with the portal system.  The page contains a single server event
   handler -- RegisterBtn_Click -- that executes in response to the page's Register
   Button being clicked.

   The Register.aspx page uses the UsersDB class to manage the actual account creation.
   Note that the Usernames and passwords are stored within a table in a SQL database.

--%>

<HTML>
  <HEAD>
        <link rel="stylesheet" href='/portal.css' type="text/css">
  </HEAD>
    <body leftmargin="0" bottommargin="0" rightmargin="0" topmargin="0" marginheight="0" marginwidth="0">
        <form runat="server">
            <table width="100%" cellspacing="0" cellpadding="0">
                <tr valign="top">
                    <td colspan="2">
                        <portal:Banner ShowTabs="false" runat="server" id="Banner1" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <br>
                        <table width="98%" cellspacing="0" cellpadding="4" border="0">
                            <tr>
                                <td width="150">
                                    &nbsp;
                                </td>
                                <td width="*">
                                    <table cellpadding="2" cellspacing="1" border="0">
                                        <tr>
                                            <td width="450">
                                                <table width="100%" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td>
                                                            <span class="Head">Create a New Account </span>
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
                                        <tr valign="top">
                                            <td class="Normal">
                                                Name:
                                                <br>
                                                <asp:TextBox size="25" id="Name" runat="server" />
                                                &nbsp;
                                                <asp:RequiredFieldValidator ControlToValidate="Name" ErrorMessage="'Name' must not be left blank." runat="server" id="RequiredFieldValidator1" />
                                                <p>
                                                    Email:
                                                    <br>
                                                    <asp:TextBox size="25" id="Email" runat="server" />
                                                    &nbsp;
                                                    <asp:RegularExpressionValidator ControlToValidate="Email" ValidationExpression="[\w\.-]+(\+[\w-]*)?@([\w-]+\.)+[\w-]+" Display="Dynamic" ErrorMessage="Must use a valid email address." runat="server" id="RegularExpressionValidator1" />
                                                    <asp:RequiredFieldValidator ControlToValidate="Email" ErrorMessage="'Email' must not be left blank." runat="server" id="RequiredFieldValidator2" />
                                                <p>
                                                    Password:
                                                    <br>
                                                    <asp:TextBox size="25" id="Password" TextMode="Password" runat="server" />
                                                    &nbsp;
                                                    <asp:RequiredFieldValidator ControlToValidate="Password" ErrorMessage="'Password' must not be left blank." runat="server" id="RequiredFieldValidator3" />
                                                <p>
                                                    Confirm Password:
                                                    <br>
                                                    <asp:TextBox size="25" id="ConfirmPassword" TextMode="Password" runat="server" />
                                                    &nbsp;
                                                    <asp:RequiredFieldValidator ControlToValidate="ConfirmPassword" Display="Dynamic" ErrorMessage="'Confirm' must not be left blank." runat="server" id="RequiredFieldValidator4" />
                                                    <asp:CompareValidator ControlToValidate="ConfirmPassword" ControlToCompare="Password" ErrorMessage="Password fields do not match." runat="server" id="CompareValidator1" />
                                                <p>
                                                    <asp:LinkButton class="CommandButton" Text="Register and Sign In Now" runat="server" id="RegisterBtn" />
                                                    <br>
                                                    <br>
                                                <p>
                                                    <asp:Label id="Message" CssClass="NormalRed" runat="server" />
                                                </p>
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
