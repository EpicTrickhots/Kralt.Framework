using OpenTK.Graphics.OpenGL4;

namespace Kralt.Framework.Graphics
{
    public class Mesh
    {
        public Vertex[] vertices = [];
        public uint[] indices = [];

        internal int VertexBufferObject { get; private set; }
        internal int VertexArrayObject { get; private set; }
        internal int ElementBufferObject { get; private set; }

        public Mesh(Vertex[] vertices, uint[] indices)
        {
            this.vertices = vertices;
            this.indices = indices;

            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * Vertex.SizeInBytes, vertices, BufferUsageHint.StaticDraw);

            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            // POSITION
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 0);
            GL.EnableVertexAttribArray(0);

            // COLOR
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            // TEXCOORD0
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 7 * sizeof(float));
            GL.EnableVertexAttribArray(2);

            GL.BindVertexArray(0);
        }
    }
}
