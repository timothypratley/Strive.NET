using System;
using System.Collections.Generic;
using System.Windows.Media.Media3D;
using Microsoft.CSharp.RuntimeBinder;
using Strive.Client.Model;
using Strive.Network.Messages;
using Strive.Common;
using Strive.Network.Messages.ToServer;
using Strive.Network.Messages.ToClient;


namespace Strive.Network.Messaging
{
    public class ServerConnection : Connection
    {
        public readonly PersistentTreeMapModel<int, PersistentTreeMapModel<string, EntityModel>> History
            = new PersistentTreeMapModel<int, PersistentTreeMapModel<string, EntityModel>>();

        private int _worldVersion = 1;
        public PersistentTreeMapModel<string, EntityModel> WorldModel
        {
            get { return History.Last(); }
            set { History.Set(_worldVersion++, value); }
        }

        public ServerConnection()
        {
            WorldModel = new PersistentTreeMapModel<string, EntityModel>(); 
            MessageRecieved += ConnectionMessageRecieved;
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
            Log.Info("Recieved Time and Weather " + m.ServerNow);
        }

        void Process(AddMobile m)
        {
            Log.Info("Recieved message" + m);
            WorldModel.Set(m.Mobile.ObjectInstanceId.ToString(),
                                 new EntityModel(m.Mobile.ObjectInstanceId.ToString(),
                                                 "Robot",
                                                 m.Mobile.Position,
                                                 m.Mobile.Rotation));
        }

        void Process(AddTerrain m)
        {
            Log.Trace("Terrain " + m);
        }

        void Process(PositionUpdate m)
        {
            Log.Trace("bar");
            EntityModel e = WorldModel.GetEntity(m.InstanceId.ToString());
            e.Position = m.Position;
            e.Rotation = m.Rotation;
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

        public void CreateMobile(int templateId, Vector3D position, Quaternion rotation)
        {
            Send(new CreateMobile(templateId, position, rotation));
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

        public void MyPosition(int possessingId, Vector3D position, Quaternion rotation)
        {
            Send(new MyPosition(possessingId, position, rotation));
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
