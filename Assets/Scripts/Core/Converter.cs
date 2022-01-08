namespace EcsCollision
{
    public class Converter
    {
        public static Matrix3x3 QuaternionToMatrix3x3(float x, float y, float z, float w)
        {
            var m = new Matrix3x3(Matrix3x3InitType.Zero);
            float wx, wy, wz, xx, yy, yz, xy, xz, zz;
            xx = x * x;
            xy = x * y;
            xz = x * z;
            yy = y * y;
            yz = y * z;
            zz = z * z;
            wx = w * x;
            wy = w * y;
            wz = w * z;
    
            m[0, 0] = 1.0f - 2 * (yy + zz);
            m[1, 0] = 2 * (xy - wz);
            m[2, 0] = 2 * (xz + wy);
    
            m[0, 1] = 2 * (xy + wz);
            m[1, 1] = 1.0f - 2 * (xx + zz);
            m[2, 1] = 2 * (yz - wx);
    
            m[0, 2] = 2 * (xz - wy);
            m[1, 2] = 2 * (yz + wx);
            m[2, 2] = 1.0f - 2 * (xx + yy);
            return m;
        }
    
        public static Matrix3x3 QuaternionToTransposeMatrix3x3(float x, float y, float z, float w)
        {
            var m = new Matrix3x3(Matrix3x3InitType.Zero);
            float wx, wy, wz, xx, yy, yz, xy, xz, zz;
            xx = x * x;
            xy = x * y;
            xz = x * z;
            yy = y * y;
            yz = y * z;
            zz = z * z;
            wx = w * x;
            wy = w * y;
            wz = w * z;
    
            m[0, 0] = 1.0f - 2 * (yy + zz);
            m[0, 1] = 2 * (xy - wz);
            m[0, 2] = 2 * (xz + wy);
    
            m[1, 0] = 2 * (xy + wz);
            m[1, 1] = 1.0f - 2 * (xx + zz);
            m[1, 2] = 2 * (yz - wx);
    
            m[2, 0] = 2 * (xz - wy);
            m[2, 1] = 2 * (yz + wx);
            m[2, 2] = 1.0f - 2 * (xx + yy);
            return m;
        }
    }
}