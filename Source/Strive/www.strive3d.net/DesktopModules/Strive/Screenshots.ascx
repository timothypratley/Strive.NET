<%@ Control Language="c#" AutoEventWireup="false" Codebehind="Screenshots.ascx.cs" Inherits="www.strive3d.net.Screenshots" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="Portal" TagName="Title" Src="~/DesktopModuleTitle.ascx"%>
<portal:title runat="server" ID="Title1" NAME="Title1"/>
<%
const string downloadsFolder = "~/screenshots";

%>
<table>
	<tr>
<%
int count = 0;			
foreach(string s in System.IO.Directory.GetFiles(Server.MapPath(downloadsFolder)))
{
	DateTime created = System.IO.File.GetCreationTime(s);
	if(count % 4 == 0)
	{
%>
	</tr><tr>
<%
	}
%>
		<td class="Normal" align="right"><a href="<%=www.strive3d.net.Utils.ApplicationPath%>/screenshots/<%=System.IO.Path.GetFileName(s)%>"><img border="0" height="200" width="200" src="<%=www.strive3d.net.Utils.ApplicationPath%>/DesktopModules/Strive/Thumbnailer.aspx?i=<%=Server.UrlEncode(downloadsFolder.Replace("~", www.strive3d.net.Utils.ApplicationPath) + "/" + System.IO.Path.GetFileName(s)) + "&amp;h=200&amp;w=200"%>" /></a><br /><%=created.ToString("dd MMM yyyy h:mm tt")%></td>
<%
	count++;
}	
%>
</tr>
</table>