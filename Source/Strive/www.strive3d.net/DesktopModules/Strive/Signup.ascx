<%@ Register TagPrefix="Portal" TagName="Title" Src="~/DesktopModuleTitle.ascx"%>
<%@ Control Language="c#" AutoEventWireup="false" Codebehind="Signup.ascx.cs" Inherits="www.strive3d.net.Signup" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%
if(!www.strive3d.net.PortalSecurity.IsInRoles("Players"))
{
%>
<portal:title runat="server" ID="Title1" NAME="Title1"/>

<asp:Panel id=signupform runat="server">
<TABLE>
  <TR>
    <TD class=Normal>E-mail Address</TD>
    <TD><asp:TextBox id=PlayerEmail runat="server" CssClass="NormalTextBox"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator id=RequiredFieldValidator1 runat="server" CssClass="InputValidation" EnableClientScript="False" ControlToValidate="PlayerEmail" ErrorMessage="You must enter a valid e-mail address."></asp:RequiredFieldValidator><asp:RegularExpressionValidator id=RegularExpressionValidator1 runat="server" EnableClientScript="False" ControlToValidate="PlayerEmail" ErrorMessage="You must enter a valid e-mail address." ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator></TD></TR>
  <TR>
    <TD class=Normal>Password</TD>
    <TD><asp:TextBox id=PlayerPassword runat="server" CssClass="NormalTextBox" TextMode="Password"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator id=RequiredFieldValidator2 runat="server" CssClass="InputValidation" EnableClientScript="False" ControlToValidate="PlayerPassword" ErrorMessage="You must specify a password for your new account."></asp:RequiredFieldValidator></TD></TR>
  <TR>
    <TD colSpan=2><asp:Button id=Button1 CssClass="NormalButton" Runat="server" Text="Create Your Account"></asp:Button></TD></TR></TABLE>
</asp:Panel>  
<asp:Panel ID="signupsuccess" Runat="server">
<P align=left>Thanks for signing up. You will receive an e-mail with 
instructions to activate your account.</P>

</asp:Panel>
<%
}
%>