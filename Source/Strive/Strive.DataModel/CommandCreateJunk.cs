using System.Collections.Generic;
using Ncqrs.Commanding;
using System.Windows.Media.Media3D;

namespace Strive.DataModel
{
    public class CommandCreateJunk : CommandBase
    {
        public CommandCreateJunk(string name, Vector3D position, Quaternion rotation)
        {
            Name = name;
            Position = position;
            Rotation = rotation;
        }

        public string Name { get; private set; }
        public Vector3D Position { get; private set; }
        public Quaternion Rotation { get; private set; }
    }
}
