using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace Kralt.Mathematics;

/// <summary>
/// Represents a 4D vector using four single-precision floating-point numbers.
/// </summary>
/// <remarks>
/// The Vector4 structure is suitable for interoperation with unmanaged code requiring four consecutive floats.
/// </remarks>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct Vector4 : IEquatable<Vector4>, IFormattable
{
    /// <summary>
    /// The X component of the Vector4.
    /// </summary>
    public float x;

    /// <summary>
    /// The Y component of the Vector4.
    /// </summary>
    public float y;

    /// <summary>
    /// The Z component of the Vector4.
    /// </summary>
    public float z;

    /// <summary>
    /// The W component of the Vector4.
    /// </summary>
    public float w;

    /// <summary>
    /// Defines a unit-length Vector4 that points towards the X-axis.
    /// </summary>
    public static readonly Vector4 UnitX = new Vector4(1, 0, 0, 0);

    /// <summary>
    /// Defines a unit-length Vector4 that points towards the Y-axis.
    /// </summary>
    public static readonly Vector4 UnitY = new Vector4(0, 1, 0, 0);

    /// <summary>
    /// Defines a unit-length Vector4 that points towards the Z-axis.
    /// </summary>
    public static readonly Vector4 UnitZ = new Vector4(0, 0, 1, 0);

    /// <summary>
    /// Defines a unit-length Vector4 that points towards the W-axis.
    /// </summary>
    public static readonly Vector4 UnitW = new Vector4(0, 0, 0, 1);

    /// <summary>
    /// Defines an instance with all components set to 0.
    /// </summary>
    public static readonly Vector4 Zero = new Vector4(0, 0, 0, 0);

    /// <summary>
    /// Defines an instance with all components set to 1.
    /// </summary>
    public static readonly Vector4 One = new Vector4(1, 1, 1, 1);

    /// <summary>
    /// Defines an instance with all components set to positive infinity.
    /// </summary>
    public static readonly Vector4 PositiveInfinity = new Vector4(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);

    /// <summary>
    /// Defines an instance with all components set to negative infinity.
    /// </summary>
    public static readonly Vector4 NegativeInfinity = new Vector4(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);

    /// <summary>
    /// Defines the size of the Vector4 struct in bytes.
    /// </summary>
    public static readonly int SizeInBytes = Unsafe.SizeOf<Vector4>();

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector4"/> struct.
    /// </summary>
    /// <param name="value">The value that will initialize this instance.</param>
    public Vector4(float value)
    {
        x = value;
        y = value;
        z = value;
        w = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector4"/> struct.
    /// </summary>
    /// <param name="x">The x component of the Vector4.</param>
    /// <param name="y">The y component of the Vector4.</param>
    /// <param name="z">The z component of the Vector4.</param>
    /// <param name="w">The w component of the Vector4.</param>
    public Vector4(float x, float y, float z, float w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector4"/> struct.
    /// </summary>
    /// <param name="v">The Vector2 to copy components from.</param>
    public Vector4(Vector2 v)
    {
        x = v.x;
        y = v.y;
        z = 0.0f;
        w = 0.0f;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector4"/> struct.
    /// </summary>
    /// <param name="v">The Vector3 to copy components from.</param>
    /// <remarks>
    ///  .<seealso cref="Vector4(Vector3, float)"/>
    /// </remarks>
    public Vector4(Vector3 v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
        w = 0.0f;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector4"/> struct.
    /// </summary>
    /// <param name="v">The Vector3 to copy components from.</param>
    /// <param name="w">The w component of the new Vector4.</param>
    public Vector4(Vector3 v, float w)
    {
        x = v.x;
        y = v.y;
        z = v.z;
        this.w = w;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector4"/> struct.
    /// </summary>
    /// <param name="v">The Vector4 to copy components from.</param>
    public Vector4(Vector4 v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
        w = v.w;
    }

    /// <summary>
    /// Gets or sets the value at the index of the Vector.
    /// </summary>
    /// <param name="index">The index of the component from the Vector.</param>
    /// <exception cref="IndexOutOfRangeException">Thrown if the index is less than 0 or greater than 3.</exception>
    public float this[int index]
    {
        readonly get
        {
            if (((uint)index) < 4)
            {
                return GetElementUnsafe(in this, index);
            }
            else
            {
                Mathf.ThrowOutOfRangeException("You tried to access this vector at index: " + index);
                return default;
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
                Mathf.ThrowOutOfRangeException("You tried to set this vector at index: " + index);
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private readonly ref float GetElementUnsafe(in Vector4 v, int index)
    {
        ref float address = ref Unsafe.AsRef(in v.x);
        return ref Unsafe.Add(ref address, index);
    }

    /// <summary>
    /// Gets the length (magnitude) of the vector.
    /// </summary>
    /// <see cref="LengthFast"/>
    /// <seealso cref="LengthSquared"/>
    public readonly float Length => MathF.Sqrt((x * x) + (y * y) + (z * z) + (w * w));

    /// <summary>
    /// Gets an approximation of 1 over the length (magnitude) of the vector.
    /// </summary>
    public readonly float ReciprocalLengthFast => Mathf.InverseSqrtFast((x * x) + (y * y) + (z * z) + (w * w));

    /// <summary>
    /// Gets an approximation of the vector length (magnitude).
    /// </summary>
    /// <remarks>
    /// This property uses an approximation of the square root function to calculate vector magnitude, with
    /// an upper error bound of 0.001.
    /// </remarks>
    /// <see cref="Length"/>
    /// <seealso cref="LengthSquared"/>
    public readonly float LengthFast => 1.0f / Mathf.InverseSqrtFast((x * x) + (y * y) + (z * z) + (w * w));

    /// <summary>
    /// Gets the square of the vector length (magnitude).
    /// </summary>
    /// <remarks>
    /// This property avoids the costly square root operation required by the Length property. This makes it more suitable
    /// for comparisons.
    /// </remarks>
    /// <see cref="Length"/>
    /// <seealso cref="LengthFast"/>
    public readonly float LengthSquared => (x * x) + (y * y) + (z * z) + (w * w);

    /// <summary>
    /// Returns a copy of the Vector4 scaled to unit length.
    /// </summary>
    /// <returns>The normalized copy.</returns>
    public readonly Vector4 Normalized()
    {
        Vector4 v = this;
        v.Normalize();
        return v;
    }

    /// <summary>
    /// Scales the Vector4 to unit length.
    /// </summary>
    public void Normalize()
    {
        float scale = 1.0f / Length;
        x *= scale;
        y *= scale;
        z *= scale;
        w *= scale;
    }

    /// <summary>
    /// Scales the Vector4 to approximately unit length.
    /// </summary>
    public void NormalizeFast()
    {
        float scale = Mathf.InverseSqrtFast((x * x) + (y * y) + (z * z) + (w * w));
        x *= scale;
        y *= scale;
        z *= scale;
        w *= scale;
    }

    /// <summary>
    /// Returns a new vector that is the component-wise absolute value of the vector.
    /// </summary>
    /// <returns>The component-wise absolute value vector.</returns>
    public readonly Vector4 Abs()
    {
        Vector4 result = this;
        result.x = MathF.Abs(result.x);
        result.y = MathF.Abs(result.y);
        result.z = MathF.Abs(result.z);
        result.w = MathF.Abs(result.w);
        return result;
    }

    /// <summary>
    /// Returns a new vector were component-wise rounding has been applied.
    /// Equivalent to calling <see cref="MathF.Round(float)"/> on each component.
    /// </summary>
    /// <returns>The rounded vector.</returns>
    public readonly Vector4 Round()
    {
        return Round(this);
    }

    /// <summary>
    /// Returns a new vector were component-wise rounding has been applied with the specified midpoint rounding rule.
    /// Equivalent to calling <see cref="MathF.Round(float,MidpointRounding)"/> on each component.
    /// </summary>
    /// <param name="rounding">The midpoint rounding rule to use.</param>
    /// <returns>The rounded vector.</returns>
    public readonly Vector4 Round(MidpointRounding rounding)
    {
        return Round(this, rounding);
    }

    /// <summary>
    /// Returns a new vector were a component-wise ceiling operation has been applied.
    /// Equivalent to calling <see cref="MathF.Ceiling(float)"/> on each component.
    /// </summary>
    /// <returns>The ceiled vector.</returns>
    public readonly Vector4 Ceiling()
    {
        return Ceiling(this);
    }

    /// <summary>
    /// Returns a new vector were a component-wise floor operation has been applied.
    /// Equivalent to calling <see cref="MathF.Floor(float)"/> on each component.
    /// </summary>
    /// <returns>The floored vector.</returns>
    public readonly Vector4 Floor()
    {
        return Floor(this);
    }

    /// <summary>
    /// Returns a new vector were component-wise truncation has been applied.
    /// Equivalent to calling <see cref="MathF.Truncate(float)"/> on each component.
    /// </summary>
    /// <returns>The truncated vector.</returns>
    public readonly Vector4 Truncate()
    {
        return Truncate(this);
    }

    /// <summary>
    /// Adds two vectors.
    /// </summary>
    /// <param name="a">Left operand.</param>
    /// <param name="b">Right operand.</param>
    /// <returns>Result of operation.</returns>
    [Pure]
    public static Vector4 Add(Vector4 a, Vector4 b)
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
    public static void Add(in Vector4 a, in Vector4 b, out Vector4 result)
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
    public static Vector4 Subtract(Vector4 a, Vector4 b)
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
    public static void Subtract(in Vector4 a, in Vector4 b, out Vector4 result)
    {
        result.x = a.x - b.x;
        result.y = a.y - b.y;
        result.z = a.z - b.z;
        result.w = a.w - b.w;
    }

    /// <summary>
    /// Multiplies a vector by a scalar.
    /// </summary>
    /// <param name="vector">Left operand.</param>
    /// <param name="scale">Right operand.</param>
    /// <returns>Result of the operation.</returns>
    [Pure]
    public static Vector4 Multiply(Vector4 vector, float scale)
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
    public static void Multiply(in Vector4 vector, float scale, out Vector4 result)
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
    public static Vector4 Multiply(Vector4 vector, Vector4 scale)
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
    public static void Multiply(in Vector4 vector, in Vector4 scale, out Vector4 result)
    {
        result.x = vector.x * scale.x;
        result.y = vector.y * scale.y;
        result.z = vector.z * scale.z;
        result.w = vector.w * scale.w;
    }

    /// <summary>
    /// Divides a vector by a scalar.
    /// </summary>
    /// <param name="vector">Left operand.</param>
    /// <param name="scale">Right operand.</param>
    /// <returns>Result of the operation.</returns>
    [Pure]
    public static Vector4 Divide(Vector4 vector, float scale)
    {
        Divide(in vector, scale, out vector);
        return vector;
    }

    /// <summary>
    /// Divides a vector by a scalar.
    /// </summary>
    /// <param name="vector">Left operand.</param>
    /// <param name="scale">Right operand.</param>
    /// <param name="result">Result of the operation.</param>
    public static void Divide(in Vector4 vector, float scale, out Vector4 result)
    {
        result.x = vector.x / scale;
        result.y = vector.y / scale;
        result.z = vector.z / scale;
        result.w = vector.w / scale;
    }

    /// <summary>
    /// Divides a vector by the components of a vector (scale).
    /// </summary>
    /// <param name="vector">Left operand.</param>
    /// <param name="scale">Right operand.</param>
    /// <returns>Result of the operation.</returns>
    [Pure]
    public static Vector4 Divide(Vector4 vector, Vector4 scale)
    {
        Divide(in vector, in scale, out vector);
        return vector;
    }

    /// <summary>
    /// Divide a vector by the components of a vector (scale).
    /// </summary>
    /// <param name="vector">Left operand.</param>
    /// <param name="scale">Right operand.</param>
    /// <param name="result">Result of the operation.</param>
    public static void Divide(in Vector4 vector, in Vector4 scale, out Vector4 result)
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
    public static Vector4 ComponentMin(Vector4 a, Vector4 b)
    {
        a.x = a.x < b.x ? a.x : b.x;
        a.y = a.y < b.y ? a.y : b.y;
        a.z = a.z < b.z ? a.z : b.z;
        a.w = a.w < b.w ? a.w : b.w;
        return a;
    }

    /// <summary>
    /// Returns a vector created from the smallest of the corresponding components of the given vectors.
    /// </summary>
    /// <param name="a">First operand.</param>
    /// <param name="b">Second operand.</param>
    /// <param name="result">The component-wise minimum.</param>
    public static void ComponentMin(in Vector4 a, in Vector4 b, out Vector4 result)
    {
        result.x = a.x < b.x ? a.x : b.x;
        result.y = a.y < b.y ? a.y : b.y;
        result.z = a.z < b.z ? a.z : b.z;
        result.w = a.w < b.w ? a.w : b.w;
    }

    /// <summary>
    /// Returns a vector created from the largest of the corresponding components of the given vectors.
    /// </summary>
    /// <param name="a">First operand.</param>
    /// <param name="b">Second operand.</param>
    /// <returns>The component-wise maximum.</returns>
    [Pure]
    public static Vector4 ComponentMax(Vector4 a, Vector4 b)
    {
        a.x = a.x > b.x ? a.x : b.x;
        a.y = a.y > b.y ? a.y : b.y;
        a.z = a.z > b.z ? a.z : b.z;
        a.w = a.w > b.w ? a.w : b.w;
        return a;
    }

    /// <summary>
    /// Returns a vector created from the largest of the corresponding components of the given vectors.
    /// </summary>
    /// <param name="a">First operand.</param>
    /// <param name="b">Second operand.</param>
    /// <param name="result">The component-wise maximum.</param>
    public static void ComponentMax(in Vector4 a, in Vector4 b, out Vector4 result)
    {
        result.x = a.x > b.x ? a.x : b.x;
        result.y = a.y > b.y ? a.y : b.y;
        result.z = a.z > b.z ? a.z : b.z;
        result.w = a.w > b.w ? a.w : b.w;
    }

    /// <summary>
    /// Returns the Vector4 with the minimum magnitude. If the magnitudes are equal, the second vector
    /// is selected.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>The minimum Vector4.</returns>
    [Pure]
    public static Vector4 MagnitudeMin(Vector4 left, Vector4 right)
    {
        return left.LengthSquared < right.LengthSquared ? left : right;
    }

    /// <summary>
    /// Returns the Vector4 with the minimum magnitude. If the magnitudes are equal, the second vector
    /// is selected.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <param name="result">The magnitude-wise minimum.</param>
    public static void MagnitudeMin(in Vector4 left, in Vector4 right, out Vector4 result)
    {
        result = left.LengthSquared < right.LengthSquared ? left : right;
    }

    /// <summary>
    /// Returns the Vector4 with the maximum magnitude. If the magnitudes are equal, the first vector
    /// is selected.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>The maximum Vector4.</returns>
    [Pure]
    public static Vector4 MagnitudeMax(Vector4 left, Vector4 right)
    {
        return left.LengthSquared >= right.LengthSquared ? left : right;
    }

    /// <summary>
    /// Returns the Vector4 with the maximum magnitude. If the magnitudes are equal, the first vector
    /// is selected.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <param name="result">The magnitude-wise maximum.</param>
    public static void MagnitudeMax(in Vector4 left, in Vector4 right, out Vector4 result)
    {
        result = left.LengthSquared >= right.LengthSquared ? left : right;
    }

    /// <summary>
    /// Clamp a vector to the given minimum and maximum vectors.
    /// </summary>
    /// <param name="vec">Input vector.</param>
    /// <param name="min">Minimum vector.</param>
    /// <param name="max">Maximum vector.</param>
    /// <returns>The clamped vector.</returns>
    [Pure]
    public static Vector4 Clamp(Vector4 vec, Vector4 min, Vector4 max)
    {
        vec.x = vec.x < min.x ? min.x : vec.x > max.x ? max.x : vec.x;
        vec.y = vec.y < min.y ? min.y : vec.y > max.y ? max.y : vec.y;
        vec.z = vec.z < min.z ? min.z : vec.z > max.z ? max.z : vec.z;
        vec.w = vec.w < min.w ? min.w : vec.w > max.w ? max.w : vec.w;
        return vec;
    }

    /// <summary>
    /// Clamp a vector to the given minimum and maximum vectors.
    /// </summary>
    /// <param name="vec">Input vector.</param>
    /// <param name="min">Minimum vector.</param>
    /// <param name="max">Maximum vector.</param>
    /// <param name="result">The clamped vector.</param>
    public static void Clamp(in Vector4 vec, in Vector4 min, in Vector4 max, out Vector4 result)
    {
        result.x = vec.x < min.x ? min.x : vec.x > max.x ? max.x : vec.x;
        result.y = vec.y < min.y ? min.y : vec.y > max.y ? max.y : vec.y;
        result.z = vec.z < min.z ? min.z : vec.z > max.z ? max.z : vec.z;
        result.w = vec.w < min.w ? min.w : vec.w > max.w ? max.w : vec.w;
    }

    /// <summary>
    /// Take the component-wise absolute value of a vector.
    /// </summary>
    /// <param name="vec">The vector to apply component-wise absolute value to.</param>
    /// <returns>The component-wise absolute value vector.</returns>
    public static Vector4 Abs(Vector4 vec)
    {
        vec.x = MathF.Abs(vec.x);
        vec.y = MathF.Abs(vec.y);
        vec.z = MathF.Abs(vec.z);
        vec.w = MathF.Abs(vec.w);
        return vec;
    }

    /// <summary>
    /// Take the component-wise absolute value of a vector.
    /// </summary>
    /// <param name="vec">The vector to apply component-wise absolute value to.</param>
    /// <param name="result">The component-wise absolute value vector.</param>
    public static void Abs(in Vector4 vec, out Vector4 result)
    {
        result.x = MathF.Abs(vec.x);
        result.y = MathF.Abs(vec.y);
        result.z = MathF.Abs(vec.z);
        result.w = MathF.Abs(vec.w);
    }

    /// <summary>
    /// Component-wise rounding. Equivalent to calling <see cref="MathF.Round(float)"/> on each component.
    /// </summary>
    /// <param name="vec">The vector to round.</param>
    /// <returns>The component-wise rounded vector.</returns>
    [Pure]
    public static Vector4 Round(Vector4 vec)
    {
        vec.x = MathF.Round(vec.x);
        vec.y = MathF.Round(vec.y);
        vec.z = MathF.Round(vec.z);
        vec.w = MathF.Round(vec.w);
        return vec;
    }

    /// <summary>
    /// Component-wise rounding. Equivalent to calling <see cref="MathF.Round(float)"/> on each component.
    /// </summary>
    /// <param name="vec">The vector to round.</param>
    /// <param name="result">The component-wise rounded vector.</param>
    public static void Round(in Vector4 vec, out Vector4 result)
    {
        result.x = MathF.Round(vec.x);
        result.y = MathF.Round(vec.y);
        result.z = MathF.Round(vec.z);
        result.w = MathF.Round(vec.w);
    }

    /// <summary>
    /// Component-wise rounding with specified midpoint rounding rule.
    /// Equivalent to calling <see cref="MathF.Round(float,MidpointRounding)"/> on each component.
    /// </summary>
    /// <param name="vec">The vector to round.</param>
    /// <param name="rounding">The midpoint rounding rule to use.</param>
    /// <returns>The component-wise rounded vector.</returns>
    [Pure]
    public static Vector4 Round(Vector4 vec, MidpointRounding rounding)
    {
        vec.x = MathF.Round(vec.x, rounding);
        vec.y = MathF.Round(vec.y, rounding);
        vec.z = MathF.Round(vec.z, rounding);
        vec.w = MathF.Round(vec.w, rounding);
        return vec;
    }

    /// <summary>
    /// Component-wise rounding with specified midpoint rounding rule.
    /// Equivalent to calling <see cref="MathF.Round(float,MidpointRounding)"/> on each component.
    /// </summary>
    /// <param name="vec">The vector to round.</param>
    /// <param name="rounding">The midpoint rounding rule to use.</param>
    /// <param name="result">The component-wise rounded vector.</param>
    public static void Round(in Vector4 vec, MidpointRounding rounding, out Vector4 result)
    {
        result.x = MathF.Round(vec.x, rounding);
        result.y = MathF.Round(vec.y, rounding);
        result.z = MathF.Round(vec.z, rounding);
        result.w = MathF.Round(vec.w, rounding);
    }

    /// <summary>
    /// Component-wise ceiling operation.
    /// Equivalent to calling <see cref="MathF.Ceiling(float)"/> on each component.
    /// </summary>
    /// <param name="vec">The vector to take the ceiling of.</param>
    /// <returns>The component-wise ceiling vector.</returns>
    [Pure]
    public static Vector4 Ceiling(Vector4 vec)
    {
        vec.x = MathF.Ceiling(vec.x);
        vec.y = MathF.Ceiling(vec.y);
        vec.z = MathF.Ceiling(vec.z);
        vec.w = MathF.Ceiling(vec.w);
        return vec;
    }

    /// <summary>
    /// Component-wise ceiling operation.
    /// Equivalent to calling <see cref="MathF.Ceiling(float)"/> on each component.
    /// </summary>
    /// <param name="vec">The vector to take the ceiling of.</param>
    /// <param name="result">The component-wise ceiling vector.</param>
    public static void Ceiling(in Vector4 vec, out Vector4 result)
    {
        result.x = MathF.Ceiling(vec.x);
        result.y = MathF.Ceiling(vec.y);
        result.z = MathF.Ceiling(vec.z);
        result.w = MathF.Ceiling(vec.w);
    }

    /// <summary>
    /// Component-wise floor operation.
    /// Equivalent to calling <see cref="MathF.Floor(float)"/> on each component.
    /// </summary>
    /// <param name="vec">The vector to take the floor of.</param>
    /// <returns>The component-wise floored vector.</returns>
    [Pure]
    public static Vector4 Floor(Vector4 vec)
    {
        vec.x = MathF.Floor(vec.x);
        vec.y = MathF.Floor(vec.y);
        vec.z = MathF.Floor(vec.z);
        vec.w = MathF.Floor(vec.w);
        return vec;
    }

    /// <summary>
    /// Component-wise floor operation.
    /// Equivalent to calling <see cref="MathF.Floor(float)"/> on each component.
    /// </summary>
    /// <param name="vec">The vector to take the floor of.</param>
    /// <param name="result">The component-wise floored vector.</param>
    public static void Floor(in Vector4 vec, out Vector4 result)
    {
        result.x = MathF.Floor(vec.x);
        result.y = MathF.Floor(vec.y);
        result.z = MathF.Floor(vec.z);
        result.w = MathF.Floor(vec.w);
    }

    /// <summary>
    /// Component-wise truncation.
    /// Equivalent to calling <see cref="MathF.Truncate(float)"/> on each component.
    /// </summary>
    /// <param name="vec">The vector to truncate.</param>
    /// <returns>The component-wise truncated vector.</returns>
    [Pure]
    public static Vector4 Truncate(Vector4 vec)
    {
        vec.x = MathF.Truncate(vec.x);
        vec.y = MathF.Truncate(vec.y);
        vec.z = MathF.Truncate(vec.z);
        vec.w = MathF.Truncate(vec.w);
        return vec;
    }

    /// <summary>
    /// Component-wise truncation.
    /// Equivalent to calling <see cref="MathF.Truncate(float)"/> on each component.
    /// </summary>
    /// <param name="vec">The vector to truncate.</param>
    /// <param name="result">The component-wise truncated vector.</param>
    public static void Truncate(in Vector4 vec, out Vector4 result)
    {
        result.x = MathF.Truncate(vec.x);
        result.y = MathF.Truncate(vec.y);
        result.z = MathF.Truncate(vec.z);
        result.w = MathF.Truncate(vec.w);
    }

    /// <summary>
    /// Compute the euclidean distance between two vectors.
    /// </summary>
    /// <param name="vec1">The first vector.</param>
    /// <param name="vec2">The second vector.</param>
    /// <returns>The distance.</returns>
    [Pure]
    public static float Distance(Vector4 vec1, Vector4 vec2)
    {
        Distance(in vec1, in vec2, out float result);
        return result;
    }

    /// <summary>
    /// Compute the euclidean distance between two vectors.
    /// </summary>
    /// <param name="vec1">The first vector.</param>
    /// <param name="vec2">The second vector.</param>
    /// <param name="result">The distance.</param>
    public static void Distance(in Vector4 vec1, in Vector4 vec2, out float result)
    {
        result = MathF.Sqrt(((vec2.x - vec1.x) * (vec2.x - vec1.x)) + ((vec2.y - vec1.y) * (vec2.y - vec1.y)) + ((vec2.z - vec1.z) * (vec2.z - vec1.z)) + ((vec2.w - vec1.w) * (vec2.w - vec1.w)));
    }

    /// <summary>
    /// Compute the squared euclidean distance between two vectors.
    /// </summary>
    /// <param name="vec1">The first vector.</param>
    /// <param name="vec2">The second vector.</param>
    /// <returns>The squared distance.</returns>
    [Pure]
    public static float DistanceSquared(Vector4 vec1, Vector4 vec2)
    {
        DistanceSquared(in vec1, in vec2, out float result);
        return result;
    }

    /// <summary>
    /// Compute the squared euclidean distance between two vectors.
    /// </summary>
    /// <param name="vec1">The first vector.</param>
    /// <param name="vec2">The second vector.</param>
    /// <param name="result">The squared distance.</param>
    public static void DistanceSquared(in Vector4 vec1, in Vector4 vec2, out float result)
    {
        result = ((vec2.x - vec1.x) * (vec2.x - vec1.x)) + ((vec2.y - vec1.y) * (vec2.y - vec1.y)) + ((vec2.z - vec1.z) * (vec2.z - vec1.z)) + ((vec2.w - vec1.w) * (vec2.w - vec1.w));
    }

    /// <summary>
    /// Scale a vector to unit length.
    /// </summary>
    /// <param name="vec">The input vector.</param>
    /// <returns>The normalized copy.</returns>
    [Pure]
    public static Vector4 Normalize(Vector4 vec)
    {
        float scale = 1.0f / vec.Length;
        vec.x *= scale;
        vec.y *= scale;
        vec.z *= scale;
        vec.w *= scale;
        return vec;
    }

    /// <summary>
    /// Scale a vector to unit length.
    /// </summary>
    /// <param name="vec">The input vector.</param>
    /// <param name="result">The normalized vector.</param>
    public static void Normalize(in Vector4 vec, out Vector4 result)
    {
        float scale = 1.0f / vec.Length;
        result.x = vec.x * scale;
        result.y = vec.y * scale;
        result.z = vec.z * scale;
        result.w = vec.w * scale;
    }

    /// <summary>
    /// Scale a vector to approximately unit length.
    /// </summary>
    /// <param name="vec">The input vector.</param>
    /// <returns>The normalized copy.</returns>
    [Pure]
    public static Vector4 NormalizeFast(Vector4 vec)
    {
        float scale = Mathf.InverseSqrtFast((vec.x * vec.x) + (vec.y * vec.y) + (vec.z * vec.z) + (vec.w * vec.w));
        vec.x *= scale;
        vec.y *= scale;
        vec.z *= scale;
        vec.w *= scale;
        return vec;
    }

    /// <summary>
    /// Scale a vector to approximately unit length.
    /// </summary>
    /// <param name="vec">The input vector.</param>
    /// <param name="result">The normalized copy.</param>
    public static void NormalizeFast(in Vector4 vec, out Vector4 result)
    {
        float scale = Mathf.InverseSqrtFast((vec.x * vec.x) + (vec.y * vec.y) + (vec.z * vec.z) + (vec.w * vec.w));
        result.x = vec.x * scale;
        result.y = vec.y * scale;
        result.z = vec.z * scale;
        result.w = vec.w * scale;
    }

    /// <summary>
    /// Calculate the dot product of two vectors.
    /// </summary>
    /// <param name="left">First operand.</param>
    /// <param name="right">Second operand.</param>
    /// <returns>The dot product of the two inputs.</returns>
    [Pure]
    public static float Dot(Vector4 left, Vector4 right)
    {
        return (left.x * right.x) + (left.y * right.y) + (left.z * right.z) + (left.w * right.w);
    }

    /// <summary>
    /// Calculate the dot product of two vectors.
    /// </summary>
    /// <param name="left">First operand.</param>
    /// <param name="right">Second operand.</param>
    /// <param name="result">The dot product of the two inputs.</param>
    public static void Dot(in Vector4 left, in Vector4 right, out float result)
    {
        result = (left.x * right.x) + (left.y * right.y) + (left.z * right.z) + (left.w * right.w);
    }

    /// <summary>
    /// Returns a new vector that is the linear blend of the 2 given vectors.
    /// </summary>
    /// <param name="a">First input vector.</param>
    /// <param name="b">Second input vector.</param>
    /// <param name="blend">The blend factor.</param>
    /// <returns>a when blend=0, b when blend=1, and a linear combination otherwise.</returns>
    [Pure]
    public static Vector4 Lerp(Vector4 a, Vector4 b, float blend)
    {
        a.x = (blend * (b.x - a.x)) + a.x;
        a.y = (blend * (b.y - a.y)) + a.y;
        a.z = (blend * (b.z - a.z)) + a.z;
        a.w = (blend * (b.w - a.w)) + a.w;
        return a;
    }

    /// <summary>
    /// Returns a new vector that is the linear blend of the 2 given vectors.
    /// </summary>
    /// <param name="a">First input vector.</param>
    /// <param name="b">Second input vector.</param>
    /// <param name="blend">The blend factor.</param>
    /// <param name="result">a when blend=0, b when blend=1, and a linear combination otherwise.</param>
    public static void Lerp(in Vector4 a, in Vector4 b, float blend, out Vector4 result)
    {
        result.x = (blend * (b.x - a.x)) + a.x;
        result.y = (blend * (b.y - a.y)) + a.y;
        result.z = (blend * (b.z - a.z)) + a.z;
        result.w = (blend * (b.w - a.w)) + a.w;
    }

    /// <summary>
    /// Returns a new vector that is the component-wise linear blend of the 2 given vectors.
    /// </summary>
    /// <param name="a">First input vector.</param>
    /// <param name="b">Second input vector.</param>
    /// <param name="blend">The blend factor.</param>
    /// <returns>a when blend=0, b when blend=1, and a component-wise linear combination otherwise.</returns>
    [Pure]
    public static Vector4 Lerp(Vector4 a, Vector4 b, Vector4 blend)
    {
        a.x = (blend.x * (b.x - a.x)) + a.x;
        a.y = (blend.y * (b.y - a.y)) + a.y;
        a.z = (blend.z * (b.z - a.z)) + a.z;
        a.w = (blend.w * (b.w - a.w)) + a.w;
        return a;
    }

    /// <summary>
    /// Returns a new vector that is the component-wise linear blend of the 2 given vectors.
    /// </summary>
    /// <param name="a">First input vector.</param>
    /// <param name="b">Second input vector.</param>
    /// <param name="blend">The blend factor.</param>
    /// <param name="result">a when blend=0, b when blend=1, and a component-wise linear combination otherwise.</param>
    public static void Lerp(in Vector4 a, in Vector4 b, Vector4 blend, out Vector4 result)
    {
        result.x = (blend.x * (b.x - a.x)) + a.x;
        result.y = (blend.y * (b.y - a.y)) + a.y;
        result.z = (blend.z * (b.z - a.z)) + a.z;
        result.w = (blend.w * (b.w - a.w)) + a.w;
    }

    /// <summary>
    /// Returns a new vector that is the spherical interpolation of the two given vectors.
    /// <paramref name="a"/> and <paramref name="b"/> need to be normalized for this function to work properly.
    /// Results are undefined for vectors that point in opposite directions or very close to opposite directions.
    /// </summary>
    /// <param name="a">Unit vector start point.</param>
    /// <param name="b">Unit vector end point.</param>
    /// <param name="t">The blend factor.</param>
    /// <returns><paramref name="a"/> when <paramref name="t"/>=0, <paramref name="b"/> when <paramref name="t"/>=1, and a spherical interpolation between the vectors otherwise.</returns>
    [Pure]
    public static Vector4 Slerp(Vector4 a, Vector4 b, float t)
    {
        float abLength = a.Length * b.Length;
        float cosTheta;
        if (abLength == 0 || Math.Abs(cosTheta = Dot(a, b) / abLength) > 0.9999f)
        {
            return Lerp(a, b, t);
        }
        else
        {
            float theta = MathF.Acos(Math.Clamp(cosTheta, -1, 1));
            // We use the fact that:
            // sin(θ) = sqrt(1 - cos(θ)^2)
            // to avoid doing sin(θ) which is slower than sqrt.
            float sinTheta = MathF.Sqrt(1 - (cosTheta * cosTheta));
            float acoef = MathF.Sin((1 - t) * theta) / sinTheta;
            float bcoef = MathF.Sin(t * theta) / sinTheta;
            return (acoef * a) + (bcoef * b);
        }
    }

    /// <summary>
    /// Returns a new vector that is the spherical interpolation of the two given vectors.
    /// <paramref name="a"/> and <paramref name="b"/> need to be normalized for this function to work properly.
    /// Results are undefined for vectors that point in opposite directions or very close to opposite directions.
    /// </summary>
    /// <param name="a">Unit vector start point.</param>
    /// <param name="b">Unit vector end point.</param>
    /// <param name="t">The blend factor.</param>
    /// <param name="result">Is <paramref name="a"/> when <paramref name="t"/>=0, <paramref name="b"/> when <paramref name="t"/>=1, and a spherical interpolation between the vectors otherwise.</param>
    public static void Slerp(in Vector4 a, in Vector4 b, float t, out Vector4 result)
    {
        float abLength = a.Length * b.Length;
        if (abLength == 0)
        {
            Lerp(in a, in b, t, out result);
        }
        else
        {
            Dot(in a, in b, out float cosTheta);
            cosTheta /= abLength;
            if (Math.Abs(cosTheta) > 0.9999f)
            {
                Lerp(in a, in b, t, out result);
            }
            else
            {
                float theta = MathF.Acos(cosTheta);
                // We use the fact that:
                // sin(θ) = sqrt(1 - cos(θ)^2)
                // to avoid doing sin(θ) which is slower than sqrt.
                float sinTheta = MathF.Sqrt(1 - (cosTheta * cosTheta));
                float acoef = MathF.Sin((1 - t) * theta) / sinTheta;
                float bcoef = MathF.Sin(t * theta) / sinTheta;
                result = (acoef * a) + (bcoef * b);
            }
        }
    }

    /// <summary>
    /// Returns a new vector that is the exponential interpolation of the two vectors.
    /// Equivalent to <c>a * pow(b/a, t)</c>.
    /// </summary>
    /// <param name="a">The starting value. Must be non-negative.</param>
    /// <param name="b">The end value. Must be non-negative.</param>
    /// <param name="t">The blend factor.</param>
    /// <returns>The exponential interpolation between <paramref name="a"/> and <paramref name="b"/>.</returns>
    /// <seealso cref="Mathf.Elerp(float, float, float)"/>
    public static Vector4 Elerp(Vector4 a, Vector4 b, float t)
    {
        a.x = MathF.Pow(a.x, 1 - t) * MathF.Pow(b.x, t);
        a.y = MathF.Pow(a.y, 1 - t) * MathF.Pow(b.y, t);
        a.z = MathF.Pow(a.z, 1 - t) * MathF.Pow(b.z, t);
        a.w = MathF.Pow(a.w, 1 - t) * MathF.Pow(b.w, t);
        return a;
    }

    /// <summary>
    /// Returns a new vector that is the exponential interpolation of the two vectors.
    /// Equivalent to <c>a * pow(b/a, t)</c>.
    /// </summary>
    /// <param name="a">The starting value. Must be non-negative.</param>
    /// <param name="b">The end value. Must be non-negative.</param>
    /// <param name="t">The blend factor.</param>
    /// <param name="result">The exponential interpolation between <paramref name="a"/> and <paramref name="b"/>.</param>
    /// <seealso cref="Mathf.Elerp(float, float, float)"/>
    public static void Elerp(in Vector4 a, in Vector4 b, float t, out Vector4 result)
    {
        result.x = MathF.Pow(a.x, 1 - t) * MathF.Pow(b.x, t);
        result.y = MathF.Pow(a.y, 1 - t) * MathF.Pow(b.y, t);
        result.z = MathF.Pow(a.z, 1 - t) * MathF.Pow(b.z, t);
        result.w = MathF.Pow(a.w, 1 - t) * MathF.Pow(b.w, t);
    }

    /// <summary>
    /// Interpolate 3 Vectors using Barycentric coordinates.
    /// </summary>
    /// <param name="a">First input Vector.</param>
    /// <param name="b">Second input Vector.</param>
    /// <param name="c">Third input Vector.</param>
    /// <param name="u">First Barycentric Coordinate.</param>
    /// <param name="v">Second Barycentric Coordinate.</param>
    /// <returns>a when u=v=0, b when u=1,v=0, c when u=0,v=1, and a linear combination of a,b,c otherwise.</returns>
    [Pure]
    public static Vector4 BaryCentric(Vector4 a, Vector4 b, Vector4 c, float u, float v)
    {
        BaryCentric(in a, in b, in c, u, v, out Vector4 result);
        return result;
    }

    /// <summary>
    /// Interpolate 3 Vectors using Barycentric coordinates.
    /// </summary>
    /// <param name="a">First input Vector.</param>
    /// <param name="b">Second input Vector.</param>
    /// <param name="c">Third input Vector.</param>
    /// <param name="u">First Barycentric Coordinate.</param>
    /// <param name="v">Second Barycentric Coordinate.</param>
    /// <param name="result">
    /// Output Vector. a when u=v=0, b when u=1,v=0, c when u=0,v=1, and a linear combination of a,b,c
    /// otherwise.
    /// </param>
    public static void BaryCentric
    (
        in Vector4 a,
        in Vector4 b,
        in Vector4 c,
        float u,
        float v,
        out Vector4 result
    )
    {
        Subtract(in b, in a, out Vector4 ab);
        Multiply(in ab, u, out Vector4 abU);
        Add(in a, in abU, out Vector4 uPos);

        Subtract(in c, in a, out Vector4 ac);
        Multiply(in ac, v, out Vector4 acV);
        Add(in uPos, in acV, out result);
    }

    /// <summary>
    /// Transform a Vector by the given Matrix.
    /// </summary>
    /// <param name="vec">The vector to transform.</param>
    /// <param name="mat">The desired transformation.</param>
    /// <returns>The transformed vector.</returns>
    [Pure]
    public static Vector4 TransformRow(Vector4 vec, Matrix4x4 mat)
    {
        TransformRow(in vec, in mat, out Vector4 result);
        return result;
    }

    /// <summary>
    /// Transform a Vector by the given Matrix.
    /// </summary>
    /// <param name="vec">The vector to transform.</param>
    /// <param name="mat">The desired transformation.</param>
    /// <param name="result">The transformed vector.</param>
    public static void TransformRow(in Vector4 vec, in Matrix4x4 mat, out Vector4 result)
    {
        result = new Vector4(
            (vec.x * mat.Row0.x) + (vec.y * mat.Row1.x) + (vec.z * mat.Row2.x) + (vec.w * mat.Row3.x),
            (vec.x * mat.Row0.y) + (vec.y * mat.Row1.y) + (vec.z * mat.Row2.y) + (vec.w * mat.Row3.y),
            (vec.x * mat.Row0.z) + (vec.y * mat.Row1.z) + (vec.z * mat.Row2.z) + (vec.w * mat.Row3.z),
            (vec.x * mat.Row0.w) + (vec.y * mat.Row1.w) + (vec.z * mat.Row2.w) + (vec.w * mat.Row3.w));
    }

    /// <summary>
    /// Transforms a vector by a quaternion rotation.
    /// </summary>
    /// <param name="vec">The vector to transform.</param>
    /// <param name="quat">The quaternion to rotate the vector by.</param>
    /// <returns>The result of the operation.</returns>
    [Pure]
    public static Vector4 Transform(Vector4 vec, Quaternion quat)
    {
        Transform(in vec, in quat, out Vector4 result);
        return result;
    }

    /// <summary>
    /// Transforms a vector by a quaternion rotation.
    /// </summary>
    /// <param name="vec">The vector to transform.</param>
    /// <param name="quat">The quaternion to rotate the vector by.</param>
    /// <param name="result">The result of the operation.</param>
    public static void Transform(in Vector4 vec, in Quaternion quat, out Vector4 result)
    {
        Quaternion v = new Quaternion(vec.x, vec.y, vec.z, vec.w);
        Quaternion.Invert(in quat, out Quaternion i);
        Quaternion.Multiply(in quat, in v, out Quaternion t);
        Quaternion.Multiply(in t, in i, out v);

        result.x = v.X;
        result.y = v.Y;
        result.z = v.Z;
        result.w = v.w;
    }

    /// <summary>
    /// Transform a Vector by the given Matrix using right-handed notation.
    /// </summary>
    /// <param name="mat">The desired transformation.</param>
    /// <param name="vec">The vector to transform.</param>
    /// <returns>The transformed vector.</returns>
    [Pure]
    public static Vector4 TransformColumn(Matrix4x4 mat, Vector4 vec)
    {
        TransformColumn(in mat, in vec, out Vector4 result);
        return result;
    }

    /// <summary>
    /// Transform a Vector by the given Matrix using right-handed notation.
    /// </summary>
    /// <param name="mat">The desired transformation.</param>
    /// <param name="vec">The vector to transform.</param>
    /// <param name="result">The transformed vector.</param>
    public static void TransformColumn(in Matrix4x4 mat, in Vector4 vec, out Vector4 result)
    {
        result = new Vector4(
            (mat.Row0.x * vec.x) + (mat.Row0.y * vec.y) + (mat.Row0.z * vec.z) + (mat.Row0.w * vec.w),
            (mat.Row1.x * vec.x) + (mat.Row1.y * vec.y) + (mat.Row1.z * vec.z) + (mat.Row1.w * vec.w),
            (mat.Row2.x * vec.x) + (mat.Row2.y * vec.y) + (mat.Row2.z * vec.z) + (mat.Row2.w * vec.w),
            (mat.Row3.x * vec.x) + (mat.Row3.y * vec.y) + (mat.Row3.z * vec.z) + (mat.Row3.w * vec.w));
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector2 with the X and Y components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector2 Xy
    {
        get => Unsafe.As<Vector4, Vector2>(ref this);
        set
        {
            x = value.x;
            y = value.y;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector2 with the X and Z components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector2 Xz
    {
        readonly get => new Vector2(x, z);
        set
        {
            x = value.x;
            z = value.y;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector2 with the X and W components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector2 Xw
    {
        readonly get => new Vector2(x, w);
        set
        {
            x = value.x;
            w = value.y;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector2 with the Y and X components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector2 Yx
    {
        readonly get => new Vector2(y, x);
        set
        {
            y = value.x;
            x = value.y;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector2 with the Y and Z components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector2 Yz
    {
        readonly get => new Vector2(y, z);
        set
        {
            y = value.x;
            z = value.y;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector2 with the Y and W components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector2 Yw
    {
        readonly get => new Vector2(y, w);
        set
        {
            y = value.x;
            w = value.y;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector2 with the Z and X components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector2 Zx
    {
        readonly get => new Vector2(z, x);
        set
        {
            z = value.x;
            x = value.y;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector2 with the Z and Y components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector2 Zy
    {
        readonly get => new Vector2(z, y);
        set
        {
            z = value.x;
            y = value.y;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector2 with the Z and W components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector2 Zw
    {
        readonly get => new Vector2(z, w);
        set
        {
            z = value.x;
            w = value.y;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector2 with the W and X components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector2 Wx
    {
        readonly get => new Vector2(w, x);
        set
        {
            w = value.x;
            x = value.y;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector2 with the W and Y components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector2 Wy
    {
        readonly get => new Vector2(w, y);
        set
        {
            w = value.x;
            y = value.y;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector2 with the W and Z components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector2 Wz
    {
        readonly get => new Vector2(w, z);
        set
        {
            w = value.x;
            z = value.y;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector3 with the X, Y, and Z components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector3 Xyz
    {
        get => Unsafe.As<Vector4, Vector3>(ref this);
        set
        {
            x = value.x;
            y = value.y;
            z = value.z;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector3 with the X, Y, and Z components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector3 Xyw
    {
        readonly get => new Vector3(x, y, w);
        set
        {
            x = value.x;
            y = value.y;
            w = value.z;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector3 with the X, Z, and Y components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector3 Xzy
    {
        readonly get => new Vector3(x, z, y);
        set
        {
            x = value.x;
            z = value.y;
            y = value.z;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector3 with the X, Z, and W components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector3 Xzw
    {
        readonly get => new Vector3(x, z, w);
        set
        {
            x = value.x;
            z = value.y;
            w = value.z;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector3 with the X, W, and Y components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector3 Xwy
    {
        readonly get => new Vector3(x, w, y);
        set
        {
            x = value.x;
            w = value.y;
            y = value.z;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector3 with the X, W, and Z components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector3 Xwz
    {
        readonly get => new Vector3(x, w, z);
        set
        {
            x = value.x;
            w = value.y;
            z = value.z;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector3 with the Y, X, and Z components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector3 Yxz
    {
        readonly get => new Vector3(y, x, z);
        set
        {
            y = value.x;
            x = value.y;
            z = value.z;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector3 with the Y, X, and W components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector3 Yxw
    {
        readonly get => new Vector3(y, x, w);
        set
        {
            y = value.x;
            x = value.y;
            w = value.z;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector3 with the Y, Z, and X components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector3 Yzx
    {
        readonly get => new Vector3(y, z, x);
        set
        {
            y = value.x;
            z = value.y;
            x = value.z;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector3 with the Y, Z, and W components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector3 Yzw
    {
        readonly get => new Vector3(y, z, w);
        set
        {
            y = value.x;
            z = value.y;
            w = value.z;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector3 with the Y, W, and X components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector3 Ywx
    {
        readonly get => new Vector3(y, w, x);
        set
        {
            y = value.x;
            w = value.y;
            x = value.z;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector3 with the Y, W, and Z components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector3 Ywz
    {
        readonly get => new Vector3(y, w, z);
        set
        {
            y = value.x;
            w = value.y;
            z = value.z;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector3 with the Z, X, and Y components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector3 Zxy
    {
        readonly get => new Vector3(z, x, y);
        set
        {
            z = value.x;
            x = value.y;
            y = value.z;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector3 with the Z, X, and W components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector3 Zxw
    {
        readonly get => new Vector3(z, x, w);
        set
        {
            z = value.x;
            x = value.y;
            w = value.z;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector3 with the Z, Y, and X components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector3 Zyx
    {
        readonly get => new Vector3(z, y, x);
        set
        {
            z = value.x;
            y = value.y;
            x = value.z;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector3 with the Z, Y, and W components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector3 Zyw
    {
        readonly get => new Vector3(z, y, w);
        set
        {
            z = value.x;
            y = value.y;
            w = value.z;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector3 with the Z, W, and X components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector3 Zwx
    {
        readonly get => new Vector3(z, w, x);
        set
        {
            z = value.x;
            w = value.y;
            x = value.z;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector3 with the Z, W, and Y components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector3 Zwy
    {
        readonly get => new Vector3(z, w, y);
        set
        {
            z = value.x;
            w = value.y;
            y = value.z;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector3 with the W, X, and Y components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector3 Wxy
    {
        readonly get => new Vector3(w, x, y);
        set
        {
            w = value.x;
            x = value.y;
            y = value.z;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector3 with the W, X, and Z components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector3 Wxz
    {
        readonly get => new Vector3(w, x, z);
        set
        {
            w = value.x;
            x = value.y;
            z = value.z;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector3 with the W, Y, and X components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector3 Wyx
    {
        readonly get => new Vector3(w, y, x);
        set
        {
            w = value.x;
            y = value.y;
            x = value.z;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector3 with the W, Y, and Z components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector3 Wyz
    {
        readonly get => new Vector3(w, y, z);
        set
        {
            w = value.x;
            y = value.y;
            z = value.z;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector3 with the W, Z, and X components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector3 Wzx
    {
        readonly get => new Vector3(w, z, x);
        set
        {
            w = value.x;
            z = value.y;
            x = value.z;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector3 with the W, Z, and Y components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector3 Wzy
    {
        readonly get => new Vector3(w, z, y);
        set
        {
            w = value.x;
            z = value.y;
            y = value.z;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector4 with the X, Y, W, and Z components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4 Xywz
    {
        readonly get => new Vector4(x, y, w, z);
        set
        {
            x = value.x;
            y = value.y;
            w = value.z;
            z = value.w;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector4 with the X, Z, Y, and W components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4 Xzyw
    {
        readonly get => new Vector4(x, z, y, w);
        set
        {
            x = value.x;
            z = value.y;
            y = value.z;
            w = value.w;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector4 with the X, Z, W, and Y components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4 Xzwy
    {
        readonly get => new Vector4(x, z, w, y);
        set
        {
            x = value.x;
            z = value.y;
            w = value.z;
            y = value.w;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector4 with the X, W, Y, and Z components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4 Xwyz
    {
        readonly get => new Vector4(x, w, y, z);
        set
        {
            x = value.x;
            w = value.y;
            y = value.z;
            z = value.w;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector4 with the X, W, Z, and Y components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4 Xwzy
    {
        readonly get => new Vector4(x, w, z, y);
        set
        {
            x = value.x;
            w = value.y;
            z = value.z;
            y = value.w;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector4 with the Y, X, Z, and W components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4 Yxzw
    {
        readonly get => new Vector4(y, x, z, w);
        set
        {
            y = value.x;
            x = value.y;
            z = value.z;
            w = value.w;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector4 with the Y, X, W, and Z components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4 Yxwz
    {
        readonly get => new Vector4(y, x, w, z);
        set
        {
            y = value.x;
            x = value.y;
            w = value.z;
            z = value.w;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector4 with the Y, Y, Z, and W components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4 Yyzw
    {
        readonly get => new Vector4(y, y, z, w);
        set
        {
            x = value.x;
            y = value.y;
            z = value.z;
            w = value.w;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector4 with the Y, Y, W, and Z components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4 Yywz
    {
        readonly get => new Vector4(y, y, w, z);
        set
        {
            x = value.x;
            y = value.y;
            w = value.z;
            z = value.w;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector4 with the Y, Z, X, and W components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4 Yzxw
    {
        readonly get => new Vector4(y, z, x, w);
        set
        {
            y = value.x;
            z = value.y;
            x = value.z;
            w = value.w;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector4 with the Y, Z, W, and X components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4 Yzwx
    {
        readonly get => new Vector4(y, z, w, x);
        set
        {
            y = value.x;
            z = value.y;
            w = value.z;
            x = value.w;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector4 with the Y, W, X, and Z components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4 Ywxz
    {
        readonly get => new Vector4(y, w, x, z);
        set
        {
            y = value.x;
            w = value.y;
            x = value.z;
            z = value.w;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector4 with the Y, W, Z, and X components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4 Ywzx
    {
        readonly get => new Vector4(y, w, z, x);
        set
        {
            y = value.x;
            w = value.y;
            z = value.z;
            x = value.w;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector4 with the Z, X, Y, and Z components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4 Zxyw
    {
        readonly get => new Vector4(z, x, y, w);
        set
        {
            z = value.x;
            x = value.y;
            y = value.z;
            w = value.w;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector4 with the Z, X, W, and Y components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4 Zxwy
    {
        readonly get => new Vector4(z, x, w, y);
        set
        {
            z = value.x;
            x = value.y;
            w = value.z;
            y = value.w;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector4 with the Z, Y, X, and W components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4 Zyxw
    {
        readonly get => new Vector4(z, y, x, w);
        set
        {
            z = value.x;
            y = value.y;
            x = value.z;
            w = value.w;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector4 with the Z, Y, W, and X components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4 Zywx
    {
        readonly get => new Vector4(z, y, w, x);
        set
        {
            z = value.x;
            y = value.y;
            w = value.z;
            x = value.w;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector4 with the Z, W, X, and Y components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4 Zwxy
    {
        readonly get => new Vector4(z, w, x, y);
        set
        {
            z = value.x;
            w = value.y;
            x = value.z;
            y = value.w;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector4 with the Z, W, Y, and X components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4 Zwyx
    {
        readonly get => new Vector4(z, w, y, x);
        set
        {
            z = value.x;
            w = value.y;
            y = value.z;
            x = value.w;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector4 with the Z, W, Z, and Y components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4 Zwzy
    {
        readonly get => new Vector4(z, w, z, y);
        set
        {
            x = value.x;
            w = value.y;
            z = value.z;
            y = value.w;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector4 with the W, X, Y, and Z components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4 Wxyz
    {
        readonly get => new Vector4(w, x, y, z);
        set
        {
            w = value.x;
            x = value.y;
            y = value.z;
            z = value.w;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector4 with the W, X, Z, and Y components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4 Wxzy
    {
        readonly get => new Vector4(w, x, z, y);
        set
        {
            w = value.x;
            x = value.y;
            z = value.z;
            y = value.w;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector4 with the W, Y, X, and Z components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4 Wyxz
    {
        readonly get => new Vector4(w, y, x, z);
        set
        {
            w = value.x;
            y = value.y;
            x = value.z;
            z = value.w;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector4 with the W, Y, Z, and X components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4 Wyzx
    {
        readonly get => new Vector4(w, y, z, x);
        set
        {
            w = value.x;
            y = value.y;
            z = value.z;
            x = value.w;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector4 with the W, Z, X, and Y components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4 Wzxy
    {
        readonly get => new Vector4(w, z, x, y);
        set
        {
            w = value.x;
            z = value.y;
            x = value.z;
            y = value.w;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector4 with the W, Z, Y, and X components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4 Wzyx
    {
        readonly get => new Vector4(w, z, y, x);
        set
        {
            w = value.x;
            z = value.y;
            y = value.z;
            x = value.w;
        }
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector4 with the W, Z, Y, and W components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector4 Wzyw
    {
        readonly get => new Vector4(w, z, y, w);
        set
        {
            x = value.x;
            z = value.y;
            y = value.z;
            w = value.w;
        }
    }

    /// <summary>
    /// Adds two instances.
    /// </summary>
    /// <param name="left">The first instance.</param>
    /// <param name="right">The second instance.</param>
    /// <returns>The result of the calculation.</returns>
    [Pure]
    public static Vector4 operator +(Vector4 left, Vector4 right)
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
    public static Vector4 operator -(Vector4 left, Vector4 right)
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
    public static Vector4 operator -(Vector4 vec)
    {
        vec.x = -vec.x;
        vec.y = -vec.y;
        vec.z = -vec.z;
        vec.w = -vec.w;
        return vec;
    }

    /// <summary>
    /// Multiplies an instance by a scalar.
    /// </summary>
    /// <param name="vec">The instance.</param>
    /// <param name="scale">The scalar.</param>
    /// <returns>The result of the calculation.</returns>
    [Pure]
    public static Vector4 operator *(Vector4 vec, float scale)
    {
        vec.x *= scale;
        vec.y *= scale;
        vec.z *= scale;
        vec.w *= scale;
        return vec;
    }

    /// <summary>
    /// Multiplies an instance by a scalar.
    /// </summary>
    /// <param name="scale">The scalar.</param>
    /// <param name="vec">The instance.</param>
    /// <returns>The result of the calculation.</returns>
    [Pure]
    public static Vector4 operator *(float scale, Vector4 vec)
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
    public static Vector4 operator *(Vector4 vec, Vector4 scale)
    {
        vec.x *= scale.x;
        vec.y *= scale.y;
        vec.z *= scale.z;
        vec.w *= scale.w;
        return vec;
    }

    /// <summary>
    /// Transform a Vector by the given Matrix.
    /// </summary>
    /// <param name="vec">The vector to transform.</param>
    /// <param name="mat">The desired transformation.</param>
    /// <returns>The transformed vector.</returns>
    [Pure]
    public static Vector4 operator *(Vector4 vec, Matrix4x4 mat)
    {
        TransformRow(in vec, in mat, out Vector4 result);
        return result;
    }

    /// <summary>
    /// Transform a Vector by the given Matrix using right-handed notation.
    /// </summary>
    /// <param name="mat">The desired transformation.</param>
    /// <param name="vec">The vector to transform.</param>
    /// <returns>The transformed vector.</returns>
    [Pure]
    public static Vector4 operator *(Matrix4x4 mat, Vector4 vec)
    {
        TransformColumn(in mat, in vec, out Vector4 result);
        return result;
    }

    /// <summary>
    /// Transforms a vector by a quaternion rotation.
    /// </summary>
    /// <param name="quat">The quaternion to rotate the vector by.</param>
    /// <param name="vec">The vector to transform.</param>
    /// <returns>The transformed vector.</returns>
    [Pure]
    public static Vector4 operator *(Quaternion quat, Vector4 vec)
    {
        Transform(in vec, in quat, out Vector4 result);
        return result;
    }

    /// <summary>
    /// Divides an instance by a scalar.
    /// </summary>
    /// <param name="vec">The instance.</param>
    /// <param name="scale">The scalar.</param>
    /// <returns>The result of the calculation.</returns>
    [Pure]
    public static Vector4 operator /(Vector4 vec, float scale)
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
    public static Vector4 operator /(Vector4 vec, Vector4 scale)
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
    public static bool operator ==(Vector4 left, Vector4 right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Compares two instances for inequality.
    /// </summary>
    /// <param name="left">The first instance.</param>
    /// <param name="right">The second instance.</param>
    /// <returns>True, if left does not equa lright; false otherwise.</returns>
    public static bool operator !=(Vector4 left, Vector4 right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Returns a pointer to the first element of the specified instance.
    /// </summary>
    /// <param name="v">The instance.</param>
    /// <returns>A pointer to the first element of v.</returns>
    [Pure]
    public static unsafe explicit operator float*(Vector4 v)
    {
        return &v.x;
    }

    /// <summary>
    /// Returns a pointer to the first element of the specified instance.
    /// </summary>
    /// <param name="v">The instance.</param>
    /// <returns>A pointer to the first element of v.</returns>
    [Pure]
    public static explicit operator IntPtr(Vector4 v)
    {
        unsafe
        {
            return (IntPtr)(&v.x);
        }
    }

    /// <summary>
    /// Returns this Vector4 as a Color4. The resulting struct will have RGBA mapped to XYZW, in that order.
    /// </summary>
    /// <param name="v">The Vector4 to convert.</param>
    /// <returns>The Vector4, converted to a Color4.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public static explicit operator Color(Vector4 v)
    {
        return Unsafe.As<Vector4, Color>(ref v);
    }

    /// <summary>
    /// Converts OpenTK.Vector4 to OpenTK.Vector4i.
    /// </summary>
    /// <param name="vec">The Vector4 to convert.</param>
    /// <returns>The resulting Vector4i.</returns>
    [Pure]
    public static explicit operator Vector4Int(Vector4 vec)
    {
        return new Vector4Int((int)vec.x, (int)vec.y, (int)vec.z, (int)vec.w);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector4"/> struct using a tuple containing the component
    /// values.
    /// </summary>
    /// <param name="values">A tuple containing the component values.</param>
    /// <returns>A new instance of the <see cref="Vector4"/> struct with the given component values.</returns>
    [Pure]
    public static implicit operator Vector4((float X, float Y, float Z, float W) values)
    {
        return new Vector4(values.X, values.Y, values.Z, values.W);
    }

    /// <summary>
    /// Converts <see cref="System.Numerics.Vector4"/> to <see cref="Vector4"/>.
    /// </summary>
    /// <param name="vec">The <see cref="System.Numerics.Vector4"/> to cast.</param>
    [Pure]
    public static explicit operator Vector4(System.Numerics.Vector4 vec)
    {
        return Unsafe.As<System.Numerics.Vector4, Vector4>(ref vec);
    }

    /// <summary>
    /// Converts <see cref="Vector4"/> to <see cref="System.Numerics.Vector4"/>.
    /// </summary>
    /// <param name="vec">The <see cref="Vector4"/> to cast.</param>
    [Pure]
    public static explicit operator System.Numerics.Vector4(Vector4 vec)
    {
        return Unsafe.As<Vector4, System.Numerics.Vector4>(ref vec);
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
        return obj is Vector4 && Equals((Vector4)obj);
    }

    /// <inheritdoc />
    public readonly bool Equals(Vector4 other)
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
    public readonly void Deconstruct(out float x, out float y, out float z, out float w)
    {
        x = this.x;
        y = this.y;
        z = this.z;
        w = this.w;
    }
}
