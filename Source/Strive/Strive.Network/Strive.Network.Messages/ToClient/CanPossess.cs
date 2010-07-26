using System;

namespace Strive.Network.Messages.ToClient
{
	/// <summary>
	/// Summary description for CanPossess.
	/// </summary>
	public class CanPossess : IMessage {

		public class id_name_tuple {
			public id_name_tuple(){}
			public id_name_tuple( int id, string name ) {
				this.id = id;
				this.name = name;
			}
			public int id;
			public string name;
		}

		public id_name_tuple [] possesable;

		public CanPossess(){}
		public CanPossess( id_name_tuple [] possesable ) {
			this.possesable = possesable;
		}
	}
}
