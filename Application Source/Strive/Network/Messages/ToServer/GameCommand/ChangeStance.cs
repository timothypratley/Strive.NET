using System;

namespace Strive.Network.Messages.ToServer.GameCommand
{
	/// <summary>
	/// Summary description for Animation.
	/// </summary>
	[Serializable]
	public class ChangeStance : IMessage	{
		public ChangeStance( int StanceID )	{
			this.StanceID = StanceID;	
		}
		public int StanceID;

		public enum Stances {
			// descriptive stances
			NoStance,
			Passive,
			Agressive,
			Evasive,

			// fighting stances
			Terrain,
			Patient,
			Direct,
			Circle,
			TheOtherOneIForgot,
		}
	}
}
