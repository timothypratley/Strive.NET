<%@ Control language="c#" Inherits="www.strive3d.net.Signin" CodeBehind="Signin.ascx.cs" AutoEventWireup="false" %>
<%--

   The SignIn User Control enables clients to authenticate themselves using 
   the ASP.NET Forms based authentication system.

   When a client enters their username/password within the appropriate
   textboxes and clicks the "Login" button, the LoginBtn_Click event
   handler executes on the server and attempts to validate their
   credentials against a SQL database.

   If the password check succeeds, then the LoginBtn_Click event handler
   sets the customers username in an encrypted cookieID and redirects
   back to the portal home page.

   If the password check fails, then an appropriate error message
   is displayed.

--%>

<hr noshade size="1" width="98%">
<span class="SubSubHead" style="HEIGHT:20px">Account Login</span>
<br>
<span class="Normal">Email:</span>
<br>
<asp:TextBox id="email" columns="9" width="130" cssclass="NormalTextBox" runat="server" />
<br>
<span class="Normal">Password:</span>
<br>
<asp:TextBox id="password" columns="9" width="130" textmode="password" cssclass="NormalTextBox" runat="server" />
<br>
<asp:checkbox id="RememberCheckbox" class="Normal" Text="Remember Login" runat="server" />
<table width="100%" cellspacing="0" cellpadding="4" border="0">
    <tr>
        <td>
            <asp:ImageButton id="SigninBtn" ImageUrl="~/images/signin.gif" runat="server" />
            <br>
            <a href="Admin/Register.aspx"><img src="images/register.gif" border="0"></a>
            <asp:label id="Message" class="NormalRed" runat="server" />
        </td>
    </tr>
</table>
<br>
