using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;

namespace Strive.UI.Forms.Controls.Html
{
	public class HtmlControl : AxHost, IWebBrowserEvents
	{
		public BrowserNavigateEventHandler BeforeNavigate;
		public BrowserNavigateEventHandler NavigateComplete;
		private IWebBrowser control = null;
		private AxHost.ConnectionPointCookie cookie;
		private string url = "";
		private string html = "";
		private string cssStyleSheet = "";
		private bool initialized = false;
		public const int OLEIVERB_UIACTIVATE = -4; 

		public HtmlControl()
			: base("8856f961-340a-11d0-a96b-00c04fd705a2")
		{
		} 
        
		public virtual void RaiseNavigateComplete(string url)
		{
			if (initialized)
			{
				BrowserNavigateEventArgs e = new BrowserNavigateEventArgs(url, false);
				if (NavigateComplete != null)
					NavigateComplete(this, e);
			}
		} 
        
		public virtual void RaiseBeforeNavigate(string url, int flags, string targetFrameName, ref object postData, string headers, ref bool cancel)
		{
			if (initialized)
			{
				BrowserNavigateEventArgs e = new BrowserNavigateEventArgs(url, false);
				if (BeforeNavigate != null)
					BeforeNavigate(this, e);
				cancel = e.Cancel;
			}
		} 
        
		public string CascadingStyleSheet
		{
			get
			{
				return cssStyleSheet;
			}
			set
			{
				cssStyleSheet = value;
				ApplyCascadingStyleSheet();
			}
		}

		public string Url 
		{ 
			set
			{
				this.url = value;
			}
		}
        
		public string Html 
		{ 
			set
			{
				this.html = value;
			}
		}
        
		protected override void DetachSink()
		{
			try 
			{
				this.cookie.Disconnect();
			}
			catch 
			{
			}
		} 
        
		protected override void CreateSink()
		{
			try 
			{
				this.cookie = new ConnectionPointCookie(this.GetOcx(), this, typeof(IWebBrowserEvents));
			}
			catch 
			{
			}
		} 
        
		protected override void AttachInterfaces()
		{
			try 
			{
				this.control = (IWebBrowser) this.GetOcx();
			}
			catch
			{
			}
		} 
        
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			object flags = 0;
			object targetFrame = string.Empty;
			object postData = string.Empty;
			object headers = string.Empty;
			this.control.Navigate("about:blank", ref flags, ref targetFrame, ref postData, ref headers);
			
			MethodInvoker mi = new MethodInvoker(this.DelayedInitialize);
			this.BeginInvoke(mi);
		} 

		public void DelayedInitialize()  
		{
			initialized = true;
			if (html != "")
				ApplyBody(html);
			UIActivate();
			ApplyCascadingStyleSheet();
		}

		void UIActivate()
		{
			this.DoVerb(OLEIVERB_UIACTIVATE);
		}

		void ApplyBody(string value)
		{ 
			if (control != null)
			{
				IHTMLElement el = null;
				IHTMLDocument2 doc = this.control.GetDocument();
				if (doc != null) 
					el = doc.GetBody();
				if (el != null) 
				{
					UIActivate();
					el.SetInnerHTML(value);
					return;
				}
			}
		}

		void ApplyCascadingStyleSheet()
		{
			if (control != null)
			{
				IHTMLDocument2 htmlDoc = control.GetDocument();
				if (htmlDoc != null)
					htmlDoc.CreateStyleSheet(cssStyleSheet, 0);
			}
		}


	} 
}
