using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.FSharp.Collections;
using System.Diagnostics.Contracts;

namespace Strive.Model
{
    // TODO: remove, unnecessary
    public class PartyModel
    {
        public PartyModel(IEnumerable<string> users, IEnumerable<int> entities, string leader)
        {
            Users = new FSharpSet<string>(users);
            Entities = new FSharpSet<int>(entities);
            Leader = leader;
        }

        FSharpSet<string> Users { get; protected set; }
        FSharpSet<int> Entities { get; protected set; }
        string Leader { get; protected set; }

        public PartyModel WithMember(int Id)
        {
            var r = (PartyModel)MemberwiseClone();
            r.Entities = r.Entities.Add(Id);
            return r;
        }

        public PartyModel WithLeader(string user)
        {
            Contract.Requires<ArgumentException>(Users.Contains(user));

            var r = (PartyModel)MemberwiseClone();
            r.Leader = user;
            return r;
        }

        public PartyModel WithoutEntity(int Id)
        {
            var r = (PartyModel)MemberwiseClone();
            r.Entities = r.Entities.Remove(Id);
            return r;
        }

        public PartyModel WithoutUser(string user)
        {
            Contract.Requires<ArgumentException>(Leader != user);

            var r = (PartyModel)MemberwiseClone();
            r.Users = r.Users.Remove(user);
            return r;
        }

        [ContractInvariantMethod]
        protected void ObjectInvariant() {
            Contract.Invariant(Users.Contains(Leader));
        }
    }
}
