<%@ Page language="c#" Codebehind="activate.aspx.cs" AutoEventWireup="false" Inherits="www.strive3d.net.players.activate" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD>
    <title>active your strive3d.net account</title>
    <meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
    <meta name="CODE_LANGUAGE" Content="C#">
    <meta name=vs_defaultClientScript content="JavaScript">
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0">
    <style> BODY { FONT-SIZE: xx-small; BACKGROUND-IMAGE: url(../images/background.gif); BACKGROUND-REPEAT: no-repeat; FONT-FAMILY: Verdana, Arial, Helvetica }
	TD { font-size: xx-small; VERTICAL-ALIGN: middle }
	LABEL {font-size: xx-small; font-weight:bolder; font-family: Verdana,Arial,Helvetica}
	INPUT {font-family: Verdana,Arial,Helvetica}
	INPUTVALIDATION {font-size: xx-small; font-weight:bolder; font-family: Verdana,Arial,Helvetica; vertical-align:middle;}
	</style>
</HEAD>
  <body MS_POSITIONING="FlowLayout">	
	
	<asp:Panel ID="success" Runat="server">
<P align=left>Player <STRONG><%=this.playerName%></STRONG> successfully 
activated.</P>
	</asp:Panel>
	
	<asp:Panel ID="failure" Runat="server">
<P align=left>Player <STRONG>WAS NOT</STRONG> 
activatated.</P>
	</asp:Panel>	
	
  </body>
</HTML>
