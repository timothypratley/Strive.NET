using System;
using System.Collections;
using System.Threading;
using System.Net;

using Common.Logging;
using Strive.Network.Messages;
using ToClient = Strive.Network.Messages.ToClient;
using ToServer = Strive.Network.Messages.ToServer;
using Strive.Network.Server;
using Strive.Math3D;
using Strive.Server.Model;
using Strive.Common;


namespace Strive.Server.Logic
{
    public class MessageProcessor
    {
        World world;
        Listener listener;
        ILog Log = LogManager.GetCurrentClassLogger();

        public MessageProcessor(World world, Listener listener)
        {
            this.world = world;
            this.listener = listener;
        }

        public void ProcessNextMessage()
        {
            ClientMessage clientMessage = (ClientMessage)listener.PopNextMessage();
            try
            {
                ProcessMessage(clientMessage.Client, clientMessage.Message);
            }
            catch (Exception e)
            {
                Log.Error("Message was not processed correctly, resuming.", e);
            }
        }

        public void ProcessMessage(Client client, IMessage message)
        {
            if (!client.Authenticated)
            {
                // new connection... only allow login
                if (message is ToServer.Login)
                {
                    ProcessLoginMessage(client, (ToServer.Login)message);
                }
                else
                {
                    Log.Warn("Non-login message " + message.GetType() + " from " + client.EndPoint);
                }
                return;
            }

            if (message is ToServer.RequestPossessable)
            {
                ProcessRequestPossessable(client, message as ToServer.RequestPossessable);
                return;
            }

            if (client.Avatar == null)
            {
                // no character selected yet... only allow posses
                // and logout
                if (message is ToServer.EnterWorldAsMobile)
                {
                    ProcessEnterWorldAsMobile(client, (ToServer.EnterWorldAsMobile)message);
                }
                else if (message is ToServer.Logout)
                {

                }
                else
                {

                    Log.Warn("ERROR: Non-posses message " + message.GetType() + " from " + client.EndPoint);
                }
                return;
            }

            // normal message for logged in user
            if (message is ToServer.Position)
            {
                ProcessPositionMessage(client, message as ToServer.Position);
            }
            else if (message is ToServer.Communication)
            {
                ProcessChatMessage(client, message as ToServer.Communication);
            }
            else if (message is ToServer.UseSkill)
            {
                SkillCommandProcessor.ProcessUseSkill(client, message as ToServer.UseSkill);
            }
            else if (message is ToServer.CancelSkill)
            {
                SkillCommandProcessor.ProcessCancelSkill(client, message as ToServer.CancelSkill);
            }
            else if (message is ToServer.ReloadWorld)
            {
                ProcessReloadWorldMessage(client, message as ToServer.ReloadWorld);
            }
            else if (message is ToServer.Logout)
            {
                ProcessLogout(client);
            }
            else if (message is ToServer.SkillList)
            {
                ProcessSkillList(client, message as ToServer.SkillList);
            }
            else if (message is ToServer.WhoList)
            {
                ProcessWhoList(client, message as ToServer.WhoList);
            }
            else if (message is ToServer.CreateParty)
            {
                ProcessCreateParty(client, message as ToServer.CreateParty);
            }
            else if (message is ToServer.CreateParty)
            {
                ProcessLeaveParty(client, message as ToServer.LeaveParty);
            }
            else if (message is ToServer.JoinParty)
            {
                ProcessJoinParty(client, message as ToServer.JoinParty);
            }
            else if (message is ToServer.InviteToParty)
            {
                ProcessInviteToParty(client, message as ToServer.InviteToParty);
            }
            else if (message is ToServer.TransferPartyLeadership)
            {
                ProcessTransferPartyLeadership(client, message as ToServer.TransferPartyLeadership);
            }
            else if (message is ToServer.Pong)
            {
                ProcessPong(client, message as ToServer.Pong);
            }
            else
            {
                Log.Warn("ERROR: Unknown message " + message.GetType());
            }
        }

        void ProcessLoginMessage(Client client, ToServer.Login loginMessage)
        {
            if (
                world.UserLookup(loginMessage.username, loginMessage.password, ref client.PlayerID)
            )
            {
                // login succeeded, check there isnt an existing connection for this
                // player
                Client old = (Client)listener.Clients[loginMessage.username];
                if (old != null)
                {
                    old.Close();
                }
                Log.Info("User " + loginMessage.username + " logged in");
                client.AuthenticatedUsername = loginMessage.username;
                client.Protocol = loginMessage.protocol;
            }
            else
            {
                Log.Info("Login failed for username " + loginMessage.username + " " + loginMessage.password);
                client.Close();
            }
        }

