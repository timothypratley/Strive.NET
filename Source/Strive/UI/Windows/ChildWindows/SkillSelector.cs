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
	public class SkillSelector : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public SkillSelector()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			Game.CurrentGameLoop._message_processor.OnSkillList
				+= new MessageProcessor.SkillListHandler( HandleSkillListThreadSafe );
			RefreshForm();
		}

		void RefreshForm() {
			Game.CurrentServerConnection.Send(
				new Strive.Network.Messages.ToServer.GameCommand.SkillList() );
			Controls.Clear();
			this.Text = "Requesting...";
			// ToDo: start a timer with a timeout
		}

		// umg this is zanny code
		void HandleSkillListThreadSafe( Strive.Network.Messages.ToClient.SkillList sl ) {
			this.Invoke( new MessageProcessor.SkillListHandler( HandleSkillList ),
				new object [] { sl } );
		}

		class IdButton : Button {
			public int id;
		}
		void HandleSkillList( Strive.Network.Messages.ToClient.SkillList sl ) {
			if ( sl.skills.Length < 1 ) {
				this.Text = "You have no skills yet.";
				return;
			}
			for ( int i=0; i<sl.skills.Length; i++ ) {
				IdButton b = new IdButton();
				// todo: do the name lookup
				b.Text = "skill " + sl.skills[i] + " : " + sl.competancy[i] + "%";
				b.id = sl.skills[i];
				b.Click += new System.EventHandler(this.Button_Click);
				Controls.Add( b );
			}
		}

		private void Button_Click(object sender, System.EventArgs e) {			
			int [] targets = {};
			Game.CurrentServerConnection.Send(
				new Strive.Network.Messages.ToServer.GameCommand.UseSkill( (Strive.Multiverse.EnumSkill)((IdButton)sender).id, targets ) );
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
			// SkillSelector
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 373);
			this.Name = "SkillSelector";
			this.Text = "SkillSelector";

		}
		#endregion

		private void Refresh_Click(object sender, System.EventArgs e) {
			RefreshForm();
		}
	}
}
