<%@ Register TagPrefix="Controls" TagName="Header" Src="~/players/Controls/Header.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Footer" Src="~/players/Controls/Footer.ascx" %>
<%@ Import Namespace="System.Data" %>
<%@ Page language="c#" Codebehind="default.aspx.cs" AutoEventWireup="false" Inherits="www.strive3d.net.players.builders.textures._default" %>
<Controls:Header runat="Server" title="Textures - List View" ID="Header1"/>
<asp:DataList id=DataList1 runat="server">
<HeaderTemplate>
<table>
<tr>
	<th>Model ID</th>
	<th>Model Name</th>
	<th>Model Description</th>
	<th><a href="./addtexture.aspx">[Add]</a></th>

</tr>

</HeaderTemplate>
<ItemTemplate>
<tr>
<td><%# DataBinder.Eval(Container.DataItem, "ModelID") %></td>
<td><%# DataBinder.Eval(Container.DataItem, "ModelName") %></td>
<td><%# DataBinder.Eval(Container.DataItem, "Description") %></td>
<td><a href="..<%=System.Configuration.ConfigurationSettings.AppSettings["resourcepath"]%>/textures/<%# DataBinder.Eval(Container.DataItem, "ModelID") %>.bmp"><img border="0" height="20" width="20" src="..<%=System.Configuration.ConfigurationSettings.AppSettings["resourcepath"]%>/textures/<%# DataBinder.Eval(Container.DataItem, "ModelID") %>.bmp"/></a></td>
<td><a href="deletetexture.aspx?ModelID=<%# DataBinder.Eval(Container.DataItem, "ModelID") %>">[Delete]</a></td>

</tr>
</ItemTemplate>
<FooterTemplate>
</table>
</FooterTemplate>
</asp:DataList>

<Controls:Footer runat="server" />
