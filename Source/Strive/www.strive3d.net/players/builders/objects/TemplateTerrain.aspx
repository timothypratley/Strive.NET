<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="Controls" TagName="Footer" Src="~/players/Controls/Footer.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Header" Src="~/players/Controls/Header.ascx" %>
<%@ Page language="c#" Codebehind="TemplateTerrain.aspx.cs" AutoEventWireup="false" Inherits="www.strive3d.net.players.builders.objects.TemplateTerrain" %>
<CONTROLS:HEADER id="Header1" title="Resources - List View" runat="Server"></CONTROLS:HEADER>
<form runat="server" ID="Form1">
	<table>
		<tr>
			<td><span class="label">Name</span></td>
			<td colSpan="3"><asp:textbox id="TemplateObjectName" runat="server" cssclass="input"></asp:textbox></td>
		</tr>
		<tr>
			<td><span class="label">Resource</span></td>
			<td><asp:dropdownlist id="ResourceID" runat="Server" DataValueField="ResourceID" DataTextField="ResourceName"></asp:dropdownlist></td>
			<td><input type="hidden" id="Height" runat="server" cssclass="input" size="2" value="0"></td>
		</tr>
		<tr>
			<td colspan="4"><hr>
			</td>
		</tr>
		<tr>
			<td><span class="label">Type</span></td>
			<td><asp:dropdownlist id="EnumTerrainTypeID" runat="Server" DataValueField="EnumTerrainTypeID" DataTextField="EnumTerrainTypeName"
					CssClass="input"></asp:dropdownlist></td>
			<td><input type="hidden" id="AreaID" runat="server" cssclass="input" value="1" size="2"></td>
		</tr>
		<tr>
			<td colspan="4"><hr>
			</td>
		</tr>
		<tr>
			<td colSpan="4"><asp:button id="Save" runat="server" Text="Save"></asp:button>&nbsp;<asp:button id="Cancel" runat="server" Text="Cancel"></asp:button>
			</td>
		</tr>
	</table>
</form>
<CONTROLS:FOOTER id="Footer1" runat="server" NAME="Footer1"></CONTROLS:FOOTER>
