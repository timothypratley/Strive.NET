using System;
using System.ComponentModel;

namespace Strive.UI.Forms.Controls.Html
{
	public delegate void BrowserNavigateEventHandler(object s, BrowserNavigateEventArgs e);

	public class BrowserNavigateEventArgs : CancelEventArgs
	{
		private string url;
        
		public BrowserNavigateEventArgs(string url, bool cancel)
			: base(cancel)
		{
			this.url = url;
		}
        
		public string Url 
		{ 
			get
			{
				return this.url;
			} 
		}
	} 
} 

