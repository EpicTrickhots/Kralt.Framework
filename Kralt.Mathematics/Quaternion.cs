using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace Kralt.Mathematics;

/// <summary>
/// Represents a Quaternion.
/// </summary>
[Serializable, StructLayout(LayoutKind.Sequential)]
public struct Quaternion : IEquatable<Quaternion>, IFormattable
{
    /// <summary>
    /// The X, Y and Z components of this instance.
    /// </summary>
    public Vector3 xyz;

    /// <summary>
    /// The W component of this instance.
    /// </summary>
    public float w;

    /// <summary>
    /// Initializes a new instance of the <see cref="Quaternion"/> struct.
    /// </summary>
    /// <param name="v">The vector part.</param>
    /// <param name="w">The w part.</param>
    public Quaternion(Vector3 v, float w)
    {
        xyz = v;
        this.w = w;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Quaternion"/> struct.
    /// </summary>
    /// <param name="x">The x component.</param>
    /// <param name="y">The y component.</param>
    /// <param name="z">The z component.</param>
    /// <param name="w">The w component.</param>
    public Quaternion(float x, float y, float z, float w)
        : this(new Vector3(x, y, z), w)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Quaternion"/> struct from given Euler angles in radians.
    /// The rotations will get applied in following order:
    /// 1. around X axis, 2. around Y axis, 3. around Z axis.
    /// </summary>
    /// <param name="rotationX">Counterclockwise rotation around X axis in radian.</param>
    /// <param name="rotationY">Counterclockwise rotation around Y axis in radian.</param>
    /// <param name="rotationZ">Counterclockwise rotation around Z axis in radian.</param>
    public Quaternion(float rotationX, float rotationY, float rotationZ)
    {
        rotationX *= 0.5f;
        rotationY *= 0.5f;
        rotationZ *= 0.5f;

        var c1 = MathF.Cos(rotationX);
        var c2 = MathF.Cos(rotationY);
        var c3 = MathF.Cos(rotationZ);
        var s1 = MathF.Sin(rotationX);
        var s2 = MathF.Sin(rotationY);
        var s3 = MathF.Sin(rotationZ);

        w = (c1 * c2 * c3) - (s1 * s2 * s3);
        xyz.x = (s1 * c2 * c3) + (c1 * s2 * s3);
        xyz.y = (c1 * s2 * c3) - (s1 * c2 * s3);
        xyz.z = (c1 * c2 * s3) + (s1 * s2 * c3);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Quaternion"/> struct from given Euler angles in radians.
    /// The rotations will get applied in following order:
    /// 1. Around X, 2. Around Y, 3. Around Z.
    /// </summary>
    /// <param name="eulerAngles">The counterclockwise euler angles as a Vector3.</param>
    public Quaternion(Vector3 eulerAngles)
        : this(eulerAngles.x, eulerAngles.y, eulerAngles.z)
    {
    }

    /// <summary>
    /// Gets or sets the X component of this instance.
    /// </summary>
    [XmlIgnore]
    public float X
    {
        readonly get => xyz.x;
        set => xyz.x = value;
    }

    /// <summary>
    /// Gets or sets the Y component of this instance.
    /// </summary>
    [XmlIgnore]
    public float Y
    {
        readonly get => xyz.y;
        set => xyz.y = value;
    }

    /// <summary>
    /// Gets or sets the Z component of this instance.
    /// </summary>
    [XmlIgnore]
    public float Z
    {
        readonly get => xyz.z;
        set => xyz.z = value;
    }

    /// <summary>
    /// Convert the current quaternion to axis angle representation.
    /// </summary>
    /// <param name="axis">The resultant axis.</param>
    /// <param name="angle">The resultant angle.</param>
    public void ToAxisAngle(out Vector3 axis, out float angle)
    {
        var result = ToAxisAngle();
        axis = result.Xyz;
        angle = result.w;
    }

    /// <summary>
    /// Convert this instance to an axis-angle representation.
    /// </summary>
    /// <returns>A Vector4 that is the axis-angle representation of this quaternion.</returns>
    public Vector4 ToAxisAngle()
    {
        var q = this;
        if (Math.Abs(q.w) > 1.0f)
        {
            q.Normalize();
        }

        var result = new Vector4
        {
            w = 2.0f * MathF.Acos(q.w) // angle
        };

        var den = MathF.Sqrt(1.0f - (q.w * q.w));
        if (den > 0.0001f)
        {
            result.Xyz = q.xyz / den;
        }
        else
        {
            // This occurs when the angle is zero.
            // Not a problem: just set an arbitrary normalized axis.
            result.Xyz = Vector3.UnitX;
        }

        return result;
    }

    /// <summary>
    /// Convert the current quaternion to Euler angle representation.
    /// </summary>
    /// <param name="angles">The Euler angles in radians.</param>
    public readonly void ToEulerAngles(out Vector3 angles)
    {
        angles = ToEulerAngles();
    }

    /// <summary>
    /// Convert this instance to an Euler angle representation.
    /// </summary>
    /// <returns>The Euler angles in radians.</returns>
    public readonly Vector3 ToEulerAngles()
    {
        /*
        reference
        http://en.wikipedia.org/wiki/Conversion_between_quaternions_and_Euler_angles
        http://www.euclideanspace.com/maths/geometry/rotations/conversions/quaternionToEuler/
        */

        var q = this;

        Vector3 eulerAngles;

        // Threshold for the singularities found at the north/south poles.
        const float SINGULARITY_THRESHOLD = 0.4999995f;

        var sqw = q.w * q.w;
        var sqx = q.X * q.X;
        var sqy = q.Y * q.Y;
        var sqz = q.Z * q.Z;
        var unit = sqx + sqy + sqz + sqw; // if normalised is one, otherwise is correction factor
        var singularityTest = (q.X * q.Z) + (q.w * q.Y);

        if (singularityTest > SINGULARITY_THRESHOLD * unit)
        {
            eulerAngles.z = 2 * MathF.Atan2(q.X, q.w);
            eulerAngles.y = Mathf.PiOver2;
            eulerAngles.x = 0;
        }
        else if (singularityTest < -SINGULARITY_THRESHOLD * unit)
        {
            eulerAngles.z = -2 * MathF.Atan2(q.X, q.w);
            eulerAngles.y = -Mathf.PiOver2;
            eulerAngles.x = 0;
        }
        else
        {
            eulerAngles.z = MathF.Atan2(2 * ((q.w * q.Z) - (q.X * q.Y)), sqw + sqx - sqy - sqz);
            eulerAngles.y = MathF.Asin(2 * singularityTest / unit);
            eulerAngles.x = MathF.Atan2(2 * ((q.w * q.X) - (q.Y * q.Z)), sqw - sqx - sqy + sqz);
        }

        return eulerAngles;
    }

    /// <summary>
    /// Gets the length (magnitude) of the quaternion.
    /// </summary>
    /// <seealso cref="LengthSquared"/>
    public readonly float Length => MathF.Sqrt((w * w) + xyz.LengthSquared);

    /// <summary>
    /// Gets the square of the quaternion length (magnitude).
    /// </summary>
    public readonly float LengthSquared => (w * w) + xyz.LengthSquared;

    /// <summary>
    /// Returns a copy of the Quaternion scaled to unit length.
    /// </summary>
    /// <returns>The normalized copy.</returns>
    public readonly Quaternion Normalized()
    {
        var q = this;
        q.Normalize();
        return q;
    }

    /// <summary>
    /// Inverts this Quaternion.
    /// </summary>
    public void Invert()
    {
        Invert(in this, out this);
    }

    /// <summary>
    /// Returns the inverse of this Quaternion.
    /// </summary>
    /// <returns>The inverted copy.</returns>
    public readonly Quaternion Inverted()
    {
        var q = this;
        q.Invert();
        return q;
    }

    /// <summary>
    /// Scales the Quaternion to unit length.
    /// </summary>
    public void Normalize()
    {
        var scale = 1.0f / Length;
        xyz *= scale;
        w *= scale;
    }

    /// <summary>
    /// Inverts the Vector3 component of this Quaternion.
    /// </summary>
    public void Conjugate()
    {
        xyz = -xyz;
    }

    /// <summary>
    /// Defines the identity quaternion.
    /// </summary>
    public static readonly Quaternion Identity = new Quaternion(0, 0, 0, 1);

    /// <summary>
    /// Add two quaternions.
    /// </summary>
    /// <param name="left">The first operand.</param>
    /// <param name="right">The second operand.</param>
    /// <returns>The result of the addition.</returns>
    [Pure]
    public static Quaternion Add(Quaternion left, Quaternion right)
    {
        return new Quaternion(
            left.xyz + right.xyz,
            left.w + right.w);
    }

    /// <summary>
    /// Add two quaternions.
    /// </summary>
    /// <param name="left">The first operand.</param>
    /// <param name="right">The second operand.</param>
    /// <param name="result">The result of the addition.</param>
    public static void Add(in Quaternion left, in Quaternion right, out Quaternion result)
    {
        result = new Quaternion(
            left.xyz + right.xyz,
            left.w + right.w);
    }

    /// <summary>
    /// Subtracts two instances.
    /// </summary>
    /// <param name="left">The left instance.</param>
    /// <param name="right">The right instance.</param>
    /// <returns>The result of the operation.</returns>
    [Pure]
    public static Quaternion Sub(Quaternion left, Quaternion right)
    {
        return new Quaternion(
            left.xyz - right.xyz,
            left.w - right.w);
    }

    /// <summary>
    /// Subtracts two instances.
    /// </summary>
    /// <param name="left">The left instance.</param>
    /// <param name="right">The right instance.</param>
    /// <param name="result">The result of the operation.</param>
    public static void Sub(in Quaternion left, in Quaternion right, out Quaternion result)
    {
        result = new Quaternion(
            left.xyz - right.xyz,
            left.w - right.w);
    }

    /// <summary>
    /// Multiplies two instances.
    /// </summary>
    /// <param name="left">The first instance.</param>
    /// <param name="right">The second instance.</param>
    /// <returns>A new instance containing the result of the calculation.</returns>
    [Pure]
    public static Quaternion Multiply(Quaternion left, Quaternion right)
    {
        Multiply(in left, in right, out Quaternion result);
        return result;
    }

    /// <summary>
    /// Multiplies two instances.
    /// </summary>
    /// <param name="left">The first instance.</param>
    /// <param name="right">The second instance.</param>
    /// <param name="result">A new instance containing the result of the calculation.</param>
    public static void Multiply(in Quaternion left, in Quaternion right, out Quaternion result)
    {
        result = new Quaternion(
            (right.w * left.xyz) + (left.w * right.xyz) + Vector3.Cross(left.xyz, right.xyz),
            (left.w * right.w) - Vector3.Dot(left.xyz, right.xyz));
    }

    /// <summary>
    /// Multiplies an instance by a scalar.
    /// </summary>
    /// <param name="quaternion">The instance.</param>
    /// <param name="scale">The scalar.</param>
    /// <param name="result">A new instance containing the result of the calculation.</param>
    public static void Multiply(in Quaternion quaternion, float scale, out Quaternion result)
    {
        result = new Quaternion
        (
            quaternion.X * scale,
            quaternion.Y * scale,
            quaternion.Z * scale,
            quaternion.w * scale
        );
    }

    /// <summary>
    /// Multiplies an instance by a scalar.
    /// </summary>
    /// <param name="quaternion">The instance.</param>
    /// <param name="scale">The scalar.</param>
    /// <returns>A new instance containing the result of the calculation.</returns>
    [Pure]
    public static Quaternion Multiply(Quaternion quaternion, float scale)
    {
        return new Quaternion
        (
            quaternion.X * scale,
            quaternion.Y * scale,
            quaternion.Z * scale,
            quaternion.w * scale
        );
    }

    /// <summary>
    /// Get the conjugate of the given quaternion.
    /// </summary>
    /// <param name="q">The quaternion.</param>
    /// <returns>The conjugate of the given quaternion.</returns>
    [Pure]
    public static Quaternion Conjugate(Quaternion q)
    {
        return new Quaternion(-q.xyz, q.w);
    }

    /// <summary>
    /// Get the conjugate of the given quaternion.
    /// </summary>
    /// <param name="q">The quaternion.</param>
    /// <param name="result">The conjugate of the given quaternion.</param>
    public static void Conjugate(in Quaternion q, out Quaternion result)
    {
        result = new Quaternion(-q.xyz, q.w);
    }

    /// <summary>
    /// Get the inverse of the given quaternion.
    /// </summary>
    /// <param name="q">The quaternion to invert.</param>
    /// <returns>The inverse of the given quaternion.</returns>
    [Pure]
    public static Quaternion Invert(Quaternion q)
    {
        Invert(in q, out Quaternion result);
        return result;
    }

    /// <summary>
    /// Get the inverse of the given quaternion.
    /// </summary>
    /// <param name="q">The quaternion to invert.</param>
    /// <param name="result">The inverse of the given quaternion.</param>
    public static void Invert(in Quaternion q, out Quaternion result)
    {
        var lengthSq = q.LengthSquared;
        if (lengthSq != 0.0)
        {
            var i = 1.0f / lengthSq;
            result = new Quaternion(q.xyz * -i, q.w * i);
        }
        else
        {
            result = q;
        }
    }

    /// <summary>
    /// Scale the given quaternion to unit length.
    /// </summary>
    /// <param name="q">The quaternion to normalize.</param>
    /// <returns>The normalized copy.</returns>
    [Pure]
    public static Quaternion Normalize(Quaternion q)
    {
        Normalize(in q, out Quaternion result);
        return result;
    }

    /// <summary>
    /// Scale the given quaternion to unit length.
    /// </summary>
    /// <param name="q">The quaternion to normalize.</param>
    /// <param name="result">The normalized quaternion.</param>
    public static void Normalize(in Quaternion q, out Quaternion result)
    {
        var scale = 1.0f / q.Length;
        result = new Quaternion(q.xyz * scale, q.w * scale);
    }

    /// <summary>
    /// Build a quaternion from the given axis and angle in radians.
    /// </summary>
    /// <param name="axis">The axis to rotate about.</param>
    /// <param name="angle">The rotation angle in radians.</param>
    /// <returns>The equivalent quaternion.</returns>
    [Pure]
    public static Quaternion FromAxisAngle(Vector3 axis, float angle)
    {
        if (axis.LengthSquared == 0.0f)
        {
            return Identity;
        }

        var result = Identity;

        angle *= 0.5f;
        axis.Normalize();
        result.xyz = axis * MathF.Sin(angle);
        result.w = MathF.Cos(angle);

        return Normalize(result);
    }

    /// <summary>
    /// Builds a Quaternion from the given euler angles in radians
    /// The rotations will get applied in following order:
    /// 1. pitch (X axis), 2. yaw (Y axis), 3. roll (Z axis).
    /// </summary>
    /// <param name="pitch">The pitch (attitude), counterclockwise rotation around X axis.</param>
    /// <param name="yaw">The yaw (heading), counterclockwise rotation around Y axis.</param>
    /// <param name="roll">The roll (bank), counterclockwise rotation around Z axis.</param>
    /// <returns>The quaternion.</returns>
    [Pure]
    public static Quaternion FromEulerAngles(float pitch, float yaw, float roll)
    {
        return new Quaternion(pitch, yaw, roll);
    }

    /// <summary>
    /// Builds a Quaternion from the given euler angles in radians.
    /// The rotations will get applied in following order:
    /// 1. X axis, 2. Y axis, 3. Z axis.
    /// </summary>
    /// <param name="eulerAngles">The counterclockwise euler angles as a vector.</param>
    /// <returns>The equivalent Quaternion.</returns>
    [Pure]
    public static Quaternion FromEulerAngles(Vector3 eulerAngles)
    {
        return new Quaternion(eulerAngles);
    }

    /// <summary>
    /// Builds a Quaternion from the given euler angles in radians.
    /// The rotations will get applied in following order:
    /// 1. Around X, 2. Around Y, 3. Around Z.
    /// </summary>
    /// <param name="eulerAngles">The counterclockwise euler angles a vector.</param>
    /// <param name="result">The equivalent Quaternion.</param>
    public static void FromEulerAngles(in Vector3 eulerAngles, out Quaternion result)
    {
        var c1 = MathF.Cos(eulerAngles.x * 0.5f);
        var c2 = MathF.Cos(eulerAngles.y * 0.5f);
        var c3 = MathF.Cos(eulerAngles.z * 0.5f);
        var s1 = MathF.Sin(eulerAngles.x * 0.5f);
        var s2 = MathF.Sin(eulerAngles.y * 0.5f);
        var s3 = MathF.Sin(eulerAngles.z * 0.5f);

        result.w = (c1 * c2 * c3) - (s1 * s2 * s3);
        result.xyz.x = (s1 * c2 * c3) + (c1 * s2 * s3);
        result.xyz.y = (c1 * s2 * c3) - (s1 * c2 * s3);
        result.xyz.z = (c1 * c2 * s3) + (s1 * s2 * c3);
    }

    /// <summary>
    /// Converts a quaternion to it's euler angle representation.
    /// </summary>
    /// <param name="q">The Quaternion.</param>
    /// <param name="result">The resulting euler angles in radians.</param>
    public static void ToEulerAngles(in Quaternion q, out Vector3 result)
    {
        q.ToEulerAngles(out result);
    }

    /// <summary>
    /// Builds a quaternion from the given rotation matrix.
    /// </summary>
    /// <param name="matrix">A rotation matrix.</param>
    /// <returns>The equivalent quaternion.</returns>
    [Pure]
    public static Quaternion FromMatrix(Matrix3x3 matrix)
    {
        FromMatrix(in matrix, out Quaternion result);
        return result;
    }

    /// <summary>
    /// Builds a quaternion from the given rotation matrix.
    /// </summary>
    /// <param name="matrix">A rotation matrix.</param>
    /// <param name="result">The equivalent quaternion.</param>
    public static void FromMatrix(in Matrix3x3 matrix, out Quaternion result)
    {
        var trace = matrix.Trace;

        if (trace > 0)
        {
            var s = MathF.Sqrt(trace + 1) * 2;
            var invS = 1f / s;

            result.w = s * 0.25f;
            result.xyz.x = (matrix.Row2.y - matrix.Row1.z) * invS;
            result.xyz.y = (matrix.Row0.z - matrix.Row2.x) * invS;
            result.xyz.z = (matrix.Row1.x - matrix.Row0.y) * invS;
        }
        else
        {
            float m00 = matrix.Row0.x, m11 = matrix.Row1.y, m22 = matrix.Row2.z;

            if (m00 > m11 && m00 > m22)
            {
                var s = MathF.Sqrt(1 + m00 - m11 - m22) * 2;
                var invS = 1f / s;

                result.w = (matrix.Row2.y - matrix.Row1.z) * invS;
                result.xyz.x = s * 0.25f;
                result.xyz.y = (matrix.Row0.y + matrix.Row1.x) * invS;
                result.xyz.z = (matrix.Row0.z + matrix.Row2.x) * invS;
            }
            else if (m11 > m22)
            {
                var s = MathF.Sqrt(1 + m11 - m00 - m22) * 2;
                var invS = 1f / s;

                result.w = (matrix.Row0.z - matrix.Row2.x) * invS;
                result.xyz.x = (matrix.Row0.y + matrix.Row1.x) * invS;
                result.xyz.y = s * 0.25f;
                result.xyz.z = (matrix.Row1.z + matrix.Row2.y) * invS;
            }
            else
            {
                var s = MathF.Sqrt(1 + m22 - m00 - m11) * 2;
                var invS = 1f / s;

                result.w = (matrix.Row1.x - matrix.Row0.y) * invS;
                result.xyz.x = (matrix.Row0.z + matrix.Row2.x) * invS;
                result.xyz.y = (matrix.Row1.z + matrix.Row2.y) * invS;
                result.xyz.z = s * 0.25f;
            }
        }
    }

    /// <summary>
    /// Do Spherical linear interpolation between two quaternions.
    /// </summary>
    /// <param name="q1">The first quaternion.</param>
    /// <param name="q2">The second quaternion.</param>
    /// <param name="blend">The blend factor.</param>
    /// <returns>A smooth blend between the given quaternions.</returns>
    [Pure]
    public static Quaternion Slerp(Quaternion q1, Quaternion q2, float blend)
    {
        // if either input is zero, return the other.
        if (q1.LengthSquared == 0.0f)
        {
            if (q2.LengthSquared == 0.0f)
            {
                return Identity;
            }

            return q2;
        }

        if (q2.LengthSquared == 0.0f)
        {
            return q1;
        }

        var cosHalfAngle = (q1.w * q2.w) + Vector3.Dot(q1.xyz, q2.xyz);

        if (cosHalfAngle >= 1.0f || cosHalfAngle <= -1.0f)
        {
            // angle = 0.0f, so just return one input.
            return q1;
        }

        if (cosHalfAngle < 0.0f)
        {
            q2.xyz = -q2.xyz;
            q2.w = -q2.w;
            cosHalfAngle = -cosHalfAngle;
        }

        float blendA;
        float blendB;
        if (cosHalfAngle < 0.99f)
        {
            // do proper slerp for big angles
            var halfAngle = MathF.Acos(cosHalfAngle);
            var sinHalfAngle = MathF.Sin(halfAngle);
            var oneOverSinHalfAngle = 1.0f / sinHalfAngle;
            blendA = MathF.Sin(halfAngle * (1.0f - blend)) * oneOverSinHalfAngle;
            blendB = MathF.Sin(halfAngle * blend) * oneOverSinHalfAngle;
        }
        else
        {
            // do lerp if angle is really small.
            blendA = 1.0f - blend;
            blendB = blend;
        }

        var result = new Quaternion((blendA * q1.xyz) + (blendB * q2.xyz), (blendA * q1.w) + (blendB * q2.w));
        if (result.LengthSquared > 0.0f)
        {
            return Normalize(result);
        }

        return Identity;
    }

    /// <summary>
    /// Adds two instances.
    /// </summary>
    /// <param name="left">The first instance.</param>
    /// <param name="right">The second instance.</param>
    /// <returns>The result of the calculation.</returns>
    [Pure]
    public static Quaternion operator +(Quaternion left, Quaternion right)
    {
        left.xyz += right.xyz;
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
    public static Quaternion operator -(Quaternion left, Quaternion right)
    {
        left.xyz -= right.xyz;
        left.w -= right.w;
        return left;
    }

    /// <summary>
    /// Multiplies two instances.
    /// </summary>
    /// <param name="left">The first instance.</param>
    /// <param name="right">The second instance.</param>
    /// <returns>The result of the calculation.</returns>
    [Pure]
    public static Quaternion operator *(Quaternion left, Quaternion right)
    {
        Multiply(in left, in right, out left);
        return left;
    }

    /// <summary>
    /// Multiplies an instance by a scalar.
    /// </summary>
    /// <param name="quaternion">The instance.</param>
    /// <param name="scale">The scalar.</param>
    /// <returns>A new instance containing the result of the calculation.</returns>
    [Pure]
    public static Quaternion operator *(Quaternion quaternion, float scale)
    {
        Multiply(in quaternion, scale, out quaternion);
        return quaternion;
    }

    /// <summary>
    /// Multiplies an instance by a scalar.
    /// </summary>
    /// <param name="quaternion">The instance.</param>
    /// <param name="scale">The scalar.</param>
    /// <returns>A new instance containing the result of the calculation.</returns>
    [Pure]
    public static Quaternion operator *(float scale, Quaternion quaternion)
    {
        return new Quaternion
        (
            quaternion.X * scale,
            quaternion.Y * scale,
            quaternion.Z * scale,
            quaternion.w * scale
        );
    }

    /// <summary>
    /// Compares two instances for equality.
    /// </summary>
    /// <param name="left">The first instance.</param>
    /// <param name="right">The second instance.</param>
    /// <returns>True, if left equals right; false otherwise.</returns>
    public static bool operator ==(Quaternion left, Quaternion right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Compares two instances for inequality.
    /// </summary>
    /// <param name="left">The first instance.</param>
    /// <param name="right">The second instance.</param>
    /// <returns>True, if left does not equal right; false otherwise.</returns>
    public static bool operator !=(Quaternion left, Quaternion right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Converts <see cref="System.Numerics.Quaternion"/> to <see cref="Quaternion"/>.
    /// </summary>
    /// <param name="quat">The <see cref="System.Numerics.Quaternion"/> to cast.</param>
    [Pure]
    public static explicit operator Quaternion(System.Numerics.Quaternion quat)
    {
        return Unsafe.As<System.Numerics.Quaternion, Quaternion>(ref quat);
    }

    /// <summary>
    /// Converts <see cref="Quaternion"/> to <see cref="System.Numerics.Quaternion"/>.
    /// </summary>
    /// <param name="quat">The <see cref="Quaternion"/> to cast.</param>
    [Pure]
    public static explicit operator System.Numerics.Quaternion(Quaternion quat)
    {
        return Unsafe.As<Quaternion, System.Numerics.Quaternion>(ref quat);
    }

    /// <inheritdoc />
    public override readonly bool Equals(object obj)
    {
        return obj is Quaternion && Equals((Quaternion)obj);
    }

    /// <inheritdoc />
    public readonly bool Equals(Quaternion other)
    {
        return xyz.Equals(other.xyz) &&
               w == other.w;
    }

    /// <inheritdoc />
    public override readonly int GetHashCode()
    {
        return HashCode.Combine(xyz, w);
    }

    /// <summary>
    /// Returns a System.String that represents the current Quaternion.
    /// </summary>
    /// <returns>A human-readable representation of the quaternion.</returns>
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
        var ls = Mathf.GetListSeparator(formatProvider);
        var xyz = this.xyz.ToString(format, formatProvider);
        var w = this.w.ToString(format, formatProvider);
        return $"V: {xyz}{ls} W: {w}";
    }
}
