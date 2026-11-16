using OpenTK.Graphics.OpenGL4;
using Kralt.Mathematics;
using System;

namespace Kralt.Framework.Graphics
{
    public class Renderer
    {
        internal void BeginFrame()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        internal void EndFrame()
        {

        }

        Matrix4x4 currentCameraViewMatrix;
        Matrix4x4 currentCameraProjectionMatrix;

        internal void RenderFrame(Scene scene, float aspect)
        {
            BeginFrame();

            if (scene.outputCamera == null)
                return;

            Camera camera = scene.outputCamera;
            currentCameraViewMatrix = camera.viewMatrix;
            currentCameraProjectionMatrix = Matrix4x4.CreatePerspectiveFieldOfView(
                camera.fieldOfView * Mathf.Deg2Rad,
                aspect,
                camera.nearPlaneDistance,
                camera.farPlaneDistance
            );

            foreach (Transformable transform in scene.hierarchy)
                TraverseRecursive(transform);

            EndFrame();
        }

        private void TraverseRecursive(Transformable transform)
        {
            if (transform is IRenderable renderable)
                renderable.Render(this);

            foreach (Transformable child in transform)
                TraverseRecursive(child);
        }

        public void Draw(Mesh mesh, ShaderProgram shader, Matrix4x4 transformation)
        {
            shader.Use(transformation, currentCameraViewMatrix, currentCameraProjectionMatrix);
            GL.BindVertexArray(mesh.VertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, mesh.indices.Length, DrawElementsType.UnsignedInt, 0);
        }
    }
}
