using System;
using System.Collections.Generic;
using System.Windows.Media.Media3D;
using Ncqrs.Domain;


namespace Strive.DataModel
{
    public enum EnumAttribute
    {
        Name,
        Health,
        Location
    }

    public class PhysicalObject : AggregateRootMappedByConvention
    {
        private static IDictionary<EnumAttribute, Type> AttributeTypes = new Dictionary<EnumAttribute, Type>
                                                                             {
                                                                                 {EnumAttribute.Name, typeof (string)},
                                                                                 {EnumAttribute.Health, typeof (double)},
                                                                                 {EnumAttribute.Location, typeof (Vector3D)}
                                                                             };

        private readonly IDictionary<EnumAttribute, object> _attributes = new Dictionary<EnumAttribute, object>();

        public PhysicalObject(string name)
        {
            var e = new AttributesSetEvent
                        {
                            Attributes = new[] { new KeyValuePair<EnumAttribute, object>(EnumAttribute.Name, name) }
                        };

            ApplyEvent(e);
        }

        protected void OnAttributesSet(AttributesSetEvent e)
        {
            foreach (KeyValuePair<EnumAttribute, object> a in e.Attributes)
                _attributes[a.Key] = a.Value;
        }
    }
}
