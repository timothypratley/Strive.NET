using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using Strive.Logging;

namespace Strive.UI.Windows.Controls
{
	/// <summary>
	/// Responsible for rapid command entry, autocomplete etc.
	/// </summary>
	public class CommandInput : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.TextBox Command;
		private System.Windows.Forms.Button Go;
		private System.Windows.Forms.IButtonControl _cacheDefault;
		private ArrayList _previousCommands = new ArrayList();
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public CommandInput()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitForm call

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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Command = new System.Windows.Forms.TextBox();
			this.Go = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// Command
			// 
			this.Command.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.Command.Location = new System.Drawing.Point(0, 4);
			this.Command.Name = "Command";
			this.Command.Size = new System.Drawing.Size(521, 20);
			this.Command.TabIndex = 0;
			this.Command.Text = "";
			this.Command.Leave += new System.EventHandler(this.Command_Leave);
			this.Command.Enter += new System.EventHandler(this.Command_Enter);
			// 
			// Go
			// 
			this.Go.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.Go.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.Go.Location = new System.Drawing.Point(525, 4);
			this.Go.Name = "Go";
			this.Go.Size = new System.Drawing.Size(75, 20);
			this.Go.TabIndex = 1;
			this.Go.Text = "Execute";
			this.Go.Click += new System.EventHandler(this.Go_Click);
			// 
			// CommandInput
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.Go,
																		  this.Command});
			this.Name = "CommandInput";
			this.Size = new System.Drawing.Size(600, 28);
			this.ResumeLayout(false);

		}
		#endregion

		private void Command_Enter(object sender, System.EventArgs e)
		{
			// Stuffs to set the default winform button when the textbox gets focus
			Form parentForm = this.FindForm();
			if(parentForm != null)
			{
				if(parentForm.AcceptButton  != this.Go)
				{
					_cacheDefault = parentForm.AcceptButton;
					parentForm.AcceptButton = (IButtonControl)this.Go;
				}

			}
		}

		private void Command_Leave(object sender, System.EventArgs e)
		{
			Form parentForm = this.FindForm();
			if(parentForm != null)
			{
				parentForm.AcceptButton = _cacheDefault;
			}
		}

		private void Go_Click(object sender, System.EventArgs e)
		{
			// only clear the window if they typed a parseable command
			if(executeCommand(Command.Text))
			{
				Command.Clear();
			}
			
		}

		public bool executeCommand(string command)
		{
			// TODO: process the command

			// Log the command
			Log.LogMessage("Executed command '" + command + "'.");

			// Save the command for up-arrow completion
			_previousCommands.Add(command);

			return true;

		}

	}
}
