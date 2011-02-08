using System;
using System.Linq;

using Common.Logging;
using Strive.Network.Messages;
using Strive.Network.Messages.ToServer;
using Strive.Network.Server;
using Strive.Common;


namespace Strive.Server.Logic
{
    public class MessageProcessor
    {
        readonly World _world;
        readonly Listener _listener;
        readonly ILog _log = LogManager.GetCurrentClassLogger();

        public MessageProcessor(World world, Listener listener)
        {
            _world = world;
            _listener = listener;
        }

        public void ProcessNextMessage()
        {
            ClientMessage clientMessage = _listener.PopNextMessage();
            try
            {
                CheckAndProcessMessage(clientMessage.Client, clientMessage.Message);
            }
            catch (Exception e)
            {
                _log.Error("Message was not processed correctly, resuming.", e);
            }
        }

        public void CheckAndProcessMessage(Client client, dynamic message)
        {
            if (!client.Authenticated)
            {
                // new connection... only allow login
                if (!(message is Login))
                {
                    _log.Warn("Non-login message " + message.GetType() + " from " + client.RemoteEndPoint);
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
                    _log.Warn("ERROR: Non-posses message " + message.GetType() + " from " + client.RemoteEndPoint);
                }
                return;
            }

            try
            {
                ProcessMessage(client, message);
            }
            catch (Exception)
            {
                _log.Warn("ERROR: Unable to process message " + message);
            }
        }

        void ProcessMessage(Client client, Login loginMessage)
        {
            if (_world.UserLookup(loginMessage.username, loginMessage.password, ref client.PlayerId))
            {
                // login succeeded, check there isnt an existing connection for this player
                foreach (Client c in _listener.Clients)
                {
                    if (c.AuthenticatedUsername == loginMessage.username)
                        c.Close();
                }

                _log.Info("User " + loginMessage.username + " logged in");
                client.AuthenticatedUsername = loginMessage.username;
            }
            else
            {
                _log.Info("Login failed for username " + loginMessage.username + " " + loginMessage.password);
                client.Close();
            }
        }

        void ProcessMessage(Client client, RequestPossessable message)
        {
            //Strive.Data.MultiverseFactory.refreshMultiverseForPlayer(Global.modelSchema, client.PlayerID);
            client.CanPossess(_world.GetPossessable(client.AuthenticatedUsername));
        }

        void ProcessMessage(Client client, Logout message)
        {
            if (client.Avatar != null)
            {
                // remove from world.
                _world.Remove(client.Avatar);
            }
            _log.Info("Logged out '" + client.AuthenticatedUsername + "'.");
            client.Close();
        }



        void ProcessMessage(Client client, EnterWorldAsMobile message)
        {
            MobileAvatar a;
            if (_world.PhysicalObjects.ContainsKey(message.InstanceID))
            {
                // reconnected
                // simply replace existing connection with the new
                // todo: the old 'connection' should timeout or die or be killed
                Object o = _world.PhysicalObjects[message.InstanceID];
                if (o is MobileAvatar)
                {
                    a = (MobileAvatar)o;
                }
                else
                {
                    _log.Warn("Can only possess mobiles.");
                    return;
                }
                if (a.Client == client)
                {
                    _log.Warn("A client " + client.ToString() + " attempted to take control of the same mobile " + a.ObjectInstanceID + " twice... ignoring request.");
                }
                else if (a.Client != null)
                {
                    _log.Info("Mobile " + a.ObjectInstanceID + " has been taken over by a new connection.");
                    a.Client.Avatar = null;
                    a.Client.Close();
                    a.Client = client;
                    client.Avatar = a;
                }
                else
                {
                    a.Client = client;
                    client.Avatar = a;
                }
            }
            else
            {
                // try to load the character
                a = _world.LoadMobile(message.InstanceID);
                if (a == null)
                {
                    _log.Warn("Character " + message.InstanceID + " not found.");
                    client.Close();
                    return;
                }
                a.Client = client;
                client.Avatar = a;

                // try to add the character to the world
                _world.Add(client.Avatar);
            }
            _world.SendInitialWorldView(client);
        }

        void ProcessMessage(Client client, Position message)
        {
            if (client.Avatar == null)
            {
                _log.Warn("Position message from client " + client.RemoteEndPoint + " who has no avatar.");
                return;
            }
            var ma = (MobileAvatar)client.Avatar;

            // don't go to running for pure heading changes
            if (message.position != client.Avatar.Position)
            {
                ma.LastMoveUpdate = Global.Now;
                if (ma.MobileState != EnumMobileState.Running)
                {
                    ma.SetMobileState(EnumMobileState.Running);
                }
            }
            _world.Relocate(client.Avatar, message.position, message.rotation);
        }

