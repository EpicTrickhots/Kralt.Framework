using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using OpenTK.Graphics.OpenGL4;
using Kralt.Framework.Graphics;
using Kralt.Mathematics;

namespace Kralt.Framework.UI;

public static class UIRenderer
{
    private const int K_MAX_RETAINED_DRAWABLES = 16384;

    private static int VertexBufferObject { get; set; }

    private static int VertexArrayObject { get; set; }

    private static int ElementBufferObject { get; set; }

    private readonly static UIVertex[] vertices = new UIVertex[K_MAX_RETAINED_DRAWABLES * 4];

    private readonly static uint[] indices = new uint[K_MAX_RETAINED_DRAWABLES * 6];

    internal static void Initialize()
    {
        // Pre-fill index buffer for a full scene
        for (uint i = 0; i < K_MAX_RETAINED_DRAWABLES; i++)
        {
            uint v = i * 4;
            uint idx = i * 6;
            indices[idx + 0] = v + 0;
            indices[idx + 1] = v + 1;
            indices[idx + 2] = v + 2;
            indices[idx + 3] = v + 2;
            indices[idx + 4] = v + 3;
            indices[idx + 5] = v + 0;
        }

        int stride = Unsafe.SizeOf<UIVertex>();

        VertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(VertexArrayObject);

        // Allocate the full vertex buffer, pass IntPtr.Zero because we upload slices later
        VertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * Unsafe.SizeOf<UIVertex>(), IntPtr.Zero, BufferUsageHint.DynamicDraw);

        ElementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

        // POSITION
        
        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, stride, 0);
        GL.EnableVertexAttribArray(0);

        // COLOR
        GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, stride, 2 * sizeof(float));
        GL.EnableVertexAttribArray(1);

        // TEXCOORD0
        GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, stride, 6 * sizeof(float));
        GL.EnableVertexAttribArray(2);

        GL.BindVertexArray(0);
    }
    
    public static void RenderFrame(Drawable[] elements, Vector2Int viewport, Material? material = null)
    {
        if (material == null)
        {
            RenderFrame(elements, viewport, Resources.materials["ui-default"]);
            return;
        }

        material.Apply();
        material.shader.SetVec2("VIEWPORT", viewport);
    }
}
