using System;
using System.Windows.Media.Media3D;
using System.Diagnostics.Contracts;


namespace Strive.Client.Model
{
    /// <summary>
    /// EntityModel is immutable to facilitate versioning.
    /// All changes result in a new object which is stored in the history (RecordedMapModel).
    /// Helpers to transition between states are located in WorldModel.
    /// </summary>
    public class EntityModel
    {
        public EntityModel(string name, string modelId, Vector3D position, Quaternion rotation)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(name));
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(modelId));
            Name = name;
            ModelId = modelId;
            Position = position;
            Rotation = rotation;
        }

        public string Name { get; private set; }
        public string ModelId { get; private set; }
        public Vector3D Position{ get; private set; }
        public Quaternion Rotation{ get; private set; }
    }
}
