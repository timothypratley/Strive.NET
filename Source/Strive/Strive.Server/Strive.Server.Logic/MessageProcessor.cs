using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Media.Media3D;
using Common.Logging;
using Strive.Data.Events;
using Strive.Model;
using Strive.Network.Messages;
using Strive.Network.Messages.ToServer;
using Strive.Network.Messaging;
using Strive.Server.DB;


namespace Strive.Server.Logic
{
    public partial class MessageProcessor
    {
        public World World { get; private set; }
        public Listener Listener { get; private set; }

        readonly ILog _log = LogManager.GetCurrentClassLogger();

        public MessageProcessor(World world, Listener listener)
        {
            World = world;
            Listener = listener;
        }

        public void ProcessMessages()
        {
            lock (Listener)
            {
                foreach (ClientConnection client in Listener.Clients.Where(c => c.MessageCount > 0))
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
            //TODO: why don't these work?
            //Contract.Requires<ArgumentNullException>(client != null);
            //Contract.Requires<ArgumentNullException>(message != null);

            // new connection... only allow login);
            if (!client.Authenticated && !(message is Login))
            {
                _log.Warn("Non-login message " + message.GetType() + " from " + client.RemoteEndPoint);
                return;
            }

            if (client.Avatar == null
                && !(message is PossessMobile
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
                _log.Trace("Processed message " + message);
            }
            catch
            {
                _log.Warn("ERROR: Unable to process message " + message);
            }
        }

        void ProcessMessage(ClientConnection client, Login loginMessage)
        {
            if (World.UserLookup(loginMessage.Username, loginMessage.Password))
            {
                // login succeeded, check there is not an existing connection for this player
                ClientConnection current;
                if (World.Users.TryGetValue(loginMessage.Username, out current))
                    current.Close();

                _log.Info("User " + loginMessage.Username + " logged in");
                client.AuthenticatedUsername = loginMessage.Username;
                World.Users.Add(loginMessage.Username, client);
            }
            else
            {
                _log.Info("Login failed for username '" + loginMessage.Username + "'");
                client.Close();
            }
        }

        public Tuple<int, string>[] GetPossessable(string username)
        {
            DataRow[] dr = Global.Schema.Player.Select("Email = '" + username + "'");
            Schema.PlayerRow pr = Global.Schema.Player.FindByPlayerID((int)dr[0][0]);
            Schema.MobilePossesableByPlayerRow[] mpbpr = pr.GetMobilePossesableByPlayerRows();
            return mpbpr.Select(mpr => new Tuple<int, string>(
                mpr.ObjectInstanceID, mpr.ObjectInstanceRow.TemplateObjectRow.TemplateObjectName)).ToArray();
        }

        void ProcessMessage(ClientConnection client, RequestPossessable message)
        {
            //Strive.Data.MultiverseFactory.refreshMultiverseForPlayer(Global.modelSchema, client.PlayerID);
            client.CanPossess(GetPossessable(client.AuthenticatedUsername));
        }

        void ProcessMessage(ClientConnection client, Logout message)
        {
            if (client.Avatar != null)
            {
                // remove from world.
                // TODO: do I want to remove the avatar?
                // World.Remove((EntityModel)client.Avatar);
            }
            _log.Info("Logged out '" + client.AuthenticatedUsername + "'.");
            client.Close();
        }

        void ProcessMessage(ClientConnection client, PossessMobile message)
        {
            var e = World.History.Head.Entity.TryFind(message.InstanceId);
            if (e != null)
            {
                // reconnected, replace existing connection with the new
                var avatar = e.Value;
                ClientConnection possessedBy;
                if (World.Possession.TryGetValue(avatar.Id, out possessedBy))
                {
                    if (possessedBy == client)
                    {
                        client.LogMessage("You already possess " + avatar);
                        return;
                    }
                    else
                    {
                        _log.Info("Mobile " + avatar + " has been taken over by " + client);
                        possessedBy.LogMessage("You lost control of " + avatar);
                        possessedBy.Avatar = null;
                    }
                }
                if (client.Avatar != null && client.Avatar.Id != avatar.Id)
                {
                    // TODO: omg releasing this sends a combatant which crashes
                    // release control of previous avatar
                    //World.Possession.Remove(client.Avatar.Id);
                }
                World.Possession[avatar.Id] = client;
                client.Avatar = avatar;
                client.LogMessage("You are now controlling " + avatar);
            }
            else
            {
                // try to load the character
                var avatar = World.LoadMobile(message.InstanceId);
                if (avatar == null)
                {
                    _log.Warn("Character " + message.InstanceId + " not found.");
                    //TODO: rely on world loading
                    //client.Close();
                    //return;
                    avatar = new CombatantModel(
                        Global.Rand.Next(), client.AuthenticatedUsername, "RTSRobot",
                        new Vector3D(), Quaternion.Identity, 100, 100, Common.EnumMobileState.Standing, 1.7f,
                        20, 20, 20, 20, 20);
                }

                World.Possession[avatar.Id] = client;
                client.Avatar = avatar;

                // try to add the character to the world
                World.Apply(new EntityUpdateEvent(avatar, "Loaded for possession"));
            }
            World.SendInitialWorldView(client);
        }

        void ProcessMessage(ClientConnection client, MyPosition message)
        {
            var avatar = client.Avatar;
            if (avatar == null)
            {
                client.LogMessage("Position message sent, but have no avatar.");
                return;
            }

            if (message.Position != avatar.Position)
                World.Apply(new EntityUpdateEvent(
                    avatar.Move(message.MobileState, message.Position, message.Rotation, Global.Now),
                    "Player movement"));
        }

        void ProcessMessage(ClientConnection client, Communicate message)
        {
            // TODO: chat as character or user?
            string name = client.Avatar == null ? client.AuthenticatedUsername : client.Avatar.Name;

            if (message.CommunicationType == CommunicationType.Chat)
                World.SendToUsers(new Network.Messages.ToClient.Communication(
                    name, message.Message, message.CommunicationType));
            else if (message.CommunicationType == CommunicationType.PartyTalk)
                World.SendPartyTalk(client, message.Message);
            else
                _log.Error("Unexpected CommunicationType " + message.CommunicationType);
        }

        void ProcessMessage(ClientConnection client, ReloadWorld message)
        {
            _log.Info("ReloadWorld received.");
            World.Load();
            foreach (ClientConnection c in Listener.Clients
                .Where(c => c.Avatar != null
                    && c.Status == ConnectionStatus.Connected))
            {
                // re-spawn their mobile
                c.DropAll();
                ProcessMessage(c, new PossessMobile(c.Avatar.Id));
            }
        }

        void ProcessMessage(ClientConnection client, RequestWhoList message)
        {
            client.WhoList(
                Listener.Clients
                .Select(c => c.Avatar)
                .Except(null)
                .Select(a => new Tuple<int, string>(a.Id, a.Name))
                .ToArray());
        }

        void ProcessMessage(ClientConnection client, TransferPartyLeadership message)
        {
            var avatar = client.Avatar;
            ClientConnection target;
            if (!World.Users.TryGetValue(message.Leader, out target))
            {
                client.LogMessage("Invalid target");
                return;
            }

            if (client.AuthenticatedUsername == message.Leader)
            {
                client.LogMessage("You are already the leader");
                return;
            }

            HashSet<string> party;
            if (!World.Party.TryGetValue(client.AuthenticatedUsername, out party))
            {
                client.LogMessage("You are not the leader of a party");
                return;
            }

            // TODO: use character names instead of username??
            if (!party.Contains(message.Leader))
            {
                client.LogMessage(message.Leader + " is not in your party");
                return;
            }

            World.TransferLeadership(client.AuthenticatedUsername, message.Leader);
            World.SendPartyTalk(client, "Party leadership has been transferred to " + message.Leader);
        }


        void ProcessMessage(ClientConnection client, Pong message)
        {
            client.Latency = (DateTime.Now - client.PingedAt).Milliseconds;
            World.Weather.Latency = client.Latency;
            client.Send(World.Weather);
        }

        void ProcessMessage(ClientConnection client, LeaveParty message)
        {
            World.LeaveParty(client, client.AuthenticatedUsername);
            client.LogMessage("You have left party");
        }

        void ProcessMessage(ClientConnection client, JoinParty message)
        {
            Dictionary<string, DateTime> invitation;
            if (!World.InvitedToParty.TryGetValue(client.AuthenticatedUsername, out invitation))
            {
                client.LogMessage("You are not currently invited to join a party");
                return;
            }

            // make sure they are trying to join the party they were invited to
            if (!invitation.ContainsKey(message.Leader))
            {
                client.LogMessage("You are not currently invited to join " + message.Leader);
                return;
            }

            HashSet<string> party;
            if (!World.Party.TryGetValue(message.Leader, out party))
            {
                client.LogMessage("That party does not exist anymore");
                return;
            }

            World.SendPartyTalk(client, message.Leader, client.Avatar.Name + " has joined your ");
            party.Add(client.AuthenticatedUsername);
            client.LogMessage("You are now in party '" + message.Leader + "'.");

            //World.InvitedToParty.Remove(message.Leader);
        }

        void ProcessMessage(ClientConnection client, InviteToParty message)
        {
            if (World.Party[client.AuthenticatedUsername] == null)
            {
                client.LogMessage("You are not the leader of a party");
                return;
            }
            var target = World.Users[message.User];
            if (target == null)
                client.LogMessage("Invalid target");
            else
            {
                World.InvitedToParty[target.AuthenticatedUsername]
                    .Add(client.AuthenticatedUsername, Global.Now);
                // TODO: probably want a graphical thing or something for invites
                target.LogMessage("You have been invited to party '" + client.AuthenticatedUsername + "'");
            }
        }

        void ProcessMessage(ClientConnection client, RequestSkillList message)
        {
            client.SkillList(
                Global.Schema.EnumSkill
                    .Select(e => Global.Schema.MobileHasSkill.FindByTemplateObjectIDEnumSkillID(
                        client.Avatar.Id, (int)e.EnumSkillID))
                    .Where(mhs => mhs != null)
                    .Select(mhs => new Tuple<int, double>(mhs.EnumSkillID, mhs.Rating))
                    .ToArray());
        }

        void ProcessMessage(ClientConnection client, CreateEntity c)
        {
            // TODO: would like to at least set the id
            // ObjectInstanceId = Global.Rand.Next(),
            World.Apply(new EntityUpdateEvent(c.Entity, "Created by " + client.AuthenticatedUsername));
        }

        void ProcessMessage(ClientConnection client, CreatePlan c)
        {
            // TODO: would like to at least set the id
            // ObjectInstanceId = Global.Rand.Next(),
            World.Apply(new PlanUpdateEvent(c.Plan, "Created by " + client.AuthenticatedUsername));
        }

        void ProcessMessage(ClientConnection client, ProduceEntity p)
        {
            var factory = World.History.Head.Entity.TryFind(p.FactoryId);
            if (factory == null)
            {
                client.LogMessage("Could not find factory " + p.FactoryId);
                return;
            }

            World.Apply(new ProductionStartedEvent(
                p.FactoryId, p.Id,
                client.AuthenticatedUsername + " producing " + p.Name + " from " + factory));
        }

        void ProcessMessage(ClientConnection client, UseSkill message)
        {
            World.ProcessUseSkill(client, message);
        }

        void ProcessMessage(ClientConnection client, CancelSkill message)
        {
            World.ProcessCancelSkill(client, message);
        }
    }
}
