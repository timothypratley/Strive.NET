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
	public class CharacterSelector : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public CharacterSelector()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			Game.CurrentGameLoop._message_processor.OnCanPossess
				+= new MessageProcessor.CanPossessHandler( HandleCanPossessThreadSafe );
			RefreshForm();
		}

		void RefreshForm() {
			Game.CurrentServerConnection.Send(
				new Strive.Network.Messages.ToServer.RequestPossessable() );
			Controls.Clear();
			this.Text = "Requesting...";
			// ToDo: start a timer with a timeout
		}

		// umg this is zanny code
		void HandleCanPossessThreadSafe( Strive.Network.Messages.ToClient.CanPossess cp ) {
			this.Invoke( new MessageProcessor.CanPossessHandler( HandleCanPossess ),
				new object [] { cp } );
		}

		class IdButton : Button {
			public int id;
		}
		void HandleCanPossess( Strive.Network.Messages.ToClient.CanPossess cp ) {
			if ( cp.possesable.Length < 1 ) {
				this.Text = "You have no characters yet, create one.";
				return;
			}
			foreach ( Strive.Network.Messages.ToClient.CanPossess.id_name_tuple tuple in cp.possesable ) {
				IdButton b = new IdButton();
				b.Text = tuple.id + " : " + tuple.name;
				b.id = tuple.id;
				b.Click += new System.EventHandler(this.Button_Click);
				Controls.Add( b );
			}
		}

		private void Button_Click(object sender, System.EventArgs e) {			
			Game.CurrentPlayerID = ((IdButton)sender).id;
			Game.CurrentServerConnection.Send(
				new Strive.Network.Messages.ToServer.EnterWorldAsMobile( Game.CurrentPlayerID ) );
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
			// CharacterSelector
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 373);
			this.Name = "CharacterSelector";
			this.Text = "CharacterSelector";

		}
		#endregion

		private void Refresh_Click(object sender, System.EventArgs e) {
			RefreshForm();
		}
	}
}
