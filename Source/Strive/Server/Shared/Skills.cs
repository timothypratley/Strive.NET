using System;
using Strive.Network.Server;
using Strive.Multiverse;

namespace Strive.Server.Shared
{
	/// <summary>
	/// Summary description for Skills.
	/// </summary>
	public class Skills
	{
		public static void Backstab( Client client, Mobile target ) {
			Global.log.LogMessage( client.Avatar.ObjectTemplateName + " backstabs "+ target.ObjectTemplateName + "." );
		}
	}
}
