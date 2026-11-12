using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace Kralt.Mathematics;

/// <summary>
/// Represents a 3D vector using three 32-bit integer numbers.
/// </summary>
/// <remarks>
/// The Vector3i structure is suitable for interoperation with unmanaged code requiring three consecutive integers.
/// </remarks>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct Vector3Int : IEquatable<Vector3Int>, IFormattable
{
    /// <summary>
    /// The X component of the Vector3i.
    /// </summary>
    public int x;

    /// <summary>
    /// The Y component of the Vector3i.
    /// </summary>
    public int y;

    /// <summary>
    /// The Z component of the Vector3i.
    /// </summary>
    public int z;

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector3Int"/> struct.
    /// </summary>
    /// <param name="value">The value that will initialize this instance.</param>
    public Vector3Int(int value)
    {
        x = value;
        y = value;
        z = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector3Int"/> struct.
    /// </summary>
    /// <param name="x">The x component of the Vector3.</param>
    /// <param name="y">The y component of the Vector3.</param>
    /// <param name="z">The z component of the Vector3.</param>
    public Vector3Int(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector3Int"/> struct.
    /// </summary>
    /// <param name="v">The <see cref="Vector2Int"/> to copy components from.</param>
    public Vector3Int(Vector2Int v)
    {
        x = v.x;
        y = v.y;
        z = 0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector3Int"/> struct.
    /// </summary>
    /// <param name="v">The <see cref="Vector2Int"/> to copy components from.</param>
    /// <param name="z">The z component of the new Vector3.</param>
    public Vector3Int(Vector2Int v, int z)
    {
        x = v.x;
        y = v.y;
        this.z = z;
    }

    /// <summary>
    /// Gets or sets the value at the index of the vector.
    /// </summary>
    /// <param name="index">The index of the component from the vector.</param>
    /// <exception cref="IndexOutOfRangeException">Thrown if the index is less than 0 or greater than 2.</exception>
    public int this[int index]
    {
        readonly get
        {
            if (((uint)index) < 3)
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
            if (((uint)index) < 3)
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
    private readonly ref int GetElementUnsafe(in Vector3Int v, int index)
    {
        ref int address = ref Unsafe.AsRef(in v.x);
        return ref Unsafe.Add(ref address, index);
    }

    /// <summary>
    /// Gets the manhattan length of the vector.
    /// </summary>
    public readonly int ManhattanLength => Math.Abs(x) + Math.Abs(y) + Math.Abs(z);

    /// <summary>
    /// Gets the squared euclidean length of the vector.
    /// </summary>
    public readonly int EuclideanLengthSquared => (x * x) + (y * y) + (z * z);

    /// <summary>
    /// Gets the euclidean length of the vector.
    /// </summary>
    public readonly float EuclideanLength => MathF.Sqrt((x * x) + (y * y) + (z * z));

    /// <summary>
    /// Returns a new vector that is the component-wise absolute value of the vector.
    /// </summary>
    /// <returns>The component-wise absolute value vector.</returns>
    public readonly Vector3Int Abs()
    {
        Vector3Int result = this;
        result.x = Math.Abs(result.x);
        result.y = Math.Abs(result.y);
        result.z = Math.Abs(result.z);
        return result;
    }

    /// <summary>
    /// Defines a unit-length Vector3i that points towards the X-axis.
    /// </summary>
    public static readonly Vector3Int UnitX = new Vector3Int(1, 0, 0);

    /// <summary>
    /// Defines a unit-length Vector3i that points towards the Y-axis.
    /// </summary>
    public static readonly Vector3Int UnitY = new Vector3Int(0, 1, 0);

    /// <summary>
    /// Defines a unit-length Vector3i that points towards the Z-axis.
    /// </summary>
    public static readonly Vector3Int UnitZ = new Vector3Int(0, 0, 1);

    /// <summary>
    /// Defines an instance with all components set to 0.
    /// </summary>
    public static readonly Vector3Int Zero = new Vector3Int(0, 0, 0);

    /// <summary>
    /// Defines an instance with all components set to 1.
    /// </summary>
    public static readonly Vector3Int One = new Vector3Int(1, 1, 1);

    /// <summary>
    /// Defines the size of the Vector3i struct in bytes.
    /// </summary>
    public static readonly int SizeInBytes = Unsafe.SizeOf<Vector3Int>();

    /// <summary>
    /// Adds two vectors.
    /// </summary>
    /// <param name="a">Left operand.</param>
    /// <param name="b">Right operand.</param>
    /// <returns>Result of operation.</returns>
    [Pure]
    public static Vector3Int Add(Vector3Int a, Vector3Int b)
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
    public static void Add(in Vector3Int a, in Vector3Int b, out Vector3Int result)
    {
        result.x = a.x + b.x;
        result.y = a.y + b.y;
        result.z = a.z + b.z;
    }

    /// <summary>
    /// Subtract one vector from another.
    /// </summary>
    /// <param name="a">First operand.</param>
    /// <param name="b">Second operand.</param>
    /// <returns>Result of subtraction.</returns>
    [Pure]
    public static Vector3Int Subtract(Vector3Int a, Vector3Int b)
    {
        Subtract(in a, in b, out a);
        return a;
    }

    /// <summary>
    /// Subtract one vector from another.
    /// </summary>
    /// <param name="a">First operand.</param>
    /// <param name="b">Second operand.</param>
    /// <param name="result">Result of subtraction.</param>
    public static void Subtract(in Vector3Int a, in Vector3Int b, out Vector3Int result)
    {
        result.x = a.x - b.x;
        result.y = a.y - b.y;
        result.z = a.z - b.z;
    }

    /// <summary>
    /// Multiplies a vector by a scalar.
    /// </summary>
    /// <param name="vector">Left operand.</param>
    /// <param name="scale">Right operand.</param>
    /// <returns>Result of the operation.</returns>
    [Pure]
    public static Vector3Int Multiply(Vector3Int vector, int scale)
    {
        Multiply(in vector, scale, out vector);
        return vector;
    }

    /// <summary>
    /// Multiplies a vector by a scalar.
    /// </summary>
    /// <param name="vector">Left operand.</param>
    /// <param name="scale">Right operand.</param>
    /// <param name="result">Result of the operation.</param>
    public static void Multiply(in Vector3Int vector, int scale, out Vector3Int result)
    {
        result.x = vector.x * scale;
        result.y = vector.y * scale;
        result.z = vector.z * scale;
    }

    /// <summary>
    /// Multiplies a vector by the components a vector (scale).
    /// </summary>
    /// <param name="vector">Left operand.</param>
    /// <param name="scale">Right operand.</param>
    /// <returns>Result of the operation.</returns>
    [Pure]
    public static Vector3Int Multiply(Vector3Int vector, Vector3Int scale)
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
    public static void Multiply(in Vector3Int vector, in Vector3Int scale, out Vector3Int result)
    {
        result.x = vector.x * scale.x;
        result.y = vector.y * scale.y;
        result.z = vector.z * scale.z;
    }

    /// <summary>
    /// Divides a vector by a scalar using integer division, floor(a/b).
    /// </summary>
    /// <param name="vector">Left operand.</param>
    /// <param name="scale">Right operand.</param>
    /// <returns>Result of the operation.</returns>
    [Pure]
    public static Vector3Int Divide(Vector3Int vector, int scale)
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
    public static void Divide(in Vector3Int vector, int scale, out Vector3Int result)
    {
        result.x = vector.x / scale;
        result.y = vector.y / scale;
        result.z = vector.z / scale;
    }

    /// <summary>
    /// Divides a vector by the components of a vector using integer division, floor(a/b).
    /// </summary>
    /// <param name="vector">Left operand.</param>
    /// <param name="scale">Right operand.</param>
    /// <returns>Result of the operation.</returns>
    [Pure]
    public static Vector3Int Divide(Vector3Int vector, Vector3Int scale)
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
    public static void Divide(in Vector3Int vector, in Vector3Int scale, out Vector3Int result)
    {
        result.x = vector.x / scale.x;
        result.y = vector.y / scale.y;
        result.z = vector.z / scale.z;
    }

    /// <summary>
    /// Returns a vector created from the smallest of the corresponding components of the given vectors.
    /// </summary>
    /// <param name="a">First operand.</param>
    /// <param name="b">Second operand.</param>
    /// <returns>The component-wise minimum.</returns>
    [Pure]
    public static Vector3Int ComponentMin(Vector3Int a, Vector3Int b)
    {
        Vector3Int result;
        result.x = Math.Min(a.x, b.x);
        result.y = Math.Min(a.y, b.y);
        result.z = Math.Min(a.z, b.z);
        return result;
    }

    /// <summary>
    /// Returns a vector created from the smallest of the corresponding components of the given vectors.
    /// </summary>
    /// <param name="a">First operand.</param>
    /// <param name="b">Second operand.</param>
    /// <param name="result">The component-wise minimum.</param>
    public static void ComponentMin(in Vector3Int a, in Vector3Int b, out Vector3Int result)
    {
        result.x = Math.Min(a.x, b.x);
        result.y = Math.Min(a.y, b.y);
        result.z = Math.Min(a.z, b.z);
    }

    /// <summary>
    /// Returns a vector created from the largest of the corresponding components of the given vectors.
    /// </summary>
    /// <param name="a">First operand.</param>
    /// <param name="b">Second operand.</param>
    /// <returns>The component-wise maximum.</returns>
    [Pure]
    public static Vector3Int ComponentMax(Vector3Int a, Vector3Int b)
    {
        Vector3Int result;
        result.x = Math.Max(a.x, b.x);
        result.y = Math.Max(a.y, b.y);
        result.z = Math.Max(a.z, b.z);
        return result;
    }

    /// <summary>
    /// Returns a vector created from the largest of the corresponding components of the given vectors.
    /// </summary>
    /// <param name="a">First operand.</param>
    /// <param name="b">Second operand.</param>
    /// <param name="result">The component-wise maximum.</param>
    public static void ComponentMax(in Vector3Int a, in Vector3Int b, out Vector3Int result)
    {
        result.x = Math.Max(a.x, b.x);
        result.y = Math.Max(a.y, b.y);
        result.z = Math.Max(a.z, b.z);
    }

    /// <summary>
    /// Clamp a vector to the given minimum and maximum vectors.
    /// </summary>
    /// <param name="vec">Input vector.</param>
    /// <param name="min">Minimum vector.</param>
    /// <param name="max">Maximum vector.</param>
    /// <returns>The clamped vector.</returns>
    [Pure]
    public static Vector3Int Clamp(Vector3Int vec, Vector3Int min, Vector3Int max)
    {
        Vector3Int result;
        result.x = Mathf.Clamp(vec.x, min.x, max.x);
        result.y = Mathf.Clamp(vec.y, min.y, max.y);
        result.z = Mathf.Clamp(vec.z, min.z, max.z);
        return result;
    }

    /// <summary>
    /// Clamp a vector to the given minimum and maximum vectors.
    /// </summary>
    /// <param name="vec">Input vector.</param>
    /// <param name="min">Minimum vector.</param>
    /// <param name="max">Maximum vector.</param>
    /// <param name="result">The clamped vector.</param>
    public static void Clamp(in Vector3Int vec, in Vector3Int min, in Vector3Int max, out Vector3Int result)
    {
        result.x = Mathf.Clamp(vec.x, min.x, max.x);
        result.y = Mathf.Clamp(vec.y, min.y, max.y);
        result.z = Mathf.Clamp(vec.z, min.z, max.z);
    }

    /// <summary>
    /// Take the component-wise absolute value of a vector.
    /// </summary>
    /// <param name="vec">The vector to apply component-wise absolute value to.</param>
    /// <returns>The component-wise absolute value vector.</returns>
    public static Vector3Int Abs(Vector3Int vec)
    {
        vec.x = Math.Abs(vec.x);
        vec.y = Math.Abs(vec.y);
        vec.z = Math.Abs(vec.z);
        return vec;
    }

    /// <summary>
    /// Take the component-wise absolute value of a vector.
    /// </summary>
    /// <param name="vec">The vector to apply component-wise absolute value to.</param>
    /// <param name="result">The component-wise absolute value vector.</param>
    public static void Abs(in Vector3Int vec, out Vector3Int result)
    {
        result.x = Math.Abs(vec.x);
        result.y = Math.Abs(vec.y);
        result.z = Math.Abs(vec.z);
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector2Int"/> with the X and Y components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector2Int Xy
    {
        get => Unsafe.As<Vector3Int, Vector2Int>(ref this);
        set
        {
            x = value.x;
            y = value.y;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector2Int"/> with the X and Z components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector2Int Xz
    {
        readonly get => new Vector2Int(x, z);
        set
        {
            x = value.x;
            z = value.y;
        }
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
    /// Gets or sets a <see cref="Vector2Int"/> with the Y and Z components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector2Int Yz
    {
        readonly get => new Vector2Int(y, z);
        set
        {
            y = value.x;
            z = value.y;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector2Int"/> with the Z and X components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector2Int Zx
    {
        readonly get => new Vector2Int(z, x);
        set
        {
            z = value.x;
            x = value.y;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector2Int"/> with the Z and Y components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector2Int Zy
    {
        readonly get => new Vector2Int(z, y);
        set
        {
            z = value.x;
            y = value.y;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector3Int"/> with the X, Z, and Y components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector3Int Xzy
    {
        readonly get => new Vector3Int(x, z, y);
        set
        {
            x = value.x;
            z = value.y;
            y = value.z;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector3Int"/> with the Y, X, and Z components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector3Int Yxz
    {
        readonly get => new Vector3Int(y, x, z);
        set
        {
            y = value.x;
            x = value.y;
            z = value.z;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector3Int"/> with the Y, Z, and X components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector3Int Yzx
    {
        readonly get => new Vector3Int(y, z, x);
        set
        {
            y = value.x;
            z = value.y;
            x = value.z;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector3Int"/> with the Z, X, and Y components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector3Int Zxy
    {
        readonly get => new Vector3Int(z, x, y);
        set
        {
            z = value.x;
            x = value.y;
            y = value.z;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector3Int"/> with the Z, Y, and X components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector3Int Zyx
    {
        readonly get => new Vector3Int(z, y, x);
        set
        {
            z = value.x;
            y = value.y;
            x = value.z;
        }
    }

    /// <summary>
    /// Gets a <see cref="Vector3"/> object with the same component values as the <see cref="Vector3Int"/> instance.
    /// </summary>
    /// <returns>The resulting <see cref="Vector3"/> instance.</returns>
    public readonly Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }

    /// <summary>
    /// Gets a <see cref="Vector3"/> object with the same component values as the <see cref="Vector3Int"/> instance.
    /// </summary>
    /// <param name="input">The given <see cref="Vector3Int"/> to convert.</param>
    /// <param name="result">The resulting <see cref="Vector3"/>.</param>
    public static void ToVector3(in Vector3Int input, out Vector3 result)
    {
        result.x = input.x;
        result.y = input.y;
        result.z = input.z;
    }

    /// <summary>
    /// Adds two instances.
    /// </summary>
    /// <param name="left">The first instance.</param>
    /// <param name="right">The second instance.</param>
    /// <returns>The result of the calculation.</returns>
    [Pure]
    public static Vector3Int operator +(Vector3Int left, Vector3Int right)
    {
        left.x += right.x;
        left.y += right.y;
        left.z += right.z;
        return left;
    }

    /// <summary>
    /// Subtracts two instances.
    /// </summary>
    /// <param name="left">The first instance.</param>
    /// <param name="right">The second instance.</param>
    /// <returns>The result of the calculation.</returns>
    [Pure]
    public static Vector3Int operator -(Vector3Int left, Vector3Int right)
    {
        left.x -= right.x;
        left.y -= right.y;
        left.z -= right.z;
        return left;
    }

    /// <summary>
    /// Negates an instance.
    /// </summary>
    /// <param name="vec">The instance.</param>
    /// <returns>The result of the calculation.</returns>
    [Pure]
    public static Vector3Int operator -(Vector3Int vec)
    {
        vec.x = -vec.x;
        vec.y = -vec.y;
        vec.z = -vec.z;
        return vec;
    }

    /// <summary>
    /// Multiplies an instance by an integer scalar.
    /// </summary>
    /// <param name="vec">The instance.</param>
    /// <param name="scale">The scalar.</param>
    /// <returns>The result of the calculation.</returns>
    [Pure]
    public static Vector3Int operator *(Vector3Int vec, int scale)
    {
        vec.x *= scale;
        vec.y *= scale;
        vec.z *= scale;
        return vec;
    }

    /// <summary>
    /// Multiplies an instance by an integer scalar.
    /// </summary>
    /// <param name="scale">The scalar.</param>
    /// <param name="vec">The instance.</param>
    /// <returns>The result of the calculation.</returns>
    [Pure]
    public static Vector3Int operator *(int scale, Vector3Int vec)
    {
        vec.x *= scale;
        vec.y *= scale;
        vec.z *= scale;
        return vec;
    }

    /// <summary>
    /// Component-wise multiplication between the specified instance by a scale vector.
    /// </summary>
    /// <param name="scale">Left operand.</param>
    /// <param name="vec">Right operand.</param>
    /// <returns>Result of multiplication.</returns>
    [Pure]
    public static Vector3Int operator *(Vector3Int vec, Vector3Int scale)
    {
        vec.x *= scale.x;
        vec.y *= scale.y;
        vec.z *= scale.z;
        return vec;
    }

    /// <summary>
    /// Divides the instance by a scalar using integer division, floor(a/b).
    /// </summary>
    /// <param name="vec">The instance.</param>
    /// <param name="scale">The scalar.</param>
    /// <returns>The result of the calculation.</returns>
    [Pure]
    public static Vector3Int operator /(Vector3Int vec, int scale)
    {
        vec.x /= scale;
        vec.y /= scale;
        vec.z /= scale;
        return vec;
    }

    /// <summary>
    /// Component-wise division between the specified instance by a scale vector.
    /// </summary>
    /// <param name="vec">Left operand.</param>
    /// <param name="scale">Right operand.</param>
    /// <returns>Result of the division.</returns>
    [Pure]
    public static Vector3Int operator /(Vector3Int vec, Vector3Int scale)
    {
        vec.x /= scale.x;
        vec.y /= scale.y;
        vec.z /= scale.z;
        return vec;
    }

    /// <summary>
    /// Compares two instances for equality.
    /// </summary>
    /// <param name="left">The first instance.</param>
    /// <param name="right">The second instance.</param>
    /// <returns>True, if left equals right; false otherwise.</returns>
    public static bool operator ==(Vector3Int left, Vector3Int right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Compares two instances for inequality.
    /// </summary>
    /// <param name="left">The first instance.</param>
    /// <param name="right">The second instance.</param>
    /// <returns>True, if left does not equal right; false otherwise.</returns>
    public static bool operator !=(Vector3Int left, Vector3Int right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Converts OpenTK.Vector3i to OpenTK.Vector3.
    /// </summary>
    /// <param name="vec">The Vector3i to convert.</param>
    /// <returns>The resulting Vector3.</returns>
    [Pure]
    public static implicit operator Vector3(Vector3Int vec)
    {
        return new Vector3(vec.x, vec.y, vec.z);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector3Int"/> struct using a tuple containing the component
    /// values.
    /// </summary>
    /// <param name="values">A tuple containing the component values.</param>
    /// <returns>A new instance of the <see cref="Vector3Int"/> struct with the given component values.</returns>
    [Pure]
    public static implicit operator Vector3Int((int X, int Y, int Z) values)
    {
        return new Vector3Int(values.X, values.Y, values.Z);
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
            "({0}{3} {1}{3} {2})",
            x.ToString(format, formatProvider),
            y.ToString(format, formatProvider),
            z.ToString(format, formatProvider),
            Mathf.GetListSeparator(formatProvider));
    }

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
        return obj is Vector3Int && Equals((Vector3Int)obj);
    }

    /// <inheritdoc />
    public readonly bool Equals(Vector3Int other)
    {
        return x == other.x &&
               y == other.y &&
               z == other.z;
    }

    /// <inheritdoc />
    public override readonly int GetHashCode()
    {
        return HashCode.Combine(x, y, z);
    }

    /// <summary>
    /// Deconstructs the vector into it's individual components.
    /// </summary>
    /// <param name="x">The X component of the vector.</param>
    /// <param name="y">The Y component of the vector.</param>
    /// <param name="z">The Z component of the vector.</param>
    [Pure]
    public readonly void Deconstruct(out int x, out int y, out int z)
    {
        x = this.x;
        y = this.y;
        z = this.z;
    }
}
