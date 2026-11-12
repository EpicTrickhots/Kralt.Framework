using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace Kralt.Mathematics;

/// <summary>
/// Represents a 4D vector using four 32-bit integer numbers.
/// </summary>
/// <remarks>
/// The Vector4i structure is suitable for interoperation with unmanaged code requiring four consecutive integers.
/// </remarks>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct Vector4Int : IEquatable<Vector4Int>, IFormattable
{
    /// <summary>
    /// The X component of the Vector4i.
    /// </summary>
    public int x;

    /// <summary>
    /// The Y component of the Vector4i.
    /// </summary>
    public int y;

    /// <summary>
    /// The Z component of the Vector4i.
    /// </summary>
    public int z;

    /// <summary>
    /// The W component of the Vector4i.
    /// </summary>
    public int w;

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector4Int"/> struct.
    /// </summary>
    /// <param name="value">The value that will initialize this instance.</param>
    public Vector4Int(int value)
    {
        x = value;
        y = value;
        z = value;
        w = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector4Int"/> struct.
    /// </summary>
    /// <param name="x">The X component of the Vector4i.</param>
    /// <param name="y">The Y component of the Vector4i.</param>
    /// <param name="z">The Z component of the Vector4i.</param>
    /// <param name="w">The W component of the Vector4i.</param>
    public Vector4Int(int x, int y, int z, int w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector4Int"/> struct.
    /// </summary>
    /// <param name="v">The <see cref="Vector2Int"/> to copy components from.</param>
    public Vector4Int(Vector2Int v)
    {
        x = v.x;
        y = v.y;
        z = 0;
        w = 0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector4Int"/> struct.
    /// </summary>
    /// <param name="v1">The <see cref="Vector2Int"/> to get the X and Y components for the Vector4.</param>
    /// <param name="v2">The <see cref="Vector2Int"/> to get the Z and W components for the Vector4.</param>
    public Vector4Int(Vector2Int v1, Vector2Int v2)
    {
        x = v1.x;
        y = v1.y;
        z = v2.x;
        w = v2.y;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector4Int"/> struct.
    /// </summary>
    /// <param name="v">The <see cref="Vector3Int"/> to copy components from.</param>
    public Vector4Int(Vector3Int v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
        w = 0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector4Int"/> struct.
    /// </summary>
    /// <param name="v">The <see cref="Vector3Int"/> to copy components from.</param>
    /// <param name="w">The w component of the new Vector4.</param>
    public Vector4Int(Vector3Int v, int w)
    {
        x = v.x;
        y = v.y;
        z = v.z;
        this.w = w;
    }

    /// <summary>
    /// Gets or sets the value at the index of the vector.
    /// </summary>
    /// <param name="index">The index of the component from the vector.</param>
    /// <exception cref="IndexOutOfRangeException">Thrown if the index is less than 0 or greater than 3.</exception>
    public int this[int index]
    {
        readonly get
        {
            if (((uint)index) < 4)
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
            if (((uint)index) < 4)
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
    private readonly ref int GetElementUnsafe(in Vector4Int v, int index)
    {
        ref int address = ref Unsafe.AsRef(in v.x);
        return ref Unsafe.Add(ref address, index);
    }

    /// <summary>
    /// Gets the manhattan length of the vector.
    /// </summary>
    public readonly int ManhattanLength => Math.Abs(x) + Math.Abs(y) + Math.Abs(z) + Math.Abs(w);

    /// <summary>
    /// Gets the squared euclidean length of the vector.
    /// </summary>
    public readonly int EuclideanLengthSquared => (x * x) + (y * y) + (z * z) + (w * w);

    /// <summary>
    /// Gets the euclidean length of the vector.
    /// </summary>
    public readonly float EuclideanLength => MathF.Sqrt((x * x) + (y * y) + (z * z) + (w * w));

    /// <summary>
    /// Returns a new vector that is the component-wise absolute value of the vector.
    /// </summary>
    /// <returns>The component-wise absolute value vector.</returns>
    public readonly Vector4Int Abs()
    {
        Vector4Int result = this;
        result.x = Math.Abs(result.x);
        result.y = Math.Abs(result.y);
        result.z = Math.Abs(result.z);
        result.w = Math.Abs(result.w);
        return result;
    }

    /// <summary>
    /// Defines a unit-length <see cref="Vector4Int"/> that points towards the X-axis.
    /// </summary>
    public static readonly Vector4Int UnitX = new Vector4Int(1, 0, 0, 0);

    /// <summary>
    /// Defines a unit-length <see cref="Vector4Int"/> that points towards the Y-axis.
    /// </summary>
    public static readonly Vector4Int UnitY = new Vector4Int(0, 1, 0, 0);

    /// <summary>
    /// Defines a unit-length <see cref="Vector4Int"/> that points towards the Z-axis.
    /// </summary>
    public static readonly Vector4Int UnitZ = new Vector4Int(0, 0, 1, 0);

    /// <summary>
    /// Defines a unit-length <see cref="Vector4Int"/> that points towards the W-axis.
    /// </summary>
    public static readonly Vector4Int UnitW = new Vector4Int(0, 0, 0, 1);

    /// <summary>
    /// Defines an instance with all components set to 0.
    /// </summary>
    public static readonly Vector4Int Zero = new Vector4Int(0, 0, 0, 0);

    /// <summary>
    /// Defines an instance with all components set to 1.
    /// </summary>
    public static readonly Vector4Int One = new Vector4Int(1, 1, 1, 1);

    /// <summary>
    /// Defines the size of the <see cref="Vector4Int"/> struct in bytes.
    /// </summary>
    public static readonly int SizeInBytes = Unsafe.SizeOf<Vector4Int>();

    /// <summary>
    /// Adds two vectors.
    /// </summary>
    /// <param name="a">Left operand.</param>
    /// <param name="b">Right operand.</param>
    /// <returns>Result of operation.</returns>
    [Pure]
    public static Vector4Int Add(Vector4Int a, Vector4Int b)
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
    public static void Add(in Vector4Int a, in Vector4Int b, out Vector4Int result)
    {
        result.x = a.x + b.x;
        result.y = a.y + b.y;
        result.z = a.z + b.z;
        result.w = a.w + b.w;
    }

    /// <summary>
    /// Subtract one Vector from another.
    /// </summary>
    /// <param name="a">First operand.</param>
    /// <param name="b">Second operand.</param>
    /// <returns>Result of subtraction.</returns>
    [Pure]
    public static Vector4Int Subtract(Vector4Int a, Vector4Int b)
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
    public static void Subtract(in Vector4Int a, in Vector4Int b, out Vector4Int result)
    {
        result.x = a.x - b.x;
        result.y = a.y - b.y;
        result.z = a.z - b.z;
        result.w = a.w - b.w;
    }

    /// <summary>
    /// Multiplies a vector by an integer scalar.
    /// </summary>
    /// <param name="vector">Left operand.</param>
    /// <param name="scale">Right operand.</param>
    /// <returns>Result of the operation.</returns>
    [Pure]
    public static Vector4Int Multiply(Vector4Int vector, int scale)
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
    public static void Multiply(in Vector4Int vector, int scale, out Vector4Int result)
    {
        result.x = vector.x * scale;
        result.y = vector.y * scale;
        result.z = vector.z * scale;
        result.w = vector.w * scale;
    }

    /// <summary>
    /// Multiplies a vector by the components a vector (scale).
    /// </summary>
    /// <param name="vector">Left operand.</param>
    /// <param name="scale">Right operand.</param>
    /// <returns>Result of the operation.</returns>
    [Pure]
    public static Vector4Int Multiply(Vector4Int vector, Vector4Int scale)
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
    public static void Multiply(in Vector4Int vector, in Vector4Int scale, out Vector4Int result)
    {
        result.x = vector.x * scale.x;
        result.y = vector.y * scale.y;
        result.z = vector.z * scale.z;
        result.w = vector.w * scale.w;
    }

    /// <summary>
    /// Divides a vector by a scalar using integer division, floor(a/b).
    /// </summary>
    /// <param name="vector">Left operand.</param>
    /// <param name="scale">Right operand.</param>
    /// <returns>Result of the operation.</returns>
    [Pure]
    public static Vector4Int Divide(Vector4Int vector, int scale)
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
    public static void Divide(in Vector4Int vector, int scale, out Vector4Int result)
    {
        result.x = vector.x / scale;
        result.y = vector.y / scale;
        result.z = vector.z / scale;
        result.w = vector.w / scale;
    }

    /// <summary>
    /// Divides a vector by the components of a vector using integer division, floor(a/b).
    /// </summary>
    /// <param name="vector">Left operand.</param>
    /// <param name="scale">Right operand.</param>
    /// <returns>Result of the operation.</returns>
    [Pure]
    public static Vector4Int Divide(Vector4Int vector, Vector4Int scale)
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
    public static void Divide(in Vector4Int vector, in Vector4Int scale, out Vector4Int result)
    {
        result.x = vector.x / scale.x;
        result.y = vector.y / scale.y;
        result.z = vector.z / scale.z;
        result.w = vector.w / scale.w;
    }

    /// <summary>
    /// Returns a vector created from the smallest of the corresponding components of the given vectors.
    /// </summary>
    /// <param name="a">First operand.</param>
    /// <param name="b">Second operand.</param>
    /// <returns>The component-wise minimum.</returns>
    [Pure]
    public static Vector4Int ComponentMin(Vector4Int a, Vector4Int b)
    {
        Vector4Int result;
        result.x = Math.Min(a.x, b.x);
        result.y = Math.Min(a.y, b.y);
        result.z = Math.Min(a.z, b.z);
        result.w = Math.Min(a.w, b.w);
        return result;
    }

    /// <summary>
    /// Returns a vector created from the smallest of the corresponding components of the given vectors.
    /// </summary>
    /// <param name="a">First operand.</param>
    /// <param name="b">Second operand.</param>
    /// <param name="result">The component-wise minimum.</param>
    public static void ComponentMin(in Vector4Int a, in Vector4Int b, out Vector4Int result)
    {
        result.x = Math.Min(a.x, b.x);
        result.y = Math.Min(a.y, b.y);
        result.z = Math.Min(a.z, b.z);
        result.w = Math.Min(a.w, b.w);
    }

    /// <summary>
    /// Returns a vector created from the largest of the corresponding components of the given vectors.
    /// </summary>
    /// <param name="a">First operand.</param>
    /// <param name="b">Second operand.</param>
    /// <returns>The component-wise maximum.</returns>
    [Pure]
    public static Vector4Int ComponentMax(Vector4Int a, Vector4Int b)
    {
        Vector4Int result;
        result.x = Math.Max(a.x, b.x);
        result.y = Math.Max(a.y, b.y);
        result.z = Math.Max(a.z, b.z);
        result.w = Math.Max(a.w, b.w);
        return result;
    }

    /// <summary>
    /// Returns a vector created from the largest of the corresponding components of the given vectors.
    /// </summary>
    /// <param name="a">First operand.</param>
    /// <param name="b">Second operand.</param>
    /// <param name="result">The component-wise maximum.</param>
    public static void ComponentMax(in Vector4Int a, in Vector4Int b, out Vector4Int result)
    {
        result.x = Math.Max(a.x, b.x);
        result.y = Math.Max(a.y, b.y);
        result.z = Math.Max(a.z, b.z);
        result.w = Math.Max(a.w, b.w);
    }

    /// <summary>
    /// Clamp a vector to the given minimum and maximum vectors.
    /// </summary>
    /// <param name="vec">Input vector.</param>
    /// <param name="min">Minimum vector.</param>
    /// <param name="max">Maximum vector.</param>
    /// <returns>The clamped vector.</returns>
    [Pure]
    public static Vector4Int Clamp(Vector4Int vec, Vector4Int min, Vector4Int max)
    {
        Vector4Int result;
        result.x = Mathf.Clamp(vec.x, min.x, max.x);
        result.y = Mathf.Clamp(vec.y, min.y, max.y);
        result.z = Mathf.Clamp(vec.z, min.z, max.z);
        result.w = Mathf.Clamp(vec.w, min.w, max.w);
        return result;
    }

    /// <summary>
    /// Clamp a vector to the given minimum and maximum vectors.
    /// </summary>
    /// <param name="vec">Input vector.</param>
    /// <param name="min">Minimum vector.</param>
    /// <param name="max">Maximum vector.</param>
    /// <param name="result">The clamped vector.</param>
    public static void Clamp(in Vector4Int vec, in Vector4Int min, in Vector4Int max, out Vector4Int result)
    {
        result.x = Mathf.Clamp(vec.x, min.x, max.x);
        result.y = Mathf.Clamp(vec.y, min.y, max.y);
        result.z = Mathf.Clamp(vec.z, min.z, max.z);
        result.w = Mathf.Clamp(vec.w, min.w, max.w);
    }

    /// <summary>
    /// Take the component-wise absolute value of a vector.
    /// </summary>
    /// <param name="vec">The vector to apply component-wise absolute value to.</param>
    /// <returns>The component-wise absolute value vector.</returns>
    public static Vector4Int Abs(Vector4Int vec)
    {
        vec.x = Math.Abs(vec.x);
        vec.y = Math.Abs(vec.y);
        vec.z = Math.Abs(vec.z);
        vec.w = Math.Abs(vec.w);
        return vec;
    }

    /// <summary>
    /// Take the component-wise absolute value of a vector.
    /// </summary>
    /// <param name="vec">The vector to apply component-wise absolute value to.</param>
    /// <param name="result">The component-wise absolute value vector.</param>
    public static void Abs(in Vector4Int vec, out Vector4Int result)
    {
        result.x = Math.Abs(vec.x);
        result.y = Math.Abs(vec.y);
        result.z = Math.Abs(vec.z);
        result.w = Math.Abs(vec.w);
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector2Int"/> with the X and Y components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector2Int Xy
    {
        get => Unsafe.As<Vector4Int, Vector2Int>(ref this);
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
    /// Gets or sets a <see cref="Vector2Int"/> with the X and W components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector2Int Xw
    {
        readonly get => new Vector2Int(x, w);
        set
        {
            x = value.x;
            w = value.y;
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
    /// Gets or sets a <see cref="Vector2Int"/> with the Y and W components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector2Int Yw
    {
        readonly get => new Vector2Int(y, w);
        set
        {
            y = value.x;
            w = value.y;
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
    /// Gets or sets a <see cref="Vector2Int"/> with the Z and W components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector2Int Zw
    {
        readonly get => new Vector2Int(z, w);
        set
        {
            z = value.x;
            w = value.y;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector2Int"/> with the W and X components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector2Int Wx
    {
        readonly get => new Vector2Int(w, x);
        set
        {
            w = value.x;
            x = value.y;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector2Int"/> with the W and Y components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector2Int Wy
    {
        readonly get => new Vector2Int(w, y);
        set
        {
            w = value.x;
            y = value.y;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector2Int"/> with the W and Z components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector2Int Wz
    {
        readonly get => new Vector2Int(w, z);
        set
        {
            w = value.x;
            z = value.y;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector3Int"/> with the X, Y, and Z components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector3Int Xyz
    {
        get => Unsafe.As<Vector4Int, Vector3Int>(ref this);
        set
        {
            x = value.x;
            y = value.y;
            z = value.z;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector3Int"/> with the X, Y, and Z components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector3Int Xyw
    {
        readonly get => new Vector3Int(x, y, w);
        set
        {
            x = value.x;
            y = value.y;
            w = value.z;
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
    /// Gets or sets a <see cref="Vector3Int"/> with the X, Z, and W components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector3Int Xzw
    {
        readonly get => new Vector3Int(x, z, w);
        set
        {
            x = value.x;
            z = value.y;
            w = value.z;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector3Int"/> with the X, W, and Y components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector3Int Xwy
    {
        readonly get => new Vector3Int(x, w, y);
        set
        {
            x = value.x;
            w = value.y;
            y = value.z;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector3Int"/> with the X, W, and Z components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector3Int Xwz
    {
        readonly get => new Vector3Int(x, w, z);
        set
        {
            x = value.x;
            w = value.y;
            z = value.z;
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
    /// Gets or sets a <see cref="Vector3Int"/> with the Y, X, and W components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector3Int Yxw
    {
        readonly get => new Vector3Int(y, x, w);
        set
        {
            y = value.x;
            x = value.y;
            w = value.z;
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
    /// Gets or sets a <see cref="Vector3Int"/> with the Y, Z, and W components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector3Int Yzw
    {
        readonly get => new Vector3Int(y, z, w);
        set
        {
            y = value.x;
            z = value.y;
            w = value.z;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector3Int"/> with the Y, W, and X components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector3Int Ywx
    {
        readonly get => new Vector3Int(y, w, x);
        set
        {
            y = value.x;
            w = value.y;
            x = value.z;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector3Int"/> with the Y, W, and Z components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector3Int Ywz
    {
        readonly get => new Vector3Int(y, w, z);
        set
        {
            y = value.x;
            w = value.y;
            z = value.z;
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
    /// Gets or sets a <see cref="Vector3Int"/> with the Z, X, and W components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector3Int Zxw
    {
        readonly get => new Vector3Int(z, x, w);
        set
        {
            z = value.x;
            x = value.y;
            w = value.z;
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
    /// Gets or sets a <see cref="Vector3Int"/> with the Z, Y, and W components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector3Int Zyw
    {
        readonly get => new Vector3Int(z, y, w);
        set
        {
            z = value.x;
            y = value.y;
            w = value.z;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector3Int"/> with the Z, W, and X components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector3Int Zwx
    {
        readonly get => new Vector3Int(z, w, x);
        set
        {
            z = value.x;
            w = value.y;
            x = value.z;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector3Int"/> with the Z, W, and Y components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector3Int Zwy
    {
        readonly get => new Vector3Int(z, w, y);
        set
        {
            z = value.x;
            w = value.y;
            y = value.z;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector3Int"/> with the W, X, and Y components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector3Int Wxy
    {
        readonly get => new Vector3Int(w, x, y);
        set
        {
            w = value.x;
            x = value.y;
            y = value.z;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector3Int"/> with the W, X, and Z components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector3Int Wxz
    {
        readonly get => new Vector3Int(w, x, z);
        set
        {
            w = value.x;
            x = value.y;
            z = value.z;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector3Int"/> with the W, Y, and X components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector3Int Wyx
    {
        readonly get => new Vector3Int(w, y, x);
        set
        {
            w = value.x;
            y = value.y;
            x = value.z;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector3Int"/> with the W, Y, and Z components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector3Int Wyz
    {
        readonly get => new Vector3Int(w, y, z);
        set
        {
            w = value.x;
            y = value.y;
            z = value.z;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector3Int"/> with the W, Z, and X components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector3Int Wzx
    {
        readonly get => new Vector3Int(w, z, x);
        set
        {
            w = value.x;
            z = value.y;
            x = value.z;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector3Int"/> with the W, Z, and Y components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector3Int Wzy
    {
        readonly get => new Vector3Int(w, z, y);
        set
        {
            w = value.x;
            z = value.y;
            y = value.z;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector4Int"/> with the X, Y, W, and Z components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4Int Xywz
    {
        readonly get => new Vector4Int(x, y, w, z);
        set
        {
            x = value.x;
            y = value.y;
            w = value.z;
            z = value.w;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector4Int"/> with the X, Z, Y, and W components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4Int Xzyw
    {
        readonly get => new Vector4Int(x, z, y, w);
        set
        {
            x = value.x;
            z = value.y;
            y = value.z;
            w = value.w;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector4Int"/> with the X, Z, W, and Y components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4Int Xzwy
    {
        readonly get => new Vector4Int(x, z, w, y);
        set
        {
            x = value.x;
            z = value.y;
            w = value.z;
            y = value.w;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector4Int"/> with the X, W, Y, and Z components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4Int Xwyz
    {
        readonly get => new Vector4Int(x, w, y, z);
        set
        {
            x = value.x;
            w = value.y;
            y = value.z;
            z = value.w;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector4Int"/> with the X, W, Z, and Y components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4Int Xwzy
    {
        readonly get => new Vector4Int(x, w, z, y);
        set
        {
            x = value.x;
            w = value.y;
            z = value.z;
            y = value.w;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector4Int"/> with the Y, X, Z, and W components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4Int Yxzw
    {
        readonly get => new Vector4Int(y, x, z, w);
        set
        {
            y = value.x;
            x = value.y;
            z = value.z;
            w = value.w;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector4Int"/> with the Y, X, W, and Z components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4Int Yxwz
    {
        readonly get => new Vector4Int(y, x, w, z);
        set
        {
            y = value.x;
            x = value.y;
            w = value.z;
            z = value.w;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector4Int"/> with the Y, Y, Z, and W components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4Int Yyzw
    {
        readonly get => new Vector4Int(y, y, z, w);
        set
        {
            x = value.x;
            y = value.y;
            z = value.z;
            w = value.w;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector4Int"/> with the Y, Y, W, and Z components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4Int Yywz
    {
        readonly get => new Vector4Int(y, y, w, z);
        set
        {
            x = value.x;
            y = value.y;
            w = value.z;
            z = value.w;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector4Int"/> with the Y, Z, X, and W components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4Int Yzxw
    {
        readonly get => new Vector4Int(y, z, x, w);
        set
        {
            y = value.x;
            z = value.y;
            x = value.z;
            w = value.w;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector4Int"/> with the Y, Z, W, and X components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4Int Yzwx
    {
        readonly get => new Vector4Int(y, z, w, x);
        set
        {
            y = value.x;
            z = value.y;
            w = value.z;
            x = value.w;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector4Int"/> with the Y, W, X, and Z components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4Int Ywxz
    {
        readonly get => new Vector4Int(y, w, x, z);
        set
        {
            y = value.x;
            w = value.y;
            x = value.z;
            z = value.w;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector4Int"/> with the Y, W, Z, and X components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4Int Ywzx
    {
        readonly get => new Vector4Int(y, w, z, x);
        set
        {
            y = value.x;
            w = value.y;
            z = value.z;
            x = value.w;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector4Int"/> with the Z, X, Y, and Z components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4Int Zxyw
    {
        readonly get => new Vector4Int(z, x, y, w);
        set
        {
            z = value.x;
            x = value.y;
            y = value.z;
            w = value.w;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector4Int"/> with the Z, X, W, and Y components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4Int Zxwy
    {
        readonly get => new Vector4Int(z, x, w, y);
        set
        {
            z = value.x;
            x = value.y;
            w = value.z;
            y = value.w;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector4Int"/> with the Z, Y, X, and W components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4Int Zyxw
    {
        readonly get => new Vector4Int(z, y, x, w);
        set
        {
            z = value.x;
            y = value.y;
            x = value.z;
            w = value.w;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector4Int"/> with the Z, Y, W, and X components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4Int Zywx
    {
        readonly get => new Vector4Int(z, y, w, x);
        set
        {
            z = value.x;
            y = value.y;
            w = value.z;
            x = value.w;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector4Int"/> with the Z, W, X, and Y components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4Int Zwxy
    {
        readonly get => new Vector4Int(z, w, x, y);
        set
        {
            z = value.x;
            w = value.y;
            x = value.z;
            y = value.w;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector4Int"/> with the Z, W, Y, and X components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4Int Zwyx
    {
        readonly get => new Vector4Int(z, w, y, x);
        set
        {
            z = value.x;
            w = value.y;
            y = value.z;
            x = value.w;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector4Int"/> with the Z, W, Z, and Y components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4Int Zwzy
    {
        readonly get => new Vector4Int(z, w, z, y);
        set
        {
            x = value.x;
            w = value.y;
            z = value.z;
            y = value.w;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector4Int"/> with the W, X, Y, and Z components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4Int Wxyz
    {
        readonly get => new Vector4Int(w, x, y, z);
        set
        {
            w = value.x;
            x = value.y;
            y = value.z;
            z = value.w;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector4Int"/> with the W, X, Z, and Y components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4Int Wxzy
    {
        readonly get => new Vector4Int(w, x, z, y);
        set
        {
            w = value.x;
            x = value.y;
            z = value.z;
            y = value.w;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector4Int"/> with the W, Y, X, and Z components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4Int Wyxz
    {
        readonly get => new Vector4Int(w, y, x, z);
        set
        {
            w = value.x;
            y = value.y;
            x = value.z;
            z = value.w;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector4Int"/> with the W, Y, Z, and X components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4Int Wyzx
    {
        readonly get => new Vector4Int(w, y, z, x);
        set
        {
            w = value.x;
            y = value.y;
            z = value.z;
            x = value.w;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector4Int"/> with the W, Z, X, and Y components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4Int Wzxy
    {
        readonly get => new Vector4Int(w, z, x, y);
        set
        {
            w = value.x;
            z = value.y;
            x = value.z;
            y = value.w;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector4Int"/> with the W, Z, Y, and X components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4Int Wzyx
    {
        readonly get => new Vector4Int(w, z, y, x);
        set
        {
            w = value.x;
            z = value.y;
            y = value.z;
            x = value.w;
        }
    }

    /// <summary>
    /// Gets or sets a <see cref="Vector4Int"/> with the W, Z, Y, and W components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4Int Wzyw
    {
        readonly get => new Vector4Int(w, z, y, w);
        set
        {
            x = value.x;
            z = value.y;
            y = value.z;
            w = value.w;
        }
    }

    /// <summary>
    /// Gets a <see cref="Vector4"/> object with the same component values as the <see cref="Vector4Int"/> instance.
    /// </summary>
    /// <returns>The resulting <see cref="Vector4"/> instance.</returns>
    public readonly Vector4 ToVector4()
    {
        return new Vector4(x, y, z, w);
    }

    /// <summary>
    /// Gets a <see cref="Vector4"/> object with the same component values as the <see cref="Vector4Int"/> instance.
    /// </summary>
    /// <param name="input">The given <see cref="Vector4Int"/> to convert.</param>
    /// <param name="result">The resulting <see cref="Vector4"/>.</param>
    public static void ToVector4(in Vector4Int input, out Vector4 result)
    {
        result.x = input.x;
        result.y = input.y;
        result.z = input.z;
        result.w = input.w;
    }

    /// <summary>
    /// Adds two instances.
    /// </summary>
    /// <param name="left">The first instance.</param>
    /// <param name="right">The second instance.</param>
    /// <returns>The result of the calculation.</returns>
    [Pure]
    public static Vector4Int operator +(Vector4Int left, Vector4Int right)
    {
        left.x += right.x;
        left.y += right.y;
        left.z += right.z;
        left.w += right.w;
        return left;
    }

    /// <summary>
    /// Subtracts two instances.
    /// </summary>
    /// <param name="left">The first instance.</param>
    /// <param name="right">The second instance.</param>
    /// <returns>The result of the calculation.</returns>
    [Pure]
    public static Vector4Int operator -(Vector4Int left, Vector4Int right)
    {
        left.x -= right.x;
        left.y -= right.y;
        left.z -= right.z;
        left.w -= right.w;
        return left;
    }

    /// <summary>
    /// Negates an instance.
    /// </summary>
    /// <param name="vec">The instance.</param>
    /// <returns>The result of the calculation.</returns>
    [Pure]
    public static Vector4Int operator -(Vector4Int vec)
    {
        vec.x = -vec.x;
        vec.y = -vec.y;
        vec.z = -vec.z;
        vec.w = -vec.w;
        return vec;
    }

    /// <summary>
    /// Multiplies an instance by an integer scalar.
    /// </summary>
    /// <param name="vec">The instance.</param>
    /// <param name="scale">The scalar.</param>
    /// <returns>The result of the calculation.</returns>
    [Pure]
    public static Vector4Int operator *(Vector4Int vec, int scale)
    {
        vec.x *= scale;
        vec.y *= scale;
        vec.z *= scale;
        vec.w *= scale;
        return vec;
    }

    /// <summary>
    /// Multiplies an instance by an integer scalar.
    /// </summary>
    /// <param name="scale">The scalar.</param>
    /// <param name="vec">The instance.</param>
    /// <returns>The result of the calculation.</returns>
    [Pure]
    public static Vector4Int operator *(int scale, Vector4Int vec)
    {
        vec.x *= scale;
        vec.y *= scale;
        vec.z *= scale;
        vec.w *= scale;
        return vec;
    }

    /// <summary>
    /// Component-wise multiplication between the specified instance by a scale vector.
    /// </summary>
    /// <param name="scale">Left operand.</param>
    /// <param name="vec">Right operand.</param>
    /// <returns>Result of multiplication.</returns>
    [Pure]
    public static Vector4Int operator *(Vector4Int vec, Vector4Int scale)
    {
        vec.x *= scale.x;
        vec.y *= scale.y;
        vec.z *= scale.z;
        vec.w *= scale.w;
        return vec;
    }

    /// <summary>
    /// Divides the instance by a scalar using integer division, floor(a/b).
    /// </summary>
    /// <param name="vec">The instance.</param>
    /// <param name="scale">The scalar.</param>
    /// <returns>The result of the calculation.</returns>
    [Pure]
    public static Vector4Int operator /(Vector4Int vec, int scale)
    {
        vec.x /= scale;
        vec.y /= scale;
        vec.z /= scale;
        vec.w /= scale;
        return vec;
    }

    /// <summary>
    /// Component-wise division between the specified instance by a scale vector.
    /// </summary>
    /// <param name="vec">Left operand.</param>
    /// <param name="scale">Right operand.</param>
    /// <returns>Result of the division.</returns>
    [Pure]
    public static Vector4Int operator /(Vector4Int vec, Vector4Int scale)
    {
        vec.x /= scale.x;
        vec.y /= scale.y;
        vec.z /= scale.z;
        vec.w /= scale.w;
        return vec;
    }

    /// <summary>
    /// Compares two instances for equality.
    /// </summary>
    /// <param name="left">The first instance.</param>
    /// <param name="right">The second instance.</param>
    /// <returns>True, if left equals right; false otherwise.</returns>
    public static bool operator ==(Vector4Int left, Vector4Int right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Compares two instances for inequality.
    /// </summary>
    /// <param name="left">The first instance.</param>
    /// <param name="right">The second instance.</param>
    /// <returns>True, if left does not equa lright; false otherwise.</returns>
    public static bool operator !=(Vector4Int left, Vector4Int right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Returns a pointer to the first element of the specified instance.
    /// </summary>
    /// <param name="v">The instance.</param>
    /// <returns>A pointer to the first element of v.</returns>
    [Pure]
    public static unsafe explicit operator int*(Vector4Int v)
    {
        return &v.x;
    }

    /// <summary>
    /// Returns a pointer to the first element of the specified instance.
    /// </summary>
    /// <param name="v">The instance.</param>
    /// <returns>A pointer to the first element of v.</returns>
    [Pure]
    public static explicit operator IntPtr(Vector4Int v)
    {
        unsafe
        {
            return (IntPtr)(&v.x);
        }
    }

    /// <summary>
    /// Converts OpenTK.Vector4i to OpenTK.Vector4.
    /// </summary>
    /// <param name="vec">The Vector4i to convert.</param>
    /// <returns>The resulting Vector4.</returns>
    [Pure]
    public static implicit operator Vector4(Vector4Int vec)
    {
        return new Vector4(vec.x, vec.y, vec.z, vec.w);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector4Int"/> struct using a tuple containing the component
    /// values.
    /// </summary>
    /// <param name="values">A tuple containing the component values.</param>
    /// <returns>A new instance of the <see cref="Vector4Int"/> struct with the given component values.</returns>
    [Pure]
    public static implicit operator Vector4Int((int X, int Y, int Z, int W) values)
    {
        return new Vector4Int(values.X, values.Y, values.Z, values.W);
    }

    /// <inheritdoc/>
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

    /// <inheritdoc />
    public readonly string ToString(string format, IFormatProvider formatProvider)
    {
        return string.Format(
            "({0}{4} {1}{4} {2}{4} {3})",
            x.ToString(format, formatProvider),
            y.ToString(format, formatProvider),
            z.ToString(format, formatProvider),
            w.ToString(format, formatProvider),
            Mathf.GetListSeparator(formatProvider));
    }

    /// <inheritdoc />
    public override readonly bool Equals(object obj)
    {
        return obj is Vector4Int && Equals((Vector4Int)obj);
    }

    /// <inheritdoc />
    public readonly bool Equals(Vector4Int other)
    {
        return x == other.x &&
               y == other.y &&
               z == other.z &&
               w == other.w;
    }

    /// <inheritdoc />
    public override readonly int GetHashCode()
    {
        return HashCode.Combine(x, y, z, w);
    }

    /// <summary>
    /// Deconstructs the vector into it's individual components.
    /// </summary>
    /// <param name="x">The X component of the vector.</param>
    /// <param name="y">The Y component of the vector.</param>
    /// <param name="z">The Z component of the vector.</param>
    /// <param name="w">The W component of the vector.</param>
    [Pure]
    public readonly void Deconstruct(out int x, out int y, out int z, out int w)
    {
        x = this.x;
        y = this.y;
        z = this.z;
        w = this.w;
    }
}
