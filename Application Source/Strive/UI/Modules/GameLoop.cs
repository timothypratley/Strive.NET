using System;
using System.Collections;
using System.IO;

using Strive.Math3D;
using Strive.Network.Client;
using Strive.Network.Messages;
using Strive.Rendering;
using Strive.Rendering.Controls;
using Strive.Rendering.Models;
using Strive.Resources;
using System.Threading;

namespace Strive.UI.Modules
{
	public class GameLoop
	{
		private static bool _isMainRunning = false;
		private static Scene _scene;
		private static System.Windows.Forms.IWin32Window _screen;
		private static ServerConnection _connection;
		public static string modelPath;

		static GameLoop()
		{

		}
	

		public static void Start(Scene scene, System.Windows.Forms.IWin32Window screen, ServerConnection connection)
		{
			if ( _isMainRunning ) {
				throw new Exception( "already running" );
			}
			ResourceManager.SetPath( "D:/projects/Strive/Application Source/Strive/UI/Resources" );
			_scene = scene;
			_screen = screen;
			_connection = connection;
			_isMainRunning = true;
			Thread thisThread = new Thread(	new ThreadStart( MainLoop )	);
			thisThread.Start();
		}

		public static void Stop()
		{
			_isMainRunning = false;
		}

		public static void MainLoop()
		{
			while(_isMainRunning)
			{
				ProcessOutstandingMessages();
				ProcessKeyboardInput();
				ProcessMouseInput();
				ProcessAnimations();
				Render();
			}
		}

		private static void ProcessAnimations() {
			foreach ( Model model in _scene.Models.Values ) {
				if ( model.ModelFormat == ModelFormat.MDL ) {
					model.nextFrame();
				}
			}
		}

