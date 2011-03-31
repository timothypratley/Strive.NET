using System;
using System.Collections.Generic;
using System.Windows.Media.Media3D;
using Ncqrs.Domain;


namespace Strive.DataModel
{
    public enum EnumProperty
    {
        Name,
        Health,
        Position,
        Rotation
    }

    public class PhysicalObject : AggregateRootMappedByConvention
    {
        private static IDictionary<EnumProperty, Type> PropertyTypes = new Dictionary<EnumProperty, Type>
                                                                             {
                                                                                 {EnumProperty.Name, typeof (string)},
                                                                                 {EnumProperty.Health, typeof (double)},
                                                                                 {EnumProperty.Position, typeof (Vector3D)},
                                                                                 {EnumProperty.Rotation, typeof (Quaternion)}
                                                                             };

        private readonly IDictionary<EnumProperty, object> _properties = new Dictionary<EnumProperty, object>();

        public PhysicalObject(string name)
        {
            var e = new EventPropertySet(
                new Dictionary<EnumProperty, object> { { EnumProperty.Name, name } });

            ApplyEvent(e);
        }

        protected void OnPropertySet(EventPropertySet e)
        {
            foreach (KeyValuePair<EnumProperty, object> a in e.Properties)
                _properties[a.Key] = a.Value;
        }
    }
}
