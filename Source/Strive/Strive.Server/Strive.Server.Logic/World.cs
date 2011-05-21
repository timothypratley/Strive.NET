using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Common.Logging;
using Microsoft.FSharp.Core;
using Strive.Common;
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

        public World(Listener listener, int worldId, History history)
        {
            History = history;
            CubeClients = new Dictionary<int, HashSet<ClientConnection>>();
            ClientCubes = new Dictionary<ClientConnection, HashSet<int>>();
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
            UpdatePlanTasks();

            foreach (var e in History.Head.Entity.Select(p => p.Value))
            {
                if (e is CombatantModel)
                    this.UpdateCombatant((CombatantModel)e);
            }

            UpdateProduction();

            WeatherUpdate();
        }

        public void UpdatePlanTasks()
        {
            foreach (var plan in History.Head.Plan.Select(o => o.Value))
            {
                var tasks = History.Head.Requires.TryFind(plan.Id);
                if (tasks == null)
                {
                    Apply(new TaskUpdateEvent(
                        new TaskModel(Global.Rand.Next(), plan.Id, plan.Start.Position, plan.Finish.Position),
                        "Added task for " + plan.Action + " plan"));
                }
            }
        }

        public TaskModel DoingTask(EntityModel entity)
        {
            var doing = History.Head.Doing.TryFind(entity.Id);
            if (doing == null)
                return null;
            return History.Head.Task[doing.Value.First()];
        }

        public void UpdateProduction()
        {
            foreach (var p in History.Head.Producing)
            {
                var factory = History.Head.Entity.TryFind(p.Key);
                var progressChange = p.Value.Rate * (float)(Global.Now - p.Value.LastUpdated).TotalSeconds;
                if (p.Value.Progress + progressChange >= p.Value.Target)
                {
                    var creation = new EntityModel(p.Value.Queue.First(),
                        "Creation", "RTSRobot", factory.Value.Position, factory.Value.Rotation, 100, 100, EnumMobileState.Standing, 1.7f);
                    Apply(new ProductionCompleteEvent(p.Key, creation,
                        "Production of " + creation.Name + " by " + factory.Value.Name + " complete"));
                }
                else
                {
                    var produce = p.Value.WithProgressChange(progressChange, Global.Now);
                    Apply(new ProductionUpdateEvent(p.Key, progressChange, "Production update"));
                }
            }
        }

        // TODO: Make weather recorded
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
            _log.Debug(e.GetType() + " " + e.Description);

            var oldEntities = e.Entities.Select(entity => History.Head.Entity.TryFind(entity.Id)).ToArray();

            History.Add(e.Entities);

            e.Entities.Zip(oldEntities, (ee, old) => UpdateCubes(ee, old)).LastOrDefault();
        }

        public int UpdateCubes(EntityModel entity, FSharpOption<EntityModel> old)
        {
            int newCube = WorldModel.GetCubeKey(entity.Position);

            // update client cubes if the entity was possessed
            ClientConnection client;
            Possession.TryGetValue(entity.Id, out client);
            if (client != null)
            {
                var newCubes = new HashSet<int>(WorldModel.GetNearbyCubeKeys(newCube));

                HashSet<int> oldCubes;
                HashSet<ClientConnection> clients;
                IEnumerable<int> add;
                if (ClientCubes.TryGetValue(client, out oldCubes))
                {
                    foreach (var cube in oldCubes.Except(newCubes))
                    {
                        if (CubeClients.TryGetValue(cube, out clients))
                        {
                            clients.Remove(client);
                            if (clients.Count == 0)
                                CubeClients.Remove(cube);
                        }
                    }
                    add = newCubes.Except(oldCubes);
                }
                else
                    add = newCubes;

                foreach (var cube in add)
                    if (CubeClients.TryGetValue(cube, out clients))
                        clients.Add(client);
                    else
                        CubeClients.Add(cube, new HashSet<ClientConnection> { client });

                ClientCubes[client] = newCubes;
            }

            // notify all clients subscribed to the relevant cubes
            HashSet<ClientConnection> newClients;
            if (!CubeClients.TryGetValue(newCube, out newClients))
                newClients = new HashSet<ClientConnection>();

            if (old != null)
            {
                int oldCube = WorldModel.GetCubeKey(old.Value.Position);
                HashSet<ClientConnection> oldClients;
                if (!CubeClients.TryGetValue(oldCube, out oldClients))
                    oldClients = new HashSet<ClientConnection>();
                if (oldCube != newCube)
                    foreach (var c in oldClients.Except(newClients))
                        c.Drop(entity);
            }

            foreach (var c in newClients.Where(x => x != client))
                c.Send(entity);

            return newCube;
        }

        public void Apply(TaskUpdateEvent e)
        {
            _log.Debug(e.GetType() + " " + e.Description);
            History.Add(e.Task);
        }

        public void Apply(PlanUpdateEvent e)
        {
            _log.Debug(e.GetType() + " " + e.Description);
            History.Add(e.Plan);
        }

        public void Apply(TaskCompleteEvent e)
        {
            _log.Debug(e.GetType() + " " + e.Description);
            History.Complete(e.Doer, e.Task);
        }

        public void Apply(PlanCompleteEvent e)
        {
            _log.Debug(e.GetType() + " " + e.Description);
            History.Add(e.Plan);
        }

        public void Apply(SkillEvent e)
        {
            _log.Debug(e.GetType() + " " + e.Description);
            History.Add(new[] { e.Source, e.Target });

            var sourceCube = WorldModel.GetCubeKey(e.Source.Position);
            HashSet<ClientConnection> sourceClients;
            if (!CubeClients.TryGetValue(sourceCube, out sourceClients))
                sourceClients = new HashSet<ClientConnection>();
            var targetCube = WorldModel.GetCubeKey(e.Target.Position);
            HashSet<ClientConnection> targetClients;
            if (!CubeClients.TryGetValue(targetCube, out targetClients))
                targetClients = new HashSet<ClientConnection>();

            foreach (var c in sourceClients.Union(targetClients))
                c.Send(new ToClient.CombatReport(e.Source, e.Skill, e.Target, 20));
        }

        public void Apply(ProductionStartedEvent e)
        {
            _log.Debug(e.GetType() + " " + e.Description);
            History.Head = History.Head.WithProduction(e.ProducerId, e.ProductId, Global.Now);
        }

        public void Apply(ProductionUpdateEvent e)
        {
            _log.Debug(e.GetType() + " " + e.Description);
            History.Head = History.Head.WithProductionProgressChange(e.EntityId, e.ProgressChange, Global.Now);
        }

        public void Apply(ProductionCompleteEvent e)
        {
            _log.Debug(e.GetType() + " " + e.Description);
            History.Head = History.Head.WithProductionComplete(e.ProducerId, e.Entity, Global.Now);
            UpdateCubes(e.Entity, null);
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
            HashSet<string> party;
            if (Party.TryGetValue(client.AuthenticatedUsername, out party))
                TransferLeadership(client.AuthenticatedUsername, party.First());
            Users.Remove(client.AuthenticatedUsername);
        }
    }
}
