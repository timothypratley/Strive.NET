using System;
using System.Linq;
using System.Windows.Media.Media3D;
using Common.Logging;
using Strive.Model;
using Strive.Network.Messages;
using Strive.Network.Messages.ToServer;
using Strive.Network.Messaging;


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
            if (World.UserLookup(loginMessage.Username, loginMessage.Password, ref client.PlayerId))
            {
                // login succeeded, check there is not an existing connection for this player
                foreach (ClientConnection c in Listener.Clients
                    .Where(c => c != client && c.AuthenticatedUsername == loginMessage.Username))
                    c.Close();

                _log.Info("User " + loginMessage.Username + " logged in");
                client.AuthenticatedUsername = loginMessage.Username;
            }
            else
            {
                _log.Info("Login failed for username '" + loginMessage.Username + "'");
                client.Close();
            }
        }

        void ProcessMessage(ClientConnection client, RequestPossessable message)
        {
            //Strive.Data.MultiverseFactory.refreshMultiverseForPlayer(Global.modelSchema, client.PlayerID);
            client.CanPossess(World.GetPossessable(client.AuthenticatedUsername));
        }

        void ProcessMessage(ClientConnection client, Logout message)
        {
            if (client.Avatar != null)
            {
                // remove from world.
                World.Remove(client.Avatar);
            }
            _log.Info("Logged out '" + client.AuthenticatedUsername + "'.");
            client.Close();
        }

        void ProcessMessage(ClientConnection client, PossessMobile message)
        {
            if (World.PhysicalObjects.ContainsKey(message.InstanceId))
            {
                // reconnected
                // simply replace existing connection with the new
                // TODO: the old 'connection' should timeout or die or be killed
                var avatar = World.PhysicalObjects[message.InstanceId] as Avatar;
                if (avatar == null)
                {
                    client.LogMessage("Can only possess mobiles.");
                    return;
                }
                if (avatar.Client == client)
                {
                    client.LogMessage("You already possess " + avatar);
                    return;
                }

                if (avatar.Client != null)
                {
                    _log.Info("Mobile " + avatar + " has been taken over by " + client);
                    avatar.Client.LogMessage("You lost control of " + avatar);
                    avatar.Client.Avatar = null;
                }
                avatar.Client = client;
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
                    avatar = new Avatar(World,
                        Global.Rand.Next(), client.AuthenticatedUsername, "RTSRobot",
                        new Vector3D(0, 0, 0), Quaternion.Identity, 100, Common.EnumMobileState.Standing, 1.7f,
                        20, 20, 20, 20);
                }

                avatar.Client = client;
                client.Avatar = avatar;

                // try to add the character to the world
                World.Add(avatar);
            }
            World.SendInitialWorldView(client);
        }

        void ProcessMessage(ClientConnection client, MyPosition message)
        {
            if (client.Avatar == null)
            {
                _log.Warn("Position message from client " + client.RemoteEndPoint + " who has no avatar.");
                return;
            }
            var ma = (Avatar)client.Avatar;

            if (message.Position != client.Avatar.Position)
                ma.LastMoveUpdate = Global.Now;
            ma.SetMobileState(message.State);

            World.Relocate(client.Avatar, message.Position, message.Rotation);
            World.Move();
        }

        void ProcessMessage(ClientConnection client, Communicate message)
        {
            if (message.CommunicationType == CommunicationType.Chat)
            {
                World.NotifyMobiles(new Network.Messages.ToClient.Communication(
                    client.Avatar.TemplateObjectName, message.Message,
                    message.CommunicationType));
            }
            else if (message.CommunicationType == CommunicationType.PartyTalk)
            {
                var ma = (Avatar)client.Avatar;
                ma.SendPartyTalk(message.Message);
            }
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
                ProcessMessage(c, new PossessMobile(c.Avatar.ObjectInstanceId));
            }
        }

        void ProcessMessage(ClientConnection client, RequestWhoList message)
        {
            client.WhoList(
                Listener.Clients
                .Where(c => c.Avatar != null)
                .Select(c => new Tuple<int, string>(c.Avatar.ObjectInstanceId, c.Avatar.TemplateObjectName))
                .ToArray());
        }

        void ProcessMessage(ClientConnection client, CreateParty message)
        {
            var ma = (Avatar)client.Avatar;
            if (ma.Party != null)
            {
                ma.SendLog("You are currently in party '" + ma.Party.Name + "'.");
                return;
            }

            ma.Party = new Party(message.Name, ma);
        }

        void ProcessMessage(ClientConnection client, TransferPartyLeadership message)
        {
            var ma = (Avatar)client.Avatar;
            var target = World.PhysicalObjects[message.ObjectInstanceId] as Avatar;
            if (target == null)
                ma.SendLog("Invalid target");
            else if (ma.Party != target.Party)
                ma.SendLog("You are not in the same party as " + target.Name);
            else
            {
                ma.Party.Leader = target;
                ma.SendPartyTalk("Party leadership has been transferred to " + target.Name);
            }
        }


        void ProcessMessage(ClientConnection client, Pong message)
        {
            client.Latency = (DateTime.Now - client.PingedAt).Milliseconds;
            World.Weather.Latency = client.Latency;
            client.Send(World.Weather);
        }

        void ProcessMessage(ClientConnection client, LeaveParty message)
        {
            var ma = (Avatar)client.Avatar;
            Party p = ma.Party;
            if (p != null)
            {
                p.Remove(ma.Id);
                ma.SendLog("You have left party '" + p.Name + "'");
                ma.Party = null;
                p.SendPartyTalk(ma.Name + " has left your party");
            }
        }

        void ProcessMessage(ClientConnection client, JoinParty message)
        {
            var ma = (Avatar)client.Avatar;

            // make sure they are trying to join the party they were invited to
            if (message.LeaderId != ma.InvitedToParty.Leader.Id)
            {
                ma.SendLog("Join party failed, get a new invitation.");
                return;
            }

            ma.InvitedToParty.SendPartyTalk(ma.Name + " has joined your ");
            ma.Party = ma.InvitedToParty;
            ma.Party.Add(ma);
            ma.SendLog("You are now in party '" + ma.Party.Name + "'.");
            ma.InvitedToParty = null;
        }

        void ProcessMessage(ClientConnection client, InviteToParty message)
        {
            Party p = ((Avatar)client.Avatar).Party;
            if (p == null)
            {
                client.LogMessage("You are not in a party");
                return;
            }
            if (p.Leader != client.Avatar)
            {
                client.LogMessage("You are not the party leader");
                return;
            }
            var target = World.PhysicalObjects[message.ObjectInstanceId] as Avatar;
            if (target == null)
                client.LogMessage("Invalid target");
            else
            {
                target.InvitedToParty = p;
                // TODO: probably want a graphical thing or something for invites
                target.SendLog("You have been invited to party '" + p.Name + "'");
            }
        }

        void ProcessMessage(ClientConnection client, RequestSkillList message)
        {
            client.SkillList(
                Global.ModelSchema.EnumSkill
                    .Select(e => Global.ModelSchema.MobileHasSkill.FindByTemplateObjectIDEnumSkillID(
                        client.Avatar.TemplateObjectId, e.EnumSkillID))
                    .Where(mhs => mhs != null)
                    .Select(mhs => new Tuple<int, double>(mhs.EnumSkillID, mhs.Rating))
                    .ToArray());
        }

        void ProcessMessage(ClientConnection client, EntityModel e)
        {
            // TODO: would like to at least set the id
            // ObjectInstanceId = Global.Rand.Next(),
            World.Add(e);
        }

        void ProcessMessage(ClientConnection client, TaskModel t)
        {
            // TODO: would like to at least set the id
            // ObjectInstanceId = Global.Rand.Next(),
            World.WorldModel.Add(t);
        }

        void ProcessMessage(ClientConnection client, UseSkill message)
        {
            SkillCommandProcessor.ProcessUseSkill(World, client, message);
        }

        void ProcessMessage(ClientConnection client, CancelSkill message)
        {
            SkillCommandProcessor.ProcessCancelSkill(World, client, message);
        }
    }
}
