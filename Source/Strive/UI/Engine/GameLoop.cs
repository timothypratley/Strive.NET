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
			const int moveunit = 2;
			Vector3D avatarPosition = Game.CurrentWorld.CurrentAvatar.model.Position;
			Vector3D avatarRotation = Game.CurrentWorld.CurrentAvatar.model.Rotation;
			Vector3D changeOfPosition = new Vector3D( 0, 0, 0 );
			if ( keyboard.GetKeyState(Key.key_W) ) {
				WasKeyboardInput = true;
				changeOfPosition.X += (float)Math.Sin( avatarRotation.Y * Math.PI/180.0 ) * moveunit;
				changeOfPosition.Z += (float)Math.Cos( avatarRotation.Y * Math.PI/180.0 ) * moveunit;
			}

			if ( keyboard.GetKeyState(Key.key_S) ) {
				WasKeyboardInput = true;
				changeOfPosition.X -= (float)Math.Sin( avatarRotation.Y * Math.PI/180.0 ) * moveunit/2F;
				changeOfPosition.Z -= (float)Math.Cos( avatarRotation.Y * Math.PI/180.0 ) * moveunit/2F;
			}

			if ( keyboard.GetKeyState(Key.key_D) ) {
				WasKeyboardInput = true;
				changeOfPosition.X += (float)Math.Cos( avatarRotation.Y * Math.PI/180.0 ) * moveunit/2F;
				changeOfPosition.Z -= (float)Math.Sin( avatarRotation.Y * Math.PI/180.0 ) * moveunit/2F;
			}

			if( keyboard.GetKeyState(Key.key_A) ) {
				WasKeyboardInput = true;
				changeOfPosition.X -=	(float)Math.Cos( avatarRotation.Y * Math.PI/180.0 ) * moveunit/2F;
				changeOfPosition.Z +=	(float)Math.Sin( avatarRotation.Y * Math.PI/180.0 ) * moveunit/2F;
			}

			if( keyboard.GetKeyState(Key.key_Q) ) {
				WasKeyboardInput = true;
				avatarRotation.Y -= moveunit*2F;
			}

			if(keyboard.GetKeyState(Key.key_E)) {
				WasKeyboardInput = true;
				avatarRotation.Y += moveunit*2F;
			}

			if(keyboard.GetKeyState(Key.key_ESCAPE )) {
				WasKeyboardInput = true;
				System.Windows.Forms.Application.Exit();
			}				

			if(WasKeyboardInput) {
				// stay on the ground
				// TODO: What about when walking on objects?
				Vector3D newPosition = avatarPosition + changeOfPosition;
				newPosition.Y = Game.CurrentWorld.TerrainPieces.AltitudeAt( avatarPosition.X, avatarPosition.Z ) + Game.CurrentWorld.CurrentAvatar.physicalObject.Height/2;
				changeOfPosition.Y = newPosition.Y - avatarPosition.Y;

				// check that we can go there
				foreach ( PhysicalObjectInstance poi in Game.CurrentWorld.physicalObjectInstances.Values ) {
					if ( poi.physicalObject is Terrain ) {
						continue;
					}
					if ( poi == Game.CurrentWorld.CurrentAvatar ) {
						//ignore ourselves
						continue;
					}

					// do a bounding sphere test to see if the movement will go near poi
					// test from the middle of the line drawn between current position
					// and future position, distance is the radius of both objects
					// plus half the distance of the movement.
					float dx = avatarPosition.X + changeOfPosition.X / 2F - poi.model.Position.X;
					float dy = avatarPosition.Y + changeOfPosition.Y / 2F - poi.model.Position.Y;
					float dz = avatarPosition.Z + changeOfPosition.Z / 2F - poi.model.Position.Z;
					float distance_squared = dx*dx + dy*dy + dz*dz;
					float distance_moved = changeOfPosition.X * changeOfPosition.X + changeOfPosition.Y * changeOfPosition.Y + changeOfPosition.Z * changeOfPosition.Z;
// TODO: optimize, no sqrts
					if ( Math.Sqrt(distance_squared) < Math.Sqrt(poi.model.RadiusSquared) + Math.Sqrt(Game.CurrentWorld.CurrentAvatar.model.RadiusSquared) + Math.Sqrt(distance_moved)/2F ) {
						// ok, these objects are close enough to collide,
						// but did a collision really happen?
						// now we do a more acurate test to find out.

						Vector3D boxmin1 = new Vector3D();
						Vector3D boxmax1 = new Vector3D();
						Vector3D boxmin2 = new Vector3D();
						Vector3D boxmax2 = new Vector3D();
						poi.model.GetBoundingBox( boxmin1, boxmax1 );
						Game.CurrentWorld.CurrentAvatar.model.GetBoundingBox( boxmin2, boxmax2 );
						Vector3D halfbox1size = (boxmax1 - boxmin1)/2;
						boxmin2 -= halfbox1size;
						boxmax2 += halfbox1size;
						boxmin2 += poi.model.Position;
						boxmax2 += poi.model.Position;

						if (
							avatarPosition.X > boxmin2.X
							&& avatarPosition.Y > boxmin2.Y
							&& avatarPosition.Z > boxmin2.Z
							&& avatarPosition.X < boxmax2.X
							&& avatarPosition.Y < boxmax2.Y
							&& avatarPosition.Z < boxmax2.Z
						) {
							// already in a collision, ignore collision detection
							break;
						}

						if (
							newPosition.X > boxmin2.X
							&& newPosition.Y > boxmin2.Y
							&& newPosition.Z > boxmin2.Z
							&& newPosition.X < boxmax2.X
							&& newPosition.Y < boxmax2.Y
							&& newPosition.Z < boxmax2.Z
						) {
							// would be a collision
							changeOfPosition.Set( 0, 0, 0 );
							newPosition.Set( avatarPosition );
							break;
							// TODO: should actually figure out the collision point
						}
					}
				}
				Game.CurrentWorld.CurrentAvatar.model.Position = avatarPosition+changeOfPosition;
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
