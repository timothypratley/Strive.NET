using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace Strive.Utils
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class WinMain : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button LoadXSDGenerator;
		private System.Windows.Forms.Button WorldBuilderButton;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public WinMain()
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
				if (components != null) 
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
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.LoadXSDGenerator = new System.Windows.Forms.Button();
			this.WorldBuilderButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(8, 16);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(144, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "Command Generator";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(160, 16);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(144, 23);
			this.button2.TabIndex = 1;
			this.button2.Text = "Stored Procedure UI";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// LoadXSDGenerator
			// 
			this.LoadXSDGenerator.Location = new System.Drawing.Point(312, 16);
			this.LoadXSDGenerator.Name = "LoadXSDGenerator";
			this.LoadXSDGenerator.Size = new System.Drawing.Size(88, 23);
			this.LoadXSDGenerator.TabIndex = 2;
			this.LoadXSDGenerator.Text = "XSD Generator";
			this.LoadXSDGenerator.Click += new System.EventHandler(this.LoadXSDGenerator_Click);
			// 
			// WorldBuilderButton
			// 
			this.WorldBuilderButton.Location = new System.Drawing.Point(8, 48);
			this.WorldBuilderButton.Name = "WorldBuilderButton";
			this.WorldBuilderButton.Size = new System.Drawing.Size(88, 23);
			this.WorldBuilderButton.TabIndex = 3;
			this.WorldBuilderButton.Text = "World Builder";
			this.WorldBuilderButton.Click += new System.EventHandler(this.WorldBuilderButton_Click);
			// 
			// WinMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(440, 126);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.WorldBuilderButton,
																		  this.LoadXSDGenerator,
																		  this.button2,
																		  this.button1});
			this.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Name = "WinMain";
			this.Text = "Strive.Utils";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new WinMain());
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			CommandGenerator.WinMain win = new CommandGenerator.WinMain();
			win.ShowDialog(this);
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			StoredProcedureUI.WinMain win = new StoredProcedureUI.WinMain();
			win.ShowDialog(this);
		}

		private void LoadXSDGenerator_Click(object sender, System.EventArgs e)
		{
			XSDGenerator.WinMain win = new XSDGenerator.WinMain();
			win.ShowDialog(this);
		}

		private void WorldBuilderButton_Click(object sender, System.EventArgs e)
		{
			WorldBuilder.WinMain win = new WorldBuilder.WinMain();
			win.ShowDialog(this);
		}
	}
}
