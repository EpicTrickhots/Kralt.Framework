using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace Kralt.Mathematics;

/// <summary>
/// Represents a 3D vector using three single-precision floating-point numbers.
/// </summary>
/// <remarks>
/// The Vector3 structure is suitable for interoperation with unmanaged code requiring three consecutive floats.
/// </remarks>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct Vector3 : IEquatable<Vector3>, IFormattable
{
    /// <summary>
    /// The X component of the Vector3.
    /// </summary>
    public float x;

    /// <summary>
    /// The Y component of the Vector3.
    /// </summary>
    public float y;

    /// <summary>
    /// The Z component of the Vector3.
    /// </summary>
    public float z;

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector3"/> struct.
    /// </summary>
    /// <param name="value">The value that will initialize this instance.</param>
    public Vector3(float value)
    {
        x = value;
        y = value;
        z = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector3"/> struct.
    /// </summary>
    /// <param name="x">The x component of the Vector3.</param>
    /// <param name="y">The y component of the Vector3.</param>
    /// <param name="z">The z component of the Vector3.</param>
    public Vector3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector3"/> struct.
    /// </summary>
    /// <param name="v">The Vector2 to copy components from.</param>
    public Vector3(Vector2 v)
    {
        x = v.x;
        y = v.y;
        z = 0.0f;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector3"/> struct.
    /// </summary>
    /// <param name="v">The Vector3 to copy components from.</param>
    public Vector3(Vector3 v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector3"/> struct.
    /// </summary>
    /// <param name="v">The Vector4 to copy components from.</param>
    public Vector3(Vector4 v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
    }

    /// <summary>
    /// Gets or sets the value at the index of the Vector.
    /// </summary>
    /// <param name="index">The index of the component from the Vector.</param>
    /// <exception cref="IndexOutOfRangeException">Thrown if the index is less than 0 or greater than 2.</exception>
    public float this[int index]
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
    private readonly ref float GetElementUnsafe(in Vector3 v, int index)
    {
        ref float address = ref Unsafe.AsRef(in v.x);
        return ref Unsafe.Add(ref address, index);
    }

    /// <summary>
    /// Gets the length (magnitude) of the vector.
    /// </summary>
    /// <see cref="LengthFast"/>
    /// <seealso cref="LengthSquared"/>
    public readonly float Length => MathF.Sqrt((x * x) + (y * y) + (z * z));

    /// <summary>
    /// Gets an approximation of 1 over the length (magnitude) of the vector.
    /// </summary>
    public readonly float ReciprocalLengthFast => Mathf.InverseSqrtFast((x * x) + (y * y) + (z * z));

    /// <summary>
    /// Gets an approximation of the vector length (magnitude).
    /// </summary>
    /// <remarks>
    /// This property uses an approximation of the square root function to calculate vector magnitude, with
    /// an upper error bound of 0.001.
    /// </remarks>
    /// <see cref="Length"/>
    /// <seealso cref="LengthSquared"/>
    public readonly float LengthFast => 1.0f / Mathf.InverseSqrtFast((x * x) + (y * y) + (z * z));

    /// <summary>
    /// Gets the square of the vector length (magnitude).
    /// </summary>
    /// <remarks>
    /// This property avoids the costly square root operation required by the Length property. This makes it more suitable
    /// for comparisons.
    /// </remarks>
    /// <see cref="Length"/>
    /// <seealso cref="LengthFast"/>
    public readonly float LengthSquared => (x * x) + (y * y) + (z * z);

    /// <summary>
    /// Returns a copy of the Vector3 scaled to unit length.
    /// </summary>
    /// <returns>The normalized copy.</returns>
    public readonly Vector3 Normalized()
    {
        Vector3 v = this;
        v.Normalize();
        return v;
    }

    /// <summary>
    /// Scales the Vector3 to unit length.
    /// </summary>
    public void Normalize()
    {
        float scale = 1.0f / Length;
        x *= scale;
        y *= scale;
        z *= scale;
    }

    /// <summary>
    /// Scales the Vector3 to approximately unit length.
    /// </summary>
    public void NormalizeFast()
    {
        float scale = Mathf.InverseSqrtFast((x * x) + (y * y) + (z * z));
        x *= scale;
        y *= scale;
        z *= scale;
    }

    /// <summary>
    /// Returns a new vector that is the component-wise absolute value of the vector.
    /// </summary>
    /// <returns>The component-wise absolute value vector.</returns>
    public readonly Vector3 Abs()
    {
        Vector3 result = this;
        result.x = MathF.Abs(result.x);
        result.y = MathF.Abs(result.y);
        result.z = MathF.Abs(result.z);
        return result;
    }

    /// <summary>
    /// Returns a new vector were component-wise rounding has been applied.
    /// Equivalent to calling <see cref="MathF.Round(float)"/> on each component.
    /// </summary>
    /// <returns>The rounded vector.</returns>
    public readonly Vector3 Round()
    {
        return Round(this);
    }

    /// <summary>
    /// Returns a new vector were component-wise rounding has been applied with the specified midpoint rounding rule.
    /// Equivalent to calling <see cref="MathF.Round(float,MidpointRounding)"/> on each component.
    /// </summary>
    /// <param name="rounding">The midpoint rounding rule to use.</param>
    /// <returns>The rounded vector.</returns>
    public readonly Vector3 Round(MidpointRounding rounding)
    {
        return Round(this, rounding);
    }

    /// <summary>
    /// Returns a new vector were a component-wise ceiling operation has been applied.
    /// Equivalent to calling <see cref="MathF.Ceiling(float)"/> on each component.
    /// </summary>
    /// <returns>The ceiled vector.</returns>
    public readonly Vector3 Ceiling()
    {
        return Ceiling(this);
    }

    /// <summary>
    /// Returns a new vector were a component-wise floor operation has been applied.
    /// Equivalent to calling <see cref="MathF.Floor(float)"/> on each component.
    /// </summary>
    /// <returns>The floored vector.</returns>
    public readonly Vector3 Floor()
    {
        return Floor(this);
    }

    /// <summary>
    /// Returns a new vector were component-wise truncation has been applied.
    /// Equivalent to calling <see cref="MathF.Truncate(float)"/> on each component.
    /// </summary>
    /// <returns>The truncated vector.</returns>
    public readonly Vector3 Truncate()
    {
        return Truncate(this);
    }

    /// <summary>
    /// Defines a unit-length Vector3 that points towards the X-axis.
    /// </summary>
    public static readonly Vector3 UnitX = new Vector3(1, 0, 0);

    /// <summary>
    /// Defines a unit-length Vector3 that points towards the Y-axis.
    /// </summary>
    public static readonly Vector3 UnitY = new Vector3(0, 1, 0);

    /// <summary>
    /// Defines a unit-length Vector3 that points towards the Z-axis.
    /// </summary>
    public static readonly Vector3 UnitZ = new Vector3(0, 0, 1);

    /// <summary>
    /// Defines an instance with all components set to 0.
    /// </summary>
    public static readonly Vector3 Zero = new Vector3(0, 0, 0);

    /// <summary>
    /// Defines an instance with all components set to 1.
    /// </summary>
    public static readonly Vector3 One = new Vector3(1, 1, 1);

    /// <summary>
    /// Defines an instance with all components set to positive infinity.
    /// </summary>
    public static readonly Vector3 PositiveInfinity = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);

    /// <summary>
    /// Defines an instance with all components set to negative infinity.
    /// </summary>
    public static readonly Vector3 NegativeInfinity = new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);

    /// <summary>
    /// Defines the size of the Vector3 struct in bytes.
    /// </summary>
    public static readonly int SizeInBytes = Marshal.SizeOf<Vector3>();

    /// <summary>
    /// Adds two vectors.
    /// </summary>
    /// <param name="a">Left operand.</param>
    /// <param name="b">Right operand.</param>
    /// <returns>Result of operation.</returns>
    [Pure]
    public static Vector3 Add(Vector3 a, Vector3 b)
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
    public static void Add(in Vector3 a, in Vector3 b, out Vector3 result)
    {
        result.x = a.x + b.x;
        result.y = a.y + b.y;
        result.z = a.z + b.z;
    }

    /// <summary>
    /// Subtract one Vector from another.
    /// </summary>
    /// <param name="a">First operand.</param>
    /// <param name="b">Second operand.</param>
    /// <returns>Result of subtraction.</returns>
    [Pure]
    public static Vector3 Subtract(Vector3 a, Vector3 b)
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
    public static void Subtract(in Vector3 a, in Vector3 b, out Vector3 result)
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
    public static Vector3 Multiply(Vector3 vector, float scale)
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
    public static void Multiply(in Vector3 vector, float scale, out Vector3 result)
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
    public static Vector3 Multiply(Vector3 vector, Vector3 scale)
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
    public static void Multiply(in Vector3 vector, in Vector3 scale, out Vector3 result)
    {
        result.x = vector.x * scale.x;
        result.y = vector.y * scale.y;
        result.z = vector.z * scale.z;
    }

    /// <summary>
    /// Divides a vector by a scalar.
    /// </summary>
    /// <param name="vector">Left operand.</param>
    /// <param name="scale">Right operand.</param>
    /// <returns>Result of the operation.</returns>
    [Pure]
    public static Vector3 Divide(Vector3 vector, float scale)
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
    public static void Divide(in Vector3 vector, float scale, out Vector3 result)
    {
        result.x = vector.x / scale;
        result.y = vector.y / scale;
        result.z = vector.z / scale;
    }

    /// <summary>
    /// Divides a vector by the components of a vector (scale).
    /// </summary>
    /// <param name="vector">Left operand.</param>
    /// <param name="scale">Right operand.</param>
    /// <returns>Result of the operation.</returns>
    [Pure]
    public static Vector3 Divide(Vector3 vector, Vector3 scale)
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
    public static void Divide(in Vector3 vector, in Vector3 scale, out Vector3 result)
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
    public static Vector3 ComponentMin(Vector3 a, Vector3 b)
    {
        a.x = a.x < b.x ? a.x : b.x;
        a.y = a.y < b.y ? a.y : b.y;
        a.z = a.z < b.z ? a.z : b.z;
        return a;
    }

    /// <summary>
    /// Returns a vector created from the smallest of the corresponding components of the given vectors.
    /// </summary>
    /// <param name="a">First operand.</param>
    /// <param name="b">Second operand.</param>
    /// <param name="result">The component-wise minimum.</param>
    public static void ComponentMin(in Vector3 a, in Vector3 b, out Vector3 result)
    {
        result.x = a.x < b.x ? a.x : b.x;
        result.y = a.y < b.y ? a.y : b.y;
        result.z = a.z < b.z ? a.z : b.z;
    }

    /// <summary>
    /// Returns a vector created from the largest of the corresponding components of the given vectors.
    /// </summary>
    /// <param name="a">First operand.</param>
    /// <param name="b">Second operand.</param>
    /// <returns>The component-wise maximum.</returns>
    [Pure]
    public static Vector3 ComponentMax(Vector3 a, Vector3 b)
    {
        a.x = a.x > b.x ? a.x : b.x;
        a.y = a.y > b.y ? a.y : b.y;
        a.z = a.z > b.z ? a.z : b.z;
        return a;
    }

    /// <summary>
    /// Returns a vector created from the largest of the corresponding components of the given vectors.
    /// </summary>
    /// <param name="a">First operand.</param>
    /// <param name="b">Second operand.</param>
    /// <param name="result">The component-wise maximum.</param>
    public static void ComponentMax(in Vector3 a, in Vector3 b, out Vector3 result)
    {
        result.x = a.x > b.x ? a.x : b.x;
        result.y = a.y > b.y ? a.y : b.y;
        result.z = a.z > b.z ? a.z : b.z;
    }

    /// <summary>
    /// Returns the Vector3 with the minimum magnitude. If the magnitudes are equal, the second vector
    /// is selected.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>The minimum Vector3.</returns>
    [Pure]
    public static Vector3 MagnitudeMin(Vector3 left, Vector3 right)
    {
        return left.LengthSquared < right.LengthSquared ? left : right;
    }

    /// <summary>
    /// Returns the Vector3 with the minimum magnitude. If the magnitudes are equal, the second vector
    /// is selected.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <param name="result">The magnitude-wise minimum.</param>
    public static void MagnitudeMin(in Vector3 left, in Vector3 right, out Vector3 result)
    {
        result = left.LengthSquared < right.LengthSquared ? left : right;
    }

    /// <summary>
    /// Returns the Vector3 with the maximum magnitude. If the magnitudes are equal, the first vector
    /// is selected.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>The maximum Vector3.</returns>
    [Pure]
    public static Vector3 MagnitudeMax(Vector3 left, Vector3 right)
    {
        return left.LengthSquared >= right.LengthSquared ? left : right;
    }

    /// <summary>
    /// Returns the Vector3 with the maximum magnitude. If the magnitudes are equal, the first vector
    /// is selected.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <param name="result">The magnitude-wise maximum.</param>
    public static void MagnitudeMax(in Vector3 left, in Vector3 right, out Vector3 result)
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
    public static Vector3 Clamp(Vector3 vec, Vector3 min, Vector3 max)
    {
        vec.x = vec.x < min.x ? min.x : vec.x > max.x ? max.x : vec.x;
        vec.y = vec.y < min.y ? min.y : vec.y > max.y ? max.y : vec.y;
        vec.z = vec.z < min.z ? min.z : vec.z > max.z ? max.z : vec.z;
        return vec;
    }

    /// <summary>
    /// Clamp a vector to the given minimum and maximum vectors.
    /// </summary>
    /// <param name="vec">Input vector.</param>
    /// <param name="min">Minimum vector.</param>
    /// <param name="max">Maximum vector.</param>
    /// <param name="result">The clamped vector.</param>
    public static void Clamp(in Vector3 vec, in Vector3 min, in Vector3 max, out Vector3 result)
    {
        result.x = vec.x < min.x ? min.x : vec.x > max.x ? max.x : vec.x;
        result.y = vec.y < min.y ? min.y : vec.y > max.y ? max.y : vec.y;
        result.z = vec.z < min.z ? min.z : vec.z > max.z ? max.z : vec.z;
    }

    /// <summary>
    /// Take the component-wise absolute value of a vector.
    /// </summary>
    /// <param name="vec">The vector to apply component-wise absolute value to.</param>
    /// <returns>The component-wise absolute value vector.</returns>
    [Pure]
    public static Vector3 Abs(Vector3 vec)
    {
        vec.x = MathF.Abs(vec.x);
        vec.y = MathF.Abs(vec.y);
        vec.z = MathF.Abs(vec.z);
        return vec;
    }

    /// <summary>
    /// Take the component-wise absolute value of a vector.
    /// </summary>
    /// <param name="vec">The vector to apply component-wise absolute value to.</param>
    /// <param name="result">The component-wise absolute value vector.</param>
    public static void Abs(in Vector3 vec, out Vector3 result)
    {
        result.x = MathF.Abs(vec.x);
        result.y = MathF.Abs(vec.y);
        result.z = MathF.Abs(vec.z);
    }

    /// <summary>
    /// Component-wise rounding. Equivalent to calling <see cref="MathF.Round(float)"/> on each component.
    /// </summary>
    /// <param name="vec">The vector to round.</param>
    /// <returns>The component-wise rounded vector.</returns>
    [Pure]
    public static Vector3 Round(Vector3 vec)
    {
        vec.x = MathF.Round(vec.x);
        vec.y = MathF.Round(vec.y);
        vec.z = MathF.Round(vec.z);
        return vec;
    }

    /// <summary>
    /// Component-wise rounding. Equivalent to calling <see cref="MathF.Round(float)"/> on each component.
    /// </summary>
    /// <param name="vec">The vector to round.</param>
    /// <param name="result">The component-wise rounded vector.</param>
    public static void Round(in Vector3 vec, out Vector3 result)
    {
        result.x = MathF.Round(vec.x);
        result.y = MathF.Round(vec.y);
        result.z = MathF.Round(vec.z);
    }

    /// <summary>
    /// Component-wise rounding with specified midpoint rounding rule.
    /// Equivalent to calling <see cref="MathF.Round(float,MidpointRounding)"/> on each component.
    /// </summary>
    /// <param name="vec">The vector to round.</param>
    /// <param name="rounding">The midpoint rounding rule to use.</param>
    /// <returns>The component-wise rounded vector.</returns>
    [Pure]
    public static Vector3 Round(Vector3 vec, MidpointRounding rounding)
    {
        vec.x = MathF.Round(vec.x, rounding);
        vec.y = MathF.Round(vec.y, rounding);
        vec.z = MathF.Round(vec.z, rounding);
        return vec;
    }

    /// <summary>
    /// Component-wise rounding with specified midpoint rounding rule.
    /// Equivalent to calling <see cref="MathF.Round(float,MidpointRounding)"/> on each component.
    /// </summary>
    /// <param name="vec">The vector to round.</param>
    /// <param name="rounding">The midpoint rounding rule to use.</param>
    /// <param name="result">The component-wise rounded vector.</param>
    public static void Round(in Vector3 vec, MidpointRounding rounding, out Vector3 result)
    {
        result.x = MathF.Round(vec.x, rounding);
        result.y = MathF.Round(vec.y, rounding);
        result.z = MathF.Round(vec.z, rounding);
    }

    /// <summary>
    /// Component-wise ceiling operation.
    /// Equivalent to calling <see cref="MathF.Ceiling(float)"/> on each component.
    /// </summary>
    /// <param name="vec">The vector to take the ceiling of.</param>
    /// <returns>The component-wise ceiling vector.</returns>
    [Pure]
    public static Vector3 Ceiling(Vector3 vec)
    {
        vec.x = MathF.Ceiling(vec.x);
        vec.y = MathF.Ceiling(vec.y);
        vec.z = MathF.Ceiling(vec.z);
        return vec;
    }

    /// <summary>
    /// Component-wise ceiling operation.
    /// Equivalent to calling <see cref="MathF.Ceiling(float)"/> on each component.
    /// </summary>
    /// <param name="vec">The vector to take the ceiling of.</param>
    /// <param name="result">The component-wise ceiling vector.</param>
    public static void Ceiling(in Vector3 vec, out Vector3 result)
    {
        result.x = MathF.Ceiling(vec.x);
        result.y = MathF.Ceiling(vec.y);
        result.z = MathF.Ceiling(vec.z);
    }

    /// <summary>
    /// Component-wise floor operation.
    /// Equivalent to calling <see cref="MathF.Floor(float)"/> on each component.
    /// </summary>
    /// <param name="vec">The vector to take the floor of.</param>
    /// <returns>The component-wise floored vector.</returns>
    [Pure]
    public static Vector3 Floor(Vector3 vec)
    {
        vec.x = MathF.Floor(vec.x);
        vec.y = MathF.Floor(vec.y);
        vec.z = MathF.Floor(vec.z);
        return vec;
    }

    /// <summary>
    /// Component-wise floor operation.
    /// Equivalent to calling <see cref="MathF.Floor(float)"/> on each component.
    /// </summary>
    /// <param name="vec">The vector to take the floor of.</param>
    /// <param name="result">The component-wise floored vector.</param>
    public static void Floor(in Vector3 vec, out Vector3 result)
    {
        result.x = MathF.Floor(vec.x);
        result.y = MathF.Floor(vec.y);
        result.z = MathF.Floor(vec.z);
    }

    /// <summary>
    /// Component-wise truncation.
    /// Equivalent to calling <see cref="MathF.Truncate(float)"/> on each component.
    /// </summary>
    /// <param name="vec">The vector to truncate.</param>
    /// <returns>The component-wise truncated vector.</returns>
    [Pure]
    public static Vector3 Truncate(Vector3 vec)
    {
        vec.x = MathF.Truncate(vec.x);
        vec.y = MathF.Truncate(vec.y);
        vec.z = MathF.Truncate(vec.z);
        return vec;
    }

    /// <summary>
    /// Component-wise truncation.
    /// Equivalent to calling <see cref="MathF.Truncate(float)"/> on each component.
    /// </summary>
    /// <param name="vec">The vector to truncate.</param>
    /// <param name="result">The component-wise truncated vector.</param>
    public static void Truncate(in Vector3 vec, out Vector3 result)
    {
        result.x = MathF.Truncate(vec.x);
        result.y = MathF.Truncate(vec.y);
        result.z = MathF.Truncate(vec.z);
    }

    /// <summary>
    /// Compute the euclidean distance between two vectors.
    /// </summary>
    /// <param name="vec1">The first vector.</param>
    /// <param name="vec2">The second vector.</param>
    /// <returns>The distance.</returns>
    [Pure]
    public static float Distance(Vector3 vec1, Vector3 vec2)
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
    public static void Distance(in Vector3 vec1, in Vector3 vec2, out float result)
    {
        result = MathF.Sqrt(((vec2.x - vec1.x) * (vec2.x - vec1.x)) + ((vec2.y - vec1.y) * (vec2.y - vec1.y)) + ((vec2.z - vec1.z) * (vec2.z - vec1.z)));
    }

    /// <summary>
    /// Compute the squared euclidean distance between two vectors.
    /// </summary>
    /// <param name="vec1">The first vector.</param>
    /// <param name="vec2">The second vector.</param>
    /// <returns>The squared distance.</returns>
    [Pure]
    public static float DistanceSquared(Vector3 vec1, Vector3 vec2)
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
    public static void DistanceSquared(in Vector3 vec1, in Vector3 vec2, out float result)
    {
        result = ((vec2.x - vec1.x) * (vec2.x - vec1.x)) + ((vec2.y - vec1.y) * (vec2.y - vec1.y)) + ((vec2.z - vec1.z) * (vec2.z - vec1.z));
    }

    /// <summary>
    /// Scale a vector to unit length.
    /// </summary>
    /// <param name="vec">The input vector.</param>
    /// <returns>The normalized copy.</returns>
    [Pure]
    public static Vector3 Normalize(Vector3 vec)
    {
        float scale = 1.0f / vec.Length;
        vec.x *= scale;
        vec.y *= scale;
        vec.z *= scale;
        return vec;
    }

    /// <summary>
    /// Scale a vector to unit length.
    /// </summary>
    /// <param name="vec">The input vector.</param>
    /// <param name="result">The normalized vector.</param>
    public static void Normalize(in Vector3 vec, out Vector3 result)
    {
        float scale = 1.0f / vec.Length;
        result.x = vec.x * scale;
        result.y = vec.y * scale;
        result.z = vec.z * scale;
    }

    /// <summary>
    /// Scale a vector to approximately unit length.
    /// </summary>
    /// <param name="vec">The input vector.</param>
    /// <returns>The normalized copy.</returns>
    [Pure]
    public static Vector3 NormalizeFast(Vector3 vec)
    {
        float scale = Mathf.InverseSqrtFast((vec.x * vec.x) + (vec.y * vec.y) + (vec.z * vec.z));
        vec.x *= scale;
        vec.y *= scale;
        vec.z *= scale;
        return vec;
    }

    /// <summary>
    /// Scale a vector to approximately unit length.
    /// </summary>
    /// <param name="vec">The input vector.</param>
    /// <param name="result">The normalized vector.</param>
    public static void NormalizeFast(in Vector3 vec, out Vector3 result)
    {
        float scale = Mathf.InverseSqrtFast((vec.x * vec.x) + (vec.y * vec.y) + (vec.z * vec.z));
        result.x = vec.x * scale;
        result.y = vec.y * scale;
        result.z = vec.z * scale;
    }

    /// <summary>
    /// Calculate the dot (scalar) product of two vectors.
    /// </summary>
    /// <param name="left">First operand.</param>
    /// <param name="right">Second operand.</param>
    /// <returns>The dot product of the two inputs.</returns>
    [Pure]
    public static float Dot(Vector3 left, Vector3 right)
    {
        return (left.x * right.x) + (left.y * right.y) + (left.z * right.z);
    }

    /// <summary>
    /// Calculate the dot (scalar) product of two vectors.
    /// </summary>
    /// <param name="left">First operand.</param>
    /// <param name="right">Second operand.</param>
    /// <param name="result">The dot product of the two inputs.</param>
    public static void Dot(in Vector3 left, in Vector3 right, out float result)
    {
        result = (left.x * right.x) + (left.y * right.y) + (left.z * right.z);
    }

    /// <summary>
    /// Caclulate the cross (vector) product of two vectors.
    /// </summary>
    /// <param name="left">First operand.</param>
    /// <param name="right">Second operand.</param>
    /// <returns>The cross product of the two inputs.</returns>
    [Pure]
    public static Vector3 Cross(Vector3 left, Vector3 right)
    {
        Cross(in left, in right, out Vector3 result);
        return result;
    }

    /// <summary>
    /// Caclulate the cross (vector) product of two vectors.
    /// </summary>
    /// <param name="left">First operand.</param>
    /// <param name="right">Second operand.</param>
    /// <param name="result">The cross product of the two inputs.</param>
    public static void Cross(in Vector3 left, in Vector3 right, out Vector3 result)
    {
        result.x = (left.y * right.z) - (left.z * right.y);
        result.y = (left.z * right.x) - (left.x * right.z);
        result.z = (left.x * right.y) - (left.y * right.x);
    }

    /// <summary>
    /// Returns a new vector that is the linear blend of the 2 given vectors.
    /// </summary>
    /// <param name="a">First input vector.</param>
    /// <param name="b">Second input vector.</param>
    /// <param name="blend">The blend factor.</param>
    /// <returns>a when blend=0, b when blend=1, and a linear combination otherwise.</returns>
    [Pure]
    public static Vector3 Lerp(Vector3 a, Vector3 b, float blend)
    {
        a.x = (blend * (b.x - a.x)) + a.x;
        a.y = (blend * (b.y - a.y)) + a.y;
        a.z = (blend * (b.z - a.z)) + a.z;
        return a;
    }

    /// <summary>
    /// Returns a new vector that is the linear blend of the 2 given vectors.
    /// </summary>
    /// <param name="a">First input vector.</param>
    /// <param name="b">Second input vector.</param>
    /// <param name="blend">The blend factor.</param>
    /// <param name="result">a when blend=0, b when blend=1, and a linear combination otherwise.</param>
    public static void Lerp(in Vector3 a, in Vector3 b, float blend, out Vector3 result)
    {
        result.x = (blend * (b.x - a.x)) + a.x;
        result.y = (blend * (b.y - a.y)) + a.y;
        result.z = (blend * (b.z - a.z)) + a.z;
    }

    /// <summary>
    /// Returns a new vector that is the component-wise linear blend of the 2 given vectors.
    /// </summary>
    /// <param name="a">First input vector.</param>
    /// <param name="b">Second input vector.</param>
    /// <param name="blend">The blend factor.</param>
    /// <returns>a when blend=0, b when blend=1, and a component-wise linear combination otherwise.</returns>
    [Pure]
    public static Vector3 Lerp(Vector3 a, Vector3 b, Vector3 blend)
    {
        a.x = (blend.x * (b.x - a.x)) + a.x;
        a.y = (blend.y * (b.y - a.y)) + a.y;
        a.z = (blend.z * (b.z - a.z)) + a.z;
        return a;
    }

    /// <summary>
    /// Returns a new vector that is the component-wise linear blend of the 2 given vectors.
    /// </summary>
    /// <param name="a">First input vector.</param>
    /// <param name="b">Second input vector.</param>
    /// <param name="blend">The blend factor.</param>
    /// <param name="result">a when blend=0, b when blend=1, and a component-wise linear combination otherwise.</param>
    public static void Lerp(in Vector3 a, in Vector3 b, Vector3 blend, out Vector3 result)
    {
        result.x = (blend.x * (b.x - a.x)) + a.x;
        result.y = (blend.y * (b.y - a.y)) + a.y;
        result.z = (blend.z * (b.z - a.z)) + a.z;
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
    public static Vector3 Slerp(Vector3 a, Vector3 b, float t)
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
    public static void Slerp(in Vector3 a, in Vector3 b, float t, out Vector3 result)
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
    [Pure]
    public static Vector3 Elerp(Vector3 a, Vector3 b, float t)
    {
        a.x = MathF.Pow(a.x, 1 - t) * MathF.Pow(b.x, t);
        a.y = MathF.Pow(a.y, 1 - t) * MathF.Pow(b.y, t);
        a.z = MathF.Pow(a.z, 1 - t) * MathF.Pow(b.z, t);

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
    public static void Elerp(in Vector3 a, in Vector3 b, float t, out Vector3 result)
    {
        result.x = MathF.Pow(a.x, 1 - t) * MathF.Pow(b.x, t);
        result.y = MathF.Pow(a.y, 1 - t) * MathF.Pow(b.y, t);
        result.z = MathF.Pow(a.z, 1 - t) * MathF.Pow(b.z, t);
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
    public static Vector3 BaryCentric(Vector3 a, Vector3 b, Vector3 c, float u, float v)
    {
        BaryCentric(in a, in b, in c, u, v, out Vector3 result);
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
    [Pure]
    public static void BaryCentric
    (
        in Vector3 a,
        in Vector3 b,
        in Vector3 c,
        float u,
        float v,
        out Vector3 result
    )
    {
        Subtract(in b, in a, out Vector3 ab);
        Multiply(in ab, u, out Vector3 abU);
        Add(in a, in abU, out Vector3 uPos);

        Subtract(in c, in a, out Vector3 ac);
        Multiply(in ac, v, out Vector3 acV);
        Add(in uPos, in acV, out result);
    }

    /// <summary>
    /// Transform a direction vector by the given Matrix.
    /// Assumes the matrix has a bottom row of (0,0,0,1), that is the translation part is ignored.
    /// </summary>
    /// <param name="vec">The vector to transform.</param>
    /// <param name="mat">The desired transformation.</param>
    /// <returns>The transformed vector.</returns>
    [Pure]
    public static Vector3 TransformVector(Vector3 vec, Matrix4x4 mat)
    {
        TransformVector(in vec, in mat, out Vector3 result);
        return result;
    }

    /// <summary>
    /// Transform a direction vector by the given Matrix.
    /// Assumes the matrix has a bottom row of (0,0,0,1), that is the translation part is ignored.
    /// </summary>
    /// <param name="vec">The vector to transform.</param>
    /// <param name="mat">The desired transformation.</param>
    /// <param name="result">The transformed vector.</param>
    public static void TransformVector(in Vector3 vec, in Matrix4x4 mat, out Vector3 result)
    {
        result.x = (vec.x * mat.Row0.x) +
                   (vec.y * mat.Row1.x) +
                   (vec.z * mat.Row2.x);

        result.y = (vec.x * mat.Row0.y) +
                   (vec.y * mat.Row1.y) +
                   (vec.z * mat.Row2.y);

        result.z = (vec.x * mat.Row0.z) +
                   (vec.y * mat.Row1.z) +
                   (vec.z * mat.Row2.z);
    }

    /// <summary>
    /// Transform a Normal by the given Matrix.
    /// </summary>
    /// <remarks>
    /// This calculates the inverse of the given matrix, use TransformNormalInverse if you
    /// already have the inverse to avoid this extra calculation.
    /// </remarks>
    /// <param name="norm">The normal to transform.</param>
    /// <param name="mat">The desired transformation.</param>
    /// <returns>The transformed normal.</returns>
    [Pure]
    public static Vector3 TransformNormal(Vector3 norm, Matrix4x4 mat)
    {
        TransformNormal(in norm, in mat, out Vector3 result);
        return result;
    }

    /// <summary>
    /// Transform a Normal by the given Matrix.
    /// </summary>
    /// <remarks>
    /// This calculates the inverse of the given matrix, use TransformNormalInverse if you
    /// already have the inverse to avoid this extra calculation.
    /// </remarks>
    /// <param name="norm">The normal to transform.</param>
    /// <param name="mat">The desired transformation.</param>
    /// <param name="result">The transformed normal.</param>
    public static void TransformNormal(in Vector3 norm, in Matrix4x4 mat, out Vector3 result)
    {
        Matrix4x4 inverse = Matrix4x4.Invert(mat);
        TransformNormalInverse(in norm, in inverse, out result);
    }

    /// <summary>
    /// Transform a Normal by the (transpose of the) given Matrix.
    /// </summary>
    /// <remarks>
    /// This version doesn't calculate the inverse matrix.
    /// Use this version if you already have the inverse of the desired transform to hand.
    /// </remarks>
    /// <param name="norm">The normal to transform.</param>
    /// <param name="invMat">The inverse of the desired transformation.</param>
    /// <returns>The transformed normal.</returns>
    [Pure]
    public static Vector3 TransformNormalInverse(Vector3 norm, Matrix4x4 invMat)
    {
        TransformNormalInverse(in norm, in invMat, out Vector3 result);
        return result;
    }

    /// <summary>
    /// Transform a Normal by the (transpose of the) given Matrix.
    /// </summary>
    /// <remarks>
    /// This version doesn't calculate the inverse matrix.
    /// Use this version if you already have the inverse of the desired transform to hand.
    /// </remarks>
    /// <param name="norm">The normal to transform.</param>
    /// <param name="invMat">The inverse of the desired transformation.</param>
    /// <param name="result">The transformed normal.</param>
    public static void TransformNormalInverse(in Vector3 norm, in Matrix4x4 invMat, out Vector3 result)
    {
        result.x = (norm.x * invMat.Row0.x) +
                   (norm.y * invMat.Row0.y) +
                   (norm.z * invMat.Row0.z);

        result.y = (norm.x * invMat.Row1.x) +
                   (norm.y * invMat.Row1.y) +
                   (norm.z * invMat.Row1.z);

        result.z = (norm.x * invMat.Row2.x) +
                   (norm.y * invMat.Row2.y) +
                   (norm.z * invMat.Row2.z);
    }

    /// <summary>
    /// Transform a Position by the given Matrix.
    /// </summary>
    /// <param name="pos">The position to transform.</param>
    /// <param name="mat">The desired transformation.</param>
    /// <returns>The transformed position.</returns>
    [Pure]
    public static Vector3 TransformPosition(Vector3 pos, Matrix4x4 mat)
    {
        TransformPosition(in pos, in mat, out Vector3 result);
        return result;
    }

    /// <summary>
    /// Transform a Position by the given Matrix.
    /// </summary>
    /// <param name="pos">The position to transform.</param>
    /// <param name="mat">The desired transformation.</param>
    /// <param name="result">The transformed position.</param>
    public static void TransformPosition(in Vector3 pos, in Matrix4x4 mat, out Vector3 result)
    {
        result.x = (pos.x * mat.Row0.x) +
                   (pos.y * mat.Row1.x) +
                   (pos.z * mat.Row2.x) +
                   mat.Row3.x;

        result.y = (pos.x * mat.Row0.y) +
                   (pos.y * mat.Row1.y) +
                   (pos.z * mat.Row2.y) +
                   mat.Row3.y;

        result.z = (pos.x * mat.Row0.z) +
                   (pos.y * mat.Row1.z) +
                   (pos.z * mat.Row2.z) +
                   mat.Row3.z;
    }

    /// <summary>
    /// Transform a Vector by the given Matrix.
    /// </summary>
    /// <param name="vec">The vector to transform.</param>
    /// <param name="mat">The desired transformation.</param>
    /// <returns>The transformed vector.</returns>
    [Pure]
    public static Vector3 TransformRow(Vector3 vec, Matrix3x3 mat)
    {
        TransformRow(in vec, in mat, out Vector3 result);
        return result;
    }

    /// <summary>
    /// Transform a Vector by the given Matrix.
    /// </summary>
    /// <param name="vec">The vector to transform.</param>
    /// <param name="mat">The desired transformation.</param>
    /// <param name="result">The transformed vector.</param>
    public static void TransformRow(in Vector3 vec, in Matrix3x3 mat, out Vector3 result)
    {
        result.x = (vec.x * mat.Row0.x) + (vec.y * mat.Row1.x) + (vec.z * mat.Row2.x);
        result.y = (vec.x * mat.Row0.y) + (vec.y * mat.Row1.y) + (vec.z * mat.Row2.y);
        result.z = (vec.x * mat.Row0.z) + (vec.y * mat.Row1.z) + (vec.z * mat.Row2.z);
    }

    /// <summary>
    /// Transforms a vector by a quaternion rotation.
    /// </summary>
    /// <param name="vec">The vector to transform.</param>
    /// <param name="quat">The quaternion to rotate the vector by.</param>
    /// <returns>The result of the operation.</returns>
    [Pure]
    public static Vector3 Transform(Vector3 vec, Quaternion quat)
    {
        Transform(in vec, in quat, out Vector3 result);
        return result;
    }

    /// <summary>
    /// Transforms a vector by a quaternion rotation.
    /// </summary>
    /// <param name="vec">The vector to transform.</param>
    /// <param name="quat">The quaternion to rotate the vector by.</param>
    /// <param name="result">The result of the operation.</param>
    public static void Transform(in Vector3 vec, in Quaternion quat, out Vector3 result)
    {
        // Since vec.W == 0, we can optimize quat * vec * quat^-1 as follows:
        // vec + 2.0 * cross(quat.xyz, cross(quat.xyz, vec) + quat.w * vec)
        Vector3 xyz = quat.xyz;
        Cross(in xyz, in vec, out Vector3 temp);
        Multiply(in vec, quat.w, out Vector3 temp2);
        Add(in temp, in temp2, out temp);
        Cross(in xyz, in temp, out temp2);
        Multiply(in temp2, 2f, out temp2);
        Add(in vec, in temp2, out result);
    }

    /// <summary>
    /// Transform a Vector by the given Matrix using right-handed notation.
    /// </summary>
    /// <param name="mat">The desired transformation.</param>
    /// <param name="vec">The vector to transform.</param>
    /// <returns>The transformed vector.</returns>
    [Pure]
    public static Vector3 TransformColumn(Matrix3x3 mat, Vector3 vec)
    {
        TransformColumn(in mat, in vec, out Vector3 result);
        return result;
    }

    /// <summary>
    /// Transform a Vector by the given Matrix using right-handed notation.
    /// </summary>
    /// <param name="mat">The desired transformation.</param>
    /// <param name="vec">The vector to transform.</param>
    /// <param name="result">The transformed vector.</param>
    public static void TransformColumn(in Matrix3x3 mat, in Vector3 vec, out Vector3 result)
    {
        result.x = (mat.Row0.x * vec.x) + (mat.Row0.y * vec.y) + (mat.Row0.z * vec.z);
        result.y = (mat.Row1.x * vec.x) + (mat.Row1.y * vec.y) + (mat.Row1.z * vec.z);
        result.z = (mat.Row2.x * vec.x) + (mat.Row2.y * vec.y) + (mat.Row2.z * vec.z);
    }

    /// <summary>
    /// Transform a Vector3 by the given Matrix, and project the resulting Vector4 back to a Vector3.
    /// </summary>
    /// <param name="vec">The vector to transform.</param>
    /// <param name="mat">The desired transformation.</param>
    /// <returns>The transformed vector.</returns>
    [Pure]
    public static Vector3 TransformPerspective(Vector3 vec, Matrix4x4 mat)
    {
        TransformPerspective(in vec, in mat, out Vector3 result);
        return result;
    }

    /// <summary>
    /// Transform a Vector3 by the given Matrix, and project the resulting Vector4 back to a Vector3.
    /// </summary>
    /// <param name="vec">The vector to transform.</param>
    /// <param name="mat">The desired transformation.</param>
    /// <param name="result">The transformed vector.</param>
    public static void TransformPerspective(in Vector3 vec, in Matrix4x4 mat, out Vector3 result)
    {
        Vector4 v = new Vector4(vec.x, vec.y, vec.z, 1);
        Vector4.TransformRow(in v, in mat, out v);
        result.x = v.x / v.w;
        result.y = v.y / v.w;
        result.z = v.z / v.w;
    }

    /// <summary>
    /// Calculates the angle (in radians) between two vectors.
    /// </summary>
    /// <param name="first">The first vector.</param>
    /// <param name="second">The second vector.</param>
    /// <returns>Angle (in radians) between the vectors.</returns>
    /// <remarks>Note that the returned angle is never bigger than the constant Pi.</remarks>
    [Pure]
    public static float CalculateAngle(Vector3 first, Vector3 second)
    {
        CalculateAngle(in first, in second, out float result);
        return result;
    }

    /// <summary>
    /// Calculates the angle (in radians) between two vectors.
    /// </summary>
    /// <param name="first">The first vector.</param>
    /// <param name="second">The second vector.</param>
    /// <param name="result">Angle (in radians) between the vectors.</param>
    /// <remarks>Note that the returned angle is never bigger than the constant Pi.</remarks>
    public static void CalculateAngle(in Vector3 first, in Vector3 second, out float result)
    {
        Dot(in first, in second, out float temp);
        result = MathF.Acos(Mathf.Clamp(temp / (first.Length * second.Length), -1.0f, 1.0f));
    }

    /// <summary>
    /// Projects a vector from object space into screen space.
    /// </summary>
    /// <param name="vector">The vector to project.</param>
    /// <param name="x">The X coordinate of the viewport.</param>
    /// <param name="y">The Y coordinate of the viewport.</param>
    /// <param name="width">The width of the viewport.</param>
    /// <param name="height">The height of the viewport.</param>
    /// <param name="minZ">The minimum depth of the viewport.</param>
    /// <param name="maxZ">The maximum depth of the viewport.</param>
    /// <param name="worldViewProjection">The world-view-projection matrix.</param>
    /// <returns>The vector in screen space.</returns>
    /// <remarks>
    /// To project to normalized device coordinates (NDC) use the following parameters:
    /// Project(vector, -1, -1, 2, 2, -1, 1, worldViewProjection).
    /// </remarks>
    [Pure]
    public static Vector3 Project
    (
        Vector3 vector,
        float x,
        float y,
        float width,
        float height,
        float minZ,
        float maxZ,
        Matrix4x4 worldViewProjection
    )
    {
        Vector4 result;

        result.x =
            (vector.x * worldViewProjection.M11) +
            (vector.y * worldViewProjection.M21) +
            (vector.z * worldViewProjection.M31) +
            worldViewProjection.M41;

        result.y =
            (vector.x * worldViewProjection.M12) +
            (vector.y * worldViewProjection.M22) +
            (vector.z * worldViewProjection.M32) +
            worldViewProjection.M42;

        result.z =
            (vector.x * worldViewProjection.M13) +
            (vector.y * worldViewProjection.M23) +
            (vector.z * worldViewProjection.M33) +
            worldViewProjection.M43;

        result.w =
            (vector.x * worldViewProjection.M14) +
            (vector.y * worldViewProjection.M24) +
            (vector.z * worldViewProjection.M34) +
            worldViewProjection.M44;

        result /= result.w;

        result.x = x + (width * ((result.x + 1.0f) / 2.0f));
        result.y = y + (height * ((result.y + 1.0f) / 2.0f));
        result.z = minZ + ((maxZ - minZ) * ((result.z + 1.0f) / 2.0f));

        return new Vector3(result.x, result.y, result.z);
    }

    /// <summary>
    /// Projects a vector from screen space into object space.
    /// </summary>
    /// <param name="vector">The vector to project.</param>
    /// <param name="x">The X coordinate of the viewport.</param>
    /// <param name="y">The Y coordinate of the viewport.</param>
    /// <param name="width">The width of the viewport.</param>
    /// <param name="height">The height of the viewport.</param>
    /// <param name="minZ">The minimum depth of the viewport.</param>
    /// <param name="maxZ">The maximum depth of the viewport.</param>
    /// <param name="inverseWorldViewProjection">The inverse of the world-view-projection matrix.</param>
    /// <returns>The vector in object space.</returns>
    /// <remarks>
    /// To project from normalized device coordinates (NDC) use the following parameters:
    /// Project(vector, -1, -1, 2, 2, -1, 1, inverseWorldViewProjection).
    /// </remarks>
    [Pure]
    public static Vector3 Unproject
    (
        Vector3 vector,
        float x,
        float y,
        float width,
        float height,
        float minZ,
        float maxZ,
        Matrix4x4 inverseWorldViewProjection
    )
    {
        float tempX = ((vector.x - x) / width * 2.0f) - 1.0f;
        float tempY = ((vector.y - y) / height * 2.0f) - 1.0f;
        float tempZ = ((vector.z - minZ) / (maxZ - minZ) * 2.0f) - 1.0f;

        Vector3 result;
        result.x =
            (tempX * inverseWorldViewProjection.M11) +
            (tempY * inverseWorldViewProjection.M21) +
            (tempZ * inverseWorldViewProjection.M31) +
            inverseWorldViewProjection.M41;

        result.y =
            (tempX * inverseWorldViewProjection.M12) +
            (tempY * inverseWorldViewProjection.M22) +
            (tempZ * inverseWorldViewProjection.M32) +
            inverseWorldViewProjection.M42;

        result.z =
            (tempX * inverseWorldViewProjection.M13) +
            (tempY * inverseWorldViewProjection.M23) +
            (tempZ * inverseWorldViewProjection.M33) +
            inverseWorldViewProjection.M43;

        float tempW =
            (tempX * inverseWorldViewProjection.M14) +
            (tempY * inverseWorldViewProjection.M24) +
            (tempZ * inverseWorldViewProjection.M34) +
            inverseWorldViewProjection.M44;

        result /= tempW;

        return result;
    }

    /// <summary>
    /// Gets or sets an OpenTK.Vector2 with the X and Y components of this instance.
    /// </summary>
    [XmlIgnore]
    public Vector2 Xy
    {
        get => Unsafe.As<Vector3, Vector2>(ref this);
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
    /// Adds two instances.
    /// </summary>
    /// <param name="left">The first instance.</param>
    /// <param name="right">The second instance.</param>
    /// <returns>The result of the calculation.</returns>
    [Pure]
    public static Vector3 operator +(Vector3 left, Vector3 right)
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
    public static Vector3 operator -(Vector3 left, Vector3 right)
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
    public static Vector3 operator -(Vector3 vec)
    {
        vec.x = -vec.x;
        vec.y = -vec.y;
        vec.z = -vec.z;
        return vec;
    }

    /// <summary>
    /// Multiplies an instance by a scalar.
    /// </summary>
    /// <param name="vec">The instance.</param>
    /// <param name="scale">The scalar.</param>
    /// <returns>The result of the calculation.</returns>
    [Pure]
    public static Vector3 operator *(Vector3 vec, float scale)
    {
        vec.x *= scale;
        vec.y *= scale;
        vec.z *= scale;
        return vec;
    }

    /// <summary>
    /// Multiplies an instance by a scalar.
    /// </summary>
    /// <param name="scale">The scalar.</param>
    /// <param name="vec">The instance.</param>
    /// <returns>The result of the calculation.</returns>
    [Pure]
    public static Vector3 operator *(float scale, Vector3 vec)
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
    public static Vector3 operator *(Vector3 vec, Vector3 scale)
    {
        vec.x *= scale.x;
        vec.y *= scale.y;
        vec.z *= scale.z;
        return vec;
    }

    /// <summary>
    /// Transform a Vector by the given Matrix.
    /// </summary>
    /// <param name="vec">The vector to transform.</param>
    /// <param name="mat">The desired transformation.</param>
    /// <returns>The transformed vector.</returns>
    [Pure]
    public static Vector3 operator *(Vector3 vec, Matrix3x3 mat)
    {
        TransformRow(in vec, in mat, out Vector3 result);
        return result;
    }

    /// <summary>
    /// Transform a Vector by the given Matrix using right-handed notation.
    /// </summary>
    /// <param name="mat">The desired transformation.</param>
    /// <param name="vec">The vector to transform.</param>
    /// <returns>The transformed vector.</returns>
    [Pure]
    public static Vector3 operator *(Matrix3x3 mat, Vector3 vec)
    {
        TransformColumn(in mat, in vec, out Vector3 result);
        return result;
    }

    /// <summary>
    /// Transforms a vector by a quaternion rotation.
    /// </summary>
    /// <param name="vec">The vector to transform.</param>
    /// <param name="quat">The quaternion to rotate the vector by.</param>
    /// <returns>The multiplied vector.</returns>
    [Pure]
    public static Vector3 operator *(Quaternion quat, Vector3 vec)
    {
        Transform(in vec, in quat, out Vector3 result);
        return result;
    }

    /// <summary>
    /// Divides an instance by a scalar.
    /// </summary>
    /// <param name="vec">The instance.</param>
    /// <param name="scale">The scalar.</param>
    /// <returns>The result of the calculation.</returns>
    [Pure]
    public static Vector3 operator /(Vector3 vec, float scale)
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
    public static Vector3 operator /(Vector3 vec, Vector3 scale)
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
    public static bool operator ==(Vector3 left, Vector3 right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Compares two instances for inequality.
    /// </summary>
    /// <param name="left">The first instance.</param>
    /// <param name="right">The second instance.</param>
    /// <returns>True, if left does not equal right; false otherwise.</returns>
    public static bool operator !=(Vector3 left, Vector3 right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Converts OpenTK.Vector3 to OpenTK.Vector3i.
    /// </summary>
    /// <param name="vec">The Vector3 to convert.</param>
    /// <returns>The resulting Vector3i.</returns>
    [Pure]
    public static explicit operator Vector3Int(Vector3 vec)
    {
        return new Vector3Int((int)vec.x, (int)vec.y, (int)vec.z);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector3"/> struct using a tuple containing the component
    /// values.
    /// </summary>
    /// <param name="values">A tuple containing the component values.</param>
    /// <returns>A new instance of the <see cref="Vector3"/> struct with the given component values.</returns>
    [Pure]
    public static implicit operator Vector3((float X, float Y, float Z) values)
    {
        return new Vector3(values.X, values.Y, values.Z);
    }

    /// <summary>
    /// Converts <see cref="System.Numerics.Vector3"/> to <see cref="Vector3"/>.
    /// </summary>
    /// <param name="vec">The <see cref="System.Numerics.Vector3"/> to cast.</param>
    [Pure]
    public static explicit operator Vector3(System.Numerics.Vector3 vec)
    {
        return Unsafe.As<System.Numerics.Vector3, Vector3>(ref vec);
    }

    /// <summary>
    /// Converts <see cref="Vector3"/> to <see cref="System.Numerics.Vector3"/>.
    /// </summary>
    /// <param name="vec">The <see cref="Vector3"/> to cast.</param>
    [Pure]
    public static explicit operator System.Numerics.Vector3(Vector3 vec)
    {
        return Unsafe.As<Vector3, System.Numerics.Vector3>(ref vec);
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
            "({0}{3} {1}{3} {2})",
            x.ToString(format, formatProvider),
            y.ToString(format, formatProvider),
            z.ToString(format, formatProvider),
            Mathf.GetListSeparator(formatProvider));
    }

    /// <inheritdoc />
    public override readonly bool Equals(object obj)
    {
        return obj is Vector3 && Equals((Vector3)obj);
    }

    /// <inheritdoc />
    public readonly bool Equals(Vector3 other)
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
    public readonly void Deconstruct(out float x, out float y, out float z)
    {
        x = this.x;
        y = this.y;
        z = this.z;
    }
}
