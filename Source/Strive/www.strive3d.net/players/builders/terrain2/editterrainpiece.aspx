<%@ Register TagPrefix="Controls" TagName="Footer" Src="~/players/Controls/Footer.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Header" Src="~/players/Controls/Header.ascx" %>
<%@ Import Namespace="System.Data" %>
<%@ Page language="c#" Codebehind="editterrainpiece.aspx.cs" AutoEventWireup="false" Inherits="www.strive3d.net.players.builders.terrain.editterrainpiece" %>
<form runat="server">
	<input id="referer" type="hidden" runat="server">
	<table>
		<tr>
			<td height="16"><span class="Label">Terrain</span></td>
			<td height="16"><asp:dropdownlist id="TemplateObject" runat="server" DataTextField="TemplateObjectName" DataValueField="TemplateObjectID"
					AutoPostBack="True"></asp:dropdownlist></td>
		</tr>
		<tr>
			<td colSpan="2"><img id="textureshower" height="100" width="100" runat="server"></td>
		</tr>
		<tr>
			<td><span class="Label">Altitude</span></td>
			<td><asp:textbox id="Altitude" runat="server" Width="30px"></asp:textbox></td>
		</tr>
		<tr>
			<td colspan="2">
				<asp:button id="cancel" runat="server" text="close"></asp:button></td>
		</tr>
		<tr>
			<td colSpan="2">
				<table>
					<tr>
						<th>
							Junk</th>
						<td><A href="./editterrainpieceobjectinstance.aspx?TemplateName=ItemJunk&amp;Y=<%=Altitude.Text%>&amp;StartX=<%=TerrainX%>&amp;StartZ=<%=TerrainZ%>" target=ObjectInstanceEditor >[Add]</A></td>
						<td vAlign="top" colSpan="2"><asp:repeater id="TemplateItemJunkList" runat="server"><ItemTemplate><a target="ObjectInstanceEditor" href='./editterrainpieceobjectinstance.aspx?TemplateName=ItemJunk&amp;ObjectInstanceID=<%#DataBinder.Eval(Container.DataItem, "ObjectInstanceID")%>&amp;Y=<%=Altitude.Text%>&amp;StartX=<%=TerrainX%>&amp;StartZ=<%=TerrainZ%>'><%#DataBinder.Eval(Container.DataItem, "TemplateObjectName")%></a>
								</ItemTemplate>
							</asp:repeater></td>
					</tr>
					<tr>
						<th>
							Wieldable</th>
						<td><A href="./editterrainpieceobjectinstance.aspx?TemplateName=ItemWieldable&amp;Y=<%=Altitude.Text%>&amp;StartX=<%=TerrainX%>&amp;StartZ=<%=TerrainZ%>" target=ObjectInstanceEditor >[Add]</A></td>
						<td vAlign="top" colSpan="2"><asp:repeater id="TemplateItemWieldableList" runat="server"><ItemTemplate><a target="ObjectInstanceEditor" href='./editterrainpieceobjectinstance.aspx?TemplateName=ItemWieldable&amp;ObjectInstanceID=<%#DataBinder.Eval(Container.DataItem, "ObjectInstanceID")%>&amp;Y=<%=Altitude.Text%>&amp;StartX=<%=TerrainX%>&amp;StartZ=<%=TerrainZ%>'><%#DataBinder.Eval(Container.DataItem, "TemplateObjectName")%></a><br />
								</ItemTemplate>
							</asp:repeater></td>
					</tr>
					<tr>
						<th>
							Readable</th>
						<td><A href="./editterrainpieceobjectinstance.aspx?TemplateName=ItemReadable&amp;Y=<%=Altitude.Text%>&amp;StartX=<%=TerrainX%>&amp;StartZ=<%=TerrainZ%>" target=ObjectInstanceEditor >[Add]</A></td>
						<td vAlign="top" colSpan="2"><asp:repeater id="TemplateItemReadableList" runat="server"><ItemTemplate><a target="ObjectInstanceEditor" href='./editterrainpieceobjectinstance.aspx?TemplateName=ItemReadable&amp;ObjectInstanceID=<%#DataBinder.Eval(Container.DataItem, "ObjectInstanceID")%>&amp;Y=<%=Altitude.Text%>&amp;StartX=<%=TerrainX%>&amp;StartZ=<%=TerrainZ%>'><%#DataBinder.Eval(Container.DataItem, "TemplateObjectName")%></a><br />
								</ItemTemplate>
							</asp:repeater></td>
					</tr>
					<tr>
						<th>
							Equipable</th>
						<td><A href="./editterrainpieceobjectinstance.aspx?TemplateName=ItemEquipable&amp;Y=<%=Altitude.Text%>&amp;StartX=<%=TerrainX%>&amp;StartZ=<%=TerrainZ%>" target=ObjectInstanceEditor >[Add]</A></td>
						<td vAlign="top" colSpan="2"><asp:repeater id="TemplateItemEquipableList" runat="server"><ItemTemplate><a target="ObjectInstanceEditor" href='./editterrainpieceobjectinstance.aspx?TemplateName=ItemEquipable&amp;ObjectInstanceID=<%#DataBinder.Eval(Container.DataItem, "ObjectInstanceID")%>&amp;Y=<%=Altitude.Text%>&amp;StartX=<%=TerrainX%>&amp;StartZ=<%=TerrainZ%>'><%#DataBinder.Eval(Container.DataItem, "TemplateObjectName")%></a><br />
								</ItemTemplate>
							</asp:repeater></td>
					</tr>
					<tr>
						<th>
							Quaffable</th>
						<td><A href="./editterrainpieceobjectinstance.aspx?TemplateName=ItemQuaffable&amp;Y=<%=Altitude.Text%>&amp;StartX=<%=TerrainX%>&amp;StartZ=<%=TerrainZ%>" target=ObjectInstanceEditor >[Add]</A></td>
						<td vAlign="top" colSpan="2"><asp:repeater id="TemplateItemQuaffableList" runat="server"><ItemTemplate><a target="ObjectInstanceEditor" href='./editterrainpieceobjectinstance.aspx?TemplateName=ItemQuaffable&amp;ObjectInstanceID=<%#DataBinder.Eval(Container.DataItem, "ObjectInstanceID")%>&amp;Y=<%=Altitude.Text%>&amp;StartX=<%=TerrainX%>&amp;StartZ=<%=TerrainZ%>'><%#DataBinder.Eval(Container.DataItem, "TemplateObjectName")%></a><br />
								</ItemTemplate>
							</asp:repeater></td>
					</tr>
					<tr>
						<th>
							Mobile</th>
						<td><A href="./editterrainpieceobjectinstance.aspx?TemplateName=Mobile&amp;Y=<%=Altitude.Text%>&amp;StartX=<%=TerrainX%>&amp;StartZ=<%=TerrainZ%>" target=ObjectInstanceEditor >[Add]</A></td>
						<td vAlign="top" colSpan="2"><asp:repeater id="TemplateMobileList" runat="server"><ItemTemplate><a target="ObjectInstanceEditor" href='./editterrainpieceobjectinstance.aspx?TemplateName=Mobile&amp;ObjectInstanceID=<%#DataBinder.Eval(Container.DataItem, "ObjectInstanceID")%>&amp;Y=<%=Altitude.Text%>&amp;StartX=<%=TerrainX%>&amp;StartZ=<%=TerrainZ%>'><%#DataBinder.Eval(Container.DataItem, "TemplateObjectName")%></a><br />
								</ItemTemplate>
							</asp:repeater></td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
</form>
