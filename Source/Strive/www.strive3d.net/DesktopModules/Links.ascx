<%@ Control language="c#" Inherits="www.strive3d.net.Links" CodeBehind="Links.ascx.cs" AutoEventWireup="false" %>
<%@ Register TagPrefix="Portal" TagName="Title" Src="~/DesktopModuleTitle.ascx"%>

<portal:title EditUrl="~/DesktopModules/EditLinks.aspx" EditText="Add Link" runat="server" id=Title1 />

<asp:DataList id="myDataList" CellPadding="4" Width="100%" runat="server">
    <ItemTemplate>
        <span class="Normal">
            <asp:HyperLink id="editLink" ImageUrl="<%# linkImage %>" NavigateUrl='<%# "~/DesktopModules/EditLinks.aspx?ItemID=" + DataBinder.Eval(Container.DataItem,"ItemID") + "&mid=" + ModuleId %>' runat="server" />
            <asp:HyperLink Text='<%# DataBinder.Eval(Container.DataItem,"Title") %>' NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"Url") %>' ToolTip='<%# DataBinder.Eval(Container.DataItem,"Description") %>' runat="server" />
        </span>
        <br>
    </ItemTemplate>
</asp:DataList>
