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
		
		private Connection.ConnectionWindowState windowState;

		private System.Windows.Forms.RadioButton NetworkProtocolTypeTCP;
		private System.Windows.Forms.RadioButton NetworkProtocolTypeUDP;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ListView RecentConnections;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ProgressBar ConnectionProgress;
		private System.Windows.Forms.Timer timer1;
		private System.ComponentModel.IContainer components;

		public Connection()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			loadRecentServers();
			StriveWindowState = ConnectionWindowState.NotConnected;
			Game.CurrentMessageProcessor.OnCanPossess
				+= new Engine.MessageProcessor.CanPossessHandler( HandleCanPossess );
			Game.CurrentServerConnection.OnConnect += new ServerConnection.OnConnectHandler( HandleConnect );
			Game.CurrentServerConnection.OnConnectFailed += new ServerConnection.OnConnectFailedHandler( HandleConnectFailed );
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
			this.components = new System.ComponentModel.Container();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.NetworkProtocolTypeUDP = new System.Windows.Forms.RadioButton();
			this.NetworkProtocolTypeTCP = new System.Windows.Forms.RadioButton();
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
			this.RecentConnections = new System.Windows.Forms.ListView();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.ConnectionProgress = new System.Windows.Forms.ProgressBar();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.NetworkProtocolTypeUDP);
			this.groupBox1.Controls.Add(this.NetworkProtocolTypeTCP);
			this.groupBox1.Controls.Add(this.Password);
			this.groupBox1.Controls.Add(this.Email);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.ConnectNow);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.PortNumber);
			this.groupBox1.Controls.Add(this.ServerAddress);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(0, 291);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(656, 176);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Server Details";
			// 
			// NetworkProtocolTypeUDP
			// 
			this.NetworkProtocolTypeUDP.Location = new System.Drawing.Point(224, 48);
			this.NetworkProtocolTypeUDP.Name = "NetworkProtocolTypeUDP";
			this.NetworkProtocolTypeUDP.Size = new System.Drawing.Size(46, 24);
			this.NetworkProtocolTypeUDP.TabIndex = 3;
			this.NetworkProtocolTypeUDP.Text = "UDP";
			// 
			// NetworkProtocolTypeTCP
			// 
			this.NetworkProtocolTypeTCP.Checked = true;
			this.NetworkProtocolTypeTCP.Location = new System.Drawing.Point(176, 48);
			this.NetworkProtocolTypeTCP.Name = "NetworkProtocolTypeTCP";
			this.NetworkProtocolTypeTCP.Size = new System.Drawing.Size(46, 24);
			this.NetworkProtocolTypeTCP.TabIndex = 2;
			this.NetworkProtocolTypeTCP.TabStop = true;
			this.NetworkProtocolTypeTCP.Text = "TCP";
			// 
			// Password
			// 
			this.Password.Location = new System.Drawing.Point(64, 112);
			this.Password.Name = "Password";
			this.Password.PasswordChar = '•';
			this.Password.Size = new System.Drawing.Size(200, 20);
			this.Password.TabIndex = 5;
			this.Password.Text = "";
			// 
			// Email
			// 
			this.Email.Location = new System.Drawing.Point(64, 80);
			this.Email.Name = "Email";
			this.Email.Size = new System.Drawing.Size(200, 20);
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
			this.ConnectNow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.ConnectNow.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ConnectNow.Location = new System.Drawing.Point(112, 146);
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
			this.PortNumber.Size = new System.Drawing.Size(88, 20);
			this.PortNumber.TabIndex = 1;
			this.PortNumber.Text = "";
			// 
			// ServerAddress
			// 
			this.ServerAddress.Location = new System.Drawing.Point(64, 16);
			this.ServerAddress.Name = "ServerAddress";
			this.ServerAddress.Size = new System.Drawing.Size(200, 20);
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
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.RecentConnections);
			this.groupBox2.Location = new System.Drawing.Point(0, 0);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(656, 280);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Recent Connections";
			// 
			// RecentConnections
			// 
			this.RecentConnections.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.RecentConnections.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																								this.columnHeader2,
																								this.columnHeader3,
																								this.columnHeader1});
			this.RecentConnections.FullRowSelect = true;
			this.RecentConnections.Location = new System.Drawing.Point(16, 32);
			this.RecentConnections.Name = "RecentConnections";
			this.RecentConnections.Size = new System.Drawing.Size(624, 232);
			this.RecentConnections.TabIndex = 0;
			this.RecentConnections.View = System.Windows.Forms.View.Details;
			this.RecentConnections.DoubleClick += new System.EventHandler(this.RecentConnections_DoubleClick);
			this.RecentConnections.SelectedIndexChanged += new System.EventHandler(this.RecentConnections_SelectedIndexChanged);
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Server";
			this.columnHeader2.Width = 209;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Port";
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Email Address";
			this.columnHeader1.Width = 268;
			// 
			// ConnectionProgress
			// 
			this.ConnectionProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.ConnectionProgress.Location = new System.Drawing.Point(0, 472);
			this.ConnectionProgress.Name = "ConnectionProgress";
			this.ConnectionProgress.Size = new System.Drawing.Size(656, 23);
			this.ConnectionProgress.Step = 5;
			this.ConnectionProgress.TabIndex = 2;
			this.ConnectionProgress.Visible = false;
			// 
			// timer1
			// 
			this.timer1.Interval = 1000;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// Connection
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(656, 501);
			this.Controls.Add(this.ConnectionProgress);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Name = "Connection";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
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

					Settings.SettingsManager.AddRecentServer(ServerAddress.Text, 
						int.Parse(PortNumber.Text),
						protocol);
						Settings.SettingsManager.AddRecentPlayer(ServerAddress.Text, int.Parse(PortNumber.Text), protocol, Email.Text, Password.Text);

					// TODO: is this teh ghey? might flicker
					loadRecentServers();

					StriveWindowState = ConnectionWindowState.Connecting;
					Application.DoEvents();

					ConnectionProgress.Visible = true;
					ConnectionProgress.Value = 0;
					timer1.Start();
					Game.Play(ServerAddress.Text, Email.Text, Password.Text, int.Parse(PortNumber.Text), protocol, Game.CurrentMainWindow.RenderTarget);
					break;
				}
				case ConnectionWindowState.Connecting:
				case ConnectionWindowState.Connected:
				case ConnectionWindowState.Playing:
				{
					// disconnect:
					Game.CurrentServerConnection.Logout();
					StriveWindowState = ConnectionWindowState.NotConnected;
					Game.Stop();
					break;
				}

			}
		}

		void HandleConnect() {
			StriveWindowState = ConnectionWindowState.Connected;
		}

		void HandleConnectFailed() {
			// TODO: might want to show a message etc
			StriveWindowState = ConnectionWindowState.NotConnected;
		}

		void HandleDisconnect() {
			StriveWindowState = ConnectionWindowState.NotConnected;
		}

		/*** no longer on another thread
		void HandleCanPossessThreadSafe( Strive.Network.Messages.ToClient.CanPossess cp ) 
		{
			this.Invoke( new Engine.MessageProcessor.CanPossessHandler( HandleCanPossess ),
				new object [] { cp } );
		}
		*/

		void HandleCanPossess( Strive.Network.Messages.ToClient.CanPossess cp ) {
			if ( cp.possesable.Length < 1 ) {
				this.Text = "No characters available.";
				return;
			}
			StriveWindowState = ConnectionWindowState.Connected;
			Game.CurrentServerConnection.PossessMobile(cp.possesable[0].id);
			Game.CurrentPlayerID = cp.possesable[0].id;
			StriveWindowState = ConnectionWindowState.Playing;
		}


		private void loadRecentServers()
		{
			RecentConnections.Items.Clear();
			// Initialise Recent Servers
			foreach(DataRow serverRow in Settings.SettingsManager.RecentServers.Rows)
			{
				foreach(DataRow playerRow in serverRow.GetChildRows("ServerPlayers")) {
					ListViewItem li = new ListViewItem(  serverRow["serveraddress"].ToString() );
					li.SubItems.Add( serverRow["serverport"].ToString() );
					li.SubItems.Add( playerRow["emailaddress"].ToString() );
					li.Tag = new object[] {serverRow, playerRow};
					//RecentConnections.ImageList = Icons.IconManager.GlobalImageList;
					//li.ImageIndex = (int)Icons.AvailableIcons.Player;
					RecentConnections.Items.Add( li );
				}
			}
			RecentConnections.Refresh();
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
					timer1.Stop();
					ConnectionProgress.Visible = false;
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
					this.Text = "Connected";
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
					this.Text = "Connecting";
					break;

				}
				case ConnectionWindowState.Playing:
				{
					this.Text = "Playing";
					timer1.Stop();
					this.Close();
					break;
				}
				
			
			}
			Application.DoEvents();
		}

		private void RecentConnections_DoubleClick(object sender, System.EventArgs e)
		{
			ConnectNow_Click(sender, e);
		}

		private void RecentConnections_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
				ListViewItem lvi = RecentConnections.FocusedItem;
				if ( lvi.Tag != null ) 
				{
					ServerAddress.Text = (string)((DataRow)((object[])lvi.Tag)[0])["serveraddress"];
					PortNumber.Text = (string)((DataRow)((object[])lvi.Tag)[0])["serverport"];
					Strive.Network.Messages.NetworkProtocolType protocol = (Strive.Network.Messages.NetworkProtocolType)Enum.Parse(typeof(Strive.Network.Messages.NetworkProtocolType), (string)((DataRow)((object[])lvi.Tag)[0])["protocol"]);
					switch(protocol)
					{
						case Strive.Network.Messages.NetworkProtocolType.TcpOnly:
						{
							NetworkProtocolTypeTCP.Checked = true;
							break;
						}
						case Strive.Network.Messages.NetworkProtocolType.UdpAndTcp:
						{
							NetworkProtocolTypeUDP.Checked = true;
							break;
						}
					}
					Email.Text = (string)((DataRow)((object[])lvi.Tag)[1])["emailaddress"];
					Password.Text = (string)((DataRow)((object[])lvi.Tag)[1])["password"];
					return;
				}
			}
			catch(Exception ex)
			{
				Exception rx = new Exception("Your settings file may be corrupt", ex);
				Strive.Logging.Log.ErrorMessage(rx);
			}
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


		private void timer1_Tick(object sender, System.EventArgs e) {
			ConnectionProgress.PerformStep();
			if ( ConnectionProgress.Value == ConnectionProgress.Maximum ) {
				StriveWindowState = ConnectionWindowState.NotConnected;
			}
		}

	}
}
