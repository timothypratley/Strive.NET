using System;
using System.Collections;
using System.IO;

using Strive.Math3D;
using Strive.Network.Client;
using Strive.Network.Messages;
using Strive.Rendering;
using Strive.Rendering.Controls;
using Strive.Rendering.Models;


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
			_scene = scene;
			_screen = screen;
			_connection = connection;
			_isMainRunning = true;
		}

		public static void Stop()
		{
			_isMainRunning = false;
		}

		public static void Main()
		{
			while(_isMainRunning)
			{
				ProcessOutstandingMessages();
				ProcessKeyboardInput();
				Render();
			}

			Stop();
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
						System.Console.WriteLine( "Bad communicationType" );
					}
				}
					#endregion
					#region AddPhysicalObject Message
				else if ( m is Strive.Network.Messages.ToClient.AddPhysicalObject) {
					Strive.Network.Messages.ToClient.AddPhysicalObject apo = (Strive.Network.Messages.ToClient.AddPhysicalObject)m;
					if ( apo.spawn_id == Global._myid ) {
						// load self... this contains the players initial position
						_scene.View.Position = new Vector3D( apo.x, apo.y, apo.z );
						_scene.View.Rotation = GetRotationFromHeading( apo.heading_x, apo.heading_y, apo.heading_z );
						System.Console.WriteLine( "Initial position is " + _scene.View.Position );
						System.Console.WriteLine( "Initial rotation is " + _scene.View.Rotation );
						continue;
					}
					// EEERRR hack, need better way of desciding what is a mdl what is a 3ds
					Model model = Resources.ResourceManager.LoadModel(apo.spawn_id);
					try {
						_scene.Models.Add( model );
					} catch ( Exception ) {
						System.Console.WriteLine( "Could not add model " + model.Key );
						continue;
					}
					model.Position = new Vector3D( apo.x, apo.y, apo.z );
					model.Rotation = GetRotationFromHeading( apo.heading_x, apo.heading_y, apo.heading_z );
				}
					#endregion
					#region 1.1 Position Message

				else if( m is Strive.Network.Messages.ToClient.Position) {
					Strive.Network.Messages.ToClient.Position p = (Strive.Network.Messages.ToClient.Position)m;
					Model workingModel;
							
						#region 1.1.1 Check that the model exists
					if ( p.spawn_id == Global._myid ) {
						// ignoring self positions for now
						continue;
					}
					try {
						workingModel = _scene.Models[p.spawn_id.ToString()];
					} catch (Exception) {
						System.Console.WriteLine( "Model for " + p.spawn_id + " has not been loaded" );
						continue;
					}
						#endregion

						#region 1.1.2 Move and rotate model
						
					workingModel.Position = new Vector3D(p.position_x, p.position_y, p.position_z);
					workingModel.Rotation = GetRotationFromHeading(p.heading_x, p.heading_y, p.heading_z);

					//System.Console.WriteLine( "Position message applied to " + p.spawn_id );
						#endregion
				}

					#endregion
			}

				#endregion
		}

		private static void ProcessKeyboardInput() {
			#region 2.0 Get keyboard input 
				
			bool WasKeyboardInput = false;
			const int moveunit = 5;
			Vector3D cameraPosition = _scene.View.Position;
			Vector3D cameraRotation = _scene.View.Rotation;

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

			_scene.View.Position = cameraPosition;
			_scene.View.Rotation = cameraRotation;
			Network.Messages.ToServer.Position pos = new Network.Messages.ToServer.Position();
			Vector3D cameraHeading = GetHeadingFromRotation( cameraRotation );
			pos.heading_x = cameraHeading.X;
			pos.heading_y = cameraHeading.Y;
			pos.heading_z = cameraHeading.Z;
			pos.position_x = _scene.View.Position.X;
			pos.position_y = _scene.View.Position.Y;
			pos.position_z = _scene.View.Position.Z;
				

			if(WasKeyboardInput) {
				_connection.Send(pos);
			}

				#endregion
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
