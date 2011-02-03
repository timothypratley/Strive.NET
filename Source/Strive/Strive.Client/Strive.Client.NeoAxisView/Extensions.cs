using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

using Engine.MathEx;


namespace Strive.Client.NeoAxisView
{
    public static class Extensions
    {
        public static Vector3D ToVector3D(this Vec3 v)
        {
            return new Vector3D(v.X, v.Y, v.Z);
        }

        public static Quaternion ToQuaternion(this Quat q)
        {
            return new Quaternion(q.X, q.Y, q.Z, q.W);
        }

        public static Vec3 ToVec3(this Vector3D v)
        {
            return new Vec3((float)v.X, (float)v.Y, (float)v.Z);
        }

        public static Quat ToQuat(this Quaternion q)
        {
            return new Quat((float)q.X, (float)q.Y, (float)q.Z, (float)q.W);
        }
    }
}
