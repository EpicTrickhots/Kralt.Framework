#nullable disable

namespace Kralt.Framework.Graphics
{
    public class MeshRenderer : Transformable, IRenderable
    {
        public Mesh mesh;
        public Material material;

        public void Render(Renderer renderer)
        {
            if (mesh == null)
                return;

            renderer.Draw(mesh, material, worldMatrix);
        }
    }
}
