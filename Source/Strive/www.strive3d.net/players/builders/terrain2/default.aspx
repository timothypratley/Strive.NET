<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="Controls" TagName="Footer" Src="~/players/Controls/Footer.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Header" Src="~/players/Controls/Header.ascx" %>
<%@ Page language="c#" Codebehind="default.aspx.cs" AutoEventWireup="false" Inherits="www.strive3d.net.players.builders.terrain._default" %>

<Controls:Header runat="Server" title="Terrain Builder - Refedex View" ID="Header1"/>	
<form runat="server">
	<table>
  <TBODY>
		<tr>
			<td><span class="Label">Offset</span></td>
			<td><asp:TextBox CssClassName="Input" runat="server" Text="500" id=TextBox1 /></td>
		</tr>
		<tr>
			<td><span class="Label">Zoom Level</span></td>
			<td><asp:TextBox CssClassName="Input" runat="server" Text="100" id=TextBox2 /></td>
		</tr>
		<tr>
			<td colspan="2">&nbsp;<asp:Button id=Redraw runat="server" Text="Redraw"></asp:Button></td>
		</tr></TBODY>
		<tr>
		</tr>
	</table>

</form>

<table border="1" cellpadding="0" cellspacing="0">
	<tr>
		<td></td>
<%
bool passed = false;
foreach(DataRow d in this.squares.Rows)
{

	int x = (int)d["GroupZSimple"];

	if((x == int.Parse(TextBox1.Text) / int.Parse(TextBox2.Text) ||
		x == -(int.Parse(TextBox1.Text)  / int.Parse(TextBox2.Text)) ) && passed)
	{
		break;

	}
%>
		<td><nobr>X=<%=d["GroupZSimple"]%></nobr></td>
<%	
	
	passed = true;
	
}%>

	</tr>

	<tr>
<%
int enumx = 0;
int enumz = 0;

for(;enumx < this.squares.Rows.Count; enumx++)
{
	DataRow d = this.squares.Rows[enumx];
	int x = (int)this.squares.Rows[enumx]["GroupXSimple"];
	int z = (int)this.squares.Rows[enumx]["GroupZSimple"];

	if(z == -(int.Parse(TextBox1.Text) / int.Parse(TextBox2.Text)))
	{
%>
	</tr>
	<tr>
		<td valign="middle"><nobr>Z=<%=x%></nobr></td>
<%
	}


	%>

	<%
	if((bool)this.squares.Rows[enumx]["Contains"])
	{
	%>
		<td width="50" height="50" bgcolor="green" /><a href="editsquare.aspx?GroupXStart=<%=d["GroupXStart"]%>&GroupZStart=<%=d["GroupZStart"]%>&GroupXEnd=<%=d["GroupXEnd"]%>&GroupZEnd=<%=d["GroupZEnd"]%>">(<%=x%>,<%=z%>)</a></td>
	<%
	}
	else
	{
	%>
		<td width="50" height="50" bgcolor="red" /><a href="editsquare.aspx?GroupXStart=<%=d["GroupXStart"]%>&GroupZStart=<%=d["GroupZStart"]%>&GroupXEnd=<%=d["GroupXEnd"]%>&GroupZEnd=<%=d["GroupZEnd"]%>">(<%=x%>,<%=z%>)</a></td>
	<%	
	}
	%>

	<%


}

%>
</tr>
</table>



<Controls:Footer runat="server" id=Footer1 /></TR></TBODY></TABLE></FORM>
