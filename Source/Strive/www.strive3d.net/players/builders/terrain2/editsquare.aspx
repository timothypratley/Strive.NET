<%@ Page language="c#" Codebehind="editsquare.aspx.cs" AutoEventWireup="false" Inherits="www.strive3d.net.players.builders.terrain2.editsquare" %>
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="Controls" TagName="Footer" Src="~/players/Controls/Footer.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Header" Src="~/players/Controls/Header.ascx" %>

<Controls:Header runat="Server" title="Terrain Builder - Map View" ID="Header1"/>	

<table border="0" cellpadding="0" cellspacing="0" width="100%">
<tr>

<%
int lastsx = 0;
bool writtenRowSpan = false;
int c = 0;
foreach(DataRow t in this.terrain.Rows)
{

	int sx = (int)t["GroupXSimple"];
	int sz = (int)t["GroupZSimple"];
	
	if(lastsx != sx)
	{
	if(!writtenRowSpan)
	{
		writtenRowSpan = true;
	%>
		<td height="100%" width="100%" rowspan="<%=this.terrain.Rows.Count%>"><iframe id="Editor" name="Editor" marginwidth=0 marginheight=0 hspace=0 vspace=0 width="100%" height="100%" frameborder=0></iframe></td>
	<%	
	}	
%>
	</tr>
	<tr>
<%
	}
	lastsx = sx;
	
%>
		<td height="75" width="75" valign="middle" align="center"><iframe id="frame<%=c.ToString()%>" src="showterrainpiece.aspx?FrameID=frame<%=c.ToString()%>&ObjectInstanceID=<%=t["ObjectInstanceID"]%>&X=<%=sx*100%>&Z=<%=sz*100%>" marginwidth=0 marginheight=0 hspace=0 vspace=0 width="75" height="75" frameborder=0 scrolling=no></iframe></td>
<%
	c++;
}
%>


</tr>

</table>

<Controls:Footer runat="server" />