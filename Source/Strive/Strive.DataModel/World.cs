using System.Collections.Generic;


namespace Strive.DataModel
{
    class World
    {
        private readonly IDictionary<int,PhysicalObject> _physicalObjects = new Dictionary<int, PhysicalObject>();

        protected void OnAdd(){}
        protected void OnRemove(){}
    }
}
