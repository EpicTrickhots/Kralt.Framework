using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Kralt.Mathematics;

[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct Color : IEquatable<Color>, IFormattable
{
    public float r;
    public float g;
    public float b;
    public float a;

    public Color(float r, float g, float b, float a)
    {
        this.r = r;
        this.g = g;
        this.b = b;
        this.a = a;
    }

    public Color(byte r, byte g, byte b, byte a)
    {
        this.r = r / (float)byte.MaxValue;
        this.g = g / (float)byte.MaxValue;
        this.b = b / (float)byte.MaxValue;
        this.a = a / (float)byte.MaxValue;
    }

    public static Color white => new Color(1f, 1f, 1f, 1f);

    public static Color black => new Color(0f, 0f, 0f, 1f);

    public static Color clear => new Color(0f, 0f, 0f, 0f);

    public static Color red => new Color(1f, 0f, 0f, 1f);

    public static Color green => new Color(0f, 1f, 0f, 1f);

    public static Color blue => new Color(0f, 0f, 1f, 1f);

    public readonly int ToArgb()
    {
        var value =
            ((uint)(a * byte.MaxValue) << 24) |
            ((uint)(r * byte.MaxValue) << 16) |
            ((uint)(g * byte.MaxValue) << 8) |
            (uint)(b * byte.MaxValue);

        return unchecked((int)value);
    }

    [Pure]
    public static bool operator ==(Color left, Color right)
    {
        return left.Equals(right);
    }

    [Pure]
    public static bool operator !=(Color left, Color right)
    {
        return !left.Equals(right);
    }

    [Pure]
    public static implicit operator Color(System.Drawing.Color color)
    {
        return new Color(color.R, color.G, color.B, color.A);
    }

    [Pure]
    public static explicit operator System.Drawing.Color(Color color)
    {
        return System.Drawing.Color.FromArgb(
            (int)(color.a * byte.MaxValue),
            (int)(color.r * byte.MaxValue),
            (int)(color.g * byte.MaxValue),
            (int)(color.b * byte.MaxValue));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public static explicit operator Vector4(Color c)
    {
        return Unsafe.As<Color, Vector4>(ref c);
    }

    [Pure]
    public override bool Equals(object obj)
    {
        return obj is Color && Equals((Color)obj);
    }

    public override readonly int GetHashCode()
    {
        return HashCode.Combine(r, g, b, a);
    }

    public override string ToString()
    {
        return ToString(null, null);
    }

    public string ToString(string format)
    {
        return ToString(format, null);
    }

    public string ToString(IFormatProvider formatProvider)
    {
        return ToString(null, formatProvider);
    }

    public readonly string ToString(string format, IFormatProvider formatProvider)
    {
        var ls = Mathf.GetListSeparator(formatProvider);
        var r = this.r.ToString(format, formatProvider);
        var g = this.g.ToString(format, formatProvider);
        var b = this.b.ToString(format, formatProvider);
        var a = this.a.ToString(format, formatProvider);

        return $"{{(R{ls} G{ls} B{ls} A) = ({r}{ls} {g}{ls} {b}{ls} {a})}}";
    }

    [Pure]
    public static Color FromSrgb(Color srgb)
    {
        float r, g, b;

        if (srgb.r <= 0.04045f)
        {
            r = srgb.r / 12.92f;
        }
        else
        {
            r = MathF.Pow((srgb.r + 0.055f) / (1.0f + 0.055f), 2.4f);
        }

        if (srgb.g <= 0.04045f)
        {
            g = srgb.g / 12.92f;
        }
        else
        {
            g = MathF.Pow((srgb.g + 0.055f) / (1.0f + 0.055f), 2.4f);
        }

        if (srgb.b <= 0.04045f)
        {
            b = srgb.b / 12.92f;
        }
        else
        {
            b = MathF.Pow((srgb.b + 0.055f) / (1.0f + 0.055f), 2.4f);
        }

        return new Color(r, g, b, srgb.a);
    }

    [Pure]
    public static Color ToSrgb(Color rgb)
    {
        float r, g, b;

        if (rgb.r <= 0.0031308)
        {
            r = 12.92f * rgb.r;
        }
        else
        {
            r = ((1.0f + 0.055f) * MathF.Pow(rgb.r, 1.0f / 2.4f)) - 0.055f;
        }

        if (rgb.g <= 0.0031308)
        {
            g = 12.92f * rgb.g;
        }
        else
        {
            g = ((1.0f + 0.055f) * MathF.Pow(rgb.g, 1.0f / 2.4f)) - 0.055f;
        }

        if (rgb.b <= 0.0031308)
        {
            b = 12.92f * rgb.b;
        }
        else
        {
            b = ((1.0f + 0.055f) * MathF.Pow(rgb.b, 1.0f / 2.4f)) - 0.055f;
        }

        return new Color(r, g, b, rgb.a);
    }

    [Pure]
    public static Color FromHsl(Vector4 hsl)
    {
        var hue = hsl.x * 360.0f;
        var saturation = hsl.y;
        var lightness = hsl.z;

        var c = (1.0f - Math.Abs((2.0f * lightness) - 1.0f)) * saturation;

        var h = hue / 60.0f;
        var x = c * (1.0f - Math.Abs((h % 2.0f) - 1.0f));

        float r, g, b;
        if (h >= 0.0f && h < 1.0f)
        {
            r = c;
            g = x;
            b = 0.0f;
        }
        else if (h >= 1.0f && h < 2.0f)
        {
            r = x;
            g = c;
            b = 0.0f;
        }
        else if (h >= 2.0f && h < 3.0f)
        {
            r = 0.0f;
            g = c;
            b = x;
        }
        else if (h >= 3.0f && h < 4.0f)
        {
            r = 0.0f;
            g = x;
            b = c;
        }
        else if (h >= 4.0f && h < 5.0f)
        {
            r = x;
            g = 0.0f;
            b = c;
        }
        else if (h >= 5.0f && h <= 6.0f)
        {
            r = c;
            g = 0.0f;
            b = x;
        }
        else
        {
            r = 0.0f;
            g = 0.0f;
            b = 0.0f;
        }

        var m = lightness - (c / 2.0f);
        if (m < 0)
        {
            m = 0;
        }
        return new Color(r + m, g + m, b + m, hsl.w);
    }

    [Pure]
    public static Vector4 ToHsl(Color rgb)
    {
        var max = Math.Max(rgb.r, Math.Max(rgb.g, rgb.b));
        var min = Math.Min(rgb.r, Math.Min(rgb.g, rgb.b));
        var diff = max - min;

        var h = 0.0f;
        if (diff == 0)
        {
            h = 0.0f;
        }
        else if (max == rgb.r)
        {
            h = ((rgb.g - rgb.b) / diff) % 6;
            if (h < 0)
            {
                h += 6;
            }
        }
        else if (max == rgb.g)
        {
            h = ((rgb.b - rgb.r) / diff) + 2.0f;
        }
        else if (max == rgb.b)
        {
            h = ((rgb.r - rgb.g) / diff) + 4.0f;
        }

        var hue = h / 6.0f;
        if (hue < 0.0f)
        {
            hue += 1.0f;
        }

        var lightness = (max + min) / 2.0f;

        var saturation = 0.0f;
        if ((1.0f - Math.Abs((2.0f * lightness) - 1.0f)) != 0)
        {
            saturation = diff / (1.0f - Math.Abs((2.0f * lightness) - 1.0f));
        }

        return new Vector4(hue, saturation, lightness, rgb.a);
    }

    [Pure]
    public static Color FromHsv(Vector4 hsv)
    {
        var hue = hsv.x * 360.0f;
        var saturation = hsv.y;
        var value = hsv.z;

        var c = value * saturation;

        var h = hue / 60.0f;
        var x = c * (1.0f - Math.Abs((h % 2.0f) - 1.0f));

        float r, g, b;
        if (h >= 0.0f && h < 1.0f)
        {
            r = c;
            g = x;
            b = 0.0f;
        }
        else if (h >= 1.0f && h < 2.0f)
        {
            r = x;
            g = c;
            b = 0.0f;
        }
        else if (h >= 2.0f && h < 3.0f)
        {
            r = 0.0f;
            g = c;
            b = x;
        }
        else if (h >= 3.0f && h < 4.0f)
        {
            r = 0.0f;
            g = x;
            b = c;
        }
        else if (h >= 4.0f && h < 5.0f)
        {
            r = x;
            g = 0.0f;
            b = c;
        }
        else if (h >= 5.0f && h <= 6.0f)
        {
            r = c;
            g = 0.0f;
            b = x;
        }
        else
        {
            r = 0.0f;
            g = 0.0f;
            b = 0.0f;
        }

        var m = value - c;
        return new Color(r + m, g + m, b + m, hsv.w);
    }

    [Pure]
    public static Vector4 ToHsv(Color rgb)
    {
        var max = Math.Max(rgb.r, Math.Max(rgb.g, rgb.b));
        var min = Math.Min(rgb.r, Math.Min(rgb.g, rgb.b));
        var diff = max - min;

        var h = 0.0f;
        if (diff == 0)
        {
            h = 0.0f;
        }
        else if (max == rgb.r)
        {
            h = ((rgb.g - rgb.b) / diff) % 6.0f;
            if (h < 0)
            {
                h += 6f;
            }
        }
        else if (max == rgb.g)
        {
            h = ((rgb.b - rgb.r) / diff) + 2.0f;
        }
        else if (max == rgb.b)
        {
            h = ((rgb.r - rgb.g) / diff) + 4.0f;
        }

        var hue = h * 60.0f / 360.0f;

        var saturation = 0.0f;
        if (max != 0.0f)
        {
            saturation = diff / max;
        }

        return new Vector4(hue, saturation, max, rgb.a);
    }

    [Pure]
    public static Color FromXyz(Vector4 xyz)
    {
        var r = (0.41847f * xyz.x) + (-0.15866f * xyz.y) + (-0.082835f * xyz.z);
        var g = (-0.091169f * xyz.x) + (0.25243f * xyz.y) + (0.015708f * xyz.z);
        var b = (0.00092090f * xyz.x) + (-0.0025498f * xyz.y) + (0.17860f * xyz.z);
        return new Color(r, g, b, xyz.w);
    }

    [Pure]
    public static Vector4 ToXyz(Color rgb)
    {
        var x = ((0.49f * rgb.r) + (0.31f * rgb.g) + (0.20f * rgb.b)) / 0.17697f;
        var y = ((0.17697f * rgb.r) + (0.81240f * rgb.g) + (0.01063f * rgb.b)) / 0.17697f;
        var z = ((0.00f * rgb.r) + (0.01f * rgb.g) + (0.99f * rgb.b)) / 0.17697f;
        return new Vector4(x, y, z, rgb.a);
    }

    [Pure]
    public static Color FromYcbcr(Vector4 ycbcr)
    {
        var r = (1.0f * ycbcr.x) + (0.0f * ycbcr.y) + (1.402f * ycbcr.z);
        var g = (1.0f * ycbcr.x) + (-0.344136f * ycbcr.y) + (-0.714136f * ycbcr.z);
        var b = (1.0f * ycbcr.x) + (1.772f * ycbcr.y) + (0.0f * ycbcr.z);
        return new Color(r, g, b, ycbcr.w);
    }

    [Pure]
    public static Vector4 ToYcbcr(Color rgb)
    {
        var y = (0.299f * rgb.r) + (0.587f * rgb.g) + (0.114f * rgb.b);
        var u = (-0.168736f * rgb.r) + (-0.331264f * rgb.g) + (0.5f * rgb.b);
        var v = (0.5f * rgb.r) + (-0.418688f * rgb.g) + (-0.081312f * rgb.b);
        return new Vector4(y, u, v, rgb.a);
    }

    [Pure]
    public static Color FromHcy(Vector4 hcy)
    {
        var hue = hcy.x * 360.0f;
        var y = hcy.y;
        var luminance = hcy.z;

        var h = hue / 60.0f;
        var x = y * (1.0f - Math.Abs((h % 2.0f) - 1.0f));

        float r, g, b;
        if (h >= 0.0f && h < 1.0f)
        {
            r = y;
            g = x;
            b = 0.0f;
        }
        else if (h >= 1.0f && h < 2.0f)
        {
            r = x;
            g = y;
            b = 0.0f;
        }
        else if (h >= 2.0f && h < 3.0f)
        {
            r = 0.0f;
            g = y;
            b = x;
        }
        else if (h >= 3.0f && h < 4.0f)
        {
            r = 0.0f;
            g = x;
            b = y;
        }
        else if (h >= 4.0f && h < 5.0f)
        {
            r = x;
            g = 0.0f;
            b = y;
        }
        else if (h >= 5.0f && h < 6.0f)
        {
            r = y;
            g = 0.0f;
            b = x;
        }
        else
        {
            r = 0.0f;
            g = 0.0f;
            b = 0.0f;
        }

        var m = luminance - (0.30f * r) + (0.59f * g) + (0.11f * b);
        return new Color(r + m, g + m, b + m, hcy.w);
    }

    [Pure]
    public static Vector4 ToHcy(Color rgb)
    {
        var max = Math.Max(rgb.r, Math.Max(rgb.g, rgb.b));
        var min = Math.Min(rgb.r, Math.Min(rgb.g, rgb.b));
        var diff = max - min;

        var h = 0.0f;
        if (max == rgb.r)
        {
            h = ((rgb.g - rgb.b) / diff) % 6.0f;
        }
        else if (max == rgb.g)
        {
            h = ((rgb.b - rgb.r) / diff) + 2.0f;
        }
        else if (max == rgb.b)
        {
            h = ((rgb.r - rgb.g) / diff) + 4.0f;
        }

        var hue = h * 60.0f / 360.0f;

        var luminance = (0.30f * rgb.r) + (0.59f * rgb.g) + (0.11f * rgb.b);

        return new Vector4(hue, diff, luminance, rgb.a);
    }

    [Pure]
    public readonly bool Equals(Color other)
    {
        return
            r == other.r &&
            g == other.g &&
            b == other.b &&
            a == other.a;
    }
}
