using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using www.strive3d.net.Game;
using thisterminal.Web;

namespace www.strive3d.net.players.builders.objects
{
	/// <summary>
	/// Summary description for GeneralGenericSpecialiser.
	/// </summary>
	public class GeneralGenericSpecialiser : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e) {
			GetTables();
			if ( DBTableDropDown.SelectedIndex != 0 ) {
				//display column schema
				TableSchemaDataGrid.DataSource = GetDatabaseSchema(DBTableDropDown.SelectedItem.Value);
				TableSchemaDataGrid.DataBind();

				//display the data
				TableDataGrid.DataSource = GetTableData(DBTableDropDown.SelectedItem.Value);
				TableDataGrid.DataBind();
			}
		}
		protected System.Web.UI.WebControls.DataGrid TableSchemaDataGrid;
		protected System.Web.UI.WebControls.DataGrid TableDataGrid;
		protected System.Web.UI.WebControls.DropDownList DBTableDropDown;
		protected System.Web.UI.WebControls.DropDownList DropDownList1;
		protected System.Web.UI.WebControls.DataGrid DataGrid1;
		protected System.Web.UI.WebControls.DataGrid DataGrid2;
		protected System.Web.UI.WebControls.PlaceHolder PlaceHolder2;
		protected System.Web.UI.WebControls.PlaceHolder PlaceHolder1;

		private void GetTables() {
			SqlConnection myConnection = new CommandFactory().Connection;
			string SQL = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' ORDER BY TABLE_NAME";
			SqlDataAdapter schemaDA = new SqlDataAdapter(SQL, myConnection);

			DataTable schemaTable = new DataTable();

			schemaDA.Fill(schemaTable);

			int si = DBTableDropDown.SelectedIndex;
			DBTableDropDown.DataSource=schemaTable;
			DBTableDropDown.DataTextField = "TABLE_NAME";
			DBTableDropDown.DataValueField = "TABLE_NAME";
			DBTableDropDown.DataBind();
			DBTableDropDown.Items.Insert(0, new ListItem("Select a Table")); 
			DBTableDropDown.SelectedIndex = si;
		}

		public void DBTableDropDown_Changed(Object sender, EventArgs e) {
			// TODO: this is zanny, I don't really want an event, just a postback?
		}

		string glDBTable;
		private DataView GetDatabaseSchema(string DBTable) {
			glDBTable = DBTable;
			//gets the database column schema.
			SqlConnection myConnection = new CommandFactory().Connection;
			string SQL = "SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '"+DBTable+"'";
			SqlDataAdapter myCommand = new SqlDataAdapter(SQL, myConnection);
			DataSet ds = new DataSet();
			myCommand.Fill(ds, DBTable);

			// TODO: even with clear there is non-deterministic behaviour
			PlaceHolder1.Controls.Clear();

			foreach ( DataRow r in ds.Tables[DBTable].Rows ) {
				string data_type = (string)r["DATA_TYPE"];
				if ( data_type == "int" ) {
					TextBox tb = new TextBox();
					tb.Text = "int";
					tb.ID = (string)r["COLUMN_NAME"];
					PlaceHolder1.Controls.Add( tb );
					PlaceHolder1.Controls.Add( new LiteralControl( "<br>" ) );
				} else if ( data_type == "char" ) {
					TextBox tb = new TextBox();
					tb.Text = "char";
					tb.ID = (string)r["COLUMN_NAME"];
					PlaceHolder1.Controls.Add( tb );
					PlaceHolder1.Controls.Add( new LiteralControl( "<br>" ) );
				} else if ( data_type == "float" ) {
					TextBox tb = new TextBox();
					tb.Text = "float";
					tb.ID = (string)r["COLUMN_NAME"];
					PlaceHolder1.Controls.Add( tb );
					PlaceHolder1.Controls.Add( new LiteralControl( "<br>" ) );
				} else if ( data_type == "varchar" ) {
					TextBox tb = new TextBox();
					tb.Text = "varchar";
					tb.ID = (string)r["COLUMN_NAME"];
					PlaceHolder1.Controls.Add( tb );
					PlaceHolder1.Controls.Add( new LiteralControl( "<br>" ) );
				} else if ( data_type == "datetime" ) {
					TextBox tb = new TextBox();
					tb.Text = "datetime";
					tb.ID = (string)r["COLUMN_NAME"];
					PlaceHolder1.Controls.Add( tb );
					PlaceHolder1.Controls.Add( new LiteralControl( "<br>" ) );
				} else {
					TextBox tb = new TextBox();
					tb.Text = "Unknown DATA_TYPE " + data_type;
					tb.ID = (string)r["COLUMN_NAME"];
					PlaceHolder1.Controls.Add( tb );
					PlaceHolder1.Controls.Add( new LiteralControl( "<br>" ) );
				}
			}

			Button b = new Button();
			b.Text = "Update";
			b.Click += new System.EventHandler(this.Update);
			PlaceHolder1.Controls.Add( b );

			return ds.Tables[DBTable].DefaultView;
		}

		private void Update(Object sender, EventArgs e) {
			SqlConnection myConnection = new CommandFactory().Connection;
			string SQL = "UPDATE " + glDBTable + " SET ";
			string where_clause = "";
			int i = 2;
			foreach ( Control c in PlaceHolder1.Controls ) {
				i++;
				TextBox tb = c as TextBox;
				if ( tb == null ) continue;
				if ( tb.ID == glDBTable+"ID" ) {
					where_clause = " WHERE " + glDBTable + "ID = " + tb.Text;
					continue;
				}
				SQL += tb.ID + "=";
				try {
					int.Parse( tb.Text );
					SQL += tb.Text;
				} catch ( Exception ) {
					SQL += "'" + tb.Text + "'";
				}
				if ( i >= PlaceHolder1.Controls.Count ) break;
				else SQL += ", ";
			}
			SQL += where_clause;
			myConnection.Open();
			SqlCommand sc = new SqlCommand( SQL, myConnection );
			sc.ExecuteNonQuery();
		}

		private DataView GetTableData(string DBTable) {
			//Gets all the data from the table
			SqlConnection myConnection = new CommandFactory().Connection;
			string SQL = "SELECT * FROM " + DBTable;
			SqlDataAdapter myCommand = new SqlDataAdapter(SQL, myConnection);
			DataSet ds = new DataSet();
			myCommand.Fill(ds, DBTable);
			return ds.Tables[DBTable].DefaultView;
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)	{
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
		private void InitializeComponent() {    
			this.ID = "Form1";
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
