using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using Strive.Network.Messages;
using Strive.Rendering;


namespace Strive.UI.Forms
{
	public class Splash : Strive.UI.Forms.FormBase
	{
		private System.Windows.Forms.PictureBox SplashLogo;
		private System.Windows.Forms.Label StriveLabel;
		private System.Windows.Forms.Button Go;
		private System.Windows.Forms.ComboBox Resolutions;
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.ComboBox LoginNames;
//		private Strive.UI.Forms.Controls.Html.HtmlControl label1;

		private static Game game;

		public Splash()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
		}

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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Splash));
			this.SplashLogo = new System.Windows.Forms.PictureBox();
			this.StriveLabel = new System.Windows.Forms.Label();
			this.Resolutions = new System.Windows.Forms.ComboBox();
			this.Go = new System.Windows.Forms.Button();
			this.LoginNames = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// SplashLogo
			// 
			this.SplashLogo.Image = ((System.Drawing.Bitmap)(resources.GetObject("SplashLogo.Image")));
			this.SplashLogo.Location = new System.Drawing.Point(16, 8);
			this.SplashLogo.Name = "SplashLogo";
			this.SplashLogo.Size = new System.Drawing.Size(184, 232);
			this.SplashLogo.TabIndex = 0;
			this.SplashLogo.TabStop = false;
			// 
			// StriveLabel
			// 
			this.StriveLabel.Font = new System.Drawing.Font("Trebuchet MS", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.StriveLabel.Location = new System.Drawing.Point(208, 8);
			this.StriveLabel.Name = "StriveLabel";
			this.StriveLabel.Size = new System.Drawing.Size(264, 56);
			this.StriveLabel.TabIndex = 1;
			this.StriveLabel.Text = "Strive3D.Net";
			// 
			// Resolutions
			// 
			this.Resolutions.Items.AddRange(new object[] {
															 "640 X 480 (16bit)",
															 "640 X 480 (32bit)",
															 "800 X 600 (16bit)",
															 "800 X 600 (32bit)",
															 "1024 X 728 (16bit)",
															 "1024 X 728 (32bit)",
															 "1024 X 728 (16bit)",
															 "1280 X 1024 (16bit)",
															 "1280 X 1024 (32bit)",
															 "1600 X 1200 (16bit)",
															 "1600 X 1200 (32bit)"});
			this.Resolutions.Location = new System.Drawing.Point(208, 64);
			this.Resolutions.Name = "Resolutions";
			this.Resolutions.Size = new System.Drawing.Size(200, 24);
			this.Resolutions.TabIndex = 3;
			this.Resolutions.Visible = false;
			// 
			// Go
			// 
			this.Go.Location = new System.Drawing.Point(352, 128);
			this.Go.Name = "Go";
			this.Go.Size = new System.Drawing.Size(56, 24);
			this.Go.TabIndex = 2;
			this.Go.Text = "Go >";
			this.Go.Click += new System.EventHandler(this.Go_Click);
			// 
			// LoginNames
			// 
			this.LoginNames.Items.AddRange(new object[] {
															"timothypratley@yahoo.com",
															"nathan@rogers.name"});
			this.LoginNames.Location = new System.Drawing.Point(208, 96);
			this.LoginNames.Name = "LoginNames";
			this.LoginNames.Size = new System.Drawing.Size(200, 24);
			this.LoginNames.TabIndex = 1;
			this.LoginNames.Leave += new System.EventHandler(this.Complete_LoginNames);
			// 
			// Splash
			// 
			this.AcceptButton = this.Go;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(492, 266);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.LoginNames,
																		  this.Go,
																		  this.Resolutions,
																		  this.StriveLabel,
																		  this.SplashLogo});
			this.Name = "Splash";
			this.Load += new System.EventHandler(this.Splash_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void Splash_Load(object sender, System.EventArgs e)
		{
	

		}


		private void Go_Click(object sender, System.EventArgs e)
		{
			if(! (this.LoginNames.SelectedIndex < 0))
			{
				// EEERRR hardcoded player spawnids
				Global._myid = this.LoginNames.SelectedIndex == 1 ? 27 : 26;
				Global._serverConnection.Send(new Strive.Network.Messages.ToServer.Login(this.LoginNames.Text, this.LoginNames.Text));
				Global._serverConnection.Send(new Strive.Network.Messages.ToServer.EnterWorldAsMobile(0, Global._myid ));
				game = new Game();
				game._scene.Initialise(game.RenderTarget, RenderTarget.PictureBox, Resolution.Automatic);
				Global._game = game;
				game.Show();
			}

		
		}

		private void Complete_LoginNames(object sender, EventArgs e)
		{
			LoginNames.SelectedIndex = LoginNames.FindString(LoginNames.Text,0);
			if(LoginNames.SelectedIndex < 0)
			{
				LoginNames.Focus();
			}
		}
	}
}

