using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Kralt.Mathematics;

/// <summary>
/// Represents a 2x4 matrix.
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct Matrix2x4 : IEquatable<Matrix2x4>, IFormattable
{
    /// <summary>
    /// Top row of the matrix.
    /// </summary>
    public Vector4 Row0;

    /// <summary>
    /// Bottom row of the matrix.
    /// </summary>
    public Vector4 Row1;

    /// <summary>
    /// The zero matrix.
    /// </summary>
    public static readonly Matrix2x4 Zero = new Matrix2x4(Vector4.zero, Vector4.zero);

    /// <summary>
    /// Initializes a new instance of the <see cref="Matrix2x4"/> struct.
    /// </summary>
    /// <param name="row0">Top row of the matrix.</param>
    /// <param name="row1">Bottom row of the matrix.</param>
    public Matrix2x4(Vector4 row0, Vector4 row1)
    {
        Row0 = row0;
        Row1 = row1;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Matrix2x4"/> struct.
    /// </summary>
    /// <param name="m00">First item of the first row of the matrix.</param>
    /// <param name="m01">Second item of the first row of the matrix.</param>
    /// <param name="m02">Third item of the first row of the matrix.</param>
    /// <param name="m03">Fourth item of the first row of the matrix.</param>
    /// <param name="m10">First item of the second row of the matrix.</param>
    /// <param name="m11">Second item of the second row of the matrix.</param>
    /// <param name="m12">Third item of the second row of the matrix.</param>
    /// <param name="m13">Fourth item of the second row of the matrix.</param>
    [SuppressMessage("ReSharper", "SA1117", Justification = "For better readability of Matrix struct.")]
    public Matrix2x4
    (
        float m00, float m01, float m02, float m03,
        float m10, float m11, float m12, float m13
    )
    {
        Row0 = new Vector4(m00, m01, m02, m03);
        Row1 = new Vector4(m10, m11, m12, m13);
    }

    /// <summary>
    /// Gets or sets the first column of the matrix.
    /// </summary>
    public Vector2 Column0
    {
        readonly get => new Vector2(Row0.x, Row1.x);
        set
        {
            Row0.x = value.x;
            Row1.x = value.y;
        }
    }

    /// <summary>
    /// Gets or sets the second column of the matrix.
    /// </summary>
    public Vector2 Column1
    {
        readonly get => new Vector2(Row0.y, Row1.y);
        set
        {
            Row0.y = value.x;
            Row1.y = value.y;
        }
    }

    /// <summary>
    /// Gets or sets the third column of the matrix.
    /// </summary>
    public Vector2 Column2
    {
        readonly get => new Vector2(Row0.z, Row1.z);
        set
        {
            Row0.z = value.x;
            Row1.z = value.y;
        }
    }

    /// <summary>
    /// Gets or sets the fourth column of the matrix.
    /// </summary>
    public Vector2 Column3
    {
        readonly get => new Vector2(Row0.w, Row1.w);
        set
        {
            Row0.w = value.x;
            Row1.w = value.y;
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
    /// Gets or sets the values along the main diagonal of the matrix.
    /// </summary>
    public Vector2 Diagonal
    {
        readonly get => new Vector2(Row0.x, Row1.y);
        set
        {
            Row0.x = value.x;
            Row1.y = value.y;
        }
    }

    /// <summary>
    /// Gets the trace of the matrix, the sum of the values along the diagonal.
    /// </summary>
    public readonly float Trace => Row0.x + Row1.y;

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
            if (((uint)rowIndex) < 2 && ((uint)columnIndex) < 4)
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
            if (((uint)rowIndex) < 2 && ((uint)columnIndex) < 4)
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
    private readonly ref Vector4 GetRowUnsafe(in Matrix2x4 m, int index)
    {
        ref Vector4 address = ref Unsafe.AsRef(in m.Row0);
        return ref Unsafe.Add(ref address, index);
    }

    /// <summary>
    /// Returns a transposed copy of this instance.
    /// </summary>
    /// <returns>The transposed copy.</returns>
    public readonly Matrix4x2 Transposed()
    {
        return Transpose(this);
    }

    /// <summary>
    /// Swizzles this instance. Swiches places of the rows of the matrix.
    /// </summary>
    /// <param name="rowForRow0">Which row to place in <see cref="Row0"/>.</param>
    /// <param name="rowForRow1">Which row to place in <see cref="Row1"/>.</param>
    public void Swizzle(int rowForRow0, int rowForRow1)
    {
        this = Swizzle(this, rowForRow0, rowForRow1);
    }

    /// <summary>
    /// Returns a swizzled copy of this instance.
    /// </summary>
    /// <param name="rowForRow0">Which row to place in <see cref="Row0"/>.</param>
    /// <param name="rowForRow1">Which row to place in <see cref="Row1"/>.</param>
    /// <returns>The swizzled copy.</returns>
    public readonly Matrix2x4 Swizzled(int rowForRow0, int rowForRow1)
    {
        Matrix2x4 m = this;
        m.Swizzle(rowForRow0, rowForRow1);
        return m;
    }

    /// <summary>
    /// Builds a rotation matrix.
    /// </summary>
    /// <param name="angle">The counter-clockwise angle in radians.</param>
    /// <param name="result">The resulting Matrix2x4 instance.</param>
    public static void CreateRotation(float angle, out Matrix2x4 result)
    {
        var cos = MathF.Cos(angle);
        var sin = MathF.Sin(angle);

        result.Row0.x = cos;
        result.Row0.y = sin;
        result.Row0.z = 0;
        result.Row0.w = 0;
        result.Row1.x = -sin;
        result.Row1.y = cos;
        result.Row1.z = 0;
        result.Row1.w = 0;
    }

    /// <summary>
    /// Builds a rotation matrix.
    /// </summary>
    /// <param name="angle">The counter-clockwise angle in radians.</param>
    /// <returns>The resulting Matrix2x3 instance.</returns>
    [Pure]
    public static Matrix2x4 CreateRotation(float angle)
    {
        CreateRotation(angle, out Matrix2x4 result);
        return result;
    }

    /// <summary>
    /// Creates a scale matrix.
    /// </summary>
    /// <param name="scale">Single scale factor for the x, y, and z axes.</param>
    /// <param name="result">A scale matrix.</param>
    public static void CreateScale(float scale, out Matrix2x4 result)
    {
        result.Row0.x = scale;
        result.Row0.y = 0;
        result.Row0.z = 0;
        result.Row0.w = 0;
        result.Row1.x = 0;
        result.Row1.y = scale;
        result.Row1.z = 0;
        result.Row1.w = 0;
    }

    /// <summary>
    /// Creates a scale matrix.
    /// </summary>
    /// <param name="scale">Single scale factor for the x and y axes.</param>
    /// <returns>A scale matrix.</returns>
    [Pure]
    public static Matrix2x4 CreateScale(float scale)
    {
        CreateScale(scale, out Matrix2x4 result);
        return result;
    }

    /// <summary>
    /// Creates a scale matrix.
    /// </summary>
    /// <param name="scale">Scale factors for the x and y axes.</param>
    /// <param name="result">A scale matrix.</param>
    public static void CreateScale(Vector2 scale, out Matrix2x4 result)
    {
        result.Row0.x = scale.x;
        result.Row0.y = 0;
        result.Row0.z = 0;
        result.Row0.w = 0;
        result.Row1.x = 0;
        result.Row1.y = scale.y;
        result.Row1.z = 0;
        result.Row1.w = 0;
    }

    /// <summary>
    /// Creates a scale matrix.
    /// </summary>
    /// <param name="scale">Scale factors for the x and y axes.</param>
    /// <returns>A scale matrix.</returns>
    [Pure]
    public static Matrix2x4 CreateScale(Vector2 scale)
    {
        CreateScale(scale, out Matrix2x4 result);
        return result;
    }

    /// <summary>
    /// Creates a scale matrix.
    /// </summary>
    /// <param name="x">Scale factor for the x axis.</param>
    /// <param name="y">Scale factor for the y axis.</param>
    /// <param name="result">A scale matrix.</param>
    public static void CreateScale(float x, float y, out Matrix2x4 result)
    {
        result.Row0.x = x;
        result.Row0.y = 0;
        result.Row0.z = 0;
        result.Row0.w = 0;
        result.Row1.x = 0;
        result.Row1.y = y;
        result.Row1.z = 0;
        result.Row1.w = 0;
    }

    /// <summary>
    /// Creates a scale matrix.
    /// </summary>
    /// <param name="x">Scale factor for the x axis.</param>
    /// <param name="y">Scale factor for the y axis.</param>
    /// <returns>A scale matrix.</returns>
    [Pure]
    public static Matrix2x4 CreateScale(float x, float y)
    {
        CreateScale(x, y, out Matrix2x4 result);
        return result;
    }

    /// <summary>
    /// Multiplies and instance by a scalar.
    /// </summary>
    /// <param name="left">The left operand of the multiplication.</param>
    /// <param name="right">The right operand of the multiplication.</param>
    /// <param name="result">A new instance that is the result of the multiplication.</param>
    public static void Mult(in Matrix2x4 left, float right, out Matrix2x4 result)
    {
        result.Row0.x = left.Row0.x * right;
        result.Row0.y = left.Row0.y * right;
        result.Row0.z = left.Row0.z * right;
        result.Row0.w = left.Row0.w * right;
        result.Row1.x = left.Row1.x * right;
        result.Row1.y = left.Row1.y * right;
        result.Row1.z = left.Row1.z * right;
        result.Row1.w = left.Row1.w * right;
    }

    /// <summary>
    /// Multiplies and instance by a scalar.
    /// </summary>
    /// <param name="left">The left operand of the multiplication.</param>
    /// <param name="right">The right operand of the multiplication.</param>
    /// <returns>A new instance that is the result of the multiplication.</returns>
    [Pure]
    public static Matrix2x4 Mult(Matrix2x4 left, float right)
    {
        Mult(in left, right, out Matrix2x4 result);
        return result;
    }

    /// <summary>
    /// Multiplies two instances.
    /// </summary>
    /// <param name="left">The left operand of the multiplication.</param>
    /// <param name="right">The right operand of the multiplication.</param>
    /// <param name="result">A new instance that is the result of the multiplication.</param>
    public static void Mult(in Matrix2x4 left, in Matrix4x2 right, out Matrix2x2 result)
    {
        float leftM11 = left.Row0.x;
        float leftM12 = left.Row0.y;
        float leftM13 = left.Row0.z;
        float leftM14 = left.Row0.w;
        float leftM21 = left.Row1.x;
        float leftM22 = left.Row1.y;
        float leftM23 = left.Row1.z;
        float leftM24 = left.Row1.w;
        float rightM11 = right.Row0.x;
        float rightM12 = right.Row0.y;
        float rightM21 = right.Row1.x;
        float rightM22 = right.Row1.y;
        float rightM31 = right.Row2.x;
        float rightM32 = right.Row2.y;
        float rightM41 = right.Row3.x;
        float rightM42 = right.Row3.y;

        result.Row0.x = (leftM11 * rightM11) + (leftM12 * rightM21) + (leftM13 * rightM31) + (leftM14 * rightM41);
        result.Row0.y = (leftM11 * rightM12) + (leftM12 * rightM22) + (leftM13 * rightM32) + (leftM14 * rightM42);
        result.Row1.x = (leftM21 * rightM11) + (leftM22 * rightM21) + (leftM23 * rightM31) + (leftM24 * rightM41);
        result.Row1.y = (leftM21 * rightM12) + (leftM22 * rightM22) + (leftM23 * rightM32) + (leftM24 * rightM42);
    }

    /// <summary>
    /// Multiplies two instances.
    /// </summary>
    /// <param name="left">The left operand of the multiplication.</param>
    /// <param name="right">The right operand of the multiplication.</param>
    /// <returns>A new instance that is the result of the multiplication.</returns>
    [Pure]
    public static Matrix2x2 Mult(Matrix2x4 left, Matrix4x2 right)
    {
        Mult(in left, in right, out Matrix2x2 result);
        return result;
    }

    /// <summary>
    /// Multiplies two instances.
    /// </summary>
    /// <param name="left">The left operand of the multiplication.</param>
    /// <param name="right">The right operand of the multiplication.</param>
    /// <param name="result">A new instance that is the result of the multiplication.</param>
    public static void Mult(in Matrix2x4 left, in Matrix4x3 right, out Matrix2x3 result)
    {
        float leftM11 = left.Row0.x;
        float leftM12 = left.Row0.y;
        float leftM13 = left.Row0.z;
        float leftM14 = left.Row0.w;
        float leftM21 = left.Row1.x;
        float leftM22 = left.Row1.y;
        float leftM23 = left.Row1.z;
        float leftM24 = left.Row1.w;
        float rightM11 = right.Row0.x;
        float rightM12 = right.Row0.y;
        float rightM13 = right.Row0.z;
        float rightM21 = right.Row1.x;
        float rightM22 = right.Row1.y;
        float rightM23 = right.Row1.z;
        float rightM31 = right.Row2.x;
        float rightM32 = right.Row2.y;
        float rightM33 = right.Row2.z;
        float rightM41 = right.Row3.x;
        float rightM42 = right.Row3.y;
        float rightM43 = right.Row3.z;

        result.Row0.x = (leftM11 * rightM11) + (leftM12 * rightM21) + (leftM13 * rightM31) + (leftM14 * rightM41);
        result.Row0.y = (leftM11 * rightM12) + (leftM12 * rightM22) + (leftM13 * rightM32) + (leftM14 * rightM42);
        result.Row0.z = (leftM11 * rightM13) + (leftM12 * rightM23) + (leftM13 * rightM33) + (leftM14 * rightM43);
        result.Row1.x = (leftM21 * rightM11) + (leftM22 * rightM21) + (leftM23 * rightM31) + (leftM24 * rightM41);
        result.Row1.y = (leftM21 * rightM12) + (leftM22 * rightM22) + (leftM23 * rightM32) + (leftM24 * rightM42);
        result.Row1.z = (leftM21 * rightM13) + (leftM22 * rightM23) + (leftM23 * rightM33) + (leftM24 * rightM43);
    }

    /// <summary>
    /// Multiplies two instances.
    /// </summary>
    /// <param name="left">The left operand of the multiplication.</param>
    /// <param name="right">The right operand of the multiplication.</param>
    /// <returns>A new instance that is the result of the multiplication.</returns>
    [Pure]
    public static Matrix2x3 Mult(Matrix2x4 left, Matrix4x3 right)
    {
        Mult(in left, in right, out Matrix2x3 result);
        return result;
    }

    /// <summary>
    /// Multiplies two instances.
    /// </summary>
    /// <param name="left">The left operand of the multiplication.</param>
    /// <param name="right">The right operand of the multiplication.</param>
    /// <param name="result">A new instance that is the result of the multiplication.</param>
    public static void Mult(in Matrix2x4 left, in Matrix4x4 right, out Matrix2x4 result)
    {
        float leftM11 = left.Row0.x;
        float leftM12 = left.Row0.y;
        float leftM13 = left.Row0.z;
        float leftM14 = left.Row0.w;
        float leftM21 = left.Row1.x;
        float leftM22 = left.Row1.y;
        float leftM23 = left.Row1.z;
        float leftM24 = left.Row1.w;
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
    }

    /// <summary>
    /// Multiplies two instances.
    /// </summary>
    /// <param name="left">The left operand of the multiplication.</param>
    /// <param name="right">The right operand of the multiplication.</param>
    /// <returns>A new instance that is the result of the multiplication.</returns>
    [Pure]
    public static Matrix2x4 Mult(Matrix2x4 left, Matrix4x4 right)
    {
        Mult(in left, in right, out Matrix2x4 result);
        return result;
    }

    /// <summary>
    /// Adds two instances.
    /// </summary>
    /// <param name="left">The left operand of the addition.</param>
    /// <param name="right">The right operand of the addition.</param>
    /// <param name="result">A new instance that is the result of the addition.</param>
    public static void Add(in Matrix2x4 left, in Matrix2x4 right, out Matrix2x4 result)
    {
        result.Row0.x = left.Row0.x + right.Row0.x;
        result.Row0.y = left.Row0.y + right.Row0.y;
        result.Row0.z = left.Row0.z + right.Row0.z;
        result.Row0.w = left.Row0.w + right.Row0.w;
        result.Row1.x = left.Row1.x + right.Row1.x;
        result.Row1.y = left.Row1.y + right.Row1.y;
        result.Row1.z = left.Row1.z + right.Row1.z;
        result.Row1.w = left.Row1.w + right.Row1.w;
    }

    /// <summary>
    /// Adds two instances.
    /// </summary>
    /// <param name="left">The left operand of the addition.</param>
    /// <param name="right">The right operand of the addition.</param>
    /// <returns>A new instance that is the result of the addition.</returns>
    [Pure]
    public static Matrix2x4 Add(Matrix2x4 left, Matrix2x4 right)
    {
        Add(in left, in right, out Matrix2x4 result);
        return result;
    }

    /// <summary>
    /// Subtracts two instances.
    /// </summary>
    /// <param name="left">The left operand of the subtraction.</param>
    /// <param name="right">The right operand of the subtraction.</param>
    /// <param name="result">A new instance that is the result of the subtraction.</param>
    public static void Subtract(in Matrix2x4 left, in Matrix2x4 right, out Matrix2x4 result)
    {
        result.Row0.x = left.Row0.x - right.Row0.x;
        result.Row0.y = left.Row0.y - right.Row0.y;
        result.Row0.z = left.Row0.z - right.Row0.z;
        result.Row0.w = left.Row0.w - right.Row0.w;
        result.Row1.x = left.Row1.x - right.Row1.x;
        result.Row1.y = left.Row1.y - right.Row1.y;
        result.Row1.z = left.Row1.z - right.Row1.z;
        result.Row1.w = left.Row1.w - right.Row1.w;
    }

    /// <summary>
    /// Subtracts two instances.
    /// </summary>
    /// <param name="left">The left operand of the subtraction.</param>
    /// <param name="right">The right operand of the subtraction.</param>
    /// <returns>A new instance that is the result of the subtraction.</returns>
    [Pure]
    public static Matrix2x4 Subtract(Matrix2x4 left, Matrix2x4 right)
    {
        Subtract(in left, in right, out Matrix2x4 result);
        return result;
    }

    /// <summary>
    /// Calculate the transpose of the given matrix.
    /// </summary>
    /// <param name="mat">The matrix to transpose.</param>
    /// <param name="result">The transpose of the given matrix.</param>
    public static void Transpose(in Matrix2x4 mat, out Matrix4x2 result)
    {
        result.Row0.x = mat.Row0.x;
        result.Row0.y = mat.Row1.x;
        result.Row1.x = mat.Row0.y;
        result.Row1.y = mat.Row1.y;
        result.Row2.x = mat.Row0.z;
        result.Row2.y = mat.Row1.z;
        result.Row3.x = mat.Row0.w;
        result.Row3.y = mat.Row1.w;
    }

    /// <summary>
    /// Calculate the transpose of the given matrix.
    /// </summary>
    /// <param name="mat">The matrix to transpose.</param>
    /// <returns>The transpose of the given matrix.</returns>
    [Pure]
    public static Matrix4x2 Transpose(Matrix2x4 mat)
    {
        Transpose(in mat, out Matrix4x2 result);
        return result;
    }

    /// <summary>
    /// Swizzles a matrix, i.e. switches rows of the matrix.
    /// </summary>
    /// <param name="mat">The matrix to swizzle.</param>
    /// <param name="rowForRow0">Which row to place in <see cref="Row0"/>.</param>
    /// <param name="rowForRow1">Which row to place in <see cref="Row1"/>.</param>
    /// <returns>The swizzled matrix.</returns>
    /// <exception cref="IndexOutOfRangeException">If any of the rows are outside of the range [0, 1].</exception>
    public static Matrix2x4 Swizzle(Matrix2x4 mat, int rowForRow0, int rowForRow1)
    {
        Swizzle(mat, rowForRow0, rowForRow1, out Matrix2x4 result);
        return result;
    }

    /// <summary>
    /// Swizzles a matrix, i.e. switches rows of the matrix.
    /// </summary>
    /// <param name="mat">The matrix to swizzle.</param>
    /// <param name="rowForRow0">Which row to place in <see cref="Row0"/>.</param>
    /// <param name="rowForRow1">Which row to place in <see cref="Row1"/>.</param>
    /// <param name="result">The swizzled matrix.</param>
    /// <exception cref="IndexOutOfRangeException">If any of the rows are outside of the range [0, 1].</exception>
    public static void Swizzle(in Matrix2x4 mat, int rowForRow0, int rowForRow1, out Matrix2x4 result)
    {
        result.Row0 = rowForRow0 switch
        {
            0 => mat.Row0,
            1 => mat.Row1,
            _ => throw new IndexOutOfRangeException($"{nameof(rowForRow0)} must be a number between 0 and 1. Got {rowForRow0}."),
        };

        result.Row1 = rowForRow1 switch
        {
            0 => mat.Row0,
            1 => mat.Row1,
            _ => throw new IndexOutOfRangeException($"{nameof(rowForRow1)} must be a number between 0 and 1. Got {rowForRow1}."),
        };
    }

    /// <summary>
    /// Scalar multiplication.
    /// </summary>
    /// <param name="left">left-hand operand.</param>
    /// <param name="right">right-hand operand.</param>
    /// <returns>A new Matrix2x4 which holds the result of the multiplication.</returns>
    [Pure]
    public static Matrix2x4 operator *(float left, Matrix2x4 right)
    {
        return Mult(right, left);
    }

    /// <summary>
    /// Scalar multiplication.
    /// </summary>
    /// <param name="left">left-hand operand.</param>
    /// <param name="right">right-hand operand.</param>
    /// <returns>A new Matrix2x4 which holds the result of the multiplication.</returns>
    [Pure]
    public static Matrix2x4 operator *(Matrix2x4 left, float right)
    {
        return Mult(left, right);
    }

    /// <summary>
    /// Matrix multiplication.
    /// </summary>
    /// <param name="left">left-hand operand.</param>
    /// <param name="right">right-hand operand.</param>
    /// <returns>A new Matrix2 which holds the result of the multiplication.</returns>
    [Pure]
    public static Matrix2x2 operator *(Matrix2x4 left, Matrix4x2 right)
    {
        return Mult(left, right);
    }

    /// <summary>
    /// Matrix multiplication.
    /// </summary>
    /// <param name="left">left-hand operand.</param>
    /// <param name="right">right-hand operand.</param>
    /// <returns>A new Matrix2x3 which holds the result of the multiplication.</returns>
    [Pure]
    public static Matrix2x3 operator *(Matrix2x4 left, Matrix4x3 right)
    {
        return Mult(left, right);
    }

    /// <summary>
    /// Matrix multiplication.
    /// </summary>
    /// <param name="left">left-hand operand.</param>
    /// <param name="right">right-hand operand.</param>
    /// <returns>A new Matrix2x4 which holds the result of the multiplication.</returns>
    [Pure]
    public static Matrix2x4 operator *(Matrix2x4 left, Matrix4x4 right)
    {
        return Mult(left, right);
    }

    /// <summary>
    /// Matrix addition.
    /// </summary>
    /// <param name="left">left-hand operand.</param>
    /// <param name="right">right-hand operand.</param>
    /// <returns>A new Matrix2 which holds the result of the addition.</returns>
    [Pure]
    public static Matrix2x4 operator +(Matrix2x4 left, Matrix2x4 right)
    {
        return Add(left, right);
    }

    /// <summary>
    /// Matrix subtraction.
    /// </summary>
    /// <param name="left">left-hand operand.</param>
    /// <param name="right">right-hand operand.</param>
    /// <returns>A new Matrix2x4 which holds the result of the subtraction.</returns>
    [Pure]
    public static Matrix2x4 operator -(Matrix2x4 left, Matrix2x4 right)
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
    public static bool operator ==(Matrix2x4 left, Matrix2x4 right)
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
    public static bool operator !=(Matrix2x4 left, Matrix2x4 right)
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
        return $"{row0}\n{row1}";
    }

    /// <summary>
    /// Returns the hashcode for this instance.
    /// </summary>
    /// <returns>A System.Int32 containing the unique hashcode for this instance.</returns>
    public override readonly int GetHashCode()
    {
        return HashCode.Combine(Row0, Row1);
    }

    /// <summary>
    /// Indicates whether this instance and a specified object are equal.
    /// </summary>
    /// <param name="obj">The object to compare to.</param>
    /// <returns>True if the instances are equal; false otherwise.</returns>
    [Pure]
    public override readonly bool Equals(object obj)
    {
        return obj is Matrix2x4 && Equals((Matrix2x4)obj);
    }

    /// <summary>
    /// Indicates whether the current matrix is equal to another matrix.
    /// </summary>
    /// <param name="other">An matrix to compare with this matrix.</param>
    /// <returns>true if the current matrix is equal to the matrix parameter; otherwise, false.</returns>
    [Pure]
    public readonly bool Equals(Matrix2x4 other)
    {
        return
            Row0 == other.Row0 &&
            Row1 == other.Row1;
    }
}
