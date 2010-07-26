using System;

namespace Strive.Network.Messages.ToServer
{
	/// <summary>
	/// Summary description for Animation.
	/// </summary>
	[Serializable]
	public class ChangeStance : IMessage	{
		public int StanceID;
		public ChangeStance(){}
		public ChangeStance( Stances StanceID )	{
			this.StanceID = (int)StanceID;	
		}

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
