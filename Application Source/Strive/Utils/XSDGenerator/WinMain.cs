using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;
using SQLDMO;
using Strive.Utils.Shared;
using Strive.Utils.Shared.Controls;

namespace Strive.Utils.XSDGenerator
{
	/// <summary>
	/// Summary description for WinMain.
	/// </summary>
	public class WinMain : System.Windows.Forms.Form
	{
		private Strive.Utils.Shared.Controls.DBPicker dbPicker1;

		private SQLDMO.Application SQLDMOApplication;
		private SQLDMO.Database SQLDMODatabase;
		private SQLDMO.SQLServer SQLDMOServer;
		private string ConnectionString;
		private System.Windows.Forms.TextBox XSDResults;
		private System.Windows.Forms.Button GenerateXSD;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public WinMain()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.dbPicker1 = new Strive.Utils.Shared.Controls.DBPicker();
			this.XSDResults = new System.Windows.Forms.TextBox();
			this.GenerateXSD = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// dbPicker1
			// 
			this.dbPicker1.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.dbPicker1.Name = "dbPicker1";
			this.dbPicker1.Size = new System.Drawing.Size(640, 176);
			this.dbPicker1.TabIndex = 0;
			this.dbPicker1.DatabaseSelected += new Strive.Utils.Shared.Controls.DatabaseSelectedEventHandler(this.dbPicker1_DatabaseSelected);
			// 
			// XSDResults
			// 
			this.XSDResults.Location = new System.Drawing.Point(8, 176);
			this.XSDResults.Multiline = true;
			this.XSDResults.Name = "XSDResults";
			this.XSDResults.Size = new System.Drawing.Size(616, 360);
			this.XSDResults.TabIndex = 2;
			this.XSDResults.Text = "";
			// 
			// GenerateXSD
			// 
			this.GenerateXSD.Enabled = false;
			this.GenerateXSD.Location = new System.Drawing.Point(536, 544);
			this.GenerateXSD.Name = "GenerateXSD";
			this.GenerateXSD.Size = new System.Drawing.Size(88, 23);
			this.GenerateXSD.TabIndex = 3;
			this.GenerateXSD.Text = "Generate XSD";
			this.GenerateXSD.Click += new System.EventHandler(this.GenerateXSD_Click);
			// 
			// WinMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(632, 574);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.GenerateXSD,
																		  this.XSDResults,
																		  this.dbPicker1});
			this.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Name = "WinMain";
			this.Text = "WinMain";
			this.ResumeLayout(false);

		}
		#endregion

		private void dbPicker1_DatabaseSelected(Strive.Utils.Shared.Controls.DBPicker sender, Strive.Utils.Shared.Controls.DBPicker.DatabaseSelectedEventArgs e)
		{
			this.SQLDMOApplication = e.Application;
			this.SQLDMODatabase = e.Database;
			this.SQLDMOServer = e.Server;
			this.ConnectionString = e.ConnectionString;
			

			GenerateXSD.Enabled = true;
		}

		private void GenerateXSD_Click(object sender, System.EventArgs e)
		{
			
			XSDResults.Text = API.GenerateSchemaFromTables(SQLDMODatabase.Tables, ConnectionString);
		}

	}
}
