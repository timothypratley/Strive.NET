using System;

using Common.Logging;
using Strive.Network.Messages;
using ToClient = Strive.Network.Messages.ToClient;
using ToServer = Strive.Network.Messages.ToServer;
using Strive.Math3D;
using Strive.Server.Model;


namespace Strive.Client.WinForms.Engine
{
    public class MessageProcessor
    {
        public delegate void CanPossessHandler(ToClient.CanPossess message);
        public delegate void SkillListHandler(ToClient.SkillList message);
        public delegate void WhoListHandler(ToClient.WhoList message);
        public delegate void ChatHandler(ToClient.Communication message);

        public event CanPossessHandler CanPossess;
        public event SkillListHandler SkillList;
        public event WhoListHandler WhoList;
        public event ChatHandler Chat;

        ILog Log = LogManager.GetCurrentClassLogger();

        public MessageProcessor()
        {
        }

        public void Process(IMessage m)
        {
            #region Communication Message
            if (m is ToClient.Communication)
            {
                ToClient.Communication c = (ToClient.Communication)m;
                if (c.communicationType == CommunicationType.Chat)
                {
                    Chat(c);
                }
                else
                {
                    Log.Error("Bad communicationType " + c.communicationType);
                }
            }
            #endregion
            #region AddPhysicalObject Message
            else if (m is ToClient.AddPhysicalObject)
            {
                // TODO: should probabbly treat terrain differently
                PhysicalObject po = ToClient.AddPhysicalObject.GetPhysicalObject((ToClient.AddPhysicalObject)m);
                //po.Position.X = (float)Math.Round(po.Position.X * 8F / 10F);
                //po.Position.Y = (float)Math.Round(po.Position.Y * 8F / 10F);
                //po.Position.Z = (float)Math.Round(po.Position.Z * 8F / 10F);
                /*
                PhysicalObjectInstance poi = (PhysicalObjectInstance)_world.physicalObjectInstances[po.ObjectInstanceID];
                if (poi != null)
                {
                    // replace an existing physical object
                    poi.physicalObject = po;
                }
                else
                {
                    // add a new one
                    poi = _world.Add(po);
                }
                if (po.ObjectInstanceID == Game.CurrentPlayerID)
                {
                    // current player gets followed by camera etc.
                    _world.Possess(Game.CurrentPlayerID);
                    Log.Info("Initial position is " + po.Position);
                    Log.Info("Initial heading is " + Helper.GetHeadingFromRotation(po.Rotation));
                }
                else
                {
                    //Log.Info( "Added object " + po.ObjectInstanceID + " with model " + po.ModelID + " at " + po.Position );
                }
                if (poi != null && po is Mobile)
                {
                    SetMobileState(((Mobile)po).MobileState, (IActor)poi.model);
                }
                 */
            }
            #endregion
            #region AddTerrainCollection Message
            // TODO: unused atm, unnesessary?
            else if (m is ToClient.AddTerrainCollection)
            {
                ToClient.AddTerrainCollection atc = (ToClient.AddTerrainCollection)m;
                //_world.TerrainPieces.AddMany(atc.startX, atc.startZ, atc.width, atc.height, atc.gap_size, atc.map);
            }
            #endregion
            #region Position Message

            else if (m is ToClient.Position)
            {
                ToClient.Position p = (ToClient.Position)m;
                /*
                PhysicalObjectInstance poi = _world.Find(p.instance_id);
                if (poi == null)
                {
                    Log.Error("Model for " + p.instance_id + " has not been loaded");
                    return;
                }

                poi.model.Position = p.position;
                poi.model.Rotation = p.rotation;
                if (poi == _world.CurrentAvatar)
                {
                    _world.RepositionCamera();
                }
                //				Log.Info( "Position message applied to " + p.instance_id + " rotation " + Rotation );
                 */
            }
            #endregion
            #region CombatReport Message
            else if (m is ToClient.CombatReport)
            {
                ToClient.CombatReport cr = m as ToClient.CombatReport;
                switch (cr.combat_event)
                {
                    case EnumCombatEvent.Attacks:
                        Log.Info(
                            cr.attackerObjectInstanceID.ToString() + " attacks " + cr.targetObjectInstanceID.ToString() + "!");
                        break;
                    case EnumCombatEvent.Avoids:
                        Log.Info(
                            cr.targetObjectInstanceID.ToString() + " avoids " + cr.attackerObjectInstanceID.ToString() + ".");
                        break;
                    case EnumCombatEvent.Hits:
                        Log.Info(
                            cr.attackerObjectInstanceID.ToString() + " hits " + cr.targetObjectInstanceID.ToString() + " for " + cr.damage + " damage.");
                        break;
                    case EnumCombatEvent.Misses:
                        Log.Info(
                            cr.attackerObjectInstanceID.ToString() + " misses " + cr.targetObjectInstanceID.ToString() + ".");
                        break;
                    default:
                        Log.Error("Unknown CombatEvent " + cr.combat_event);
                        break;
                }
            }
            #endregion
            #region DropPhysicalObject Message
            else if (m is ToClient.DropPhysicalObject)
            {
                ToClient.DropPhysicalObject dpo = (ToClient.DropPhysicalObject)m;
                //_world.Remove(dpo.instance_id);
                //Log.Info( "Removed "+ dpo.instance_id.ToString() );
            }
            #endregion
            #region MobileState
            else if (m is ToClient.MobileState)
            {
                ToClient.MobileState ms = (ToClient.MobileState)m;
                
                //				Log.Info( "Mobile " + ms.ObjectInstanceID + " is " + ms.State );
                /*
                PhysicalObjectInstance poi = _world.Find(ms.ObjectInstanceID);

                #region 1.1.1 Check that the model exists
                if (poi == null || poi.model == null || !(poi.model is IActor))
                {
                    Log.Error("Actor for " + ms.ObjectInstanceID + " has not been loaded");
                    return;
                }
                #endregion

                IActor actor = (IActor)poi.model;
                SetMobileState(ms.State, actor);
                */
                //TODO: evaluate whether just to ignore postion for oneself
                //poi.model.Position = ms.position;
            }
            #endregion
            #region CurrentHitpoints
            else if (m is ToClient.CurrentHitpoints)
            {
                ToClient.CurrentHitpoints chp = (ToClient.CurrentHitpoints)m;
                Log.Info("You now have " + chp.HitPoints);
            }
            #endregion
            #region CanPossess
            else if (m is ToClient.CanPossess)
            {
                ToClient.CanPossess cp = (ToClient.CanPossess)m;
                CanPossess(cp);
            }
            #endregion
            #region DropAll
            else if (m is ToClient.DropAll)
            {
                ToClient.DropAll da = (ToClient.DropAll)m;
                Log.Info("DropAll recieved");
                //_world.Clear();
            }
            #endregion
            #region Weather
            else if (m is ToClient.TimeAndWeather)
            {
                ToClient.TimeAndWeather w = (ToClient.TimeAndWeather)m;
                //Log.Info( "Weather update recieved" );
                /*
                ITexture td = Game.resources.GetTexture(w.DaySkyTextureID);
                ITexture tn = Game.resources.GetTexture(w.NightSkyTextureID);
                ITexture tc = Game.resources.GetTexture(w.CuspSkyTextureID);
                ITexture ts = Game.resources.GetTexture(w.SunTextureID);


                // TODO: don't hardcode the clouds textureid
                ITexture ct = Game.resources.GetTexture(46);
                _world.SetSky(td, tn, tc, ts);
                _world.SetClouds(ct);
                DateTime dt = new DateTime(w.ServerNow);
                _world.SetTime(dt);
                 */
            }
            #endregion
            #region Ping
            else if (m is ToClient.Ping)
            {
                ToClient.Ping p = (ToClient.Ping)m;
                Game.CurrentServerConnection.Pong(p.SequenceNumber);
            }
            #endregion
            #region SkillList
            else if (m is ToClient.SkillList)
            {
                ToClient.SkillList sl = (ToClient.SkillList)m;
                Log.Info("SkillList recieved");
                SkillList(sl);
            }
            #endregion
            #region WhoList
            else if (m is ToClient.WhoList)
            {
                ToClient.WhoList wl = (ToClient.WhoList)m;
                WhoList(wl);
            }
            #endregion
            #region PartyInfo
            else if (m is ToClient.PartyInfo)
            {
                ToClient.PartyInfo pi = (ToClient.PartyInfo)m;
                Log.Info("PartyInfo received: " + pi);
            }
            #endregion
            #region LogMessage
            else if (m is ToClient.LogMessage)
            {
                ToClient.LogMessage logmessage = (ToClient.LogMessage)m;
                Log.Info(logmessage.Message);
            }
            #endregion
            #region Default
            else
            {
                Log.Error("Unknown message of type " + m.GetType());
            }
            #endregion
        }

    }
}
