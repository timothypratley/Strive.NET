<%@ Register TagPrefix="Controls" TagName="Footer" Src="~/players/Controls/Footer.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Header" Src="~/players/Controls/Header.ascx" %>
<%@ Page language="c#" Codebehind="pingemail.aspx.cs" AutoEventWireup="false" Inherits="www.strive3d.net.players.pingemail" %>
<Controls:Header runat="Server" title="Create a New Account" ID="Header1" />
<asp:Panel id="pingme" runat="server">
	<FORM runat="server">
		<TABLE>
			<TR>
				<TD><SPAN class="Label">Your E-mail Address</SPAN></TD>
				<TD>
					<asp:TextBox id="EmailAddress" runat="server" CssClass="Input"></asp:TextBox></TD>
			</TR>
		</TABLE>
	</FORM>
</asp:Panel>
<asp:Panel id="pinged" runat="server"></asp:Panel>
<Controls:Footer runat="server" id="Footer1" />
