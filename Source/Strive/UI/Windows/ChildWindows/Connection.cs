using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Strive.Network.Client;

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
		private TreeNode CurrentPlayerNode;
		
		private Connection.ConnectionWindowState windowState;


		public bool Connected = false;
		private System.Windows.Forms.RadioButton NetworkProtocolTypeTCP;
		private System.Windows.Forms.RadioButton NetworkProtocolTypeUDP;
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
			RecentServers.ImageList = Icons.IconManager.GlobalImageList;
			loadRecentServers();
			StriveWindowState = ConnectionWindowState.NotConnected;
			Game.CurrentGameLoop._message_processor.OnCanPossess
				+= new Engine.MessageProcessor.CanPossessHandler( HandleCanPossessThreadSafe );
			Game.CurrentServerConnection.OnConnect += new ServerConnection.OnConnectHandler( HandleConnect );
			Game.CurrentServerConnection.OnDisconnect += new ServerConnection.OnDisconnectHandler( HandleDisconnect );
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
			this.NetworkProtocolTypeTCP = new System.Windows.Forms.RadioButton();
			this.NetworkProtocolTypeUDP = new System.Windows.Forms.RadioButton();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.groupBox1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.NetworkProtocolTypeUDP,
																					this.NetworkProtocolTypeTCP,
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
			this.Password.PasswordChar = '�';
			this.Password.Size = new System.Drawing.Size(120, 20);
			this.Password.TabIndex = 5;
			this.Password.Text = "";
			// 
			// Email
			// 
			this.Email.Location = new System.Drawing.Point(64, 80);
			this.Email.Name = "Email";
			this.Email.Size = new System.Drawing.Size(120, 20);
			this.Email.TabIndex = 4;
			this.Email.Text = "";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(4, 112);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(56, 16);
			this.label4.TabIndex = 6;
			this.label4.Text = "Password";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(4, 80);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(48, 16);
			this.label3.TabIndex = 5;
			this.label3.Text = "E-mail";
			// 
			// ConnectNow
			// 
			this.ConnectNow.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.ConnectNow.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ConnectNow.Location = new System.Drawing.Point(112, 138);
			this.ConnectNow.Name = "ConnectNow";
			this.ConnectNow.TabIndex = 6;
			this.ConnectNow.Text = "Connect";
			this.ConnectNow.Click += new System.EventHandler(this.ConnectNow_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(4, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(48, 16);
			this.label2.TabIndex = 3;
			this.label2.Text = "Port";
			// 
			// PortNumber
			// 
			this.PortNumber.Location = new System.Drawing.Point(64, 48);
			this.PortNumber.Name = "PortNumber";
			this.PortNumber.Size = new System.Drawing.Size(32, 20);
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
			this.label1.Location = new System.Drawing.Point(4, 16);
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
			// NetworkProtocolTypeTCP
			// 
			this.NetworkProtocolTypeTCP.Checked = true;
			this.NetworkProtocolTypeTCP.Location = new System.Drawing.Point(100, 48);
			this.NetworkProtocolTypeTCP.Name = "NetworkProtocolTypeTCP";
			this.NetworkProtocolTypeTCP.Size = new System.Drawing.Size(46, 24);
			this.NetworkProtocolTypeTCP.TabIndex = 2;
			this.NetworkProtocolTypeTCP.TabStop = true;
			this.NetworkProtocolTypeTCP.Text = "TCP";
			// 
			// NetworkProtocolTypeUDP
			// 
			this.NetworkProtocolTypeUDP.Location = new System.Drawing.Point(144, 48);
			this.NetworkProtocolTypeUDP.Name = "NetworkProtocolTypeUDP";
			this.NetworkProtocolTypeUDP.Size = new System.Drawing.Size(46, 24);
			this.NetworkProtocolTypeUDP.TabIndex = 3;
			this.NetworkProtocolTypeUDP.Text = "UDP";
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
			switch(windowState)
			{
				case ConnectionWindowState.NotConnected:
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
					if((!NetworkProtocolTypeTCP.Checked) &&
						(!NetworkProtocolTypeUDP.Checked))
					{
						NetworkProtocolTypeTCP.Focus();
						MessageBox.Show("You must select a protocol type.");
						return;
					}
					Strive.Network.Messages.NetworkProtocolType protocol;
					if(NetworkProtocolTypeTCP.Checked)
					{
						protocol = Strive.Network.Messages.NetworkProtocolType.TcpOnly;
					}
					else
					{
						protocol = Strive.Network.Messages.NetworkProtocolType.UdpAndTcp;
					}
					// umg ghey check for existance cause umg
					string label = ServerAddress.Text + ":" + PortNumber.Text;
					TreeNode serverNode = null;
					foreach ( TreeNode n in RecentServers.Nodes ) 
					{
						if ( n.Text == label ) 
						{
							serverNode = n;
							break;
						}
					}
					if ( serverNode == null ) 
					{
						serverNode = new TreeNode( label );
						
						serverNode.Tag = Settings.SettingsManager.AddRecentServer(ServerAddress.Text, 
							int.Parse(PortNumber.Text),
							protocol);
						RecentServers.Nodes.Add( serverNode );
					}
					else
					{
						Settings.SettingsManager.AddRecentServer(ServerAddress.Text, 
							int.Parse(PortNumber.Text),
							protocol);
					}
					label = Email.Text;
					TreeNode playerNode = null;
					foreach ( TreeNode n in serverNode.Nodes ) 
					{
						if ( n.Text == label ) 
						{
							playerNode = n;
							break;
						}
					}
					if ( playerNode == null ) 
					{
						playerNode = new TreeNode( label );
						playerNode.Tag = Settings.SettingsManager.AddRecentPlayer(ServerAddress.Text, int.Parse(PortNumber.Text), protocol, Email.Text, Password.Text);
						serverNode.Nodes.Add( playerNode );
						// build a datarow of these settings:

					}
					else
					{
						Settings.SettingsManager.AddRecentPlayer(ServerAddress.Text, int.Parse(PortNumber.Text), protocol, Email.Text, Password.Text);
					}
					playerNode.ImageIndex = (int)Icons.AvailableIcons.Player;
					playerNode.SelectedImageIndex = playerNode.ImageIndex;
					serverNode.ImageIndex = (int)Icons.AvailableIcons.StoppedServer;
					serverNode.SelectedImageIndex = serverNode.ImageIndex;
					serverNode.ExpandAll();
					playerNode.ExpandAll();

					CurrentPlayerNode = playerNode;
					StriveWindowState = ConnectionWindowState.Connecting;
					Application.DoEvents();
					Game.Play(ServerAddress.Text, Email.Text, Password.Text, int.Parse(PortNumber.Text), protocol, Game.CurrentMainWindow.RenderTarget);
					Connected = true;
					break;
				}
				case ConnectionWindowState.Connecting:
				case ConnectionWindowState.Connected:
				case ConnectionWindowState.Playing:
				{
					// disconnect:
					Game.CurrentServerConnection.Logout();
					StriveWindowState = ConnectionWindowState.NotConnected;
					if( CurrentPlayerNode != null ) 
					{
						CurrentPlayerNode.Nodes.Clear();
					}
					break;
				}

			}
		}

		void HandleConnect() {
			StriveWindowState = ConnectionWindowState.Connected;
		}

		void HandleDisconnect() {
			StriveWindowState = ConnectionWindowState.NotConnected;
		}

		void HandleCanPossessThreadSafe( Strive.Network.Messages.ToClient.CanPossess cp ) 
		{
			this.Invoke( new Engine.MessageProcessor.CanPossessHandler( HandleCanPossess ),
				new object [] { cp } );
		}

		void HandleCanPossess( Strive.Network.Messages.ToClient.CanPossess cp ) {
			if ( cp.possesable.Length < 1 ) {
				this.Text = "No characters available.";
			}
			CurrentPlayerNode.Nodes.Clear();
			foreach ( Strive.Network.Messages.ToClient.CanPossess.id_name_tuple tuple in cp.possesable ) {
				TreeNode n = new TreeNode(tuple.name);
				n.Tag = tuple.id;
				n.ImageIndex = (int)Icons.AvailableIcons.Mobile;
				n.SelectedImageIndex = n.ImageIndex;
				CurrentPlayerNode.Nodes.Add( n );
			}
			CurrentPlayerNode.Expand();
			StriveWindowState = ConnectionWindowState.Connected;
		}


		private void loadRecentServers()
		{
			// Initialise Recent Servers
			foreach(DataRow serverRow in Settings.SettingsManager.RecentServers.Rows)
			{
				TreeNode serverNode = new TreeNode( serverRow["serveraddress"] + ":" + serverRow["serverport"] );
				serverNode.Tag = serverRow;
				serverNode.ImageIndex = (int)Icons.AvailableIcons.StoppedServer;
				serverNode.SelectedImageIndex = serverNode.ImageIndex;
				RecentServers.Nodes.Add(serverNode);
				foreach(DataRow playerRow in serverRow.GetChildRows("ServerPlayers")) {
					TreeNode playerNode = new TreeNode( playerRow["emailaddress"].ToString() );
					playerNode.Tag = playerRow;
					playerNode.ImageIndex = (int)Icons.AvailableIcons.Player;
					playerNode.SelectedImageIndex = playerNode.ImageIndex;
					serverNode.Nodes.Add( playerNode );
				}
			}
			RecentServers.ExpandAll();
		}

		private void RecentServers_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			if( e.Node.Tag is int ) {
				foreach ( TreeNode n in CurrentPlayerNode.Nodes ) {
					n.ImageIndex = (int)Icons.AvailableIcons.Mobile;
					n.SelectedImageIndex = n.ImageIndex;
				}
				Game.CurrentPlayerID = (int)e.Node.Tag;
				Game.CurrentServerConnection.PossessMobile( Game.CurrentPlayerID );
				StriveWindowState = ConnectionWindowState.Playing;
				e.Node.ImageIndex = (int)Icons.AvailableIcons.MobilePossessed;
				e.Node.SelectedImageIndex = e.Node.ImageIndex;
				e.Node.Parent.Parent.ImageIndex = (int)Icons.AvailableIcons.StartedServer;
				e.Node.Parent.Parent.SelectedImageIndex = e.Node.Parent.Parent.ImageIndex;
				return;
			}

			DataRow serverSetting = (DataRow)e.Node.Tag;

			// How to do this and stay sane
			if(serverSetting.Table.Columns.Contains("playerkey"))
			{
				Email.Text = serverSetting["emailaddress"].ToString();
				Password.Text = serverSetting["password"].ToString();
				DataRow serverRow = serverSetting.GetParentRow("ServerPlayers");
				ServerAddress.Text = serverRow["serveraddress"].ToString();
				PortNumber.Text = serverRow["serverport"].ToString();
				Strive.Network.Messages.NetworkProtocolType protocol = (Strive.Network.Messages.NetworkProtocolType)Enum.Parse(typeof(Strive.Network.Messages.NetworkProtocolType), serverRow["protocol"].ToString());
				if(protocol == Strive.Network.Messages.NetworkProtocolType.UdpAndTcp)
				{
					NetworkProtocolTypeUDP.Checked = true;
				}
				else
				{
					NetworkProtocolTypeTCP.Checked = true;
				}
				return;
			}
			if(serverSetting.Table.Columns.Contains("serverkey"))
			{
				ServerAddress.Text = serverSetting["serveraddress"].ToString();
				PortNumber.Text = serverSetting["serverport"].ToString();
				Strive.Network.Messages.NetworkProtocolType protocol = (Strive.Network.Messages.NetworkProtocolType)Enum.Parse(typeof(Strive.Network.Messages.NetworkProtocolType), serverSetting["protocol"].ToString());
				if(protocol == Strive.Network.Messages.NetworkProtocolType.UdpAndTcp)
				{
					NetworkProtocolTypeUDP.Checked = true;
				}
				else
				{
					NetworkProtocolTypeTCP.Checked = true;
				}
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
					NetworkProtocolTypeTCP.Enabled = true;
					NetworkProtocolTypeUDP.Enabled = true;

					// 1.  set the icon for the connected server
					if ( CurrentPlayerNode != null ) {
						CurrentPlayerNode.Parent.ImageIndex = (int)Icons.AvailableIcons.StoppedServer;
						CurrentPlayerNode.Parent.SelectedImageIndex = CurrentPlayerNode.Parent.ImageIndex;
					}
					break;
				}
				case ConnectionWindowState.Connected:
				{
					ConnectNow.Text = "Disconnect";
					Email.Enabled = false;
					ServerAddress.Enabled = false;
					PortNumber.Enabled = false;
					Password.Enabled = false;
					NetworkProtocolTypeTCP.Enabled = false;
					NetworkProtocolTypeUDP.Enabled = false;

					// 1.  set the icon for the connected server
					if ( CurrentPlayerNode != null ) {
						CurrentPlayerNode.Parent.ImageIndex = (int)Icons.AvailableIcons.StartedServer;
						CurrentPlayerNode.Parent.SelectedImageIndex = CurrentPlayerNode.Parent.ImageIndex;
					}
					break;
				}
				case ConnectionWindowState.Connecting:
				{
					ConnectNow.Text = "Cancel...";
					Email.Enabled = false;
					ServerAddress.Enabled = false;
					PortNumber.Enabled = false;
					Password.Enabled = false;
					NetworkProtocolTypeTCP.Enabled = false;
					NetworkProtocolTypeUDP.Enabled = false;
					break;

				}
				case ConnectionWindowState.Playing:
				{
					break;
				}
				
			
			}
			Application.DoEvents();
		}

		
		protected enum ConnectionWindowState
		{
			NotConnected,
			Connecting,
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
