<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="Controls" TagName="Footer" Src="~/players/Controls/Footer.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Header" Src="~/players/Controls/Header.ascx" %>
<%@ Page language="c#" Codebehind="addresource.aspx.cs" AutoEventWireup="false" Inherits="www.strive3d.net.players.builders.resources.addresource" %>
<Controls:Header runat="Server" title="Resources - List View" ID="Header1" />
<form enctype="multipart/form-data" runat="server" ID="Form1">
	<table>
		<tr>
			<td><span class="Label">Resource Pak</span></td>
			<td><asp:DropDownList id="ResourcePak" runat="server" cssclass="input" DataValueField="ResourcePak" DataTextField="ResourcePak" /><asp:TextBox id="ResourcePakOther" Css="Input" runat="server" />
			</td>
		</tr>
		<tr>
			<td><span class="Label">Resource Name</span></td>
			<td><asp:TextBox id="ResourceName" runat="server" CssClass="Input"></asp:TextBox><asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="You must enter a  Resource Name"
					ControlToValidate="ResourceName" CssClass="InputValidation"></asp:RequiredFieldValidator></td>
		</tr>
		<tr>
			<td><span class="Label">Resource Description</span></td>
			<td><asp:TextBox id="ResourceDescription" runat="server" CssClass="Input" TextMode="MultiLine"></asp:TextBox></td>
		</tr>
		<tr>
			<td>Resource</td>
			<td><input name="ResourceFile" class="Input" type="file"><asp:Label CssClass="InputValidation" id="BitmapWarning" runat="server"></asp:Label></td>
		</tr>
		<tr>
			<td colspan="2"><asp:Button id="Add" runat="server" Text="Save Resource" /></td>
		</tr>
	</table>
</form>
<Controls:Footer runat="server" ID="Footer1" NAME="Footer1" />
