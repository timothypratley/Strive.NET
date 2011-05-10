using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Media.Media3D;
using Microsoft.CSharp.RuntimeBinder;
using Strive.Common;
using Strive.Model;
using Strive.Network.Messages;
using Strive.Network.Messages.ToClient;
using Strive.Network.Messages.ToServer;


namespace Strive.Network.Messaging
{
    public class ServerConnection : Connection
    {
        public ServerConnection()
        {
        }

        #region message sending

        public void Chat(string channel, string message)
        {
            Send(new Communicate(channel, CommunicationType.Chat, message));
        }

        public void PossessMobile(int mobileId)
        {
            Send(new PossessMobile(mobileId));
        }

        public void CreateEntity(int id, string name, string modelId, Vector3D position, Quaternion rotation)
        {
            Send(new EntityModel(id, name, modelId, position, rotation, 100, 100, EnumMobileState.Standing, 1.7f));
        }

        public void ProduceEntity(int id, string name, string modelId, EntityModel factory)
        {
            Send(new ProduceEntity(id, name, modelId, factory.Id));
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
