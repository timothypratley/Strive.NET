<%@ Control language="c#" Inherits="www.strive3d.net.Contacts" CodeBehind="Contacts.ascx.cs" AutoEventWireup="false" %>
<%@ Register TagPrefix="Portal" TagName="Title" Src="~/DesktopModuleTitle.ascx"%>

<portal:title EditText="Add New Contact" EditUrl="~/DesktopModules/EditContacts.aspx" runat="server" id=Title1 />
<asp:datagrid id="myDataGrid" Border="0" width="100%" AutoGenerateColumns="false" EnableViewState="false" runat="server">
    <Columns>
        <asp:TemplateColumn>
            <ItemTemplate>
                <asp:HyperLink ImageUrl="~/images/edit.gif" NavigateUrl='<%# "~/DesktopModules/EditContacts.aspx?ItemID=" + DataBinder.Eval(Container.DataItem,"ItemID") + "&mid=" + ModuleId %>' Visible="<%# IsEditable %>" runat="server" />
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
			<ItemTemplate>
				<asp:Label CssClass="NormalBold" Runat="server"><%#DataBinder.Eval(Container.DataItem, "Name")%></asp:Label><br />
				<asp:Label CssClass="Normal" Runat="server"><%#DataBinder.Eval(Container.DataItem, "Role")%></asp:Label><br />
				<a href="mailto:<%#DataBinder.Eval(Container.DataItem, "Email")%>"><asp:Label CssClass="Normal" Runat="server"><%#DataBinder.Eval(Container.DataItem, "Email")%></asp:Label></a><br />
			</ItemTemplate>
        </asp:TemplateColumn>
    </Columns>
</asp:datagrid>
