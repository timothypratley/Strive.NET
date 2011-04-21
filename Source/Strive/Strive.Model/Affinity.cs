
namespace Strive.Model
{
    public class Affinity
    {
        public Affinity(float air, float earth, float fire, float life, float water)
        {
            Air = air;
            Earth = earth;
            Fire = fire;
            Life = life;
            Water = water;
        }

        public float Air { get; private set; }
        public float Earth { get; private set; }
        public float Fire { get; private set; }
        public float Life { get; private set; }
        public float Water { get; private set; }
    }
}
