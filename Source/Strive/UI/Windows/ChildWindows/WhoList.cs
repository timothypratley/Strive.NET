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
		private System.Windows.Forms.ToolBar WhoCommands;
		protected ImageList whoImageList;
		private System.Windows.Forms.ToolBarButton RefreshList;
		private System.Windows.Forms.ListView CharactersOnline;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
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
			Game.CurrentServerConnection.WhoList();
		}

		// umg this is zanny code
		void HandleWhoListThreadSafe( Strive.Network.Messages.ToClient.WhoList wl ) {
			this.Invoke( new MessageProcessor.WhoListHandler( HandleWhoList ),
				new object [] { wl } );
		}

		void HandleWhoList( Strive.Network.Messages.ToClient.WhoList wl ) {
			CharactersOnline.Items.Clear();

			for ( int i=0; i< wl.MobileID.Length; i++ ) {
				ListViewItem currentChar = new ListViewItem(new string[] {wl.MobileID[i].ToString(), wl.MobileName[i].ToString()});
				CharactersOnline.Items.Add(currentChar);
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
			this.WhoCommands = new System.Windows.Forms.ToolBar();
			this.RefreshList = new System.Windows.Forms.ToolBarButton();
			this.CharactersOnline = new System.Windows.Forms.ListView();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.SuspendLayout();
			// 
			// WhoCommands
			// 
			this.WhoCommands.Anchor = ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.WhoCommands.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																						   this.RefreshList});
			this.WhoCommands.Dock = System.Windows.Forms.DockStyle.None;
			this.WhoCommands.DropDownArrows = true;
			this.WhoCommands.Location = new System.Drawing.Point(0, 343);
			this.WhoCommands.Name = "WhoCommands";
			this.WhoCommands.ShowToolTips = true;
			this.WhoCommands.Size = new System.Drawing.Size(298, 39);
			this.WhoCommands.TabIndex = 1;
			this.WhoCommands.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.WhoCommands_ButtonClick);
			// 
			// RefreshList
			// 
			this.RefreshList.ToolTipText = "Refresh this list";
			// 
			// CharactersOnline
			// 
			this.CharactersOnline.AllowColumnReorder = true;
			this.CharactersOnline.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.CharactersOnline.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							   this.columnHeader2,
																							   this.columnHeader1});
			this.CharactersOnline.FullRowSelect = true;
			this.CharactersOnline.Name = "CharactersOnline";
			this.CharactersOnline.Size = new System.Drawing.Size(294, 342);
			this.CharactersOnline.TabIndex = 2;
			this.CharactersOnline.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Id";
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Character";
			// 
			// WhoList
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(290, 371);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.CharactersOnline,
																		  this.WhoCommands});
			this.Name = "WhoList";
			this.Text = "Who List";
			this.ResumeLayout(false);

		}
		#endregion

		private void WhoCommands_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			RefreshForm();
		}


	}
}
