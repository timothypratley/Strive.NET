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
			this.RenderTarget.Size = new System.Drawing.Size(304, 272);
			this.RenderTarget.TabIndex = 0;
			// 
			// MiniMap
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(304, 277);
			this.Controls.Add(this.RenderTarget);
			this.Name = "MiniMap";
			this.Text = "MiniMap";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
