using UnityEngine;

namespace EcsCollision
{
    public class OBBIntersectionTester
    {
        public static bool PreTest(OBB obb1, OBB obb2)
        {
            if (Vector3.Distance(obb1.position, obb2.position) >
                0.5f * obb1.diagonalLength + 0.5f * obb2.diagonalLength) return false;

            if (TestIntersectionByAxis(obb1, obb2, ObbAxis.X) &&
                TestIntersectionByAxis(obb1, obb2, ObbAxis.Y) &&
                TestIntersectionByAxis(obb1, obb2, ObbAxis.Z))
                return true;

            return false;
        }

        private static bool TestIntersectionByAxis(OBB obb1, OBB obb2, ObbAxis axisName)
        {
            // [F,A] the numbers of the angle A belonging to the F edge
            var anglesBelongingEdges = new uint[6, 4]
            {
                {2, 3, 6, 7}, {0, 1, 4, 5}, {0, 1, 2, 3}, {4, 5, 6, 7}, {0, 3, 4, 7}, {0, 1, 4, 5}
            };
            
            var axis = (int)axisName;
            // Assume that the first OBB is to the left along the axis than the second
            if (obb1.position[axis] > obb2.position[axis])
            {
                // Were wrong. We swap them
                var obbTemp = obb1;
                obb1 = obb2;
                obb2 = obbTemp;
            }

            // Checking the intersection along the axis axisName
            uint i = 0;
            
            // Remember the numbers of the faces depending on the axis
            uint face1 = 0, face2 = 0;
            if (axis == 0 /*X*/)
            {
                face1 = (uint) Face.PosX /*0*/;
                face2 = (uint) Face.NegX /*1*/;
            }
            
            if (axis == 1 /*Y*/)
            {
                face1 = (uint) Face.PosY /*2*/;
                face2 = (uint) Face.NegY /*3*/;
            }

            if (axis == 2 /*Z*/)
            {
                face1 = (uint) Face.PosZ /*4*/;
                face2 = (uint) Face.NegZ /*5*/;
            }
            
            // Looking for the rightmost angles for the 1st OBB and the leftmost angles for the 2nd OBB
            
            // Started angles
            var closeAngle1 = obb1.bounds[anglesBelongingEdges[face1, i]][axis];
            var closeAngle2 = obb2.bounds[anglesBelongingEdges[face2, i]][axis];
            
            // Check rest 3
            for (i = 1; i < 4; i++)
            {
                var newAngle1 = obb1.bounds[anglesBelongingEdges[face1, i]][axis];
                var newAngle2 = obb2.bounds[anglesBelongingEdges[face2, i]][axis];
                
                if (newAngle1 > closeAngle1) closeAngle1 = newAngle1;
                if (newAngle2 < closeAngle2) closeAngle2 = newAngle2;
            }

            // If extreme angle of 2nd OBB is more to left than angle of 1st, then there is intersection
            return (obb1.position[axis] + closeAngle1) > (obb2.position[axis] + closeAngle2);
        }

