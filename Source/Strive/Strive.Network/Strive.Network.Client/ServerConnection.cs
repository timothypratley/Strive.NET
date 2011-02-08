using System.Windows.Media.Media3D;
using Strive.Network.Messages;
using Strive.Network.Messages.ToServer;
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

        public void Position(Vector3D position, Quaternion rotation)
        {
            Send(new Position(position, rotation));
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
    }
}
