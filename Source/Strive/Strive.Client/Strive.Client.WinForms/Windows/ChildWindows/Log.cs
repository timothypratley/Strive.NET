using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Strive.Client.WinForms.Windows.ChildWindows
{
	/// <summary>
	/// Summary description for Log.
	/// </summary>
	public class Log : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox LogOutput;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Log()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			LogOutput.Width = this.Width - 5;
			LogOutput.Height = this.Height - 25;

			//Strive.Logging.Log.SetLogOutput(LogOutput);

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
			this.LogOutput = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// LogOutput
			// 
			this.LogOutput.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.LogOutput.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.LogOutput.Multiline = true;
			this.LogOutput.Name = "LogOutput";
			this.LogOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.LogOutput.TabIndex = 0;
			this.LogOutput.Text = "";
			// 
			// Log
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 273);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.LogOutput});
			this.Name = "Log";
			this.Text = "Log";
			this.ResumeLayout(false);

		}
		#endregion

	}
}
