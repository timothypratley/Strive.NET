<%@ Control Language="c#" AutoEventWireup="false" Codebehind="Header.ascx.cs" Inherits="www.strive3d.net.players.Controls.Header" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="Controls" TagName="Login" Src="Login.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD>
    <title><%=this.Title%></title>
    <meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
    <meta name="CODE_LANGUAGE" Content="C#">
    <meta name=vs_defaultClientScript content="JavaScript">
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0">
    <style>
    BODY { FONT-SIZE: xx-small; background: url(<%=www.strive3d.net.Utils.ApplicationPath%>/net/strive3d/images/background.gif) #FFFFFF no-repeat left 0%;   FONT-FAMILY: Verdana, Arial, Helvetica }
    TD { font-size: xx-small; VERTICAL-ALIGN: middle }
    TH { font-size: xx-small; VERTICAL-ALIGN: middle }
    TD.Content {font-size:xx-small; vertical-align:top }
    LABEL {font-size: xx-small; font-weight:bolder; font-family: Verdana,Arial,Helvetica}
    INPUT {font-family: Verdana,Arial,Helvetica}
    INPUTVALIDATION {font-size: xx-small; color:red; font-weight:bolder; font-family: Verdana,Arial,Helvetica; vertical-align:middle;}
    </style>
</HEAD>
  <body MS_POSITIONING="FlowLayout">	
  <table width="100%" height="100%" >
	<tr>
		<td valign="top" class="Content"><h3><%=this.Title%></h3></td>
		<td valign="top" class="Content" align="right"><a href="/players" runat="server">Players</a> |<a href="/players/builders" runat="server" >Builders</a> |
		<br /><Controls:Login runat="server" /></td>
	</tr>
	<tr >
		<td height="100%" align="left" class="Content">
  