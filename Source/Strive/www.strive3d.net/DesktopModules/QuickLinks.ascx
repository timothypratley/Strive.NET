<%@ Control language="c#" Inherits="www.strive3d.net.QuickLinks" CodeBehind="QuickLinks.ascx.cs" AutoEventWireup="false" %>
<%@ Register TagPrefix="Portal" TagName="Title" Src="~/DesktopModuleTitle.ascx"%>
<portal:title runat="server"/>
<asp:hyperlink id="EditButton" cssclass="CommandButton" EnableViewState="false" runat="server" />

<asp:DataList id="myDataList" CellPadding="4" Width="100%" EnableViewState="false" runat="server">
    <ItemTemplate>
        <span class="Normal">
            <asp:HyperLink id="editLink" ImageUrl="<%# linkImage %>" NavigateUrl='<%# "~/DesktopModules/EditLinks.aspx?ItemID=" + DataBinder.Eval(Container.DataItem,"ItemID") + "&mid=" + ModuleId %>' runat="server" />
            <a href='<%# DataBinder.Eval(Container.DataItem,"Url") %>'>
                <%# DataBinder.Eval(Container.DataItem,"Title") %>
            </a></span>
        <br>
    </ItemTemplate>
</asp:DataList>

<br>
