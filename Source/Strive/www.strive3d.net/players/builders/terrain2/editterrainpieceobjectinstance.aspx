<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="Controls" TagName="Header" Src="~/players/Controls/Header.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Footer" Src="~/players/Controls/Footer.ascx" %>
<%@ Page language="c#" Codebehind="editterrainpieceobjectinstance.aspx.cs" AutoEventWireup="false" Inherits="www.strive3d.net.players.builders.terrain2.editterrainpieceobjectinstance" %>
<form runat="server">
	<table>
		<tr>
			<td><span class="label">Template</span></td>
			<td><asp:dropdownlist id="TemplateList" runat="Server" cssclass="input" DataTextField="TemplateObjectName"
					DataValueField="TemplateObjectID"></asp:dropdownlist></td>
		</tr>
		<tr>
			<td colSpan="2"><span class="label">Position</span></td>
		</tr>
		<tr>
			<td colspan="2"><asp:radiobuttonlist id="Coords" RepeatDirection="Horizontal" cssclass="input" DataTextField="" DataTextFormatString=" " RepeatColumns="10"
					runat="Server" /></td>
		</tr>
		<tr>
			<td colspan="2"><asp:Button ID="Save" runat="server" Text="Save" /></td>
		</tr>
	</table>
</form>
