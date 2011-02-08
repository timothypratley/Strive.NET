namespace Strive.Network.Messages.ToClient
{
	public class Beat : IMessage
	{
		public int BeatNumber;

		public Beat(){}
		public Beat(int beatNumber)
		{
			BeatNumber = beatNumber;
		}
	}
}
