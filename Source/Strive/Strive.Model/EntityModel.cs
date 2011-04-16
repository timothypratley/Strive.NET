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
    public class EntityModel
    {
        public EntityModel(int id, string name, string modelId, Vector3D position, Quaternion rotation,
            float health, EnumMobileState mobileState, float height)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(name));
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(modelId));
            Id = id;
            Name = name;
            ModelId = modelId;
            Position = position;
            Rotation = rotation;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public string ModelId { get; private set; }
        public Vector3D Position { get; private set; }
        public Quaternion Rotation { get; private set; }
        public float Health { get; private set; }
        public EnumMobileState MobileState { get; private set; }
        public float Height { get; private set; }

        public EntityModel Move(Vector3D position, Quaternion rotation)
        {
            return new EntityModel(Id, Name, ModelId, position, rotation, Health, MobileState, Height);
        }
    }
}
