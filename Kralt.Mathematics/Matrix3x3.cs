using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Kralt.Mathematics;

/// <summary>
/// Represents a 3x3 matrix containing 3D rotation and scale.
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct Matrix3x3 : IEquatable<Matrix3x3>, IFormattable
{
    /// <summary>
    /// First row of the matrix.
    /// </summary>
    public Vector3 Row0;

    /// <summary>
    /// Second row of the matrix.
    /// </summary>
    public Vector3 Row1;

    /// <summary>
    /// Third row of the matrix.
    /// </summary>
    public Vector3 Row2;

    /// <summary>
    /// The identity matrix.
    /// </summary>
    public static readonly Matrix3x3 Identity = new Matrix3x3(Vector3.right, Vector3.up, Vector3.forward);

    /// <summary>
    /// The zero matrix.
    /// </summary>
    public static readonly Matrix3x3 Zero = new Matrix3x3(Vector3.zero, Vector3.zero, Vector3.zero);

    /// <summary>
    /// Initializes a new instance of the <see cref="Matrix3x3"/> struct.
    /// </summary>
    /// <param name="row0">Top row of the matrix.</param>
    /// <param name="row1">Second row of the matrix.</param>
    /// <param name="row2">Bottom row of the matrix.</param>
    public Matrix3x3(Vector3 row0, Vector3 row1, Vector3 row2)
    {
        Row0 = row0;
        Row1 = row1;
        Row2 = row2;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Matrix3x3"/> struct.
    /// </summary>
    /// <param name="m00">First item of the first row of the matrix.</param>
    /// <param name="m01">Second item of the first row of the matrix.</param>
    /// <param name="m02">Third item of the first row of the matrix.</param>
    /// <param name="m10">First item of the second row of the matrix.</param>
    /// <param name="m11">Second item of the second row of the matrix.</param>
    /// <param name="m12">Third item of the second row of the matrix.</param>
    /// <param name="m20">First item of the third row of the matrix.</param>
    /// <param name="m21">Second item of the third row of the matrix.</param>
    /// <param name="m22">Third item of the third row of the matrix.</param>
    [SuppressMessage("ReSharper", "SA1117", Justification = "For better readability of Matrix struct.")]
    public Matrix3x3
    (
        float m00, float m01, float m02,
        float m10, float m11, float m12,
        float m20, float m21, float m22
    )
    {
        Row0 = new Vector3(m00, m01, m02);
        Row1 = new Vector3(m10, m11, m12);
        Row2 = new Vector3(m20, m21, m22);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Matrix3x3"/> struct.
    /// </summary>
    /// <param name="matrix">A Matrix4 to take the upper-left 3x3 from.</param>
    public Matrix3x3(Matrix4x4 matrix)
    {
        Row0 = matrix.Row0.Xyz;
        Row1 = matrix.Row1.Xyz;
        Row2 = matrix.Row2.Xyz;
    }

    /// <summary>
    /// Gets the determinant of this matrix.
    /// </summary>
    public readonly float Determinant
    {
        get
        {
            float m11 = Row0.x;
            float m12 = Row0.y;
            float m13 = Row0.z;
            float m21 = Row1.x;
            float m22 = Row1.y;
            float m23 = Row1.z;
            float m31 = Row2.x;
            float m32 = Row2.y;
            float m33 = Row2.z;

            return (m11 * m22 * m33) + (m12 * m23 * m31) + (m13 * m21 * m32)
                   - (m13 * m22 * m31) - (m11 * m23 * m32) - (m12 * m21 * m33);
        }
    }

    /// <summary>
    /// Gets the first column of this matrix.
    /// </summary>
    public Vector3 Column0
    {
        readonly get => new Vector3(Row0.x, Row1.x, Row2.x);
        set
        {
            Row0.x = value.x;
            Row1.x = value.y;
            Row2.x = value.z;
        }
    }

    /// <summary>
    /// Gets the second column of this matrix.
    /// </summary>
    public Vector3 Column1
    {
        readonly get => new Vector3(Row0.y, Row1.y, Row2.y);
        set
        {
            Row0.y = value.x;
            Row1.y = value.y;
            Row2.y = value.z;
        }
    }

    /// <summary>
    /// Gets the third column of this matrix.
    /// </summary>
    public Vector3 Column2
    {
        readonly get => new Vector3(Row0.z, Row1.z, Row2.z);
        set
        {
            Row0.z = value.x;
            Row1.z = value.y;
            Row2.z = value.z;
        }
    }

    /// <summary>
    /// Gets or sets the value at row 1, column 1 of this instance.
    /// </summary>
    public float M11
    {
        readonly get => Row0.x;
        set => Row0.x = value;
    }

    /// <summary>
    /// Gets or sets the value at row 1, column 2 of this instance.
    /// </summary>
    public float M12
    {
        readonly get => Row0.y;
        set => Row0.y = value;
    }

    /// <summary>
    /// Gets or sets the value at row 1, column 3 of this instance.
    /// </summary>
    public float M13
    {
        readonly get => Row0.z;
        set => Row0.z = value;
    }

    /// <summary>
    /// Gets or sets the value at row 2, column 1 of this instance.
    /// </summary>
    public float M21
    {
        readonly get => Row1.x;
        set => Row1.x = value;
    }

    /// <summary>
    /// Gets or sets the value at row 2, column 2 of this instance.
    /// </summary>
    public float M22
    {
        readonly get => Row1.y;
        set => Row1.y = value;
    }

    /// <summary>
    /// Gets or sets the value at row 2, column 3 of this instance.
    /// </summary>
    public float M23
    {
        readonly get => Row1.z;
        set => Row1.z = value;
    }

    /// <summary>
    /// Gets or sets the value at row 3, column 1 of this instance.
    /// </summary>
    public float M31
    {
        readonly get => Row2.x;
        set => Row2.x = value;
    }

    /// <summary>
    /// Gets or sets the value at row 3, column 2 of this instance.
    /// </summary>
    public float M32
    {
        readonly get => Row2.y;
        set => Row2.y = value;
    }

    /// <summary>
    /// Gets or sets the value at row 3, column 3 of this instance.
    /// </summary>
    public float M33
    {
        readonly get => Row2.z;
        set => Row2.z = value;
    }

    /// <summary>
    /// Gets or sets the values along the main diagonal of the matrix.
    /// </summary>
    public Vector3 Diagonal
    {
        readonly get => new Vector3(Row0.x, Row1.y, Row2.z);
        set
        {
            Row0.x = value.x;
            Row1.y = value.y;
            Row2.z = value.z;
        }
    }

    /// <summary>
    /// Gets the trace of the matrix, the sum of the values along the diagonal.
    /// </summary>
    public readonly float Trace => Row0.x + Row1.y + Row2.z;

    /// <summary>
    /// Gets or sets the value at a specified row and column.
    /// </summary>
    /// <param name="rowIndex">The index of the row.</param>
    /// <param name="columnIndex">The index of the column.</param>
    /// <returns>The element at the given row and column index.</returns>
    public float this[int rowIndex, int columnIndex]
    {
        readonly get
        {
            if (((uint)rowIndex) < 3 && ((uint)columnIndex) < 3)
            {
                return GetRowUnsafe(in this, rowIndex)[columnIndex];
            }
            else
            {
                Mathf.ThrowOutOfRangeException($"You tried to access this matrix at: ({rowIndex}, {columnIndex})");
                return default;
            }
        }

        set
        {
            if (((uint)rowIndex) < 3 && ((uint)columnIndex) < 3)
            {
                GetRowUnsafe(in this, rowIndex)[columnIndex] = value;
            }
            else
            {
                Mathf.ThrowOutOfRangeException($"You tried to set this matrix at: ({rowIndex}, {columnIndex})");
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private readonly ref Vector3 GetRowUnsafe(in Matrix3x3 m, int index)
    {
        ref Vector3 address = ref Unsafe.AsRef(in m.Row0);
        return ref Unsafe.Add(ref address, index);
    }

    /// <summary>
    /// Converts this instance into its inverse.
    /// </summary>
    public void Invert()
    {
        this = Invert(this);
    }

    /// <summary>
    /// Returns an inverted copy of this instance.
    /// </summary>
    /// <remarks>
    /// If the matrix is singular this function does not throw an exception,
    /// instead it returns the original un-inverted matrix.
    /// </remarks>
    /// <returns>The inverted copy.</returns>
    public readonly Matrix3x3 Inverted()
    {
        var m = this;
        // FIXME: This is an unfortunate implementation detail that we can't really change without
        // creating a breaking change...
        // - Noggin_bops 2024-02-27
        if (m.Determinant != 0)
        {
            m.Invert();
        }

        return m;
    }

    /// <summary>
    /// Converts this instance into its transpose.
    /// </summary>
    public void Transpose()
    {
        this = Transpose(this);
    }

    /// <summary>
    /// Returns a transposed copy of this instance.
    /// </summary>
    /// <returns>The transposed copy.</returns>
    public readonly Matrix3x3 Transposed()
    {
        Matrix3x3 m = this;
        m.Transpose();
        return m;
    }

    /// <summary>
    /// Divides each element in the Matrix by the <see cref="Determinant"/>.
    /// </summary>
    public void Normalize()
    {
        var determinant = Determinant;
        Row0 /= determinant;
        Row1 /= determinant;
        Row2 /= determinant;
    }

    /// <summary>
    /// Returns a normalized copy of this instance.
    /// </summary>
    /// <returns>The normalized copy.</returns>
    public readonly Matrix3x3 Normalized()
    {
        var m = this;
        m.Normalize();
        return m;
    }

    /// <summary>
    /// Swizzles this instance. Switches places of the rows of the matrix.
    /// </summary>
    /// <param name="rowForRow0">Which row to place in <see cref="Row0"/>.</param>
    /// <param name="rowForRow1">Which row to place in <see cref="Row1"/>.</param>
    /// <param name="rowForRow2">Which row to place in <see cref="Row2"/>.</param>
    public void Swizzle(int rowForRow0, int rowForRow1, int rowForRow2)
    {
        this = Swizzle(this, rowForRow0, rowForRow1, rowForRow2);
    }

    /// <summary>
    /// Returns a swizzled copy of this instance.
    /// </summary>
    /// <param name="rowForRow0">Which row to place in <see cref="Row0"/>.</param>
    /// <param name="rowForRow1">Which row to place in <see cref="Row1"/>.</param>
    /// <param name="rowForRow2">Which row to place in <see cref="Row2"/>.</param>
    /// <returns>The swizzled copy.</returns>
    public readonly Matrix3x3 Swizzled(int rowForRow0, int rowForRow1, int rowForRow2)
    {
        Matrix3x3 m = this;
        m.Swizzle(rowForRow0, rowForRow1, rowForRow2);
        return m;
    }

    /// <summary>
    /// Returns a copy of this Matrix3 without scale.
    /// </summary>
    /// <returns>The matrix without scaling.</returns>
    public readonly Matrix3x3 ClearScale()
    {
        var m = this;
        m.Row0 = m.Row0.Normalized();
        m.Row1 = m.Row1.Normalized();
        m.Row2 = m.Row2.Normalized();
        return m;
    }

    /// <summary>
    /// Returns a copy of this Matrix3 without rotation.
    /// </summary>
    /// <returns>The matrix without rotation.</returns>
    public readonly Matrix3x3 ClearRotation()
    {
        var m = this;
        m.Row0 = new Vector3(m.Row0.Length, 0, 0);
        m.Row1 = new Vector3(0, m.Row1.Length, 0);
        m.Row2 = new Vector3(0, 0, m.Row2.Length);
        return m;
    }

    /// <summary>
    /// Returns the scale component of this instance.
    /// </summary>
    /// <returns>The scale components.</returns>
    public readonly Vector3 ExtractScale()
    {
        return new Vector3(Row0.Length, Row1.Length, Row2.Length);
    }

    /// <summary>
    /// Returns the rotation component of this instance. Quite slow.
    /// </summary>
    /// <param name="rowNormalize">
    /// Whether the method should row-normalize (i.e. remove scale from) the Matrix. Pass false if
    /// you know it's already normalized.
    /// </param>
    /// <returns>The rotation.</returns>
    public readonly Quaternion ExtractRotation(bool rowNormalize = true)
    {
        var row0 = Row0;
        var row1 = Row1;
        var row2 = Row2;

        if (rowNormalize)
        {
            row0 = row0.Normalized();
            row1 = row1.Normalized();
            row2 = row2.Normalized();
        }

        // code below adapted from Blender
        var q = default(Quaternion);
        var trace = 0.25 * (row0[0] + row1[1] + row2[2] + 1.0);

        if (trace > 0)
        {
            var sq = Math.Sqrt(trace);

            q.w = (float)sq;
            sq = 1.0 / (4.0 * sq);
            q.X = (float)((row1[2] - row2[1]) * sq);
            q.Y = (float)((row2[0] - row0[2]) * sq);
            q.Z = (float)((row0[1] - row1[0]) * sq);
        }
        else if (row0[0] > row1[1] && row0[0] > row2[2])
        {
            var sq = 2.0 * Math.Sqrt(1.0 + row0[0] - row1[1] - row2[2]);

            q.X = (float)(0.25 * sq);
            sq = 1.0 / sq;
            q.w = (float)((row2[1] - row1[2]) * sq);
            q.Y = (float)((row1[0] + row0[1]) * sq);
            q.Z = (float)((row2[0] + row0[2]) * sq);
        }
        else if (row1[1] > row2[2])
        {
            var sq = 2.0 * Math.Sqrt(1.0 + row1[1] - row0[0] - row2[2]);

            q.Y = (float)(0.25 * sq);
            sq = 1.0 / sq;
            q.w = (float)((row2[0] - row0[2]) * sq);
            q.X = (float)((row1[0] + row0[1]) * sq);
            q.Z = (float)((row2[1] + row1[2]) * sq);
        }
        else
        {
            var sq = 2.0 * Math.Sqrt(1.0 + row2[2] - row0[0] - row1[1]);

            q.Z = (float)(0.25 * sq);
            sq = 1.0 / sq;
            q.w = (float)((row1[0] - row0[1]) * sq);
            q.X = (float)((row2[0] + row0[2]) * sq);
            q.Y = (float)((row2[1] + row1[2]) * sq);
        }

        q.Normalize();
        return q;
    }

    /// <summary>
    /// Build a rotation matrix from the specified axis/angle rotation.
    /// </summary>
    /// <param name="axis">The axis to rotate about.</param>
    /// <param name="angle">Angle in radians to rotate counter-clockwise (looking in the direction of the given axis).</param>
    /// <param name="result">A matrix instance.</param>
    public static void CreateFromAxisAngle(Vector3 axis, float angle, out Matrix3x3 result)
    {
        // normalize and create a local copy of the vector.
        axis.Normalize();
        float axisX = axis.x, axisY = axis.y, axisZ = axis.z;

        // calculate angles
        var cos = MathF.Cos(-angle);
        var sin = MathF.Sin(-angle);
        var t = 1.0f - cos;

        // do the conversion math once
        float tXX = t * axisX * axisX;
        float tXY = t * axisX * axisY;
        float tXZ = t * axisX * axisZ;
        float tYY = t * axisY * axisY;
        float tYZ = t * axisY * axisZ;
        float tZZ = t * axisZ * axisZ;

        float sinX = sin * axisX;
        float sinY = sin * axisY;
        float sinZ = sin * axisZ;

        result.Row0.x = tXX + cos;
        result.Row0.y = tXY - sinZ;
        result.Row0.z = tXZ + sinY;
        result.Row1.x = tXY + sinZ;
        result.Row1.y = tYY + cos;
        result.Row1.z = tYZ - sinX;
        result.Row2.x = tXZ - sinY;
        result.Row2.y = tYZ + sinX;
        result.Row2.z = tZZ + cos;
    }

    /// <summary>
    /// Build a rotation matrix from the specified axis/angle rotation.
    /// </summary>
    /// <param name="axis">The axis to rotate about.</param>
    /// <param name="angle">Angle in radians to rotate counter-clockwise (looking in the direction of the given axis).</param>
    /// <returns>A matrix instance.</returns>
    [Pure]
    public static Matrix3x3 CreateFromAxisAngle(Vector3 axis, float angle)
    {
        CreateFromAxisAngle(axis, angle, out Matrix3x3 result);
        return result;
    }

    /// <summary>
    /// Build a rotation matrix from the specified quaternion.
    /// </summary>
    /// <param name="q">Quaternion to translate.</param>
    /// <param name="result">Matrix result.</param>
    public static void CreateFromQuaternion(in Quaternion q, out Matrix3x3 result)
    {
        // Adapted from https://en.wikipedia.org/wiki/Quaternions_and_spatial_rotation#Quaternion-derived_rotation_matrix
        // with the caviat that opentk uses row-major matrices so the matrix we create is transposed
        float sqx = q.X * q.X;
        float sqy = q.Y * q.Y;
        float sqz = q.Z * q.Z;
        float sqw = q.w * q.w;

        float xy = q.X * q.Y;
        float xz = q.X * q.Z;
        float xw = q.X * q.w;

        float yz = q.Y * q.Z;
        float yw = q.Y * q.w;

        float zw = q.Z * q.w;

        float s2 = 2f / (sqx + sqy + sqz + sqw);

        result.Row0.x = 1f - (s2 * (sqy + sqz));
        result.Row1.y = 1f - (s2 * (sqx + sqz));
        result.Row2.z = 1f - (s2 * (sqx + sqy));

        result.Row0.y = s2 * (xy + zw);
        result.Row1.x = s2 * (xy - zw);

        result.Row2.x = s2 * (xz + yw);
        result.Row0.z = s2 * (xz - yw);

        result.Row2.y = s2 * (yz - xw);
        result.Row1.z = s2 * (yz + xw);
    }

    /// <summary>
    /// Build a rotation matrix from the specified quaternion.
    /// </summary>
    /// <param name="q">Quaternion to translate.</param>
    /// <returns>A matrix instance.</returns>
    [Pure]
    public static Matrix3x3 CreateFromQuaternion(Quaternion q)
    {
        CreateFromQuaternion(in q, out Matrix3x3 result);
        return result;
    }

    /// <summary>
    /// Builds a rotation matrix for a rotation around the x-axis.
    /// </summary>
    /// <param name="angle">The counter-clockwise angle in radians.</param>
    /// <param name="result">The resulting Matrix3 instance.</param>
    public static void CreateRotationX(float angle, out Matrix3x3 result)
    {
        var cos = MathF.Cos(angle);
        var sin = MathF.Sin(angle);

        result = Identity;
        result.Row1.y = cos;
        result.Row1.z = sin;
        result.Row2.y = -sin;
        result.Row2.z = cos;
    }

    /// <summary>
    /// Builds a rotation matrix for a rotation around the x-axis.
    /// </summary>
    /// <param name="angle">The counter-clockwise angle in radians.</param>
    /// <returns>The resulting Matrix3 instance.</returns>
    [Pure]
    public static Matrix3x3 CreateRotationX(float angle)
    {
        CreateRotationX(angle, out Matrix3x3 result);
        return result;
    }

    /// <summary>
    /// Builds a rotation matrix for a rotation around the y-axis.
    /// </summary>
    /// <param name="angle">The counter-clockwise angle in radians.</param>
    /// <param name="result">The resulting Matrix3 instance.</param>
    public static void CreateRotationY(float angle, out Matrix3x3 result)
    {
        var cos = MathF.Cos(angle);
        var sin = MathF.Sin(angle);

        result = Identity;
        result.Row0.x = cos;
        result.Row0.z = -sin;
        result.Row2.x = sin;
        result.Row2.z = cos;
    }

    /// <summary>
    /// Builds a rotation matrix for a rotation around the y-axis.
    /// </summary>
    /// <param name="angle">The counter-clockwise angle in radians.</param>
    /// <returns>The resulting Matrix3 instance.</returns>
    [Pure]
    public static Matrix3x3 CreateRotationY(float angle)
    {
        CreateRotationY(angle, out Matrix3x3 result);
        return result;
    }

    /// <summary>
    /// Builds a rotation matrix for a rotation around the z-axis.
    /// </summary>
    /// <param name="angle">The counter-clockwise angle in radians.</param>
    /// <param name="result">The resulting Matrix3 instance.</param>
    public static void CreateRotationZ(float angle, out Matrix3x3 result)
    {
        var cos = MathF.Cos(angle);
        var sin = MathF.Sin(angle);

        result = Identity;
        result.Row0.x = cos;
        result.Row0.y = sin;
        result.Row1.x = -sin;
        result.Row1.y = cos;
    }

    /// <summary>
    /// Builds a rotation matrix for a rotation around the z-axis.
    /// </summary>
    /// <param name="angle">The counter-clockwise angle in radians.</param>
    /// <returns>The resulting Matrix3 instance.</returns>
    [Pure]
    public static Matrix3x3 CreateRotationZ(float angle)
    {
        CreateRotationZ(angle, out Matrix3x3 result);
        return result;
    }

    /// <summary>
    /// Creates a scale matrix.
    /// </summary>
    /// <param name="scale">Single scale factor for the x, y, and z axes.</param>
    /// <returns>A scale matrix.</returns>
    [Pure]
    public static Matrix3x3 CreateScale(float scale)
    {
        CreateScale(scale, out Matrix3x3 result);
        return result;
    }

    /// <summary>
    /// Creates a scale matrix.
    /// </summary>
    /// <param name="scale">Scale factors for the x, y, and z axes.</param>
    /// <returns>A scale matrix.</returns>
    [Pure]
    public static Matrix3x3 CreateScale(Vector3 scale)
    {
        CreateScale(in scale, out Matrix3x3 result);
        return result;
    }

    /// <summary>
    /// Creates a scale matrix.
    /// </summary>
    /// <param name="x">Scale factor for the x axis.</param>
    /// <param name="y">Scale factor for the y axis.</param>
    /// <param name="z">Scale factor for the z axis.</param>
    /// <returns>A scale matrix.</returns>
    [Pure]
    public static Matrix3x3 CreateScale(float x, float y, float z)
    {
        CreateScale(x, y, z, out Matrix3x3 result);
        return result;
    }

    /// <summary>
    /// Creates a scale matrix.
    /// </summary>
    /// <param name="scale">Single scale factor for the x, y, and z axes.</param>
    /// <param name="result">A scale matrix.</param>
    public static void CreateScale(float scale, out Matrix3x3 result)
    {
        result = Identity;
        result.Row0.x = scale;
        result.Row1.y = scale;
        result.Row2.z = scale;
    }

    /// <summary>
    /// Creates a scale matrix.
    /// </summary>
    /// <param name="scale">Scale factors for the x, y, and z axes.</param>
    /// <param name="result">A scale matrix.</param>
    public static void CreateScale(in Vector3 scale, out Matrix3x3 result)
    {
        result = Identity;
        result.Row0.x = scale.x;
        result.Row1.y = scale.y;
        result.Row2.z = scale.z;
    }

    /// <summary>
    /// Creates a scale matrix.
    /// </summary>
    /// <param name="x">Scale factor for the x axis.</param>
    /// <param name="y">Scale factor for the y axis.</param>
    /// <param name="z">Scale factor for the z axis.</param>
    /// <param name="result">A scale matrix.</param>
    public static void CreateScale(float x, float y, float z, out Matrix3x3 result)
    {
        result = Identity;
        result.Row0.x = x;
        result.Row1.y = y;
        result.Row2.z = z;
    }

    /// <summary>
    /// Create a swizzle matrix that can be used to change the row order of matrices.
    /// </summary>
    /// <remarks>
    /// If you are looking to swizzle vectors there are properties like <see cref="Vector3.Zyx"/> that can do this more effectively.
    /// </remarks>
    /// <param name="rowForRow0">Which row to place in <see cref="Row0"/>.</param>
    /// <param name="rowForRow1">Which row to place in <see cref="Row1"/>.</param>
    /// <param name="rowForRow2">Which row to place in <see cref="Row2"/>.</param>
    /// <returns>The resulting swizzle matrix.</returns>
    public static Matrix3x3 CreateSwizzle(int rowForRow0, int rowForRow1, int rowForRow2)
    {
        CreateSwizzle(rowForRow0, rowForRow1, rowForRow2, out Matrix3x3 result);
        return result;
    }

    /// <summary>
    /// Create a swizzle matrix that can be used to change the row order of matrices.
    /// </summary>
    /// /// <remarks>
    /// If you are looking to swizzle vectors there are properties like <see cref="Vector3.Zyx"/> that can do this more effectively.
    /// </remarks>
    /// <param name="rowForRow0">Which row to place in <see cref="Row0"/>.</param>
    /// <param name="rowForRow1">Which row to place in <see cref="Row1"/>.</param>
    /// <param name="rowForRow2">Which row to place in <see cref="Row2"/>.</param>
    /// <param name="result">The resulting swizzle matrix.</param>
    public static void CreateSwizzle(int rowForRow0, int rowForRow1, int rowForRow2, out Matrix3x3 result)
    {
        if (rowForRow0 < 0 || rowForRow0 >= 3)
        {
            throw new IndexOutOfRangeException($"{nameof(rowForRow0)} must be a number between 0 and 2. Got {rowForRow0}.");
        }
        if (rowForRow1 < 0 || rowForRow1 >= 3)
        {
            throw new IndexOutOfRangeException($"{nameof(rowForRow1)} must be a number between 0 and 2. Got {rowForRow1}.");
        }
        if (rowForRow2 < 0 || rowForRow2 >= 3)
        {
            throw new IndexOutOfRangeException($"{nameof(rowForRow2)} must be a number between 0 and 2. Got {rowForRow2}.");
        }

        result = Zero;
        result[0, rowForRow0] = 1.0f;
        result[1, rowForRow1] = 1.0f;
        result[2, rowForRow2] = 1.0f;
    }

    /// <summary>
    /// Adds two instances.
    /// </summary>
    /// <param name="left">The left operand of the addition.</param>
    /// <param name="right">The right operand of the addition.</param>
    /// <returns>A new instance that is the result of the addition.</returns>
    [Pure]
    public static Matrix3x3 Add(Matrix3x3 left, Matrix3x3 right)
    {
        Add(in left, in right, out Matrix3x3 result);
        return result;
    }

    /// <summary>
    /// Adds two instances.
    /// </summary>
    /// <param name="left">The left operand of the addition.</param>
    /// <param name="right">The right operand of the addition.</param>
    /// <param name="result">A new instance that is the result of the addition.</param>
    public static void Add(in Matrix3x3 left, in Matrix3x3 right, out Matrix3x3 result)
    {
        Vector3.Add(in left.Row0, in right.Row0, out result.Row0);
        Vector3.Add(in left.Row1, in right.Row1, out result.Row1);
        Vector3.Add(in left.Row2, in right.Row2, out result.Row2);
    }

    /// <summary>
    /// Multiplies two instances.
    /// </summary>
    /// <param name="left">The left operand of the multiplication.</param>
    /// <param name="right">The right operand of the multiplication.</param>
    /// <returns>A new instance that is the result of the multiplication.</returns>
    [Pure]
    public static Matrix3x3 Mult(Matrix3x3 left, Matrix3x3 right)
    {
        Mult(in left, in right, out Matrix3x3 result);
        return result;
    }

    /// <summary>
    /// Multiplies two instances.
    /// </summary>
    /// <param name="left">The left operand of the multiplication.</param>
    /// <param name="right">The right operand of the multiplication.</param>
    /// <param name="result">A new instance that is the result of the multiplication.</param>
    public static void Mult(in Matrix3x3 left, in Matrix3x3 right, out Matrix3x3 result)
    {
        float leftM11 = left.Row0.x;
        float leftM12 = left.Row0.y;
        float leftM13 = left.Row0.z;
        float leftM21 = left.Row1.x;
        float leftM22 = left.Row1.y;
        float leftM23 = left.Row1.z;
        float leftM31 = left.Row2.x;
        float leftM32 = left.Row2.y;
        float leftM33 = left.Row2.z;
        float rightM11 = right.Row0.x;
        float rightM12 = right.Row0.y;
        float rightM13 = right.Row0.z;
        float rightM21 = right.Row1.x;
        float rightM22 = right.Row1.y;
        float rightM23 = right.Row1.z;
        float rightM31 = right.Row2.x;
        float rightM32 = right.Row2.y;
        float rightM33 = right.Row2.z;

        result.Row0.x = (leftM11 * rightM11) + (leftM12 * rightM21) + (leftM13 * rightM31);
        result.Row0.y = (leftM11 * rightM12) + (leftM12 * rightM22) + (leftM13 * rightM32);
        result.Row0.z = (leftM11 * rightM13) + (leftM12 * rightM23) + (leftM13 * rightM33);
        result.Row1.x = (leftM21 * rightM11) + (leftM22 * rightM21) + (leftM23 * rightM31);
        result.Row1.y = (leftM21 * rightM12) + (leftM22 * rightM22) + (leftM23 * rightM32);
        result.Row1.z = (leftM21 * rightM13) + (leftM22 * rightM23) + (leftM23 * rightM33);
        result.Row2.x = (leftM31 * rightM11) + (leftM32 * rightM21) + (leftM33 * rightM31);
        result.Row2.y = (leftM31 * rightM12) + (leftM32 * rightM22) + (leftM33 * rightM32);
        result.Row2.z = (leftM31 * rightM13) + (leftM32 * rightM23) + (leftM33 * rightM33);
    }

    /// <summary>
    /// Calculate the inverse of the given matrix.
    /// </summary>
    /// <param name="matrix">The matrix to invert.</param>
    /// <param name="result">The inverse of the given matrix if it has one, or the input if it is singular.</param>
    /// <exception cref="InvalidOperationException">Thrown if the Matrix3 is singular.</exception>
    public static void Invert(in Matrix3x3 matrix, out Matrix3x3 result)
    {
        // Original implementation can be found here:
        // https://github.com/niswegmann/small-matrix-inverse/blob/6eac02b84ad06870692abaf828638a391548502c/invert3x3_c.h
        float row0X = matrix.Row0.x, row0Y = matrix.Row0.y, row0Z = matrix.Row0.z;
        float row1X = matrix.Row1.x, row1Y = matrix.Row1.y, row1Z = matrix.Row1.z;
        float row2X = matrix.Row2.x, row2Y = matrix.Row2.y, row2Z = matrix.Row2.z;

        // Compute the elements needed to calculate the determinant
        // so that we can throw without writing anything to the out parameter.
        float invRow0X = (+row1Y * row2Z) - (row1Z * row2Y);
        float invRow1X = (-row1X * row2Z) + (row1Z * row2X);
        float invRow2X = (+row1X * row2Y) - (row1Y * row2X);

        // Compute determinant:
        float det = (row0X * invRow0X) + (row0Y * invRow1X) + (row0Z * invRow2X);

        if (det == 0f)
        {
            throw new InvalidOperationException("Matrix is singular and cannot be inverted.");
        }

        // Compute adjoint:
        result.Row0.x = invRow0X;
        result.Row0.y = (-row0Y * row2Z) + (row0Z * row2Y);
        result.Row0.z = (+row0Y * row1Z) - (row0Z * row1Y);
        result.Row1.x = invRow1X;
        result.Row1.y = (+row0X * row2Z) - (row0Z * row2X);
        result.Row1.z = (-row0X * row1Z) + (row0Z * row1X);
        result.Row2.x = invRow2X;
        result.Row2.y = (-row0X * row2Y) + (row0Y * row2X);
        result.Row2.z = (+row0X * row1Y) - (row0Y * row1X);

        // Multiply adjoint with reciprocal of determinant:
        det = 1.0f / det;

        result.Row0.x *= det;
        result.Row0.y *= det;
        result.Row0.z *= det;
        result.Row1.x *= det;
        result.Row1.y *= det;
        result.Row1.z *= det;
        result.Row2.x *= det;
        result.Row2.y *= det;
        result.Row2.z *= det;
    }

    /// <summary>
    /// Calculate the inverse of the given matrix.
    /// </summary>
    /// <param name="mat">The matrix to invert.</param>
    /// <returns>The inverse of the given matrix.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the Matrix4 is singular.</exception>
    [Pure]
    public static Matrix3x3 Invert(Matrix3x3 mat)
    {
        Invert(in mat, out Matrix3x3 result);
        return result;
    }

    /// <summary>
    /// Calculate the transpose of the given matrix.
    /// </summary>
    /// <param name="mat">The matrix to transpose.</param>
    /// <param name="result">The result of the calculation.</param>
    public static void Transpose(in Matrix3x3 mat, out Matrix3x3 result)
    {
        result.Row0.x = mat.Row0.x;
        result.Row0.y = mat.Row1.x;
        result.Row0.z = mat.Row2.x;
        result.Row1.x = mat.Row0.y;
        result.Row1.y = mat.Row1.y;
        result.Row1.z = mat.Row2.y;
        result.Row2.x = mat.Row0.z;
        result.Row2.y = mat.Row1.z;
        result.Row2.z = mat.Row2.z;
    }

    /// <summary>
    /// Calculate the transpose of the given matrix.
    /// </summary>
    /// <param name="mat">The matrix to transpose.</param>
    /// <returns>The transpose of the given matrix.</returns>
    [Pure]
    public static Matrix3x3 Transpose(Matrix3x3 mat)
    {
        return new Matrix3x3(mat.Column0, mat.Column1, mat.Column2);
    }

    /// <summary>
    /// Swizzles a matrix, i.e. switches rows of the matrix.
    /// </summary>
    /// <param name="mat">The matrix to swizzle.</param>
    /// <param name="rowForRow0">Which row to place in <see cref="Row0"/>.</param>
    /// <param name="rowForRow1">Which row to place in <see cref="Row1"/>.</param>
    /// <param name="rowForRow2">Which row to place in <see cref="Row2"/>.</param>
    /// <returns>The swizzled matrix.</returns>
    /// <exception cref="IndexOutOfRangeException">If any of the rows are outside of the range [0, 2].</exception>
    public static Matrix3x3 Swizzle(Matrix3x3 mat, int rowForRow0, int rowForRow1, int rowForRow2)
    {
        Swizzle(mat, rowForRow0, rowForRow1, rowForRow2, out Matrix3x3 result);
        return result;
    }

    /// <summary>
    /// Swizzles a matrix, i.e. switches rows of the matrix.
    /// </summary>
    /// <param name="mat">The matrix to swizzle.</param>
    /// <param name="rowForRow0">Which row to place in <see cref="Row0"/>.</param>
    /// <param name="rowForRow1">Which row to place in <see cref="Row1"/>.</param>
    /// <param name="rowForRow2">Which row to place in <see cref="Row2"/>.</param>
    /// <param name="result">The swizzled matrix.</param>
    /// <exception cref="IndexOutOfRangeException">If any of the rows are outside of the range [0, 2].</exception>
    public static void Swizzle(in Matrix3x3 mat, int rowForRow0, int rowForRow1, int rowForRow2, out Matrix3x3 result)
    {
        result.Row0 = rowForRow0 switch
        {
            0 => mat.Row0,
            1 => mat.Row1,
            2 => mat.Row2,
            _ => throw new IndexOutOfRangeException($"{nameof(rowForRow0)} must be a number between 0 and 2. Got {rowForRow0}."),
        };

        result.Row1 = rowForRow1 switch
        {
            0 => mat.Row0,
            1 => mat.Row1,
            2 => mat.Row2,
            _ => throw new IndexOutOfRangeException($"{nameof(rowForRow1)} must be a number between 0 and 2. Got {rowForRow1}."),
        };

        result.Row2 = rowForRow2 switch
        {
            0 => mat.Row0,
            1 => mat.Row1,
            2 => mat.Row2,
            _ => throw new IndexOutOfRangeException($"{nameof(rowForRow2)} must be a number between 0 and 2. Got {rowForRow2}."),
        };
    }

    /// <summary>
    /// Matrix multiplication.
    /// </summary>
    /// <param name="left">left-hand operand.</param>
    /// <param name="right">right-hand operand.</param>
    /// <returns>A new Matrix3d which holds the result of the multiplication.</returns>
    [Pure]
    public static Matrix3x3 operator *(Matrix3x3 left, Matrix3x3 right)
    {
        return Mult(left, right);
    }

    /// <summary>
    /// Compares two instances for equality.
    /// </summary>
    /// <param name="left">The first instance.</param>
    /// <param name="right">The second instance.</param>
    /// <returns>True, if left equals right; false otherwise.</returns>
    [Pure]
    public static bool operator ==(Matrix3x3 left, Matrix3x3 right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Compares two instances for inequality.
    /// </summary>
    /// <param name="left">The first instance.</param>
    /// <param name="right">The second instance.</param>
    /// <returns>True, if left does not equal right; false otherwise.</returns>
    [Pure]
    public static bool operator !=(Matrix3x3 left, Matrix3x3 right)
    {
        return !left.Equals(right);
    }

    /// <summary>
    /// Returns a System.String that represents the current Matrix3d.
    /// </summary>
    /// <returns>The string representation of the matrix.</returns>
    public override readonly string ToString()
    {
        return ToString(null, null);
    }

    /// <inheritdoc cref="ToString(string, IFormatProvider)"/>
    public readonly string ToString(string format)
    {
        return ToString(format, null);
    }

    /// <inheritdoc cref="ToString(string, IFormatProvider)"/>
    public readonly string ToString(IFormatProvider formatProvider)
    {
        return ToString(null, formatProvider);
    }

    /// <inheritdoc/>
    public readonly string ToString(string format, IFormatProvider formatProvider)
    {
        var row0 = Row0.ToString(format, formatProvider);
        var row1 = Row1.ToString(format, formatProvider);
        var row2 = Row2.ToString(format, formatProvider);
        return $"{row0}\n{row1}\n{row2}";
    }

    /// <summary>
    /// Returns the hashcode for this instance.
    /// </summary>
    /// <returns>A System.Int32 containing the unique hashcode for this instance.</returns>
    public override readonly int GetHashCode()
    {
        return HashCode.Combine(Row0, Row1, Row2);
    }

    /// <summary>
    /// Indicates whether this instance and a specified object are equal.
    /// </summary>
    /// <param name="obj">The object to compare to.</param>
    /// <returns>True if the instances are equal; false otherwise.</returns>
    [Pure]
    public override readonly bool Equals(object obj)
    {
        return obj is Matrix3x3 && Equals((Matrix3x3)obj);
    }

    /// <summary>
    /// Indicates whether the current matrix is equal to another matrix.
    /// </summary>
    /// <param name="other">A matrix to compare with this matrix.</param>
    /// <returns>true if the current matrix is equal to the matrix parameter; otherwise, false.</returns>
    [Pure]
    public readonly bool Equals(Matrix3x3 other)
    {
        return
            Row0 == other.Row0 &&
            Row1 == other.Row1 &&
            Row2 == other.Row2;
    }
}