		private static void ProcessOutstandingMessages() {
			#region 1.0 Check for messages from server and act upon them
				
			while(_connection.MessageCount > 0) {
				IMessage m = _connection.PopNextMessage();

					#region Communication Message
				if ( m is Strive.Network.Messages.ToClient.Communication ) {
					Strive.Network.Messages.ToClient.Communication c = (Strive.Network.Messages.ToClient.Communication)m;
					if ( c.communicationType == CommunicationType.Chat ) {
						Global._game.displayChat( c.name, c.message );
					} else {
						Global._log.ErrorMessage( "Bad communicationType" );
					}
				}
					#endregion
					#region AddPhysicalObject Message
				else if ( m is Strive.Network.Messages.ToClient.AddPhysicalObject) {
					Strive.Network.Messages.ToClient.AddPhysicalObject apo = (Strive.Network.Messages.ToClient.AddPhysicalObject)m;
					if ( apo.instance_id == Global._myid ) {
						// load self... this contains the players initial position
						_scene.View.Position = new Vector3D( apo.x, apo.y, apo.z );
						_scene.View.Rotation = GetRotationFromHeading( apo.heading_x, apo.heading_y, apo.heading_z );
						Global._log.LogMessage( "Initial position is " + _scene.View.Position );
						Global._log.LogMessage( "Initial rotation is " + _scene.View.Rotation );
						continue;
					}
					Model model = ResourceManager.LoadModel(apo.instance_id, apo.model_id);
					try {
						_scene.Models.Add( model );
					} catch ( Exception ) {
						Global._log.ErrorMessage( "Could not add model " + model.Key );
						continue;
					}
					model.Position = new Vector3D( apo.x, apo.y, apo.z );
					model.Rotation = GetRotationFromHeading( apo.heading_x, apo.heading_y, apo.heading_z );
					Global._log.LogMessage( "Loaded " + model.Key );

				}
					#endregion
					#region Position Message

				else if( m is Strive.Network.Messages.ToClient.Position) {
					Strive.Network.Messages.ToClient.Position p = (Strive.Network.Messages.ToClient.Position)m;
					Model workingModel;
							
						#region 1.1.1 Check that the model exists
					if ( p.instance_id == Global._myid ) {
						// ignoring self positions for now
						continue;
					}
					try {
						workingModel = _scene.Models[p.instance_id.ToString()];
					} catch (Exception) {
						Global._log.ErrorMessage( "Model for " + p.instance_id + " has not been loaded" );
						continue;
					}
						#endregion

						#region 1.1.2 Move and rotate model
						
					workingModel.Position = new Vector3D(p.position_x, p.position_y, p.position_z);
					workingModel.Rotation = GetRotationFromHeading(p.heading_x, p.heading_y, p.heading_z);
					Global._log.LogMessage( "Position message applied to " + p.instance_id + " rotation " + workingModel.Rotation );
						#endregion
				}

					#endregion
					#region CombatReport Message
				else if ( m is Strive.Network.Messages.ToClient.CombatReport ) {
					Strive.Network.Messages.ToClient.CombatReport cr = m as Strive.Network.Messages.ToClient.CombatReport;
					switch ( cr.combat_event ) {
						case EnumCombatEvent.Attacks:
							Global._log.LogMessage(
								cr.attackerObjectInstanceID.ToString() + " attacks " + cr.targetObjectInstanceID.ToString() + "!" );
							break;
						case EnumCombatEvent.Avoids:
							Global._log.LogMessage(
								cr.targetObjectInstanceID.ToString() + " avoids " + cr.attackerObjectInstanceID.ToString() + "." );
							break;
						case EnumCombatEvent.FleeFails:
							Global._log.LogMessage(
								cr.attackerObjectInstanceID.ToString() + " attempts to flee but fails." );
							break;
						case EnumCombatEvent.FleeSuccess:
							Global._log.LogMessage(
								cr.attackerObjectInstanceID.ToString() + " flees!" );
							break;
						case EnumCombatEvent.Hits:
							Global._log.LogMessage(
								cr.attackerObjectInstanceID.ToString() + " hits " + cr.targetObjectInstanceID.ToString() + " for " + cr.damage + " damage." );
							break;
						case EnumCombatEvent.Misses:
							Global._log.LogMessage(
								cr.attackerObjectInstanceID.ToString() + " misses " + cr.targetObjectInstanceID.ToString() + "." );
							break;
						default:
							Global._log.ErrorMessage(
								"Unknown CombatEvent " + cr.combat_event );
							break;
					}
				}
				#endregion
					#region DropPhysicalObject Message
				else if ( m is Strive.Network.Messages.ToClient.DropPhysicalObject) {
					Strive.Network.Messages.ToClient.DropPhysicalObject dpo = (Strive.Network.Messages.ToClient.DropPhysicalObject)m;
					_scene.Models.Remove( dpo.instance_id );
				}
				#endregion
					#region MobileState
				else if ( m is Strive.Network.Messages.ToClient.MobileState) {
					Strive.Network.Messages.ToClient.MobileState ms = (Strive.Network.Messages.ToClient.MobileState)m;
					Global._log.LogMessage( "Mobile " + ms.ObjectInstanceID + " is " + ms.State );
					#region 1.1.1 Check that the model exists
					if ( ms.ObjectInstanceID == Global._myid ) {
						// ignoring self positions for now
						continue;
					}
					Model workingModel;
					try {
						workingModel = _scene.Models[ms.ObjectInstanceID.ToString()];
					} catch (Exception) {
						Global._log.ErrorMessage( "Model for " + ms.ObjectInstanceID + " has not been loaded" );
						continue;
					}
					#endregion

					workingModel.AnimationSequence = (int)ms.State;
				}
				#endregion
					#region CurrentHitpoints
				else if ( m is Strive.Network.Messages.ToClient.CurrentHitpoints) {
					Strive.Network.Messages.ToClient.CurrentHitpoints chp = (Strive.Network.Messages.ToClient.CurrentHitpoints)m;
					Global._log.LogMessage( "You now have " + chp.HitPoints );
				}
				#endregion
					#region CanPossess
				else if ( m is Strive.Network.Messages.ToClient.CanPossess ) {
					Strive.Network.Messages.ToClient.CanPossess cp = (Strive.Network.Messages.ToClient.CanPossess)m;
					Global._log.LogMessage( "You can possess... " );
					foreach ( Strive.Network.Messages.ToClient.CanPossess.id_name_tuple tuple in cp.possesable ) {
						Global._log.LogMessage( "	" + tuple.id + " : " + tuple.name );
						System.Console.WriteLine( "	" + tuple.id + " : " + tuple.name );
					}
					Global._log.LogMessage( "Entering world as default: " + cp.possesable[0].name );
					Global._myid = cp.possesable[0].id;
					Global._serverConnection.Send(new Strive.Network.Messages.ToServer.EnterWorldAsMobile( Global._myid ));
				}
				#endregion
					#region Default
				else {
					Global._log.ErrorMessage( "Unknown message of type " + m.GetType() );
				}
				#endregion
			}

				#endregion
		}

		private static void ProcessMouseInput() {
			#region ProcessMouseInput
			if ( !Global._game._mouseCaptured ) {
				//		System.Windows.Forms.MessageBox.Show("No keyboard");
				return;
			}
			bool WasMouseInput = false;
			Vector3D cameraPosition = _scene.View.Position;
			Vector3D cameraRotation = _scene.View.Rotation;

			Mouse m = Mouse.GetState();
			if( m.x != 0 ) {
				WasMouseInput = true;
				cameraRotation.Y += m.x*0.2f; 
			}
			//if( m.y != 0 ) {
				//WasMouseInput = true;
				//cameraRotation.Y -= m.y; 
			//}
			if(WasMouseInput) {
				_scene.View.Rotation = cameraRotation;
				Vector3D cameraHeading = GetHeadingFromRotation( cameraRotation );
				SendCurrentPosition();
			}
#endregion
		}

