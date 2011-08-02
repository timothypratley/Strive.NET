using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Common.Logging;
using Microsoft.FSharp.Collections;
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

        /// <summary>
        /// Entity id maps to a client connection if that entity is currently possessed by a user
        /// </summary>
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

        public void Update(DateTime when)
        {
            UpdateMissions();
            UpdateEntities(when);
            UpdateProduction(when);
            WeatherUpdate(when);
        }

        public void UpdateMissions()
        {
            foreach (var doing in History.Head.EntityDoingTasks)
                UpdateDoingTasks(doing);
            foreach (var mission in History.Head.Mission.Values())
                UpdateMission(mission);
            foreach (var task in History.Head.Task.Values())
                AssignTaskToNearestEntity(task);
        }

        public void UpdateEntities(DateTime when)
        {
            foreach (var e in History.Head.Entity.Values())
                e.UpdateEntity(this, when);
        }

        EntityModel ClosestToTask(TaskModel task, EntityModel e1, EntityModel e2)
        {
            return (e1.Position - task.Finish).Length <= (e2.Position - task.Finish).Length
                ? e1
                : e2;
        }

        void AssignTaskToNearestEntity(TaskModel task)
        {
            var world = History.Head;
            var mission = world.Mission[task.MissionId];
            if (mission.DoerIds.IsEmpty)
            {
                // TODO: find an appropriate entity
            }
            else
            {
                var doer = mission.DoerIds
                    .Select(z => world.Entity[z])
                    .Aggregate((x, y) => ClosestToTask(task, x, y));
                Apply(new TaskAssignmentEvent(task, doer.Id, "Task " + task + " assigned to " + doer));
            }
        }

        void UpdateMission(MissionModel mission)
        {
            var world = History.Head;
            var doers = mission.DoerIds.Select(x => world.Entity[x]);

            // Has this mission been completed?
            if (doers.Any(d => IsMissionComplete(mission, d)))
            {
                Apply(new MissionCompleteEvent(mission, "Finished " + mission));
                return;
            }

            // What task chains exist to satisfy this mission?
            // Build a graph of connections from current state to final state

            // Which task chain should I choose?
            // Shortest path search of the graph

            var tasks = new List<TaskModel>();
            var task = new TaskModel(Global.Rand.Next(), mission.Id, mission.Destination, null);
            tasks.Add(task);

            // Does this match my current task allocation?
            // Only change if there is a significant reason to
            IEnumerable<TaskModel> old;
            var TaskIds = world.MissionRequiresTasks.ValueOrDefault(mission.Id);

            if (TaskIds == null)
                old = Enumerable.Empty<TaskModel>();
            else
            {
                old = TaskIds.Select(id => world.Task[id]);
                foreach (var t in old.Where(x => !tasks.Any(y => y.Matches(x))))
                    Apply(new TaskCompleteEvent(t, null, "Remove task " + t + " of " + mission));
            }
            foreach (var t in tasks.Where(x => !old.Any(y => y.Matches(x))))
                Apply(new TaskUpdateEvent(t, "Added task " + t + " of " + mission));
        }

        void UpdateDoingTasks(KeyValuePair<int, Microsoft.FSharp.Collections.FSharpSet<int>> doing)
        {
            var world = History.Head;
            var doer = world.Entity[doing.Key];
            var tasks = doing.Value.Select(id => world.Task[id]);
            foreach (var task in tasks.Where(t => IsTaskComplete(t, doer)))
                Apply(new TaskCompleteEvent(task, doer, "Finished " + task + " by " + doer));
        }

        bool IsTaskComplete(TaskModel task, EntityModel doer)
        {
            return (doer.Position - task.Finish).Length <= 1;
        }

        // TODO: Might be useful to differentiate between success and failure reason for completion
        bool IsMissionComplete(MissionModel mission, EntityModel doer)
        {
            if ((doer.Position - mission.Destination).Length > 1)
                return false;

            var world = History.Head;
            var holding = world.EntityHoldingEntities.ValueOr(doer.Id, SetModule.Empty<int>());

            switch (mission.Action)
            {
                case EnumMissionAction.Move:
                    return !mission.Targets.Except(holding).Any();
                case EnumMissionAction.Destroy:
                    return !mission.Targets.Any();
                case EnumMissionAction.Protect:
                    return !mission.Targets.Any();
                case EnumMissionAction.Build:
                    // TODO: hmmm targets are not built yet
                    return true;
                case EnumMissionAction.Capture:
                    return mission.Targets.All(t =>
                    {
                        var oo = world.Entity.TryFind(t);
                        return oo == null || oo.Value.Owner == doer.Owner;
                    });
                case EnumMissionAction.Harvest:
                    return false;
                default:
                    Contract.Assert(false, "Unexpected " + mission.Action);
                    return false;
            }
        }

        public TaskModel DoingTask(EntityModel entity)
        {
            var doing = History.Head.EntityDoingTasks.TryFind(entity.Id);
            if (doing == null)
                return null;
            return History.Head.Task[doing.Value.First()];
        }

        public void UpdateProduction(DateTime when)
        {
            foreach (var p in History.Head.EntityProducing)
            {
                var producerId = p.Key;
                var production = p.Value;
                var factory = History.Head.Entity[producerId];
                var progressChange = p.Value.Rate * (float)(when - p.Value.LastUpdated).TotalSeconds;
                if (production.Progress + progressChange >= p.Value.Span)
                {
                    var creation = new EntityModel(production.Queue.First(),
                        "Creation", "RTSRobot", factory.Position, factory.Rotation, 100, 100, EnumMobileState.Standing, 1.7f);
                    Apply(new ProductionCompleteEvent(producerId, creation,
                        "Production of " + creation.Name + " by " + factory.Name + " complete"));
                }
                else
                {
                    Apply(new ProductionUpdateEvent(producerId, progressChange, "Production update"));
                }
            }
        }

        // TODO: Make weather recorded
        void WeatherUpdate(DateTime when)
        {
            if ((when.Ticks - Weather.ServerNow) < 1)
                return;

            Weather.ServerNow = when.Ticks;

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

        public void LogMessage(EntityModel entity, string message)
        {
            ClientConnection client;
            if (Possession.TryGetValue(entity.Id, out client))
                client.LogMessage(message);
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
            SendToUsers(e.Task);
        }

        public void Apply(TaskCompleteEvent e)
        {
            _log.Debug(e.GetType() + " " + e.Description);
            History.Complete(e.Task, e.Doer);
        }

        private void Apply(TaskAssignmentEvent e)
        {
            _log.Debug(e.GetType() + " " + e.Description);
            History.Assign(e.Task, e.DoerId);
            // TODO: SendToUsers(new ToClient.DropMission(e.Mission.Id));
        }

        public void Apply(MissionUpdateEvent e)
        {
            _log.Debug(e.GetType() + " " + e.Description);
            History.Add(e.Mission);
            SendToUsers(e.Mission);
        }

        public void Apply(MissionCompleteEvent e)
        {
            _log.Debug(e.GetType() + " " + e.Description);
            History.Complete(e.Mission);
            // TODO: SendToUsers(new ToClient.DropMission(e.Mission.Id));
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
            // TODO: lookup the actual target build time required
            var span = 3;
            History.Head = History.Head.WithProduction(e.ProducerId, e.ProductId, span, Global.Now);
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
