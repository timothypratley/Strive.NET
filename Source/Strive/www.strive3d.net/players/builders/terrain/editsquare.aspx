<%@ Page language="c#" Codebehind="editsquare.aspx.cs" AutoEventWireup="false" Inherits="www.strive3d.net.players.builders.terrain.editsquare" %>
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="Controls" TagName="Footer" Src="~/players/Controls/Footer.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Header" Src="~/players/Controls/Header.ascx" %>

<Controls:Header runat="Server" title="Terrain Builder - Map View" ID="Header1"/>	
<table>
	<tr>
		<td><a href="<%=Request.Url.ToString().Replace("GroupXStart=" + startX, "GroupXStart=" + (startX - 100)).Replace(
		"GroupZStart=" + startZ, "GroupZStart=" + (startZ + 100)).Replace(
		"GroupXEnd=" + endX, "GroupXEnd=" + (endX - 100)).Replace(
		"GroupZEnd=" + endZ, "GroupZEnd=" + (endZ + 100))%>">[NW]</a></td>
		<td><a href="<%=Request.Url.ToString().Replace("GroupZStart=" + startZ, "GroupZStart=" + (startZ + 100)).Replace(
		"GroupZEnd=" + endZ, "GroupZEnd=" + (endZ + 100))%>">[N]</a></td>
		<td><a href="<%=Request.Url.ToString().Replace("GroupXStart=" + startX, "GroupXStart=" + (startX + 100)).Replace(
		"GroupZStart=" + startZ, "GroupZStart=" + (startZ + 100)).Replace(
		"GroupXEnd=" + endX, "GroupXEnd=" + (endX + 100)).Replace(
		"GroupZEnd=" + endZ, "GroupZEnd=" + (endZ + 100))%>">[NE]</a></td>
	</tr>
	<tr>
		<td><a href="<%=Request.Url.ToString().Replace("GroupXStart=" + startX, "GroupXStart=" + (startX - 100)).Replace(
		"GroupXEnd=" + endX, "GroupXEnd=" + (endX - 100))%>">[W]</a></td>
		<td></td>
		<td><a href="<%=Request.Url.ToString().Replace("GroupXStart=" + startX, "GroupXStart=" + (startX + 100)).Replace(
		"GroupXEnd=" + endX, "GroupXEnd=" + (endX + 100))%>">[E]</a></td>		
	</tr>
	<tr>
		<td><a href="<%=Request.Url.ToString().Replace("GroupXStart=" + startX, "GroupXStart=" + (startX - 100)).Replace(
		"GroupZStart=" + startZ, "GroupZStart=" + (startZ - 100)).Replace(
		"GroupXEnd=" + endX, "GroupXEnd=" + (endX - 100)).Replace(
		"GroupZEnd=" + endZ, "GroupZEnd=" + (endZ - 100))%>">[SW]</a></td>
		<td><a href="<%=Request.Url.ToString().Replace("GroupZStart=" + startZ, "GroupZStart=" + (startZ - 100)).Replace(
		"GroupZEnd=" + endZ, "GroupZEnd=" + (endZ - 100))%>">[S]</a></td>
		<td><a href="<%=Request.Url.ToString().Replace("GroupXStart=" + startX, "GroupXStart=" + (startX + 100)).Replace(
		"GroupZStart=" + startZ, "GroupZStart=" + (startZ - 100)).Replace(
		"GroupXEnd=" + endX, "GroupXEnd=" + (endX + 100)).Replace(
		"GroupZEnd=" + endZ, "GroupZEnd=" + (endZ - 100))%>">[SE]</a></td>
	</tr>		
</table>
<table border="0" cellpadding="0" cellspacing="0" width="100%">
<tr>

<%
int lastsx = 0;
bool writtenRowSpan = false;
int c = 0;
foreach(DataRow t in this.terrain.Rows)
{

	if(c % 10 == 0 && c != 0)
	{
	if(!writtenRowSpan)
	{
		writtenRowSpan = true;
	%>
		<td height="100%" width="100%" rowspan="<%=this.terrain.Rows.Count%>"><iframe id="Editor" name="Editor" marginwidth=0 marginheight=0 hspace=0 vspace=0 width="100%" height="50%" frameborder=0></iframe>
			<iframe id="ObjectInstanceEditor" name="ObjectInstanceEditor" marginwidth=0 marginheight=0 hspace=0 vspace=0 width="100%" height="50%" frameborder=0></iframe>
		</td>
	<%	
	}	
%>
	</tr>
	<tr>
<%
	}
%>
		<td height="75" width="75" valign="middle" align="center"><iframe id="frame<%=c.ToString()%>" src="showterrainpiece.aspx?FrameID=frame<%=c.ToString()%>&ObjectInstanceID=<%=t["ObjectInstanceID"]%>&X=<%=t["X"]%>&Z=<%=t["Z"]%><%=www.strive3d.net.Utils.TabHref%>" marginwidth=0 marginheight=0 hspace=0 vspace=0 width="75" height="75" frameborder=0 scrolling=no></iframe></td>
<%
	c++;
}
%>


</tr>

</table>
<Controls:Footer runat="server" />