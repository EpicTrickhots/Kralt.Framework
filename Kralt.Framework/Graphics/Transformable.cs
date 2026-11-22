using System.Collections;
using System.Collections.Generic;
using Kralt.Mathematics;

namespace Kralt.Framework.Graphics
{
    public class Transformable : IEnumerable<Transformable>
    {
        /// <summary>
        /// Position of this <see cref="Transformable"/> relative to its parent
        /// </summary>
        public Vector3 localPosition = Vector3.zero;

        /// <summary>
        /// Rotation of this <see cref="Transformable"/> relative to its parent
        /// </summary>
        public Quaternion localRotation = Quaternion.identity;

        /// <summary>
        /// Scale of this <see cref="Transformable"/> relative to its parent
        /// </summary>
        public Vector3 localScale = Vector3.one;

        #region Constructors

        public Transformable() { }

        public Transformable(Vector3 position)
        {
            localPosition = position;
        }

        public Transformable(Vector3 position, Quaternion rotation)
        {
            localPosition = position;
            localRotation = rotation;
        }

        public Transformable(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            localPosition = position;
            localRotation = rotation;
            localScale = scale;
        }

        #endregion

        #region Transformation matrices

        /// <summary>
        /// World-space transformation matrix of this <see cref="Transformable"/>
        /// </summary>
        public Matrix4x4 worldMatrix
        {
            get
            {
                if (parent == null)
                    return localMatrix;

                return localMatrix * parent.worldMatrix;
            }
        }

        /// <summary>
        /// Local-space transformation matrix of this <see cref="Transformable"/>
        /// </summary>
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

        #endregion

        #region Properties

        /// <summary>
        /// World-space position of this <see cref="Transformable"/>
        /// </summary>
        public Vector3 position
        {
            get => parent == null ? localPosition : Vector3.TransformVector(localPosition, parent.worldMatrix);
            set => localPosition = parent == null ? value : Vector3.TransformVector(value, parent.worldMatrix.Inverted());
        }

        /// <summary>
        /// World-space rotation of this <see cref="Transformable"/>
        /// </summary>
        public Quaternion rotation
        {
            get => parent == null ? localRotation : parent.rotation * localRotation;
            set => localRotation = parent == null ? value : parent.rotation.Inverted() * value;
        }

        /// <summary>
        /// World-space scale of this <see cref="Transformable"/>
        /// A simple Vector3 cannot accurately represent a non-uniform scale combined with rotation in a parent-child hierarchy
        /// </summary>
        public Vector3 lossyScale => parent == null ? localScale : localScale * parent.lossyScale;

        /// <summary>
        /// Shorthand for (0, 1, 0)
        /// </summary>
        public Vector3 up => (localRotation * Vector3.up).Normalized();

        /// <summary>
        /// Shorthand for (0, 0, -1)
        /// </summary>
        public Vector3 forward => (localRotation * -Vector3.forward).Normalized();

        /// <summary>
        /// Shorthand for (1, 0, 0)
        /// </summary>
        public Vector3 right => (localRotation * Vector3.right).Normalized();

        #endregion

        #region Object lifecycle

        internal void Start_Internal()
        {
            Start();
            foreach (Transformable t in children)
                t.Start();
        }

        internal void Update_Internal()
        {
            Update();
            foreach (Transformable t in children)
                t.Update();
        }

        public virtual void Start() { }

        public virtual void Update() { }

        #endregion

        #region Enumeration and parent-child relationship

        /// <summary>
        /// Parent of this Transformable (if it exists)
        /// </summary>
        public Transformable? parent;

        private readonly List<Transformable> children = [];

        /// <summary>
        /// Length of this instance's internal children list
        /// </summary>
        public int childCount => children.Count;

        public Transformable this[int i] => children[i];

        public IEnumerator<Transformable> GetEnumerator()
        {
            foreach(Transformable transform in children)
                yield return transform;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Appends a new <see cref="Transformable"/>
        /// </summary>
        /// <param name="child"></param>
        public void Add(Transformable child)
        {
            child.parent = this;
            children.Add(child);
        }

        #endregion
    }
}
