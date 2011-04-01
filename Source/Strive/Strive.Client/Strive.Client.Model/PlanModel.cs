using System;
using System.Windows.Media.Media3D;
using System.Diagnostics.Contracts;


namespace Strive.Client.Model
{
    public class PlanModel
    {
        public PlanModel(int id, Vector3D start, Vector3D finish)
        {
            Id = id;
            Start = start;
            Finish = finish;
        }

        public int Id { get; private set; }
        public Vector3D Start { get; private set; }
        public Vector3D Finish{ get; private set; }
    }
}
