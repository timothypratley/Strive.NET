using System;

using Strive.Network.Messages;
using Strive.Math3D;
using Strive.Rendering.Models;
using Strive.Resources;
using Strive.Logging;


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
				Strive.Network.Messages.ToClient.AddPhysicalObject apo = (Strive.Network.Messages.ToClient.AddPhysicalObject)m;
				if ( apo.instance_id == Game.CurrentPlayerID ) {
					// load self... this contains the players initial position
					Game.CurrentScene.View.Position = new Vector3D( apo.x, apo.y, apo.z );
					Game.CurrentScene.View.Rotation = Helper.GetRotationFromHeading( apo.heading_x, apo.heading_y, apo.heading_z );
					Log.LogMessage( "Initial position is " + Game.CurrentScene.View.Position );
					Log.LogMessage( "Initial rotation is " + Game.CurrentScene.View.Rotation );
					return;
				}
				Model model;
				try {
					model = ResourceManager.LoadModel(apo.instance_id, apo.model_id);
					Game.CurrentScene.Models.Add( model );
				} catch ( Exception e ) {
					Log.ErrorMessage( "Could not add model " + apo.instance_id );
					Log.ErrorMessage( e.Message );
					return;
				}
				model.Position = new Vector3D( apo.x, apo.y, apo.z );
				model.Rotation = Helper.GetRotationFromHeading( apo.heading_x, apo.heading_y, apo.heading_z );
				Log.LogMessage( "Added object " + apo.instance_id + " with model " + apo.model_id + " at " + model.Position );
			}
					#endregion
			#region Position Message

			else if( m is Strive.Network.Messages.ToClient.Position) {
				Strive.Network.Messages.ToClient.Position p = (Strive.Network.Messages.ToClient.Position)m;
				Model workingModel;
							
						#region 1.1.1 Check that the model exists
				if ( p.instance_id == Game.CurrentPlayerID) {
					Game.CurrentScene.View.Position = new Vector3D( p.position_x, p.position_y, p.position_z );
					Game.CurrentScene.View.Rotation = Helper.GetRotationFromHeading( p.heading_x, p.heading_y, p.heading_z );
					return;
				}
				try {
					workingModel = Game.CurrentScene.Models[p.instance_id.ToString()];
				} catch (Exception) {
					Log.ErrorMessage( "Model for " + p.instance_id + " has not been loaded" );
					return;
				}
						#endregion

						#region 1.1.2 Move and rotate model
						
				workingModel.Position = new Vector3D(p.position_x, p.position_y, p.position_z);
				workingModel.Rotation = Helper.GetRotationFromHeading(p.heading_x, p.heading_y, p.heading_z);
				//				Log.LogMessage( "Position message applied to " + p.instance_id + " rotation " + workingModel.Rotation );
						#endregion
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
				Game.CurrentScene.Models.Remove( dpo.instance_id.ToString() );
				Log.LogMessage( "Removed "+ dpo.instance_id.ToString() );
			}
				#endregion
			#region MobileState
			else if ( m is Strive.Network.Messages.ToClient.MobileState) {
				Strive.Network.Messages.ToClient.MobileState ms = (Strive.Network.Messages.ToClient.MobileState)m;
				//				Log.LogMessage( "Mobile " + ms.ObjectInstanceID + " is " + ms.State );
					#region 1.1.1 Check that the model exists
				if ( ms.ObjectInstanceID == Game.CurrentPlayerID ) {
					// ignoring self positions for now
					return;
				}
				Model workingModel;
				try {
					workingModel = Game.CurrentScene.Models[ms.ObjectInstanceID.ToString()];
				} catch (Exception) {
					Log.ErrorMessage( "Model for " + ms.ObjectInstanceID + " has not been loaded" );
					return;
				}
					#endregion

				workingModel.AnimationSequence = (int)ms.State;
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
				Game.CurrentScene.DropAll();
			}
				#endregion
			#region Weather
			else if ( m is Strive.Network.Messages.ToClient.Weather ) {
				Strive.Network.Messages.ToClient.Weather w = (Strive.Network.Messages.ToClient.Weather)m;
				//Log.LogMessage( "Weather update recieved" );
				string texture_name = ResourceManager.LoadTexture(w.SkyTextureID);
				Game.CurrentScene.SetSky( "sky", texture_name );
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
