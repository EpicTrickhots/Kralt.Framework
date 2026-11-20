using System;
using OpenTK.Graphics.OpenGL4;
using Kralt.Mathematics;
using Kralt.Framework.UI;

namespace Kralt.Framework.Graphics
{
    public static class Renderer
    {
        internal const string TRANSFORM_MODEL = "TR_MESH";

        internal const string TRANSFORM_VIEW = "TR_VIEW";

        internal const string TRANSFORM_PROJ = "TR_PROJ";

        private static Matrix4x4 currentCameraViewMatrix;

        private static Matrix4x4 currentCameraProjectionMatrix;

        public static void RenderUI(Drawable root, Vector2Int viewport)
        {

        }

        public static void RenderScene(Scene scene, float aspect)
        {
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
        }

        private static void TraverseRecursive(Transformable transform)
        {
            if (transform is IRenderable renderable)
                renderable.Render();

            foreach (Transformable child in transform)
                TraverseRecursive(child);
        }

        public static void Draw(Mesh mesh, Material material, Matrix4x4 transformation)
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
