using System;
using Kralt.Mathematics;

namespace Kralt.Framework.Physics;

public struct AABB : IEquatable<AABB>, IFormattable
{
    public Vector3 min;

    public Vector3 max;

    public AABB(Vector3 min, Vector3 max)
    {
        this.min = min;
        this.max = max;
    }

    public AABB(Vector3 position, float width)
    {
        min = position + new Vector3(-width, -width, -width);
        max = position + new Vector3(width, width, width);
    }

    public readonly bool Intersects(AABB other) =>
            min.x < other.max.x && max.x > other.min.x &&
            min.y < other.max.y && max.y > other.min.y &&
            min.z < other.max.z && max.z > other.min.z;

    public static bool operator ==(AABB lhs, AABB rhs) => lhs.Equals(rhs);

    public static bool operator !=(AABB lhs, AABB rhs) => !lhs.Equals(rhs);

    public override readonly bool Equals(object? obj)
    {
        return obj is AABB && Equals((AABB)obj);
    }

    public readonly bool Equals(AABB other)
    {
        return min == other.min &&
               max == other.max;
    }

    public override readonly int GetHashCode()
    {
        return HashCode.Combine(min.GetHashCode(), max.GetHashCode());
    }

    public readonly string ToString(string format)
    {
        return ToString(format, null);
    }

    public readonly string ToString(IFormatProvider formatProvider)
    {
        return ToString(null, formatProvider);
    }

    public readonly string ToString(string? format, IFormatProvider? formatProvider)
    {
        return $"{min.ToString(format, formatProvider)}, {max.ToString(format, formatProvider)}";
    }
}
