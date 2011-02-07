using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Windows.Media.Media3D;

using Common.Logging;

using Strive.Network.Messages;
using Strive.Network.Messages.ToServer;
using Strive.Server.Model;
using Strive.Common;


namespace Strive.Network.Client
{
    public class ServerConnection : Connection {

        public void Chat(string message)
        {
            Send(new Communication(CommunicationType.Chat, message));
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
            Send(new WhoList());
        }

        public void UseSkill(EnumSkill Skill, int InvokationID)
        {
            Send(new UseSkill(Skill, InvokationID));
        }

        public void UseSkill(EnumSkill Skill, int InvokationID, int[] Targets)
        {
            Send(new UseSkill(Skill, InvokationID, Targets));
        }

        public void UseSkill(int SkillID, int InvokationID)
        {
            this.UseSkill((EnumSkill)SkillID, InvokationID);
        }

        public void UseSkill(int SkillID, int InvokationID, int[] Targets)
        {
            this.UseSkill((EnumSkill)SkillID, InvokationID, Targets);
        }

        public void Position(Vector3D position, Quaternion rotation)
        {
            Send(new Position(position, rotation));
        }

        public void RequestPossessable()
        {
            Send(new RequestPossessable());
        }

        public void Pong(int SequenceNumber)
        {
            Send(new Pong(SequenceNumber));
        }

        public void ReloadWorld()
        {
            Send(new ReloadWorld());
        }
    }
}
