using EcsCollision;
using UnityEngine;

/// <summary>
/// It is not recommended to use the default constructor. Use new Matrix3x3(Matrix3x3InitType initType) with Matrix3x3InitType.Zero.
/// </summary>
public struct Matrix3x3
{
    #region Fields

    public float[,] matrix;
    
    private static readonly float[,] MatrixZero = new float[,]
    {
        {0.0f, 0.0f, 0.0f}, 
        {0.0f, 0.0f, 0.0f}, 
        {0.0f, 0.0f, 0.0f}
    };

    private static readonly float[,] MatrixIdentify = new float[,]
    {
        {1.0f, 0.0f, 0.0f}, 
        {0.0f, 1.0f, 0.0f}, 
        {0.0f, 0.0f, 1.0f}
    };

    #endregion

    #region Constructors
    
    public Matrix3x3(Matrix3x3InitType initType)
    {
        if (initType == Matrix3x3InitType.Identity)
        {
            matrix = MatrixIdentify;
        }
        
        matrix = MatrixZero;
    }

    public Matrix3x3(float[,] matrix)
    {
        this.matrix = matrix;
    }

    #endregion


    #region Methods

    public Vector3 TransformVector(Vector3 V)
    {
        return new Vector3
        {
            x = (V.x * matrix[0, 0]) + (V.y * matrix[1, 0]) + (V.z * matrix[2, 0]),
            y = (V.x * matrix[0, 1]) + (V.y * matrix[1, 1]) + (V.z * matrix[2, 1]),
            z = (V.x * matrix[0, 2]) + (V.y * matrix[1, 2]) + (V.z * matrix[2, 2])
        };
    }

    #endregion
    
    #region Iterators
    
    public float this[uint row, uint col]
    {
        get
        {
            if (row < 4 && col < 4) return matrix[row, col];
            return 0.0f;
        }
        set
        {
            if (row < 4 && col < 4) matrix[row, col] = value;
        }
    }

    #endregion

    #region Operators

    public static Matrix3x3 operator *(Matrix3x3 A, Matrix3x3 B)
    {
        var mat = new Matrix3x3(Matrix3x3InitType.Zero);

        // First
        mat.matrix[0, 0] = A.matrix[0, 0] * B.matrix[0, 0] + A.matrix[1, 0] * B.matrix[0, 1] + A.matrix[2, 0] * B.matrix[0, 2];
        mat.matrix[0, 1] = A.matrix[0, 1] * B.matrix[0, 0] + A.matrix[1, 1] * B.matrix[0, 1] + A.matrix[2, 1] * B.matrix[0, 2];
        mat.matrix[0, 2] = A.matrix[0, 2] * B.matrix[0, 0] + A.matrix[1, 2] * B.matrix[0, 1] + A.matrix[2, 2] * B.matrix[0, 2];

        // Second              
        mat.matrix[1, 0] = A.matrix[0, 0] * B.matrix[1, 0] + A.matrix[1, 0] * B.matrix[1, 1] + A.matrix[2, 0] * B.matrix[1, 2];
        mat.matrix[1, 1] = A.matrix[0, 1] * B.matrix[1, 0] + A.matrix[1, 1] * B.matrix[1, 1] + A.matrix[2, 1] * B.matrix[1, 2];
        mat.matrix[1, 2] = A.matrix[0, 2] * B.matrix[1, 0] + A.matrix[1, 2] * B.matrix[1, 1] + A.matrix[2, 2] * B.matrix[1, 2];

        // Third
        mat.matrix[2, 0] = A.matrix[0, 0] * B.matrix[2, 0] + A.matrix[1, 0] * B.matrix[2, 1] + A.matrix[2, 0] * B.matrix[2, 2];
        mat.matrix[2, 1] = A.matrix[0, 1] * B.matrix[2, 0] + A.matrix[1, 1] * B.matrix[2, 1] + A.matrix[2, 1] * B.matrix[2, 2];
        mat.matrix[2, 2] = A.matrix[0, 2] * B.matrix[2, 0] + A.matrix[1, 2] * B.matrix[2, 1] + A.matrix[2, 2] * B.matrix[2, 2];

        return mat;
    }

    #endregion
}