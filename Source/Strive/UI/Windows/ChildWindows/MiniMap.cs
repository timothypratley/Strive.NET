using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Strive.UI.Windows.ChildWindows
{
	/// <summary>
	/// Summary description for MiniMap.
	/// </summary>
	public class MiniMap : System.Windows.Forms.Form
	{
		public System.Windows.Forms.Panel RenderTarget;
		private System.Windows.Forms.StatusBarPanel X;
		private System.Windows.Forms.StatusBarPanel Y;
		private System.Windows.Forms.StatusBarPanel Z;
		private System.Windows.Forms.StatusBarPanel Information;
		private System.Windows.Forms.StatusBar Status;
		private System.Windows.Forms.StatusBarPanel RY;
		private System.Windows.Forms.StatusBarPanel Triangles;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MiniMap()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//

			Game.CurrentServerConnection.OnPositionSent += new Strive.Network.Client.ServerConnection.OnPositionSentHandler(MiniMap_Update);
			
		}

		private void MiniMap_Update(Strive.Network.Messages.ToServer.Position newPosition)
		{
			Z.Text = ((int)newPosition.position.Z).ToString();
			Y.Text = ((int)newPosition.position.Y).ToString();
			X.Text = ((int)newPosition.position.X).ToString();
			RY.Text = ((int)newPosition.rotation.Y).ToString();
            Triangles.Text = Game.CurrentWorld.RenderingScene.VisibleTriangleCount.ToString();
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
			this.RenderTarget = new System.Windows.Forms.Panel();
			this.Status = new System.Windows.Forms.StatusBar();
			this.Information = new System.Windows.Forms.StatusBarPanel();
			this.RY = new System.Windows.Forms.StatusBarPanel();
			this.X = new System.Windows.Forms.StatusBarPanel();
			this.Z = new System.Windows.Forms.StatusBarPanel();
			this.Y = new System.Windows.Forms.StatusBarPanel();
			this.Triangles = new System.Windows.Forms.StatusBarPanel();
			((System.ComponentModel.ISupportInitialize)(this.Information)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.RY)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.X)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.Z)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.Y)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.Triangles)).BeginInit();
			this.SuspendLayout();
			// 
			// RenderTarget
			// 
			this.RenderTarget.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.RenderTarget.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.RenderTarget.Location = new System.Drawing.Point(0, 0);
			this.RenderTarget.Name = "RenderTarget";
			this.RenderTarget.Size = new System.Drawing.Size(304, 248);
			this.RenderTarget.TabIndex = 0;
			// 
			// Status
			// 
			this.Status.Location = new System.Drawing.Point(0, 256);
			this.Status.Name = "Status";
			this.Status.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
																					  this.Information,
																					  this.RY,
																					  this.X,
																					  this.Z,
																					  this.Y,
																					  this.Triangles});
			this.Status.ShowPanels = true;
			this.Status.Size = new System.Drawing.Size(304, 22);
			this.Status.TabIndex = 1;
			// 
			// Information
			// 
			this.Information.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.Information.Width = 148;
			// 
			// RY
			// 
			this.RY.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			this.RY.ToolTipText = "Heading";
			this.RY.Width = 10;
			// 
			// X
			// 
			this.X.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			this.X.ToolTipText = "Latitude";
			this.X.Width = 10;
			// 
			// Z
			// 
			this.Z.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			this.Z.ToolTipText = "Longitude";
			this.Z.Width = 10;
			// 
			// Y
			// 
			this.Y.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			this.Y.ToolTipText = "Altitude";
			this.Y.Width = 10;
			// 
			// Triangles
			// 
			this.Triangles.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			this.Triangles.ToolTipText = "Triangles";
			this.Triangles.Width = 10;
			// 
			// MiniMap
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(304, 278);
			this.Controls.Add(this.Status);
			this.Controls.Add(this.RenderTarget);
			this.Name = "MiniMap";
			this.Text = "MiniMap";
			((System.ComponentModel.ISupportInitialize)(this.Information)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.RY)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.X)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.Z)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.Y)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.Triangles)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion
	}
}
