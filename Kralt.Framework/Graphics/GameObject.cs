using Kralt.Mathematics;
using System.Collections.Generic;

namespace Kralt.Framework.Graphics
{
    public abstract class GameObject : Transformable
    {
        protected List<GameObject> children = [];

        public GameObject? parent;

        public void Append(GameObject child)
        {
            child.parent = this;
            children.Add(child);
        }

        public virtual void OnRender(Application renderer) { }
    }
}
