using System;

using Strive.Network.Messages;
using Strive.Math3D;
using Strive.Rendering.Models;
using Strive.Rendering.Textures;
using Strive.Logging;
using Strive.Multiverse;
using Strive.Resources;
using Strive.UI.WorldView;


namespace Strive.UI.Engine {
	/// <summary>
	/// Summary description for MessageProcessor.
	/// </summary>
	public class MessageProcessor {
		public delegate void CanPossessHandler( Strive.Network.Messages.ToClient.CanPossess message );
		public delegate void SkillListHandler( Strive.Network.Messages.ToClient.SkillList message );
		public delegate void WhoListHandler( Strive.Network.Messages.ToClient.WhoList message );
		public delegate void ChatHandler( Strive.Network.Messages.ToClient.Communication message);

		public event CanPossessHandler OnCanPossess;
		public event SkillListHandler OnSkillList;
		public event WhoListHandler OnWhoList;
		public event ChatHandler OnChat;

		World _world;

		public MessageProcessor( World w ) {
			_world = w;
		}

		public void Process( IMessage m ) {
			#region Communication Message
			if ( m is Strive.Network.Messages.ToClient.Communication ) {
				Strive.Network.Messages.ToClient.Communication c = (Strive.Network.Messages.ToClient.Communication)m;
				if ( c.communicationType == CommunicationType.Chat ) {
					OnChat(c);
				} 
				else {
					Log.ErrorMessage( "Bad communicationType" );
				}
			}
					#endregion
			#region AddPhysicalObject Message
			else if ( m is Strive.Network.Messages.ToClient.AddPhysicalObject ) {
				// TODO: should probabbly treat terrain differently
				PhysicalObject po = Strive.Network.Messages.ToClient.AddPhysicalObject.GetPhysicalObject( (Strive.Network.Messages.ToClient.AddPhysicalObject)m );
				//po.Position.X = (float)Math.Round(po.Position.X * 8F / 10F);
				//po.Position.Y = (float)Math.Round(po.Position.Y * 8F / 10F);
				//po.Position.Z = (float)Math.Round(po.Position.Z * 8F / 10F);
				PhysicalObjectInstance poi = (PhysicalObjectInstance)_world.physicalObjectInstances[po.ObjectInstanceID];
				if ( poi != null ) {
					// replace an existing physical object
					poi.physicalObject = po;
				} 
				else {
					// add a new one
					poi = _world.Add( po );
				}
				if ( po.ObjectInstanceID == Game.CurrentPlayerID ) {
					// current player gets followed by camera etc.
					_world.Possess( Game.CurrentPlayerID );
					Log.LogMessage( "Initial position is " + po.Position );
					Log.LogMessage( "Initial heading is " + Helper.GetHeadingFromRotation(po.Rotation) );
				} 
				else {
					//Log.LogMessage( "Added object " + po.ObjectInstanceID + " with model " + po.ModelID + " at " + po.Position );
				}
				if ( poi != null && po is Mobile ) {
					SetMobileState( ((Mobile)po).MobileState, (IActor)poi.model );
				}
			}
					#endregion
			#region AddTerrainCollection Message
				// TODO: unused atm, unnesessary?
			else if ( m is Strive.Network.Messages.ToClient.AddTerrainCollection ) {
				Strive.Network.Messages.ToClient.AddTerrainCollection atc = (Strive.Network.Messages.ToClient.AddTerrainCollection)m;
				_world.TerrainPieces.AddMany( atc.startX, atc.startZ, atc.width, atc.height, atc.gap_size, atc.map );
			}
				#endregion
			#region Position Message

			else if( m is Strive.Network.Messages.ToClient.Position) {
				Strive.Network.Messages.ToClient.Position p = (Strive.Network.Messages.ToClient.Position)m;
				PhysicalObjectInstance poi = _world.Find( p.instance_id );
				if ( poi == null ) {
					Log.ErrorMessage( "Model for " + p.instance_id + " has not been loaded" );
					return;
				}

				poi.model.Position = p.position;
				poi.model.Rotation = p.rotation;
				if ( poi == _world.CurrentAvatar ) {
					_world.RepositionCamera();
				}
				//				Log.LogMessage( "Position message applied to " + p.instance_id + " rotation " + Rotation );
			}
			#endregion
			#region CombatReport Message
			else if ( m is Strive.Network.Messages.ToClient.CombatReport ) {
				Strive.Network.Messages.ToClient.CombatReport cr = m as Strive.Network.Messages.ToClient.CombatReport;
				switch ( cr.combat_event ) {
					case EnumCombatEvent.Attacks:
						Log.LogMessage(
							cr.attackerObjectInstanceID.ToString() + " attacks " + cr.targetObjectInstanceID.ToString() + "!" );
						break;
					case EnumCombatEvent.Avoids:
						Log.LogMessage(
							cr.targetObjectInstanceID.ToString() + " avoids " + cr.attackerObjectInstanceID.ToString() + "." );
						break;
					case EnumCombatEvent.Hits:
						Log.LogMessage(
							cr.attackerObjectInstanceID.ToString() + " hits " + cr.targetObjectInstanceID.ToString() + " for " + cr.damage + " damage." );
						break;
					case EnumCombatEvent.Misses:
						Log.LogMessage(
							cr.attackerObjectInstanceID.ToString() + " misses " + cr.targetObjectInstanceID.ToString() + "." );
						break;
					default:
						Log.ErrorMessage(
							"Unknown CombatEvent " + cr.combat_event );
						break;
				}
			}
				#endregion
			#region DropPhysicalObject Message
			else if ( m is Strive.Network.Messages.ToClient.DropPhysicalObject) {
				Strive.Network.Messages.ToClient.DropPhysicalObject dpo = (Strive.Network.Messages.ToClient.DropPhysicalObject)m;
				_world.Remove( dpo.instance_id );
				//Log.LogMessage( "Removed "+ dpo.instance_id.ToString() );
			}
				#endregion
			#region MobileState
			else if ( m is Strive.Network.Messages.ToClient.MobileState) {
				Strive.Network.Messages.ToClient.MobileState ms = (Strive.Network.Messages.ToClient.MobileState)m;
				//				Log.LogMessage( "Mobile " + ms.ObjectInstanceID + " is " + ms.State );

				PhysicalObjectInstance poi = _world.Find( ms.ObjectInstanceID );
				
				#region 1.1.1 Check that the model exists
				if ( poi == null || poi.model == null || !(poi.model is IActor ) ) {
					Log.ErrorMessage( "Actor for " + ms.ObjectInstanceID + " has not been loaded" );
					return;
				}
				#endregion

				IActor actor = (IActor)poi.model;
				SetMobileState( ms.State, actor );

				//TODO: evaluate whether just to ignore postion for oneself
				//poi.model.Position = ms.position;
			}
				#endregion
			#region CurrentHitpoints
			else if ( m is Strive.Network.Messages.ToClient.CurrentHitpoints) {
				Strive.Network.Messages.ToClient.CurrentHitpoints chp = (Strive.Network.Messages.ToClient.CurrentHitpoints)m;
				Log.LogMessage( "You now have " + chp.HitPoints );
			}
				#endregion
			#region CanPossess
			else if ( m is Strive.Network.Messages.ToClient.CanPossess ) {
				Strive.Network.Messages.ToClient.CanPossess cp = (Strive.Network.Messages.ToClient.CanPossess)m;
				OnCanPossess( cp );
			}
				#endregion
			#region DropAll
			else if ( m is Strive.Network.Messages.ToClient.DropAll ) {
				Strive.Network.Messages.ToClient.DropAll da = (Strive.Network.Messages.ToClient.DropAll)m;
				Log.LogMessage( "DropAll recieved" );
				_world.Clear();
			}
				#endregion
			#region Weather
			else if ( m is Strive.Network.Messages.ToClient.TimeAndWeather ) {
				Strive.Network.Messages.ToClient.TimeAndWeather w = (Strive.Network.Messages.ToClient.TimeAndWeather)m;
				//Log.LogMessage( "Weather update recieved" );
				ITexture t = Game.resources.GetTexture(w.SkyTextureID);

				// TODO: don't hardcode the clouds textureid
				ITexture ct = Game.resources.GetTexture( 46 );
				_world.SetSky( t );
				_world.SetClouds( ct );
				_world.SetLighting( w.Lighting );
			}
			#endregion
			#region Ping
			else if ( m is Strive.Network.Messages.ToClient.Ping ) {
				Strive.Network.Messages.ToClient.Ping p = (Strive.Network.Messages.ToClient.Ping)m;
				Game.CurrentServerConnection.Pong( p.SequenceNumber );
			}
			#endregion
			#region SkillList
			else if ( m is Strive.Network.Messages.ToClient.SkillList ) {
				Strive.Network.Messages.ToClient.SkillList sl = (Strive.Network.Messages.ToClient.SkillList)m;
				Log.LogMessage( "SkillList recieved" );
				OnSkillList( sl );
			}
				#endregion
			#region WhoList
			else if ( m is Strive.Network.Messages.ToClient.WhoList ) {
				Strive.Network.Messages.ToClient.WhoList wl = (Strive.Network.Messages.ToClient.WhoList)m;
				OnWhoList( wl );
			}
			#endregion
			#region PartyInfo
			else if ( m is Strive.Network.Messages.ToClient.PartyInfo ) {
				Strive.Network.Messages.ToClient.PartyInfo pi = (Strive.Network.Messages.ToClient.PartyInfo)m;
				Log.LogMessage( "PartyInfo received: " + pi );
			}
			#endregion
			#region LogMessage
			else if (m is Strive.Network.Messages.ToClient.LogMessage) 
			{
				Strive.Network.Messages.ToClient.LogMessage logmessage = (Strive.Network.Messages.ToClient.LogMessage)m;
				Log.LogMessage( logmessage.Message );
			}
			#endregion
			#region Default
			else 
			{
				Log.ErrorMessage( "Unknown message of type " + m.GetType() );
			}
				#endregion
		}

		void SetMobileState( EnumMobileState ms, IActor actor ) {
			switch( ms ) {
				case EnumMobileState.Dead:
					actor.AnimationSequence = "deadback"; break;
				case EnumMobileState.Incapacitated:
					actor.AnimationSequence = "deadback"; break;
				case EnumMobileState.Sleeping:
					actor.AnimationSequence = "deadstomach"; break;
				case EnumMobileState.Resting:
					actor.AnimationSequence = "deadsitting"; break;
				case EnumMobileState.Standing:
					actor.AnimationSequence = "idle"; break;
				case EnumMobileState.Walking:
					actor.AnimationSequence = "walk"; break;
				case EnumMobileState.Running:
					actor.AnimationSequence = "run"; break;
				case EnumMobileState.Fighting:
					actor.AnimationSequence = "ref_shoot_crowbar"; break;
				default:
					Log.ErrorMessage( "Unknown mobile state " + ms );
					break;
			}
			actor.playAnimation();
		}
	}

}
