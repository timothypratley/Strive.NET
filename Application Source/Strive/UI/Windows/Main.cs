using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using Crownwood.Magic;
using Crownwood.Magic.Common;
using Crownwood.Magic.Controls;
using Crownwood.Magic.Docking;


using System.Windows.Forms;


namespace Strive.UI.Windows
{
	public class Main : Strive.UI.Windows.SystemFormBase
	{
		private Crownwood.Magic.Controls.TabControl MainTabs;
		private Crownwood.Magic.Controls.TabPage GameTab;
		protected Crownwood.Magic.Docking.DockingManager DockingManager;
		private Crownwood.Magic.Menus.MenuControl MainMenu;
		private Crownwood.Magic.Menus.MenuCommand FileMenu;
		private System.Windows.Forms.StatusBar MainStatus;
		public System.Windows.Forms.PictureBox RenderTarget;
		private Crownwood.Magic.Menus.MenuCommand ViewMenu;
		private Crownwood.Magic.Menus.MenuCommand ViewCharacters;
		private Crownwood.Magic.Menus.MenuCommand FileQuit;
		private Crownwood.Magic.Menus.MenuCommand FileSave;
		private Crownwood.Magic.Menus.MenuCommand ViewLog;
		private Crownwood.Magic.Menus.MenuCommand ViewConnect;

		private System.ComponentModel.IContainer components = null;

		public Main()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			// Double Bufferring to stop flicker
			SetStyle(ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);

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
			Content connectionWindow = DockingManager.Contents.Add(new ChildWindows.Connection(), "Connection", Icons.IconManager.GetAsImageList(Icons.AvailableIcons.Connection), 0);
			connectionWindow.DisplaySize = new Size(200, GameTab.Height);
			connectionWindow.CaptionBar = true;
			connectionWindow.CloseButton = false;
			DockingManager.AddContentWithState(connectionWindow, State.DockLeft);
			// Log
			Content logWindow = DockingManager.Contents.Add(new ChildWindows.Log(), "Log", new ImageList(), -1);
			DockingManager.AddContentWithState(logWindow, State.DockBottom);

			#endregion

			#region Load Settings

			if(System.IO.File.Exists(Settings.SettingsManager.MagicWindowSettingsPath))
			{
				DockingManager.LoadConfigFromFile(Settings.SettingsManager.MagicWindowSettingsPath);
				Game.CurrentLog.LogMessage("Loaded Magic Window config from '" + Settings.SettingsManager.MagicWindowSettingsPath + "'");
			}

			Settings.SettingsManager.InitialiseWindow(this);

			#endregion

			#endregion
			

		}

		#region Window Management Stuff
	


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
			this.MainTabs = new Crownwood.Magic.Controls.TabControl();
			this.GameTab = new Crownwood.Magic.Controls.TabPage();
			this.RenderTarget = new System.Windows.Forms.PictureBox();
			this.MainMenu = new Crownwood.Magic.Menus.MenuControl();
			this.FileMenu = new Crownwood.Magic.Menus.MenuCommand();
			this.FileSave = new Crownwood.Magic.Menus.MenuCommand();
			this.FileQuit = new Crownwood.Magic.Menus.MenuCommand();
			this.ViewMenu = new Crownwood.Magic.Menus.MenuCommand();
			this.ViewCharacters = new Crownwood.Magic.Menus.MenuCommand();
			this.MainStatus = new System.Windows.Forms.StatusBar();
			this.ViewLog = new Crownwood.Magic.Menus.MenuCommand();
			this.ViewConnect = new Crownwood.Magic.Menus.MenuCommand();
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
			this.MainTabs.Size = new System.Drawing.Size(596, 538);
			this.MainTabs.TabIndex = 0;
			this.MainTabs.TabPages.AddRange(new Crownwood.Magic.Controls.TabPage[] {
																					   this.GameTab});
			// 
			// GameTab
			// 
			this.GameTab.Controls.AddRange(new System.Windows.Forms.Control[] {
																				  this.RenderTarget});
			this.GameTab.Name = "GameTab";
			this.GameTab.Size = new System.Drawing.Size(596, 513);
			this.GameTab.TabIndex = 0;
			this.GameTab.Title = "Game";
			// 
			// RenderTarget
			// 
			this.RenderTarget.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.RenderTarget.Location = new System.Drawing.Point(256, 224);
			this.RenderTarget.Name = "RenderTarget";
			this.RenderTarget.TabIndex = 0;
			this.RenderTarget.TabStop = false;
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
			this.MainMenu.Size = new System.Drawing.Size(596, 25);
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
																							this.ViewCharacters});
			this.ViewMenu.Text = "&View";
			// 
			// ViewCharacters
			// 
			this.ViewCharacters.Description = "MenuItem";
			this.ViewCharacters.Text = "Characters";
			this.ViewCharacters.Click += new System.EventHandler(this.ViewCharacters_Click);
			// 
			// MainStatus
			// 
			this.MainStatus.Location = new System.Drawing.Point(2, 565);
			this.MainStatus.Name = "MainStatus";
			this.MainStatus.Size = new System.Drawing.Size(596, 22);
			this.MainStatus.TabIndex = 1;
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
			// Main
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.ClientSize = new System.Drawing.Size(600, 589);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.MainTabs,
																		  this.MainMenu,
																		  this.MainStatus});
			this.DockPadding.All = 2;
			this.Name = "Main";
			this.Text = "";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.Main_Closing);
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
			Game.CurrentLog.LogMessage("Saved Magic Window config to '" + Settings.SettingsManager.MagicWindowSettingsPath + "'");
			Settings.SettingsManager.SaveWindowSetting(this);			
			Settings.SettingsManager.PersistStriveSettings();
			Game.CurrentLog.LogMessage("Saved Strive config to '" + Settings.SettingsManager.StriveSettingsPath + "'");
		}

		private void Main_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			DialogResult r = MessageBox.Show(this, "Save Settings?", "Quitting", MessageBoxButtons.YesNoCancel) ;
			if(r == DialogResult.Cancel)
			{
				e.Cancel = true;
				return;
			}
			if(r == DialogResult.Yes)
			{
				saveSettings();
			}
			Game.Stop();

		}

		private void FileQuit_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void ViewCharacters_Click(object sender, System.EventArgs e) {
			// Log
			Content characterWindow = DockingManager.Contents.Add(new ChildWindows.CharacterSelector(), "Character", new ImageList(), -1);
			DockingManager.AddContentWithState(characterWindow, State.Floating);
		}

		private void ViewConnect_Click(object sender, System.EventArgs e) {
			// Connection
			Content connectionWindow = DockingManager.Contents.Add(new ChildWindows.Connection(), "Connection", Icons.IconManager.GetAsImageList(Icons.AvailableIcons.Connection), 0);
			connectionWindow.DisplaySize = new Size(200, GameTab.Height);
			connectionWindow.CaptionBar = true;
			connectionWindow.CloseButton = false;
			DockingManager.AddContentWithState(connectionWindow, State.DockLeft);
		}

		private void ViewLog_Click(object sender, System.EventArgs e) {
			// Log
			Content logWindow = DockingManager.Contents.Add(new ChildWindows.Log(), "Log", new ImageList(), -1);
			DockingManager.AddContentWithState(logWindow, State.DockBottom);
		}

	}
}

