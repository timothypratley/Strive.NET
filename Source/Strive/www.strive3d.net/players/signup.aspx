<%@ Page language="c#" Codebehind="signup.aspx.cs" AutoEventWireup="false" Inherits="www.strive3d.net.players.signup" %>
<%@ Register TagPrefix="Controls" TagName="Header" Src="~/players/Controls/Header.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Footer" Src="~/players/Controls/Footer.ascx" %>
<Controls:Header runat="Server" title="Create a New Account" ID="Header1"/>
<asp:Panel id=signupform runat="server">

    <form id="signup" method="post" runat="server">

		<table>
			<tr>
				<td><span class="Label">E-mail Address</span></td>
				<td><asp:TextBox CssClass="Input" ID="PlayerEmail" runat="server" />&nbsp;<asp:RequiredFieldValidator id=RequiredFieldValidator1 runat="server" ErrorMessage="You must enter a valid e-mail address." ControlToValidate="PlayerEmail" CssClass="InputValidation" EnableClientScript="False"></asp:RequiredFieldValidator><asp:RegularExpressionValidator id=RegularExpressionValidator1 runat="server" ErrorMessage="You must enter a valid e-mail address." ControlToValidate="PlayerEmail" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" EnableClientScript="False"></asp:RegularExpressionValidator></td>
			</tr>
			<tr>
				<td><span class="Label">Password</span></td>
				<td><asp:TextBox CssClass="Input" ID="PlayerPassword" TextMode="Password" runat="server" />&nbsp;<asp:RequiredFieldValidator id=RequiredFieldValidator2 runat="server" ErrorMessage="You must specify a password for your new account." CssClass="InputValidation" ControlToValidate="PlayerPassword" EnableClientScript="False"></asp:RequiredFieldValidator></td>
			</tr>
			<tr>
				<td colspan="2"><asp:Button CssClass="DefaultButton" Text="Create Your Account" Runat="server" id=Button1 /> </td>
			</tr>		
		
		</table>
     </form>		
</asp:Panel>  
<asp:Panel ID="signupsuccess" Runat="server">
	<p align="left">Thanks for signing up.  You will receive an e-mail with instructions to activate your account.</p>
	<p>
		<a href="./default.aspx">Return to Player Pages</a>
	</p>

</asp:Panel>  

<Controls:Footer runat="Server" ID="Footer1"/>