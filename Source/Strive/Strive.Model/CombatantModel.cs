using System.Windows.Media.Media3D;
using Strive.Common;

namespace Strive.Model
{
    public class CombatantModel : EntityModel
    {
        public CombatantModel(int id, string name, string modelId, Vector3D position, Quaternion rotation,
            float health, float energy, EnumMobileState mobileState, float height,
            int constitution, int dexterity, int willpower, int strength)
            : base(id, name, modelId, position, rotation, health, energy, mobileState, height)
        {
            Constitution = constitution;
            Dexterity = dexterity;
            Willpower = willpower;
            Strength = strength;
        }

        public int Constitution { get; private set; }
        public int Dexterity { get; private set; }
        public int Willpower { get; private set; }
        public int Strength { get; private set; }
    }
}
