using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using Crownwood.Magic;
using Crownwood.Magic.Common;
using Crownwood.Magic.Controls;
using Crownwood.Magic.Docking;

using Strive.Logging;
using Strive.UI.WorldView;
using Strive.Multiverse;


namespace Strive.UI.Windows
{
	public class Main : Strive.UI.Windows.SystemFormBase
	{
		public Crownwood.Magic.Controls.TabControl MainTabs;
		public Crownwood.Magic.Controls.TabPage GameTab;
		protected Crownwood.Magic.Docking.DockingManager DockingManager;
		private Crownwood.Magic.Menus.MenuControl MainMenu;
		private Crownwood.Magic.Menus.MenuCommand FileMenu;
		private System.Windows.Forms.StatusBar MainStatus;
		private Crownwood.Magic.Menus.MenuCommand ViewMenu;
		private Crownwood.Magic.Menus.MenuCommand FileQuit;
		private Crownwood.Magic.Menus.MenuCommand FileSave;
		private Crownwood.Magic.Menus.MenuCommand ViewLog;
		private Crownwood.Magic.Menus.MenuCommand ViewConnect;
		private Crownwood.Magic.Menus.MenuCommand ViewWhoList;
		private Crownwood.Magic.Menus.MenuCommand ViewSkillList;
		private Crownwood.Magic.Menus.MenuCommand ViewCommand;
		private Crownwood.Magic.Menus.MenuCommand ViewChat;
		private Crownwood.Magic.Menus.MenuCommand ViewFirstPerson;
		private Crownwood.Magic.Menus.MenuCommand ViewChaseCam;
		public System.Windows.Forms.Panel RenderTarget;
		public ChildWindows.MiniMap miniMap;

		private Crownwood.Magic.Menus.MenuCommand ViewMiniMap;

		private System.ComponentModel.IContainer components = null;

		public Main()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			// Double Bufferring to stop flicker
			SetStyle(ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
		}

		

		#region Window Management Stuff
	
		private void loadSettings()
		{
			if(System.IO.File.Exists(Settings.SettingsManager.MagicWindowSettingsPath))
			{
				DockingManager.LoadConfigFromFile(Settings.SettingsManager.MagicWindowSettingsPath);
				Log.LogMessage("Loaded Magic Window config from '" + Settings.SettingsManager.MagicWindowSettingsPath + "'");
			}

			Settings.SettingsManager.InitialiseWindow(this);
		}


