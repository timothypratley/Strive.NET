using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Strive.Network.Messages
{
	/// <summary>
	/// Summary description for CustomFormatter.
	/// </summary>
	public class CustomFormatter {
		public static MessageTypeMap messageTypeMap = new MessageTypeMap();
		public static byte[] Serialize( Object obj ) {
			MemoryStream Buffer = new MemoryStream();
			Type t = obj.GetType();
			byte[] EncodedID = BitConverter.GetBytes(
				(int)messageTypeMap.idFromMessageType[t]
			);
			Buffer.Write( EncodedID, 0, EncodedID.Length );
			FieldInfo[] fi = t.GetFields( );
			foreach( FieldInfo i in fi ) {
				if ( i.FieldType == typeof( Int32 ) ) {
					byte[] EncodedInt = BitConverter.GetBytes((Int32)i.GetValue( obj ));
					Buffer.Write(
						EncodedInt,
						0, EncodedInt.Length
						);
				} else if ( i.FieldType == typeof( string ) ) {
					byte[] EncodedString = Encoding.Unicode.GetBytes(
						((string)i.GetValue( obj ))
						);
					byte[] EncodedInt = BitConverter.GetBytes( EncodedString.Length );
					Buffer.Write( EncodedInt, 0, EncodedInt.Length );
					Buffer.Write( EncodedString, 0, EncodedString.Length );
				} else {
					System.Console.WriteLine( "Unknown type: " + t );
				}
			}
			return Buffer.ToArray();
		}
		
		public static IMessage Deserialize( byte[] buffer ) {
			int Offset = 0;
			int message_id = BitConverter.ToInt32( buffer, Offset );
			Type t = (Type)messageTypeMap.messageTypeFromID[message_id];
			Offset += 4;
			System.Console.WriteLine( t );
			IMessage obj = (IMessage)t.GetConstructor( new System.Type[0] ).Invoke( null );
			FieldInfo[] fi = t.GetFields( );
			foreach( FieldInfo i in fi ) {
				if ( i.FieldType == typeof( Int32 ) ) {
					i.SetValue( obj, BitConverter.ToInt32( buffer, Offset ) );
					Offset += 4;
				} else if ( i.FieldType == typeof( string ) ) {
					int StringLength = BitConverter.ToInt32( buffer, Offset );
					Offset += 4;
					string DecodedString = Encoding.Unicode.GetString( buffer, Offset, StringLength );
					i.SetValue( obj, DecodedString );
					Offset = Offset + StringLength;
				} else {
					System.Console.WriteLine( "Unknown type: " + t );
				}
			}
			return obj;
		}
	}
}
