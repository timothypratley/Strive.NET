using System;
using Strive.Network.Server;
using Strive.Multiverse;

namespace Strive.Server
{
	/// <summary>
	/// Summary description for Skills.
	/// </summary>
	public class Skills
	{
		public static void Backstab( Client client, Mobile target ) {
			System.Console.WriteLine( client.Avatar.physicalObject.PhysicalObjectName + " backstabs "+ target.physicalObject.PhysicalObjectName );
		}
	}
}
