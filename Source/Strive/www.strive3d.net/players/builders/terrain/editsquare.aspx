<%@ Page language="c#" Codebehind="editsquare.aspx.cs" AutoEventWireup="false" Inherits="www.strive3d.net.players.builders.terrain.editsquare" %>
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="Controls" TagName="Footer" Src="~/players/Controls/Footer.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Header" Src="~/players/Controls/Header.ascx" %>

<Controls:Header runat="Server" title="Terrain Builder - Map View" ID="Header1"/>	
<table>
	<tr>
		<td><a href="<%=Request.Url.ToString().Replace("GroupXStart=" + startX, "GroupXStart=" + (startX - 50)).Replace(
		"GroupZStart=" + startZ, "GroupZStart=" + (startZ + 50)).Replace(
		"GroupXEnd=" + endX, "GroupXEnd=" + (endX - 50)).Replace(
		"GroupZEnd=" + endZ, "GroupZEnd=" + (endZ + 50))%>">[NW]</a></td>
		<td><a href="<%=Request.Url.ToString().Replace("GroupZStart=" + startZ, "GroupZStart=" + (startZ + 50)).Replace(
		"GroupZEnd=" + endZ, "GroupZEnd=" + (endZ + 50))%>">[N]</a></td>
		<td><a href="<%=Request.Url.ToString().Replace("GroupXStart=" + startX, "GroupXStart=" + (startX + 50)).Replace(
		"GroupZStart=" + startZ, "GroupZStart=" + (startZ + 50)).Replace(
		"GroupXEnd=" + endX, "GroupXEnd=" + (endX + 50)).Replace(
		"GroupZEnd=" + endZ, "GroupZEnd=" + (endZ + 50))%>">[NE]</a></td>
	</tr>
	<tr>
		<td><a href="<%=Request.Url.ToString().Replace("GroupXStart=" + startX, "GroupXStart=" + (startX - 50)).Replace(
		"GroupXEnd=" + endX, "GroupXEnd=" + (endX - 50))%>">[W]</a></td>
		<td></td>
		<td><a href="<%=Request.Url.ToString().Replace("GroupXStart=" + startX, "GroupXStart=" + (startX + 50)).Replace(
		"GroupXEnd=" + endX, "GroupXEnd=" + (endX + 50))%>">[E]</a></td>		
	</tr>
	<tr>
		<td><a href="<%=Request.Url.ToString().Replace("GroupXStart=" + startX, "GroupXStart=" + (startX - 50)).Replace(
		"GroupZStart=" + startZ, "GroupZStart=" + (startZ - 50)).Replace(
		"GroupXEnd=" + endX, "GroupXEnd=" + (endX - 50)).Replace(
		"GroupZEnd=" + endZ, "GroupZEnd=" + (endZ - 50))%>">[SW]</a></td>
		<td><a href="<%=Request.Url.ToString().Replace("GroupZStart=" + startZ, "GroupZStart=" + (startZ - 50)).Replace(
		"GroupZEnd=" + endZ, "GroupZEnd=" + (endZ - 50))%>">[S]</a></td>
		<td><a href="<%=Request.Url.ToString().Replace("GroupXStart=" + startX, "GroupXStart=" + (startX + 50)).Replace(
		"GroupZStart=" + startZ, "GroupZStart=" + (startZ - 50)).Replace(
		"GroupXEnd=" + endX, "GroupXEnd=" + (endX + 50)).Replace(
		"GroupZEnd=" + endZ, "GroupZEnd=" + (endZ - 50))%>">[SE]</a></td>
	</tr>		
</table>
<%
if(Request.QueryString["view"] != null && Request.QueryString["view"] != "")
{
	%><a href="<%=Request.Url.ToString().Replace("view=object", "")%>">Object View</a><%
}
else
{
	%><a href="<%=Request.Url.ToString() + "&amp;view=object"%>">Terrain View</a><%
}
%>
<table border="0" cellpadding="0" cellspacing="0" width="100%">



<%
int lastsx = 0;
bool writtenRowSpan = false;
int c = 0;


for(int row = 0; row < Strive.Common.Constants.worldBuilderTerrainSquareSize; row ++)
{
	%>
	<tr>		
	<%
	for(int col = 0; col < Strive.Common.Constants.worldBuilderTerrainSquareSize; col ++)
	{
		float x = startX + col * Strive.Common.Constants.terrainPieceSize;
		float z = startZ + Strive.Common.Constants.worldBuilderTerrainSquareSize * Strive.Common.Constants.terrainPieceSize - ((row+1) * Strive.Common.Constants.terrainPieceSize);
		DataRow[] tCandidates = terrain.Select("X = " + x + " AND Z = " + z);
		string ObjectInstanceID = "0";
		if(tCandidates.Length > 0)
		{
			DataRow t = tCandidates[0];
			ObjectInstanceID = t["ObjectInstanceID"].ToString();
		}
		
	%>
	<td height="<%=Strive.Common.Constants.worldBuilderTerrainPieceSize%>" width="<%=Strive.Common.Constants.worldBuilderTerrainPieceSize%>" valign="middle" align="center"><iframe id="frame<%=c.ToString()%>" src="showterrainpiece<%=Request.QueryString["view"]%>.aspx?view=<%=Request.QueryString["view"]%>&FrameID=frame<%=c.ToString()%>&ObjectInstanceID=<%=ObjectInstanceID%>&X=<%=x%>&Z=<%=z%><%=www.strive3d.net.Utils.TabHref%>" marginwidth=0 marginheight=0 hspace=0 vspace=0 width="75" height="75" frameborder=0 scrolling=no></iframe></td>
	<%
		c++;
	}
	if(!writtenRowSpan)
	{
	%>
		<td height="100%" width="100%" rowspan="<%=this.terrain.Rows.Count%>"><iframe id="Editor" name="Editor" marginwidth=0 marginheight=0 hspace=0 vspace=0 width="100%" height="50%" frameborder=0></iframe>
			<iframe id="ObjectInstanceEditor" name="ObjectInstanceEditor" marginwidth=0 marginheight=0 hspace=0 vspace=0 width="100%" height="50%" frameborder=0></iframe>
		</td>	
	</tr>		
	<%
		writtenRowSpan = true;
	}
	
}
%>
</table>
<Controls:Footer runat="server" />