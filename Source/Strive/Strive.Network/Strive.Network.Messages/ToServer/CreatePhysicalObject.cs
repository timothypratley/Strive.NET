using System.Windows.Media.Media3D;

namespace Strive.Network.Messages.ToServer
{
    public class CreatePhysicalObject : IMessage
    {
        public int TemplateId;
        public Vector3D Position;
        public Quaternion Rotation;

        public CreatePhysicalObject(int templateId, Vector3D position, Quaternion rotation)
        {
            TemplateId = templateId;
            Position = position;
            Rotation = rotation;
        }
    }
}
