using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using SQLDMO;

namespace Strive.Utils.Shared.Controls
{
	/// <summary>
	/// Summary description for DBPicker.
	/// </summary>
	public class DBPicker : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox servers;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton UseTrustedAuthentication;
		private System.Windows.Forms.RadioButton UseSQLAuthentication;
		private System.Windows.Forms.TextBox UserName;
		private System.Windows.Forms.Button Connect;
		private System.Windows.Forms.ComboBox Databases;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label DatabaseStatus;
		private System.Windows.Forms.TextBox UserPassword;
		private System.Windows.Forms.Label lblUserPassword;
		private System.Windows.Forms.Label lblUserName;

		private SQLDMO.Application SQLDMOApplication;
		private SQLDMO.SQLServer SQLDMOServer;
		private System.Windows.Forms.Label DatabasesLabel;
		private System.Windows.Forms.TextBox ConnectionStatus;
		private System.Windows.Forms.Button Register; 
		private SQLDMO.Database SQLDMODatabase;
		


		public event DatabaseSelectedEventHandler DatabaseSelected;

		protected virtual void OnDatabaseSelected(DatabaseSelectedEventArgs e)
		{
			if(DatabaseSelected != null)
			{
				DatabaseSelected(this, e);
			}
		}

		public DBPicker()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitForm call

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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.servers = new System.Windows.Forms.ComboBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.Register = new System.Windows.Forms.Button();
			this.ConnectionStatus = new System.Windows.Forms.TextBox();
			this.DatabaseStatus = new System.Windows.Forms.Label();
			this.DatabasesLabel = new System.Windows.Forms.Label();
			this.Databases = new System.Windows.Forms.ComboBox();
			this.Connect = new System.Windows.Forms.Button();
			this.UserPassword = new System.Windows.Forms.TextBox();
			this.lblUserPassword = new System.Windows.Forms.Label();
			this.lblUserName = new System.Windows.Forms.Label();
			this.UserName = new System.Windows.Forms.TextBox();
			this.UseSQLAuthentication = new System.Windows.Forms.RadioButton();
			this.UseTrustedAuthentication = new System.Windows.Forms.RadioButton();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(16, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "Server:";
			// 
			// servers
			// 
			this.servers.Location = new System.Drawing.Point(72, 24);
			this.servers.Name = "servers";
			this.servers.Size = new System.Drawing.Size(152, 24);
			this.servers.TabIndex = 0;
			this.servers.TextChanged += new System.EventHandler(this.servers_SelectedIndexChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.Register,
																					this.ConnectionStatus,
																					this.DatabaseStatus,
																					this.DatabasesLabel,
																					this.Databases,
																					this.Connect,
																					this.UserPassword,
																					this.lblUserPassword,
																					this.lblUserName,
																					this.UserName,
																					this.UseSQLAuthentication,
																					this.UseTrustedAuthentication,
																					this.servers,
																					this.label1});
			this.groupBox1.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.groupBox1.Location = new System.Drawing.Point(8, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(624, 160);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Specify a Server";
			// 
			// Register
			// 
			this.Register.Enabled = false;
			this.Register.Location = new System.Drawing.Point(16, 88);
			this.Register.Name = "Register";
			this.Register.TabIndex = 13;
			this.Register.Text = "Remember";
			this.Register.Click += new System.EventHandler(this.Register_Click);
			// 
			// ConnectionStatus
			// 
			this.ConnectionStatus.Location = new System.Drawing.Point(104, 56);
			this.ConnectionStatus.Multiline = true;
			this.ConnectionStatus.Name = "ConnectionStatus";
			this.ConnectionStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.ConnectionStatus.Size = new System.Drawing.Size(272, 56);
			this.ConnectionStatus.TabIndex = 0;
			this.ConnectionStatus.TabStop = false;
			this.ConnectionStatus.Text = "";
			// 
			// DatabaseStatus
			// 
			this.DatabaseStatus.Location = new System.Drawing.Point(384, 120);
			this.DatabaseStatus.Name = "DatabaseStatus";
			this.DatabaseStatus.Size = new System.Drawing.Size(224, 23);
			this.DatabaseStatus.TabIndex = 12;
			// 
			// DatabasesLabel
			// 
			this.DatabasesLabel.Enabled = false;
			this.DatabasesLabel.Location = new System.Drawing.Point(16, 120);
			this.DatabasesLabel.Name = "DatabasesLabel";
			this.DatabasesLabel.Size = new System.Drawing.Size(56, 23);
			this.DatabasesLabel.TabIndex = 10;
			this.DatabasesLabel.Text = "Database:";
			// 
			// Databases
			// 
			this.Databases.Enabled = false;
			this.Databases.Location = new System.Drawing.Point(72, 120);
			this.Databases.Name = "Databases";
			this.Databases.Size = new System.Drawing.Size(304, 24);
			this.Databases.TabIndex = 9;
			this.Databases.SelectedIndexChanged += new System.EventHandler(this.Databases_SelectedIndexChanged);
			// 
			// Connect
			// 
			this.Connect.Enabled = false;
			this.Connect.Location = new System.Drawing.Point(16, 56);
			this.Connect.Name = "Connect";
			this.Connect.TabIndex = 8;
			this.Connect.Text = "Connect >";
			this.Connect.Click += new System.EventHandler(this.Connect_Click);
			// 
			// UserPassword
			// 
			this.UserPassword.Enabled = false;
			this.UserPassword.Location = new System.Drawing.Point(456, 80);
			this.UserPassword.Name = "UserPassword";
			this.UserPassword.PasswordChar = '•';
			this.UserPassword.Size = new System.Drawing.Size(152, 20);
			this.UserPassword.TabIndex = 7;
			this.UserPassword.Text = "";
			// 
			// lblUserPassword
			// 
			this.lblUserPassword.Enabled = false;
			this.lblUserPassword.Location = new System.Drawing.Point(384, 80);
			this.lblUserPassword.Name = "lblUserPassword";
			this.lblUserPassword.Size = new System.Drawing.Size(64, 23);
			this.lblUserPassword.TabIndex = 6;
			this.lblUserPassword.Text = "Password:";
			// 
			// lblUserName
			// 
			this.lblUserName.Enabled = false;
			this.lblUserName.Location = new System.Drawing.Point(384, 56);
			this.lblUserName.Name = "lblUserName";
			this.lblUserName.Size = new System.Drawing.Size(64, 23);
			this.lblUserName.TabIndex = 5;
			this.lblUserName.Text = "User Name:";
			// 
			// UserName
			// 
			this.UserName.Enabled = false;
			this.UserName.Location = new System.Drawing.Point(456, 56);
			this.UserName.Name = "UserName";
			this.UserName.Size = new System.Drawing.Size(152, 20);
			this.UserName.TabIndex = 4;
			this.UserName.Text = "";
			// 
			// UseSQLAuthentication
			// 
			this.UseSQLAuthentication.Enabled = false;
			this.UseSQLAuthentication.Location = new System.Drawing.Point(384, 24);
			this.UseSQLAuthentication.Name = "UseSQLAuthentication";
			this.UseSQLAuthentication.Size = new System.Drawing.Size(216, 24);
			this.UseSQLAuthentication.TabIndex = 3;
			this.UseSQLAuthentication.Text = "Use SQL Authentication";
			this.UseSQLAuthentication.CheckedChanged += new System.EventHandler(this.UseSQLAuthentication_CheckedChanged);
			// 
			// UseTrustedAuthentication
			// 
			this.UseTrustedAuthentication.Enabled = false;
			this.UseTrustedAuthentication.Location = new System.Drawing.Point(248, 24);
			this.UseTrustedAuthentication.Name = "UseTrustedAuthentication";
			this.UseTrustedAuthentication.Size = new System.Drawing.Size(136, 24);
			this.UseTrustedAuthentication.TabIndex = 1;
			this.UseTrustedAuthentication.Text = "Use Trusted Security";
			this.UseTrustedAuthentication.CheckedChanged += new System.EventHandler(this.UseTrustedAuthentication_CheckedChanged);
			// 
			// DBPicker
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.groupBox1});
			this.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Name = "DBPicker";
			this.Size = new System.Drawing.Size(640, 176);
			this.Load += new System.EventHandler(this.DBPicker_Load);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void DBPicker_Load(object sender, System.EventArgs e)
		{
			SQLDMOApplication = new SQLDMO.ApplicationClass();
			

			foreach(SQLDMO.ServerGroup s in SQLDMOApplication.ServerGroups)
			{
				foreach(SQLDMO.RegisteredServer r in s.RegisteredServers)
				{
					servers.Items.Add(r.Name);
				}
			}
		}

		private void servers_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			UseTrustedAuthentication.Enabled = true;
			UseSQLAuthentication.Enabled = true;
		}

		private void UseTrustedAuthentication_CheckedChanged(object sender, System.EventArgs e)
		{
			if(UseTrustedAuthentication.Checked)
			{
				Connect.Enabled = true;
			}
		}

		private void ConnectionStatus_Click(object sender, System.EventArgs e)
		{
		
		}

		private void UseSQLAuthentication_CheckedChanged(object sender, System.EventArgs e)
		{
			UserName.Enabled = UseSQLAuthentication.Checked;
			UserPassword.Enabled = UseSQLAuthentication.Checked;
			lblUserName.Enabled = UseSQLAuthentication.Checked;
			lblUserPassword.Enabled = UseSQLAuthentication.Checked;	
			if(UseSQLAuthentication.Checked)
			{
				Connect.Enabled = true;
			}
		}

		private void Connect_Click(object sender, System.EventArgs e)
		{
			SQLDMOServer = new SQLDMO.SQLServerClass();

			try
			{
				if(!UseTrustedAuthentication.Checked)
				{

					SQLDMOServer.Connect(servers.Text, UserName.Text, UserPassword.Text);
				}
				else
				{
					SQLDMOServer.LoginSecure = true;
					SQLDMOServer.Connect(servers.Text, null, null);
				}
			}
			catch(Exception ex)
			{
				ConnectionStatus.Text += ex.Message + "\r\n----------------\r\n";
			}
			this.Refresh();
			try
			{
				populateDatabases();
			}
			catch(Exception)
			{
			}
			Register.Enabled = true;
		}
		
		private void populateDatabases()
		{
			Databases.Items.Clear();
			Databases.Enabled = true;
			DatabasesLabel.Enabled = true;
			if(SQLDMOServer != null )
			{
				foreach(SQLDMO.Database d in SQLDMOServer.Databases)
				{
					Databases.Items.Add(d.Name);
				}
			}
		}

		private void Databases_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(Databases.SelectedIndex >= 0)
			{
				SQLDMODatabase = (Database)SQLDMOServer.Databases.Item(Databases.Text, System.Type.Missing);
				DatabaseStatus.Text = "Connected to " + SQLDMODatabase.Name;
				DatabaseSelectedEventArgs dbArgs = new DatabaseSelectedEventArgs(SQLDMOApplication, 
					SQLDMOServer,
					SQLDMODatabase);
				OnDatabaseSelected(dbArgs);
			}
		}

		private void Register_Click(object sender, System.EventArgs e)
		{
			const string DEFAULTGROUPNAME = "DBPicker";

			ServerGroup s = null;

			// Look for default group
			foreach(ServerGroup g in SQLDMOApplication.ServerGroups)
			{
				if(g.Name == DEFAULTGROUPNAME)
				{
					s = g;
					break;
				}
			}

			// Create and add default group if it doesn't exist
			if(s == null)
			{
				s = new SQLDMO.ServerGroupClass();
				s.Name = DEFAULTGROUPNAME;
				SQLDMOApplication.ServerGroups.Add(s);
			}


			// return if server already registered in a group
			foreach(ServerGroup enumServerGroup in SQLDMOApplication.ServerGroups)
			{
				foreach(RegisteredServer r in enumServerGroup.RegisteredServers)
				{
					if(r.Name == SQLDMOServer.Name)
					{
						MessageBox.Show(this, "Already regstered in group '" + enumServerGroup.Name + "'.");
						return;
					}
				}
			}

			RegisteredServer t = new RegisteredServerClass();

			t.Name = SQLDMOServer.Name;
			t.Login = SQLDMOServer.Login;
			t.Password = SQLDMOServer.Password;
			t.UseTrustedConnection = (UseTrustedAuthentication.Checked == true ? 1 : 0);
			
			s.RegisteredServers.Add(t);

			MessageBox.Show(this, "Registered '" + SQLDMOServer.Name + "'.");
		}

		public class DatabaseSelectedEventArgs : System.EventArgs
		{
			public SQLDMO.Database Database;
			public SQLDMO.Application Application;
			public SQLDMO.SQLServer Server;

			public DatabaseSelectedEventArgs(SQLDMO.Application application,
				SQLDMO.SQLServer server,
				SQLDMO.Database database)
			{
				Application = application;
				Server = server;
				Database = database;
			}
		}
	}

	public delegate void DatabaseSelectedEventHandler(object sender, DBPicker.DatabaseSelectedEventArgs e);





}
