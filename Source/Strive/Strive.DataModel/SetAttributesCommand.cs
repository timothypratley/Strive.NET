using System.Collections.Generic;
using Ncqrs.Commanding;

namespace Strive.DataModel
{
    public class SetAttributesCommand : CommandBase
    {
        public KeyValuePair<EnumAttribute, object>[] Attributes { get; set; }
    }
}
