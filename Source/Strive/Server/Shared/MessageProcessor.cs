using System;
using System.Collections;
using System.Threading;
using System.Net;

using Strive.Network.Messages;
using Strive.Network.Server;
using Strive.Math3D;
using Strive.Multiverse;
using Strive.Logging;

namespace Strive.Server.Shared {
	public class MessageProcessor {
		World world;
		Listener listener;

		public MessageProcessor(
			World world, Listener listener
		) {
			this.world = world;
			this.listener = listener;
		}

		public void ProcessNextMessage() {
			ClientMessage clientMessage = (ClientMessage)listener.PopNextMessage();
			try 
			{
				ProcessMessage( clientMessage.Client, clientMessage.Message );
			} 
			catch ( Exception e ) 
			{
				Log.ErrorMessage( e );
				Log.LogMessage( "Message was not processed correctly, resuming." );
			}
		}

		public void ProcessMessage( Client client, IMessage message ) {
			if ( !client.Authenticated ) {
				// new connection... only allow login
				if ( message is Network.Messages.ToServer.Login ) {
					ProcessLoginMessage(
						client, (Network.Messages.ToServer.Login)message
					);
				} else {
					Log.WarningMessage( "Non-login message " + message.GetType() + " from " + client.EndPoint );
				}
				return;
			}

			if ( message is Network.Messages.ToServer.RequestPossessable ) {
				ProcessRequestPossessable( client, message as Network.Messages.ToServer.RequestPossessable );
				return;
			}

			if ( client.Avatar == null ) {
				// no character selected yet... only allow posses
				// and logout
				if ( message is Network.Messages.ToServer.EnterWorldAsMobile ) 
				{
					ProcessEnterWorldAsMobile( client, (Network.Messages.ToServer.EnterWorldAsMobile)message );
				} 
				else if(message is Network.Messages.ToServer.Logout)
				{

				}
				else
				{

					Log.WarningMessage( "ERROR: Non-posses message " + message.GetType() + " from " + client.EndPoint );
				}
				return;
			}

			// normal message for logged in user
			if ( message is Network.Messages.ToServer.Position ) 
			{
				ProcessPositionMessage( client, message as Network.Messages.ToServer.Position );
			} 
			else if ( message is Network.Messages.ToServer.GameCommand.Communication ) 
			{
				ProcessChatMessage( client, message as Network.Messages.ToServer.GameCommand.Communication );
			} 
			else if ( message is Network.Messages.ToServer.GameCommand.UseSkill ) 
			{
				SkillCommandProcessor.ProcessUseSkill( client, message as Network.Messages.ToServer.GameCommand.UseSkill );
			} 
			else if ( message is Network.Messages.ToServer.GameCommand.CancelSkill ) {
				SkillCommandProcessor.ProcessCancelSkill( client, message as Network.Messages.ToServer.GameCommand.CancelSkill );
			} 
			else if ( message is Network.Messages.ToServer.ReloadWorld ) 
			{
				ProcessReloadWorldMessage( client, message as Network.Messages.ToServer.ReloadWorld );
			} 
			else if ( message is Network.Messages.ToServer.Logout ) 
			{
				ProcessLogout(client);
			}
			else if ( message is Network.Messages.ToServer.GameCommand.SkillList )
			{
				ProcessSkillList(client, message as Network.Messages.ToServer.GameCommand.SkillList);
			}
			else if ( message is Network.Messages.ToServer.GameCommand.WhoList )
			{
				ProcessWhoList(client, message as Network.Messages.ToServer.GameCommand.WhoList);
			}
			else 
			{
				Log.WarningMessage(
					"ERROR: Unknown message " + message.GetType()
					);
			}
		}

		void ProcessLoginMessage(
			Client client, Strive.Network.Messages.ToServer.Login loginMessage
		) {
			if (
				world.UserLookup( loginMessage.username, loginMessage.password, ref client.PlayerID )
			) {
				// login succeeded, check there isnt an existing connection for this
				// player
				Client old = (Client)listener.Clients[ loginMessage.username ];
				if ( old != null ) {
					old.Close();
				}
				Log.LogMessage(
					"User " + loginMessage.username + " logged in"
					);
				client.AuthenticatedUsername = loginMessage.username;
				client.Protocol = loginMessage.protocol;
			} else {
				Log.LogMessage(
					"Login failed for username " + loginMessage.username +  " "
					+ loginMessage.password
				);
				client.Close();
			}
		}

		void ProcessRequestPossessable( Client client, Strive.Network.Messages.ToServer.RequestPossessable message ) {
			Strive.Data.MultiverseFactory.refreshMultiverseForPlayer( Global.multiverse, client.PlayerID );
			Strive.Network.Messages.ToClient.CanPossess canPossess = new Strive.Network.Messages.ToClient.CanPossess(
				world.getPossessable( client.AuthenticatedUsername ) );
			client.Send( canPossess );
		}

