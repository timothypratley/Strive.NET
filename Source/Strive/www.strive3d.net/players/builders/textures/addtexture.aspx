<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="Controls" TagName="Footer" Src="~/players/Controls/Footer.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Header" Src="~/players/Controls/Header.ascx" %>
<%@ Page language="c#" Codebehind="addtexture.aspx.cs" AutoEventWireup="false" Inherits="www.strive3d.net.players.builders.textures.addtexture" %>
<Controls:Header runat="Server" title="Textures - Add A Texture" ID="Header1"/>

<form enctype="multipart/form-data" runat="server">

<table>
	<tr>
		<td><span class="Label">Model Name</span></td>
		<td><asp:TextBox id=ModelName runat="server" CssClass="Input"></asp:TextBox><asp:RequiredFieldValidator id=RequiredFieldValidator1 runat="server" ErrorMessage="You must enter a  Model Name" ControlToValidate="ModelName" CssClass="InputValidation"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td><span class="Label">Model Description</span></td>
		<td><asp:TextBox id=ModelDescription runat="server" CssClass="Input" TextMode="MultiLine"></asp:TextBox><asp:RequiredFieldValidator id=RequiredFieldValidator2 runat="server" ErrorMessage="You must enter a Model Description" CssClass="InputValidation" ControlToValidate="ModelDescription"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td>Bitmap</td>
		<td><input name="ModelFile" class="Input" type="file"><asp:Label CssClass="InputValidation" id="BitmapWarning" runat="server"></asp:Label></td>
	</tr>
	<tr>
		<td colspan="2"><asp:Button id=Add runat="server" Text="Add Texture"/></td>
	</tr>
</table>

</form>

<Controls:Footer runat="server" id=Footer1 />
