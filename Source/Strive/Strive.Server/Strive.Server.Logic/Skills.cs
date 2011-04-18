using System;
using Strive.Network.Server;
using Strive.Multiverse;

namespace Strive.Server.Logic
{
	public class Skills
	{
		public static void Backstab( Client client, Mobile target ) {
			Log.LogMessage( client.Avatar.ObjectTemplateName + " backstabs "+ target.ObjectTemplateName + "." );
		}
	}
}
