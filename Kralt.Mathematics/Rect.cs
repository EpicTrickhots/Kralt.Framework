using System;
using System.Runtime.InteropServices;

namespace Kralt.Mathematics;

[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct Rect : IEquatable<Rect>, IFormattable
{
    public float x;

    public float y;

    public float width;

    public float height;

    public Vector2 position
    {
        readonly get => new Vector2(x, y);
        set
        {
            if (!float.IsFinite(value.x) || !float.IsFinite(value.y))
                return;

            x = value.x;
            y = value.y;
        }
    }

    public Vector2 size
    {
        readonly get => new Vector2(width, height);
        set
        {
            if (!float.IsFinite(value.x) || !float.IsFinite(value.y) || value.x < 0f || value.y < 0f)
                return;

            width = value.x;
            height = value.y;
        }
    }

    public readonly float xMin => x - width * 0.5f;

    public readonly float xMax => x + width * 0.5f;

    public readonly float yMin => y - height * 0.5f;

    public readonly float yMax => y + height * 0.5f;

    public readonly Vector2 topLeft => new(xMin, yMax);

    public readonly Vector2 topRight => new(xMax, yMax);

    public readonly Vector2 bottomRight => new(xMax, yMin);

    public readonly Vector2 bottomLeft => new(xMin, yMin);

    public Rect(Vector2 position, Vector2 size)
    {
        x = position.x;
        y = position.y;
        width = size.x;
        height = size.y;
    }

    public Rect(float x, float y, float width, float height) : this(new Vector2(x, y), new Vector2(width, height))
    {
    }

    public readonly bool Contains(Vector2 point) =>
        xMin < point.x && xMax > point.x &&
        yMin < point.y && yMax > point.y;

    public readonly bool Contains(Rect other) =>
        xMin < other.xMin && xMax > other.xMax &&
        yMin < other.yMin && yMax > other.yMax;

    public void Translate(Vector2 translation)
    {
        position += translation;
    }

    public static bool operator ==(Rect lhs, Rect rhs) => lhs.Equals(rhs);

    public static bool operator !=(Rect lhs, Rect rhs) => !lhs.Equals(rhs);

    public override bool Equals(object obj)
    {
        return obj is Rect && Equals((Rect)obj);
    }

    public readonly bool Equals(Rect other)
    {
        return x == other.x && y == other.y && 
            width == other.width && height == other.height;
    }

    public override readonly int GetHashCode()
    {
        return HashCode.Combine(x, y, width, height);
    }

    public override readonly string ToString()
    {
        return ToString(null, null);
    }

    public readonly string ToString(string format)
    {
        return ToString(format, null);
    }

    public readonly string ToString(IFormatProvider formatProvider)
    {
        return ToString(null, formatProvider);
    }

    public readonly string ToString(string format, IFormatProvider formatProvider)
    {
        return $"({x}, {y}, {width}, {height})";
    }
}
