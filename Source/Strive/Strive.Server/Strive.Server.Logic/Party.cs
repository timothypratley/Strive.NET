using System;
using System.Collections;

using Strive.Server.Model;

namespace Strive.Server.Logic {
	/// <summary>
	/// Summary description for 
	/// </summary>
	public class Party {
		Hashtable _members = new Hashtable();
		string _name;
		MobileAvatar _leader;

		public Party( string name, MobileAvatar leader ) {
			_name = name;
			_leader = leader;
		}

		public void Add( MobileAvatar m ) {
			_members.Add( m.ObjectInstanceID, m );
		}

		public void Remove( int ObjectInstanceID ) {
			if ( _members[ObjectInstanceID] == _leader ) {
				// find a new leader
				_leader = null;
				foreach ( MobileAvatar ma in _members.Values ) {
					if ( _leader == null || ma.Level > _leader.Level ) {
						_leader = ma;
					}
				}
			}
			_members.Remove( ObjectInstanceID );
		}

		public ICollection GetMembers() {
			return _members.Values;
		}

		public MobileAvatar Leader {
			get { return _leader; }
			set { _leader = value; }
		}

		public string Name {
			get { return _name; }
			set { _name = value; }
		}

		/*** better to use PartyTalk for everything?
		public void SendLog( string message ) {
			foreach ( MobileAvatar ma in _members.Values ) {
				ma.SendLog( message );
			}
		}
		*/

		public void SendPartyTalk( string message ) {
			SendPartyTalk( "", message );
		}

		public void SendPartyTalk( string sender, string message ) {
			foreach( MobileAvatar ma in _members.Values ) {
				ma.client.Send( new	Network.Messages.ToClient.Communication( sender, message, Strive.Network.Messages.CommunicationType.PartyTalk ) );
			}
		}
	}
}
