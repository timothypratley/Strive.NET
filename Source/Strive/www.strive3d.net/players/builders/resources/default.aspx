<%@ Page language="c#" Codebehind="default.aspx.cs" AutoEventWireup="false" Inherits="www.strive3d.net.players.builders.resources._default" %>
<%@ Register TagPrefix="Controls" TagName="Header" Src="~/players/Controls/Header.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Footer" Src="~/players/Controls/Footer.ascx" %>
<%@ Import Namespace="System.Data" %>
<Controls:Header runat="Server" title="Resources - List View" ID="Header1"/>
<strong>Filter By: <%
	foreach(DataRow d in resourceTypes.Rows)
	{
	%><a href="./default.aspx?EnumResourceTypeID=<%=d["EnumResourceTypeID"]%>">[<%=d["EnumResourceTypeName"]%>]</a> <%
	}
	%> <a href="./default.aspx">[All]</a></strong><br /><br />
<asp:DataList id=DataList1 runat="server">
<HeaderTemplate>
<table>
<tr>
	<th>ID</th>
	<th>Type</th>
	<th>Pak</th>
	<th>Name</th>
	<th>Description</th>
	<th colspan="2"><%
	foreach(DataRow d in resourceTypes.Rows)
	{
	%><a href="./addresource.aspx?EnumResourceTypeID=<%=d["EnumResourceTypeID"]%>">[Add <%=d["EnumResourceTypeName"]%>]</a> <%
	}
	%>
	
	</th>

</tr>

</HeaderTemplate>
<ItemTemplate>
<tr>
<td><%# DataBinder.Eval(Container.DataItem, "ResourceID") %></td>
<td><%# DataBinder.Eval(Container.DataItem, "EnumResourceTypeName") %></td>
<td><%# DataBinder.Eval(Container.DataItem, "ResourcePak") %></td>
<td><%# DataBinder.Eval(Container.DataItem, "ResourceName") %></td>
<td><%# DataBinder.Eval(Container.DataItem, "Description") %></td>
<td><a href="./<%# DataBinder.Eval(Container.DataItem, "EnumResourceTypeName") %>/<%# DataBinder.Eval(Container.DataItem, "ResourceID") %><%# DataBinder.Eval(Container.DataItem, "ResourceFileExtension") %>"><img border="0" height="20" width="20" src="./<%# DataBinder.Eval(Container.DataItem, "EnumResourceTypeName") %>/<%# DataBinder.Eval(Container.DataItem, "ResourceID") %><%# DataBinder.Eval(Container.DataItem, "ResourceFileExtension") %>"/></a></td>
<td><a href="deleteresource.aspx?ResourceID=<%# DataBinder.Eval(Container.DataItem, "ResourceID") %>">[Delete]</a></td>

</tr>
</ItemTemplate>
<FooterTemplate>
</table>
</FooterTemplate>
</asp:DataList>

<Controls:Footer runat="server" ID="Footer1" NAME="Footer1"/>
