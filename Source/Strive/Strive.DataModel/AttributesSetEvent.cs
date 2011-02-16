using System.Collections.Generic;
using Ncqrs.Domain;

namespace Strive.DataModel
{
    public class AttributesSetEvent : DomainEvent
    {
        public KeyValuePair<EnumAttribute, object>[] Attributes { get; set; }
    }
}
