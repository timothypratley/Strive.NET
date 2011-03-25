using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Media.Media3D;
using Microsoft.CSharp.RuntimeBinder;
using Strive.Client.Model;
using Strive.Common;
using Strive.Network.Messages;
using Strive.Network.Messages.ToClient;
using Strive.Network.Messages.ToServer;


namespace Strive.Network.Messaging
{
    public class ServerConnection : Connection
    {
        public WorldModel WorldModel { get; private set; }
        public TraceListenerCollection ChatListeners;

        public ServerConnection()
        {
            WorldModel = new WorldModel();
            MessageRecieved += ConnectionMessageRecieved;

            // I wish there were a public constructor for TraceListenerCollection, but there is not
            ConstructorInfo constructor = typeof(TraceListenerCollection).GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { }, null);
            ChatListeners = (TraceListenerCollection)constructor.Invoke(new object[0] { });
        }

        #region message handling

        void ConnectionMessageRecieved(object sender, EventArgs e)
        {
            dynamic m = PopNextMessage();
            Log.Trace("Processing " + m.GetType() + " message: " + m);
            try
            {
                Process(m);
            }
            catch (RuntimeBinderException)
            {
                Log.Error("Received unknown message type " + m);
            }
            catch (Exception ex)
            {
                Log.Error("Failed to process message " + m, ex);
            }
        }

        void Process(TimeAndWeather m)
        {
            Log.Info("Received Time and Weather " + m.ServerNow);
        }

        void Process(AddMobile m)
        {
            WorldModel.Set(new EntityModel(m.Mobile.ObjectInstanceId.ToString(),
                                           "RTSRobot",
                                           m.Mobile.Position,
                                           m.Mobile.Rotation));
        }

        void Process(AddJunk m)
        {
            WorldModel.Set(new EntityModel(m.Junk.ObjectInstanceId.ToString(),
                                           "StaticBox",
                                           m.Junk.Position,
                                           m.Junk.Rotation));
        }

        void Process(AddTerrain m)
        {
        }

        void Process(PositionUpdate m)
        {
            EntityModel e = WorldModel.Get(m.InstanceId.ToString());
            WorldModel.Set(new EntityModel(e.Name, e.ModelId, m.Position, m.Rotation));
        }

        void Process(MobileState m)
        {
            //EntityModel e = WorldModel.Get(m.ObjectInstanceId.ToString());
            //WorldModel.Set(new EntityModel(e.Name, e.ModelId, m.Position, m.Rotation, m.State));
        }

        void Process(Communication m)
        {
            foreach (TraceListener listener in ChatListeners)
                listener.WriteLine("[" + m.CommunicationType + "] " + m.Name + ": " + m.Message);
        }

        #endregion


        #region message sending

        public void Chat(string channel, string message)
        {
            Send(new Communicate(channel, CommunicationType.Chat, message));
        }

        public void PossessMobile(int mobileId)
        {
            Send(new PossessMobile(mobileId));
        }

        public void CreatePhysicalObject(int templateId, Vector3D position, Quaternion rotation)
        {
            Send(new CreatePhysicalObject(templateId, position, rotation));
        }

        public void Login(string username, string password)
        {
            Send(new Login(username, password));
        }

        public void Logout()
        {
            Send(new Logout());
        }

        public void SkillList()
        {
            Send(new RequestSkillList());
        }

        public void WhoList()
        {
            Send(new RequestWhoList());
        }

        public void UseSkill(EnumSkill skill, int invokationId)
        {
            Send(new UseSkill(skill, invokationId));
        }

        public void UseSkill(EnumSkill skill, int invokationId, int[] targets)
        {
            Send(new UseSkill(skill, invokationId, targets));
        }

        public void UseSkill(int skillId, int invokationId)
        {
            UseSkill((EnumSkill)skillId, invokationId);
        }

        public void UseSkill(int skillId, int invokationId, int[] targets)
        {
            UseSkill((EnumSkill)skillId, invokationId, targets);
        }

        public void MyPosition(int possessingId, Vector3D position, Quaternion rotation, EnumMobileState state)
        {
            Send(new MyPosition(possessingId, position, rotation, state));
        }

        public void RequestPossessable()
        {
            Send(new RequestPossessable());
        }

        public void Pong(int sequenceNumber)
        {
            Send(new Pong(sequenceNumber));
        }

        public void ReloadWorld()
        {
            Send(new ReloadWorld());
        }

        #endregion
    }
}
