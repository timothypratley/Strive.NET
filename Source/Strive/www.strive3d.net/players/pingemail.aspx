<%@ Page language="c#" Codebehind="pingemail.aspx.cs" AutoEventWireup="false" Inherits="www.strive3d.net.players.pingemail" %>
<%@ Register TagPrefix="Controls" TagName="Header" Src="~/players/Controls/Header.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Footer" Src="~/players/Controls/Footer.ascx" %>
<Controls:Header runat="Server" title="Create a New Account" ID="Header1"/>

<asp:Panel id="pingme" runat="server">
<form runat="server">
<table>
	<tr>
		<td><span class="Label">Your E-mail Address</span></td>
		<td><asp:TextBox CssClass="Input" ID="EmailAddress" runat="server" /></td>
	</tr>

</table>
</form>
</asp:Panel>

<asp:Panel id="pinged" runat="server">


</asp:Panel>

<Controls:Footer runat="server" />