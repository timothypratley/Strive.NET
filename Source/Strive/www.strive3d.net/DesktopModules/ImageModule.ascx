<%@ Control language="c#" Inherits="www.strive3d.net.ImageModule" CodeBehind="ImageModule.ascx.cs" AutoEventWireup="false" %>
<%@ Register TagPrefix="Portal" TagName="Title" Src="~/DesktopModuleTitle.ascx"%>

<portal:title EditText="Edit" EditUrl="~/DesktopModules/EditImage.aspx" runat="server" id=Title1 />

<asp:image id="Image1" border="0" runat="server" />
<br>
