<%@ Page language="c#" Codebehind="default.aspx.cs" AutoEventWireup="false" Inherits="www.strive3d.net.players.builders._default" %>
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="Controls" TagName="Footer" Src="~/players/Controls/Footer.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Header" Src="~/players/Controls/Header.ascx" %>

<Controls:Header runat="Server" title="Home" ID="Header1"/>	


  <ul>
	<li><strong>Resource Management</strong>
	<ul>
		<li><a href="./resources/default.aspx">Create, Delete, View Resources</a>
			<ul>
				<li>Follow this link to manage resources</li>
			</ul>
		</li>
		<li><a href="./terrain2">Terrain Builder</a>
			<ul>
				<li>Follow this link to build the terrain of the world</li>
			</ul>
		</li>			
	</ul>
	</li>
	<li><strong>Strive3d.net Home</strong></li>
	<ul>
		<li><a href="~" runat="server">www.strive3d.net</a></li>
			<ul>
				<li>Visit the Strive3d.net home</li>
			</ul>
	</ul>
  </ul>

<Controls:Footer runat="server" />