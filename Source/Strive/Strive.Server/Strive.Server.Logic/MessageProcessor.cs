using System;
using System.Linq;

using Common.Logging;
using Strive.Network.Messages;
using Strive.Network.Messages.ToServer;
using Strive.Network.Messaging;
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

        public void ProcessMessages()
        {
            lock (_listener.Clients)
            {
                foreach (ClientConnection client in _listener.Clients.Where(c => c.MessageCount > 0))
                {
                    try
                    {
                        CheckAndProcessMessage(client, client.PopNextMessage());
                    }
                    catch (Exception e)
                    {
                        _log.Error("Message was not processed correctly, resuming.", e);
                    }
                }
            }
        }

        public void CheckAndProcessMessage(ClientConnection client, dynamic message)
        {
            // new connection... only allow login
            if (!client.Authenticated && !(message is Login))
            {
                _log.Warn("Non-login message " + message.GetType() + " from " + client.RemoteEndPoint);
                return;
            }

            if (client.Avatar == null
                && !(message is EnterWorldAsMobile
                     || message is RequestPossessable
                     || message is Logout
                     || message is Login))
            {
                _log.Warn("ERROR: Non-posses message " + message.GetType() + " from " + client.RemoteEndPoint);
                return;
            }

            try
            {
                ProcessMessage(client, message);
                _log.Info("Processed message " + message);
            }
            catch (Exception)
            {
                _log.Warn("ERROR: Unable to process message " + message);
            }
        }

        void ProcessMessage(ClientConnection client, Login loginMessage)
        {
            if (_world.UserLookup(loginMessage.Username, loginMessage.Password, ref client.PlayerId))
            {
                // login succeeded, check there isnt an existing connection for this player
                foreach (ClientConnection c in _listener.Clients)
                {
                    if (c.AuthenticatedUsername == loginMessage.Username)
                        c.Close();
                }

                _log.Info("User " + loginMessage.Username + " logged in");
                client.AuthenticatedUsername = loginMessage.Username;
            }
            else
            {
                _log.Info("Login failed for username " + loginMessage.Username + " " + loginMessage.Password);
                client.Close();
            }
        }

        void ProcessMessage(ClientConnection client, RequestPossessable message)
        {
            //Strive.Data.MultiverseFactory.refreshMultiverseForPlayer(Global.modelSchema, client.PlayerID);
            client.CanPossess(_world.GetPossessable(client.AuthenticatedUsername));
        }

        void ProcessMessage(ClientConnection client, Logout message)
        {
            if (client.Avatar != null)
            {
                // remove from world.
                _world.Remove(client.Avatar);
            }
            _log.Info("Logged out '" + client.AuthenticatedUsername + "'.");
            client.Close();
        }



        void ProcessMessage(ClientConnection client, EnterWorldAsMobile message)
        {
            MobileAvatar a;
            if (_world.PhysicalObjects.ContainsKey(message.InstanceId))
            {
                // reconnected
                // simply replace existing connection with the new
                // todo: the old 'connection' should timeout or die or be killed
                Object o = _world.PhysicalObjects[message.InstanceId];
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
                a = _world.LoadMobile(message.InstanceId);
                if (a == null)
                {
                    _log.Warn("Character " + message.InstanceId + " not found.");
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

        void ProcessMessage(ClientConnection client, MyPosition message)
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

        void ProcessMessage(ClientConnection client, Communicate message)
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

        void ProcessMessage(ClientConnection client, ReloadWorld message)
        {
            _log.Info("ReloadWorld received.");
            _world.Load();
            foreach (ClientConnection c in _listener.Clients
                .Where(c => c.Avatar != null
                    && c.Status == ConnectionStatus.Connected))
            {
                // respawn their mobile, old instance will be given over
                // to Garbage Collector
                c.DropAll();
                ProcessMessage(c, new EnterWorldAsMobile(c.Avatar.ObjectInstanceID));
            }
        }

        void ProcessMessage(ClientConnection client, RequestWhoList message)
        {
            client.WhoList(
                _listener.Clients
                .Where(c => c.Avatar != null)
                .Select(c => new Tuple<int, string>(c.Avatar.ObjectInstanceID, c.Avatar.TemplateObjectName))
                .ToArray());
        }

        void ProcessMessage(ClientConnection client, CreateParty message)
        {
            var ma = (MobileAvatar)client.Avatar;
            if (ma.Party != null)
            {
                ma.SendLog("You are currently in party '" + ma.Party.Name + "'.");
                return;
            }
            ma.Party = new Party(message.Name, ma);
        }

        void ProcessMessage(ClientConnection client, TransferPartyLeadership message)
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
    

        void ProcessMessage(ClientConnection client, Pong message)
        {
            client.Latency = (DateTime.Now - client.PingedAt).Milliseconds;
            _world.Weather.Latency = client.Latency;
            client.Send(_world.Weather);
        }

        void ProcessMessage(ClientConnection client, LeaveParty message)
        {
            var ma = (MobileAvatar)client.Avatar;
            Party p = ma.Party;
            p.Remove(ma.ObjectInstanceID);
            ma.SendLog("You have left party '" + p.Name + "'.");
            ma.Party = null;
            p.SendPartyTalk(ma.TemplateObjectName + " has left your ");
        }

        void ProcessMessage(ClientConnection client, JoinParty message)
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

        void ProcessMessage(ClientConnection client, InviteToParty message)
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

        void ProcessMessage(ClientConnection client, RequestSkillList message)
        {
            client.SkillList(
                Global.ModelSchema.EnumSkill
                    .Select(e => Global.ModelSchema.MobileHasSkill.FindByTemplateObjectIDEnumSkillID(
                        client.Avatar.TemplateObjectID, e.EnumSkillID))
                    .Where(mhs => mhs != null)
                    .Select(mhs => new Tuple<int, double>(mhs.EnumSkillID, mhs.Rating))
                    .ToArray());
        }
    }
}
