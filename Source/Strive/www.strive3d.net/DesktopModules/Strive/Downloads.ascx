<%@ Control Language="c#" AutoEventWireup="false" Codebehind="Downloads.ascx.cs" Inherits="www.strive3d.net.Downloads" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="Portal" TagName="Title" Src="~/DesktopModuleTitle.ascx"%>
<portal:title runat="server" ID="Title1" NAME="Title1"/>
<%
const string downloadsFolder = "~/downloads";

%>
<table>
<%			
foreach(string s in System.IO.Directory.GetFiles(Server.MapPath(downloadsFolder)))
{%>
	<tr>
		<td class="NormalBold"><a href="<%=www.strive3d.net.Utils.ApplicationPath%>/downloads/<%=System.IO.Path.GetFileName(s)%>" ><%=System.IO.Path.GetFileName(s)%></a></td>
	</tr>	<%
}
%>

</table>