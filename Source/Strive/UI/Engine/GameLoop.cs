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

		public void Stop() 
		{
			thisThread.Stop();
		}

		public void MainLoop()
		{
			// Only get the current time once per iteration.
			// This is faster, and makes time apply uniformly for the update.
			Game.now = DateTime.Now;

			// handle network input from the server
			ProcessOutstandingMessages();

			// handle user input
			//if ( Game.GameControlMode ) {
			ProcessPlayerInput();
			//}

			// display the new world
			try 
			{
				Game.CurrentWorld.Render();
			} 
			catch ( Exception ) 
			{
				Log.LogMessage( "Render failed." );
				Stop();
			}

			// give the CPU a breather
			System.Threading.Thread.Sleep( 10 );
		}

		void ProcessOutstandingMessages() 
		{
			// TODO: we need to be carefull its not possible to flood
			int i = 0;
			while(_connection.MessageCount > 0) 
			{
				IMessage m = _connection.PopNextMessage();
				if ( m == null ) break;
				_message_processor.Process( m );
				i++;
				if ( i>5 ) break;
			}
		}

		// todo: replace pitch with a more elegant solution
		float pitch = 0;
		void ProcessPlayerInput() 
		{
			// no avatar... no nothing
			if ( Game.CurrentWorld.CurrentAvatar == null ) return;

			if ( keyboard.GetKeyState( Key.key_F5 ) ) 
			{
				Game.CurrentMainWindow.SetGameControlMode();
			} 
			else if ( keyboard.GetKeyState( Key.key_F6 ) ) 
			{
				Game.CurrentMainWindow.ReleaseGameControlMode();
			}
			if ( Game.GameControlMode ) 
			{
				if ( keyboard.GetKeyState( Key.key_ESCAPE ) ) 
				{
					Game.CurrentMainWindow.ReleaseGameControlMode();
				} 
				else if ( keyboard.GetKeyState( Key.key_1 ) ) 
				{
					Game.CurrentGameCommand = EnumSkill.Kill;
				} 
				else if ( keyboard.GetKeyState( Key.key_2 ) ) 
				{
					Game.CurrentGameCommand = EnumSkill.AcidBlast;
				} 
				else if ( keyboard.GetKeyState( Key.key_3 ) ) 
				{
					Game.CurrentGameCommand = EnumSkill.Kick;
				} 
				else if ( keyboard.GetKeyState( Key.key_4 ) ) 
				{
					Game.CurrentGameCommand = EnumSkill.Levitate;
				} 
				else if ( keyboard.GetKeyState( Key.key_0 ) ) 
				{
					Game.CurrentGameCommand = EnumSkill.None;
				}
			}

			// if not in game control mode, don't respond to game controls
			if ( !Game.GameControlMode ) 
			{
				return;
			}
			
			float moveunit = 1.39F * Game.RenderingFactory.TimeSinceLastFrame()/1000F;
			if ( keyboard.GetKeyState(Key.key_LEFTSHIFT) ) {
				moveunit *= 2.5F;
			}

			
			#region ProcessRotationInput

			Vector3D avatarPosition = Game.CurrentWorld.CurrentAvatar.model.Position.Clone();
			Vector3D newRotation = Game.CurrentWorld.CurrentAvatar.model.Rotation.Clone();
			mouse.GetState();
			if( mouse.X != 0 ) 
			{
				newRotation.Y += mouse.X*0.2f; 
				newRotation.X = pitch;
			}
			if( mouse.Y != 0 ) 
			{
				pitch += mouse.Y*0.2f;
				if ( pitch > 60 ) { pitch = 60; }
				if ( pitch < -60 ) { pitch = -60; }
				newRotation.X = pitch;
			}

			if( keyboard.GetKeyState(Key.key_Q) ) 
			{
				newRotation.Y -= moveunit*2F;
			}
			if(keyboard.GetKeyState(Key.key_E)) 
			{
				newRotation.Y += moveunit*2F;
			}

			#endregion
			#region 2.0 Process Movement Input


			Vector3D changeOfPosition = new Vector3D( 0, 0, 0 );
			if ( keyboard.GetKeyState(Key.key_W) ) 
			{
				changeOfPosition.X += (float)Math.Sin( newRotation.Y * Math.PI/180.0 ) * moveunit;
				changeOfPosition.Z += (float)Math.Cos( newRotation.Y * Math.PI/180.0 ) * moveunit;
			}
			if ( keyboard.GetKeyState(Key.key_S) ) 
			{
				changeOfPosition.X -= (float)Math.Sin( newRotation.Y * Math.PI/180.0 ) * moveunit/2F;
				changeOfPosition.Z -= (float)Math.Cos( newRotation.Y * Math.PI/180.0 ) * moveunit/2F;
			}
			if ( keyboard.GetKeyState(Key.key_D) ) 
			{
				changeOfPosition.X += (float)Math.Cos( newRotation.Y * Math.PI/180.0 ) * moveunit/2F;
				changeOfPosition.Z -= (float)Math.Sin( newRotation.Y * Math.PI/180.0 ) * moveunit/2F;
			}
			if( keyboard.GetKeyState(Key.key_A) ) 
			{
				changeOfPosition.X -=	(float)Math.Cos( newRotation.Y * Math.PI/180.0 ) * moveunit/2F;
				changeOfPosition.Z +=	(float)Math.Sin( newRotation.Y * Math.PI/180.0 ) * moveunit/2F;
			}

			if( changeOfPosition.GetMagnitudeSquared() != 0 ) 
			{
				// stay on the ground
				// TODO: What about when walking on objects?
				Vector3D newPosition = avatarPosition + changeOfPosition;
				try 
				{
					newPosition.Y = Game.CurrentWorld.TerrainPieces.AltitudeAt( newPosition.X, newPosition.Z ) + Game.CurrentWorld.CurrentAvatar.physicalObject.Height/2;
					changeOfPosition.Y = newPosition.Y - avatarPosition.Y;

					// check that we can go there
					foreach ( PhysicalObjectInstance poi in Game.CurrentWorld.physicalObjectInstances.Values ) 
					{
						if ( poi.physicalObject is Terrain ) 
						{
							continue;
						}
						if ( poi == Game.CurrentWorld.CurrentAvatar ) 
						{
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
						if ( Math.Sqrt(distance_squared) < Math.Sqrt(poi.model.RadiusSquared) + Math.Sqrt(Game.CurrentWorld.CurrentAvatar.model.RadiusSquared) + Math.Sqrt(distance_moved)/2F ) 
						{
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
								) 
							{
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
								) 
							{
								// would be a collision
								changeOfPosition.Set( 0, 0, 0 );
								break;
								// TODO: should actually figure out the collision point
							}
						}
					}
				} 
				catch ( TerrainCollection.InvalidLocationException ) 
				{
					changeOfPosition.Set( 0, 0, 0 );
				}
				// NB: PlayerMovement is called regardless,
				// as we need to update values for message throtling
			}

			#endregion
			PlayerMovement( avatarPosition, changeOfPosition, newRotation );
			Game.CurrentWorld.RepositionCamera();
		}

		public void PlayerMovement( Vector3D oldPosition, Vector3D velocity, Vector3D newRotation ) 
		{
			PhysicalObjectInstance poi = Game.CurrentWorld.CurrentAvatar;
			poi.Move( oldPosition, velocity, newRotation );
			if ( poi.NeedsUpdate( Game.now ) ) 
			{
				_connection.Position(
					poi.model.Position, 
					poi.model.Rotation );
				poi.SentUpdate( Game.now );
			}
		}
	}
}
