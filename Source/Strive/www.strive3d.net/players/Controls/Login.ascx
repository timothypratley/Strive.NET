<%@ Control Language="c#" AutoEventWireup="false" Codebehind="Login.ascx.cs" Inherits="www.strive3d.net.players.Controls.Login" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%
if(this.LoginFailed)
{
%>
<font color="red">Login Failed</font><br />
<%
}
%>
<%
if(www.strive3d.net.players.SecurityProvider.PlayerAuthenticator.CurrentPlayerLoggedIn)
{
%>
<%=www.strive3d.net.players.SecurityProvider.PlayerAuthenticator.CurrentPlayerName%><%
}
else
{
%>
<form method="post" action="<%=this.Page.Request.ServerVariables["SCRIPT_NAME"]%>">
<input type="hidden" name="Referer" value="<%=this.Page.Request.ServerVariables["HTTP_REFERER"]%>" />
<input type="hidden" name="LoginRequested" value="true" />
<input type="submit" value="Login" />
</form>
<%
}
%>