using System;
using System.Collections.Generic;

namespace Strive.Network.Messages
{
    /// <summary>
    /// Contains the mapping between message_ids and message_types.
    /// </summary>
    public class MessageTypeMap
    {
        public const int BufferSize = 16384;	// max message size
        public const int MessageLengthLength = 4;
        public Dictionary<EnumMessageId, Type> MessageTypeFromId = new Dictionary<EnumMessageId, Type>();
        public Dictionary<Type, EnumMessageId> IdFromMessageType = new Dictionary<Type, EnumMessageId>();

        public enum EnumMessageId
        {
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

        public MessageTypeMap()
        {
            // build the mapping between message_id and message_type
            MessageTypeFromId.Add(EnumMessageId.ToClientAcknowledge, typeof(ToClient.Acknowledge));
            MessageTypeFromId.Add(EnumMessageId.ToClientAddReadable, typeof(ToClient.AddReadable));
            MessageTypeFromId.Add(EnumMessageId.ToClientAddEquipable, typeof(ToClient.AddEquipable));
            MessageTypeFromId.Add(EnumMessageId.ToClientAddWieldable, typeof(ToClient.AddWieldable));
            MessageTypeFromId.Add(EnumMessageId.ToClientAddMobile, typeof(ToClient.AddMobile));
            MessageTypeFromId.Add(EnumMessageId.ToClientAddJunk, typeof(ToClient.AddJunk));
            MessageTypeFromId.Add(EnumMessageId.ToClientAddTerrain, typeof(ToClient.AddTerrain));
            MessageTypeFromId.Add(EnumMessageId.ToClientAddQuaffable, typeof(ToClient.AddQuaffable));
            MessageTypeFromId.Add(EnumMessageId.ToClientCanPossess, typeof(ToClient.CanPossess));
            MessageTypeFromId.Add(EnumMessageId.ToClientCombatReport, typeof(ToClient.CombatReport));
            MessageTypeFromId.Add(EnumMessageId.ToClientCommunication, typeof(ToClient.Communication));
            MessageTypeFromId.Add(EnumMessageId.ToClientCurrentHitpoints, typeof(ToClient.CurrentHitpoints));
            MessageTypeFromId.Add(EnumMessageId.ToClientDropAll, typeof(ToClient.DropAll));
            MessageTypeFromId.Add(EnumMessageId.ToClientDropPhysicalObject, typeof(ToClient.DropPhysicalObject));
            MessageTypeFromId.Add(EnumMessageId.ToClientDropPhysicalObjects, typeof(ToClient.DropPhysicalObjects));
            MessageTypeFromId.Add(EnumMessageId.ToClientLogMessage, typeof(ToClient.LogMessage));
            MessageTypeFromId.Add(EnumMessageId.ToClientNegativeAcknowledge, typeof(ToClient.NegativeAcknowledge));
            MessageTypeFromId.Add(EnumMessageId.ToClientPosition, typeof(ToClient.Position));
            MessageTypeFromId.Add(EnumMessageId.ToClientMobileState, typeof(ToClient.MobileState));
            MessageTypeFromId.Add(EnumMessageId.ToClientServerInfo, typeof(ToClient.ServerInfo));
            MessageTypeFromId.Add(EnumMessageId.ToClientSkillList, typeof(ToClient.SkillList));
            MessageTypeFromId.Add(EnumMessageId.ToClientWeather, typeof(ToClient.TimeAndWeather));
            MessageTypeFromId.Add(EnumMessageId.ToClientWhoList, typeof(ToClient.WhoList));
            MessageTypeFromId.Add(EnumMessageId.ToClientPartyInfo, typeof(ToClient.PartyInfo));
            MessageTypeFromId.Add(EnumMessageId.ToClientPing, typeof(ToClient.Ping));

            MessageTypeFromId.Add(EnumMessageId.ToServerChangeStance, typeof(ToServer.ChangeStance));
            MessageTypeFromId.Add(EnumMessageId.ToServerCommunication, typeof(ToServer.Communication));
            MessageTypeFromId.Add(EnumMessageId.ToServerEmote, typeof(ToServer.Emote));
            MessageTypeFromId.Add(EnumMessageId.ToServerSkillList, typeof(ToServer.SkillList));
            MessageTypeFromId.Add(EnumMessageId.ToServerUseSkill, typeof(ToServer.UseSkill));
            MessageTypeFromId.Add(EnumMessageId.ToServerCancelSkill, typeof(ToServer.CancelSkill));
            MessageTypeFromId.Add(EnumMessageId.ToServerCreateParty, typeof(ToServer.CreateParty));
            MessageTypeFromId.Add(EnumMessageId.ToServerLeaveParty, typeof(ToServer.LeaveParty));
            MessageTypeFromId.Add(EnumMessageId.ToServerJoinParty, typeof(ToServer.JoinParty));
            MessageTypeFromId.Add(EnumMessageId.ToServerTransferPartyLeadership, typeof(ToServer.TransferPartyLeadership));
            MessageTypeFromId.Add(EnumMessageId.ToServerInviteToParty, typeof(ToServer.InviteToParty));
            MessageTypeFromId.Add(EnumMessageId.ToServerQueryParty, typeof(ToServer.QueryParty));
            MessageTypeFromId.Add(EnumMessageId.ToServerWhoList, typeof(ToServer.WhoList));
            MessageTypeFromId.Add(EnumMessageId.ToServerEnterWorldAsMobile, typeof(ToServer.EnterWorldAsMobile));
            MessageTypeFromId.Add(EnumMessageId.ToServerLogin, typeof(ToServer.Login));
            MessageTypeFromId.Add(EnumMessageId.ToServerLogout, typeof(ToServer.Logout));
            MessageTypeFromId.Add(EnumMessageId.ToServerReloadWorld, typeof(ToServer.ReloadWorld));
            MessageTypeFromId.Add(EnumMessageId.ToServerRequestPossessable, typeof(ToServer.RequestPossessable));
            MessageTypeFromId.Add(EnumMessageId.ToServerRequestServerInfo, typeof(ToServer.RequestServerInfo));
            MessageTypeFromId.Add(EnumMessageId.ToServerPosition, typeof(ToServer.Position));
            MessageTypeFromId.Add(EnumMessageId.ToServerPong, typeof(ToServer.Pong));

            // build the reverse lookup
            foreach (EnumMessageId id in MessageTypeFromId.Keys)
            {
                IdFromMessageType.Add(MessageTypeFromId[id], id);
            }
        }
    }
}
