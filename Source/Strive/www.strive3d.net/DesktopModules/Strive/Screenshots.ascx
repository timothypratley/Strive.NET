<%@ Control Language="c#" AutoEventWireup="false" Codebehind="Screenshots.ascx.cs" Inherits="www.strive3d.net.Screenshots" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="Portal" TagName="Title" Src="~/DesktopModuleTitle.ascx"%>
<portal:title runat="server" ID="Title1" NAME="Title1"/>
<script language=C# runat="server">
class FileDateSorter : System.Collections.IComparer
{
 public int Compare(object a, object b)
 {
  System.IO.FileInfo f1 = (System.IO.FileInfo)a;
  System.IO.FileInfo f2 = (System.IO.FileInfo)b;
  
  if(f1.LastWriteTime  > f2.LastWriteTime ) { return -1; } 
  else if(f1.LastWriteTime  < f2.LastWriteTime ) { return 1; }
  else {return 0; }

}
}

</script>
<%
const string downloadsFolder = "~/screenshots";

%>
<table>
	<tr>
<%
System.IO.FileInfo[] files = new System.IO.DirectoryInfo(Server.MapPath(downloadsFolder)).GetFiles();
System.Array.Sort(files, new FileDateSorter());
int count = 0;			
foreach(System.IO.FileInfo f in files)
{
	DateTime created = f.LastWriteTime ;
	string s = f.FullName;
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