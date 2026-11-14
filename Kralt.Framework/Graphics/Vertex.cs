using System;
using System.Runtime.InteropServices;
using Kralt.Mathematics;

namespace Kralt.Framework.Graphics
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Vertex
    {
        public Vector3 position { get; }
        public Color color { get; }
        public Vector2 uv0 { get; }

        public Vertex(Vector3 position, Color color, Vector2 uv0)
        {
            this.position = position;
            this.color = color;
            this.uv0 = uv0;
        }

        public Vertex(float x, float y, float z, Color color, float u, float v)
        {
            position = new Vector3(x, y, z);
            this.color = color;
            uv0 = new Vector2(u, v);
        }

        public Vertex(float x, float y, float z, float r, float g, float b, float u, float v)
        {
            position = new Vector3(x, y, z);
            color = new Color(r, g, b, 1f);
            uv0 = new Vector2(u, v);
        }

        public static int SizeInBytes => Marshal.SizeOf<Vertex>();
    }
}
