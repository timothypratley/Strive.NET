using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Strive.UI.Forms
{
	/// <summary>
	/// Summary description for FormBase.
	/// </summary>
	public class FormBase : System.Windows.Forms.Form
	{
		private System.Windows.Forms.PictureBox iconBox;
		private System.Windows.Forms.Label Copyright;
		private System.Windows.Forms.LinkLabel Acknowledgments;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FormBase()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FormBase));
			this.iconBox = new System.Windows.Forms.PictureBox();
			this.Copyright = new System.Windows.Forms.Label();
			this.Acknowledgments = new System.Windows.Forms.LinkLabel();
			this.SuspendLayout();
			// 
			// iconBox
			// 
			this.iconBox.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.iconBox.BackColor = System.Drawing.Color.Transparent;
			this.iconBox.Image = ((System.Drawing.Bitmap)(resources.GetObject("iconBox.Image")));
			this.iconBox.Location = new System.Drawing.Point(432, 200);
			this.iconBox.Name = "iconBox";
			this.iconBox.Size = new System.Drawing.Size(56, 64);
			this.iconBox.TabIndex = 0;
			this.iconBox.TabStop = false;
			// 
			// Copyright
			// 
			this.Copyright.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.Copyright.BackColor = System.Drawing.Color.Transparent;
			this.Copyright.Location = new System.Drawing.Point(208, 224);
			this.Copyright.Name = "Copyright";
			this.Copyright.Size = new System.Drawing.Size(216, 16);
			this.Copyright.TabIndex = 0;
			this.Copyright.Text = "(C) 2002 Timothy Pratley, Nathan Rogers";
			this.Copyright.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.Copyright.Click += new System.EventHandler(this.Copyright_Click);
			// 
			// Acknowledgments
			// 
			this.Acknowledgments.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.Acknowledgments.BackColor = System.Drawing.Color.Transparent;
			this.Acknowledgments.Location = new System.Drawing.Point(224, 240);
			this.Acknowledgments.Name = "Acknowledgments";
			this.Acknowledgments.Size = new System.Drawing.Size(200, 16);
			this.Acknowledgments.TabIndex = 0;
			this.Acknowledgments.TabStop = true;
			this.Acknowledgments.Text = "Acknowledgments";
			this.Acknowledgments.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.Acknowledgments.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Acknowledgments_LinkClicked);
			// 
			// FormBase
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(492, 266);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.Acknowledgments,
																		  this.Copyright,
																		  this.iconBox});
			this.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Name = "FormBase";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Load += new System.EventHandler(this.FormBase_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormBase_Load(object sender, System.EventArgs e)
		{
		}

		private void Copyright_Click(object sender, System.EventArgs e)
		{
		
		}

		private void Acknowledgments_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			Acknowledgements Acknowledgements = new Acknowledgements();
			Acknowledgements.ShowDialog(this);
		}
	}
}
