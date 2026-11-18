using OpenTK.Graphics.OpenGL4;
using Kralt.Mathematics;
using System;

namespace Kralt.Framework.Graphics
{
    public class Renderer
    {
        internal const string TRANSFORM_MODEL = "TR_MESH";

        internal const string TRANSFORM_VIEW = "TR_VIEW";

        internal const string TRANSFORM_PROJ = "TR_PROJ";

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

        public void Draw(Mesh mesh, Material material, Matrix4x4 transformation)
        {
            if(material == null)
            {
                Draw(mesh, Resources.materials["fallback"], transformation);
                return;
            }

            material.Apply();

            // This is conditional - not all shaders will need matrix transformations
            // OpenGL safely returns -1 if a shader does not use this uniform.
            material.shader.SetMatrix4x4(TRANSFORM_MODEL, transformation);
            material.shader.SetMatrix4x4(TRANSFORM_VIEW, currentCameraViewMatrix);
            material.shader.SetMatrix4x4(TRANSFORM_PROJ, currentCameraProjectionMatrix);

            // Bind mesh data and issue draw call
            GL.BindVertexArray(mesh.VertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, mesh.indices.Length, DrawElementsType.UnsignedInt, 0);
        }
    }
}
