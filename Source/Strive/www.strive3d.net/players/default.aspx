<%@ Page language="c#" %>
<%@ Register TagPrefix="Controls" TagName="Header" Src="~/players/Controls/Header.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Footer" Src="~/players/Controls/Footer.ascx" %>
<Controls:Header runat="Server" title="Home" />

  <ul>
	<li><strong>Player Management</strong>
	<ul>
		<li><a href="./signup.aspx">Create A New Account [Signup]</a>
			<ul>
				<li>Follow this link to Create a new account</li>
			</ul>
		</li>
		<li><a href="./pingemail.aspx">Retrieve your password [Ping Me]</a>
			<ul>
				<li>Follow this link to retrieve a lost password</li>
			</ul>
		</li>			
	</ul>
	</li>
	<li><strong>Strive3d.net Home</strong></li>
	<ul>
		<li><a href="../">www.strive3d.net</a></li>
			<ul>
				<li>Visit the Strive3d.net home</li>
			</ul>
	</ul>
  </ul>

<Controls:Footer runat="Server"/>