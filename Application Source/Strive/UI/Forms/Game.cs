using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

using Strive.Rendering;
using Strive.Rendering.Controls;
using Strive.Rendering.Models;
using Strive.Math3D;
using Strive.Network.Messages;

using Strive.UI.Forms.Controls.Html;

namespace Strive.UI.Forms
{
	/// <summary>
	/// Summary description for Game.
	/// </summary>
	public class Game : System.Windows.Forms.Form
	{
		internal Scene _scene = new Scene();
		internal bool _isRendering = false;
		private System.Windows.Forms.TabControl gameTabs;
		private System.Windows.Forms.TabPage InGameTab;
		internal System.Windows.Forms.PictureBox RenderTarget;
		private System.Windows.Forms.TabControl inGameTabs;
		private System.Windows.Forms.TabPage inGameSpells;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage inGameInventory;
		private System.Windows.Forms.ComboBox quickCommand;
		private System.Windows.Forms.TabPage NoteBoardTabs;
		private System.Windows.Forms.TabPage CharacterSheet;
		private System.Windows.Forms.Button GoCommand;
		private System.Windows.Forms.TextBox CommandText;
		private System.Windows.Forms.RichTextBox Communications;
		private System.Windows.Forms.Button butSpell1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Game()
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
			this.gameTabs = new System.Windows.Forms.TabControl();
			this.InGameTab = new System.Windows.Forms.TabPage();
			this.Communications = new System.Windows.Forms.RichTextBox();
			this.GoCommand = new System.Windows.Forms.Button();
			this.quickCommand = new System.Windows.Forms.ComboBox();
			this.inGameTabs = new System.Windows.Forms.TabControl();
			this.inGameSpells = new System.Windows.Forms.TabPage();
			this.butSpell1 = new System.Windows.Forms.Button();
			this.inGameInventory = new System.Windows.Forms.TabPage();
			this.CommandText = new System.Windows.Forms.TextBox();
			this.RenderTarget = new System.Windows.Forms.PictureBox();
			this.NoteBoardTabs = new System.Windows.Forms.TabPage();
			this.CharacterSheet = new System.Windows.Forms.TabPage();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.gameTabs.SuspendLayout();
			this.InGameTab.SuspendLayout();
			this.inGameTabs.SuspendLayout();
			this.inGameSpells.SuspendLayout();
			this.SuspendLayout();
			// 
			// gameTabs
			// 
			this.gameTabs.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.gameTabs.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
			this.gameTabs.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.InGameTab,
																				   this.NoteBoardTabs,
																				   this.CharacterSheet});
			this.gameTabs.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.gameTabs.HotTrack = true;
			this.gameTabs.Location = new System.Drawing.Point(3, 8);
			this.gameTabs.Name = "gameTabs";
			this.gameTabs.SelectedIndex = 0;
			this.gameTabs.ShowToolTips = true;
			this.gameTabs.Size = new System.Drawing.Size(1016, 680);
			this.gameTabs.TabIndex = 0;
			// 
			// InGameTab
			// 
			this.InGameTab.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.Communications,
																					this.GoCommand,
																					this.quickCommand,
																					this.inGameTabs,
																					this.CommandText,
																					this.RenderTarget});
			this.InGameTab.Location = new System.Drawing.Point(4, 28);
			this.InGameTab.Name = "InGameTab";
			this.InGameTab.Size = new System.Drawing.Size(1008, 648);
			this.InGameTab.TabIndex = 0;
			this.InGameTab.Text = "In Game";
			this.InGameTab.ToolTipText = "Displays your current in game view";
			// 
			// Communications
			// 
			this.Communications.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.Communications.Location = new System.Drawing.Point(8, 520);
			this.Communications.Name = "Communications";
			this.Communications.ReadOnly = true;
			this.Communications.Size = new System.Drawing.Size(800, 88);
			this.Communications.TabIndex = 5;
			this.Communications.Text = "";
			// 
			// GoCommand
			// 
			this.GoCommand.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.GoCommand.Font = new System.Drawing.Font("Trebuchet MS", 8.25F);
			this.GoCommand.Location = new System.Drawing.Point(920, 616);
			this.GoCommand.Name = "GoCommand";
			this.GoCommand.TabIndex = 3;
			this.GoCommand.Text = "Go >";
			this.GoCommand.Click += new System.EventHandler(this.GoCommand_Click);
			// 
			// quickCommand
			// 
			this.quickCommand.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.quickCommand.Font = new System.Drawing.Font("Trebuchet MS", 8.25F);
			this.quickCommand.Items.AddRange(new object[] {
															  "cast",
															  "chat",
															  "ctalk",
															  "say",
															  "sleep"});
			this.quickCommand.Location = new System.Drawing.Point(8, 616);
			this.quickCommand.Name = "quickCommand";
			this.quickCommand.Size = new System.Drawing.Size(121, 24);
			this.quickCommand.Sorted = true;
			this.quickCommand.TabIndex = 1;
			this.quickCommand.Leave += new System.EventHandler(this.Complete_quickCommand);
			// 
			// inGameTabs
			// 
			this.inGameTabs.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.inGameTabs.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
			this.inGameTabs.Controls.AddRange(new System.Windows.Forms.Control[] {
																					 this.inGameSpells,
																					 this.inGameInventory});
			this.inGameTabs.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.inGameTabs.HotTrack = true;
			this.inGameTabs.ItemSize = new System.Drawing.Size(0, 24);
			this.inGameTabs.Location = new System.Drawing.Point(810, 8);
			this.inGameTabs.Multiline = true;
			this.inGameTabs.Name = "inGameTabs";
			this.inGameTabs.SelectedIndex = 0;
			this.inGameTabs.Size = new System.Drawing.Size(200, 600);
			this.inGameTabs.TabIndex = 4;
			// 
			// inGameSpells
			// 
			this.inGameSpells.Controls.AddRange(new System.Windows.Forms.Control[] {
																					   this.butSpell1});
			this.inGameSpells.Location = new System.Drawing.Point(4, 28);
			this.inGameSpells.Name = "inGameSpells";
			this.inGameSpells.Size = new System.Drawing.Size(192, 568);
			this.inGameSpells.TabIndex = 0;
			this.inGameSpells.Text = "Spells";
			this.inGameSpells.ToolTipText = "Your spells";
			// 
			// butSpell1
			// 
			this.butSpell1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.butSpell1.Location = new System.Drawing.Point(8, 8);
			this.butSpell1.Name = "butSpell1";
			this.butSpell1.Size = new System.Drawing.Size(176, 23);
			this.butSpell1.TabIndex = 0;
			this.butSpell1.Text = "Acid Blast";
			// 
			// inGameInventory
			// 
			this.inGameInventory.Location = new System.Drawing.Point(4, 28);
			this.inGameInventory.Name = "inGameInventory";
			this.inGameInventory.Size = new System.Drawing.Size(192, 568);
			this.inGameInventory.TabIndex = 1;
			this.inGameInventory.Text = "Inventory";
			// 
			// CommandText
			// 
			this.CommandText.Anchor = ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.CommandText.Font = new System.Drawing.Font("Trebuchet MS", 8.25F);
			this.CommandText.Location = new System.Drawing.Point(136, 616);
			this.CommandText.Name = "CommandText";
			this.CommandText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.CommandText.Size = new System.Drawing.Size(776, 20);
			this.CommandText.TabIndex = 2;
			this.CommandText.Text = "";
			// 
			// RenderTarget
			// 
			this.RenderTarget.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.RenderTarget.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.RenderTarget.Location = new System.Drawing.Point(8, 8);
			this.RenderTarget.Name = "RenderTarget";
			this.RenderTarget.Size = new System.Drawing.Size(800, 500);
			this.RenderTarget.TabIndex = 1;
			this.RenderTarget.TabStop = false;
			// 
			// NoteBoardTabs
			// 
			this.NoteBoardTabs.Location = new System.Drawing.Point(4, 28);
			this.NoteBoardTabs.Name = "NoteBoardTabs";
			this.NoteBoardTabs.Size = new System.Drawing.Size(1008, 648);
			this.NoteBoardTabs.TabIndex = 1;
			this.NoteBoardTabs.Text = "Note Boards";
			this.NoteBoardTabs.ToolTipText = "View in game ntoe boards";
			// 
			// CharacterSheet
			// 
			this.CharacterSheet.Location = new System.Drawing.Point(4, 28);
			this.CharacterSheet.Name = "CharacterSheet";
			this.CharacterSheet.Size = new System.Drawing.Size(1008, 648);
			this.CharacterSheet.TabIndex = 2;
			this.CharacterSheet.Text = "Character Sheet";
			// 
			// tabPage1
			// 
			this.tabPage1.Location = new System.Drawing.Point(4, 4);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(90, 592);
			this.tabPage1.TabIndex = 1;
			this.tabPage1.Text = "Inventory";
			// 
			// Game
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(1016, 694);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.gameTabs});
			this.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Name = "Game";
			this.Text = "Game";
			this.Load += new System.EventHandler(this.Game_Load);
			this.Closed += new System.EventHandler(this.Game_Unload);
			this.gameTabs.ResumeLayout(false);
			this.InGameTab.ResumeLayout(false);
			this.inGameTabs.ResumeLayout(false);
			this.inGameSpells.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void Game_Load(object sender, System.EventArgs e)
		{
			_scene.View.FieldOfView = 60;
			_scene.View.ViewDistance = 200;
			_scene.View.Position = new Vector3D(0, -50, 70);
		
			Modules.GameLoop.Start(_scene, RenderTarget, Global._serverConnection);

			Thread renderer = new Thread(new ThreadStart(Modules.GameLoop.Main));
			renderer.Start();
			
			
		}

		private void Game_Unload(object sender, System.EventArgs e)
		{
			_isRendering = false;			
		}

		private void Complete_quickCommand(object sender, System.EventArgs e)
		{
			quickCommand.SelectedIndex = quickCommand.FindString(quickCommand.Text,0);
		}

		private void processCommand(string command, string commandParams)
		{
			CommunicationType c;

			switch(command)
			{
				case "chat":
				{
					displayChat("You", command + " " + commandParams);
					c = CommunicationType.Chat;
					Global._serverConnection.Send(new Strive.Network.Messages.ToServer.GameCommand.Communication(c, commandParams));
					break;
				}
			}


		}

		public void displayChat(string from, string message)
		{
			Communications.Font = new Font("Trebuchet MS", 8, FontStyle.Bold);
			Communications.ForeColor = Color.Fuchsia;
			Communications.Text += from + " CHAT: ";
					
			Communications.Font = new Font("Trebuchet MS", 8, FontStyle.Regular);
			Communications.Text += message + System.Environment.NewLine;
		}

		private void GoCommand_Click(object sender, System.EventArgs e)
		{
			processCommand(quickCommand.Text, CommandText.Text);
		}
	}
}
