using System.Windows.Media.Media3D;
using Strive.Common;

namespace Strive.Data.Commands
{
    public class MoveCommand
    {
        Vector3D Position { get; set; }
        Quaternion Rotation { get; set; }
        EnumMobileState MobileState { get; set; }
    }
}
