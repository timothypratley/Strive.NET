using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Strive.UI.Forms
{
	/// <summary>
	/// Summary description for Acknowledgements.
	/// </summary>
	public class Acknowledgements : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox AcknowledgementsText;
		private System.Windows.Forms.Button Ok;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Acknowledgements()
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
			this.AcknowledgementsText = new System.Windows.Forms.TextBox();
			this.Ok = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// AcknowledgementsText
			// 
			this.AcknowledgementsText.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.AcknowledgementsText.Location = new System.Drawing.Point(8, 8);
			this.AcknowledgementsText.Multiline = true;
			this.AcknowledgementsText.Name = "AcknowledgementsText";
			this.AcknowledgementsText.Size = new System.Drawing.Size(280, 208);
			this.AcknowledgementsText.TabIndex = 0;
			this.AcknowledgementsText.Text = "";
			// 
			// Ok
			// 
			this.Ok.Anchor = ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.Ok.Location = new System.Drawing.Point(112, 232);
			this.Ok.Name = "Ok";
			this.Ok.Size = new System.Drawing.Size(72, 24);
			this.Ok.TabIndex = 1;
			this.Ok.Text = "&Ok";
			this.Ok.Click += new System.EventHandler(this.button1_Click);
			// 
			// Acknowledgements
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.ControlBox = false;
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.Ok,
																		  this.AcknowledgementsText});
			this.MaximizeBox = false;
			this.Name = "Acknowledgements";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Acknowledgements";
			this.Load += new System.EventHandler(this.Acknowledgements_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void Acknowledgements_Load(object sender, System.EventArgs e)
		{
			AcknowledgementsText.Text += "Aguido Davies";
			AcknowledgementsText.Text += System.Environment.NewLine + "Martin Dick";
			AcknowledgementsText.Text += System.Environment.NewLine + "Frank Ellis";
			AcknowledgementsText.Text += System.Environment.NewLine + "Kelly Grant";
			AcknowledgementsText.Text += System.Environment.NewLine + "Robert Houston";
			AcknowledgementsText.Text += System.Environment.NewLine + "Mark Morgan";
			AcknowledgementsText.Text += System.Environment.NewLine + "Steven Nagy";
			AcknowledgementsText.Text += System.Environment.NewLine + "Clint Rowbotham";
			AcknowledgementsText.Text += System.Environment.NewLine + "Aaron Sobey";
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}
	}
}
