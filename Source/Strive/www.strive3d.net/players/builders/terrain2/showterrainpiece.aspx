<%@ Page language="c#" Codebehind="showterrainpiece.aspx.cs" AutoEventWireup="false" Inherits="www.strive3d.net.players.builders.terrain2.showterrainpiece" %>
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="Controls" TagName="Header" Src="~/players/Controls/Header.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Footer" Src="~/players/Controls/Footer.ascx" %>
<form runat="server">
	<table height="100%" width="100%" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<%if(Loaded) {%>
			<td background="<%=TextureSrc%>" valign="middle" align="center" height="100%" width="100%">
				<table>
					<tr>
						<td colspan="3"><a target="Editor" href="editterrainpiece.aspx?ObjectInstanceID=<%=ObjectInstanceID.ToString()%>&amp;X=<%=X%>&amp;Z=<%=Z%>&FrameID=<%=Request.QueryString["FrameID"]%>">[Edit]</a></td>
					</tr>
					<tr>
						<td><asp:Button ID="Higher" Text="+" Runat="server" /></td>
						<td><%=Math.Round(Altitude, 0).ToString()%></td>
						<td><asp:Button ID="Lower" Text="-" Runat="server" /></td>
					</tr>
					<tr>
						<td></td>
						<td></td>
						<td><asp:Button ID="Rotate" Text="/" Runat="server" /></td>
					</tr>
				</table>
			</td>
			<%} else { %>
			<td><a target="Editor" href="editterrainpiece.aspx?X=<%=X%>&amp;Z=<%=Z%>&FrameID=<%=Request.QueryString["FrameID"]%>">[Create]</a></td>
			<%}%>
		</tr>
	</table>
</form>
