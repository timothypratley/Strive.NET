using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Strive.UI.Windows.ChildWindows
{
	/// <summary>
	/// Summary description for Connection.
	/// </summary>
	public class Connection : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox ServerAddress;
		private System.Windows.Forms.TextBox PortNumber;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button ConnectNow;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox Email;
		private System.Windows.Forms.TextBox Password;
		private System.Windows.Forms.TreeView RecentServers;
		
		private Hashtable serverNodes = new Hashtable();
		private Hashtable playerNodes = new Hashtable();
		private Hashtable characterNodes = new Hashtable();
		private Connection.ConnectionWindowState windowState;

		public bool Connected = false;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Connection()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			ImageList i = new ImageList();
			i.Images.Add(Icons.IconManager.GetAsBitmap(Icons.AvailableIcons.StoppedServer));
			i.Images.Add(Icons.IconManager.GetAsBitmap(Icons.AvailableIcons.StartedServer));
			i.Images.Add(Icons.IconManager.GetAsBitmap(Icons.AvailableIcons.Player));
			RecentServers.ImageList = i;
			synchRecentServers();
			StriveWindowState = ConnectionWindowState.NotConnected;
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.Password = new System.Windows.Forms.TextBox();
			this.Email = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.ConnectNow = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.PortNumber = new System.Windows.Forms.TextBox();
			this.ServerAddress = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.RecentServers = new System.Windows.Forms.TreeView();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.groupBox1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.Password,
																					this.Email,
																					this.label4,
																					this.label3,
																					this.ConnectNow,
																					this.label2,
																					this.PortNumber,
																					this.ServerAddress,
																					this.label1});
			this.groupBox1.Location = new System.Drawing.Point(0, 216);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(192, 168);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Server Details";
			// 
			// Password
			// 
			this.Password.Location = new System.Drawing.Point(64, 112);
			this.Password.Name = "Password";
			this.Password.PasswordChar = '•';
			this.Password.Size = new System.Drawing.Size(120, 20);
			this.Password.TabIndex = 3;
			this.Password.Text = "";
			// 
			// Email
			// 
			this.Email.Location = new System.Drawing.Point(64, 80);
			this.Email.Name = "Email";
			this.Email.Size = new System.Drawing.Size(120, 20);
			this.Email.TabIndex = 2;
			this.Email.Text = "";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 112);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(56, 16);
			this.label4.TabIndex = 6;
			this.label4.Text = "Password";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 80);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(48, 16);
			this.label3.TabIndex = 5;
			this.label3.Text = "E-mail";
			// 
			// ConnectNow
			// 
			this.ConnectNow.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.ConnectNow.Location = new System.Drawing.Point(112, 138);
			this.ConnectNow.Name = "ConnectNow";
			this.ConnectNow.TabIndex = 4;
			this.ConnectNow.Text = "Connect";
			this.ConnectNow.Click += new System.EventHandler(this.ConnectNow_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(48, 16);
			this.label2.TabIndex = 3;
			this.label2.Text = "Port";
			// 
			// PortNumber
			// 
			this.PortNumber.Location = new System.Drawing.Point(64, 48);
			this.PortNumber.Name = "PortNumber";
			this.PortNumber.Size = new System.Drawing.Size(40, 20);
			this.PortNumber.TabIndex = 1;
			this.PortNumber.Text = "";
			// 
			// ServerAddress
			// 
			this.ServerAddress.Location = new System.Drawing.Point(64, 16);
			this.ServerAddress.Name = "ServerAddress";
			this.ServerAddress.Size = new System.Drawing.Size(120, 20);
			this.ServerAddress.TabIndex = 0;
			this.ServerAddress.Text = "";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "Server";
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.groupBox2.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.RecentServers});
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(192, 216);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Recent Connections";
			// 
			// RecentServers
			// 
			this.RecentServers.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.RecentServers.ImageIndex = -1;
			this.RecentServers.Location = new System.Drawing.Point(4, 16);
			this.RecentServers.Name = "RecentServers";
			this.RecentServers.SelectedImageIndex = -1;
			this.RecentServers.Size = new System.Drawing.Size(184, 192);
			this.RecentServers.TabIndex = 0;
			this.RecentServers.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.RecentServers_AfterSelect);
			// 
			// Connection
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(192, 389);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.groupBox2,
																		  this.groupBox1});
			this.Name = "Connection";
			this.Text = "Connection";
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void ConnectNow_Click(object sender, System.EventArgs e)
		{
			if(windowState == ConnectionWindowState.NotConnected)
			{
				if(ServerAddress.Text == "")
				{
					ServerAddress.Focus();
					MessageBox.Show("You must enter a server.");
					return;
				}
				if(PortNumber.Text == "")
				{
					PortNumber.Focus();
					MessageBox.Show("You must enter a port.");
					return;
				}
				if(Email.Text == "")
				{
					Email.Focus();
					MessageBox.Show("You must enter an e-mail.");
					return;
				}
				Settings.SettingsManager.AddRecentServer(ServerAddress.Text, int.Parse(PortNumber.Text));
				Settings.SettingsManager.AddRecentPlayer(ServerAddress.Text, int.Parse(PortNumber.Text), Email.Text, Password.Text);
				Game.Play(ServerAddress.Text, Email.Text, Password.Text, int.Parse(PortNumber.Text), Game.CurrentMainWindow.RenderTarget);
				setConnectedImage();
				Connected = true;
				Game.CurrentGameLoop._message_processor.OnCanPossess
					+= new Engine.MessageProcessor.CanPossessHandler( HandleCanPossessThreadSafe );

				Game.CurrentServerConnection.RequestPossessable();

				StriveWindowState = ConnectionWindowState.Connected;
			}
			else if(windowState == ConnectionWindowState.Connected ||
				windowState == ConnectionWindowState.Playing)
			{
				// disconnect:
				Game.CurrentServerConnection.Logout();
				StriveWindowState = ConnectionWindowState.NotConnected;


			}
		}

		private void setConnectedImage()
		{
			foreach(TreeNode n in RecentServers.Nodes)
			{
				if(n.Text == ServerAddress.Text + ":" + PortNumber.Text)
				{
					n.ImageIndex = 1;
				}
			}
		}

		void HandleCanPossessThreadSafe( Strive.Network.Messages.ToClient.CanPossess cp ) 
		{
			this.Invoke( new Engine.MessageProcessor.CanPossessHandler( HandleCanPossess ),
				new object [] { cp } );
		}

		void HandleCanPossess( Strive.Network.Messages.ToClient.CanPossess cp ) 
		{
			if ( cp.possesable.Length < 1 ) 
			{
				throw new Exception("No characters available.");
			}
			foreach ( Strive.Network.Messages.ToClient.CanPossess.id_name_tuple tuple in cp.possesable ) 
			{
				Settings.SettingsManager.AddRecentCharacter(ServerAddress.Text, 
					int.Parse(PortNumber.Text),
					Email.Text,
					Password.Text,
					tuple.id,
					tuple.name);
			}

			synchRecentServers();
		}


		private void synchRecentServers()
		{
			RecentServers.Nodes.Clear();
			serverNodes.Clear();
			playerNodes.Clear();
			characterNodes.Clear();
			// Initialise Recent Servers
			foreach(DataRow serverRow in Settings.SettingsManager.RecentServers.Rows)
			{
				TreeNode n = new TreeNode(serverRow["serveraddress"] + ":" + serverRow["serverport"]);
				n.Tag = serverRow;
				//n.ImageIndex = 0;

				serverNodes.Add(serverRow["serveraddress"] + ":" + serverRow["serverport"],
					n);

				foreach(DataRow playerRow in serverRow.GetChildRows("ServerPlayers"))
				{
					TreeNode p = new TreeNode(playerRow["emailaddress"].ToString());
					p.Tag = playerRow;
					p.ImageIndex = 2;

					playerNodes.Add(playerRow["emailaddress"].ToString(),
						p);

					if(Connected)
					{
						foreach(DataRow characterRow in playerRow.GetChildRows("PlayerCharacters"))
						{
							TreeNode c = new TreeNode(characterRow["charactername"].ToString());
							c.Tag = characterRow;
							p.Nodes.Add(c);

							characterNodes.Add(characterRow["charactername"].ToString(),
								c);
						}
					}

					n.Nodes.Add(p);
				}
				RecentServers.Nodes.Add(n);
			}

			RecentServers.ExpandAll();

		}

		private void RecentServers_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			DataRow serverSetting = (DataRow)e.Node.Tag;

			// How to do this and stay sane
			// this is a character node
			if(serverSetting.Table.Columns.Contains("charactername"))
			{
				Game.CurrentServerConnection.PossessMobile(int.Parse(serverSetting["characterid"].ToString()));
				StriveWindowState = ConnectionWindowState.Playing;
				return;
			}
			if(serverSetting.Table.Columns.Contains("playerkey"))
			{
				Email.Text = serverSetting["emailaddress"].ToString();
				Password.Text = serverSetting["password"].ToString();
				DataRow serverRow = serverSetting.GetParentRow("ServerPlayers");
				ServerAddress.Text = serverRow["serveraddress"].ToString();
				PortNumber.Text = serverRow["serverport"].ToString();

				e.Node.ImageIndex = 2;
				return;
			}
			if(serverSetting.Table.Columns.Contains("serverkey"))
			{
				ServerAddress.Text = serverSetting["serveraddress"].ToString();
				PortNumber.Text = serverSetting["serverport"].ToString();
			}
		}


		private void setWindowState()
		{
			switch(windowState)
			{
				case ConnectionWindowState.NotConnected:
				{
					ConnectNow.Text = "Connect";
					Email.Enabled = true;
					ServerAddress.Enabled = true;
					PortNumber.Enabled = true;
					Password.Enabled = true;
					break;
				}
				case ConnectionWindowState.Connected:
				{
					ConnectNow.Text = "Disconnect";
					Email.Enabled = false;
					ServerAddress.Enabled = false;
					PortNumber.Enabled = false;
					Password.Enabled = false;

					// 1.  set the icon for the connected server

					string connectedServerKey = ServerAddress.Text + ":" + PortNumber.Text;

					((TreeNode)serverNodes[connectedServerKey]).ImageIndex = 1;

					break;
				}
				case ConnectionWindowState.Playing:
				{
					break;
				}
				
			}
		}

		
		protected enum ConnectionWindowState
		{
			NotConnected,
			Connected,
			Playing
		}


		protected Connection.ConnectionWindowState StriveWindowState
		{
			set
			{
				windowState = value;
				setWindowState();
			}
		}


	}
}
