using System;
using System.Windows.Media.Media3D;
using Strive.Common;
using Strive.Model;
using Strive.Server.DB;


namespace Strive.Server.Logic
{
    public partial class World
    {
        public bool UserLookup(string email, string password)
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

        public CombatantModel LoadMobile(int instanceId)
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
            return new CombatantModel(instance.ObjectInstanceID, template.TemplateObjectName, template.TemplateObjectName,
                new Vector3D(instance.X, instance.Y, instance.Z),
                new Quaternion(instance.RotationX, instance.RotationY, instance.RotationZ, instance.RotationW),
                (float)instance.HealthCurrent, (float)instance.EnergyCurrent,
                (EnumMobileState)mobile.EnumMobileStateID, template.Height,
                mobile.Constitution, mobile.Dexterity, mobile.Willpower, mobile.Cognition, mobile.Strength);
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
            History.Head = WorldModel.Empty;

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

            Schema.WorldRow wr = Global.Schema.World.FindByWorldID(_worldId);
            if (wr == null)
                throw new InvalidWorld();

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
