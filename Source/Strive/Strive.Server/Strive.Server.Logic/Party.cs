using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Strive.Network.Messages;
using Strive.Model;

namespace Strive.Server.Logic
{
    public class Party
    {
        readonly Dictionary<int, CombatantModel> _members = new Dictionary<int, CombatantModel>();

        public Party(string name, CombatantModel leader)
        {
            Contract.Requires<ArgumentNullException>(leader != null);
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(name));

            Name = name;
            Leader = leader;
        }

        public void Add(CombatantModel member)
        {
            _members.Add(member.Id, member);
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

        public IEnumerable<CombatantModel> GetMembers()
        {
            return _members.Values;
        }

        public CombatantModel Leader { get; set; }

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
            foreach (var ma in _members.Values)
            {
                // TODO: lookup the client
                //ma.Client.Send(new Network.Messages.ToClient.Communication(sender, message, CommunicationType.PartyTalk));
            }
        }
    }
}