        void ProcessMessage(Client client, Communication message)
        {
            if (message.CommunicationType == CommunicationType.Chat)
            {
                _world.NotifyMobiles(new Network.Messages.ToClient.Communication(
                    client.Avatar.TemplateObjectName, message.Message,
                    message.CommunicationType));
            }
            else if (message.CommunicationType == CommunicationType.PartyTalk)
            {
                var ma = (MobileAvatar)client.Avatar;
                ma.SendPartyTalk(message.Message);
            }
            else
            {
                _log.Error("Unexpected CommunicationType " + message.CommunicationType);
            }
            //Log.Info( "Sent communication message" );
        }

        void ProcessMessage(Client client, ReloadWorld message)
        {
            _log.Info("ReloadWorld received.");
            _world.Load();
            foreach (Client c in _listener.Clients.Where(c => c.Avatar != null))
            {
                // respawn their mobile, old instance will be given over
                // to Garbage Collector
                c.DropAll();
                ProcessMessage(c, new EnterWorldAsMobile(c.Avatar.ObjectInstanceID));
            }
        }

        void ProcessMessage(Client client, WhoList message)
        {
            client.WhoList(
                _listener.Clients
                .Where(c => c.Avatar != null)
                .Select(c => new Tuple<int, string>(c.Avatar.ObjectInstanceID, c.Avatar.TemplateObjectName))
                .ToArray());
        }

        void ProcessMessage(Client client, CreateParty message)
        {
            var ma = (MobileAvatar)client.Avatar;
            if (ma.Party != null)
            {
                ma.SendLog("You are currently in party '" + ma.Party.Name + "'.");
                return;
            }
            ma.Party = new Party(message.Name, ma);
        }

        void ProcessMessage(Client client, TransferPartyLeadership message)
        {
            var ma = (MobileAvatar) client.Avatar;
            var target = Global.World.PhysicalObjects[message.ObjectInstanceId] as MobileAvatar;
            if (target == null)
            {
                ma.SendLog("Invalid target");
            }
            else if (ma.Party != target.Party)
            {
                ma.SendLog("You are not in the same party as " + target.TemplateObjectName);
            }
            else
            {
                ma.Party.Leader = target;
                ma.SendPartyTalk("Party leadership has been transfered to " + target.TemplateObjectName);
            }
        }
    

        void ProcessMessage(Client client, Pong message)
        {
            client.Latency = (DateTime.Now - client.PingedAt).Milliseconds;
            _world.Weather.Latency = client.Latency;
            client.Send(_world.Weather);
        }

        void ProcessMessage(Client client, LeaveParty message)
        {
            var ma = (MobileAvatar)client.Avatar;
            Party p = ma.Party;
            p.Remove(ma.ObjectInstanceID);
            ma.SendLog("You have left party '" + p.Name + "'.");
            ma.Party = null;
            p.SendPartyTalk(ma.TemplateObjectName + " has left your ");
        }

        void ProcessMessage(Client client, JoinParty message)
        {
            var ma = (MobileAvatar)client.Avatar;

            // make sure they are trying to join the party they were invited to
            if (message.ObjectInstanceId != ma.InvitedToParty.Leader.ObjectInstanceID)
            {
                ma.SendLog("Join party failed, get a new invitation.");
                return;
            }

            ma.InvitedToParty.SendPartyTalk(ma.TemplateObjectName + " has joined your ");
            ma.Party = ma.InvitedToParty;
            ma.Party.Add(ma);
            ma.SendLog("You are now in party '" + ma.Party.Name + "'.");
            ma.InvitedToParty = null;
        }

        void ProcessMessage(Client client, InviteToParty message)
        {
            Party p = ((MobileAvatar) client.Avatar).Party;
            if (p == null)
            {
                client.LogMessage("You are not in a ");
                return;
            }
            if (p.Leader != client.Avatar)
            {
                client.LogMessage("You are not the party leader.");
                return;
            }
            var target = Global.World.PhysicalObjects[message.ObjectInstanceId] as MobileAvatar;
            if (target == null)
            {
                client.LogMessage("Invalid target");
            }
            else
            {
                target.InvitedToParty = p;
                // TODO: probabbly want a graphical thing or something for invites
                target.SendLog("You have been invited to party '" + p.Name + "'.");
            }
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
