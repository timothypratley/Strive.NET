using System;

using Strive.Network.Messages;
using Strive.Math3D;
using Strive.Rendering.Models;
using Strive.Resources;
using Strive.Logging;
using Strive.Multiverse;


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

		public MessageProcessor() {
		}

		public void Process( IMessage m ) {
			#region Communication Message
			if ( m is Strive.Network.Messages.ToClient.Communication ) {
				Strive.Network.Messages.ToClient.Communication c = (Strive.Network.Messages.ToClient.Communication)m;
				if ( c.communicationType == CommunicationType.Chat ) {
					OnChat(c);
				} else {
					Log.ErrorMessage( "Bad communicationType" );
				}
			}
					#endregion
			#region AddPhysicalObject Message
			else if ( m is Strive.Network.Messages.ToClient.AddPhysicalObject) {
				PhysicalObject po;
				if ( m is Strive.Network.Messages.ToClient.AddMobile ) {
					Strive.Network.Messages.ToClient.AddMobile am = m as Strive.Network.Messages.ToClient.AddMobile;
					po = new Mobile();
					((Mobile)po).MobileState = am.state;
				} else if ( m is Strive.Network.Messages.ToClient.AddQuaffable ) {
					Strive.Network.Messages.ToClient.AddQuaffable aq = m as Strive.Network.Messages.ToClient.AddQuaffable;
					po = new Quaffable();
				} else if ( m is Strive.Network.Messages.ToClient.AddReadable ) {
					Strive.Network.Messages.ToClient.AddReadable ar = m as Strive.Network.Messages.ToClient.AddReadable;
					po = new Readable();
				} else if ( m is Strive.Network.Messages.ToClient.AddEquipable ) {
					Strive.Network.Messages.ToClient.AddEquipable ae = m as Strive.Network.Messages.ToClient.AddEquipable;
					po = new Equipable();
				} else if ( m is Strive.Network.Messages.ToClient.AddWieldable ) {
					Strive.Network.Messages.ToClient.AddWieldable aw = m as Strive.Network.Messages.ToClient.AddWieldable;
					po = new Wieldable();
				} else if ( m is Strive.Network.Messages.ToClient.AddJunk ) {
					Strive.Network.Messages.ToClient.AddJunk aj = m as Strive.Network.Messages.ToClient.AddJunk;
					po = new Junk();
				} else if ( m is Strive.Network.Messages.ToClient.AddTerrain ) {
					Strive.Network.Messages.ToClient.AddTerrain at = m as Strive.Network.Messages.ToClient.AddTerrain;
					po = new Terrain();
				} else {
					Log.ErrorMessage( "Unknown PhysicalObject received - " + m.GetType() );
					return;
				}
				Strive.Network.Messages.ToClient.AddPhysicalObject apo = m as Strive.Network.Messages.ToClient.AddPhysicalObject;
				po.ObjectInstanceID = apo.instance_id;
				po.ModelID = apo.model_id;
				po.ObjectTemplateName = apo.name;
				Vector3D position = new Vector3D( apo.x, apo.y, apo.z );
				Vector3D rotation = Helper.GetRotationFromHeading( apo.heading_x, apo.heading_y, apo.heading_z );
				try {
					Game.CurrentWorld.Add( po, position, rotation );
				} catch ( Exception e ) {
					Log.ErrorMessage( "Got a double add message." );
				}
				if ( apo.instance_id == Game.CurrentPlayerID ) {
					// load self... this contains the players initial position
					Game.CurrentWorld.Possess( apo.instance_id );
					Log.LogMessage( "Initial position is " + position );
					Log.LogMessage( "Initial rotation is " + rotation );
					return;
				}
				Log.LogMessage( "Added object " + apo.instance_id + " with model " + apo.model_id + " at " + position );
			}
					#endregion
			#region Position Message

			else if( m is Strive.Network.Messages.ToClient.Position) {
				Strive.Network.Messages.ToClient.Position p = (Strive.Network.Messages.ToClient.Position)m;
				PhysicalObjectInstance poi = Game.CurrentWorld.Find( p.instance_id );
				if ( poi == null ) {
					Log.ErrorMessage( "Model for " + p.instance_id + " has not been loaded" );
					return;
				}
				poi.model.Position = new Vector3D( p.position_x, p.position_y, p.position_z );
				poi.model.Rotation = Helper.GetRotationFromHeading( p.heading_x, p.heading_y, p.heading_z );
				if ( poi == Game.CurrentWorld.CurrentAvatar ) {
					Game.CurrentWorld.RepositionCamera();
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
			else if ( m is Strive.Network.Messages.ToClient.DropPhysicalObject) {
				Strive.Network.Messages.ToClient.DropPhysicalObject dpo = (Strive.Network.Messages.ToClient.DropPhysicalObject)m;
				Game.CurrentWorld.Remove( dpo.instance_id );
				Log.LogMessage( "Removed "+ dpo.instance_id.ToString() );
			}
				#endregion
			#region MobileState
			else if ( m is Strive.Network.Messages.ToClient.MobileState) {
				Strive.Network.Messages.ToClient.MobileState ms = (Strive.Network.Messages.ToClient.MobileState)m;
				//				Log.LogMessage( "Mobile " + ms.ObjectInstanceID + " is " + ms.State );

				PhysicalObjectInstance poi = Game.CurrentWorld.Find( ms.ObjectInstanceID );
				
				#region 1.1.1 Check that the model exists
				if ( poi == Game.CurrentWorld.CurrentAvatar ) {
					// ignoring self positions for now
					return;
				}
				if ( poi == null ) {
					Log.ErrorMessage( "Model for " + ms.ObjectInstanceID + " has not been loaded" );
					return;
				}
				#endregion

				poi.model.AnimationSequence = (int)ms.State;
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
				Game.CurrentWorld.Clear();
			}
				#endregion
			#region Weather
			else if ( m is Strive.Network.Messages.ToClient.Weather ) {
				Strive.Network.Messages.ToClient.Weather w = (Strive.Network.Messages.ToClient.Weather)m;
				//Log.LogMessage( "Weather update recieved" );
				string texture_name = ResourceManager.LoadTexture(w.SkyTextureID);
				Game.CurrentWorld.SetSky( texture_name );
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
				Log.LogMessage( "WhoList recieved" );
				OnWhoList( wl );
			}
				#endregion
			#region Default
			else {
				Log.ErrorMessage( "Unknown message of type " + m.GetType() );
			}
				#endregion
		}
	}

}
