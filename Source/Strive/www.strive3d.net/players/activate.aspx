<%@ Page language="c#" Codebehind="activate.aspx.cs" AutoEventWireup="false" Inherits="www.strive3d.net.players.activate" %>
<%@ Register TagPrefix="Controls" TagName="Footer" Src="~/players/Controls/Footer.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Header" Src="~/players/Controls/Header.ascx" %>
<Controls:Header runat="server" Title="Create a new account" id="Header1" />
	
	<asp:Panel ID="success" Runat="server">
<P align=left>Player <STRONG><%=this.playerName%></STRONG> successfully 
activated.</P>
	</asp:Panel>
	
	<asp:Panel ID="failure" Runat="server">
<P align=left>Player <STRONG>WAS NOT</STRONG> 
activatated.</P>
	</asp:Panel>	
<controls:footer runat="Server" />
