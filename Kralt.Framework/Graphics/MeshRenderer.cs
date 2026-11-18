#nullable disable

namespace Kralt.Framework.Graphics
{
    public class MeshRenderer : Transformable, IRenderable
    {
        public Mesh mesh;
        public Material material;

        public void Render()
        {
            if (mesh == null)
                return;

            Renderer.Draw(mesh, material, worldMatrix);
        }
    }
}