        public static bool Test(OBB obb1, OBB obb2)
        {
            //WalkZone z1 = this[n1]; WalkZone z2 = this[n2];


            var obb1RotationMatrix =
                Converter.QuaternionToMatrix3x3(obb1.rotation.x, obb1.rotation.y, obb1.rotation.z, obb1.rotation.w);
            var obb2RotationMatrix =
                Converter.QuaternionToTransposeMatrix3x3(obb2.rotation.x, obb2.rotation.y, obb2.rotation.z,
                    obb2.rotation.w);

            var worldOffset = obb2.position - obb1.position;

            // offset of the first OBB coordinate system
            var obb1CoordsOffset = obb1RotationMatrix.TransformVector(worldOffset); //A * worldOffset;

            // rotation matrix of A relative to B
            var rotationMatrixAToB = obb1RotationMatrix * obb2RotationMatrix;

            float ra, rb, t;
            uint i, k;
            float[] obb1Sizes = { obb1.size.x, obb1.size.y, obb1.size.z };
            float[] obb2Sizes = { obb2.size.x, obb2.size.y, obb2.size.z };

            // А coordinate system
            for (i = 0; i < 3; i++)
            {
                ra = obb1.size[(int) i];
                rb = obb2.size[0] * Mathf.Abs(rotationMatrixAToB[i, 0]) +
                     obb2.size[1] * Mathf.Abs(rotationMatrixAToB[i, 1]) +
                     obb2.size[2] * Mathf.Abs(rotationMatrixAToB[i, 2]);
                t = Mathf.Abs(obb1CoordsOffset[(int) i]);

                if (t > ra + rb) return false;
            }

            // B coordinate system
            for (k = 0; k < 3; k++)
            {
                ra = obb1Sizes[0] * Mathf.Abs(rotationMatrixAToB[0, k]) + obb1Sizes[1] * Mathf.Abs(rotationMatrixAToB[1, k]) +
                     obb1Sizes[2] * Mathf.Abs(rotationMatrixAToB[2, k]);
                rb = obb2Sizes[(int) k];
                t = Mathf.Abs(obb1CoordsOffset[0] * rotationMatrixAToB[0, k] +
                              obb1CoordsOffset[1] * rotationMatrixAToB[1, k] +
                              obb1CoordsOffset[2] * rotationMatrixAToB[2, k]);

                if (t > (ra + rb)) return false;
            }

            // 9 vector products
            //L = A0 x B0
            ra = obb1Sizes[1] * Mathf.Abs(rotationMatrixAToB[2, 0]) + obb1Sizes[2] * Mathf.Abs(rotationMatrixAToB[1, 0]);
            rb = obb2Sizes[1] * Mathf.Abs(rotationMatrixAToB[0, 2]) + obb2Sizes[2] * Mathf.Abs(rotationMatrixAToB[0, 1]);
            t = Mathf.Abs(obb1CoordsOffset[2] * rotationMatrixAToB[1, 0] - obb1CoordsOffset[1] * rotationMatrixAToB[2, 0]);

            if (t > (ra + rb)) return false;

            //L = A0 x B1
            ra = obb1Sizes[1] * Mathf.Abs(rotationMatrixAToB[2, 1]) + obb1Sizes[2] * Mathf.Abs(rotationMatrixAToB[1, 1]);
            rb = obb2Sizes[0] * Mathf.Abs(rotationMatrixAToB[0, 2]) + obb2Sizes[2] * Mathf.Abs(rotationMatrixAToB[0, 0]);
            t = Mathf.Abs(obb1CoordsOffset[2] * rotationMatrixAToB[1, 1] - obb1CoordsOffset[1] * rotationMatrixAToB[2, 1]);

            if (t > (ra + rb)) return false;

            //L = A0 x B2
            ra = obb1Sizes[1] * Mathf.Abs(rotationMatrixAToB[2, 2]) + obb1Sizes[2] * Mathf.Abs(rotationMatrixAToB[1, 2]);
            rb = obb2Sizes[0] * Mathf.Abs(rotationMatrixAToB[0, 1]) + obb2Sizes[1] * Mathf.Abs(rotationMatrixAToB[0, 0]);
            t = Mathf.Abs(obb1CoordsOffset[2] * rotationMatrixAToB[1, 2] - obb1CoordsOffset[1] * rotationMatrixAToB[2, 2]);

            if (t > (ra + rb)) return false;

            //L = A1 x B0
            ra = obb1Sizes[0] * Mathf.Abs(rotationMatrixAToB[2, 0]) + obb1Sizes[2] * Mathf.Abs(rotationMatrixAToB[0, 0]);
            rb = obb2Sizes[1] * Mathf.Abs(rotationMatrixAToB[1, 2]) + obb2Sizes[2] * Mathf.Abs(rotationMatrixAToB[1, 1]);
            t = Mathf.Abs(obb1CoordsOffset[0] * rotationMatrixAToB[2, 0] - obb1CoordsOffset[2] * rotationMatrixAToB[0, 0]);

            if (t > (ra + rb)) return false;

            //L = A1 x B1
            ra = obb1Sizes[0] * Mathf.Abs(rotationMatrixAToB[2, 1]) + obb1Sizes[2] * Mathf.Abs(rotationMatrixAToB[0, 1]);
            rb = obb2Sizes[0] * Mathf.Abs(rotationMatrixAToB[1, 2]) + obb2Sizes[2] * Mathf.Abs(rotationMatrixAToB[1, 0]);
            t = Mathf.Abs(obb1CoordsOffset[0] * rotationMatrixAToB[2, 1] - obb1CoordsOffset[2] * rotationMatrixAToB[0, 1]);

            if (t > (ra + rb)) return false;

            //L = A1 x B2
            ra = obb1Sizes[0] * Mathf.Abs(rotationMatrixAToB[2, 2]) + obb1Sizes[2] * Mathf.Abs(rotationMatrixAToB[0, 2]);
            rb = obb2Sizes[0] * Mathf.Abs(rotationMatrixAToB[1, 1]) + obb2Sizes[1] * Mathf.Abs(rotationMatrixAToB[1, 0]);
            t = Mathf.Abs(obb1CoordsOffset[0] * rotationMatrixAToB[2, 2] - obb1CoordsOffset[2] * rotationMatrixAToB[0, 2]);

            if (t > (ra + rb)) return false;

            //L = A2 x B0
            ra = obb1Sizes[0] * Mathf.Abs(rotationMatrixAToB[1, 0]) + obb1Sizes[1] * Mathf.Abs(rotationMatrixAToB[0, 0]);
            rb = obb2Sizes[1] * Mathf.Abs(rotationMatrixAToB[2, 2]) + obb2Sizes[2] * Mathf.Abs(rotationMatrixAToB[2, 1]);
            t = Mathf.Abs(obb1CoordsOffset[1] * rotationMatrixAToB[0, 0] - obb1CoordsOffset[0] * rotationMatrixAToB[1, 0]);

            if (t > (ra + rb)) return false;

            //L = A2 x B1
            ra = obb1Sizes[0] * Mathf.Abs(rotationMatrixAToB[1, 1]) + obb1Sizes[1] * Mathf.Abs(rotationMatrixAToB[0, 1]);
            rb = obb2Sizes[0] * Mathf.Abs(rotationMatrixAToB[2, 2]) + obb2Sizes[2] * Mathf.Abs(rotationMatrixAToB[2, 0]);
            t = Mathf.Abs(obb1CoordsOffset[1] * rotationMatrixAToB[0, 1] - obb1CoordsOffset[0] * rotationMatrixAToB[1, 1]);

            if (t > (ra + rb)) return false;

            //L = A2 x B2
            ra = obb1Sizes[0] * Mathf.Abs(rotationMatrixAToB[1, 2]) + obb1Sizes[1] * Mathf.Abs(rotationMatrixAToB[0, 2]);
            rb = obb2Sizes[0] * Mathf.Abs(rotationMatrixAToB[2, 1]) + obb2Sizes[1] * Mathf.Abs(rotationMatrixAToB[2, 0]);
            t = Mathf.Abs(obb1CoordsOffset[1] * rotationMatrixAToB[0, 2] - obb1CoordsOffset[0] * rotationMatrixAToB[1, 2]);

            if (t > (ra + rb)) return false;

            return true;
        }
    }
}