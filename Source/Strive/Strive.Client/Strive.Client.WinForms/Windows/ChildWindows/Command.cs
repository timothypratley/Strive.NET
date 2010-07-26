using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Strive.Client.WinForms.Windows.ChildWindows
{
	/// <summary>
	/// Summary description for Command.
	/// </summary>
	public class Command : System.Windows.Forms.Form
	{
		private Strive.Client.WinForms.Windows.Controls.CommandInput Input;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Command()
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
			this.Input = new Strive.Client.WinForms.Windows.Controls.CommandInput();
			this.SuspendLayout();
			// 
			// Input
			// 
			this.Input.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.Input.Name = "Input";
			this.Input.Size = new System.Drawing.Size(688, 28);
			this.Input.TabIndex = 0;
			// 
			// Command
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(688, 273);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.Input});
			this.Name = "Command";
			this.Text = "Command";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
