using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using www.strive3d.net.Game;

namespace www.strive3d.net.players.Controls
{
	/// <summary>
	/// Summary description for LoggedInArea.
	/// </summary>
	[DefaultProperty("Text"), 
		ToolboxData("<{0}:LoggedInArea runat=server></{0}:LoggedInArea>")]
	public class LoggedInArea : System.Web.UI.WebControls.WebControl
	{
		/// <summary> 
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void Render(HtmlTextWriter output)
		{
			if(!thisterminal.Web.Authentication.Basic.LogonCurrentUser(new PlayerAuthenticator(), "www.strive3d.net"))
			{
				throw new thisterminal.Web.Authentication.AuthenticationException("You must be logged in to view this resource.");
			}
			else
			{
				base.Render(output);
			}
		}
	}
}
