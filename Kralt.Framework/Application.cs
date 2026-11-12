using System;
using System.IO;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Kralt.Framework.Graphics.Rendering;
using Kralt.Mathematics;

#nullable disable

namespace Kralt.Framework
{
    public abstract class Application(string title, int width, int height) : 
        GameWindow(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title })
    {
        readonly Vector3[] vertices = [
            new(0.5f,  0.5f, 0.0f),  // top right
            new(0.5f, -0.5f, 0.0f),  // bottom right
            new(-0.5f, -0.5f, 0.0f),  // bottom left
            new(-0.5f,  0.5f, 0.0f)   // top left
        ];

        readonly uint[] indices = [ 0, 1, 3, 1, 2, 3 ];

        ShaderProgram shader;
        static readonly string shaderDirectory = Path.Combine(AppContext.BaseDirectory, "Graphics", "Shaders");
        
        int vertexBufferObject;
        int elementBufferObject;
        int vertexArrayObject;

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            shader = ShaderProgram.Compile(shaderDirectory, "Fallback");

            GL.ClearColor((System.Drawing.Color)Color.black);

            vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * Vector3.SizeInBytes, vertices, BufferUsageHint.StaticDraw);

            vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            shader.Use();
            GL.BindVertexArray(vertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

            SwapBuffers();
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);
        }
    }
}
