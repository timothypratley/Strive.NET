using System;

using Strive.Network.Messages;
using Strive.Math3D;
using Strive.Rendering.Models;
using Strive.Rendering.Textures;
using Strive.Logging;
using Strive.Multiverse;
using Strive.Resources;
using Strive.UI.WorldView;


namespace Strive.UI.Engine 
{
	/// <summary>
	/// Summary description for MessageProcessor.
	/// </summary>
	public class MessageProcessor 
	{
		public delegate void CanPossessHandler( Strive.Network.Messages.ToClient.CanPossess message );
		public delegate void SkillListHandler( Strive.Network.Messages.ToClient.SkillList message );
		public delegate void WhoListHandler( Strive.Network.Messages.ToClient.WhoList message );
		public delegate void ChatHandler( Strive.Network.Messages.ToClient.Communication message);

		public event CanPossessHandler OnCanPossess;
		public event SkillListHandler OnSkillList;
		public event WhoListHandler OnWhoList;
		public event ChatHandler OnChat;

		public MessageProcessor() 
		{
		}

		public void Process( IMessage m ) 
		{
			#region Communication Message
			if ( m is Strive.Network.Messages.ToClient.Communication ) 
			{
				Strive.Network.Messages.ToClient.Communication c = (Strive.Network.Messages.ToClient.Communication)m;
				if ( c.communicationType == CommunicationType.Chat ) 
				{
					OnChat(c);
				} 
				else 
				{
					Log.ErrorMessage( "Bad communicationType" );
				}
			}
					#endregion
			#region AddPhysicalObject Message
			else if ( m is Strive.Network.Messages.ToClient.AddPhysicalObject ) 
			{
				PhysicalObject po = Strive.Network.Messages.ToClient.AddPhysicalObject.GetPhysicalObject( (Strive.Network.Messages.ToClient.AddPhysicalObject)m );
				PhysicalObjectInstance existing = (PhysicalObjectInstance)Game.CurrentWorld.physicalObjectInstances[po.ObjectInstanceID];
				if ( existing != null ) 
				{
					// replace an existing physical object
					existing.physicalObject = po;
				} 
				else 
				{
					// add a new one
					Game.CurrentWorld.Add( po );
				}
				if ( po.ObjectInstanceID == Game.CurrentPlayerID ) 
				{
					// current player gets followed by camera etc.
					Game.CurrentWorld.Possess( Game.CurrentPlayerID );
					Log.LogMessage( "Initial position is " + po.Position );
					Log.LogMessage( "Initial heading is " + Helper.GetHeadingFromRotation(po.Rotation) );
				} 
				else 
				{
					Log.LogMessage( "Added object " + po.ObjectInstanceID + " with model " + po.ModelID + " at " + po.Position );
				}
				if ( m is Strive.Network.Messages.ToClient.AddMobile ) 
				{
					// todo: should set the animation sequence/play here
				}
			}
					#endregion
			#region Position Message

			else if( m is Strive.Network.Messages.ToClient.Position) 
			{
				Strive.Network.Messages.ToClient.Position p = (Strive.Network.Messages.ToClient.Position)m;
				PhysicalObjectInstance poi = Game.CurrentWorld.Find( p.instance_id );
				if ( poi == null ) 
				{
					Log.ErrorMessage( "Model for " + p.instance_id + " has not been loaded" );
					return;
				}

				poi.model.Position = p.position;
				poi.model.Rotation = p.rotation;
				if ( poi == Game.CurrentWorld.CurrentAvatar ) 
				{
					Game.CurrentWorld.RepositionCamera();
				}
				//				Log.LogMessage( "Position message applied to " + p.instance_id + " rotation " + Rotation );
			}
			#endregion
			#region CombatReport Message
			else if ( m is Strive.Network.Messages.ToClient.CombatReport ) 
			{
				Strive.Network.Messages.ToClient.CombatReport cr = m as Strive.Network.Messages.ToClient.CombatReport;
				switch ( cr.combat_event ) 
				{
					case EnumCombatEvent.Attacks:
						Log.LogMessage(
							cr.attackerObjectInstanceID.ToString() + " attacks " + cr.targetObjectInstanceID.ToString() + "!" );
						break;
					case EnumCombatEvent.Avoids:
						Log.LogMessage(
							cr.targetObjectInstanceID.ToString() + " avoids " + cr.attackerObjectInstanceID.ToString() + "." );
						break;
					case EnumCombatEvent.FleeFails:
						Log.LogMessage(
							cr.attackerObjectInstanceID.ToString() + " attempts to flee but fails." );
						break;
					case EnumCombatEvent.FleeSuccess:
						Log.LogMessage(
							cr.attackerObjectInstanceID.ToString() + " flees!" );
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
			else if ( m is Strive.Network.Messages.ToClient.DropPhysicalObject) 
			{
				Strive.Network.Messages.ToClient.DropPhysicalObject dpo = (Strive.Network.Messages.ToClient.DropPhysicalObject)m;
				Game.CurrentWorld.Remove( dpo.instance_id );
				Log.LogMessage( "Removed "+ dpo.instance_id.ToString() );
			}
				#endregion
			#region MobileState
			else if ( m is Strive.Network.Messages.ToClient.MobileState) 
			{
				Strive.Network.Messages.ToClient.MobileState ms = (Strive.Network.Messages.ToClient.MobileState)m;
				//				Log.LogMessage( "Mobile " + ms.ObjectInstanceID + " is " + ms.State );

				PhysicalObjectInstance poi = Game.CurrentWorld.Find( ms.ObjectInstanceID );
				
				#region 1.1.1 Check that the model exists
				if ( poi == Game.CurrentWorld.CurrentAvatar ) 
				{
					// ignoring self positions for now
					return;
				}
				if ( poi == null || !(poi.model is IActor ) ) 
				{
					Log.ErrorMessage( "Model for " + ms.ObjectInstanceID + " has not been loaded" );
					return;
				}
				#endregion

				IActor actor = (IActor)poi.model;
				actor.AnimationSequence = (int)ms.State;
				actor.playAnimation();
			}
				#endregion
			#region CurrentHitpoints
			else if ( m is Strive.Network.Messages.ToClient.CurrentHitpoints) 
			{
				Strive.Network.Messages.ToClient.CurrentHitpoints chp = (Strive.Network.Messages.ToClient.CurrentHitpoints)m;
				Log.LogMessage( "You now have " + chp.HitPoints );
			}
				#endregion
			#region CanPossess
			else if ( m is Strive.Network.Messages.ToClient.CanPossess ) 
			{
				Strive.Network.Messages.ToClient.CanPossess cp = (Strive.Network.Messages.ToClient.CanPossess)m;
				OnCanPossess( cp );
			}
				#endregion
			#region DropAll
			else if ( m is Strive.Network.Messages.ToClient.DropAll ) 
			{
				Strive.Network.Messages.ToClient.DropAll da = (Strive.Network.Messages.ToClient.DropAll)m;
				Log.LogMessage( "DropAll recieved" );
				Game.CurrentWorld.Clear();
			}
				#endregion
			#region Weather
			else if ( m is Strive.Network.Messages.ToClient.Weather ) 
			{
				Strive.Network.Messages.ToClient.Weather w = (Strive.Network.Messages.ToClient.Weather)m;
				//Log.LogMessage( "Weather update recieved" );
				ITexture t = ResourceManager.LoadTexture(w.SkyTextureID);
				Game.CurrentWorld.SetSky( t );
			}
				#endregion
			#region SkillList
			else if ( m is Strive.Network.Messages.ToClient.SkillList ) 
			{
				Strive.Network.Messages.ToClient.SkillList sl = (Strive.Network.Messages.ToClient.SkillList)m;
				Log.LogMessage( "SkillList recieved" );
				OnSkillList( sl );
			}
				#endregion
			#region WhoList
			else if ( m is Strive.Network.Messages.ToClient.WhoList ) 
			{
				Strive.Network.Messages.ToClient.WhoList wl = (Strive.Network.Messages.ToClient.WhoList)m;
				Log.LogMessage( "WhoList recieved" );
				OnWhoList( wl );
			}
				#endregion
			#region Beat
			else if (m is Strive.Network.Messages.ToClient.Beat)
			{
				Strive.Network.Messages.ToClient.Beat beat = (Strive.Network.Messages.ToClient.Beat)m;
				Log.LogMessage("Beat [" + beat.BeatNumber.ToString() + "].");
			}
				#endregion
			#region Default
			else 
			{
				Log.ErrorMessage( "Unknown message of type " + m.GetType() );
			}
				#endregion
		}
	}

}
