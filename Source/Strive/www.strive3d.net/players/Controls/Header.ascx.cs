namespace www.strive3d.net.players.Controls
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Summary description for Header.
	/// </summary>
	public abstract class Header : System.Web.UI.UserControl
	{

		private string _title;

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
		}

		public string Title
		{
			get
			{
				if(System.Configuration.ConfigurationSettings.AppSettings["site"] != null)
				{
					return	System.Configuration.ConfigurationSettings.AppSettings["site"] + " - " + _title;
				}
				else
				{
					return _title;
				}
			}
			set
			{
				_title = value;
			}
		}



		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	}
}
