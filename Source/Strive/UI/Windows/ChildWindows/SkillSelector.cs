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

			Game.CurrentMessageProcessor.OnSkillList
				+= new MessageProcessor.SkillListHandler( HandleSkillList );
			RefreshForm();
		}

		void RefreshForm() {
			Game.CurrentServerConnection.SkillList();
			Controls.Clear();
			this.Text = "Requesting...";
			// ToDo: start a timer with a timeout
		}

		/*** no longer on another thread
		// umg this is zanny code
		void HandleSkillListThreadSafe( Strive.Network.Messages.ToClient.SkillList sl ) {
			this.Invoke( new MessageProcessor.SkillListHandler( HandleSkillList ),
				new object [] { sl } );
		}
		*/

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
				// TODO: do the name lookup
				b.Text = "skill " + sl.skills[i] + " : " + sl.competancy[i] + "%";
				b.id = sl.skills[i];
				b.Click += new System.EventHandler(this.Button_Click);
				Controls.Add( b );
			}
		}

		private void Button_Click(object sender, System.EventArgs e) {			
			int [] targets = {};
			// TODO: use InvokationID to keep track of casting and cancellation
			Game.CurrentServerConnection.UseSkill(((IdButton)sender).id, 0, targets);
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
