using System;
using System.Collections.Generic;
using System.Linq;
using Strive.Common;
using Strive.Model;
using Strive.Server.DB;


namespace Strive.Server.Logic
{
    public partial class World
    {
        public bool UserLookup(string email, string password, ref int playerId)
        {
            // TODO: have disabled password checking for testing purposes
            return !string.IsNullOrEmpty(email);

            /*
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
             */
        }

        public Avatar LoadMobile(int instanceId)
        {
            Schema.ObjectInstanceRow instance = Global.Schema.ObjectInstance.FindByObjectInstanceID(instanceId);
            if (instance == null)
                return null;
            Schema.TemplateObjectRow template = Global.Schema.TemplateObject.FindByTemplateObjectID(instance.TemplateObjectID);
            if (template == null)
                return null;
            Schema.TemplateMobileRow mobile = Global.Schema.TemplateMobile.FindByTemplateObjectID(instance.TemplateObjectID);
            if (mobile == null)
                return null;
            return new Avatar(this, instance, template, mobile);
        }

        public void CreateDefaultWorld()
        {
            Global.Schema = new Schema();
            Global.Schema.World.AddWorldRow(_worldId, "Empty", "An empty world");
            var p = Global.Schema.Player.AddPlayerRow(
                "Bob", 35, "Bob", "Smith", "bob@smith.com", 1, "bob",
                100, "This is Bob", -1, new Guid(), Global.Now, Global.Now);
        }

        public class InvalidWorld : Exception { }
        public void Load()
        {
            PhysicalObjects = new Dictionary<int, EntityModel>();
            Mobiles = new List<Avatar>();

            // TODO: would be nice to be able to load only the world in question... but for now load them all
            if (Global.WorldFilename != null)
            {
                _log.Info("Loading Global.modelSchema from file:" + Global.WorldFilename);
                Global.Schema = new Schema();
                Global.Schema.ReadXml(Global.WorldFilename);
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
            foreach (Schema.ObjectInstanceRow r in Global.Schema.ObjectInstance.Rows)
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
            _terrain = new TerrainModel[_squaresInX * Square.SquareSize / Constants.TerrainPieceSize, _squaresInZ * Square.SquareSize / Constants.TerrainPieceSize];

            Schema.WorldRow wr = Global.Schema.World.FindByWorldID(_worldId);
            if (wr == null)
            {
                throw new InvalidWorld();
            }

            /** TODO: support loading
            _log.Info("Loading world \"" + wr.WorldName + "\"...");
            _log.Info("Loading terrain...");
            foreach (Schema.TemplateTerrainRow ttr in Global.Schema.TemplateTerrain.Rows)
            {
                foreach (Schema.ObjectInstanceRow oir in ttr.TemplateObjectRow.GetObjectInstanceRows())
                    Add(new Terrain(ttr, ttr.TemplateObjectRow, oir));
            }
            _log.Info("Loading physical objects...");
            foreach (Schema.TemplateObjectRow otr in Global.Schema.TemplateObject.Rows)
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
             */
        }
    }
}
