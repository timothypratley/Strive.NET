<%@ Register TagPrefix="Controls" TagName="Footer" Src="~/players/Controls/Footer.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Header" Src="~/players/Controls/Header.ascx" %>
<%@ Page language="c#" Codebehind="CreateAccount.aspx.cs" AutoEventWireup="false" Inherits="www.strive3d.net.players.CreateAccount" %>
<Controls:Header runat="server" Title="Create a new account" id="Header1" />
<form runat="server">
	<table width="100%">
		<asp:PlaceHolder id="Chargen" runat="Server" visible="true">
			<TBODY>
  <TR>
					<TD class="label">Character name</TD>
					<TD colSpan="6">
						<asp:textbox id="CharacterName" runat="server" CssClass="Input"></asp:textbox>&nbsp;
						<asp:requiredfieldvalidator id="Requiredfieldvalidator4" runat="server" CssClass="InputValidation" EnableClientScript="True"
							ControlToValidate="CharacterName" ErrorMessage="You must enter a character name."></asp:requiredfieldvalidator></TD>
				</TR>
  <TR>
					<TD></TD>
					<TD colSpan="6">Please see the <A href="~/DesktopDefault.aspx?tabindex=5&amp;tabid=12#0%20'NAMING%20POLICY'" target="_blank"
							runat="server">naming guidelines</A></TD>
				</TR>
  <TR>
					<TD><SPAN class="Label">Character race</SPAN></TD>
					<TD colSpan="6">
						<asp:dropdownlist id="EnumRaceID" runat="server" DataTextField="EnumRaceName" DataValueField="EnumRaceID"
							AutoPostBack="True"></asp:dropdownlist>&nbsp;
						<asp:requiredfieldvalidator id="Requiredfieldvalidator5" runat="server" CssClass="InputValidation" EnableClientScript="True"
							ControlToValidate="EnumRaceID" ErrorMessage="You must select a character race."></asp:requiredfieldvalidator></TD>
				</TR>
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
  <TR>
					<TD colSpan="7">
						<HR noShade>
					</TD>
				</TR>
  <TR>
					<TD class="label"><NOBR>Your e-mail address</NOBR></TD>
					<TD class="input" colSpan="5">
						<asp:TextBox id="PlayerEmail" runat="server"></asp:TextBox>&nbsp;
						<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" CssClass="InputValidation" EnableClientScript="False"
							ControlToValidate="PlayerEmail" ErrorMessage="You must enter a valid e-mail address."></asp:RequiredFieldValidator>
						<asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" CssClass="InputValidation" EnableClientScript="False"
							ControlToValidate="PlayerEmail" ErrorMessage="You must enter a valid e-mail address." ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator></TD>
					<TD width="100%"></TD>
				</TR>
  <TR>
					<TD class="label">Password</TD>
					<TD class="input" colSpan="6">
						<asp:TextBox id="PlayerPassword" runat="server" CssClass="Input" TextMode="Password"></asp:TextBox>&nbsp;
						<asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" CssClass="InputValidation" EnableClientScript="False"
							ControlToValidate="PlayerPassword" ErrorMessage="You must specify a password for your new account."></asp:RequiredFieldValidator></TD>
					</TD></TR>
  <TR>
					<TD class="label">Confirm password</TD>
					<TD class="input" colSpan="6">
						<asp:TextBox id="ConfirmPlayerPassword" runat="server" CssClass="Input" TextMode="Password"></asp:TextBox>&nbsp;
						<asp:RequiredFieldValidator id="Requiredfieldvalidator3" runat="server" CssClass="InputValidation" EnableClientScript="False"
							ControlToValidate="PlayerPassword" ErrorMessage="You must confirm the password for your new account."></asp:RequiredFieldValidator></TD>
					</TD></TR>
  <TR>
					<TD colSpan="7">
						<HR noShade>
					</TD>
				</TR>
  <TR>
					<TD colSpan="7">
						<asp:CompareValidator id=CompareValidator1 runat="server" CssClass="InputValidation" EnableClientScript="false" ControlToValidate="PlayerPassword" ErrorMessage="Passwords must match" ControlToCompare="ConfirmPlayerPassword" ValueToCompare="<%PlayerPassword.Text%>">
						</asp:CompareValidator></TD>
				</TR>
  <TR>
					<TD colSpan="7">
						<asp:Button id="Save" runat="server" text="Save"></asp:Button>
						<asp:Button id="Cancel" runat="server" Text="Cancel"></asp:Button></TD>
				</TR>
		</asp:PlaceHolder>
		<asp:PlaceHolder id="Success" runat="server" visible="False">
			<TR>
				<TD colSpan="7">Character created successfully. You will now receive an e-mail 
					which contains instructions to active your account. <br />
					<BR>
					<asp:Button id="Help" runat="server" Text="Home"></asp:Button></TD>
			</TR>
		</asp:PlaceHolder></TBODY>
	</table>
</form>
</CONTROLS:FOOTER>
