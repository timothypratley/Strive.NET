using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Strive.UI.Engine;

namespace Strive.UI.Windows.ChildWindows
{
	/// <summary>
	/// Summary description for CharacterSelector.
	/// </summary>
	public class WhoList : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public WhoList()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			Game.CurrentGameLoop._message_processor.OnWhoList
				+= new MessageProcessor.WhoListHandler( HandleWhoListThreadSafe );
			RefreshForm();
		}

		void RefreshForm() {
			Game.CurrentServerConnection.Send(
				new Strive.Network.Messages.ToServer.GameCommand.WhoList() );
			Controls.Clear();
			this.Text = "Requesting...";
			// ToDo: start a timer with a timeout
		}

		// umg this is zanny code
		void HandleWhoListThreadSafe( Strive.Network.Messages.ToClient.WhoList wl ) {
			this.Invoke( new MessageProcessor.WhoListHandler( HandleWhoList ),
				new object [] { wl } );
		}

		class IdButton : Button {
			public int id;
		}
		void HandleWhoList( Strive.Network.Messages.ToClient.WhoList wl ) {
			if ( wl.MobileID.Length < 1 ) {
				this.Text = "No-one is online";
				return;
			}
			for ( int i=0; i< wl.MobileID.Length; i++ ) {
				IdButton b = new IdButton();
				b.Text = wl.MobileID[i] + " : " + wl.MobileName[i];
				b.id = wl.MobileID[i];
				b.Click += new System.EventHandler(this.Button_Click);
				Controls.Add( b );
			}
		}

		private void Button_Click(object sender, System.EventArgs e) {			
//			Game.CurrentServerConnection.Send(
//				new Strive.Network.Messages.ToServer.GameCommand.Communication( ((IdButton)sender).id ) );
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
			// 
			// WhoList
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 373);
			this.Name = "WhoList";
			this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.Text = "WhoList";

		}
		#endregion

		private void Refresh_Click(object sender, System.EventArgs e) {
			RefreshForm();
		}
	}
}
