using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;

namespace Strive.Client.WinForms.Windows.Controls
{
	/// <summary>
	/// Summary description for RichScrollBox.
	/// </summary>
	public class RichScrollBox : System.Windows.Forms.RichTextBox
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		/// <summary>
		/// Scroll control information.
		/// </summary>
		const int WM_VSCROLL = 0x0115;
		readonly IntPtr SB_BOTTOM = new IntPtr( 7 );

		/// <summary>
		/// Preference to scroll to the bottom when text is added.
		/// </summary>
		private bool isBottomPreferred = true;
		public bool IsBottomPreferred
		{
			get
			{
				return isBottomPreferred;
			}
			set
			{
				isBottomPreferred = value;
			}
		}

		public RichScrollBox()
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
				if( components != null )
					components.Dispose();
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
			// 
			// RichScrollBox
			// 
			this.TextChanged += new System.EventHandler(this.ScrollText_TextChanged);
		}
		#endregion

		/// <summary>
		/// For OnTextChanged event, check if is near bottom and, if so,
		///	scroll the window to the bottom after text is added.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ScrollText_TextChanged(object sender, System.EventArgs e)
		{
			if( isBottomPreferred )
			{
				Point lastCharPos = this.GetPositionFromCharIndex( this.TextLength - 1 );
				int scrollPos = lastCharPos.Y - this.ClientSize.Height;
				if( scrollPos < this.FontHeight )
					ScrollToBottom();
			}
		}

		private void ScrollToBottom()
		{
			System.Windows.Forms.Message msg = 
				Message.Create( this.Handle, WM_VSCROLL, SB_BOTTOM, IntPtr.Zero );
			this.DefWndProc( ref msg );
		}
	}
}
