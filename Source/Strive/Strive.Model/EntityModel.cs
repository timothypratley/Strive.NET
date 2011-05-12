using System;
using System.Diagnostics.Contracts;
using System.Windows.Media.Media3D;
using Microsoft.FSharp.Collections;
using Strive.Common;


namespace Strive.Model
{
    /// <summary>
    /// EntityModel is immutable to facilitate versioning.
    /// All changes result in a new object which is stored in the history.
    /// Helpers to transition between states are located in WorldModel.
    /// </summary>
    public class EntityModel : IComparable<EntityModel>, IEquatable<EntityModel>, IComparable
    {
        public EntityModel(int id, string name, string modelId, Vector3D position, Quaternion rotation,
            float health, float energy, EnumMobileState mobileState, float height)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(name));
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(modelId));
            Id = id;
            Owner = string.Empty;
            Name = name;
            ModelId = modelId;
            Position = position;
            Rotation = rotation;
            Health = health;
            Energy = energy;
            MobileState = mobileState;
            Height = height;
            Affinity = new Affinity(0, 0, 0, 0, 0);
            Production = new Production(1, ListModule.Empty<int>(), 0, 0);
            MoveRunSpeed = 1;
            MoveTurnSpeed = 1;
        }

        public int Id { get; protected set; }
        public string Owner { get; private set; }
        public string Name { get; protected set; }
        public string ModelId { get; protected set; }
        public Vector3D Position { get; protected set; }
        public Quaternion Rotation { get; protected set; }
        public float Health { get; protected set; }
        public float Energy { get; protected set; }
        public EnumMobileState MobileState { get; protected set; }
        public float Height { get; protected set; }
        public DateTime LastMoveUpdate { get; protected set; }
        public DateTime LastMobileStateUpdate { get; protected set; }
        public Affinity Affinity { get; protected set; }
        public Production Production { get; protected set; }
        public float MoveRunSpeed { get; protected set; }
        public float MoveTurnSpeed { get; protected set; }

        public EntityModel Move(EnumMobileState state, Vector3D position, Quaternion rotation, DateTime when)
        {
            var r = (EntityModel)this.MemberwiseClone();
            if (r.MobileState != state)
                r.LastMobileStateUpdate = when;
            r.MobileState = state;
            r.Position = position;
            r.Rotation = rotation;
            r.LastMoveUpdate = when;
            return r;
            // TODO: MemberwiseClone is not good for performance, just convenient
            //return new EntityModel(Id, Name, ModelId, position, rotation, Health, MobileState, Height);
        }

        public EntityModel WithOwner(string owner)
        {
            var r = (EntityModel)this.MemberwiseClone();
            r.Owner = owner;
            return r;
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

        public EntityModel WithProduction(int id, DateTime when)
        {
            var r = (EntityModel)this.MemberwiseClone();
            r.Production = Production.WithProduction(id, when);
            return r;
        }

        public EntityModel WithProductionComplete(DateTime when)
        {
            var r = (EntityModel)this.MemberwiseClone();
            r.Production = Production.WithProductionComplete(when);
            return r;
        }

        public EntityModel WithProductionProgressChange(float progressChange, DateTime when)
        {
            var r = (EntityModel)this.MemberwiseClone();
            r.Production = Production.WithProgressChange(progressChange, when);
            return r;
        }

        public float CurrentHeight { get { return MobileState <= EnumMobileState.Resting ? 0.3f : Height; } }

        public override string ToString()
        {
            return GetType().ToString() + " '" + Name + "' (" + Id + ")";
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as EntityModel;
            if (obj == null)
                return false;
            return Id == other.Id;
        }

        bool IEquatable<EntityModel>.Equals(EntityModel other)
        {
            if (other == null)
                return false;
            return Id == other.Id;
        }

        int IComparable<EntityModel>.CompareTo(EntityModel other)
        {
            if (other == null)
                return 1;
            return Id.CompareTo(other.Id);
        }

        int IComparable.CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            var other = obj as EntityModel;
            if (obj == null)
                throw new ArgumentException("Object is not an EntityModel");
            return Id.CompareTo(other.Id);
        }
    }
}
