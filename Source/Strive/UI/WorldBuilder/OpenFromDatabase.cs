using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Strive.UI.WorldBuilder
{
	/// <summary>
	/// Summary description for OpenFromDatabase.
	/// </summary>
	public class OpenFromDatabase : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;

		private DBPicker dbPicker1;
		public string connectionString;


		public OpenFromDatabase()
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
			this.dbPicker1 = new Strive.UI.WorldBuilder.DBPicker();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// dbPicker1
			// 
			this.dbPicker1.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.dbPicker1.Name = "dbPicker1";
			this.dbPicker1.Size = new System.Drawing.Size(640, 176);
			this.dbPicker1.TabIndex = 0;
			this.dbPicker1.DatabaseSelected += new Strive.UI.WorldBuilder.DatabaseSelectedEventHandler(this.dbPicker1_DatabaseSelected);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(208, 184);
			this.button1.Name = "button1";
			this.button1.TabIndex = 1;
			this.button1.Text = "Ok";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(352, 184);
			this.button2.Name = "button2";
			this.button2.TabIndex = 2;
			this.button2.Text = "Cancel";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// OpenFromDatabase
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(640, 221);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.button2,
																		  this.button1,
																		  this.dbPicker1});
			this.Name = "OpenFromDatabase";
			this.Text = "OpenFromDatabase";
			this.ResumeLayout(false);

		}
		#endregion

		private void dbPicker1_DatabaseSelected(DBPicker sender, DBPicker.DatabaseSelectedEventArgs e) {
			connectionString = e.ConnectionString;
		}

		private void button2_Click(object sender, System.EventArgs e) {
			connectionString = null;
			Close();
		}

		private void button1_Click(object sender, System.EventArgs e) {
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}
