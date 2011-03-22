using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Media.Media3D;
using Common.Logging;
using Strive.Common;
using Strive.Network.Messages;
using Strive.Network.Messaging;
using Strive.Server.Model;
using ToClient = Strive.Network.Messages.ToClient;


// todo: this object needs to be made threadsafe... why?
namespace Strive.Server.Logic
{
    public class World
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
        Terrain[,] _terrain;

        // all physical objects are indexed in a hashtable
        public Dictionary<int, PhysicalObject> PhysicalObjects { get; private set; }
        public List<MobileAvatar> Mobiles { get; private set; }

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

        public class InvalidWorld : Exception { }
        public void Load()
        {
            PhysicalObjects = new Dictionary<int, PhysicalObject>();
            Mobiles = new List<MobileAvatar>();

            // todo: would be nice to be able to load only the
            // world in question... but for now load them all
            if (Global.WorldFilename != null)
            {
                _log.Info("Loading Global.modelSchema from file:" + Global.WorldFilename);
                Global.ModelSchema = new Schema();
                Global.ModelSchema.ReadXml(Global.WorldFilename);
            }
            //else if (Global.connectionstring != null)
            //{
            //Global.modelSchema = Strive.Data.MultiverseFactory.getMultiverseFromDatabase(Global.connectionstring);
            //}
            else
            {
                _log.Info("Creating an empty Global.modelSchema");
                CreateDefaultWorld();
            }
            _log.Info("Global.modelSchema loaded");

            // find highX and lowX for our world dimensions

            // TODO: make expandable universe, don't code these values
            _highX = 1000;
            _lowX = -1000;
            _highZ = 1000;
            _lowZ = -1000;
            foreach (Schema.ObjectInstanceRow r in Global.ModelSchema.ObjectInstance.Rows)
            {
                if (_highX == 0)
                    _highX = r.X;
                if (_lowX == 0)
                    _lowX = r.X;
                if (_highZ == 0)
                    _highZ = r.Z;
                if (_lowZ == 0)
                    _lowZ = 0;

                if (r.X > _highX)
                    _highX = r.X;
                if (r.X < _lowX)
                    _lowX = r.X;
                if (r.Z > _highZ)
                    _highZ = r.Z;
                if (r.Z < _lowZ)
                    _lowZ = r.Z;
            }
            //highX = ((Schema.ObjectInstanceRow)Global.multiverse.ObjectInstance.Select( "X = max(X)" )[0]).X;
            //lowX = ((Schema.ObjectInstanceRow)Global.multiverse.ObjectInstance.Select( "X = min(X)" )[0]).X;
            //highZ = ((Schema.ObjectInstanceRow)Global.multiverse.ObjectInstance.Select( "Z = max(Z)" )[0]).Z;
            //lowZ = ((Schema.ObjectInstanceRow)Global.multiverse.ObjectInstance.Select( "Z = min(Z)" )[0]).Z;
            _log.Info("Global.multiverse bounds are " + _lowX + "," + _lowZ + " " + _highX + "," + _highZ);

            // figure out how many squares we need
            _squaresInX = (int)(_highX - _lowX) / Square.SquareSize + 1;
            _squaresInZ = (int)(_highZ - _lowZ) / Square.SquareSize + 1;

            //			if ( squaresInX * squaresInZ > 10000 ) {
            //				throw new Exception( "World is too big. Total area must not exceed " + 10000*Square.squareSize + ". Please fix the database." );
            //			}

            // allocate the grid of squares used for grouping
            // physical objects that are close to each other
            _square = new Square[_squaresInX, _squaresInZ];
            _terrain = new Terrain[_squaresInX * Square.SquareSize / Constants.TerrainPieceSize, _squaresInZ * Square.SquareSize / Constants.TerrainPieceSize];

            Schema.WorldRow wr = Global.ModelSchema.World.FindByWorldID(_worldId);
            if (wr == null)
            {
                throw new InvalidWorld();
            }

            _log.Info("Loading world \"" + wr.WorldName + "\"...");
            _log.Info("Loading terrain...");
            foreach (Schema.TemplateTerrainRow ttr in Global.ModelSchema.TemplateTerrain.Rows)
            {
                foreach (Schema.ObjectInstanceRow oir in ttr.TemplateObjectRow.GetObjectInstanceRows())
                    Add(new Terrain(ttr, ttr.TemplateObjectRow, oir));
            }
            _log.Info("Loading physical objects...");
            foreach (Schema.TemplateObjectRow otr in Global.ModelSchema.TemplateObject.Rows)
            {
                foreach (Schema.TemplateMobileRow tmr in otr.GetTemplateMobileRows())
                {
                    foreach (Schema.ObjectInstanceRow oir in otr.GetObjectInstanceRows()
                        .Where(oir => oir.GetMobilePossesableByPlayerRows().Length <= 0))
                    {
                        // NB: we only add avatars to our world, not mobiles
                        Add(new MobileAvatar(this, tmr, otr, oir));
                    }
                }
                foreach (Schema.TemplateItemRow tir in otr.GetTemplateItemRows())
                {
                    foreach (Schema.TemplateItemEquipableRow ier in tir.GetTemplateItemEquipableRows())
                    {
                        foreach (Schema.ObjectInstanceRow oir in otr.GetObjectInstanceRows())
                            Add(new Equipable(ier, tir, otr, oir));
                    }
                    foreach (Schema.TemplateItemJunkRow ijr in tir.GetTemplateItemJunkRows())
                    {
                        foreach (Schema.ObjectInstanceRow oir in otr.GetObjectInstanceRows())
                            Add(new Junk(ijr, tir, otr, oir));
                    }
                    foreach (Schema.TemplateItemQuaffableRow iqr in tir.GetTemplateItemQuaffableRows())
                    {
                        foreach (Schema.ObjectInstanceRow oir in otr.GetObjectInstanceRows())
                            Add(new Quaffable(iqr, tir, otr, oir));
                    }
                    foreach (Schema.TemplateItemReadableRow irr in tir.GetTemplateItemReadableRows())
                    {
                        foreach (Schema.ObjectInstanceRow oir in otr.GetObjectInstanceRows())
                            Add(new Readable(irr, tir, otr, oir));
                    }
                    foreach (Schema.TemplateItemWieldableRow iwr in tir.GetTemplateItemWieldableRows())
                    {
                        foreach (Schema.ObjectInstanceRow oir in otr.GetObjectInstanceRows())
                            Add(new Wieldable(iwr, tir, otr, oir));
                    }
                }
            }
            _log.Info("Loaded world.");
        }

