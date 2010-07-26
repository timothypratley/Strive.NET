using System;

namespace Strive.Network.Messages
{
	/// <summary>
	/// Summary description for IMessage.
	/// </summary>
	public interface IMessage
	{
		// need a default constructor so that the deserializer
		// can construct and empty object and fill out the fields
		//public IMessage(){}
	}
}