        void ProcessRequestPossessable(Client client, ToServer.RequestPossessable message)
        {
            //Strive.Data.MultiverseFactory.refreshMultiverseForPlayer(Global.modelSchema, client.PlayerID);
            ToClient.CanPossess canPossess = new ToClient.CanPossess(
                world.getPossessable(client.AuthenticatedUsername));
            client.Send(canPossess);
        }

        void ProcessLogout(Client client)
        {
            if (client.Avatar != null)
            {
                // remove from world.
                world.Remove(client.Avatar);
            }
            Log.Info("Logged out '" + client.AuthenticatedUsername + "'.");
            client.Close();
        }



        void ProcessEnterWorldAsMobile(Client client, ToServer.EnterWorldAsMobile message)
        {
            MobileAvatar a;
            if (world.physicalObjects.ContainsKey(message.InstanceID))
            {
                // reconnected
                // simply replace existing connection with the new
                // todo: the old 'connection' should timeout or die or be killed
                Object o = world.physicalObjects[message.InstanceID];
                if (o is MobileAvatar)
                {
                    a = (MobileAvatar)o;
                }
                else
                {
                    Log.Warn("Can only possess mobiles.");
                    return;
                }
                if (a.client == client)
                {
                    Log.Warn("A client " + client.ToString() + " attempted to take control of the same mobile " + a.ObjectInstanceID + " twice... ignoring request.");
                }
                else if (a.client != null)
                {
                    Log.Info("Mobile " + a.ObjectInstanceID + " has been taken over by a new connection.");
                    a.client.Avatar = null;
                    a.client.Close();
                    listener.Clients.Remove(a.client.EndPoint);
                    a.client = client;
                    client.Avatar = a;
                }
                else
                {
                    a.client = client;
                    client.Avatar = a;
                }
            }
            else
            {
                // try to load the character
                a = world.LoadMobile(message.InstanceID);
                if (a == null)
                {
                    Log.Warn("Character " + message.InstanceID + " not found.");
                    client.Close();
                    return;
                }
                a.client = client;
                client.Avatar = a;

                // try to add the character to the world
                world.Add(client.Avatar);
            }
            world.SendInitialWorldView(client);
        }

        void ProcessPositionMessage(
            Client client, ToServer.Position message
        )
        {
            if (client.Avatar == null)
            {
                Log.Warn("Position message from client " + client.EndPoint + " who has no avatar.");
                return;
            }
            MobileAvatar ma = (MobileAvatar)client.Avatar;

            // don't go to running for pure heading changes
            if (message.position != client.Avatar.Position)
            {
                ma.lastMoveUpdate = Global.now;
                if (ma.MobileState != EnumMobileState.Running)
                {
                    ma.SetMobileState(EnumMobileState.Running);
                }
            }
            world.Relocate(client.Avatar, message.position, message.rotation);
        }

        void ProcessChatMessage(
            Client client, ToServer.Communication message
        )
        {
            if (message.communicationType == CommunicationType.Chat)
            {
                world.NotifyMobiles(new ToClient.Communication(client.Avatar.TemplateObjectName, message.message, (Strive.Network.Messages.CommunicationType)message.communicationType));
            }
            else if (message.communicationType == CommunicationType.PartyTalk)
            {
                MobileAvatar ma = (MobileAvatar)client.Avatar;
                ma.SendPartyTalk(message.message);
            }
            else
            {
                Log.Error("Unexpected CommunicationType " + message.communicationType);
            }
            //Log.Info( "Sent communication message" );
        }

        void ProcessReloadWorldMessage(
            Client client, ToServer.ReloadWorld message
        )
        {
            Log.Info("ReloadWorld received.");
            world.Load();
            foreach (Client c in listener.Clients.Values)
            {
                if (c.Avatar != null)
                {
                    // respawn their mobile, old instance will be given over
                    // to Garbage Collector
                    c.Send(new ToClient.DropAll());
                    ProcessEnterWorldAsMobile(c, new ToServer.EnterWorldAsMobile(c.Avatar.ObjectInstanceID));
                }
            }
        }