        public void Update()
        {
            foreach (MobileAvatar ma in PhysicalObjects.Values.OfType<MobileAvatar>())
                ma.Update();
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

        public void NotifyMobiles(IMessage message)
        {
            foreach (MobileAvatar ma in Mobiles.Where(ma => ma.Client != null))
                ma.Client.Send(message);
        }

        public void Add(PhysicalObject po)
        {
            if (po.Position.X > _highX || po.Position.Z > _highZ
                || po.Position.X < _lowX || po.Position.Z < _lowZ)
            {
                _log.Error("Tried to add physical object " + po.ObjectInstanceId + " outside the world.");
                return;
            }

            if (po is Terrain)
            {
                // keep terrain seperate
                int terrainX = DivTruncate((int)(po.Position.X - _lowX), Constants.TerrainPieceSize);
                int terrainZ = DivTruncate((int)(po.Position.Z - _lowZ), Constants.TerrainPieceSize);
                _terrain[terrainX, terrainZ] = (Terrain)po;
            }
            else
            {
                // keep everything at ground level
                double? altitude = AltitudeAt(po.Position.X, po.Position.Z);
                if (altitude.HasValue)
                    po.Position.Y = altitude.Value + po.Height / 2F;
                else
                    _log.Warn("Physical object " + po.ObjectInstanceId + " is not on terrain.");

                // add the object to the world
                PhysicalObjects.Add(po.ObjectInstanceId, po);
                if (po is MobileAvatar)
                    Mobiles.Add((MobileAvatar)po);
                int squareX = (int)(po.Position.X - _lowX) / Square.SquareSize;
                int squareZ = (int)(po.Position.Z - _lowZ) / Square.SquareSize;
                if (_square[squareX, squareZ] == null)
                    _square[squareX, squareZ] = new Square();
                _square[squareX, squareZ].Add(po);
            }
            // notify all nearby clients that a new
            // physical object has entered the world
            InformNearby(po, ToClient.AddPhysicalObject.CreateMessage(po));
            //Log.Info( "Added new " + po.GetType() + " " + po.ObjectInstanceID + " at (" + po.Position.X + "," + po.Position.Y + "," +po.Position.Z + ") - ("+squareX+","+squareZ+")" );
        }

        public void Remove(PhysicalObject po)
        {
            int squareX = (int)(po.Position.X - _lowX) / Square.SquareSize;
            int squareZ = (int)(po.Position.Z - _lowZ) / Square.SquareSize;
            InformNearby(po, new ToClient.DropPhysicalObject(po));
            _square[squareX, squareZ].Remove(po);
            PhysicalObjects.Remove(po.ObjectInstanceId);
            if (po is MobileAvatar)
                Mobiles.Remove((MobileAvatar)po);
            _log.Info("Removed " + po.GetType() + " " + po.ObjectInstanceId + " from the world.");
        }

        public void Relocate(PhysicalObject po, Vector3D newPosition, Quaternion newRotation)
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

            // TODO: disallow the relocation if it is outside terain
            //return;
            // keep everything on the ground
            // TODO: refactor below ma and overload Height
            double? altitude = AltitudeAt(newPosition.X, newPosition.Z);
            if (altitude.HasValue)
            {
                if (po is MobileAvatar)
                    altitude += ((MobileAvatar)po).CurrentHeight / 2;
                else
                    altitude += po.Height / 2;
                newPosition.Y = altitude.Value;
            }
            
            var ma = po as MobileAvatar;

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

                    // Normalise to a 'grid' point
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
                                    Terrain t = _terrain[terrainX, terrainZ];
                                    if (t != null)
                                    {
                                        if (// there is no higher zoom order
                                            k == (Constants.TerrainZoomOrder - 1)
                                            // this is not a higher order point
                                            || (tx % Constants.scale[k + 1]) != 0 || (tz % Constants.scale[k + 1]) != 0)
                                            ma.Client.Send(ToClient.AddPhysicalObject.CreateMessage(t));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //}

            po.Position = newPosition;
            po.Rotation = newRotation;

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
                            _square[toSquareX - i, toSquareZ - j].NotifyClients(ToClient.AddPhysicalObject.CreateMessage(po));
                            // if the object is a player, it needs to be made aware
                            // of its new world view
                            if (ma != null && ma.Client != null)
                            {
                                foreach (PhysicalObject toAdd in _square[toSquareX - i, toSquareZ - j].PhysicalObjects)
                                {
                                    ma.Client.Send(ToClient.AddPhysicalObject.CreateMessage(toAdd));
                                    //Log.Info( "Told client to add " + toAdd.ObjectInstanceID + "." );
                                }
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

        public MobileAvatar LoadMobile(int instanceId)
        {
            Schema.ObjectInstanceRow rpr = Global.ModelSchema.ObjectInstance.FindByObjectInstanceID(instanceId);
            if (rpr == null)
                return null;
            Schema.TemplateObjectRow por = Global.ModelSchema.TemplateObject.FindByTemplateObjectID(rpr.TemplateObjectID);
            if (por == null)
                return null;
            Schema.TemplateMobileRow mr = Global.ModelSchema.TemplateMobile.FindByTemplateObjectID(rpr.TemplateObjectID);
            if (mr == null)
                return null;
            return new MobileAvatar(this, mr, por, rpr);
        }

        public bool UserLookup(string email, string password, ref int playerId)
        {
            //Strive.Data.MultiverseFactory.refreshPlayerList(Global.modelSchema);
            DataRow[] dr = Global.ModelSchema.Player.Select("Email = '" + email + "'");
            if (dr.Length != 1)
            {
                _log.Error(dr.Length + " players found with email '" + email + "'.");
                return false;
            }
            if (String.Compare((string)dr[0]["Password"], password) == 0)
            {
                playerId = (int)dr[0]["PlayerID"];
                return true;
            }
            _log.Info("Incorrect password for player with email '" + email + "'.");
            return false;
        }

        public void InformNearby(PhysicalObject po, IMessage message)
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
                    // check that neigbour exists
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
            var mob = (MobileAvatar)client.Avatar;
            client.Send(Weather);

            // TODO: zomg I don't know if this divtruncate is right
            int squareX = DivTruncate((int)(mob.Position.X - _lowX), Square.SquareSize);
            int squareZ = DivTruncate((int)(mob.Position.Z - _lowZ), Square.SquareSize);
            int i, j;
            var nearbyPhysicalObjects = new List<PhysicalObject>();
            // TODO: use xorder, zorder to descide the resolution?
            for (i = -1; i <= 1; i++)
            {
                for (j = -1; j <= 1; j++)
                {
                    // check that neigbour exists
                    if (squareX + i < 0 || squareX + i >= _squaresInX
                        || squareZ + j < 0 || squareZ + j >= _squaresInZ
                        || _square[squareX + i, squareZ + j] == null)
                        continue;

                    // add all neighbouring physical objects
                    // to the clients world view
                    // that are in scope

                    /** TODO:
     * could only send based upon a radius, but this makes
     * relocations harder... maybe be better to just use squares
    // NB: using Manhattan distance not Cartesian
    float distx = Math.Abs(p.Position.X - mob.Position.X);
    float distz = Math.Abs(p.Position.Z - mob.Position.Z);
    if ( distx <= Constants.objectScopeRadius && distz <= Constants.objectScopeRadius )
    */

                    nearbyPhysicalObjects.AddRange(
                        _square[squareX + i, squareZ + j].PhysicalObjects);
                }
            }
            /*
                ToClient.AddPhysicalObjects message = new ToClient.AddPhysicalObjects(
                    nearbyPhysicalObjects
                );
                client.Send( message );
                */
            foreach (PhysicalObject p in nearbyPhysicalObjects)
                client.Send(ToClient.AddPhysicalObject.CreateMessage(p));

            for (int k = 0; k < Constants.TerrainZoomOrder; k++)
            {
                int tbx = DivTruncate((int)mob.Position.X, Constants.TerrainPieceSize) - Constants.xRadius[k];
                int tbz = DivTruncate((int)mob.Position.Z, Constants.TerrainPieceSize) - Constants.zRadius[k];

                // Normalise to a 'grid' point
                tbx = DivTruncate(tbx, Constants.scale[k]) * Constants.scale[k];
                tbz = DivTruncate(tbz, Constants.scale[k]) * Constants.scale[k];

                for (i = 0; i <= Constants.xRadius[k] * 2; i += Constants.scale[k])
                {
                    for (j = 0; j <= Constants.zRadius[k] * 2; j += Constants.scale[k])
                    {
                        int tx = tbx + i;
                        int tz = tbz + j;
                        int terrainX = tx - (int)_lowX / Constants.TerrainPieceSize;
                        int terrainZ = tz - (int)_lowZ / Constants.TerrainPieceSize;
                        if (terrainX >= 0 && terrainX < _squaresInX * Square.SquareSize / Constants.TerrainPieceSize && terrainZ >= 0 && terrainZ < _squaresInZ * Square.SquareSize / Constants.TerrainPieceSize)
                        {
                            Terrain t = _terrain[terrainX, terrainZ];
                            if (t != null)
                            {
                                if (// there is no higher zoom order
                                    k == (Constants.TerrainZoomOrder - 1)
                                    // this is not a higher order point
                                    || (tx % Constants.scale[k + 1]) != 0 || (tz % Constants.scale[k + 1]) != 0)
                                    client.Send(ToClient.AddPhysicalObject.CreateMessage(t));
                            }
                        }
                    }
                }
            }
        }

        public Tuple<int, string>[] GetPossessable(string username)
        {
            DataRow[] dr = Global.ModelSchema.Player.Select("Email = '" + username + "'");
            Schema.PlayerRow pr = Global.ModelSchema.Player.FindByPlayerID((int)dr[0][0]);
            Schema.MobilePossesableByPlayerRow[] mpbpr = pr.GetMobilePossesableByPlayerRows();
            return mpbpr.Select(mpr => new Tuple<int, string>(
                mpr.ObjectInstanceID, mpr.ObjectInstanceRow.TemplateObjectRow.TemplateObjectName)).ToArray();
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
                // which touch the altitude points of 4 neighbouring terrain
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

        public void CreateDefaultWorld()
        {
            Global.ModelSchema = new Schema();
            Global.ModelSchema.World.AddWorldRow(_worldId, "Empty", "An empty world");
            var p = Global.ModelSchema.Player.AddPlayerRow(
                "Bob", 35, "Bob", "Smith", "bob@smith.com", 1, "bob",
                100, "This is Bob", -1, new Guid(), Global.Now, Global.Now);
        }

        // handle negative numbers
        static public int DivTruncate(int x, int y)
        {
            return (x / y - ((x < 0 && (x % y != 0)) ? 1 : 0));
        }
    }
}
