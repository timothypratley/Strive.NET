using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;
using Common.Logging;
using Strive.Common;
using Strive.Data.Events;
using Strive.Model;
using Strive.Network.Messaging;
using ToClient = Strive.Network.Messages.ToClient;


namespace Strive.Server.Logic
{
    public partial class World
    {
        double _highX;
        double _highZ;
        double _lowX;
        double _lowZ;
        int _squaresInX;		// see squareSize in Square
        int _squaresInZ;
        readonly int _worldId;

        // squares are used to group physical objects
        Square[,] _square;
        TerrainModel[,] _terrain;

        // all physical objects are indexed in a hash-table
        public Dictionary<int, EntityModel> PhysicalObjects { get; private set; }
        public List<CombatantModel> Mobiles { get; private set; }

        // TODO: use this!!
        public History History = new History();

        // TODO: do we know the sun texture etc here?
        const int DefaultDay = 147;
        const int DefaultNight = 148;
        const int DefaultCusp = 5;
        const int DefaultSun = 146;
        public ToClient.TimeAndWeather Weather = new ToClient.TimeAndWeather(
            Global.Now, 0, DefaultDay, DefaultNight, DefaultCusp, DefaultSun, 0, 0);

        readonly ILog _log = LogManager.GetCurrentClassLogger();

        public World(int worldId)
        {
            _worldId = worldId;
            Load();
        }

        public void Update()
        {
            foreach (CombatantModel c in PhysicalObjects.Values.OfType<CombatantModel>())
                this.Updatee(c);
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
                NotifyMobiles(Weather);
        }

        public void NotifyMobiles(object message)
        {
            foreach (Avatar ma in Mobiles.Where(ma => ma.Client != null))
                ma.Client.Send(message);
        }

        public void Apply(EntityUpdateEvent e)
        {
            _log.Debug(e.GetType() + e.Description);
            History.Add(e.Entity);
            Add(e.Entity);
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

            // TODO: yikes! just send the entire entity??
            // TODO: perhaps I can accumulate all changes in a 'tick' and send out 'dirty'
            // TODO: should inform near the target as well
            InformNearby(
                e.Source,
                new ToClient.CombatReport(e.Source, e.Skill, e.Target, 20));
        }


        public void Add(TerrainModel t)
        {
            // keep terrain separate
            int terrainX = DivTruncate((int)(t.Position.X - _lowX), Constants.TerrainPieceSize);
            int terrainZ = DivTruncate((int)(t.Position.Z - _lowZ), Constants.TerrainPieceSize);
            _terrain[terrainX, terrainZ] = t;
        }

        public void Add(EntityModel po)
        {
            // keep everything at ground level
            /*
            double? altitude = AltitudeAt(po.Position.X, po.Position.Z);
            if (altitude.HasValue)
                po.Position.Y = altitude.Value + po.Height / 2F;
            else
                _log.Warn("Physical object " + po.Id + " is not on terrain.");
             */

            History.Add(po);

            if (po.Position.X > _highX || po.Position.Z > _highZ
                || po.Position.X < _lowX || po.Position.Z < _lowZ)
            {
                _log.Error("Tried to add physical object " + po.Id + " outside the world.");
                return;
            }

            // add the object to the world
            PhysicalObjects.Add(po.Id, po);
            if (po is CombatantModel)
                Mobiles.Add((CombatantModel)po);
            int squareX = (int)(po.Position.X - _lowX) / Square.SquareSize;
            int squareZ = (int)(po.Position.Z - _lowZ) / Square.SquareSize;
            if (_square[squareX, squareZ] == null)
                _square[squareX, squareZ] = new Square();
            _square[squareX, squareZ].Add(po);

            // notify all nearby clients that a new
            // physical object has entered the world
            InformNearby(po, po);
            //Log.Info( "Added new " + po.GetType() + " " + po.ObjectInstanceID + " at (" + po.Position.X + "," + po.Position.Y + "," +po.Position.Z + ") - ("+squareX+","+squareZ+")" );
        }

