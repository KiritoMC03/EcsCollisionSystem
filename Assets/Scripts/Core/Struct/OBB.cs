using UnityEngine;

namespace EcsCollision
{
    public struct OBB
    {
        public readonly Vector3 position;
        public readonly Quaternion rotation;

        public readonly Vector3 size;

        public readonly Vector3[] bounds;
        public readonly float diagonalLength;


        public OBB(Vector3 position, Quaternion rotation, Vector3 sizeValue)
        {
            this.position = position;
            this.rotation = rotation;
            bounds = EvaluateBoundsOBB(rotation, sizeValue);
            size = sizeValue;
            diagonalLength = sizeValue.magnitude;
        }

        public static Vector3[] EvaluateBoundsOBB(Quaternion rotation, Vector3 size)
        {
            var boundsAxisDirections = new Vector3[8];

            var x = rotation * Vector3.right;
            var y = rotation * Vector3.up;
            var z = rotation * Vector3.forward;

            x *= size.x / 2f;
            y *= size.y / 2f;
            z *= size.z / 2f;

            // Top edge
            boundsAxisDirections[0] = -x + y + z;
            boundsAxisDirections[1] = -x + y - z;
            boundsAxisDirections[2] = x + y - z;
            boundsAxisDirections[3] = x + y + z;

            // Bottom edge
            boundsAxisDirections[4] = -x - y + z;
            boundsAxisDirections[5] = -x - y - z;
            boundsAxisDirections[6] = x - y - z;
            boundsAxisDirections[7] = x - y + z;

            return boundsAxisDirections;
        }
    }
}