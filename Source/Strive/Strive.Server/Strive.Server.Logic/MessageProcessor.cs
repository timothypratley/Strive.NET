using System;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using System.Linq;

using Common.Logging;
using Strive.Network.Messages;
using Strive.Network.Messages.ToServer;
using Strive.Network.Server;
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
                CheckAndProcessMessage(clientMessage.Client, clientMessage.Message);
            }
            catch (Exception e)
            {
                Log.Error("Message was not processed correctly, resuming.", e);
            }
        }

        public void CheckAndProcessMessage(Client client, dynamic message)
        {
            if (!client.Authenticated)
            {
                // new connection... only allow login
                if (!(message is Login))
                {
                    Log.Warn("Non-login message " + message.GetType() + " from " + client.RemoteEndPoint);
                }
                return;
            }

            if (client.Avatar == null)
            {
                // no character selected yet... only allow posses, request and logout
                if (!(message is EnterWorldAsMobile
                    || message is RequestPossessable
                    || message is Logout))
                {
                    Log.Warn("ERROR: Non-posses message " + message.GetType() + " from " + client.RemoteEndPoint);
                }
                return;
            }

            try
            {
                ProcessMessage(client, message);
            }
            catch (Exception)
            {
                Log.Warn("ERROR: Unable to process message " + message);
            }
        }

        void ProcessMessage(Client client, Login loginMessage)
        {
            if (world.UserLookup(loginMessage.username, loginMessage.password, ref client.PlayerID))
            {
                // login succeeded, check there isnt an existing connection for this player
                foreach (Client c in listener.Clients)
                {
                    if (c.AuthenticatedUsername == loginMessage.username)
                        c.Close();
                }

                Log.Info("User " + loginMessage.username + " logged in");
                client.AuthenticatedUsername = loginMessage.username;
            }
            else
            {
                Log.Info("Login failed for username " + loginMessage.username + " " + loginMessage.password);
                client.Close();
            }
        }

        void ProcessMessage(Client client, RequestPossessable message)
        {
            //Strive.Data.MultiverseFactory.refreshMultiverseForPlayer(Global.modelSchema, client.PlayerID);
            client.CanPossess(
                world.getPossessable(client.AuthenticatedUsername));
        }

        void ProcessMessage(Client client, Logout message)
        {
            if (client.Avatar != null)
            {
                // remove from world.
                world.Remove(client.Avatar);
            }
            Log.Info("Logged out '" + client.AuthenticatedUsername + "'.");
            client.Close();
        }



        void ProcessMessage(Client client, EnterWorldAsMobile message)
        {
            MobileAvatar a;
            if (world.PhysicalObjects.ContainsKey(message.InstanceID))
            {
                // reconnected
                // simply replace existing connection with the new
                // todo: the old 'connection' should timeout or die or be killed
                Object o = world.PhysicalObjects[message.InstanceID];
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

        void ProcessMessage(Client client, Position message)
        {
            if (client.Avatar == null)
            {
                Log.Warn("Position message from client " + client.RemoteEndPoint + " who has no avatar.");
                return;
            }
            MobileAvatar ma = (MobileAvatar)client.Avatar;

            // don't go to running for pure heading changes
            if (message.position != client.Avatar.Position)
            {
                ma.lastMoveUpdate = Global.Now;
                if (ma.MobileState != EnumMobileState.Running)
                {
                    ma.SetMobileState(EnumMobileState.Running);
                }
            }
            world.Relocate(client.Avatar, message.position, message.rotation);
        }

        void ProcessMessage(Client client, Communication message)
        {
            if (message.communicationType == CommunicationType.Chat)
            {
                world.NotifyMobiles(new Strive.Network.Messages.ToClient.Communication(
                    client.Avatar.TemplateObjectName, message.message,
                    (Strive.Network.Messages.CommunicationType)message.communicationType));
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

        void ProcessMessage(Client client, ReloadWorld message)
        {
            Log.Info("ReloadWorld received.");
            world.Load();
            foreach (Client c in listener.Clients)
            {
                if (c.Avatar != null)
                {
                    // respawn their mobile, old instance will be given over
                    // to Garbage Collector
                    c.DropAll();
                    ProcessMessage(c, new EnterWorldAsMobile(c.Avatar.ObjectInstanceID));
                }
            }
        }

        void ProcessMessage(Client client, WhoList message)
        {
            List<int> MobileIDs = new List<int>();
            List<string> Names = new List<string>();
            client.WhoList(
                listener.Clients
                .Where(c => c.Avatar != null)
                .Select(c => new Tuple<int, string>(c.Avatar.ObjectInstanceID, c.Avatar.TemplateObjectName))
                .ToArray());
        }

        void ProcessMessage(Client client, CreateParty message)
        {
            MobileAvatar ma = (MobileAvatar)client.Avatar;
            if (ma.party != null)
            {
                ma.SendLog("You are currently in party '" + ma.party.Name + "'.");
                return;
            }
            ma.party = new Party(message.name, ma);
        }

        void ProcessMessage(Client client, TransferPartyLeadership message)
        {
            MobileAvatar ma = (MobileAvatar)client.Avatar;
            MobileAvatar target = (MobileAvatar)Global.World.PhysicalObjects[message.ObjectInstanceID];
            if (ma.party != target.party)
            {
                ma.SendLog("You are not in the same party as " + target.TemplateObjectName);
                return;
            }
            ma.party.Leader = target;
            ma.SendPartyTalk("Party leadership has been transfered to " + target.TemplateObjectName);
        }

        void ProcessMessage(Client client, Pong message)
        {
            client.latency = (DateTime.Now - client.pingedAt).Milliseconds;
            world.weather.Latency = client.latency;
            client.Send(world.weather);
        }

        void ProcessMessage(Client client, LeaveParty message)
        {
            MobileAvatar ma = (MobileAvatar)client.Avatar;
            Party p = ma.party;
            p.Remove(ma.ObjectInstanceID);
            ma.SendLog("You have left party '" + p.Name + "'.");
            ma.party = null;
            p.SendPartyTalk(ma.TemplateObjectName + " has left your ");
        }

        void ProcessMessage(Client client, JoinParty message)
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

        void ProcessMessage(Client client, InviteToParty message)
        {
            Party p = ((MobileAvatar)client.Avatar).party;
            if (p == null)
            {
                client.Log("You are not in a ");
                return;
            }
            if (p.Leader != client.Avatar)
            {
                client.Log("You are not the party leader.");
                return;
            }
            MobileAvatar target = (MobileAvatar)Global.World.PhysicalObjects[message.ObjectInstanceID];
            target.invitedToParty = p;
            // TODO: probabbly want a graphical thing or something for invites
            target.SendLog("You have been invited to party '" + p.Name + "'.");
        }

        void ProcessMessage(Client client, SkillList message)
        {
            client.SkillList(
                Global.ModelSchema.EnumSkill
                .Select(
                    e => Global.ModelSchema.MobileHasSkill.FindByTemplateObjectIDEnumSkillID(
                        client.Avatar.TemplateObjectID, e.EnumSkillID))
                .Where(mhs => mhs != null)
                .Select(mhs => new Tuple<int, double>(mhs.EnumSkillID, mhs.Rating)).ToArray());
        }
    }
}
