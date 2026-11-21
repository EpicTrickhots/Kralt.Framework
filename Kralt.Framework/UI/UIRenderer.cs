using Kralt.Framework.Graphics;
using Kralt.Mathematics;

namespace Kralt.Framework.UI;

public static class UIRenderer
{
    internal const int K_MAX_RETAINED_VERTICES = 65536;

    internal static void Initialize()
    {

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
