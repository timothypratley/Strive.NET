<%@ Page language="c#" Codebehind="showterrainpiece.aspx.cs" AutoEventWireup="false" Inherits="www.strive3d.net.players.builders.terrain.showterrainpiece" %>
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="Controls" TagName="Header" Src="~/players/Controls/Header.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Footer" Src="~/players/Controls/Footer.ascx" %>
<form runat="server">
			<%if(Loaded) {%>
			<a style="position:absolute;top:0px;left:0px" target="Editor" href="editterrainpiece.aspx?ObjectInstanceID=<%=ObjectInstanceID.ToString()%>&amp;X=<%=X%>&amp;Z=<%=Z%>&FrameID=<%=Request.QueryString["FrameID"]%>">
				<img src="<%=TextureSrc%>" border="0" />
			</a>
			<%
			foreach(DataRow foundObject in Objects.Rows)
			{
				float htmlX = float.Parse(foundObject["X"].ToString())- X;
				float htmlZ = Strive.Common.Constants.terrainPieceSize - (float.Parse(foundObject["Z"].ToString()) - Z);
				htmlX *= Strive.Common.Constants.worldBuilderTerrainPieceSize / Strive.Common.Constants.terrainPieceSize; 
				htmlZ *= Strive.Common.Constants.worldBuilderTerrainPieceSize / Strive.Common.Constants.terrainPieceSize; 
				htmlX -= 2.5f;
				htmlZ -= 2.5f;
				int htmlObjectInstanceID = int.Parse(foundObject["ObjectInstanceID"].ToString());				
				string TemplateObjectTemplateName = foundObject["TemplateName"].ToString();
				string TemplateObjectName = htmlObjectInstanceID + ": " + foundObject["TemplateObjectName"].ToString() + " (" + TemplateObjectTemplateName.Replace("Item", "") + ")";
			%>
				<a target="Editor" style="position:absolute;left:<%=htmlX%>px;top:<%=htmlZ%>px" href="editterrainpiece.aspx?ObjectInstanceID=<%=ObjectInstanceID.ToString()%>&amp;X=<%=X%>&amp;Z=<%=Z%>&FrameID=<%=Request.QueryString["FrameID"]%>&amp;LoadObjectInstanceID=<%=htmlObjectInstanceID%>&amp;LoadedObjectTemplateName=<%=TemplateObjectTemplateName%>"><img alt="<%=TemplateObjectName%>" src="<%=www.strive3d.net.Utils.ApplicationPath%>/images/object.gif" /></a>
			<%		
			
			}
			%>
		
			<%} else { %>
		<a target="Editor" href="editterrainpiece.aspx?X=<%=X%>&amp;Z=<%=Z%>&FrameID=<%=Request.QueryString["FrameID"]%>">[Create]</a></td>
			<%}%>
		</tr>
	</table>
</form>
