using System;

using Strive.Server.Model;

namespace Strive.Network.Messages.ToClient
{
	/// <summary>
	/// A helper class to construct add messages,
	/// do not create instances of this class!
	/// </summary>
	public class AddPhysicalObject : IMessage {
		protected AddPhysicalObject(){}
		public static IMessage CreateMessage( PhysicalObject po ) {
			if ( po is Equipable ) {
				return new AddEquipable( (Equipable)po );
			} else if ( po is Junk ) {
				return new AddJunk( (Junk)po );
			} else if ( po is Mobile ) {
				return new AddMobile( (Mobile)po );
			} else if ( po is Quaffable ) {
				return new AddQuaffable( (Quaffable)po );
			} else if ( po is Readable ) {
				return new AddReadable( (Readable)po );
			} else if ( po is Terrain ) {
				return new AddTerrain( (Terrain)po );
			} else if ( po is Wieldable ) {
				return new AddWieldable( (Wieldable)po );
			} else {
				throw new Exception( "AddPhysicalObject of unknown type " + po.GetType() );
			}
		}

		public static PhysicalObject GetPhysicalObject( IMessage message ) {
			if ( message is AddEquipable ) {
				return ((AddEquipable)message).Equipable;
			} else if ( message is AddJunk ) {
				return ((AddJunk)message).Junk;
			} else if ( message is AddMobile ) {
				return ((AddMobile)message).Mobile;
			} else if ( message is AddQuaffable ) {
				return ((AddQuaffable)message).Quaffable;
			} else if ( message is AddReadable ) {
				return ((AddReadable)message).Readable;
			} else if ( message is AddTerrain ) {
				return ((AddTerrain)message).Terrain;
			} else if ( message is AddWieldable ) {
				return ((AddWieldable)message).Weildable;
			} else {
				throw new Exception( "Unknown AddPhysicalObject message type " + message.GetType() );
			}
		}
	}
}
