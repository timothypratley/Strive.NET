using System;
using Strive.Multiverse;

namespace Strive.Network.Messages.ToClient
{
	/// <summary>
	/// Summary description for AddPhysicalObject.
	/// </summary>
	[Serializable]
	public class AddPhysicalObject : IMessage
	{
		public AddPhysicalObject( PhysicalObject po ) {
			this.instance_id = po.instance.ObjectInstanceID;
			this.model_id = po.template.ModelID;
			this.name = po.template.ObjectTemplateName;
			this.x = (float)po.instance.X;
			this.y = (float)po.instance.Y;
			this.z = (float)po.instance.Z;
			this.heading_x = (float)po.instance.HeadingX;
			this.heading_y = (float)po.instance.HeadingY;
			this.heading_z = (float)po.instance.HeadingZ;
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
