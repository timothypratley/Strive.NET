using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Strive.Utils.Shared.Controls;
using SQLDMO;
using System.Xml;
using System.Xml.Xsl;

namespace Strive.Utils.CommandGenerator
{
	/// <summary>
	/// Summary description for WinMain.
	/// </summary>
	public class WinMain : System.Windows.Forms.Form
	{
		private Strive.Utils.Shared.Controls.DBPicker dbPicker1;
		private System.Windows.Forms.ListBox StoredProcedures;
		private System.Windows.Forms.OpenFileDialog GeneratedFileLocation;
		private SQLDMO.Application SQLDMOApplication;
		private SQLDMO.SQLServer SQLDMOServer;
		private SQLDMO.Database SQLDMODatabase;
		private System.Windows.Forms.Button RefreshStoredProcedures;
		private System.Windows.Forms.Button InvertStorecProcedures;
		private System.Windows.Forms.Button GenerateCommandFile;
		private System.Windows.Forms.ComboBox XSLFileName;
		private System.Windows.Forms.TextBox Output;
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
			this.StoredProcedures = new System.Windows.Forms.ListBox();
			this.GeneratedFileLocation = new System.Windows.Forms.OpenFileDialog();
			this.RefreshStoredProcedures = new System.Windows.Forms.Button();
			this.InvertStorecProcedures = new System.Windows.Forms.Button();
			this.GenerateCommandFile = new System.Windows.Forms.Button();
			this.XSLFileName = new System.Windows.Forms.ComboBox();
			this.Output = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// dbPicker1
			// 
			this.dbPicker1.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.dbPicker1.Location = new System.Drawing.Point(0, 8);
			this.dbPicker1.Name = "dbPicker1";
			this.dbPicker1.Size = new System.Drawing.Size(640, 176);
			this.dbPicker1.TabIndex = 0;
			// 
			// StoredProcedures
			// 
			this.StoredProcedures.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left);
			this.StoredProcedures.Enabled = false;
			this.StoredProcedures.ItemHeight = 16;
			this.StoredProcedures.Location = new System.Drawing.Point(8, 184);
			this.StoredProcedures.Name = "StoredProcedures";
			this.StoredProcedures.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.StoredProcedures.Size = new System.Drawing.Size(300, 340);
			this.StoredProcedures.TabIndex = 1;
			// 
			// GeneratedFileLocation
			// 
			this.GeneratedFileLocation.CheckFileExists = false;
			this.GeneratedFileLocation.DefaultExt = "c";
			this.GeneratedFileLocation.Filter = "C# files|*.cs|All files|*.*";
			this.GeneratedFileLocation.RestoreDirectory = true;
			this.GeneratedFileLocation.Title = "Select Generated File Location";
			// 
			// RefreshStoredProcedures
			// 
			this.RefreshStoredProcedures.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.RefreshStoredProcedures.Enabled = false;
			this.RefreshStoredProcedures.Location = new System.Drawing.Point(96, 528);
			this.RefreshStoredProcedures.Name = "RefreshStoredProcedures";
			this.RefreshStoredProcedures.TabIndex = 2;
			this.RefreshStoredProcedures.Text = "Refresh";
			this.RefreshStoredProcedures.Click += new System.EventHandler(this.RefreshStoredProcedures_Click);
			// 
			// InvertStorecProcedures
			// 
			this.InvertStorecProcedures.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.InvertStorecProcedures.Location = new System.Drawing.Point(16, 528);
			this.InvertStorecProcedures.Name = "InvertStorecProcedures";
			this.InvertStorecProcedures.TabIndex = 3;
			this.InvertStorecProcedures.Text = "Invert";
			this.InvertStorecProcedures.Click += new System.EventHandler(this.InvertStorecProcedures_Click);
			// 
			// GenerateCommandFile
			// 
			this.GenerateCommandFile.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.GenerateCommandFile.Location = new System.Drawing.Point(232, 528);
			this.GenerateCommandFile.Name = "GenerateCommandFile";
			this.GenerateCommandFile.TabIndex = 4;
			this.GenerateCommandFile.Text = "Generate";
			this.GenerateCommandFile.Click += new System.EventHandler(this.GenerateCommandFile_Click);
			// 
			// XSLFileName
			// 
			this.XSLFileName.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.XSLFileName.Enabled = false;
			this.XSLFileName.Location = new System.Drawing.Point(320, 184);
			this.XSLFileName.Name = "XSLFileName";
			this.XSLFileName.Size = new System.Drawing.Size(304, 24);
			this.XSLFileName.TabIndex = 5;
			// 
			// Output
			// 
			this.Output.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.Output.Enabled = false;
			this.Output.Location = new System.Drawing.Point(320, 216);
			this.Output.Multiline = true;
			this.Output.Name = "Output";
			this.Output.Size = new System.Drawing.Size(304, 304);
			this.Output.TabIndex = 6;
			this.Output.Text = "";
			// 
			// WinMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(632, 566);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.Output,
																		  this.XSLFileName,
																		  this.GenerateCommandFile,
																		  this.InvertStorecProcedures,
																		  this.RefreshStoredProcedures,
																		  this.StoredProcedures,
																		  this.dbPicker1});
			this.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Name = "WinMain";
			this.Text = "Command Generator";
			this.Load += new System.EventHandler(this.WinMain_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void WinMain_Load(object sender, System.EventArgs e)
		{
			dbPicker1.DatabaseSelected+=new DatabaseSelectedEventHandler(DBPicker_DatabaseSelected);

			string XSLPath = System.IO.Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["UtilsBaseFolder"], System.Configuration.ConfigurationSettings.AppSettings["CommandGeneratorXSLPath"]);

			string[] files  = System.IO.Directory.GetFiles(XSLPath, "*.xsl");
			foreach(string filename in files)
			{
				XSLFileName.Items.Add(filename);
			}
			
			

		}

		private void listStoredProcedures()
		{
			StoredProcedures.Items.Clear();
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
				disableForm();
			}
			else
			{
				enableForm();
			}
		}

		private void disableForm()
		{
			setFormEnablement(false);
		}

		private void enableForm()
		{
			setFormEnablement(true);
		}

		private void setFormEnablement(bool Enabled)
		{
			StoredProcedures.Enabled = Enabled;
			Output.Enabled = Enabled;
			XSLFileName.Enabled = Enabled;
		}

		private void invertStoredProcedures()
		{

			for(int item = 0; item < StoredProcedures.Items.Count; item++)
			{
				if(StoredProcedures.SelectedIndices.Contains(item))
				{
					StoredProcedures.SetSelected(item, false);
				}
				else
				{
					StoredProcedures.SetSelected(item, true);
				}

			}
		}

		protected void DBPicker_DatabaseSelected(object sender, DBPicker.DatabaseSelectedEventArgs e)
		{
			SQLDMOApplication = e.Application;
			SQLDMOServer = e.Server;
			SQLDMODatabase = e.Database;
			RefreshStoredProcedures.Enabled = true;
			listStoredProcedures();
		}

		private void RefreshStoredProcedures_Click(object sender, System.EventArgs e)
		{
			listStoredProcedures();
		}

		private void InvertStorecProcedures_Click(object sender, System.EventArgs e)
		{
			invertStoredProcedures();
		}

		// Used arraylist because could not create an array of IStoredProceudre
		private ArrayList getSelectedProcedures()
		{
			ArrayList spReturn = new ArrayList();
			int i = 0;
			
			foreach(int selectedIndex in StoredProcedures.SelectedIndices)
			{
				StoredProcedure s = getSingleStoredProcedure(StoredProcedures.Items[selectedIndex].ToString());
				spReturn.Add(s);
				i++;
			}

			return spReturn;

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


		private void GenerateCommandFile_Click(object sender, System.EventArgs e)
		{
			XmlDocument x = API.GetStoredProcedures(getSelectedProcedures());

			if(XSLFileName.SelectedIndex < 0)
			{
				Output.Text = x.InnerXml;
			}
			else
			{
				XslTransform xsl = new XslTransform();
				xsl.Load(XSLFileName.Text);

				StringBuilder sb = new StringBuilder();
				StringWriter sw = new StringWriter(sb);

				xsl.Transform(x, null, sw);

				Output.Text = sb.ToString();
			}
				
			
			
		}


	}
}
