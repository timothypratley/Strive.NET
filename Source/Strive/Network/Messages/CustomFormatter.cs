using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections;

namespace Strive.Network.Messages {
	/// <summary>
	/// Summary description for CustomFormatter.
	/// </summary>
	public class CustomFormatter {
		public static MessageTypeMap messageTypeMap = new MessageTypeMap();

		public static byte[] Serialize( Object obj ) {
			MemoryStream Buffer = new MemoryStream();

			// unique type identifier
			Type t = obj.GetType();
			byte[] EncodedInt;
			try {
				EncodedInt = BitConverter.GetBytes((int)messageTypeMap.idFromMessageType[t]
				);
			} catch ( Exception  ) {
				throw new Exception( "Message " + t + " has not been added to MessageTypeMap" );
			}
			// reserve space for the message length field, we will fill it later,
			// and fill out the unique type identifier
			Buffer.Write( EncodedInt, 0, EncodedInt.Length );
			Buffer.Write( EncodedInt, 0, EncodedInt.Length );

			// encode the object
			Encode( obj, Buffer, t );

			// now fill in the length field, the first field in the message.
			EncodedInt = BitConverter.GetBytes( (int)Buffer.Length );
			Buffer.Position = 0;
			Buffer.Write( EncodedInt, 0, EncodedInt.Length );
			return Buffer.ToArray();
		}

		public static void Encode( Object obj, MemoryStream Buffer, Type t ) {
			// if the object is a basic type, encode and return
			if ( EncodeBasicType( obj, Buffer ) ) return;

			FieldInfo[] fi = t.GetFields();
			foreach( FieldInfo i in fi ) {
				if ( i.IsStatic ) continue;
				Object field = i.GetValue( obj );
				if ( field == null ) {
					throw new Exception( "Cannot serialise object with null fields" );
				}
				// NB: we encode to a specific type,
				// this prevents encoding derived types,
				// which would break the message protocol.
				// derived types will be encoded as messages of the
				// type they were assigned in the message_id lookup.
				Encode( field, Buffer, i.FieldType );
			}
		}
		
		public static bool EncodeBasicType( Object obj, MemoryStream Buffer ) {
			Type t = obj.GetType();
			if ( t.IsEnum ) {
				byte[] EncodedInt = BitConverter.GetBytes((Int32)obj);
				Buffer.Write(
					EncodedInt,
					0, EncodedInt.Length );
					
			} else if ( t == typeof( Int32 ) ) {
				byte[] EncodedInt = BitConverter.GetBytes((Int32)obj);
				Buffer.Write(
					EncodedInt,
					0, EncodedInt.Length
					);
			} else if ( t == typeof( float ) ) {
				byte[] EncodedFloat = BitConverter.GetBytes((float)obj);
				Buffer.Write(
					EncodedFloat,
					0, EncodedFloat.Length
					);
			} else if ( t == typeof( string ) ) {
				byte[] EncodedString = Encoding.Unicode.GetBytes((string)obj);
				byte[] EncodedInt = BitConverter.GetBytes( EncodedString.Length );
				Buffer.Write( EncodedInt, 0, EncodedInt.Length );
				Buffer.Write( EncodedString, 0, EncodedString.Length );
			} else if ( t.IsArray ) {
				Array a = (Array)obj;
				byte[] EncodedLength = BitConverter.GetBytes( a.Length );
				Buffer.Write( EncodedLength, 0, EncodedLength.Length );
				for ( int j=0; j<a.Length; j++ ) {
					// recursively encode the objects of the array
					object o = a.GetValue(j);
					Encode( o, Buffer, o.GetType() );
				}
			} else {
				return false;
			}
			return true;
		}

		public static Object Deserialize( byte[] buffer, int Offset ) {
			MessageTypeMap.EnumMessageID message_id = (MessageTypeMap.EnumMessageID)BitConverter.ToInt32( buffer, Offset );
			Type t = (Type)messageTypeMap.messageTypeFromID[message_id];
			Offset += 4;
			//Log.LogMessage( t );

			return Decode( t, buffer, ref Offset );
		}

		public static Object Decode( Type t, byte[] buffer, ref int Offset ) {
			// if its a basic type, return it
			Object obj = DecodeBasicType( t, buffer, ref Offset );
			if ( obj != null ) return obj;

			// otherwise create the complex object, and decode its fields
			obj = t.GetConstructor( new System.Type[0] ).Invoke( null );
			FieldInfo[] fi = t.GetFields();
			foreach( FieldInfo i in fi ) {
				if ( i.IsStatic ) continue;
				i.SetValue( obj, Decode( i.FieldType, buffer, ref Offset ) );
			}
			return obj;
		}

		public static Object DecodeBasicType( Type t, byte[] buffer, ref int Offset ) {
			Object result = null;
			if ( t == typeof( Int32 ) ) {
				result = BitConverter.ToInt32( buffer, Offset );
				Offset += 4;
			} else if ( t.IsEnum ) {
				result = Enum.ToObject( t, BitConverter.ToInt32( buffer, Offset ) );
				Offset += 4;
			} else if ( t == typeof( float ) ) {
				result = BitConverter.ToSingle( buffer, Offset );
				Offset += 4;
			} else if ( t == typeof( string ) ) {
				int StringLength = BitConverter.ToInt32( buffer, Offset );
				Offset += 4;
				result = Encoding.Unicode.GetString( buffer, Offset, StringLength );
				Offset += StringLength;
			} else if ( t.IsArray ) {
				int length = BitConverter.ToInt32( buffer, Offset );
				Offset += 4;
				ArrayList DecodedArray = new ArrayList();
				for ( int j=0; j<length; j++ ) {
					DecodedArray.Add(
						Decode( t.GetElementType(), buffer, ref Offset )
					);
				}
				result = DecodedArray.ToArray( t.GetElementType() );
			}
			return result;
		}
	}
}
