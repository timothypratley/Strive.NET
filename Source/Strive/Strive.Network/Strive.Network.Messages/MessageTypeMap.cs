using System;
using System.Collections.Generic;
using Strive.Model;

namespace Strive.Network.Messages
{
    /// <summary>
    /// Contains the mapping between message_ids and message_types.
    /// </summary>
    public class MessageTypeMap
    {
        public const int BufferSize = 16384;	// max message size
        public const int MessageLengthLength = sizeof(Int16);
        public readonly Dictionary<EnumMessageId, Type> MessageTypeFromId = new Dictionary<EnumMessageId, Type>();
        public readonly Dictionary<Type, EnumMessageId> IdFromMessageType = new Dictionary<Type, EnumMessageId>();

        public enum EnumMessageId
        {
            ToClientEntity,
            ToClientTask,
            ToClientCanPossess,
            ToClientCombatReport,
            ToClientCommunication,
            ToClientCurrentHealth,
            ToClientDropAll,
            ToClientDropPhysicalObject,
            ToClientDropPhysicalObjects,
            ToClientLogMessage,
            ToClientPosition,
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
            ToServerRequestSkillList,
            ToServerUseSkill,
            ToServerCancelSkill,
            ToServerLeaveParty,
            ToServerJoinParty,
            ToServerQueryParty,
            ToServerTransferPartyLeadership,
            ToServerInviteToParty,
            ToServerRequestWhoList,
            ToServerCreateEntity,
            ToServerCreateTask,
            ToServerPossess,
            ToServerProduceEntity,
            ToServerLogin,
            ToServerLogout,
            ToServerReloadWorld,
            ToServerRequestPossessable,
            ToServerRequestServerInfo,
            ToServerMyPosition,
            ToServerPong
        }

        public MessageTypeMap()
        {
            // build the mapping between message_id and message_type
            MessageTypeFromId.Add(EnumMessageId.ToClientEntity, typeof(EntityModel));
            MessageTypeFromId.Add(EnumMessageId.ToClientTask, typeof(TaskModel));
            MessageTypeFromId.Add(EnumMessageId.ToClientCanPossess, typeof(ToClient.CanPossess));
            MessageTypeFromId.Add(EnumMessageId.ToClientCombatReport, typeof(ToClient.CombatReport));
            MessageTypeFromId.Add(EnumMessageId.ToClientCommunication, typeof(ToClient.Communication));
            MessageTypeFromId.Add(EnumMessageId.ToClientCurrentHealth, typeof(ToClient.CurrentHealth));
            MessageTypeFromId.Add(EnumMessageId.ToClientDropAll, typeof(ToClient.DropAll));
            MessageTypeFromId.Add(EnumMessageId.ToClientDropPhysicalObject, typeof(ToClient.DropPhysical));
            MessageTypeFromId.Add(EnumMessageId.ToClientDropPhysicalObjects, typeof(ToClient.DropPhysicalObjects));
            MessageTypeFromId.Add(EnumMessageId.ToClientLogMessage, typeof(ToClient.LogMessage));
            MessageTypeFromId.Add(EnumMessageId.ToClientPosition, typeof(ToClient.PositionUpdate));
            MessageTypeFromId.Add(EnumMessageId.ToClientMobileState, typeof(ToClient.MobileState));
            MessageTypeFromId.Add(EnumMessageId.ToClientServerInfo, typeof(ToClient.ServerInfo));
            MessageTypeFromId.Add(EnumMessageId.ToClientSkillList, typeof(ToClient.SkillList));
            MessageTypeFromId.Add(EnumMessageId.ToClientWeather, typeof(ToClient.TimeAndWeather));
            MessageTypeFromId.Add(EnumMessageId.ToClientWhoList, typeof(ToClient.WhoList));
            MessageTypeFromId.Add(EnumMessageId.ToClientPartyInfo, typeof(ToClient.PartyInfo));
            MessageTypeFromId.Add(EnumMessageId.ToClientPing, typeof(ToClient.Ping));

            MessageTypeFromId.Add(EnumMessageId.ToServerChangeStance, typeof(ToServer.ChangeStance));
            MessageTypeFromId.Add(EnumMessageId.ToServerCommunication, typeof(ToServer.Communicate));
            MessageTypeFromId.Add(EnumMessageId.ToServerEmote, typeof(ToServer.Emote));
            MessageTypeFromId.Add(EnumMessageId.ToServerRequestSkillList, typeof(ToServer.RequestSkillList));
            MessageTypeFromId.Add(EnumMessageId.ToServerUseSkill, typeof(ToServer.UseSkill));
            MessageTypeFromId.Add(EnumMessageId.ToServerCancelSkill, typeof(ToServer.CancelSkill));
            MessageTypeFromId.Add(EnumMessageId.ToServerLeaveParty, typeof(ToServer.LeaveParty));
            MessageTypeFromId.Add(EnumMessageId.ToServerJoinParty, typeof(ToServer.JoinParty));
            MessageTypeFromId.Add(EnumMessageId.ToServerTransferPartyLeadership, typeof(ToServer.TransferPartyLeadership));
            MessageTypeFromId.Add(EnumMessageId.ToServerInviteToParty, typeof(ToServer.InviteToParty));
            MessageTypeFromId.Add(EnumMessageId.ToServerQueryParty, typeof(ToServer.QueryParty));
            MessageTypeFromId.Add(EnumMessageId.ToServerRequestWhoList, typeof(ToServer.RequestWhoList));
            MessageTypeFromId.Add(EnumMessageId.ToServerPossess, typeof(ToServer.PossessMobile));
            MessageTypeFromId.Add(EnumMessageId.ToServerCreateEntity, typeof(ToServer.CreateEntity));
            MessageTypeFromId.Add(EnumMessageId.ToServerCreateTask, typeof(ToServer.CreateTask));
            MessageTypeFromId.Add(EnumMessageId.ToServerProduceEntity, typeof(ToServer.ProduceEntity));
            MessageTypeFromId.Add(EnumMessageId.ToServerLogin, typeof(ToServer.Login));
            MessageTypeFromId.Add(EnumMessageId.ToServerLogout, typeof(ToServer.Logout));
            MessageTypeFromId.Add(EnumMessageId.ToServerReloadWorld, typeof(ToServer.ReloadWorld));
            MessageTypeFromId.Add(EnumMessageId.ToServerRequestPossessable, typeof(ToServer.RequestPossessable));
            MessageTypeFromId.Add(EnumMessageId.ToServerRequestServerInfo, typeof(ToServer.RequestServerInfo));
            MessageTypeFromId.Add(EnumMessageId.ToServerMyPosition, typeof(ToServer.MyPosition));
            MessageTypeFromId.Add(EnumMessageId.ToServerPong, typeof(ToServer.Pong));

            // build the reverse lookup
            foreach (EnumMessageId id in MessageTypeFromId.Keys)
                IdFromMessageType.Add(MessageTypeFromId[id], id);
        }
    }
}
