<%@ Control language="c#" Inherits="www.strive3d.net.Events" CodeBehind="Events.ascx.cs" AutoEventWireup="false" %>
<%@ Register TagPrefix="Portal" TagName="Title" Src="~/DesktopModuleTitle.ascx"%>

<portal:title EditText="Add New Event" EditUrl="~/DesktopModules/EditEvents.aspx" runat="server" id=Title1 />

<asp:DataList id="myDataList" CellPadding="4" Width="98%" EnableViewState="false" runat="server">
    <ItemTemplate>
        <span class="ItemTitle">
            <asp:HyperLink id="editLink" ImageUrl="~/images/edit.gif" NavigateUrl='<%# "~/DesktopModules/EditEvents.aspx?ItemID=" + DataBinder.Eval(Container.DataItem,"ItemID") + "&mid=" + ModuleId %>' Visible="<%# IsEditable %>" runat="server" />
            <asp:Label Text='<%# DataBinder.Eval(Container.DataItem,"Title") %>' runat="server" />
        </span>
        <br>
        <span class="Normal"><i>
                <%# DataBinder.Eval(Container.DataItem,"WhereWhen") %>
            </i></span>
        <br>
        <span class="Normal">
            <%# DataBinder.Eval(Container.DataItem,"Description") %>
        </span>
        <br>
    </ItemTemplate>
</asp:DataList>
