<%@ Control Language="c#" AutoEventWireup="false" Codebehind="CharGen.ascx.cs" Inherits="www.strive3d.net.CharGen" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="Portal" TagName="Title" Src="~/DesktopModuleTitle.ascx"%>
<%
if(www.strive3d.net.PortalSecurity.IsInRoles("Players"))
{
%>
<portal:title runat="server" ID="Title1" NAME="Title1"/>

<INPUT id="referer" type="hidden" runat="server" NAME="referer"> 
<table>
  <tr>
    <td class="Normal">Character Name</td>
    <td colspan="5"><asp:textbox id="CharacterName" runat="server" CssClass="NormalTextBox" />&nbsp;<asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" CssClass="InputValidation" EnableClientScript="True" ControlToValidate="CharacterName" ErrorMessage="You must enter a character name." /></td>
  </tr>
  <tr>
    <td class="Normal">Character Race</td>
    <td colspan="5"><asp:dropdownlist id="EnumRaceID" runat="server" DataTextField="EnumRaceName" DataValueField="EnumRaceID" CssClass="NormalTextBox" AutoPostBack="True" /></td>
  </tr>
  <asp:panel id=showrace runat="server" Visible="False">
  <TR>
	<TD valign="top" class=Normal>Description</TD>
    <TD colspan="5" class="Normal"><asp:Label id=RaceDescription runat="server">Label</asp:Label></TD></TR>
    
  <TR>
          <TH valign="top" class="NormalBold">Modifiers</TH>
          <TH class="Normal">Strength</TH>
          <td align="right" class="Normal"><asp:Label id=StrengthModifier runat="server">Label</asp:Label></TD>
          <TH class="NormalBold">Saves</TD>
          <TH class="Normal">Air</TH>
		  <td align="right" class="Normal"><asp:Label id=AirModifier runat="server">Label</asp:Label></TD>          
	</tr>
	<tr>
		<td></td>          
          <TH class="Normal">Constitution</TH>
          <td align="right" class="Normal"><asp:Label id=ConstitutionModifier runat="server">Label</asp:Label></TD>
		<td></td>
		<TH class="Normal">Earth</TH>          
         <td align="right" class="Normal"><asp:Label id=EarthModifier runat="server">Label</asp:Label></TD>		
    </tr>
    <tr>
		<td></td>
          <TH class="Normal">Dexterity</TH>
          <td align="right" class="Normal"><asp:Label id=DexterityModifier runat="server">Label</asp:Label></TD>
         <td></td>
         <TH class="Normal">Water</TH>         
                <td align="right" class="Normal"><asp:Label id=WaterModifier runat="server">Label</asp:Label></TD>         
    </tr>
	<tr>
		<td></td>
		 <TH class="Normal">Cognition</TH>                
         <td align="right" class="Normal"><asp:Label id=CognitionModifier runat="server">Label</asp:Label></TD>
         <td></td>
         <TH class="Normal">Fire</TH>
                <td align="right" class="Normal"><asp:Label id=FireModifier runat="server">Label</asp:Label></TD>         
    </tr>
    <tr>
		<td></td>
         <TH class="Normal">Willpower</TH><td align="right" class="Normal"><asp:Label id=WillpowerModifier runat="server">Label</asp:Label></TD>
       <td></td>
       <TH class="Normal">Spirit</TH>
       <td align="right" class="Normal"><asp:Label id=SpiritModifier runat="server">Label</asp:Label></TD>
    </TR>
           </asp:panel>
  <tr>

    <td colSpan=6><asp:button id=Button1 CssClass="NormalButton" Runat="server" Text="Create Your Character"></asp:button></td></tr></table>

<%
}
%>