using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace Strive.UI.Windows.Controls
{
	/// <summary>
	/// Summary description for Html.
	/// </summary>
	public class Html : System.Windows.Forms.UserControl
	{
		private AxSHDocVw.AxWebBrowser Browser;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private string _url;

		public Html() : this("about:blank")
		{
		}

		public Html(string url)
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
			this.Navigate(url);

		}

		public string Url 
		{
			get
			{
				return _url;
			}
		}

		public void Navigate(string Url)
		{
			object nullRef = 0;
			Browser.Navigate(Url, ref nullRef, ref nullRef, ref nullRef, ref nullRef);
			// update title if this is in a tabpage
			if(this.Parent is Crownwood.Magic.Controls.TabPage)
			{
				((Crownwood.Magic.Controls.TabPage)this.Container).Title = Url;
			}
			_url = Url;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Html));
			this.Browser = new AxSHDocVw.AxWebBrowser();
			((System.ComponentModel.ISupportInitialize)(this.Browser)).BeginInit();
			this.SuspendLayout();
			// 
			// Browser
			// 
			this.Browser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.Browser.Enabled = true;
			this.Browser.Location = new System.Drawing.Point(0, 0);
			this.Browser.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("Browser.OcxState")));
			this.Browser.Size = new System.Drawing.Size(500, 500);
			this.Browser.TabIndex = 0;
			// 
			// Html
			// 
			this.Controls.Add(this.Browser);
			this.Name = "Html";
			this.Size = new System.Drawing.Size(500, 500);
			((System.ComponentModel.ISupportInitialize)(this.Browser)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion
	}
}
