<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="Controls" TagName="Header" Src="~/players/Controls/Header.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Footer" Src="~/players/Controls/Footer.ascx" %>
<%@ Page language="c#" Codebehind="editterrainpieceobjectinstance.aspx.cs" AutoEventWireup="false" Inherits="www.strive3d.net.players.builders.terrain.editterrainpieceobjectinstance" %>
<form runat="server">
	<table>
		<tr>
			<td><span class="label">Template</span></td>
			<td><asp:dropdownlist id="TemplateList" runat="Server" DataValueField="TemplateObjectID" DataTextField="TemplateObjectName"
					cssclass="input"></asp:dropdownlist></td>
		</tr>
		<tr>
			<td><span class="label">X</span></td>
			<td><asp:textbox id="X" runat="server" size="2" CssClass="Input"></asp:textbox></td>
		</tr>
		<tr>
			<td><span class="label">Z</span></td>
			<td><asp:textbox id="Z" runat="server" size="2" CssClass="Input"></asp:textbox></td>
		</tr>
		<tr>
			<td><span class="label">Roptation</span></td>
			<td><asp:DropDownList ID="RotationY" runat="Server" cssclass="Input">
					<asp:ListItem Value="0">0</asp:ListItem>
					<asp:ListItem Value="90">90</asp:ListItem>
					<asp:ListItem Value="180">180</asp:ListItem>
					<asp:ListItem Value="270">270</asp:ListItem>
				</asp:DropDownList></td>
		</tr>
		<tr>
			<td colSpan="2"></td>
		</tr>
		<tr>
			<td colSpan="2"><asp:button id="Save" runat="server" Text="Save"></asp:button>
				<asp:Button ID="Cancel" runat="Server" Text="Cancel" />
				<asp:Button ID="Delete" runat="server" Text="Delete" /></td>
		</tr>
	</table>
</form>
