<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="Controls" TagName="Footer" Src="~/players/Controls/Footer.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Header" Src="~/players/Controls/Header.ascx" %>
<%@ Page language="c#" Codebehind="TemplateMobile.aspx.cs" AutoEventWireup="false" Inherits="www.strive3d.net.players.builders.objects.TemplateMobile" %>
<CONTROLS:HEADER id="Header1" title="Resources - List View" runat="Server"></CONTROLS:HEADER>
<form runat="server">
	<table>
		<tr>
			<td><span class="label">Name</span></td>
			<td colSpan="3"><asp:textbox id="TemplateObjectName" runat="server" cssclass="input"></asp:textbox></td>
		</tr>
		<tr>
			<td><span class="label">Resource</span></td>
			<td><asp:dropdownlist id="ResourceID" runat="Server" DataValueField="ResourceID" DataTextField="ResourceName"></asp:dropdownlist></td>
			<td><span class="label">Height</span></td>
			<td><asp:textbox id="Height" runat="server" cssclass="input" size="2" Text="20"></asp:textbox>(be 
				careful)</td>
		</tr>
		<tr>
			<td colspan="4"><hr /></td>
		</tr>		
		<tr>
			<td><span class="label">Race</span></td>
			<td><asp:dropdownlist id="EnumRaceID" runat="Server" DataValueField="EnumRaceID" DataTextField="EnumRaceName"
					CssClass="input" AutoPostBack="True"></asp:dropdownlist></td>
			<td><span class="label">Size</span></td>
			<td><asp:dropdownlist id="EnumMobileSizeID" runat="Server" DataValueField="EnumMobileSizeID" DataTextField="EnumMobileSizeName"
					CssClass="input"></asp:dropdownlist></td>
		</tr>
		<tr>
			<td><span class="label">Sex</span></td>
			<td><asp:dropdownlist id="EnumSexID" runat="server" CssClass="input" DataValueField="EnumSexID" DataTextField="EnumSexName"></asp:dropdownlist></td>
			<td><span class="label">Level</span></td>
			<td><asp:TextBox id="Level" CssClass="input" runat="Server" Text="15" /></td>
		</tr>
		<tr>
			<td><span class="label">Strength</span></td>
			<td><asp:textbox id="Strength" runat="server" size="2"></asp:textbox></td>
			<td><span class="label">Constitution</span></td>
			<td><asp:textbox id="Constitution" runat="server" size="2"></asp:textbox></td>
		</tr>
		<tr>
			<td><span class="label">Cognition</span></td>
			<td><asp:textbox id="Cognition" runat="server" size="2"></asp:textbox></td>
			<td><span class="label">Willpower</span></td>
			<td><asp:textbox id="Willpower" runat="server" size="2"></asp:textbox></td>
		</tr>
		<tr>
			<td><span class="label">Dexterity</span></td>
			<td><asp:textbox id="Dexterity" runat="server" size="2"></asp:textbox></td>
			<td><span class="label">State</span></td>
			<td><asp:dropdownlist id="EnumMobileStateID" runat="Server" DataValueField="EnumMobileStateID" DataTextField="EnumMobileStateName"
					CssClass="input"></asp:dropdownlist></td>
		</tr>
		<tr>
			<td colspan="4"><hr /></td>
		</tr>		
		<tr>
			<td colSpan="4"><asp:button id="Save" runat="server" Text="Save"></asp:button>&nbsp;<asp:button id="Cancel" runat="server" Text="Cancel"></asp:button>
			</td>
		</tr>
	</table>
</form>
<CONTROLS:FOOTER id="Footer1" runat="server" NAME="Footer1"></CONTROLS:FOOTER>