        void ProcessWhoList(
            Client client, ToServer.WhoList message
        )
        {
            ArrayList MobileIDs = new ArrayList();
            ArrayList Names = new ArrayList();
            foreach (Client c in listener.Clients.Values)
            {
                if (c.Avatar != null)
                {
                    MobileIDs.Add(c.Avatar.ObjectInstanceID);
                    Names.Add(c.Avatar.TemplateObjectName);
                }
            }
            client.Send(new ToClient.WhoList((int[])MobileIDs.ToArray(typeof(int)), (string[])Names.ToArray(typeof(string))));
        }

        void ProcessCreateParty(Client client, ToServer.CreateParty message)
        {
            MobileAvatar ma = (MobileAvatar)client.Avatar;
            if (ma.party != null)
            {
                ma.SendLog("You are currently in party '" + ma.party.Name + "'.");
                return;
            }
            ma.party = new Party(message.name, ma);
        }

        void ProcessTransferPartyLeadership(
            Client client, ToServer.TransferPartyLeadership message
        )
        {
            MobileAvatar ma = (MobileAvatar)client.Avatar;
            MobileAvatar target = (MobileAvatar)Global.world.physicalObjects[message.ObjectInstanceID];
            if (ma.party != target.party)
            {
                ma.SendLog("You are not in the same party as " + target.TemplateObjectName);
                return;
            }
            ma.party.Leader = target;
            ma.SendPartyTalk("Party leadership has been transfered to " + target.TemplateObjectName);
        }

        void ProcessPong(Client client, ToServer.Pong message)
        {
            client.latency = (DateTime.Now - client.pingedAt).Milliseconds;
            world.weather.Latency = client.latency;
            client.Send(world.weather);
        }

        void ProcessLeaveParty(
            Client client, ToServer.LeaveParty message
        )
        {
            MobileAvatar ma = (MobileAvatar)client.Avatar;
            Party p = ma.party;
            p.Remove(ma.ObjectInstanceID);
            ma.SendLog("You have left party '" + p.Name + "'.");
            ma.party = null;
            p.SendPartyTalk(ma.TemplateObjectName + " has left your ");
        }

        void ProcessJoinParty(Client client, ToServer.JoinParty message)
        {
            MobileAvatar ma = (MobileAvatar)client.Avatar;

            // make sure they are trying to join the party they were invited to
            if (message.ObjectInstanceID != ma.invitedToParty.Leader.ObjectInstanceID)
            {
                ma.SendLog("Join party failed, get a new invitation.");
                return;
            }

            ma.invitedToParty.SendPartyTalk(ma.TemplateObjectName + " has joined your ");
            ma.party = ma.invitedToParty;
            ma.party.Add(ma);
            ma.SendLog("You are now in party '" + ma.party.Name + "'.");
            ma.invitedToParty = null;
        }

        void ProcessInviteToParty(Client client, ToServer.InviteToParty message)
        {
            Party p = ((MobileAvatar)client.Avatar).party;
            if (p == null)
            {
                client.SendLog("You are not in a ");
                return;
            }
            if (p.Leader != client.Avatar)
            {
                client.SendLog("You are not the party leader.");
                return;
            }
            MobileAvatar target = (MobileAvatar)Global.world.physicalObjects[message.ObjectInstanceID];
            target.invitedToParty = p;
            // TODO: probabbly want a graphical thing or something for invites
            target.SendLog("You have been invited to party '" + p.Name + "'.");
        }

        void ProcessSkillList(Client client, ToServer.SkillList message)
        {
            // TODO: umg this seems a bit primitive, but I guess it works so :/
            ArrayList skillIDs = new ArrayList();
            ArrayList competancy = new ArrayList();
            for (int i = 1; i < Global.modelSchema.EnumSkill.Count; i++)
            {
                Schema.MobileHasSkillRow mhs = Global.modelSchema.MobileHasSkill.FindByTemplateObjectIDEnumSkillID(client.Avatar.TemplateObjectID, i);
                if (mhs != null)
                {
                    skillIDs.Add(mhs.EnumSkillID);
                    competancy.Add(mhs.Rating);
                }
            }
            client.Send(new ToClient.SkillList((int[])skillIDs.ToArray(typeof(int)), (float[])competancy.ToArray(typeof(float))));
        }
    }
}
