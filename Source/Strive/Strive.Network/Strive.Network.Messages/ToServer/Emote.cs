using Strive.Common;

namespace Strive.Network.Messages.ToServer
{
    public class Emote
    {
        public EmoteType EmoteId;
        public int MobileId;	// optional target

        public Emote(EmoteType emoteId, int mobileId)
        {
            EmoteId = emoteId;
            MobileId = mobileId;
        }
    }
}
