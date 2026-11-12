using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace Kralt.Mathematics;

/// <summary>
/// Represents a 2D vector using two 32-bit integer numbers.
/// </summary>
/// <remarks>
/// The Vector2i structure is suitable for interoperation with unmanaged code requiring two consecutive integers.
/// </remarks>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct Vector2Int : IEquatable<Vector2Int>, IFormattable
{
    /// <summary>
    /// The X component of the Vector2i.
    /// </summary>
    public int x;

    /// <summary>
    /// The Y component of the Vector2i.
    /// </summary>
    public int y;

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector2Int"/> struct.
    /// </summary>
    /// <param name="value">The value that will initialize this instance.</param>
    public Vector2Int(int value)
    {
        x = value;
        y = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector2Int"/> struct.
    /// </summary>
    /// <param name="x">The X component of the Vector2i.</param>
    /// <param name="y">The Y component of the Vector2i.</param>
    public Vector2Int(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    /// <summary>
    /// Gets or sets the value at the index of the vector.
    /// </summary>
    /// <param name="index">The index of the component from the vector.</param>
    /// <exception cref="IndexOutOfRangeException">Thrown if the index is less than 0 or greater than 1.</exception>
    public int this[int index]
    {
        readonly get
        {
            if (((uint)index) < 2)
            {
                return GetElementUnsafe(in this, index);
            }
            else
            {
                throw new IndexOutOfRangeException("You tried to access this vector at index: " + index);
            }
        }

        set
        {
            if (((uint)index) < 2)
            {
                GetElementUnsafe(in this, index) = value;
            }
            else
            {
                throw new IndexOutOfRangeException("You tried to set this vector at index: " + index);
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private readonly ref int GetElementUnsafe(in Vector2Int v, int index)
    {
        ref int address = ref Unsafe.AsRef(in v.x);
        return ref Unsafe.Add(ref address, index);
    }

    /// <summary>
    /// Gets the manhattan length of the vector.
    /// </summary>
    public readonly int ManhattanLength => Math.Abs(x) + Math.Abs(y);

    /// <summary>
    /// Gets the squared euclidean length of the vector.
    /// </summary>
    public readonly int EuclideanLengthSquared => (x * x) + (y * y);

    /// <summary>
    /// Gets the euclidean length of the vector.
    /// </summary>
    public readonly float EuclideanLength => MathF.Sqrt((x * x) + (y * y));

    /// <summary>
    /// Gets the perpendicular vector on the right side of this vector.
    /// </summary>
    public readonly Vector2Int PerpendicularRight => new Vector2Int(y, -x);

    /// <summary>
    /// Gets the perpendicular vector on the left side of this vector.
    /// </summary>
    public readonly Vector2Int PerpendicularLeft => new Vector2Int(-y, x);

    /// <summary>
    /// Returns a new vector that is the component-wise absolute value of the vector.
    /// </summary>
    /// <returns>The component-wise absolute value vector.</returns>
    public readonly Vector2Int Abs()
    {
        Vector2Int result = this;
        result.x = Math.Abs(result.x);
        result.y = Math.Abs(result.y);
        return result;
    }

    /// <summary>
    /// Defines a unit-length <see cref="Vector2Int"/> that points towards the X-axis.
    /// </summary>
    public static readonly Vector2Int UnitX = new Vector2Int(1, 0);

    /// <summary>
    /// Defines a unit-length <see cref="Vector2Int"/> that points towards the Y-axis.
    /// </summary>
    public static readonly Vector2Int UnitY = new Vector2Int(0, 1);

    /// <summary>
    /// Defines an instance with all components set to 0.
    /// </summary>
    public static readonly Vector2Int Zero = new Vector2Int(0, 0);

    /// <summary>
    /// Defines an instance with all components set to 1.
    /// </summary>
    public static readonly Vector2Int One = new Vector2Int(1, 1);

    /// <summary>
    /// Defines the size of the <see cref="Vector2Int"/> struct in bytes.
    /// </summary>
    public static readonly int SizeInBytes = Unsafe.SizeOf<Vector2Int>();

    /// <summary>
    /// Adds two vectors.
    /// </summary>
    /// <param name="a">Left operand.</param>
    /// <param name="b">Right operand.</param>
    /// <returns>Result of operation.</returns>
    [Pure]
    public static Vector2Int Add(Vector2Int a, Vector2Int b)
    {
        Add(in a, in b, out a);
        return a;
    }

    /// <summary>
    /// Adds two vectors.
    /// </summary>
    /// <param name="a">Left operand.</param>
    /// <param name="b">Right operand.</param>
    /// <param name="result">Result of operation.</param>
    public static void Add(in Vector2Int a, in Vector2Int b, out Vector2Int result)
    {
        result.x = a.x + b.x;
        result.y = a.y + b.y;
    }

    /// <summary>
    /// Subtract one Vector from another.
    /// </summary>
    /// <param name="a">First operand.</param>
    /// <param name="b">Second operand.</param>
    /// <returns>Result of subtraction.</returns>
    [Pure]
    public static Vector2Int Subtract(Vector2Int a, Vector2Int b)
    {
        Subtract(in a, in b, out a);
        return a;
    }

    /// <summary>
    /// Subtract one Vector from another.
    /// </summary>
    /// <param name="a">First operand.</param>
    /// <param name="b">Second operand.</param>
    /// <param name="result">Result of subtraction.</param>
    public static void Subtract(in Vector2Int a, in Vector2Int b, out Vector2Int result)
    {
        result.x = a.x - b.x;
        result.y = a.y - b.y;
    }

    /// <summary>
    /// Multiplies a vector by an integer scalar.
    /// </summary>
    /// <param name="vector">Left operand.</param>
    /// <param name="scale">Right operand.</param>
    /// <returns>Result of the operation.</returns>
    [Pure]
    public static Vector2Int Multiply(Vector2Int vector, int scale)
    {
        Multiply(in vector, scale, out vector);
        return vector;
    }

    /// <summary>
    /// Multiplies a vector by an integer scalar.
    /// </summary>
    /// <param name="vector">Left operand.</param>
    /// <param name="scale">Right operand.</param>
    /// <param name="result">Result of the operation.</param>
    public static void Multiply(in Vector2Int vector, int scale, out Vector2Int result)
    {
        result.x = vector.x * scale;
        result.y = vector.y * scale;
    }

    /// <summary>
    /// Multiplies a vector by the components a vector (scale).
    /// </summary>
    /// <param name="vector">Left operand.</param>
    /// <param name="scale">Right operand.</param>
    /// <returns>Result of the operation.</returns>
    [Pure]
    public static Vector2Int Multiply(Vector2Int vector, Vector2Int scale)
    {
        Multiply(in vector, in scale, out vector);
        return vector;
    }

    /// <summary>
    /// Multiplies a vector by the components of a vector (scale).
    /// </summary>
    /// <param name="vector">Left operand.</param>
    /// <param name="scale">Right operand.</param>
    /// <param name="result">Result of the operation.</param>
    public static void Multiply(in Vector2Int vector, in Vector2Int scale, out Vector2Int result)
    {
        result.x = vector.x * scale.x;
        result.y = vector.y * scale.y;
    }

    /// <summary>
    /// Divides a vector by a scalar using integer division, floor(a/b).
    /// </summary>
    /// <param name="vector">Left operand.</param>
    /// <param name="scale">Right operand.</param>
    /// <returns>Result of the operation.</returns>
    [Pure]
    public static Vector2Int Divide(Vector2Int vector, int scale)
    {
        Divide(in vector, scale, out vector);
        return vector;
    }

    /// <summary>
    /// Divides a vector by a scalar using integer division, floor(a/b).
    /// </summary>
    /// <param name="vector">Left operand.</param>
    /// <param name="scale">Right operand.</param>
    /// <param name="result">Result of the operation.</param>
    public static void Divide(in Vector2Int vector, int scale, out Vector2Int result)
    {
        result.x = vector.x / scale;
        result.y = vector.y / scale;
    }

    /// <summary>
    /// Divides a vector by the components of a vector using integer division, floor(a/b).
    /// </summary>
    /// <param name="vector">Left operand.</param>
    /// <param name="scale">Right operand.</param>
    /// <returns>Result of the operation.</returns>
    [Pure]
    public static Vector2Int Divide(Vector2Int vector, Vector2Int scale)
    {
        Divide(in vector, in scale, out vector);
        return vector;
    }

    /// <summary>
    /// Divides a vector by the components of a vector using integer division, floor(a/b).
    /// </summary>
    /// <param name="vector">Left operand.</param>
    /// <param name="scale">Right operand.</param>
    /// <param name="result">Result of the operation.</param>
    public static void Divide(in Vector2Int vector, in Vector2Int scale, out Vector2Int result)
    {
        result.x = vector.x / scale.x;
        result.y = vector.y / scale.y;
    }

    /// <summary>
    /// Returns a vector created from the smallest of the corresponding components of the given vectors.
    /// </summary>
    /// <param name="a">First operand.</param>
    /// <param name="b">Second operand.</param>
    /// <returns>The component-wise minimum.</returns>
    [Pure]
    public static Vector2Int ComponentMin(Vector2Int a, Vector2Int b)
    {
        a.x = Math.Min(a.x, b.x);
        a.y = Math.Min(a.y, b.y);
        return a;
    }

    /// <summary>
    /// Returns a vector created from the smallest of the corresponding components of the given vectors.
    /// </summary>
    /// <param name="a">First operand.</param>
    /// <param name="b">Second operand.</param>
    /// <param name="result">The component-wise minimum.</param>
    public static void ComponentMin(in Vector2Int a, in Vector2Int b, out Vector2Int result)
    {
        result.x = Math.Min(a.x, b.x);
        result.y = Math.Min(a.y, b.y);
    }

    /// <summary>
    /// Returns a vector created from the largest of the corresponding components of the given vectors.
    /// </summary>
    /// <param name="a">First operand.</param>
    /// <param name="b">Second operand.</param>
    /// <returns>The component-wise maximum.</returns>
    [Pure]
    public static Vector2Int ComponentMax(Vector2Int a, Vector2Int b)
    {
        a.x = Math.Max(a.x, b.x);
        a.y = Math.Max(a.y, b.y);
        return a;
    }

    /// <summary>
    /// Returns a vector created from the largest of the corresponding components of the given vectors.
    /// </summary>
    /// <param name="a">First operand.</param>
    /// <param name="b">Second operand.</param>
    /// <param name="result">The component-wise maximum.</param>
    public static void ComponentMax(in Vector2Int a, in Vector2Int b, out Vector2Int result)
    {
        result.x = Math.Max(a.x, b.x);
        result.y = Math.Max(a.y, b.y);
    }

    /// <summary>
    /// Clamp a vector to the given minimum and maximum vectors.
    /// </summary>
    /// <param name="vec">Input vector.</param>
    /// <param name="min">Minimum vector.</param>
    /// <param name="max">Maximum vector.</param>
    /// <returns>The clamped vector.</returns>
    [Pure]
    public static Vector2Int Clamp(Vector2Int vec, Vector2Int min, Vector2Int max)
    {
        vec.x = Mathf.Clamp(vec.x, min.x, max.x);
        vec.y = Mathf.Clamp(vec.y, min.y, max.y);
        return vec;
    }

    /// <summary>
    /// Clamp a vector to the given minimum and maximum vectors.
    /// </summary>
    /// <param name="vec">Input vector.</param>
    /// <param name="min">Minimum vector.</param>
    /// <param name="max">Maximum vector.</param>
    /// <param name="result">The clamped vector.</param>
    public static void Clamp(in Vector2Int vec, in Vector2Int min, in Vector2Int max, out Vector2Int result)
    {
        result.x = Mathf.Clamp(vec.x, min.x, max.x);
        result.y = Mathf.Clamp(vec.y, min.y, max.y);
    }

    /// <summary>
    /// Take the component-wise absolute value of a vector.
    /// </summary>
    /// <param name="vec">The vector to apply component-wise absolute value to.</param>
    /// <returns>The component-wise absolute value vector.</returns>
    public static Vector2Int Abs(Vector2Int vec)
    {
        vec.x = Math.Abs(vec.x);
        vec.y = Math.Abs(vec.y);
        return vec;
    }

    /// <summary>
    /// Take the component-wise absolute value of a vector.
    /// </summary>
    /// <param name="vec">The vector to apply component-wise absolute value to.</param>
    /// <param name="result">The component-wise absolute value vector.</param>
    public static void Abs(in Vector2Int vec, out Vector2Int result)
    {
        result.x = Math.Abs(vec.x);
        result.y = Math.Abs(vec.y);
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector2Int"/> with the Y and X components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector2Int Yx
    {
        readonly get => new Vector2Int(y, x);
        set
        {
            y = value.x;
            x = value.y;
        }
    }

    /// <summary>
    /// Gets a <see cref="Vector2"/> object with the same component values as the <see cref="Vector2Int"/> instance.
    /// </summary>
    /// <returns>The resulting <see cref="Vector3"/> instance.</returns>
    public readonly Vector2 ToVector2()
    {
        return new Vector2(x, y);
    }

    /// <summary>
    /// Gets a <see cref="Vector2"/> object with the same component values as the <see cref="Vector2Int"/> instance.
    /// </summary>
    /// <param name="input">The given <see cref="Vector2Int"/> to convert.</param>
    /// <param name="result">The resulting <see cref="Vector2"/>.</param>
    public static void ToVector2(in Vector2Int input, out Vector2 result)
    {
        result.x = input.x;
        result.y = input.y;
    }

    /// <summary>
    /// Adds the specified instances.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Result of addition.</returns>
    [Pure]
    public static Vector2Int operator +(Vector2Int left, Vector2Int right)
    {
        left.x += right.x;
        left.y += right.y;
        return left;
    }

    /// <summary>
    /// Subtracts the specified instances.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Result of subtraction.</returns>
    [Pure]
    public static Vector2Int operator -(Vector2Int left, Vector2Int right)
    {
        left.x -= right.x;
        left.y -= right.y;
        return left;
    }

    /// <summary>
    /// Negates the specified instance.
    /// </summary>
    /// <param name="vec">Operand.</param>
    /// <returns>Result of negation.</returns>
    [Pure]
    public static Vector2Int operator -(Vector2Int vec)
    {
        vec.x = -vec.x;
        vec.y = -vec.y;
        return vec;
    }

    /// <summary>
    /// Multiplies the specified instance by a scalar.
    /// </summary>
    /// <param name="vec">Left operand.</param>
    /// <param name="scale">Right operand.</param>
    /// <returns>Result of multiplication.</returns>
    [Pure]
    public static Vector2Int operator *(Vector2Int vec, int scale)
    {
        vec.x *= scale;
        vec.y *= scale;
        return vec;
    }

    /// <summary>
    /// Multiplies the specified instance by a scalar.
    /// </summary>
    /// <param name="scale">Left operand.</param>
    /// <param name="vec">Right operand.</param>
    /// <returns>Result of multiplication.</returns>
    [Pure]
    public static Vector2Int operator *(int scale, Vector2Int vec)
    {
        vec.x *= scale;
        vec.y *= scale;
        return vec;
    }

    /// <summary>
    /// Component-wise multiplication between the specified instance by a scale vector.
    /// </summary>
    /// <param name="scale">Left operand.</param>
    /// <param name="vec">Right operand.</param>
    /// <returns>Result of multiplication.</returns>
    [Pure]
    public static Vector2Int operator *(Vector2Int vec, Vector2Int scale)
    {
        vec.x *= scale.x;
        vec.y *= scale.y;
        return vec;
    }

    /// <summary>
    /// Divides the instance by a scalar using integer division, floor(a/b).
    /// </summary>
    /// <param name="vec">Left operand.</param>
    /// <param name="scale">Right operand.</param>
    /// <returns>Result of the division.</returns>
    [Pure]
    public static Vector2Int operator /(Vector2Int vec, int scale)
    {
        vec.x /= scale;
        vec.y /= scale;
        return vec;
    }

    /// <summary>
    /// Component-wise division between the specified instance by a scale vector.
    /// </summary>
    /// <param name="vec">Left operand.</param>
    /// <param name="scale">Right operand.</param>
    /// <returns>Result of the division.</returns>
    [Pure]
    public static Vector2Int operator /(Vector2Int vec, Vector2Int scale)
    {
        vec.x /= scale.x;
        vec.y /= scale.y;
        return vec;
    }

    /// <summary>
    /// Compares the specified instances for equality.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>True if both instances are equal; false otherwise.</returns>
    public static bool operator ==(Vector2Int left, Vector2Int right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Compares the specified instances for inequality.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>True if both instances are not equal; false otherwise.</returns>
    public static bool operator !=(Vector2Int left, Vector2Int right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Converts OpenTK.Vector2i to OpenTK.Vector2.
    /// </summary>
    /// <param name="vec">The Vector2i to convert.</param>
    /// <returns>The resulting Vector2.</returns>
    [Pure]
    public static implicit operator Vector2(Vector2Int vec)
    {
        return new Vector2(vec.x, vec.y);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector2Int"/> struct using a tuple containing the component
    /// values.
    /// </summary>
    /// <param name="values">A tuple containing the component values.</param>
    /// <returns>A new instance of the <see cref="Vector2Int"/> struct with the given component values.</returns>
    [Pure]
    public static implicit operator Vector2Int((int X, int Y) values)
    {
        return new Vector2Int(values.X, values.Y);
    }

    /// <summary>
    /// Converts <see cref="Vector2Int"/> to <see cref="System.Drawing.Point"/>.
    /// </summary>
    /// <param name="vec">The <see cref="Vector2Int"/> to cast.</param>
    /// <returns>The resulting <see cref="System.Drawing.Point"/>.</returns>
    [Pure]
    public static explicit operator System.Drawing.Point(Vector2Int vec)
    {
        return new System.Drawing.Point(vec.x, vec.y);
    }

    /// <summary>
    /// Converts <see cref="Vector2Int"/> to <see cref="System.Drawing.Size"/>.
    /// </summary>
    /// <param name="vec">The <see cref="Vector2Int"/> to cast.</param>
    /// <returns>The resulting <see cref="System.Drawing.Size"/>.</returns>
    [Pure]
    public static explicit operator System.Drawing.Size(Vector2Int vec)
    {
        return new System.Drawing.Size(vec.x, vec.y);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return ToString(null, null);
    }

    /// <inheritdoc cref="ToString(string, IFormatProvider)"/>
    public string ToString(string format)
    {
        return ToString(format, null);
    }

    /// <inheritdoc cref="ToString(string, IFormatProvider)"/>
    public string ToString(IFormatProvider formatProvider)
    {
        return ToString(null, formatProvider);
    }

    /// <inheritdoc />
    public readonly string ToString(string format, IFormatProvider formatProvider)
    {
        return string.Format(
            "({0}{2} {1})",
            x.ToString(format, formatProvider),
            y.ToString(format, formatProvider),
            Mathf.GetListSeparator(formatProvider));
    }

    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
        return obj is Vector2Int && Equals((Vector2Int)obj);
    }

    /// <inheritdoc/>
    public readonly bool Equals(Vector2Int other)
    {
        return x == other.x &&
               y == other.y;
    }

    /// <inheritdoc/>
    public override readonly int GetHashCode()
    {
        return HashCode.Combine(x, y);
    }

    /// <summary>
    /// Deconstructs the vector into it's individual components.
    /// </summary>
    /// <param name="x">The X component of the vector.</param>
    /// <param name="y">The Y component of the vector.</param>
    [Pure]
    public readonly void Deconstruct(out int x, out int y)
    {
        x = this.x;
        y = this.y;
    }
}
