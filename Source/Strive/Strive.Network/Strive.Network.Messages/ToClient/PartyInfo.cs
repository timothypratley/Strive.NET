namespace Strive.Network.Messages.ToClient
{
    public class PartyInfo
    {
        public int[] MobileId;
        public string[] MobileName;

        // TODO: prefer tuples or classes? or something?
        public PartyInfo(int[] mobileId, string[] mobileName)
        {
            MobileId = mobileId;
            MobileName = mobileName;
        }
    }
}
