namespace Strive.Network.Messages.ToClient
{
    public class Weather : IMessage
    {
        public int SkyTextureId;
        public int Lighting;
        public int Fog;
        public int Rain;

        public Weather(int skyTextureId, int lighting, int fog, int rain)
        {
            SkyTextureId = skyTextureId;
            Lighting = lighting;
            Fog = fog;
            Rain = rain;
        }
    }
}