        public void Remove(EntityModel po)
        {
            int squareX = (int)(po.Position.X - _lowX) / Square.SquareSize;
            int squareZ = (int)(po.Position.Z - _lowZ) / Square.SquareSize;
            InformNearby(po, new ToClient.DropPhysical(po));
            _square[squareX, squareZ].Remove(po);
            PhysicalObjects.Remove(po.Id);
            if (po is Avatar)
                Mobiles.Remove((Avatar)po);
            _log.Info("Removed " + po.GetType() + " " + po.Id + " from the world.");
        }

#if false
        public void Relocate(EntityModel po, Vector3D newPosition, Quaternion newRotation, EnumMobileState mobileState)
        {
            // keep everything inside world bounds
            if (newPosition.X >= _highX)
                newPosition.X = (float)_highX - 1;
            if (newPosition.Z >= _highZ)
                newPosition.Z = (float)_highZ - 1;
            if (newPosition.X <= _lowX)
                newPosition.X = (float)_lowX + 1;
            if (newPosition.Z <= _lowZ)
                newPosition.Z = (float)_lowZ + 1;

            int fromSquareX = (int)(po.Position.X - _lowX) / Square.SquareSize;
            int fromSquareZ = (int)(po.Position.Z - _lowZ) / Square.SquareSize;
            int toSquareX = (int)(newPosition.X - _lowX) / Square.SquareSize;
            int toSquareZ = (int)(newPosition.Z - _lowZ) / Square.SquareSize;
            int i, j;

            // TODO: disallow the relocation if it is outside terrain
            //return;
            // keep everything on the ground
            // TODO: re-factor below ma and overload Height
            double? altitude = AltitudeAt(newPosition.X, newPosition.Z);
            if (altitude.HasValue)
            {
                if (po is Avatar)
                    altitude += ((Avatar)po).CurrentHeight / 2;
                else
                    altitude += po.Height / 2;
                newPosition.Y = altitude.Value;
            }

            var ma = po as Avatar;

            // check that the object can fit there
            // TODO: revisit
            /** TODO:
            for ( i=-1; i<=1; i++ ) {
                for ( j=-1; j<=1; j++ ) {
                    if (
                        toSquareX+i < 0 || toSquareX+i >= squaresInX
                        || toSquareZ+j < 0 || toSquareZ+j >= squaresInZ
                        || square[toSquareX+i,toSquareZ+j] == null
                    ) continue;

                    foreach ( PhysicalObject spo in square[toSquareX+i,toSquareZ+j].physicalObjects ) {
                        // ignoring yourself
                        if ( spo == po ) continue;

                        // distance between two objects in 2d space
                        float dx = newPosition.X - spo.Position.X;
                        float dy = newPosition.Y - spo.Position.Y;
                        float dz = newPosition.Z - spo.Position.Z;
                        float distance_squared = dx*dx + dy*dy + dz*dz;
                        if ( distance_squared <= spo.BoundingSphereRadiusSquared + po.BoundingSphereRadiusSquared ) {
                            // only if not already collided!
                            float dx1 = po.Position.X - spo.Position.X;
                            float dy1 = po.Position.Y - spo.Position.Y;
                            float dz1 = po.Position.Z - spo.Position.Z;
                            float distance_squared2 = dx1*dx1 + dy1*dy1 + dz1*dz1;
                            if ( distance_squared2 <= spo.BoundingSphereRadiusSquared + po.BoundingSphereRadiusSquared ) {
                                continue;
                            }
                            if ( ma != null && ma.client != null ) {
                                ma.client.Send(
                                    new ToClient.Position( ma ) );
                                return;
                            }
                        }
                    }
                }
            }
            */

            // send everything nearby
            //public void server_foo(float x1, float z1, float x, float z) {
            if (ma != null && ma.Client != null && po.Position != newPosition)
            {
                int tx1 = DivTruncate((int)po.Position.X, Constants.TerrainPieceSize);
                int tz1 = DivTruncate((int)po.Position.Z, Constants.TerrainPieceSize);
                for (int k = 0; k < Constants.TerrainZoomOrder; k++)
                {
                    int tbx = DivTruncate((int)newPosition.X, Constants.TerrainPieceSize) - Constants.xRadius[k];
                    int tbz = DivTruncate((int)newPosition.Z, Constants.TerrainPieceSize) - Constants.zRadius[k];

                    // Normalize to a 'grid' point
                    tbx = DivTruncate(tbx, Constants.scale[k]) * Constants.scale[k];
                    tbz = DivTruncate(tbz, Constants.scale[k]) * Constants.scale[k];

                    for (i = 0; i <= Constants.xRadius[k] * 2; i += Constants.scale[k])
                    {
                        for (j = 0; j <= Constants.zRadius[k] * 2; j += Constants.scale[k])
                        {
                            int tx = tbx + i;
                            int tz = tbz + j;
                            if ((Math.Abs(tx - tx1) > Constants.xRadius[k]) || (Math.Abs(tz - tz1) > Constants.zRadius[k]))
                            {
                                int terrainX = tx - (int)_lowX / Constants.TerrainPieceSize;
                                int terrainZ = tz - (int)_lowZ / Constants.TerrainPieceSize;
                                if (terrainX >= 0 && terrainX < _squaresInX * Square.SquareSize / Constants.TerrainPieceSize && terrainZ >= 0 && terrainZ < _squaresInZ * Square.SquareSize / Constants.TerrainPieceSize)
                                {
                                    TerrainModel t = _terrain[terrainX, terrainZ];
                                    if (t != null)
                                    {
                                        if (// there is no higher zoom order
                                            k == (Constants.TerrainZoomOrder - 1)
                                            // this is not a higher order point
                                            || (tx % Constants.scale[k + 1]) != 0 || (tz % Constants.scale[k + 1]) != 0)
                                            ma.Client.Send(t);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //}

            // TODO: This will result in too many messages, should only send messages in one place
            Add(po.Move(newPosition, newRotation));

            for (i = -1; i <= 1; i++)
            {
                for (j = -1; j <= 1; j++)
                {
                    if (Math.Abs(fromSquareX + i - toSquareX) > 1
                        || Math.Abs(fromSquareZ + j - toSquareZ) > 1)
                    {
                        // squares which need to have their clients
                        // add or remove the object
                        // as the jump has brought the object in or out of focus
                        /**** let the client remove them
                                                // remove from
                                                if (
                                                    // check the square exists
                                                    fromSquareX+i >= 0 && fromSquareX+i < squaresInX
                                                    && fromSquareZ+j >= 0 && fromSquareZ+j < squaresInZ
                                                    && square[fromSquareX+i, fromSquareZ+j] != null
                                                ) {
                                                    square[fromSquareX+i, fromSquareZ+j].NotifyClients(
                                                        new ToClient.DropPhysicalObject( po ) );
                                                    // if the object is a mobile, it needs to be made aware
                                                    // of its new world view
                                                    if ( ma != null && ma.client != null ) {
                                                        foreach( PhysicalObject toDrop in square[fromSquareX+i, fromSquareZ+j].physicalObjects ) {
                                                            // Don't drop terrain, client is responsible for that
                                                            if ( toDrop is Terrain ) continue;
                                                            ma.client.Send(
                                                                new ToClient.DropPhysicalObject( toDrop ) );
                                                            //Log.Info( "Told client to drop " + toDrop.ObjectInstanceID + "." );
                                                        }
                                                    }
                                                }
                                                ***/

                        // add to
                        if (// check the square exists
                            toSquareX - i >= 0 && toSquareX - i < _squaresInX
                            && toSquareZ - j >= 0 && toSquareZ - j < _squaresInZ
                            && _square[toSquareX - i, toSquareZ - j] != null)
                        {
                            _square[toSquareX - i, toSquareZ - j].NotifyClients(po);
                            // if the object is a player, it needs to be made aware
                            // of its new world view
                            if (ma != null && ma.Client != null)
                            {
                                foreach (EntityModel toAdd in _square[toSquareX - i, toSquareZ - j].PhysicalObjects)
                                    ma.Client.Send(toAdd);
                                //Log.Info( "Told client to add " + toAdd.ObjectInstanceID + "." );
                            }
                        }
                    }
                    else
                    {
                        // clients that have the object already in scope need to be
                        // told its new position
                        if (// check the square exists
                            toSquareX + i >= 0 && toSquareX + i < _squaresInX
                            && toSquareZ + j >= 0 && toSquareZ + j < _squaresInZ
                            && _square[toSquareX + i, toSquareZ + j] != null)
                        {
                            if (ma != null && ma.Client != null)
                                _square[toSquareX + i, toSquareZ + j].NotifyClientsExcept(
                                    new ToClient.PositionUpdate(po),
                                    ma.Client);
                            else
                                _square[toSquareX + i, toSquareZ + j].NotifyClients(
                                    new ToClient.PositionUpdate(po));
                        }
                    }
                }
            }

            // transition the object to its new square if it changed squares
            if (fromSquareX != toSquareX || fromSquareZ != toSquareZ)
            {
                _square[fromSquareX, fromSquareZ].Remove(po);
                if (_square[toSquareX, toSquareZ] == null)
                    _square[toSquareX, toSquareZ] = new Square();
                _square[toSquareX, toSquareZ].Add(po);
            }
        }
#endif

        public void InformNearby(EntityModel po, object message)
        {
            // notify all nearby clients
            int squareX = (int)(po.Position.X - _lowX) / Square.SquareSize;
            int squareZ = (int)(po.Position.Z - _lowZ) / Square.SquareSize;
            int i;
            for (i = -1; i <= 1; i++)
            {
                int j;
                for (j = -1; j <= 1; j++)
                {
                    // check that neighbor exists
                    if (squareX + i < 0 || squareX + i >= _squaresInX
                        || squareZ + j < 0 || squareZ + j >= _squaresInZ
                        || _square[squareX + i, squareZ + j] == null)
                        continue;

                    // need to send a message to all nearby clients
                    // so long as the square isn't empty
                    _square[squareX + i, squareZ + j].NotifyClients(message);
                }
            }
        }

        public void SendInitialWorldView(ClientConnection client)
        {
            // if a new client has entered the world,
            // notify them about surrounding physical objects
            // NB: this routine will send the client mobile's
            // position as one of the 'nearby' mobiles.

            // TODO: figure out how to make client.Avatar the right type
            var mob = (Avatar)client.Avatar;
            client.Send(Weather);

            foreach (EntityModel p in GetNearby(mob))
                client.Send(p);

            for (int k = 0; k < Constants.TerrainZoomOrder; k++)
            {
                int tbx = DivTruncate((int)mob.Position.X, Constants.TerrainPieceSize) - Constants.xRadius[k];
                int tbz = DivTruncate((int)mob.Position.Z, Constants.TerrainPieceSize) - Constants.zRadius[k];

                // Normalize to a 'grid' point
                tbx = DivTruncate(tbx, Constants.scale[k]) * Constants.scale[k];
                tbz = DivTruncate(tbz, Constants.scale[k]) * Constants.scale[k];

                for (int i = 0; i <= Constants.xRadius[k] * 2; i += Constants.scale[k])
                {
                    for (int j = 0; j <= Constants.zRadius[k] * 2; j += Constants.scale[k])
                    {
                        int tx = tbx + i;
                        int tz = tbz + j;
                        int terrainX = tx - (int)_lowX / Constants.TerrainPieceSize;
                        int terrainZ = tz - (int)_lowZ / Constants.TerrainPieceSize;
                        if (terrainX >= 0 && terrainX < _squaresInX * Square.SquareSize / Constants.TerrainPieceSize && terrainZ >= 0 && terrainZ < _squaresInZ * Square.SquareSize / Constants.TerrainPieceSize)
                        {
                            TerrainModel t = _terrain[terrainX, terrainZ];
                            if (t != null)
                            {
                                if (// there is no higher zoom order
                                    k == (Constants.TerrainZoomOrder - 1)
                                    // this is not a higher order point
                                    || (tx % Constants.scale[k + 1]) != 0 || (tz % Constants.scale[k + 1]) != 0)
                                    client.Send(t);
                            }
                        }
                    }
                }
            }
        }

        private IEnumerable<EntityModel> GetNearby(Avatar mob)
        {
            // TODO: zomg I don't know if this div truncate is right
            int squareX = DivTruncate((int)(mob.Position.X - _lowX), Square.SquareSize);
            int squareZ = DivTruncate((int)(mob.Position.Z - _lowZ), Square.SquareSize);

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    // check that neighbor exists
                    if (squareX + i < 0 || squareX + i >= _squaresInX
                        || squareZ + j < 0 || squareZ + j >= _squaresInZ
                        || _square[squareX + i, squareZ + j] == null)
                        continue;

                    // all neighboring physical objects
                    // to the clients world view
                    // that are in scope

                    foreach (var p in _square[squareX + i, squareZ + j].PhysicalObjects)
                        yield return p;
                }
            }
        }

        public double? AltitudeAt(double x, double z)
        {
            int terrainX = DivTruncate((int)(x - _lowX), Constants.TerrainPieceSize);
            int terrainZ = DivTruncate((int)(z - _lowZ), Constants.TerrainPieceSize);

            // if terrain piece exists, keep everything on the ground
            if (_terrain[terrainX, terrainZ] != null
                && _terrain[terrainX + 1, terrainZ] != null
                && _terrain[terrainX, terrainZ + 1] != null
                && _terrain[terrainX + 1, terrainZ + 1] != null)
            {
                double dx = x - _terrain[terrainX, terrainZ].Position.X;
                double dz = z - _terrain[terrainX, terrainZ].Position.Z;

                // terrain is a diagonally split square, forming two triangles
                // which touch the altitude points of 4 neighboring terrain
                // points, the current terrain and its xplus, zplus, xpluszplus.
                // so for either triangle, just apply the slope in x and z
                // to find the altitude at that point
                double xslope;
                double zslope;
                if (dz < dx)
                {
                    // lower triangle
                    xslope = (_terrain[terrainX + 1, terrainZ].Position.Y - _terrain[terrainX, terrainZ].Position.Y) / Constants.TerrainPieceSize;
                    zslope = (_terrain[terrainX + 1, terrainZ + 1].Position.Y - _terrain[terrainX + 1, terrainZ].Position.Y) / Constants.TerrainPieceSize;
                }
                else
                {
                    // upper triangle
                    xslope = (_terrain[terrainX + 1, terrainZ + 1].Position.Y - _terrain[terrainX, terrainZ + 1].Position.Y) / Constants.TerrainPieceSize;
                    zslope = (_terrain[terrainX, terrainZ + 1].Position.Y - _terrain[terrainX, terrainZ].Position.Y) / Constants.TerrainPieceSize;
                }
                return _terrain[terrainX, terrainZ].Position.Y + xslope * dx + zslope * dz;
            }
            // no terrain here
            return null;
        }

        // handle negative numbers
        static public int DivTruncate(int x, int y)
        {
            return (x / y - ((x < 0 && (x % y != 0)) ? 1 : 0));
        }

        public void Set(EntityModel entity, EnumMobileState state)
        {
            // TODO: Should apply an event
            if (entity.MobileState != state)
                Add(entity.WithState(state));
        }
    }
}
