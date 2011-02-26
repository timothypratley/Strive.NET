using System.Collections.Generic;
using System.Linq;
using System.Diagnostics.Contracts;
using System;
using Strive.Network.Messages;

namespace Strive.Server.Logic
{
    public class Party
    {
        readonly Dictionary<int, MobileAvatar> _members = new Dictionary<int, MobileAvatar>();

        public Party(string name, MobileAvatar leader)
        {
            Contract.Requires<ArgumentNullException>(leader != null);
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(name));

            Name = name;
            Leader = leader;
        }

        public void Add(MobileAvatar m)
        {
            _members.Add(m.ObjectInstanceId, m);
        }

        public void Remove(int objectInstanceId)
        {
            bool wasLeader = _members[objectInstanceId] == Leader;

            // find a new leader
            _members.Remove(objectInstanceId);
            if (wasLeader)
                Leader = _members.Values.OrderBy(ma => ma.Level).FirstOrDefault();
        }

        public IEnumerable<MobileAvatar> GetMembers()
        {
            return _members.Values;
        }

        public MobileAvatar Leader { get; set; }

        public string Name { get; private set; }

        /*** better to use PartyTalk for everything?
        public void SendLog( string message ) {
            foreach ( MobileAvatar ma in _members.Values ) {
                ma.SendLog( message );
            }
        }
        */

        public void SendPartyTalk(string message)
        {
            SendPartyTalk(Name, message);
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
