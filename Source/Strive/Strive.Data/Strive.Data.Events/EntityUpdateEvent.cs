using System.Collections.Generic;
using Microsoft.FSharp.Collections;
using Strive.Model;

namespace Strive.Data.Events
{
    public class EntityUpdateEvent : Event
    {
        public EntityUpdateEvent(EntityModel entity, string description)
        {
            Entities = ListModule.OfArray(new[] { entity });
            Description = description;
        }

        public EntityUpdateEvent(IEnumerable<EntityModel> entities, string description)
        {
            Entities = ListModule.OfSeq(entities);
            Description = description;
        }


        public FSharpList<EntityModel> Entities { get; set; }
    }
}
