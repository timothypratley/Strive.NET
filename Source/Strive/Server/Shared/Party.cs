using System;
using System.Collections;

using Strive.Multiverse;

namespace Strive.Server.Shared {
	/// <summary>
	/// Summary description for Party.
	/// </summary>
	public class Party {
		Hashtable _members = new Hashtable();
		string _name;
		Mobile _leader;

		public Party( string name, Mobile leader ) {
			_name = name;
			_leader = leader;
		}

		public void Add( Mobile m ) {
			_members.Add( m.ObjectInstanceID, m );
		}

		public void Remove( int ObjectInstanceID ) {
			_members.Remove( ObjectInstanceID );
		}

		public ICollection GetMembers() {
			return _members.Values;
		}

		public Mobile Leader {
			get { return _leader; }
			set { _leader = value; }
		}
	}
}
