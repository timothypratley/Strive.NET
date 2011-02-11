using System;
using System.Windows.Media.Media3D;
using Strive.Client.Model;
using Strive.Network.Messages;
using Strive.Common;
using Strive.Network.Messages.ToServer;
using Strive.Network.Messages.ToClient;

namespace Strive.Network.Messaging
{
    public class ServerConnection : Connection
    {
        public DictionaryModel<string, EntityModel> WorldModel { get; private set; }

        public ServerConnection()
        {
            WorldModel = new DictionaryModel<string, EntityModel>(); 
            MessageRecieved += ConnectionMessageRecieved;
        }

        #region message handling

        void ConnectionMessageRecieved(object sender, EventArgs e)
        {
            dynamic m = PopNextMessage();
            Log.Trace("Processing " + m.GetType() + " message: " + m);
            Process(m);
        }

        void Process(AddTerrain m)
        {
            Log.Trace("foo");
        }

        void Process(Position m)
        {
            Log.Trace("bar");
            EntityModel e = WorldModel.EntityDictionary[m.instance_id.ToString()];
            e.Position = m.position;
            e.Rotation = m.rotation;
        }

        #endregion

        #region message sending

        public void Chat(string channel, string message)
        {
            Send(new Communicate(channel, CommunicationType.Chat, message));
        }

        public void PossessMobile(int mobileId)
        {
            Send(new EnterWorldAsMobile(mobileId));
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
            Send(new SkillList());
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

        public void MyPosition(Vector3D position, Quaternion rotation)
        {
            Send(new MyPosition(position, rotation));
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
