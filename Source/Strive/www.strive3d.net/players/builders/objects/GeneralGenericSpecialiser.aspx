<%@ Page language="c#" Codebehind="GeneralGenericSpecialiser.aspx.cs" AutoEventWireup="false" Inherits="www.strive3d.net.players.builders.objects.GeneralGenericSpecialiser" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>GeneralGenericSpecialiser</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<asp:DropDownList id="DBTableDropDown" runat="server" AutoPostBack="True"></asp:DropDownList>
			<asp:DataGrid id="TableSchemaDataGrid" runat="server"></asp:DataGrid>
			<asp:DataGrid id="TableDataGrid" runat="server"></asp:DataGrid>
			<asp:PlaceHolder id="PlaceHolder1" runat="server"></asp:PlaceHolder>
		</form>
	</body>
</HTML>