		#endregion

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Main));
			this.MainTabs = new Crownwood.Magic.Controls.TabControl();
			this.GameTab = new Crownwood.Magic.Controls.TabPage();
			// TODO: would be nice to use something that doesn't re-paint itself
			this.RenderTarget = new System.Windows.Forms.Panel();
			this.MainMenu = new Crownwood.Magic.Menus.MenuControl();
			this.FileMenu = new Crownwood.Magic.Menus.MenuCommand();
			this.FileSave = new Crownwood.Magic.Menus.MenuCommand();
			this.FileQuit = new Crownwood.Magic.Menus.MenuCommand();
			this.ViewMenu = new Crownwood.Magic.Menus.MenuCommand();
			this.ViewLog = new Crownwood.Magic.Menus.MenuCommand();
			this.ViewConnect = new Crownwood.Magic.Menus.MenuCommand();
			this.ViewWhoList = new Crownwood.Magic.Menus.MenuCommand();
			this.ViewSkillList = new Crownwood.Magic.Menus.MenuCommand();
			this.ViewChat = new Crownwood.Magic.Menus.MenuCommand();
			this.ViewCommand = new Crownwood.Magic.Menus.MenuCommand();
			this.ViewFirstPerson = new Crownwood.Magic.Menus.MenuCommand();
			this.ViewChaseCam = new Crownwood.Magic.Menus.MenuCommand();
			this.ViewMiniMap = new Crownwood.Magic.Menus.MenuCommand();
			this.MainStatus = new System.Windows.Forms.StatusBar();
			this.GameTab.SuspendLayout();
			this.SuspendLayout();
			// 
			// MainTabs
			// 
			this.MainTabs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MainTabs.Location = new System.Drawing.Point(2, 27);
			this.MainTabs.Name = "MainTabs";
			this.MainTabs.SelectedIndex = 0;
			this.MainTabs.SelectedTab = this.GameTab;
			this.MainTabs.Size = new System.Drawing.Size(888, 626);
			this.MainTabs.TabIndex = 0;
			this.MainTabs.TabPages.AddRange(new Crownwood.Magic.Controls.TabPage[] {
																					   this.GameTab});
			// 
			// GameTab
			// 
			this.GameTab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.GameTab.Controls.Add(this.RenderTarget);
			this.GameTab.Location = new System.Drawing.Point(0, 0);
			this.GameTab.Name = "GameTab";
			this.GameTab.Size = new System.Drawing.Size(888, 601);
			this.GameTab.TabIndex = 0;
			this.GameTab.Title = "Game";
			// 
			// RenderTarget
			// 
			this.RenderTarget.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.RenderTarget.Location = new System.Drawing.Point(0, 0);
			this.RenderTarget.Name = "RenderTarget";
			this.RenderTarget.Size = new System.Drawing.Size(800, 454);
			this.RenderTarget.TabIndex = 1;
			// 
			// MainMenu
			// 
			this.MainMenu.AnimateStyle = Crownwood.Magic.Menus.Animation.System;
			this.MainMenu.AnimateTime = 100;
			this.MainMenu.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.MainMenu.Direction = Crownwood.Magic.Common.Direction.Horizontal;
			this.MainMenu.Dock = System.Windows.Forms.DockStyle.Top;
			this.MainMenu.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.MainMenu.HighlightTextColor = System.Drawing.SystemColors.MenuText;
			this.MainMenu.Location = new System.Drawing.Point(2, 2);
			this.MainMenu.MenuCommands.AddRange(new Crownwood.Magic.Menus.MenuCommand[] {
																							this.FileMenu,
																							this.ViewMenu});
			this.MainMenu.Name = "MainMenu";
			this.MainMenu.Size = new System.Drawing.Size(888, 25);
			this.MainMenu.Style = Crownwood.Magic.Common.VisualStyle.IDE;
			this.MainMenu.TabIndex = 0;
			this.MainMenu.TabStop = false;
			// 
			// FileMenu
			// 
			this.FileMenu.Description = "The File menu";
			this.FileMenu.MenuCommands.AddRange(new Crownwood.Magic.Menus.MenuCommand[] {
																							this.FileSave,
																							this.FileQuit});
			this.FileMenu.Text = "&File";
			// 
			// FileSave
			// 
			this.FileSave.Description = "MenuItem";
			this.FileSave.Text = "Save";
			this.FileSave.Click += new System.EventHandler(this.FileSave_Click);
			// 
			// FileQuit
			// 
			this.FileQuit.Description = "MenuItem";
			this.FileQuit.Text = "Quit";
			this.FileQuit.Click += new System.EventHandler(this.FileQuit_Click);
			// 
			// ViewMenu
			// 
			this.ViewMenu.Description = "MenuItem";
			this.ViewMenu.MenuCommands.AddRange(new Crownwood.Magic.Menus.MenuCommand[] {
																							this.ViewLog,
																							this.ViewConnect,
																							this.ViewWhoList,
																							this.ViewSkillList,
																							this.ViewChat,
																							this.ViewCommand,
																							this.ViewFirstPerson,
																							this.ViewChaseCam,
																							this.ViewMiniMap});
			this.ViewMenu.Text = "&View";
			// 
			// ViewLog
			// 
			this.ViewLog.Description = "MenuItem";
			this.ViewLog.Text = "Log";
			this.ViewLog.Click += new System.EventHandler(this.ViewLog_Click);
			// 
			// ViewConnect
			// 
			this.ViewConnect.Description = "MenuItem";
			this.ViewConnect.Text = "Connect";
			this.ViewConnect.Click += new System.EventHandler(this.ViewConnect_Click);
			// 
			// ViewWhoList
			// 
			this.ViewWhoList.Description = "MenuItem";
			this.ViewWhoList.Text = "Who";
			this.ViewWhoList.Click += new System.EventHandler(this.ViewWhoList_Click);
			// 
			// ViewSkillList
			// 
			this.ViewSkillList.Description = "MenuItem";
			this.ViewSkillList.Text = "Skills";
			this.ViewSkillList.Click += new System.EventHandler(this.ViewSkillList_Click);
			// 
			// ViewChat
			// 
			this.ViewChat.Description = "View Chat";
			this.ViewChat.Text = "Chat";
			this.ViewChat.Click += new System.EventHandler(this.ViewChat_Click);
			// 
			// ViewCommand
			// 
			this.ViewCommand.Description = "View Command";
			this.ViewCommand.Text = "Command";
			this.ViewCommand.Click += new System.EventHandler(this.ViewCommand_Click);
			// 
			// ViewFirstPerson
			// 
			this.ViewFirstPerson.Description = "First Person Mode";
			this.ViewFirstPerson.Text = "First Person Mode";
			this.ViewFirstPerson.Click += new System.EventHandler(this.ViewFirstPerson_Click);
			// 
			// ViewChaseCam
			// 
			this.ViewChaseCam.Description = "Chase Camera";
			this.ViewChaseCam.Text = "Chase Camera";
			this.ViewChaseCam.Click += new System.EventHandler(this.ViewChaseCam_Click);
			// 
			// ViewMiniMap
			// 
			this.ViewMiniMap.Description = "MenuItem";
			this.ViewMiniMap.Text = "Mini Map";
			this.ViewMiniMap.Click += new System.EventHandler(this.ViewMiniMap_Click);
			// 
			// MainStatus
			// 
			this.MainStatus.Location = new System.Drawing.Point(2, 653);
			this.MainStatus.Name = "MainStatus";
			this.MainStatus.Size = new System.Drawing.Size(888, 98);
			this.MainStatus.TabIndex = 1;
			// 
			// Main
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(892, 753);
			this.Controls.Add(this.MainTabs);
			this.Controls.Add(this.MainMenu);
			this.Controls.Add(this.MainStatus);
			this.DockPadding.All = 2;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Main";
			this.Text = "";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Main_KeyDown);
			this.Closing += new System.ComponentModel.CancelEventHandler(this.Main_Closing);
			this.Load += new System.EventHandler(this.Load_Form);
			this.GameTab.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void FileSave_Click(object sender, System.EventArgs e)
		{
			saveSettings();
		}

		private void saveSettings()
		{
			DockingManager.SaveConfigToFile(Settings.SettingsManager.MagicWindowSettingsPath);
			Log.LogMessage("Saved Magic Window config to '" + Settings.SettingsManager.MagicWindowSettingsPath + "'");
			Settings.SettingsManager.SaveWindowSetting(this);			
			Settings.SettingsManager.PersistStriveSettings();
			Log.LogMessage("Saved Strive config to '" + Settings.SettingsManager.StriveSettingsPath + "'");
		}

		private void Main_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			saveSettings();
		}

		private void FileQuit_Click(object sender, System.EventArgs e) {
			this.Close();
		}

		private void ViewConnect_Click(object sender, System.EventArgs e) {
			// Connection
			Game.Stop();
			ChildWindows.Connection con = new ChildWindows.Connection();
			con.ShowDialog(this);
		}

		private void ViewLog_Click(object sender, System.EventArgs e) {
			// Log
			Content logWindow = DockingManager.Contents.Add(new ChildWindows.Log(), "Log", new ImageList(), -1);
			DockingManager.AddContentWithState(logWindow, State.DockBottom);
		}

		private void RenderTarget_LostFocus( object sender, System.EventArgs e ) {
			ReleaseGameControlMode();
		}

		private void RenderTarget_Click( object sender, System.EventArgs e ) {
			if ( Game.CurrentWorld.CurrentAvatar == null ) 
			{
				if ( Game.GameControlMode ) 
				{
					ReleaseGameControlMode();
				}
				return;
			}
			if ( Game.GameControlMode ) 
			{
				if ( Game.CurrentGameCommand == EnumSkill.None ) 
				{
					return;
				}
				//ReleaseGameControlMode();
				EnumSkill skill = Game.CurrentGameCommand;
                //if ( targettypeof( skill ) == targetmobil ) {
				Game.RenderingFactory.Mouse.GetAbsState();
				Strive.Rendering.Models.IModel m = Game.CurrentWorld.RenderingScene.MousePick();
				if ( m == null ) 
				{
					return;
				}
				PhysicalObjectInstance poi = Game.CurrentWorld.physicalObjectInstances[int.Parse(m.Name)] as PhysicalObjectInstance;
				if ( poi == null ) 
				{
					return;
				}
				int [] targets = new int[1];
				targets[0] = poi.physicalObject.ObjectInstanceID;
				// TODO: use InvokationID to track casting and allow for cancelations
				Game.CurrentServerConnection.Send(
					new Strive.Network.Messages.ToServer.GameCommand.UseSkill(
						Game.CurrentGameCommand, 0, targets )
				);
				Log.LogMessage( "Using skill " + Game.CurrentGameCommand + " on " + m.Name );
			} else {
				SetGameControlMode();
			}
		}

		public void SetGameControlMode() {
			if ( !Game.GameControlMode ) 
			{
				//Game.RenderingFactory.FullScreen = true;
				RenderTarget.Focus();
				Cursor.Clip = RenderTarget.RectangleToScreen( RenderTarget.ClientRectangle );
				Game.RenderingFactory.Mouse.ShowCursor(false);
				Game.CurrentGameCommand = EnumSkill.None;
				Game.GameControlMode = true;
			}
		}
		
		public void ReleaseGameControlMode() {
			if ( Game.GameControlMode ) {
				//Game.RenderingFactory.FullScreen = false;
				Cursor.Clip = new Rectangle( 0, 0, 0, 0 );
				Game.RenderingFactory.Mouse.ShowCursor(true);
				Game.GameControlMode = false;
			}
		}

		private void ViewWhoList_Click(object sender, System.EventArgs e) {
			Content whoWindow = DockingManager.Contents.Add(new ChildWindows.WhoList(), "Who's online", Icons.IconManager.GlobalImageList,-1);
			DockingManager.AddContentWithState(whoWindow, State.Floating);
		}

		private void ViewSkillList_Click(object sender, System.EventArgs e) {
			Content skillWindow = DockingManager.Contents.Add(new ChildWindows.SkillSelector(), "Skills", Icons.IconManager.GlobalImageList, -1);
			DockingManager.AddContentWithState(skillWindow, State.Floating);
		}

		private void ViewCommand_Click(object sender, System.EventArgs e)
		{
			Content commandWindow = DockingManager.Contents.Add(new ChildWindows.Command(), "Command", null, -1);
			DockingManager.AddContentWithState(commandWindow, State.Floating);
		
		}

		private void ViewChat_Click(object sender, System.EventArgs e)
		{
			Content chatWindow = DockingManager.Contents.Add(new ChildWindows.Chat(), "Chat", null, -1);
			DockingManager.AddContentWithState(chatWindow, State.Floating);
		}

		private void Load_Form(object sender, System.EventArgs e)
		{
			#region Magic Controls workarounds

			// Setting this using designer causes a Code Generation Error
			MainTabs.Appearance = Crownwood.Magic.Controls.TabControl.VisualAppearance.MultiDocument;
			// Setting this using designer appears to have no affect
			MainTabs.IDEPixelBorder = true;

			// Order is important for docking to work.  This isn't really a Magic Controls
			// issue, but related to how Winforms processes docking events.
			// Since InitializeComponent is GENERATED code, the form control collection gets cleared
			// and then the controls are added in the correct order;
			this.Controls.Clear();

			this.Controls.Add(MainTabs);
			this.Controls.Add(MainStatus);
			this.Controls.Add(MainMenu);
			
			#endregion

			#region Magic Controls Initialisation

			DockingManager = new DockingManager(this, VisualStyle.IDE);
			
			DockingManager.InnerControl = MainTabs;
			DockingManager.OuterControl = MainStatus;

			#endregion

			#region Our Initialisation
			
			#region RenderContainer

			RenderTarget.Left = GameTab.Left;
			RenderTarget.Top = GameTab.Top;
			RenderTarget.Height = GameTab.Height;
			RenderTarget.Width = GameTab.Width;

			#endregion

			#region Add our windows


			// Connection
//			Content connectionWindow = DockingManager.Contents.Add(new ChildWindows.Connection(), "Connection", Icons.IconManager.GlobalImageList, (int)Icons.AvailableIcons.Connection);
//			connectionWindow.DisplaySize = new Size(200, GameTab.Height);
//			connectionWindow.CaptionBar = true;
//			connectionWindow.CloseButton = false;
//			DockingManager.AddContentWithState(connectionWindow, State.DockLeft);
			// Log
			Content logWindow = DockingManager.Contents.Add(new ChildWindows.Log(), "Log", Icons.IconManager.GlobalImageList, (int)Icons.AvailableIcons.Log);
			DockingManager.AddContentWithState(logWindow, State.DockBottom);
			// Who
			Content whoWindow = DockingManager.Contents.Add(new ChildWindows.WhoList(), "Who's online", Icons.IconManager.GlobalImageList,-1);
			DockingManager.AddContentWithState(whoWindow, State.DockRight);
			// Command
			Content commandWindow = DockingManager.Contents.Add(new ChildWindows.Command(), "Command", Icons.IconManager.GlobalImageList, (int)Icons.AvailableIcons.Command);
			DockingManager.AddContentWithState(commandWindow, State.DockBottom);
			// Chat
			Content chatWindow = DockingManager.Contents.Add(new ChildWindows.Chat(), "Chat", Icons.IconManager.GlobalImageList, (int)Icons.AvailableIcons.Chat);
			DockingManager.AddContentWithState(chatWindow, State.DockBottom);
			// MiniMap
			miniMap = new ChildWindows.MiniMap();
			Content mapWindow = DockingManager.Contents.Add(miniMap, "Mini Map", Icons.IconManager.GlobalImageList,-1);
			DockingManager.AddContentWithState(mapWindow, State.DockBottom);

			
			
			this.Navigate("http://www.strive3d.net");


			#endregion

			loadSettings();

			#region Events
			RenderTarget.LostFocus += new EventHandler( RenderTarget_LostFocus );
			RenderTarget.Click += new EventHandler( RenderTarget_Click );
			#endregion

			#endregion		
		}

		public void Navigate(string url)
		{
			// navigates to the supplied url
			// Web site
			Windows.Controls.Html webpage = new Windows.Controls.Html(url);
			Crownwood.Magic.Controls.TabPage website = new Crownwood.Magic.Controls.TabPage(url, webpage);
			MainTabs.TabPages.Add(website);

		}

		private void ViewFirstPerson_Click(object sender, System.EventArgs e) {
			Game.CurrentWorld.CameraMode = EnumCameraMode.FirstPerson;
		}

		private void ViewChaseCam_Click(object sender, System.EventArgs e) {
			Game.CurrentWorld.CameraMode = EnumCameraMode.Chase;	
		}

		private void Main_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e) {
			// TODO: why doesn't this work???? umg
			// is it because of the hax directx keyboard handling?

			if ( e.KeyCode == Keys.F5 ) {
				SetGameControlMode();
			} else if ( e.KeyCode == Keys.F6 ) {
				ReleaseGameControlMode();
			}
			if ( Game.GameControlMode ) {
				if ( e.KeyCode == Keys.Escape ) {
					ReleaseGameControlMode();
				} else if ( e.KeyCode == Keys.D1 ) {
					Game.CurrentGameCommand = EnumSkill.Kill;
				} else if ( e.KeyCode == Keys.D0 ) {
					Game.CurrentGameCommand = EnumSkill.None;
				}
			}
		}

		private void ViewMiniMap_Click(object sender, System.EventArgs e) {
			if ( miniMap == null ) {
				miniMap = new ChildWindows.MiniMap();
			} else {
				Strive.Logging.Log.ErrorMessage( "Only one minimap allowed." );
				return;
			}
			//miniMap.Show();
			Content mapWindow = DockingManager.Contents.Add(miniMap, "Mini Map");
			DockingManager.AddContentWithState(mapWindow, State.Floating);
			Game.CurrentWorld.MiniMapTarget = miniMap.RenderTarget;
		}

	}
}

