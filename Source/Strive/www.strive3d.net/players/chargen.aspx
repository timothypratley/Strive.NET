<%@ Page language="c#" Codebehind="chargen.aspx.cs" AutoEventWireup="false" Inherits="www.strive3d.net.players.chargen" %>
<%@ Register TagPrefix="Controls" TagName="Header" Src="~/players/Controls/Header.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Footer" Src="~/players/Controls/Footer.ascx" %>
<%@ Register TagPrefix="Controls" Namespace="www.strive3d.net.players.Controls" Assembly="www.strive3d.net" %>
<Controls:Header title="New Character Generation" runat="server" id=Header1 />
<form method="post" runat="server">
<INPUT id="referer" type="hidden" runat="server"> 
<table>
  <tr>
    <td><span class=Label>Character 
      Name</span></td>
    <td colspan="5"><asp:textbox id="CharacterName" runat="server" CssClass="Input" />&nbsp;<asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" CssClass="InputValidation" EnableClientScript="True" ControlToValidate="CharacterName" ErrorMessage="You must enter a character name." /></td>
  </tr>
  <tr>
    <td ><span class="Label">Character Race</span></td>
    <td colspan="5"><asp:dropdownlist id="EnumRaceID" runat="server" DataTextField="EnumRaceName" DataValueField="EnumRaceID" AutoPostBack="True" /></td>
  </tr>
  <asp:panel id=showrace runat="server" Visible="False">
  <TR>
	<TD valign="top"><SPAN class=Label>Description</SPAN></TD>
    <TD colspan="5"><asp:Label id=RaceDescription runat="server">Label</asp:Label></TD></TR>
    
  <TR>
          <TD valign="top"><SPAN class=Label>Modifiers</SPAN></TD>
          <TH><SPAN class=Label>Strength</SPAN></TH>
          <TH><SPAN class=Label>Constitution</SPAN></TH>
          <TH><SPAN class=Label>Dexterity</SPAN></TH>
          <TH><SPAN class=Label>Cognition</SPAN></TH>
          <TH><SPAN class=Label>Willpower</SPAN></TH></TR>
      <TR>
	<td></td>
                <TD><asp:Label id=StrengthModifier runat="server">Label</asp:Label></TD>
                <TD><asp:Label id=ConstitutionModifier runat="server">Label</asp:Label></TD>
                <TD><asp:Label id=DexterityModifier runat="server">Label</asp:Label></TD>
                <TD><asp:Label id=CognitionModifier runat="server">Label</asp:Label></TD>
                <TD><asp:Label id=WillpowerModifier runat="server">Label</asp:Label></TD></TR>
       
        <TR>
          <TD><SPAN class=Label>Saves</SPAN></TD>
                <TH><SPAN class=Label>Air</SPAN></TH>
                <TH><SPAN class=Label>Earth</SPAN></TH>
                <TH><SPAN class=Label>Water</SPAN></TH>
                <TH><SPAN class=Label>Fire</SPAN></TH>
                <TH><SPAN class=Label>Spirit</SPAN></TH></TR>
              <TR>
              <td></td>
                <TD><asp:Label id=AirModifier runat="server">Label</asp:Label></TD>
                <TD><asp:Label id=EarthModifier runat="server">Label</asp:Label></TD>
                <TD><asp:Label id=WaterModifier runat="server">Label</asp:Label></TD>
                <TD><asp:Label id=FireModifier runat="server">Label</asp:Label></TD>
                <TD><asp:Label id=SpiritModifier runat="server">Label</asp:Label></TD></TR>
           </asp:panel>
  <tr>

    <td colSpan=6><asp:button id=Button1 CssClass="DefaultButton" Runat="server" Text="Create Your Character"></asp:button></td></tr></table></form>		

<Controls:Footer runat="server" id=Footer1 />
