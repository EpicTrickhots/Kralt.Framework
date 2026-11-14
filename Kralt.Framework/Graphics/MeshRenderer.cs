using Kralt.Framework.Graphics.Rendering;

namespace Kralt.Framework.Graphics
{
    public class MeshRenderer : GameObject
    {
        public Mesh? mesh;
        public ShaderProgram material = Application.resources.fallbackShader;

        public override void OnRender(Application renderer)
        {
            if (mesh == null)
                return;

            renderer.Draw(mesh, material, worldMatrix);
        }
    }
}
