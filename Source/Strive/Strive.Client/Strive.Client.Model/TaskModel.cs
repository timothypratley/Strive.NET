using System;
using System.Windows.Media.Media3D;
using System.Diagnostics.Contracts;


namespace Strive.Client.Model
{
    public class TaskModel
    {
        public TaskModel(string name, Vector3D start, Vector3D finish)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(name));
            Name = name;
            Start = start;
            Finish = finish;
        }

        public string Name { get; private set; }
        public Vector3D Start { get; private set; }
        public Vector3D Finish{ get; private set; }
    }
}
