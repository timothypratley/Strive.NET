using System;
using System.Collections;

using Strive.Network.Messages;

namespace Strive.Network.Messages {
	/// <summary>
    /// Contains the mapping between message_ids and message_types.
	/// </summary>
	public class MessageTypeMap	{
		public const int BufferSize = 16384;	// max message size
		public const int MessageLengthLength = 4;
		public Hashtable messageTypeFromID = new Hashtable();
		public Hashtable idFromMessageType = new Hashtable();
		public enum EnumMessageID {
			ToClientAcknowledge = 1,
			ToClientAddReadable,
			ToClientAddEquipable,
			ToClientAddWieldable,
			ToClientAddJunk,
			ToClientAddTerrain,
			ToClientAddMobile,
			ToClientAddQuaffable,
			ToClientCanPossess,
			ToClientCombatReport,
			ToClientCommunication,
			ToClientCurrentHitpoints,
			ToClientDropAll,
			ToClientDropPhysicalObject,
			ToClientDropPhysicalObjects,
			ToClientLogMessage,
			ToClientNegativeAcknowledge,
			ToClientPosition,
			ToClientBeat,
			ToClientMobileState,
			ToClientServerInfo,
			ToClientSkillList,
			ToClientWeather,
			ToClientWhoList,
			ToClientPartyInfo,
			ToClientPing,
			ToServerAttack,
			ToServerChangeStance,
			ToServerCommunication,
			ToServerEmote,
			ToServerFlee,
			ToServerSkillList,
			ToServerUseSkill,
			ToServerCancelSkill,
			ToServerCreateParty,
			ToServerLeaveParty,
			ToServerJoinParty,
			ToServerQueryParty,
			ToServerTransferPartyLeadership,
			ToServerInviteToParty,
			ToServerWhoList,
			ToServerEnterWorldAsMobile,
			ToServerLogin,
			ToServerLogout,
			ToServerReloadWorld,
			ToServerRequestPossessable,
			ToServerRequestServerInfo,
			ToServerPosition,
			ToServerPong
		}
		public MessageTypeMap()	{
			// build the mapping between message_id and message_type
			messageTypeFromID.Add( EnumMessageID.ToClientAcknowledge, typeof( ToClient.Acknowledge ) );
			messageTypeFromID.Add( EnumMessageID.ToClientAddReadable, typeof( ToClient.AddReadable ) );
			messageTypeFromID.Add( EnumMessageID.ToClientAddEquipable, typeof( ToClient.AddEquipable ) );
			messageTypeFromID.Add( EnumMessageID.ToClientAddWieldable, typeof( ToClient.AddWieldable ) );
			messageTypeFromID.Add( EnumMessageID.ToClientAddMobile, typeof( ToClient.AddMobile ) );
			messageTypeFromID.Add( EnumMessageID.ToClientAddJunk, typeof( ToClient.AddJunk ) );
			messageTypeFromID.Add( EnumMessageID.ToClientAddTerrain, typeof( ToClient.AddTerrain ) );
			messageTypeFromID.Add( EnumMessageID.ToClientAddQuaffable, typeof( ToClient.AddQuaffable ) );
			messageTypeFromID.Add( EnumMessageID.ToClientCanPossess, typeof( ToClient.CanPossess ) );
			messageTypeFromID.Add( EnumMessageID.ToClientCombatReport, typeof( ToClient.CombatReport ) );
			messageTypeFromID.Add( EnumMessageID.ToClientCommunication, typeof( ToClient.Communication ) );
			messageTypeFromID.Add( EnumMessageID.ToClientCurrentHitpoints, typeof( ToClient.CurrentHitpoints ) );
			messageTypeFromID.Add( EnumMessageID.ToClientDropAll, typeof( ToClient.DropAll ) );
			messageTypeFromID.Add( EnumMessageID.ToClientDropPhysicalObject, typeof( ToClient.DropPhysicalObject ) );
			messageTypeFromID.Add( EnumMessageID.ToClientDropPhysicalObjects, typeof( ToClient.DropPhysicalObjects ) );
			messageTypeFromID.Add( EnumMessageID.ToClientLogMessage, typeof( ToClient.LogMessage ) );
			messageTypeFromID.Add( EnumMessageID.ToClientNegativeAcknowledge, typeof( ToClient.NegativeAcknowledge ) );
			messageTypeFromID.Add( EnumMessageID.ToClientPosition, typeof( ToClient.Position ) );
			messageTypeFromID.Add( EnumMessageID.ToClientMobileState, typeof( ToClient.MobileState ) );					
			messageTypeFromID.Add( EnumMessageID.ToClientServerInfo, typeof( ToClient.ServerInfo ) );					
			messageTypeFromID.Add( EnumMessageID.ToClientSkillList, typeof( ToClient.SkillList ) );					
			messageTypeFromID.Add( EnumMessageID.ToClientWeather, typeof( ToClient.TimeAndWeather ) );					
			messageTypeFromID.Add( EnumMessageID.ToClientWhoList, typeof( ToClient.WhoList ) );					
			messageTypeFromID.Add( EnumMessageID.ToClientPartyInfo, typeof( ToClient.PartyInfo ) );
			messageTypeFromID.Add( EnumMessageID.ToClientPing, typeof( ToClient.Ping ) );

			messageTypeFromID.Add( EnumMessageID.ToServerChangeStance, typeof( ToServer.ChangeStance ) );
			messageTypeFromID.Add( EnumMessageID.ToServerCommunication, typeof( ToServer.Communication ) );
			messageTypeFromID.Add( EnumMessageID.ToServerEmote, typeof( ToServer.Emote ) );
			messageTypeFromID.Add( EnumMessageID.ToServerSkillList, typeof( ToServer.SkillList ) );
			messageTypeFromID.Add( EnumMessageID.ToServerUseSkill, typeof( ToServer.UseSkill ) );
			messageTypeFromID.Add( EnumMessageID.ToServerCancelSkill, typeof( ToServer.CancelSkill ) );
			messageTypeFromID.Add( EnumMessageID.ToServerCreateParty,typeof( ToServer.CreateParty ) );
			messageTypeFromID.Add( EnumMessageID.ToServerLeaveParty,typeof( ToServer.LeaveParty ) );
			messageTypeFromID.Add( EnumMessageID.ToServerJoinParty,typeof( ToServer.JoinParty ) );
			messageTypeFromID.Add( EnumMessageID.ToServerTransferPartyLeadership,typeof( ToServer.TransferPartyLeadership ) );
			messageTypeFromID.Add( EnumMessageID.ToServerInviteToParty,typeof( ToServer.InviteToParty ) );
			messageTypeFromID.Add( EnumMessageID.ToServerQueryParty,typeof( ToServer.QueryParty ) );
			messageTypeFromID.Add( EnumMessageID.ToServerWhoList, typeof( ToServer.WhoList ) );
			messageTypeFromID.Add( EnumMessageID.ToServerEnterWorldAsMobile, typeof( ToServer.EnterWorldAsMobile ) );
			messageTypeFromID.Add( EnumMessageID.ToServerLogin, typeof( ToServer.Login ) );
			messageTypeFromID.Add( EnumMessageID.ToServerLogout, typeof( ToServer.Logout ) );
			messageTypeFromID.Add( EnumMessageID.ToServerReloadWorld, typeof( ToServer.ReloadWorld ) );
			messageTypeFromID.Add( EnumMessageID.ToServerRequestPossessable, typeof( ToServer.RequestPossessable ) );
			messageTypeFromID.Add( EnumMessageID.ToServerRequestServerInfo, typeof( ToServer.RequestServerInfo ) );
			messageTypeFromID.Add( EnumMessageID.ToServerPosition, typeof( ToServer.Position ) );
			messageTypeFromID.Add( EnumMessageID.ToServerPong, typeof( ToServer.Pong ) );

			// build the reverse lookup
			foreach ( EnumMessageID id in messageTypeFromID.Keys ) {
				idFromMessageType.Add( messageTypeFromID[id], id );
			}
		}
	}
}