		void ProcessLogout( Client client) 
		{
			if(client.Avatar != null)
			{
				// remove from world.
				world.Remove(client.Avatar);
			}
			Log.LogMessage("Logged out '" + client.AuthenticatedUsername +"'.");
			client.Close(); 
		}

			

		void ProcessEnterWorldAsMobile( Client client, Strive.Network.Messages.ToServer.EnterWorldAsMobile message ) {
			MobileAvatar a;
			if ( world.physicalObjects.ContainsKey( message.InstanceID ) ) {
				// reconnected
				// simply replace existing connection with the new
				// todo: the old 'connection' should timeout or die or be killed
				Object o = world.physicalObjects[message.InstanceID];
				if ( o is MobileAvatar ) {
					a = (MobileAvatar)o;
				} else {
					Log.WarningMessage( "Can only possess mobiles." );
					return;
				}
				if ( a.client == client ) {
					Log.WarningMessage( "A client " + client.ToString() + " attempted to take control of the same mobile " + a.ObjectInstanceID + " twice... ignoring request." );
				} else if ( a.client != null ) {
					Log.LogMessage( "Mobile " + a.ObjectInstanceID + " has been taken over by a new connection." );
					a.client.Avatar = null;
					a.client.Close();
					listener.Clients.Remove( a.client.EndPoint );
					a.client = client;
					client.Avatar = a;
				} else {
					a.client = client;
					client.Avatar = a;
				}
			} else {
				// try to load the character
				a = world.LoadMobile( message.InstanceID );
				if ( a == null ) {
					Log.WarningMessage( "Character "+message.InstanceID+" not found." );
					client.Close();
					return;
				}
				a.client = client;
				client.Avatar = a;

				// try to add the character to the world
				world.Add( client.Avatar );
			}
			world.SendInitialWorldView( client );
		}

		void ProcessPositionMessage(
			Client client, Strive.Network.Messages.ToServer.Position message
		) {
			if ( client.Avatar == null ) {
				Log.WarningMessage( "Position message from client " + client.EndPoint + " who has no avatar." );
				return;
			}
			MobileAvatar ma = (MobileAvatar)client.Avatar;

			// don't go to running for pure heading changes
			if ( message.position != client.Avatar.Position ) {
				ma.lastMoveUpdate = Global.now;
				if ( ma.MobileState != EnumMobileState.Running ) {
					ma.SetMobileState( EnumMobileState.Running );
				}
			}
			world.Relocate( client.Avatar, message.position, message.rotation );
		}

		void ProcessChatMessage(
			Client client, Strive.Network.Messages.ToServer.GameCommand.Communication message
		) {
			world.NotifyMobiles(
				new Network.Messages.ToClient.Communication( client.Avatar.TemplateObjectName, message.message, (Strive.Network.Messages.CommunicationType)message.communicationType )
			);
			//Log.LogMessage( "Sent communication message" );
		}

		void ProcessReloadWorldMessage(
			Client client, Strive.Network.Messages.ToServer.ReloadWorld message
		) {
			Log.LogMessage( "ReloadWorld received." );
			world.Load();
			foreach( Client c in listener.Clients.Values ) {
				if ( c.Avatar != null ) {
					// respawn their mobile, old instance will be given over
					// to Garbage Collector
					c.Send( new Strive.Network.Messages.ToClient.DropAll() );
					ProcessEnterWorldAsMobile( c, new Strive.Network.Messages.ToServer.EnterWorldAsMobile( c.Avatar.ObjectInstanceID ) );
				}
			}
		}

		void ProcessWhoList(
			Client client, Strive.Network.Messages.ToServer.GameCommand.WhoList message
		) {
			ArrayList MobileIDs = new ArrayList();
			ArrayList Names = new ArrayList();
			foreach( Client c in listener.Clients.Values ) {
				if ( c.Avatar != null ) {
					MobileIDs.Add( c.Avatar.ObjectInstanceID );
					Names.Add( c.Avatar.TemplateObjectName );
				}
			}
			client.Send( new Strive.Network.Messages.ToClient.WhoList( (int[])MobileIDs.ToArray( typeof( int ) ), (string[])Names.ToArray( typeof( string ) ) ) );
		}

		void ProcessSkillList(
			Client client, Strive.Network.Messages.ToServer.GameCommand.SkillList message
		) {
			// TODO: umg this seems a bit primitive, but I guess it works so :/
			ArrayList skillIDs = new ArrayList();
			ArrayList competancy = new ArrayList();
			for ( int i = 1; i < Global.multiverse.EnumSkill.Count; i++ ) {
				Schema.MobileHasSkillRow mhs = Global.multiverse.MobileHasSkill.FindByTemplateObjectIDEnumSkillID( client.Avatar.TemplateObjectID, i );
				if ( mhs != null ) {
					skillIDs.Add( mhs.EnumSkillID );
					competancy.Add( mhs.Rating );
				}
			}
			client.Send( new Strive.Network.Messages.ToClient.SkillList( (int [])skillIDs.ToArray( typeof( int ) ), (float [])competancy.ToArray( typeof( float ) ) ) );
		}
	}
}
