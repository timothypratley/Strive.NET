<%@ Register TagPrefix="Controls" TagName="Footer" Src="~/players/Controls/Footer.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Header" Src="~/players/Controls/Header.ascx" %>
<%@ Import Namespace="System.Data" %>
<%@ Page language="c#" Codebehind="editterrainpiece.aspx.cs" AutoEventWireup="false" Inherits="www.strive3d.net.players.builders.terrain.editterrainpiece" %>
<Controls:Header runat="Server" title="Terrain Builder - Edit Terrain Square" ID="Header1"/>	

<form runat="server">
<input type="hidden" runat="server" id="referer" >
<table>
	<tr>	
		<td><span class="Label">Terrain</span></td>
		<td><asp:DropDownList id=EnumTerrainID runat="server" DataValueField="EnumTerrainTypeID" DataTextField="EnumTerrainTypeName"></asp:DropDownList></td>
	</tr>
	<tr>
		<td><span class="Label">Texture</span></td>
		<td><asp:DropDownList id=ModelID runat="server" AutoPostBack="True" DataValueField="ModelID" DataTextField="ModelName"></asp:DropDownList></td>
	</tr>
	<tr>
		<td colspan="2"><img runat="server" id="textureshower" height="100" width="100"></td>
	</tr>
	<tr>
		<td><span class="Label">Altitude</span></td>
		<td><asp:TextBox id=Altitude runat="server" Width="30px"></asp:TextBox></td>
	</tr>
	<tr>
		<td colspan="2"><asp:Button id=Save runat="server" Text="Save"></asp:Button><asp:Button id=Cancel runat="server" Text="Cancel"></asp:Button></td>
	</tr>
</table>

</form>

<Controls:Footer runat="server" id=Footer1 />
