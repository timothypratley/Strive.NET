<%@ Page language="c#" Codebehind="editsquare.aspx.cs" AutoEventWireup="false" Inherits="www.strive3d.net.players.builders.terrain.editsquare" %>
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="Controls" TagName="Footer" Src="~/players/Controls/Footer.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Header" Src="~/players/Controls/Header.ascx" %>

<Controls:Header runat="Server" title="Terrain Builder - Map View" ID="Header1"/>	

<table border="1" cellpadding="0" cellspacing="0">
<tr>

<%
int lastsx = 0;
foreach(DataRow t in this.terrain.Rows)
{

	int sx = (int)t["GroupXSimple"];
	int sz = (int)t["GroupZSimple"];
	
	if(lastsx != sx)
	{
%>
	</tr>
	<tr>
<%
	}
	lastsx = sx;
	
%>
		<td height="75" width="75" valign="middle" align="center">
		<a href="editterrainpiece.aspx?X=<%=sx*100%>&Z=<%=sz*100%><%
	if(t["ObjectInstanceID"].ToString() != "")
	{
%>&ObjectInstanceID=<%=t["ObjectInstanceID"]%>">
	<img border="0" height="75" width="75" src="<%=www.strive3d.net.Utils.ApplicationPath%>/DesktopModules/Strive/Thumbnailer.aspx?i=<%=www.strive3d.net.Utils.ApplicationPath%>/players/builders<%=System.Configuration.ConfigurationSettings.AppSettings["resourcepath"]%>/textures/<%=t["ModelID"]%>.bmp&amp;h=75&amp;w=75" />
<%
	}
	else
	{
%>">[Add]
<%
	}
%>		</a>
		</td>
<%


}
%>


</tr>

</table>

<Controls:Footer runat="server" />