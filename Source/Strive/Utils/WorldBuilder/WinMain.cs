using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Strive.Data;
using Strive.Multiverse;

namespace Strive.Utils.WorldBuilder
{
	/// <summary>
	/// Summary description for WinMain.
	/// </summary>
	public class WinMain : System.Windows.Forms.Form
	{
		private System.Windows.Forms.DataGrid World;
		private Schema Multiverse = new Schema();
		//private Schema Multiverse = MultiverseFactory.getMultiverseFromFile( "world.xml" );
		//private Schema Multiverse = MultiverseFactory.getMultiverseFromDatabase(
				//System.Configuration.ConfigurationSettings.AppSettings["databaseConnectionString"]
		//);
		private System.Windows.Forms.Button SaveChanges;

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
			this.World = new System.Windows.Forms.DataGrid();
			this.SaveChanges = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.World)).BeginInit();
			this.SuspendLayout();
			// 
			// World
			// 
			this.World.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.World.DataMember = "";
			this.World.FlatMode = true;
			this.World.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.World.Location = new System.Drawing.Point(8, 8);
			this.World.Name = "World";
			this.World.Size = new System.Drawing.Size(616, 504);
			this.World.TabIndex = 0;
			// 
			// SaveChanges
			// 
			this.SaveChanges.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.SaveChanges.Location = new System.Drawing.Point(544, 520);
			this.SaveChanges.Name = "SaveChanges";
			this.SaveChanges.TabIndex = 1;
			this.SaveChanges.Text = "Save";
			this.SaveChanges.Click += new System.EventHandler(this.SaveChanges_Click);
			// 
			// WinMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(632, 550);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.SaveChanges,
																		  this.World});
			this.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Name = "WinMain";
			this.Text = "WinMain";
			this.Load += new System.EventHandler(this.WinMain_Load);
			((System.ComponentModel.ISupportInitialize)(this.World)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void WinMain_Load(object sender, System.EventArgs e)
		{
			World.DataSource = Multiverse;
			World.Refresh();
		}

		private void SaveChanges_Click(object sender, System.EventArgs e)
		{
			//MultiverseFactory.persistMultiverseToFile((Schema)World.DataSource, "world.xml" );
			MultiverseFactory.persistMultiverseToDatabase( Multiverse );
			Close();
		}
	}
}
