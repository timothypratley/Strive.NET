using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Strive.Utils.Shared.Controls;
using SQLDMO;

namespace Strive.Utils.StoredProcedureUI
{
	/// <summary>
	/// Summary description for WinMain.
	/// </summary>
	public class WinMain : System.Windows.Forms.Form
	{
		private Strive.Utils.Shared.Controls.DBPicker dbPicker1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ListBox StoredProcedures;
		private SQLDMO.Database SQLDMODatabase;
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
			dbPicker1.DatabaseSelected+=new DatabaseSelectedEventHandler(DBPicker_DatabaseSelected);
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

		protected void DBPicker_DatabaseSelected(object sender, DBPicker.DatabaseSelectedEventArgs e)
		{
			SQLDMODatabase = e.Database;
			PopulateWithStoredProcedures(StoredProcedures);
			groupBox1.Enabled = true;
		}

		private void PopulateWithStoredProcedures(ListBox list)
		{
			list.Items.Clear();

			foreach(SQLDMO.StoredProcedure s in SQLDMODatabase.StoredProcedures)
			{
				if(s.Type == SQLDMO.SQLDMO_PROCEDURE_TYPE.SQLDMOProc_Standard &&
					s.SystemObject == false)
				{
					StoredProcedures.Items.Add(s.Name);
				}
			}
			if(StoredProcedures.Items.Count == 0)
			{
				MessageBox.Show(this, "No stored procedures in selected database.");
			}
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
			this.StoredProcedures = new System.Windows.Forms.ListBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// dbPicker1
			// 
			this.dbPicker1.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.dbPicker1.Name = "dbPicker1";
			this.dbPicker1.Size = new System.Drawing.Size(640, 176);
			this.dbPicker1.TabIndex = 0;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.StoredProcedures});
			this.groupBox1.Enabled = false;
			this.groupBox1.Location = new System.Drawing.Point(8, 176);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(624, 376);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Select a Stored Procedure";
			// 
			// StoredProcedures
			// 
			this.StoredProcedures.ItemHeight = 16;
			this.StoredProcedures.Location = new System.Drawing.Point(8, 24);
			this.StoredProcedures.MultiColumn = true;
			this.StoredProcedures.Name = "StoredProcedures";
			this.StoredProcedures.Size = new System.Drawing.Size(608, 340);
			this.StoredProcedures.TabIndex = 0;
			this.StoredProcedures.DoubleClick += new System.EventHandler(this.StoredProcedureDoubleClicked);
			// 
			// WinMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(632, 573);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.groupBox1,
																		  this.dbPicker1});
			this.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Name = "WinMain";
			this.Text = "WinMain";
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void StoredProcedureDoubleClicked(object sender, System.EventArgs e)
		{
			if(StoredProcedures.SelectedIndex >= 0)
			{
				if(getSingleStoredProcedure(StoredProcedures.Text) == null)
				{
					MessageBox.Show(this, "Could not locate procedure '" + StoredProcedures.Text);
					return;
				}
				else
				{
					SPUI spui = new SPUI(getSingleStoredProcedure(StoredProcedures.Text));
					spui.Show();
				}
			}
		}

		private StoredProcedure getSingleStoredProcedure(string name)
		{
			foreach(SQLDMO.StoredProcedure s in SQLDMODatabase.StoredProcedures)
			{
				if(s.Name == name)
				{
					return s;
				}
			}
			return null;
		}
	}
}
