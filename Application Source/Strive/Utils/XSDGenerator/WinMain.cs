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
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button GenerateXSD;

		private SQLDMO.Application SQLDMOApplication;
		private SQLDMO.Database SQLDMODatabase;
		private SQLDMO.SQLServer SQLDMOServer;
		private string ConnectionString;
		
		private System.Windows.Forms.ListBox TablesList;
		private System.Windows.Forms.TextBox XSDResults;

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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.TablesList = new System.Windows.Forms.ListBox();
			this.GenerateXSD = new System.Windows.Forms.Button();
			this.XSDResults = new System.Windows.Forms.TextBox();
			this.groupBox1.SuspendLayout();
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
			// groupBox1
			// 
			this.groupBox1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.TablesList,
																					this.GenerateXSD});
			this.groupBox1.Location = new System.Drawing.Point(8, 176);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(280, 360);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Select the Root table";
			// 
			// TablesList
			// 
			this.TablesList.Location = new System.Drawing.Point(8, 16);
			this.TablesList.Name = "TablesList";
			this.TablesList.Size = new System.Drawing.Size(256, 303);
			this.TablesList.TabIndex = 1;
			// 
			// GenerateXSD
			// 
			this.GenerateXSD.Location = new System.Drawing.Point(176, 328);
			this.GenerateXSD.Name = "GenerateXSD";
			this.GenerateXSD.Size = new System.Drawing.Size(88, 23);
			this.GenerateXSD.TabIndex = 0;
			this.GenerateXSD.Text = "Generate XSD";
			this.GenerateXSD.Click += new System.EventHandler(this.GenerateXSD_Click);
			// 
			// XSDResults
			// 
			this.XSDResults.Location = new System.Drawing.Point(296, 176);
			this.XSDResults.Multiline = true;
			this.XSDResults.Name = "XSDResults";
			this.XSDResults.Size = new System.Drawing.Size(328, 360);
			this.XSDResults.TabIndex = 2;
			this.XSDResults.Text = "";
			// 
			// WinMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(632, 542);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.XSDResults,
																		  this.groupBox1,
																		  this.dbPicker1});
			this.Name = "WinMain";
			this.Text = "WinMain";
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void dbPicker1_DatabaseSelected(Strive.Utils.Shared.Controls.DBPicker sender, Strive.Utils.Shared.Controls.DBPicker.DatabaseSelectedEventArgs e)
		{
			this.SQLDMOApplication = e.Application;
			this.SQLDMODatabase = e.Database;
			this.SQLDMOServer = e.Server;
			this.ConnectionString = e.ConnectionString;
			

			RepopulateTables();
		}

		private void RepopulateTables()
		{
			TablesList.Items.Clear();
			foreach(Table t in SQLDMODatabase.Tables)
			{
				if(!t.SystemObject)
				{
					TablesList.Items.Add(t.Name);
				}
			}
		}

		private void GenerateXSD_Click(object sender, System.EventArgs e)
		{
			// get the table:
			if(TablesList.SelectedIndex < 0)
			{
				MessageBox.Show(this, "You must select a table.");
				return;
			}
			
			Table rootTable = null;

			foreach(Table t in SQLDMODatabase.Tables)
			{
				if(t.Name == TablesList.Text)
				{
					rootTable = t;
				}
			}

			if(rootTable == null)
			{
				MessageBox.Show(this, "Could not locate table '" + TablesList.Text);
				return;
			}


			StringBuilder sb = new StringBuilder();
			StringWriter s = new StringWriter(sb);
			
			
			XSDResults.Text = API.GenerateSchemaFromTableCollection(rootTable, ConnectionString);
			
		}
	}
}
