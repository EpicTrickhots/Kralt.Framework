using System.Collections.Generic;

namespace Kralt.Framework.Graphics
{
    public abstract class Scene
    {
        public Camera? outputCamera;

        internal readonly List<Transformable> hierarchy = [];

        internal void Start_Internal()
        {
            Start();
            foreach (Transformable g in hierarchy)
                g.Start_Internal();
        }

        internal void Update_Internal()
        {
            Update();
            foreach (Transformable g in hierarchy)
                g.Update_Internal();
        }

        public void Add(Transformable g)
        {
            hierarchy.Add(g);
        }

        public virtual void Start() { }

        public virtual void Update() { }

        public abstract void Initialize();
    }
}
