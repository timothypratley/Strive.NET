using System.Windows.Media.Media3D;

namespace Strive.Network.Messages.ToServer
{
    public class CreateMobile : IMessage
    {
        public int TemplateId;
        public Vector3D Position;
        public Quaternion Rotation;

        public CreateMobile(int templateId, Vector3D position, Quaternion rotation)
        {
            TemplateId = templateId;
            Position = position;
            Rotation = rotation;
        }
    }
}
