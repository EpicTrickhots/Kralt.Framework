using Kralt.Mathematics;
using System.Collections;
using System.Collections.Generic;

namespace Kralt.Framework.Graphics
{
    public class Transformable : IEnumerable<Transformable>
    {
        public Vector3 localPosition = Vector3.zero;

        public Quaternion localRotation = Quaternion.identity;

        public Vector3 localScale = Vector3.one;

        public Matrix4x4 worldMatrix
        {
            get
            {
                if (parent == null)
                    return localMatrix;

                return localMatrix * parent.worldMatrix;
            }
        }

        public Matrix4x4 localMatrix
        {
            get
            {
                Matrix4x4 s = Matrix4x4.CreateScale(localScale);
                Matrix4x4 r = Matrix4x4.CreateFromQuaternion(localRotation);
                Matrix4x4 t = Matrix4x4.CreateTranslation(localPosition);
                return s * r * t;
            }
        }

        public Vector3 position
        {
            get => parent == null ? localPosition : Vector3.TransformVector(localPosition, parent.worldMatrix);
            set => localPosition = parent == null ? value : Vector3.TransformVector(value, parent.worldMatrix.Inverted());
        }

        public Quaternion rotation
        {
            get => parent == null ? localRotation : parent.rotation * localRotation;
            set => localRotation = parent == null ? value : parent.rotation.Inverted() * value;
        }

        public Vector3 lossyScale => parent == null ? localScale : localScale * parent.lossyScale;

        public Vector3 up => (localRotation * Vector3.up).Normalized();
        public Vector3 forward => (localRotation * -Vector3.forward).Normalized();
        public Vector3 right => (localRotation * Vector3.right).Normalized();

        public Transformable? parent;

        private readonly List<Transformable> children = [];

        public int childCount => children.Count;

        public Transformable this[int i] => children[i];

        public IEnumerator<Transformable> GetEnumerator()
        {
            foreach(Transformable transform in children)
                yield return transform;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(Transformable child)
        {
            child.parent = this;
            children.Add(child);
        }

        public virtual void Start()
        {
            foreach (Transformable t in children)
                t.Start();
        }

        public virtual void Update()
        {
            foreach (Transformable t in children)
                t.Update();
        }
    }
}
