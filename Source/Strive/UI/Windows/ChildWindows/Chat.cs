using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Strive.UI.Engine;

namespace Strive.UI.Windows.ChildWindows
{
	/// <summary>
	/// Summary description for Chat.
	/// </summary>
	public class Chat : System.Windows.Forms.Form
	{
		private Strive.UI.Windows.Controls.RichScrollBox ChatOutput;
		private System.Windows.Forms.RichTextBox ChatInput;
		private System.Windows.Forms.Button Send;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Chat()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			
			// register for chats
			Game.CurrentGameLoop._message_processor.OnChat += new MessageProcessor.ChatHandler(ProcessChat);

		}

		private void ProcessChat(Strive.Network.Messages.ToClient.Communication chatMessage)
		{
			ChatOutput.Rtf += Environment.NewLine + chatMessage.name + ":";
			// Make it red and add colon
			ChatOutput.Select(ChatOutput.Text.Length - chatMessage.name.Length - 1, chatMessage.name.Length + 1);
			ChatOutput.SelectionColor = System.Drawing.Color.Red;
			ChatOutput.Rtf += " " + chatMessage.message;
		}

		private void ProcessClientChat(string message)
		{
			ChatOutput.Text += Environment.NewLine + "You say:";
			// Make it red and add colon
			ChatOutput.Select(ChatOutput.Text.Length - "You say:".Length, "You say:".Length);
			ChatOutput.SelectionColor = System.Drawing.Color.Green;
			ChatOutput.Text += " " + message;

			// send the text
			Game.CurrentServerConnection.Chat(message);
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
			this.ChatOutput = new Strive.UI.Windows.Controls.RichScrollBox();
			this.ChatInput = new System.Windows.Forms.RichTextBox();
			this.Send = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// ChatOutput
			// 
			this.ChatOutput.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.ChatOutput.IsBottomPreferred = true;
			this.ChatOutput.Name = "ChatOutput";
			this.ChatOutput.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.ChatOutput.Size = new System.Drawing.Size(648, 184);
			this.ChatOutput.TabIndex = 0;
			this.ChatOutput.Text = "";
			// 
			// ChatInput
			// 
			this.ChatInput.Anchor = ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.ChatInput.Location = new System.Drawing.Point(0, 184);
			this.ChatInput.Multiline = false;
			this.ChatInput.Name = "ChatInput";
			this.ChatInput.Size = new System.Drawing.Size(600, 23);
			this.ChatInput.TabIndex = 1;
			this.ChatInput.Text = "";
			// 
			// Send
			// 
			this.Send.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.Send.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.Send.Location = new System.Drawing.Point(608, 184);
			this.Send.Name = "Send";
			this.Send.Size = new System.Drawing.Size(40, 23);
			this.Send.TabIndex = 2;
			this.Send.Text = "Send";
			this.Send.Click += new System.EventHandler(this.Send_Click);
			// 
			// Chat
			// 
			this.AcceptButton = this.Send;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(648, 221);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.Send,
																		  this.ChatInput,
																		  this.ChatOutput});
			this.Name = "Chat";
			this.Text = "B";
			this.Enter += new System.EventHandler(this.Chat_Enter);
			this.ResumeLayout(false);

		}
		#endregion

		private void Chat_Enter(object sender, System.EventArgs e)
		{
			ChatInput.Focus();
		}

		private void Send_Click(object sender, System.EventArgs e)
		{
			ProcessClientChat(ChatInput.Rtf);
		}
	}
}
