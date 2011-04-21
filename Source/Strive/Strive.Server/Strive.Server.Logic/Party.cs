using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Strive.Network.Messages;

namespace Strive.Server.Logic
{
    public class Party
    {
        readonly Dictionary<int, Avatar> _members = new Dictionary<int, Avatar>();

        public Party(string name, Avatar leader)
        {
            Contract.Requires<ArgumentNullException>(leader != null);
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(name));

            Name = name;
            Leader = leader;
        }

        public void Add(Avatar m)
        {
            _members.Add(m.Id, m);
        }

        public void Remove(int objectInstanceId)
        {
            bool wasLeader = _members[objectInstanceId] == Leader;

            // find a new leader
            _members.Remove(objectInstanceId);
            if (wasLeader)
                Leader = _members.Values.FirstOrDefault();
            //_members.Values.OrderBy(ma => ma.Level).FirstOrDefault();
        }

        public IEnumerable<Avatar> GetMembers()
        {
            return _members.Values;
        }

        public Avatar Leader { get; set; }

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
            foreach (Avatar ma in _members.Values)
            {
                ma.Client.Send(new Network.Messages.ToClient.Communication(sender, message, CommunicationType.PartyTalk));
            }
        }
    }
}
