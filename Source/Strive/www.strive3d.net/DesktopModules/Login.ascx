<%@ Control Language="c#" AutoEventWireup="false" Codebehind="Login.ascx.cs" Inherits="www.strive3d.net.Login" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%
if(www.strive3d.net.Game.PlayerAuthenticator.CurrentPlayerLoggedIn)
{
%><span class="SiteLink"><%=www.strive3d.net.Game.PlayerAuthenticator.CurrentPlayerEmail%></span><%
			www.strive3d.net.UsersDB user = new www.strive3d.net.UsersDB();
			string[] userRoles = user.GetRoles(www.strive3d.net.Game.PlayerAuthenticator.CurrentPlayerEmail);
			//String.Join(";", userRoles)%><%
}
else
{
%>
<a class="SiteLink" href="<%=this.Page.Request.ServerVariables["SCRIPT_NAME"]%>?Referer=<%=this.Page.Request.ServerVariables["HTTP_REFERER"]%>&LoginRequested=true">Login</a>
<%
}
%>