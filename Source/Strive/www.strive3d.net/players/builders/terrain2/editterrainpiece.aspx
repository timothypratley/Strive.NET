<%@ Register TagPrefix="Controls" TagName="Footer" Src="~/players/Controls/Footer.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Header" Src="~/players/Controls/Header.ascx" %>
<%@ Import Namespace="System.Data" %>
<%@ Page language="c#" Codebehind="editterrainpiece.aspx.cs" AutoEventWireup="false" Inherits="www.strive3d.net.players.builders.terrain2.editterrainpiece" %>
<form runat="server">
	<input type="hidden" runat="server" id="referer">
	<table>
		<tr>
			<td><span class="Label">Terrain</span></td>
			<td><asp:DropDownList id="EnumTerrainID" runat="server" DataValueField="EnumTerrainTypeID" DataTextField="EnumTerrainTypeName"></asp:DropDownList></td>
		</tr>
		<tr>
			<td><span class="Label">Texture</span></td>
			<td><asp:DropDownList id="ResourceID" runat="server" AutoPostBack="True" DataValueField="ResourceID" DataTextField="ResourceDisplayName"></asp:DropDownList></td>
		</tr>
		<tr>
			<td colspan="2"><img runat="server" id="textureshower" height="100" width="100"></td>
		</tr>
		<tr>
			<td><span class="Label">Altitude</span></td>
			<td><asp:TextBox id="Altitude" runat="server" Width="30px"></asp:TextBox></td>
		</tr>
		<tr>
			<td colspan="2"><asp:Button id="Save" runat="server" Text="Save"></asp:Button><asp:Button id="Cancel" runat="server" Text="Cancel"></asp:Button></td>
		</tr>
		<tr>
			<td colspan="2">
				<table>
					<tr>
						<th>
							Junk</th>
						<td><a href="./editterrainpieceobjectinstance.aspx?TemplateName=ItemJunk&amp;Y=<%=Altitude.Text%>&amp;StartX=<%=TerrainX%>&amp;StartZ=<%=TerrainZ%>" target="ObjectInstanceEditor">[Add]</a></td>
					</tr>
					<tr>
						<td colSpan="2" valign="top"><asp:Repeater ID="TemplateItemJunkList" runat="server"><ItemTemplate><a target="ObjectInstanceEditor" href='./editterrainpieceobjectinstance.aspx?TemplateName=ItemJunk&amp;ObjectInstanceID=<%#DataBinder.Eval(Container.DataItem, "ObjectInstanceID")%>&amp;Y=<%=Altitude.Text%>&amp;StartX=<%=TerrainX%>&amp;StartZ=<%=TerrainZ%>'><%#DataBinder.Eval(Container.DataItem, "TemplateObjectName")%></a><br />
								</ItemTemplate>
							</asp:Repeater></td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
</form>
