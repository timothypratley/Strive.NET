<%@ Control Inherits="www.strive3d.net.ModuleDefs" CodeBehind="ModuleDefs.ascx.cs" Language="c#" AutoEventWireup="false" %>
<%@ Register TagPrefix="Portal" TagName="Title" Src="~/DesktopModuleTitle.ascx"%>

<portal:title runat="server" id="Title1" />
<table cellpadding="2" cellspacing="0" border="0">
    <tr valign="top">
        <td>
            <asp:DataList id="defsList" DataKeyField="ModuleDefID" runat="server">
                <ItemTemplate>
                    <asp:ImageButton ImageUrl="~/images/edit.gif" AlternateText="Edit this item" runat="server" />
                    &nbsp;&nbsp;
                    <asp:Label Text='<%# DataBinder.Eval(Container.DataItem, "FriendlyName") %>' CssClass="Normal" runat="server" />
                </ItemTemplate>
            </asp:DataList>
        </td>
    </tr>
    <tr>
        <td>
            <asp:LinkButton cssclass="CommandButton" Text="Add New Module Type" runat="server" id="AddDefBtn" />
        </td>
    </tr>
</table>
