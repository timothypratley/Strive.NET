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
using Strive.Common;
using Strive.Logging;
using Strive.UI.WorldView;
using Strive.Multiverse;

namespace Strive.UI.Engine
{
	public class GameLoop
	{
		public MessageProcessor _message_processor = new MessageProcessor();
		public string modelPath;
		ServerConnection _connection;
		StoppableThread thisThread;

		public IKeyboard keyboard = Game.RenderingFactory.Keyboard;
		public IMouse mouse = Game.RenderingFactory.Mouse;

		public GameLoop()
		{
			thisThread = new StoppableThread( new StoppableThread.WhileRunning( MainLoop ) );
		}
	

		public void Start(ServerConnection connection)
		{
			_connection = connection;
			thisThread.Start();
		}

		public void Stop() {
			thisThread.Stop();
		}

		public void MainLoop()
		{
			ProcessOutstandingMessages();
			if ( Game.GameControlMode ) {
				ProcessKeyboardInput();
				ProcessMouseInput();
			}
			Game.CurrentWorld.Render();
			System.Threading.Thread.Sleep( 10 );
		}

		void ProcessOutstandingMessages() {
			while(_connection.MessageCount > 0) {
				IMessage m = _connection.PopNextMessage();
				_message_processor.Process( m );
			}
		}

		// todo: replace pitch with a more elegant solution
		float pitch = 0;
		void ProcessMouseInput() {
			#region ProcessMouseInput

			// no avatar... no movement
			if ( Game.CurrentWorld.CurrentAvatar == null ) return;

			bool WasMouseInput = false;
			Vector3D avatarPosition = Game.CurrentWorld.CurrentAvatar.model.Position;
			Vector3D avatarRotation = Game.CurrentWorld.CurrentAvatar.model.Rotation;

			mouse.GetState();
			if( mouse.X != 0 ) {
				WasMouseInput = true;
				avatarRotation.Y += mouse.X*0.2f; 
				avatarRotation.X = pitch;
			}
			if( mouse.Y != 0 ) {
				WasMouseInput = true;
				pitch += mouse.Y*0.2f;
				if ( pitch > 60 ) { pitch = 60; }
				if ( pitch < -60 ) { pitch = -60; }
				avatarRotation.X = pitch;
			}
			// todo: only send once for mouse or keyboard input
			if(WasMouseInput) {
				Game.CurrentWorld.CurrentAvatar.model.Rotation = avatarRotation;
				Game.CurrentWorld.RepositionCamera();
				SendCurrentPosition();
			}
#endregion
		}

		void ProcessKeyboardInput() {
			#region 2.0 Get keyboard input 

			// no avatar... no movement
			if ( Game.CurrentWorld.CurrentAvatar == null ) return;

			bool WasKeyboardInput = false;
			const int moveunit = 5;
			Vector3D avatarPosition = Game.CurrentWorld.CurrentAvatar.model.Position.Clone();
			Vector3D avatarRotation = Game.CurrentWorld.CurrentAvatar.model.Rotation;
			if(keyboard.GetKeyState(Key.key_W)) {
				WasKeyboardInput = true;
				avatarPosition.X +=
					(float)Math.Sin( avatarRotation.Y * Math.PI/180.0 ) * moveunit*2;
				avatarPosition.Z +=
					(float)Math.Cos( avatarRotation.Y * Math.PI/180.0 ) * moveunit*2;
			}

			if(keyboard.GetKeyState(Key.key_S)) {
				WasKeyboardInput = true;
				avatarPosition.X -=
					(float)Math.Sin( avatarRotation.Y * Math.PI/180.0 ) * moveunit;
				avatarPosition.Z -=
					(float)Math.Cos( avatarRotation.Y * Math.PI/180.0 ) * moveunit;
			}

			if(keyboard.GetKeyState(Key.key_D)) {
				WasKeyboardInput = true;
				avatarPosition.X +=
					(float)Math.Cos( avatarRotation.Y * Math.PI/180.0 ) * moveunit/2.0F;
				avatarPosition.Z -=
					(float)Math.Sin( avatarRotation.Y * Math.PI/180.0 ) * moveunit/2.0F;
			}

			if(keyboard.GetKeyState(Key.key_A)) {
				WasKeyboardInput = true;
				avatarPosition.X -=
					(float)Math.Cos( avatarRotation.Y * Math.PI/180.0 ) * moveunit/2.0F;
				avatarPosition.Z += 
					(float)Math.Sin( avatarRotation.Y * Math.PI/180.0 ) * moveunit/2.0F;
			}

			if(keyboard.GetKeyState(Key.key_Q)) {
				WasKeyboardInput = true;
				avatarRotation.Y -= 5.0F; 
			}

			if(keyboard.GetKeyState(Key.key_E)) {
				WasKeyboardInput = true;
				avatarRotation.Y += 5.0F;
			}

			if(keyboard.GetKeyState(Key.key_ESCAPE )) {
				WasKeyboardInput = true;
				System.Windows.Forms.Application.Exit();
			}				

			if(WasKeyboardInput) {
				// stay on the ground
				// TODO: What about when walking on objects?
				avatarPosition.Y = Game.CurrentWorld.TerrainPieces.AltitudeAt( avatarPosition.X, avatarPosition.Z ) + Game.CurrentWorld.CurrentAvatar.physicalObject.Height/2;

				// check that we can go there
				foreach ( PhysicalObjectInstance poi in Game.CurrentWorld.physicalObjectInstances.Values ) {
					if ( poi.physicalObject is Terrain ) {
						continue;
					}
					if ( poi == Game.CurrentWorld.CurrentAvatar ) {
						continue;
					}
					float dx1 = Game.CurrentWorld.CurrentAvatar.model.Position.X - poi.model.Position.X;
					float dz1 = Game.CurrentWorld.CurrentAvatar.model.Position.Z - poi.model.Position.Z;
					float dy1 = Game.CurrentWorld.CurrentAvatar.model.Position.Y - poi.model.Position.Y;
					float distance_squared1 = dx1*dx1 + dz1*dz1 + dy1*dy1;
					if ( distance_squared1 < poi.model.BoundingSphereRadiusSquared + Game.CurrentWorld.CurrentAvatar.model.BoundingSphereRadiusSquared ) {
						// already a collision, ignore collision detection
						continue;
					}
					float dx = avatarPosition.X - poi.model.Position.X;
					float dz = avatarPosition.Z - poi.model.Position.Z;
					float dy = avatarPosition.Y - poi.model.Position.Y;
					float distance_squared = dx*dx + dz*dz + dy*dy;
					if ( distance_squared < poi.model.BoundingSphereRadiusSquared + Game.CurrentWorld.CurrentAvatar.model.BoundingSphereRadiusSquared ) {
						//Log.LogMessage( "Canceled move due to collision" );
						return;
					}
				}
				Game.CurrentWorld.CurrentAvatar.model.Position = avatarPosition;
				Game.CurrentWorld.CurrentAvatar.model.Rotation = avatarRotation;
				Game.CurrentWorld.RepositionCamera();
				SendCurrentPosition();
			}

				#endregion
		}

		void SendCurrentPosition() {
			// no avatar... no position
			if ( Game.CurrentWorld.CurrentAvatar == null ) return;

			_connection.Position(
				Game.CurrentWorld.CurrentAvatar.model.Position, 
				Game.CurrentWorld.CurrentAvatar.model.Rotation );
		}
	}
}
