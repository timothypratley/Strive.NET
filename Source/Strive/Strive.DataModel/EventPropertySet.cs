using System.Collections.Generic;
using System.Linq;
using Ncqrs.Domain;

namespace Strive.DataModel
{
    public class EventPropertySet : DomainEvent
    {
        public EventPropertySet(Dictionary<EnumProperty, object> properties)
        {
            Properties = properties.ToArray();
        }

        public KeyValuePair<EnumProperty, object>[] Properties { get; private set; }
    }
}
