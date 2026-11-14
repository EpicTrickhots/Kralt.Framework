using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
#if NETCOREAPP3_1_OR_GREATER
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
#endif

namespace Kralt.Mathematics;

/// <summary>
/// Represents a 4x4 matrix containing 3D rotation, scale, transform, and projection.
/// </summary>
/// <seealso cref="Matrix4d"/>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct Matrix4x4 : IEquatable<Matrix4x4>, IFormattable
{
    /// <summary>
    /// Top row of the matrix.
    /// </summary>
    public Vector4 Row0;

    /// <summary>
    /// 2nd row of the matrix.
    /// </summary>
    public Vector4 Row1;

    /// <summary>
    /// 3rd row of the matrix.
    /// </summary>
    public Vector4 Row2;

    /// <summary>
    /// Bottom row of the matrix.
    /// </summary>
    public Vector4 Row3;

    /// <summary>
    /// The identity matrix.
    /// </summary>
    public static readonly Matrix4x4 Identity = new Matrix4x4(Vector4.UnitX, Vector4.UnitY, Vector4.UnitZ, Vector4.UnitW);

    /// <summary>
    /// The zero matrix.
    /// </summary>
    public static readonly Matrix4x4 Zero = new Matrix4x4(Vector4.Zero, Vector4.Zero, Vector4.Zero, Vector4.Zero);

    /// <summary>
    /// Initializes a new instance of the <see cref="Matrix4x4"/> struct.
    /// </summary>
    /// <param name="row0">Top row of the matrix.</param>
    /// <param name="row1">Second row of the matrix.</param>
    /// <param name="row2">Third row of the matrix.</param>
    /// <param name="row3">Bottom row of the matrix.</param>
    public Matrix4x4(Vector4 row0, Vector4 row1, Vector4 row2, Vector4 row3)
    {
        Row0 = row0;
        Row1 = row1;
        Row2 = row2;
        Row3 = row3;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Matrix4x4"/> struct.
    /// </summary>
    /// <param name="m00">First item of the first row of the matrix.</param>
    /// <param name="m01">Second item of the first row of the matrix.</param>
    /// <param name="m02">Third item of the first row of the matrix.</param>
    /// <param name="m03">Fourth item of the first row of the matrix.</param>
    /// <param name="m10">First item of the second row of the matrix.</param>
    /// <param name="m11">Second item of the second row of the matrix.</param>
    /// <param name="m12">Third item of the second row of the matrix.</param>
    /// <param name="m13">Fourth item of the second row of the matrix.</param>
    /// <param name="m20">First item of the third row of the matrix.</param>
    /// <param name="m21">Second item of the third row of the matrix.</param>
    /// <param name="m22">Third item of the third row of the matrix.</param>
    /// <param name="m23">Fourth item of the third row of the matrix.</param>
    /// <param name="m30">First item of the fourth row of the matrix.</param>
    /// <param name="m31">Second item of the fourth row of the matrix.</param>
    /// <param name="m32">Third item of the fourth row of the matrix.</param>
    /// <param name="m33">Fourth item of the fourth row of the matrix.</param>
    [SuppressMessage("ReSharper", "SA1117", Justification = "For better readability of Matrix struct.")]
    public Matrix4x4
    (
        float m00, float m01, float m02, float m03,
        float m10, float m11, float m12, float m13,
        float m20, float m21, float m22, float m23,
        float m30, float m31, float m32, float m33
    )
    {
        Row0 = new Vector4(m00, m01, m02, m03);
        Row1 = new Vector4(m10, m11, m12, m13);
        Row2 = new Vector4(m20, m21, m22, m23);
        Row3 = new Vector4(m30, m31, m32, m33);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Matrix4x4"/> struct.
    /// </summary>
    /// <param name="topLeft">The top left 3x3 of the matrix.</param>
    public Matrix4x4(Matrix3x3 topLeft)
    {
        Row0.x = topLeft.Row0.x;
        Row0.y = topLeft.Row0.y;
        Row0.z = topLeft.Row0.z;
        Row0.w = 0;
        Row1.x = topLeft.Row1.x;
        Row1.y = topLeft.Row1.y;
        Row1.z = topLeft.Row1.z;
        Row1.w = 0;
        Row2.x = topLeft.Row2.x;
        Row2.y = topLeft.Row2.y;
        Row2.z = topLeft.Row2.z;
        Row2.w = 0;
        Row3.x = 0;
        Row3.y = 0;
        Row3.z = 0;
        Row3.w = 1;
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
            float m14 = Row0.w;
            float m21 = Row1.x;
            float m22 = Row1.y;
            float m23 = Row1.z;
            float m24 = Row1.w;
            float m31 = Row2.x;
            float m32 = Row2.y;
            float m33 = Row2.z;
            float m34 = Row2.w;
            float m41 = Row3.x;
            float m42 = Row3.y;
            float m43 = Row3.z;
            float m44 = Row3.w;

            return
                (m11 * m22 * m33 * m44) - (m11 * m22 * m34 * m43) + (m11 * m23 * m34 * m42) - (m11 * m23 * m32 * m44)
                + (m11 * m24 * m32 * m43) - (m11 * m24 * m33 * m42) - (m12 * m23 * m34 * m41) + (m12 * m23 * m31 * m44)
                - (m12 * m24 * m31 * m43) + (m12 * m24 * m33 * m41) - (m12 * m21 * m33 * m44) + (m12 * m21 * m34 * m43)
                                                                                        + (m13 * m24 * m31 * m42) -
                (m13 * m24 * m32 * m41) + (m13 * m21 * m32 * m44) - (m13 * m21 * m34 * m42)
                + (m13 * m22 * m34 * m41) - (m13 * m22 * m31 * m44) - (m14 * m21 * m32 * m43) + (m14 * m21 * m33 * m42)
                - (m14 * m22 * m33 * m41) + (m14 * m22 * m31 * m43) - (m14 * m23 * m31 * m42) + (m14 * m23 * m32 * m41);
        }
    }

    /// <summary>
    /// Gets or sets the first column of this matrix.
    /// </summary>
    public Vector4 Column0
    {
        readonly get => new Vector4(Row0.x, Row1.x, Row2.x, Row3.x);
        set
        {
            Row0.x = value.x;
            Row1.x = value.y;
            Row2.x = value.z;
            Row3.x = value.w;
        }
    }

    /// <summary>
    /// Gets or sets the second column of this matrix.
    /// </summary>
    public Vector4 Column1
    {
        readonly get => new Vector4(Row0.y, Row1.y, Row2.y, Row3.y);
        set
        {
            Row0.y = value.x;
            Row1.y = value.y;
            Row2.y = value.z;
            Row3.y = value.w;
        }
    }

    /// <summary>
    /// Gets or sets the third column of this matrix.
    /// </summary>
    public Vector4 Column2
    {
        readonly get => new Vector4(Row0.z, Row1.z, Row2.z, Row3.z);
        set
        {
            Row0.z = value.x;
            Row1.z = value.y;
            Row2.z = value.z;
            Row3.z = value.w;
        }
    }

    /// <summary>
    /// Gets or sets the fourth column of this matrix.
    /// </summary>
    public Vector4 Column3
    {
        readonly get => new Vector4(Row0.w, Row1.w, Row2.w, Row3.w);
        set
        {
            Row0.w = value.x;
            Row1.w = value.y;
            Row2.w = value.z;
            Row3.w = value.w;
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
    /// Gets or sets the value at row 1, column 4 of this instance.
    /// </summary>
    public float M14
    {
        readonly get => Row0.w;
        set => Row0.w = value;
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
    /// Gets or sets the value at row 2, column 4 of this instance.
    /// </summary>
    public float M24
    {
        readonly get => Row1.w;
        set => Row1.w = value;
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
    /// Gets or sets the value at row 3, column 4 of this instance.
    /// </summary>
    public float M34
    {
        readonly get => Row2.w;
        set => Row2.w = value;
    }

    /// <summary>
    /// Gets or sets the value at row 4, column 1 of this instance.
    /// </summary>
    public float M41
    {
        readonly get => Row3.x;
        set => Row3.x = value;
    }

    /// <summary>
    /// Gets or sets the value at row 4, column 2 of this instance.
    /// </summary>
    public float M42
    {
        readonly get => Row3.y;
        set => Row3.y = value;
    }

    /// <summary>
    /// Gets or sets the value at row 4, column 3 of this instance.
    /// </summary>
    public float M43
    {
        readonly get => Row3.z;
        set => Row3.z = value;
    }

    /// <summary>
    /// Gets or sets the value at row 4, column 4 of this instance.
    /// </summary>
    public float M44
    {
        readonly get => Row3.w;
        set => Row3.w = value;
    }

    /// <summary>
    /// Gets or sets the values along the main diagonal of the matrix.
    /// </summary>
    public Vector4 Diagonal
    {
        readonly get => new Vector4(Row0.x, Row1.y, Row2.z, Row3.w);
        set
        {
            Row0.x = value.x;
            Row1.y = value.y;
            Row2.z = value.z;
            Row3.w = value.w;
        }
    }

    /// <summary>
    /// Gets the trace of the matrix, the sum of the values along the diagonal.
    /// </summary>
    public readonly float Trace => Row0.x + Row1.y + Row2.z + Row3.w;

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
            if (((uint)rowIndex) < 4 && ((uint)columnIndex) < 4)
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
            if (((uint)rowIndex) < 4 && ((uint)columnIndex) < 4)
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
    private readonly ref Vector4 GetRowUnsafe(in Matrix4x4 m, int index)
    {
        ref Vector4 address = ref Unsafe.AsRef(in m.Row0);
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
    public readonly Matrix4x4 Inverted()
    {
        var m = this;
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
    public readonly Matrix4x4 Transposed()
    {
        Matrix4x4 m = this;
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
        Row3 /= determinant;
    }

    /// <summary>
    /// Returns a normalized copy of this instance.
    /// </summary>
    /// <returns>The normalized copy.</returns>
    public readonly Matrix4x4 Normalized()
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
    /// <param name="rowForRow3">Which row to place in <see cref="Row3"/>.</param>
    public void Swizzle(int rowForRow0, int rowForRow1, int rowForRow2, int rowForRow3)
    {
        this = Swizzle(this, rowForRow0, rowForRow1, rowForRow2, rowForRow3);
    }

    /// <summary>
    /// Returns a swizzled copy of this instance.
    /// </summary>
    /// <param name="rowForRow0">Which row to place in <see cref="Row0"/>.</param>
    /// <param name="rowForRow1">Which row to place in <see cref="Row1"/>.</param>
    /// <param name="rowForRow2">Which row to place in <see cref="Row2"/>.</param>
    /// <param name="rowForRow3">Which row to place in <see cref="Row3"/>.</param>
    /// <returns>The swizzled copy.</returns>
    public readonly Matrix4x4 Swizzled(int rowForRow0, int rowForRow1, int rowForRow2, int rowForRow3)
    {
        Matrix4x4 m = this;
        m.Swizzle(rowForRow0, rowForRow1, rowForRow2, rowForRow3);
        return m;
    }

    /// <summary>
    /// Returns a copy of this Matrix4 without translation.
    /// </summary>
    /// <returns>The matrix without translation.</returns>
    public readonly Matrix4x4 ClearTranslation()
    {
        var m = this;
        m.Row3.Xyz = Vector3.zero;
        return m;
    }

    /// <summary>
    /// Returns a copy of this Matrix4 without scale.
    /// </summary>
    /// <returns>The matrix without scaling.</returns>
    public readonly Matrix4x4 ClearScale()
    {
        var m = this;
        m.Row0.Xyz = m.Row0.Xyz.Normalized();
        m.Row1.Xyz = m.Row1.Xyz.Normalized();
        m.Row2.Xyz = m.Row2.Xyz.Normalized();
        return m;
    }

    /// <summary>
    /// Returns a copy of this Matrix4 without rotation.
    /// </summary>
    /// <returns>The matrix without rotation.</returns>
    public readonly Matrix4x4 ClearRotation()
    {
        var m = this;
        m.Row0.Xyz = new Vector3(m.Row0.Xyz.Length, 0, 0);
        m.Row1.Xyz = new Vector3(0, m.Row1.Xyz.Length, 0);
        m.Row2.Xyz = new Vector3(0, 0, m.Row2.Xyz.Length);
        return m;
    }

    /// <summary>
    /// Returns a copy of this Matrix4 without projection. (equivalent to setting <see cref="Column3"/> = (0, 0, 0, 0)).
    /// </summary>
    /// <returns>The matrix without projection.</returns>
    [Obsolete("This function doesn't actually clear the projection of the matrix. This is equivalent of setting Column3 = (0, 0, 0, 0).")]
    public readonly Matrix4x4 ClearProjection()
    {
        var m = this;
        m.Column3 = Vector4.Zero;
        return m;
    }

    /// <summary>
    /// Returns the translation component of this instance.
    /// </summary>
    /// <returns>The translation.</returns>
    public readonly Vector3 ExtractTranslation()
    {
        return Row3.Xyz;
    }

    /// <summary>
    /// Returns the scale component of this instance.
    /// </summary>
    /// <returns>The scale.</returns>
    public readonly Vector3 ExtractScale()
    {
        return new Vector3(Row0.Xyz.Length, Row1.Xyz.Length, Row2.Xyz.Length);
    }

    /// <summary>
    /// Returns the rotation component of this instance. Quite slow.
    /// </summary>
    /// <param name="rowNormalize">
    /// Whether the method should row-normalize (i.e. remove scale from) the Matrix. Pass false if
    /// you know it's already normalized.
    /// </param>
    /// <returns>The rotation.</returns>
    [Pure]
    public readonly Quaternion ExtractRotation(bool rowNormalize = true)
    {
        var row0 = Row0.Xyz;
        var row1 = Row1.Xyz;
        var row2 = Row2.Xyz;

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
    /// Returns the projection component of this instance (equivalent to <see cref="Column3"/>).
    /// </summary>
    /// <returns>The projection (<see cref="Column3"/>).</returns>
    [Obsolete("Use Column3 if the old behaviour is needed, or use ExtractPerspective*() or ExtractOrthographic* instead.")]
    public readonly Vector4 ExtractProjection()
    {
        return Column3;
    }

    /// <summary>
    /// Returns the off-center projection parameters of this instance.
    /// This only works if the matrix was created using <see cref="CreatePerspectiveOffCenter(float, float, float, float, float, float)"/>.
    /// </summary>
    /// <param name="left">The left edge of the view frustum.</param>
    /// <param name="right">The right edge of the view frustum.</param>
    /// <param name="bottom">The bottom edge of the view frustum.</param>
    /// <param name="top">The top edge of the view frustum.</param>
    /// <param name="depthNear">The distance to the near clip plane.</param>
    /// <param name="depthFar">The distance to the far clip plane.</param>
    public readonly void ExtractPerspectiveOffCenter
    (
        out float left,
        out float right,
        out float bottom,
        out float top,
        out float depthNear,
        out float depthFar
    )
    {
        depthNear = Row3.z / (Row2.z - 1);
        depthFar = Row3.z / (Row2.z + 1);
        left = depthNear * (Row2.x - 1) / Row0.x;
        right = depthNear * (Row2.x + 1) / Row0.x;
        bottom = depthNear * (Row2.y - 1) / Row1.y;
        top = depthNear * (Row2.y + 1) / Row1.y;
    }

    /// <summary>
    /// Returns the field of view projection parameters of this instance.
    /// This only works if the matrix was created using <see cref="CreatePerspectiveFieldOfView(float, float, float, float)"/>.
    /// </summary>
    /// <param name="fovy">Angle of the field of view in the y direction (in radians).</param>
    /// <param name="aspect">Aspect ratio of the view (width / height).</param>
    /// <param name="depthNear">The distance to the near clip plane.</param>
    /// <param name="depthFar">The distance to the far clip plane.</param>
    public readonly void ExtractPerspectiveFieldOfView(out float fovy, out float aspect, out float depthNear, out float depthFar)
    {
        fovy = 2.0f * MathF.Atan(1 / Row1.y);
        aspect = Row1.y / Row0.x;
        depthNear = Row3.z / (Row2.z - 1);
        depthFar = Row3.z / (Row2.z + 1);
    }

    /// <summary>
    /// Returns the off-center orthographic projection parameters of this instance.
    /// This only works if the matrix was created using <see cref="CreateOrthographicOffCenter(float, float, float, float, float, float)"/>.
    /// </summary>
    /// <param name="left">The left edge of the projection volume.</param>
    /// <param name="right">The right edge of the projection volume.</param>
    /// <param name="bottom">The bottom edge of the projection volume.</param>
    /// <param name="top">The top edge of the projection volume.</param>
    /// <param name="depthNear">The distance to the near clip plane.</param>
    /// <param name="depthFar">The distance to the far clip plane.</param>
    public readonly void ExtractOrthographicOffCenter
    (
        out float left,
        out float right,
        out float bottom,
        out float top,
        out float depthNear,
        out float depthFar
    )
    {
        left = -(1 + Row3.x) / Row0.x;
        right = (1 - Row3.x) / Row0.x;
        bottom = -(1 + Row3.y) / Row1.y;
        top = (1 - Row3.y) / Row1.y;
        depthNear = (1 + Row3.z) / Row2.z;
        depthFar = -(1 - Row3.z) / Row2.z;
    }

    /// <summary>
    /// Returns the orthographic projection parameters of this instance.
    /// This only works if the matrix was created using <see cref="CreateOrthographic(float, float, float, float)"/>.
    /// </summary>
    /// <param name="width">The width of the projection volume.</param>
    /// <param name="height">The height of the projection volume.</param>
    /// <param name="depthNear">The distance to the near clip plane.</param>
    /// <param name="depthFar">The distance to the far clip plane.</param>
    public readonly void ExtractOrthographic(out float width, out float height, out float depthNear, out float depthFar)
    {
        float left = -(1 + Row3.x) / Row0.x;
        float right = (1 - Row3.x) / Row0.x;
        width = right - left;
        float bottom = -(1 + Row3.y) / Row1.y;
        float top = (1 - Row3.y) / Row1.y;
        height = top - bottom;
        depthNear = (1 + Row3.z) / Row2.z;
        depthFar = -(1 - Row3.z) / Row2.z;
    }

    /// <summary>
    /// Build a rotation matrix from the specified axis/angle rotation.
    /// </summary>
    /// <param name="axis">The axis to rotate about.</param>
    /// <param name="angle">Angle in radians to rotate counter-clockwise (looking in the direction of the given axis).</param>
    /// <param name="result">A matrix instance.</param>
    public static void CreateFromAxisAngle(Vector3 axis, float angle, out Matrix4x4 result)
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
        result.Row0.w = 0;
        result.Row1.x = tXY + sinZ;
        result.Row1.y = tYY + cos;
        result.Row1.z = tYZ - sinX;
        result.Row1.w = 0;
        result.Row2.x = tXZ - sinY;
        result.Row2.y = tYZ + sinX;
        result.Row2.z = tZZ + cos;
        result.Row2.w = 0;
        result.Row3 = Vector4.UnitW;
    }

    /// <summary>
    /// Build a rotation matrix from the specified axis/angle rotation.
    /// </summary>
    /// <param name="axis">The axis to rotate about.</param>
    /// <param name="angle">Angle in radians to rotate counter-clockwise (looking in the direction of the given axis).</param>
    /// <returns>A matrix instance.</returns>
    [Pure]
    public static Matrix4x4 CreateFromAxisAngle(Vector3 axis, float angle)
    {
        CreateFromAxisAngle(axis, angle, out Matrix4x4 result);
        return result;
    }

    /// <summary>
    /// Builds a rotation matrix from a quaternion.
    /// </summary>
    /// <param name="q">The quaternion to rotate by.</param>
    /// <param name="result">A matrix instance.</param>
    public static void CreateFromQuaternion(in Quaternion q, out Matrix4x4 result)
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

        result.Row0.w = 0;
        result.Row1.w = 0;
        result.Row2.w = 0;
        result.Row3 = new Vector4(0, 0, 0, 1);
    }

    /// <summary>
    /// Builds a rotation matrix from a quaternion.
    /// </summary>
    /// <param name="q">The quaternion to rotate by.</param>
    /// <returns>A matrix instance.</returns>
    [Pure]
    public static Matrix4x4 CreateFromQuaternion(Quaternion q)
    {
        CreateFromQuaternion(in q, out Matrix4x4 result);
        return result;
    }

    /// <summary>
    /// Builds a rotation matrix for a rotation around the x-axis.
    /// </summary>
    /// <param name="angle">The counter-clockwise angle in radians.</param>
    /// <param name="result">The resulting Matrix4 instance.</param>
    public static void CreateRotationX(float angle, out Matrix4x4 result)
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
    /// <returns>The resulting Matrix4 instance.</returns>
    [Pure]
    public static Matrix4x4 CreateRotationX(float angle)
    {
        CreateRotationX(angle, out Matrix4x4 result);
        return result;
    }

    /// <summary>
    /// Builds a rotation matrix for a rotation around the y-axis.
    /// </summary>
    /// <param name="angle">The counter-clockwise angle in radians.</param>
    /// <param name="result">The resulting Matrix4 instance.</param>
    public static void CreateRotationY(float angle, out Matrix4x4 result)
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
    /// <returns>The resulting Matrix4 instance.</returns>
    [Pure]
    public static Matrix4x4 CreateRotationY(float angle)
    {
        CreateRotationY(angle, out Matrix4x4 result);
        return result;
    }

    /// <summary>
    /// Builds a rotation matrix for a rotation around the z-axis.
    /// </summary>
    /// <param name="angle">The counter-clockwise angle in radians.</param>
    /// <param name="result">The resulting Matrix4 instance.</param>
    public static void CreateRotationZ(float angle, out Matrix4x4 result)
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
    /// <returns>The resulting Matrix4 instance.</returns>
    [Pure]
    public static Matrix4x4 CreateRotationZ(float angle)
    {
        CreateRotationZ(angle, out Matrix4x4 result);
        return result;
    }

    /// <summary>
    /// Creates a translation matrix.
    /// </summary>
    /// <param name="x">X translation.</param>
    /// <param name="y">Y translation.</param>
    /// <param name="z">Z translation.</param>
    /// <param name="result">The resulting Matrix4 instance.</param>
    public static void CreateTranslation(float x, float y, float z, out Matrix4x4 result)
    {
        result = Identity;
        result.Row3.x = x;
        result.Row3.y = y;
        result.Row3.z = z;
    }

    /// <summary>
    /// Creates a translation matrix.
    /// </summary>
    /// <param name="vector">The translation vector.</param>
    /// <param name="result">The resulting Matrix4 instance.</param>
    public static void CreateTranslation(in Vector3 vector, out Matrix4x4 result)
    {
        result = Identity;
        result.Row3.x = vector.x;
        result.Row3.y = vector.y;
        result.Row3.z = vector.z;
    }

    /// <summary>
    /// Creates a translation matrix.
    /// </summary>
    /// <param name="x">X translation.</param>
    /// <param name="y">Y translation.</param>
    /// <param name="z">Z translation.</param>
    /// <returns>The resulting Matrix4 instance.</returns>
    [Pure]
    public static Matrix4x4 CreateTranslation(float x, float y, float z)
    {
        CreateTranslation(x, y, z, out Matrix4x4 result);
        return result;
    }

    /// <summary>
    /// Creates a translation matrix.
    /// </summary>
    /// <param name="vector">The translation vector.</param>
    /// <returns>The resulting Matrix4 instance.</returns>
    [Pure]
    public static Matrix4x4 CreateTranslation(Vector3 vector)
    {
        CreateTranslation(vector.x, vector.y, vector.z, out Matrix4x4 result);
        return result;
    }

    /// <summary>
    /// Creates a scale matrix.
    /// </summary>
    /// <param name="scale">Single scale factor for the x, y, and z axes.</param>
    /// <returns>A scale matrix.</returns>
    [Pure]
    public static Matrix4x4 CreateScale(float scale)
    {
        CreateScale(scale, out Matrix4x4 result);
        return result;
    }

    /// <summary>
    /// Creates a scale matrix.
    /// </summary>
    /// <param name="scale">Scale factors for the x, y, and z axes.</param>
    /// <returns>A scale matrix.</returns>
    [Pure]
    public static Matrix4x4 CreateScale(Vector3 scale)
    {
        CreateScale(in scale, out Matrix4x4 result);
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
    public static Matrix4x4 CreateScale(float x, float y, float z)
    {
        CreateScale(x, y, z, out Matrix4x4 result);
        return result;
    }

    /// <summary>
    /// Creates a scale matrix.
    /// </summary>
    /// <param name="scale">Single scale factor for the x, y, and z axes.</param>
    /// <param name="result">A scale matrix.</param>
    public static void CreateScale(float scale, out Matrix4x4 result)
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
    public static void CreateScale(in Vector3 scale, out Matrix4x4 result)
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
    public static void CreateScale(float x, float y, float z, out Matrix4x4 result)
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
    /// <param name="rowForRow3">Which row to place in <see cref="Row3"/>.</param>
    /// <returns>The resulting swizzle matrix.</returns>
    public static Matrix4x4 CreateSwizzle(int rowForRow0, int rowForRow1, int rowForRow2, int rowForRow3)
    {
        CreateSwizzle(rowForRow0, rowForRow1, rowForRow2, rowForRow3, out Matrix4x4 result);
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
    /// <param name="rowForRow3">Which row to place in <see cref="Row3"/>.</param>
    /// <param name="result">The resulting swizzle matrix.</param>
    public static void CreateSwizzle(int rowForRow0, int rowForRow1, int rowForRow2, int rowForRow3, out Matrix4x4 result)
    {
        if (rowForRow0 < 0 || rowForRow0 >= 4)
        {
            throw new IndexOutOfRangeException($"{nameof(rowForRow0)} must be a number between 0 and 3. Got {rowForRow0}.");
        }
        if (rowForRow1 < 0 || rowForRow1 >= 4)
        {
            throw new IndexOutOfRangeException($"{nameof(rowForRow1)} must be a number between 0 and 3. Got {rowForRow1}.");
        }
        if (rowForRow2 < 0 || rowForRow2 >= 4)
        {
            throw new IndexOutOfRangeException($"{nameof(rowForRow2)} must be a number between 0 and 3. Got {rowForRow2}.");
        }
        if (rowForRow3 < 0 || rowForRow3 >= 4)
        {
            throw new IndexOutOfRangeException($"{nameof(rowForRow3)} must be a number between 0 and 3. Got {rowForRow3}.");
        }

        result = Zero;
        result[0, rowForRow0] = 1.0f;
        result[1, rowForRow1] = 1.0f;
        result[2, rowForRow2] = 1.0f;
        result[3, rowForRow3] = 1.0f;
    }

    /// <summary>
    /// Creates an orthographic projection matrix.
    /// </summary>
    /// <param name="width">The width of the projection volume.</param>
    /// <param name="height">The height of the projection volume.</param>
    /// <param name="depthNear">The near edge of the projection volume.</param>
    /// <param name="depthFar">The far edge of the projection volume.</param>
    /// <param name="result">The resulting Matrix4 instance.</param>
    public static void CreateOrthographic(float width, float height, float depthNear, float depthFar, out Matrix4x4 result)
    {
        CreateOrthographicOffCenter(-width / 2, width / 2, -height / 2, height / 2, depthNear, depthFar, out result);
    }

    /// <summary>
    /// Creates an orthographic projection matrix.
    /// </summary>
    /// <param name="width">The width of the projection volume.</param>
    /// <param name="height">The height of the projection volume.</param>
    /// <param name="depthNear">The near edge of the projection volume.</param>
    /// <param name="depthFar">The far edge of the projection volume.</param>
    /// <returns>The resulting Matrix4 instance.</returns>
    [Pure]
    public static Matrix4x4 CreateOrthographic(float width, float height, float depthNear, float depthFar)
    {
        CreateOrthographicOffCenter(-width / 2, width / 2, -height / 2, height / 2, depthNear, depthFar, out Matrix4x4 result);
        return result;
    }

    /// <summary>
    /// Creates an orthographic projection matrix.
    /// </summary>
    /// <param name="left">The left edge of the projection volume.</param>
    /// <param name="right">The right edge of the projection volume.</param>
    /// <param name="bottom">The bottom edge of the projection volume.</param>
    /// <param name="top">The top edge of the projection volume.</param>
    /// <param name="depthNear">The near edge of the projection volume.</param>
    /// <param name="depthFar">The far edge of the projection volume.</param>
    /// <param name="result">The resulting Matrix4 instance.</param>
    public static void CreateOrthographicOffCenter
    (
        float left,
        float right,
        float bottom,
        float top,
        float depthNear,
        float depthFar,
        out Matrix4x4 result
    )
    {
        result = Identity;

        var invRL = 1.0f / (right - left);
        var invTB = 1.0f / (top - bottom);
        var invFN = 1.0f / (depthFar - depthNear);

        result.Row0.x = 2 * invRL;
        result.Row1.y = 2 * invTB;
        result.Row2.z = -2 * invFN;

        result.Row3.x = -(right + left) * invRL;
        result.Row3.y = -(top + bottom) * invTB;
        result.Row3.z = -(depthFar + depthNear) * invFN;
    }

    /// <summary>
    /// Creates an orthographic projection matrix.
    /// </summary>
    /// <param name="left">The left edge of the projection volume.</param>
    /// <param name="right">The right edge of the projection volume.</param>
    /// <param name="bottom">The bottom edge of the projection volume.</param>
    /// <param name="top">The top edge of the projection volume.</param>
    /// <param name="depthNear">The near edge of the projection volume.</param>
    /// <param name="depthFar">The far edge of the projection volume.</param>
    /// <returns>The resulting Matrix4 instance.</returns>
    [Pure]
    public static Matrix4x4 CreateOrthographicOffCenter
    (
        float left,
        float right,
        float bottom,
        float top,
        float depthNear,
        float depthFar
    )
    {
        CreateOrthographicOffCenter(left, right, bottom, top, depthNear, depthFar, out Matrix4x4 result);
        return result;
    }

    /// <summary>
    /// Creates a perspective projection matrix.
    /// </summary>
    /// <param name="fovy">Angle of the field of view in the y direction (in radians).</param>
    /// <param name="aspect">Aspect ratio of the view (width / height).</param>
    /// <param name="depthNear">Distance to the near clip plane.</param>
    /// <param name="depthFar">Distance to the far clip plane.</param>
    /// <param name="result">A projection matrix that transforms camera space to raster space.</param>
    /// <exception cref="System.ArgumentOutOfRangeException">
    /// Thrown under the following conditions:
    ///  <list type="bullet">
    ///  <item>fovy is zero, less than zero or larger than Math.PI</item>
    ///  <item>aspect is negative or zero</item>
    ///  <item>depthNear is negative or zero</item>
    ///  <item>depthFar is negative or zero</item>
    ///  <item>depthNear is larger than depthFar</item>
    ///  </list>
    /// </exception>
    public static void CreatePerspectiveFieldOfView
    (
        float fovy,
        float aspect,
        float depthNear,
        float depthFar,
        out Matrix4x4 result
    )
    {
        if (fovy <= 0 || fovy > MathF.PI)
        {
            throw new ArgumentOutOfRangeException(nameof(fovy), fovy, "Fovy must be in the range [0, PI].");
        }

        if (aspect <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(aspect), aspect, "Aspect cannot be negative.");
        }

        if (depthNear <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(depthNear), depthNear, "depthNear cannot be negative.");
        }

        if (depthFar <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(depthFar), depthFar, "depthFar cannot be negative.");
        }

        float maxY = depthNear * MathF.Tan(0.5f * fovy);
        float minY = -maxY;
        float minX = minY * aspect;
        float maxX = maxY * aspect;

        CreatePerspectiveOffCenter(minX, maxX, minY, maxY, depthNear, depthFar, out result);
    }

    /// <summary>
    /// Creates a perspective projection matrix.
    /// </summary>
    /// <param name="fovy">Angle of the field of view in the y direction (in radians).</param>
    /// <param name="aspect">Aspect ratio of the view (width / height).</param>
    /// <param name="depthNear">Distance to the near clip plane.</param>
    /// <param name="depthFar">Distance to the far clip plane.</param>
    /// <returns>A projection matrix that transforms camera space to raster space.</returns>
    /// <exception cref="System.ArgumentOutOfRangeException">
    /// Thrown under the following conditions:
    ///  <list type="bullet">
    ///  <item>fovy is zero, less than zero or larger than Math.PI</item>
    ///  <item>aspect is negative or zero</item>
    ///  <item>depthNear is negative or zero</item>
    ///  <item>depthFar is negative or zero</item>
    ///  <item>depthNear is larger than depthFar</item>
    ///  </list>
    /// </exception>
    [Pure]
    public static Matrix4x4 CreatePerspectiveFieldOfView(float fovy, float aspect, float depthNear, float depthFar)
    {
        CreatePerspectiveFieldOfView(fovy, aspect, depthNear, depthFar, out Matrix4x4 result);
        return result;
    }

    /// <summary>
    /// Creates an perspective projection matrix.
    /// </summary>
    /// <param name="left">Left edge of the view frustum.</param>
    /// <param name="right">Right edge of the view frustum.</param>
    /// <param name="bottom">Bottom edge of the view frustum.</param>
    /// <param name="top">Top edge of the view frustum.</param>
    /// <param name="depthNear">Distance to the near clip plane.</param>
    /// <param name="depthFar">Distance to the far clip plane.</param>
    /// <param name="result">A projection matrix that transforms camera space to raster space.</param>
    /// <exception cref="System.ArgumentOutOfRangeException">
    /// Thrown under the following conditions:
    ///  <list type="bullet">
    ///  <item>depthNear is negative or zero</item>
    ///  <item>depthFar is negative or zero</item>
    ///  <item>depthNear is larger than depthFar</item>
    ///  </list>
    /// </exception>
    public static void CreatePerspectiveOffCenter
    (
        float left,
        float right,
        float bottom,
        float top,
        float depthNear,
        float depthFar,
        out Matrix4x4 result
    )
    {
        if (depthNear <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(depthNear));
        }

        if (depthFar <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(depthFar));
        }

        if (depthNear >= depthFar)
        {
            throw new ArgumentOutOfRangeException(nameof(depthNear));
        }

        var x = 2.0f * depthNear / (right - left);
        var y = 2.0f * depthNear / (top - bottom);
        var a = (right + left) / (right - left);
        var b = (top + bottom) / (top - bottom);
        var c = -(depthFar + depthNear) / (depthFar - depthNear);
        var d = -(2.0f * depthFar * depthNear) / (depthFar - depthNear);

        result.Row0.x = x;
        result.Row0.y = 0;
        result.Row0.z = 0;
        result.Row0.w = 0;
        result.Row1.x = 0;
        result.Row1.y = y;
        result.Row1.z = 0;
        result.Row1.w = 0;
        result.Row2.x = a;
        result.Row2.y = b;
        result.Row2.z = c;
        result.Row2.w = -1;
        result.Row3.x = 0;
        result.Row3.y = 0;
        result.Row3.z = d;
        result.Row3.w = 0;
    }

    /// <summary>
    /// Creates an perspective projection matrix.
    /// </summary>
    /// <param name="left">Left edge of the view frustum.</param>
    /// <param name="right">Right edge of the view frustum.</param>
    /// <param name="bottom">Bottom edge of the view frustum.</param>
    /// <param name="top">Top edge of the view frustum.</param>
    /// <param name="depthNear">Distance to the near clip plane.</param>
    /// <param name="depthFar">Distance to the far clip plane.</param>
    /// <returns>A projection matrix that transforms camera space to raster space.</returns>
    /// <exception cref="System.ArgumentOutOfRangeException">
    /// Thrown under the following conditions:
    ///  <list type="bullet">
    ///  <item>depthNear is negative or zero</item>
    ///  <item>depthFar is negative or zero</item>
    ///  <item>depthNear is larger than depthFar</item>
    ///  </list>
    /// </exception>
    [Pure]
    public static Matrix4x4 CreatePerspectiveOffCenter
    (
        float left,
        float right,
        float bottom,
        float top,
        float depthNear,
        float depthFar
    )
    {
        CreatePerspectiveOffCenter(left, right, bottom, top, depthNear, depthFar, out Matrix4x4 result);
        return result;
    }

    /// <summary>
    /// Build a world space to camera space matrix.
    /// </summary>
    /// <param name="eye">Eye (camera) position in world space.</param>
    /// <param name="target">Target position in world space.</param>
    /// <param name="up">Up vector in world space (should not be parallel to the camera direction, that is target - eye).</param>
    /// <returns>A Matrix4 that transforms world space to camera space.</returns>
    [Pure]
    public static Matrix4x4 LookAt(Vector3 eye, Vector3 target, Vector3 up)
    {
        var z = Vector3.Normalize(eye - target);
        var x = Vector3.Normalize(Vector3.Cross(up, z));
        var y = Vector3.Normalize(Vector3.Cross(z, x));

        Matrix4x4 result;

        result.Row0.x = x.x;
        result.Row0.y = y.x;
        result.Row0.z = z.x;
        result.Row0.w = 0;
        result.Row1.x = x.y;
        result.Row1.y = y.y;
        result.Row1.z = z.y;
        result.Row1.w = 0;
        result.Row2.x = x.z;
        result.Row2.y = y.z;
        result.Row2.z = z.z;
        result.Row2.w = 0;
        result.Row3.x = -((x.x * eye.x) + (x.y * eye.y) + (x.z * eye.z));
        result.Row3.y = -((y.x * eye.x) + (y.y * eye.y) + (y.z * eye.z));
        result.Row3.z = -((z.x * eye.x) + (z.y * eye.y) + (z.z * eye.z));
        result.Row3.w = 1;

        return result;
    }

    /// <summary>
    /// Build a world space to camera space matrix.
    /// </summary>
    /// <param name="eyeX">Eye (camera) X-position in world space.</param>
    /// <param name="eyeY">Eye (camera) Y-position in world space.</param>
    /// <param name="eyeZ">Eye (camera) Z-position in world space.</param>
    /// <param name="targetX">Target X-position in world space.</param>
    /// <param name="targetY">Target Y-position in world space.</param>
    /// <param name="targetZ">Target Z-position in world space.</param>
    /// <param name="upX">
    /// X of the up vector in world space (should not be parallel to the camera direction, that is target - eye).
    /// </param>
    /// <param name="upY">
    /// Y of the up vector in world space (should not be parallel to the camera direction, that is target - eye).
    /// </param>
    /// <param name="upZ">
    /// Z of the up vector in world space (should not be parallel to the camera direction, that is target - eye).
    /// </param>
    /// <returns>A Matrix4 that transforms world space to camera space.</returns>
    [Pure]
    public static Matrix4x4 LookAt
    (
        float eyeX,
        float eyeY,
        float eyeZ,
        float targetX,
        float targetY,
        float targetZ,
        float upX,
        float upY,
        float upZ
    )
    {
        return LookAt
        (
            new Vector3(eyeX, eyeY, eyeZ),
            new Vector3(targetX, targetY, targetZ),
            new Vector3(upX, upY, upZ)
        );
    }

    /// <summary>
    /// Adds two instances.
    /// </summary>
    /// <param name="left">The left operand of the addition.</param>
    /// <param name="right">The right operand of the addition.</param>
    /// <returns>A new instance that is the result of the addition.</returns>
    [Pure]
    public static Matrix4x4 Add(Matrix4x4 left, Matrix4x4 right)
    {
        Add(in left, in right, out Matrix4x4 result);
        return result;
    }

    /// <summary>
    /// Adds two instances.
    /// </summary>
    /// <param name="left">The left operand of the addition.</param>
    /// <param name="right">The right operand of the addition.</param>
    /// <param name="result">A new instance that is the result of the addition.</param>
    public static void Add(in Matrix4x4 left, in Matrix4x4 right, out Matrix4x4 result)
    {
        result.Row0 = left.Row0 + right.Row0;
        result.Row1 = left.Row1 + right.Row1;
        result.Row2 = left.Row2 + right.Row2;
        result.Row3 = left.Row3 + right.Row3;
    }

    /// <summary>
    /// Subtracts one instance from another.
    /// </summary>
    /// <param name="left">The left operand of the subraction.</param>
    /// <param name="right">The right operand of the subraction.</param>
    /// <returns>A new instance that is the result of the subraction.</returns>
    [Pure]
    public static Matrix4x4 Subtract(Matrix4x4 left, Matrix4x4 right)
    {
        Subtract(in left, in right, out Matrix4x4 result);
        return result;
    }

    /// <summary>
    /// Subtracts one instance from another.
    /// </summary>
    /// <param name="left">The left operand of the subraction.</param>
    /// <param name="right">The right operand of the subraction.</param>
    /// <param name="result">A new instance that is the result of the subraction.</param>
    public static void Subtract(in Matrix4x4 left, in Matrix4x4 right, out Matrix4x4 result)
    {
        result.Row0 = left.Row0 - right.Row0;
        result.Row1 = left.Row1 - right.Row1;
        result.Row2 = left.Row2 - right.Row2;
        result.Row3 = left.Row3 - right.Row3;
    }

    /// <summary>
    /// Multiplies two instances.
    /// </summary>
    /// <param name="left">The left operand of the multiplication.</param>
    /// <param name="right">The right operand of the multiplication.</param>
    /// <returns>A new instance that is the result of the multiplication.</returns>
    [Pure]
    public static Matrix4x4 Mult(Matrix4x4 left, Matrix4x4 right)
    {
        Mult(in left, in right, out Matrix4x4 result);
        return result;
    }

    /// <summary>
    /// Multiplies two instances.
    /// </summary>
    /// <param name="left">The left operand of the multiplication.</param>
    /// <param name="right">The right operand of the multiplication.</param>
    /// <param name="result">A new instance that is the result of the multiplication.</param>
    public static void Mult(in Matrix4x4 left, in Matrix4x4 right, out Matrix4x4 result)
    {
        float leftM11 = left.Row0.x;
        float leftM12 = left.Row0.y;
        float leftM13 = left.Row0.z;
        float leftM14 = left.Row0.w;
        float leftM21 = left.Row1.x;
        float leftM22 = left.Row1.y;
        float leftM23 = left.Row1.z;
        float leftM24 = left.Row1.w;
        float leftM31 = left.Row2.x;
        float leftM32 = left.Row2.y;
        float leftM33 = left.Row2.z;
        float leftM34 = left.Row2.w;
        float leftM41 = left.Row3.x;
        float leftM42 = left.Row3.y;
        float leftM43 = left.Row3.z;
        float leftM44 = left.Row3.w;
        float rightM11 = right.Row0.x;
        float rightM12 = right.Row0.y;
        float rightM13 = right.Row0.z;
        float rightM14 = right.Row0.w;
        float rightM21 = right.Row1.x;
        float rightM22 = right.Row1.y;
        float rightM23 = right.Row1.z;
        float rightM24 = right.Row1.w;
        float rightM31 = right.Row2.x;
        float rightM32 = right.Row2.y;
        float rightM33 = right.Row2.z;
        float rightM34 = right.Row2.w;
        float rightM41 = right.Row3.x;
        float rightM42 = right.Row3.y;
        float rightM43 = right.Row3.z;
        float rightM44 = right.Row3.w;

        result.Row0.x = (leftM11 * rightM11) + (leftM12 * rightM21) + (leftM13 * rightM31) + (leftM14 * rightM41);
        result.Row0.y = (leftM11 * rightM12) + (leftM12 * rightM22) + (leftM13 * rightM32) + (leftM14 * rightM42);
        result.Row0.z = (leftM11 * rightM13) + (leftM12 * rightM23) + (leftM13 * rightM33) + (leftM14 * rightM43);
        result.Row0.w = (leftM11 * rightM14) + (leftM12 * rightM24) + (leftM13 * rightM34) + (leftM14 * rightM44);
        result.Row1.x = (leftM21 * rightM11) + (leftM22 * rightM21) + (leftM23 * rightM31) + (leftM24 * rightM41);
        result.Row1.y = (leftM21 * rightM12) + (leftM22 * rightM22) + (leftM23 * rightM32) + (leftM24 * rightM42);
        result.Row1.z = (leftM21 * rightM13) + (leftM22 * rightM23) + (leftM23 * rightM33) + (leftM24 * rightM43);
        result.Row1.w = (leftM21 * rightM14) + (leftM22 * rightM24) + (leftM23 * rightM34) + (leftM24 * rightM44);
        result.Row2.x = (leftM31 * rightM11) + (leftM32 * rightM21) + (leftM33 * rightM31) + (leftM34 * rightM41);
        result.Row2.y = (leftM31 * rightM12) + (leftM32 * rightM22) + (leftM33 * rightM32) + (leftM34 * rightM42);
        result.Row2.z = (leftM31 * rightM13) + (leftM32 * rightM23) + (leftM33 * rightM33) + (leftM34 * rightM43);
        result.Row2.w = (leftM31 * rightM14) + (leftM32 * rightM24) + (leftM33 * rightM34) + (leftM34 * rightM44);
        result.Row3.x = (leftM41 * rightM11) + (leftM42 * rightM21) + (leftM43 * rightM31) + (leftM44 * rightM41);
        result.Row3.y = (leftM41 * rightM12) + (leftM42 * rightM22) + (leftM43 * rightM32) + (leftM44 * rightM42);
        result.Row3.z = (leftM41 * rightM13) + (leftM42 * rightM23) + (leftM43 * rightM33) + (leftM44 * rightM43);
        result.Row3.w = (leftM41 * rightM14) + (leftM42 * rightM24) + (leftM43 * rightM34) + (leftM44 * rightM44);
    }

    /// <summary>
    /// Multiplies an instance by a scalar.
    /// </summary>
    /// <param name="left">The left operand of the multiplication.</param>
    /// <param name="right">The right operand of the multiplication.</param>
    /// <returns>A new instance that is the result of the multiplication.</returns>
    [Pure]
    public static Matrix4x4 Mult(Matrix4x4 left, float right)
    {
        Mult(in left, right, out Matrix4x4 result);
        return result;
    }

    /// <summary>
    /// Multiplies an instance by a scalar.
    /// </summary>
    /// <param name="left">The left operand of the multiplication.</param>
    /// <param name="right">The right operand of the multiplication.</param>
    /// <param name="result">A new instance that is the result of the multiplication.</param>
    public static void Mult(in Matrix4x4 left, float right, out Matrix4x4 result)
    {
        result.Row0 = left.Row0 * right;
        result.Row1 = left.Row1 * right;
        result.Row2 = left.Row2 * right;
        result.Row3 = left.Row3 * right;
    }

    /// <summary>
    /// Calculate the inverse of the given matrix.
    /// </summary>
    /// <param name="mat">The matrix to invert.</param>
    /// <param name="result">The inverse of the given matrix if it has one, or the input if it is singular.</param>
    /// <exception cref="InvalidOperationException">Thrown if the Matrix4 is singular.</exception>
    public static void Invert(in Matrix4x4 mat, out Matrix4x4 result)
    {
#if NETCOREAPP3_1_OR_GREATER
        if (Sse3.IsSupported)
        {
            InvertSse3(in mat, out result);
        }
        else
        {
            InvertFallback(in mat, out result);
        }
#else
        InvertFallback(in mat, out result);
#endif
    }

#if NETCOREAPP3_1_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe void InvertSse3(in Matrix4x4 mat, out Matrix4x4 result)
    {
#pragma warning disable SA1114 // Parameter list should follow declaration
#pragma warning disable SA1312 // Variable names should begin with lower-case letter
#pragma warning disable SA1512 // Single-line comments should not be followed by blank line
#pragma warning disable SA1515 // Single-line comment should be preceded by blank line

        // Original derivation and implementation can be found here:
        // https://lxjk.github.io/2017/09/03/Fast-4x4-Matrix-Inverse-with-SSE-SIMD-Explained.html

        Vector128<float> row0;
        Vector128<float> row1;
        Vector128<float> row2;
        Vector128<float> row3;

        fixed (float* m = &mat.Row0.x)
        {
            row0 = Sse.LoadVector128(m);
            row1 = Sse.LoadVector128(m + 4);
            row2 = Sse.LoadVector128(m + 8);
            row3 = Sse.LoadVector128(m + 12);
        }

        // __m128 A = VecShuffle_0101(inM.mVec[0], inM.mVec[1]);
        var A = Sse.MoveLowToHigh(row0, row1);
        // __m128 B = VecShuffle_2323(inM.mVec[0], inM.mVec[1]);
        var B = Sse.MoveHighToLow(row1, row0);
        // __m128 C = VecShuffle_0101(inM.mVec[2], inM.mVec[3]);
        var C = Sse.MoveLowToHigh(row2, row3);
        // __m128 D = VecShuffle_2323(inM.mVec[2], inM.mVec[3]);
        var D = Sse.MoveHighToLow(row3, row2);

        const byte Shuffle_0202 = 0b1000_1000;
        const byte Shuffle_1313 = 0b1101_1101;
        // __m128 detSub = _mm_sub_ps(
        var detSub = Sse.Subtract(
            // _mm_mul_ps(VecShuffle(inM.mVec[0], inM.mVec[2], 0, 2, 0, 2), VecShuffle(inM.mVec[1], inM.mVec[3], 1, 3, 1, 3)),
            Sse.Multiply(
                Sse.Shuffle(row0, row2, Shuffle_0202),
                Sse.Shuffle(row1, row3, Shuffle_1313)),
            // _mm_mul_ps(VecShuffle(inM.mVec[0], inM.mVec[2], 1, 3, 1, 3), VecShuffle(inM.mVec[1], inM.mVec[3], 0, 2, 0, 2)));
            Sse.Multiply(
                Sse.Shuffle(row0, row2, Shuffle_1313),
                Sse.Shuffle(row1, row3, Shuffle_0202)));

        const byte Shuffle_0000 = 0b0000_0000;
        const byte Shuffle_1111 = 0b0101_0101;
        const byte Shuffle_2222 = 0b1010_1010;
        const byte Shuffle_3333 = 0b1111_1111;

        // VecSwizzle1(detSub, 0);
        // #define VecSwizzle1(vec, x)                VecSwizzleMask(vec, MakeShuffleMask(x,x,x,x))
        // #define VecSwizzleMask(vec, mask)          _mm_castsi128_ps(_mm_shuffle_epi32(_mm_castps_si128(vec), mask))

        // __m128 detA = VecSwizzle1(detSub, 0);
        var detA = Sse2.Shuffle(detSub.AsInt32(), Shuffle_0000).AsSingle();
        // __m128 detB = VecSwizzle1(detSub, 1);
        var detB = Sse2.Shuffle(detSub.AsInt32(), Shuffle_1111).AsSingle();
        // __m128 detC = VecSwizzle1(detSub, 2);
        var detC = Sse2.Shuffle(detSub.AsInt32(), Shuffle_2222).AsSingle();
        // __m128 detD = VecSwizzle1(detSub, 3);
        var detD = Sse2.Shuffle(detSub.AsInt32(), Shuffle_3333).AsSingle();

        const byte Shuffle_3300 = 0b0000_1111;
        const byte Shuffle_1122 = 0b1010_0101;
        const byte Shuffle_2301 = 0b0100_1110;

        // __m128 D_C = Mat2AdjMul(D, C);
        // Mat2AdjMul(vec1, vec2):
        // _mm_sub_ps(
        var D_C = Sse.Subtract(
            // _mm_mul_ps(VecSwizzle(vec1, 3,3,0,0), vec2),
            Sse.Multiply(Sse2.Shuffle(D.AsInt32(), Shuffle_3300).AsSingle(), C),
            // _mm_mul_ps(
            Sse.Multiply(
                // VecSwizzle(vec1, 1, 1, 2, 2),
                Sse2.Shuffle(D.AsInt32(), Shuffle_1122).AsSingle(),
                // VecSwizzle(vec2, 2, 3, 0, 1)));
                Sse2.Shuffle(C.AsInt32(), Shuffle_2301).AsSingle()));

        // __m128 A_B = Mat2AdjMul(A, B);
        var A_B = Sse.Subtract(
            Sse.Multiply(Sse2.Shuffle(A.AsInt32(), Shuffle_3300).AsSingle(), B),
            Sse.Multiply(
                Sse2.Shuffle(A.AsInt32(), Shuffle_1122).AsSingle(),
                Sse2.Shuffle(B.AsInt32(), Shuffle_2301).AsSingle()));

        const byte Shuffle_0303 = 0b1100_1100;
        const byte Shuffle_1032 = 0b1011_0001;
        const byte Shuffle_2121 = 0b0110_0110;

        // Mat2Mul(vec1, vec2):
        // _mm_add_ps(_mm_mul_ps(vec1, VecSwizzle(vec2, 0, 3, 0, 3)),
        //       _mm_mul_ps(VecSwizzle(vec1, 1, 0, 3, 2), VecSwizzle(vec2, 2, 1, 2, 1)));

        // __m128 X_ = _mm_sub_ps(
        var X_ = Sse.Subtract(
            // _mm_mul_ps(detD, A),
            Sse.Multiply(detD, A),
            // Mat2Mul(B, D_C));
            // _mm_add_ps(
            Sse.Add(
                // _mm_mul_ps(vec1, VecSwizzle(vec2, 0, 3, 0, 3)),
                Sse.Multiply(B, Sse2.Shuffle(D_C.AsInt32(), Shuffle_0303).AsSingle()),
                // _mm_mul_ps(
                Sse.Multiply(
                    // VecSwizzle(vec1, 1, 0, 3, 2),
                    Sse2.Shuffle(B.AsInt32(), Shuffle_1032).AsSingle(),
                    // VecSwizzle(vec2, 2, 1, 2, 1)));
                    Sse2.Shuffle(D_C.AsInt32(), Shuffle_2121).AsSingle())));

        // __m128 W_ = _mm_sub_ps(_mm_mul_ps(detA, D), Mat2Mul(C, A_B));
        var W_ = Sse.Subtract(
            Sse.Multiply(detA, D),
            Sse.Add(
                Sse.Multiply(C, Sse2.Shuffle(A_B.AsInt32(), Shuffle_0303).AsSingle()),
                Sse.Multiply(
                    Sse2.Shuffle(C.AsInt32(), Shuffle_1032).AsSingle(),
                    Sse2.Shuffle(A_B.AsInt32(), Shuffle_2121).AsSingle())));

        // __m128 detM = _mm_mul_ps(detA, detD);
        var detM = Sse.Multiply(detA, detD);

        const byte Shuffle_3030 = 0b0011_0011;

        // Mat2MulAdj(vec1, vec2):
        // _mm_sub_ps(_mm_mul_ps(vec1, VecSwizzle(vec2, 3, 0, 3, 0)),
        //       _mm_mul_ps(VecSwizzle(vec1, 1, 0, 3, 2), VecSwizzle(vec2, 2, 1, 2, 1)));

        // __m128 Y_ = _mm_sub_ps(
        var Y_ = Sse.Subtract(
            // _mm_mul_ps(detB, C),
            Sse.Multiply(detB, C),
            // Mat2MulAdj(D, A_B));
            // _mm_sub_ps(
            Sse.Subtract(
                // _mm_mul_ps(vec1, VecSwizzle(vec2, 3, 0, 3, 0)),
                Sse.Multiply(D, Sse2.Shuffle(A_B.AsInt32(), Shuffle_3030).AsSingle()),
                // _mm_mul_ps(
                Sse.Multiply(
                    // VecSwizzle(vec1, 1, 0, 3, 2),
                    Sse2.Shuffle(D.AsInt32(), Shuffle_1032).AsSingle(),
                    // VecSwizzle(vec2, 2, 1, 2, 1)));
                    Sse2.Shuffle(A_B.AsInt32(), Shuffle_2121).AsSingle())));

        // __m128 Z_ = _mm_sub_ps(_mm_mul_ps(detC, B), Mat2MulAdj(A, D_C));
        var Z_ = Sse.Subtract(
            Sse.Multiply(detC, B),
            Sse.Subtract(
                Sse.Multiply(A, Sse2.Shuffle(D_C.AsInt32(), Shuffle_3030).AsSingle()),
                Sse.Multiply(
                    Sse2.Shuffle(A.AsInt32(), Shuffle_1032).AsSingle(),
                    Sse2.Shuffle(D_C.AsInt32(), Shuffle_2121).AsSingle())));

        // detM = _mm_add_ps(detM, _mm_mul_ps(detB, detC));
        detM = Sse.Add(detM, Sse.Multiply(detB, detC));

        const byte Shuffle_0213 = 0b1101_1000;

        // __m128 tr = _mm_mul_ps(A_B, VecSwizzle(D_C, 0,2,1,3));
        var tr = Sse.Multiply(A_B, Sse2.Shuffle(D_C.AsInt32(), Shuffle_0213).AsSingle());
        // tr = _mm_hadd_ps(tr, tr);
        tr = Sse3.HorizontalAdd(tr, tr);
        // tr = _mm_hadd_ps(tr, tr);
        tr = Sse3.HorizontalAdd(tr, tr);

        // detM = _mm_sub_ps(detM, tr);
        detM = Sse.Subtract(detM, tr);

        if (MathF.Abs(detM.GetElement(0)) < float.Epsilon)
        {
            throw new InvalidOperationException("Matrix is singular and cannot be inverted.");
        }

        // const __m128 adjSignMask = _mm_setr_ps(1.f, -1.f, -1.f, 1.f);
        var adjSignMask = Vector128.Create(1.0f, -1.0f, -1.0f, 1.0f);

        // __m128 rDetM = _mm_div_ps(adjSignMask, detM);
        var rDetM = Sse.Divide(adjSignMask, detM);

        // X_ = _mm_mul_ps(X_, rDetM);
        X_ = Sse.Multiply(X_, rDetM);
        // Y_ = _mm_mul_ps(Y_, rDetM);
        Y_ = Sse.Multiply(Y_, rDetM);
        // Z_ = _mm_mul_ps(Z_, rDetM);
        Z_ = Sse.Multiply(Z_, rDetM);
        // W_ = _mm_mul_ps(W_, rDetM);
        W_ = Sse.Multiply(W_, rDetM);

        const byte Shuffle_3131 = 0b0111_0111;
        const byte Shuffle_2020 = 0b0010_0010;

        Unsafe.SkipInit(out result);

        fixed (float* r = &result.Row0.x)
        {
            // r.mVec[0] = VecShuffle(X_, Y_, 3, 1, 3, 1);
            Sse.Store(r + 0, Sse.Shuffle(X_, Y_, Shuffle_3131));
            // r.mVec[1] = VecShuffle(X_, Y_, 2, 0, 2, 0);
            Sse.Store(r + 4, Sse.Shuffle(X_, Y_, Shuffle_2020));
            // r.mVec[2] = VecShuffle(Z_, W_, 3, 1, 3, 1);
            Sse.Store(r + 8, Sse.Shuffle(Z_, W_, Shuffle_3131));
            // r.mVec[3] = VecShuffle(Z_, W_, 2, 0, 2, 0);
            Sse.Store(r + 12, Sse.Shuffle(Z_, W_, Shuffle_2020));
        }
#pragma warning restore SA1114 // Parameter list should follow declaration
#pragma warning restore SA1312 // Variable names should begin with lower-case letter
#pragma warning restore SA1512 // Single-line comments should not be followed by blank lines
#pragma warning restore SA1515 // Single-line comment should be preceded by blank line
    }
#endif

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe void InvertFallback(in Matrix4x4 mat, out Matrix4x4 result)
    {
        // Original implementation can be found here:
        // https://github.com/dotnet/runtime/blob/79ae74f5ca5c8a6fe3a48935e85bd7374959c570/src/libraries/System.Private.CoreLib/src/System/Numerics/Matrix4x4.cs#L1556
#pragma warning disable SA1407 // Arithmetic expressions should declare precedence
        float a = mat.M11, b = mat.M21, c = mat.M31, d = mat.M41;
        float e = mat.M12, f = mat.M22, g = mat.M32, h = mat.M42;
        float i = mat.M13, j = mat.M23, k = mat.M33, l = mat.M43;
        float m = mat.M14, n = mat.M24, o = mat.M34, p = mat.M44;

        float kp_lo = k * p - l * o;
        float jp_ln = j * p - l * n;
        float jo_kn = j * o - k * n;
        float ip_lm = i * p - l * m;
        float io_km = i * o - k * m;
        float in_jm = i * n - j * m;

        float a11 = +(f * kp_lo - g * jp_ln + h * jo_kn);
        float a12 = -(e * kp_lo - g * ip_lm + h * io_km);
        float a13 = +(e * jp_ln - f * ip_lm + h * in_jm);
        float a14 = -(e * jo_kn - f * io_km + g * in_jm);

        float det = a * a11 + b * a12 + c * a13 + d * a14;

        if (MathF.Abs(det) < float.Epsilon)
        {
            throw new InvalidOperationException("Matrix is singular and cannot be inverted.");
        }

        float invDet = 1.0f / det;

        result.Row0 = new Vector4(a11, a12, a13, a14) * invDet;

        result.Row1 = new Vector4(
            -(b * kp_lo - c * jp_ln + d * jo_kn),
            +(a * kp_lo - c * ip_lm + d * io_km),
            -(a * jp_ln - b * ip_lm + d * in_jm),
            +(a * jo_kn - b * io_km + c * in_jm)) * invDet;

        float gp_ho = g * p - h * o;
        float fp_hn = f * p - h * n;
        float fo_gn = f * o - g * n;
        float ep_hm = e * p - h * m;
        float eo_gm = e * o - g * m;
        float en_fm = e * n - f * m;

        result.Row2 = new Vector4(
            +(b * gp_ho - c * fp_hn + d * fo_gn),
            -(a * gp_ho - c * ep_hm + d * eo_gm),
            +(a * fp_hn - b * ep_hm + d * en_fm),
            -(a * fo_gn - b * eo_gm + c * en_fm)) * invDet;

        float gl_hk = g * l - h * k;
        float fl_hj = f * l - h * j;
        float fk_gj = f * k - g * j;
        float el_hi = e * l - h * i;
        float ek_gi = e * k - g * i;
        float ej_fi = e * j - f * i;

        result.Row3 = new Vector4(
            -(b * gl_hk - c * fl_hj + d * fk_gj),
            +(a * gl_hk - c * el_hi + d * ek_gi),
            -(a * fl_hj - b * el_hi + d * ej_fi),
            +(a * fk_gj - b * ek_gi + c * ej_fi)) * invDet;
#pragma warning restore SA1407 // Arithmetic expressions should declare precedence
    }

    /// <summary>
    /// Calculate the inverse of the given matrix.
    /// </summary>
    /// <param name="mat">The matrix to invert.</param>
    /// <returns>The inverse of the given matrix.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the Matrix4 is singular.</exception>
    [Pure]
    public static Matrix4x4 Invert(Matrix4x4 mat)
    {
        Invert(in mat, out Matrix4x4 result);
        return result;
    }

    /// <summary>
    /// Calculate the transpose of the given matrix.
    /// </summary>
    /// <param name="mat">The matrix to transpose.</param>
    /// <returns>The transpose of the given matrix.</returns>
    [Pure]
    public static Matrix4x4 Transpose(Matrix4x4 mat)
    {
        return new Matrix4x4(mat.Column0, mat.Column1, mat.Column2, mat.Column3);
    }

    /// <summary>
    /// Calculate the transpose of the given matrix.
    /// </summary>
    /// <param name="mat">The matrix to transpose.</param>
    /// <param name="result">The result of the calculation.</param>
    public static void Transpose(in Matrix4x4 mat, out Matrix4x4 result)
    {
        result.Row0 = mat.Column0;
        result.Row1 = mat.Column1;
        result.Row2 = mat.Column2;
        result.Row3 = mat.Column3;
    }

    /// <summary>
    /// Swizzles a matrix, i.e. switches rows of the matrix.
    /// </summary>
    /// <param name="mat">The matrix to swizzle.</param>
    /// <param name="rowForRow0">Which row to place in <see cref="Row0"/>.</param>
    /// <param name="rowForRow1">Which row to place in <see cref="Row1"/>.</param>
    /// <param name="rowForRow2">Which row to place in <see cref="Row2"/>.</param>
    /// <param name="rowForRow3">Which row to place in <see cref="Row3"/>.</param>
    /// <returns>The swizzled matrix.</returns>
    /// <exception cref="IndexOutOfRangeException">If any of the rows are outside of the range [0, 3].</exception>
    public static Matrix4x4 Swizzle(Matrix4x4 mat, int rowForRow0, int rowForRow1, int rowForRow2, int rowForRow3)
    {
        Swizzle(mat, rowForRow0, rowForRow1, rowForRow2, rowForRow3, out Matrix4x4 result);
        return result;
    }

    /// <summary>
    /// Swizzles a matrix, i.e. switches rows of the matrix.
    /// </summary>
    /// <param name="mat">The matrix to swizzle.</param>
    /// <param name="rowForRow0">Which row to place in <see cref="Row0"/>.</param>
    /// <param name="rowForRow1">Which row to place in <see cref="Row1"/>.</param>
    /// <param name="rowForRow2">Which row to place in <see cref="Row2"/>.</param>
    /// <param name="rowForRow3">Which row to place in <see cref="Row3"/>.</param>
    /// <param name="result">The swizzled matrix.</param>
    /// <exception cref="IndexOutOfRangeException">If any of the rows are outside of the range [0, 3].</exception>
    public static void Swizzle(in Matrix4x4 mat, int rowForRow0, int rowForRow1, int rowForRow2, int rowForRow3, out Matrix4x4 result)
    {
        result.Row0 = rowForRow0 switch
        {
            0 => mat.Row0,
            1 => mat.Row1,
            2 => mat.Row2,
            3 => mat.Row3,
            _ => throw new IndexOutOfRangeException($"{nameof(rowForRow0)} must be a number between 0 and 3. Got {rowForRow0}."),
        };

        result.Row1 = rowForRow1 switch
        {
            0 => mat.Row0,
            1 => mat.Row1,
            2 => mat.Row2,
            3 => mat.Row3,
            _ => throw new IndexOutOfRangeException($"{nameof(rowForRow1)} must be a number between 0 and 3. Got {rowForRow1}."),
        };

        result.Row2 = rowForRow2 switch
        {
            0 => mat.Row0,
            1 => mat.Row1,
            2 => mat.Row2,
            3 => mat.Row3,
            _ => throw new IndexOutOfRangeException($"{nameof(rowForRow2)} must be a number between 0 and 3. Got {rowForRow2}."),
        };

        result.Row3 = rowForRow3 switch
        {
            0 => mat.Row0,
            1 => mat.Row1,
            2 => mat.Row2,
            3 => mat.Row3,
            _ => throw new IndexOutOfRangeException($"{nameof(rowForRow3)} must be a number between 0 and 2. Got {rowForRow3}."),
        };
    }

    public static implicit operator OpenTK.Mathematics.Matrix4(Matrix4x4 value)
    {
        return new OpenTK.Mathematics.Matrix4(
            (OpenTK.Mathematics.Vector4)value.Row0,
            (OpenTK.Mathematics.Vector4)value.Row1,
            (OpenTK.Mathematics.Vector4)value.Row2,
            (OpenTK.Mathematics.Vector4)value.Row3
        );
    }

    public static implicit operator Matrix4x4(OpenTK.Mathematics.Matrix4 value)
    {
        return new Matrix4x4(
            value.Row0,
            value.Row1,
            value.Row2,
            value.Row3
        );
    }

    public static implicit operator System.Numerics.Matrix4x4(Matrix4x4 value)
    {
        return new System.Numerics.Matrix4x4(
            value.Row0.x, value.Row0.y, value.Row0.z, value.Row0.w,
            value.Row1.x, value.Row1.y, value.Row1.z, value.Row1.w,
            value.Row2.x, value.Row2.y, value.Row2.z, value.Row2.w,
            value.Row3.x, value.Row3.y, value.Row3.z, value.Row3.w
        );
    }

    public static implicit operator Matrix4x4(System.Numerics.Matrix4x4 value)
    {
        return new Matrix4x4(
            new Vector4(value.M11, value.M12, value.M13, value.M14),
            new Vector4(value.M21, value.M22, value.M23, value.M24),
            new Vector4(value.M31, value.M32, value.M33, value.M34),
            new Vector4(value.M41, value.M42, value.M43, value.M44)
        );
    }

    /// <summary>
    /// Matrix multiplication.
    /// </summary>
    /// <param name="left">left-hand operand.</param>
    /// <param name="right">right-hand operand.</param>
    /// <returns>A new Matrix4 which holds the result of the multiplication.</returns>
    [Pure]
    public static Matrix4x4 operator *(Matrix4x4 left, Matrix4x4 right)
    {
        return Mult(left, right);
    }

    /// <summary>
    /// Matrix-scalar multiplication.
    /// </summary>
    /// <param name="left">left-hand operand.</param>
    /// <param name="right">right-hand operand.</param>
    /// <returns>A new Matrix4 which holds the result of the multiplication.</returns>
    [Pure]
    public static Matrix4x4 operator *(Matrix4x4 left, float right)
    {
        return Mult(left, right);
    }

    /// <summary>
    /// Matrix addition.
    /// </summary>
    /// <param name="left">left-hand operand.</param>
    /// <param name="right">right-hand operand.</param>
    /// <returns>A new Matrix4 which holds the result of the addition.</returns>
    [Pure]
    public static Matrix4x4 operator +(Matrix4x4 left, Matrix4x4 right)
    {
        return Add(left, right);
    }

    /// <summary>
    /// Matrix subtraction.
    /// </summary>
    /// <param name="left">left-hand operand.</param>
    /// <param name="right">right-hand operand.</param>
    /// <returns>A new Matrix4 which holds the result of the subtraction.</returns>
    [Pure]
    public static Matrix4x4 operator -(Matrix4x4 left, Matrix4x4 right)
    {
        return Subtract(left, right);
    }

    /// <summary>
    /// Compares two instances for equality.
    /// </summary>
    /// <param name="left">The first instance.</param>
    /// <param name="right">The second instance.</param>
    /// <returns>True, if left equals right; false otherwise.</returns>
    [Pure]
    public static bool operator ==(Matrix4x4 left, Matrix4x4 right)
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
    public static bool operator !=(Matrix4x4 left, Matrix4x4 right)
    {
        return !left.Equals(right);
    }

    /// <summary>
    /// Returns a System.String that represents the current Matrix4.
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
        var row3 = Row3.ToString(format, formatProvider);
        return $"{row0}\n{row1}\n{row2}\n{row3}";
    }

    /// <summary>
    /// Returns the hashcode for this instance.
    /// </summary>
    /// <returns>A System.Int32 containing the unique hashcode for this instance.</returns>
    public override readonly int GetHashCode()
    {
        return HashCode.Combine(Row0, Row1, Row2, Row3);
    }

    /// <summary>
    /// Indicates whether this instance and a specified object are equal.
    /// </summary>
    /// <param name="obj">The object to compare tresult.</param>
    /// <returns>True if the instances are equal; false otherwise.</returns>
    [Pure]
    public override readonly bool Equals(object obj)
    {
        return obj is Matrix4x4 && Equals((Matrix4x4)obj);
    }

    /// <summary>
    /// Indicates whether the current matrix is equal to another matrix.
    /// </summary>
    /// <param name="other">An matrix to compare with this matrix.</param>
    /// <returns>true if the current matrix is equal to the matrix parameter; otherwise, false.</returns>
    [Pure]
    public readonly bool Equals(Matrix4x4 other)
    {
        return
            Row0 == other.Row0 &&
            Row1 == other.Row1 &&
            Row2 == other.Row2 &&
            Row3 == other.Row3;
    }
}