		private static void ProcessKeyboardInput() {
			#region 2.0 Get keyboard input 
			if ( !((System.Windows.Forms.PictureBox)_screen).Visible ) {
		//		System.Windows.Forms.MessageBox.Show("No keyboard");
				return;
			}
			bool WasKeyboardInput = false;
			const int moveunit = 5;
			Vector3D cameraPosition = _scene.View.Position;
			Vector3D cameraRotation = _scene.View.Rotation;

			Keyboard.ReadKeys();
			if(Keyboard.GetKeyState(Keys.key_W)) {
				WasKeyboardInput = true;
				cameraPosition.X +=
					(float)Math.Sin( cameraRotation.Y * Math.PI/180.0 ) * moveunit*2;
				cameraPosition.Z +=
					(float)Math.Cos( cameraRotation.Y * Math.PI/180.0 ) * moveunit*2;
			}

			if(Keyboard.GetKeyState(Keys.key_S)) {
				WasKeyboardInput = true;
				cameraPosition.X -=
					(float)Math.Sin( cameraRotation.Y * Math.PI/180.0 ) * moveunit;
				cameraPosition.Z -=
					(float)Math.Cos( cameraRotation.Y * Math.PI/180.0 ) * moveunit;
			}

			if(Keyboard.GetKeyState(Keys.key_D)) {
				WasKeyboardInput = true;
				cameraPosition.X +=
					(float)Math.Cos( cameraRotation.Y * Math.PI/180.0 ) * moveunit/2.0F;
				cameraPosition.Z -=
					(float)Math.Sin( cameraRotation.Y * Math.PI/180.0 ) * moveunit/2.0F;
			}

			if(Keyboard.GetKeyState(Keys.key_A)) {
				WasKeyboardInput = true;
				cameraPosition.X -=
					(float)Math.Cos( cameraRotation.Y * Math.PI/180.0 ) * moveunit/2.0F;
				cameraPosition.Z += 
					(float)Math.Sin( cameraRotation.Y * Math.PI/180.0 ) * moveunit/2.0F;
			}

			if(Keyboard.GetKeyState(Keys.key_Q)) {
				WasKeyboardInput = true;
				cameraRotation.Y -= 5.0F; 
			}

			if(Keyboard.GetKeyState(Keys.key_E)) {
				WasKeyboardInput = true;
				cameraRotation.Y += 5.0F;
			}

			if(Keyboard.GetKeyState(Keys.key_ESCAPE )) {
				WasKeyboardInput = true;
				System.Windows.Forms.Application.Exit();
			}				

			if(WasKeyboardInput) {
				_scene.View.Position = cameraPosition;
				_scene.View.Rotation = cameraRotation;
				SendCurrentPosition();
			}

				#endregion
		}

		private static void SendCurrentPosition() {
			Vector3D cameraHeading = GetHeadingFromRotation( _scene.View.Rotation );
			Network.Messages.ToServer.Position pos = new Network.Messages.ToServer.Position(
				_scene.View.Position.X,
				_scene.View.Position.Y,
				_scene.View.Position.Z,
				cameraHeading.X,
				cameraHeading.Y,
				cameraHeading.Z
			);
			Global._log.LogMessage( "Sending position message rotation " + _scene.View.Rotation );
			_connection.Send(pos);
		}

		private static void Render() {
			#region 3.0 Render
			_scene.Render();
			_scene.Display();
				#endregion
		}

		public static Vector3D GetRotationFromHeading( float x, float y, float z ) {
			// Rotation, convert from heading to Euler angles
			// protect from divide by zero,
			// and convert to degrees, and float.
			double dFlat = Math.Sqrt( x * x + z * z );
			double xTheta;
			if ( dFlat == 0.0 ) {
				xTheta = y > 0 ? Math.PI/2.0 : -Math.PI/2.0;
			} else {
				xTheta = Math.Atan( y/dFlat );
			}
			double yTheta;
			if ( z == 0.0 ) {
				yTheta = x > 0 ? -Math.PI/2.0 : Math.PI/2.0;	
			} else {
				yTheta = Math.Atan( x/z );
				if ( x == 0 && z < 0 ) {
					yTheta = Math.PI;
				}
			}
			return new Vector3D( (float)(xTheta*180.0/Math.PI), (float)(yTheta*180.0/Math.PI), 0.0F );
		}

		public static Vector3D GetHeadingFromRotation( Vector3D rotation ) {
			return new Vector3D(
				(float)Math.Sin( rotation.Y * Math.PI/180.0 ),
				(float)Math.Sin( rotation.X * Math.PI/180.0 ),	
				(float)Math.Cos( rotation.Y * Math.PI/180.0 )
			);
		}
	}
}
