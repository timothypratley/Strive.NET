using System;
using System.Collections;

namespace Strive.Network.Messages {
	/// <summary>
	/// Summary description for MessageTypeMap.
	/// </summary>
	public class MessageTypeMap	{
		public Hashtable messageTypeFromID = new Hashtable();
		public Hashtable idFromMessageType = new Hashtable();
		public enum EnumMessageID {
			ToClientAcknowledge = 1,
			ToClientAddPhysicalObject,
			ToClientAddPhysicalObjects,
			ToClientCanPossess,
			ToClientCombatReport,
			ToClientCommunication,
			ToClientCurrentHitpoints,
			ToClientDropPhysicalObject,
			ToClientDropPhysicalObjects,
			ToClientNegativeAcknowledge,
			ToClientPosition,
			ToClientMobileState,
			ToServerAttack,
			ToServerChangeStance,
			ToServerCommunication,
			ToServerEmote,
			ToServerFlee,
			ToServerTargetAny,
			ToServerTargetItem,
			ToServerTargetMobile,
			ToServerTargetNone,
			ToServerEnterWorldAsMobile,
			ToServerLogin,
			ToServerLogout,
			ToServerPosition
		}
		public MessageTypeMap()	{
			// build the mapping between message_id and message_type
			messageTypeFromID.Add( EnumMessageID.ToClientAcknowledge, typeof( ToClient.Acknowledge ) );
			messageTypeFromID.Add( EnumMessageID.ToClientAddPhysicalObject, typeof( ToClient.AddPhysicalObject ) );
			messageTypeFromID.Add( EnumMessageID.ToClientAddPhysicalObjects, typeof( ToClient.AddPhysicalObjects ) );
			messageTypeFromID.Add( EnumMessageID.ToClientCanPossess, typeof( ToClient.CanPossess ) );
			messageTypeFromID.Add( EnumMessageID.ToClientCombatReport, typeof( ToClient.CombatReport ) );
			messageTypeFromID.Add( EnumMessageID.ToClientCommunication, typeof( ToClient.Communication ) );
			messageTypeFromID.Add( EnumMessageID.ToClientCurrentHitpoints, typeof( ToClient.CurrentHitpoints ) );
			messageTypeFromID.Add( EnumMessageID.ToClientDropPhysicalObject, typeof( ToClient.DropPhysicalObject ) );
			messageTypeFromID.Add( EnumMessageID.ToClientDropPhysicalObjects, typeof( ToClient.DropPhysicalObjects ) );
			messageTypeFromID.Add( EnumMessageID.ToClientNegativeAcknowledge, typeof( ToClient.NegativeAcknowledge ) );
			messageTypeFromID.Add( EnumMessageID.ToClientPosition, typeof( ToClient.Position ) );
			messageTypeFromID.Add( EnumMessageID.ToClientMobileState, typeof( ToClient.MobileState ) );					

			messageTypeFromID.Add( EnumMessageID.ToServerAttack, typeof( ToServer.GameCommand.Attack ) );
			messageTypeFromID.Add( EnumMessageID.ToServerChangeStance, typeof( ToServer.GameCommand.ChangeStance ) );
			messageTypeFromID.Add( EnumMessageID.ToServerCommunication, typeof( ToServer.GameCommand.Communication ) );
			messageTypeFromID.Add( EnumMessageID.ToServerEmote, typeof( ToServer.GameCommand.Emote ) );
			messageTypeFromID.Add( EnumMessageID.ToServerFlee, typeof( ToServer.GameCommand.Flee ) );
			messageTypeFromID.Add( EnumMessageID.ToServerTargetAny, typeof( ToServer.GameCommand.TargetAny ) );
			messageTypeFromID.Add( EnumMessageID.ToServerTargetItem, typeof( ToServer.GameCommand.TargetItem ) );
			messageTypeFromID.Add( EnumMessageID.ToServerTargetMobile, typeof( ToServer.GameCommand.TargetMobile ) );
			messageTypeFromID.Add( EnumMessageID.ToServerTargetNone, typeof( ToServer.GameCommand.TargetNone ) );
			messageTypeFromID.Add( EnumMessageID.ToServerEnterWorldAsMobile, typeof( ToServer.EnterWorldAsMobile ) );
			messageTypeFromID.Add( EnumMessageID.ToServerLogin, typeof( ToServer.Login ) );
			messageTypeFromID.Add( EnumMessageID.ToServerLogout, typeof( ToServer.Logout ) );
			messageTypeFromID.Add( EnumMessageID.ToServerPosition, typeof( ToServer.Position ) );

			// build the reverse lookup
			foreach ( EnumMessageID id in messageTypeFromID.Keys ) {
				idFromMessageType.Add( messageTypeFromID[id], id );
			}
		}
	}
}
