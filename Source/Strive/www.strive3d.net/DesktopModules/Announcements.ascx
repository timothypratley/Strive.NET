<%@ Control Language="c#" Inherits="www.strive3d.net.Announcements" CodeBehind="Announcements.ascx.cs" AutoEventWireup="false" %>
<%@ Register TagPrefix="Portal" TagName="Title" Src="~/DesktopModuleTitle.ascx"%>

<portal:title EditText="Add New Announcement" EditUrl="~/DesktopModules/EditAnnouncements.aspx" runat="server" id=Title1 />
<asp:DataList id="myDataList" CellPadding="4" Width="98%" EnableViewState="false" runat="server">
    <ItemTemplate>
        <asp:HyperLink id="editLink" ImageUrl="~/images/edit.gif" NavigateUrl='<%# "~/DesktopModules/EditAnnouncements.aspx?ItemID=" + DataBinder.Eval(Container.DataItem,"ItemID") + "&mid=" + ModuleId %>' Visible="<%# IsEditable %>" runat="server" />
        <span class="ItemTitle">
            <%# DataBinder.Eval(Container.DataItem,"Title") %>
        </span>
        <br>
        <span class="Normal">
            <%# DataBinder.Eval(Container.DataItem,"Description") %>
            &nbsp;
            <asp:HyperLink id="moreLink" NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"MoreLink") %>' Visible='<%# DataBinder.Eval(Container.DataItem,"MoreLink") != String.Empty %>' runat="server">
                read more...</asp:HyperLink>
        </span>
        <br>
        <span class="Normal">
			<strong><%# DataBinder.Eval(Container.DataItem, "CreatedByUser")%></strong>
        </span>
        <hr noshade="noshade" size="1" />
    </ItemTemplate>
</asp:DataList>
