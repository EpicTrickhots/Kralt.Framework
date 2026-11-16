namespace Kralt.Framework.Graphics
{
    public class MeshRenderer : Transformable, IRenderable
    {
        public Mesh? mesh;
        public ShaderProgram? material = Resources.shaders["fallback"];

        public void Render(Renderer renderer)
        {
            if (mesh == null || material == null)
                return;

            renderer.Draw(mesh, material, worldMatrix);
        }
    }
}
