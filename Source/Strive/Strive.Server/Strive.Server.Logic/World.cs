using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Common.Logging;
using Strive.Data.Events;
using Strive.Model;
using Strive.Network.Messaging;
using ToClient = Strive.Network.Messages.ToClient;


namespace Strive.Server.Logic
{
    public partial class World
    {
        readonly int _worldId;

        public History History { get; private set; }
        public Listener Listener { get; private set; }
        public ToClient.TimeAndWeather Weather { get; private set; }

        public Dictionary<int, ClientConnection> Possession { get; private set; }
        public Dictionary<string, HashSet<string>> Party { get; private set; }
        public Dictionary<string, Dictionary<string, DateTime>> InvitedToParty { get; private set; }
        public Dictionary<ClientConnection, string> InParty { get; private set; }
        public Dictionary<string, ClientConnection> Users { get; private set; }
        public Dictionary<int, HashSet<ClientConnection>> CubeClients { get; private set; }
        public Dictionary<ClientConnection, HashSet<int>> ClientCubes { get; private set; }

        // TODO: do we know the sun texture etc here?
        const int DefaultDay = 147;
        const int DefaultNight = 148;
        const int DefaultCusp = 5;
        const int DefaultSun = 146;

        readonly ILog _log = LogManager.GetCurrentClassLogger();

        public World(Listener listener, int worldId)
        {
            History = new History();
            CubeClients = new Dictionary<int, HashSet<ClientConnection>>();
            Listener = listener;
            Weather = new ToClient.TimeAndWeather(
                Global.Now, 0, DefaultDay, DefaultNight, DefaultCusp, DefaultSun, 0, 0);
            _worldId = worldId;

            Possession = new Dictionary<int, ClientConnection>();
            Party = new Dictionary<string, HashSet<string>>();
            Users = new Dictionary<string, ClientConnection>();
            Load();
        }

        public void Update()
        {
            foreach (CombatantModel c in History.Head.Entity.OfType<CombatantModel>())
                this.UpdateCombatant(c);
            WeatherUpdate();
        }

        void WeatherUpdate()
        {
            Weather.ServerNow = Global.Now.Ticks;
            if ((Global.Now.Ticks - Weather.ServerNow) < 1)
                return;

            bool weatherChanged = false;
            if (Global.Rand.NextDouble() > 0.995)
            {
                Weather.Fog++;
                weatherChanged = true;
            }
            if (Global.Rand.NextDouble() > 0.995)
            {
                // TODO: only change textures on new days
                //weather.DaySkyTextureID = (weather.SkyTextureID + 1) % 9 + 1;
                //weatherChanged = true;
            }
            if (weatherChanged)
                SendToUsers(Weather);
        }

        public void SendToUsers(object message)
        {
            foreach (var client in Users.Values)
                client.Send(message);
        }

        public void Apply(EntityUpdateEvent e)
        {
            _log.Debug(e.GetType() + e.Description);

            var old = History.Head.Entity.TryFind(e.Entity.Id);
            History.Add(e.Entity);

            ClientConnection client = old == null ? null : client = Possession[old.Value.Id];

            // update client cubes if the entity was possessed
            if (client != null)
            {
                var currentCubes = ClientCubes[client];
                var newCubes = new HashSet<int>(WorldModel.GetNearbyCubeKeys(e.Entity.Position));
                foreach (var c in currentCubes.Except(newCubes))
                    CubeClients[c].Remove(client);
                ClientCubes[client] = newCubes;
            }

            // notify all clients subsribed to the relevant cubes
            int oldCube = old == null ? 0 : WorldModel.GetCubeKey(old.Value.Position);
            int newCube = WorldModel.GetCubeKey(e.Entity.Position);
            if (old != null && oldCube != newCube)
                foreach (var c in CubeClients[oldCube].Except(CubeClients[newCube]))
                    c.Drop(e.Entity);
            foreach (var c in CubeClients[newCube])
                c.Send(e.Entity);
        }

        public void Apply(TaskUpdateEvent e)
        {
            _log.Debug(e.GetType() + e.Description);
            History.Add(e.Task);
        }

        public void Apply(SkillEvent e)
        {
            _log.Debug(e.GetType() + e.Description);
            History.Add(new[] { e.Source, e.Target });

            var sourceCube = WorldModel.GetCubeKey(e.Source.Position);
            var targetCube = WorldModel.GetCubeKey(e.Target.Position);
            var clients = sourceCube == targetCube ? CubeClients[sourceCube] : CubeClients[sourceCube].Union(CubeClients[targetCube]);
            foreach (var c in clients)
                c.Send(new ToClient.CombatReport(e.Source, e.Skill, e.Target, 20));
        }

        /// <summary>
        /// if a new client has entered the world,
        /// notify them about surrounding physical objects
        /// this routine will send the client mobile's
        /// position as one of the 'nearby' mobiles.
        /// </summary>
        public void SendInitialWorldView(ClientConnection client)
        {

            client.Send(Weather);

            // TODO: Just subscribe them to the cubes instead?
            //foreach (EntityModel p in GetNearby(client.Avatar))
            //client.Send(p);
        }

        public void SendPartyTalk(ClientConnection client, string message)
        {
            string party;
            if (InParty.TryGetValue(client, out party))
                SendPartyTalk(client, party, message);
        }

        public void SendPartyTalk(ClientConnection client, string party, string message)
        {
            // TODO: which name to use?
            var name = client.Avatar == null ? client.AuthenticatedUsername : client.Avatar.Name;

            // TODO: can probably simplify this
            HashSet<string> members;
            if (Party.TryGetValue(party, out members))
            {
                foreach (var m in members)
                {
                    ClientConnection cc;
                    if (Users.TryGetValue(m, out cc))
                        cc.Communication(name, message, Strive.Network.Messages.CommunicationType.PartyTalk);
                }
            }
        }

        public void TransferLeadership(string currentLeader, string newLeader)
        {
            Contract.Requires<ArgumentException>(currentLeader != newLeader);

            Party[newLeader] = Party[currentLeader];
            Party.Remove(currentLeader);
        }

        public void LeaveParty(ClientConnection client, string member)
        {
            var followers = Party[member];
            if (followers != null && followers.Count > 0)
                TransferLeadership(member, followers.First());
            followers.Remove(member);
            InParty[client] = null;

            // TODO: which name to use?
            SendPartyTalk(client, member + " has left your party");
        }

        internal void RemoveClient(ClientConnection client)
        {
            var party = Party[client.AuthenticatedUsername];
            if (party != null)
                TransferLeadership(client.AuthenticatedUsername, party.First());
            Users.Remove(client.AuthenticatedUsername);
        }
    }
}
