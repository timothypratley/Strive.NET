using System;

namespace Strive.UI.Channels
{
	/// <summary>
	/// Summary description for ChannelManager.
	/// </summary>
	public class ChannelManager
	{
		public delegate void MessageReceived(Strive.Network.Messages.ToClient.Communication message);
		private System.Collections.Hashtable _registrations = new System.Collections.Hashtable();
		private Crownwood.Magic.Docking.DockingManager _dockingManager;
        
		public ChannelManager(Crownwood.Magic.Docking.DockingManager dockingManager)
		{
			Game.CurrentMessageProcessor.OnChat += new Strive.UI.Engine.MessageProcessor.ChatHandler(ProcessChat);
		}

		public void RegisterChannel(Strive.Network.Messages.CommunicationType communicationType, string name, MessageReceived callback)
		{
			if(!_registrations.Contains(CalculateChannelKey(communicationType, name)))
			{
				_registrations.Add(CalculateChannelKey(communicationType, name), callback);
			}
		}

		public string CalculateChannelKey(Strive.Network.Messages.CommunicationType communicationType, string name)
		{
			string CalculateChannelKey_Return = communicationType.ToString();
			if(communicationType == Strive.Network.Messages.CommunicationType.Tell)
			{
				CalculateChannelKey_Return = name + " - Private conversation";
			}
			return CalculateChannelKey_Return;
		}

		private void ProcessChat(Strive.Network.Messages.ToClient.Communication chatMessage)
		{
			string channelWindowName = chatMessage.communicationType.ToString();
			if(chatMessage.communicationType == Strive.Network.Messages.CommunicationType.Tell)
			{
				channelWindowName = chatMessage.name + " - Private conversation";
			}
			
			if(!_dockingManager.Contents.Contains(channelWindowName))
			{
				// add a window
				Crownwood.Magic.Docking.Content chatWindow = _dockingManager.Contents.Add(new Strive.UI.Windows.ChildWindows.Chat(chatMessage.communicationType, chatMessage.name), "Chat", null, -1);
				_dockingManager.AddContentWithState(chatWindow, Crownwood.Magic.Docking.State.DockBottom);
			}
			MessageReceived callback = (MessageReceived)_registrations[CalculateChannelKey(chatMessage.communicationType, chatMessage.name)];

			if(callback != null)
			{
				callback(chatMessage);
			}
		}
	}
}
