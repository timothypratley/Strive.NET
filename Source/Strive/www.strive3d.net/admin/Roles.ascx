<%@ Control Inherits="www.strive3d.net.Roles" CodeBehind="Roles.ascx.cs" Language="c#" AutoEventWireup="false" %>
<%@ Register TagPrefix="Portal" TagName="Title" Src="~/DesktopModuleTitle.ascx"%>

<portal:title runat="server" id="Title1" />

<table cellpadding="2" cellspacing="0" border="0">
    <tr valign="top">
        <td class="Normal" width="100">
            &nbsp;
        </td>
        <td>
            <asp:DataList id="rolesList" DataKeyField="RoleID" runat="server">
                <ItemTemplate>
                    <asp:ImageButton ImageUrl="~/images/edit.gif" CommandName="edit" AlternateText="Edit this item" runat="server" />
                    <asp:ImageButton ImageUrl="~/images/delete.gif" CommandName="delete" AlternateText="Delete this item" runat="server" />
                    &nbsp;&nbsp;
                    <asp:Label Text='<%# DataBinder.Eval(Container.DataItem, "RoleName") %>' cssclass="Normal" runat="server" />
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:Textbox id="roleName" width="200" cssclass="NormalTextBox" Text='<%# DataBinder.Eval(Container.DataItem, "RoleName") %>' runat="server" />
                    &nbsp;
                    <asp:LinkButton Text="Apply" CommandName="apply" cssclass="CommandButton" runat="server" />
                    &nbsp;
                    <asp:LinkButton Text="Change Role Members" CommandName="members" cssclass="CommandButton" runat="server" />
                </EditItemTemplate>
            </asp:DataList>
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;
        </td>
        <td>
            <asp:LinkButton cssclass="CommandButton" Text="Add New Role" runat="server" id="AddRoleBtn">
                Add New Role</asp:LinkButton>
        </td>
    </tr>
</table>
