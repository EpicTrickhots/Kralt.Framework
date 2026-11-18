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
        public Vector4 uv0 { get; }
        public Vector4 uv1 { get; }

        public Vertex(Vector3 position, Color color, Vector4 uv0)
        {
            this.position = position;
            this.color = color;
            this.uv0 = uv0;
        }

        public Vertex(Vector3 position, Color color, Vector4 uv0, Vector4 uv1)
        {
            this.position = position;
            this.color = color;
            this.uv0 = uv0;
            this.uv1 = uv1;
        }

        public Vertex(float x, float y, float z, float u, float v)
        {
            position = new Vector3(x, y, z);
            color = Color.white;
            uv0 = new Vector4(u, v, 0f, 0f);
        }

        public Vertex(float x, float y, float z, Color color, float u, float v)
        {
            position = new Vector3(x, y, z);
            this.color = color;
            uv0 = new Vector4(u, v, 0f, 0f);
        }

        public Vertex(float x, float y, float z, float r, float g, float b, float u, float v)
        {
            position = new Vector3(x, y, z);
            color = new Color(r, g, b, 1f);
            uv0 = new Vector4(u, v, 0f, 0f);
        }

        public static int SizeInBytes => Marshal.SizeOf<Vertex>();
    }
}
