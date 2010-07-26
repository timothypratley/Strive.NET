using System;
using Strive.Server.Model;

namespace Strive.Network.Messages.ToClient
{
	/// <summary>
	/// Summary description for Acknowledge.
	/// </summary>
	[Serializable]
	public class NegativeAcknowledge : IMessage 
	{
		public NegativeAcknowledge(){}
	}
}
