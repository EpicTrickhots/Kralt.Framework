using System.Collections.Generic;
using Kralt.Framework.Graphics;
using Kralt.Framework.Graphics.Rendering;

namespace Kralt.Framework.Screens
{
    public abstract class Screen
    {
        public Camera camera = new Camera();

        private readonly List<GameObject> hierarchy = [];

        public void Append(GameObject g)
        {
            hierarchy.Add(g);
        }

        internal void Traverse(Application renderer)
        {
            foreach(GameObject g in hierarchy)
            {
                g.OnRender(renderer);
            }
        }

        public abstract void Initialize();
    }
}
