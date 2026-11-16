using System.Collections.Generic;

namespace Kralt.Framework.Graphics
{
    public abstract class Scene
    {
        public Camera? outputCamera;

        internal readonly List<Transformable> hierarchy = [];

        public virtual void Start()
        {
            foreach (Transformable g in hierarchy)
            {
                g.Start();
            }
        }

        public virtual void Update()
        {
            foreach (Transformable g in hierarchy)
            {
                g.Update();
            }
        }

        public void Add(Transformable g)
        {
            hierarchy.Add(g);

            if (outputCamera == null && g is Camera c)
                outputCamera = c;
        }

        public abstract void Initialize();
    }
}
