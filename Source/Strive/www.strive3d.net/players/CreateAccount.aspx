<%@ Page language="c#" Codebehind="CreateAccount.aspx.cs" AutoEventWireup="false" Inherits="www.strive3d.net.players.CreateAccount" %>
<%@ Register TagPrefix="Controls" TagName="Header" Src="~/players/Controls/Header.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Footer" Src="~/players/Controls/Footer.ascx" %>
<Controls:Header runat="server" Title="Create a new account" id="Header1" />
<form runat="server">
	<table width="100%">
		<tr>
			<td class="label">Character name</td>
			<td colspan="6"><asp:textbox id="CharacterName" runat="server" CssClass="Input" />&nbsp;<asp:requiredfieldvalidator id="Requiredfieldvalidator4" runat="server" CssClass="InputValidation" EnableClientScript="True"
					ControlToValidate="CharacterName" ErrorMessage="You must enter a character name." /></td>
		</tr>
		<tr>
			<td></td>
			<td colspan="6">Please see the <a runat="server" href="~/DesktopDefault.aspx?tabindex=5&amp;tabid=12#0%20'NAMING%20POLICY'"
					target="_blank">naming guidelines</a></td>
		</tr>
		<tr>
			<td><span class="Label">Character race</span></td>
			<td colspan="6"><asp:dropdownlist id="EnumRaceID" runat="server" DataTextField="EnumRaceName" DataValueField="EnumRaceID"
					AutoPostBack="True" />&nbsp;<asp:requiredfieldvalidator id="Requiredfieldvalidator5" runat="server" CssClass="InputValidation" EnableClientScript="True"
					ControlToValidate="EnumRaceID" ErrorMessage="You must select a character race." /></td>
		</tr>
		<asp:panel id="showrace" runat="server" Visible="False">
			<TR>
				<TD vAlign="top"><SPAN class="Label">Description</SPAN></TD>
				<TD colSpan="6">
					<asp:Label id="RaceDescription" runat="server">Label</asp:Label></TD>
			</TR>
			<TR>
				<TD vAlign="top"><SPAN class="Label">Modifiers</SPAN></TH>
					<TH>
						<SPAN class="Label">Strength</SPAN></TH>
					<TH>
						<SPAN class="Label">Constitution</SPAN></TH>
					<TH>
						<SPAN class="Label">Dexterity</SPAN></TH>
					<TH>
						<SPAN class="Label">Cognition</SPAN></TH>
					<TH>
						<SPAN class="Label">Willpower</SPAN></TH>
			</TR>
			<TR>
				<TH>
				</TH>
				<TH>
					<asp:Label id="StrengthModifier" runat="server">Label</asp:Label></TH>
				<TH>
					<asp:Label id="ConstitutionModifier" runat="server">Label</asp:Label></TH>
				<TH>
					<asp:Label id="DexterityModifier" runat="server">Label</asp:Label></TH>
				<TH>
					<asp:Label id="CognitionModifier" runat="server">Label</asp:Label></TH>
				<TH>
					<asp:Label id="WillpowerModifier" runat="server">Label</asp:Label></TH></TR>
			<TR>
				<TD><SPAN class="Label">Saves</SPAN></TD>
				<TH>
					<SPAN class="Label">Air</SPAN></TH>
				<TH>
					<SPAN class="Label">Earth</SPAN></TH>
				<TH>
					<SPAN class="Label">Water</SPAN></TH>
				<TH>
					<SPAN class="Label">Fire</SPAN></TH>
				<TH>
					<SPAN class="Label">Spirit</SPAN></TH></TR>
			<TR>
				<TH>
				</TH>
				<TH>
					<asp:Label id="AirModifier" runat="server">Label</asp:Label></TH>
				<TH>
					<asp:Label id="EarthModifier" runat="server">Label</asp:Label></TH>
				<TH>
					<asp:Label id="WaterModifier" runat="server">Label</asp:Label></TH>
				<TH>
					<asp:Label id="FireModifier" runat="server">Label</asp:Label></TH>
				<TH>
					<asp:Label id="SpiritModifier" runat="server">Label</asp:Label></TH></TR>
		</asp:panel></TR>
		<tr>
			<td colspan="7"><hr noshade>
			</td>
		</tr>
		<tr>
			<td class="label"><nobr>Your e-mail address</nobr></td>
			<td colspan="5" class="input"><asp:TextBox id="PlayerEmail" runat="server" />&nbsp;<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="You must enter a valid e-mail address."
					ControlToValidate="PlayerEmail" CssClass="InputValidation" EnableClientScript="False"></asp:RequiredFieldValidator><asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" ErrorMessage="You must enter a valid e-mail address."
					ControlToValidate="PlayerEmail" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" EnableClientScript="False" CssClass="InputValidation"></asp:RegularExpressionValidator></td>
			<td width="100%"></td>
		</tr>
		<tr>
			<td class="label">Password</td>
			<td colspan="6" class="input"><asp:TextBox CssClass="Input" ID="PlayerPassword" TextMode="Password" runat="server" />&nbsp;<asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" ErrorMessage="You must specify a password for your new account."
					CssClass="InputValidation" ControlToValidate="PlayerPassword" EnableClientScript="False"></asp:RequiredFieldValidator></td>
			</TD>
		</tr>
		<tr>
			<td class="label">Confirm password</td>
			<td colspan="6" class="input"><asp:TextBox CssClass="Input" ID="ConfirmPlayerPassword" TextMode="Password" runat="server" />&nbsp;<asp:RequiredFieldValidator id="Requiredfieldvalidator3" runat="server" ErrorMessage="You must confirm the password for your new account."
					CssClass="InputValidation" ControlToValidate="PlayerPassword" EnableClientScript="False"></asp:RequiredFieldValidator></td>
			</TD>
		</tr>
		<tr>
			<td colspan="7"><hr noshade>
			</td>
		</tr>
		<tr>
			<td colspan="7"><asp:CompareValidator id="CompareValidator1" runat="server" ErrorMessage="Passwords must match" ControlToValidate="PlayerPassword"
					ControlToCompare="ConfirmPlayerPassword" CssClass="InputValidation" ValueToCompare='<%PlayerPassword.Text%>' EnableClientScript="false"></asp:CompareValidator></td>
		</tr>
		<tr>
			<td colspan="7">
				<asp:Button ID="Save" text="Save" runat="server" />
			</td>
		</tr>
	</table>
</form>
</CONTROLS:FOOTER>
