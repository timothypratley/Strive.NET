<%@ Control Language="c#" AutoEventWireup="false" Codebehind="Header.ascx.cs" Inherits="www.strive3d.net.players.Controls.Header" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="Controls" TagName="Login" Src="Login.ascx" %>
<%@ Register TagPrefix="portal" TagName="Banner" Src="~/DesktopPortalBanner.ascx" %>
<%@ Register TagPrefix="Portal" TagName="Title" Src="~/DesktopModuleTitle.ascx"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
    <head>
        <title>s t r i v e 3 d . n e t </title>
        <link href="<%=www.strive3d.net.Utils.ApplicationPath%>/portal.css" type="text/css" rel="stylesheet">
    <style>
    BODY { FONT-SIZE: xx-small; background: url(<%=www.strive3d.net.Utils.ApplicationPath%>/images/background.gif) #FFFFFF no-repeat left 0%;   FONT-FAMILY: Verdana, Arial, Helvetica }
    TD { font-size: xx-small; VERTICAL-ALIGN: middle }
    TH { font-size: xx-small; VERTICAL-ALIGN: middle }
    TD.Content {font-size:xx-small; vertical-align:top }
    LABEL {font-size: xx-small; font-weight:bolder; font-family: Verdana,Arial,Helvetica}
    INPUT {font-family: Verdana,Arial,Helvetica}
    INPUTVALIDATION {font-size: xx-small; color:red; font-weight:bolder; font-family: Verdana,Arial,Helvetica; vertical-align:middle;}
    </style>        
    </head>
    <body leftmargin="0" bottommargin="0" rightmargin="0" topmargin="0" marginheight="0" marginwidth="0">
            <table width="100%" cellspacing="0" cellpadding="0" border="0">
                <tr valign="top">
                    <td colspan="2">
                        <portal:Banner id="Banner" SelectedTabIndex="0" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <br>
                        <table width="100%" cellspacing="0" cellpadding="4" border="0">
                            <tr height="*" valign="top">
                                <td><table width="98%" cellspacing="0" cellpadding="0">
    <tr>
        <td align="left" >
            <span class="Head"><%=this.Title%></span>
        </td>
        <td align="right">
			<a class="commandButton" href="<%=www.strive3d.net.Utils.ApplicationPath%>/DesktopDefault.aspx?<%=www.strive3d.net.Utils.TabHref%>">Back</a>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <hr noshade size="1">
        </td>
    </tr>
</table>								
	</td>
                            </tr>
							<tr>
								<td>
