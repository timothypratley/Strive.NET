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
			this.instance_id = po.ObjectInstanceID;
			this.model_id = po.ModelID;
			this.name = po.ObjectTemplateName;
			this.x = (float)po.X;
			this.y = (float)po.Y;
			this.z = (float)po.Z;
			this.heading_x = (float)po.HeadingX;
			this.heading_y = (float)po.HeadingY;
			this.heading_z = (float)po.HeadingZ;
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
