<%@ Register TagPrefix="Controls" TagName="Header" Src="~/players/Controls/Header.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Footer" Src="~/players/Controls/Footer.ascx" %>
<%@ Import Namespace="System.Data" %>
<%@ Page language="c#" Codebehind="TemplateItemEquipable.aspx.cs" AutoEventWireup="false" Inherits="www.strive3d.net.players.builders.objects.TemplateItemEquippable" %>
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
			<td><span class="label">Height</span></td>
			<td><asp:textbox id="Height" runat="server" cssclass="input" size="2" Text="2"></asp:textbox>(be 
				careful)</td>
		</tr>
		<tr>
			<td colspan="4"><hr>
			</td>
		</tr>
		<tr>
			<td><span class="label">Value</span></td>
			<td><asp:TextBox id="Value" CssClass="Input" Size="4" Runat="Server" /></td>
			<td><span class="label">Weight</span></td>
			<td><asp:TextBox id="Weight" CssClass="Input" Size="4" Runat="Server" /></td>
		</tr>
		<tr>
			<td><span class="label">Durability</span></td>
			<td><asp:DropDownList id="EnumItemDurabilityID" DataValueField="EnumItemDurabilityID" DataTextField="EnumItemDurabilityName"
					runat="Server" cssclass="Input" /></td>
		</tr>
		<tr>
			<td colspan="4"><hr>
			</td>
		</tr>
		<tr>
			<td><span class="label">Worn</span></td>
			<td><asp:DropDownList ID="EnumWearLocationID" DataValueField="EnumWearLocationID" DataTextField="EnumWearLocationName"
					CssClass="Input" runat="server" /></td>
		</tr>
		<tr>
			<td><span class="label">Armour Class</span></td>
			<td><asp:TextBox ID="ArmourClass" SIze="4" CssClass="Input" RUnat="Server" />
			</td>
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
