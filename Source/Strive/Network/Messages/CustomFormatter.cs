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

			// message starts with unique type identifier
			Type t = obj.GetType();
			byte[] EncodedID = BitConverter.GetBytes(
				(int)messageTypeMap.idFromMessageType[t]
			);
			Buffer.Write( EncodedID, 0, EncodedID.Length );
			Encode( obj, Buffer );
			return Buffer.ToArray();
		}

		public static void Encode( Object obj, MemoryStream Buffer ) {
			// if the object is a basic type, encode and return
			if ( EncodeBasicType( obj, Buffer ) ) return;

			// otherwise encode its fields
			Type t = obj.GetType();
			FieldInfo[] fi = t.GetFields();
			foreach( FieldInfo i in fi ) {
				Object field = i.GetValue( obj );
				//Convert.ChangeType( field, i.FieldType );
				Encode( field, Buffer );
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
					Encode( a.GetValue(j), Buffer );
				}
			} else {
				return false;
			}
			return true;
		}

		public static Object Deserialize( byte[] buffer ) {
			int Offset = 0;
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
