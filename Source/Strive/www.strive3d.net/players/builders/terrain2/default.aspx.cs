using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using www.strive3d.net.Game;


namespace www.strive3d.net.players.builders.terrain2
{
	/// <summary>
	/// Summary description for _default.
	/// </summary>
	public class _default : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.TextBox TextBox1;
		protected System.Web.UI.WebControls.TextBox TextBox2;
		protected System.Web.UI.WebControls.Button Redraw;
		protected DataTable squares;


		private void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				TextBox1.Text = "1000";
				TextBox2.Text = "1000";
			}
			CommandFactory cmd = new CommandFactory();
			try
			{

				SqlCommand squaresLoader = cmd.TerrainSquareDetails(int.Parse(TextBox2.Text), int.Parse(TextBox1.Text));
				SqlDataAdapter squaresFiller = new SqlDataAdapter(squaresLoader);
				squares = new DataTable("Squares");
				squaresFiller.Fill(squares);
			}
			catch(Exception c)
			{
				throw c;
			}
			finally
			{
				cmd.Close();
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
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Redraw.Click += new System.EventHandler(this.Redraw_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void Redraw_Click(object sender, System.EventArgs e)
		{
		
		}
	}
}
