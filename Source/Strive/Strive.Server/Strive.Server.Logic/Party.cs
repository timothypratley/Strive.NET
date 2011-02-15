using System.Collections;
using System.Linq;
using Strive.Network.Messages;

namespace Strive.Server.Logic
{
    public class Party
    {
        readonly Hashtable _members = new Hashtable();
        MobileAvatar _leader;

        public Party(string name, MobileAvatar leader)
        {
            Name = name;
            _leader = leader;
        }

        public void Add(MobileAvatar m)
        {
            _members.Add(m.ObjectInstanceId, m);
        }

        public void Remove(int objectInstanceId)
        {
            if (_members[objectInstanceId] == _leader)
            {
                // find a new leader
                _leader = null;
                foreach (MobileAvatar ma in _members.Values
                    .Cast<MobileAvatar>().Where(ma => _leader == null || ma.Level > _leader.Level))
                {
                    _leader = ma;
                }
            }
            _members.Remove(objectInstanceId);
        }

        public ICollection GetMembers()
        {
            return _members.Values;
        }

        public MobileAvatar Leader { get; set; }

        public string Name { get; set; }

        /*** better to use PartyTalk for everything?
        public void SendLog( string message ) {
            foreach ( MobileAvatar ma in _members.Values ) {
                ma.SendLog( message );
            }
        }
        */

        public void SendPartyTalk(string message)
        {
            SendPartyTalk("", message);
        }

        public void SendPartyTalk(string sender, string message)
        {
            foreach (MobileAvatar ma in _members.Values)
            {
                ma.Client.Send(new Network.Messages.ToClient.Communication(sender, message, CommunicationType.PartyTalk));
            }
        }
    }
}
