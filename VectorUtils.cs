using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace PRO4_lab
{
    static class VectorUtils
    {
        static public Vector4 CrossProduct(Vector4 v, Vector4 u)
        {
            return new Vector4(
                u.Y * v.Z - u.Z * v.Y,
                u.Z * v.X - u.X * v.Z,
                u.X * v.Y - u.Y * v.X,
                0
                );
        }
        static public Vector4 MultiplyMatrix4x4andVector4(Matrix4x4 mat, Vector4 v)
        {
            return new Vector4(
                mat.M11 * v.X + mat.M12 * v.Y + mat.M13 * v.Z + mat.M14 * v.W,
                mat.M21 * v.X + mat.M22 * v.Y + mat.M23 * v.Z + mat.M24 * v.W,
                mat.M31 * v.X + mat.M32 * v.Y + mat.M33 * v.Z + mat.M34 * v.W,
                mat.M41 * v.X + mat.M42 * v.Y + mat.M43 * v.Z + mat.M44 * v.W
                );
        }
    }
}
