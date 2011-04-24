using System;
using System.Diagnostics.Contracts;
using System.Windows.Media.Media3D;
using Strive.Common;


namespace Strive.Model
{
    /// <summary>
    /// EntityModel is immutable to facilitate versioning.
    /// All changes result in a new object which is stored in the history.
    /// Helpers to transition between states are located in WorldModel.
    /// </summary>
    public class EntityModel : AModel
    {
        public EntityModel(int id, string name, string modelId, Vector3D position, Quaternion rotation,
            float health, float energy, EnumMobileState mobileState, float height)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(name));
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(modelId));
            Id = id;
            Name = name;
            ModelId = modelId;
            Position = position;
            Rotation = rotation;
        }

        public int Id { get; protected set; }
        public string Name { get; protected set; }
        public string ModelId { get; protected set; }
        public Vector3D Position { get; protected set; }
        public Quaternion Rotation { get; protected set; }
        public float Health { get; protected set; }
        public float Energy { get; protected set; }
        public EnumMobileState MobileState { get; protected set; }
        public float Height { get; protected set; }
        public Affinity Affinity { get; protected set; }

        public EntityModel Move(Vector3D position, Quaternion rotation)
        {
            var r = (EntityModel)this.MemberwiseClone();
            r.Position = position;
            r.Rotation = rotation;
            return r;
            // TODO: MemberwiseClone is not 
            //return new EntityModel(Id, Name, ModelId, position, rotation, Health, MobileState, Height);
        }

        public EntityModel WithState(EnumMobileState state)
        {
            var r = (EntityModel)this.MemberwiseClone();
            r.MobileState = state;
            return r;
        }

        public EntityModel WithHealth(float health)
        {
            var r = (EntityModel)this.MemberwiseClone();

            r.Health = health;

            if (MobileState == EnumMobileState.Incapacitated)
            {
                if (Health <= -50)
                    r.MobileState = EnumMobileState.Dead;
                else if (Health > 0)
                    r.MobileState = EnumMobileState.Resting;
            }
            else if (MobileState != EnumMobileState.Dead && Health <= 0)
                r.MobileState = EnumMobileState.Incapacitated;

            return r;
        }

        public EntityModel WithHealthChange(float change)
        {
            return WithHealth(Health + change);
        }

        public EntityModel WithEnergy(float energy)
        {
            var r = (EntityModel)this.MemberwiseClone();
            r.Energy = energy;
            return r;
        }

        public EntityModel WithEnergyChange(float change)
        {
            // TODO: enforce min/max
            return WithEnergy(Energy + change);
        }

        public EntityModel WithAffinity(float air, float earth, float fire, float life, float water)
        {
            var r = (EntityModel)this.MemberwiseClone();
            r.Affinity = new Affinity(air, earth, fire, life, water);
            return r;
        }

        public EntityModel WithAffinityChange(float airChange, float earthChange, float fireChange, float lifeChange, float waterChange)
        {
            return WithAffinity(
                Affinity.Air + airChange,
                Affinity.Earth + earthChange,
                Affinity.Fire + fireChange,
                Affinity.Life + lifeChange,
                Affinity.Water + waterChange);
        }

        public float CurrentHeight { get { return MobileState <= EnumMobileState.Resting ? 0.3f : Height; } }

        public override string ToString()
        {
            return GetType().ToString() + " '" + Name + "' (" + Id + ")";
        }

        // TODO: equals and hashcode
    }
}
