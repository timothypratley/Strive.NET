using System;
using Strive.Multiverse;

namespace Strive.Network.Messages.ToClient
{
	/// <summary>
	/// Summary description for AddPhysicalObject.
	/// </summary>
	[Serializable]
	public abstract class AddPhysicalObject : IMessage
	{
		public AddPhysicalObject(){}
		public AddPhysicalObject( PhysicalObject po ) {
			this.instance_id = po.ObjectInstanceID;
			this.model_id = po.ModelID;
			this.name = po.ObjectTemplateName;
			this.x = (float)po.Position.X;
			this.y = (float)po.Position.Y;
			this.z = (float)po.Position.Z;
			this.heading_x = (float)po.Heading.X;
			this.heading_y = (float)po.Heading.Y;
			this.heading_z = (float)po.Heading.Z;
		}

		public int instance_id;
		public int model_id;
		public string name;
		public float x;
		public float y;
		public float z;
		public float heading_x;
		public float heading_y;
		public float heading_z;
	}
}
